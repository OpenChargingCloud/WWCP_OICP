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

using System.Net.Security;
using System.Security.Authentication;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Sockets;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;

using cloud.charging.open.protocols.OICPv2_3.CPO;
using cloud.charging.open.protocols.OICPv2_3.EMP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.p2p
{

    /// <summary>
    /// The CPO p2p combines the CPOClient(s) and EMPClientAPI
    /// and adds additional logging for all.
    /// </summary>
    public class CPOp2pAPI
    {

        #region Data

        /// <summary>
        /// The default HTTP server name.
        /// </summary>
        public  const              String                  DefaultHTTPServerName           = "Open Charging Cloud - CPO p2p HTTP API";

        /// <summary>
        /// The default HTTP service name.
        /// </summary>
        public  const              String                  DefaultHTTPServiceName          = "Open Charging Cloud - CPO p2p HTTP API";

        private readonly HTTPAPI? httpAPI;

        #endregion

        #region Properties

        /// <summary>
        /// The EMP client API.
        /// </summary>
        public EMPClientAPI  EMPClientAPI    { get; }


        private readonly HashSet<CPOClient> cpoClients;

        /// <summary>
        /// All CPO clients.
        /// </summary>
        public IEnumerable<CPOClient> CPOClients
            => cpoClients;

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

        /// <summary>
        /// Create a new CPO p2p service.
        /// </summary>
        public CPOp2pAPI(HTTPHostname?                         HTTPHostname                       = null,
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
                         DNSClient?                            DNSClient                          = null,
                         Boolean                               Autostart                          = false)

        {

            httpAPI = new HTTPAPI(HTTPHostname,
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

            httpAPI.HTTPServer.AddMethodCallback(org.GraphDefined.Vanaheimr.Hermod.HTTP.HTTPHostname.Any,
                                                 HTTPMethod.GET,
                                                 new HTTPPath[] {
                                                     URLPathPrefix + "/",
                                                     URLPathPrefix + "/{FileName}"
                                                 },
                                                 HTTPDelegate: Request => {
                                                     return Task.FromResult(
                                                         new HTTPResponse.Builder(Request) {
                                                             HTTPStatusCode  = HTTPStatusCode.OK,
                                                             Server          = httpAPI.HTTPServer.DefaultServerName,
                                                             Date            = Timestamp.Now,
                                                             ContentType     = HTTPContentType.TEXT_UTF8,
                                                             Content         = "This is an OICP v2.3 CPO p2p HTTP/JSON endpoint!".ToUTF8Bytes(),
                                                             CacheControl    = "public, max-age=300",
                                                             Connection      = "close"
                                                         }.AsImmutable);
                                                 });

            this.EMPClientAPI          = new EMPClientAPI(httpAPI);

            // Link HTTP events...
            EMPClientAPI.RequestLog   += (HTTPProcessor, ServerTimestamp, Request)                                 => RequestLog. WhenAll(HTTPProcessor, ServerTimestamp, Request);
            EMPClientAPI.ResponseLog  += (HTTPProcessor, ServerTimestamp, Request, Response)                       => ResponseLog.WhenAll(HTTPProcessor, ServerTimestamp, Request, Response);
            EMPClientAPI.ErrorLog     += (HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException) => ErrorLog.   WhenAll(HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException);

            this.cpoClients            = new HashSet<CPOClient>();

            if (Autostart)
                httpAPI.Start();

        }

        #endregion


        #region Start()

        public void Start()
        {

            if (httpAPI is not null)
                httpAPI.Start();

            else
            {
                EMPClientAPI.Start();
            }

        }

        #endregion

        #region Shutdown(Message = null, Wait = true)

        public void Shutdown(String?  Message   = null,
                             Boolean  Wait      = true)
        {

            if (httpAPI is not null)
                httpAPI.Shutdown(Message, Wait);

            else
            {
                EMPClientAPI.Shutdown(Message, Wait);
            }

        }

        #endregion

        #region Dispose()

        public void Dispose()
        {

            if (httpAPI is not null)
                httpAPI.Dispose();

            else
            {

                if (EMPClientAPI is not null)
                    EMPClientAPI.Dispose();

            }

            foreach (var cpoClient in CPOClients)
                cpoClient.Dispose();

        }

        #endregion

    }

}
