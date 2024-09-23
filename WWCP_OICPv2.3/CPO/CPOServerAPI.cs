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

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    /// <summary>
    /// The CPO Server API.
    /// </summary>
    public partial class CPOServerAPI : HTTPAPI
    {

        #region (class) APICounters

        public class APICounters(APICounterValues?  AuthorizeRemoteReservationStart   = null,
                                 APICounterValues?  AuthorizeRemoteReservationStop    = null,

                                 APICounterValues?  AuthorizeRemoteStart              = null,
                                 APICounterValues?  AuthorizeRemoteStop               = null)
        {

            public APICounterValues AuthorizeRemoteReservationStart    { get; } = AuthorizeRemoteReservationStart ?? new APICounterValues();
            public APICounterValues AuthorizeRemoteReservationStop     { get; } = AuthorizeRemoteReservationStop  ?? new APICounterValues();

            public APICounterValues AuthorizeRemoteStart               { get; } = AuthorizeRemoteStart            ?? new APICounterValues();
            public APICounterValues AuthorizeRemoteStop                { get; } = AuthorizeRemoteStop             ?? new APICounterValues();

            public JObject ToJSON()

                => JSONObject.Create(

                       new JProperty("AuthorizeRemoteReservationStart",  AuthorizeRemoteReservationStart.ToJSON()),
                       new JProperty("AuthorizeRemoteReservationStop",   AuthorizeRemoteReservationStop. ToJSON()),

                       new JProperty("AuthorizeRemoteStart",             AuthorizeRemoteStart.           ToJSON()),
                       new JProperty("AuthorizeRemoteStop",              AuthorizeRemoteStop.            ToJSON())

                   );

        }

        #endregion


        #region Data

        /// <summary>
        /// The default HTTP server name.
        /// </summary>
        public new const String  DefaultHTTPServerName   = "GraphDefined OICP " + Version.String + " CPO HTTP API";

        /// <summary>
        /// The default HTTP service name.
        /// </summary>
        public new const String  DefaultHTTPServiceName  = "GraphDefined OICP " + Version.String + " CPO HTTP API";

        /// <summary>
        /// The default logging context.
        /// </summary>
        public     const String  DefaultLoggingContext   = "CPOServerAPI";

        #endregion

        #region Properties

        /// <summary>
        /// The optional URL path prefix, used when defining URL templates.
        /// </summary>
        public new HTTPPath                URLPathPrefix     { get; }

        /// <summary>
        /// The attached HTTP logger.
        /// </summary>
        public new HTTP_Logger             HTTPLogger
#pragma warning disable CS8603 // Possible null reference return.
            => base.HTTPLogger as HTTP_Logger;
#pragma warning restore CS8603 // Possible null reference return.

        /// <summary>
        /// The attached Server API logger.
        /// </summary>
        public ServerAPILogger?            Logger            { get; }


        public APICounters                 Counters          { get; }

        public Newtonsoft.Json.Formatting  JSONFormatting    { get; set; }

        #endregion

        #region Custom JSON parsers

        public CustomJObjectParserDelegate<AuthorizeRemoteReservationStartRequest>?  CustomAuthorizeRemoteReservationStartRequestParser    { get; set; }

        public CustomJObjectParserDelegate<AuthorizeRemoteReservationStopRequest>?   CustomAuthorizeRemoteReservationStopRequestParser     { get; set; }


        public CustomJObjectParserDelegate<AuthorizeRemoteStartRequest>?             CustomAuthorizeRemoteStartRequestParser               { get; set; }

        public CustomJObjectParserDelegate<AuthorizeRemoteStopRequest>?              CustomAuthorizeRemoteStopRequestParser                { get; set; }

        #endregion

        #region Custom JSON serializers

        public CustomJObjectSerializerDelegate<Acknowledgement>?                     CustomAcknowledgementSerializer                       { get; set; }

        public CustomJObjectSerializerDelegate<StatusCode>?                          CustomStatusCodeSerializer                            { get; set; }

        #endregion

        #region Custom request/response logging converters

        #region AuthorizeRemoteReservationStart(Request/Response)Converter

        public Func<DateTime, Object, AuthorizeRemoteReservationStartRequest, String>
            AuthorizeRemoteReservationStartRequestConverter                     { get; set; }

            = (timestamp, sender, authorizeRemoteReservationStartRequest)
            => String.Concat(authorizeRemoteReservationStartRequest.Identification.ToString(), " at ", authorizeRemoteReservationStartRequest.EVSEId);

        public Func<DateTime, Object, AuthorizeRemoteReservationStartRequest, Acknowledgement<AuthorizeRemoteReservationStartRequest>, TimeSpan, String>
            AuthorizeRemoteReservationStartResponseConverter                    { get; set; }

            = (timestamp, sender, authorizeRemoteReservationStartRequest, authorizeRemoteReservationStartResponse, runtime)
            => String.Concat(authorizeRemoteReservationStartRequest.Identification.ToString(), " at ", authorizeRemoteReservationStartRequest.EVSEId,
                             " => ",
                             authorizeRemoteReservationStartResponse.StatusCode.ToString() ?? "failed!");

        #endregion

        #region AuthorizeRemoteReservationStop (Request/Response)Converter

        public Func<DateTime, Object, AuthorizeRemoteReservationStopRequest, String>
            AuthorizeRemoteReservationStopRequestConverter                     { get; set; }

            = (timestamp, sender, authorizeRemoteReservationStopRequest)
            => String.Concat(authorizeRemoteReservationStopRequest.SessionId.ToString(), " at ", authorizeRemoteReservationStopRequest.EVSEId);

        public Func<DateTime, Object, AuthorizeRemoteReservationStopRequest, Acknowledgement<AuthorizeRemoteReservationStopRequest>, TimeSpan, String>
            AuthorizeRemoteReservationStopResponseConverter                    { get; set; }

            = (timestamp, sender, authorizeRemoteReservationStopRequest, authorizeRemoteReservationStopResponse, runtime)
            => String.Concat(authorizeRemoteReservationStopRequest.SessionId.ToString(), " at ", authorizeRemoteReservationStopRequest.EVSEId,
                             " => ",
                             authorizeRemoteReservationStopResponse.StatusCode.ToString() ?? "failed!");

        #endregion

        #region AuthorizeRemoteStart           (Request/Response)Converter

        public Func<DateTime, Object, AuthorizeRemoteStartRequest, String>
            AuthorizeRemoteStartRequestConverter                     { get; set; }

            = (timestamp, sender, authorizeRemoteStartRequest)
            => String.Concat(authorizeRemoteStartRequest.Identification.ToString(), " at ", authorizeRemoteStartRequest.EVSEId);

        public Func<DateTime, Object, AuthorizeRemoteStartRequest, Acknowledgement<AuthorizeRemoteStartRequest>, TimeSpan, String>
            AuthorizeRemoteStartResponseConverter                    { get; set; }

            = (timestamp, sender, authorizeRemoteStartRequest, authorizeRemoteStartResponse, runtime)
            => String.Concat(authorizeRemoteStartRequest.Identification.ToString(), " at ", authorizeRemoteStartRequest.EVSEId,
                             " => ",
                             authorizeRemoteStartResponse.StatusCode.ToString() ?? "failed!");

        #endregion

        #region AuthorizeRemoteStop            (Request/Response)Converter

        public Func<DateTime, Object, AuthorizeRemoteStopRequest, String>
            AuthorizeRemoteStopRequestConverter                     { get; set; }

            = (timestamp, sender, authorizeRemoteStopRequest)
            => String.Concat(authorizeRemoteStopRequest.SessionId.ToString(), " at ", authorizeRemoteStopRequest.EVSEId);

        public Func<DateTime, Object, AuthorizeRemoteStopRequest, Acknowledgement<AuthorizeRemoteStopRequest>, TimeSpan, String>
            AuthorizeRemoteStopResponseConverter                    { get; set; }

            = (timestamp, sender, authorizeRemoteStopRequest, authorizeRemoteStopResponse, runtime)
            => String.Concat(authorizeRemoteStopRequest.SessionId.ToString(), " at ", authorizeRemoteStopRequest.EVSEId,
                             " => ",
                             authorizeRemoteStopResponse.StatusCode.ToString() ?? "failed!");

        #endregion

        #endregion

        #region Events

        #region (protected internal) OnAuthorizeRemoteReservationStartHTTPRequest

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteReservationStart HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnAuthorizeRemoteReservationStartHTTPRequest = new ();

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteReservationStart HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Server HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logAuthorizeRemoteReservationStartHTTPRequest(DateTime     Timestamp,
                                                                              HTTPAPI      API,
                                                                              HTTPRequest  Request)

            => OnAuthorizeRemoteReservationStartHTTPRequest.WhenAll(Timestamp,
                                                                    API ?? this,
                                                                    Request);

        #endregion

        #region (event)              OnAuthorizeRemoteReservationStart(Request-/Response)

        /// <summary>
        /// An event send whenever an AuthorizeRemoteReservationStart request was received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStartRequestDelegate?   OnAuthorizeRemoteReservationStartRequest;

        /// <summary>
        /// An event send whenever an AuthorizeRemoteReservationStart request was received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStartDelegate?          OnAuthorizeRemoteReservationStart;

        /// <summary>
        /// An event send whenever a response to an AuthorizeRemoteReservationStart request was sent.
        /// </summary>
        public event OnAuthorizeRemoteReservationStartResponseDelegate?  OnAuthorizeRemoteReservationStartResponse;

        #endregion

        #region (protected internal) OnAuthorizeRemoteReservationStartHTTPResponse

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteReservationStart HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnAuthorizeRemoteReservationStartHTTPResponse = new ();

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteReservationStart HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Server HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logAuthorizeRemoteReservationStartHTTPResponse(DateTime      Timestamp,
                                                                               HTTPAPI       API,
                                                                               HTTPRequest   Request,
                                                                               HTTPResponse  Response)

            => OnAuthorizeRemoteReservationStartHTTPResponse.WhenAll(Timestamp,
                                                                     API ?? this,
                                                                     Request,
                                                                     Response);

        #endregion


        #region (protected internal) OnAuthorizeRemoteReservationStopHTTPRequest

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteReservationStop HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnAuthorizeRemoteReservationStopHTTPRequest = new ();

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteReservationStop HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Server HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logAuthorizeRemoteReservationStopHTTPRequest(DateTime     Timestamp,
                                                                             HTTPAPI      API,
                                                                             HTTPRequest  Request)

            => OnAuthorizeRemoteReservationStopHTTPRequest.WhenAll(Timestamp,
                                                                   API ?? this,
                                                                   Request);

        #endregion

        #region (event)              OnAuthorizeRemoteReservationStop(Request-/Response)

        /// <summary>
        /// An event send whenever an AuthorizeRemoteReservationStop request was received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStopRequestDelegate?   OnAuthorizeRemoteReservationStopRequest;

        /// <summary>
        /// An event send whenever an AuthorizeRemoteReservationStop request was received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStopDelegate?          OnAuthorizeRemoteReservationStop;

        /// <summary>
        /// An event send whenever a response to an AuthorizeRemoteReservationStop request was sent.
        /// </summary>
        public event OnAuthorizeRemoteReservationStopResponseDelegate?  OnAuthorizeRemoteReservationStopResponse;

        #endregion

        #region (protected internal) OnAuthorizeRemoteReservationStopHTTPResponse

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteReservationStop HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnAuthorizeRemoteReservationStopHTTPResponse = new ();

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteReservationStop HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Server HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logAuthorizeRemoteReservationStopHTTPResponse(DateTime      Timestamp,
                                                                              HTTPAPI       API,
                                                                              HTTPRequest   Request,
                                                                              HTTPResponse  Response)

            => OnAuthorizeRemoteReservationStopHTTPResponse.WhenAll(Timestamp,
                                                                    API ?? this,
                                                                    Request,
                                                                    Response);

        #endregion



        #region (protected internal) OnAuthorizeRemoteStartHTTPRequest

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteStart HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnAuthorizeRemoteStartHTTPRequest = new ();

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteStart HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Server HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logAuthorizeRemoteStartHTTPRequest(DateTime     Timestamp,
                                                                   HTTPAPI      API,
                                                                   HTTPRequest  Request)

            => OnAuthorizeRemoteStartHTTPRequest.WhenAll(Timestamp,
                                                         API ?? this,
                                                         Request);

        #endregion

        #region (event)              OnAuthorizeRemoteStart(Request-/Response)

        /// <summary>
        /// An event send whenever an AuthorizeRemoteStart request was received.
        /// </summary>
        public event OnAuthorizeRemoteStartRequestDelegate?   OnAuthorizeRemoteStartRequest;

        /// <summary>
        /// An event send whenever an AuthorizeRemoteStart request was received.
        /// </summary>
        public event OnAuthorizeRemoteStartDelegate?          OnAuthorizeRemoteStart;

        /// <summary>
        /// An event send whenever a response to an AuthorizeRemoteStart request was sent.
        /// </summary>
        public event OnAuthorizeRemoteStartResponseDelegate?  OnAuthorizeRemoteStartResponse;

        #endregion

        #region (protected internal) OnAuthorizeRemoteStartHTTPResponse

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteStart HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnAuthorizeRemoteStartHTTPResponse = new ();

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteStart HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Server HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logAuthorizeRemoteStartHTTPResponse(DateTime      Timestamp,
                                                                    HTTPAPI       API,
                                                                    HTTPRequest   Request,
                                                                    HTTPResponse  Response)

            => OnAuthorizeRemoteStartHTTPResponse.WhenAll(Timestamp,
                                                          API ?? this,
                                                          Request,
                                                          Response);

        #endregion


        #region (protected internal) OnAuthorizeRemoteStopHTTPRequest

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteStop HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnAuthorizeRemoteStopHTTPRequest = new ();

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteStop HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Server HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logAuthorizeRemoteStopHTTPRequest(DateTime     Timestamp,
                                                                  HTTPAPI      API,
                                                                  HTTPRequest  Request)

            => OnAuthorizeRemoteStopHTTPRequest.WhenAll(Timestamp,
                                                        API ?? this,
                                                        Request);

        #endregion

        #region (event)              OnAuthorizeRemoteStop(Request-/Response)

        /// <summary>
        /// An event send whenever an AuthorizeRemoteStop request was received.
        /// </summary>
        public event OnAuthorizeRemoteStopRequestDelegate?   OnAuthorizeRemoteStopRequest;

        /// <summary>
        /// An event send whenever an AuthorizeRemoteStop request was received.
        /// </summary>
        public event OnAuthorizeRemoteStopDelegate?          OnAuthorizeRemoteStop;

        /// <summary>
        /// An event send whenever a response to an AuthorizeRemoteStop request was sent.
        /// </summary>
        public event OnAuthorizeRemoteStopResponseDelegate?  OnAuthorizeRemoteStopResponse;

        #endregion

        #region (protected internal) OnAuthorizeRemoteStopHTTPResponse

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteStop HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnAuthorizeRemoteStopHTTPResponse = new ();

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteStop HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Server HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logAuthorizeRemoteStopHTTPResponse(DateTime      Timestamp,
                                                                   HTTPAPI       API,
                                                                   HTTPRequest   Request,
                                                                   HTTPResponse  Response)

            => OnAuthorizeRemoteStopHTTPResponse.WhenAll(Timestamp,
                                                         API ?? this,
                                                         Request,
                                                         Response);

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new CPO HTTP Server API.
        /// </summary>
        /// <param name="HTTPHostname">The HTTP hostname for all URLs within this API.</param>
        /// <param name="ExternalDNSName">The offical URL/DNS name of this service, e.g. for sending e-mails.</param>
        /// <param name="HTTPServerPort">A TCP port to listen on.</param>
        /// <param name="BasePath">When the API is served from an optional subdirectory path.</param>
        /// <param name="HTTPServerName">The default HTTP servername, used whenever no HTTP Host-header has been given.</param>
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
        /// <param name="ServerThreadName">The optional name of the TCP server thread.</param>
        /// <param name="ServerThreadPriority">The optional priority of the TCP server thread.</param>
        /// <param name="ServerThreadIsBackground">Whether the TCP server thread is a background thread or not.</param>
        /// <param name="ConnectionIdBuilder">An optional delegate to build a connection identification based on IP socket information.</param>
        /// <param name="ConnectionTimeout">The TCP client timeout for all incoming client connections in seconds (default: 30 sec).</param>
        /// <param name="MaxClientConnections">The maximum number of concurrent TCP client connections (default: 4096).</param>
        /// 
        /// <param name="DisableMaintenanceTasks">Disable all maintenance tasks.</param>
        /// <param name="MaintenanceInitialDelay">The initial delay of the maintenance tasks.</param>
        /// <param name="MaintenanceEvery">The maintenance intervall.</param>
        /// 
        /// <param name="DisableWardenTasks">Disable all warden tasks.</param>
        /// <param name="WardenInitialDelay">The initial delay of the warden tasks.</param>
        /// <param name="WardenCheckEvery">The warden intervall.</param>
        /// 
        /// <param name="IsDevelopment">This HTTP API runs in development mode.</param>
        /// <param name="DevelopmentServers">An enumeration of server names which will imply to run this service in development mode.</param>
        /// <param name="DisableLogging">Disable the log file.</param>
        /// <param name="LoggingPath">The path for all logfiles.</param>
        /// <param name="LoggingContext">The context of all logfiles.</param>
        /// <param name="LogfileCreator">A delegate for creating the name of the logfile for this API.</param>
        /// <param name="DNSClient">The DNS client of the API.</param>
        /// <param name="AutoStart">Whether to start the API automatically.</param>
        public CPOServerAPI(HTTPHostname?                                              HTTPHostname                 = null,
                            String?                                                    ExternalDNSName              = null,
                            IPPort?                                                    HTTPServerPort               = null,
                            HTTPPath?                                                  BasePath                     = null,
                            String                                                     HTTPServerName               = DefaultHTTPServerName,

                            HTTPPath?                                                  URLPathPrefix                = null,
                            String                                                     HTTPServiceName              = DefaultHTTPServiceName,
                            JObject?                                                   APIVersionHashes             = null,

                            ServerCertificateSelectorDelegate?                         ServerCertificateSelector    = null,
                            LocalCertificateSelectionHandler?                          LocalCertificateSelector     = null,
                            RemoteTLSClientCertificateValidationHandler<IHTTPServer>?  ClientCertificateValidator   = null,
                            SslProtocols?                                              AllowedTLSProtocols          = null,
                            Boolean?                                                   ClientCertificateRequired    = null,
                            Boolean?                                                   CheckCertificateRevocation   = null,

                            ServerThreadNameCreatorDelegate?                           ServerThreadNameCreator      = null,
                            ServerThreadPriorityDelegate?                              ServerThreadPrioritySetter   = null,
                            Boolean?                                                   ServerThreadIsBackground     = null,
                            ConnectionIdBuilder?                                       ConnectionIdBuilder          = null,
                            TimeSpan?                                                  ConnectionTimeout            = null,
                            UInt32?                                                    MaxClientConnections         = null,

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
                            LogfileCreatorDelegate?                                    LogfileCreator               = null,
                            DNSClient?                                                 DNSClient                    = null,
                            Boolean                                                    AutoStart                    = false)

            : base(HTTPHostname,
                   ExternalDNSName,
                   HTTPServerPort,
                   BasePath,
                   HTTPServerName,

                   URLPathPrefix,
                   HTTPServiceName,
                   null, //HTMLTemplate,
                   APIVersionHashes,

                   ServerCertificateSelector,
                   ClientCertificateValidator,
                   LocalCertificateSelector,
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
                   false) //AutoStart)

        {

            this.URLPathPrefix   = base.URLPathPrefix + (URLPathPrefix ?? HTTPPath.Root);

            this.Counters        = new APICounters();

            this.JSONFormatting  = Newtonsoft.Json.Formatting.None;

            base.HTTPLogger      = this.DisableLogging == false
                                       ? new HTTP_Logger(
                                             this,
                                             LoggingPath,
                                             LoggingContext ?? DefaultLoggingContext,
                                             LogfileCreator
                                         )
                                       : null;

            this.Logger          = this.DisableLogging == false
                                       ? new ServerAPILogger(
                                             this,
                                             LoggingPath,
                                             LoggingContext ?? DefaultLoggingContext,
                                             LogfileCreator
                                         )
                                       : null;

            RegisterURLTemplates();

            if (AutoStart)
                Start();

        }

        #endregion


        #region (private) RegisterURLTemplates(RegisterRootService = true)

        private void RegisterURLTemplates(Boolean RegisterRootService = true)
        {

            #region / (HTTPRoot)

            if (RegisterRootService)
                AddMethodCallback(HTTPHostname.Any,
                                  HTTPMethod.GET,
                                  [
                                      URLPathPrefix + "/",
                                      URLPathPrefix + "/{FileName}"
                                  ],
                                  HTTPDelegate: Request => {
                                      return Task.FromResult(
                                          new HTTPResponse.Builder(Request) {
                                              HTTPStatusCode  = HTTPStatusCode.OK,
                                              Server          = HTTPServer.DefaultServerName,
                                              Date            = Timestamp.Now,
                                              ContentType     = HTTPContentType.Text.PLAIN,
                                              Content         = "This is an OICP v2.3 CPO Server HTTP/JSON endpoint!".ToUTF8Bytes(),
                                              CacheControl    = "public, max-age=300",
                                              Connection      = ConnectionType.Close
                                          }.AsImmutable);
                                  },
                                  AllowReplacement: URLReplacement.Allow);

            #endregion


            #region POST  ~/api/oicp/charging/v21/providers/{providerId}/authorize-remote-reservation/start

            // POST /api/oicp/charging/v21/providers/DE*ICE/authorize-remote-reservation/start HTTP/1.1
            // Accept:          application/json;charset=UTF-8
            // Content-Type:    application/json;charset=UTF-8
            // Content-Length:  250
            // Host:            api.chargeit-mobility.com:3000
            // Connection:      Keep-Alive
            // User-Agent:      Apache-HttpAsyncClient/4.1.4 (Java/1.8.0_265)
            // 
            // {
            //     "SessionID":           "b3be9e19-ca56-4965-80f1-ba835d5069c7",
            //     "CPOPartnerSessionID":  null,
            //     "EMPPartnerSessionID":  null,
            //     "ProviderID":          "DE*ICE",
            //     "EvseID":              "DE*BDO*E*TEST*1",
            //     "Identification": {
            //         "RemoteReservationIdentification": {
            //             "EvcoID":          "DE*ICE*I01000*6"
            //         }
            //     },
            //     "PartnerProductID": null
            // }

            // ---------------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3000/api/oicp/charging/v21/providers/{providerId}/authorize-remote-reservation/start
            // ---------------------------------------------------------------------------------------------------------------------------------------------------
            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.POST,
                              URLPathPrefix + "api/oicp/charging/v21/providers/{providerId}/authorize-remote-reservation/start",
                              HTTPContentType.Application.JSON_UTF8,
                              HTTPRequestLogger:   logAuthorizeRemoteReservationStartHTTPRequest,
                              HTTPResponseLogger:  logAuthorizeRemoteReservationStartHTTPResponse,
                              HTTPDelegate:        async Request => {

                                  var startTime  = Timestamp.Now;
                                  var processId  = Request.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse);

                                  Acknowledgement<AuthorizeRemoteReservationStartRequest>? authorizeRemoteReservationStartResponse = null;

                                  try
                                  {

                                      #region Try to parse ProviderId URL parameter

                                      if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                      {

                                          Counters.AuthorizeRemoteReservationStart.IncRequests_Error();

                                          authorizeRemoteReservationStartResponse = Acknowledgement<AuthorizeRemoteReservationStartRequest>.SystemError(
                                                                                        null,
                                                                                        "The expected 'providerId' URL parameter could not be parsed!"
                                                                                    );

                                      }

                                      #endregion

                                      else if (AuthorizeRemoteReservationStartRequest.TryParse(JObject.Parse(Request.HTTPBody?.ToUTF8String() ?? ""),
                                                                                               providerId,
                                                                                               out var authorizeRemoteReservationStartRequest,
                                                                                               out var errorResponse,
                                                                                               processId,
                                                                                               Request.Timestamp,
                                                                                               Request.EventTrackingId,
                                                                                               Request.Timeout ?? DefaultRequestTimeout,
                                                                                               CustomAuthorizeRemoteReservationStartRequestParser,
                                                                                               Request.CancellationToken))
                                      {

                                          Counters.AuthorizeRemoteReservationStart.IncRequests_OK();

                                          #region Send OnAuthorizeRemoteReservationStartRequest event

                                          try
                                          {

                                              if (OnAuthorizeRemoteReservationStartRequest is not null)
                                                  await Task.WhenAll(OnAuthorizeRemoteReservationStartRequest.GetInvocationList().
                                                                     Cast<OnAuthorizeRemoteReservationStartRequestDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   authorizeRemoteReservationStartRequest!))).
                                                                     ConfigureAwait(false);

                                          }
                                          catch (Exception e)
                                          {
                                              DebugX.LogException(e, nameof(CPOServerAPI) + "." + nameof(OnAuthorizeRemoteReservationStartRequest));
                                          }

                                          #endregion

                                          #region Call async subscribers

                                          var OnAuthorizeRemoteReservationStartLocal = OnAuthorizeRemoteReservationStart;
                                          if (OnAuthorizeRemoteReservationStartLocal is not null)
                                          {

                                              try
                                              {

                                                  authorizeRemoteReservationStartResponse = (await Task.WhenAll(OnAuthorizeRemoteReservationStartLocal.GetInvocationList().
                                                                                                                                                       Cast<OnAuthorizeRemoteReservationStartDelegate>().
                                                                                                                                                       Select(e => e(Timestamp.Now,
                                                                                                                                                                     this,
                                                                                                                                                                     authorizeRemoteReservationStartRequest!))))?.FirstOrDefault();

                                                  Counters.AuthorizeRemoteReservationStart.IncResponses_OK();

                                              }
                                              catch (Exception e)
                                              {

                                                  Counters.AuthorizeRemoteReservationStart.IncResponses_Error();

                                                  authorizeRemoteReservationStartResponse = Acknowledgement<AuthorizeRemoteReservationStartRequest>.DataError(
                                                                                                Request:                   authorizeRemoteReservationStartRequest,
                                                                                                StatusCodeDescription:     e.Message,
                                                                                                StatusCodeAdditionalInfo:  e.StackTrace,
                                                                                                SessionId:                 authorizeRemoteReservationStartRequest?.SessionId,
                                                                                                CPOPartnerSessionId:       authorizeRemoteReservationStartRequest?.CPOPartnerSessionId
                                                                                            );

                                              }

                                          }

                                          if (authorizeRemoteReservationStartResponse is null)
                                          {

                                              Counters.AuthorizeRemoteReservationStart.IncResponses_Error();

                                              authorizeRemoteReservationStartResponse = Acknowledgement<AuthorizeRemoteReservationStartRequest>.SystemError(
                                                                                            authorizeRemoteReservationStartRequest,
                                                                                            "Could not process the received AuthorizeRemoteReservationStart request!"
                                                                                        );

                                          }

                                          #endregion

                                          #region Send OnAuthorizeRemoteReservationStartResponse event

                                          try
                                          {

                                              if (OnAuthorizeRemoteReservationStartResponse is not null)
                                                  await Task.WhenAll(OnAuthorizeRemoteReservationStartResponse.GetInvocationList().
                                                                     Cast<OnAuthorizeRemoteReservationStartResponseDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   authorizeRemoteReservationStartRequest!,
                                                                                   authorizeRemoteReservationStartResponse,
                                                                                   Timestamp.Now - startTime))).
                                                                     ConfigureAwait(false);

                                          }
                                          catch (Exception e)
                                          {
                                              DebugX.LogException(e, nameof(CPOServerAPI) + "." + nameof(OnAuthorizeRemoteReservationStartResponse));
                                          }

                                          #endregion

                                      }
                                      else
                                      {

                                          Counters.AuthorizeRemoteReservationStart.IncRequests_Error();

                                          authorizeRemoteReservationStartResponse = Acknowledgement<AuthorizeRemoteReservationStartRequest>.DataError(
                                                                                        Request:                   authorizeRemoteReservationStartRequest,
                                                                                        StatusCodeDescription:     "We could not handle the given AuthorizeRemoveReservationStart request!",
                                                                                        StatusCodeAdditionalInfo:  errorResponse,
                                                                                        SessionId:                 authorizeRemoteReservationStartRequest?.SessionId,
                                                                                        CPOPartnerSessionId:       authorizeRemoteReservationStartRequest?.CPOPartnerSessionId
                                                                                    );

                                      }

                                  }
                                  catch (Exception e)
                                  {

                                      Counters.AuthorizeRemoteReservationStart.IncResponses_Error();

                                      authorizeRemoteReservationStartResponse = Acknowledgement<AuthorizeRemoteReservationStartRequest>.SystemError(
                                                                                    Request:                   null,
                                                                                    StatusCodeDescription:     e.Message,
                                                                                    StatusCodeAdditionalInfo:  e.StackTrace
                                                                                );
                                  }


                                  return new HTTPResponse.Builder(Request) {
                                             HTTPStatusCode             = HTTPStatusCode.OK,
                                             Server                     = HTTPServer.DefaultServerName,
                                             Date                       = Timestamp.Now,
                                             AccessControlAllowOrigin   = "*",
                                             AccessControlAllowMethods  = [ "POST" ],
                                             AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                             ContentType                = HTTPContentType.Application.JSON_UTF8,
                                             Content                    = authorizeRemoteReservationStartResponse.ToJSON(CustomAcknowledgementSerializer,
                                                                                                                         CustomStatusCodeSerializer).
                                                                                                                  ToString(JSONFormatting).
                                                                                                                  ToUTF8Bytes(),
                                             Connection                 = ConnectionType.Close
                                         }.AsImmutable;

                               }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region POST  ~/api/oicp/charging/v21/providers/{providerId}/authorize-remote-reservation/stop

            // POST /api/oicp/charging/v21/providers/DE*ICE/authorize-remote-reservation/stop HTTP/1.1
            // Accept:          application/json;charset=UTF-8
            // Content-Type:    application/json;charset=UTF-8
            // Content-Length:  250
            // Host:            api.chargeit-mobility.com:3000
            // Connection:      Keep-Alive
            // User-Agent:      Apache-HttpAsyncClient/4.1.4 (Java/1.8.0_265)
            // 
            // {
            //     "SessionID":           "b3be9e19-ca56-4965-80f1-ba835d5069c7",
            //     "CPOPartnerSessionID":  null,
            //     "EMPPartnerSessionID":  null,
            //     "ProviderID":          "DE*ICE",
            //     "EvseID":              "DE*BDO*E*TEST*1",
            //     "Identification": {
            //         "RemoteReservationIdentification": {
            //             "EvcoID":          "DE*ICE*I01000*6"
            //         }
            //     },
            //     "PartnerProductID": null
            // }

            // --------------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3000/api/oicp/charging/v21/providers/{providerId}/authorize-remote-reservation/stop
            // --------------------------------------------------------------------------------------------------------------------------------------------------
            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.POST,
                              URLPathPrefix + "api/oicp/charging/v21/providers/{providerId}/authorize-remote-reservation/stop",
                              HTTPContentType.Application.JSON_UTF8,
                              HTTPRequestLogger:   logAuthorizeRemoteReservationStopHTTPRequest,
                              HTTPResponseLogger:  logAuthorizeRemoteReservationStopHTTPResponse,
                              HTTPDelegate:        async Request => {

                                  var startTime  = Timestamp.Now;
                                  var processId  = Request.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse);

                                  Acknowledgement<AuthorizeRemoteReservationStopRequest>? authorizeRemoteReservationStopResponse = null;

                                  try
                                  {

                                      #region Try to parse ProviderId URL parameter

                                      if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                      {

                                          Counters.AuthorizeRemoteReservationStop.IncRequests_Error();

                                          authorizeRemoteReservationStopResponse = Acknowledgement<AuthorizeRemoteReservationStopRequest>.SystemError(
                                                                                       null,
                                                                                       "The expected 'providerId' URL parameter could not be parsed!"
                                                                                   );

                                      }

                                      #endregion

                                      else if (AuthorizeRemoteReservationStopRequest.TryParse(JObject.Parse(Request.HTTPBody?.ToUTF8String() ?? ""),
                                                                                              providerId,
                                                                                              out var authorizeRemoteReservationStopRequest,
                                                                                              out var errorResponse,
                                                                                              processId,
                                                                                              Request.Timestamp,
                                                                                              Request.EventTrackingId,
                                                                                              Request.Timeout ?? DefaultRequestTimeout,
                                                                                              CustomAuthorizeRemoteReservationStopRequestParser,
                                                                                              Request.CancellationToken))
                                      {

                                          Counters.AuthorizeRemoteReservationStop.IncRequests_OK();

                                          #region Send OnAuthorizeRemoteReservationStopRequest event

                                          try
                                          {

                                              if (OnAuthorizeRemoteReservationStopRequest is not null)
                                                  await Task.WhenAll(OnAuthorizeRemoteReservationStopRequest.GetInvocationList().
                                                                     Cast<OnAuthorizeRemoteReservationStopRequestDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   authorizeRemoteReservationStopRequest!))).
                                                                     ConfigureAwait(false);

                                          }
                                          catch (Exception e)
                                          {
                                              DebugX.LogException(e, nameof(CPOServerAPI) + "." + nameof(OnAuthorizeRemoteReservationStopRequest));
                                          }

                                          #endregion

                                          #region Call async subscribers

                                          var OnAuthorizeRemoteReservationStopLocal = OnAuthorizeRemoteReservationStop;
                                          if (OnAuthorizeRemoteReservationStopLocal is not null)
                                          {

                                              try
                                              {

                                                  authorizeRemoteReservationStopResponse = (await Task.WhenAll(OnAuthorizeRemoteReservationStopLocal.GetInvocationList().
                                                                                                                                                     Cast<OnAuthorizeRemoteReservationStopDelegate>().
                                                                                                                                                     Select(e => e(Timestamp.Now,
                                                                                                                                                                   this,
                                                                                                                                                                   authorizeRemoteReservationStopRequest!))))?.FirstOrDefault();

                                                  Counters.AuthorizeRemoteReservationStop.IncResponses_OK();

                                              }
                                              catch (Exception e)
                                              {

                                                  Counters.AuthorizeRemoteReservationStop.IncResponses_Error();

                                                  authorizeRemoteReservationStopResponse = Acknowledgement<AuthorizeRemoteReservationStopRequest>.DataError(
                                                                                               Request:                   authorizeRemoteReservationStopRequest,
                                                                                               StatusCodeDescription:     e.Message,
                                                                                               StatusCodeAdditionalInfo:  e.StackTrace,
                                                                                               SessionId:                 authorizeRemoteReservationStopRequest?.SessionId,
                                                                                               CPOPartnerSessionId:       authorizeRemoteReservationStopRequest?.CPOPartnerSessionId
                                                                                           );

                                              }

                                          }

                                          if (authorizeRemoteReservationStopResponse is null)
                                          {

                                              Counters.AuthorizeRemoteReservationStop.IncResponses_Error();

                                              authorizeRemoteReservationStopResponse = Acknowledgement<AuthorizeRemoteReservationStopRequest>.SystemError(
                                                                                           authorizeRemoteReservationStopRequest,
                                                                                           "Could not process the received AuthorizeRemoteReservationStop request!"
                                                                                       );

                                          }

                                          #endregion

                                          #region Send OnAuthorizeRemoteReservationStopResponse event

                                          try
                                          {

                                              if (OnAuthorizeRemoteReservationStopResponse is not null)
                                                  await Task.WhenAll(OnAuthorizeRemoteReservationStopResponse.GetInvocationList().
                                                                     Cast<OnAuthorizeRemoteReservationStopResponseDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   authorizeRemoteReservationStopRequest!,
                                                                                   authorizeRemoteReservationStopResponse,
                                                                                   Timestamp.Now - startTime))).
                                                                     ConfigureAwait(false);

                                          }
                                          catch (Exception e)
                                          {
                                              DebugX.LogException(e, nameof(CPOServerAPI) + "." + nameof(OnAuthorizeRemoteReservationStopResponse));
                                          }

                                          #endregion

                                      }
                                      else
                                      {

                                          Counters.AuthorizeRemoteReservationStop.IncRequests_Error();

                                          authorizeRemoteReservationStopResponse = Acknowledgement<AuthorizeRemoteReservationStopRequest>.DataError(
                                                                                       Request:                   authorizeRemoteReservationStopRequest,
                                                                                       StatusCodeDescription:     "We could not handle the given AuthorizeRemoveReservationStop request!",
                                                                                       StatusCodeAdditionalInfo:  errorResponse,
                                                                                       SessionId:                 authorizeRemoteReservationStopRequest?.SessionId,
                                                                                       CPOPartnerSessionId:       authorizeRemoteReservationStopRequest?.CPOPartnerSessionId
                                                                                   );

                                      }

                                  }
                                  catch (Exception e)
                                  {

                                      Counters.AuthorizeRemoteReservationStop.IncResponses_Error();

                                      authorizeRemoteReservationStopResponse = Acknowledgement<AuthorizeRemoteReservationStopRequest>.SystemError(
                                                                                   Request:                   null,
                                                                                   StatusCodeDescription:     e.Message,
                                                                                   StatusCodeAdditionalInfo:  e.StackTrace
                                                                               );

                                  }


                                  return new HTTPResponse.Builder(Request) {
                                             HTTPStatusCode             = HTTPStatusCode.OK,
                                             Server                     = HTTPServer.DefaultServerName,
                                             Date                       = Timestamp.Now,
                                             AccessControlAllowOrigin   = "*",
                                             AccessControlAllowMethods  = [ "POST" ],
                                             AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                             ContentType                = HTTPContentType.Application.JSON_UTF8,
                                             Content                    = authorizeRemoteReservationStopResponse.ToJSON(CustomAcknowledgementSerializer,
                                                                                                                        CustomStatusCodeSerializer).
                                                                                                                 ToString(JSONFormatting).
                                                                                                                 ToUTF8Bytes(),
                                             Connection                 = ConnectionType.Close
                                         }.AsImmutable;

                               }, AllowReplacement: URLReplacement.Allow);

            #endregion


            #region POST  ~/api/oicp/charging/v21/providers/{providerId}/authorize-remote/start

            // POST /api/oicp/charging/v21/providers/DE*ICE/authorize-remote/start HTTP/1.1
            // Accept:          application/json;charset=UTF-8
            // Content-Type:    application/json;charset=UTF-8
            // Content-Length:  250
            // Host:            api.chargeit-mobility.com:3000
            // Connection:      Keep-Alive
            // User-Agent:      Apache-HttpAsyncClient/4.1.4 (Java/1.8.0_265)
            // 
            // {
            //     "SessionID":           "b3be9e19-ca56-4965-80f1-ba835d5069c7",
            //     "CPOPartnerSessionID":  null,
            //     "EMPPartnerSessionID":  null,
            //     "ProviderID":          "DE*ICE",
            //     "EvseID":              "DE*BDO*E*TEST*1",
            //     "Identification": {
            //         "RemoteIdentification": {
            //             "EvcoID":          "DE*ICE*I01000*6"
            //         }
            //     },
            //     "PartnerProductID": null
            // }

            // ---------------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3000/api/oicp/charging/v21/providers/{providerId}/authorize-remote/start
            // ---------------------------------------------------------------------------------------------------------------------------------------------------
            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.POST,
                              URLPathPrefix + "api/oicp/charging/v21/providers/{providerId}/authorize-remote/start",
                              HTTPContentType.Application.JSON_UTF8,
                              HTTPRequestLogger:   logAuthorizeRemoteStartHTTPRequest,
                              HTTPResponseLogger:  logAuthorizeRemoteStartHTTPResponse,
                              HTTPDelegate:        async Request => {

                                  var startTime  = Timestamp.Now;
                                  var processId  = Request.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse);

                                  Acknowledgement<AuthorizeRemoteStartRequest>? authorizeRemoteStartResponse = null;

                                  try
                                  {

                                      #region Try to parse ProviderId URL parameter

                                      if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                      {

                                          Counters.AuthorizeRemoteStart.IncRequests_Error();

                                          authorizeRemoteStartResponse = Acknowledgement<AuthorizeRemoteStartRequest>.SystemError(
                                                                             null,
                                                                             "The expected 'providerId' URL parameter could not be parsed!"
                                                                         );

                                      }

                                      #endregion

                                      else if (AuthorizeRemoteStartRequest.TryParse(JObject.Parse(Request.HTTPBody?.ToUTF8String() ?? ""),
                                                                                    providerId,
                                                                                    out var authorizeRemoteStartRequest,
                                                                                    out var errorResponse,
                                                                                    processId,
                                                                                    Request.Timestamp,
                                                                                    Request.EventTrackingId,
                                                                                    Request.Timeout ?? DefaultRequestTimeout,
                                                                                    CustomAuthorizeRemoteStartRequestParser,
                                                                                    Request.CancellationToken))
                                      {

                                          Counters.AuthorizeRemoteStart.IncRequests_OK();

                                          #region Send OnAuthorizeRemoteStartRequest event

                                          try
                                          {

                                              if (OnAuthorizeRemoteStartRequest is not null)
                                                  await Task.WhenAll(OnAuthorizeRemoteStartRequest.GetInvocationList().
                                                                     Cast<OnAuthorizeRemoteStartRequestDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   authorizeRemoteStartRequest!))).
                                                                     ConfigureAwait(false);

                                          }
                                          catch (Exception e)
                                          {
                                              DebugX.LogException(e, nameof(CPOServerAPI) + "." + nameof(OnAuthorizeRemoteStartRequest));
                                          }

                                          #endregion

                                          #region Call async subscribers

                                          var OnAuthorizeRemoteStartLocal = OnAuthorizeRemoteStart;
                                          if (OnAuthorizeRemoteStartLocal is not null)
                                          {

                                              try
                                              {

                                                  authorizeRemoteStartResponse = (await Task.WhenAll(OnAuthorizeRemoteStartLocal.GetInvocationList().
                                                                                                                    Cast<OnAuthorizeRemoteStartDelegate>().
                                                                                                                    Select(e => e(Timestamp.Now,
                                                                                                                                  this,
                                                                                                                                  authorizeRemoteStartRequest!))))?.FirstOrDefault();

                                                  Counters.AuthorizeRemoteStart.IncResponses_OK();

                                              }
                                              catch (Exception e)
                                              {

                                                  Counters.AuthorizeRemoteStart.IncResponses_Error();

                                                  authorizeRemoteStartResponse = Acknowledgement<AuthorizeRemoteStartRequest>.DataError(
                                                                                     Request:                   authorizeRemoteStartRequest,
                                                                                     StatusCodeDescription:     e.Message,
                                                                                     StatusCodeAdditionalInfo:  e.StackTrace,
                                                                                     SessionId:                 authorizeRemoteStartRequest?.SessionId,
                                                                                     CPOPartnerSessionId:       authorizeRemoteStartRequest?.CPOPartnerSessionId
                                                                                 );

                                              }

                                          }

                                          if (authorizeRemoteStartResponse is null)
                                          {

                                              Counters.AuthorizeRemoteStart.IncResponses_Error();

                                              authorizeRemoteStartResponse = Acknowledgement<AuthorizeRemoteStartRequest>.SystemError(
                                                                                 authorizeRemoteStartRequest,
                                                                                 "Could not process the received AuthorizeRemoteStart request!"
                                                                             );

                                          }

                                          #endregion

                                          #region Send OnAuthorizeRemoteStartResponse event

                                          try
                                          {

                                              if (OnAuthorizeRemoteStartResponse is not null)
                                                  await Task.WhenAll(OnAuthorizeRemoteStartResponse.GetInvocationList().
                                                                     Cast<OnAuthorizeRemoteStartResponseDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   authorizeRemoteStartRequest!,
                                                                                   authorizeRemoteStartResponse,
                                                                                   Timestamp.Now - startTime))).
                                                                     ConfigureAwait(false);

                                          }
                                          catch (Exception e)
                                          {
                                              DebugX.LogException(e, nameof(CPOServerAPI) + "." + nameof(OnAuthorizeRemoteStartResponse));
                                          }

                                          #endregion

                                      }
                                      else
                                      {

                                          Counters.AuthorizeRemoteStart.IncRequests_Error();

                                          authorizeRemoteStartResponse = Acknowledgement<AuthorizeRemoteStartRequest>.DataError(
                                                                             Request:                   authorizeRemoteStartRequest,
                                                                             StatusCodeDescription:     "We could not handle the given AuthorizeRemoveStart request!",
                                                                             StatusCodeAdditionalInfo:  errorResponse,
                                                                             SessionId:                 authorizeRemoteStartRequest?.SessionId,
                                                                             CPOPartnerSessionId:       authorizeRemoteStartRequest?.CPOPartnerSessionId
                                                                         );

                                      }

                                  }
                                  catch (Exception e)
                                  {

                                      Counters.AuthorizeRemoteStart.IncResponses_Error();

                                      authorizeRemoteStartResponse = Acknowledgement<AuthorizeRemoteStartRequest>.SystemError(
                                                                         Request:                   null,
                                                                         StatusCodeDescription:     e.Message,
                                                                         StatusCodeAdditionalInfo:  e.StackTrace
                                                                     );

                                  }


                                  return new HTTPResponse.Builder(Request) {
                                             HTTPStatusCode             = HTTPStatusCode.OK,
                                             Server                     = HTTPServer.DefaultServerName,
                                             Date                       = Timestamp.Now,
                                             AccessControlAllowOrigin   = "*",
                                             AccessControlAllowMethods  = [ "POST" ],
                                             AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                             ContentType                = HTTPContentType.Application.JSON_UTF8,
                                             Content                    = authorizeRemoteStartResponse.ToJSON(CustomAcknowledgementSerializer,
                                                                                                              CustomStatusCodeSerializer).
                                                                                                       ToString(JSONFormatting).
                                                                                                       ToUTF8Bytes(),
                                             Connection                 = ConnectionType.Close
                                         }.AsImmutable;

                               }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region POST  ~/api/oicp/charging/v21/providers/{providerId}/authorize-remote/stop

            // POST /api/oicp/charging/v21/providers/DE*ICE/authorize-remote/stop HTTP/1.1
            // Accept:          application/json;charset=UTF-8
            // Content-Type:    application/json;charset=UTF-8
            // Content-Length:  250
            // Host:            api.chargeit-mobility.com:3000
            // Connection:      Keep-Alive
            // User-Agent:      Apache-HttpAsyncClient/4.1.4 (Java/1.8.0_265)
            // 
            // {
            //     "SessionID":           "b3be9e19-ca56-4965-80f1-ba835d5069c7",
            //     "CPOPartnerSessionID":  null,
            //     "EMPPartnerSessionID":  null,
            //     "ProviderID":          "DE*ICE",
            //     "EvseID":              "DE*BDO*E*TEST*1",
            //     "Identification": {
            //         "RemoteIdentification": {
            //             "EvcoID":          "DE*ICE*I01000*6"
            //         }
            //     },
            //     "PartnerProductID": null
            // }

            // --------------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3000/api/oicp/charging/v21/providers/{providerId}/authorize-remote/stop
            // --------------------------------------------------------------------------------------------------------------------------------------------------
            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.POST,
                              URLPathPrefix + "api/oicp/charging/v21/providers/{providerId}/authorize-remote/stop",
                              HTTPContentType.Application.JSON_UTF8,
                              HTTPRequestLogger:   logAuthorizeRemoteStopHTTPRequest,
                              HTTPResponseLogger:  logAuthorizeRemoteStopHTTPResponse,
                              HTTPDelegate:        async Request => {

                                  var startTime  = Timestamp.Now;
                                  var processId  = Request.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse);

                                  Acknowledgement<AuthorizeRemoteStopRequest>? authorizeRemoteStopResponse = null;

                                  try
                                  {

                                      #region Try to parse ProviderId URL parameter

                                      if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                      {

                                          Counters.AuthorizeRemoteStop.IncRequests_Error();

                                          authorizeRemoteStopResponse = Acknowledgement<AuthorizeRemoteStopRequest>.SystemError(
                                                                            null,
                                                                            "The expected 'providerId' URL parameter could not be parsed!"
                                                                        );

                                      }

                                      #endregion

                                      else if (AuthorizeRemoteStopRequest.TryParse(JObject.Parse(Request.HTTPBody?.ToUTF8String() ?? ""),
                                                                                   providerId,
                                                                                   out var authorizeRemoteStopRequest,
                                                                                   out var errorResponse,
                                                                                   processId,
                                                                                   Request.Timestamp,
                                                                                   Request.EventTrackingId,
                                                                                   Request.Timeout ?? DefaultRequestTimeout,
                                                                                   CustomAuthorizeRemoteStopRequestParser,
                                                                                   Request.CancellationToken))
                                      {

                                          Counters.AuthorizeRemoteStop.IncRequests_OK();

                                          #region Send OnAuthorizeRemoteStopRequest event

                                          try
                                          {

                                              if (OnAuthorizeRemoteStopRequest is not null)
                                                  await Task.WhenAll(OnAuthorizeRemoteStopRequest.GetInvocationList().
                                                                     Cast<OnAuthorizeRemoteStopRequestDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   authorizeRemoteStopRequest!))).
                                                                     ConfigureAwait(false);

                                          }
                                          catch (Exception e)
                                          {
                                              DebugX.LogException(e, nameof(CPOServerAPI) + "." + nameof(OnAuthorizeRemoteStopRequest));
                                          }

                                          #endregion

                                          #region Call async subscribers

                                          var OnAuthorizeRemoteStopLocal = OnAuthorizeRemoteStop;
                                          if (OnAuthorizeRemoteStopLocal is not null)
                                          {

                                              try
                                              {

                                                  authorizeRemoteStopResponse = (await Task.WhenAll(OnAuthorizeRemoteStopLocal.GetInvocationList().
                                                                                                                               Cast<OnAuthorizeRemoteStopDelegate>().
                                                                                                                               Select(e => e(Timestamp.Now,
                                                                                                                                             this,
                                                                                                                                             authorizeRemoteStopRequest!))))?.FirstOrDefault();

                                                  Counters.AuthorizeRemoteStop.IncResponses_OK();

                                              }
                                              catch (Exception e)
                                              {

                                                  Counters.AuthorizeRemoteStop.IncResponses_Error();

                                                  authorizeRemoteStopResponse = Acknowledgement<AuthorizeRemoteStopRequest>.DataError(
                                                                                    Request:                   authorizeRemoteStopRequest,
                                                                                    StatusCodeDescription:     e.Message,
                                                                                    StatusCodeAdditionalInfo:  e.StackTrace,
                                                                                    SessionId:                 authorizeRemoteStopRequest?.SessionId,
                                                                                    CPOPartnerSessionId:       authorizeRemoteStopRequest?.CPOPartnerSessionId
                                                                                );

                                              }

                                          }

                                          if (authorizeRemoteStopResponse is null)
                                          {

                                              Counters.AuthorizeRemoteStop.IncResponses_Error();

                                              authorizeRemoteStopResponse = Acknowledgement<AuthorizeRemoteStopRequest>.SystemError(
                                                                                authorizeRemoteStopRequest,
                                                                                "Could not process the received AuthorizeRemoteStop request!"
                                                                            );

                                          }

                                          #endregion

                                          #region Send OnAuthorizeRemoteStopResponse event

                                          try
                                          {

                                              if (OnAuthorizeRemoteStopResponse is not null)
                                                  await Task.WhenAll(OnAuthorizeRemoteStopResponse.GetInvocationList().
                                                                     Cast<OnAuthorizeRemoteStopResponseDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   authorizeRemoteStopRequest!,
                                                                                   authorizeRemoteStopResponse,
                                                                                   Timestamp.Now - startTime))).
                                                                     ConfigureAwait(false);

                                          }
                                          catch (Exception e)
                                          {
                                              DebugX.LogException(e, nameof(CPOServerAPI) + "." + nameof(OnAuthorizeRemoteStopResponse));
                                          }

                                          #endregion

                                      }
                                      else
                                      {

                                          Counters.AuthorizeRemoteStop.IncRequests_Error();

                                          authorizeRemoteStopResponse = Acknowledgement<AuthorizeRemoteStopRequest>.DataError(
                                                                            Request:                   authorizeRemoteStopRequest,
                                                                            StatusCodeDescription:     "We could not handle the given AuthorizeRemoteStop request!",
                                                                            StatusCodeAdditionalInfo:  errorResponse,
                                                                            SessionId:                 authorizeRemoteStopRequest?.SessionId,
                                                                            CPOPartnerSessionId:       authorizeRemoteStopRequest?.CPOPartnerSessionId
                                                                        );

                                      }

                                  }
                                  catch (Exception e)
                                  {

                                      Counters.AuthorizeRemoteStop.IncResponses_Error();

                                      authorizeRemoteStopResponse = Acknowledgement<AuthorizeRemoteStopRequest>.SystemError(
                                                                        Request:                   null,
                                                                        StatusCodeDescription:     e.Message,
                                                                        StatusCodeAdditionalInfo:  e.StackTrace
                                                                    );

                                  }


                                  return new HTTPResponse.Builder(Request) {
                                             HTTPStatusCode             = HTTPStatusCode.OK,
                                             Server                     = HTTPServer.DefaultServerName,
                                             Date                       = Timestamp.Now,
                                             AccessControlAllowOrigin   = "*",
                                             AccessControlAllowMethods  = [ "POST" ],
                                             AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                             ContentType                = HTTPContentType.Application.JSON_UTF8,
                                             Content                    = authorizeRemoteStopResponse.ToJSON(CustomAcknowledgementSerializer,
                                                                                                             CustomStatusCodeSerializer).
                                                                                                      ToString(JSONFormatting).
                                                                                                      ToUTF8Bytes(),
                                             Connection                 = ConnectionType.Close
                                         }.AsImmutable;

                               }, AllowReplacement: URLReplacement.Allow);

            #endregion

        }

        #endregion


        #region Dispose()

        /// <summary>
        /// Dispose this object.
        /// </summary>
        public override void Dispose()
        { }

        #endregion

    }

}
