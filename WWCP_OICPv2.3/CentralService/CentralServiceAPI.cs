/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using org.GraphDefined.Vanaheimr.Hermod.Mail;
using org.GraphDefined.Vanaheimr.Hermod.SMTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CentralService
{

    /// <summary>
    /// The central ev roaming combines the EMPClientAPI, EMPServerAPIClient, CPOClientAPI and CPOServerAPIClient
    /// and adds additional logging for all.
    /// </summary>
    public class CentralServiceAPI : AHTTPExtAPIExtension1<HTTPExtAPI>
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

        ///// <summary>
        ///// An event called whenever a HTTP request came in.
        ///// </summary>
        //public HTTPRequestLogEvent   RequestLog    = new();

        ///// <summary>
        ///// An event called whenever a HTTP request could successfully be processed.
        ///// </summary>
        //public HTTPResponseLogEvent  ResponseLog   = new();

        ///// <summary>
        ///// An event called whenever a HTTP request resulted in an error.
        ///// </summary>
        //public HTTPErrorLogEvent     ErrorLog      = new();

        #endregion

        #endregion

        #region Custom request mappers


        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new central ev roaming service.
        /// </summary>
        public CentralServiceAPI(HTTPExtAPI                     HTTPAPI,

                                 IEnumerable<HTTPHostname>?     Hostnames                 = null,
                                 HTTPPath?                      RootPath                  = null,
                                 IEnumerable<HTTPContentType>?  HTTPContentTypes          = null,
                                 I18NString?                    Description               = null,

                                 HTTPPath?                      BasePath                  = null,  // For URL prefixes in HTML!

                                 String?                        ExternalDNSName           = null,
                                 String?                        HTTPServerName            = DefaultHTTPServerName,
                                 String?                        HTTPServiceName           = DefaultHTTPServiceName,
                                 String?                        APIVersionHash            = null,
                                 JObject?                       APIVersionHashes          = null,

                                 EMailAddress?                  APIRobotEMailAddress      = null,
                                 String?                        APIRobotGPGPassphrase     = null,
                                 ISMTPClient?                   SMTPClient                = null,

                                 HTTPPath?                      AdditionalURLPathPrefix   = null,
                                 Boolean?                       LocationsAsOpenData       = null,
                                 Boolean?                       TariffsAsOpenData         = null,
                                 Boolean?                       AllowDowngrades           = null,

                                 Boolean                        RegisterRootService       = true,
                                 String?                        RemotePartyDBFileName     = null,

                                 Boolean?                       IsDevelopment             = null,
                                 IEnumerable<String>?           DevelopmentServers        = null,
                                 //Boolean?                       SkipURLTemplates          = false,
                                 String?                        DatabaseFileName          = "DefaultAssetsDBFileName",
                                 Boolean?                       DisableNotifications      = false,

                                 Boolean?                       DisableLogging            = null,
                                 String?                        LoggingContext            = null,
                                 String?                        LoggingPath               = null,
                                 String?                        LogfileName               = null,
                                 LogfileCreatorDelegate?        LogfileCreator            = null)


            : base(Description ?? I18NString.Create("CentralService API"),
                   HTTPAPI,
                   RootPath,
                   BasePath,

                   ExternalDNSName,
                   HTTPServerName,
                   HTTPServiceName,
                   APIVersionHash,
                   APIVersionHashes,

                   IsDevelopment,
                   DevelopmentServers,
                   DisableLogging,
                   LoggingPath,
                   LogfileName,
                   LogfileCreator is not null
                       ? (loggingPath, context, logfileName) => LogfileCreator(loggingPath, context, logfileName)
                       : (loggingPath, context, logfileName) => String.Concat(
                                                                    loggingPath + Path.DirectorySeparatorChar,
                                                                 //   remoteParty is not null
                                                                 //       ? remoteParty.Id.ToString() + Path.DirectorySeparatorChar
                                                                 //       : null,
                                                                    context is not null ? context + "_" : "",
                                                                    logfileName, "_",
                                                                    Timestamp.Now.Year, "-",
                                                                    Timestamp.Now.Month.ToString("D2"),
                                                                    ".log"
                                                                ))

        {

            if (RegisterRootService)
                HTTPBaseAPI.AddHandler(
                    HTTPPath.Root,
                    HTTPMethod:    HTTPMethod.GET,
                    HTTPDelegate:  request => {
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode  = HTTPStatusCode.OK,
                                Server          = HTTPServerName,
                                Date            = Timestamp.Now,
                                ContentType     = HTTPContentType.Text.PLAIN,
                                Content         = "This is an OICP v2.3 Central Service HTTP/JSON endpoint!".ToUTF8Bytes(),
                                CacheControl    = "public, max-age=300"
                              //  Connection      = this.Connection
                            }.AsImmutable);
                    },
                    AllowReplacement: URLReplacement.Allow
                );

            this.EMPClientAPI  = new EMPClientAPI(HTTPBaseAPI);
            this.CPOClientAPI  = new CPOClientAPI(HTTPBaseAPI);

            //// Link HTTP events...
            //EMPClientAPI.RequestLog   += RequestLog. WhenAll;
            //EMPClientAPI.ResponseLog  += ResponseLog.WhenAll;
            //EMPClientAPI.ErrorLog     += ErrorLog.   WhenAll;

            //CPOClientAPI.RequestLog   += RequestLog. WhenAll;
            //CPOClientAPI.ResponseLog  += ResponseLog.WhenAll;
            //CPOClientAPI.ErrorLog     += ErrorLog.   WhenAll;

            this.EMPServerAPIClients   = [];
            this.CPOServerAPIClients   = [];

            //if (AutoStart)
            //    httpAPI.Start();

        }

        #endregion


    }

}
