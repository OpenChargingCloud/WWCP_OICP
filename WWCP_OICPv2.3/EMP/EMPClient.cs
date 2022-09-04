﻿/*
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

using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.EMP
{

    /// <summary>
    /// The EMP client.
    /// </summary>
    public partial class EMPClient : AHTTPClient,
                                     IEMPClient
    {

        #region (class) APICounters

        public class APICounters
        {

            public APICounterValues  PullEVSEData                       { get; }
            public APICounterValues  PullEVSEStatus                     { get; }
            public APICounterValues  PullEVSEStatusById                 { get; }
            public APICounterValues  PullEVSEStatusByOperatorId         { get; }

            public APICounterValues  PullPricingProductData             { get; }
            public APICounterValues  PullEVSEPricing                    { get; }

            public APICounterValues  PushAuthenticationData             { get; }

            public APICounterValues  AuthorizeRemoteReservationStart    { get; }
            public APICounterValues  AuthorizeRemoteReservationStop     { get; }
            public APICounterValues  AuthorizeRemoteStart               { get; }
            public APICounterValues  AuthorizeRemoteStop                { get; }

            public APICounterValues  GetChargeDetailRecords             { get; }

            public APICounters(APICounterValues? PullEVSEData                       = null,
                               APICounterValues? PullEVSEStatus                     = null,
                               APICounterValues? PullEVSEStatusById                 = null,
                               APICounterValues? PullEVSEStatusByOperatorId         = null,

                               APICounterValues? PullPricingProductData             = null,
                               APICounterValues? PullEVSEPricing                    = null,

                               APICounterValues? PushAuthenticationData             = null,

                               APICounterValues? AuthorizeRemoteReservationStart    = null,
                               APICounterValues? AuthorizeRemoteReservationStop     = null,
                               APICounterValues? AuthorizeRemoteStart               = null,
                               APICounterValues? AuthorizeRemoteStop                = null,

                               APICounterValues? GetChargeDetailRecords             = null)
            {

                this.PullEVSEData                     = PullEVSEData                    ?? new APICounterValues();
                this.PullEVSEStatus                   = PullEVSEStatus                  ?? new APICounterValues();
                this.PullEVSEStatusById               = PullEVSEStatusById              ?? new APICounterValues();
                this.PullEVSEStatusByOperatorId       = PullEVSEStatusByOperatorId      ?? new APICounterValues();

                this.PullPricingProductData           = PullPricingProductData          ?? new APICounterValues();
                this.PullEVSEPricing                  = PullEVSEPricing                 ?? new APICounterValues();

                this.PushAuthenticationData           = PushAuthenticationData          ?? new APICounterValues();

                this.AuthorizeRemoteReservationStart  = AuthorizeRemoteReservationStart ?? new APICounterValues();
                this.AuthorizeRemoteReservationStop   = AuthorizeRemoteReservationStop  ?? new APICounterValues();
                this.AuthorizeRemoteStart             = AuthorizeRemoteStart            ?? new APICounterValues();
                this.AuthorizeRemoteStop              = AuthorizeRemoteStop             ?? new APICounterValues();

                this.GetChargeDetailRecords           = GetChargeDetailRecords          ?? new APICounterValues();

            }

            public JObject ToJSON()

                => JSONObject.Create(
                       new JProperty("PullEVSEData",                     PullEVSEData.                   ToJSON()),
                       new JProperty("PullEVSEStatus",                   PullEVSEStatus.                 ToJSON()),
                       new JProperty("PullEVSEStatusById",               PullEVSEStatusById.             ToJSON()),
                       new JProperty("PullEVSEStatusByOperatorId",       PullEVSEStatusByOperatorId.     ToJSON()),

                       new JProperty("PullPricingProductData",           PullPricingProductData.         ToJSON()),
                       new JProperty("PullEVSEPricing",                  PullEVSEPricing.                ToJSON()),

                       new JProperty("PushAuthenticationData",           PushAuthenticationData.         ToJSON()),

                       new JProperty("AuthorizeRemoteReservationStart",  AuthorizeRemoteReservationStart.ToJSON()),
                       new JProperty("AuthorizeRemoteReservationStop",   AuthorizeRemoteReservationStop. ToJSON()),
                       new JProperty("AuthorizeRemoteStart",             AuthorizeRemoteStart.           ToJSON()),
                       new JProperty("AuthorizeRemoteStop",              AuthorizeRemoteStop.            ToJSON()),

                       new JProperty("GetChargeDetailRecords",           GetChargeDetailRecords.         ToJSON())
                   );

        }

        #endregion


        #region Data

        /// <summary>
        /// The default HTTP user agent.
        /// </summary>
        public new const        String    DefaultHTTPUserAgent        = "GraphDefined OICP " + Version.Number + " EMP Client";

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

        /// <summary>
        /// The attached HTTP client logger.
        /// </summary>
        public new HTTP_Logger             HTTPLogger
#pragma warning disable CS8603 // Possible null reference return.
            => base.HTTPLogger as HTTP_Logger;
#pragma warning restore CS8603 // Possible null reference return.

        /// <summary>
        /// The attached client logger.
        /// </summary>
        public EMPClientLogger?            Logger        { get; }

        public APICounters                 Counters      { get; }

        public Newtonsoft.Json.Formatting  JSONFormat    { get; set; }

        #endregion

        #region Custom request/response converters

        #region PullEVSEData                (Request/Response)Converter

        public Func<DateTime, Object, PullEVSEDataRequest, String>
            PullEVSEDataRequestConverter                     { get; set; }

            = (timestamp, sender, pullEVSEDataRequest)
            => String.Concat(pullEVSEDataRequest.ProviderId, pullEVSEDataRequest.LastCall.HasValue ? ", last call: " + pullEVSEDataRequest.LastCall.Value.ToLocalTime().ToString() : "");

        public Func<DateTime, Object, PullEVSEDataRequest, OICPResult<PullEVSEDataResponse>, TimeSpan, String>
            PullEVSEDataResponseConverter                    { get; set; }

            = (timestamp, sender, pullEVSEDataRequest, pullEVSEDataResponse, runtime)
            => String.Concat(pullEVSEDataRequest.ProviderId, pullEVSEDataRequest.LastCall.HasValue ? ", last call: " + pullEVSEDataRequest.LastCall.Value.ToLocalTime().ToString() : "",
                             " => ",
                             pullEVSEDataResponse.Response?.StatusCode?.ToString() ?? "failed!", " ", pullEVSEDataResponse.Response?.NumberOfElements ?? 0, " evse data record(s)");

        #endregion

        #region PullEVSEStatus              (Request/Response)Converter

        public Func<DateTime, Object, PullEVSEStatusRequest, String>
            PullEVSEStatusRequestConverter                   { get; set; }

            = (timestamp, sender, pullEVSEStatusRequest)
            => String.Concat(pullEVSEStatusRequest.ProviderId, pullEVSEStatusRequest.EVSEStatusFilter.HasValue ? ", status filter: " + pullEVSEStatusRequest.EVSEStatusFilter.Value.ToString() : "");

        public Func<DateTime, Object, PullEVSEStatusRequest, OICPResult<PullEVSEStatusResponse>, TimeSpan, String>
            PullEVSEStatusResponseConverter                  { get; set; }

            = (timestamp, sender, pullEVSEStatusRequest, pullEVSEStatusResponse, runtime)
            => String.Concat(pullEVSEStatusRequest.ProviderId, pullEVSEStatusRequest.EVSEStatusFilter.HasValue ? ", status filter: " + pullEVSEStatusRequest.EVSEStatusFilter.Value.ToString() : "",
                             " => ",
                             pullEVSEStatusResponse.Response?.StatusCode?.ToString() ?? "failed!", " ", pullEVSEStatusResponse.Response?.OperatorEVSEStatus.Count() ?? 0, " evse status record(s)");

        #endregion

        #endregion

        #region Custom JSON parsers

        public CustomJObjectParserDelegate<PullEVSEDataResponse>?                                     CustomPullEVSEDataResponseParser                              { get; set; }

        public CustomJObjectParserDelegate<PullEVSEStatusResponse>?                                   CustomPullEVSEStatusResponseParser                            { get; set; }

        public CustomJObjectParserDelegate<PullEVSEStatusByIdResponse>?                               CustomPullEVSEStatusByIdResponseParser                        { get; set; }

        public CustomJObjectParserDelegate<PullEVSEStatusByOperatorIdResponse>?                       CustomPullEVSEStatusByOperatorIdResponseParser                { get; set; }


        public CustomJObjectParserDelegate<PullPricingProductDataResponse>?                           CustomPullPricingProductDataResponseParser                    { get; set; }

        public CustomJObjectParserDelegate<PullEVSEPricingResponse>?                                  CustomPullEVSEPricingResponseParser                           { get; set; }

        
        public CustomJObjectParserDelegate<Acknowledgement<PushAuthenticationDataRequest>>?           CustomPushAuthenticationDataAcknowledgementParser             { get; set; }


        public CustomJObjectParserDelegate<Acknowledgement<AuthorizeRemoteReservationStartRequest>>?  CustomAuthorizeRemoteReservationStartAcknowledgementParser    { get; set; }

        public CustomJObjectParserDelegate<Acknowledgement<AuthorizeRemoteReservationStopRequest>>?   CustomAuthorizeRemoteReservationStopAcknowledgementParser     { get; set; }

        public CustomJObjectParserDelegate<Acknowledgement<AuthorizeRemoteStartRequest>>?             CustomAuthorizeRemoteStartAcknowledgementParser               { get; set; }

        public CustomJObjectParserDelegate<Acknowledgement<AuthorizeRemoteStopRequest>>?              CustomAuthorizeRemoteStopAcknowledgementParser                { get; set; }


        public CustomJObjectParserDelegate<GetChargeDetailRecordsResponse>?                           CustomGetChargeDetailRecordsResponseParser                    { get; set; }


        public CustomJObjectParserDelegate<StatusCode>?                                               CustomStatusCodeParser                                        { get; set; }

        #endregion

        #region Custom JSON serializers

        public CustomJObjectSerializerDelegate<PullEVSEDataRequest>?                     CustomPullEVSEDataRequestSerializer                        { get; set; }
        public CustomJObjectSerializerDelegate<GeoCoordinates>?                          CustomGeoCoordinatesSerializer                             { get; set; }

        public CustomJObjectSerializerDelegate<PullEVSEStatusRequest>?                   CustomPullEVSEStatusRequestSerializer                      { get; set; }

        public CustomJObjectSerializerDelegate<PullEVSEStatusByIdRequest>?               CustomPullEVSEStatusByIdRequestSerializer                  { get; set; }

        public CustomJObjectSerializerDelegate<PullEVSEStatusByOperatorIdRequest>?       CustomPullEVSEStatusByOperatorIdRequestSerializer          { get; set; }


        public CustomJObjectSerializerDelegate<PullPricingProductDataRequest>?           CustomPullPricingProductDataRequestSerializer              { get; set; }

        public CustomJObjectSerializerDelegate<PullEVSEPricingRequest>?                  CustomPullEVSEPricingRequestSerializer                     { get; set; }


        public CustomJObjectSerializerDelegate<PushAuthenticationDataRequest>?           CustomPushAuthenticationDataRequestSerializer              { get; set; }
        public CustomJObjectSerializerDelegate<ProviderAuthenticationData>?              CustomProviderAuthenticationDataSerializer                 { get; set; }
        public CustomJObjectSerializerDelegate<Identification>?                          CustomIdentificationSerializer                             { get; set; }


        public CustomJObjectSerializerDelegate<AuthorizeRemoteReservationStartRequest>?  CustomAuthorizeRemoteReservationStartRequestSerializer     { get; set; }
        public CustomJObjectSerializerDelegate<AuthorizeRemoteReservationStopRequest>?   CustomAuthorizeRemoteReservationStopRequestSerializer      { get; set; }
        public CustomJObjectSerializerDelegate<AuthorizeRemoteStartRequest>?             CustomAuthorizeRemoteStartRequestSerializer                { get; set; }
        public CustomJObjectSerializerDelegate<AuthorizeRemoteStopRequest>?              CustomAuthorizeRemoteStopRequestSerializer                 { get; set; }


        public CustomJObjectSerializerDelegate<GetChargeDetailRecordsRequest>?           CustomGetChargeDetailRecordsRequestSerializer              { get; set; }

        #endregion

        #region Events

        #region OnPullEVSEDataRequest/-Response

        /// <summary>
        /// An event fired whenever a PullEVSEData request will be send.
        /// </summary>
        public event OnPullEVSEDataRequestDelegate?   OnPullEVSEDataRequest;

        /// <summary>
        /// An event fired whenever a PullEVSEData HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?         OnPullEVSEDataHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for a PullEVSEData HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?        OnPullEVSEDataHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for a PullEVSEData request had been received.
        /// </summary>
        public event OnPullEVSEDataResponseDelegate?  OnPullEVSEDataResponse;

        #endregion

        #region OnPullEVSEStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a PullEVSEStatus request will be send.
        /// </summary>
        public event OnPullEVSEStatusRequestDelegate?   OnPullEVSEStatusRequest;

        /// <summary>
        /// An event fired whenever a PullEVSEStatus HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?           OnPullEVSEStatusHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for a PullEVSEStatus HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?          OnPullEVSEStatusHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for a PullEVSEStatus request had been received.
        /// </summary>
        public event OnPullEVSEStatusResponseDelegate?  OnPullEVSEStatusResponse;

        #endregion

        #region OnPullEVSEStatusByIdRequest/-Response

        /// <summary>
        /// An event fired whenever a PullEVSEStatusById request will be send.
        /// </summary>
        public event OnPullEVSEStatusByIdRequestDelegate?   OnPullEVSEStatusByIdRequest;

        /// <summary>
        /// An event fired whenever a PullEVSEStatusById HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?               OnPullEVSEStatusByIdHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for a PullEVSEStatusById HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?              OnPullEVSEStatusByIdHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for a PullEVSEStatusById request had been received.
        /// </summary>
        public event OnPullEVSEStatusByIdResponseDelegate?  OnPullEVSEStatusByIdResponse;

        #endregion

        #region OnPullEVSEStatusByOperatorIdRequest/-Response

        /// <summary>
        /// An event fired whenever a PullEVSEStatusByOperatorId request will be send.
        /// </summary>
        public event OnPullEVSEStatusByOperatorIdRequestDelegate?   OnPullEVSEStatusByOperatorIdRequest;

        /// <summary>
        /// An event fired whenever a PullEVSEStatusByOperatorId HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                       OnPullEVSEStatusByOperatorIdHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for a PullEVSEStatusByOperatorId HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                      OnPullEVSEStatusByOperatorIdHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for a PullEVSEStatusByOperatorId request had been received.
        /// </summary>
        public event OnPullEVSEStatusByOperatorIdResponseDelegate?  OnPullEVSEStatusByOperatorIdResponse;

        #endregion


        #region OnPullPricingProductDataRequest/-Response

        /// <summary>
        /// An event fired whenever a PullPricingProductData request will be send.
        /// </summary>
        public event OnPullPricingProductDataRequestDelegate?   OnPullPricingProductDataRequest;

        /// <summary>
        /// An event fired whenever a PullPricingProductData HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                   OnPullPricingProductDataHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for a PullPricingProductData HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                  OnPullPricingProductDataHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for a PullPricingProductData request had been received.
        /// </summary>
        public event OnPullPricingProductDataResponseDelegate?  OnPullPricingProductDataResponse;

        #endregion

        #region OnPullEVSEPricingRequest/-Response

        /// <summary>
        /// An event fired whenever a PullEVSEPricing request will be send.
        /// </summary>
        public event OnPullEVSEPricingRequestDelegate?   OnPullEVSEPricingRequest;

        /// <summary>
        /// An event fired whenever a PullEVSEPricing HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?            OnPullEVSEPricingHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for a PullEVSEPricing HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?           OnPullEVSEPricingHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for a PullEVSEPricing request had been received.
        /// </summary>
        public event OnPullEVSEPricingResponseDelegate?  OnPullEVSEPricingResponse;

        #endregion


        #region OnPushAuthenticationDataRequest/-Response

        /// <summary>
        /// An event fired whenever an PushAuthenticationData request will be send.
        /// </summary>
        public event OnPushAuthenticationDataRequestDelegate?   OnPushAuthenticationDataRequest;

        /// <summary>
        /// An event fired whenever an PushAuthenticationData HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                   OnPushAuthenticationDataHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for an PushAuthenticationData HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                  OnPushAuthenticationDataHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for an PushAuthenticationData request had been received.
        /// </summary>
        public event OnPushAuthenticationDataResponseDelegate?  OnPushAuthenticationDataResponse;

        #endregion


        #region OnAuthorizeRemoteReservationStartRequest/-Response

        /// <summary>
        /// An event fired whenever an AuthorizeRemoteReservationReservationStart request will be send.
        /// </summary>
        public event OnAuthorizeRemoteReservationStartRequestDelegate?   OnAuthorizeRemoteReservationStartRequest;

        /// <summary>
        /// An event fired whenever an AuthorizeRemoteReservationReservationStart HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                            OnAuthorizeRemoteReservationStartHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeRemoteReservationReservationStart HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                           OnAuthorizeRemoteReservationStartHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeRemoteReservationReservationStart request had been received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStartResponseDelegate?  OnAuthorizeRemoteReservationStartResponse;

        #endregion

        #region OnAuthorizeRemoteReservationStopRequest/-Response

        /// <summary>
        /// An event fired whenever an AuthorizeRemoteReservationReservationStop request will be send.
        /// </summary>
        public event OnAuthorizeRemoteReservationStopRequestDelegate?   OnAuthorizeRemoteReservationStopRequest;

        /// <summary>
        /// An event fired whenever an AuthorizeRemoteReservationReservationStop HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                           OnAuthorizeRemoteReservationStopHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeRemoteReservationReservationStop HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                          OnAuthorizeRemoteReservationStopHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeRemoteReservationReservationStop request had been received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStopResponseDelegate?  OnAuthorizeRemoteReservationStopResponse;

        #endregion

        #region OnAuthorizeRemoteStartRequest/-Response

        /// <summary>
        /// An event fired whenever an AuthorizeRemoteStart request will be send.
        /// </summary>
        public event OnAuthorizeRemoteStartRequestDelegate?   OnAuthorizeRemoteStartRequest;

        /// <summary>
        /// An event fired whenever an AuthorizeRemoteStart HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                 OnAuthorizeRemoteStartHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeRemoteStart HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                OnAuthorizeRemoteStartHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeRemoteStart request had been received.
        /// </summary>
        public event OnAuthorizeRemoteStartResponseDelegate?  OnAuthorizeRemoteStartResponse;

        #endregion

        #region OnAuthorizeRemoteStopRequest/-Response

        /// <summary>
        /// An event fired whenever an AuthorizeRemoteStop request will be send.
        /// </summary>
        public event OnAuthorizeRemoteStopRequestDelegate?   OnAuthorizeRemoteStopRequest;

        /// <summary>
        /// An event fired whenever an AuthorizeRemoteStop HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                OnAuthorizeRemoteStopHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeRemoteStop HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?               OnAuthorizeRemoteStopHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for an AuthorizeRemoteStop request had been received.
        /// </summary>
        public event OnAuthorizeRemoteStopResponseDelegate?  OnAuthorizeRemoteStopResponse;

        #endregion


        #region OnGetChargeDetailRecordsRequest/-Response

        /// <summary>
        /// An event fired whenever a GetChargeDetailRecords request will be send.
        /// </summary>
        public event OnGetChargeDetailRecordsRequestDelegate?   OnGetChargeDetailRecordsRequest;

        /// <summary>
        /// An event fired whenever a GetChargeDetailRecords HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                   OnGetChargeDetailRecordsHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for a GetChargeDetailRecords HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                  OnGetChargeDetailRecordsHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for a GetChargeDetailRecords request had been received.
        /// </summary>
        public event OnGetChargeDetailRecordsResponseDelegate?  OnGetChargeDetailRecordsResponse;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EMP client.
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
        public EMPClient(URL?                                  RemoteURL                    = null,
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
                         String                                LoggingContext               = EMPClientLogger.DefaultContext,
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

            this.Counters    = new APICounters();

            this.JSONFormat  = Newtonsoft.Json.Formatting.None;

            base.HTTPLogger  = DisableLogging == false
                                   ? new HTTP_Logger(this,
                                                     LoggingPath,
                                                     LoggingContext,
                                                     LogfileCreator)
                                   : null;

            this.Logger      = DisableLogging == false
                                   ? new EMPClientLogger(this,
                                                         LoggingPath,
                                                         LoggingContext,
                                                         LogfileCreator)
                                   : null;

        }

        #endregion


        //public override JObject ToJSON()
        //    => base.ToJSON(nameof(EMPClient));


        #region PullEVSEData                   (Request)

        /// <summary>
        /// Download EVSE data records.
        /// The request might either have none, 'SearchCenter + DistanceKM' or 'LastCall' parameters.
        /// Because of limitations at Hubject the SearchCenter and LastCall parameters can not be used at the same time!
        /// </summary>
        /// <param name="Request">A PullEVSEData request.</param>
        public async Task<OICPResult<PullEVSEDataResponse>>

            PullEVSEData(PullEVSEDataRequest Request)

        {

            #region Initial checks

            //Request = _CustomPullEVSEDataRequestMapper(Request);

            Byte                               TransmissionRetry   = 0;
            OICPResult<PullEVSEDataResponse>?  result              = null;

            #endregion

            #region Send OnPullEVSEDataRequest event

            var startTime = Timestamp.Now;

            Counters.PullEVSEData.IncRequests_OK();

            try
            {

                if (OnPullEVSEDataRequest is not null)
                    await Task.WhenAll(OnPullEVSEDataRequest.GetInvocationList().
                                       Cast<OnPullEVSEDataRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnPullEVSEDataRequest));
            }

            #endregion


            try
            {

                do
                {

                    #region Upstream HTTP request...

                    #region Create pagination query string

                    // ?page=0&size=20

                    var queryStrings = new List<String>();

                    if (Request.Page.HasValue)
                        queryStrings.Add("page=" + Request.Page.Value);

                    if (Request.Size.HasValue)
                        queryStrings.Add("size=" + Request.Size.Value);

                    var queryString = queryStrings.Count > 0
                                          ? "?" + queryStrings.AggregateWith("&")
                                          : "";

                    #endregion

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

                                              Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/evsepull/v23/providers/" + Request.ProviderId.ToString().Replace("*", "%2A") + "/data-records" + queryString),
                                                                                   requestbuilder => {
                                                                                       requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                       requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                       requestbuilder.Content      = Request.ToJSON(CustomPullEVSEDataRequestSerializer,
                                                                                                                                    CustomGeoCoordinatesSerializer).
                                                                                                                             ToString(JSONFormat).
                                                                                                                             ToUTF8Bytes();
                                                                                       requestbuilder.Connection   = "close";
                                                                                   }),

                                                      RequestLogDelegate:   OnPullEVSEDataHTTPRequest,
                                                      ResponseLogDelegate:  OnPullEVSEDataHTTPResponse,
                                                      CancellationToken:    Request.CancellationToken,
                                                      EventTrackingId:      Request.EventTrackingId,
                                                      RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                              ConfigureAwait(false);

                    #endregion


                    var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse) ?? Process_Id.NewRandom;

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                if (PullEVSEDataResponse.TryParse(Request,
                                                                  JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                  HTTPResponse.Timestamp,
                                                                  HTTPResponse.EventTrackingId,
                                                                  HTTPResponse.Runtime,
                                                                  out PullEVSEDataResponse?  pullEVSEDataResponse,
                                                                  out String?                ErrorResponse,
                                                                  processId,
                                                                  HTTPResponse,
                                                                  CustomPullEVSEDataResponseParser))
                                {

                                    Counters.PullEVSEData.IncResponses_OK();

                                    result = OICPResult<PullEVSEDataResponse>.Success(Request,
                                                                                      pullEVSEDataResponse!,
                                                                                      processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<PullEVSEDataResponse>.Failed(
                                             Request,
                                             new PullEVSEDataResponse(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 Array.Empty<EVSEDataRecord>(),
                                                 Request,
                                                 StatusCode: new StatusCode(
                                                                 StatusCodes.SystemError,
                                                                 e.Message,
                                                                 e.StackTrace
                                                             )
                                             )
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

                                result = OICPResult<PullEVSEDataResponse>.BadRequest(Request,
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
                                                        out String?      ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<PullEVSEDataResponse>.Failed(Request,
                                                                                     new PullEVSEDataResponse(
                                                                                         HTTPResponse.Timestamp,
                                                                                         HTTPResponse.EventTrackingId,
                                                                                         processId,
                                                                                         HTTPResponse.Runtime,
                                                                                         Array.Empty<EVSEDataRecord>(),
                                                                                         Request,
                                                                                         StatusCode:   statusCode,
                                                                                         HTTPResponse: HTTPResponse
                                                                                     ),
                                                                                     processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<PullEVSEDataResponse>.Failed(
                                             Request,
                                             new PullEVSEDataResponse(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 Array.Empty<EVSEDataRecord>(),
                                                 Request,
                                                 StatusCode: new StatusCode(
                                                                 StatusCodes.SystemError,
                                                                 e.Message,
                                                                 e.StackTrace
                                                             )
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

                result = OICPResult<PullEVSEDataResponse>.Failed(
                             Request,
                             new PullEVSEDataResponse(
                                 Timestamp.Now,
                                 Request.EventTrackingId ?? EventTracking_Id.New,
                                 Process_Id.NewRandom,
                                 Timestamp.Now - Request.Timestamp,
                                 Array.Empty<EVSEDataRecord>(),
                                 Request,
                                 StatusCode: new StatusCode(
                                                 StatusCodes.SystemError,
                                                 e.Message,
                                                 e.StackTrace
                                             )
                             )
                         );

            }

            result ??= OICPResult<PullEVSEDataResponse>.Failed(
                           Request,
                           new PullEVSEDataResponse(
                               Timestamp.Now,
                               Request.EventTrackingId ?? EventTracking_Id.New,
                               Process_Id.NewRandom,
                               Timestamp.Now - Request.Timestamp,
                               Array.Empty<EVSEDataRecord>(),
                               Request,
                               StatusCode: new StatusCode(
                                               StatusCodes.SystemError,
                                               "HTTP request failed!"
                                           )
                           )
                       );

            if (result.IsNotSuccessful)
                Counters.PullEVSEData.IncResponses_Error();


            #region Send OnPullEVSEDataResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnPullEVSEDataResponse is not null)
                    await Task.WhenAll(OnPullEVSEDataResponse.GetInvocationList().
                                       Cast<OnPullEVSEDataResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnPullEVSEDataResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region PullEVSEStatus                 (Request)

        /// <summary>
        /// Download EVSE status records.
        /// The request might have an optional search radius and/or status filter.
        /// </summary>
        /// <param name="Request">A PullEVSEStatus request.</param>
        public async Task<OICPResult<PullEVSEStatusResponse>>

            PullEVSEStatus(PullEVSEStatusRequest Request)

        {

            #region Initial checks

            //Request = _CustomPullEVSEStatusRequestMapper(Request);

            Byte                                 TransmissionRetry   = 0;
            OICPResult<PullEVSEStatusResponse>?  result              = null;

            #endregion

            #region Send OnPullEVSEStatusRequest event

            var startTime = Timestamp.Now;

            Counters.PullEVSEStatus.IncRequests_OK();

            try
            {

                if (OnPullEVSEStatusRequest is not null)
                    await Task.WhenAll(OnPullEVSEStatusRequest.GetInvocationList().
                                       Cast<OnPullEVSEStatusRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnPullEVSEStatusRequest));
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

                                              Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/evsepull/v21/providers/" + Request.ProviderId.ToString().Replace("*", "%2A") + "/status-records"),
                                                                                   requestbuilder => {
                                                                                       requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                       requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                       requestbuilder.Content      = Request.ToJSON(CustomPullEVSEStatusRequestSerializer,
                                                                                                                                    CustomGeoCoordinatesSerializer).
                                                                                                                             ToString(JSONFormat).
                                                                                                                             ToUTF8Bytes();
                                                                                       requestbuilder.Connection   = "close";
                                                                                   }),

                                                      RequestLogDelegate:   OnPullEVSEStatusHTTPRequest,
                                                      ResponseLogDelegate:  OnPullEVSEStatusHTTPResponse,
                                                      CancellationToken:    Request.CancellationToken,
                                                      EventTrackingId:      Request.EventTrackingId,
                                                      RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                              ConfigureAwait(false);

                    #endregion


                    var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse) ?? Process_Id.NewRandom;

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                if (PullEVSEStatusResponse.TryParse(Request,
                                                                    JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                    HTTPResponse.Timestamp,
                                                                    HTTPResponse.EventTrackingId,
                                                                    HTTPResponse.Runtime,
                                                                    out PullEVSEStatusResponse?  pullEVSEStatusResponse,
                                                                    out String?                  ErrorResponse,
                                                                    processId,
                                                                    HTTPResponse,
                                                                    CustomPullEVSEStatusResponseParser))
                                {

                                    Counters.PullEVSEStatus.IncResponses_OK();

                                    result = OICPResult<PullEVSEStatusResponse>.Success(Request,
                                                                                        pullEVSEStatusResponse!,
                                                                                        processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<PullEVSEStatusResponse>.Failed(
                                             Request,
                                             new PullEVSEStatusResponse(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 Array.Empty<OperatorEVSEStatus>(),
                                                 Request,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 )
                                             )
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

                                result = OICPResult<PullEVSEStatusResponse>.BadRequest(Request,
                                                                                       validationErrorList,
                                                                                       processId);

                            }

                        }

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                    {

                        // HTTP/1.1 403 Forbidden
                        // Server: nginx/1.18.0 (Ubuntu)
                        // Date: Wed, 03 Aug 2022 18:20:47 GMT
                        // Content-Type: text/html
                        // Content-Length: 162
                        // Connection: close
                        // 
                        // <html>
                        // <head><title>403 Forbidden</title></head>
                        // <body>
                        // <center><h1>403 Forbidden</h1></center>
                        // <hr><center>nginx/1.18.0 (Ubuntu)</center>
                        // </body>
                        // </html>

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
                                                        out String?      ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<PullEVSEStatusResponse>.Failed(Request,
                                                                                       new PullEVSEStatusResponse(
                                                                                           HTTPResponse.Timestamp,
                                                                                           HTTPResponse.EventTrackingId,
                                                                                           processId,
                                                                                           HTTPResponse.Runtime,
                                                                                           Array.Empty<OperatorEVSEStatus>(),
                                                                                           Request,
                                                                                           statusCode,
                                                                                           HTTPResponse
                                                                                       ),
                                                                                       processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<PullEVSEStatusResponse>.Failed(
                                             Request,
                                             new PullEVSEStatusResponse(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 Array.Empty<OperatorEVSEStatus>(),
                                                 Request,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 )
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

                result = OICPResult<PullEVSEStatusResponse>.Failed(
                             Request,
                             new PullEVSEStatusResponse(
                                 Timestamp.Now,
                                 Request.EventTrackingId ?? EventTracking_Id.New,
                                 Process_Id.NewRandom,
                                 Timestamp.Now - Request.Timestamp,
                                 Array.Empty<OperatorEVSEStatus>(),
                                 Request,
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     e.Message,
                                     e.StackTrace
                                 )
                             )
                         );

            }

            result ??= OICPResult<PullEVSEStatusResponse>.Failed(
                           Request,
                           new PullEVSEStatusResponse(
                               Timestamp.Now,
                               Request.EventTrackingId ?? EventTracking_Id.New,
                               Process_Id.NewRandom,
                               Timestamp.Now - Request.Timestamp,
                               Array.Empty<OperatorEVSEStatus>(),
                               Request,
                               new StatusCode(
                                   StatusCodes.SystemError,
                                   "HTTP request failed!"
                               )
                           )
                       );

            if (result.IsNotSuccessful)
                Counters.PullEVSEStatus.IncResponses_Error();


            #region Send OnPullEVSEStatusResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnPullEVSEStatusResponse is not null)
                    await Task.WhenAll(OnPullEVSEStatusResponse.GetInvocationList().
                                       Cast<OnPullEVSEStatusResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnPullEVSEStatusResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region PullEVSEStatusById             (Request)

        /// <summary>
        /// Download the current status of up to 100 EVSEs.
        /// </summary>
        /// <param name="Request">A PullEVSEStatusById request.</param>
        public async Task<OICPResult<PullEVSEStatusByIdResponse>>

            PullEVSEStatusById(PullEVSEStatusByIdRequest Request)

        {

            #region Initial checks

            //Request = _CustomPullEVSEStatusByIdRequestMapper(Request);

            Byte                                     TransmissionRetry   = 0;
            OICPResult<PullEVSEStatusByIdResponse>?  result              = null;

            #endregion

            #region Send OnPullEVSEStatusByIdRequest event

            var startTime = Timestamp.Now;

            Counters.PullEVSEStatusById.IncRequests_OK();

            try
            {

                if (OnPullEVSEStatusByIdRequest is not null)
                    await Task.WhenAll(OnPullEVSEStatusByIdRequest.GetInvocationList().
                                       Cast<OnPullEVSEStatusByIdRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnPullEVSEStatusByIdRequest));
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

                                              Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/evsepull/v21/providers/" + Request.ProviderId.ToString().Replace("*", "%2A") + "/status-records-by-id"),
                                                                                   requestbuilder => {
                                                                                       requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                       requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                       requestbuilder.Content      = Request.ToJSON(CustomPullEVSEStatusByIdRequestSerializer).
                                                                                                                             ToString(JSONFormat).
                                                                                                                             ToUTF8Bytes();
                                                                                       requestbuilder.Connection   = "close";
                                                                                   }),

                                                      RequestLogDelegate:   OnPullEVSEStatusByIdHTTPRequest,
                                                      ResponseLogDelegate:  OnPullEVSEStatusByIdHTTPResponse,
                                                      CancellationToken:    Request.CancellationToken,
                                                      EventTrackingId:      Request.EventTrackingId,
                                                      RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                              ConfigureAwait(false);

                    #endregion


                    var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse) ?? Process_Id.NewRandom;

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                if (PullEVSEStatusByIdResponse.TryParse(Request,
                                                                        JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                        HTTPResponse.Timestamp,
                                                                        HTTPResponse.EventTrackingId,
                                                                        HTTPResponse.Runtime,
                                                                        out PullEVSEStatusByIdResponse?  pullEVSEStatusByIdResponse,
                                                                        out String?                      ErrorResponse,
                                                                        processId,
                                                                        HTTPResponse,
                                                                        CustomPullEVSEStatusByIdResponseParser))
                                {

                                    Counters.PullEVSEStatusById.IncResponses_OK();

                                    result = OICPResult<PullEVSEStatusByIdResponse>.Success(Request,
                                                                                            pullEVSEStatusByIdResponse!,
                                                                                            processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<PullEVSEStatusByIdResponse>.Failed(
                                             Request,
                                             new PullEVSEStatusByIdResponse(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 Array.Empty<EVSEStatusRecord>(),
                                                 Request,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 )
                                             )
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

                                result = OICPResult<PullEVSEStatusByIdResponse>.BadRequest(Request,
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
                                                        out String?      ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<PullEVSEStatusByIdResponse>.Failed(Request,
                                                                                       new PullEVSEStatusByIdResponse(
                                                                                           HTTPResponse.Timestamp,
                                                                                           HTTPResponse.EventTrackingId,
                                                                                           processId,
                                                                                           HTTPResponse.Runtime,
                                                                                           Array.Empty<EVSEStatusRecord>(),
                                                                                           Request,
                                                                                           statusCode,
                                                                                           HTTPResponse
                                                                                       ),
                                                                                       processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<PullEVSEStatusByIdResponse>.Failed(
                                             Request,
                                             new PullEVSEStatusByIdResponse(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 Array.Empty<EVSEStatusRecord>(),
                                                 Request,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 )
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

                result = OICPResult<PullEVSEStatusByIdResponse>.Failed(
                             Request,
                             new PullEVSEStatusByIdResponse(
                                 Timestamp.Now,
                                 Request.EventTrackingId ?? EventTracking_Id.New,
                                 Process_Id.NewRandom,
                                 Timestamp.Now - Request.Timestamp,
                                 Array.Empty<EVSEStatusRecord>(),
                                 Request,
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     e.Message,
                                     e.StackTrace
                                 )
                             )
                         );

            }

            result ??= OICPResult<PullEVSEStatusByIdResponse>.Failed(
                           Request,
                           new PullEVSEStatusByIdResponse(
                               Timestamp.Now,
                               Request.EventTrackingId ?? EventTracking_Id.New,
                               Process_Id.NewRandom,
                               Timestamp.Now - Request.Timestamp,
                               Array.Empty<EVSEStatusRecord>(),
                               Request,
                               new StatusCode(
                                   StatusCodes.SystemError,
                                   "HTTP request failed!"
                               )
                           )
                       );

            if (result.IsNotSuccessful)
                Counters.PullEVSEStatusById.IncResponses_Error();


            #region Send OnPullEVSEStatusByIdResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnPullEVSEStatusByIdResponse is not null)
                    await Task.WhenAll(OnPullEVSEStatusByIdResponse.GetInvocationList().
                                       Cast<OnPullEVSEStatusByIdResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnPullEVSEStatusByIdResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region PullEVSEStatusByOperatorId     (Request)

        /// <summary>
        /// Download the current EVSE status of the given charge point operators.
        /// </summary>
        /// <param name="Request">A PullEVSEStatusByOperatorId request.</param>
        public async Task<OICPResult<PullEVSEStatusByOperatorIdResponse>>

            PullEVSEStatusByOperatorId(PullEVSEStatusByOperatorIdRequest Request)

        {

            #region Initial checks

            //Request = _CustomPullEVSEStatusByOperatorIdRequestMapper(Request);

            Byte                                             TransmissionRetry   = 0;
            OICPResult<PullEVSEStatusByOperatorIdResponse>?  result              = null;

            #endregion

            #region Send OnPullEVSEStatusByOperatorIdRequest event

            var startTime = Timestamp.Now;

            Counters.PullEVSEStatusByOperatorId.IncRequests_OK();

            try
            {

                if (OnPullEVSEStatusByOperatorIdRequest is not null)
                    await Task.WhenAll(OnPullEVSEStatusByOperatorIdRequest.GetInvocationList().
                                       Cast<OnPullEVSEStatusByOperatorIdRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnPullEVSEStatusByOperatorIdRequest));
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

                                              Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/evsepull/v21/providers/" + Request.ProviderId.ToString().Replace("*", "%2A") + "/status-records-by-operator-id"),
                                                                                   requestbuilder => {
                                                                                       requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                       requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                       requestbuilder.Content      = Request.ToJSON(CustomPullEVSEStatusByOperatorIdRequestSerializer).
                                                                                                                             ToString(JSONFormat).
                                                                                                                             ToUTF8Bytes();
                                                                                       requestbuilder.Connection   = "close";
                                                                                   }),

                                                      RequestLogDelegate:   OnPullEVSEStatusByOperatorIdHTTPRequest,
                                                      ResponseLogDelegate:  OnPullEVSEStatusByOperatorIdHTTPResponse,
                                                      CancellationToken:    Request.CancellationToken,
                                                      EventTrackingId:      Request.EventTrackingId,
                                                      RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                              ConfigureAwait(false);

                    #endregion


                    var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse) ?? Process_Id.NewRandom;

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                if (PullEVSEStatusByOperatorIdResponse.TryParse(Request,
                                                                                JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                                HTTPResponse.Timestamp,
                                                                                HTTPResponse.EventTrackingId,
                                                                                HTTPResponse.Runtime,
                                                                                out PullEVSEStatusByOperatorIdResponse?  pullEVSEStatusResponse,
                                                                                out String?                              ErrorResponse,
                                                                                processId,
                                                                                HTTPResponse,
                                                                                CustomPullEVSEStatusByOperatorIdResponseParser))
                                {

                                    Counters.PullEVSEStatusByOperatorId.IncResponses_OK();

                                    result = OICPResult<PullEVSEStatusByOperatorIdResponse>.Success(Request,
                                                                                                    pullEVSEStatusResponse!,
                                                                                                    processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<PullEVSEStatusByOperatorIdResponse>.Failed(
                                             Request,
                                             new PullEVSEStatusByOperatorIdResponse(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 Array.Empty<OperatorEVSEStatus>(),
                                                 Request,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 )
                                             )
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

                                result = OICPResult<PullEVSEStatusByOperatorIdResponse>.BadRequest(Request,
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
                                                        out String?      ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<PullEVSEStatusByOperatorIdResponse>.Failed(Request,
                                                                                                   new PullEVSEStatusByOperatorIdResponse(
                                                                                                       HTTPResponse.Timestamp,
                                                                                                       HTTPResponse.EventTrackingId,
                                                                                                       processId,
                                                                                                       HTTPResponse.Runtime,
                                                                                                       Array.Empty<OperatorEVSEStatus>(),
                                                                                                       Request,
                                                                                                       statusCode,
                                                                                                       HTTPResponse
                                                                                                   ),
                                                                                                   processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<PullEVSEStatusByOperatorIdResponse>.Failed(
                                             Request,
                                             new PullEVSEStatusByOperatorIdResponse(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 Array.Empty<OperatorEVSEStatus>(),
                                                 Request,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 )
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

                result = OICPResult<PullEVSEStatusByOperatorIdResponse>.Failed(
                             Request,
                             new PullEVSEStatusByOperatorIdResponse(
                                 Timestamp.Now,
                                 Request.EventTrackingId ?? EventTracking_Id.New,
                                 Process_Id.NewRandom,
                                 Timestamp.Now - Request.Timestamp,
                                 Array.Empty<OperatorEVSEStatus>(),
                                 Request,
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     e.Message,
                                     e.StackTrace
                                 )
                             )
                         );

            }

            result ??= OICPResult<PullEVSEStatusByOperatorIdResponse>.Failed(
                           Request,
                           new PullEVSEStatusByOperatorIdResponse(
                               Timestamp.Now,
                               Request.EventTrackingId ?? EventTracking_Id.New,
                               Process_Id.NewRandom,
                               Timestamp.Now - Request.Timestamp,
                               Array.Empty<OperatorEVSEStatus>(),
                               Request,
                               new StatusCode(
                                   StatusCodes.SystemError,
                                   "HTTP request failed!"
                               )
                           )
                       );

            if (result.IsNotSuccessful)
                Counters.PullEVSEStatusByOperatorId.IncResponses_Error();


            #region Send OnPullEVSEStatusByOperatorIdResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnPullEVSEStatusByOperatorIdResponse is not null)
                    await Task.WhenAll(OnPullEVSEStatusByOperatorIdResponse.GetInvocationList().
                                       Cast<OnPullEVSEStatusByOperatorIdResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnPullEVSEStatusByOperatorIdResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region PullPricingProductData         (Request)

        /// <summary>
        /// Download pricing product data.
        /// </summary>
        /// <param name="Request">A PullPricingProductData request.</param>
        public async Task<OICPResult<PullPricingProductDataResponse>>

            PullPricingProductData(PullPricingProductDataRequest Request)

        {

            #region Initial checks

            //Request = _CustomPullPricingProductDataRequestMapper(Request);

            Byte                                         TransmissionRetry   = 0;
            OICPResult<PullPricingProductDataResponse>?  result              = null;

            #endregion

            #region Send OnPullPricingProductDataRequest event

            var startTime = Timestamp.Now;

            Counters.PullPricingProductData.IncRequests_OK();

            try
            {

                if (OnPullPricingProductDataRequest is not null)
                    await Task.WhenAll(OnPullPricingProductDataRequest.GetInvocationList().
                                       Cast<OnPullPricingProductDataRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnPullPricingProductDataRequest));
            }

            #endregion


            try
            {

                do
                {

                    #region Upstream HTTP request...

                    #region Create pagination query string

                    // ?page=0&size=20

                    var queryStrings = new List<String>();

                    if (Request.Page.HasValue)
                        queryStrings.Add("page=" + Request.Page.Value);

                    if (Request.Size.HasValue)
                        queryStrings.Add("size=" + Request.Size.Value);

                    var queryString = queryStrings.Count > 0
                                          ? "?" + queryStrings.AggregateWith("&")
                                          : "";

                    #endregion

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

                                              Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/dynamicpricing/v10/providers/" + Request.ProviderId.ToString().Replace("*", "%2A") + "/pricing-products" + queryString),
                                                                                   requestbuilder => {
                                                                                       requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                       requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                       requestbuilder.Content      = Request.ToJSON(CustomPullPricingProductDataRequestSerializer).
                                                                                                                             ToString(JSONFormat).
                                                                                                                             ToUTF8Bytes();
                                                                                       requestbuilder.Connection   = "close";
                                                                                   }),

                                                      RequestLogDelegate:   OnPullPricingProductDataHTTPRequest,
                                                      ResponseLogDelegate:  OnPullPricingProductDataHTTPResponse,
                                                      CancellationToken:    Request.CancellationToken,
                                                      EventTrackingId:      Request.EventTrackingId,
                                                      RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                              ConfigureAwait(false);

                    #endregion


                    var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse) ?? Process_Id.NewRandom;

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                if (PullPricingProductDataResponse.TryParse(Request,
                                                                            JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                            HTTPResponse.Timestamp,
                                                                            HTTPResponse.EventTrackingId,
                                                                            HTTPResponse.Runtime,
                                                                            out PullPricingProductDataResponse?  pullEVSEDataResponse,
                                                                            out String?                          ErrorResponse,
                                                                            processId,
                                                                            HTTPResponse,
                                                                            CustomPullPricingProductDataResponseParser))
                                {

                                    Counters.PullPricingProductData.IncResponses_OK();

                                    result = OICPResult<PullPricingProductDataResponse>.Success(Request,
                                                                                                pullEVSEDataResponse!,
                                                                                                processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<PullPricingProductDataResponse>.Failed(
                                             Request,
                                             new PullPricingProductDataResponse(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 Array.Empty<PricingProductData>(),
                                                 Request,
                                                 StatusCode: new StatusCode(
                                                                 StatusCodes.SystemError,
                                                                 e.Message,
                                                                 e.StackTrace
                                                             )
                                             )
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

                                result = OICPResult<PullPricingProductDataResponse>.BadRequest(Request,
                                                                                               validationErrorList,
                                                                                               processId);

                            }

                        }

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                    {

                        // Until now...
                        // Hubject firewall problem!
                        // Only HTML response!

                        // Now also...

                        // HTTP/1.1 403
                        // Server:          nginx/1.18.0 (Ubuntu)
                        // Date:            Tue, 02 Aug 2022 17:41:47 GMT
                        // Content-Type:    application/json;charset=ISO-8859-1
                        // Content-Length:  96
                        // Connection:      close
                        // Process-ID:      b3ca0072-d207-4038-8ecf-e50162737f24
                        // 
                        // {
                        //     "StatusCode": {
                        //         "Code":            "210",
                        //         "Description":     "No active subscription found",
                        //         "AdditionalInfo":   null
                        //     }
                        // }

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                if (StatusCode.TryParse(JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String())["StatusCode"] as JObject,
                                                        out StatusCode?  statusCode,
                                                        out String?      ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<PullPricingProductDataResponse>.Failed(Request,
                                                                                               new PullPricingProductDataResponse(
                                                                                                   HTTPResponse.Timestamp,
                                                                                                   HTTPResponse.EventTrackingId,
                                                                                                   processId,
                                                                                                   HTTPResponse.Runtime,
                                                                                                   Array.Empty<PricingProductData>(),
                                                                                                   Request,
                                                                                                   StatusCode:   statusCode,
                                                                                                   HTTPResponse: HTTPResponse
                                                                                               ),
                                                                                               processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<PullPricingProductDataResponse>.Failed(
                                             Request,
                                             new PullPricingProductDataResponse(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 Array.Empty<PricingProductData>(),
                                                 Request,
                                                 StatusCode: new StatusCode(
                                                                 StatusCodes.SystemError,
                                                                 e.Message,
                                                                 e.StackTrace
                                                             )
                                             )
                                         );

                            }

                        }

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
                                                        out String?      ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<PullPricingProductDataResponse>.Failed(Request,
                                                                                               new PullPricingProductDataResponse(
                                                                                                   HTTPResponse.Timestamp,
                                                                                                   HTTPResponse.EventTrackingId,
                                                                                                   processId,
                                                                                                   HTTPResponse.Runtime,
                                                                                                   Array.Empty<PricingProductData>(),
                                                                                                   Request,
                                                                                                   StatusCode:   statusCode,
                                                                                                   HTTPResponse: HTTPResponse
                                                                                               ),
                                                                                               processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<PullPricingProductDataResponse>.Failed(
                                             Request,
                                             new PullPricingProductDataResponse(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 Array.Empty<PricingProductData>(),
                                                 Request,
                                                 StatusCode: new StatusCode(
                                                                 StatusCodes.SystemError,
                                                                 e.Message,
                                                                 e.StackTrace
                                                             )
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

                result = OICPResult<PullPricingProductDataResponse>.Failed(
                             Request,
                             new PullPricingProductDataResponse(
                                 Timestamp.Now,
                                 Request.EventTrackingId ?? EventTracking_Id.New,
                                 Process_Id.NewRandom,
                                 Timestamp.Now - Request.Timestamp,
                                 Array.Empty<PricingProductData>(),
                                 Request,
                                 StatusCode: new StatusCode(
                                                 StatusCodes.SystemError,
                                                 e.Message,
                                                 e.StackTrace
                                             )
                             )
                         );

            }

            result ??= OICPResult<PullPricingProductDataResponse>.Failed(
                           Request,
                           new PullPricingProductDataResponse(
                               Timestamp.Now,
                               Request.EventTrackingId ?? EventTracking_Id.New,
                               Process_Id.NewRandom,
                               Timestamp.Now - Request.Timestamp,
                               Array.Empty<PricingProductData>(),
                               Request,
                               StatusCode: new StatusCode(
                                               StatusCodes.SystemError,
                                               "HTTP request failed!"
                                           )
                           )
                       );

            if (result.IsNotSuccessful)
                Counters.PullPricingProductData.IncResponses_Error();


            #region Send OnPullPricingProductDataResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnPullPricingProductDataResponse is not null)
                    await Task.WhenAll(OnPullPricingProductDataResponse.GetInvocationList().
                                       Cast<OnPullPricingProductDataResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnPullPricingProductDataResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region PullEVSEPricing                (Request)

        /// <summary>
        /// Download EVSE pricing data.
        /// </summary>
        /// <param name="Request">A PullEVSEPricing request.</param>
        public async Task<OICPResult<PullEVSEPricingResponse>>

            PullEVSEPricing(PullEVSEPricingRequest Request)

        {

            #region Initial checks

            //Request = _CustomPullEVSEPricingRequestMapper(Request);

            Byte                                  TransmissionRetry   = 0;
            OICPResult<PullEVSEPricingResponse>?  result              = null;

            #endregion

            #region Send OnPullEVSEPricingRequest event

            var startTime = Timestamp.Now;

            Counters.PullEVSEPricing.IncRequests_OK();

            try
            {

                if (OnPullEVSEPricingRequest is not null)
                    await Task.WhenAll(OnPullEVSEPricingRequest.GetInvocationList().
                                       Cast<OnPullEVSEPricingRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnPullEVSEPricingRequest));
            }

            #endregion


            try
            {

                do
                {

                    #region Upstream HTTP request...

                    #region Create pagination query string

                    // ?page=0&size=20

                    var queryStrings = new List<String>();

                    if (Request.Page.HasValue)
                        queryStrings.Add("page=" + Request.Page.Value);

                    if (Request.Size.HasValue)
                        queryStrings.Add("size=" + Request.Size.Value);

                    var queryString = queryStrings.Count > 0
                                          ? "?" + queryStrings.AggregateWith("&")
                                          : "";

                    #endregion

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

                                              Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/dynamicpricing/v10/providers/" + Request.ProviderId.ToString().Replace("*", "%2A") + "/evse-pricing" + queryString),
                                                                                   requestbuilder => {
                                                                                       requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                       requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                       requestbuilder.Content      = Request.ToJSON(CustomPullEVSEPricingRequestSerializer).
                                                                                                                             ToString(JSONFormat).
                                                                                                                             ToUTF8Bytes();
                                                                                       requestbuilder.Connection   = "close";
                                                                                   }),

                                                      RequestLogDelegate:   OnPullEVSEPricingHTTPRequest,
                                                      ResponseLogDelegate:  OnPullEVSEPricingHTTPResponse,
                                                      CancellationToken:    Request.CancellationToken,
                                                      EventTrackingId:      Request.EventTrackingId,
                                                      RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                              ConfigureAwait(false);

                    #endregion


                    var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse) ?? Process_Id.NewRandom;

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                if (PullEVSEPricingResponse.TryParse(Request,
                                                                     JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                     HTTPResponse.Timestamp,
                                                                     HTTPResponse.EventTrackingId,
                                                                     HTTPResponse.Runtime,
                                                                     out PullEVSEPricingResponse?  pullEVSEDataResponse,
                                                                     out String?                   ErrorResponse,
                                                                     processId,
                                                                     HTTPResponse,
                                                                     CustomPullEVSEPricingResponseParser))
                                {

                                    Counters.PullEVSEPricing.IncResponses_OK();

                                    result = OICPResult<PullEVSEPricingResponse>.Success(Request,
                                                                                         pullEVSEDataResponse!,
                                                                                         processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<PullEVSEPricingResponse>.Failed(
                                             Request,
                                             new PullEVSEPricingResponse(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 Array.Empty<OperatorEVSEPricing>(),
                                                 Request,
                                                 StatusCode: new StatusCode(
                                                                 StatusCodes.SystemError,
                                                                 e.Message,
                                                                 e.StackTrace
                                                             )
                                             )
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

                                result = OICPResult<PullEVSEPricingResponse>.BadRequest(Request,
                                                                                        validationErrorList,
                                                                                        processId);

                            }

                        }

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                    {

                        // Until now...
                        // Hubject firewall problem!
                        // Only HTML response!

                        // Now also...

                        // HTTP/1.1 403
                        // Server:          nginx/1.18.0 (Ubuntu)
                        // Date:            Tue, 02 Aug 2022 17:53:14 GMT
                        // Content-Type:    application/json;charset=ISO-8859-1
                        // Content-Length:  96
                        // Connection:      close
                        // Process-ID:      3078ed02-54d3-4c3e-90c7-4e08731ac17a
                        // 
                        // {
                        //     "StatusCode": {
                        //         "Code":            "210",
                        //         "Description":     "No active subscription found",
                        //         "AdditionalInfo":   null
                        //     }
                        // }

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                if (StatusCode.TryParse(JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String())["StatusCode"] as JObject,
                                                        out StatusCode?  statusCode,
                                                        out String?      ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<PullEVSEPricingResponse>.Failed(Request,
                                                                                        new PullEVSEPricingResponse(
                                                                                            HTTPResponse.Timestamp,
                                                                                            HTTPResponse.EventTrackingId,
                                                                                            processId,
                                                                                            HTTPResponse.Runtime,
                                                                                            Array.Empty<OperatorEVSEPricing>(),
                                                                                            Request,
                                                                                            StatusCode:   statusCode,
                                                                                            HTTPResponse: HTTPResponse
                                                                                        ),
                                                                                        processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<PullEVSEPricingResponse>.Failed(
                                             Request,
                                             new PullEVSEPricingResponse(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 Array.Empty<OperatorEVSEPricing>(),
                                                 Request,
                                                 StatusCode: new StatusCode(
                                                                 StatusCodes.SystemError,
                                                                 e.Message,
                                                                 e.StackTrace
                                                             )
                                             )
                                         );

                            }

                        }

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
                                                        out String?      ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<PullEVSEPricingResponse>.Failed(Request,
                                                                                        new PullEVSEPricingResponse(
                                                                                            HTTPResponse.Timestamp,
                                                                                            HTTPResponse.EventTrackingId,
                                                                                            processId,
                                                                                            HTTPResponse.Runtime,
                                                                                            Array.Empty<OperatorEVSEPricing>(),
                                                                                            Request,
                                                                                            StatusCode:   statusCode,
                                                                                            HTTPResponse: HTTPResponse
                                                                                        ),
                                                                                        processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<PullEVSEPricingResponse>.Failed(
                                             Request,
                                             new PullEVSEPricingResponse(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 Array.Empty<OperatorEVSEPricing>(),
                                                 Request,
                                                 StatusCode: new StatusCode(
                                                                 StatusCodes.SystemError,
                                                                 e.Message,
                                                                 e.StackTrace
                                                             )
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

                result = OICPResult<PullEVSEPricingResponse>.Failed(
                             Request,
                             new PullEVSEPricingResponse(
                                 Timestamp.Now,
                                 Request.EventTrackingId ?? EventTracking_Id.New,
                                 Process_Id.NewRandom,
                                 Timestamp.Now - Request.Timestamp,
                                 Array.Empty<OperatorEVSEPricing>(),
                                 Request,
                                 StatusCode: new StatusCode(
                                                 StatusCodes.SystemError,
                                                 e.Message,
                                                 e.StackTrace
                                             )
                             )
                         );

            }

            result ??= OICPResult<PullEVSEPricingResponse>.Failed(
                           Request,
                           new PullEVSEPricingResponse(
                               Timestamp.Now,
                               Request.EventTrackingId ?? EventTracking_Id.New,
                               Process_Id.NewRandom,
                               Timestamp.Now - Request.Timestamp,
                               Array.Empty<OperatorEVSEPricing>(),
                               Request,
                               StatusCode: new StatusCode(
                                               StatusCodes.SystemError,
                                               "HTTP request failed!"
                                           )
                           )
                       );

            if (result.IsNotSuccessful)
                Counters.PullEVSEPricing.IncResponses_Error();


            #region Send OnPullEVSEPricingResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnPullEVSEPricingResponse is not null)
                    await Task.WhenAll(OnPullEVSEPricingResponse.GetInvocationList().
                                       Cast<OnPullEVSEPricingResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnPullEVSEPricingResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region PushAuthenticationData         (Request)

        /// <summary>
        /// Upload provider authentication data records.
        /// </summary>
        /// <param name="Request">An PushAuthenticationData request.</param>
        public async Task<OICPResult<Acknowledgement<PushAuthenticationDataRequest>>>

            PushAuthenticationData(PushAuthenticationDataRequest Request)

        {

            #region Initial checks

            //Request = _CustomPushAuthenticationDataRequestMapper(Request);

            Byte                                                         TransmissionRetry   = 0;
            OICPResult<Acknowledgement<PushAuthenticationDataRequest>>?  result              = null;

            #endregion

            #region Send OnPushAuthenticationDataRequest event

            var startTime = Timestamp.Now;

            Counters.PushAuthenticationData.IncRequests_OK();

            try
            {

                if (OnPushAuthenticationDataRequest is not null)
                    await Task.WhenAll(OnPushAuthenticationDataRequest.GetInvocationList().
                                       Cast<OnPushAuthenticationDataRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnPushAuthenticationDataRequest));
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

                                              Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/authdata/v21/providers/" + Request.ProviderId.ToString().Replace("*", "%2A") + "/push-request"),
                                                                                   requestbuilder => {
                                                                                       requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                       requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                       requestbuilder.Content      = Request.ToJSON(CustomPushAuthenticationDataRequestSerializer,
                                                                                                                                    CustomProviderAuthenticationDataSerializer,
                                                                                                                                    CustomIdentificationSerializer).
                                                                                                                             ToString(JSONFormat).
                                                                                                                             ToUTF8Bytes();
                                                                                       requestbuilder.Connection   = "close";
                                                                                   }),

                                                      RequestLogDelegate:   OnAuthorizeRemoteReservationStartHTTPRequest,
                                                      ResponseLogDelegate:  OnAuthorizeRemoteReservationStartHTTPResponse,
                                                      CancellationToken:    Request.CancellationToken,
                                                      EventTrackingId:      Request.EventTrackingId,
                                                      RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                              ConfigureAwait(false);

                    #endregion


                    var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse) ?? Process_Id.NewRandom;

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                if (Acknowledgement<PushAuthenticationDataRequest>.TryParse(Request,
                                                                                            JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                                            out Acknowledgement<PushAuthenticationDataRequest>?  pushAuthenticationDataResponse,
                                                                                            out String?                                          ErrorResponse,
                                                                                            HTTPResponse,
                                                                                            HTTPResponse.Timestamp,
                                                                                            HTTPResponse.EventTrackingId,
                                                                                            HTTPResponse.Runtime,
                                                                                            processId,
                                                                                            CustomPushAuthenticationDataAcknowledgementParser))
                                {

                                    Counters.PushAuthenticationData.IncResponses_OK();

                                    result = OICPResult<Acknowledgement<PushAuthenticationDataRequest>>.Success(Request,
                                                                                                                pushAuthenticationDataResponse!,
                                                                                                                processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<PushAuthenticationDataRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<PushAuthenticationDataRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 ),
                                                 Request,
                                                 HTTPResponse,
                                                 false,
                                                 null, //Request.SessionId,
                                                 null, //Request.CPOPartnerSessionId,
                                                 null, //Request.EMPPartnerSessionId,
                                                 Request.CustomData
                                             )
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

                                result = OICPResult<Acknowledgement<PushAuthenticationDataRequest>>.BadRequest(Request,
                                                                                                               validationErrorList,
                                                                                                               processId);

                            }

                        }

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                    {

                        // You must create an offer for this service wihtin the Hubject portal
                        // and Hubject must be subscribed to it!

                        // HTTP/1.1 403
                        // Server:          nginx/1.18.0 (Ubuntu)
                        // Date:            Tue, 02 Aug 2022 21:23:37 GMT
                        // Content-Type:    application/json;charset=ISO-8859-1
                        // Content-Length:  96
                        // Connection:      close
                        // Process-ID:      c6c78551-00d0-4c77-a48c-1ba2d1947b07
                        // 
                        // {
                        //     "StatusCode": {
                        //         "Code":           "210",
                        //         "Description":    "No active subscription found",
                        //         "AdditionalInfo":  null
                        //     }
                        // }

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                if (StatusCode.TryParse(JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String())["StatusCode"] as JObject,
                                                        out StatusCode?  statusCode,
                                                        out String?      ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<Acknowledgement<PushAuthenticationDataRequest>>.Failed(Request,
                                                                                                               new Acknowledgement<PushAuthenticationDataRequest>(
                                                                                                                   HTTPResponse.Timestamp,
                                                                                                                   HTTPResponse.EventTrackingId,
                                                                                                                   processId,
                                                                                                                   HTTPResponse.Runtime,
                                                                                                                   statusCode!,
                                                                                                                   Request,
                                                                                                                   HTTPResponse,
                                                                                                                   false,
                                                                                                                   null, //Request.SessionId,
                                                                                                                   null, //Request.CPOPartnerSessionId,
                                                                                                                   null, //Request.EMPPartnerSessionId,
                                                                                                                   Request.CustomData
                                                                                                               ),
                                                                                                               processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<PushAuthenticationDataRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<PushAuthenticationDataRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 ),
                                                 Request,
                                                 HTTPResponse,
                                                 false,
                                                 null, //Request.SessionId,
                                                 null, //Request.CPOPartnerSessionId,
                                                 null, //Request.EMPPartnerSessionId,
                                                 Request.CustomData
                                             )
                                         );

                            }

                        }

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
                                                        out String?      ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<Acknowledgement<PushAuthenticationDataRequest>>.Failed(Request,
                                                                                                               new Acknowledgement<PushAuthenticationDataRequest>(
                                                                                                                   HTTPResponse.Timestamp,
                                                                                                                   HTTPResponse.EventTrackingId,
                                                                                                                   processId,
                                                                                                                   HTTPResponse.Runtime,
                                                                                                                   statusCode!,
                                                                                                                   Request,
                                                                                                                   HTTPResponse,
                                                                                                                   false,
                                                                                                                   null, //Request.SessionId,
                                                                                                                   null, //Request.CPOPartnerSessionId,
                                                                                                                   null, //Request.EMPPartnerSessionId,
                                                                                                                   Request.CustomData
                                                                                                               ),
                                                                                                               processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<PushAuthenticationDataRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<PushAuthenticationDataRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 ),
                                                 Request,
                                                 HTTPResponse,
                                                 false,
                                                 null, //Request.SessionId,
                                                 null, //Request.CPOPartnerSessionId,
                                                 null, //Request.EMPPartnerSessionId,
                                                 Request.CustomData
                                             )
                                         );

                            }

                        }

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                    {

                        // e.g. DE*GDF instead of DE-GDF, because they interpret ids as strings
                        //      not as complex data type having optional elements!

                        // HTTP/1.1 404
                        // Server:          nginx/1.18.0 (Ubuntu)
                        // Date:            Tue, 02 Aug 2022 21:04:53 GMT
                        // Content-Type:    application/json;charset=ISO-8859-1
                        // Content-Length:  85
                        // Connection:      close
                        // Process-ID:      d52f2fc8-382b-4f74-9104-faf1902d8788
                        // 
                        // {
                        //     "StatusCode": {
                        //         "Code":           "300",
                        //         "Description":    "Partner not found",
                        //         "AdditionalInfo":  null
                        //     }
                        // }

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                if (StatusCode.TryParse(JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String())["StatusCode"] as JObject,
                                                        out StatusCode?  statusCode,
                                                        out String?      ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<Acknowledgement<PushAuthenticationDataRequest>>.Failed(Request,
                                                                                                               new Acknowledgement<PushAuthenticationDataRequest>(
                                                                                                                   HTTPResponse.Timestamp,
                                                                                                                   HTTPResponse.EventTrackingId,
                                                                                                                   processId,
                                                                                                                   HTTPResponse.Runtime,
                                                                                                                   statusCode!,
                                                                                                                   Request,
                                                                                                                   HTTPResponse,
                                                                                                                   false,
                                                                                                                   null, //Request.SessionId,
                                                                                                                   null, //Request.CPOPartnerSessionId,
                                                                                                                   null, //Request.EMPPartnerSessionId,
                                                                                                                   Request.CustomData
                                                                                                               ),
                                                                                                               processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<PushAuthenticationDataRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<PushAuthenticationDataRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 ),
                                                 Request,
                                                 HTTPResponse,
                                                 false,
                                                 null, //Request.SessionId,
                                                 null, //Request.CPOPartnerSessionId,
                                                 null, //Request.EMPPartnerSessionId,
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

                result = OICPResult<Acknowledgement<PushAuthenticationDataRequest>>.Failed(
                             Request,
                             new Acknowledgement<PushAuthenticationDataRequest>(
                                 Timestamp.Now,
                                 Request.EventTrackingId ?? EventTracking_Id.New,
                                 Process_Id.NewRandom,
                                 Timestamp.Now - Request.Timestamp,
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     e.Message,
                                     e.StackTrace
                                 ),
                                 Request,
                                 null,
                                 false,
                                 null, //Request.SessionId,
                                 null, //Request.CPOPartnerSessionId,
                                 null, //Request.EMPPartnerSessionId,
                                 Request.CustomData
                             )
                         );

            }

            result ??= OICPResult<Acknowledgement<PushAuthenticationDataRequest>>.Failed(
                           Request,
                           new Acknowledgement<PushAuthenticationDataRequest>(
                               Timestamp.Now,
                               Request.EventTrackingId ?? EventTracking_Id.New,
                               Process_Id.NewRandom,
                               Timestamp.Now - Request.Timestamp,
                               new StatusCode(
                                   StatusCodes.SystemError,
                                   "HTTP request failed!"
                               ),
                               Request,
                               null,
                               false,
                               null, //Request.SessionId,
                               null, //Request.CPOPartnerSessionId,
                               null, //Request.EMPPartnerSessionId,
                               Request.CustomData
                           )
                       );

            if (result.IsNotSuccessful)
                Counters.PushAuthenticationData.IncResponses_Error();


            #region Send OnPushAuthenticationDataResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnPushAuthenticationDataResponse is not null)
                    await Task.WhenAll(OnPushAuthenticationDataResponse.GetInvocationList().
                                       Cast<OnPushAuthenticationDataResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnPushAuthenticationDataResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region AuthorizeRemoteReservationStart(Request)

        /// <summary>
        /// Create a charging reservation at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteReservationStart request.</param>
        public async Task<OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>>

            AuthorizeRemoteReservationStart(AuthorizeRemoteReservationStartRequest Request)

        {

            #region Initial checks

            //Request = _CustomAuthorizeRemoteReservationStartRequestMapper(Request);

            Byte                                                                  TransmissionRetry   = 0;
            OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>?  result              = null;

            #endregion

            #region Send OnAuthorizeRemoteReservationStartRequest event

            var startTime = Timestamp.Now;

            Counters.AuthorizeRemoteReservationStart.IncRequests_OK();

            try
            {

                if (OnAuthorizeRemoteReservationStartRequest is not null)
                    await Task.WhenAll(OnAuthorizeRemoteReservationStartRequest.GetInvocationList().
                                       Cast<OnAuthorizeRemoteReservationStartRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnAuthorizeRemoteReservationStartRequest));
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

                                              Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/charging/v21/providers/" + Request.ProviderId.ToString().Replace("*", "%2A") + "/authorize-remote-reservation/start"),
                                                                                   requestbuilder => {
                                                                                       requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                       requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                       requestbuilder.Content      = Request.ToJSON(CustomAuthorizeRemoteReservationStartRequestSerializer,
                                                                                                                                    CustomIdentificationSerializer).
                                                                                                                             ToString(JSONFormat).
                                                                                                                             ToUTF8Bytes();
                                                                                       requestbuilder.Connection   = "close";
                                                                                   }),

                                                      RequestLogDelegate:   OnAuthorizeRemoteReservationStartHTTPRequest,
                                                      ResponseLogDelegate:  OnAuthorizeRemoteReservationStartHTTPResponse,
                                                      CancellationToken:    Request.CancellationToken,
                                                      EventTrackingId:      Request.EventTrackingId,
                                                      RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                              ConfigureAwait(false);

                    #endregion


                    var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse) ?? Process_Id.NewRandom;

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
                                             new Acknowledgement<AuthorizeRemoteReservationStartRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 ),
                                                 Request,
                                                 HTTPResponse,
                                                 false,
                                                 Request.SessionId,
                                                 Request.CPOPartnerSessionId,
                                                 Request.EMPPartnerSessionId,
                                                 Request.CustomData
                                             )
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
                                                        out String?      ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>.Failed(Request,
                                                                                                                        new Acknowledgement<AuthorizeRemoteReservationStartRequest>(
                                                                                                                            HTTPResponse.Timestamp,
                                                                                                                            HTTPResponse.EventTrackingId,
                                                                                                                            processId,
                                                                                                                            HTTPResponse.Runtime,
                                                                                                                            statusCode,
                                                                                                                            Request,
                                                                                                                            HTTPResponse,
                                                                                                                            false,
                                                                                                                            Request.SessionId,
                                                                                                                            Request.CPOPartnerSessionId,
                                                                                                                            Request.EMPPartnerSessionId,
                                                                                                                            Request.CustomData
                                                                                                                        ),
                                                                                                                        processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<AuthorizeRemoteReservationStartRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 ),
                                                 Request,
                                                 HTTPResponse,
                                                 false,
                                                 Request.SessionId,
                                                 Request.CPOPartnerSessionId,
                                                 Request.EMPPartnerSessionId,
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
                             new Acknowledgement<AuthorizeRemoteReservationStartRequest>(
                                 Timestamp.Now,
                                 Request.EventTrackingId ?? EventTracking_Id.New,
                                 Process_Id.NewRandom,
                                 Timestamp.Now - Request.Timestamp,
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     e.Message,
                                     e.StackTrace
                                 ),
                                 Request,
                                 null,
                                 false,
                                 Request.SessionId,
                                 Request.CPOPartnerSessionId,
                                 Request.EMPPartnerSessionId,
                                 Request.CustomData
                             )
                         );

            }

            result ??= OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>.Failed(
                           Request,
                           new Acknowledgement<AuthorizeRemoteReservationStartRequest>(
                               Timestamp.Now,
                               Request.EventTrackingId ?? EventTracking_Id.New,
                               Process_Id.NewRandom,
                               Timestamp.Now - Request.Timestamp,
                               new StatusCode(
                                   StatusCodes.SystemError,
                                   "HTTP request failed!"
                               ),
                               Request,
                               null,
                               false,
                               Request.SessionId,
                               Request.CPOPartnerSessionId,
                               Request.EMPPartnerSessionId,
                               Request.CustomData
                           )
                       );

            if (result.IsNotSuccessful)
                Counters.AuthorizeRemoteReservationStart.IncResponses_Error();


            #region Send OnAuthorizeRemoteReservationStartResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnAuthorizeRemoteReservationStartResponse is not null)
                    await Task.WhenAll(OnAuthorizeRemoteReservationStartResponse.GetInvocationList().
                                       Cast<OnAuthorizeRemoteReservationStartResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnAuthorizeRemoteReservationStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeRemoteReservationStop (Request)

        /// <summary>
        /// Stop the given charging reservation.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteReservationStop request.</param>
        public async Task<OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>>

            AuthorizeRemoteReservationStop(AuthorizeRemoteReservationStopRequest Request)

        {

            #region Initial checks

            //Request = _CustomAuthorizeRemoteReservationStopRequestMapper(Request);

            Byte                                                                 TransmissionRetry   = 0;
            OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>?  result              = null;

            #endregion

            #region Send OnAuthorizeRemoteReservationStopRequest event

            var startTime = Timestamp.Now;

            Counters.AuthorizeRemoteReservationStop.IncRequests_OK();

            try
            {

                if (OnAuthorizeRemoteReservationStopRequest is not null)
                    await Task.WhenAll(OnAuthorizeRemoteReservationStopRequest.GetInvocationList().
                                       Cast<OnAuthorizeRemoteReservationStopRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnAuthorizeRemoteReservationStopRequest));
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

                                              Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/charging/v21/providers/" + Request.ProviderId.ToString().Replace("*", "%2A") + "/authorize-remote-reservation/stop"),
                                                                                   requestbuilder => {
                                                                                       requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                       requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                       requestbuilder.Content      = Request.ToJSON(CustomAuthorizeRemoteReservationStopRequestSerializer).
                                                                                                                             ToString(JSONFormat).
                                                                                                                             ToUTF8Bytes();
                                                                                       requestbuilder.Connection   = "close";
                                                                                   }),

                                                      RequestLogDelegate:   OnAuthorizeRemoteReservationStopHTTPRequest,
                                                      ResponseLogDelegate:  OnAuthorizeRemoteReservationStopHTTPResponse,
                                                      CancellationToken:    Request.CancellationToken,
                                                      EventTrackingId:      Request.EventTrackingId,
                                                      RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                              ConfigureAwait(false);

                    #endregion


                    var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse) ?? Process_Id.NewRandom;

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
                                             new Acknowledgement<AuthorizeRemoteReservationStopRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 ),
                                                 Request,
                                                 HTTPResponse,
                                                 false,
                                                 Request.SessionId,
                                                 Request.CPOPartnerSessionId,
                                                 Request.EMPPartnerSessionId,
                                                 Request.CustomData
                                             )
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
                                                        out String?      ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>.Failed(Request,
                                                                                                                       new Acknowledgement<AuthorizeRemoteReservationStopRequest>(
                                                                                                                           HTTPResponse.Timestamp,
                                                                                                                           HTTPResponse.EventTrackingId,
                                                                                                                           processId,
                                                                                                                           HTTPResponse.Runtime,
                                                                                                                           statusCode,
                                                                                                                           Request,
                                                                                                                           HTTPResponse,
                                                                                                                           false,
                                                                                                                           Request.SessionId,
                                                                                                                           Request.CPOPartnerSessionId,
                                                                                                                           Request.EMPPartnerSessionId,
                                                                                                                           Request.CustomData
                                                                                                                       ),
                                                                                                                       processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<AuthorizeRemoteReservationStopRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 ),
                                                 Request,
                                                 HTTPResponse,
                                                 false,
                                                 Request.SessionId,
                                                 Request.CPOPartnerSessionId,
                                                 Request.EMPPartnerSessionId,
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
                             new Acknowledgement<AuthorizeRemoteReservationStopRequest>(
                                 Timestamp.Now,
                                 Request.EventTrackingId ?? EventTracking_Id.New,
                                 Process_Id.NewRandom,
                                 Timestamp.Now - Request.Timestamp,
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     e.Message,
                                     e.StackTrace
                                 ),
                                 Request,
                                 null,
                                 false,
                                 Request.SessionId,
                                 Request.CPOPartnerSessionId,
                                 Request.EMPPartnerSessionId,
                                 Request.CustomData
                             )
                         );

            }

            result ??= OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>.Failed(
                           Request,
                           new Acknowledgement<AuthorizeRemoteReservationStopRequest>(
                               Timestamp.Now,
                               Request.EventTrackingId ?? EventTracking_Id.New,
                               Process_Id.NewRandom,
                               Timestamp.Now - Request.Timestamp,
                               new StatusCode(
                                   StatusCodes.SystemError,
                                   "HTTP request failed!"
                               ),
                               Request,
                               null,
                               false,
                               Request.SessionId,
                               Request.CPOPartnerSessionId,
                               Request.EMPPartnerSessionId,
                               Request.CustomData
                           )
                       );

            if (result.IsNotSuccessful)
                Counters.AuthorizeRemoteReservationStop.IncResponses_Error();


            #region Send OnAuthorizeRemoteReservationStopResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnAuthorizeRemoteReservationStopResponse is not null)
                    await Task.WhenAll(OnAuthorizeRemoteReservationStopResponse.GetInvocationList().
                                       Cast<OnAuthorizeRemoteReservationStopResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnAuthorizeRemoteReservationStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeRemoteStart           (Request)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteStart request.</param>
        public async Task<OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>>

            AuthorizeRemoteStart(AuthorizeRemoteStartRequest Request)

        {

            #region Initial checks

            //Request = _CustomAuthorizeRemoteStartRequestMapper(Request);

            Byte                                                       TransmissionRetry   = 0;
            OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>?  result              = null;

            #endregion

            #region Send OnAuthorizeRemoteStartRequest event

            var startTime = Timestamp.Now;

            Counters.AuthorizeRemoteStart.IncRequests_OK();

            try
            {

                if (OnAuthorizeRemoteStartRequest is not null)
                    await Task.WhenAll(OnAuthorizeRemoteStartRequest.GetInvocationList().
                                       Cast<OnAuthorizeRemoteStartRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnAuthorizeRemoteStartRequest));
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

                                              Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/charging/v21/providers/" + Request.ProviderId.ToString().Replace("*", "%2A") + "/authorize-remote/start"),
                                                                                   requestbuilder => {
                                                                                       requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                       requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                       requestbuilder.Content      = Request.ToJSON(CustomAuthorizeRemoteStartRequestSerializer).
                                                                                                                             ToString(JSONFormat).
                                                                                                                             ToUTF8Bytes();
                                                                                       requestbuilder.Connection   = "close";
                                                                                   }),

                                                      RequestLogDelegate:   OnAuthorizeRemoteStartHTTPRequest,
                                                      ResponseLogDelegate:  OnAuthorizeRemoteStartHTTPResponse,
                                                      CancellationToken:    Request.CancellationToken,
                                                      EventTrackingId:      Request.EventTrackingId,
                                                      RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                              ConfigureAwait(false);

                    #endregion


                    var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse) ?? Process_Id.NewRandom;

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
                                             new Acknowledgement<AuthorizeRemoteStartRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 ),
                                                 Request,
                                                 HTTPResponse,
                                                 false,
                                                 Request.SessionId,
                                                 Request.CPOPartnerSessionId,
                                                 Request.EMPPartnerSessionId,
                                                 Request.CustomData
                                             )
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
                                                        out String?      ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>.Failed(Request,
                                                                                                             new Acknowledgement<AuthorizeRemoteStartRequest>(
                                                                                                                 HTTPResponse.Timestamp,
                                                                                                                 HTTPResponse.EventTrackingId,
                                                                                                                 processId,
                                                                                                                 HTTPResponse.Runtime,
                                                                                                                 statusCode,
                                                                                                                 Request,
                                                                                                                 HTTPResponse,
                                                                                                                 false,
                                                                                                                 Request.SessionId,
                                                                                                                 Request.CPOPartnerSessionId,
                                                                                                                 Request.EMPPartnerSessionId,
                                                                                                                 Request.CustomData
                                                                                                             ),
                                                                                                             processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<AuthorizeRemoteStartRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 ),
                                                 Request,
                                                 HTTPResponse,
                                                 false,
                                                 Request.SessionId,
                                                 Request.CPOPartnerSessionId,
                                                 Request.EMPPartnerSessionId,
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
                             new Acknowledgement<AuthorizeRemoteStartRequest>(
                                 Timestamp.Now,
                                 Request.EventTrackingId ?? EventTracking_Id.New,
                                 Process_Id.NewRandom,
                                 Timestamp.Now - Request.Timestamp,
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     e.Message,
                                     e.StackTrace
                                 ),
                                 Request,
                                 null,
                                 false,
                                 Request.SessionId,
                                 Request.CPOPartnerSessionId,
                                 Request.EMPPartnerSessionId,
                                 Request.CustomData
                             )
                         );

            }

            result ??= OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>.Failed(
                           Request,
                           new Acknowledgement<AuthorizeRemoteStartRequest>(
                               Timestamp.Now,
                               Request.EventTrackingId ?? EventTracking_Id.New,
                               Process_Id.NewRandom,
                               Timestamp.Now - Request.Timestamp,
                               new StatusCode(
                                   StatusCodes.SystemError,
                                   "HTTP request failed!"
                               ),
                               Request,
                               null,
                               false,
                               Request.SessionId,
                               Request.CPOPartnerSessionId,
                               Request.EMPPartnerSessionId,
                               Request.CustomData
                           )
                       );

            if (result.IsNotSuccessful)
                Counters.AuthorizeRemoteStart.IncResponses_Error();


            #region Send OnAuthorizeRemoteStartResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnAuthorizeRemoteStartResponse is not null)
                    await Task.WhenAll(OnAuthorizeRemoteStartResponse.GetInvocationList().
                                       Cast<OnAuthorizeRemoteStartResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnAuthorizeRemoteStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeRemoteStop            (Request)

        /// <summary>
        /// Stop the given charging session.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteStop request.</param>
        public async Task<OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>>

            AuthorizeRemoteStop(AuthorizeRemoteStopRequest Request)

        {

            #region Initial checks

            //Request = _CustomAuthorizeRemoteStopRequestMapper(Request);

            Byte                                                      TransmissionRetry   = 0;
            OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>?  result              = null;

            #endregion

            #region Send OnAuthorizeRemoteStopRequest event

            var startTime = Timestamp.Now;

            Counters.AuthorizeRemoteStop.IncRequests_OK();

            try
            {

                if (OnAuthorizeRemoteStopRequest is not null)
                    await Task.WhenAll(OnAuthorizeRemoteStopRequest.GetInvocationList().
                                       Cast<OnAuthorizeRemoteStopRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnAuthorizeRemoteStopRequest));
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

                                              Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/charging/v21/providers/" + Request.ProviderId.ToString().Replace("*", "%2A") + "/authorize-remote/stop"),
                                                                                   requestbuilder => {
                                                                                       requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                       requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                       requestbuilder.Content      = Request.ToJSON(CustomAuthorizeRemoteStopRequestSerializer).
                                                                                                                             ToString(JSONFormat).
                                                                                                                             ToUTF8Bytes();
                                                                                       requestbuilder.Connection   = "close";
                                                                                   }),

                                                      RequestLogDelegate:   OnAuthorizeRemoteStopHTTPRequest,
                                                      ResponseLogDelegate:  OnAuthorizeRemoteStopHTTPResponse,
                                                      CancellationToken:    Request.CancellationToken,
                                                      EventTrackingId:      Request.EventTrackingId,
                                                      RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                              ConfigureAwait(false);

                    #endregion


                    var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse) ?? Process_Id.NewRandom;

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
                                             new Acknowledgement<AuthorizeRemoteStopRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 ),
                                                 Request,
                                                 HTTPResponse,
                                                 false,
                                                 Request.SessionId,
                                                 Request.CPOPartnerSessionId,
                                                 Request.EMPPartnerSessionId,
                                                 Request.CustomData
                                             )
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
                                                        out String?      ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>.Failed(Request,
                                                                                                            new Acknowledgement<AuthorizeRemoteStopRequest>(
                                                                                                                HTTPResponse.Timestamp,
                                                                                                                HTTPResponse.EventTrackingId,
                                                                                                                processId,
                                                                                                                HTTPResponse.Runtime,
                                                                                                                statusCode,
                                                                                                                Request,
                                                                                                                HTTPResponse,
                                                                                                                false,
                                                                                                                Request.SessionId,
                                                                                                                Request.CPOPartnerSessionId,
                                                                                                                Request.EMPPartnerSessionId,
                                                                                                                Request.CustomData
                                                                                                            ),
                                                                                                            processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<AuthorizeRemoteStopRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 ),
                                                 Request,
                                                 HTTPResponse,
                                                 false,
                                                 Request.SessionId,
                                                 Request.CPOPartnerSessionId,
                                                 Request.EMPPartnerSessionId,
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
                             new Acknowledgement<AuthorizeRemoteStopRequest>(
                                 Timestamp.Now,
                                 Request.EventTrackingId ?? EventTracking_Id.New,
                                 Process_Id.NewRandom,
                                 Timestamp.Now - Request.Timestamp,
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     e.Message,
                                     e.StackTrace
                                 ),
                                 Request,
                                 null,
                                 false,
                                 Request.SessionId,
                                 Request.CPOPartnerSessionId,
                                 Request.EMPPartnerSessionId,
                                 Request.CustomData
                             )
                         );

            }

            result ??= OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>.Failed(
                           Request,
                           new Acknowledgement<AuthorizeRemoteStopRequest>(
                               Timestamp.Now,
                               Request.EventTrackingId ?? EventTracking_Id.New,
                               Process_Id.NewRandom,
                               Timestamp.Now - Request.Timestamp,
                               new StatusCode(
                                   StatusCodes.SystemError,
                                   "HTTP request failed!"
                               ),
                               Request,
                               null,
                               false,
                               Request.SessionId,
                               Request.CPOPartnerSessionId,
                               Request.EMPPartnerSessionId,
                               Request.CustomData
                           )
                       );

            if (result.IsNotSuccessful)
                Counters.AuthorizeRemoteStop.IncResponses_Error();


            #region Send OnAuthorizeRemoteStopResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnAuthorizeRemoteStopResponse is not null)
                    await Task.WhenAll(OnAuthorizeRemoteStopResponse.GetInvocationList().
                                       Cast<OnAuthorizeRemoteStopResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnAuthorizeRemoteStopResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region GetChargeDetailRecords         (Request)

        /// <summary>
        /// Download charge detail records.
        /// </summary>
        /// <param name="Request">A GetChargeDetailRecords request.</param>
        public async Task<OICPResult<GetChargeDetailRecordsResponse>>

            GetChargeDetailRecords(GetChargeDetailRecordsRequest Request)

        {

            #region Initial checks

            //Request = _CustomGetChargeDetailRecordsRequestMapper(Request);

            Byte                                         TransmissionRetry   = 0;
            OICPResult<GetChargeDetailRecordsResponse>?  result              = null;

            #endregion

            #region Send OnGetChargeDetailRecordsRequest event

            var startTime = Timestamp.Now;

            Counters.GetChargeDetailRecords.IncRequests_OK();

            try
            {

                if (OnGetChargeDetailRecordsRequest is not null)
                    await Task.WhenAll(OnGetChargeDetailRecordsRequest.GetInvocationList().
                                       Cast<OnGetChargeDetailRecordsRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnGetChargeDetailRecordsRequest));
            }

            #endregion


            try
            {

                do
                {

                    #region Upstream HTTP request...

                    #region Create pagination query string

                    // ?page=0&size=20

                    var queryStrings = new List<String>();

                    if (Request.Page.HasValue)
                        queryStrings.Add("page=" + Request.Page.Value);

                    if (Request.Size.HasValue)
                        queryStrings.Add("size=" + Request.Size.Value);

                    var queryString = queryStrings.Count > 0
                                          ? "?" + queryStrings.AggregateWith("&")
                                          : "";

                    #endregion

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

                                              Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/cdrmgmt/v22/providers/" + Request.ProviderId.ToString().Replace("*", "%2A") + "/get-charge-detail-records-request" + queryString),
                                                                                   requestbuilder => {
                                                                                       requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                       requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                       requestbuilder.Content      = Request.ToJSON(CustomGetChargeDetailRecordsRequestSerializer).
                                                                                                                             ToString(JSONFormat).
                                                                                                                             ToUTF8Bytes();
                                                                                       requestbuilder.Connection   = "close";
                                                                                   }),

                                                      RequestLogDelegate:   OnGetChargeDetailRecordsHTTPRequest,
                                                      ResponseLogDelegate:  OnGetChargeDetailRecordsHTTPResponse,
                                                      CancellationToken:    Request.CancellationToken,
                                                      EventTrackingId:      Request.EventTrackingId,
                                                      RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                              ConfigureAwait(false);

                    #endregion


                    var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse) ?? Process_Id.NewRandom;

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                if (GetChargeDetailRecordsResponse.TryParse(Request,
                                                                            JObject.Parse(HTTPResponse.HTTPBody?.ToUTF8String()),
                                                                            HTTPResponse.Timestamp,
                                                                            HTTPResponse.EventTrackingId,
                                                                            HTTPResponse.Runtime,
                                                                            out GetChargeDetailRecordsResponse?  getChargeDetailRecordsResponse,
                                                                            out String?                          ErrorResponse,
                                                                            HTTPResponse,
                                                                            processId,
                                                                            CustomGetChargeDetailRecordsResponseParser))
                                {

                                    Counters.GetChargeDetailRecords.IncResponses_OK();

                                    result = OICPResult<GetChargeDetailRecordsResponse>.Success(Request,
                                                                                                getChargeDetailRecordsResponse!,
                                                                                                processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<GetChargeDetailRecordsResponse>.Failed(
                                             Request,
                                             new GetChargeDetailRecordsResponse(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 Array.Empty<ChargeDetailRecord>(),
                                                 Request,
                                                 StatusCode: new StatusCode(
                                                                 StatusCodes.SystemError,
                                                                 e.Message,
                                                                 e.StackTrace
                                                             )
                                             )
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

                                result = OICPResult<GetChargeDetailRecordsResponse>.BadRequest(Request,
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
                                                        out String?      ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<GetChargeDetailRecordsResponse>.Failed(Request,
                                                                                               new GetChargeDetailRecordsResponse(
                                                                                                   HTTPResponse.Timestamp,
                                                                                                   HTTPResponse.EventTrackingId,
                                                                                                   processId,
                                                                                                   HTTPResponse.Runtime,
                                                                                                   Array.Empty<ChargeDetailRecord>(),
                                                                                                   Request,
                                                                                                   StatusCode: statusCode
                                                                                               ),
                                                                                               processId);

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<GetChargeDetailRecordsResponse>.Failed(
                                             Request,
                                             new GetChargeDetailRecordsResponse(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 Array.Empty<ChargeDetailRecord>(),
                                                 Request,
                                                 StatusCode: new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 )
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

                result = OICPResult<GetChargeDetailRecordsResponse>.Failed(
                             Request,
                             new GetChargeDetailRecordsResponse(
                                 Timestamp.Now,
                                 Request.EventTrackingId ?? EventTracking_Id.New,
                                 Process_Id.NewRandom,
                                 Timestamp.Now - Request.Timestamp,
                                 Array.Empty<ChargeDetailRecord>(),
                                 Request,
                                 StatusCode: new StatusCode(
                                                 StatusCodes.SystemError,
                                                 e.Message,
                                                 e.StackTrace
                                             )
                             )
                         );

            }

            result ??= OICPResult<GetChargeDetailRecordsResponse>.Failed(
                           Request,
                           new GetChargeDetailRecordsResponse(
                               Timestamp.Now,
                               Request.EventTrackingId ?? EventTracking_Id.New,
                               Process_Id.NewRandom,
                               Timestamp.Now - Request.Timestamp,
                               Array.Empty<ChargeDetailRecord>(),
                               Request,
                               StatusCode: new StatusCode(
                                               StatusCodes.SystemError,
                                               "HTTP request failed!"
                                           )
                           )
                       );

            if (result.IsNotSuccessful)
                Counters.GetChargeDetailRecords.IncResponses_Error();


            #region Send OnGetChargeDetailRecordsResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnGetChargeDetailRecordsResponse is not null)
                    await Task.WhenAll(OnGetChargeDetailRecordsResponse.GetInvocationList().
                                       Cast<OnGetChargeDetailRecordsResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnGetChargeDetailRecordsResponse));
            }

            #endregion

            return result;

        }

        #endregion


    }

}
