/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of eMI3 OICP <http://www.github.com/eMI3/OICP-Bindings>
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
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.Services.DNS;
using System.Collections.Concurrent;

#endregion

namespace org.GraphDefined.eMI3.IO.OICP_1_2
{

    /// <summary>
    /// A service to import EVSE data from OICP/Hubject.
    /// </summary>
    public class OICPImporter<TContext>
    {

        #region Data

        public static readonly TimeSpan      DefaultUpdateEVSEDataEvery      = TimeSpan.FromHours(2);
        public static readonly TimeSpan      DefaultUpdateEVSEDataTimeout    = TimeSpan.FromMinutes(10);
        public static readonly TimeSpan      DefaultUpdateEVSEStatusEvery    = TimeSpan.FromSeconds(20);
        public static readonly TimeSpan      DefaultUpdateEVSEStatusTimeout  = TimeSpan.FromSeconds(120);

        private readonly EMPUpstreamService                             OICPUpstreamService;
        private readonly Object                                         UpdateEVSEsLock  = new Object();
        private readonly Timer                                          UpdateEVSEDataTimer;
        private readonly Timer                                          UpdateEVSEStatusTimer;

        private readonly Func<IEnumerable<EVSE_Id>>                             _GetEVSEIds;
        private readonly Func<TContext>                                         _UpdateContextCreator;
        private readonly Action<TContext>                                       _UpdateContextDisposer;
        private readonly Action<TContext>                                       _StartBulkUpdate;
        private readonly Action<TContext, DateTime, XElement>                   _EVSEOperatorDataHandler;
        private readonly Action<TContext, DateTime, EVSE_Id, HubjectEVSEState>  _EVSEStatusHandler;
        private readonly Action                                                 _StopBulkUpdate;

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
        /// <param name="GetEVSEIds"></param>
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
                            DNSClient                                              DNSClient                = null,
                            TimeSpan?                                              UpdateEVSEDataEvery      = null,
                            TimeSpan?                                              UpdateEVSEDataTimeout    = null,
                            TimeSpan?                                              UpdateEVSEStatusEvery    = null,
                            TimeSpan?                                              UpdateEVSEStatusTimeout  = null,
                            Func<IEnumerable<EVSE_Id>>                             GetEVSEIds               = null,
                            Func<TContext>                                         UpdateContextCreator     = null,
                            Action<TContext>                                       UpdateContextDisposer    = null,
                            Action<TContext>                                       StartBulkUpdate          = null,
                            Action<TContext, DateTime, XElement>                   EVSEOperatorDataHandler  = null,
                            Action<TContext, DateTime, EVSE_Id, HubjectEVSEState>  EVSEStatusHandler        = null,
                            Action                                                 StopBulkUpdate           = null)

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

            if (GetEVSEIds == null)
                throw new ArgumentNullException("The given GetEVSEIds must not be null!");

            if (EVSEOperatorDataHandler == null)
                throw new ArgumentNullException("The given OperatorDataHandler must not be null!");

            if (EVSEStatusHandler == null)
                throw new ArgumentNullException("The given EVSEStatusHandler must not be null!");

            #endregion

            #region Init parameters

            this._Identification           = Identification;
            this._Hostname                 = Hostname;
            this._TCPPort                  = TCPPort;
            this._ProviderId               = ProviderId;

            if (!UpdateEVSEDataEvery.HasValue)
                this._UpdateEVSEDataEvery      = DefaultUpdateEVSEDataEvery;

            if (!UpdateEVSEDataTimeout.HasValue)
                this._UpdateEVSEDataTimeout    = DefaultUpdateEVSEDataTimeout;

            if (!UpdateEVSEStatusEvery.HasValue)
                this._UpdateEVSEStatusEvery    = DefaultUpdateEVSEStatusEvery;

            if (!UpdateEVSEStatusTimeout.HasValue)
                this._UpdateEVSEStatusTimeout  = DefaultUpdateEVSEStatusTimeout;

            this._DNSClient                = DNSClient != null
                                                 ? DNSClient
                                                 : new DNSClient();

            this._GetEVSEIds               = GetEVSEIds;
            this._UpdateContextCreator     = UpdateContextCreator;
            this._UpdateContextDisposer    = UpdateContextDisposer;
            this._StartBulkUpdate          = StartBulkUpdate;
            this._EVSEOperatorDataHandler  = EVSEOperatorDataHandler;
            this._EVSEStatusHandler        = EVSEStatusHandler;
            this._StopBulkUpdate           = StopBulkUpdate;

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


        #region (threaded!) UpdateEVSEData(State)

