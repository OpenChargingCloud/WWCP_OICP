/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System.Runtime.CompilerServices;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.HTTPTest;
using org.GraphDefined.Vanaheimr.Hermod.Logging;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// An abstract OICP HTTP API.
    /// </summary>
    public abstract class AOICPHTTPAPI : HTTPAPIX
    {

        #region Data

        private   const String  DefaultHTTPServerName        = "OICP HTTP Server";
        private   const String  DefaultHTTPServiceName       = "OICP HTTP Service";
        private   const String  DefaultHTTPAPI_LoggingPath   = "logs/oicp/httpAPI";
        private   const String  DefaultLoggingContext        = "OICP HTTP API";
        private   const String  DefaultHTTPAPI_LogfileName   = "OICP_HTTP_API.log";

        #endregion

        #region Properties

        /// <summary>
        /// The optional URL path prefix, used when defining URL templates.
        /// </summary>
        public HTTPPath         URLPathPrefix     { get; }


        public Formatting       JSONFormatting    { get; }


        public ConnectionType?  Connection        { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new CPO HTTP Server API.
        /// </summary>
        /// <param name="HTTPHostname">The HTTP hostname for all URLs within this API.</param>
        /// <param name="ExternalDNSName">The official URL/DNS name of this service, e.g. for sending e-mails.</param>
        /// <param name="HTTPServerPort">A TCP port to listen on.</param>
        /// <param name="BasePath">When the API is served from an optional subdirectory path.</param>
        /// <param name="HTTPServerName">The default HTTP server name, used whenever no HTTP Host-header has been given.</param>
        /// 
        /// <param name="URLPathPrefix">A common prefix for all URLs.</param>
        /// <param name="HTTPServiceName">The name of the HTTP service.</param>
        /// <param name="APIVersionHashes">The API version hashes (git commit hash values).</param>
        /// 
        /// <param name="ServerCertificateSelector">An optional delegate to select a TLS server certificate.</param>
        /// <param name="ClientCertificateValidator">An optional delegate to verify the TLS client certificate used for authentication.</param>
        /// <param name="LocalCertificateSelector">An optional delegate to select the TLS client certificate used for authentication.</param>
        /// <param name="AllowedTLSProtocols">The TLS protocol(s) allowed for this connection.</param>
        /// 
        /// <param name="ConnectionIdBuilder">An optional delegate to build a connection identification based on IP socket information.</param>
        /// <param name="ConnectionTimeout">The TCP client timeout for all incoming client connections in seconds (default: 30 sec).</param>
        /// <param name="MaxClientConnections">The maximum number of concurrent TCP client connections (default: 4096).</param>
        /// 
        /// <param name="DisableMaintenanceTasks">Disable all maintenance tasks.</param>
        /// <param name="MaintenanceInitialDelay">The initial delay of the maintenance tasks.</param>
        /// <param name="MaintenanceEvery">The maintenance interval.</param>
        /// 
        /// <param name="DisableWardenTasks">Disable all warden tasks.</param>
        /// <param name="WardenInitialDelay">The initial delay of the warden tasks.</param>
        /// <param name="WardenCheckEvery">The warden interval.</param>
        /// 
        /// <param name="IsDevelopment">This HTTP API runs in development mode.</param>
        /// <param name="DevelopmentServers">An enumeration of server names which will imply to run this service in development mode.</param>
        /// <param name="DisableLogging">Disable the log file.</param>
        /// <param name="LoggingPath">The path for all logfiles.</param>
        /// <param name="LoggingContext">The context of all logfiles.</param>
        /// <param name="LogfileCreator">A delegate for creating the name of the logfile for this API.</param>
        /// <param name="DNSClient">The DNS client of the API.</param>
        /// <param name="AutoStart">Whether to start the API automatically.</param>
        public AOICPHTTPAPI(//HTTPHostname?                                              HTTPHostname                 = null,
                            //String?                                                    ExternalDNSName              = null,
                            //IPPort?                                                    HTTPServerPort               = null,
                            //HTTPPath?                                                  BasePath                     = null,
                            //String                                                     HTTPServerName               = DefaultHTTPServerName,
                            //
                            //HTTPPath?                                                  URLPathPrefix                = null,
                            //String                                                     HTTPServiceName              = DefaultHTTPServiceName,
                            //JObject?                                                   APIVersionHashes             = null,
                            //
                            //ServerCertificateSelectorDelegate?                         ServerCertificateSelector    = null,
                            //LocalCertificateSelectionHandler?                          LocalCertificateSelector     = null,
                            //RemoteTLSClientCertificateValidationHandler<IHTTPServer>?  ClientCertificateValidator   = null,
                            //SslProtocols?                                              AllowedTLSProtocols          = null,
                            //Boolean?                                                   ClientCertificateRequired    = null,
                            //Boolean?                                                   CheckCertificateRevocation   = null,
                            //
                            //ConnectionIdBuilder?                                       ConnectionIdBuilder          = null,
                            //TimeSpan?                                                  ConnectionTimeout            = null,
                            //UInt32?                                                    MaxClientConnections         = null,

                            //Boolean?                                                   DisableMaintenanceTasks      = false,
                            //TimeSpan?                                                  MaintenanceInitialDelay      = null,
                            //TimeSpan?                                                  MaintenanceEvery             = null,

                            //Boolean?                                                   DisableWardenTasks           = false,
                            //TimeSpan?                                                  WardenInitialDelay           = null,
                            //TimeSpan?                                                  WardenCheckEvery             = null,

                            //Boolean?                                                   IsDevelopment                = null,
                            //IEnumerable<String>?                                       DevelopmentServers           = null,
                            //Boolean                                                    DisableLogging               = false,
                            //String                                                     LoggingPath                  = DefaultHTTPAPI_LoggingPath,
                            //String                                                     LoggingContext               = DefaultLoggingContext,
                            //String                                                     LogfileName                  = DefaultHTTPAPI_LogfileName,
                            //LogfileCreatorDelegate?                                    LogfileCreator               = null,
                            //DNSClient?                                                 DNSClient                    = null,
                            //Boolean                                                    AutoStart                    = false)
                            HTTPTestServerX?               HTTPTestServer        = null,
                            IEnumerable<HTTPHostname>?     Hostnames             = null,
                            HTTPPath?                      RootPath              = null,
                            IEnumerable<HTTPContentType>?  HTTPContentTypes      = null,
                            I18NString?                    Description           = null,

                            String?                        ExternalDNSName       = null,
                            HTTPPath?                      BasePath              = null,

                            JObject?                       APIVersionHashes      = null,

                            Boolean                        RegisterRootService   = true,
                            HTTPPath?                      URLPathPrefix         = null,
                            Formatting?                    JSONFormatting        = null,
                            ConnectionType?                Connection            = null,

                            Boolean?                                                   DisableMaintenanceTasks      = false,
                            TimeSpan?                                                  MaintenanceInitialDelay      = null,
                            TimeSpan?                                                  MaintenanceEvery             = null,

                            Boolean?                                                   DisableWardenTasks           = false,
                            TimeSpan?                                                  WardenInitialDelay           = null,
                            TimeSpan?                                                  WardenCheckEvery             = null,

                            Boolean?                                                   IsDevelopment                = null,
                            IEnumerable<String>?                                       DevelopmentServers           = null,
                            Boolean                                                    DisableLogging               = false,
                            String                                                     LoggingPath                  = DefaultHTTPAPI_LoggingPath,
                            String                                                     LoggingContext               = DefaultLoggingContext,
                            String                                                     LogfileName                  = DefaultHTTPAPI_LogfileName,
                            LogfileCreatorDelegate?                                    LogfileCreator               = null)

            : base(HTTPTestServer,
                   Hostnames,
                   RootPath,
                   HTTPContentTypes,
                   Description,

            //: base(HTTPHostname,
            //       ExternalDNSName,
            //       HTTPServerPort,
            //       BasePath,
            //       HTTPServerName,

            //       URLPathPrefix,
            //       HTTPServiceName,
            //       null, //HTMLTemplate,
            //       APIVersionHashes,

            //       ServerCertificateSelector,
            //       ClientCertificateValidator,
            //       LocalCertificateSelector,
            //       AllowedTLSProtocols,
            //       ClientCertificateRequired,
            //       CheckCertificateRevocation,

            //       ConnectionIdBuilder,
            //       ConnectionTimeout,
            //       MaxClientConnections,

            //       DisableMaintenanceTasks,
            //       MaintenanceInitialDelay,
            //       MaintenanceEvery,

            //       DisableWardenTasks,
            //       WardenInitialDelay,
            //       WardenCheckEvery,

            //       IsDevelopment,
            //       DevelopmentServers,
            //       DisableLogging,
            //       LoggingPath,
            //       LogfileName,
            //       LogfileCreator,
            //       DNSClient,
            //       false) //AutoStart)

                   ExternalDNSName,
                   BasePath,

                   null,
                   null,
                   APIVersionHashes,

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
                   LoggingContext,
                   LogfileName,
                   LogfileCreator)

        {

            this.URLPathPrefix   = URLPathPrefix  ?? HTTPPath.Root;
            this.JSONFormatting  = JSONFormatting ?? Formatting.None;
            this.Connection      = Connection     ?? ConnectionType.KeepAlive;

        }

        #endregion


        #region (protected) LogEvent     (OICPIO, Logger, LogHandler, ...)

        protected async Task LogEvent<TDelegate>(String                                             OICPIO,
                                                 TDelegate?                                         Logger,
                                                 Func<TDelegate, Task>                              LogHandler,
                                                 [CallerArgumentExpression(nameof(Logger))] String  EventName     = "",
                                                 [CallerMemberName()]                       String  OICPCommand   = "")

            where TDelegate : Delegate

        {
            if (Logger is not null)
            {
                try
                {

                    await Task.WhenAll(
                              Logger.GetInvocationList().
                                     OfType<TDelegate>().
                                     Select(LogHandler)
                          );

                }
                catch (Exception e)
                {
                    await HandleErrors(OICPIO, $"{OICPCommand}.{EventName}", e);
                }
            }
        }

        #endregion

        #region (virtual)   HandleErrors (Module, Caller, ErrorResponse)

        public virtual Task HandleErrors(String  Module,
                                         String  Caller,
                                         String  ErrorResponse)
        {

            DebugX.Log($"{Module}.{Caller}: {ErrorResponse}");

            return Task.CompletedTask;

        }

        #endregion

        #region (virtual)   HandleErrors (Module, Caller, ExceptionOccurred)

        public virtual Task HandleErrors(String     Module,
                                         String     Caller,
                                         Exception  ExceptionOccurred)
        {

            DebugX.LogException(ExceptionOccurred, $"{Module}.{Caller}");

            return Task.CompletedTask;

        }

        #endregion


        #region Dispose()

        ///// <summary>
        ///// Dispose this object.
        ///// </summary>
        //public override void Dispose()
        //{
        //    base.Dispose();
        //}

        #endregion


    }

}
