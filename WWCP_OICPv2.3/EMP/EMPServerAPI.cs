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

namespace cloud.charging.open.protocols.OICPv2_3.EMP
{

    /// <summary>
    /// The EMP HTTP Server API.
    /// </summary>
    public partial class EMPServerAPI : HTTPAPI
    {

        #region (class) Counters

        public class Counters
        {

            public CounterValues  AuthorizeStart                  { get; }
            public CounterValues  AuthorizeStop                   { get; }
            public CounterValues  ChargingStartNotification       { get; }
            public CounterValues  ChargingProgressNotification    { get; }
            public CounterValues  ChargingEndNotification         { get; }
            public CounterValues  ChargingErrorNotification       { get; }
            public CounterValues  ChargeDetailRecord              { get; }


            public Counters(CounterValues? AuthorizeStart                 = null,
                            CounterValues? AuthorizeStop                  = null,
                            CounterValues? ChargingStartNotification      = null,
                            CounterValues? ChargingProgressNotification   = null,
                            CounterValues? ChargingEndNotification        = null,
                            CounterValues? ChargingErrorNotification      = null,
                            CounterValues? ChargeDetailRecord             = null)
            {

                this.AuthorizeStart                = AuthorizeStart               ?? new CounterValues();
                this.AuthorizeStop                 = AuthorizeStop                ?? new CounterValues();
                this.ChargingStartNotification     = ChargingStartNotification    ?? new CounterValues();
                this.ChargingProgressNotification  = ChargingProgressNotification ?? new CounterValues();
                this.ChargingEndNotification       = ChargingEndNotification      ?? new CounterValues();
                this.ChargingErrorNotification     = ChargingErrorNotification    ?? new CounterValues();
                this.ChargeDetailRecord            = ChargeDetailRecord           ?? new CounterValues();

            }

            public JObject ToJSON()

                => JSONObject.Create(
                       new JProperty("AuthorizeStart",                AuthorizeStart.              ToJSON()),
                       new JProperty("AuthorizeStop",                 AuthorizeStop.               ToJSON()),
                       new JProperty("ChargingStartNotification",     ChargingStartNotification.   ToJSON()),
                       new JProperty("ChargingProgressNotification",  ChargingProgressNotification.ToJSON()),
                       new JProperty("ChargingEndNotification",       ChargingEndNotification.     ToJSON()),
                       new JProperty("ChargingErrorNotification",     ChargingErrorNotification.   ToJSON()),
                       new JProperty("ChargeDetailRecord",            ChargeDetailRecord.          ToJSON())
                   );

        }

        #endregion


        #region Data

        /// <summary>
        /// The default HTTP server name.
        /// </summary>
        public new const String  DefaultHTTPServerName   = "GraphDefined OICP " + Version.Number + " EMP HTTP API";

        /// <summary>
        /// The default HTTP service name.
        /// </summary>
        public new const String  DefaultHTTPServiceName  = "GraphDefined OICP " + Version.Number + " EMP HTTP API";

        /// <summary>
        /// The default logging context.
        /// </summary>
        public     const String  DefaultLoggingContext   = "EMPServerAPI";

        #endregion

        #region Properties

        public Counters                                                          Counter                                            { get; }

        // Custom JSON parsers

        public CustomJObjectParserDelegate<AuthorizeStartRequest>                CustomAuthorizeStartRequestParser                  { get; set; }
        public CustomJObjectParserDelegate<AuthorizeStopRequest>                 CustomAuthorizeStopRequestParser                   { get; set; }


        public CustomJObjectParserDelegate<ChargingStartNotificationRequest>     CustomChargingStartNotificationRequestParser       { get; set; }
        public CustomJObjectParserDelegate<ChargingProgressNotificationRequest>  CustomChargingProgressNotificationRequestParser    { get; set; }
        public CustomJObjectParserDelegate<ChargingEndNotificationRequest>       CustomChargingEndNotificationRequestParser         { get; set; }
        public CustomJObjectParserDelegate<ChargingErrorNotificationRequest>     CustomChargingErrorNotificationRequestParser       { get; set; }


        public CustomJObjectParserDelegate<ChargeDetailRecordRequest>            CustomChargeDetailRecordRequestParser              { get; set; }


        // Custom JSON serializers
        public CustomJObjectSerializerDelegate<Acknowledgement>                  CustomAcknowledgementSerializer                    { get; set; }

        public CustomJObjectSerializerDelegate<StatusCode>                       CustomStatusCodeSerializer                         { get; set; }


        public Newtonsoft.Json.Formatting                                        JSONFormatting                                     { get; set; }

        #endregion

        #region Events

        #region (protected internal) OnAuthorizeStartHTTPRequest

        /// <summary>
        /// An event sent whenever an AuthorizeStart HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnAuthorizeStartHTTPRequest = new HTTPRequestLogEvent();

        /// <summary>
        /// An event sent whenever an AuthorizeStart HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Server HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logAuthorizeStartHTTPRequest(DateTime     Timestamp,
                                                             HTTPAPI      API,
                                                             HTTPRequest  Request)

            => OnAuthorizeStartHTTPRequest?.WhenAll(Timestamp,
                                                    API ?? this,
                                                    Request);

        #endregion

        #region (event)              OnAuthorizeStart(Request-/Response)

        /// <summary>
        /// An event send whenever an AuthorizeStart request was received.
        /// </summary>
        public event OnAuthorizeStartRequestDelegate   OnAuthorizeStartRequest;

        /// <summary>
        /// An event send whenever an AuthorizeStart request was received.
        /// </summary>
        public event OnAuthorizeStartDelegate          OnAuthorizeStart;

        /// <summary>
        /// An event send whenever a response to an AuthorizeStart request was sent.
        /// </summary>
        public event OnAuthorizeStartResponseDelegate  OnAuthorizeStartResponse;