        /// <summary>
        /// A timer controlled method to update all EVSE data.
        /// </summary>
        /// <param name="State">State object.</param>
        public void UpdateEVSEData(Object State)
        {

            // Wait till a concurrent UpdateEVSEStatus(...) has finished!
            if (Monitor.TryEnter(UpdateEVSEsLock, _UpdateEVSEDataTimeout))
            {

                Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

                try
                {

                    Debug.WriteLine("[" + DateTime.Now + "] 'UpdateEVSEData' started");

                    var StopWatch = new Stopwatch();
                    StopWatch.Start();

                    var UpdateContextCreatorLocal = _UpdateContextCreator;
                    var UpdateContext = UpdateContextCreatorLocal != null
                                            ? UpdateContextCreatorLocal()
                                            : default(TContext);

                    var StartBulkUpdateLocal = _StartBulkUpdate;
                    if (StartBulkUpdateLocal != null)
                        StartBulkUpdateLocal(UpdateContext);

                    OICPUpstreamService.
                        PullEVSEDataRequest(_ProviderId).
                        ContinueWith(PullEVSEDataTask => {

                            if (PullEVSEDataTask.Result != null)
                                PullEVSEDataTask.Result.Content.ForEach(XML => _EVSEOperatorDataHandler(UpdateContext, DateTime.Now, XML));

                        }).
                        Wait();

                    var UpdateContextDisposerLocal = _UpdateContextDisposer;
                    if (UpdateContextDisposerLocal != null)
                        UpdateContextDisposerLocal(UpdateContext);

                    var StopBulkUpdateLocal = _StopBulkUpdate;
                    if (StopBulkUpdateLocal != null)
                        StopBulkUpdateLocal();

                    StopWatch.Stop();

                    Debug.WriteLine("[" + DateTime.Now + "] 'UpdateEVSEData' finished after " + StopWatch.Elapsed.TotalSeconds + " seconds!");


                }
                catch (Exception e)
                {
                    Debug.WriteLine("[" + DateTime.Now + "] Thread " + Thread.CurrentThread.ManagedThreadId + ",  'UpdateEVSEData' lead to an exception: " + e.Message + Environment.NewLine + e.StackTrace);
                }

                finally
                {
                    Monitor.Exit(UpdateEVSEsLock);
                }

            }

            else
                Debug.WriteLine("[" + DateTime.Now + "] Thread " + Thread.CurrentThread.ManagedThreadId + ",  'UpdateEVSEData' skipped!");

        }

        #endregion

        #region (threaded!) UpdateEVSEStatus(State)

        /// <summary>
        /// A timer controlled method to update all EVSE status.
        /// </summary>
        /// <param name="State">State object.</param>
        public void UpdateEVSEStatus(Object State)
        {

            // If a concurrent UpdateEVSEData/UpdateEVSEStatus(...) is still running, skip this round!
            if (Monitor.TryEnter(UpdateEVSEsLock))
            {

                //UpdateEVSEStatusTimer.Change

                Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

                try
                {

                    Debug.WriteLine("[" + DateTime.Now + "] Thread " + Thread.CurrentThread.ManagedThreadId + ",  'UpdateEVSEStatus' started");

                    var StopWatch = new Stopwatch();
                    StopWatch.Start();

                    var UpdateContextCreatorLocal = _UpdateContextCreator;
                    var UpdateContext = UpdateContextCreatorLocal != null
                                            ? UpdateContextCreatorLocal()
                                            : default(TContext);

                    var StartBulkUpdateLocal = _StartBulkUpdate;
                    if (StartBulkUpdateLocal != null)
                        StartBulkUpdateLocal(UpdateContext);

                    Debug.WriteLine("[" + DateTime.Now + "] Thread " + Thread.CurrentThread.ManagedThreadId + ", Starting all data collection subtasks concurrently...");

                    var EVSEStatusUpdateBuffer = new ConcurrentDictionary<EVSE_Id, HubjectEVSEState>();

                    Task.WaitAll(_GetEVSEIds().         // Get the data via the GetEVSEIds delegate!
                                     ToPartitions(100). // Hubject has a limit of 100 EVSEIds per request!
                                     Select(EVSEPartition =>

                                         OICPUpstreamService.
                                             PullEVSEStatusByIdRequest(_ProviderId, EVSEPartition).
                                             ContinueWith(NewEVSEStatusTask => {

                                                 // Add data to internal buffer...
                                                 if (NewEVSEStatusTask.Result != null)
                                                     NewEVSEStatusTask.Result.Content.ForEach(NewEVSEStatus => EVSEStatusUpdateBuffer.TryAdd(NewEVSEStatus.Key, NewEVSEStatus.Value));

                                             }))
                                             .ToArray(),

                                 millisecondsTimeout: (Int32) _UpdateEVSEStatusTimeout.TotalMilliseconds
                                 //CancellationToken cancellationToken
                                );

                    Debug.WriteLine("[" + DateTime.Now + "] Thread " + Thread.CurrentThread.ManagedThreadId + ", Starting external data processing...");

                    EVSEStatusUpdateBuffer.ForEach(StatusUpdate => _EVSEStatusHandler(UpdateContext, DateTime.Now, StatusUpdate.Key, StatusUpdate.Value));

                    Debug.WriteLine("[" + DateTime.Now + "] Thread " + Thread.CurrentThread.ManagedThreadId + ", 'UpdateEVSEStatus' finished external update delegates!");

                    var StopBulkUpdateLocal = _StopBulkUpdate;
                    if (StopBulkUpdateLocal != null)
                        StopBulkUpdateLocal();

                    var UpdateContextDisposerLocal = _UpdateContextDisposer;
                    if (UpdateContextDisposerLocal != null)
                        UpdateContextDisposerLocal(UpdateContext);

                    StopWatch.Stop();

                    Debug.WriteLine("[" + DateTime.Now + "] 'UpdateEVSEStatus' finished after " + StopWatch.Elapsed.TotalSeconds + " seconds!");

                }
                catch (Exception e)
                {
                    Debug.WriteLine("[" + DateTime.Now + "] Thread " + Thread.CurrentThread.ManagedThreadId + ",  'UpdateEVSEStatus' lead to an exception: " + e.Message + Environment.NewLine + e.StackTrace);
                }

                finally
                {
                    Monitor.Exit(UpdateEVSEsLock);
                }

            }

            else
                Debug.WriteLine("[" + DateTime.Now + "] Thread " + Thread.CurrentThread.ManagedThreadId + ",  'UpdateEVSEStatus' skipped!");

        }

        #endregion


    }

}
