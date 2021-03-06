﻿/*
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
using System.Net.Security;
using System.Threading.Tasks;
using System.Security.Authentication;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.EMP
{

    /// <summary>
    /// The OICP EMP HTTP Server API.
    /// </summary>
    public partial class EMPServerAPI : HTTPAPI
    {

        #region Data

        /// <summary>
        /// The default HTTP server name.
        /// </summary>
        public new const String  DefaultHTTPServerName   = "GraphDefined OICP " + Version.Number + " EMP HTTP API";

        /// <summary>
        /// The default HTTP service name.
        /// </summary>
        public new const String  DefaultHTTPServiceName  = "GraphDefined OICP " + Version.Number + " EMP HTTP API";

        #endregion

        #region Properties

        // Custom JSON parsers

        public CustomJObjectParserDelegate<AuthorizeStartRequest>                CustomAuthorizeStartRequestParser                  { get; set; }
        public CustomJObjectParserDelegate<AuthorizeStopRequest>                 CustomAuthorizeStopRequestParser                   { get; set; }


        public CustomJObjectParserDelegate<ChargingStartNotificationRequest>     CustomChargingStartNotificationRequestParser       { get; set; }
        public CustomJObjectParserDelegate<ChargingProgressNotificationRequest>  CustomChargingProgressNotificationRequestParser    { get; set; }
        public CustomJObjectParserDelegate<ChargingEndNotificationRequest>       CustomChargingEndNotificationRequestParser         { get; set; }
        public CustomJObjectParserDelegate<ChargingErrorNotificationRequest>     CustomChargingErrorNotificationRequestParser       { get; set; }


        public CustomJObjectParserDelegate<SendChargeDetailRecordRequest>        CustomSendChargeDetailRecordRequestParser          { get; set; }


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
        protected internal Task logAuthorizeStartHTTPRequest(DateTime      Timestamp,
                                                             HTTPAPI       API,
                                                             HTTPRequest   Request)

            => OnAuthorizeStartHTTPRequest?.WhenAll(Timestamp,
                                                    API ?? this,
                                                    Request);

        #endregion

        public event OnAuthorizeStartRequestDelegate   OnAuthorizeStartRequest;
        public event OnAuthorizeStartDelegate          OnAuthorizeStart;
        public event OnAuthorizeStartResponseDelegate  OnAuthorizeStartResponse;

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

        public event OnAuthorizeStopRequestDelegate   OnAuthorizeStopRequest;
        public event OnAuthorizeStopDelegate          OnAuthorizeStop;
        public event OnAuthorizeStopResponseDelegate  OnAuthorizeStopResponse;

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

        public event OnChargeDetailRecordRequestDelegate   OnChargeDetailRecordRequest;
        public event OnChargeDetailRecordDelegate          OnChargeDetailRecord;
        public event OnChargeDetailRecordResponseDelegate  OnChargeDetailRecordResponse;

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
        public EMPServerAPI(ServerCertificateSelectorDelegate    ServerCertificateSelector    = null,
                            LocalCertificateSelectionCallback    ClientCertificateSelector    = null,
                            RemoteCertificateValidationCallback  ClientCertificateValidator   = null,
                            SslProtocols                         AllowedTLSProtocols          = SslProtocols.Tls12 | SslProtocols.Tls13,
                            HTTPHostname?                        HTTPHostname                 = null,
                            IPPort?                              HTTPServerPort               = null,
                            String                               HTTPServerName               = DefaultHTTPServerName,
                            String                               ExternalDNSName              = null,
                            HTTPPath?                            BasePath                     = null,
                            HTTPPath?                            URLPathPrefix                = null,
                            String                               ServiceName                  = DefaultHTTPServiceName,
                            Boolean                              DisableLogging               = false,
                            String                               LoggingContext               = null,
                            LogfileCreatorDelegate               LogfileCreator               = null,
                            DNSClient                            DNSClient                    = null,
                            Boolean                              AutoStart                    = false)

            : base(ServerCertificateSelector,
                   ClientCertificateSelector,
                   ClientCertificateValidator,
                   AllowedTLSProtocols,
                   HTTPHostname,
                   HTTPServerPort,
                   HTTPServerName,
                   ExternalDNSName,
                   URLPathPrefix,
                   BasePath,
                   ServiceName,
                   DNSClient,
                   false)

        {

            RegisterURLTemplates();

            this.HTTPLogger  = DisableLogging == false
                                   ? new Logger(this,
                                                LoggingContext,
                                                LogfileCreator)
                                   : null;

            if (AutoStart)
                HTTPServer.Start();

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
                                                     Date            = DateTime.UtcNow,
                                                     ContentType     = HTTPContentType.TEXT_UTF8,
                                                     Content         = "This is an OICP v2.3 HTTP/JSON endpoint!".ToUTF8Bytes(),
                                                     CacheControl    = "public, max-age=300",
                                                     Connection      = "close"
                                                 }.AsImmutable);
                                         });

            #endregion


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

                                             if (AuthorizeStartRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                Request.Timeout ?? DefaultRequestTimeout,
                                                                                out AuthorizeStartRequest  authorizeStartRequest,
                                                                                out String                 errorResponse,
                                                                                Request.Timestamp,
                                                                                Request.EventTrackingId,
                                                                                CustomAuthorizeStartRequestParser))
                                             {

                                                 var OnAuthorizeStartLocal = OnAuthorizeStart;
                                                 if (OnAuthorizeStartLocal != null)
                                                 {

                                                     try
                                                     {

                                                         var response = await OnAuthorizeStartLocal.Invoke(DateTime.UtcNow,
                                                                                                           this,
                                                                                                           authorizeStartRequest);

                                                         return new HTTPResponse.Builder(Request) {
                                                                    HTTPStatusCode             = HTTPStatusCode.OK,
                                                                    Server                     = HTTPServer.DefaultServerName,
                                                                    Date                       = DateTime.UtcNow,
                                                                    AccessControlAllowOrigin   = "*",
                                                                    AccessControlAllowMethods  = "POST",
                                                                    AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                    ContentType                = HTTPContentType.JSON_UTF8,
                                                                    Content                    = response.ToJSON().ToString(JSONFormatting).ToUTF8Bytes(),
                                                                    Connection                 = "close"
                                                                }.AsImmutable;

                                                     }
                                                     catch (Exception e)
                                                     {

                                                         return new HTTPResponse.Builder(Request) {
                                                                    HTTPStatusCode             = HTTPStatusCode.OK,
                                                                    Server                     = HTTPServer.DefaultServerName,
                                                                    Date                       = DateTime.UtcNow,
                                                                    AccessControlAllowOrigin   = "*",
                                                                    AccessControlAllowMethods  = "POST",
                                                                    AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                    ContentType                = HTTPContentType.JSON_UTF8,
                                                                    Content                    = AuthorizationStartResponse.DataError(
                                                                                                                                Request:                   authorizeStartRequest,
                                                                                                                                StatusCodeDescription:     e.Message,
                                                                                                                                StatusCodeAdditionalInfo:  e.StackTrace,
                                                                                                                                SessionId:                 authorizeStartRequest.SessionId,
                                                                                                                                CPOPartnerSessionId:       authorizeStartRequest.CPOPartnerSessionId
                                                                                                                            ).
                                                                                                                            ToJSON().
                                                                                                                            ToString(JSONFormatting).
                                                                                                                            ToUTF8Bytes(),
                                                                    Connection                 = "close"
                                                                }.AsImmutable;

                                                     }

                                                 }

                                             }

                                             return new HTTPResponse.Builder(Request) {
                                                        HTTPStatusCode             = HTTPStatusCode.OK,
                                                        Server                     = HTTPServer.DefaultServerName,
                                                        Date                       = DateTime.UtcNow,
                                                        AccessControlAllowOrigin   = "*",
                                                        AccessControlAllowMethods  = "POST",
                                                        AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                        ContentType                = HTTPContentType.JSON_UTF8,
                                                        Content                    = AuthorizationStartResponse.DataError(
                                                                                                                    Request:                   authorizeStartRequest,
                                                                                                                    StatusCodeDescription:     "We could not handle the given AuthorizeStart request!",
                                                                                                                    StatusCodeAdditionalInfo:  errorResponse,
                                                                                                                    SessionId:                 authorizeStartRequest.SessionId,
                                                                                                                    CPOPartnerSessionId:       authorizeStartRequest.CPOPartnerSessionId
                                                                                                                ).
                                                                                                                ToJSON().
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

                                             if (AuthorizeStopRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                               Request.Timeout ?? DefaultRequestTimeout,
                                                                               out AuthorizeStopRequest  authorizeStopRequest,
                                                                               out String                errorResponse,
                                                                               Request.Timestamp,
                                                                               Request.EventTrackingId,
                                                                               CustomAuthorizeStopRequestParser))
                                             {

                                                 var OnAuthorizeStopLocal = OnAuthorizeStop;
                                                 if (OnAuthorizeStopLocal != null)
                                                 {

                                                     try
                                                     {

                                                         var response = await OnAuthorizeStopLocal.Invoke(DateTime.UtcNow,
                                                                                                          this,
                                                                                                          authorizeStopRequest);

                                                         return new HTTPResponse.Builder(Request) {
                                                                    HTTPStatusCode             = HTTPStatusCode.OK,
                                                                    Server                     = HTTPServer.DefaultServerName,
                                                                    Date                       = DateTime.UtcNow,
                                                                    AccessControlAllowOrigin   = "*",
                                                                    AccessControlAllowMethods  = "POST",
                                                                    AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                    ContentType                = HTTPContentType.JSON_UTF8,
                                                                    Content                    = response.ToJSON().ToString(JSONFormatting).ToUTF8Bytes(),
                                                                    Connection                 = "close"
                                                                }.AsImmutable;

                                                     }
                                                     catch (Exception e)
                                                     {

                                                         return new HTTPResponse.Builder(Request) {
                                                                    HTTPStatusCode             = HTTPStatusCode.OK,
                                                                    Server                     = HTTPServer.DefaultServerName,
                                                                    Date                       = DateTime.UtcNow,
                                                                    AccessControlAllowOrigin   = "*",
                                                                    AccessControlAllowMethods  = "POST",
                                                                    AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                    ContentType                = HTTPContentType.JSON_UTF8,
                                                                    Content                    = AuthorizationStopResponse.DataError(
                                                                                                                               Request:                   authorizeStopRequest,
                                                                                                                               StatusCodeDescription:     e.Message,
                                                                                                                               StatusCodeAdditionalInfo:  e.StackTrace,
                                                                                                                               SessionId:                 authorizeStopRequest.SessionId,
                                                                                                                               CPOPartnerSessionId:       authorizeStopRequest.CPOPartnerSessionId
                                                                                                                           ).
                                                                                                                           ToJSON().
                                                                                                                           ToString(JSONFormatting).
                                                                                                                           ToUTF8Bytes(),
                                                                    Connection                 = "close"
                                                                }.AsImmutable;

                                                     }

                                                 }

                                             }

                                             return new HTTPResponse.Builder(Request) {
                                                        HTTPStatusCode             = HTTPStatusCode.OK,
                                                        Server                     = HTTPServer.DefaultServerName,
                                                        Date                       = DateTime.UtcNow,
                                                        AccessControlAllowOrigin   = "*",
                                                        AccessControlAllowMethods  = "POST",
                                                        AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                        ContentType                = HTTPContentType.JSON_UTF8,
                                                        Content                    = AuthorizationStopResponse.DataError(
                                                                                                                   Request:                   authorizeStopRequest,
                                                                                                                   StatusCodeDescription:     "We could not handle the given AuthorizeStop request!",
                                                                                                                   StatusCodeAdditionalInfo:  errorResponse,
                                                                                                                   SessionId:                 authorizeStopRequest.SessionId,
                                                                                                                   CPOPartnerSessionId:       authorizeStopRequest.CPOPartnerSessionId
                                                                                                               ).
                                                                                                               ToJSON().
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

                                                 var StartTime      = DateTime.UtcNow;
                                                 var errorResponse  = String.Empty;

                                                 if (!Request.TryParseJObjectRequestBody(out JObject JSONRequest, out HTTPResponse.Builder HTTPResponse)
                                                        ||
                                                     !JSONRequest.ParseMandatory("Type",
                                                                                 "charging notification type",
                                                                                 ChargingNotificationTypesExtentions.TryParse,
                                                                                 out ChargingNotificationTypes  ChargingNotificationType,
                                                                                 out                            errorResponse))
                                                 {

                                                     return new HTTPResponse.Builder(Request) {
                                                                HTTPStatusCode             = HTTPStatusCode.BadRequest,
                                                                Server                     = HTTPServer.DefaultServerName,
                                                                Date                       = DateTime.UtcNow,
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


                                                 switch (ChargingNotificationType)
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

                                                             #region Send OnChargingStartNotificationRequest event

                                                             try
                                                             {

                                                                 if (OnChargingStartNotificationRequest != null)
                                                                     await Task.WhenAll(OnChargingStartNotificationRequest.GetInvocationList().
                                                                                        Cast<OnChargingStartNotificationRequestDelegate>().
                                                                                        Select(e => e(DateTime.UtcNow,
                                                                                                      this,
                                                                                                      chargingStartNotificationRequest))).
                                                                                        ConfigureAwait(false);

                                                             }
                                                             catch (Exception e)
                                                             {
                                                                 e.Log(nameof(EMPServerAPI) + "." + nameof(OnChargingStartNotificationRequest));
                                                             }

                                                             #endregion

                                                             #region Call async subscribers

                                                             Acknowledgement<ChargingStartNotificationRequest> acknowledgement = null;

                                                             if (OnChargingStartNotification != null)
                                                             {

                                                                 var results = await Task.WhenAll(OnChargingStartNotification.GetInvocationList().
                                                                                                      Cast<OnChargingStartNotificationDelegate>().
                                                                                                      Select(e => e(DateTime.UtcNow,
                                                                                                                    this,
                                                                                                                    chargingStartNotificationRequest))).
                                                                                                      ConfigureAwait(false);

                                                                 acknowledgement = results.FirstOrDefault();

                                                             }

                                                             if (acknowledgement == null)
                                                                 acknowledgement = Acknowledgement<ChargingStartNotificationRequest>.SystemError(
                                                                                       chargingStartNotificationRequest,
                                                                                       "Could not process the incoming ChargingStartNotification request!"
                                                                                   );

                                                             #endregion

                                                             #region Send OnChargingStartNotificationResponse event

                                                             try
                                                             {

                                                                 if (OnChargingStartNotificationResponse != null)
                                                                     await Task.WhenAll(OnChargingStartNotificationResponse.GetInvocationList().
                                                                                        Cast<OnChargingStartNotificationResponseDelegate>().
                                                                                        Select(e => e(DateTime.UtcNow,
                                                                                                      this,
                                                                                                      acknowledgement,
                                                                                                      DateTime.UtcNow - StartTime))).
                                                                                        ConfigureAwait(false);

                                                             }
                                                             catch (Exception e)
                                                             {
                                                                 e.Log(nameof(EMPServerAPI) + "." + nameof(OnChargingStartNotificationResponse));
                                                             }

                                                             #endregion

                                                             return new HTTPResponse.Builder(Request) {
                                                                        HTTPStatusCode             = HTTPStatusCode.OK,
                                                                        Server                     = HTTPServer.DefaultServerName,
                                                                        Date                       = DateTime.UtcNow,
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

                                                             #region Send OnChargingProgressNotificationRequest event

                                                             try
                                                             {

                                                                 if (OnChargingProgressNotificationRequest != null)
                                                                     await Task.WhenAll(OnChargingProgressNotificationRequest.GetInvocationList().
                                                                                        Cast<OnChargingProgressNotificationRequestDelegate>().
                                                                                        Select(e => e(DateTime.UtcNow,
                                                                                                      this,
                                                                                                      chargingProgressNotificationRequest))).
                                                                                        ConfigureAwait(false);

                                                             }
                                                             catch (Exception e)
                                                             {
                                                                 e.Log(nameof(EMPServerAPI) + "." + nameof(OnChargingProgressNotificationRequest));
                                                             }

                                                             #endregion

                                                             #region Call async subscribers

                                                             Acknowledgement<ChargingProgressNotificationRequest> acknowledgement = null;

                                                             if (OnChargingProgressNotification != null)
                                                             {

                                                                 var results = await Task.WhenAll(OnChargingProgressNotification.GetInvocationList().
                                                                                                      Cast<OnChargingProgressNotificationDelegate>().
                                                                                                      Select(e => e(DateTime.UtcNow,
                                                                                                                    this,
                                                                                                                    chargingProgressNotificationRequest))).
                                                                                                      ConfigureAwait(false);

                                                                 acknowledgement = results.FirstOrDefault();

                                                             }

                                                             if (acknowledgement == null)
                                                                 acknowledgement = Acknowledgement<ChargingProgressNotificationRequest>.SystemError(
                                                                                       chargingProgressNotificationRequest,
                                                                                       "Could not process the incoming ChargingProgressNotification request!"
                                                                                   );

                                                             #endregion

                                                             #region Send OnChargingProgressNotificationResponse event

                                                             try
                                                             {

                                                                 if (OnChargingProgressNotificationResponse != null)
                                                                     await Task.WhenAll(OnChargingProgressNotificationResponse.GetInvocationList().
                                                                                        Cast<OnChargingProgressNotificationResponseDelegate>().
                                                                                        Select(e => e(DateTime.UtcNow,
                                                                                                      this,
                                                                                                      acknowledgement,
                                                                                                      DateTime.UtcNow - StartTime))).
                                                                                        ConfigureAwait(false);

                                                             }
                                                             catch (Exception e)
                                                             {
                                                                 e.Log(nameof(EMPServerAPI) + "." + nameof(OnChargingProgressNotificationResponse));
                                                             }

                                                             #endregion

                                                             return new HTTPResponse.Builder(Request) {
                                                                        HTTPStatusCode             = HTTPStatusCode.OK,
                                                                        Server                     = HTTPServer.DefaultServerName,
                                                                        Date                       = DateTime.UtcNow,
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

                                                             #region Send OnChargingEndNotificationRequest event

                                                             try
                                                             {

                                                                 if (OnChargingEndNotificationRequest != null)
                                                                     await Task.WhenAll(OnChargingEndNotificationRequest.GetInvocationList().
                                                                                        Cast<OnChargingEndNotificationRequestDelegate>().
                                                                                        Select(e => e(DateTime.UtcNow,
                                                                                                      this,
                                                                                                      chargingEndNotificationRequest))).
                                                                                        ConfigureAwait(false);

                                                             }
                                                             catch (Exception e)
                                                             {
                                                                 e.Log(nameof(EMPServerAPI) + "." + nameof(OnChargingEndNotificationRequest));
                                                             }

                                                             #endregion

                                                             #region Call async subscribers

                                                             Acknowledgement<ChargingEndNotificationRequest> acknowledgement = null;

                                                             if (OnChargingEndNotification != null)
                                                             {

                                                                 var results = await Task.WhenAll(OnChargingEndNotification.GetInvocationList().
                                                                                                      Cast<OnChargingEndNotificationDelegate>().
                                                                                                      Select(e => e(DateTime.UtcNow,
                                                                                                                    this,
                                                                                                                    chargingEndNotificationRequest))).
                                                                                                      ConfigureAwait(false);

                                                                 acknowledgement = results.FirstOrDefault();

                                                             }

                                                             if (acknowledgement == null)
                                                                 acknowledgement = Acknowledgement<ChargingEndNotificationRequest>.SystemError(
                                                                                       chargingEndNotificationRequest,
                                                                                       "Could not process the incoming ChargingEndNotification request!"
                                                                                   );

                                                             #endregion

                                                             #region Send OnChargingEndNotificationResponse event

                                                             try
                                                             {

                                                                 if (OnChargingEndNotificationResponse != null)
                                                                     await Task.WhenAll(OnChargingEndNotificationResponse.GetInvocationList().
                                                                                        Cast<OnChargingEndNotificationResponseDelegate>().
                                                                                        Select(e => e(DateTime.UtcNow,
                                                                                                      this,
                                                                                                      acknowledgement,
                                                                                                      DateTime.UtcNow - StartTime))).
                                                                                        ConfigureAwait(false);

                                                             }
                                                             catch (Exception e)
                                                             {
                                                                 e.Log(nameof(EMPServerAPI) + "." + nameof(OnChargingEndNotificationResponse));
                                                             }

                                                             #endregion

                                                             return new HTTPResponse.Builder(Request) {
                                                                        HTTPStatusCode             = HTTPStatusCode.OK,
                                                                        Server                     = HTTPServer.DefaultServerName,
                                                                        Date                       = DateTime.UtcNow,
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

                                                             #region Send OnChargingErrorNotificationRequest event

                                                             try
                                                             {

                                                                 if (OnChargingErrorNotificationRequest != null)
                                                                     await Task.WhenAll(OnChargingErrorNotificationRequest.GetInvocationList().
                                                                                        Cast<OnChargingErrorNotificationRequestDelegate>().
                                                                                        Select(e => e(DateTime.UtcNow,
                                                                                                      this,
                                                                                                      chargingErrorNotificationRequest))).
                                                                                        ConfigureAwait(false);

                                                             }
                                                             catch (Exception e)
                                                             {
                                                                 e.Log(nameof(EMPServerAPI) + "." + nameof(OnChargingErrorNotificationRequest));
                                                             }

                                                             #endregion

                                                             #region Call async subscribers

                                                             Acknowledgement<ChargingErrorNotificationRequest> acknowledgement = null;

                                                             if (OnChargingErrorNotification != null)
                                                             {

                                                                 var results = await Task.WhenAll(OnChargingErrorNotification.GetInvocationList().
                                                                                                      Cast<OnChargingErrorNotificationDelegate>().
                                                                                                      Select(e => e(DateTime.UtcNow,
                                                                                                                    this,
                                                                                                                    chargingErrorNotificationRequest))).
                                                                                                      ConfigureAwait(false);

                                                                 acknowledgement = results.FirstOrDefault();

                                                             }

                                                             if (acknowledgement == null)
                                                                 acknowledgement = Acknowledgement<ChargingErrorNotificationRequest>.SystemError(
                                                                                       chargingErrorNotificationRequest,
                                                                                       "Could not process the incoming ChargingErrorNotification request!"
                                                                                   );

                                                             #endregion

                                                             #region Send OnChargingErrorNotificationResponse event

                                                             try
                                                             {

                                                                 if (OnChargingErrorNotificationResponse != null)
                                                                     await Task.WhenAll(OnChargingErrorNotificationResponse.GetInvocationList().
                                                                                        Cast<OnChargingErrorNotificationResponseDelegate>().
                                                                                        Select(e => e(DateTime.UtcNow,
                                                                                                      this,
                                                                                                      acknowledgement,
                                                                                                      DateTime.UtcNow - StartTime))).
                                                                                        ConfigureAwait(false);

                                                             }
                                                             catch (Exception e)
                                                             {
                                                                 e.Log(nameof(EMPServerAPI) + "." + nameof(OnChargingErrorNotificationResponse));
                                                             }

                                                             #endregion

                                                             return new HTTPResponse.Builder(Request) {
                                                                        HTTPStatusCode             = HTTPStatusCode.OK,
                                                                        Server                     = HTTPServer.DefaultServerName,
                                                                        Date                       = DateTime.UtcNow,
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

                                                         }

                                                     break;

                                                     #endregion

                                                 }

                                                 return new HTTPResponse.Builder(Request) {
                                                        HTTPStatusCode             = HTTPStatusCode.BadRequest,
                                                        Server                     = HTTPServer.DefaultServerName,
                                                        Date                       = DateTime.UtcNow,
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
                                             catch (Exception e)
                                             {

                                                 return new HTTPResponse.Builder(Request) {
                                                            HTTPStatusCode             = HTTPStatusCode.BadRequest,
                                                            Server                     = HTTPServer.DefaultServerName,
                                                            Date                       = DateTime.UtcNow,
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

                                             try
                                             {

                                                 #region Check URI parameter

                                                 if (Request.ParsedURLParameters.Length != 1 || !Operator_Id.TryParse(Request.ParsedURLParameters[0], out Operator_Id operatorId)) {

                                                     return new HTTPResponse.Builder(Request) {
                                                                HTTPStatusCode             = HTTPStatusCode.BadRequest,
                                                                Server                     = HTTPServer.DefaultServerName,
                                                                Date                       = DateTime.UtcNow,
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


                                                 if (SendChargeDetailRecordRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                            operatorId,
                                                                                            Request.Timeout ?? DefaultRequestTimeout,
                                                                                            out SendChargeDetailRecordRequest  sendChargeDetailRecordRequest,
                                                                                            out String                         errorResponse,
                                                                                            Request.Timestamp,
                                                                                            Request.EventTrackingId,
                                                                                            CustomSendChargeDetailRecordRequestParser))
                                                 {

                                                     var OnChargeDetailRecordLocal = OnChargeDetailRecord;
                                                     if (OnChargeDetailRecordLocal != null)
                                                     {

                                                         var response = await OnChargeDetailRecord.Invoke(DateTime.UtcNow,
                                                                                                          this,
                                                                                                          sendChargeDetailRecordRequest);

                                                         return new HTTPResponse.Builder(Request) {
                                                                    HTTPStatusCode             = HTTPStatusCode.OK,
                                                                    Server                     = HTTPServer.DefaultServerName,
                                                                    Date                       = DateTime.UtcNow,
                                                                    AccessControlAllowOrigin   = "*",
                                                                    AccessControlAllowMethods  = "POST",
                                                                    AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                                    ContentType                = HTTPContentType.JSON_UTF8,
                                                                    Content                    = response.ToJSON(CustomAcknowledgementSerializer,
                                                                                                                 CustomStatusCodeSerializer).
                                                                                                          ToString(JSONFormatting).
                                                                                                          ToUTF8Bytes(),
                                                                    Connection                 = "close"
                                                                }.AsImmutable;

                                                     }

                                                 }

                                                 return new HTTPResponse.Builder(Request) {
                                                            HTTPStatusCode             = HTTPStatusCode.OK,
                                                            Server                     = HTTPServer.DefaultServerName,
                                                            Date                       = DateTime.UtcNow,
                                                            AccessControlAllowOrigin   = "*",
                                                            AccessControlAllowMethods  = "POST",
                                                            AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                            ContentType                = HTTPContentType.JSON_UTF8,
                                                            Content                    = Acknowledgement<SendChargeDetailRecordRequest>.DataError(
                                                                                                                                            Request:                   sendChargeDetailRecordRequest,
                                                                                                                                            StatusCodeDescription:     "We could not parse the given ChargeDetailRecord request!",
                                                                                                                                            StatusCodeAdditionalInfo:  errorResponse
                                                                                                                                        ).
                                                                                                                                        ToJSON(CustomAcknowledgementSerializer,
                                                                                                                                               CustomStatusCodeSerializer).
                                                                                                                                        ToString(JSONFormatting).
                                                                                                                                        ToUTF8Bytes(),
                                                            Connection                 = "close"
                                                        }.AsImmutable;

                                             }
                                             catch (Exception e)
                                             {

                                                 return new HTTPResponse.Builder(Request) {
                                                            HTTPStatusCode             = HTTPStatusCode.OK,
                                                            Server                     = HTTPServer.DefaultServerName,
                                                            Date                       = DateTime.UtcNow,
                                                            AccessControlAllowOrigin   = "*",
                                                            AccessControlAllowMethods  = "POST",
                                                            AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                            ContentType                = HTTPContentType.JSON_UTF8,
                                                            Content                    = Acknowledgement.DataError(
                                                                                                             StatusCodeDescription:    "We could not parse the given ChargeDetailRecord request!",
                                                                                                             StatusCodeAdditionalInfo: e.Message + Environment.NewLine + e.StackTrace
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
