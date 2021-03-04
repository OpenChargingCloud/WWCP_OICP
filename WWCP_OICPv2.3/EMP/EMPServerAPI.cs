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

namespace cloud.charging.open.protocols.OICPv2_3.HTTP
{

    /// <summary>
    /// The EMP HTTP server API.
    /// </summary>
    public class EMPServerAPI : HTTPAPI
    {

        #region Data


        #endregion

        #region Properties

        public X509Certificate  ServerCert    { get; }

        public DNSClient        DNSClient     { get; }


        /// <summary>
        /// The EMP client (HTTP client) logger.
        /// </summary>
        //public Logger                               HTTPLogger                    { get; }

        #endregion

        #region Events

        #endregion

        #region Constructor(s)

        public EMPServerAPI(ServerCertificateSelectorDelegate    ServerCertificateSelector,
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

            


            #region POST  ~/api/oicp/charging/v21/operators/{operatorId}/authorize/start

            // --------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/charging/v21/operators/{operatorId}/authorize/start
            // --------------------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/charging/v21/operators/{operatorId}/authorize/start",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPDelegate: Request => {

                                             if (AuthorizeStartRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                out AuthorizeStartRequest authorizeStartRequest,
                                                                                out String ErrorResponse,
                                                                                null))
                                             {

                                                 var response = authorizeStartRequest.Identification.RFIDId?.ToString() == "11223344"

                                                                    ? AuthorizationStartResponse.Authorized   (authorizeStartRequest,
                                                                                                               authorizeStartRequest.SessionId,
                                                                                                               authorizeStartRequest.CPOPartnerSessionId,
                                                                                                               EMPPartnerSession_Id.NewRandom,
                                                                                                               Provider_Id.Parse("DE-XXX"),
                                                                                                               "Nice to meet you!",
                                                                                                               "Happy charging!",
                                                                                                               new Identification[] {
                                                                                                                   Identification.FromRFID(UID.Parse("99887766")),
                                                                                                                   Identification.FromRFID(UID.Parse("77665544"))
                                                                                                               })

                                                                    : AuthorizationStartResponse.NotAuthorized(authorizeStartRequest,
                                                                                                               new StatusCode(
                                                                                                                   StatusCodes.RFIDAuthenticationfailed_InvalidUID,
                                                                                                                   "We don't like this UID!",
                                                                                                                   "We don't like this UID!"
                                                                                                               ),
                                                                                                               authorizeStartRequest.SessionId,
                                                                                                               authorizeStartRequest.CPOPartnerSessionId,
                                                                                                               EMPPartnerSession_Id.NewRandom,
                                                                                                               Provider_Id.Parse("DE-XXX"));

                                                 return Task.FromResult(
                                                     new HTTPResponse.Builder(Request) {
                                                         HTTPStatusCode  = HTTPStatusCode.OK,
                                                         ContentType     = HTTPContentType.JSON_UTF8,
                                                         Content         = response.ToJSON().ToString(Newtonsoft.Json.Formatting.None).ToUTF8Bytes(),
                                                         Connection      = "close"
                                                     }.AsImmutable);

                                             }

                                             return Task.FromResult(
                                                 new HTTPResponse.Builder(Request) {

                                                     HTTPStatusCode  = HTTPStatusCode.OK,
                                                     ContentType     = HTTPContentType.TEXT_UTF8,
                                                     Content         = "Hello world!".ToUTF8Bytes(),
                                                     Connection      = "close"

                                                 }.AsImmutable);

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
                                         HTTPDelegate: Request => {

                                             if (SendChargeDetailRecordRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                        out SendChargeDetailRecordRequest sendChargeDetailRecordRequest,
                                                                                        out String ErrorResponse,
                                                                                        null))
                                             {

                                                 var response = new Acknowledgement<SendChargeDetailRecordRequest>(sendChargeDetailRecordRequest,
                                                                                                                   new StatusCode(
                                                                                                                       StatusCodes.Success,
                                                                                                                       "Fine!",
                                                                                                                       "Thank you!"
                                                                                                                   ),
                                                                                                                   true);

                                                 return Task.FromResult(
                                                     new HTTPResponse.Builder(Request) {
                                                         HTTPStatusCode  = HTTPStatusCode.OK,
                                                         ContentType     = HTTPContentType.JSON_UTF8,
                                                         Content         = response.ToJSON().ToString(Newtonsoft.Json.Formatting.None).ToUTF8Bytes(),
                                                         Connection      = "close"
                                                     }.AsImmutable);

                                             }

                                             return Task.FromResult(
                                                 new HTTPResponse.Builder(Request) {

                                                     HTTPStatusCode  = HTTPStatusCode.OK,
                                                     ContentType     = HTTPContentType.TEXT_UTF8,
                                                     Content         = "Hello world!".ToUTF8Bytes(),
                                                     Connection      = "close"

                                                 }.AsImmutable);

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
