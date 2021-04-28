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
using System.Net.Security;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

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

        public CustomJObjectParserDelegate<AuthorizeStartRequest>          CustomAuthorizeStartRequestParser            { get; set; }
        public CustomJObjectParserDelegate<AuthorizeStopRequest>           CustomAuthorizeStopRequestParser             { get; set; }

        public CustomJObjectParserDelegate<SendChargeDetailRecordRequest>  CustomSendChargeDetailRecordRequestParser    { get; set; }

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
        /// Create a new CPO HTTP Server API.
        /// </summary>
        public EMPServerAPI(ServerCertificateSelectorDelegate    ServerCertificateSelector,
                            LocalCertificateSelectionCallback    ClientCertificateSelector,
                            RemoteCertificateValidationCallback  ClientCertificateValidator,
                            SslProtocols                         AllowedTLSProtocols   = SslProtocols.Tls12 | SslProtocols.Tls13,
                            HTTPHostname?                        HTTPHostname          = null,
                            IPPort?                              HTTPServerPort        = null,
                            String                               HTTPServerName        = DefaultHTTPServerName,
                            String                               ExternalDNSName       = null,
                            HTTPPath?                            BasePath              = null,
                            HTTPPath?                            URLPathPrefix         = null,
                            String                               ServiceName           = DefaultHTTPServiceName,
                            Boolean                              DisableLogging        = false,
                            String                               LoggingContext        = null,
                            LogfileCreatorDelegate               LogfileCreator        = null,
                            DNSClient                            DNSClient             = null,
                            Boolean                              AutoStart             = false)

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

            HTTPServer.AddFilter(request => {

                //if (request.RemoteSocket.IPAddress.IsIPv4 &&
                //    request.RemoteSocket.IPAddress.IsLocalhost)
                //{
                //    return null;
                //}

                #region Got API Key...

                if (request.API_Key.HasValue)
                {

                    if (request.API_Key.Value.ToString() == "1234")
                        return null;

                    //if (IsValidAPIKey(request.API_Key.Value))
                    //    return null;

                    DebugX.LogT("Invalid HTTP API Key: " + request.API_Key);

                    return new HTTPResponse.Builder(request) {
                        HTTPStatusCode  = HTTPStatusCode.Unauthorized,
                        Date            = DateTime.UtcNow,
                        Server          = HTTPServer.DefaultServerName,
                        CacheControl    = "private, max-age=0, no-cache",
                        Connection      = "close"
                    };

                }

                #endregion

                return null;

            });

            HTTPServer.Rewrite  (request => {
                return request;
            });


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
                                                                    Content                    = response.ToJSON().ToString(Newtonsoft.Json.Formatting.None).ToUTF8Bytes(),
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
                                                                                                                            ToString(Newtonsoft.Json.Formatting.None).
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
                                                                                                                ToString(Newtonsoft.Json.Formatting.None).
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
                                                                    Content                    = response.ToJSON().ToString(Newtonsoft.Json.Formatting.None).ToUTF8Bytes(),
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
                                                                                                                           ToString(Newtonsoft.Json.Formatting.None).
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
                                                                                                               ToString(Newtonsoft.Json.Formatting.None).
                                                                                                               ToUTF8Bytes(),
                                                        Connection                 = "close"
                                                    }.AsImmutable;

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

                                             if (SendChargeDetailRecordRequest.TryParse(Request.HTTPBody.ToUTF8String(),
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

                                                     try
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
                                                                    Content                    = response.ToJSON().ToString(Newtonsoft.Json.Formatting.None).ToUTF8Bytes(),
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
                                                                    Content                    = Acknowledgement<SendChargeDetailRecordRequest>.DataError(
                                                                                                                                                    Request:                   sendChargeDetailRecordRequest,
                                                                                                                                                    StatusCodeDescription:     e.Message,
                                                                                                                                                    StatusCodeAdditionalInfo:  e.StackTrace
                                                                                                                                                ).
                                                                                                                                                ToJSON().
                                                                                                                                                ToString(Newtonsoft.Json.Formatting.None).
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
                                                        Content                    = Acknowledgement<SendChargeDetailRecordRequest>.DataError(
                                                                                                                                        Request:                   sendChargeDetailRecordRequest,
                                                                                                                                        StatusCodeDescription:     "We could not handle the given ChargeDetailRecord request!",
                                                                                                                                        StatusCodeAdditionalInfo:  errorResponse
                                                                                                                                    ).
                                                                                                                                    ToJSON().
                                                                                                                                    ToString(Newtonsoft.Json.Formatting.None).
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
