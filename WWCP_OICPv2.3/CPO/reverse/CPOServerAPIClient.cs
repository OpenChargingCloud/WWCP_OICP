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
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    /// <summary>
    /// The CPO server API client, aka. the Hubject side of the API.
    /// </summary>
    public partial class CPOServerAPIClient : AHTTPClient//,
                                      //        ICPOServerAPIClient
    {

        #region (class) Counters

        public class Counters
        {

            public APICounterValues  AuthorizeRemoteReservationStart    { get; }
            public APICounterValues  AuthorizeRemoteReservationStop     { get; }
            public APICounterValues  AuthorizeRemoteStart               { get; }
            public APICounterValues  AuthorizeRemoteStop                { get; }

            public Counters(APICounterValues? AuthorizeRemoteReservationStart   = null,
                            APICounterValues? AuthorizeRemoteReservationStop    = null,
                            APICounterValues? AuthorizeRemoteStart              = null,
                            APICounterValues? AuthorizeRemoteStop               = null)
            {

                this.AuthorizeRemoteReservationStart  = AuthorizeRemoteReservationStart ?? new APICounterValues();
                this.AuthorizeRemoteReservationStop   = AuthorizeRemoteReservationStop  ?? new APICounterValues();
                this.AuthorizeRemoteStart             = AuthorizeRemoteStart            ?? new APICounterValues();
                this.AuthorizeRemoteStop              = AuthorizeRemoteStop             ?? new APICounterValues();

            }

            public JObject ToJSON()

                => JSONObject.Create(
                       new JProperty("AuthorizeRemoteReservationStart", AuthorizeRemoteReservationStart.ToJSON()),
                       new JProperty("AuthorizeRemoteReservationStop",  AuthorizeRemoteReservationStop. ToJSON()),
                       new JProperty("AuthorizeRemoteStart",            AuthorizeRemoteStart.           ToJSON()),
                       new JProperty("AuthorizeRemoteStop",             AuthorizeRemoteStop.            ToJSON())
                   );

        }

        #endregion


        #region Data

        /// <summary>
        /// The default HTTP user agent.
        /// </summary>
        public new const        String    DefaultHTTPUserAgent        = "GraphDefined OICP " + Version.Number + " CPO Server API Client";

        /// <summary>
        /// The default timeout for HTTP requests.
        /// </summary>
        public new readonly     TimeSpan  DefaultRequestTimeout       = TimeSpan.FromSeconds(10);

        /// <summary>
        /// The default maximum number of transmission retries for HTTP request.
        /// </summary>
        public new const        UInt16    DefaultMaxNumberOfRetries   = 3;

        /// <summary>
        /// The default remote HTTP URL.
        /// </summary>
        public static readonly  URL       DefaultRemoteURL            = URL.Parse("https://service.hubject-qa.com");

        #endregion

        #region Properties

        public Counters  Counter    { get; }

        /// <summary>
        /// The attached HTTP client logger.
        /// </summary>
        public new Logger HTTPLogger
        {
            get
            {
                return base.HTTPLogger as Logger;
            }
            set
            {
                base.HTTPLogger = value;
            }
        }

        public Newtonsoft.Json.Formatting  JSONFormat    { get; set; }

        #endregion

        #region Events

        #region OnAuthorizeRemoteReservationStart/-Response

        /// <summary>
        /// An event fired whenever an AuthorizeRemoteReservationStart request will be send.
        /// </summary>
        public event OnAuthorizeRemoteReservationStartClientRequestDelegate?   OnAuthorizeRemoteReservationStart;

        /// <summary>
        /// An event fired whenever an AuthorizeRemoteReservationStart HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                                  OnAuthorizeRemoteReservationStartHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeRemoteReservationStart HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                                 OnAuthorizeRemoteReservationStartHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeRemoteReservationStart request had been received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStartClientResponseDelegate?  OnAuthorizeRemoteReservationStartResponse;

        #endregion

        #region OnAuthorizeRemoteReservationStop/-Response

        /// <summary>
        /// An event fired whenever an AuthorizeRemoteReservationStop request will be send.
        /// </summary>
        public event OnAuthorizeRemoteReservationStopClientRequestDelegate?   OnAuthorizeRemoteReservationStop;

        /// <summary>
        /// An event fired whenever an AuthorizeRemoteReservationStop HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                                 OnAuthorizeRemoteReservationStopHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeRemoteReservationStop HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                                OnAuthorizeRemoteReservationStopHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeRemoteReservationStop request had been received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStopClientResponseDelegate?  OnAuthorizeRemoteReservationStopResponse;

        #endregion


        #region OnAuthorizeRemoteStart/-Response

        /// <summary>
        /// An event fired whenever an AuthorizeRemoteStart request will be send.
        /// </summary>
        public event OnAuthorizeRemoteStartClientRequestDelegate?   OnAuthorizeRemoteStart;

        /// <summary>
        /// An event fired whenever an AuthorizeRemoteStart HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                       OnAuthorizeRemoteStartHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeRemoteStart HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                      OnAuthorizeRemoteStartHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeRemoteStart request had been received.
        /// </summary>
        public event OnAuthorizeRemoteStartClientResponseDelegate?  OnAuthorizeRemoteStartResponse;

        #endregion

        #region OnAuthorizeRemoteStop/-Response

        /// <summary>
        /// An event fired whenever an AuthorizeRemoteStop request will be send.
        /// </summary>
        public event OnAuthorizeRemoteStopClientRequestDelegate?   OnAuthorizeRemoteStop;

        /// <summary>
        /// An event fired whenever an AuthorizeRemoteStop HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                      OnAuthorizeRemoteStopHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeRemoteStop HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                     OnAuthorizeRemoteStopHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeRemoteStop request had been received.
        /// </summary>
        public event OnAuthorizeRemoteStopClientResponseDelegate?  OnAuthorizeRemoteStopResponse;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new CPO client.
        /// </summary>
        /// <param name="RemoteURL">The remote URL of the OICP HTTP endpoint to connect to.</param>
        /// <param name="VirtualHostname">An optional HTTP virtual hostname.</param>
        /// <param name="Description">An optional description of this CPO client.</param>
        /// <param name="RemoteCertificateValidator">The remote SSL/TLS certificate validator.</param>
        /// <param name="ClientCertificateSelector">A delegate to select a TLS client certificate.</param>
        /// <param name="ClientCert">The SSL/TLS client certificate to use of HTTP authentication.</param>
        /// <param name="HTTPUserAgent">The HTTP user agent identification.</param>
        /// <param name="RequestTimeout">An optional request timeout.</param>
        /// <param name="TransmissionRetryDelay">The delay between transmission retries.</param>
        /// <param name="MaxNumberOfRetries">The maximum number of transmission retries for HTTP request.</param>
        /// <param name="DisableLogging">Disable all logging.</param>
        /// <param name="LoggingPath">The logging path.</param>
        /// <param name="LoggingContext">An optional context for logging.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// <param name="DNSClient">The DNS client to use.</param>
        public CPOServerAPIClient(URL?                                  RemoteURL                    = null,
                                  HTTPHostname?                         VirtualHostname              = null,
                                  String?                               Description                  = null,
                                  RemoteCertificateValidationCallback?  RemoteCertificateValidator   = null,
                                  LocalCertificateSelectionCallback?    ClientCertificateSelector    = null,
                                  X509Certificate?                      ClientCert                   = null,
                                  SslProtocols?                         TLSProtocol                  = null,
                                  Boolean?                              PreferIPv4                   = null,
                                  String                                HTTPUserAgent                = DefaultHTTPUserAgent,
                                  TimeSpan?                             RequestTimeout               = null,
                                  TransmissionRetryDelayDelegate?       TransmissionRetryDelay       = null,
                                  UInt16?                               MaxNumberOfRetries           = DefaultMaxNumberOfRetries,
                                  Boolean                               DisableLogging               = false,
                                  String?                               LoggingPath                  = null,
                                  String                                LoggingContext               = Logger.DefaultContext,
                                  LogfileCreatorDelegate?               LogfileCreator               = null,
                                  DNSClient?                            DNSClient                    = null)

            : base(RemoteURL           ?? DefaultRemoteURL,
                   VirtualHostname,
                   Description,
                   RemoteCertificateValidator,
                   ClientCertificateSelector,
                   ClientCert,
                   TLSProtocol,
                   PreferIPv4,
                   HTTPUserAgent       ?? DefaultHTTPUserAgent,
                   RequestTimeout,
                   TransmissionRetryDelay,
                   MaxNumberOfRetries  ?? DefaultMaxNumberOfRetries,
                   false,
                   null,
                   DNSClient)

        {

            this.Counter     = new Counters();

            this.JSONFormat  = Newtonsoft.Json.Formatting.None;

            base.HTTPLogger  = DisableLogging == false
                                   ? new Logger(this,
                                                LoggingPath,
                                                LoggingContext,
                                                LogfileCreator)
                                   : null;

        }

        #endregion


        #region AuthorizeRemoteReservationStart(Request)

        /// <summary>
        /// Send an authorize remote reservation start request.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteReservationStart request.</param>
        public async Task<OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>> AuthorizeRemoteReservationStart(AuthorizeRemoteReservationStartRequest Request)
        {

            #region Initial checks

            //Request = _CustomAuthorizeRemoteReservationStartMapper(Request);

            Byte                                                                  TransmissionRetry   = 0;
            OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>?  result              = null;
            var                                                                   processId           = Process_Id.NewRandom;

            #endregion

            #region Send OnAuthorizeRemoteReservationStartClientRequest event

            var startTime = Timestamp.Now;

            Counter.AuthorizeRemoteReservationStart.IncRequests_OK();

            try
            {

                if (OnAuthorizeRemoteReservationStart is not null)
                    await Task.WhenAll(OnAuthorizeRemoteReservationStart.GetInvocationList().
                                       Cast<OnAuthorizeRemoteReservationStartClientRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOServerAPIClient) + "." + nameof(OnAuthorizeRemoteReservationStart));
            }

            #endregion


            try
            {

                do
                {

                    #region Upstream HTTP request...

                    var HTTPResponse = await HTTPClientFactory.Create(RemoteURL,
                                                                      VirtualHostname,
                                                                      Description,
                                                                      RemoteCertificateValidator,
                                                                      ClientCertificateSelector,
                                                                      ClientCert,
                                                                      TLSProtocol,
                                                                      PreferIPv4,
                                                                      HTTPUserAgent,
                                                                      RequestTimeout,
                                                                      TransmissionRetryDelay,
                                                                      MaxNumberOfRetries,
                                                                      false,
                                                                      null,
                                                                      DNSClient).

                                              Execute(client => client.POSTRequest(RemoteURL.Path + ("api/oicp/charging/v21/providers/" + Request.ProviderId.ToString().Replace("*", "%2A") + "/authorize-remote-reservation/start"),
                                                                                   requestbuilder => {
                                                                                       requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                       requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                       requestbuilder.Content      = Request.ToJSON().ToString(JSONFormat).ToUTF8Bytes();
                                                                                       requestbuilder.Connection   = "close";
                                                                                       requestbuilder.Set("Process-ID", processId.ToString());
                                                                                   }),

                                                      RequestLogDelegate:   OnAuthorizeRemoteReservationStartHTTPRequest,
                                                      ResponseLogDelegate:  OnAuthorizeRemoteReservationStartHTTPResponse,
                                                      CancellationToken:    Request.CancellationToken,
                                                      EventTrackingId:      Request.EventTrackingId,
                                                      RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                              ConfigureAwait(false);

                    #endregion


                    // Re-read it from the HTTP response!
                    var processId2 = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse);
                    //ToDo: Verify that processId == processId2!

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                if (Acknowledgement<AuthorizeRemoteReservationStartRequest>.TryParse(Request,
                                                                                                     JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                                                     out Acknowledgement<AuthorizeRemoteReservationStartRequest>?  authorizeRemoteReservationStartResponse,
                                                                                                     out String?                                                   ErrorResponse,
                                                                                                     HTTPResponse,
                                                                                                     HTTPResponse.Timestamp,
                                                                                                     HTTPResponse.EventTrackingId,
                                                                                                     HTTPResponse.Runtime,
                                                                                                     processId))
                                {

                                    result = OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>.Success(Request,
                                                                                                                         authorizeRemoteReservationStartResponse!,
                                                                                                                         processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>.Failed(
                                             Request,
                                             Acknowledgement<AuthorizeRemoteReservationStartRequest>.SystemError(
                                                 Request,
                                                 e.Message,
                                                 e.StackTrace,
                                                 Request.SessionId,
                                                 Request.CPOPartnerSessionId,
                                                 Request.EMPPartnerSessionId,
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 HTTPResponse.Runtime,
                                                 processId,
                                                 HTTPResponse,
                                                 Request.CustomData
                                             ),
                                             processId
                                         );

                            }

                        }

                        TransmissionRetry = Byte.MaxValue - 1;
                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.BadRequest)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            // HTTP/1.1 400 BadRequest
                            // Server:             nginx/1.18.0
                            // Date:               Fri, 08 Jan 2021 14:19:25 GMT
                            // Content-Type:       application/json;charset=utf-8
                            // Transfer-Encoding:  chunked
                            // Connection:         keep-alive
                            // Process-ID:         b87fd67b-2d74-4318-86cf-0d2c2c50cabb
                            // 
                            // {
                            //     "extendedInfo":  null,
                            //     "message":      "Error parsing/validating JSON.",
                            //     "validationErrors": [
                            //         {
                            //             "fieldReference": "operatorEvseData.evseDataRecord[0].hotlinePhoneNumber",
                            //             "errorMessage":   "must match \"^\\+[0-9]{5,15}$\""
                            //         },
                            //         {
                            //             "fieldReference": "operatorEvseData.evseDataRecord[0].geoCoordinates",
                            //             "errorMessage":   "may not be null"
                            //         },
                            //         {
                            //             "fieldReference": "operatorEvseData.evseDataRecord[0].chargingStationNames",
                            //             "errorMessage":   "may not be empty"
                            //         },
                            //         {
                            //             "fieldReference": "operatorEvseData.evseDataRecord[0].plugs",
                            //             "errorMessage":   "may not be empty"
                            //         }
                            //     ]
                            // }

                            if (ValidationErrorList.TryParse(HTTPResponse.HTTPBody?.ToUTF8String(),
                                                             out ValidationErrorList?  validationErrorList,
                                                             out String?               errorResponse))
                            {

                                result = OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>.BadRequest(Request,
                                                                                                                        validationErrorList,
                                                                                                                        processId);

                            }

                        }

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                    {

                        // Hubject firewall problem!
                        // Only HTML response!
                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Unauthorized)
                    {

                        // HTTP/1.1 401 Unauthorized
                        // Server:          nginx/1.18.0 (Ubuntu)
                        // Date:            Tue, 02 Mar 2021 23:09:35 GMT
                        // Content-Type:    application/json;charset=UTF-8
                        // Content-Length:  87
                        // Connection:      keep-alive
                        // Process-ID:      cefd3dfc-8807-4160-8913-d3153dfea8ab
                        // 
                        // {
                        //     "StatusCode": {
                        //         "Code":            "017",
                        //         "Description":     "Unauthorized Access",
                        //         "AdditionalInfo":   null
                        //     }
                        // }

                        // Operator/provider identification is not linked to the TLS client certificate!

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                if (StatusCode.TryParse(JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String())["StatusCode"] as JObject,
                                                        out StatusCode?  statusCode,
                                                        out String?      ErrorResponse))
                                {

                                    result = OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>.Failed(Request,
                                                                                                                        Acknowledgement<AuthorizeRemoteReservationStartRequest>.DataError(
                                                                                                                            Request,
                                                                                                                            statusCode!.Description,
                                                                                                                            statusCode!.AdditionalInfo,
                                                                                                                            Request.SessionId,
                                                                                                                            Request.CPOPartnerSessionId,
                                                                                                                            Request.EMPPartnerSessionId,
                                                                                                                            HTTPResponse.Timestamp,
                                                                                                                            HTTPResponse.EventTrackingId,
                                                                                                                            HTTPResponse.Runtime,
                                                                                                                            processId,
                                                                                                                            HTTPResponse,
                                                                                                                            Request.CustomData
                                                                                                                        ),
                                                                                                                        processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>.Failed(
                                             Request,
                                             Acknowledgement<AuthorizeRemoteReservationStartRequest>.SystemError(
                                                 Request,
                                                 e.Message,
                                                 e.StackTrace,
                                                 Request.SessionId,
                                                 Request.CPOPartnerSessionId,
                                                 Request.EMPPartnerSessionId,
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 HTTPResponse.Runtime,
                                                 processId,
                                                 HTTPResponse,
                                                 Request.CustomData
                                             )
                                         );

                            }

                        }

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.RequestTimeout)
                    { }

                }
                while (TransmissionRetry++ < MaxNumberOfRetries);

            }
            catch (Exception e)
            {

                result = OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>.Failed(
                             Request,
                             Acknowledgement<AuthorizeRemoteReservationStartRequest>.SystemError(
                                 Request,
                                 e.Message,
                                 e.StackTrace,
                                 Request.SessionId,
                                 Request.CPOPartnerSessionId,
                                 Request.EMPPartnerSessionId,
                                 Timestamp.Now,
                                 Request.EventTrackingId,
                                 Timestamp.Now - Request.Timestamp,
                                 processId,
                                 null, // HTTPResponse
                                 Request.CustomData
                             )
                         );

            }

            result ??= OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>.Failed(
                           Request,
                           Acknowledgement<AuthorizeRemoteReservationStartRequest>.SystemError(
                               Request,
                               "HTTP request failed!",
                               null,
                               Request.SessionId,
                               Request.CPOPartnerSessionId,
                               Request.EMPPartnerSessionId,
                               Timestamp.Now,
                               Request.EventTrackingId,
                               Timestamp.Now - Request.Timestamp,
                               processId,
                               null, // HTTPResponse
                               Request.CustomData
                           )
                       );


            #region Send OnAuthorizeRemoteReservationStartClientResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnAuthorizeRemoteReservationStartResponse != null)
                    await Task.WhenAll(OnAuthorizeRemoteReservationStartResponse.GetInvocationList().
                                       Cast<OnAuthorizeRemoteReservationStartClientResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     result,
                                                     result.Runtime ?? TimeSpan.Zero))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOServerAPIClient) + "." + nameof(OnAuthorizeRemoteReservationStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeRemoteReservationStop (Request)

        /// <summary>
        /// Send an authorize remote reservation stop request.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteReservationStop request.</param>
        public async Task<OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>> AuthorizeRemoteReservationStop(AuthorizeRemoteReservationStopRequest Request)
        {

            #region Initial checks

            //Request = _CustomAuthorizeRemoteReservationStopMapper(Request);

            Byte                                                                 TransmissionRetry   = 0;
            OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>?  result              = null;
            var                                                                  processId           = Process_Id.NewRandom;

            #endregion

            #region Send OnAuthorizeRemoteReservationStopClientRequest event

            var startTime = Timestamp.Now;

            Counter.AuthorizeRemoteReservationStop.IncRequests_OK();

            try
            {

                if (OnAuthorizeRemoteReservationStop is not null)
                    await Task.WhenAll(OnAuthorizeRemoteReservationStop.GetInvocationList().
                                       Cast<OnAuthorizeRemoteReservationStopClientRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOServerAPIClient) + "." + nameof(OnAuthorizeRemoteReservationStop));
            }

            #endregion


            try
            {

                do
                {

                    #region Upstream HTTP request...

                    var HTTPResponse = await HTTPClientFactory.Create(RemoteURL,
                                                                      VirtualHostname,
                                                                      Description,
                                                                      RemoteCertificateValidator,
                                                                      ClientCertificateSelector,
                                                                      ClientCert,
                                                                      TLSProtocol,
                                                                      PreferIPv4,
                                                                      HTTPUserAgent,
                                                                      RequestTimeout,
                                                                      TransmissionRetryDelay,
                                                                      MaxNumberOfRetries,
                                                                      false,
                                                                      null,
                                                                      DNSClient).

                                              Execute(client => client.POSTRequest(RemoteURL.Path + ("api/oicp/charging/v21/providers/" + Request.ProviderId.ToString().Replace("*", "%2A") + "/authorize-remote-reservation/stop"),
                                                                                   requestbuilder => {
                                                                                       requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                       requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                       requestbuilder.Content      = Request.ToJSON().ToString(JSONFormat).ToUTF8Bytes();
                                                                                       requestbuilder.Connection   = "close";
                                                                                       requestbuilder.Set("Process-ID", processId.ToString());
                                                                                   }),

                                                      RequestLogDelegate:   OnAuthorizeRemoteReservationStopHTTPRequest,
                                                      ResponseLogDelegate:  OnAuthorizeRemoteReservationStopHTTPResponse,
                                                      CancellationToken:    Request.CancellationToken,
                                                      EventTrackingId:      Request.EventTrackingId,
                                                      RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                              ConfigureAwait(false);

                    #endregion


                    // Re-read it from the HTTP response!
                    var processId2 = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse);
                    //ToDo: Verify that processId == processId2!

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                if (Acknowledgement<AuthorizeRemoteReservationStopRequest>.TryParse(Request,
                                                                                                    JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                                                    out Acknowledgement<AuthorizeRemoteReservationStopRequest>?  authorizeRemoteReservationStopResponse,
                                                                                                    out String?                                                  ErrorResponse,
                                                                                                    HTTPResponse,
                                                                                                    HTTPResponse.Timestamp,
                                                                                                    HTTPResponse.EventTrackingId,
                                                                                                    HTTPResponse.Runtime,
                                                                                                    processId))
                                {

                                    result = OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>.Success(Request,
                                                                                                                        authorizeRemoteReservationStopResponse!,
                                                                                                                        processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>.Failed(
                                             Request,
                                             Acknowledgement<AuthorizeRemoteReservationStopRequest>.SystemError(
                                                 Request,
                                                 e.Message,
                                                 e.StackTrace,
                                                 Request.SessionId,
                                                 Request.CPOPartnerSessionId,
                                                 Request.EMPPartnerSessionId,
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 HTTPResponse.Runtime,
                                                 processId,
                                                 HTTPResponse,
                                                 Request.CustomData
                                             ),
                                             processId
                                         );

                            }

                        }

                        TransmissionRetry = Byte.MaxValue - 1;
                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.BadRequest)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            // HTTP/1.1 400 BadRequest
                            // Server:             nginx/1.18.0
                            // Date:               Fri, 08 Jan 2021 14:19:25 GMT
                            // Content-Type:       application/json;charset=utf-8
                            // Transfer-Encoding:  chunked
                            // Connection:         keep-alive
                            // Process-ID:         b87fd67b-2d74-4318-86cf-0d2c2c50cabb
                            // 
                            // {
                            //     "extendedInfo":  null,
                            //     "message":      "Error parsing/validating JSON.",
                            //     "validationErrors": [
                            //         {
                            //             "fieldReference": "operatorEvseData.evseDataRecord[0].hotlinePhoneNumber",
                            //             "errorMessage":   "must match \"^\\+[0-9]{5,15}$\""
                            //         },
                            //         {
                            //             "fieldReference": "operatorEvseData.evseDataRecord[0].geoCoordinates",
                            //             "errorMessage":   "may not be null"
                            //         },
                            //         {
                            //             "fieldReference": "operatorEvseData.evseDataRecord[0].chargingStationNames",
                            //             "errorMessage":   "may not be empty"
                            //         },
                            //         {
                            //             "fieldReference": "operatorEvseData.evseDataRecord[0].plugs",
                            //             "errorMessage":   "may not be empty"
                            //         }
                            //     ]
                            // }

                            if (ValidationErrorList.TryParse(HTTPResponse.HTTPBody?.ToUTF8String(),
                                                             out ValidationErrorList?  validationErrorList,
                                                             out String?               errorResponse))
                            {

                                result = OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>.BadRequest(Request,
                                                                                                                       validationErrorList,
                                                                                                                       processId);

                            }

                        }

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                    {

                        // Hubject firewall problem!
                        // Only HTML response!
                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Unauthorized)
                    {

                        // HTTP/1.1 401 Unauthorized
                        // Server:          nginx/1.18.0 (Ubuntu)
                        // Date:            Tue, 02 Mar 2021 23:09:35 GMT
                        // Content-Type:    application/json;charset=UTF-8
                        // Content-Length:  87
                        // Connection:      keep-alive
                        // Process-ID:      cefd3dfc-8807-4160-8913-d3153dfea8ab
                        // 
                        // {
                        //     "StatusCode": {
                        //         "Code":            "017",
                        //         "Description":     "Unauthorized Access",
                        //         "AdditionalInfo":   null
                        //     }
                        // }

                        // Operator/provider identification is not linked to the TLS client certificate!

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                if (StatusCode.TryParse(JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String())["StatusCode"] as JObject,
                                                        out StatusCode?  statusCode,
                                                        out String?      ErrorResponse))
                                {

                                    result = OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>.Failed(Request,
                                                                                                                       Acknowledgement<AuthorizeRemoteReservationStopRequest>.DataError(
                                                                                                                           Request,
                                                                                                                           statusCode!.Description,
                                                                                                                           statusCode!.AdditionalInfo,
                                                                                                                           Request.SessionId,
                                                                                                                           Request.CPOPartnerSessionId,
                                                                                                                           Request.EMPPartnerSessionId,
                                                                                                                           HTTPResponse.Timestamp,
                                                                                                                           HTTPResponse.EventTrackingId,
                                                                                                                           HTTPResponse.Runtime,
                                                                                                                           processId,
                                                                                                                           HTTPResponse,
                                                                                                                           Request.CustomData
                                                                                                                       ),
                                                                                                                       processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>.Failed(
                                             Request,
                                             Acknowledgement<AuthorizeRemoteReservationStopRequest>.SystemError(
                                                 Request,
                                                 e.Message,
                                                 e.StackTrace,
                                                 Request.SessionId,
                                                 Request.CPOPartnerSessionId,
                                                 Request.EMPPartnerSessionId,
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 HTTPResponse.Runtime,
                                                 processId,
                                                 HTTPResponse,
                                                 Request.CustomData
                                             )
                                         );

                            }

                        }

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.RequestTimeout)
                    { }

                }
                while (TransmissionRetry++ < MaxNumberOfRetries);

            }
            catch (Exception e)
            {

                result = OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>.Failed(
                             Request,
                             Acknowledgement<AuthorizeRemoteReservationStopRequest>.SystemError(
                                 Request,
                                 e.Message,
                                 e.StackTrace,
                                 Request.SessionId,
                                 Request.CPOPartnerSessionId,
                                 Request.EMPPartnerSessionId,
                                 Timestamp.Now,
                                 Request.EventTrackingId,
                                 Timestamp.Now - Request.Timestamp,
                                 processId,
                                 null, // HTTPResponse
                                 Request.CustomData
                             )
                         );

            }

            result ??= OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>.Failed(
                           Request,
                           Acknowledgement<AuthorizeRemoteReservationStopRequest>.SystemError(
                               Request,
                               "HTTP request failed!",
                               null,
                               Request.SessionId,
                               Request.CPOPartnerSessionId,
                               Request.EMPPartnerSessionId,
                               Timestamp.Now,
                               Request.EventTrackingId,
                               Timestamp.Now - Request.Timestamp,
                               processId,
                               null, // HTTPResponse
                               Request.CustomData
                           )
                       );


            #region Send OnAuthorizeRemoteReservationStopClientResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnAuthorizeRemoteReservationStopResponse != null)
                    await Task.WhenAll(OnAuthorizeRemoteReservationStopResponse.GetInvocationList().
                                       Cast<OnAuthorizeRemoteReservationStopClientResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     result,
                                                     result.Runtime ?? TimeSpan.Zero))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOServerAPIClient) + "." + nameof(OnAuthorizeRemoteReservationStopResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region AuthorizeRemoteStart(Request)

        /// <summary>
        /// Send an authorize remote reservation start request.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteStart request.</param>
        public async Task<OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>> AuthorizeRemoteStart(AuthorizeRemoteStartRequest Request)
        {

            #region Initial checks

            //Request = _CustomAuthorizeRemoteStartMapper(Request);

            Byte                                                       TransmissionRetry   = 0;
            OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>?  result              = null;
            var                                                        processId           = Process_Id.NewRandom;

            #endregion

            #region Send OnAuthorizeRemoteStartClientRequest event

            var startTime = Timestamp.Now;

            Counter.AuthorizeRemoteStart.IncRequests_OK();

            try
            {

                if (OnAuthorizeRemoteStart is not null)
                    await Task.WhenAll(OnAuthorizeRemoteStart.GetInvocationList().
                                       Cast<OnAuthorizeRemoteStartClientRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOServerAPIClient) + "." + nameof(OnAuthorizeRemoteStart));
            }

            #endregion


            try
            {

                do
                {

                    #region Upstream HTTP request...

                    var HTTPResponse = await HTTPClientFactory.Create(RemoteURL,
                                                                      VirtualHostname,
                                                                      Description,
                                                                      RemoteCertificateValidator,
                                                                      ClientCertificateSelector,
                                                                      ClientCert,
                                                                      TLSProtocol,
                                                                      PreferIPv4,
                                                                      HTTPUserAgent,
                                                                      RequestTimeout,
                                                                      TransmissionRetryDelay,
                                                                      MaxNumberOfRetries,
                                                                      false,
                                                                      null,
                                                                      DNSClient).

                                              Execute(client => client.POSTRequest(RemoteURL.Path + ("api/oicp/charging/v21/providers/" + Request.ProviderId.ToString().Replace("*", "%2A") + "/authorize-remote/start"),
                                                                                   requestbuilder => {
                                                                                       requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                       requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                       requestbuilder.Content      = Request.ToJSON().ToString(JSONFormat).ToUTF8Bytes();
                                                                                       requestbuilder.Connection   = "close";
                                                                                       requestbuilder.Set("Process-ID", processId.ToString());
                                                                                   }),

                                                      RequestLogDelegate:   OnAuthorizeRemoteStartHTTPRequest,
                                                      ResponseLogDelegate:  OnAuthorizeRemoteStartHTTPResponse,
                                                      CancellationToken:    Request.CancellationToken,
                                                      EventTrackingId:      Request.EventTrackingId,
                                                      RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                              ConfigureAwait(false);

                    #endregion


                    // Re-read it from the HTTP response!
                    var processId2 = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse);
                    //ToDo: Verify that processId == processId2!

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                if (Acknowledgement<AuthorizeRemoteStartRequest>.TryParse(Request,
                                                                                          JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                                          out Acknowledgement<AuthorizeRemoteStartRequest>?  authorizeRemoteStartResponse,
                                                                                          out String?                                        ErrorResponse,
                                                                                          HTTPResponse,
                                                                                          HTTPResponse.Timestamp,
                                                                                          HTTPResponse.EventTrackingId,
                                                                                          HTTPResponse.Runtime,
                                                                                          processId))
                                {

                                    result = OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>.Success(Request,
                                                                                                              authorizeRemoteStartResponse!,
                                                                                                              processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>.Failed(
                                             Request,
                                             Acknowledgement<AuthorizeRemoteStartRequest>.SystemError(
                                                 Request,
                                                 e.Message,
                                                 e.StackTrace,
                                                 Request.SessionId,
                                                 Request.CPOPartnerSessionId,
                                                 Request.EMPPartnerSessionId,
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 HTTPResponse.Runtime,
                                                 processId,
                                                 HTTPResponse,
                                                 Request.CustomData
                                             ),
                                             processId
                                         );

                            }

                        }

                        TransmissionRetry = Byte.MaxValue - 1;
                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.BadRequest)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            // HTTP/1.1 400 BadRequest
                            // Server:             nginx/1.18.0
                            // Date:               Fri, 08 Jan 2021 14:19:25 GMT
                            // Content-Type:       application/json;charset=utf-8
                            // Transfer-Encoding:  chunked
                            // Connection:         keep-alive
                            // Process-ID:         b87fd67b-2d74-4318-86cf-0d2c2c50cabb
                            // 
                            // {
                            //     "extendedInfo":  null,
                            //     "message":      "Error parsing/validating JSON.",
                            //     "validationErrors": [
                            //         {
                            //             "fieldReference": "operatorEvseData.evseDataRecord[0].hotlinePhoneNumber",
                            //             "errorMessage":   "must match \"^\\+[0-9]{5,15}$\""
                            //         },
                            //         {
                            //             "fieldReference": "operatorEvseData.evseDataRecord[0].geoCoordinates",
                            //             "errorMessage":   "may not be null"
                            //         },
                            //         {
                            //             "fieldReference": "operatorEvseData.evseDataRecord[0].chargingStationNames",
                            //             "errorMessage":   "may not be empty"
                            //         },
                            //         {
                            //             "fieldReference": "operatorEvseData.evseDataRecord[0].plugs",
                            //             "errorMessage":   "may not be empty"
                            //         }
                            //     ]
                            // }

                            if (ValidationErrorList.TryParse(HTTPResponse.HTTPBody?.ToUTF8String(),
                                                             out ValidationErrorList?  validationErrorList,
                                                             out String?               errorResponse))
                            {

                                result = OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>.BadRequest(Request,
                                                                                                                        validationErrorList,
                                                                                                                        processId);

                            }

                        }

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                    {

                        // Hubject firewall problem!
                        // Only HTML response!
                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Unauthorized)
                    {

                        // HTTP/1.1 401 Unauthorized
                        // Server:          nginx/1.18.0 (Ubuntu)
                        // Date:            Tue, 02 Mar 2021 23:09:35 GMT
                        // Content-Type:    application/json;charset=UTF-8
                        // Content-Length:  87
                        // Connection:      keep-alive
                        // Process-ID:      cefd3dfc-8807-4160-8913-d3153dfea8ab
                        // 
                        // {
                        //     "StatusCode": {
                        //         "Code":            "017",
                        //         "Description":     "Unauthorized Access",
                        //         "AdditionalInfo":   null
                        //     }
                        // }

                        // Operator/provider identification is not linked to the TLS client certificate!

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                if (StatusCode.TryParse(JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String())["StatusCode"] as JObject,
                                                        out StatusCode?  statusCode,
                                                        out String?      ErrorResponse))
                                {

                                    result = OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>.Failed(Request,
                                                                                                                        Acknowledgement<AuthorizeRemoteStartRequest>.DataError(
                                                                                                                            Request,
                                                                                                                            statusCode!.Description,
                                                                                                                            statusCode!.AdditionalInfo,
                                                                                                                            Request.SessionId,
                                                                                                                            Request.CPOPartnerSessionId,
                                                                                                                            Request.EMPPartnerSessionId,
                                                                                                                            HTTPResponse.Timestamp,
                                                                                                                            HTTPResponse.EventTrackingId,
                                                                                                                            HTTPResponse.Runtime,
                                                                                                                            processId,
                                                                                                                            HTTPResponse,
                                                                                                                            Request.CustomData
                                                                                                                        ),
                                                                                                                        processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>.Failed(
                                             Request,
                                             Acknowledgement<AuthorizeRemoteStartRequest>.SystemError(
                                                 Request,
                                                 e.Message,
                                                 e.StackTrace,
                                                 Request.SessionId,
                                                 Request.CPOPartnerSessionId,
                                                 Request.EMPPartnerSessionId,
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 HTTPResponse.Runtime,
                                                 processId,
                                                 HTTPResponse,
                                                 Request.CustomData
                                             )
                                         );

                            }

                        }

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.RequestTimeout)
                    { }

                }
                while (TransmissionRetry++ < MaxNumberOfRetries);

            }
            catch (Exception e)
            {

                result = OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>.Failed(
                             Request,
                             Acknowledgement<AuthorizeRemoteStartRequest>.SystemError(
                                 Request,
                                 e.Message,
                                 e.StackTrace,
                                 Request.SessionId,
                                 Request.CPOPartnerSessionId,
                                 Request.EMPPartnerSessionId,
                                 Timestamp.Now,
                                 Request.EventTrackingId,
                                 Timestamp.Now - Request.Timestamp,
                                 processId,
                                 null, // HTTPResponse
                                 Request.CustomData
                             )
                         );

            }

            result ??= OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>.Failed(
                           Request,
                           Acknowledgement<AuthorizeRemoteStartRequest>.SystemError(
                               Request,
                               "HTTP request failed!",
                               null,
                               Request.SessionId,
                               Request.CPOPartnerSessionId,
                               Request.EMPPartnerSessionId,
                               Timestamp.Now,
                               Request.EventTrackingId,
                               Timestamp.Now - Request.Timestamp,
                               processId,
                               null, // HTTPResponse
                               Request.CustomData
                           )
                       );


            #region Send OnAuthorizeRemoteStartClientResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnAuthorizeRemoteStartResponse != null)
                    await Task.WhenAll(OnAuthorizeRemoteStartResponse.GetInvocationList().
                                       Cast<OnAuthorizeRemoteStartClientResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     result,
                                                     result.Runtime ?? TimeSpan.Zero))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOServerAPIClient) + "." + nameof(OnAuthorizeRemoteStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeRemoteStop (Request)

        /// <summary>
        /// Send an authorize remote reservation stop request.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteStop request.</param>
        public async Task<OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>> AuthorizeRemoteStop(AuthorizeRemoteStopRequest Request)
        {

            #region Initial checks

            //Request = _CustomAuthorizeRemoteStopMapper(Request);

            Byte                                                      TransmissionRetry   = 0;
            OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>?  result              = null;
            var                                                       processId           = Process_Id.NewRandom;

            #endregion

            #region Send OnAuthorizeRemoteStopClientRequest event

            var startTime = Timestamp.Now;

            Counter.AuthorizeRemoteStop.IncRequests_OK();

            try
            {

                if (OnAuthorizeRemoteStop is not null)
                    await Task.WhenAll(OnAuthorizeRemoteStop.GetInvocationList().
                                       Cast<OnAuthorizeRemoteStopClientRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOServerAPIClient) + "." + nameof(OnAuthorizeRemoteStop));
            }

            #endregion


            try
            {

                do
                {

                    #region Upstream HTTP request...

                    var HTTPResponse = await HTTPClientFactory.Create(RemoteURL,
                                                                      VirtualHostname,
                                                                      Description,
                                                                      RemoteCertificateValidator,
                                                                      ClientCertificateSelector,
                                                                      ClientCert,
                                                                      TLSProtocol,
                                                                      PreferIPv4,
                                                                      HTTPUserAgent,
                                                                      RequestTimeout,
                                                                      TransmissionRetryDelay,
                                                                      MaxNumberOfRetries,
                                                                      false,
                                                                      null,
                                                                      DNSClient).

                                              Execute(client => client.POSTRequest(RemoteURL.Path + ("api/oicp/charging/v21/providers/" + Request.ProviderId.ToString().Replace("*", "%2A") + "/authorize-remote/stop"),
                                                                                   requestbuilder => {
                                                                                       requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                       requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                       requestbuilder.Content      = Request.ToJSON().ToString(JSONFormat).ToUTF8Bytes();
                                                                                       requestbuilder.Connection   = "close";
                                                                                       requestbuilder.Set("Process-ID", processId.ToString());
                                                                                   }),

                                                      RequestLogDelegate:   OnAuthorizeRemoteStopHTTPRequest,
                                                      ResponseLogDelegate:  OnAuthorizeRemoteStopHTTPResponse,
                                                      CancellationToken:    Request.CancellationToken,
                                                      EventTrackingId:      Request.EventTrackingId,
                                                      RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                              ConfigureAwait(false);

                    #endregion


                    // Re-read it from the HTTP response!
                    var processId2 = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse);
                    //ToDo: Verify that processId == processId2!

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                if (Acknowledgement<AuthorizeRemoteStopRequest>.TryParse(Request,
                                                                                         JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                                         out Acknowledgement<AuthorizeRemoteStopRequest>?  authorizeRemoteStopResponse,
                                                                                         out String?                                       ErrorResponse,
                                                                                         HTTPResponse,
                                                                                         HTTPResponse.Timestamp,
                                                                                         HTTPResponse.EventTrackingId,
                                                                                         HTTPResponse.Runtime,
                                                                                         processId))
                                {

                                    result = OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>.Success(Request,
                                                                                                             authorizeRemoteStopResponse!,
                                                                                                             processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>.Failed(
                                             Request,
                                             Acknowledgement<AuthorizeRemoteStopRequest>.SystemError(
                                                 Request,
                                                 e.Message,
                                                 e.StackTrace,
                                                 Request.SessionId,
                                                 Request.CPOPartnerSessionId,
                                                 Request.EMPPartnerSessionId,
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 HTTPResponse.Runtime,
                                                 processId,
                                                 HTTPResponse,
                                                 Request.CustomData
                                             ),
                                             processId
                                         );

                            }

                        }

                        TransmissionRetry = Byte.MaxValue - 1;
                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.BadRequest)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            // HTTP/1.1 400 BadRequest
                            // Server:             nginx/1.18.0
                            // Date:               Fri, 08 Jan 2021 14:19:25 GMT
                            // Content-Type:       application/json;charset=utf-8
                            // Transfer-Encoding:  chunked
                            // Connection:         keep-alive
                            // Process-ID:         b87fd67b-2d74-4318-86cf-0d2c2c50cabb
                            // 
                            // {
                            //     "extendedInfo":  null,
                            //     "message":      "Error parsing/validating JSON.",
                            //     "validationErrors": [
                            //         {
                            //             "fieldReference": "operatorEvseData.evseDataRecord[0].hotlinePhoneNumber",
                            //             "errorMessage":   "must match \"^\\+[0-9]{5,15}$\""
                            //         },
                            //         {
                            //             "fieldReference": "operatorEvseData.evseDataRecord[0].geoCoordinates",
                            //             "errorMessage":   "may not be null"
                            //         },
                            //         {
                            //             "fieldReference": "operatorEvseData.evseDataRecord[0].chargingStationNames",
                            //             "errorMessage":   "may not be empty"
                            //         },
                            //         {
                            //             "fieldReference": "operatorEvseData.evseDataRecord[0].plugs",
                            //             "errorMessage":   "may not be empty"
                            //         }
                            //     ]
                            // }

                            if (ValidationErrorList.TryParse(HTTPResponse.HTTPBody?.ToUTF8String(),
                                                             out ValidationErrorList?  validationErrorList,
                                                             out String?               errorResponse))
                            {

                                result = OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>.BadRequest(Request,
                                                                                                                       validationErrorList,
                                                                                                                       processId);

                            }

                        }

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                    {

                        // Hubject firewall problem!
                        // Only HTML response!
                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Unauthorized)
                    {

                        // HTTP/1.1 401 Unauthorized
                        // Server:          nginx/1.18.0 (Ubuntu)
                        // Date:            Tue, 02 Mar 2021 23:09:35 GMT
                        // Content-Type:    application/json;charset=UTF-8
                        // Content-Length:  87
                        // Connection:      keep-alive
                        // Process-ID:      cefd3dfc-8807-4160-8913-d3153dfea8ab
                        // 
                        // {
                        //     "StatusCode": {
                        //         "Code":            "017",
                        //         "Description":     "Unauthorized Access",
                        //         "AdditionalInfo":   null
                        //     }
                        // }

                        // Operator/provider identification is not linked to the TLS client certificate!

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                if (StatusCode.TryParse(JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String())["StatusCode"] as JObject,
                                                        out StatusCode?  statusCode,
                                                        out String?      ErrorResponse))
                                {

                                    result = OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>.Failed(Request,
                                                                                                                       Acknowledgement<AuthorizeRemoteStopRequest>.DataError(
                                                                                                                           Request,
                                                                                                                           statusCode!.Description,
                                                                                                                           statusCode!.AdditionalInfo,
                                                                                                                           Request.SessionId,
                                                                                                                           Request.CPOPartnerSessionId,
                                                                                                                           Request.EMPPartnerSessionId,
                                                                                                                           HTTPResponse.Timestamp,
                                                                                                                           HTTPResponse.EventTrackingId,
                                                                                                                           HTTPResponse.Runtime,
                                                                                                                           processId,
                                                                                                                           HTTPResponse,
                                                                                                                           Request.CustomData
                                                                                                                       ),
                                                                                                                       processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>.Failed(
                                             Request,
                                             Acknowledgement<AuthorizeRemoteStopRequest>.SystemError(
                                                 Request,
                                                 e.Message,
                                                 e.StackTrace,
                                                 Request.SessionId,
                                                 Request.CPOPartnerSessionId,
                                                 Request.EMPPartnerSessionId,
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 HTTPResponse.Runtime,
                                                 processId,
                                                 HTTPResponse,
                                                 Request.CustomData
                                             )
                                         );

                            }

                        }

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.RequestTimeout)
                    { }

                }
                while (TransmissionRetry++ < MaxNumberOfRetries);

            }
            catch (Exception e)
            {

                result = OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>.Failed(
                             Request,
                             Acknowledgement<AuthorizeRemoteStopRequest>.SystemError(
                                 Request,
                                 e.Message,
                                 e.StackTrace,
                                 Request.SessionId,
                                 Request.CPOPartnerSessionId,
                                 Request.EMPPartnerSessionId,
                                 Timestamp.Now,
                                 Request.EventTrackingId,
                                 Timestamp.Now - Request.Timestamp,
                                 processId,
                                 null, // HTTPResponse
                                 Request.CustomData
                             )
                         );

            }

            result ??= OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>.Failed(
                           Request,
                           Acknowledgement<AuthorizeRemoteStopRequest>.SystemError(
                               Request,
                               "HTTP request failed!",
                               null,
                               Request.SessionId,
                               Request.CPOPartnerSessionId,
                               Request.EMPPartnerSessionId,
                               Timestamp.Now,
                               Request.EventTrackingId,
                               Timestamp.Now - Request.Timestamp,
                               processId,
                               null, // HTTPResponse
                               Request.CustomData
                           )
                       );


            #region Send OnAuthorizeRemoteStopClientResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnAuthorizeRemoteStopResponse != null)
                    await Task.WhenAll(OnAuthorizeRemoteStopResponse.GetInvocationList().
                                       Cast<OnAuthorizeRemoteStopClientResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     result,
                                                     result.Runtime ?? TimeSpan.Zero))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOServerAPIClient) + "." + nameof(OnAuthorizeRemoteStopResponse));
            }

            #endregion

            return result;

        }

        #endregion


    }

}
