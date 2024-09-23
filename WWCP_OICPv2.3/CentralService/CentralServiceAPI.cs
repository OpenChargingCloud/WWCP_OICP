/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System.Security.Authentication;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Logging;
using org.GraphDefined.Vanaheimr.Hermod.Sockets;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;

using cloud.charging.open.protocols.OICPv2_3.CPO;
using cloud.charging.open.protocols.OICPv2_3.EMP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CentralService
{

    /// <summary>
    /// The central ev roaming combines the EMPClientAPI, EMPServerAPIClient, CPOClientAPI and CPOServerAPIClient
    /// and adds additional logging for all.
    /// </summary>
    public class CentralServiceAPI
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

        private readonly HTTPAPI? httpAPI;

        #endregion

        #region Properties

        /// <summary>
        /// The EMP client API.
        /// </summary>
        public EMPClientAPI        EMPClientAPI          { get; }

        /// <summary>
        /// All EMP Server API clients.
        /// </summary>
        public readonly Dictionary<Provider_Id, EMPServerAPIClient> EMPServerAPIClients;


        /// <summary>
        /// The CPO client API.
        /// </summary>
        public CPOClientAPI        CPOClientAPI          { get; }

        public readonly Dictionary<Operator_Id, CPOServerAPIClient> CPOServerAPIClients;

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

        #region CentralServiceAPI(HTTPHostname = null, ...)

        /// <summary>
        /// Create a new central ev roaming service.
        /// </summary>
        public CentralServiceAPI(HTTPHostname?                                              HTTPHostname                 = null,
                                 String?                                                    ExternalDNSName              = null,
                                 IPPort?                                                    HTTPServerPort               = null,
                                 HTTPPath?                                                  BasePath                     = null,
                                 String?                                                    HTTPServerName               = DefaultHTTPServerName,

                                 HTTPPath?                                                  URLPathPrefix                = null,
                                 String?                                                    HTTPServiceName              = DefaultHTTPServiceName,
                                 String?                                                    HTMLTemplate                 = null,
                                 JObject?                                                   APIVersionHashes             = null,

                                 ServerCertificateSelectorDelegate?                         ServerCertificateSelector    = null,
                                 RemoteTLSClientCertificateValidationHandler<IHTTPServer>?  ClientCertificateValidator   = null,
                                 LocalCertificateSelectionHandler?                          ClientCertificateSelector    = null,
                                 SslProtocols?                                              AllowedTLSProtocols          = null,
                                 Boolean?                                                   ClientCertificateRequired    = null,
                                 Boolean?                                                   CheckCertificateRevocation   = null,

                                 ServerThreadNameCreatorDelegate?                           ServerThreadNameCreator      = null,
                                 ServerThreadPriorityDelegate?                              ServerThreadPrioritySetter   = null,
                                 Boolean?                                                   ServerThreadIsBackground     = null,
                                 ConnectionIdBuilder?                                       ConnectionIdBuilder          = null,
                                 TimeSpan?                                                  ConnectionTimeout            = null,
                                 UInt32?                                                    MaxClientConnections         = null,

                                 Boolean?                                                   DisableMaintenanceTasks      = null,
                                 TimeSpan?                                                  MaintenanceInitialDelay      = null,
                                 TimeSpan?                                                  MaintenanceEvery             = null,

                                 Boolean?                                                   DisableWardenTasks           = null,
                                 TimeSpan?                                                  WardenInitialDelay           = null,
                                 TimeSpan?                                                  WardenCheckEvery             = null,

                                 Boolean?                                                   IsDevelopment                = null,
                                 IEnumerable<String>?                                       DevelopmentServers           = null,
                                 Boolean?                                                   DisableLogging               = null,
                                 String?                                                    LoggingPath                  = null,
                                 String?                                                    LogfileName                  = null,
                                 LogfileCreatorDelegate?                                    LogfileCreator               = null,
                                 DNSClient?                                                 DNSClient                    = null,
                                 Boolean                                                    AutoStart                    = false)

        {

            httpAPI = new HTTPAPI(
                          HTTPHostname,
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

                          ServerThreadNameCreator,
                          ServerThreadPrioritySetter,
                          ServerThreadIsBackground,
                          ConnectionIdBuilder,
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
                          false
                      );

            httpAPI.AddMethodCallback(org.GraphDefined.Vanaheimr.Hermod.HTTP.HTTPHostname.Any,
                                      HTTPMethod.GET,
                                      [
                                          URLPathPrefix + "/",
                                          URLPathPrefix + "/{FileName}"
                                      ],
                                      HTTPDelegate: Request => {
                                          return Task.FromResult(
                                              new HTTPResponse.Builder(Request) {
                                                  HTTPStatusCode  = HTTPStatusCode.OK,
                                                  Server          = httpAPI.HTTPServer.DefaultServerName,
                                                  Date            = Timestamp.Now,
                                                  ContentType     = HTTPContentType.Text.PLAIN,
                                                  Content         = "This is an OICP v2.3 Central Service HTTP/JSON endpoint!".ToUTF8Bytes(),
                                                  CacheControl    = "public, max-age=300",
                                                  Connection      = ConnectionType.Close
                                              }.AsImmutable);
                                      });

            this.EMPClientAPI          = new EMPClientAPI(httpAPI);
            this.CPOClientAPI          = new CPOClientAPI(httpAPI);

            // Link HTTP events...
            EMPClientAPI.RequestLog   += RequestLog. WhenAll;
            EMPClientAPI.ResponseLog  += ResponseLog.WhenAll;
            EMPClientAPI.ErrorLog     += ErrorLog.   WhenAll;

            CPOClientAPI.RequestLog   += RequestLog. WhenAll;
            CPOClientAPI.ResponseLog  += ResponseLog.WhenAll;
            CPOClientAPI.ErrorLog     += ErrorLog.   WhenAll;

            this.EMPServerAPIClients   = [];
            this.CPOServerAPIClients   = [];

            if (AutoStart)
                httpAPI.Start();

        }

        #endregion

        #region CentralServiceAPI(EMPClientAPI, CPOClientAPI)

        /// <summary>
        /// Create a new central ev roaming service.
        /// </summary>
        /// <param name="EMPServer">An EMP Server.</param>
        /// <param name="EMPClient">An EMP client.</param>
        public CentralServiceAPI(EMPClientAPI  EMPClientAPI,
                                 CPOClientAPI  CPOClientAPI)
        {

            this.EMPClientAPI          = EMPClientAPI ?? throw new ArgumentNullException(nameof(EMPClientAPI), "The given EMPClientAPI must not be null!");
            this.CPOClientAPI          = CPOClientAPI ?? throw new ArgumentNullException(nameof(CPOClientAPI), "The given CPOClientAPI must not be null!");

            // Link HTTP events...
            EMPClientAPI.RequestLog   += RequestLog. WhenAll;
            EMPClientAPI.ResponseLog  += ResponseLog.WhenAll;
            EMPClientAPI.ErrorLog     += ErrorLog.   WhenAll;

            CPOClientAPI.RequestLog   += RequestLog. WhenAll;
            CPOClientAPI.ResponseLog  += ResponseLog.WhenAll;
            CPOClientAPI.ErrorLog     += ErrorLog.   WhenAll;

            this.EMPServerAPIClients   = [];
            this.CPOServerAPIClients   = [];

        }

        #endregion

        #endregion


        #region Start()

        public void Start()
        {

            if (httpAPI is not null)
                httpAPI.Start();

            else
            {
                EMPClientAPI.Start();
                CPOClientAPI.Start();
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
                CPOClientAPI.Shutdown(Message, Wait);
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
                EMPClientAPI?.Dispose();
                CPOClientAPI?.Dispose();
            }

            foreach (var empServerAPIClient in EMPServerAPIClients.Values)
                empServerAPIClient.Dispose();

            foreach (var cpoServerAPIClient in CPOServerAPIClients.Values)
                cpoServerAPIClient.Dispose();

        }

        #endregion

    }

}
