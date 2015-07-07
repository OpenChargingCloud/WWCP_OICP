/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICPClient <https://github.com/WorldWideCharging/WWCP_OICPClient>
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

namespace org.GraphDefined.WWCP.OICPClient_1_2
{

    /// <summary>
    /// A service to import EVSE data (from Hubject) via OICP v1.2.
    /// </summary>
    public class OICPImporter<TContext>
    {

        #region Data

        public static readonly TimeSpan      DefaultUpdateEVSEDataEvery      = TimeSpan.FromHours  (  2);
        public static readonly TimeSpan      DefaultUpdateEVSEDataTimeout    = TimeSpan.FromMinutes( 15);   // First import might be big and slow!
        public static readonly TimeSpan      DefaultUpdateEVSEStatusEvery    = TimeSpan.FromSeconds( 20);
        public static readonly TimeSpan      DefaultUpdateEVSEStatusTimeout  = TimeSpan.FromMinutes( 15);   // First import might be big and slow!

        private readonly EMPUpstreamService                                     OICPUpstreamService;
        private readonly Object                                                 UpdateEVSEsLock  = new Object();
        private readonly Timer                                                  UpdateEVSEDataTimer;
        private readonly Timer                                                  UpdateEVSEStatusTimer;

        private readonly Func<TContext>                                         _UpdateContextCreator;
        private readonly Action<TContext>                                       _UpdateContextDisposer;
        private readonly Action<TContext>                                       _StartBulkUpdate;
        private readonly Action<TContext, DateTime, XElement>                   _EVSEOperatorDataHandler;
        private readonly Func<TContext, DateTime, IEnumerable<EVSE_Id>>         _GetEVSEIdsForStatusUpdate;
        private readonly Action<TContext, DateTime, EVSE_Id, HubjectEVSEState>  _EVSEStatusHandler;
        private readonly Action<TContext>                                       _StopBulkUpdate;

        private readonly Func<StreamReader>                                     _LoadStaticDataFromStream;
        private readonly Func<StreamReader>                                     _LoadDynamicDataFromStream;

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

        private readonly DNSClient      _DNSClient;

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
        /// <param name="GetEVSEIdsForStatusUpdate"></param>
        /// <param name="UpdateContextCreator"></param>
        /// <param name="UpdateContextDisposer"></param>
        /// <param name="StartBulkUpdate"></param>
        /// <param name="EVSEOperatorDataHandler"></param>
        /// <param name="EVSEStatusHandler"></param>
        /// <param name="StopBulkUpdate"></param>
        public OICPImporter(String                                                 Identification,
                            String                                                 Hostname,
                            IPPort                                                 TCPPort,
                            EVSP_Id                                                ProviderId,
                            DNSClient                                              DNSClient                    = null,

                            Func<StreamReader>                                     LoadStaticDataFromStream     = null,
                            Func<StreamReader>                                     LoadDynamicDataFromStream    = null,

                            TimeSpan?                                              UpdateEVSEDataEvery          = null,
                            TimeSpan?                                              UpdateEVSEDataTimeout        = null,
                            TimeSpan?                                              UpdateEVSEStatusEvery        = null,
                            TimeSpan?                                              UpdateEVSEStatusTimeout      = null,
                            Func<TContext>                                         UpdateContextCreator         = null,
                            Action<TContext>                                       UpdateContextDisposer        = null,
                            Action<TContext>                                       StartBulkUpdate              = null,
                            Action<TContext, DateTime, XElement>                   EVSEOperatorDataHandler      = null,
                            Func<TContext, DateTime, IEnumerable<EVSE_Id>>         GetEVSEIdsForStatusUpdate    = null,
                            Action<TContext, DateTime, EVSE_Id, HubjectEVSEState>  EVSEStatusHandler            = null,
                            Action<TContext>                                       StopBulkUpdate               = null)

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

            if (EVSEOperatorDataHandler == null)
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
            this._EVSEOperatorDataHandler      = EVSEOperatorDataHandler;
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

