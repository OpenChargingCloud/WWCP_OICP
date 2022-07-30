/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;
using System.Net.Security;
using System.Security.Authentication;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

using cloud.charging.open.protocols.OICPv2_3.CPO;
using cloud.charging.open.protocols.OICPv2_3.EMP;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;
using org.GraphDefined.Vanaheimr.Hermod.Sockets;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CentralService
{

    /// <summary>
    /// The central ev roaming combines the EMPClientAPI, EMPServerAPIClient, CPOClientAPI and CPOServerAPIClient
    /// and adds additional logging for all.
    /// </summary>
    public class CentralService
    {

        #region Data

        /// <summary>
        /// The default HTTP server name.
        /// </summary>
        public  const              String                  DefaultHTTPServerName           = "Open Charging Cloud - Central OICP HTTP API";

        /// <summary>
        /// The default HTTP service name.
        /// </summary>
        public  const              String                  DefaultHTTPServiceName          = "Open Charging Cloud - Central OICP HTTP API";

        #endregion

        #region Properties

        /// <summary>
        /// The EMP client API.
        /// </summary>
        public EMPClientAPI        EMPClientAPI          { get; }

        private readonly HashSet<EMPServerAPIClient> empServerAPIClients;

        /// <summary>
        /// All EMP Server API clients.
        /// </summary>
        public IEnumerable<EMPServerAPIClient> EMPServerAPIClients
            => empServerAPIClients;


        /// <summary>
        /// The CPO client API.
        /// </summary>
        public CPOClientAPI        CPOClientAPI          { get; }

        private readonly HashSet<CPOServerAPIClient> cpoServerAPIClients;

        /// <summary>
        /// All CPO Server API clients.
        /// </summary>
        public IEnumerable<CPOServerAPIClient> CPOServerAPIClients
            => cpoServerAPIClients;

        #endregion

        #region Events

        #region Generic HTTP server logging

        /// <summary>
        /// An event called whenever a HTTP request came in.
        /// </summary>
        public HTTPRequestLogEvent   RequestLog    = new ();

        /// <summary>
        /// An event called whenever a HTTP request could successfully be processed.
        /// </summary>
        public HTTPResponseLogEvent  ResponseLog   = new ();

        /// <summary>
        /// An event called whenever a HTTP request resulted in an error.
        /// </summary>
        public HTTPErrorLogEvent     ErrorLog      = new ();

        #endregion

        #endregion

        #region Custom request mappers


        #endregion

        #region Constructor(s)

        #region CentralService(HTTPHostname = null, ...)

        /// <summary>
        /// Create a new central ev roaming service.
        /// </summary>
        public CentralService(HTTPHostname?                         HTTPHostname                       = null,
                              String?                               ExternalDNSName                    = null,
                              IPPort?                               HTTPServerPort                     = null,
                              HTTPPath?                             BasePath                           = null,
                              String?                               HTTPServerName                     = DefaultHTTPServerName,

                              HTTPPath?                             URLPathPrefix                      = null,
                              String?                               HTTPServiceName                    = DefaultHTTPServiceName,
                              String?                               HTMLTemplate                       = null,
                              JObject?                              APIVersionHashes                   = null,
                              ServerCertificateSelectorDelegate?    ServerCertificateSelector          = null,
                              RemoteCertificateValidationCallback?  ClientCertificateValidator         = null,
                              LocalCertificateSelectionCallback?    ClientCertificateSelector          = null,
                              SslProtocols?                         AllowedTLSProtocols                = null,
                              Boolean?                              ClientCertificateRequired          = null,
                              Boolean?                              CheckCertificateRevocation         = null,

                              String?                               ServerThreadName                   = null,
                              ThreadPriority?                       ServerThreadPriority               = null,
                              Boolean?                              ServerThreadIsBackground           = null,
                              ConnectionIdBuilder?                  ConnectionIdBuilder                = null,
                              ConnectionThreadsNameBuilder?         ConnectionThreadsNameBuilder       = null,
                              ConnectionThreadsPriorityBuilder?     ConnectionThreadsPriorityBuilder   = null,
                              Boolean?                              ConnectionThreadsAreBackground     = null,
                              TimeSpan?                             ConnectionTimeout                  = null,
                              UInt32?                               MaxClientConnections               = null,

                              Boolean?                              DisableMaintenanceTasks            = null,
                              TimeSpan?                             MaintenanceInitialDelay            = null,
                              TimeSpan?                             MaintenanceEvery                   = null,

                              Boolean?                              DisableWardenTasks                 = null,
                              TimeSpan?                             WardenInitialDelay                 = null,
                              TimeSpan?                             WardenCheckEvery                   = null,

                              Boolean?                              IsDevelopment                      = null,
                              IEnumerable<String>?                  DevelopmentServers                 = null,
                              Boolean?                              DisableLogging                     = null,
                              String?                               LoggingPath                        = null,
                              String?                               LogfileName                        = null,
                              LogfileCreatorDelegate?               LogfileCreator                     = null,
                              DNSClient?                            DNSClient                          = null)

        {

            var httpAPI = new HTTPAPI(HTTPHostname,
                                      ExternalDNSName,
                                      HTTPServerPort,
                                      BasePath,
                                      HTTPServerName,

                                      URLPathPrefix,
                                      HTTPServiceName,
                                      HTMLTemplate,
                                      APIVersionHashes,

                                      ServerCertificateSelector,
                                      ClientCertificateValidator,
                                      ClientCertificateSelector,
                                      AllowedTLSProtocols,
                                      ClientCertificateRequired,
                                      CheckCertificateRevocation,

                                      ServerThreadName,
                                      ServerThreadPriority,
                                      ServerThreadIsBackground,

                                      ConnectionIdBuilder,
                                      ConnectionThreadsNameBuilder,
                                      ConnectionThreadsPriorityBuilder,
                                      ConnectionThreadsAreBackground,
                                      ConnectionTimeout,
                                      MaxClientConnections,

                                      DisableMaintenanceTasks,
                                      MaintenanceInitialDelay,
                                      MaintenanceEvery,

                                      DisableWardenTasks,
                                      WardenInitialDelay,
                                      WardenCheckEvery,

                                      IsDevelopment,
                                      DevelopmentServers,
                                      DisableLogging,
                                      LoggingPath,
                                      LogfileName,
                                      LogfileCreator,
                                      DNSClient,
                                      false);

            this.EMPClientAPI          = new EMPClientAPI(httpAPI);
            this.CPOClientAPI          = new CPOClientAPI(httpAPI);

            // Link HTTP events...
            EMPClientAPI.RequestLog   += (HTTPProcessor, ServerTimestamp, Request)                                 => RequestLog. WhenAll(HTTPProcessor, ServerTimestamp, Request);
            EMPClientAPI.ResponseLog  += (HTTPProcessor, ServerTimestamp, Request, Response)                       => ResponseLog.WhenAll(HTTPProcessor, ServerTimestamp, Request, Response);
            EMPClientAPI.ErrorLog     += (HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException) => ErrorLog.   WhenAll(HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException);

            CPOClientAPI.RequestLog   += (HTTPProcessor, ServerTimestamp, Request)                                 => RequestLog. WhenAll(HTTPProcessor, ServerTimestamp, Request);
            CPOClientAPI.ResponseLog  += (HTTPProcessor, ServerTimestamp, Request, Response)                       => ResponseLog.WhenAll(HTTPProcessor, ServerTimestamp, Request, Response);
            CPOClientAPI.ErrorLog     += (HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException) => ErrorLog.   WhenAll(HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException);

            this.empServerAPIClients   = new HashSet<EMPServerAPIClient>();
            this.cpoServerAPIClients   = new HashSet<CPOServerAPIClient>();

        }

        #endregion

        #region CentralService(EMPClientAPI, CPOClientAPI)

        /// <summary>
        /// Create a new central ev roaming service.
        /// </summary>
        /// <param name="EMPServer">An EMP Server.</param>
        /// <param name="EMPClient">An EMP client.</param>
        public CentralService(EMPClientAPI  EMPClientAPI,
                              CPOClientAPI  CPOClientAPI)
        {

            this.EMPClientAPI          = EMPClientAPI ?? throw new ArgumentNullException(nameof(EMPClientAPI), "The given EMPClientAPI must not be null!");
            this.CPOClientAPI          = CPOClientAPI ?? throw new ArgumentNullException(nameof(CPOClientAPI), "The given CPOClientAPI must not be null!");

            // Link HTTP events...
            EMPClientAPI.RequestLog   += (HTTPProcessor, ServerTimestamp, Request)                                 => RequestLog. WhenAll(HTTPProcessor, ServerTimestamp, Request);
            EMPClientAPI.ResponseLog  += (HTTPProcessor, ServerTimestamp, Request, Response)                       => ResponseLog.WhenAll(HTTPProcessor, ServerTimestamp, Request, Response);
            EMPClientAPI.ErrorLog     += (HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException) => ErrorLog.   WhenAll(HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException);

            CPOClientAPI.RequestLog   += (HTTPProcessor, ServerTimestamp, Request)                                 => RequestLog. WhenAll(HTTPProcessor, ServerTimestamp, Request);
            CPOClientAPI.ResponseLog  += (HTTPProcessor, ServerTimestamp, Request, Response)                       => ResponseLog.WhenAll(HTTPProcessor, ServerTimestamp, Request, Response);
            CPOClientAPI.ErrorLog     += (HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException) => ErrorLog.   WhenAll(HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException);

            this.empServerAPIClients   = new HashSet<EMPServerAPIClient>();
            this.cpoServerAPIClients   = new HashSet<CPOServerAPIClient>();

        }

        #endregion

        #endregion



        #region Start()

        public void Start()
        {
            EMPClientAPI.Start();
            CPOClientAPI.Start();
        }

        #endregion

        #region Shutdown(Message = null, Wait = true)

        public void Shutdown(String?  Message   = null,
                             Boolean  Wait      = true)
        {
            EMPClientAPI.Shutdown(Message, Wait);
            CPOClientAPI.Shutdown(Message, Wait);
        }

        #endregion

        #region Dispose()

        public void Dispose()
        {

            if (EMPClientAPI is not null)
                EMPClientAPI.Dispose();

            if (CPOClientAPI is not null)
                CPOClientAPI.Dispose();

            foreach (var empServerAPIClient in EMPServerAPIClients)
                empServerAPIClient.Dispose();

            foreach (var cpoServerAPIClient in CPOServerAPIClients)
                cpoServerAPIClient.Dispose();

        }

        #endregion

    }

}