        #endregion

        #region (protected internal) OnAuthorizationStartHTTPResponse

        /// <summary>
        /// An event sent whenever an AuthorizationStart HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnAuthorizationStartHTTPResponse = new HTTPResponseLogEvent();

        /// <summary>
        /// An event sent whenever an AuthorizationStart HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Server HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logAuthorizationStartHTTPResponse(DateTime      Timestamp,
                                                                  HTTPAPI       API,
                                                                  HTTPRequest   Request,
                                                                  HTTPResponse  Response)

            => OnAuthorizationStartHTTPResponse?.WhenAll(Timestamp,
                                                         API ?? this,
                                                         Request,
                                                         Response);

        #endregion


        #region (protected internal) OnAuthorizeStopHTTPRequest

        /// <summary>
        /// An event sent whenever an AuthorizeStop HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnAuthorizeStopHTTPRequest = new HTTPRequestLogEvent();

        /// <summary>
        /// An event sent whenever an AuthorizeStop HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Server HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logAuthorizeStopHTTPRequest(DateTime      Timestamp,
                                                            HTTPAPI       API,
                                                            HTTPRequest   Request)

            => OnAuthorizeStopHTTPRequest?.WhenAll(Timestamp,
                                                    API ?? this,
                                                    Request);

        #endregion

        #region (event)              OnAuthorizeStop(Request-/Response)

        /// <summary>
        /// An event send whenever an AuthorizeStop request was received.
        /// </summary>
        public event OnAuthorizeStopRequestDelegate   OnAuthorizeStopRequest;

        /// <summary>
        /// An event send whenever an AuthorizeStop request was received.
        /// </summary>
        public event OnAuthorizeStopDelegate          OnAuthorizeStop;

        /// <summary>
        /// An event send whenever a response to an AuthorizeStop request was sent.
        /// </summary>
        public event OnAuthorizeStopResponseDelegate  OnAuthorizeStopResponse;

        #endregion

        #region (protected internal) OnAuthorizationStopHTTPResponse

        /// <summary>
        /// An event sent whenever an AuthorizationStop HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnAuthorizationStopHTTPResponse = new HTTPResponseLogEvent();

        /// <summary>
        /// An event sent whenever an AuthorizationStop HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Server HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logAuthorizationStopHTTPResponse(DateTime      Timestamp,
                                                                 HTTPAPI       API,
                                                                 HTTPRequest   Request,
                                                                 HTTPResponse  Response)

            => OnAuthorizationStopHTTPResponse?.WhenAll(Timestamp,
                                                         API ?? this,
                                                         Request,
                                                         Response);

        #endregion


        #region (protected internal) OnChargingNotificationsHTTPRequest

        /// <summary>
        /// An event sent whenever a ChargingNotifications HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnChargingNotificationsHTTPRequest = new HTTPRequestLogEvent();

        /// <summary>
        /// An event sent whenever a ChargingNotifications HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the notification.</param>
        /// <param name="API">The EMP Server HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logChargingNotificationsHTTPRequest(DateTime      Timestamp,
                                                                    HTTPAPI       API,
                                                                    HTTPRequest   Request)

            => OnChargingNotificationsHTTPRequest?.WhenAll(Timestamp,
                                                           API ?? this,
                                                           Request);

        #endregion

        #region (event)              OnCharging(...)Notification(Request-/Response)

        public event OnChargingStartNotificationRequestDelegate      OnChargingStartNotificationRequest;
        public event OnChargingStartNotificationDelegate             OnChargingStartNotification;
        public event OnChargingStartNotificationResponseDelegate     OnChargingStartNotificationResponse;

        public event OnChargingProgressNotificationRequestDelegate   OnChargingProgressNotificationRequest;
        public event OnChargingProgressNotificationDelegate          OnChargingProgressNotification;
        public event OnChargingProgressNotificationResponseDelegate  OnChargingProgressNotificationResponse;

        public event OnChargingEndNotificationRequestDelegate        OnChargingEndNotificationRequest;
        public event OnChargingEndNotificationDelegate               OnChargingEndNotification;
        public event OnChargingEndNotificationResponseDelegate       OnChargingEndNotificationResponse;

        public event OnChargingErrorNotificationRequestDelegate      OnChargingErrorNotificationRequest;
        public event OnChargingErrorNotificationDelegate             OnChargingErrorNotification;
        public event OnChargingErrorNotificationResponseDelegate     OnChargingErrorNotificationResponse;

        #endregion

        #region (protected internal) OnChargingNotificationsHTTPResponse

        /// <summary>
        /// An event sent whenever a ChargingNotifications HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnChargingNotificationsHTTPResponse = new HTTPResponseLogEvent();

        /// <summary>
        /// An event sent whenever a ChargingNotifications HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the notification.</param>
        /// <param name="API">The EMP Server HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logChargingNotificationsHTTPResponse(DateTime      Timestamp,
                                                                     HTTPAPI       API,
                                                                     HTTPRequest   Request,
                                                                     HTTPResponse  Response)

            => OnChargingNotificationsHTTPResponse?.WhenAll(Timestamp,
                                                            API ?? this,
                                                            Request,
                                                            Response);

        #endregion


        #region (protected internal) OnChargeDetailRecordHTTPRequest

        /// <summary>
        /// An event sent whenever a ChargeDetailRecord HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnChargeDetailRecordHTTPRequest = new HTTPRequestLogEvent();

        /// <summary>
        /// An event sent whenever a ChargeDetailRecord HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Server HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logChargeDetailRecordHTTPRequest(DateTime      Timestamp,
                                                                 HTTPAPI       API,
                                                                 HTTPRequest   Request)

            => OnChargeDetailRecordHTTPRequest?.WhenAll(Timestamp,
                                                        API ?? this,
                                                        Request);

        #endregion

        #region (event)              OnChargeDetailRecord(Request-/Response)

        /// <summary>
        /// An event send whenever a charge detail record was received.
        /// </summary>
        public event OnChargeDetailRecordRequestDelegate   OnChargeDetailRecordRequest;

        /// <summary>
        /// An event send whenever a charge detail record was received.
        /// </summary>
        public event OnChargeDetailRecordDelegate          OnChargeDetailRecord;

        /// <summary>
        /// An event send whenever a response to a received charge detail record was sent.
        /// </summary>
        public event OnChargeDetailRecordResponseDelegate  OnChargeDetailRecordResponse;

        #endregion

        #region (protected internal) OnChargeDetailRecordHTTPResponse

        /// <summary>
        /// An event sent whenever a ChargeDetailRecord HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnChargeDetailRecordHTTPResponse = new HTTPResponseLogEvent();

        /// <summary>
        /// An event sent whenever a ChargeDetailRecord HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Server HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logChargeDetailRecordHTTPResponse(DateTime      Timestamp,
                                                                  HTTPAPI       API,
                                                                  HTTPRequest   Request,
                                                                  HTTPResponse  Response)

            => OnChargeDetailRecordHTTPResponse?.WhenAll(Timestamp,
                                                         API ?? this,
                                                         Request,
                                                         Response);

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EMP HTTP Server API.
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
        public EMPServerAPI(HTTPHostname?                        HTTPHostname                       = null,
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


            //Note: OperatorId is the remote EMP sending an authorize start/stop request!

            #region POST  ~/api/oicp/charging/v21/operators/{operatorId}/authorize/start

            // --------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/charging/v21/operators/{operatorId}/authorize/start
            // --------------------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/charging/v21/operators/{operatorId}/authorize/start",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logAuthorizeStartHTTPRequest,
                                         HTTPResponseLogger:  logAuthorizationStartHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime = Timestamp.Now;
                                             AuthorizationStartResponse authorizationStartResponse = null;

                                             try
                                             {

                                                 #region Try to parse OperatorId URL parameter

                                                 if (Request.ParsedURLParameters.Length != 1 || !Operator_Id.TryParse(Request.ParsedURLParameters[0], out Operator_Id operatorId))
                                                     authorizationStartResponse = AuthorizationStartResponse.SystemError(
                                                                                      null,
                                                                                      "The expected 'operatorId' URL parameter could not be parsed!"
                                                                                  );

                                                 #endregion

                                                 else if (AuthorizeStartRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                         operatorId,
                                                                                         Request.Timeout ?? DefaultRequestTimeout,
                                                                                         out AuthorizeStartRequest?  authorizeStartRequest,
                                                                                         out String?                 errorResponse,
                                                                                         Request.Timestamp,
                                                                                         Request.EventTrackingId,
                                                                                         CustomAuthorizeStartRequestParser))
                                                 {

                                                     Counter.AuthorizeStart.IncRequests();

                                                     #region Send OnAuthorizeStartRequest event

                                                     try
                                                     {

                                                         if (OnAuthorizeStartRequest is not null)
                                                             await Task.WhenAll(OnAuthorizeStartRequest.GetInvocationList().
                                                                                Cast<OnAuthorizeStartRequestDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              authorizeStartRequest!))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPServerAPI) + "." + nameof(OnAuthorizeStartRequest));
                                                     }

                                                     #endregion

                                                     #region Call async subscribers

                                                     var OnAuthorizeStartLocal = OnAuthorizeStart;
                                                     if (OnAuthorizeStartLocal is not null)
                                                     {

                                                         try
                                                         {

                                                             authorizationStartResponse = (await Task.WhenAll(OnAuthorizeStartLocal.GetInvocationList().
                                                                                                                                    Cast<OnAuthorizeStartDelegate>().
                                                                                                                                    Select(e => e(Timestamp.Now,
                                                                                                                                                  this,
                                                                                                                                                  authorizeStartRequest!))))?.FirstOrDefault();

                                                         }
                                                         catch (Exception e)
                                                         {
                                                             authorizationStartResponse = AuthorizationStartResponse.DataError(
                                                                                              Request:                   authorizeStartRequest!,
                                                                                              StatusCodeDescription:     e.Message,
                                                                                              StatusCodeAdditionalInfo:  e.StackTrace,
                                                                                              SessionId:                 authorizeStartRequest?.SessionId,
                                                                                              CPOPartnerSessionId:       authorizeStartRequest?.CPOPartnerSessionId
                                                                                          );
                                                         }

                                                         if (authorizationStartResponse is null)
                                                             authorizationStartResponse = AuthorizationStartResponse.SystemError(
                                                                                              authorizeStartRequest!,
                                                                                              "Could not process the received AuthorizeStart request!"
                                                                                          );

                                                     }

                                                     #endregion

                                                     #region Send OnAuthorizeStartResponse event

                                                     try
                                                     {

                                                         if (OnAuthorizeStartResponse != null)
                                                             await Task.WhenAll(OnAuthorizeStartResponse.GetInvocationList().
                                                                                Cast<OnAuthorizeStartResponseDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              authorizationStartResponse!,
                                                                                              Timestamp.Now - startTime))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPServerAPI) + "." + nameof(OnAuthorizeStartResponse));
                                                     }

                                                     #endregion

                                                 }
                                                 else
                                                     authorizationStartResponse = AuthorizationStartResponse.DataError(
                                                                                      Request:                   authorizeStartRequest, // maybe null!
                                                                                      StatusCodeDescription:     "We could not parse the given AuthorizeStart request!",
                                                                                      StatusCodeAdditionalInfo:  errorResponse,
                                                                                      SessionId:                 authorizeStartRequest?.SessionId,
                                                                                      CPOPartnerSessionId:       authorizeStartRequest?.CPOPartnerSessionId
                                                                                  );

                                             }
                                             catch (Exception e)
                                             {
                                                 authorizationStartResponse = AuthorizationStartResponse.SystemError(
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
                                                        Content                    = authorizationStartResponse.ToJSON().
                                                                                                                ToString(JSONFormatting).
                                                                                                                ToUTF8Bytes(),
                                                        Connection                 = "close"
                                                    }.AsImmutable;

                                          }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region POST  ~/api/oicp/charging/v21/operators/{operatorId}/authorize/stop

            // --------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/charging/v21/operators/{operatorId}/authorize/stop
            // --------------------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/charging/v21/operators/{operatorId}/authorize/stop",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logAuthorizeStopHTTPRequest,
                                         HTTPResponseLogger:  logAuthorizationStopHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime = Timestamp.Now;
                                             AuthorizationStopResponse authorizationStopResponse = null;

                                             try
                                             {

                                                 #region Try to parse OperatorId URL parameter

                                                 if (Request.ParsedURLParameters.Length != 1 || !Operator_Id.TryParse(Request.ParsedURLParameters[0], out Operator_Id operatorId))
                                                     authorizationStopResponse = AuthorizationStopResponse.SystemError(
                                                                                     null,
                                                                                     "The expected 'operatorId' URL parameter could not be parsed!"
                                                                                 );

                                                 #endregion

                                                 else if (AuthorizeStopRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                        operatorId,
                                                                                        Request.Timeout ?? DefaultRequestTimeout,
                                                                                        out AuthorizeStopRequest  authorizeStopRequest,
                                                                                        out String                errorResponse,
                                                                                        Request.Timestamp,
                                                                                        Request.EventTrackingId,
                                                                                        CustomAuthorizeStopRequestParser))
                                                 {

                                                     Counter.AuthorizeStop.IncRequests();

                                                     #region Send OnAuthorizeStopRequest event

                                                     try
                                                     {

                                                         if (OnAuthorizeStopRequest != null)
                                                             await Task.WhenAll(OnAuthorizeStopRequest.GetInvocationList().
                                                                                Cast<OnAuthorizeStopRequestDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              authorizeStopRequest))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPServerAPI) + "." + nameof(OnAuthorizeStopRequest));
                                                     }

                                                     #endregion

                                                     #region Call async subscribers

                                                     var OnAuthorizeStopLocal = OnAuthorizeStop;
                                                     if (OnAuthorizeStopLocal != null)
                                                     {
                                                         try
                                                         {

                                                             authorizationStopResponse = (await Task.WhenAll(OnAuthorizeStopLocal.GetInvocationList().
                                                                                                                                  Cast<OnAuthorizeStopDelegate>().
                                                                                                                                  Select(e => e(Timestamp.Now,
                                                                                                                                                this,
                                                                                                                                                authorizeStopRequest))))?.FirstOrDefault();

                                                         }
                                                         catch (Exception e)
                                                         {
                                                             authorizationStopResponse = AuthorizationStopResponse.DataError(
                                                                                             Request:                   authorizeStopRequest,
                                                                                             StatusCodeDescription:     e.Message,
                                                                                             StatusCodeAdditionalInfo:  e.StackTrace,
                                                                                             SessionId:                 authorizeStopRequest.SessionId,
                                                                                             CPOPartnerSessionId:       authorizeStopRequest.CPOPartnerSessionId
                                                                                         );
                                                         }
                                                     }

                                                     if (authorizationStopResponse == null)
                                                         authorizationStopResponse = AuthorizationStopResponse.SystemError(
                                                                                         authorizeStopRequest,
                                                                                         "Could not process the received AuthorizeStop request!"
                                                                                     );

                                                     #endregion

                                                     #region Send OnAuthorizeStopResponse event

                                                     try
                                                     {

                                                         if (OnAuthorizeStopResponse != null)
                                                             await Task.WhenAll(OnAuthorizeStopResponse.GetInvocationList().
                                                                                Cast<OnAuthorizeStopResponseDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              authorizationStopResponse,
                                                                                              Timestamp.Now - startTime))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPServerAPI) + "." + nameof(OnAuthorizeStopResponse));
                                                     }

                                                     #endregion

                                                 }
                                                 else
                                                     authorizationStopResponse = AuthorizationStopResponse.DataError(
                                                                                     Request:                   authorizeStopRequest,
                                                                                     StatusCodeDescription:     "We could not handle the given AuthorizeStop request!",
                                                                                     StatusCodeAdditionalInfo:  errorResponse,
                                                                                     SessionId:                 authorizeStopRequest.SessionId,
                                                                                     CPOPartnerSessionId:       authorizeStopRequest.CPOPartnerSessionId
                                                                                 );

                                             }
                                             catch (Exception e)
                                             {
                                                 authorizationStopResponse = AuthorizationStopResponse.SystemError(
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
                                                        Content                    = authorizationStopResponse.ToJSON().
                                                                                                               ToString(JSONFormatting).
                                                                                                               ToUTF8Bytes(),
                                                        Connection                 = "close"
                                                    }.AsImmutable;

                                          }, AllowReplacement: URLReplacement.Allow);

            #endregion


            #region POST  ~/api/oicp/notificationmgmt/v11/charging-notifications

            // ------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/notificationmgmt/v11/charging-notifications
            // ------------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/notificationmgmt/v11/charging-notifications",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logChargingNotificationsHTTPRequest,
                                         HTTPResponseLogger:  logChargingNotificationsHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             try
                                             {

                                                 #region Try to parse the charging notification

                                                 var startTime      = Timestamp.Now;
                                                 var errorResponse  = String.Empty;

                                                 JObject acknowledgementJSON = null;

                                                 if (!Request.TryParseJObjectRequestBody(out JObject JSONRequest, out HTTPResponse.Builder HTTPResponse)
                                                        ||
                                                     !JSONRequest.ParseMandatory("Type",
                                                                                 "charging notification type",
                                                                                 ChargingNotificationTypesExtensions.TryParse,
                                                                                 out ChargingNotificationTypes  chargingNotificationType,
                                                                                 out                            errorResponse))
                                                 {

                                                     return new HTTPResponse.Builder(Request) {
                                                                HTTPStatusCode             = HTTPStatusCode.BadRequest,
                                                                Server                     = HTTPServer.DefaultServerName,
                                                                Date                       = Timestamp.Now,
                                                                AccessControlAllowOrigin   = "*",
                                                                AccessControlAllowMethods  = "POST",
                                                                AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                ContentType                = HTTPContentType.JSON_UTF8,
                                                                Content                    = Acknowledgement.DataError(
                                                                                                                 RequestTimestamp:          Request.Timestamp,
                                                                                                                 StatusCodeDescription:     "We could not handle the given charging notification request!",
                                                                                                                 StatusCodeAdditionalInfo:  errorResponse
                                                                                                             ).
                                                                                                             ToJSON(CustomAcknowledgementSerializer,
                                                                                                                    CustomStatusCodeSerializer).
                                                                                                             ToString(JSONFormatting).
                                                                                                             ToUTF8Bytes(),
                                                                Connection                 = "close"
                                                            }.AsImmutable;

                                                 }

                                                 #endregion

                                                 switch (chargingNotificationType)
                                                 {

                                                     #region Start

                                                     case ChargingNotificationTypes.Start:

                                                         if (ChargingStartNotificationRequest.TryParse(JSONRequest,
                                                                                                       Request.Timeout ?? DefaultRequestTimeout,
                                                                                                       out ChargingStartNotificationRequest  chargingStartNotificationRequest,
                                                                                                       out                                   errorResponse,
                                                                                                       Request.Timestamp,
                                                                                                       Request.EventTrackingId,
                                                                                                       CustomChargingStartNotificationRequestParser))
                                                         {

                                                             Counter.ChargingStartNotification.IncRequests();

                                                             #region Send OnChargingStartNotificationRequest event

                                                             try
                                                             {

                                                                 if (OnChargingStartNotificationRequest != null)
                                                                     await Task.WhenAll(OnChargingStartNotificationRequest.GetInvocationList().
                                                                                        Cast<OnChargingStartNotificationRequestDelegate>().
                                                                                        Select(e => e(Timestamp.Now,
                                                                                                      this,
                                                                                                      chargingStartNotificationRequest))).
                                                                                        ConfigureAwait(false);

                                                             }
                                                             catch (Exception e)
                                                             {
                                                                 DebugX.LogException(e, nameof(EMPServerAPI) + "." + nameof(OnChargingStartNotificationRequest));
                                                             }

                                                             #endregion

                                                             #region Call async subscribers

                                                             Acknowledgement<ChargingStartNotificationRequest> acknowledgement = null;

                                                             if (OnChargingStartNotification != null)
                                                                 acknowledgement = (await Task.WhenAll(OnChargingStartNotification.GetInvocationList().
                                                                                                       Cast<OnChargingStartNotificationDelegate>().
                                                                                                       Select(e => e(Timestamp.Now,
                                                                                                                     this,
                                                                                                                     chargingStartNotificationRequest))).
                                                                                                       ConfigureAwait(false))?.FirstOrDefault();

                                                             if (acknowledgement == null)
                                                                 acknowledgement = Acknowledgement<ChargingStartNotificationRequest>.SystemError(
                                                                                       chargingStartNotificationRequest,
                                                                                       "Could not process the received ChargingStartNotification request!"
                                                                                   );

                                                             #endregion

                                                             #region Send OnChargingStartNotificationResponse event

                                                             try
                                                             {

                                                                 if (OnChargingStartNotificationResponse != null)
                                                                     await Task.WhenAll(OnChargingStartNotificationResponse.GetInvocationList().
                                                                                        Cast<OnChargingStartNotificationResponseDelegate>().
                                                                                        Select(e => e(Timestamp.Now,
                                                                                                      this,
                                                                                                      acknowledgement,
                                                                                                      Timestamp.Now - startTime))).
                                                                                        ConfigureAwait(false);

                                                             }
                                                             catch (Exception e)
                                                             {
                                                                 DebugX.LogException(e, nameof(EMPServerAPI) + "." + nameof(OnChargingStartNotificationResponse));
                                                             }

                                                             #endregion

                                                             acknowledgementJSON = acknowledgement.ToJSON(CustomAcknowledgementSerializer,
                                                                                                          CustomStatusCodeSerializer);

                                                         }

                                                     break;

                                                     #endregion

                                                     #region Progress

                                                     case ChargingNotificationTypes.Progress:

                                                         if (ChargingProgressNotificationRequest.TryParse(JSONRequest,
                                                                                                          Request.Timeout ?? DefaultRequestTimeout,
                                                                                                          out ChargingProgressNotificationRequest  chargingProgressNotificationRequest,
                                                                                                          out                                      errorResponse,
                                                                                                          Request.Timestamp,
                                                                                                          Request.EventTrackingId,
                                                                                                          CustomChargingProgressNotificationRequestParser))
                                                         {

                                                             Counter.ChargingProgressNotification.IncRequests();

                                                             #region Send OnChargingProgressNotificationRequest event

                                                             try
                                                             {

                                                                 if (OnChargingProgressNotificationRequest != null)
                                                                     await Task.WhenAll(OnChargingProgressNotificationRequest.GetInvocationList().
                                                                                        Cast<OnChargingProgressNotificationRequestDelegate>().
                                                                                        Select(e => e(Timestamp.Now,
                                                                                                      this,
                                                                                                      chargingProgressNotificationRequest))).
                                                                                        ConfigureAwait(false);

                                                             }
                                                             catch (Exception e)
                                                             {
                                                                 DebugX.LogException(e, nameof(EMPServerAPI) + "." + nameof(OnChargingProgressNotificationRequest));
                                                             }

                                                             #endregion

                                                             #region Call async subscribers

                                                             Acknowledgement<ChargingProgressNotificationRequest> acknowledgement = null;

                                                             if (OnChargingProgressNotification != null)
                                                                 acknowledgement = (await Task.WhenAll(OnChargingProgressNotification.GetInvocationList().
                                                                                                       Cast<OnChargingProgressNotificationDelegate>().
                                                                                                       Select(e => e(Timestamp.Now,
                                                                                                                     this,
                                                                                                                     chargingProgressNotificationRequest))).
                                                                                                       ConfigureAwait(false))?.FirstOrDefault();

                                                             if (acknowledgement == null)
                                                                 acknowledgement = Acknowledgement<ChargingProgressNotificationRequest>.SystemError(
                                                                                       chargingProgressNotificationRequest,
                                                                                       "Could not process the received ChargingProgressNotification request!"
                                                                                   );

                                                             #endregion

                                                             #region Send OnChargingProgressNotificationResponse event

                                                             try
                                                             {

                                                                 if (OnChargingProgressNotificationResponse != null)
                                                                     await Task.WhenAll(OnChargingProgressNotificationResponse.GetInvocationList().
                                                                                        Cast<OnChargingProgressNotificationResponseDelegate>().
                                                                                        Select(e => e(Timestamp.Now,
                                                                                                      this,
                                                                                                      acknowledgement,
                                                                                                      Timestamp.Now - startTime))).
                                                                                        ConfigureAwait(false);

                                                             }
                                                             catch (Exception e)
                                                             {
                                                                 DebugX.LogException(e, nameof(EMPServerAPI) + "." + nameof(OnChargingProgressNotificationResponse));
                                                             }

                                                             #endregion

                                                             acknowledgementJSON = acknowledgement.ToJSON(CustomAcknowledgementSerializer,
                                                                                                          CustomStatusCodeSerializer);

                                                         }

                                                     break;

                                                     #endregion

                                                     #region End

                                                     case ChargingNotificationTypes.End:

                                                         if (ChargingEndNotificationRequest.TryParse(JSONRequest,
                                                                                                          Request.Timeout ?? DefaultRequestTimeout,
                                                                                                          out ChargingEndNotificationRequest  chargingEndNotificationRequest,
                                                                                                          out                                      errorResponse,
                                                                                                          Request.Timestamp,
                                                                                                          Request.EventTrackingId,
                                                                                                          CustomChargingEndNotificationRequestParser))
                                                         {

                                                             Counter.ChargingEndNotification.IncRequests();

                                                             #region Send OnChargingEndNotificationRequest event

                                                             try
                                                             {

                                                                 if (OnChargingEndNotificationRequest != null)
                                                                     await Task.WhenAll(OnChargingEndNotificationRequest.GetInvocationList().
                                                                                        Cast<OnChargingEndNotificationRequestDelegate>().
                                                                                        Select(e => e(Timestamp.Now,
                                                                                                      this,
                                                                                                      chargingEndNotificationRequest))).
                                                                                        ConfigureAwait(false);

                                                             }
                                                             catch (Exception e)
                                                             {
                                                                 DebugX.LogException(e, nameof(EMPServerAPI) + "." + nameof(OnChargingEndNotificationRequest));
                                                             }

                                                             #endregion

                                                             #region Call async subscribers

                                                             Acknowledgement<ChargingEndNotificationRequest> acknowledgement = null;

                                                             if (OnChargingEndNotification != null)
                                                                 acknowledgement = (await Task.WhenAll(OnChargingEndNotification.GetInvocationList().
                                                                                                       Cast<OnChargingEndNotificationDelegate>().
                                                                                                       Select(e => e(Timestamp.Now,
                                                                                                                     this,
                                                                                                                     chargingEndNotificationRequest))).
                                                                                                       ConfigureAwait(false))?.FirstOrDefault();

                                                             if (acknowledgement == null)
                                                                 acknowledgement = Acknowledgement<ChargingEndNotificationRequest>.SystemError(
                                                                                       chargingEndNotificationRequest,
                                                                                       "Could not process the received ChargingEndNotification request!"
                                                                                   );

                                                             #endregion

                                                             #region Send OnChargingEndNotificationResponse event

                                                             try
                                                             {

                                                                 if (OnChargingEndNotificationResponse != null)
                                                                     await Task.WhenAll(OnChargingEndNotificationResponse.GetInvocationList().
                                                                                        Cast<OnChargingEndNotificationResponseDelegate>().
                                                                                        Select(e => e(Timestamp.Now,
                                                                                                      this,
                                                                                                      acknowledgement,
                                                                                                      Timestamp.Now - startTime))).
                                                                                        ConfigureAwait(false);

                                                             }
                                                             catch (Exception e)
                                                             {
                                                                 DebugX.LogException(e, nameof(EMPServerAPI) + "." + nameof(OnChargingEndNotificationResponse));
                                                             }

                                                             #endregion

                                                             acknowledgementJSON = acknowledgement.ToJSON(CustomAcknowledgementSerializer,
                                                                                                          CustomStatusCodeSerializer);

                                                         }

                                                     break;

                                                     #endregion

                                                     #region Error

                                                     case ChargingNotificationTypes.Error:

                                                         if (ChargingErrorNotificationRequest.TryParse(JSONRequest,
                                                                                                          Request.Timeout ?? DefaultRequestTimeout,
                                                                                                          out ChargingErrorNotificationRequest  chargingErrorNotificationRequest,
                                                                                                          out                                      errorResponse,
                                                                                                          Request.Timestamp,
                                                                                                          Request.EventTrackingId,
                                                                                                          CustomChargingErrorNotificationRequestParser))
                                                         {

                                                             Counter.ChargingErrorNotification.IncRequests();

                                                             #region Send OnChargingErrorNotificationRequest event

                                                             try
                                                             {

                                                                 if (OnChargingErrorNotificationRequest != null)
                                                                     await Task.WhenAll(OnChargingErrorNotificationRequest.GetInvocationList().
                                                                                        Cast<OnChargingErrorNotificationRequestDelegate>().
                                                                                        Select(e => e(Timestamp.Now,
                                                                                                      this,
                                                                                                      chargingErrorNotificationRequest))).
                                                                                        ConfigureAwait(false);

                                                             }
                                                             catch (Exception e)
                                                             {
                                                                 DebugX.LogException(e, nameof(EMPServerAPI) + "." + nameof(OnChargingErrorNotificationRequest));
                                                             }

                                                             #endregion

                                                             #region Call async subscribers

                                                             Acknowledgement<ChargingErrorNotificationRequest> acknowledgement = null;

                                                             if (OnChargingErrorNotification != null)
                                                                 acknowledgement = (await Task.WhenAll(OnChargingErrorNotification.GetInvocationList().
                                                                                                       Cast<OnChargingErrorNotificationDelegate>().
                                                                                                       Select(e => e(Timestamp.Now,
                                                                                                                     this,
                                                                                                                     chargingErrorNotificationRequest))).
                                                                                                       ConfigureAwait(false))?.FirstOrDefault();

                                                             if (acknowledgement == null)
                                                                 acknowledgement = Acknowledgement<ChargingErrorNotificationRequest>.SystemError(
                                                                                       chargingErrorNotificationRequest,
                                                                                       "Could not process the received ChargingErrorNotification request!"
                                                                                   );

                                                             #endregion

                                                             #region Send OnChargingErrorNotificationResponse event

                                                             try
                                                             {

                                                                 if (OnChargingErrorNotificationResponse != null)
                                                                     await Task.WhenAll(OnChargingErrorNotificationResponse.GetInvocationList().
                                                                                        Cast<OnChargingErrorNotificationResponseDelegate>().
                                                                                        Select(e => e(Timestamp.Now,
                                                                                                      this,
                                                                                                      acknowledgement,
                                                                                                      Timestamp.Now - startTime))).
                                                                                        ConfigureAwait(false);

                                                             }
                                                             catch (Exception e)
                                                             {
                                                                 DebugX.LogException(e, nameof(EMPServerAPI) + "." + nameof(OnChargingErrorNotificationResponse));
                                                             }

                                                             #endregion

                                                             acknowledgementJSON = acknowledgement.ToJSON(CustomAcknowledgementSerializer,
                                                                                                          CustomStatusCodeSerializer);

                                                         }

                                                     break;

                                                     #endregion

                                                     #region ...or default

                                                     default:
                                                         return new HTTPResponse.Builder(Request) {
                                                                    HTTPStatusCode             = HTTPStatusCode.BadRequest,
                                                                    Server                     = HTTPServer.DefaultServerName,
                                                                    Date                       = Timestamp.Now,
                                                                    AccessControlAllowOrigin   = "*",
                                                                    AccessControlAllowMethods  = "POST",
                                                                    AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                    ContentType                = HTTPContentType.JSON_UTF8,
                                                                    Content                    = Acknowledgement.DataError(
                                                                                                                     RequestTimestamp:          Request.Timestamp,
                                                                                                                     StatusCodeDescription:     "We could not handle the given charging notification request!",
                                                                                                                     StatusCodeAdditionalInfo:  errorResponse
                                                                                                                 ).
                                                                                                                 ToJSON(CustomAcknowledgementSerializer,
                                                                                                                        CustomStatusCodeSerializer).
                                                                                                                 ToString(JSONFormatting).
                                                                                                                 ToUTF8Bytes(),
                                                                    Connection                 = "close"
                                                                }.AsImmutable;

                                                     #endregion

                                                 }

                                                 return new HTTPResponse.Builder(Request) {
                                                            HTTPStatusCode             = HTTPStatusCode.OK,
                                                            Server                     = HTTPServer.DefaultServerName,
                                                            Date                       = Timestamp.Now,
                                                            AccessControlAllowOrigin   = "*",
                                                            AccessControlAllowMethods  = "POST",
                                                            AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                            ContentType                = HTTPContentType.JSON_UTF8,
                                                            Content                    = acknowledgementJSON.ToString(JSONFormatting).
                                                                                                             ToUTF8Bytes(),
                                                            Connection                 = "close"
                                                        }.AsImmutable;

                                             }
                                             catch (Exception e)
                                             {

                                                 return new HTTPResponse.Builder(Request) {
                                                            HTTPStatusCode             = HTTPStatusCode.BadRequest,
                                                            Server                     = HTTPServer.DefaultServerName,
                                                            Date                       = Timestamp.Now,
                                                            AccessControlAllowOrigin   = "*",
                                                            AccessControlAllowMethods  = "POST",
                                                            AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                            ContentType                = HTTPContentType.JSON_UTF8,
                                                            Content                    = Acknowledgement.DataError(
                                                                                                             RequestTimestamp:          Request.Timestamp,
                                                                                                             StatusCodeDescription:     e.Message,
                                                                                                             StatusCodeAdditionalInfo:  e.StackTrace
                                                                                                         ).
                                                                                                         ToJSON(CustomAcknowledgementSerializer,
                                                                                                                CustomStatusCodeSerializer).
                                                                                                         ToString(JSONFormatting).
                                                                                                         ToUTF8Bytes(),
                                                            Connection                 = "close"
                                                        }.AsImmutable;

                                             }

                                          }, AllowReplacement: URLReplacement.Allow);

            #endregion


            #region POST  ~/api/oicp/cdrmgmt/v22/operators/{operatorId}/charge-detail-record

            // ------------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/cdrmgmt/v22/operators/{operatorId}/charge-detail-record
            // ------------------------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/cdrmgmt/v22/operators/{operatorId}/charge-detail-record",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logChargeDetailRecordHTTPRequest,
                                         HTTPResponseLogger:  logChargeDetailRecordHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime = Timestamp.Now;
                                             Acknowledgement<ChargeDetailRecordRequest> chargeDetailRecordResponse = null;

                                             try
                                             {

                                                 #region Try to parse OperatorId URL parameter

                                                 if (Request.ParsedURLParameters.Length != 1 || !Operator_Id.TryParse(Request.ParsedURLParameters[0], out Operator_Id operatorId)) {

                                                     return new HTTPResponse.Builder(Request) {
                                                                HTTPStatusCode             = HTTPStatusCode.BadRequest,
                                                                Server                     = HTTPServer.DefaultServerName,
                                                                Date                       = Timestamp.Now,
                                                                AccessControlAllowOrigin   = "*",
                                                                AccessControlAllowMethods  = "POST",
                                                                AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                ContentType                = HTTPContentType.JSON_UTF8,
                                                                Content                    = Acknowledgement.DataError(
                                                                                                 StatusCodeDescription: "The expected 'operatorId' URL parameter could not be parsed!"
                                                                                             ).
                                                                                             ToJSON(CustomAcknowledgementSerializer,
                                                                                                    CustomStatusCodeSerializer).
                                                                                             ToString(JSONFormatting).
                                                                                             ToUTF8Bytes(),
                                                                Connection                 = "close"
                                                            }.AsImmutable;

                                                 }

                                                 #endregion

                                                 else if (ChargeDetailRecordRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                             operatorId,
                                                                                             Request.Timeout ?? DefaultRequestTimeout,
                                                                                             out ChargeDetailRecordRequest  chargeDetailRecordRequest,
                                                                                             out String                     errorResponse,
                                                                                             Request.Timestamp,
                                                                                             Request.EventTrackingId,
                                                                                             CustomChargeDetailRecordRequestParser))
                                                 {

                                                     Counter.ChargeDetailRecord.IncRequests();

                                                     #region  Send OnChargeDetailRecordRequest event

                                                     try
                                                     {

                                                         if (OnChargeDetailRecordRequest != null)
                                                             await Task.WhenAll(OnChargeDetailRecordRequest.GetInvocationList().
                                                                                Cast<OnChargeDetailRecordRequestDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              chargeDetailRecordRequest))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPServerAPI) + "." + nameof(OnChargeDetailRecordRequest));
                                                     }

                                                     #endregion

                                                     #region Call async subscribers

                                                     var OnChargeDetailRecordLocal = OnChargeDetailRecord;
                                                     if (OnChargeDetailRecordLocal != null)
                                                     {

                                                         try
                                                         {

                                                             chargeDetailRecordResponse = (await Task.WhenAll(OnChargeDetailRecordLocal.GetInvocationList().
                                                                                                                                            Cast<OnChargeDetailRecordDelegate>().
                                                                                                                                            Select(e => e(Timestamp.Now,
                                                                                                                                                          this,
                                                                                                                                                          chargeDetailRecordRequest))))?.FirstOrDefault();

                                                         }
                                                         catch (Exception e)
                                                         {
                                                             chargeDetailRecordResponse = Acknowledgement<ChargeDetailRecordRequest>.DataError(
                                                                                                  Request:                   chargeDetailRecordRequest,
                                                                                                  StatusCodeDescription:     e.Message,
                                                                                                  StatusCodeAdditionalInfo:  e.StackTrace,
                                                                                                  SessionId:                 chargeDetailRecordRequest.ChargeDetailRecord?.SessionId,
                                                                                                  CPOPartnerSessionId:       chargeDetailRecordRequest.ChargeDetailRecord?.CPOPartnerSessionId
                                                                                              );
                                                         }

                                                         if (chargeDetailRecordResponse == null)
                                                             chargeDetailRecordResponse = Acknowledgement<ChargeDetailRecordRequest>.SystemError(
                                                                                                  chargeDetailRecordRequest,
                                                                                                  "Could not process the received charge detail record!"
                                                                                              );

                                                     }

                                                     #endregion

                                                     #region Send OnChargeDetailRecordResponse event

                                                     try
                                                     {

                                                         if (OnChargeDetailRecordResponse != null)
                                                             await Task.WhenAll(OnChargeDetailRecordResponse.GetInvocationList().
                                                                                Cast<OnChargeDetailRecordResponseDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              chargeDetailRecordResponse,
                                                                                              Timestamp.Now - startTime))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPServerAPI) + "." + nameof(OnChargeDetailRecordResponse));
                                                     }

                                                     #endregion

                                                 }
                                                 else
                                                     chargeDetailRecordResponse = Acknowledgement<ChargeDetailRecordRequest>.DataError(
                                                                                      Request:                   chargeDetailRecordRequest,
                                                                                      StatusCodeDescription:     "We could not handle the given charge detail record!",
                                                                                      StatusCodeAdditionalInfo:  errorResponse,
                                                                                      SessionId:                 chargeDetailRecordRequest.ChargeDetailRecord?.SessionId,
                                                                                      CPOPartnerSessionId:       chargeDetailRecordRequest.ChargeDetailRecord?.CPOPartnerSessionId
                                                                                  );

                                             }
                                             catch (Exception e)
                                             {
                                                 chargeDetailRecordResponse = Acknowledgement<ChargeDetailRecordRequest>.SystemError(
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
                                                        Content                    = chargeDetailRecordResponse.ToJSON(CustomAcknowledgementSerializer,
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
