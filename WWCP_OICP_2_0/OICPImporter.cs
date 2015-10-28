/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/WorldWideCharging/WWCP_OICP>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.Services.DNS;

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    /// <summary>
    /// A service to import EVSE data (from Hubject) via OICP v2.0.
    /// </summary>
    public class OICPImporter<TContext>
    {

        #region Data

        public static readonly TimeSpan      DefaultUpdateEVSEDataEvery      = TimeSpan.FromHours  (  2);
        public static readonly TimeSpan      DefaultUpdateEVSEDataTimeout    = TimeSpan.FromMinutes( 30);   // First import might be big and slow!
        public static readonly TimeSpan      DefaultUpdateEVSEStatusEvery    = TimeSpan.FromSeconds( 20);
        public static readonly TimeSpan      DefaultUpdateEVSEStatusTimeout  = TimeSpan.FromMinutes( 30);   // First import might be big and slow!

        private readonly EMPUpstreamService                                       OICPUpstreamService;
        private readonly Object                                                   UpdateEVSEsLock  = new Object();
        private readonly Timer                                                    UpdateEVSEDataTimer;
        private readonly Timer                                                    UpdateEVSEStatusTimer;

        private readonly Func  <TContext>                                         _UpdateContextCreator;
        private readonly Action<TContext>                                         _UpdateContextDisposer;
        private readonly Action<TContext>                                         _StartBulkUpdate;
        private readonly Action<TContext, DateTime, IEnumerable<EVSEDataRecord>>  _EVSEDataHandler;
        private readonly Func  <TContext, DateTime, IEnumerable<EVSE_Id>>         _GetEVSEIdsForStatusUpdate;
        private readonly Action<TContext, DateTime, EVSE_Id, OICPEVSEStatusType>      _EVSEStatusHandler;
        private readonly Action<TContext>                                         _StopBulkUpdate;

        private readonly Func<UInt64, StreamReader>                               _LoadStaticDataFromStream;
        private readonly Func<UInt64, StreamReader>                               _LoadDynamicDataFromStream;
        private          Boolean                                                  Paused = false;

        #endregion

        #region Properties

        private readonly String         _Identification;
        private readonly String         _Hostname;
        private readonly IPPort         _TCPPort;
        private readonly EVSP_Id        _ProviderId;

        private readonly TimeSpan       _UpdateEVSEDataEvery;
        private readonly TimeSpan       _UpdateEVSEDataTimeout;
        private readonly TimeSpan       _UpdateEVSEStatusEvery;
        private readonly TimeSpan       _UpdateEVSEStatusTimeout;

        #region LoadStaticDataCounter

        private UInt64 _LoadStaticDataCounter = 1;

        public UInt64 LoadStaticDataCounter
        {
            get
            {
                return _LoadStaticDataCounter;
            }
        }

        #endregion

        #region LoadDynamicDataCounter

        private UInt64 _LoadDynamicDataCounter = 1;

        public UInt64 LoadDynamicDataCounter
        {
            get
            {
                return _LoadDynamicDataCounter;
            }
        }

        #endregion

        #region DNSClient

        private readonly DNSClient _DNSClient;

        public DNSClient DNSClient
        {
            get
            {
                return _DNSClient;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new service for importing EVSE data via OICP/Hubject.
        /// </summary>
        /// <param name="Identification"></param>
        /// <param name="Hostname"></param>
        /// <param name="TCPPort"></param>
        /// <param name="ProviderId"></param>
        /// <param name="DNSClient"></param>
        /// <param name="UpdateEVSEDataEvery"></param>
        /// <param name="UpdateEVSEDataTimeout"></param>
        /// <param name="UpdateEVSEStatusEvery"></param>
        /// <param name="UpdateEVSEStatusTimeout"></param>
        /// <param name="UpdateContextCreator"></param>
        /// <param name="UpdateContextDisposer"></param>
        /// <param name="StartBulkUpdate"></param>
        /// <param name="StopBulkUpdate"></param>
        /// <param name="LoadStaticDataFromStream"></param>
        /// <param name="LoadDynamicDataFromStream"></param>
        /// <param name="EVSEDataHandler"></param>
        /// <param name="GetEVSEIdsForStatusUpdate"></param>
        /// <param name="EVSEStatusHandler"></param>
        public OICPImporter(String                                                   Identification,
                            String                                                   Hostname,
                            IPPort                                                   TCPPort,
                            EVSP_Id                                                  ProviderId,
                            DNSClient                                                DNSClient                    = null,

                            TimeSpan?                                                UpdateEVSEDataEvery          = null,
                            TimeSpan?                                                UpdateEVSEDataTimeout        = null,
                            TimeSpan?                                                UpdateEVSEStatusEvery        = null,
                            TimeSpan?                                                UpdateEVSEStatusTimeout      = null,
                            Func<TContext>                                           UpdateContextCreator         = null,
                            Action<TContext>                                         UpdateContextDisposer        = null,
                            Action<TContext>                                         StartBulkUpdate              = null,
                            Action<TContext>                                         StopBulkUpdate               = null,

                            Func<UInt64, StreamReader>                               LoadStaticDataFromStream     = null,
                            Func<UInt64, StreamReader>                               LoadDynamicDataFromStream    = null,

                            Action<TContext, DateTime, IEnumerable<EVSEDataRecord>>  EVSEDataHandler              = null,
                            Func<TContext, DateTime, IEnumerable<EVSE_Id>>           GetEVSEIdsForStatusUpdate    = null,
                            Action<TContext, DateTime, EVSE_Id, OICPEVSEStatusType>      EVSEStatusHandler            = null)

        {

            #region Initial checks

            if (Identification.IsNullOrEmpty())
                throw new ArgumentNullException("The given service identification must not be null or empty!");

            if (Hostname.IsNullOrEmpty())
                throw new ArgumentNullException("The given upstream service hostname must not be null or empty!");

            if (TCPPort == null)
                throw new ArgumentNullException("The given upstream service TCP port must not be null!");

            if (ProviderId == null)
                throw new ArgumentNullException("The given EV Service Provider identification (EVSP Id) must not be null!");

            if (EVSEDataHandler == null)
                throw new ArgumentNullException("The given EVSEOperatorDataHandler must not be null!");

            if (GetEVSEIdsForStatusUpdate == null)
                throw new ArgumentNullException("The given GetEVSEIdsForStatusUpdate must not be null!");

            if (EVSEStatusHandler == null)
                throw new ArgumentNullException("The given EVSEStatusHandler must not be null!");

            #endregion

            #region Init parameters

            this._Identification               = Identification;
            this._Hostname                     = Hostname;
            this._TCPPort                      = TCPPort;
            this._ProviderId                   = ProviderId;

            if (!UpdateEVSEDataEvery.HasValue)
                this._UpdateEVSEDataEvery      = DefaultUpdateEVSEDataEvery;

            if (!UpdateEVSEDataTimeout.HasValue)
                this._UpdateEVSEDataTimeout    = DefaultUpdateEVSEDataTimeout;

            if (!UpdateEVSEStatusEvery.HasValue)
                this._UpdateEVSEStatusEvery    = DefaultUpdateEVSEStatusEvery;

            if (!UpdateEVSEStatusTimeout.HasValue)
                this._UpdateEVSEStatusTimeout  = DefaultUpdateEVSEStatusTimeout;

            this._DNSClient                    = DNSClient != null
                                                     ? DNSClient
                                                     : new DNSClient();

            this._LoadStaticDataFromStream     = LoadStaticDataFromStream;
            this._LoadDynamicDataFromStream    = LoadDynamicDataFromStream;

            this._UpdateContextCreator         = UpdateContextCreator;
            this._UpdateContextDisposer        = UpdateContextDisposer;
            this._StartBulkUpdate              = StartBulkUpdate;
            this._EVSEDataHandler              = EVSEDataHandler;
            this._GetEVSEIdsForStatusUpdate    = GetEVSEIdsForStatusUpdate;
            this._EVSEStatusHandler            = EVSEStatusHandler;
            this._StopBulkUpdate               = StopBulkUpdate;

            #endregion

            #region Init OICP EMP UpstreamService

            OICPUpstreamService  = new EMPUpstreamService(_Hostname,
                                                          _TCPPort,
                                                          DNSClient: _DNSClient);

            OICPUpstreamService.OnException += (Timestamp, Sender, Exception) => {

                if (Exception.Message.StartsWith("Unexpected end of file while parsing") ||
                    Exception.Message.StartsWith("Unexpected end of file has occurred. The following elements are not closed:"))
                    return;

                DebugX.Log("'" + Sender.ToString() + "' " + Exception.Message);

            };

            OICPUpstreamService.OnHTTPError += (Timestamp, Sender, HttpResponse) => {
                DebugX.Log("'" + Sender.ToString() + "' " + (HttpResponse != null ? HttpResponse.ToString() : "<null>"));
            };

            #endregion

            #region Init Timers

            UpdateEVSEDataTimer    = new Timer(UpdateEVSEData,   null, TimeSpan.FromSeconds(1),  UpdateEVSEDataEvery.  Value);
            UpdateEVSEStatusTimer  = new Timer(UpdateEVSEStatus, null, TimeSpan.FromSeconds(10), UpdateEVSEStatusEvery.Value);

            #endregion

        }

        #endregion


        #region Pause()

        /// <summary>
        /// Pause the OICP importer (after the current run).
        /// </summary>
        public void Pause()
        {
            lock (UpdateEVSEsLock)
            {
                Paused = true;
            }
        }

        #endregion

        #region Continue()

        /// <summary>
        /// Continue the OICP importer (with the next scheduled run).
        /// </summary>
        public void Continue()
        {
            lock (UpdateEVSEsLock)
            {
                Paused = false;
            }
        }

        #endregion


        #region (private, Timer) UpdateEVSEData(State)

        /// <summary>
        /// A timer controlled method to update all EVSE data.
        /// </summary>
        /// <param name="State">State object.</param>
        private void UpdateEVSEData(Object State)
        {

            if (Paused)
                return;

            // Wait till a concurrent UpdateEVSEStatus(...) has finished!
            if (Monitor.TryEnter(UpdateEVSEsLock, _UpdateEVSEDataTimeout))
            {

                Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

                try
                {

                    #region Debug info

                    #if DEBUG

                        DebugX.LogT("'UpdateEVSEData' started");

                        var StopWatch = new Stopwatch();
                        StopWatch.Start();

                    #endif

                    #endregion

                    #region Create update context and start bulk update

                    TContext UpdateContext = default(TContext);

                    try
                    {

                        var UpdateContextCreatorLocal = _UpdateContextCreator;
                        if (UpdateContextCreatorLocal != null)
                            UpdateContext = UpdateContextCreatorLocal();

                    }
                    catch (Exception e)
                    {
                        DebugX.Log("Could not create OICP importer update context: " + e.Message);
                    }

                    try
                    {

                        var StartBulkUpdateLocal = _StartBulkUpdate;
                        if (StartBulkUpdateLocal != null)
                            StartBulkUpdateLocal(UpdateContext);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log("Could not start OICP importer bulk update: " + e.Message);
                    }

                    #endregion

                    #region Load static EVSE data from stream...

                    if (_LoadStaticDataFromStream != null)
                    {

                        try
                        {

                            var XML               = XDocument.Parse(_LoadStaticDataFromStream(_LoadStaticDataCounter++).ReadToEnd()).Root;

                            var SOAPXML           = XML.Element(org.GraphDefined.Vanaheimr.Hermod.SOAP.NS.SOAPEnvelope + "Body").
                                                        Descendants().
                                                        FirstOrDefault();

                            // Either with SOAP-XML tags or without...
                            var OperatorEvseDataXMLs  = (SOAPXML != null ? SOAPXML : XML).
                                                            Element (OICPNS.EVSEData + "EvseData").
                                                            Elements(OICPNS.EVSEData + "OperatorEvseData");

                            if (OperatorEvseDataXMLs != null)
                            {

                                var EVSEDataRecords = OperatorEvseDataXMLs.
                                                          Select    (OperatorEvseDataXML => OperatorEVSEData.Parse(OperatorEvseDataXML)).
                                                          Where     (_OperatorEvseData   => _OperatorEvseData != null).
                                                          SelectMany(_OperatorEvseData   => _OperatorEvseData.EVSEDataRecords).
                                                          ToArray();

                                if (EVSEDataRecords.Length > 0)
                                    _EVSEDataHandler(UpdateContext,
                                                             DateTime.Now,
                                                             EVSEDataRecords);

                            }

                            else
                                DebugX.Log("Could not fetch any 'OperatorEvseData' from XML stream!");

                        }
                        catch (Exception e)
                        {
                            DebugX.LogT("Could not fetch any 'OperatorEvseData' from XML stream: " + e.Message);
                        }

                    }

                    #endregion

                    #region ...or load it (from Hubject) via OICP v2.0

                    else
                        OICPUpstreamService.
                            PullEVSEData(_ProviderId).
                            ContinueWith(PullEVSEDataTask => {

                                if (PullEVSEDataTask.Result.Content.StatusCode.Code == 0)
                                    PullEVSEDataTask.Result.Content.OperatorEVSEData.ForEach(OperatorEVSEData => _EVSEDataHandler(UpdateContext, DateTime.Now, OperatorEVSEData.EVSEDataRecords));

                            }).
                            Wait();

                    #endregion

                    #region Stop bulk update and dispose update context

                    try
                    {

                        var StopBulkUpdateLocal = _StopBulkUpdate;
                        if (StopBulkUpdateLocal != null)
                            StopBulkUpdateLocal(UpdateContext);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log("Could not stop OICP importer bulk update: " + e.Message);
                    }

                    try
                    {

                        var UpdateContextDisposerLocal = _UpdateContextDisposer;
                        if (UpdateContextDisposerLocal != null)
                            UpdateContextDisposerLocal(UpdateContext);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log("Could not dispose OICP importer context: " + e.Message);
                    }

                    #endregion

                    #region Debug info

                    #if DEBUG

                        StopWatch.Stop();

                        DebugX.LogT("'UpdateEVSEData' finished after " + StopWatch.Elapsed.TotalSeconds + " seconds!");

                    #endif

                    #endregion

                }
                catch (Exception e)
                {
                    DebugX.LogT("'UpdateEVSEData' led to an exception: " + e.Message);
                }

                finally
                {
                    Monitor.Exit(UpdateEVSEsLock);
                }

            }

            else
                DebugX.LogT("'UpdateEVSEData' skipped!");

        }

        #endregion

        #region (private, Timer) UpdateEVSEStatus(State)

        /// <summary>
        /// A timer controlled method to update all EVSE status.
        /// </summary>
        /// <param name="State">State object.</param>
        private void UpdateEVSEStatus(Object State)
        {

            if (Paused)
                return;

            // If a concurrent UpdateEVSEData/UpdateEVSEStatus(...) is still running, skip this round!
            if (Monitor.TryEnter(UpdateEVSEsLock))
            {

                Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

                try
                {

                    #region Debug info

                    #if DEBUG

                        DebugX.LogT("'UpdateEVSEStatus' started");

                        var StopWatch = new Stopwatch();
                        StopWatch.Start();

                    #endif

                    #endregion

                    #region Create update context and start bulk update

                    TContext UpdateContext = default(TContext);

                    try
                    {

                        var UpdateContextCreatorLocal = _UpdateContextCreator;
                        if (UpdateContextCreatorLocal != null)
                            UpdateContext = UpdateContextCreatorLocal();

                    }
                    catch (Exception e)
                    {
                        DebugX.Log("Could not create OICP importer update context: " + e.Message);
                    }

                    try
                    {

                        var StartBulkUpdateLocal = _StartBulkUpdate;
                        if (StartBulkUpdateLocal != null)
                            StartBulkUpdateLocal(UpdateContext);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log("Could not start OICP importer bulk update: " + e.Message);
                    }

                    #endregion

                    #region Load static EVSE data from stream...

                    if (_LoadStaticDataFromStream != null)
                    {

                        try
                        {

                            var XML         = XDocument.Parse(_LoadDynamicDataFromStream(_LoadDynamicDataCounter++).ReadToEnd()).Root;

                            var SOAPXML     = XML.Element(org.GraphDefined.Vanaheimr.Hermod.SOAP.NS.SOAPEnvelope + "Body").
                                                  Descendants().
                                                  FirstOrDefault();

                            // Either with SOAP-XML tags or without...
                            var EvseStatus  = (SOAPXML != null ? SOAPXML : XML).
                                                        Element (OICPNS.EVSEStatus + "EvseStatusRecords").
                                                        Elements(OICPNS.EVSEStatus + "EvseStatusRecord").
                                                        Select(v => new KeyValuePair<EVSE_Id, OICPEVSEStatusType>(EVSE_Id.Parse(v.Element(OICPNS.EVSEStatus + "EvseId").Value),
                                                                                                                (OICPEVSEStatusType) Enum.Parse(typeof(OICPEVSEStatusType), v.Element(OICPNS.EVSEStatus + "EvseStatus").Value))).
                                                        ToArray();

                            if (EvseStatus.Length > 0)
                                EvseStatus.ForEach(EVSEStatusKVP => _EVSEStatusHandler(UpdateContext, DateTime.Now, EVSEStatusKVP.Key, EVSEStatusKVP.Value));

                            else
                                DebugX.Log("Could not fetch any 'EvseStatusRecords' from XML stream!");

                        }
                        catch (Exception e)
                        {
                            DebugX.Log("Could not fetch any 'EvseStatusRecords' from XML stream: " + e.Message);
                        }

                    }

                    #endregion

                    #region ...or load it (from Hubject) via OICP v2.0

                    else
                    {

                        Boolean Finished = false;

                        var cancellationTokenS = new CancellationTokenSource();

                        #region 1) Fetch EVSEIds to update

                        IEnumerable<EVSE_Id> EVSEIds = null;

                        try
                        {

                            EVSEIds = _GetEVSEIdsForStatusUpdate(UpdateContext, DateTime.Now);

                        }
                        catch (Exception e)
                        {
                            DebugX.Log("Could not fetch the list of EVSE Ids for dynamic EVSE status update!");
                        }

                        if (EVSEIds == null)
                            DebugX.Log("Could not fetch the list of EVSE Ids for dynamic EVSE status update!");

                        #endregion

                        #region 2) Load the data (from Hubject) via OICP v2.0

                        // Get the data via the GetEVSEIds delegate!
                        else
                            Finished = Task.WaitAll(EVSEIds.

                                                        // Hubject has a limit of 100 EVSEIds per request!
                                                        ToPartitions(100).

                                                        Select(EVSEPartition => OICPUpstreamService.
                                                                                    PullEVSEStatusById(_ProviderId, EVSEPartition).
                                                                                    ContinueWith(NewEVSEStatusTask => {

                                                                                        if (NewEVSEStatusTask.Result != null)
                                                                                            if (NewEVSEStatusTask.Result.Content != null)
                                                                                                NewEVSEStatusTask.Result.Content.EVSEStatusRecords.ForEach(NewEVSEStatusRecord =>
                                                                                                    _EVSEStatusHandler(UpdateContext,
                                                                                                                       DateTime.Now,
                                                                                                                       NewEVSEStatusRecord.Id,
                                                                                                                       NewEVSEStatusRecord.Status));

                                                                                    }, cancellationTokenS.Token)
                                                                                    )
                                                                                    .ToArray(),

                                                    millisecondsTimeout: (Int32) _UpdateEVSEStatusTimeout.TotalMilliseconds

                                                    //CancellationToken cancellationToken

                                                   );


                        // Wait 15 seconds and then cancel all subtasks!
                        if (!Finished)
                        {
                            Thread.Sleep(TimeSpan.FromSeconds(15));
                            cancellationTokenS.Cancel();
                            DebugX.Log("Canceled all 'UpdateEVSEStatus' subtasks!");
                        }

                        #endregion

                    }

                    #endregion

                    #region Stop bulk update and dispose update context

                    try
                    {

                        var StopBulkUpdateLocal = _StopBulkUpdate;
                        if (StopBulkUpdateLocal != null)
                            StopBulkUpdateLocal(UpdateContext);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log("Could not stop OICP importer bulk update: " + e.Message);
                    }

                    try
                    {

                        var UpdateContextDisposerLocal = _UpdateContextDisposer;
                        if (UpdateContextDisposerLocal != null)
                            UpdateContextDisposerLocal(UpdateContext);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log("Could not dispose OICP importer context: " + e.Message);
                    }

                    #endregion

                    #region Debug info

                    #if DEBUG

                        StopWatch.Stop();

                        DebugX.LogT("'UpdateEVSEStatus' finished after " + StopWatch.Elapsed.TotalSeconds + " seconds!");

                    #endif

                    #endregion

                }
                catch (Exception e)
                {
                    DebugX.LogT("'UpdateEVSEStatus' led to an exception: " + e.Message);
                }

                finally
                {
                    Monitor.Exit(UpdateEVSEsLock);
                }

            }

            else
                DebugX.LogT("'UpdateEVSEStatus' skipped!");

        }

        #endregion


    }

}
