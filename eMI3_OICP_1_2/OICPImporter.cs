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

#endregion

namespace org.GraphDefined.eMI3.IO.OICP_1_2
{

    /// <summary>
    /// A service to import EVSE data from OICP/Hubject.
    /// </summary>
    public class OICPImporter
    {

        #region Data

        private readonly EMPUpstreamService  OICPUpstreamService;
        private readonly Object              UpdateEVSEsLock                 = new Object();

        private readonly Timer               UpdateEVSEDataTimer;
        private readonly Timer               UpdateEVSEStatusTimer;

        public static readonly TimeSpan      DefaultUpdateEVSEDataEvery      = TimeSpan.FromHours(2);
        public static readonly TimeSpan      DefaultUpdateEVSEDataTimeout    = TimeSpan.FromMinutes(10);
        public static readonly TimeSpan      DefaultUpdateEVSEStatusEvery    = TimeSpan.FromSeconds(20);
        public static readonly TimeSpan      DefaultUpdateEVSEStatusTimeout  = TimeSpan.FromSeconds(120);

        private readonly Func<IEnumerable<EVSE_Id>>                       _GetEVSEIds;
        private readonly Action<XElement>                                 _EVSEDataHandler;
        private readonly Action<KeyValuePair<EVSE_Id, HubjectEVSEState>>  _EVSEStatusHandler;

        #endregion

        #region Properties

        private readonly String         _Identification;
     //   private readonly EVSEDatabase   _EVSEDatabase;
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
        /// <param name="UpdateEVSEDataEvery"></param>
        /// <param name="UpdateEVSEDataTimeout"></param>
        /// <param name="UpdateEVSEStatusEvery"></param>
        /// <param name="UpdateEVSEStatusTimeout"></param>
        /// <param name="DNSClient"></param>
        /// <param name="GetEVSEIds"></param>
        public OICPImporter(String                                           Identification,
                            String                                           Hostname,
                            IPPort                                           TCPPort,
                            EVSP_Id                                          ProviderId,
                            TimeSpan?                                        UpdateEVSEDataEvery      = null,
                            TimeSpan?                                        UpdateEVSEDataTimeout    = null,
                            TimeSpan?                                        UpdateEVSEStatusEvery    = null,
                            TimeSpan?                                        UpdateEVSEStatusTimeout  = null,
                            DNSClient                                        DNSClient                = null,
                            Func<IEnumerable<EVSE_Id>>                       GetEVSEIds               = null,
                            Action<XElement>                                 EVSEDataHandler          = null,
                            Action<KeyValuePair<EVSE_Id, HubjectEVSEState>>  EVSEStatusHandler        = null)

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

            if (EVSEDataHandler == null)
                throw new ArgumentNullException("The given EVSEDataHandler must not be null!");

            if (EVSEStatusHandler == null)
                throw new ArgumentNullException("The given EVSEStatusHandler must not be null!");

            #endregion

            #region Init parameters

            this._Identification        = Identification;
            this._Hostname              = Hostname;
            this._TCPPort               = TCPPort;
            this._ProviderId            = ProviderId;

            if (!UpdateEVSEDataEvery.HasValue)
                this._UpdateEVSEDataEvery      = DefaultUpdateEVSEDataEvery;

            if (!UpdateEVSEDataTimeout.HasValue)
                this._UpdateEVSEDataTimeout    = DefaultUpdateEVSEDataTimeout;

            if (!UpdateEVSEStatusEvery.HasValue)
                this._UpdateEVSEStatusEvery    = DefaultUpdateEVSEStatusEvery;

            if (!UpdateEVSEStatusTimeout.HasValue)
                this._UpdateEVSEStatusTimeout  = DefaultUpdateEVSEStatusTimeout;

            this._DNSClient             = DNSClient != null
                                              ? DNSClient
                                              : new DNSClient();

            this._GetEVSEIds            = GetEVSEIds;
            this._EVSEDataHandler       = EVSEDataHandler;
            this._EVSEStatusHandler     = EVSEStatusHandler;

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

                try
                {

                    Debug.WriteLine("[" + DateTime.Now + "] 'UpdateEVSEData' started");

                    OICPUpstreamService.
                        PullEVSEDataRequest(_ProviderId).
                        ContinueWith(PullEVSEDataTask => {

                            if (PullEVSEDataTask.Result != null)
                                PullEVSEDataTask.Result.Content.ForEach(_EVSEDataHandler);

                        }).
                        Wait();

                    Debug.WriteLine("[" + DateTime.Now + "] 'UpdateEVSEData' finished!");

                }
                catch (Exception e)
                {
                    Debug.WriteLine("[" + DateTime.Now + "] 'UpdateEVSEData' lead to an exception: " + e.Message);
                }

                finally
                {
                    Monitor.Exit(UpdateEVSEsLock);
                }

            }

            else
                Debug.WriteLine("[" + DateTime.Now + "] 'UpdateEVSEData' skipped!");

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

                try
                {

                    Debug.WriteLine("[" + DateTime.Now + "] 'UpdateEVSEStatus' started");

                    Task.WaitAll(_GetEVSEIds(). // Get the data via the GetEVSEIds delegate!
                                     ToPartitions(100). // Hubject has a limit of 100 EVSEIds per request!
                                     Select(EVSEPartition =>

                                         OICPUpstreamService.
                                             PullEVSEStatusByIdRequest(_ProviderId, EVSEPartition).
                                             ContinueWith(NewEVSEStatusTask => {

                                                 if (NewEVSEStatusTask.Result != null)
                                                     NewEVSEStatusTask.Result.Content.ForEach(_EVSEStatusHandler);

                                             }))
                                             .ToArray(),

                                 millisecondsTimeout: (Int32) _UpdateEVSEStatusTimeout.TotalMilliseconds
                                 //CancellationToken cancellationToken
                                );

                    Debug.WriteLine("[" + DateTime.Now + "] 'UpdateEVSEStatus' finished!");

                }
                catch (Exception e)
                {
                    Debug.WriteLine("[" + DateTime.Now + "] 'UpdateEVSEStatus' lead to an exception: " + e.Message);
                }

                finally
                {
                    Monitor.Exit(UpdateEVSEsLock);
                }

            }

            else
                Debug.WriteLine("[" + DateTime.Now + "] 'UpdateEVSEStatus' skipped!");

        }

        #endregion

    }

}
