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

using System.Diagnostics;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Logging;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    /// <summary>
    /// The CPO server API client, aka. the Hubject side of the API.
    /// </summary>
    public partial class CPOServerAPIClient : AOICPClient//,
                                      //        ICPOServerAPIClient
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
        public new const        String    DefaultHTTPUserAgent        = "GraphDefined OICP " + Version.String + " CPO Server API Client";

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

        #region Custom JSON parsers

        public CustomJObjectParserDelegate<Acknowledgement<AuthorizeRemoteReservationStartRequest>>?  CustomAuthorizeRemoteReservationStartAcknowledgementParser    { get; set; }

        public CustomJObjectParserDelegate<Acknowledgement<AuthorizeRemoteReservationStopRequest>>?   CustomAuthorizeRemoteReservationStopAcknowledgementParser     { get; set; }


        public CustomJObjectParserDelegate<Acknowledgement<AuthorizeRemoteStartRequest>>?             CustomAuthorizeRemoteStartAcknowledgementParser               { get; set; }

        public CustomJObjectParserDelegate<Acknowledgement<AuthorizeRemoteStopRequest>>?              CustomAuthorizeRemoteStopAcknowledgementParser                { get; set; }

        #endregion

        #region Custom JSON serializers
        public CustomJObjectSerializerDelegate<AuthorizeRemoteReservationStartRequest>?  CustomAuthorizeRemoteReservationStartRequestSerializer     { get; set; }
        public CustomJObjectSerializerDelegate<AuthorizeRemoteReservationStopRequest>?   CustomAuthorizeRemoteReservationStopRequestSerializer      { get; set; }

        public CustomJObjectSerializerDelegate<AuthorizeRemoteStartRequest>?             CustomAuthorizeRemoteStartRequestSerializer                { get; set; }
        public CustomJObjectSerializerDelegate<AuthorizeRemoteStopRequest>?              CustomAuthorizeRemoteStopRequestSerializer                 { get; set; }

        public CustomJObjectSerializerDelegate<Identification>?                          CustomIdentificationSerializer                             { get; set; }

        #endregion

        #region Custom request/response logging converters

        #region AuthorizeRemoteReservationStart(Request/Response)Converter

        public Func<DateTimeOffset, Object, AuthorizeRemoteReservationStartRequest, String>
            AuthorizeRemoteReservationStartRequestConverter         { get; set; }

            = (timestamp, sender, authorizeRemoteReservationStartRequest)
            => String.Concat(authorizeRemoteReservationStartRequest.Identification, " at ", authorizeRemoteReservationStartRequest.EVSEId);

        public Func<DateTimeOffset, Object, AuthorizeRemoteReservationStartRequest, OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>, TimeSpan, String>
            AuthorizeRemoteReservationStartResponseConverter        { get; set; }

            = (timestamp, sender, authorizeRemoteReservationStartRequest, authorizeRemoteReservationStartResponse, runtime)
            => String.Concat(authorizeRemoteReservationStartRequest.Identification, " at ", authorizeRemoteReservationStartRequest.EVSEId, " => ", authorizeRemoteReservationStartResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion

        #region AuthorizeRemoteReservationStop (Request/Response)Converter

        public Func<DateTimeOffset, Object, AuthorizeRemoteReservationStopRequest, String>
            AuthorizeRemoteReservationStopRequestConverter         { get; set; }

            = (timestamp, sender, authorizeRemoteReservationStopRequest)
            => String.Concat(authorizeRemoteReservationStopRequest.SessionId, " at ", authorizeRemoteReservationStopRequest.EVSEId);

        public Func<DateTimeOffset, Object, AuthorizeRemoteReservationStopRequest, OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>, TimeSpan, String>
            AuthorizeRemoteReservationStopResponseConverter        { get; set; }

            = (timestamp, sender, authorizeRemoteReservationStopRequest, authorizeRemoteReservationStopResponse, runtime)
            => String.Concat(authorizeRemoteReservationStopRequest.SessionId, " at ", authorizeRemoteReservationStopRequest.EVSEId, " => ", authorizeRemoteReservationStopResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion


        #region AuthorizeRemoteStart           (Request/Response)Converter

        public Func<DateTimeOffset, Object, AuthorizeRemoteStartRequest, String>
            AuthorizeRemoteStartRequestConverter         { get; set; }

            = (timestamp, sender, authorizeRemoteStartRequest)
            => String.Concat(authorizeRemoteStartRequest.Identification, " at ", authorizeRemoteStartRequest.EVSEId);

        public Func<DateTimeOffset, Object, AuthorizeRemoteStartRequest, OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>, TimeSpan, String>
            AuthorizeRemoteStartResponseConverter        { get; set; }

            = (timestamp, sender, authorizeRemoteStartRequest, authorizeRemoteStartResponse, runtime)
            => String.Concat(authorizeRemoteStartRequest.Identification, " at ", authorizeRemoteStartRequest.EVSEId, " => ", authorizeRemoteStartResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion

        #region AuthorizeRemoteStop            (Request/Response)Converter

        public Func<DateTimeOffset, Object, AuthorizeRemoteStopRequest, String>
            AuthorizeRemoteStopRequestConverter         { get; set; }

            = (timestamp, sender, authorizeRemoteStopRequest)
            => String.Concat(authorizeRemoteStopRequest.SessionId, " at ", authorizeRemoteStopRequest.EVSEId);

        public Func<DateTimeOffset, Object, AuthorizeRemoteStopRequest, OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>, TimeSpan, String>
            AuthorizeRemoteStopResponseConverter        { get; set; }

            = (timestamp, sender, authorizeRemoteStopRequest, authorizeRemoteStopResponse, runtime)
            => String.Concat(authorizeRemoteStopRequest.SessionId, " at ", authorizeRemoteStopRequest.EVSEId, " => ", authorizeRemoteStopResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// The attached HTTP client logger.
        /// </summary>
        public new CPOServerAPIHTTPClientLogger  HTTPLogger
#pragma warning disable CS8603 // Possible null reference return.
            => base.HTTPLogger as CPOServerAPIHTTPClientLogger;
#pragma warning restore CS8603 // Possible null reference return.

        /// <summary>
        /// The attached client logger.
        /// </summary>
        public CPOServerAPIClientLogger?         Logger      { get; }

        public APICounters                       Counters    { get; }

        #endregion

        #region Events

        #region OnAuthorizeRemoteReservationStart/-Response

        /// <summary>
        /// An event fired whenever an AuthorizeRemoteReservationStart request will be send.
        /// </summary>
        public event OnAuthorizeRemoteReservationStartClientRequestDelegate?   OnAuthorizeRemoteReservationStartRequest;

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
        public event OnAuthorizeRemoteReservationStopClientRequestDelegate?   OnAuthorizeRemoteReservationStopRequest;

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
        public event OnAuthorizeRemoteStartClientRequestDelegate?   OnAuthorizeRemoteStartRequest;

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
        public event OnAuthorizeRemoteStopClientRequestDelegate?   OnAuthorizeRemoteStopRequest;

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
        /// <param name="PreferIPv4">Prefer IPv4 instead of IPv6.</param>
        /// <param name="RemoteCertificateValidator">The remote TLS certificate validator.</param>
        /// <param name="LocalCertificateSelector">A delegate to select a TLS client certificate.</param>
        /// <param name="ClientCert">The TLS client certificate to use of HTTP authentication.</param>
        /// <param name="Authentication">The optional HTTP authentication to use, e.g. HTTP Basic Auth.</param>
        /// <param name="HTTPUserAgent">The HTTP user agent identification.</param>
        /// <param name="RequestTimeout">An optional request timeout.</param>
        /// <param name="TransmissionRetryDelay">The delay between transmission retries.</param>
        /// <param name="MaxNumberOfRetries">The maximum number of transmission retries for HTTP request.</param>
        /// <param name="InternalBufferSize">An optional size of the internal buffers.</param>
        /// <param name="DisableLogging">Disable all logging.</param>
        /// <param name="LoggingPath">The logging path.</param>
        /// <param name="LoggingContext">An optional context for logging.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// <param name="DNSClient">The DNS client to use.</param>
        public CPOServerAPIClient(URL?                                                       RemoteURL                    = null,
                                  HTTPHostname?                                              VirtualHostname              = null,
                                  I18NString?                                                Description                  = null,
                                  UInt16?                                                    MaxNumberOfPooledClients     = null,
                                  Boolean?                                                   PreferIPv4                   = null,
                                  RemoteTLSServerCertificateValidationHandler<IHTTPClient>?  RemoteCertificateValidator   = null,
                                  LocalCertificateSelectionHandler?                          LocalCertificateSelector     = null,
                                  X509Certificate?                                           ClientCert                   = null,
                                  SslProtocols?                                              TLSProtocols                 = null,
                                  IHTTPAuthentication?                                       Authentication               = null,
                                  String?                                                    HTTPUserAgent                = DefaultHTTPUserAgent,
                                  TimeSpan?                                                  RequestTimeout               = null,
                                  TransmissionRetryDelayDelegate?                            TransmissionRetryDelay       = null,
                                  UInt16?                                                    MaxNumberOfRetries           = DefaultMaxNumberOfRetries,
                                  UInt32?                                                    InternalBufferSize           = null,
                                  Boolean?                                                   DisableLogging               = false,
                                  String?                                                    LoggingPath                  = null,
                                  String                                                     LoggingContext               = CPOServerAPIHTTPClientLogger.DefaultContext,
                                  LogfileCreatorDelegate?                                    LogfileCreator               = null,
                                  DNSClient?                                                 DNSClient                    = null)

            : base(RemoteURL ?? DefaultRemoteURL,
                   VirtualHostname,
                   Description,
                   MaxNumberOfPooledClients,
                   PreferIPv4,

                   RemoteCertificateValidator is not null
                       ? (sender,
                          certificate,
                          certificateChain,
                          httpClient,
                          policyErrors) => RemoteCertificateValidator.Invoke(
                                               sender,
                                               certificate,
                                               certificateChain,
                                              (httpClient as CPOServerAPIClient)!,
                                               policyErrors
                                           )
                       : null,

                   LocalCertificateSelector,
                   ClientCert,
                   TLSProtocols,
                   HTTPContentType.Application.JSON_UTF8,
                   AcceptTypes.FromHTTPContentTypes(HTTPContentType.Application.JSON_UTF8),
                   Authentication,
                   HTTPUserAgent ?? DefaultHTTPUserAgent,
                   ConnectionType.Close,
                   RequestTimeout,
                   TransmissionRetryDelay,
                   MaxNumberOfRetries ?? DefaultMaxNumberOfRetries,
                   InternalBufferSize,
                   false,
                   DisableLogging,
                   DNSClient)

        {

            this.Counters    = new APICounters();

            base.HTTPLogger  = this.DisableLogging == false
                                   ? new CPOServerAPIHTTPClientLogger(
                                         this,
                                         LoggingPath,
                                         LoggingContext,
                                         LogfileCreator
                                     )
                                   : null;

            this.Logger      = this.DisableLogging == false
                                   ? new CPOServerAPIClientLogger(
                                         this,
                                         LoggingPath,
                                         LoggingContext,
                                         LogfileCreator
                                     )
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
            var                                                                   processId           = Process_Id.NewRandom();

            #endregion

            #region Send OnAuthorizeRemoteReservationStartRequest event

            var startTime  = Timestamp.Now;
            var stopwatch  = Stopwatch.StartNew();

            Counters.AuthorizeRemoteReservationStart.IncRequests_OK();

            try
            {

                if (OnAuthorizeRemoteReservationStartRequest is not null)
                    await Task.WhenAll(OnAuthorizeRemoteReservationStartRequest.GetInvocationList().
                                       Cast<OnAuthorizeRemoteReservationStartClientRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOServerAPIClient) + "." + nameof(OnAuthorizeRemoteReservationStartRequest));
            }

            #endregion


            try
            {

                do
                {

                    #region Upstream HTTP request...

                    var httpResponse = await HTTPClientFactory.Create(
                                                 RemoteURL,
                                                 VirtualHostname,
                                                 Description,
                                                 PreferIPv4,
                                                 RemoteCertificateValidator,
                                                 LocalCertificateSelector,
                                                 ClientCert,
                                                 TLSProtocols,
                                                 ContentType,
                                                 Accept,
                                                 Authentication,
                                                 HTTPUserAgent,
                                                 Connection,
                                                 RequestTimeout,
                                                 TransmissionRetryDelay,
                                                 MaxNumberOfRetries,
                                                 InternalBufferSize,
                                                 UseHTTPPipelining,
                                                 DisableLogging,
                                                 null,
                                                 DNSClient
                                             ).

                                             POST(
                                                 Path:                 RemoteURL.Path + $"api/oicp/charging/v21/providers/{Request.ProviderId.URLEncoded}/authorize-remote-reservation/start",
                                                 Content:              Request.ToJSON(CustomAuthorizeRemoteReservationStartRequestSerializer,
                                                                                      CustomIdentificationSerializer).
                                                                               ToUTF8Bytes(JSONFormatting),
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                 RequestLogDelegate:   OnAuthorizeRemoteReservationStartHTTPRequest,
                                                 ResponseLogDelegate:  OnAuthorizeRemoteReservationStartHTTPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 RequestBuilder:       requestBuilder => {
                                                                           requestBuilder.Set("Process-ID", processId.ToString());
                                                                       }
                                             ).

                                             ConfigureAwait(false);

                    #endregion


                    // Re-read it from the HTTP response!
                    var processId2 = httpResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse);
                    //ToDo: Verify that processId == processId2!

                    if (httpResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (httpResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            httpResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

                                if (Acknowledgement<AuthorizeRemoteReservationStartRequest>.TryParse(Request,
                                                                                                     JObject.Parse(httpResponse.HTTPBody.ToUTF8String()),
                                                                                                     out Acknowledgement<AuthorizeRemoteReservationStartRequest>?  authorizeRemoteReservationStartResponse,
                                                                                                     out String?                                                   ErrorResponse,
                                                                                                     httpResponse,
                                                                                                     httpResponse.Timestamp,
                                                                                                     httpResponse.EventTrackingId,
                                                                                                     httpResponse.Runtime,
                                                                                                     processId,
                                                                                                     CustomAuthorizeRemoteReservationStartAcknowledgementParser))
                                {

                                    Counters.AuthorizeRemoteReservationStart.IncResponses_OK();

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
                                                 httpResponse.Timestamp,
                                                 httpResponse.EventTrackingId,
                                                 httpResponse.Runtime,
                                                 processId,
                                                 httpResponse,
                                                 Request.CustomData
                                             ),
                                             processId
                                         );

                            }

                        }

                        TransmissionRetry = Byte.MaxValue - 1;
                        break;

                    }

                    if (httpResponse.HTTPStatusCode == HTTPStatusCode.BadRequest)
                    {

                        if (httpResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            httpResponse.HTTPBody?.Length > 0)
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

                            if (ValidationErrorList.TryParse(JObject.Parse(httpResponse.HTTPBody.ToUTF8String() ?? ""),
                                                             out var validationErrorList,
                                                             out var errorResponse))
                            {

                                result = OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>.BadRequest(Request,
                                                                                                                        validationErrorList,
                                                                                                                        processId);

                            }

                        }

                        break;

                    }

                    if (httpResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                    {

                        // Hubject firewall problem!
                        // Only HTML response!
                        break;

                    }

                    if (httpResponse.HTTPStatusCode == HTTPStatusCode.Unauthorized)
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

                        if (httpResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            httpResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

#pragma warning disable CS8604 // Possible null reference argument.
                                if (StatusCode.TryParse(JObject.Parse(httpResponse.HTTPBody.ToUTF8String())["StatusCode"] as JObject,
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
                                                                                                                            httpResponse.Timestamp,
                                                                                                                            httpResponse.EventTrackingId,
                                                                                                                            httpResponse.Runtime,
                                                                                                                            processId,
                                                                                                                            httpResponse,
                                                                                                                            Request.CustomData
                                                                                                                        ),
                                                                                                                        processId);

                                }
#pragma warning restore CS8604 // Possible null reference argument.

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
                                                 httpResponse.Timestamp,
                                                 httpResponse.EventTrackingId,
                                                 httpResponse.Runtime,
                                                 processId,
                                                 httpResponse,
                                                 Request.CustomData
                                             )
                                         );

                            }

                        }

                        break;

                    }

                    if (httpResponse.HTTPStatusCode == HTTPStatusCode.RequestTimeout)
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

            if (result.IsNotSuccessful)
                Counters.AuthorizeRemoteReservationStart.IncResponses_Error();


            #region Send OnAuthorizeRemoteReservationStartResponse event

            var endtime = Timestamp.Now;
            stopwatch.Stop();

            try
            {

                if (OnAuthorizeRemoteReservationStartResponse is not null)
                    await Task.WhenAll(OnAuthorizeRemoteReservationStartResponse.GetInvocationList().
                                       Cast<OnAuthorizeRemoteReservationStartClientResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
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
            var                                                                  processId           = Process_Id.NewRandom();

            #endregion

            #region Send OnAuthorizeRemoteReservationStopRequest event

            var startTime  = Timestamp.Now;
            var stopwatch  = Stopwatch.StartNew();

            Counters.AuthorizeRemoteReservationStop.IncRequests_OK();

            try
            {

                if (OnAuthorizeRemoteReservationStopRequest is not null)
                    await Task.WhenAll(OnAuthorizeRemoteReservationStopRequest.GetInvocationList().
                                       Cast<OnAuthorizeRemoteReservationStopClientRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOServerAPIClient) + "." + nameof(OnAuthorizeRemoteReservationStopRequest));
            }

            #endregion


            try
            {

                do
                {

                    #region Upstream HTTP request...

                    var httpResponse = await HTTPClientFactory.Create(
                                                 RemoteURL,
                                                 VirtualHostname,
                                                 Description,
                                                 PreferIPv4,
                                                 RemoteCertificateValidator,
                                                 LocalCertificateSelector,
                                                 ClientCert,
                                                 TLSProtocols,
                                                 ContentType,
                                                 Accept,
                                                 Authentication,
                                                 HTTPUserAgent,
                                                 Connection,
                                                 RequestTimeout,
                                                 TransmissionRetryDelay,
                                                 MaxNumberOfRetries,
                                                 InternalBufferSize,
                                                 UseHTTPPipelining,
                                                 DisableLogging,
                                                 null,
                                                 DNSClient
                                             ).

                                             POST(
                                                 Path:                 RemoteURL.Path + $"api/oicp/charging/v21/providers/{Request.ProviderId.URLEncoded}/authorize-remote-reservation/stop",
                                                 Content:              Request.ToJSON(CustomAuthorizeRemoteReservationStopRequestSerializer).
                                                                               ToUTF8Bytes(JSONFormatting),
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                 RequestLogDelegate:   OnAuthorizeRemoteReservationStopHTTPRequest,
                                                 ResponseLogDelegate:  OnAuthorizeRemoteReservationStopHTTPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 RequestBuilder:       requestBuilder => {
                                                                           requestBuilder.Set("Process-ID", processId.ToString());
                                                                       }
                                             ).

                                             ConfigureAwait(false);

                    #endregion


                    // Re-read it from the HTTP response!
                    var processId2 = httpResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse);
                    //ToDo: Verify that processId == processId2!

                    if (httpResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (httpResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            httpResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

                                if (Acknowledgement<AuthorizeRemoteReservationStopRequest>.TryParse(Request,
                                                                                                    JObject.Parse(httpResponse.HTTPBody.ToUTF8String()),
                                                                                                    out Acknowledgement<AuthorizeRemoteReservationStopRequest>?  authorizeRemoteReservationStopResponse,
                                                                                                    out String?                                                  ErrorResponse,
                                                                                                    httpResponse,
                                                                                                    httpResponse.Timestamp,
                                                                                                    httpResponse.EventTrackingId,
                                                                                                    httpResponse.Runtime,
                                                                                                    processId,
                                                                                                    CustomAuthorizeRemoteReservationStopAcknowledgementParser))
                                {

                                    Counters.AuthorizeRemoteReservationStop.IncResponses_OK();

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
                                                 httpResponse.Timestamp,
                                                 httpResponse.EventTrackingId,
                                                 httpResponse.Runtime,
                                                 processId,
                                                 httpResponse,
                                                 Request.CustomData
                                             ),
                                             processId
                                         );

                            }

                        }

                        TransmissionRetry = Byte.MaxValue - 1;
                        break;

                    }

                    if (httpResponse.HTTPStatusCode == HTTPStatusCode.BadRequest)
                    {

                        if (httpResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            httpResponse.HTTPBody?.Length > 0)
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

                            if (ValidationErrorList.TryParse(JObject.Parse(httpResponse.HTTPBody.ToUTF8String() ?? ""),
                                                             out var validationErrorList,
                                                             out var errorResponse))
                            {

                                result = OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>.BadRequest(Request,
                                                                                                                       validationErrorList,
                                                                                                                       processId);

                            }

                        }

                        break;

                    }

                    if (httpResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                    {

                        // Hubject firewall problem!
                        // Only HTML response!
                        break;

                    }

                    if (httpResponse.HTTPStatusCode == HTTPStatusCode.Unauthorized)
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

                        if (httpResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            httpResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

#pragma warning disable CS8604 // Possible null reference argument.
                                if (StatusCode.TryParse(JObject.Parse(httpResponse.HTTPBody.ToUTF8String())["StatusCode"] as JObject,
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
                                                                                                                           httpResponse.Timestamp,
                                                                                                                           httpResponse.EventTrackingId,
                                                                                                                           httpResponse.Runtime,
                                                                                                                           processId,
                                                                                                                           httpResponse,
                                                                                                                           Request.CustomData
                                                                                                                       ),
                                                                                                                       processId);

                                }
#pragma warning restore CS8604 // Possible null reference argument.

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
                                                 httpResponse.Timestamp,
                                                 httpResponse.EventTrackingId,
                                                 httpResponse.Runtime,
                                                 processId,
                                                 httpResponse,
                                                 Request.CustomData
                                             )
                                         );

                            }

                        }

                        break;

                    }

                    if (httpResponse.HTTPStatusCode == HTTPStatusCode.RequestTimeout)
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

            if (result.IsNotSuccessful)
                Counters.AuthorizeRemoteReservationStop.IncResponses_Error();


            #region Send OnAuthorizeRemoteReservationStopResponse event

            var endtime = Timestamp.Now;
            stopwatch.Stop();

            try
            {

                if (OnAuthorizeRemoteReservationStopResponse is not null)
                    await Task.WhenAll(OnAuthorizeRemoteReservationStopResponse.GetInvocationList().
                                       Cast<OnAuthorizeRemoteReservationStopClientResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
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
            var                                                        processId           = Process_Id.NewRandom();

            #endregion

            #region Send OnAuthorizeRemoteStartRequest event

            var startTime  = Timestamp.Now;
            var stopwatch  = Stopwatch.StartNew();

            Counters.AuthorizeRemoteStart.IncRequests_OK();

            try
            {

                if (OnAuthorizeRemoteStartRequest is not null)
                    await Task.WhenAll(OnAuthorizeRemoteStartRequest.GetInvocationList().
                                       Cast<OnAuthorizeRemoteStartClientRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOServerAPIClient) + "." + nameof(OnAuthorizeRemoteStartRequest));
            }

            #endregion


            try
            {

                do
                {

                    #region Upstream HTTP request...

                    var httpResponse = await HTTPClientFactory.Create(
                                                 RemoteURL,
                                                 VirtualHostname,
                                                 Description,
                                                 PreferIPv4,
                                                 RemoteCertificateValidator,
                                                 LocalCertificateSelector,
                                                 ClientCert,
                                                 TLSProtocols,
                                                 ContentType,
                                                 Accept,
                                                 Authentication,
                                                 HTTPUserAgent,
                                                 Connection,
                                                 RequestTimeout,
                                                 TransmissionRetryDelay,
                                                 MaxNumberOfRetries,
                                                 InternalBufferSize,
                                                 UseHTTPPipelining,
                                                 DisableLogging,
                                                 null,
                                                 DNSClient
                                             ).

                                             POST(
                                                 Path:                 RemoteURL.Path + $"api/oicp/charging/v21/providers/{Request.ProviderId.URLEncoded}/authorize-remote/start",
                                                 Content:              Request.ToJSON(CustomAuthorizeRemoteStartRequestSerializer,
                                                                                      CustomIdentificationSerializer).
                                                                               ToUTF8Bytes(JSONFormatting),
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                 RequestLogDelegate:   OnAuthorizeRemoteStartHTTPRequest,
                                                 ResponseLogDelegate:  OnAuthorizeRemoteStartHTTPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 RequestBuilder:       requestBuilder => {
                                                                           requestBuilder.Set("Process-ID", processId.ToString());
                                                                       }
                                             ).

                                             ConfigureAwait(false);

                    #endregion


                    // Re-read it from the HTTP response!
                    var processId2 = httpResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse);
                    //ToDo: Verify that processId == processId2!

                    if (httpResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (httpResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            httpResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

                                if (Acknowledgement<AuthorizeRemoteStartRequest>.TryParse(Request,
                                                                                          JObject.Parse(httpResponse.HTTPBody.ToUTF8String()),
                                                                                          out Acknowledgement<AuthorizeRemoteStartRequest>?  authorizeRemoteStartResponse,
                                                                                          out String?                                        ErrorResponse,
                                                                                          httpResponse,
                                                                                          httpResponse.Timestamp,
                                                                                          httpResponse.EventTrackingId,
                                                                                          httpResponse.Runtime,
                                                                                          processId,
                                                                                          CustomAuthorizeRemoteStartAcknowledgementParser))
                                {

                                    Counters.AuthorizeRemoteStart.IncResponses_OK();

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
                                                 httpResponse.Timestamp,
                                                 httpResponse.EventTrackingId,
                                                 httpResponse.Runtime,
                                                 processId,
                                                 httpResponse,
                                                 Request.CustomData
                                             ),
                                             processId
                                         );

                            }

                        }

                        TransmissionRetry = Byte.MaxValue - 1;
                        break;

                    }

                    if (httpResponse.HTTPStatusCode == HTTPStatusCode.BadRequest)
                    {

                        if (httpResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            httpResponse.HTTPBody?.Length > 0)
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

                            if (ValidationErrorList.TryParse(JObject.Parse(httpResponse.HTTPBody.ToUTF8String() ?? ""),
                                                             out var validationErrorList,
                                                             out var errorResponse))
                            {

                                result = OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>.BadRequest(Request,
                                                                                                             validationErrorList,
                                                                                                             processId);

                            }

                        }

                        break;

                    }

                    if (httpResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                    {

                        // Hubject firewall problem!
                        // Only HTML response!
                        break;

                    }

                    if (httpResponse.HTTPStatusCode == HTTPStatusCode.Unauthorized)
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

                        if (httpResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            httpResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

#pragma warning disable CS8604 // Possible null reference argument.
                                if (StatusCode.TryParse(JObject.Parse(httpResponse.HTTPBody.ToUTF8String())["StatusCode"] as JObject,
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
                                                                                                                 httpResponse.Timestamp,
                                                                                                                 httpResponse.EventTrackingId,
                                                                                                                 httpResponse.Runtime,
                                                                                                                 processId,
                                                                                                                 httpResponse,
                                                                                                                 Request.CustomData
                                                                                                             ),
                                                                                                             processId);

                                }
#pragma warning restore CS8604 // Possible null reference argument.

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
                                                 httpResponse.Timestamp,
                                                 httpResponse.EventTrackingId,
                                                 httpResponse.Runtime,
                                                 processId,
                                                 httpResponse,
                                                 Request.CustomData
                                             )
                                         );

                            }

                        }

                        break;

                    }

                    if (httpResponse.HTTPStatusCode == HTTPStatusCode.RequestTimeout)
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

            if (result.IsNotSuccessful)
                Counters.AuthorizeRemoteStart.IncResponses_Error();


            #region Send OnAuthorizeRemoteStartClientResponse event

            var endtime = Timestamp.Now;
            stopwatch.Stop();

            try
            {

                if (OnAuthorizeRemoteStartResponse is not null)
                    await Task.WhenAll(OnAuthorizeRemoteStartResponse.GetInvocationList().
                                       Cast<OnAuthorizeRemoteStartClientResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
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
            var                                                       processId           = Process_Id.NewRandom();

            #endregion

            #region Send OnAuthorizeRemoteStopRequest event

            var startTime  = Timestamp.Now;
            var stopwatch  = Stopwatch.StartNew();

            Counters.AuthorizeRemoteStop.IncRequests_OK();

            try
            {

                if (OnAuthorizeRemoteStopRequest is not null)
                    await Task.WhenAll(OnAuthorizeRemoteStopRequest.GetInvocationList().
                                       Cast<OnAuthorizeRemoteStopClientRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOServerAPIClient) + "." + nameof(OnAuthorizeRemoteStopRequest));
            }

            #endregion


            try
            {

                do
                {

                    #region Upstream HTTP request...

                    var httpResponse = await HTTPClientFactory.Create(
                                                 RemoteURL,
                                                 VirtualHostname,
                                                 Description,
                                                 PreferIPv4,
                                                 RemoteCertificateValidator,
                                                 LocalCertificateSelector,
                                                 ClientCert,
                                                 TLSProtocols,
                                                 ContentType,
                                                 Accept,
                                                 Authentication,
                                                 HTTPUserAgent,
                                                 Connection,
                                                 RequestTimeout,
                                                 TransmissionRetryDelay,
                                                 MaxNumberOfRetries,
                                                 InternalBufferSize,
                                                 UseHTTPPipelining,
                                                 DisableLogging,
                                                 null,
                                                 DNSClient
                                             ).

                                             POST(
                                                 Path:                 RemoteURL.Path + $"api/oicp/charging/v21/providers/{Request.ProviderId.URLEncoded}/authorize-remote/stop",
                                                 Content:              Request.ToJSON(CustomAuthorizeRemoteStopRequestSerializer).
                                                                               ToUTF8Bytes(JSONFormatting),
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                 RequestLogDelegate:   OnAuthorizeRemoteStopHTTPRequest,
                                                 ResponseLogDelegate:  OnAuthorizeRemoteStopHTTPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 RequestBuilder:       requestBuilder => {
                                                                           requestBuilder.Set("Process-ID", processId.ToString());
                                                                       }
                                             ).

                                             ConfigureAwait(false);

                    #endregion


                    // Re-read it from the HTTP response!
                    var processId2 = httpResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse);
                    //ToDo: Verify that processId == processId2!

                    if (httpResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (httpResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            httpResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

                                if (Acknowledgement<AuthorizeRemoteStopRequest>.TryParse(Request,
                                                                                         JObject.Parse(httpResponse.HTTPBody.ToUTF8String()),
                                                                                         out Acknowledgement<AuthorizeRemoteStopRequest>?  authorizeRemoteStopResponse,
                                                                                         out String?                                       ErrorResponse,
                                                                                         httpResponse,
                                                                                         httpResponse.Timestamp,
                                                                                         httpResponse.EventTrackingId,
                                                                                         httpResponse.Runtime,
                                                                                         processId,
                                                                                         CustomAuthorizeRemoteStopAcknowledgementParser))
                                {

                                    Counters.AuthorizeRemoteStop.IncResponses_OK();

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
                                                 httpResponse.Timestamp,
                                                 httpResponse.EventTrackingId,
                                                 httpResponse.Runtime,
                                                 processId,
                                                 httpResponse,
                                                 Request.CustomData
                                             ),
                                             processId
                                         );

                            }

                        }

                        TransmissionRetry = Byte.MaxValue - 1;
                        break;

                    }

                    if (httpResponse.HTTPStatusCode == HTTPStatusCode.BadRequest)
                    {

                        if (httpResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            httpResponse.HTTPBody?.Length > 0)
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

                            if (ValidationErrorList.TryParse(JObject.Parse(httpResponse.HTTPBody.ToUTF8String() ?? ""),
                                                             out var validationErrorList,
                                                             out var errorResponse))
                            {

                                result = OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>.BadRequest(Request,
                                                                                                            validationErrorList,
                                                                                                            processId);

                            }

                        }

                        break;

                    }

                    if (httpResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                    {

                        // Hubject firewall problem!
                        // Only HTML response!
                        break;

                    }

                    if (httpResponse.HTTPStatusCode == HTTPStatusCode.Unauthorized)
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

                        if (httpResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            httpResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

#pragma warning disable CS8604 // Possible null reference argument.
                                if (StatusCode.TryParse(JObject.Parse(httpResponse.HTTPBody.ToUTF8String())["StatusCode"] as JObject,
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
                                                                                                                httpResponse.Timestamp,
                                                                                                                httpResponse.EventTrackingId,
                                                                                                                httpResponse.Runtime,
                                                                                                                processId,
                                                                                                                httpResponse,
                                                                                                                Request.CustomData
                                                                                                            ),
                                                                                                            processId);

                                }
#pragma warning restore CS8604 // Possible null reference argument.

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
                                                 httpResponse.Timestamp,
                                                 httpResponse.EventTrackingId,
                                                 httpResponse.Runtime,
                                                 processId,
                                                 httpResponse,
                                                 Request.CustomData
                                             )
                                         );

                            }

                        }

                        break;

                    }

                    if (httpResponse.HTTPStatusCode == HTTPStatusCode.RequestTimeout)
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

            if (result.IsNotSuccessful)
                Counters.AuthorizeRemoteStop.IncResponses_Error();


            #region Send OnAuthorizeRemoteStopResponse event

            var endtime = Timestamp.Now;
            stopwatch.Stop();

            try
            {

                if (OnAuthorizeRemoteStopResponse is not null)
                    await Task.WhenAll(OnAuthorizeRemoteStopResponse.GetInvocationList().
                                       Cast<OnAuthorizeRemoteStopClientResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
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
