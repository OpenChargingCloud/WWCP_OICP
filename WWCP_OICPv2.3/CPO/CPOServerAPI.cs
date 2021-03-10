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
using System.Security.Cryptography.X509Certificates;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using System.IO;
using System.Security.Authentication;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    /// <summary>
    /// The CPO HTTP server API.
    /// </summary>
    public class CPOServerAPI : HTTPAPI
    {

        #region Data

        public TimeSpan                                                             DefaultRequestTimeout                                   { get; set; }

        public CustomJObjectParserDelegate<AuthorizeRemoteReservationStartRequest>  CustomAuthorizeRemoteReservationStartRequestParser      { get; set; }

        public CustomJObjectParserDelegate<AuthorizeRemoteReservationStopRequest>   CustomAuthorizeRemoteReservationStopRequestParser       { get; set; }

        public CustomJObjectParserDelegate<AuthorizeRemoteStartRequest>             CustomAuthorizeRemoteStartRequestParser                 { get; set; }

        public CustomJObjectParserDelegate<AuthorizeRemoteStopRequest>              CustomAuthorizeRemoteStopRequestParser                  { get; set; }

        #endregion

        #region Properties

        public X509Certificate  ServerCert    { get; }

        public DNSClient        DNSClient     { get; }


        /// <summary>
        /// The CPO client (HTTP client) logger.
        /// </summary>
        //public Logger                               HTTPLogger                    { get; }

        #endregion

        #region Events

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
        protected internal Task logAuthorizeRemoteStartHTTPRequest(DateTime      Timestamp,
                                                                   HTTPAPI       API,
                                                                   HTTPRequest   Request)

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

        public CPOServerAPI(ServerCertificateSelectorDelegate    ServerCertificateSelector,
                            LocalCertificateSelectionCallback    ClientCertificateSelector,
                            RemoteCertificateValidationCallback  ClientCertificateValidator,
                            SslProtocols                         AllowedTLSProtocols   = SslProtocols.Tls12,
                            HTTPHostname?                        HTTPHostname          = null,
                            IPPort?                              HTTPServerPort        = null,
                            String                               HTTPServerName        = DefaultHTTPServerName,
                            String                               ExternalDNSName       = null,
                            HTTPPath?                            URLPathPrefix         = null,
                            String                               ServiceName           = DefaultHTTPServiceName,
                            DNSClient                            DNSClient             = null,
                            Boolean                              Autostart             = false)

            : base(ServerCertificateSelector,
                   ClientCertificateSelector,
                   ClientCertificateValidator,
                   AllowedTLSProtocols,
                   HTTPHostname,
                   HTTPServerPort,
                   HTTPServerName,
                   ExternalDNSName,
                   URLPathPrefix,
                   ServiceName,
                   DNSClient,
                   false)

        {

            RegisterURLTemplates();

            if (Autostart)
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

                                             if (AuthorizeRemoteStartRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                      Request.Timeout ?? DefaultRequestTimeout,
                                                                                      out AuthorizeRemoteStartRequest  authorizeRemoteStartRequest,
                                                                                      out String                       errorResponse,
                                                                                      Request.Timestamp,
                                                                                      Request.EventTrackingId,
                                                                                      CustomAuthorizeRemoteStartRequestParser))
                                             {

                                                 var OnAuthorizeRemoteStartLocal = OnAuthorizeRemoteStart;
                                                 if (OnAuthorizeRemoteStartLocal != null)
                                                 {

                                                     try
                                                     {

                                                         var response = await OnAuthorizeRemoteStartLocal.Invoke(DateTime.UtcNow,
                                                                                                                 this,
                                                                                                                 authorizeRemoteStartRequest);

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
                                                                    Content                    = Acknowledgement<AuthorizeRemoteStartRequest>.DataError(
                                                                                                                                                  Request:                   authorizeRemoteStartRequest,
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
                                                        Content                    = Acknowledgement<AuthorizeRemoteStartRequest>.DataError(
                                                                                                                                      Request:                   authorizeRemoteStartRequest,
                                                                                                                                      StatusCodeDescription:     "We could not handle the given AuthorizeRemoteStart request!",
                                                                                                                                      StatusCodeAdditionalInfo:  errorResponse
                                                                                                                                  ).
                                                                                                                                  ToJSON().
                                                                                                                                  ToString(Newtonsoft.Json.Formatting.None).
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

                                             if (AuthorizeRemoteStopRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                     Request.Timeout ?? DefaultRequestTimeout,
                                                                                     out AuthorizeRemoteStopRequest  authorizeRemoteStopRequest,
                                                                                     out String                      errorResponse,
                                                                                     Request.Timestamp,
                                                                                     Request.EventTrackingId,
                                                                                     CustomAuthorizeRemoteStopRequestParser))
                                             {

                                                 var OnAuthorizeRemoteStopLocal = OnAuthorizeRemoteStop;
                                                 if (OnAuthorizeRemoteStopLocal != null)
                                                 {

                                                     try
                                                     {

                                                         var response = await OnAuthorizeRemoteStopLocal.Invoke(DateTime.UtcNow,
                                                                                                                this,
                                                                                                                authorizeRemoteStopRequest);

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
                                                                    Content                    = Acknowledgement<AuthorizeRemoteStopRequest>.DataError(
                                                                                                                                                 Request:                   authorizeRemoteStopRequest,
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
                                                        Content                    = Acknowledgement<AuthorizeRemoteStopRequest>.DataError(
                                                                                                                                     Request:                   authorizeRemoteStopRequest,
                                                                                                                                     StatusCodeDescription:     "We could not handle the given AuthorizeRemoteStop request!",
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