                Debug.WriteLine("[" + Timestamp + "] '" + Sender.ToString() + "' " + Exception.Message);

            };

            OICPUpstreamService.OnHTTPError += (Timestamp, Sender, HttpResponse) => {
                Debug.WriteLine("[" + Timestamp + "] '" + Sender.ToString() + "' " + (HttpResponse != null ? HttpResponse.ToString() : "<null>"));
            };

            #endregion

            #region Init Timers

            UpdateEVSEDataTimer    = new Timer(UpdateEVSEData,   null, TimeSpan.FromSeconds(1),  UpdateEVSEDataEvery.  Value);
            UpdateEVSEStatusTimer  = new Timer(UpdateEVSEStatus, null, TimeSpan.FromSeconds(10), UpdateEVSEStatusEvery.Value);

            #endregion

        }

        #endregion


        #region (private, Timer) UpdateEVSEData(State)

        /// <summary>
        /// A timer controlled method to update all EVSE data.
        /// </summary>
        /// <param name="State">State object.</param>
        private void UpdateEVSEData(Object State)
        {

            // Wait till a concurrent UpdateEVSEStatus(...) has finished!
            if (Monitor.TryEnter(UpdateEVSEsLock, _UpdateEVSEDataTimeout))
            {

                Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

                try
                {

                    #region Debug info

                    #if DEBUG

                        DebugX.Log(" Thread " + Thread.CurrentThread.ManagedThreadId + "] 'UpdateEVSEData' started");

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

                            var XML               = XDocument.Parse(_LoadStaticDataFromStream().ReadToEnd()).Root;

                            var SOAPXML           = XML.Element(org.GraphDefined.Vanaheimr.Hermod.SOAP.NS.SOAPEnvelope + "Body").
                                                        Descendants().
                                                        FirstOrDefault();

                            // Either with SOAP-XML tags or without...
                            var OperatorEvseData  = (SOAPXML != null ? SOAPXML : XML).
                                                        Element (NS.OICPv1_2EVSEData + "EvseData").
                                                        Elements(NS.OICPv1_2EVSEData + "OperatorEvseData").
                                                        ToArray();

                            if (OperatorEvseData.Length > 0)
                                OperatorEvseData.ForEach(OperatorXML => _EVSEOperatorDataHandler(UpdateContext, DateTime.Now, OperatorXML));

                            else
                                DebugX.Log("Could not fetch any 'OperatorEvseData' from XML stream!");

                        }
                        catch (Exception e)
                        {
                            DebugX.LogT("Could not fetch any 'OperatorEvseData' from XML stream: " + e.Message + Environment.NewLine + e.StackTrace);
                        }

                    }

                    #endregion

                    #region ...or load it (from Hubject) via OICP v1.2

                    else
                        OICPUpstreamService.
                            PullEVSEDataRequest(_ProviderId).
                            ContinueWith(PullEVSEDataTask => {

                                if (PullEVSEDataTask.Result != null)
                                    PullEVSEDataTask.Result.Content.ForEach(XML => _EVSEOperatorDataHandler(UpdateContext, DateTime.Now, XML));

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

                        DebugX.Log(" Thread " + Thread.CurrentThread.ManagedThreadId + "] 'UpdateEVSEData' finished after " + StopWatch.Elapsed.TotalSeconds + " seconds!");

                    #endif

                    #endregion

                }
                catch (Exception e)
                {
                    DebugX.Log(" Thread " + Thread.CurrentThread.ManagedThreadId + "] 'UpdateEVSEData' lead to an exception: " + e.Message + Environment.NewLine + e.StackTrace);
                }

                finally
                {
                    Monitor.Exit(UpdateEVSEsLock);
                }

            }

            else
                DebugX.Log(" Thread " + Thread.CurrentThread.ManagedThreadId + "] 'UpdateEVSEData' skipped!");

        }

        #endregion

        #region (private, Timer) UpdateEVSEStatus(State)

        /// <summary>
        /// A timer controlled method to update all EVSE status.
        /// </summary>
        /// <param name="State">State object.</param>
        private void UpdateEVSEStatus(Object State)
        {

            // If a concurrent UpdateEVSEData/UpdateEVSEStatus(...) is still running, skip this round!
            if (Monitor.TryEnter(UpdateEVSEsLock))
            {

                Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

                try
                {

                    #region Debug info

                    #if DEBUG

                        DebugX.Log("Thread " + Thread.CurrentThread.ManagedThreadId + "] 'UpdateEVSEStatus' started");

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

                            var XML         = XDocument.Parse(_LoadDynamicDataFromStream().ReadToEnd()).Root;

                            var SOAPXML     = XML.Element(org.GraphDefined.Vanaheimr.Hermod.SOAP.NS.SOAPEnvelope + "Body").
                                                  Descendants().
                                                  FirstOrDefault();

                            // Either with SOAP-XML tags or without...
                            var EvseStatus  = (SOAPXML != null ? SOAPXML : XML).
                                                        Element (NS.OICPv1_2EVSEStatus + "EvseStatusRecords").
                                                        Elements(NS.OICPv1_2EVSEStatus + "EvseStatusRecord").
                                                        Select(v => new KeyValuePair<EVSE_Id, HubjectEVSEState>(EVSE_Id.Parse(v.Element(NS.OICPv1_2EVSEStatus + "EvseId").Value),
                                                                                                                (HubjectEVSEState) Enum.Parse(typeof(HubjectEVSEState), v.Element(NS.OICPv1_2EVSEStatus + "EvseStatus").Value))).
                                                        ToArray();

                            if (EvseStatus.Length > 0)
                                EvseStatus.ForEach(EVSEStatusKVP => _EVSEStatusHandler(UpdateContext, DateTime.Now, EVSEStatusKVP.Key, EVSEStatusKVP.Value));

                            else
                                DebugX.Log("Could not fetch any 'EvseStatusRecords' from XML stream!");

                        }
                        catch (Exception e)
                        {
                            DebugX.LogT("Could not fetch any 'EvseStatusRecords' from XML stream: " + e.Message + Environment.NewLine + e.StackTrace);
                        }

                    }

                    #endregion

                    #region ...or load it (from Hubject) via OICP v1.2

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

                        #region 2) Load the data (from Hubject) via OICP v1.2

                        // Get the data via the GetEVSEIds delegate!
                        else
                            Finished = Task.WaitAll(EVSEIds.

                                                        // Hubject has a limit of 100 EVSEIds per request!
                                                        ToPartitions(100).

                                                        Select(EVSEPartition => OICPUpstreamService.
                                                                                    PullEVSEStatusByIdRequest(_ProviderId, EVSEPartition).
                                                                                    ContinueWith(NewEVSEStatusTask => {

                                                                                        if (NewEVSEStatusTask.Result != null)
                                                                                            NewEVSEStatusTask.Result.Content.ForEach(NewEVSEStatus =>
                                                                                                _EVSEStatusHandler(UpdateContext, DateTime.Now, NewEVSEStatus.Key, NewEVSEStatus.Value));

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

                        DebugX.Log("Thread " + Thread.CurrentThread.ManagedThreadId + "] 'UpdateEVSEStatus' finished after " + StopWatch.Elapsed.TotalSeconds + " seconds!");

                    #endif

                    #endregion

                }
                catch (Exception e)
                {
                    DebugX.LogT("'UpdateEVSEStatus' lead to an exception: " + e.Message + Environment.NewLine + e.StackTrace);
                }

                finally
                {
                    Monitor.Exit(UpdateEVSEsLock);
                }

            }

            else
                DebugX.Log("Thread " + Thread.CurrentThread.ManagedThreadId + "] 'UpdateEVSEStatus' skipped!");

        }

        #endregion

    }

}
