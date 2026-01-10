/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Logging;

using cloud.charging.open.protocols.OICPv2_3.CPO;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.EMP
{

    /// <summary>
    /// The EMP server API client, aka. the Hubject side of the API.
    /// </summary>
    public partial class EMPServerAPIClient : AOICPClient//,
                                      //        IEMPServerAPIClient
    {

        #region (class) APICounters

        public class APICounters(APICounterValues?  AuthorizeStart                     = null,
                                 APICounterValues?  AuthorizeStop                      = null,

                                 APICounterValues?  SendChargingStartNotification      = null,
                                 APICounterValues?  SendChargingProgressNotification   = null,
                                 APICounterValues?  SendChargingEndNotification        = null,
                                 APICounterValues?  SendChargingErrorNotification      = null,

                                 APICounterValues?  SendChargeDetailRecord             = null)
        {

            public APICounterValues AuthorizeStart                      { get; } = AuthorizeStart                   ?? new APICounterValues();
            public APICounterValues AuthorizeStop                       { get; } = AuthorizeStop                    ?? new APICounterValues();


            public APICounterValues SendChargingStartNotification       { get; } = SendChargingStartNotification    ?? new APICounterValues();

            public APICounterValues SendChargingProgressNotification    { get; } = SendChargingProgressNotification ?? new APICounterValues();

            public APICounterValues SendChargingEndNotification         { get; } = SendChargingEndNotification      ?? new APICounterValues();

            public APICounterValues SendChargingErrorNotification       { get; } = SendChargingErrorNotification    ?? new APICounterValues();


            public APICounterValues SendChargeDetailRecord              { get; } = SendChargeDetailRecord           ?? new APICounterValues();


            public JObject ToJSON()

                => JSONObject.Create(
                       new JProperty("AuthorizeStart",                   AuthorizeStart.                  ToJSON()),
                       new JProperty("AuthorizeStop",                    AuthorizeStop.                   ToJSON()),

                       new JProperty("SendChargingStartNotification",    SendChargingStartNotification.   ToJSON()),
                       new JProperty("SendChargingProgressNotification", SendChargingProgressNotification.ToJSON()),
                       new JProperty("SendChargingEndNotification",      SendChargingEndNotification.     ToJSON()),
                       new JProperty("SendChargingErrorNotification",    SendChargingErrorNotification.   ToJSON()),

                       new JProperty("SendChargeDetailRecord",           SendChargeDetailRecord.          ToJSON())
                   );

        }

        #endregion


        #region Data

        /// <summary>
        /// The default HTTP user agent.
        /// </summary>
        public new const        String    DefaultHTTPUserAgent        = "GraphDefined OICP " + Version.String + " EMP Server API Client";

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

        public CustomJObjectParserDelegate<AuthorizationStartResponse>?                            CustomAuthorizationStartResponseParser                            { get; set; }

        public CustomJObjectParserDelegate<AuthorizationStopResponse>?                             CustomAuthorizationStopResponseParser                             { get; set; }


        public CustomJObjectParserDelegate<Acknowledgement<ChargingStartNotificationRequest>>?     CustomChargingStartNotificationRequestAcknowledgementParser       { get; set; }

        public CustomJObjectParserDelegate<Acknowledgement<ChargingProgressNotificationRequest>>?  CustomChargingProgressNotificationRequestAcknowledgementParser    { get; set; }

        public CustomJObjectParserDelegate<Acknowledgement<ChargingEndNotificationRequest>>?       CustomChargingEndNotificationRequestAcknowledgementParser         { get; set; }

        public CustomJObjectParserDelegate<Acknowledgement<ChargingErrorNotificationRequest>>?     CustomChargingErrorNotificationRequestAcknowledgementParser       { get; set; }


        public CustomJObjectParserDelegate<Acknowledgement<ChargeDetailRecordRequest>>?            CustomChargeDetailRecordRequestAcknowledgementParser              { get; set; }

        #endregion

        #region Custom JSON serializers

        public CustomJObjectSerializerDelegate<AuthorizeStartRequest>?                CustomAuthorizeStartRequestSerializer                  { get; set; }

        public CustomJObjectSerializerDelegate<AuthorizeStopRequest>?                 CustomAuthorizeStopRequestSerializer                   { get; set; }

        public CustomJObjectSerializerDelegate<ChargingStartNotificationRequest>?     CustomChargingStartNotificationRequestSerializer       { get; set; }
        public CustomJObjectSerializerDelegate<ChargingProgressNotificationRequest>?  CustomChargingProgressNotificationRequestSerializer    { get; set; }
        public CustomJObjectSerializerDelegate<ChargingEndNotificationRequest>?       CustomChargingEndNotificationRequestSerializer         { get; set; }
        public CustomJObjectSerializerDelegate<ChargingErrorNotificationRequest>?     CustomChargingErrorNotificationRequestSerializer       { get; set; }

        public CustomJObjectSerializerDelegate<ChargeDetailRecordRequest>?            CustomChargeDetailRecordRequestSerializer              { get; set; }

        public CustomJObjectSerializerDelegate<ChargeDetailRecord>?                   CustomChargeDetailRecordSerializer                     { get; set; }
        public CustomJObjectSerializerDelegate<Identification>?                       CustomIdentificationSerializer                         { get; set; }
        public CustomJObjectSerializerDelegate<SignedMeteringValue>?                  CustomSignedMeteringValueSerializer                    { get; set; }
        public CustomJObjectSerializerDelegate<CalibrationLawVerification>?           CustomCalibrationLawVerificationSerializer             { get; set; }

        #endregion

        #region Custom request/response logging converters

        #region AuthorizeStart              (Request/Response)Converter

        public Func<DateTimeOffset, Object, AuthorizeStartRequest, String>
            AuthorizeStartRequestConverter         { get; set; }

            = (timestamp, sender, authorizeStartRequest)
            => String.Concat(authorizeStartRequest.Identification, " at ", authorizeStartRequest.EVSEId);

        public Func<DateTimeOffset, Object, AuthorizeStartRequest, OICPResult<AuthorizationStartResponse>, TimeSpan, String>
            AuthorizeStartResponseConverter        { get; set; }

            = (timestamp, sender, authorizeStartRequest, authorizeStartResponse, runtime)
            => String.Concat(authorizeStartRequest.Identification, " at ", authorizeStartRequest.EVSEId, " => ", authorizeStartResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion

        #region AuthorizeStop               (Request/Response)Converter

        public Func<DateTimeOffset, Object, AuthorizeStopRequest, String>
            AuthorizeStopRequestConverter         { get; set; }

            = (timestamp, sender, authorizeStopRequest)
            => String.Concat(authorizeStopRequest.Identification, " at ", authorizeStopRequest.EVSEId);

        public Func<DateTimeOffset, Object, AuthorizeStopRequest, OICPResult<AuthorizationStopResponse>, TimeSpan, String>
            AuthorizeStopResponseConverter        { get; set; }

            = (timestamp, sender, authorizeStopRequest, authorizeStopResponse, runtime)
            => String.Concat(authorizeStopRequest.Identification, " at ", authorizeStopRequest.EVSEId, " => ", authorizeStopResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion


        #region ChargingStartNotification   (Request/Response)Converter

        public Func<DateTimeOffset, Object, ChargingStartNotificationRequest, String>
            ChargingStartNotificationRequestConverter         { get; set; }

            = (timestamp, sender, chargingStartNotificationRequest)
            => String.Concat(chargingStartNotificationRequest.Identification, " at ", chargingStartNotificationRequest.EVSEId);

        public Func<DateTimeOffset, Object, ChargingStartNotificationRequest, OICPResult<Acknowledgement<ChargingStartNotificationRequest>>, TimeSpan, String>
            ChargingStartNotificationResponseConverter        { get; set; }

            = (timestamp, sender, chargingStartNotificationRequest, chargingStartNotificationResponse, runtime)
            => String.Concat(chargingStartNotificationRequest.Identification, " at ", chargingStartNotificationRequest.EVSEId, " => ", chargingStartNotificationResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion

        #region ChargingProgressNotification(Request/Response)Converter

        public Func<DateTimeOffset, Object, ChargingProgressNotificationRequest, String>
            ChargingProgressNotificationRequestConverter         { get; set; }

            = (timestamp, sender, chargingProgressNotificationRequest)
            => String.Concat(chargingProgressNotificationRequest.Identification, " at ", chargingProgressNotificationRequest.EVSEId);

        public Func<DateTimeOffset, Object, ChargingProgressNotificationRequest, OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>, TimeSpan, String>
            ChargingProgressNotificationResponseConverter        { get; set; }

            = (timestamp, sender, chargingProgressNotificationRequest, chargingProgressNotificationResponse, runtime)
            => String.Concat(chargingProgressNotificationRequest.Identification, " at ", chargingProgressNotificationRequest.EVSEId, " => ", chargingProgressNotificationResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion

        #region ChargingEndNotification     (Request/Response)Converter

        public Func<DateTimeOffset, Object, ChargingEndNotificationRequest, String>
            ChargingEndNotificationRequestConverter         { get; set; }

            = (timestamp, sender, chargingEndNotificationRequest)
            => String.Concat(chargingEndNotificationRequest.Identification, " at ", chargingEndNotificationRequest.EVSEId);

        public Func<DateTimeOffset, Object, ChargingEndNotificationRequest, OICPResult<Acknowledgement<ChargingEndNotificationRequest>>, TimeSpan, String>
            ChargingEndNotificationResponseConverter        { get; set; }

            = (timestamp, sender, chargingEndNotificationRequest, chargingEndNotificationResponse, runtime)
            => String.Concat(chargingEndNotificationRequest.Identification, " at ", chargingEndNotificationRequest.EVSEId, " => ", chargingEndNotificationResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion

        #region ChargingErrorNotification   (Request/Response)Converter

        public Func<DateTimeOffset, Object, ChargingErrorNotificationRequest, String>
            ChargingErrorNotificationRequestConverter         { get; set; }

            = (timestamp, sender, chargingErrorNotificationRequest)
            => String.Concat(chargingErrorNotificationRequest.Identification, " at ", chargingErrorNotificationRequest.EVSEId);

        public Func<DateTimeOffset, Object, ChargingErrorNotificationRequest, OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>, TimeSpan, String>
            ChargingErrorNotificationResponseConverter        { get; set; }

            = (timestamp, sender, chargingErrorNotificationRequest, chargingErrorNotificationResponse, runtime)
            => String.Concat(chargingErrorNotificationRequest.Identification, " at ", chargingErrorNotificationRequest.EVSEId, " => ", chargingErrorNotificationResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion


        #region ChargingStartNotification   (Request/Response)Converter

        public Func<DateTimeOffset, Object, ChargeDetailRecordRequest, String>
            ChargeDetailRecordRequestConverter         { get; set; }

            = (timestamp, sender, chargeDetailRecordRequest)
            => String.Concat(chargeDetailRecordRequest.ChargeDetailRecord.Identification, " at ", chargeDetailRecordRequest.ChargeDetailRecord.EVSEId);

        public Func<DateTimeOffset, Object, ChargeDetailRecordRequest, OICPResult<Acknowledgement<ChargeDetailRecordRequest>>, TimeSpan, String>
            ChargeDetailRecordResponseConverter        { get; set; }

            = (timestamp, sender, chargeDetailRecordRequest, chargeDetailRecordResponse, runtime)
            => String.Concat(chargeDetailRecordRequest.ChargeDetailRecord.Identification, " at ", chargeDetailRecordRequest.ChargeDetailRecord.EVSEId, " => ", chargeDetailRecordResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// The attached HTTP client logger.
        /// </summary>
        public new EMPServerAPIHTTPClientLogger  HTTPLogger
#pragma warning disable CS8603 // Possible null reference return.
            => base.HTTPLogger as EMPServerAPIHTTPClientLogger;
#pragma warning restore CS8603 // Possible null reference return.

        /// <summary>
        /// The attached client logger.
        /// </summary>
        public EMPServerAPIClientLogger?         Logger      { get; }

        public APICounters                       Counters    { get; }

        #endregion

        #region Events

        #region OnAuthorizeStartRequest/-Response

        /// <summary>
        /// An event fired whenever an AuthorizeStart request will be send.
        /// </summary>
        public event OnAuthorizeStartClientRequestDelegate?   OnAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever an AuthorizeStart HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                 OnAuthorizeStartHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeStart HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                OnAuthorizeStartHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeStart request had been received.
        /// </summary>
        public event OnAuthorizeStartClientResponseDelegate?  OnAuthorizeStartResponse;

        #endregion

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event fired whenever an AuthorizeStop request will be send.
        /// </summary>
        public event OnAuthorizeStopClientRequestDelegate?   OnAuthorizeStopRequest;

        /// <summary>
        /// An event fired whenever an AuthorizeStop HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                OnAuthorizeStopHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeStop HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?               OnAuthorizeStopHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeStop request had been received.
        /// </summary>
        public event OnAuthorizeStopClientResponseDelegate?  OnAuthorizeStopResponse;

        #endregion


        #region OnChargingStartNotificationRequest/-Response

        /// <summary>
        /// An event fired whenever a charging start notification request will be send.
        /// </summary>
        public event OnChargingStartNotificationClientRequestDelegate?   OnChargingStartNotificationRequest;

        /// <summary>
        /// An event fired whenever a charging start notification HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                            OnChargingStartNotificationHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for a charging start notification HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                           OnChargingStartNotificationHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for a charging start notification request had been received.
        /// </summary>
        public event OnChargingStartNotificationClientResponseDelegate?  OnChargingStartNotificationResponse;

        #endregion

        #region OnChargingProgressNotificationRequest/-Response

        /// <summary>
        /// An event fired whenever a charging progress notification request will be send.
        /// </summary>
        public event OnChargingProgressNotificationClientRequestDelegate?   OnChargingProgressNotificationRequest;

        /// <summary>
        /// An event fired whenever a charging progress notification HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                               OnChargingProgressNotificationHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for a charging progress notification HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                              OnChargingProgressNotificationHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for a charging progress notification request had been received.
        /// </summary>
        public event OnChargingProgressNotificationClientResponseDelegate?  OnChargingProgressNotificationResponse;

        #endregion

        #region OnChargingEndNotificationRequest/-Response

        /// <summary>
        /// An event fired whenever a charging end notification request will be send.
        /// </summary>
        public event OnChargingEndNotificationClientRequestDelegate?   OnChargingEndNotificationRequest;

        /// <summary>
        /// An event fired whenever a charging end notification HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                          OnChargingEndNotificationHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for a charging end notification HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                         OnChargingEndNotificationHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for a charging end notification request had been received.
        /// </summary>
        public event OnChargingEndNotificationClientResponseDelegate?  OnChargingEndNotificationResponse;

        #endregion

        #region OnChargingErrorNotificationRequest/-Response

        /// <summary>
        /// An event fired whenever a charging error notification request will be send.
        /// </summary>
        public event OnChargingErrorNotificationClientRequestDelegate?   OnChargingErrorNotificationRequest;

        /// <summary>
        /// An event fired whenever a charging error notification HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                            OnChargingErrorNotificationHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for a charging error notification HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                           OnChargingErrorNotificationHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for a charging error notification request had been received.
        /// </summary>
        public event OnChargingErrorNotificationClientResponseDelegate?  OnChargingErrorNotificationResponse;

        #endregion


        #region OnChargeDetailRecordRequest/-Response

        /// <summary>
        /// An event fired whenever a ChargeDetailRecord request will be send.
        /// </summary>
        public event OnChargeDetailRecordClientRequestDelegate?   OnChargeDetailRecordRequest;

        /// <summary>
        /// An event fired whenever a ChargeDetailRecord HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                     OnChargeDetailRecordHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for a ChargeDetailRecord HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                    OnChargeDetailRecordHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for a ChargeDetailRecord request had been received.
        /// </summary>
        public event OnChargeDetailRecordClientResponseDelegate?  OnChargeDetailRecordResponse;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EMP client.
        /// </summary>
        /// <param name="RemoteURL">The remote URL of the OICP HTTP endpoint to connect to.</param>
        /// <param name="VirtualHostname">An optional HTTP virtual hostname.</param>
        /// <param name="Description">An optional description of this CPO client.</param>
        /// <param name="PreferIPv4">Prefer IPv4 instead of IPv6.</param>
        /// <param name="RemoteCertificateValidator">The remote TLS certificate validator.</param>
        /// <param name="LocalCertificateSelector">A delegate to select a TLS client certificate.</param>
        /// <param name="ClientCertificates">An optional enumeration of client certificates.</param>
        /// <param name="ClientCertificateContext">An optional TLS client certificate context.</param>
        /// <param name="HTTPAuthentication">The optional HTTP authentication to use, e.g. HTTP Basic Auth.</param>
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
        public EMPServerAPIClient(URL?                                                       RemoteURL                    = null,
                                  HTTPHostname?                                              VirtualHostname              = null,
                                  I18NString?                                                Description                  = null,
                                  UInt16?                                                    MaxNumberOfPooledClients     = null,
                                  Boolean?                                                   PreferIPv4                   = null,
                                  RemoteTLSServerCertificateValidationHandler<IHTTPClient>?  RemoteCertificateValidator   = null,
                                  LocalCertificateSelectionHandler?                          LocalCertificateSelector     = null,
                                  IEnumerable<X509Certificate2>?                             ClientCertificates           = null,
                                  SslStreamCertificateContext?                               ClientCertificateContext     = null,
                                  IEnumerable<X509Certificate2>?                             ClientCertificateChain       = null,
                                  SslProtocols?                                              TLSProtocols                 = null,
                                  IHTTPAuthentication?                                       HTTPAuthentication           = null,
                                  TOTPConfig?                                                TOTPConfig                   = null,
                                  String?                                                    HTTPUserAgent                = DefaultHTTPUserAgent,
                                  TimeSpan?                                                  RequestTimeout               = null,
                                  TransmissionRetryDelayDelegate?                            TransmissionRetryDelay       = null,
                                  UInt16?                                                    MaxNumberOfRetries           = DefaultMaxNumberOfRetries,
                                  UInt32?                                                    InternalBufferSize           = null,
                                  Boolean?                                                   DisableLogging               = false,
                                  String?                                                    LoggingPath                  = null,
                                  String                                                     LoggingContext               = EMPServerAPIHTTPClientLogger.DefaultContext,
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
                                              (httpClient as EMPServerAPIClient)!,
                                               policyErrors
                                           )
                       : null,

                   LocalCertificateSelector,
                   ClientCertificates,
                   ClientCertificateContext,
                   ClientCertificateChain,
                   TLSProtocols,
                   HTTPContentType.Application.JSON_UTF8,
                   AcceptTypes.FromHTTPContentTypes(HTTPContentType.Application.JSON_UTF8),
                   HTTPAuthentication,
                   TOTPConfig,
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
                                   ? new EMPServerAPIHTTPClientLogger(
                                         this,
                                         LoggingPath,
                                         LoggingContext,
                                         LogfileCreator
                                     )
                                   : null;

            this.Logger      = this.DisableLogging == false
                                   ? new EMPServerAPIClientLogger(
                                         this,
                                         LoggingPath,
                                         LoggingContext,
                                         LogfileCreator
                                     )
                                   : null;

        }

        #endregion


        #region AuthorizeStart                  (Request)

        /// <summary>
        /// Send an authorize start request.
        /// </summary>
        /// <param name="Request">An AuthorizeStart request.</param>
        public async Task<OICPResult<AuthorizationStartResponse>> AuthorizeStart(AuthorizeStartRequest Request)
        {

            #region Initial checks

            //Request = _CustomAuthorizeStartRequestMapper(Request);

            Byte                                     TransmissionRetry   = 0;
            OICPResult<AuthorizationStartResponse>?  result              = null;
            var                                      processId           = Process_Id.NewRandom();

            #endregion

            #region Send OnAuthorizeStartClientRequest event

            var startTime  = Timestamp.Now;
            var stopwatch  = Stopwatch.StartNew();

            Counters.AuthorizeStart.IncRequests_OK();

            try
            {

                if (OnAuthorizeStartRequest is not null)
                    await Task.WhenAll(OnAuthorizeStartRequest.GetInvocationList().
                                       Cast<OnAuthorizeStartClientRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPServerAPIClient) + "." + nameof(OnAuthorizeStartRequest));
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
                                                 ClientCertificates,
                                                 ClientCertificateContext,
                                                 ClientCertificateChain,
                                                 TLSProtocols,
                                                 ContentType,
                                                 Accept,
                                                 HTTPAuthentication,
                                                 TOTPConfig,
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
                                                 Path:                 RemoteURL.Path + $"/api/oicp/charging/v21/operators/{Request.OperatorId.URLEncoded}/authorize/start",
                                                 Content:              Request.ToJSON(CustomAuthorizeStartRequestSerializer,
                                                                                      CustomIdentificationSerializer).
                                                                               ToUTF8Bytes(JSONFormatting),
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                 RequestLogDelegate:   OnAuthorizeStartHTTPRequest,
                                                 ResponseLogDelegate:  OnAuthorizeStartHTTPResponse,
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

                                if (AuthorizationStartResponse.TryParse(Request,
                                                                        JObject.Parse(httpResponse.HTTPBody.ToUTF8String()),
                                                                        out AuthorizationStartResponse?  authorizeStartResponse,
                                                                        out String?                      ErrorResponse,
                                                                        httpResponse.Timestamp,
                                                                        httpResponse.EventTrackingId,
                                                                        httpResponse.Runtime,
                                                                        processId,
                                                                        httpResponse,
                                                                        CustomAuthorizationStartResponseParser))
                                {

                                    Counters.AuthorizeStart.IncResponses_OK();

                                    result = OICPResult<AuthorizationStartResponse>.Success(Request,
                                                                                            authorizeStartResponse!,
                                                                                            processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<AuthorizationStartResponse>.Failed(
                                             Request,
                                             AuthorizationStartResponse.SystemError(
                                                 Request,
                                                 e.Message,
                                                 e.StackTrace,
                                                 Request.SessionId,
                                                 Request.CPOPartnerSessionId,
                                                 Request.EMPPartnerSessionId,
                                                 null, // ProviderId
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

                                result = OICPResult<AuthorizationStartResponse>.BadRequest(Request,
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

                                    result = OICPResult<AuthorizationStartResponse>.Failed(Request,
                                                                                           AuthorizationStartResponse.DataError(
                                                                                               Request,
                                                                                               statusCode!.Description,
                                                                                               statusCode!.AdditionalInfo,
                                                                                               Request.SessionId,
                                                                                               Request.CPOPartnerSessionId,
                                                                                               Request.EMPPartnerSessionId,
                                                                                               null, // ProviderId
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

                                result = OICPResult<AuthorizationStartResponse>.Failed(
                                             Request,
                                             AuthorizationStartResponse.SystemError(
                                                 Request,
                                                 e.Message,
                                                 e.StackTrace,
                                                 Request.SessionId,
                                                 Request.CPOPartnerSessionId,
                                                 Request.EMPPartnerSessionId,
                                                 null, // ProviderId
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

                result = OICPResult<AuthorizationStartResponse>.Failed(
                             Request,
                             AuthorizationStartResponse.SystemError(
                                 Request,
                                 e.Message,
                                 e.StackTrace,
                                 Request.SessionId,
                                 Request.CPOPartnerSessionId,
                                 Request.EMPPartnerSessionId,
                                 null, // ProviderId
                                 Timestamp.Now,
                                 Request.EventTrackingId,
                                 Timestamp.Now - Request.Timestamp,
                                 processId,
                                 null, // HTTPResponse
                                 Request.CustomData
                             )
                         );

            }

            result ??= OICPResult<AuthorizationStartResponse>.Failed(
                           Request,
                           AuthorizationStartResponse.SystemError(
                               Request,
                               "HTTP request failed!",
                               null,
                               Request.SessionId,
                               Request.CPOPartnerSessionId,
                               Request.EMPPartnerSessionId,
                               null, // ProviderId
                               Timestamp.Now,
                               Request.EventTrackingId,
                               Timestamp.Now - Request.Timestamp,
                               processId,
                               null, // HTTPResponse
                               Request.CustomData
                           )
                       );

            if (result.IsNotSuccessful)
                Counters.AuthorizeStart.IncResponses_Error();


            #region Send OnAuthorizeStartClientResponse event

            var endtime = Timestamp.Now;
            stopwatch.Stop();

            try
            {

                if (OnAuthorizeStartResponse is not null)
                    await Task.WhenAll(OnAuthorizeStartResponse.GetInvocationList().
                                       Cast<OnAuthorizeStartClientResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     result.Runtime ?? TimeSpan.Zero))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPServerAPIClient) + "." + nameof(OnAuthorizeStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop                   (Request)

        /// <summary>
        /// Send an authorize Stop request.
        /// </summary>
        /// <param name="Request">An AuthorizeStop request.</param>
        public async Task<OICPResult<AuthorizationStopResponse>> AuthorizeStop(AuthorizeStopRequest Request)
        {

            #region Initial checks

            //Request = _CustomAuthorizeStopRequestMapper(Request);

            Byte                                    TransmissionRetry   = 0;
            OICPResult<AuthorizationStopResponse>?  result              = null;
            var                                     processId           = Process_Id.NewRandom();

            #endregion

            #region Send OnAuthorizeStopClientRequest event

            var startTime  = Timestamp.Now;
            var stopwatch  = Stopwatch.StartNew();

            Counters.AuthorizeStop.IncRequests_OK();

            try
            {

                if (OnAuthorizeStopRequest is not null)
                    await Task.WhenAll(OnAuthorizeStopRequest.GetInvocationList().
                                       Cast<OnAuthorizeStopClientRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPServerAPIClient) + "." + nameof(OnAuthorizeStopRequest));
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
                                                 ClientCertificates,
                                                 ClientCertificateContext,
                                                 ClientCertificateChain,
                                                 TLSProtocols,
                                                 ContentType,
                                                 Accept,
                                                 HTTPAuthentication,
                                                 TOTPConfig,
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
                                                 Path:                 RemoteURL.Path + $"/api/oicp/charging/v21/operators/{Request.OperatorId.URLEncoded}/authorize/stop",
                                                 Content:              Request.ToJSON(CustomAuthorizeStopRequestSerializer,
                                                                                      CustomIdentificationSerializer).
                                                                               ToUTF8Bytes(JSONFormatting),
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                 RequestLogDelegate:   OnAuthorizeStopHTTPRequest,
                                                 ResponseLogDelegate:  OnAuthorizeStopHTTPResponse,
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

                                if (AuthorizationStopResponse.TryParse(Request,
                                                                       JObject.Parse(httpResponse.HTTPBody.ToUTF8String()),
                                                                       out AuthorizationStopResponse?  authorizeStopResponse,
                                                                       out String?                     ErrorResponse,
                                                                       httpResponse.Timestamp,
                                                                       httpResponse.EventTrackingId,
                                                                       httpResponse.Runtime,
                                                                       processId,
                                                                       httpResponse,
                                                                       CustomAuthorizationStopResponseParser))
                                {

                                    Counters.AuthorizeStop.IncResponses_OK();

                                    result = OICPResult<AuthorizationStopResponse>.Success(Request,
                                                                                           authorizeStopResponse!,
                                                                                           processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<AuthorizationStopResponse>.Failed(
                                             Request,
                                             AuthorizationStopResponse.SystemError(
                                                 Request,
                                                 e.Message,
                                                 e.StackTrace,
                                                 Request.SessionId,
                                                 Request.CPOPartnerSessionId,
                                                 Request.EMPPartnerSessionId,
                                                 null, // ProviderId
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

                                result = OICPResult<AuthorizationStopResponse>.BadRequest(Request,
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

                                    result = OICPResult<AuthorizationStopResponse>.Failed(Request,
                                                                                           AuthorizationStopResponse.DataError(
                                                                                               Request,
                                                                                               statusCode!.Description,
                                                                                               statusCode!.AdditionalInfo,
                                                                                               Request.SessionId,
                                                                                               Request.CPOPartnerSessionId,
                                                                                               Request.EMPPartnerSessionId,
                                                                                               null, // ProviderId
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

                                result = OICPResult<AuthorizationStopResponse>.Failed(
                                             Request,
                                             AuthorizationStopResponse.SystemError(
                                                 Request,
                                                 e.Message,
                                                 e.StackTrace,
                                                 Request.SessionId,
                                                 Request.CPOPartnerSessionId,
                                                 Request.EMPPartnerSessionId,
                                                 null, // ProviderId
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

                result = OICPResult<AuthorizationStopResponse>.Failed(
                             Request,
                             AuthorizationStopResponse.SystemError(
                                 Request,
                                 e.Message,
                                 e.StackTrace,
                                 Request.SessionId,
                                 Request.CPOPartnerSessionId,
                                 Request.EMPPartnerSessionId,
                                 null, // ProviderId
                                 Timestamp.Now,
                                 Request.EventTrackingId,
                                 Timestamp.Now - Request.Timestamp,
                                 processId,
                                 null, // HTTPResponse
                                 Request.CustomData
                             )
                         );

            }

            result ??= OICPResult<AuthorizationStopResponse>.Failed(
                           Request,
                           AuthorizationStopResponse.SystemError(
                               Request,
                               "HTTP request failed!",
                               null,
                               Request.SessionId,
                               Request.CPOPartnerSessionId,
                               Request.EMPPartnerSessionId,
                               null, // ProviderId
                               Timestamp.Now,
                               Request.EventTrackingId,
                               Timestamp.Now - Request.Timestamp,
                               processId,
                               null, // HTTPResponse
                               Request.CustomData
                           )
                       );

            if (result.IsNotSuccessful)
                Counters.AuthorizeStop.IncResponses_Error();


            #region Send OnAuthorizeStopClientResponse event

            var endtime = Timestamp.Now;
            stopwatch.Stop();

            try
            {

                if (OnAuthorizeStopResponse is not null)
                    await Task.WhenAll(OnAuthorizeStopResponse.GetInvocationList().
                                       Cast<OnAuthorizeStopClientResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     result.Runtime ?? TimeSpan.Zero))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPServerAPIClient) + "." + nameof(OnAuthorizeStopResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region SendChargingStartNotification   (Request)

        /// <summary>
        /// Send a charging start notification.
        /// </summary>
        /// <param name="Request">A charging start notification request.</param>
        public async Task<OICPResult<Acknowledgement<ChargingStartNotificationRequest>>> SendChargingStartNotification(ChargingStartNotificationRequest Request)
        {

            #region Initial checks

            //Request = _CustomChargingStartNotificationRequestMapper(Request);

            Byte                                                            TransmissionRetry   = 0;
            OICPResult<Acknowledgement<ChargingStartNotificationRequest>>?  result              = null;
            var                                                             processId           = Process_Id.NewRandom();

            #endregion

            #region Send OnChargingStartNotificationRequest event

            var startTime  = Timestamp.Now;
            var stopwatch  = Stopwatch.StartNew();

            Counters.SendChargingStartNotification.IncRequests_OK();

            try
            {

                if (OnChargingStartNotificationRequest is not null)
                    await Task.WhenAll(OnChargingStartNotificationRequest.GetInvocationList().
                                       Cast<OnChargingStartNotificationClientRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPServerAPIClient) + "." + nameof(OnChargingStartNotificationRequest));
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
                                                 ClientCertificates,
                                                 ClientCertificateContext,
                                                 ClientCertificateChain,
                                                 TLSProtocols,
                                                 ContentType,
                                                 Accept,
                                                 HTTPAuthentication,
                                                 TOTPConfig,
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
                                                 Path:                 RemoteURL.Path + $"/api/oicp/notificationmgmt/v11/charging-notifications",
                                                 Content:              Request.ToJSON(CustomChargingStartNotificationRequestSerializer,
                                                                                      CustomIdentificationSerializer).
                                                                               ToUTF8Bytes(JSONFormatting),
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                 RequestLogDelegate:   OnChargingStartNotificationHTTPRequest,
                                                 ResponseLogDelegate:  OnChargingStartNotificationHTTPResponse,
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

                                if (Acknowledgement<ChargingStartNotificationRequest>.TryParse(Request,
                                                                                               JObject.Parse(httpResponse.HTTPBody.ToUTF8String()),
                                                                                               out Acknowledgement<ChargingStartNotificationRequest>?  chargingStartNotificationResponse,
                                                                                               out String?                                             ErrorResponse,
                                                                                               httpResponse,
                                                                                               httpResponse.Timestamp,
                                                                                               httpResponse.EventTrackingId,
                                                                                               httpResponse.Runtime,
                                                                                               processId,
                                                                                               CustomChargingStartNotificationRequestAcknowledgementParser))
                                {

                                    Counters.SendChargingStartNotification.IncResponses_OK();

                                    result = OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Success(Request,
                                                                                                                   chargingStartNotificationResponse!,
                                                                                                                   processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Failed(
                                             Request,
                                             Acknowledgement<ChargingStartNotificationRequest>.SystemError(
                                                 Request,
                                                 e.Message,
                                                 e.StackTrace,
                                                 null, //Request.SessionId,
                                                 null, //Request.CPOPartnerSessionId,
                                                 null, //Request.EMPPartnerSessionId,
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

                                result = OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.BadRequest(Request,
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

                                    result = OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Failed(Request,
                                                                                                                  Acknowledgement<ChargingStartNotificationRequest>.DataError(
                                                                                                                      Request,
                                                                                                                      statusCode?.Description,
                                                                                                                      statusCode?.AdditionalInfo,
                                                                                                                      null, // Request.SessionId,
                                                                                                                      null, // Request.CPOPartnerSessionId,
                                                                                                                      null, // Request.EMPPartnerSessionId,
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

                                result = OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Failed(
                                             Request,
                                             Acknowledgement<ChargingStartNotificationRequest>.SystemError(
                                                 Request,
                                                 e.Message,
                                                 e.StackTrace,
                                                 null, // Request.SessionId,
                                                 null, // Request.CPOPartnerSessionId,
                                                 null, // Request.EMPPartnerSessionId,
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

                result = OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Failed(
                             Request,
                             Acknowledgement<ChargingStartNotificationRequest>.SystemError(
                                 Request,
                                 e.Message,
                                 e.StackTrace,
                                 null, // Request.SessionId,
                                 null, // Request.CPOPartnerSessionId,
                                 null, // Request.EMPPartnerSessionId,
                                 Timestamp.Now,
                                 Request.EventTrackingId,
                                 Timestamp.Now - Request.Timestamp,
                                 processId,
                                 null, // HTTPResponse
                                 Request.CustomData
                             )
                         );

            }

            result ??= OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Failed(
                           Request,
                           Acknowledgement<ChargingStartNotificationRequest>.SystemError(
                               Request,
                               "HTTP request failed!",
                               null,
                               null, // Request.SessionId,
                               null, // Request.CPOPartnerSessionId,
                               null, // Request.EMPPartnerSessionId,
                               Timestamp.Now,
                               Request.EventTrackingId,
                               Timestamp.Now - Request.Timestamp,
                               processId,
                               null, // HTTPResponse
                               Request.CustomData
                           )
                       );

            if (result.IsNotSuccessful)
                Counters.SendChargingStartNotification.IncResponses_Error();


            #region Send OnChargingStartNotificationResponse event

            var endtime = Timestamp.Now;
            stopwatch.Stop();

            try
            {

                if (OnChargingStartNotificationResponse is not null)
                    await Task.WhenAll(OnChargingStartNotificationResponse.GetInvocationList().
                                       Cast<OnChargingStartNotificationClientResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     result.Runtime ?? TimeSpan.Zero))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPServerAPIClient) + "." + nameof(OnChargingStartNotificationResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region SendChargingProgressNotification(Request)

        /// <summary>
        /// Send a charging progress notification.
        /// </summary>
        /// <param name="Request">A charging progress notification request.</param>
        public async Task<OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>> SendChargingProgressNotification(ChargingProgressNotificationRequest Request)
        {

            #region Initial checks

            //Request = _CustomChargingProgressNotificationRequestMapper(Request);

            Byte                                                               TransmissionRetry   = 0;
            OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>?  result              = null;
            var                                                                processId           = Process_Id.NewRandom();

            #endregion

            #region Send OnChargingProgressNotificationRequest event

            var startTime  = Timestamp.Now;
            var stopwatch  = Stopwatch.StartNew();

            Counters.SendChargingProgressNotification.IncRequests_OK();

            try
            {

                if (OnChargingProgressNotificationRequest is not null)
                    await Task.WhenAll(OnChargingProgressNotificationRequest.GetInvocationList().
                                       Cast<OnChargingProgressNotificationClientRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPServerAPIClient) + "." + nameof(OnChargingProgressNotificationRequest));
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
                                                 ClientCertificates,
                                                 ClientCertificateContext,
                                                 ClientCertificateChain,
                                                 TLSProtocols,
                                                 ContentType,
                                                 Accept,
                                                 HTTPAuthentication,
                                                 TOTPConfig,
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
                                                 Path:                 RemoteURL.Path + $"/api/oicp/notificationmgmt/v11/charging-notifications",
                                                 Content:              Request.ToJSON(CustomChargingProgressNotificationRequestSerializer,
                                                                                      CustomIdentificationSerializer).
                                                                               ToUTF8Bytes(JSONFormatting),
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                 RequestLogDelegate:   OnChargingProgressNotificationHTTPRequest,
                                                 ResponseLogDelegate:  OnChargingProgressNotificationHTTPResponse,
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

                                if (Acknowledgement<ChargingProgressNotificationRequest>.TryParse(Request,
                                                                                                  JObject.Parse(httpResponse.HTTPBody.ToUTF8String()),
                                                                                                  out Acknowledgement<ChargingProgressNotificationRequest>?  chargingProgressNotificationResponse,
                                                                                                  out String?                                                ErrorResponse,
                                                                                                  httpResponse,
                                                                                                  httpResponse.Timestamp,
                                                                                                  httpResponse.EventTrackingId,
                                                                                                  httpResponse.Runtime,
                                                                                                  processId,
                                                                                                  CustomChargingProgressNotificationRequestAcknowledgementParser))
                                {

                                    Counters.SendChargingProgressNotification.IncResponses_OK();

                                    result = OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Success(Request,
                                                                                                                      chargingProgressNotificationResponse!,
                                                                                                                      processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Failed(
                                             Request,
                                             Acknowledgement<ChargingProgressNotificationRequest>.SystemError(
                                                 Request,
                                                 e.Message,
                                                 e.StackTrace,
                                                 null, //Request.SessionId,
                                                 null, //Request.CPOPartnerSessionId,
                                                 null, //Request.EMPPartnerSessionId,
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

                                result = OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.BadRequest(Request,
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

                                    result = OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Failed(Request,
                                                                                                                     Acknowledgement<ChargingProgressNotificationRequest>.DataError(
                                                                                                                         Request,
                                                                                                                         statusCode?.Description,
                                                                                                                         statusCode?.AdditionalInfo,
                                                                                                                         null, // Request.SessionId,
                                                                                                                         null, // Request.CPOPartnerSessionId,
                                                                                                                         null, // Request.EMPPartnerSessionId,
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

                                result = OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Failed(
                                             Request,
                                             Acknowledgement<ChargingProgressNotificationRequest>.SystemError(
                                                 Request,
                                                 e.Message,
                                                 e.StackTrace,
                                                 null, // Request.SessionId,
                                                 null, // Request.CPOPartnerSessionId,
                                                 null, // Request.EMPPartnerSessionId,
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

                result = OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Failed(
                             Request,
                             Acknowledgement<ChargingProgressNotificationRequest>.SystemError(
                                 Request,
                                 e.Message,
                                 e.StackTrace,
                                 null, // Request.SessionId,
                                 null, // Request.CPOPartnerSessionId,
                                 null, // Request.EMPPartnerSessionId,
                                 Timestamp.Now,
                                 Request.EventTrackingId,
                                 Timestamp.Now - Request.Timestamp,
                                 processId,
                                 null, // HTTPResponse
                                 Request.CustomData
                             )
                         );

            }

            result ??= OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Failed(
                           Request,
                           Acknowledgement<ChargingProgressNotificationRequest>.SystemError(
                               Request,
                               "HTTP request failed!",
                               null,
                               null, // Request.SessionId,
                               null, // Request.CPOPartnerSessionId,
                               null, // Request.EMPPartnerSessionId,
                               Timestamp.Now,
                               Request.EventTrackingId,
                               Timestamp.Now - Request.Timestamp,
                               processId,
                               null, // HTTPResponse
                               Request.CustomData
                           )
                       );

            if (result.IsNotSuccessful)
                Counters.SendChargingProgressNotification.IncResponses_Error();


            #region Send OnChargingProgressNotificationResponse event

            var endtime = Timestamp.Now;
            stopwatch.Stop();

            try
            {

                if (OnChargingProgressNotificationResponse is not null)
                    await Task.WhenAll(OnChargingProgressNotificationResponse.GetInvocationList().
                                       Cast<OnChargingProgressNotificationClientResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     result.Runtime ?? TimeSpan.Zero))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPServerAPIClient) + "." + nameof(OnChargingProgressNotificationResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region SendChargingEndNotification     (Request)

        /// <summary>
        /// Send a charging end notification.
        /// </summary>
        /// <param name="Request">A charging end notification request.</param>
        public async Task<OICPResult<Acknowledgement<ChargingEndNotificationRequest>>> SendChargingEndNotification(ChargingEndNotificationRequest Request)
        {

            #region Initial checks

            //Request = _CustomChargingEndNotificationRequestMapper(Request);

            Byte                                                          TransmissionRetry   = 0;
            OICPResult<Acknowledgement<ChargingEndNotificationRequest>>?  result              = null;
            var                                                           processId           = Process_Id.NewRandom();

            #endregion

            #region Send OnChargingEndNotificationRequest event

            var startTime  = Timestamp.Now;
            var stopwatch  = Stopwatch.StartNew();

            Counters.SendChargingEndNotification.IncRequests_OK();

            try
            {

                if (OnChargingEndNotificationRequest is not null)
                    await Task.WhenAll(OnChargingEndNotificationRequest.GetInvocationList().
                                       Cast<OnChargingEndNotificationClientRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPServerAPIClient) + "." + nameof(OnChargingEndNotificationRequest));
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
                                                 ClientCertificates,
                                                 ClientCertificateContext,
                                                 ClientCertificateChain,
                                                 TLSProtocols,
                                                 ContentType,
                                                 Accept,
                                                 HTTPAuthentication,
                                                 TOTPConfig,
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
                                                 Path:                 RemoteURL.Path + $"/api/oicp/notificationmgmt/v11/charging-notifications",
                                                 Content:              Request.ToJSON(CustomChargingEndNotificationRequestSerializer,
                                                                                      CustomIdentificationSerializer).
                                                                               ToUTF8Bytes(JSONFormatting),
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                 RequestLogDelegate:   OnChargingEndNotificationHTTPRequest,
                                                 ResponseLogDelegate:  OnChargingEndNotificationHTTPResponse,
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

                                if (Acknowledgement<ChargingEndNotificationRequest>.TryParse(Request,
                                                                                             JObject.Parse(httpResponse.HTTPBody.ToUTF8String()),
                                                                                             out Acknowledgement<ChargingEndNotificationRequest>?  chargingEndNotificationResponse,
                                                                                             out String?                                           ErrorResponse,
                                                                                             httpResponse,
                                                                                             httpResponse.Timestamp,
                                                                                             httpResponse.EventTrackingId,
                                                                                             httpResponse.Runtime,
                                                                                             processId,
                                                                                             CustomChargingEndNotificationRequestAcknowledgementParser))
                                {

                                    Counters.SendChargingEndNotification.IncResponses_OK();

                                    result = OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Success(Request,
                                                                                                                 chargingEndNotificationResponse!,
                                                                                                                 processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Failed(
                                             Request,
                                             Acknowledgement<ChargingEndNotificationRequest>.SystemError(
                                                 Request,
                                                 e.Message,
                                                 e.StackTrace,
                                                 null, //Request.SessionId,
                                                 null, //Request.CPOPartnerSessionId,
                                                 null, //Request.EMPPartnerSessionId,
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

                                result = OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.BadRequest(Request,
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

                                    result = OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Failed(Request,
                                                                                                                Acknowledgement<ChargingEndNotificationRequest>.DataError(
                                                                                                                    Request,
                                                                                                                    statusCode?.Description,
                                                                                                                    statusCode?.AdditionalInfo,
                                                                                                                    null, // Request.SessionId,
                                                                                                                    null, // Request.CPOPartnerSessionId,
                                                                                                                    null, // Request.EMPPartnerSessionId,
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

                                result = OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Failed(
                                             Request,
                                             Acknowledgement<ChargingEndNotificationRequest>.SystemError(
                                                 Request,
                                                 e.Message,
                                                 e.StackTrace,
                                                 null, // Request.SessionId,
                                                 null, // Request.CPOPartnerSessionId,
                                                 null, // Request.EMPPartnerSessionId,
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

                result = OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Failed(
                             Request,
                             Acknowledgement<ChargingEndNotificationRequest>.SystemError(
                                 Request,
                                 e.Message,
                                 e.StackTrace,
                                 null, // Request.SessionId,
                                 null, // Request.CPOPartnerSessionId,
                                 null, // Request.EMPPartnerSessionId,
                                 Timestamp.Now,
                                 Request.EventTrackingId,
                                 Timestamp.Now - Request.Timestamp,
                                 processId,
                                 null, // HTTPResponse
                                 Request.CustomData
                             )
                         );

            }

            result ??= OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Failed(
                           Request,
                           Acknowledgement<ChargingEndNotificationRequest>.SystemError(
                               Request,
                               "HTTP request failed!",
                               null,
                               null, // Request.SessionId,
                               null, // Request.CPOPartnerSessionId,
                               null, // Request.EMPPartnerSessionId,
                               Timestamp.Now,
                               Request.EventTrackingId,
                               Timestamp.Now - Request.Timestamp,
                               processId,
                               null, // HTTPResponse
                               Request.CustomData
                           )
                       );

            if (result.IsNotSuccessful)
                Counters.SendChargingEndNotification.IncResponses_Error();


            #region Send OnChargingEndNotificationResponse event

            var endtime = Timestamp.Now;
            stopwatch.Stop();

            try
            {

                if (OnChargingEndNotificationResponse is not null)
                    await Task.WhenAll(OnChargingEndNotificationResponse.GetInvocationList().
                                       Cast<OnChargingEndNotificationClientResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     result.Runtime ?? TimeSpan.Zero))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPServerAPIClient) + "." + nameof(OnChargingEndNotificationResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region SendChargingErrorNotification   (Request)

        /// <summary>
        /// Send a charging error notification.
        /// </summary>
        /// <param name="Request">A charging error notification request.</param>
        public async Task<OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>> SendChargingErrorNotification(ChargingErrorNotificationRequest Request)
        {

            #region Initial checks

            //Request = _CustomChargingErrorNotificationRequestMapper(Request);

            Byte                                                            TransmissionRetry   = 0;
            OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>?  result              = null;
            var                                                             processId           = Process_Id.NewRandom();

            #endregion

            #region Send OnChargingErrorNotificationRequest event

            var startTime  = Timestamp.Now;
            var stopwatch  = Stopwatch.StartNew();

            Counters.SendChargingErrorNotification.IncRequests_OK();

            try
            {

                if (OnChargingErrorNotificationRequest is not null)
                    await Task.WhenAll(OnChargingErrorNotificationRequest.GetInvocationList().
                                       Cast<OnChargingErrorNotificationClientRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPServerAPIClient) + "." + nameof(OnChargingErrorNotificationRequest));
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
                                                 ClientCertificates,
                                                 ClientCertificateContext,
                                                 ClientCertificateChain,
                                                 TLSProtocols,
                                                 ContentType,
                                                 Accept,
                                                 HTTPAuthentication,
                                                 TOTPConfig,
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
                                                 Path:                 RemoteURL.Path + $"/api/oicp/notificationmgmt/v11/charging-notifications",
                                                 Content:              Request.ToJSON(CustomChargingErrorNotificationRequestSerializer,
                                                                                      CustomIdentificationSerializer).
                                                                               ToUTF8Bytes(JSONFormatting),
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                 RequestLogDelegate:   OnChargingErrorNotificationHTTPRequest,
                                                 ResponseLogDelegate:  OnChargingErrorNotificationHTTPResponse,
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

                                if (Acknowledgement<ChargingErrorNotificationRequest>.TryParse(Request,
                                                                                               JObject.Parse(httpResponse.HTTPBody.ToUTF8String()),
                                                                                               out Acknowledgement<ChargingErrorNotificationRequest>?  chargingErrorNotificationResponse,
                                                                                               out String?                                             ErrorResponse,
                                                                                               httpResponse,
                                                                                               httpResponse.Timestamp,
                                                                                               httpResponse.EventTrackingId,
                                                                                               httpResponse.Runtime,
                                                                                               processId,
                                                                                               CustomChargingErrorNotificationRequestAcknowledgementParser))
                                {

                                    Counters.SendChargingErrorNotification.IncResponses_OK();

                                    result = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Success(Request,
                                                                                                                   chargingErrorNotificationResponse!,
                                                                                                                   processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Failed(
                                             Request,
                                             Acknowledgement<ChargingErrorNotificationRequest>.SystemError(
                                                 Request,
                                                 e.Message,
                                                 e.StackTrace,
                                                 null, //Request.SessionId,
                                                 null, //Request.CPOPartnerSessionId,
                                                 null, //Request.EMPPartnerSessionId,
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

                                result = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.BadRequest(Request,
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

                                    result = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Failed(Request,
                                                                                                                  Acknowledgement<ChargingErrorNotificationRequest>.DataError(
                                                                                                                      Request,
                                                                                                                      statusCode?.Description,
                                                                                                                      statusCode?.AdditionalInfo,
                                                                                                                      null, // Request.SessionId,
                                                                                                                      null, // Request.CPOPartnerSessionId,
                                                                                                                      null, // Request.EMPPartnerSessionId,
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

                                result = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Failed(
                                             Request,
                                             Acknowledgement<ChargingErrorNotificationRequest>.SystemError(
                                                 Request,
                                                 e.Message,
                                                 e.StackTrace,
                                                 null, // Request.SessionId,
                                                 null, // Request.CPOPartnerSessionId,
                                                 null, // Request.EMPPartnerSessionId,
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

                result = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Failed(
                             Request,
                             Acknowledgement<ChargingErrorNotificationRequest>.SystemError(
                                 Request,
                                 e.Message,
                                 e.StackTrace,
                                 null, // Request.SessionId,
                                 null, // Request.CPOPartnerSessionId,
                                 null, // Request.EMPPartnerSessionId,
                                 Timestamp.Now,
                                 Request.EventTrackingId,
                                 Timestamp.Now - Request.Timestamp,
                                 processId,
                                 null, // HTTPResponse
                                 Request.CustomData
                             )
                         );

            }

            result ??= OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Failed(
                           Request,
                           Acknowledgement<ChargingErrorNotificationRequest>.SystemError(
                               Request,
                               "HTTP request failed!",
                               null,
                               null, // Request.SessionId,
                               null, // Request.CPOPartnerSessionId,
                               null, // Request.EMPPartnerSessionId,
                               Timestamp.Now,
                               Request.EventTrackingId,
                               Timestamp.Now - Request.Timestamp,
                               processId,
                               null, // HTTPResponse
                               Request.CustomData
                           )
                       );

            if (result.IsNotSuccessful)
                Counters.SendChargingErrorNotification.IncResponses_Error();


            #region Send OnChargingErrorNotificationResponse event

            var endtime = Timestamp.Now;
            stopwatch.Stop();

            try
            {

                if (OnChargingErrorNotificationResponse is not null)
                    await Task.WhenAll(OnChargingErrorNotificationResponse.GetInvocationList().
                                       Cast<OnChargingErrorNotificationClientResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     result.Runtime ?? TimeSpan.Zero))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPServerAPIClient) + "." + nameof(OnChargingErrorNotificationResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region SendChargeDetailRecord          (Request)

        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="Request">A charge detail record request.</param>
        public async Task<OICPResult<Acknowledgement<ChargeDetailRecordRequest>>> SendChargeDetailRecord(ChargeDetailRecordRequest Request)
        {

            #region Initial checks

            //Request = _CustomChargeDetailRecordRequestMapper(Request);

            Byte                                                     TransmissionRetry   = 0;
            OICPResult<Acknowledgement<ChargeDetailRecordRequest>>?  result              = null;
            var                                                      processId           = Process_Id.NewRandom();

            #endregion

            #region Send OnChargeDetailRecordClientRequest event

            var startTime  = Timestamp.Now;
            var stopwatch  = Stopwatch.StartNew();

            Counters.SendChargeDetailRecord.IncRequests_OK();

            try
            {

                if (OnChargeDetailRecordRequest is not null)
                    await Task.WhenAll(OnChargeDetailRecordRequest.GetInvocationList().
                                       Cast<OnChargeDetailRecordClientRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPServerAPIClient) + "." + nameof(OnChargeDetailRecordRequest));
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
                                                 ClientCertificates,
                                                 ClientCertificateContext,
                                                 ClientCertificateChain,
                                                 TLSProtocols,
                                                 ContentType,
                                                 Accept,
                                                 HTTPAuthentication,
                                                 TOTPConfig,
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
                                                 Path:                 RemoteURL.Path + $"/api/oicp/cdrmgmt/v22/operators/{Request.OperatorId.URLEncoded}/charge-detail-record",
                                                 Content:              Request.ToJSON(CustomChargeDetailRecordRequestSerializer,
                                                                                      CustomChargeDetailRecordSerializer,
                                                                                      CustomIdentificationSerializer,
                                                                                      CustomSignedMeteringValueSerializer,
                                                                                      CustomCalibrationLawVerificationSerializer).
                                                                               ToUTF8Bytes(JSONFormatting),
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                 RequestLogDelegate:   OnChargeDetailRecordHTTPRequest,
                                                 ResponseLogDelegate:  OnChargeDetailRecordHTTPResponse,
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

                                if (Acknowledgement<ChargeDetailRecordRequest>.TryParse(Request,
                                                                                        JObject.Parse(httpResponse.HTTPBody.ToUTF8String()),
                                                                                        out Acknowledgement<ChargeDetailRecordRequest>?  chargeDetailRecordResponse,
                                                                                        out String?                                      ErrorResponse,
                                                                                        httpResponse,
                                                                                        httpResponse.Timestamp,
                                                                                        httpResponse.EventTrackingId,
                                                                                        httpResponse.Runtime,
                                                                                        processId,
                                                                                        CustomChargeDetailRecordRequestAcknowledgementParser))
                                {

                                    Counters.SendChargeDetailRecord.IncResponses_OK();

                                    result = OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Success(Request,
                                                                                                            chargeDetailRecordResponse!,
                                                                                                            processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Failed(
                                             Request,
                                             Acknowledgement<ChargeDetailRecordRequest>.SystemError(
                                                 Request,
                                                 e.Message,
                                                 e.StackTrace,
                                                 null, //Request.SessionId,
                                                 null, //Request.CPOPartnerSessionId,
                                                 null, //Request.EMPPartnerSessionId,
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

                                result = OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.BadRequest(Request,
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

                                    result = OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Failed(Request,
                                                                                                           Acknowledgement<ChargeDetailRecordRequest>.DataError(
                                                                                                               Request,
                                                                                                               statusCode?.Description,
                                                                                                               statusCode?.AdditionalInfo,
                                                                                                               null, // Request.SessionId,
                                                                                                               null, // Request.CPOPartnerSessionId,
                                                                                                               null, // Request.EMPPartnerSessionId,
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

                                result = OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Failed(
                                             Request,
                                             Acknowledgement<ChargeDetailRecordRequest>.SystemError(
                                                 Request,
                                                 e.Message,
                                                 e.StackTrace,
                                                 null, // Request.SessionId,
                                                 null, // Request.CPOPartnerSessionId,
                                                 null, // Request.EMPPartnerSessionId,
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

                result = OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Failed(
                             Request,
                             Acknowledgement<ChargeDetailRecordRequest>.SystemError(
                                 Request,
                                 e.Message,
                                 e.StackTrace,
                                 null, // Request.SessionId,
                                 null, // Request.CPOPartnerSessionId,
                                 null, // Request.EMPPartnerSessionId,
                                 Timestamp.Now,
                                 Request.EventTrackingId,
                                 Timestamp.Now - Request.Timestamp,
                                 processId,
                                 null, // HTTPResponse
                                 Request.CustomData
                             )
                         );

            }

            result ??= OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Failed(
                           Request,
                           Acknowledgement<ChargeDetailRecordRequest>.SystemError(
                               Request,
                               "HTTP request failed!",
                               null,
                               null, // Request.SessionId,
                               null, // Request.CPOPartnerSessionId,
                               null, // Request.EMPPartnerSessionId,
                               Timestamp.Now,
                               Request.EventTrackingId,
                               Timestamp.Now - Request.Timestamp,
                               processId,
                               null, // HTTPResponse
                               Request.CustomData
                           )
                       );

            if (result.IsNotSuccessful)
                Counters.SendChargeDetailRecord.IncResponses_Error();


            #region Send OnChargeDetailRecordClientResponse event

            var endtime = Timestamp.Now;
            stopwatch.Stop();

            try
            {

                if (OnChargeDetailRecordResponse is not null)
                    await Task.WhenAll(OnChargeDetailRecordResponse.GetInvocationList().
                                       Cast<OnChargeDetailRecordClientResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     result.Runtime ?? TimeSpan.Zero))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPServerAPIClient) + "." + nameof(OnChargeDetailRecordResponse));
            }

            #endregion

            return result;

        }

        #endregion


    }

}
