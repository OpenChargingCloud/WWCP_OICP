/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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
using System.Linq;
using System.Threading;
using System.Net.Security;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Authentication;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Sockets;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    /// <summary>
    /// The CPO HTTP Server API.
    /// </summary>
    public partial class CPOServerAPI : HTTPAPI
    {

        #region (class) Counters

        public class Counters
        {

            public CounterValues  AuthorizeRemoteReservationStart    { get; }
            public CounterValues  AuthorizeRemoteReservationStop     { get; }
            public CounterValues  AuthorizeRemoteStart               { get; }
            public CounterValues  AuthorizeRemoteStop                { get; }

            public Counters(CounterValues? AuthorizeRemoteReservationStart   = null,
                            CounterValues? AuthorizeRemoteReservationStop    = null,
                            CounterValues? AuthorizeRemoteStart              = null,
                            CounterValues? AuthorizeRemoteStop               = null)
            {

                this.AuthorizeRemoteReservationStart  = AuthorizeRemoteReservationStart ?? new CounterValues();
                this.AuthorizeRemoteReservationStop   = AuthorizeRemoteReservationStop  ?? new CounterValues();
                this.AuthorizeRemoteStart             = AuthorizeRemoteStart            ?? new CounterValues();
                this.AuthorizeRemoteStop              = AuthorizeRemoteStop             ?? new CounterValues();

            }

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
        public new const String  DefaultHTTPServerName   = "GraphDefined OICP " + Version.Number + " CPO HTTP API";

        /// <summary>
        /// The default HTTP service name.
        /// </summary>
        public new const String  DefaultHTTPServiceName  = "GraphDefined OICP " + Version.Number + " CPO HTTP API";

        /// <summary>
        /// The default logging context.
        /// </summary>
        public     const String  DefaultLoggingContext   = "CPOServerAPI";

        #endregion

        #region Properties

        public Counters                                                             Counter                                               { get; }

        // Custom JSON parsers

        public CustomJObjectParserDelegate<AuthorizeRemoteReservationStartRequest>  CustomAuthorizeRemoteReservationStartRequestParser    { get; set; }

        public CustomJObjectParserDelegate<AuthorizeRemoteReservationStopRequest>   CustomAuthorizeRemoteReservationStopRequestParser     { get; set; }


        public CustomJObjectParserDelegate<AuthorizeRemoteStartRequest>             CustomAuthorizeRemoteStartRequestParser               { get; set; }

        public CustomJObjectParserDelegate<AuthorizeRemoteStopRequest>              CustomAuthorizeRemoteStopRequestParser                { get; set; }


        // Custom JSON serializers
        public CustomJObjectSerializerDelegate<Acknowledgement>                     CustomAcknowledgementSerializer                       { get; set; }

        public CustomJObjectSerializerDelegate<StatusCode>                          CustomStatusCodeSerializer                            { get; set; }


        public Newtonsoft.Json.Formatting                                           JSONFormatting                                        { get; set; }

        #endregion

        #region Events

        #region (protected internal) OnAuthorizeRemoteReservationStartHTTPRequest

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteReservationStart HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnAuthorizeRemoteReservationStartHTTPRequest = new HTTPRequestLogEvent();

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteReservationStart HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Server HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logAuthorizeRemoteReservationStartHTTPRequest(DateTime     Timestamp,
                                                                              HTTPAPI      API,
                                                                              HTTPRequest  Request)

            => OnAuthorizeRemoteReservationStartHTTPRequest?.WhenAll(Timestamp,
                                                                     API ?? this,
                                                                     Request);

        #endregion

        public event OnAuthorizeRemoteReservationStartRequestDelegate   OnAuthorizeRemoteReservationStartRequest;
        public event OnAuthorizeRemoteReservationStartDelegate          OnAuthorizeRemoteReservationStart;
        public event OnAuthorizeRemoteReservationStartResponseDelegate  OnAuthorizeRemoteReservationStartResponse;

        #region (protected internal) OnAuthorizeRemoteReservationStartHTTPResponse

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteReservationStart HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnAuthorizeRemoteReservationStartHTTPResponse = new HTTPResponseLogEvent();

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

            => OnAuthorizeRemoteReservationStartHTTPResponse?.WhenAll(Timestamp,
                                                                      API ?? this,
                                                                      Request,
                                                                      Response);

        #endregion


        #region (protected internal) OnAuthorizeRemoteReservationStopHTTPRequest

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteReservationStop HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnAuthorizeRemoteReservationStopHTTPRequest = new HTTPRequestLogEvent();

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteReservationStop HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Server HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logAuthorizeRemoteReservationStopHTTPRequest(DateTime      Timestamp,
                                                                             HTTPAPI       API,
                                                                             HTTPRequest   Request)

            => OnAuthorizeRemoteReservationStopHTTPRequest?.WhenAll(Timestamp,
                                                                    API ?? this,
                                                                    Request);

        #endregion

        public event OnAuthorizeRemoteReservationStopRequestDelegate   OnAuthorizeRemoteReservationStopRequest;
        public event OnAuthorizeRemoteReservationStopDelegate          OnAuthorizeRemoteReservationStop;
        public event OnAuthorizeRemoteReservationStopResponseDelegate  OnAuthorizeRemoteReservationStopResponse;

        #region (protected internal) OnAuthorizeRemoteReservationStopHTTPResponse

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteReservationStop HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnAuthorizeRemoteReservationStopHTTPResponse = new HTTPResponseLogEvent();

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

            => OnAuthorizeRemoteReservationStopHTTPResponse?.WhenAll(Timestamp,
                                                                     API ?? this,
                                                                     Request,
                                                                     Response);

        #endregion



        #region (protected internal) OnAuthorizeRemoteStartHTTPRequest

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteStart HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnAuthorizeRemoteStartHTTPRequest = new HTTPRequestLogEvent();

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteStart HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Server HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logAuthorizeRemoteStartHTTPRequest(DateTime     Timestamp,
                                                                   HTTPAPI      API,
                                                                   HTTPRequest  Request)

            => OnAuthorizeRemoteStartHTTPRequest?.WhenAll(Timestamp,
                                                          API ?? this,
                                                          Request);

        #endregion

        public event OnAuthorizeRemoteStartRequestDelegate   OnAuthorizeRemoteStartRequest;
        public event OnAuthorizeRemoteStartDelegate          OnAuthorizeRemoteStart;
        public event OnAuthorizeRemoteStartResponseDelegate  OnAuthorizeRemoteStartResponse;

        #region (protected internal) OnAuthorizeRemoteStartHTTPResponse

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteStart HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnAuthorizeRemoteStartHTTPResponse = new HTTPResponseLogEvent();

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

            => OnAuthorizeRemoteStartHTTPResponse?.WhenAll(Timestamp,
                                                           API ?? this,
                                                           Request,
                                                           Response);

        #endregion


        #region (protected internal) OnAuthorizeRemoteStopHTTPRequest

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteStop HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnAuthorizeRemoteStopHTTPRequest = new HTTPRequestLogEvent();

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteStop HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Server HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logAuthorizeRemoteStopHTTPRequest(DateTime      Timestamp,
                                                                   HTTPAPI       API,
                                                                   HTTPRequest   Request)

            => OnAuthorizeRemoteStopHTTPRequest?.WhenAll(Timestamp,
                                                          API ?? this,
                                                          Request);

        #endregion

        public event OnAuthorizeRemoteStopRequestDelegate   OnAuthorizeRemoteStopRequest;
        public event OnAuthorizeRemoteStopDelegate          OnAuthorizeRemoteStop;
        public event OnAuthorizeRemoteStopResponseDelegate  OnAuthorizeRemoteStopResponse;

        #region (protected internal) OnAuthorizeRemoteStopHTTPResponse

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteStop HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnAuthorizeRemoteStopHTTPResponse = new HTTPResponseLogEvent();

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

            => OnAuthorizeRemoteStopHTTPResponse?.WhenAll(Timestamp,
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
        /// <param name="ServerCertificateSelector">An optional delegate to select a SSL/TLS server certificate.</param>
        /// <param name="ClientCertificateValidator">An optional delegate to verify the SSL/TLS client certificate used for authentication.</param>
        /// <param name="ClientCertificateSelector">An optional delegate to select the SSL/TLS client certificate used for authentication.</param>
        /// <param name="AllowedTLSProtocols">The SSL/TLS protocol(s) allowed for this connection.</param>
        /// 
        /// <param name="ServerThreadName">The optional name of the TCP server thread.</param>
        /// <param name="ServerThreadPriority">The optional priority of the TCP server thread.</param>
        /// <param name="ServerThreadIsBackground">Whether the TCP server thread is a background thread or not.</param>
        /// <param name="ConnectionIdBuilder">An optional delegate to build a connection identification based on IP socket information.</param>
        /// <param name="ConnectionThreadsNameBuilder">An optional delegate to set the name of the TCP connection threads.</param>
        /// <param name="ConnectionThreadsPriorityBuilder">An optional delegate to set the priority of the TCP connection threads.</param>
        /// <param name="ConnectionThreadsAreBackground">Whether the TCP connection threads are background threads or not (default: yes).</param>
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
        /// <param name="Autostart">Whether to start the API automatically.</param>
        public CPOServerAPI(HTTPHostname?                        HTTPHostname                       = null,
                            String                               ExternalDNSName                    = null,
                            IPPort?                              HTTPServerPort                     = null,
                            HTTPPath?                            BasePath                           = null,
                            String                               HTTPServerName                     = DefaultHTTPServerName,

                            HTTPPath?                            URLPathPrefix                      = null,
                            String                               HTTPServiceName                    = DefaultHTTPServiceName,
                            JObject                              APIVersionHashes                   = null,

                            ServerCertificateSelectorDelegate    ServerCertificateSelector          = null,
                            LocalCertificateSelectionCallback    ClientCertificateSelector          = null,
                            RemoteCertificateValidationCallback  ClientCertificateValidator         = null,
                            SslProtocols                         AllowedTLSProtocols                = SslProtocols.Tls12 | SslProtocols.Tls13,

                            String                               ServerThreadName                   = null,
                            ThreadPriority?                      ServerThreadPriority               = null,
                            Boolean?                             ServerThreadIsBackground           = null,
                            ConnectionIdBuilder                  ConnectionIdBuilder                = null,
                            ConnectionThreadsNameBuilder         ConnectionThreadsNameBuilder       = null,
                            ConnectionThreadsPriorityBuilder     ConnectionThreadsPriorityBuilder   = null,
                            Boolean?                             ConnectionThreadsAreBackground     = null,
                            TimeSpan?                            ConnectionTimeout                  = null,
                            UInt32?                              MaxClientConnections               = null,

                            Boolean?                             DisableMaintenanceTasks            = false,
                            TimeSpan?                            MaintenanceInitialDelay            = null,
                            TimeSpan?                            MaintenanceEvery                   = null,

                            Boolean?                             DisableWardenTasks                 = false,
                            TimeSpan?                            WardenInitialDelay                 = null,
                            TimeSpan?                            WardenCheckEvery                   = null,

                            Boolean?                             IsDevelopment                      = null,
                            IEnumerable<String>                  DevelopmentServers                 = null,
                            Boolean                              DisableLogging                     = false,
                            String                               LoggingPath                        = DefaultHTTPAPI_LoggingPath,
                            String                               LoggingContext                     = DefaultLoggingContext,
                            String                               LogfileName                        = DefaultHTTPAPI_LogfileName,
                            LogfileCreatorDelegate               LogfileCreator                     = null,
                            DNSClient                            DNSClient                          = null,
                            Boolean                              Autostart                          = false)

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
                   ClientCertificateSelector,
                   AllowedTLSProtocols,

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
                   false) //Autostart)

        {

            RegisterURLTemplates();

            this.Counter     = new Counters();

            this.HTTPLogger  = DisableLogging == false
                                   ? new Logger(this,
                                                LoggingPath,
                                                LoggingContext ?? DefaultLoggingContext,
                                                LogfileCreator)
                                   : null;

            if (Autostart)
                Start();

        }

        #endregion


        #region (private) RegisterURLTemplates()

        private void RegisterURLTemplates()
        {

            #region / (HTTPRoot)

            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.GET,
                                         new HTTPPath[] {
                                             URLPathPrefix + "/",
                                             URLPathPrefix + "/{FileName}"
                                         },
                                         HTTPDelegate: Request => {
                                             return Task.FromResult(
                                                 new HTTPResponse.Builder(Request) {
                                                     HTTPStatusCode  = HTTPStatusCode.OK,
                                                     Server          = HTTPServer.DefaultServerName,
                                                     Date            = Timestamp.Now,
                                                     ContentType     = HTTPContentType.TEXT_UTF8,
                                                     Content         = "This is an OICP v2.3 HTTP/JSON endpoint!".ToUTF8Bytes(),
                                                     CacheControl    = "public, max-age=300",
                                                     Connection      = "close"
                                                 }.AsImmutable);
                                         });

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
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "api/oicp/charging/v21/providers/{providerId}/authorize-remote-reservation/start",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logAuthorizeRemoteReservationStartHTTPRequest,
                                         HTTPResponseLogger:  logAuthorizeRemoteReservationStartHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime = Timestamp.Now;
                                             Acknowledgement<AuthorizeRemoteReservationStartRequest> acknowledgement = null;

                                             try
                                             {

                                                 #region Try to parse ProviderId URL parameter

                                                 if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(Request.ParsedURLParameters[0], out Provider_Id providerId))
                                                     acknowledgement = Acknowledgement<AuthorizeRemoteReservationStartRequest>.SystemError(
                                                                           null,
                                                                           "The expected 'providerId' URL parameter could not be parsed!"
                                                                       );

                                                 #endregion

                                                 else if (AuthorizeRemoteReservationStartRequest.TryParse(Request.HTTPBody?.ToUTF8String(),
                                                                                                          providerId,
                                                                                                          Request.Timeout ?? DefaultRequestTimeout,
                                                                                                          out AuthorizeRemoteReservationStartRequest  authorizeRemoteReservationStartRequest,
                                                                                                          out String                                  errorResponse,
                                                                                                          Request.Timestamp,
                                                                                                          Request.EventTrackingId,
                                                                                                          CustomAuthorizeRemoteReservationStartRequestParser))
                                                 {

                                                     Counter.AuthorizeRemoteReservationStart.IncRequests();

                                                     #region Send OnAuthorizeRemoteReservationStartRequest event

                                                     try
                                                     {

                                                         if (OnAuthorizeRemoteReservationStartRequest != null)
                                                             await Task.WhenAll(OnAuthorizeRemoteReservationStartRequest.GetInvocationList().
                                                                                Cast<OnAuthorizeRemoteReservationStartRequestDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              authorizeRemoteReservationStartRequest))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(CPOServerAPI) + "." + nameof(OnAuthorizeRemoteReservationStartRequest));
                                                     }

                                                     #endregion

                                                     #region Call async subscribers

                                                     var OnAuthorizeRemoteReservationStartLocal = OnAuthorizeRemoteReservationStart;
                                                     if (OnAuthorizeRemoteReservationStartLocal != null)
                                                     {

                                                         try
                                                         {

                                                             acknowledgement = (await Task.WhenAll(OnAuthorizeRemoteReservationStartLocal.GetInvocationList().
                                                                                                                                          Cast<OnAuthorizeRemoteReservationStartDelegate>().
                                                                                                                                          Select(e => e(Timestamp.Now,
                                                                                                                                                        this,
                                                                                                                                                        authorizeRemoteReservationStartRequest))))?.FirstOrDefault();

                                                         }
                                                         catch (Exception e)
                                                         {
                                                             acknowledgement = Acknowledgement<AuthorizeRemoteReservationStartRequest>.DataError(
                                                                                   Request:                   authorizeRemoteReservationStartRequest,
                                                                                   StatusCodeDescription:     e.Message,
                                                                                   StatusCodeAdditionalInfo:  e.StackTrace,
                                                                                   SessionId:                 authorizeRemoteReservationStartRequest.SessionId,
                                                                                   CPOPartnerSessionId:       authorizeRemoteReservationStartRequest.CPOPartnerSessionId
                                                                               );
                                                         }

                                                         if (acknowledgement == null)
                                                             acknowledgement = Acknowledgement<AuthorizeRemoteReservationStartRequest>.SystemError(
                                                                                   authorizeRemoteReservationStartRequest,
                                                                                   "Could not process the received AuthorizeRemoteReservationStart request!"
                                                                               );

                                                     }

                                                     #endregion

                                                     #region Send OnAuthorizeRemoteReservationStartResponse event

                                                     try
                                                     {

                                                         if (OnAuthorizeRemoteReservationStartResponse != null)
                                                             await Task.WhenAll(OnAuthorizeRemoteReservationStartResponse.GetInvocationList().
                                                                                Cast<OnAuthorizeRemoteReservationStartResponseDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              acknowledgement,
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
                                                     acknowledgement = Acknowledgement<AuthorizeRemoteReservationStartRequest>.DataError(
                                                                           Request:                   authorizeRemoteReservationStartRequest,
                                                                           StatusCodeDescription:     "We could not handle the given AuthorizeRemoveReservationStart request!",
                                                                           StatusCodeAdditionalInfo:  errorResponse,
                                                                           SessionId:                 authorizeRemoteReservationStartRequest.SessionId,
                                                                           CPOPartnerSessionId:       authorizeRemoteReservationStartRequest.CPOPartnerSessionId
                                                                       );

                                             }
                                             catch (Exception e)
                                             {
                                                 acknowledgement = Acknowledgement<AuthorizeRemoteReservationStartRequest>.SystemError(
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
                                                        AccessControlAllowMethods  = "POST",
                                                        AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                        ContentType                = HTTPContentType.JSON_UTF8,
                                                        Content                    = acknowledgement.ToJSON(CustomAcknowledgementSerializer,
                                                                                                            CustomStatusCodeSerializer).
                                                                                                     ToString(JSONFormatting).
                                                                                                     ToUTF8Bytes(),
                                                        Connection                 = "close"
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
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "api/oicp/charging/v21/providers/{providerId}/authorize-remote-reservation/stop",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logAuthorizeRemoteReservationStopHTTPRequest,
                                         HTTPResponseLogger:  logAuthorizeRemoteReservationStopHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime = Timestamp.Now;
                                             Acknowledgement<AuthorizeRemoteReservationStopRequest> acknowledgement = null;

                                             try
                                             {

                                                 #region Try to parse ProviderId URL parameter

                                                 if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(Request.ParsedURLParameters[0], out Provider_Id providerId))
                                                     acknowledgement = Acknowledgement<AuthorizeRemoteReservationStopRequest>.SystemError(
                                                                           null,
                                                                           "The expected 'providerId' URL parameter could not be parsed!"
                                                                       );

                                                 #endregion

                                                 else if (AuthorizeRemoteReservationStopRequest.TryParse(Request.HTTPBody?.ToUTF8String(),
                                                                                                         providerId,
                                                                                                         Request.Timeout ?? DefaultRequestTimeout,
                                                                                                         out AuthorizeRemoteReservationStopRequest  authorizeRemoteReservationStopRequest,
                                                                                                         out String                                 errorResponse,
                                                                                                         Request.Timestamp,
                                                                                                         Request.EventTrackingId,
                                                                                                         CustomAuthorizeRemoteReservationStopRequestParser))
                                                 {

                                                     Counter.AuthorizeRemoteReservationStop.IncRequests();

                                                     #region Send OnAuthorizeRemoteReservationStopRequest event

                                                     try
                                                     {

                                                         if (OnAuthorizeRemoteReservationStopRequest != null)
                                                             await Task.WhenAll(OnAuthorizeRemoteReservationStopRequest.GetInvocationList().
                                                                                Cast<OnAuthorizeRemoteReservationStopRequestDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              authorizeRemoteReservationStopRequest))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(CPOServerAPI) + "." + nameof(OnAuthorizeRemoteReservationStopRequest));
                                                     }

                                                     #endregion

                                                     #region Call async subscribers

                                                     var OnAuthorizeRemoteReservationStopLocal = OnAuthorizeRemoteReservationStop;
                                                     if (OnAuthorizeRemoteReservationStopLocal != null)
                                                     {

                                                         try
                                                         {

                                                             acknowledgement = (await Task.WhenAll(OnAuthorizeRemoteReservationStopLocal.GetInvocationList().
                                                                                                                                         Cast<OnAuthorizeRemoteReservationStopDelegate>().
                                                                                                                                         Select(e => e(Timestamp.Now,
                                                                                                                                                       this,
                                                                                                                                                       authorizeRemoteReservationStopRequest))))?.FirstOrDefault();

                                                         }
                                                         catch (Exception e)
                                                         {
                                                             acknowledgement = Acknowledgement<AuthorizeRemoteReservationStopRequest>.DataError(
                                                                                   Request:                   authorizeRemoteReservationStopRequest,
                                                                                   StatusCodeDescription:     e.Message,
                                                                                   StatusCodeAdditionalInfo:  e.StackTrace,
                                                                                   SessionId:                 authorizeRemoteReservationStopRequest.SessionId,
                                                                                   CPOPartnerSessionId:       authorizeRemoteReservationStopRequest.CPOPartnerSessionId
                                                                               );
                                                         }

                                                         if (acknowledgement == null)
                                                             acknowledgement = Acknowledgement<AuthorizeRemoteReservationStopRequest>.SystemError(
                                                                                   authorizeRemoteReservationStopRequest,
                                                                                   "Could not process the received AuthorizeRemoteReservationStop request!"
                                                                               );

                                                     }

                                                     #endregion

                                                     #region Send OnAuthorizeRemoteReservationStopResponse event

                                                     try
                                                     {

                                                         if (OnAuthorizeRemoteReservationStopResponse != null)
                                                             await Task.WhenAll(OnAuthorizeRemoteReservationStopResponse.GetInvocationList().
                                                                                Cast<OnAuthorizeRemoteReservationStopResponseDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              acknowledgement,
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
                                                     acknowledgement = Acknowledgement<AuthorizeRemoteReservationStopRequest>.DataError(
                                                                           Request:                   authorizeRemoteReservationStopRequest,
                                                                           StatusCodeDescription:     "We could not handle the given AuthorizeRemoveReservationStop request!",
                                                                           StatusCodeAdditionalInfo:  errorResponse,
                                                                           SessionId:                 authorizeRemoteReservationStopRequest.SessionId,
                                                                           CPOPartnerSessionId:       authorizeRemoteReservationStopRequest.CPOPartnerSessionId
                                                                       );

                                             }
                                             catch (Exception e)
                                             {
                                                 acknowledgement = Acknowledgement<AuthorizeRemoteReservationStopRequest>.SystemError(
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
                                                        AccessControlAllowMethods  = "POST",
                                                        AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                        ContentType                = HTTPContentType.JSON_UTF8,
                                                        Content                    = acknowledgement.ToJSON(CustomAcknowledgementSerializer,
                                                                                                            CustomStatusCodeSerializer).
                                                                                                     ToString(JSONFormatting).
                                                                                                     ToUTF8Bytes(),
                                                        Connection                 = "close"
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
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "api/oicp/charging/v21/providers/{providerId}/authorize-remote/start",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logAuthorizeRemoteStartHTTPRequest,
                                         HTTPResponseLogger:  logAuthorizeRemoteStartHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime = Timestamp.Now;
                                             Acknowledgement<AuthorizeRemoteStartRequest> acknowledgement = null;

                                             try
                                             {

                                                 #region Try to parse ProviderId URL parameter

                                                 if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(Request.ParsedURLParameters[0], out Provider_Id providerId))
                                                     acknowledgement = Acknowledgement<AuthorizeRemoteStartRequest>.SystemError(
                                                                           null,
                                                                           "The expected 'providerId' URL parameter could not be parsed!"
                                                                       );

                                                 #endregion

                                                 else if (AuthorizeRemoteStartRequest.TryParse(Request.HTTPBody?.ToUTF8String(),
                                                                                               providerId,
                                                                                               Request.Timeout ?? DefaultRequestTimeout,
                                                                                               out AuthorizeRemoteStartRequest  authorizeRemoteStartRequest,
                                                                                               out String                       errorResponse,
                                                                                               Request.Timestamp,
                                                                                               Request.EventTrackingId,
                                                                                               CustomAuthorizeRemoteStartRequestParser))
                                                 {

                                                     Counter.AuthorizeRemoteStart.IncRequests();

                                                     #region Send OnAuthorizeRemoteStartRequest event

                                                     try
                                                     {

                                                         if (OnAuthorizeRemoteStartRequest != null)
                                                             await Task.WhenAll(OnAuthorizeRemoteStartRequest.GetInvocationList().
                                                                                Cast<OnAuthorizeRemoteStartRequestDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              authorizeRemoteStartRequest))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(CPOServerAPI) + "." + nameof(OnAuthorizeRemoteStartRequest));
                                                     }

                                                     #endregion

                                                     #region Call async subscribers

                                                     var OnAuthorizeRemoteStartLocal = OnAuthorizeRemoteStart;
                                                     if (OnAuthorizeRemoteStartLocal != null)
                                                     {

                                                         try
                                                         {

                                                             acknowledgement = (await Task.WhenAll(OnAuthorizeRemoteStartLocal.GetInvocationList().
                                                                                                                               Cast<OnAuthorizeRemoteStartDelegate>().
                                                                                                                               Select(e => e(Timestamp.Now,
                                                                                                                                             this,
                                                                                                                                             authorizeRemoteStartRequest))))?.FirstOrDefault();

                                                         }
                                                         catch (Exception e)
                                                         {
                                                             acknowledgement = Acknowledgement<AuthorizeRemoteStartRequest>.DataError(
                                                                                   Request:                   authorizeRemoteStartRequest,
                                                                                   StatusCodeDescription:     e.Message,
                                                                                   StatusCodeAdditionalInfo:  e.StackTrace,
                                                                                   SessionId:                 authorizeRemoteStartRequest.SessionId,
                                                                                   CPOPartnerSessionId:       authorizeRemoteStartRequest.CPOPartnerSessionId
                                                                               );
                                                         }

                                                         if (acknowledgement == null)
                                                             acknowledgement = Acknowledgement<AuthorizeRemoteStartRequest>.SystemError(
                                                                                   authorizeRemoteStartRequest,
                                                                                   "Could not process the received AuthorizeRemoteStart request!"
                                                                               );

                                                     }

                                                     #endregion

                                                     #region Send OnAuthorizeRemoteStartResponse event

                                                     try
                                                     {

                                                         if (OnAuthorizeRemoteStartResponse != null)
                                                             await Task.WhenAll(OnAuthorizeRemoteStartResponse.GetInvocationList().
                                                                                Cast<OnAuthorizeRemoteStartResponseDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              acknowledgement,
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
                                                     acknowledgement = Acknowledgement<AuthorizeRemoteStartRequest>.DataError(
                                                                           Request:                   authorizeRemoteStartRequest,
                                                                           StatusCodeDescription:     "We could not handle the given AuthorizeRemoveStart request!",
                                                                           StatusCodeAdditionalInfo:  errorResponse,
                                                                           SessionId:                 authorizeRemoteStartRequest.SessionId,
                                                                           CPOPartnerSessionId:       authorizeRemoteStartRequest.CPOPartnerSessionId
                                                                       );

                                             }
                                             catch (Exception e)
                                             {
                                                 acknowledgement = Acknowledgement<AuthorizeRemoteStartRequest>.SystemError(
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
                                                        AccessControlAllowMethods  = "POST",
                                                        AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                        ContentType                = HTTPContentType.JSON_UTF8,
                                                        Content                    = acknowledgement.ToJSON(CustomAcknowledgementSerializer,
                                                                                                            CustomStatusCodeSerializer).
                                                                                                     ToString(JSONFormatting).
                                                                                                     ToUTF8Bytes(),
                                                        Connection                 = "close"
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
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "api/oicp/charging/v21/providers/{providerId}/authorize-remote/stop",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logAuthorizeRemoteStopHTTPRequest,
                                         HTTPResponseLogger:  logAuthorizeRemoteStopHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime = Timestamp.Now;
                                             Acknowledgement<AuthorizeRemoteStopRequest> acknowledgement = null;

                                             try
                                             {

                                                 #region Try to parse ProviderId URL parameter

                                                 if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(Request.ParsedURLParameters[0], out Provider_Id providerId))
                                                     acknowledgement = Acknowledgement<AuthorizeRemoteStopRequest>.SystemError(
                                                                           null,
                                                                           "The expected 'providerId' URL parameter could not be parsed!"
                                                                       );

                                                 #endregion

                                                 else if (AuthorizeRemoteStopRequest.TryParse(Request.HTTPBody?.ToUTF8String(),
                                                                                              providerId,
                                                                                              Request.Timeout ?? DefaultRequestTimeout,
                                                                                              out AuthorizeRemoteStopRequest  authorizeRemoteStopRequest,
                                                                                              out String                      errorResponse,
                                                                                              Request.Timestamp,
                                                                                              Request.EventTrackingId,
                                                                                              CustomAuthorizeRemoteStopRequestParser))
                                                 {

                                                     Counter.AuthorizeRemoteStop.IncRequests();

                                                     #region Send OnAuthorizeRemoteStopRequest event

                                                     try
                                                     {

                                                         if (OnAuthorizeRemoteStopRequest != null)
                                                             await Task.WhenAll(OnAuthorizeRemoteStopRequest.GetInvocationList().
                                                                                Cast<OnAuthorizeRemoteStopRequestDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              authorizeRemoteStopRequest))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(CPOServerAPI) + "." + nameof(OnAuthorizeRemoteStopRequest));
                                                     }

                                                     #endregion

                                                     #region Call async subscribers

                                                     var OnAuthorizeRemoteStopLocal = OnAuthorizeRemoteStop;
                                                     if (OnAuthorizeRemoteStopLocal != null)
                                                     {

                                                         try
                                                         {

                                                             acknowledgement = (await Task.WhenAll(OnAuthorizeRemoteStopLocal.GetInvocationList().
                                                                                                                              Cast<OnAuthorizeRemoteStopDelegate>().
                                                                                                                              Select(e => e(Timestamp.Now,
                                                                                                                                            this,
                                                                                                                                            authorizeRemoteStopRequest))))?.FirstOrDefault();

                                                         }
                                                         catch (Exception e)
                                                         {
                                                             acknowledgement = Acknowledgement<AuthorizeRemoteStopRequest>.DataError(
                                                                                   Request:                   authorizeRemoteStopRequest,
                                                                                   StatusCodeDescription:     e.Message,
                                                                                   StatusCodeAdditionalInfo:  e.StackTrace,
                                                                                   SessionId:                 authorizeRemoteStopRequest.SessionId,
                                                                                   CPOPartnerSessionId:       authorizeRemoteStopRequest.CPOPartnerSessionId
                                                                               );
                                                         }

                                                         if (acknowledgement == null)
                                                             acknowledgement = Acknowledgement<AuthorizeRemoteStopRequest>.SystemError(
                                                                                   authorizeRemoteStopRequest,
                                                                                   "Could not process the received AuthorizeRemoteStop request!"
                                                                               );

                                                     }

                                                     #endregion

                                                     #region Send OnAuthorizeRemoteStopResponse event

                                                     try
                                                     {

                                                         if (OnAuthorizeRemoteStopResponse != null)
                                                             await Task.WhenAll(OnAuthorizeRemoteStopResponse.GetInvocationList().
                                                                                Cast<OnAuthorizeRemoteStopResponseDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              acknowledgement,
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
                                                     acknowledgement = Acknowledgement<AuthorizeRemoteStopRequest>.DataError(
                                                                           Request:                   authorizeRemoteStopRequest,
                                                                           StatusCodeDescription:     "We could not handle the given AuthorizeRemoveStop request!",
                                                                           StatusCodeAdditionalInfo:  errorResponse,
                                                                           SessionId:                 authorizeRemoteStopRequest.SessionId,
                                                                           CPOPartnerSessionId:       authorizeRemoteStopRequest.CPOPartnerSessionId
                                                                       );

                                             }
                                             catch (Exception e)
                                             {
                                                 acknowledgement = Acknowledgement<AuthorizeRemoteStopRequest>.SystemError(
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
                                                        AccessControlAllowMethods  = "POST",
                                                        AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                        ContentType                = HTTPContentType.JSON_UTF8,
                                                        Content                    = acknowledgement.ToJSON(CustomAcknowledgementSerializer,
                                                                                                            CustomStatusCodeSerializer).
                                                                                                     ToString(JSONFormatting).
                                                                                                     ToUTF8Bytes(),
                                                        Connection                 = "close"
                                                    }.AsImmutable;

                                          }, AllowReplacement: URLReplacement.Allow);

            #endregion

        }

        #endregion


        #region Dispose()

        /// <summary>
        /// Dispose this object.
        /// </summary>
        public void Dispose()
        { }

        #endregion

    }

}
