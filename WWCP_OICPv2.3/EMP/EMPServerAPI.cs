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

using System.Security.Authentication;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Logging;
using org.GraphDefined.Vanaheimr.Hermod.Sockets;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;
using org.GraphDefined.Vanaheimr.Hermod.HTTPTest;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.EMP
{

    /// <summary>
    /// The EMP HTTP Server API.
    /// </summary>
    public partial class EMPServerAPI : AOICPHTTPAPI
    {

        #region (class) APICounters

        public class APICounters(APICounterValues?  AuthorizeStart                 = null,
                                 APICounterValues?  AuthorizeStop                  = null,
                                 APICounterValues?  ChargingNotifications          = null,
                                 APICounterValues?  ChargingStartNotification      = null,
                                 APICounterValues?  ChargingProgressNotification   = null,
                                 APICounterValues?  ChargingEndNotification        = null,
                                 APICounterValues?  ChargingErrorNotification      = null,
                                 APICounterValues?  ChargeDetailRecord             = null)
        {

            public APICounterValues AuthorizeStart                  { get; } = AuthorizeStart               ?? new APICounterValues();
            public APICounterValues AuthorizeStop                   { get; } = AuthorizeStop                ?? new APICounterValues();
            public APICounterValues ChargingNotifications           { get; } = ChargingNotifications        ?? new APICounterValues();
            public APICounterValues ChargingStartNotification       { get; } = ChargingStartNotification    ?? new APICounterValues();
            public APICounterValues ChargingProgressNotification    { get; } = ChargingProgressNotification ?? new APICounterValues();
            public APICounterValues ChargingEndNotification         { get; } = ChargingEndNotification      ?? new APICounterValues();
            public APICounterValues ChargingErrorNotification       { get; } = ChargingErrorNotification    ?? new APICounterValues();
            public APICounterValues ChargeDetailRecord              { get; } = ChargeDetailRecord           ?? new APICounterValues();

            public JObject ToJSON()

                => JSONObject.Create(
                       new JProperty("AuthorizeStart",                AuthorizeStart.              ToJSON()),
                       new JProperty("AuthorizeStop",                 AuthorizeStop.               ToJSON()),
                       new JProperty("ChargingNotifications",         ChargingNotifications.       ToJSON()),
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
        public const String  DefaultHTTPServerName        = $"GraphDefined OICP {Version.String} EMP HTTP API";

        /// <summary>
        /// The default HTTP service name.
        /// </summary>
        public const String  DefaultHTTPServiceName       = $"GraphDefined OICP {Version.String} EMP HTTP API";

        /// <summary>
        /// The default logging context.
        /// </summary>
        public const String  DefaultLoggingContext        = "EMPServerAPI";


        public const String  DefaultHTTPAPI_LoggingPath   = "";
        public const String  DefaultHTTPAPI_LogfileName   = "";

        #endregion

        #region Properties

        /// <summary>
        /// The attached HTTP logger.
        /// </summary>
        public  HTTP_Logger?      HTTPLogger    { get; }
            //=> base.HTTPLogger as HTTP_Logger;

        /// <summary>
        /// The attached Server API logger.
        /// </summary>
        public  ServerAPILogger?  Logger        { get; }


        public  APICounters       Counters      { get; }

        #endregion

        #region Custom JSON parsers

        public CustomJObjectParserDelegate<AuthorizeStartRequest>?                CustomAuthorizeStartRequestParser                  { get; set; }
        public CustomJObjectSerializerDelegate<AuthorizationStartResponse>?       CustomAuthorizationStartSerializer                 { get; set; }

        public CustomJObjectParserDelegate<AuthorizeStopRequest>?                 CustomAuthorizeStopRequestParser                   { get; set; }
        public CustomJObjectSerializerDelegate<AuthorizationStopResponse>?        CustomAuthorizationStopSerializer                  { get; set; }


        public CustomJObjectParserDelegate<ChargingStartNotificationRequest>?     CustomChargingStartNotificationRequestParser       { get; set; }
        public CustomJObjectParserDelegate<ChargingProgressNotificationRequest>?  CustomChargingProgressNotificationRequestParser    { get; set; }
        public CustomJObjectParserDelegate<ChargingEndNotificationRequest>?       CustomChargingEndNotificationRequestParser         { get; set; }
        public CustomJObjectParserDelegate<ChargingErrorNotificationRequest>?     CustomChargingErrorNotificationRequestParser       { get; set; }


        public CustomJObjectParserDelegate<ChargeDetailRecordRequest>?            CustomChargeDetailRecordRequestParser              { get; set; }

        #endregion

        #region Custom JSON serializers
        public CustomJObjectSerializerDelegate<Acknowledgement>?                  CustomAcknowledgementSerializer                    { get; set; }

        public CustomJObjectSerializerDelegate<StatusCode>?                       CustomStatusCodeSerializer                         { get; set; }


        public CustomJObjectSerializerDelegate<Identification>?                   CustomIdentificationSerializer                     { get; set; }

        #endregion

        #region Custom request/response logging converters

        #region AuthorizeStart              (Request/Response)Converter

        public Func<DateTimeOffset, Object, AuthorizeStartRequest, String>
            AuthorizeStartRequestConverter                     { get; set; }

            = (timestamp, sender, authorizeStartRequest)
            => String.Concat(authorizeStartRequest.Identification.ToString(), " at ", authorizeStartRequest.EVSEId);

        public Func<DateTimeOffset, Object, AuthorizeStartRequest, AuthorizationStartResponse, TimeSpan, String>
            AuthorizeStartResponseConverter                    { get; set; }

            = (timestamp, sender, authorizeStartRequest, authorizationStartResponse, runtime)
            => String.Concat(authorizeStartRequest.Identification.ToString(), " at ", authorizeStartRequest.EVSEId,
                             " => ",
                             authorizationStartResponse.StatusCode.ToString() ?? "failed!");

        #endregion

        #region AuthorizeStop               (Request/Response)Converter

        public Func<DateTimeOffset, Object, AuthorizeStopRequest, String>
            AuthorizeStopRequestConverter                     { get; set; }

            = (timestamp, sender, authorizeStopRequest)
            => String.Concat(authorizeStopRequest.Identification.ToString(), " at ", authorizeStopRequest.EVSEId);

        public Func<DateTimeOffset, Object, AuthorizeStopRequest, AuthorizationStopResponse, TimeSpan, String>
            AuthorizeStopResponseConverter                    { get; set; }

            = (timestamp, sender, authorizeStopRequest, authorizationStopResponse, runtime)
            => String.Concat(authorizeStopRequest.Identification.ToString(), " at ", authorizeStopRequest.EVSEId,
                             " => ",
                             authorizationStopResponse.StatusCode.ToString() ?? "failed!");

        #endregion


        #region ChargingStartNotification   (Request/Response)Converter

        public Func<DateTimeOffset, Object, ChargingStartNotificationRequest, String>
            ChargingStartNotificationRequestConverter                     { get; set; }

            = (timestamp, sender, chargingStartNotificationRequest)
            => String.Concat(chargingStartNotificationRequest.Identification.ToString(), " at ", chargingStartNotificationRequest.EVSEId);

        public Func<DateTimeOffset, Object, ChargingStartNotificationRequest, Acknowledgement<ChargingStartNotificationRequest>, TimeSpan, String>
            ChargingStartNotificationResponseConverter                    { get; set; }

            = (timestamp, sender, chargingStartNotificationRequest, chargingStartNotificationResponse, runtime)
            => String.Concat(chargingStartNotificationRequest.Identification.ToString(), " at ", chargingStartNotificationRequest.EVSEId,
                             " => ",
                             chargingStartNotificationResponse.StatusCode.ToString() ?? "failed!");

        #endregion

        #region ChargingProgressNotification(Request/Response)Converter

        public Func<DateTimeOffset, Object, ChargingProgressNotificationRequest, String>
            ChargingProgressNotificationRequestConverter                     { get; set; }

            = (timestamp, sender, chargingProgressNotificationRequest)
            => String.Concat(chargingProgressNotificationRequest.Identification.ToString(), " at ", chargingProgressNotificationRequest.EVSEId);

        public Func<DateTimeOffset, Object, ChargingProgressNotificationRequest, Acknowledgement<ChargingProgressNotificationRequest>, TimeSpan, String>
            ChargingProgressNotificationResponseConverter                    { get; set; }

            = (timestamp, sender, chargingProgressNotificationRequest, chargingProgressNotificationResponse, runtime)
            => String.Concat(chargingProgressNotificationRequest.Identification.ToString(), " at ", chargingProgressNotificationRequest.EVSEId,
                             " => ",
                             chargingProgressNotificationResponse.StatusCode.ToString() ?? "failed!");

        #endregion

        #region ChargingEndNotification     (Request/Response)Converter

        public Func<DateTimeOffset, Object, ChargingEndNotificationRequest, String>
            ChargingEndNotificationRequestConverter                     { get; set; }

            = (timestamp, sender, chargingEndNotificationRequest)
            => String.Concat(chargingEndNotificationRequest.Identification.ToString(), " at ", chargingEndNotificationRequest.EVSEId);

        public Func<DateTimeOffset, Object, ChargingEndNotificationRequest, Acknowledgement<ChargingEndNotificationRequest>, TimeSpan, String>
            ChargingEndNotificationResponseConverter                    { get; set; }

            = (timestamp, sender, chargingEndNotificationRequest, chargingEndNotificationResponse, runtime)
            => String.Concat(chargingEndNotificationRequest.Identification.ToString(), " at ", chargingEndNotificationRequest.EVSEId,
                             " => ",
                             chargingEndNotificationResponse.StatusCode.ToString() ?? "failed!");

        #endregion

        #region ChargingErrorNotification   (Request/Response)Converter

        public Func<DateTimeOffset, Object, ChargingErrorNotificationRequest, String>
            ChargingErrorNotificationRequestConverter                     { get; set; }

            = (timestamp, sender, chargingErrorNotificationRequest)
            => String.Concat(chargingErrorNotificationRequest.Identification.ToString(), " at ", chargingErrorNotificationRequest.EVSEId);

        public Func<DateTimeOffset, Object, ChargingErrorNotificationRequest, Acknowledgement<ChargingErrorNotificationRequest>, TimeSpan, String>
            ChargingErrorNotificationResponseConverter                    { get; set; }

            = (timestamp, sender, chargingErrorNotificationRequest, chargingErrorNotificationResponse, runtime)
            => String.Concat(chargingErrorNotificationRequest.Identification.ToString(), " at ", chargingErrorNotificationRequest.EVSEId,
                             " => ",
                             chargingErrorNotificationResponse.StatusCode.ToString() ?? "failed!");

        #endregion


        #region ChargeDetailRecord          (Request/Response)Converter

        public Func<DateTimeOffset, Object, ChargeDetailRecordRequest, String>
            ChargeDetailRecordRequestConverter                     { get; set; }

            = (timestamp, sender, chargeDetailRecordRequest)
            => String.Concat(chargeDetailRecordRequest.ChargeDetailRecord.Identification.ToString(), " at ", chargeDetailRecordRequest.ChargeDetailRecord.EVSEId);

        public Func<DateTimeOffset, Object, ChargeDetailRecordRequest, Acknowledgement<ChargeDetailRecordRequest>, TimeSpan, String>
            ChargeDetailRecordResponseConverter                    { get; set; }

            = (timestamp, sender, chargeDetailRecordRequest, chargeDetailRecordResponse, runtime)
            => String.Concat(chargeDetailRecordRequest.ChargeDetailRecord.Identification.ToString(), " at ", chargeDetailRecordRequest.ChargeDetailRecord.EVSEId,
                             " => ",
                             chargeDetailRecordResponse.StatusCode.ToString() ?? "failed!");

        #endregion

        #endregion

        #region Events

        #region (protected internal) OnAuthorizeStartHTTPRequest

        /// <summary>
        /// An event sent whenever an AuthorizeStart HTTP request was received.
        /// </summary>
        public HTTPRequestLogEventX OnAuthorizeStartHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an AuthorizeStart HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Server HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logAuthorizeStartHTTPRequest(DateTimeOffset     Timestamp,
                                                             HTTPAPIX           API,
                                                             HTTPRequest        Request,
                                                             CancellationToken  CancellationToken)

            => OnAuthorizeStartHTTPRequest.WhenAll(
                   Timestamp,
                   API,
                   Request,
                   CancellationToken
               );

        #endregion

        #region (event)              OnAuthorizeStart(Request-/Response)

        /// <summary>
        /// An event send whenever an AuthorizeStart request was received.
        /// </summary>
        public event OnAuthorizeStartRequestDelegate?   OnAuthorizeStartRequest;

        /// <summary>
        /// An event send whenever an AuthorizeStart request was received.
        /// </summary>
        public event OnAuthorizeStartDelegate?          OnAuthorizeStart;

        /// <summary>
        /// An event send whenever a response to an AuthorizeStart request was sent.
        /// </summary>
        public event OnAuthorizeStartResponseDelegate?  OnAuthorizeStartResponse;

        #endregion

        #region (protected internal) OnAuthorizationStartHTTPResponse

        /// <summary>
        /// An event sent whenever an AuthorizationStart HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEventX OnAuthorizationStartHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an AuthorizationStart HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Server HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logAuthorizationStartHTTPResponse(DateTimeOffset     Timestamp,
                                                                  HTTPAPIX           API,
                                                                  HTTPRequest        Request,
                                                                  HTTPResponse       Response,
                                                                  CancellationToken  CancellationToken)

            => OnAuthorizationStartHTTPResponse.WhenAll(
                   Timestamp,
                   API,
                   Request,
                   Response,
                   CancellationToken
               );

        #endregion


        #region (protected internal) OnAuthorizeStopHTTPRequest

        /// <summary>
        /// An event sent whenever an AuthorizeStop HTTP request was received.
        /// </summary>
        public HTTPRequestLogEventX OnAuthorizeStopHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an AuthorizeStop HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Server HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logAuthorizeStopHTTPRequest(DateTimeOffset     Timestamp,
                                                            HTTPAPIX           API,
                                                            HTTPRequest        Request,
                                                            CancellationToken  CancellationToken)

            => OnAuthorizeStopHTTPRequest.WhenAll(
                   Timestamp,
                   API,
                   Request,
                   CancellationToken
               );

        #endregion

        #region (event)              OnAuthorizeStop(Request-/Response)

        /// <summary>
        /// An event send whenever an AuthorizeStop request was received.
        /// </summary>
        public event OnAuthorizeStopRequestDelegate?   OnAuthorizeStopRequest;

        /// <summary>
        /// An event send whenever an AuthorizeStop request was received.
        /// </summary>
        public event OnAuthorizeStopDelegate?          OnAuthorizeStop;

        /// <summary>
        /// An event send whenever a response to an AuthorizeStop request was sent.
        /// </summary>
        public event OnAuthorizeStopResponseDelegate?  OnAuthorizeStopResponse;

        #endregion

        #region (protected internal) OnAuthorizationStopHTTPResponse

        /// <summary>
        /// An event sent whenever an AuthorizationStop HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEventX OnAuthorizationStopHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an AuthorizationStop HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Server HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logAuthorizationStopHTTPResponse(DateTimeOffset     Timestamp,
                                                                 HTTPAPIX           API,
                                                                 HTTPRequest        Request,
                                                                 HTTPResponse       Response,
                                                                 CancellationToken  CancellationToken)

            => OnAuthorizationStopHTTPResponse.WhenAll(
                   Timestamp,
                   API,
                   Request,
                   Response,
                   CancellationToken
               );

        #endregion



        #region (protected internal) OnChargingNotificationsHTTPRequest

        /// <summary>
        /// An event sent whenever a ChargingNotification HTTP request was received.
        /// </summary>
        public HTTPRequestLogEventX OnChargingNotificationsHTTPRequest = new();

        /// <summary>
        /// An event sent whenever a ChargingNotification HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the notification.</param>
        /// <param name="API">The EMP Server HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logChargingNotificationsHTTPRequest(DateTimeOffset     Timestamp,
                                                                    HTTPAPIX           API,
                                                                    HTTPRequest        Request,
                                                                    CancellationToken  CancellationToken)

            => OnChargingNotificationsHTTPRequest.WhenAll(
                   Timestamp,
                   API,
                   Request,
                   CancellationToken
               );

        #endregion

        #region (event)              OnCharging(...)Notification(Request-/Response)

        public event OnChargingStartNotificationRequestDelegate?      OnChargingStartNotificationRequest;
        public event OnChargingStartNotificationDelegate?             OnChargingStartNotification;
        public event OnChargingStartNotificationResponseDelegate?     OnChargingStartNotificationResponse;

        public event OnChargingProgressNotificationRequestDelegate?   OnChargingProgressNotificationRequest;
        public event OnChargingProgressNotificationDelegate?          OnChargingProgressNotification;
        public event OnChargingProgressNotificationResponseDelegate?  OnChargingProgressNotificationResponse;

        public event OnChargingEndNotificationRequestDelegate?        OnChargingEndNotificationRequest;
        public event OnChargingEndNotificationDelegate?               OnChargingEndNotification;
        public event OnChargingEndNotificationResponseDelegate?       OnChargingEndNotificationResponse;

        public event OnChargingErrorNotificationRequestDelegate?      OnChargingErrorNotificationRequest;
        public event OnChargingErrorNotificationDelegate?             OnChargingErrorNotification;
        public event OnChargingErrorNotificationResponseDelegate?     OnChargingErrorNotificationResponse;

        #endregion

        #region (protected internal) OnChargingNotificationsHTTPResponse

        /// <summary>
        /// An event sent whenever a ChargingNotification HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEventX OnChargingNotificationsHTTPResponse = new();

        /// <summary>
        /// An event sent whenever a ChargingNotification HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the notification.</param>
        /// <param name="API">The EMP Server HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logChargingNotificationsHTTPResponse(DateTimeOffset     Timestamp,
                                                                     HTTPAPIX           API,
                                                                     HTTPRequest        Request,
                                                                     HTTPResponse       Response,
                                                                     CancellationToken  CancellationToken)

            => OnChargingNotificationsHTTPResponse.WhenAll(
                   Timestamp,
                   API,
                   Request,
                   Response,
                   CancellationToken
               );

        #endregion



        #region (protected internal) OnChargeDetailRecordHTTPRequest

        /// <summary>
        /// An event sent whenever a ChargeDetailRecord HTTP request was received.
        /// </summary>
        public HTTPRequestLogEventX OnChargeDetailRecordHTTPRequest = new();

        /// <summary>
        /// An event sent whenever a ChargeDetailRecord HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Server HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logChargeDetailRecordHTTPRequest(DateTimeOffset     Timestamp,
                                                                 HTTPAPIX           API,
                                                                 HTTPRequest        Request,
                                                                 CancellationToken  CancellationToken)

            => OnChargeDetailRecordHTTPRequest.WhenAll(
                   Timestamp,
                   API,
                   Request,
                   CancellationToken
               );

        #endregion

        #region (event)              OnChargeDetailRecord(Request-/Response)

        /// <summary>
        /// An event send whenever a charge detail record was received.
        /// </summary>
        public event OnChargeDetailRecordRequestDelegate?   OnChargeDetailRecordRequest;

        /// <summary>
        /// An event send whenever a charge detail record was received.
        /// </summary>
        public event OnChargeDetailRecordDelegate?          OnChargeDetailRecord;

        /// <summary>
        /// An event send whenever a response to a received charge detail record was sent.
        /// </summary>
        public event OnChargeDetailRecordResponseDelegate?  OnChargeDetailRecordResponse;

        #endregion

        #region (protected internal) OnChargeDetailRecordHTTPResponse

        /// <summary>
        /// An event sent whenever a ChargeDetailRecord HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEventX OnChargeDetailRecordHTTPResponse = new();

        /// <summary>
        /// An event sent whenever a ChargeDetailRecord HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Server HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logChargeDetailRecordHTTPResponse(DateTimeOffset     Timestamp,
                                                                  HTTPAPIX           API,
                                                                  HTTPRequest        Request,
                                                                  HTTPResponse       Response,
                                                                  CancellationToken  CancellationToken)

            => OnChargeDetailRecordHTTPResponse.WhenAll(
                   Timestamp,
                   API,
                   Request,
                   Response,
                   CancellationToken
               );

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EMP HTTP Server API.
        /// </summary>
        public EMPServerAPI(HTTPTestServerX                HTTPTestServer,
                            IEnumerable<HTTPHostname>?     Hostnames                 = null,
                            HTTPPath?                      RootPath                  = null,
                            IEnumerable<HTTPContentType>?  HTTPContentTypes          = null,
                            I18NString?                    Description               = null,

                            String?                        ExternalDNSName           = null,
                            HTTPPath?                      BasePath                  = null,

                            String?                        HTTPServerName            = DefaultHTTPServerName,
                            String?                        HTTPServiceName           = DefaultHTTPServiceName,
                            String?                        APIVersionHash            = null,
                            JObject?                       APIVersionHashes          = null,

                            Boolean                        RegisterRootService       = true,
                            HTTPPath?                      URLPathPrefix             = null,
                            Formatting?                    JSONFormatting            = null,
                            ConnectionType?                Connection                = null,

                            Boolean?                       IsDevelopment             = null,
                            IEnumerable<String>?           DevelopmentServers        = null,
                            Boolean                        DisableLogging            = false,
                            String                         LoggingPath               = DefaultHTTPAPI_LoggingPath,
                            String                         LoggingContext            = DefaultLoggingContext,
                            String                         LogfileName               = DefaultHTTPAPI_LogfileName,
                            LogfileCreatorDelegate?        LogfileCreator            = null)

            : base(HTTPTestServer,
                   Hostnames,
                   RootPath,
                   HTTPContentTypes,
                   Description,

                   ExternalDNSName,
                   BasePath,

                   HTTPServerName  ?? DefaultHTTPServerName,
                   HTTPServiceName ?? DefaultHTTPServiceName,
                   APIVersionHash,
                   APIVersionHashes,

                   URLPathPrefix,
                   JSONFormatting,
                   Connection,

                   IsDevelopment,
                   DevelopmentServers,
                   DisableLogging,
                   LoggingPath,
                   LoggingContext,
                   LogfileName,
                   LogfileCreator)

        {

            this.Counters    = new APICounters();

                 HTTPLogger  = this.DisableLogging == false
                                   ? new HTTP_Logger(
                                         this,
                                         LoggingPath,
                                         LoggingContext ?? DefaultLoggingContext,
                                         LogfileCreator
                                     )
                                   : null;

            this.Logger      = this.DisableLogging == false
                                   ? new ServerAPILogger(
                                         this,
                                         LoggingPath,
                                         LoggingContext ?? DefaultLoggingContext,
                                         LogfileCreator
                                     )
                                   : null;


            #region Register root service: / (HTTPRoot)

            if (RegisterRootService)
                AddHandler(
                    HTTPPath.Root,
                    HTTPMethod:    HTTPMethod.GET,
                    HTTPDelegate:  request => {
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode  = HTTPStatusCode.OK,
                                Server          = HTTPServerName,
                                Date            = Timestamp.Now,
                                ContentType     = HTTPContentType.Text.PLAIN,
                                Content         = $"This is an OICP {Version.String} EMP Server HTTP/JSON endpoint!".ToUTF8Bytes(),
                                CacheControl    = "public, max-age=300",
                                Connection      = this.Connection
                            }.AsImmutable);
                    },
                    AllowReplacement: URLReplacement.Allow
                );

            if (RegisterRootService)
                AddHandler(
                    HTTPPath.Parse("/{FileName}"),
                    HTTPMethod:    HTTPMethod.GET,
                    HTTPDelegate:  request => {
                        return Task.FromResult(
                            new HTTPResponse.Builder(request) {
                                HTTPStatusCode  = HTTPStatusCode.OK,
                                Server          = HTTPServerName,
                                Date            = Timestamp.Now,
                                ContentType     = HTTPContentType.Text.PLAIN,
                                Content         = $"This is an OICP {Version.String} CPO Server HTTP/JSON endpoint!".ToUTF8Bytes(),
                                CacheControl    = "public, max-age=300",
                                Connection      = this.Connection
                            }.AsImmutable);
                    },
                    AllowReplacement: URLReplacement.Allow
                );

            #endregion

            #region RegisterURLTemplates

            //Note: OperatorId is the remote EMP sending an authorize start/stop request!

            #region POST  ~/api/oicp/charging/v21/operators/{operatorId}/authorize/start

            // --------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/charging/v21/operators/DE*GDF/authorize/start
            // --------------------------------------------------------------------------------------------------------------------------------------
            AddHandler(
                HTTPMethod.POST,
                URLPathPrefix + HTTPPath.Parse($"/api/oicp/charging/v21/operators/{{{OICPExtensions.OperatorId}}}/authorize/start"),
                HTTPContentType.Application.JSON_UTF8,
                HTTPRequestLogger:   logAuthorizeStartHTTPRequest,
                HTTPResponseLogger:  logAuthorizationStartHTTPResponse,
                HTTPDelegate:        async request => {

                    var startTime  = Timestamp.Now;
                    var processId  = request.TryParseProcessId();

                    AuthorizationStartResponse? authorizationStartResponse = null;

                    try
                    {

                        #region Try to parse OperatorId URL parameter

                        if (!request.TryParseOperatorId(out var operatorId))
                        {

                            Counters.AuthorizeStart.IncRequests_Error();

                            authorizationStartResponse = AuthorizationStartResponse.SystemError(
                                                             StatusCodeDescription: "The expected 'operatorId' URL parameter could not be parsed!"
                                                         );

                        }

                        #endregion

                        else if (AuthorizeStartRequest.TryParse(JObject.Parse(request.HTTPBody?.ToUTF8String() ?? ""),
                                                                operatorId,
                                                                out var authorizeStartRequest,
                                                                out var errorResponse,
                                                                processId,
                                                                request.Timestamp,
                                                                request.EventTrackingId,
                                                                request.Timeout ?? DefaultRequestTimeout,
                                                                CustomAuthorizeStartRequestParser,
                                                                request.CancellationToken))
                        {

                            Counters.AuthorizeStart.IncRequests_OK();

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

                                    Counters.AuthorizeStart.IncResponses_OK();

                                }
                                catch (Exception e)
                                {

                                    Counters.AuthorizeStart.IncResponses_Error();

                                    authorizationStartResponse = AuthorizationStartResponse.DataError(
                                                                     Request:                   authorizeStartRequest,
                                                                     StatusCodeDescription:     e.Message,
                                                                     StatusCodeAdditionalInfo:  e.StackTrace,
                                                                     SessionId:                 authorizeStartRequest?.SessionId,
                                                                     CPOPartnerSessionId:       authorizeStartRequest?.CPOPartnerSessionId
                                                                 );

                                }
                            }

                            if (authorizationStartResponse is null)
                            {

                                Counters.AuthorizeStart.IncResponses_Error();

                                authorizationStartResponse = AuthorizationStartResponse.SystemError(
                                                                 authorizeStartRequest!,
                                                                 "Could not process the received AuthorizeStart request!"
                                                             );

                            }

                            #endregion

                            #region Send OnAuthorizeStartResponse event

                            try
                            {

                                if (OnAuthorizeStartResponse is not null)
                                    await Task.WhenAll(OnAuthorizeStartResponse.GetInvocationList().
                                                       Cast<OnAuthorizeStartResponseDelegate>().
                                                       Select(e => e(Timestamp.Now,
                                                                     this,
                                                                     authorizeStartRequest!,
                                                                     authorizationStartResponse,
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
                        {

                            Counters.AuthorizeStart.IncRequests_Error();

                            authorizationStartResponse = AuthorizationStartResponse.DataError(
                                                             Request:                   authorizeStartRequest,
                                                             StatusCodeDescription:     "We could not parse the given AuthorizeStart request!",
                                                             StatusCodeAdditionalInfo:  errorResponse,
                                                             SessionId:                 authorizeStartRequest?.SessionId,
                                                             CPOPartnerSessionId:       authorizeStartRequest?.CPOPartnerSessionId
                                                         );

                        }

                    }
                    catch (Exception e)
                    {

                        Counters.AuthorizeStart.IncResponses_Error();

                        authorizationStartResponse = AuthorizationStartResponse.SystemError(
                                                         Request:                   null,
                                                         StatusCodeDescription:     e.Message,
                                                         StatusCodeAdditionalInfo:  e.StackTrace
                                                     );

                    }


                    return new HTTPResponse.Builder(request) {
                               HTTPStatusCode             = HTTPStatusCode.OK,
                               Server                     = HTTPServerName,
                               Date                       = Timestamp.Now,
                               AccessControlAllowOrigin   = "*",
                               AccessControlAllowMethods  = [ "POST" ],
                               AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                               ContentType                = HTTPContentType.Application.JSON_UTF8,
                               Content                    = authorizationStartResponse.ToJSON(CustomAuthorizationStartSerializer,
                                                                                              CustomStatusCodeSerializer).
                                                                                       ToString(this.JSONFormatting).
                                                                                       ToUTF8Bytes(),
                               Connection                 = ConnectionType.Close
                           }.AsImmutable;

                }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region POST  ~/api/oicp/charging/v21/operators/{operatorId}/authorize/stop

            // -------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/charging/v21/operators/DE*GDF/authorize/stop
            // -------------------------------------------------------------------------------------------------------------------------------------
            AddHandler(
                HTTPMethod.POST,
                URLPathPrefix + HTTPPath.Parse($"/api/oicp/charging/v21/operators/{{{OICPExtensions.OperatorId}}}/authorize/stop"),
                HTTPContentType.Application.JSON_UTF8,
                HTTPRequestLogger:   logAuthorizeStopHTTPRequest,
                HTTPResponseLogger:  logAuthorizationStopHTTPResponse,
                HTTPDelegate:        async request => {

                    var startTime  = Timestamp.Now;
                    var processId  = request.TryParseProcessId();

                    AuthorizationStopResponse? authorizationStopResponse = null;

                    try
                    {

                        #region Try to parse OperatorId URL parameter

                        if (!request.TryParseOperatorId(out var operatorId))
                        {

                            Counters.AuthorizeStop.IncRequests_Error();

                            authorizationStopResponse = AuthorizationStopResponse.SystemError(
                                                            StatusCodeDescription: "The expected 'operatorId' URL parameter could not be parsed!"
                                                        );

                        }

                        #endregion

                        else if (AuthorizeStopRequest.TryParse(JObject.Parse(request.HTTPBody?.ToUTF8String() ?? ""),
                                                               operatorId,
                                                               out var authorizeStopRequest,
                                                               out var errorResponse,
                                                               processId,
                                                               request.Timestamp,
                                                               request.EventTrackingId,
                                                               request.Timeout ?? DefaultRequestTimeout,
                                                               CustomAuthorizeStopRequestParser,
                                                               request.CancellationToken))
                        {

                            Counters.AuthorizeStop.IncRequests_OK();

                            #region Send OnAuthorizeStopRequest event

                            try
                            {

                                if (OnAuthorizeStopRequest is not null)
                                    await Task.WhenAll(OnAuthorizeStopRequest.GetInvocationList().
                                                       Cast<OnAuthorizeStopRequestDelegate>().
                                                       Select(e => e(Timestamp.Now,
                                                                     this,
                                                                     authorizeStopRequest!))).
                                                       ConfigureAwait(false);

                            }
                            catch (Exception e)
                            {
                                DebugX.LogException(e, nameof(EMPServerAPI) + "." + nameof(OnAuthorizeStopRequest));
                            }

                            #endregion

                            #region Call async subscribers

                            var OnAuthorizeStopLocal = OnAuthorizeStop;
                            if (OnAuthorizeStopLocal is not null)
                            {
                                try
                                {

                                    authorizationStopResponse = (await Task.WhenAll(OnAuthorizeStopLocal.GetInvocationList().
                                                                                                         Cast<OnAuthorizeStopDelegate>().
                                                                                                         Select(e => e(Timestamp.Now,
                                                                                                                       this,
                                                                                                                       authorizeStopRequest!))))?.FirstOrDefault();

                                    Counters.AuthorizeStop.IncResponses_OK();

                                }
                                catch (Exception e)
                                {

                                    Counters.AuthorizeStop.IncResponses_Error();

                                    authorizationStopResponse = AuthorizationStopResponse.DataError(
                                                                    Request:                   authorizeStopRequest,
                                                                    StatusCodeDescription:     e.Message,
                                                                    StatusCodeAdditionalInfo:  e.StackTrace,
                                                                    SessionId:                 authorizeStopRequest?.SessionId,
                                                                    CPOPartnerSessionId:       authorizeStopRequest?.CPOPartnerSessionId
                                                                );

                                }
                            }

                            if (authorizationStopResponse is null)
                            {

                                Counters.AuthorizeStop.IncResponses_Error();

                                authorizationStopResponse = AuthorizationStopResponse.SystemError(
                                                                authorizeStopRequest,
                                                                "Could not process the received AuthorizeStop request!"
                                                            );

                            }

                            #endregion

                            #region Send OnAuthorizeStopResponse event

                            try
                            {

                                if (OnAuthorizeStopResponse is not null)
                                    await Task.WhenAll(OnAuthorizeStopResponse.GetInvocationList().
                                                       Cast<OnAuthorizeStopResponseDelegate>().
                                                       Select(e => e(Timestamp.Now,
                                                                     this,
                                                                     authorizeStopRequest!,
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
                        {

                            Counters.AuthorizeStop.IncRequests_Error();

                            authorizationStopResponse = AuthorizationStopResponse.DataError(
                                                            Request:                   authorizeStopRequest,
                                                            StatusCodeDescription:     "We could not handle the given AuthorizeStop request!",
                                                            StatusCodeAdditionalInfo:  errorResponse,
                                                            SessionId:                 authorizeStopRequest?.SessionId,
                                                            CPOPartnerSessionId:       authorizeStopRequest?.CPOPartnerSessionId
                                                        );

                        }

                    }
                    catch (Exception e)
                    {

                        Counters.AuthorizeStop.IncResponses_Error();

                        authorizationStopResponse = AuthorizationStopResponse.SystemError(
                                                        Request:                   null,
                                                        StatusCodeDescription:     e.Message,
                                                        StatusCodeAdditionalInfo:  e.StackTrace
                                                    );

                    }


                    return new HTTPResponse.Builder(request) {
                               HTTPStatusCode             = HTTPStatusCode.OK,
                               Server                     = HTTPServerName,
                               Date                       = Timestamp.Now,
                               AccessControlAllowOrigin   = "*",
                               AccessControlAllowMethods  = [ "POST" ],
                               AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                               ContentType                = HTTPContentType.Application.JSON_UTF8,
                               Content                    = authorizationStopResponse.ToJSON(CustomAuthorizationStopSerializer,
                                                                                             CustomStatusCodeSerializer).
                                                                                      ToString(this.JSONFormatting).
                                                                                      ToUTF8Bytes(),
                               Connection                 = ConnectionType.Close
                           }.AsImmutable;

                }, AllowReplacement: URLReplacement.Allow);

            #endregion


            #region POST  ~/api/oicp/notificationmgmt/v11/charging-notifications

            // ------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/notificationmgmt/v11/charging-notifications
            // ------------------------------------------------------------------------------------------------------------------------------------
            AddHandler(
                HTTPMethod.POST,
                URLPathPrefix + HTTPPath.Parse("/api/oicp/notificationmgmt/v11/charging-notifications"),
                HTTPContentType.Application.JSON_UTF8,
                HTTPRequestLogger:   logChargingNotificationsHTTPRequest,
                HTTPResponseLogger:  logChargingNotificationsHTTPResponse,
                HTTPDelegate:        async request => {

                    var startTime  = Timestamp.Now;
                    var processId  = request.TryParseProcessId();

                    Acknowledgement? chargingNotificationsResponse = null;

                    try
                    {

                        if (request.TryParseJSONObjectRequestBody(out var JSONRequest, out _) &&
                            JSONRequest.ParseMandatory("Type",
                                                       "charging notification type",
                                                       ChargingNotificationTypesExtensions.TryParse,
                                                       out ChargingNotificationTypes chargingNotificationType,
                                                       out _))
                        {

                            Counters.ChargingNotifications.IncRequests_OK();

                            switch (chargingNotificationType)
                            {

                                #region Start

                                case ChargingNotificationTypes.Start:

                                    if (ChargingStartNotificationRequest.TryParse(JSONRequest,
                                                                                  out var chargingStartNotificationRequest,
                                                                                  out var errorResponse,
                                                                                  processId,
                                                                                  request.Timestamp,
                                                                                  request.EventTrackingId,
                                                                                  request.Timeout ?? DefaultRequestTimeout,
                                                                                  CustomChargingStartNotificationRequestParser,
                                                                                  request.CancellationToken))
                                    {

                                        Counters.ChargingStartNotification.IncRequests_OK();

                                        #region Send OnChargingStartNotificationRequest event

                                        try
                                        {

                                            if (OnChargingStartNotificationRequest is not null)
                                                await Task.WhenAll(OnChargingStartNotificationRequest.GetInvocationList().
                                                                   Cast<OnChargingStartNotificationRequestDelegate>().
                                                                   Select(e => e(Timestamp.Now,
                                                                                 this,
                                                                                 chargingStartNotificationRequest!))).
                                                                   ConfigureAwait(false);

                                        }
                                        catch (Exception e)
                                        {
                                            DebugX.LogException(e, nameof(EMPServerAPI) + "." + nameof(OnChargingStartNotificationRequest));
                                        }

                                        #endregion

                                        #region Call async subscribers

                                        Acknowledgement<ChargingStartNotificationRequest>? chargingStartNotificationResponse = null;

                                        var OnChargingStartNotificationLocal = OnChargingStartNotification;
                                        if (OnChargingStartNotificationLocal is not null)
                                        {
                                            try
                                            {

                                                chargingStartNotificationResponse = (await Task.WhenAll(OnChargingStartNotificationLocal.GetInvocationList().
                                                                                                                                         Cast<OnChargingStartNotificationDelegate>().
                                                                                                                                         Select(e => e(Timestamp.Now,
                                                                                                                                                       this,
                                                                                                                                                       chargingStartNotificationRequest!))).
                                                                                                                                         ConfigureAwait(false))?.FirstOrDefault();

                                                Counters.ChargingNotifications.    IncResponses_OK();
                                                Counters.ChargingStartNotification.IncResponses_OK();

                                            }
                                            catch (Exception e)
                                            {

                                                Counters.ChargingNotifications.    IncResponses_Error();
                                                Counters.ChargingStartNotification.IncResponses_Error();

                                                chargingStartNotificationResponse = Acknowledgement<ChargingStartNotificationRequest>.DataError(
                                                                                        Request:                   chargingStartNotificationRequest,
                                                                                        StatusCodeDescription:     e.Message,
                                                                                        StatusCodeAdditionalInfo:  e.StackTrace,
                                                                                        SessionId:                 chargingStartNotificationRequest?.SessionId,
                                                                                        CPOPartnerSessionId:       chargingStartNotificationRequest?.CPOPartnerSessionId
                                                                                    );

                                            }
                                        }

                                        if (chargingStartNotificationResponse is null)
                                        {

                                            Counters.ChargingNotifications.    IncResponses_Error();
                                            Counters.ChargingStartNotification.IncResponses_Error();

                                            chargingStartNotificationResponse = Acknowledgement<ChargingStartNotificationRequest>.SystemError(
                                                                                    chargingStartNotificationRequest,
                                                                                    "Could not process the received ChargingStartNotification request!"
                                                                                );

                                        }

                                        #endregion

                                        #region Send OnChargingStartNotificationResponse event

                                        try
                                        {

                                            if (OnChargingStartNotificationResponse is not null)
                                                await Task.WhenAll(OnChargingStartNotificationResponse.GetInvocationList().
                                                                   Cast<OnChargingStartNotificationResponseDelegate>().
                                                                   Select(e => e(Timestamp.Now,
                                                                                 this,
                                                                                 chargingStartNotificationRequest!,
                                                                                 chargingStartNotificationResponse,
                                                                                 Timestamp.Now - startTime))).
                                                                   ConfigureAwait(false);

                                        }
                                        catch (Exception e)
                                        {
                                            DebugX.LogException(e, nameof(EMPServerAPI) + "." + nameof(OnChargingStartNotificationResponse));
                                        }

                                        #endregion

                                        chargingNotificationsResponse = chargingStartNotificationResponse;

                                    }
                                    else
                                    {

                                        Counters.ChargingNotifications.    IncResponses_Error();
                                        Counters.ChargingStartNotification.IncResponses_Error();

                                        chargingNotificationsResponse = Acknowledgement.DataError(
                                                                            StatusCodeDescription:    "Could not parse the received ChargingStartNotification request!",
                                                                            StatusCodeAdditionalInfo:  errorResponse,
                                                                            RequestTimestamp:          request.Timestamp,
                                                                            EventTrackingId:           request.EventTrackingId
                                                                        );

                                    }

                                    break;

                                #endregion

                                #region Progress

                                case ChargingNotificationTypes.Progress:

                                    if (ChargingProgressNotificationRequest.TryParse(JSONRequest,
                                                                                     out var chargingProgressNotificationRequest,
                                                                                     out errorResponse,
                                                                                     processId,
                                                                                     request.Timestamp,
                                                                                     request.EventTrackingId,
                                                                                     request.Timeout ?? DefaultRequestTimeout,
                                                                                     CustomChargingProgressNotificationRequestParser,
                                                                                     request.CancellationToken))
                                    {

                                        Counters.ChargingProgressNotification.IncRequests_OK();

                                        #region Send OnChargingProgressNotificationRequest event

                                        try
                                        {

                                            if (OnChargingProgressNotificationRequest is not null)
                                                await Task.WhenAll(OnChargingProgressNotificationRequest.GetInvocationList().
                                                                   Cast<OnChargingProgressNotificationRequestDelegate>().
                                                                   Select(e => e(Timestamp.Now,
                                                                                 this,
                                                                                 chargingProgressNotificationRequest!))).
                                                                   ConfigureAwait(false);

                                        }
                                        catch (Exception e)
                                        {
                                            DebugX.LogException(e, nameof(EMPServerAPI) + "." + nameof(OnChargingProgressNotificationRequest));
                                        }

                                        #endregion

                                        #region Call async subscribers

                                        Acknowledgement<ChargingProgressNotificationRequest>? chargingProgressNotificationResponse = null;

                                        var OnChargingProgressNotificationLocal = OnChargingProgressNotification;
                                        if (OnChargingProgressNotificationLocal is not null)
                                        {
                                            try
                                            {

                                                chargingProgressNotificationResponse = (await Task.WhenAll(OnChargingProgressNotificationLocal.GetInvocationList().
                                                                                                                                               Cast<OnChargingProgressNotificationDelegate>().
                                                                                                                                               Select(e => e(Timestamp.Now,
                                                                                                                                                             this,
                                                                                                                                                             chargingProgressNotificationRequest!))).
                                                                                                                                               ConfigureAwait(false))?.FirstOrDefault();

                                                Counters.ChargingNotifications.       IncResponses_OK();
                                                Counters.ChargingProgressNotification.IncResponses_OK();

                                            }
                                            catch (Exception e)
                                            {

                                                Counters.ChargingNotifications.       IncResponses_Error();
                                                Counters.ChargingProgressNotification.IncResponses_Error();

                                                chargingProgressNotificationResponse = Acknowledgement<ChargingProgressNotificationRequest>.DataError(
                                                                              Request:                   chargingProgressNotificationRequest,
                                                                              StatusCodeDescription:     e.Message,
                                                                              StatusCodeAdditionalInfo:  e.StackTrace,
                                                                              SessionId:                 chargingProgressNotificationRequest?.SessionId,
                                                                              CPOPartnerSessionId:       chargingProgressNotificationRequest?.CPOPartnerSessionId
                                                                          );

                                            }
                                        }

                                        if (chargingProgressNotificationResponse is null)
                                        {

                                            Counters.ChargingNotifications.       IncResponses_Error();
                                            Counters.ChargingProgressNotification.IncResponses_Error();

                                            chargingProgressNotificationResponse = Acknowledgement<ChargingProgressNotificationRequest>.SystemError(
                                                                                       chargingProgressNotificationRequest,
                                                                                       "Could not process the received ChargingProgressNotification request!"
                                                                                   );

                                        }

                                        #endregion

                                        #region Send OnChargingProgressNotificationResponse event

                                        try
                                        {

                                            if (OnChargingProgressNotificationResponse is not null)
                                                await Task.WhenAll(OnChargingProgressNotificationResponse.GetInvocationList().
                                                                   Cast<OnChargingProgressNotificationResponseDelegate>().
                                                                   Select(e => e(Timestamp.Now,
                                                                                 this,
                                                                                 chargingProgressNotificationRequest!,
                                                                                 chargingProgressNotificationResponse,
                                                                                 Timestamp.Now - startTime))).
                                                                   ConfigureAwait(false);

                                        }
                                        catch (Exception e)
                                        {
                                            DebugX.LogException(e, nameof(EMPServerAPI) + "." + nameof(OnChargingProgressNotificationResponse));
                                        }

                                        #endregion

                                        chargingNotificationsResponse = chargingProgressNotificationResponse;

                                    }
                                    else
                                    {

                                        Counters.ChargingNotifications.       IncRequests_Error();
                                        Counters.ChargingProgressNotification.IncRequests_Error();

                                        chargingNotificationsResponse = Acknowledgement.DataError(
                                                                            StatusCodeDescription:    "Could not parse the received ChargingProgressNotification request!",
                                                                            StatusCodeAdditionalInfo:  errorResponse,
                                                                            RequestTimestamp:          request.Timestamp,
                                                                            EventTrackingId:           request.EventTrackingId
                                                                        );

                                    }

                                    break;

                                #endregion

                                #region End

                                case ChargingNotificationTypes.End:

                                    if (ChargingEndNotificationRequest.TryParse(JSONRequest,
                                                                                out var chargingEndNotificationRequest,
                                                                                out errorResponse,
                                                                                processId,
                                                                                request.Timestamp,
                                                                                request.EventTrackingId,
                                                                                request.Timeout ?? DefaultRequestTimeout,
                                                                                CustomChargingEndNotificationRequestParser,
                                                                                request.CancellationToken))
                                    {

                                        Counters.ChargingEndNotification.IncRequests_OK();

                                        #region Send OnChargingEndNotificationRequest event

                                        try
                                        {

                                            if (OnChargingEndNotificationRequest is not null)
                                                await Task.WhenAll(OnChargingEndNotificationRequest.GetInvocationList().
                                                                   Cast<OnChargingEndNotificationRequestDelegate>().
                                                                   Select(e => e(Timestamp.Now,
                                                                                 this,
                                                                                 chargingEndNotificationRequest!))).
                                                                   ConfigureAwait(false);

                                        }
                                        catch (Exception e)
                                        {
                                            DebugX.LogException(e, nameof(EMPServerAPI) + "." + nameof(OnChargingEndNotificationRequest));
                                        }

                                        #endregion

                                        #region Call async subscribers

                                        Acknowledgement<ChargingEndNotificationRequest>? chargingEndNotificationResponse = null;

                                        var OnChargingEndNotificationLocal = OnChargingEndNotification;
                                        if (OnChargingEndNotificationLocal is not null)
                                        {
                                            try
                                            {

                                                chargingEndNotificationResponse = (await Task.WhenAll(OnChargingEndNotificationLocal.GetInvocationList().
                                                                                                                                     Cast<OnChargingEndNotificationDelegate>().
                                                                                                                                     Select(e => e(Timestamp.Now,
                                                                                                                                                   this,
                                                                                                                                                   chargingEndNotificationRequest!))).
                                                                                                                                     ConfigureAwait(false))?.FirstOrDefault();

                                                Counters.ChargingNotifications.  IncResponses_OK();
                                                Counters.ChargingEndNotification.IncResponses_OK();

                                            }
                                            catch (Exception e)
                                            {

                                                Counters.ChargingNotifications.  IncResponses_Error();
                                                Counters.ChargingEndNotification.IncResponses_Error();

                                                chargingEndNotificationResponse = Acknowledgement<ChargingEndNotificationRequest>.DataError(
                                                                                      Request:                   chargingEndNotificationRequest,
                                                                                      StatusCodeDescription:     e.Message,
                                                                                      StatusCodeAdditionalInfo:  e.StackTrace,
                                                                                      SessionId:                 chargingEndNotificationRequest?.SessionId,
                                                                                      CPOPartnerSessionId:       chargingEndNotificationRequest?.CPOPartnerSessionId
                                                                                  );

                                            }
                                        }

                                        if (chargingEndNotificationResponse is null)
                                        {

                                            Counters.ChargingNotifications.  IncResponses_Error();
                                            Counters.ChargingEndNotification.IncResponses_Error();

                                            chargingEndNotificationResponse = Acknowledgement<ChargingEndNotificationRequest>.SystemError(
                                                                                  chargingEndNotificationRequest,
                                                                                  "Could not process the received ChargingEndNotification request!"
                                                                              );

                                        }

                                        #endregion

                                        #region Send OnChargingEndNotificationResponse event

                                        try
                                        {

                                            if (OnChargingEndNotificationResponse is not null)
                                                await Task.WhenAll(OnChargingEndNotificationResponse.GetInvocationList().
                                                                   Cast<OnChargingEndNotificationResponseDelegate>().
                                                                   Select(e => e(Timestamp.Now,
                                                                                 this,
                                                                                 chargingEndNotificationRequest!,
                                                                                 chargingEndNotificationResponse,
                                                                                 Timestamp.Now - startTime))).
                                                                   ConfigureAwait(false);

                                        }
                                        catch (Exception e)
                                        {
                                            DebugX.LogException(e, nameof(EMPServerAPI) + "." + nameof(OnChargingEndNotificationResponse));
                                        }

                                        #endregion

                                        chargingNotificationsResponse = chargingEndNotificationResponse;

                                    }
                                    else
                                    {

                                        Counters.ChargingNotifications.  IncRequests_Error();
                                        Counters.ChargingEndNotification.IncRequests_Error();

                                        chargingNotificationsResponse = Acknowledgement.DataError(
                                                                            StatusCodeDescription:    "Could not parse the received ChargingEndNotification request!",
                                                                            StatusCodeAdditionalInfo:  errorResponse,
                                                                            RequestTimestamp:          request.Timestamp,
                                                                            EventTrackingId:           request.EventTrackingId
                                                                        );

                                    }

                                    break;

                                #endregion

                                #region Error

                                case ChargingNotificationTypes.Error:

                                    if (ChargingErrorNotificationRequest.TryParse(JSONRequest,
                                                                                  out var chargingErrorNotificationRequest,
                                                                                  out errorResponse,
                                                                                  processId,
                                                                                  request.Timestamp,
                                                                                  request.EventTrackingId,
                                                                                  request.Timeout ?? DefaultRequestTimeout,
                                                                                  CustomChargingErrorNotificationRequestParser,
                                                                                  request.CancellationToken))
                                    {

                                        Counters.ChargingErrorNotification.IncRequests_OK();

                                        #region Send OnChargingErrorNotificationRequest event

                                        try
                                        {

                                            if (OnChargingErrorNotificationRequest is not null)
                                                await Task.WhenAll(OnChargingErrorNotificationRequest.GetInvocationList().
                                                                   Cast<OnChargingErrorNotificationRequestDelegate>().
                                                                   Select(e => e(Timestamp.Now,
                                                                                 this,
                                                                                 chargingErrorNotificationRequest!))).
                                                                   ConfigureAwait(false);

                                        }
                                        catch (Exception e)
                                        {
                                            DebugX.LogException(e, nameof(EMPServerAPI) + "." + nameof(OnChargingErrorNotificationRequest));
                                        }

                                        #endregion

                                        #region Call async subscribers

                                        Acknowledgement<ChargingErrorNotificationRequest>? chargingErrorNotificationResponse = null;

                                        var OnChargingErrorNotificationLocal = OnChargingErrorNotification;
                                        if (OnChargingErrorNotificationLocal is not null)
                                        {
                                            try
                                            {

                                                chargingErrorNotificationResponse = (await Task.WhenAll(OnChargingErrorNotificationLocal.GetInvocationList().
                                                                                                                                         Cast<OnChargingErrorNotificationDelegate>().
                                                                                                                                         Select(e => e(Timestamp.Now,
                                                                                                                                                       this,
                                                                                                                                                       chargingErrorNotificationRequest!))).
                                                                                                                                         ConfigureAwait(false))?.FirstOrDefault();

                                                Counters.ChargingNotifications.    IncResponses_OK();
                                                Counters.ChargingErrorNotification.IncResponses_OK();

                                            }
                                            catch (Exception e)
                                            {

                                                Counters.ChargingNotifications.    IncResponses_Error();
                                                Counters.ChargingErrorNotification.IncResponses_Error();

                                                chargingErrorNotificationResponse = Acknowledgement<ChargingErrorNotificationRequest>.DataError(
                                                                           Request:                   chargingErrorNotificationRequest,
                                                                           StatusCodeDescription:     e.Message,
                                                                           StatusCodeAdditionalInfo:  e.StackTrace,
                                                                           SessionId:                 chargingErrorNotificationRequest?.SessionId,
                                                                           CPOPartnerSessionId:       chargingErrorNotificationRequest?.CPOPartnerSessionId
                                                                       );

                                            }
                                        }

                                        if (chargingErrorNotificationResponse is null)
                                        {

                                            Counters.ChargingNotifications.    IncResponses_Error();
                                            Counters.ChargingErrorNotification.IncResponses_Error();

                                            chargingErrorNotificationResponse = Acknowledgement<ChargingErrorNotificationRequest>.SystemError(
                                                                                    chargingErrorNotificationRequest,
                                                                                    "Could not process the received ChargingErrorNotification request!"
                                                                                );

                                        }

                                        #endregion

                                        #region Send OnChargingErrorNotificationResponse event

                                        try
                                        {

                                            if (OnChargingErrorNotificationResponse is not null)
                                                await Task.WhenAll(OnChargingErrorNotificationResponse.GetInvocationList().
                                                                   Cast<OnChargingErrorNotificationResponseDelegate>().
                                                                   Select(e => e(Timestamp.Now,
                                                                                 this,
                                                                                 chargingErrorNotificationRequest!,
                                                                                 chargingErrorNotificationResponse,
                                                                                 Timestamp.Now - startTime))).
                                                                   ConfigureAwait(false);

                                        }
                                        catch (Exception e)
                                        {
                                            DebugX.LogException(e, nameof(EMPServerAPI) + "." + nameof(OnChargingErrorNotificationResponse));
                                        }

                                        #endregion

                                        chargingNotificationsResponse = chargingErrorNotificationResponse;

                                    }
                                    else
                                    {

                                        Counters.ChargingNotifications.    IncRequests_Error();
                                        Counters.ChargingErrorNotification.IncRequests_Error();

                                        chargingNotificationsResponse = Acknowledgement.DataError(
                                                                            StatusCodeDescription:    "Could not parse the received ChargingErrorNotification request!",
                                                                            StatusCodeAdditionalInfo:  errorResponse,
                                                                            RequestTimestamp:          request.Timestamp,
                                                                            EventTrackingId:           request.EventTrackingId
                                                                        );

                                    }

                                    break;

                                #endregion

                                #region ...or default

                                default:

                                    Counters.ChargingNotifications.IncRequests_Error();

                                    chargingNotificationsResponse = Acknowledgement.DataError(
                                                                        StatusCodeDescription: "Unknown or invalid charging notification type '" + chargingNotificationType.ToString() + "'!",
                                                                        RequestTimestamp:       request.Timestamp,
                                                                        EventTrackingId:        request.EventTrackingId
                                                                    );

                                    break;

                                #endregion

                            }

                        }
                        else
                        {

                            Counters.ChargingNotifications.IncRequests_Error();

                            chargingNotificationsResponse = Acknowledgement.DataError(
                                                                StatusCodeDescription: "Could not parse the received ChargingNotifications request!",
                                                                RequestTimestamp:       request.Timestamp,
                                                                EventTrackingId:        request.EventTrackingId
                                                            );

                        }

                    }
                    catch (Exception e)
                    {

                        Counters.ChargingNotifications.IncRequests_Error();

                        chargingNotificationsResponse = Acknowledgement.SystemError(
                                                            StatusCodeDescription:     e.Message,
                                                            StatusCodeAdditionalInfo:  e.StackTrace,
                                                            RequestTimestamp:          request.Timestamp,
                                                            EventTrackingId:           request.EventTrackingId
                                                        );

                    }

                    if (chargingNotificationsResponse is null)
                    {

                        Counters.ChargingNotifications.IncResponses_Error();

                        chargingNotificationsResponse = Acknowledgement.SystemError(
                                                            StatusCodeDescription:  "Could not process the received ChargingNotifications request!",
                                                            RequestTimestamp:        request.Timestamp,
                                                            EventTrackingId:         request.EventTrackingId
                                                        );

                    }


                    return new HTTPResponse.Builder(request) {
                               HTTPStatusCode             = HTTPStatusCode.OK,
                               Server                     = HTTPServerName,
                               Date                       = Timestamp.Now,
                               AccessControlAllowOrigin   = "*",
                               AccessControlAllowMethods  = [ "POST" ],
                               AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                               ContentType                = HTTPContentType.Application.JSON_UTF8,
                               Content                    = chargingNotificationsResponse.ToJSON(CustomAcknowledgementSerializer,
                                                                                                 CustomStatusCodeSerializer).
                                                                                          ToString(this.JSONFormatting).
                                                                                          ToUTF8Bytes(),
                               Connection                 = ConnectionType.Close
                           }.AsImmutable;

                }, AllowReplacement: URLReplacement.Allow);

            #endregion


            #region POST  ~/api/oicp/cdrmgmt/v22/operators/{operatorId}/charge-detail-record

            // ------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/cdrmgmt/v22/operators/DE*GDF/charge-detail-record
            // ------------------------------------------------------------------------------------------------------------------------------------------
            AddHandler(
                HTTPMethod.POST,
                URLPathPrefix + HTTPPath.Parse($"/api/oicp/cdrmgmt/v22/operators/{{{OICPExtensions.OperatorId}}}/charge-detail-record"),
                HTTPContentType.Application.JSON_UTF8,
                HTTPRequestLogger:   logChargeDetailRecordHTTPRequest,
                HTTPResponseLogger:  logChargeDetailRecordHTTPResponse,
                HTTPDelegate:        async request => {

                    var startTime  = Timestamp.Now;
                    var processId  = request.TryParseProcessId();

                    Acknowledgement<ChargeDetailRecordRequest>? chargeDetailRecordResponse = null;

                    try
                    {

                        #region Try to parse OperatorId URL parameter

                        if (!request.TryParseOperatorId(out var operatorId))
                        {

                            Counters.ChargeDetailRecord.IncRequests_OK();

                            chargeDetailRecordResponse = Acknowledgement<ChargeDetailRecordRequest>.SystemError(
                                                             StatusCodeDescription: "The expected 'operatorId' URL parameter could not be parsed!"
                                                         );

                        }

                        #endregion

                        else if (ChargeDetailRecordRequest.TryParse(JObject.Parse(request.HTTPBody?.ToUTF8String() ?? ""),
                                                                    operatorId,
                                                                    out var chargeDetailRecordRequest,
                                                                    out var errorResponse,
                                                                    processId,
                                                                    request.Timestamp,
                                                                    request.EventTrackingId,
                                                                    request.Timeout ?? DefaultRequestTimeout,
                                                                    CustomChargeDetailRecordRequestParser,
                                                                    request.CancellationToken))
                        {

                            Counters.ChargeDetailRecord.IncRequests_OK();

                            #region  Send OnChargeDetailRecordRequest event

                            try
                            {

                                if (OnChargeDetailRecordRequest is not null)
                                    await Task.WhenAll(OnChargeDetailRecordRequest.GetInvocationList().
                                                       Cast<OnChargeDetailRecordRequestDelegate>().
                                                       Select(e => e(Timestamp.Now,
                                                                     this,
                                                                     chargeDetailRecordRequest!))).
                                                       ConfigureAwait(false);

                            }
                            catch (Exception e)
                            {
                                DebugX.LogException(e, nameof(EMPServerAPI) + "." + nameof(OnChargeDetailRecordRequest));
                            }

                            #endregion

                            #region Call async subscribers

                            var OnChargeDetailRecordLocal = OnChargeDetailRecord;
                            if (OnChargeDetailRecordLocal is not null)
                            {
                                try
                                {

                                    chargeDetailRecordResponse = (await Task.WhenAll(OnChargeDetailRecordLocal.GetInvocationList().
                                                                                                                   Cast<OnChargeDetailRecordDelegate>().
                                                                                                                   Select(e => e(Timestamp.Now,
                                                                                                                                 this,
                                                                                                                                 chargeDetailRecordRequest!))))?.FirstOrDefault();

                                    Counters.ChargeDetailRecord.IncResponses_OK();

                                }
                                catch (Exception e)
                                {

                                    Counters.ChargeDetailRecord.IncResponses_Error();

                                    chargeDetailRecordResponse = Acknowledgement<ChargeDetailRecordRequest>.DataError(
                                                                    Request:                   chargeDetailRecordRequest,
                                                                    StatusCodeDescription:     e.Message,
                                                                    StatusCodeAdditionalInfo:  e.StackTrace,
                                                                    SessionId:                 chargeDetailRecordRequest?.ChargeDetailRecord?.SessionId,
                                                                    CPOPartnerSessionId:       chargeDetailRecordRequest?.ChargeDetailRecord?.CPOPartnerSessionId
                                                                 );

                                }
                            }

                            if (chargeDetailRecordResponse is null)
                            {

                                Counters.ChargeDetailRecord.IncResponses_Error();

                                chargeDetailRecordResponse = Acknowledgement<ChargeDetailRecordRequest>.SystemError(
                                                                 chargeDetailRecordRequest,
                                                                 "Could not process the received charge detail record!"
                                                             );

                            }

                            #endregion

                            #region Send OnChargeDetailRecordResponse event

                            try
                            {

                                if (OnChargeDetailRecordResponse is not null)
                                    await Task.WhenAll(OnChargeDetailRecordResponse.GetInvocationList().
                                                       Cast<OnChargeDetailRecordResponseDelegate>().
                                                       Select(e => e(Timestamp.Now,
                                                                     this,
                                                                     chargeDetailRecordRequest!,
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
                        {

                            Counters.ChargeDetailRecord.IncRequests_OK();

                            chargeDetailRecordResponse = Acknowledgement<ChargeDetailRecordRequest>.DataError(
                                                             Request:                   chargeDetailRecordRequest,
                                                             StatusCodeDescription:     "We could not handle the given charge detail record!",
                                                             StatusCodeAdditionalInfo:  errorResponse,
                                                             SessionId:                 chargeDetailRecordRequest?.ChargeDetailRecord?.SessionId,
                                                             CPOPartnerSessionId:       chargeDetailRecordRequest?.ChargeDetailRecord?.CPOPartnerSessionId
                                                         );

                        }

                    }
                    catch (Exception e)
                    {

                        Counters.ChargeDetailRecord.IncResponses_Error();

                        chargeDetailRecordResponse = Acknowledgement<ChargeDetailRecordRequest>.SystemError(
                                                         Request:                   null,
                                                         StatusCodeDescription:     e.Message,
                                                         StatusCodeAdditionalInfo:  e.StackTrace
                                                     );

                    }


                    return new HTTPResponse.Builder(request) {
                               HTTPStatusCode             = HTTPStatusCode.OK,
                               Server                     = HTTPServerName,
                               Date                       = Timestamp.Now,
                               AccessControlAllowOrigin   = "*",
                               AccessControlAllowMethods  = [ "POST" ],
                               AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                               ContentType                = HTTPContentType.Application.JSON_UTF8,
                               Content                    = chargeDetailRecordResponse.ToJSON(CustomAcknowledgementSerializer,
                                                                                              CustomStatusCodeSerializer).
                                                                                       ToString(this.JSONFormatting).
                                                                                       ToUTF8Bytes(),
                               Connection                 = ConnectionType.Close
                           }.AsImmutable;

                },
                AllowReplacement: URLReplacement.Allow
            );

            #endregion

            #endregion

        }

        #endregion


        public static async Task<EMPServerAPI>

            CreateServer(HTTPHostname?                                              HTTPHostname                 = null,
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
                         Boolean                                                    AutoStart                    = false,

                         IEnumerable<HTTPHostname>?                                 Hostnames                    = null,
                         HTTPPath?                                                  RootPath                     = null,
                         IEnumerable<HTTPContentType>?                              HTTPContentTypes             = null,
                         I18NString?                                                Description                  = null)

        {

            var server  = new HTTPTestServerX(
                              IPAddress:             null,
                              TCPPort:               HTTPServerPort,
                              HTTPServerName:        HTTPServerName,
                              BufferSize:            null,
                              ReceiveTimeout:        null,
                              SendTimeout:           null,
                              LoggingHandler:        null
                          );

            var api     = new EMPServerAPI(

                              HTTPTestServer:        server,
                              Hostnames:             Hostnames,
                              RootPath:              RootPath,
                              HTTPContentTypes:      HTTPContentTypes,
                              Description:           Description,

                              RegisterRootService:   true,
                              URLPathPrefix:         URLPathPrefix,

                              DisableLogging:        DisableLogging,
                              LoggingPath:           LoggingPath,
                              LoggingContext:        LoggingContext,
                              LogfileName:           LogfileName,
                              LogfileCreator:        LogfileCreator

                          );

            server.AddHTTPAPI(
                HTTPPath.Root,
                HTTPHostname,
                (s, p) => api
            );

            await server.Start();

            return api;

        }



        #region Dispose()

        ///// <summary>
        ///// Dispose this object.
        ///// </summary>
        //public override void Dispose()
        //{ }

        #endregion

    }

}
