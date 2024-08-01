/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The CPO client.
    /// </summary>
    public partial class CPOClient : AHTTPClient,
                                     ICPOClient
    {

        #region (class) APICounters

        public class APICounters(APICounterValues?  PushEVSEData                       = null,
                                 APICounterValues?  PushEVSEStatus                     = null,

                                 APICounterValues?  PushPricingProductData             = null,
                                 APICounterValues?  PushEVSEPricing                    = null,

                                 APICounterValues?  PullAuthenticationData             = null,

                                 APICounterValues?  AuthorizeStart                     = null,
                                 APICounterValues?  AuthorizeStop                      = null,

                                 APICounterValues?  SendChargingStartNotification      = null,
                                 APICounterValues?  SendChargingProgressNotification   = null,
                                 APICounterValues?  SendChargingEndNotification        = null,
                                 APICounterValues?  SendChargingErrorNotification      = null,

                                 APICounterValues?  SendChargeDetailRecord             = null)
        {

            public APICounterValues PushEVSEData                        { get; } = PushEVSEData                     ?? new APICounterValues();
            public APICounterValues PushEVSEStatus                      { get; } = PushEVSEStatus                   ?? new APICounterValues();

            public APICounterValues PushPricingProductData              { get; } = PushPricingProductData           ?? new APICounterValues();
            public APICounterValues PushEVSEPricing                     { get; } = PushEVSEPricing                  ?? new APICounterValues();


            public APICounterValues PullAuthenticationData              { get; } = PullAuthenticationData           ?? new APICounterValues();

            public APICounterValues AuthorizeStart                      { get; } = AuthorizeStart                   ?? new APICounterValues();
            public APICounterValues AuthorizeStop                       { get; } = AuthorizeStop                    ?? new APICounterValues();

            public APICounterValues SendChargingStartNotification       { get; } = SendChargingStartNotification    ?? new APICounterValues();
            public APICounterValues SendChargingProgressNotification    { get; } = SendChargingProgressNotification ?? new APICounterValues();
            public APICounterValues SendChargingEndNotification         { get; } = SendChargingEndNotification      ?? new APICounterValues();
            public APICounterValues SendChargingErrorNotification       { get; } = SendChargingErrorNotification    ?? new APICounterValues();

            public APICounterValues SendChargeDetailRecord              { get; } = SendChargeDetailRecord           ?? new APICounterValues();

            public JObject ToJSON()

                => JSONObject.Create(
                       new JProperty("PushEVSEData",                 PushEVSEData.                    ToJSON()),
                       new JProperty("PushEVSEStatus",               PushEVSEStatus.                  ToJSON()),

                       new JProperty("PushPricingProductData",       PushPricingProductData.          ToJSON()),
                       new JProperty("PushEVSEPricing",              PushEVSEPricing.                 ToJSON()),

                       new JProperty("PullAuthenticationData",       PullAuthenticationData.          ToJSON()),

                       new JProperty("AuthorizeStart",               AuthorizeStart.                  ToJSON()),
                       new JProperty("AuthorizeStop",                AuthorizeStop.                   ToJSON()),

                       new JProperty("ChargingStartNotification",    SendChargingStartNotification.   ToJSON()),
                       new JProperty("ChargingProgressNotification", SendChargingProgressNotification.ToJSON()),
                       new JProperty("ChargingEndNotification",      SendChargingEndNotification.     ToJSON()),
                       new JProperty("ChargingErrorNotification",    SendChargingErrorNotification.   ToJSON()),

                       new JProperty("SendChargeDetailRecord",       SendChargeDetailRecord.          ToJSON())
                   );

        }

        #endregion


        #region Data

        /// <summary>
        /// The default HTTP user agent.
        /// </summary>
        public new const        String    DefaultHTTPUserAgent        = $"GraphDefined OICP {Version.String} CPO Client";

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
        public CPOClientLogger?            Logger            { get; }

        public APICounters                 Counters          { get; }

        public Newtonsoft.Json.Formatting  JSONFormatting    { get; set; }

        #endregion

        #region Custom JSON parsers

        public CustomJObjectParserDelegate<Acknowledgement<PushEVSEDataRequest>>?                  CustomPushEVSEDataAcknowledgementParser                      { get; set; }
        public CustomJObjectParserDelegate<Acknowledgement<PushEVSEStatusRequest>>?                CustomPushEVSEStatusAcknowledgementParser                    { get; set; }

        public CustomJObjectParserDelegate<Acknowledgement<PushPricingProductDataRequest>>?        CustomPushPricingProductDataAcknowledgementParser            { get; set; }
        public CustomJObjectParserDelegate<Acknowledgement<PushEVSEPricingRequest>>?               CustomPushEVSEPricingAcknowledgementParser                   { get; set; }

        public CustomJObjectParserDelegate<PullAuthenticationDataResponse>?                        CustomPullAuthenticationDataResponseParser                   { get; set; }

        public CustomJObjectParserDelegate<AuthorizationStartResponse>?                            CustomAuthorizationStartResponseParser                       { get; set; }
        public CustomJObjectParserDelegate<AuthorizationStopResponse>?                             CustomAuthorizationStopResponseParser                        { get; set; }


        public CustomJObjectParserDelegate<Acknowledgement<ChargingStartNotificationRequest>>?     CustomChargingStartNotificationAcknowledgementParser         { get; set; }

        public CustomJObjectParserDelegate<Acknowledgement<ChargingProgressNotificationRequest>>?  CustomChargingProgressNotificationAcknowledgementParser      { get; set; }

        public CustomJObjectParserDelegate<Acknowledgement<ChargingEndNotificationRequest>>?       CustomChargingEndNotificationAcknowledgementParser           { get; set; }

        public CustomJObjectParserDelegate<Acknowledgement<ChargingErrorNotificationRequest>>?     CustomChargingErrorNotificationAcknowledgementParser         { get; set; }


        public CustomJObjectParserDelegate<Acknowledgement<ChargeDetailRecordRequest>>?            CustomSendChargeDetailRecordAcknowledgementParser            { get; set; }


        public CustomJObjectParserDelegate<StatusCode>?                                            CustomStatusCodeParser                                       { get; set; }

        #endregion

        #region Custom JSON serializers

        public CustomJObjectSerializerDelegate<PushEVSEDataRequest>?                  CustomPushEVSEDataRequestSerializer                    { get; set; }

        public CustomJObjectSerializerDelegate<OperatorEVSEData>?                     CustomOperatorEVSEDataSerializer                       { get; set; }

        public CustomJObjectSerializerDelegate<EVSEDataRecord>?                       CustomEVSEDataRecordSerializer                         { get; set; }

        public CustomJObjectSerializerDelegate<Address>?                              CustomAddressSerializer                                { get; set; }

        public CustomJObjectSerializerDelegate<ChargingFacility>?                     CustomChargingFacilitySerializer                       { get; set; }

        public CustomJObjectSerializerDelegate<GeoCoordinates>?                       CustomGeoCoordinatesSerializer                         { get; set; }

        public CustomJObjectSerializerDelegate<EnergyMeter>?                          CustomEnergyMeterSerializer                            { get; set; }

        public CustomJObjectSerializerDelegate<TransparencySoftwareStatus>?           CustomTransparencySoftwareStatusSerializer             { get; set; }

        public CustomJObjectSerializerDelegate<TransparencySoftware>?                 CustomTransparencySoftwareSerializer                   { get; set; }

        public CustomJObjectSerializerDelegate<EnergySource>?                         CustomEnergySourceSerializer                           { get; set; }

        public CustomJObjectSerializerDelegate<EnvironmentalImpact>?                  CustomEnvironmentalImpactSerializer                    { get; set; }

        public CustomJObjectSerializerDelegate<OpeningTime>?                          CustomOpeningTimesSerializer                           { get; set; }


        public CustomJObjectSerializerDelegate<PushEVSEStatusRequest>?                CustomPushEVSEStatusRequestSerializer                  { get; set; }

        public CustomJObjectSerializerDelegate<OperatorEVSEStatus>?                   CustomOperatorEVSEStatusSerializer                     { get; set; }

        public CustomJObjectSerializerDelegate<EVSEStatusRecord>?                     CustomEVSEStatusRecordSerializer                       { get; set; }


        public CustomJObjectSerializerDelegate<PushPricingProductDataRequest>?        CustomPushPricingProductDataRequestSerializer          { get; set; }

        public CustomJObjectSerializerDelegate<PricingProductData>?                   CustomPricingProductDataSerializer                     { get; set; }

        public CustomJObjectSerializerDelegate<PricingProductDataRecord>?             CustomPricingProductDataRecordSerializer               { get; set; }


        public CustomJObjectSerializerDelegate<PushEVSEPricingRequest>?               CustomPushEVSEPricingRequestSerializer                 { get; set; }

        public CustomJObjectSerializerDelegate<EVSEPricing>?                          CustomEVSEPricingSerializer                            { get; set; }


        public CustomJObjectSerializerDelegate<PullAuthenticationDataRequest>?        CustomPullAuthenticationDataRequestSerializer          { get; set; }


        public CustomJObjectSerializerDelegate<AuthorizeStartRequest>?                CustomAuthorizeStartRequestSerializer                  { get; set; }

        public CustomJObjectSerializerDelegate<Identification>?                       CustomIdentificationSerializer                         { get; set; }


        public CustomJObjectSerializerDelegate<AuthorizeStopRequest>?                 CustomAuthorizeStopRequestSerializer                   { get; set; }


        public CustomJObjectSerializerDelegate<ChargingStartNotificationRequest>?     CustomChargingStartNotificationRequestSerializer       { get; set; }

        public CustomJObjectSerializerDelegate<ChargingProgressNotificationRequest>?  CustomChargingProgressNotificationRequestSerializer    { get; set; }

        public CustomJObjectSerializerDelegate<ChargingEndNotificationRequest>?       CustomChargingEndNotificationRequestSerializer         { get; set; }

        public CustomJObjectSerializerDelegate<ChargingErrorNotificationRequest>?     CustomChargingErrorNotificationRequestSerializer       { get; set; }


        public CustomJObjectSerializerDelegate<ChargeDetailRecordRequest>?            CustomChargeDetailRecordRequestSerializer              { get; set; }

        public CustomJObjectSerializerDelegate<ChargeDetailRecord>?                   CustomChargeDetailRecordSerializer                     { get; set; }

        public CustomJObjectSerializerDelegate<SignedMeteringValue>?                  CustomSignedMeteringValueSerializer                    { get; set; }

        public CustomJObjectSerializerDelegate<CalibrationLawVerification>?           CustomCalibrationLawVerificationSerializer             { get; set; }

        #endregion

        #region Custom request/response logging converters

        #region PushEVSEData                (Request/Response)Converter

        public Func<DateTime, Object, PushEVSEDataRequest, String>
            PushEVSEDataRequestConverter                     { get; set; }

            = (timestamp, sender, pushEVSEDataRequest)
            => String.Concat(pushEVSEDataRequest.Action, " of ", pushEVSEDataRequest.EVSEDataRecords.Count(), " evse(s)");

        public Func<DateTime, Object, PushEVSEDataRequest, OICPResult<Acknowledgement<PushEVSEDataRequest>>, TimeSpan, String>
            PushEVSEDataResponseConverter                    { get; set; }

            = (timestamp, sender, pushEVSEDataRequest, pushEVSEDataResponse, runtime)
            => String.Concat(pushEVSEDataRequest.Action, " of ", pushEVSEDataRequest.EVSEDataRecords.Count(), " evse(s) => ", pushEVSEDataResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion

        #region PushEVSEStatus              (Request/Response)Converter

        public Func<DateTime, Object, PushEVSEStatusRequest, String>
            PushEVSEStatusRequestConverter                   { get; set; }

            = (timestamp, sender, pushEVSEStatusRequest)
            => String.Concat(pushEVSEStatusRequest.Action, " of ", pushEVSEStatusRequest.EVSEStatusRecords.Count(), " evse status");

        public Func<DateTime, Object, PushEVSEStatusRequest, OICPResult<Acknowledgement<PushEVSEStatusRequest>>, TimeSpan, String>
            PushEVSEStatusResponseConverter                  { get; set; }

            = (timestamp, sender, pushEVSEStatusRequest, pushEVSEStatusResponse, runtime)
            => String.Concat(pushEVSEStatusRequest.Action, " of ", pushEVSEStatusRequest.EVSEStatusRecords.Count(), " evse status => ", pushEVSEStatusResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion


        #region PushPricingProductData      (Request/Response)Converter

        public Func<DateTime, Object, PushPricingProductDataRequest, String>
            PushPricingProductDataRequestConverter                     { get; set; }

            = (timestamp, sender, pushPricingProductDataRequest)
            => String.Concat(pushPricingProductDataRequest.Action, " of ", pushPricingProductDataRequest.PricingProductData.PricingProductDataRecords.Count(), " pricing product data record(s)");

        public Func<DateTime, Object, PushPricingProductDataRequest, OICPResult<Acknowledgement<PushPricingProductDataRequest>>, TimeSpan, String>
            PushPricingProductDataResponseConverter                    { get; set; }

            = (timestamp, sender, pushPricingProductDataRequest, pushPricingProductDataResponse, runtime)
            => String.Concat(pushPricingProductDataRequest.Action, " of ", pushPricingProductDataRequest.PricingProductData.PricingProductDataRecords.Count(), " pricing product data record(s) => ", pushPricingProductDataResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion

        #region PushEVSEPricing             (Request/Response)Converter

        public Func<DateTime, Object, PushEVSEPricingRequest, String>
            PushEVSEPricingRequestConverter                   { get; set; }

            = (timestamp, sender, pushEVSEPricingRequest)
            => String.Concat(pushEVSEPricingRequest.Action, " of ", pushEVSEPricingRequest.EVSEPricing.Count(), " evse pricing record(s)");

        public Func<DateTime, Object, PushEVSEPricingRequest, OICPResult<Acknowledgement<PushEVSEPricingRequest>>, TimeSpan, String>
            PushEVSEPricingResponseConverter                  { get; set; }

            = (timestamp, sender, pushEVSEPricingRequest, pushEVSEPricingResponse, runtime)
            => String.Concat(pushEVSEPricingRequest.Action, " of ", pushEVSEPricingRequest.EVSEPricing.Count(), " evse pricing record(s) => ", pushEVSEPricingResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion


        #region AuthorizeStart              (Request/Response)Converter

        public Func<DateTime, Object, AuthorizeStartRequest, String>
            AuthorizeStartRequestConverter                   { get; set; }

            = (timestamp, sender, authorizeStartRequest)
            => String.Concat(authorizeStartRequest.Identification, " at ", authorizeStartRequest.EVSEId);

        public Func<DateTime, Object, AuthorizeStartRequest, OICPResult<AuthorizationStartResponse>, TimeSpan, String>
            AuthorizationStartResponseConverter              { get; set; }

            = (timestamp, sender, authorizeStartRequest, authorizationStartResponse, runtime)
            => String.Concat(authorizeStartRequest.Identification, " at ", authorizeStartRequest.EVSEId, " => ", authorizationStartResponse.Response?.AuthorizationStatus.ToString() ?? "failed!");

        #endregion

        #region AuthorizeStop               (Request/Response)Converter

        public Func<DateTime, Object, AuthorizeStopRequest, String>
            AuthorizeStopRequestConverter                    { get; set; }

            = (timestamp, sender, authorizeStopRequest)
            => String.Concat(authorizeStopRequest.Identification, " at ", authorizeStopRequest.EVSEId);

        public Func<DateTime, Object, AuthorizeStopRequest, OICPResult<AuthorizationStopResponse>, TimeSpan, String>
            AuthorizationStopResponseConverter               { get; set; }

            = (timestamp, sender, authorizeStopRequest, authorizationStopResponse, runtime)
            => String.Concat(authorizeStopRequest.Identification, " at ", authorizeStopRequest.EVSEId, " => ", authorizationStopResponse.Response?.AuthorizationStatus.ToString() ?? "failed!");

        #endregion


        #region ChargingStartNotification   (Request/Response)Converter

        public Func<DateTime, Object, ChargingStartNotificationRequest, String>
            ChargingStartNotificationRequestConverter        { get; set; }

            = (timestamp, sender, chargingStartNotificationRequest)
            => String.Concat(chargingStartNotificationRequest.Identification, " at ", chargingStartNotificationRequest.EVSEId);

        public Func<DateTime, Object, ChargingStartNotificationRequest, OICPResult<Acknowledgement<ChargingStartNotificationRequest>>, TimeSpan, String>
            ChargingStartNotificationResponseConverter       { get; set; }

            = (timestamp, sender, chargingStartNotificationRequest, chargingStartNotificationResponse, runtime)
            => String.Concat(chargingStartNotificationRequest.Identification, " at ", chargingStartNotificationRequest.EVSEId, " => ", chargingStartNotificationResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion

        #region ChargingProgressNotification(Request/Response)Converter

        public Func<DateTime, Object, ChargingProgressNotificationRequest, String>
            ChargingProgressNotificationRequestConverter     { get; set; }

            = (timestamp, sender, chargingProgressNotificationRequest)
            => String.Concat(chargingProgressNotificationRequest.Identification, " at ", chargingProgressNotificationRequest.EVSEId);

        public Func<DateTime, Object, ChargingProgressNotificationRequest, OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>, TimeSpan, String>
            ChargingProgressNotificationResponseConverter    { get; set; }

            = (timestamp, sender, chargingProgressNotificationRequest, chargingProgressNotificationResponse, runtime)
            => String.Concat(chargingProgressNotificationRequest.Identification, " at ", chargingProgressNotificationRequest.EVSEId, " => ", chargingProgressNotificationResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion

        #region ChargingEndNotification     (Request/Response)Converter

        public Func<DateTime, Object, ChargingEndNotificationRequest, String>
            ChargingEndNotificationRequestConverter          { get; set; }

            = (timestamp, sender, chargingEndNotificationRequest)
            => String.Concat(chargingEndNotificationRequest.Identification, " at ", chargingEndNotificationRequest.EVSEId);

        public Func<DateTime, Object, ChargingEndNotificationRequest, OICPResult<Acknowledgement<ChargingEndNotificationRequest>>, TimeSpan, String>
            ChargingEndNotificationResponseConverter         { get; set; }

            = (timestamp, sender, chargingEndNotificationRequest, chargingEndNotificationResponse, runtime)
            => String.Concat(chargingEndNotificationRequest.Identification, " at ", chargingEndNotificationRequest.EVSEId, " => ", chargingEndNotificationResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion

        #region ChargingErrorNotification   (Request/Response)Converter

        public Func<DateTime, Object, ChargingErrorNotificationRequest, String>
            ChargingErrorNotificationRequestConverter        { get; set; }

            = (timestamp, sender, chargingErrorNotificationRequest)
            => String.Concat(chargingErrorNotificationRequest.Identification, " at ", chargingErrorNotificationRequest.EVSEId);

        public Func<DateTime, Object, ChargingErrorNotificationRequest, OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>, TimeSpan, String>
            ChargingErrorNotificationResponseConverter       { get; set; }

            = (timestamp, sender, chargingErrorNotificationRequest, chargingErrorNotificationResponse, runtime)
            => String.Concat(chargingErrorNotificationRequest.Identification, " at ", chargingErrorNotificationRequest.EVSEId, " => ", chargingErrorNotificationResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion


        #region SendChargeDetailRecord      (Request/Response)Converter

        public Func<DateTime, Object, ChargeDetailRecordRequest, String>
            SendChargeDetailRecordRequestConverter           { get; set; }

            = (timestamp, sender, chargeDetailRecordRequest)
            => String.Concat(chargeDetailRecordRequest.ChargeDetailRecord.Identification, " at ", chargeDetailRecordRequest.ChargeDetailRecord.EVSEId, " (", chargeDetailRecordRequest.ChargeDetailRecord.SessionId, ")");

        public Func<DateTime, Object, ChargeDetailRecordRequest, OICPResult<Acknowledgement<ChargeDetailRecordRequest>>, TimeSpan, String>
            SendChargeDetailRecordResponseConverter          { get; set; }

            = (timestamp, sender, chargeDetailRecordRequest, chargeDetailRecordResponse, runtime)
            => String.Concat(chargeDetailRecordRequest.ChargeDetailRecord.Identification, " at ", chargeDetailRecordRequest.ChargeDetailRecord.EVSEId, " (", chargeDetailRecordRequest.ChargeDetailRecord.SessionId, ") => ", chargeDetailRecordResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion

        #endregion

        #region Events

        #region OnPushEVSEDataRequest/-Response

        /// <summary>
        /// An event fired whenever a PushEVSEData will be send.
        /// </summary>
        public event OnPushEVSEDataRequestDelegate?   OnPushEVSEDataRequest;

        /// <summary>
        /// An event fired whenever a PushEVSEData HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?         OnPushEVSEDataHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a PushEVSEData HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?        OnPushEVSEDataHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a PushEVSEData HTTP request had been received.
        /// </summary>
        public event OnPushEVSEDataResponseDelegate?  OnPushEVSEDataResponse;

        #endregion

        #region OnPushEVSEStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a PushEVSEStatus will be send.
        /// </summary>
        public event OnPushEVSEStatusRequestDelegate?   OnPushEVSEStatusRequest;

        /// <summary>
        /// An event fired whenever a PushEVSEStatus HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?           OnPushEVSEStatusHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a PushEVSEStatus HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?          OnPushEVSEStatusHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a PushEVSEStatus HTTP request had been received.
        /// </summary>
        public event OnPushEVSEStatusResponseDelegate?  OnPushEVSEStatusResponse;

        #endregion


        #region OnPushPricingProductDataRequest/-Response

        /// <summary>
        /// An event fired whenever a PushPricingProductData will be send.
        /// </summary>
        public event OnPushPricingProductDataRequestDelegate?   OnPushPricingProductDataRequest;

        /// <summary>
        /// An event fired whenever a PushPricingProductData HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                   OnPushPricingProductDataHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a PushPricingProductData HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                  OnPushPricingProductDataHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a PushPricingProductData HTTP request had been received.
        /// </summary>
        public event OnPushPricingProductDataResponseDelegate?  OnPushPricingProductDataResponse;

        #endregion

        #region OnPushEVSEPricingRequest/-Response

        /// <summary>
        /// An event fired whenever a PushEVSEPricing will be send.
        /// </summary>
        public event OnPushEVSEPricingRequestDelegate?   OnPushEVSEPricingRequest;

        /// <summary>
        /// An event fired whenever a PushEVSEPricing HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?            OnPushEVSEPricingHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a PushEVSEPricing HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?           OnPushEVSEPricingHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a PushEVSEPricing HTTP request had been received.
        /// </summary>
        public event OnPushEVSEPricingResponseDelegate?  OnPushEVSEPricingResponse;

        #endregion


        #region OnPullAuthenticationDataRequest/-Response

        /// <summary>
        /// An event fired whenever a PullAuthenticationData will be send.
        /// </summary>
        public event OnPullAuthenticationDataRequestDelegate?   OnPullAuthenticationDataRequest;

        /// <summary>
        /// An event fired whenever a PullAuthenticationData HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                   OnPullAuthenticationDataHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a PullAuthenticationData HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                  OnPullAuthenticationDataHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a PullAuthenticationData HTTP request had been received.
        /// </summary>
        public event OnPullAuthenticationDataResponseDelegate?  OnPullAuthenticationDataResponse;

        #endregion


        #region OnAuthorizeStartRequest/-Response

        /// <summary>
        /// An event fired whenever an AuthorizeStart request will be send.
        /// </summary>
        public event OnAuthorizeStartRequestDelegate?     OnAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever an AuthorizeStart HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?             OnAuthorizeStartHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to an AuthorizeStart HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?            OnAuthorizeStartHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to an AuthorizeStart request had been received.
        /// </summary>
        public event OnAuthorizeStartResponseDelegate?    OnAuthorizeStartResponse;

        #endregion

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event fired whenever an AuthorizeStop request will be send.
        /// </summary>
        public event OnAuthorizeStopRequestDelegate?   OnAuthorizeStopRequest;

        /// <summary>
        /// An event fired whenever an AuthorizeStop HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?          OnAuthorizeStopHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to an AuthorizeStop HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?         OnAuthorizeStopHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to an AuthorizeStop request had been received.
        /// </summary>
        public event OnAuthorizeStopResponseDelegate?  OnAuthorizeStopResponse;

        #endregion


        #region OnChargingStartNotificationRequest/-Response

        /// <summary>
        /// An event fired whenever a ChargingStartNotification will be send.
        /// </summary>
        public event OnChargingStartNotificationRequestDelegate?   OnChargingStartNotificationRequest;

        /// <summary>
        /// An event fired whenever a ChargingStartNotification HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                      OnChargingStartNotificationHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a ChargingStartNotification HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                     OnChargingStartNotificationHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a ChargingStartNotification had been received.
        /// </summary>
        public event OnChargingStartNotificationResponseDelegate?  OnChargingStartNotificationResponse;

        #endregion

        #region OnChargingProgressNotificationRequest/-Response

        /// <summary>
        /// An event fired whenever a ChargingProgressNotification will be send.
        /// </summary>
        public event OnChargingProgressNotificationRequestDelegate?   OnChargingProgressNotificationRequest;

        /// <summary>
        /// An event fired whenever a ChargingProgressNotification HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                         OnChargingProgressNotificationHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a ChargingProgressNotification HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                        OnChargingProgressNotificationHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a ChargingProgressNotification had been received.
        /// </summary>
        public event OnChargingProgressNotificationResponseDelegate?  OnChargingProgressNotificationResponse;

        #endregion

        #region OnChargingEndNotificationRequest/-Response

        /// <summary>
        /// An event fired whenever a ChargingEndNotification will be send.
        /// </summary>
        public event OnChargingEndNotificationRequestDelegate?   OnChargingEndNotificationRequest;

        /// <summary>
        /// An event fired whenever a ChargingEndNotification HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                    OnChargingEndNotificationHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a ChargingEndNotification HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                   OnChargingEndNotificationHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a ChargingEndNotification had been received.
        /// </summary>
        public event OnChargingEndNotificationResponseDelegate?  OnChargingEndNotificationResponse;

        #endregion

        #region OnChargingErrorNotificationRequest/-Response

        /// <summary>
        /// An event fired whenever a ChargingErrorNotification will be send.
        /// </summary>
        public event OnChargingErrorNotificationRequestDelegate?   OnChargingErrorNotificationRequest;

        /// <summary>
        /// An event fired whenever a ChargingErrorNotification HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                      OnChargingErrorNotificationHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a ChargingErrorNotification HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                     OnChargingErrorNotificationHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a ChargingErrorNotification had been received.
        /// </summary>
        public event OnChargingErrorNotificationResponseDelegate?  OnChargingErrorNotificationResponse;

        #endregion


        #region OnSendChargeDetailRecordRequest/-Response

        /// <summary>
        /// An event fired whenever a ChargeDetailRecord will be send.
        /// </summary>
        public event OnSendChargeDetailRecordRequestDelegate?   OnSendChargeDetailRecordRequest;

        /// <summary>
        /// An event fired whenever a ChargeDetailRecord HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?                   OnSendChargeDetailRecordHTTPRequest;

        /// <summary>
        /// An event fired whenever a response to a ChargeDetailRecord HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?                  OnSendChargeDetailRecordHTTPResponse;

        /// <summary>
        /// An event fired whenever a response to a ChargeDetailRecord had been received.
        /// </summary>
        public event OnSendChargeDetailRecordResponseDelegate?  OnSendChargeDetailRecordResponse;

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
        /// <param name="HTTPUserAgent">The HTTP user agent identification.</param>
        /// <param name="HTTPAuthentication">The optional HTTP authentication to use.</param>
        /// <param name="RequestTimeout">An optional request timeout.</param>
        /// <param name="TransmissionRetryDelay">The delay between transmission retries.</param>
        /// <param name="MaxNumberOfRetries">The maximum number of transmission retries for HTTP request.</param>
        /// <param name="InternalBufferSize">An optional size of the internal buffers.</param>
        /// <param name="DisableLogging">Disable all logging.</param>
        /// <param name="LoggingContext">An optional context for logging.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// <param name="DNSClient">The DNS client to use.</param>
        public CPOClient(URL?                                                       RemoteURL                    = null,
                         HTTPHostname?                                              VirtualHostname              = null,
                         String?                                                    Description                  = null,
                         Boolean?                                                   PreferIPv4                   = null,
                         RemoteTLSServerCertificateValidationHandler<IHTTPClient>?  RemoteCertificateValidator   = null,
                         LocalCertificateSelectionHandler?                          LocalCertificateSelector    = null,
                         X509Certificate?                                           ClientCert                   = null,
                         SslProtocols?                                              TLSProtocol                  = null,
                         String                                                     HTTPUserAgent                = DefaultHTTPUserAgent,
                         IHTTPAuthentication?                                       HTTPAuthentication           = null,
                         TimeSpan?                                                  RequestTimeout               = null,
                         TransmissionRetryDelayDelegate?                            TransmissionRetryDelay       = null,
                         UInt16?                                                    MaxNumberOfRetries           = DefaultMaxNumberOfRetries,
                         UInt32?                                                    InternalBufferSize           = null,
                         Boolean?                                                   DisableLogging               = false,
                         String?                                                    LoggingPath                  = null,
                         String                                                     LoggingContext               = CPOClientLogger.DefaultContext,
                         LogfileCreatorDelegate?                                    LogfileCreator               = null,
                         DNSClient?                                                 DNSClient                    = null)

            : base(RemoteURL           ?? DefaultRemoteURL,
                   VirtualHostname,
                   Description,
                   PreferIPv4,
                   RemoteCertificateValidator,
                   LocalCertificateSelector,
                   ClientCert,
                   TLSProtocol,
                   HTTPUserAgent       ?? DefaultHTTPUserAgent,
                   HTTPAuthentication,
                   RequestTimeout,
                   TransmissionRetryDelay,
                   MaxNumberOfRetries  ?? DefaultMaxNumberOfRetries,
                   InternalBufferSize,
                   false,
                   DisableLogging,
                   null,
                   DNSClient)

        {

            this.Counters        = new APICounters();

            this.JSONFormatting  = Newtonsoft.Json.Formatting.None;

            base.HTTPLogger      = this.DisableLogging == false
                                       ? new HTTP_Logger(
                                             this,
                                             LoggingPath,
                                             LoggingContext,
                                             LogfileCreator
                                         )
                                       : null;

            this.Logger          = this.DisableLogging == false
                                       ? new CPOClientLogger(
                                             this,
                                             LoggingPath,
                                             LoggingContext,
                                             LogfileCreator
                                         )
                                       : null;

        }

        #endregion


        //public override JObject ToJSON()
        //    => base.ToJSON(nameof(CPOClient));


        #region PushEVSEData                    (Request)

        /// <summary>
        /// Upload the given EVSE data records.
        /// </summary>
        /// <param name="Request">A PushEVSEData request.</param>
        public async Task<OICPResult<Acknowledgement<PushEVSEDataRequest>>>

            PushEVSEData(PushEVSEDataRequest Request)

        {

            #region Initial checks

            //Request = _CustomPushEVSEDataRequestMapper(Request);

            Byte                                               TransmissionRetry   = 0;
            OICPResult<Acknowledgement<PushEVSEDataRequest>>?  result              = null;

            #endregion

            #region Send OnPushEVSEDataRequest event

            var startTime = Timestamp.Now;

            Counters.PushEVSEData.IncRequests_OK();

            try
            {

                if (OnPushEVSEDataRequest is not null)
                    await Task.WhenAll(OnPushEVSEDataRequest.GetInvocationList().
                                       Cast<OnPushEVSEDataRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnPushEVSEDataRequest));
            }

            #endregion


            // Apply EVSE filter!

            #region No EVSE data to push?

            if (!Request.EVSEDataRecords.Any())
            {

                result = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Success(
                             Request,
                             Acknowledgement<PushEVSEDataRequest>.Success(
                                 Request,
                                 StatusCodeDescription: "No EVSE data to push"
                             )
                         );

            }

            #endregion

            else
            {

                var statusDescription = "HTTP request failed!";

                try
                {

                    do
                    {

                        #region Upstream HTTP request...

                        var HTTPResponse = await HTTPClientFactory.Create(RemoteURL,
                                                                          VirtualHostname,
                                                                          Description,
                                                                          PreferIPv4,
                                                                          RemoteCertificateValidator,
                                                                          LocalCertificateSelector,
                                                                          ClientCert,
                                                                          TLSProtocol,
                                                                          HTTPUserAgent,
                                                                          HTTPAuthentication,
                                                                          RequestTimeout,
                                                                          TransmissionRetryDelay,
                                                                          MaxNumberOfRetries,
                                                                          InternalBufferSize,
                                                                          UseHTTPPipelining,
                                                                          DisableLogging,
                                                                          null,
                                                                          DNSClient).

                                                  Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/evsepush/v23/operators/" + Request.OperatorId.ToString().Replace("*", "%2A") + "/data-records"),
                                                                                       requestbuilder => {
                                                                                           requestbuilder.Accept?.Add(HTTPContentType.Application.JSON_UTF8);
                                                                                           requestbuilder.ContentType  = HTTPContentType.Application.JSON_UTF8;
                                                                                           requestbuilder.Content      = Request.ToJSON(CustomPushEVSEDataRequestSerializer,
                                                                                                                                        CustomOperatorEVSEDataSerializer,
                                                                                                                                        CustomEVSEDataRecordSerializer,
                                                                                                                                        CustomAddressSerializer,
                                                                                                                                        CustomChargingFacilitySerializer,
                                                                                                                                        CustomGeoCoordinatesSerializer,
                                                                                                                                        CustomEnergyMeterSerializer,
                                                                                                                                        CustomTransparencySoftwareStatusSerializer,
                                                                                                                                        CustomTransparencySoftwareSerializer,
                                                                                                                                        CustomEnergySourceSerializer,
                                                                                                                                        CustomEnvironmentalImpactSerializer,
                                                                                                                                        CustomOpeningTimesSerializer).
                                                                                                                                 ToString(JSONFormatting).
                                                                                                                                 ToUTF8Bytes();
                                                                                           requestbuilder.Connection   = "close";
                                                                                       }),

                                                          RequestLogDelegate:   OnPushEVSEDataHTTPRequest,
                                                          ResponseLogDelegate:  OnPushEVSEDataHTTPResponse,
                                                          CancellationToken:    Request.CancellationToken,
                                                          EventTrackingId:      Request.EventTrackingId,
                                                          RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                                  ConfigureAwait(false);

                        #endregion


                        var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse) ?? Process_Id.NewRandom();

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                        {

                            if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                                HTTPResponse.HTTPBody?.Length > 0)
                            {

                                try
                                {

                                    // HTTP/1.1 200 OK
                                    // Server:            nginx/1.18.0
                                    // Date:              Sat, 09 Jan 2021 06:53:50 GMT
                                    // Content-Type:      application/json;charset=utf-8
                                    // Transfer-Encoding: chunked
                                    // Connection:        keep-alive
                                    // Process-ID:        d8d4583c-ff9b-44dd-bc92-b341f15f644e
                                    // cd .
                                    // {
                                    //     "Result":               true,
                                    //     "StatusCode": {
                                    //         "Code":             "000",
                                    //         "Description":      null,
                                    //         "AdditionalInfo":   null
                                    //     },
                                    //     "SessionID":            null,
                                    //     "CPOPartnerSessionID":  null,
                                    //     "EMPPartnerSessionID":  null
                                    // }

                                    if (Acknowledgement<PushEVSEDataRequest>.TryParse(Request,
                                                                                      JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String() ?? ""),
                                                                                      out var acknowledgement,
                                                                                      out var ErrorResponse,
                                                                                      HTTPResponse,
                                                                                      HTTPResponse.Timestamp,
                                                                                      HTTPResponse.EventTrackingId,
                                                                                      HTTPResponse.Runtime,
                                                                                      processId,
                                                                                      CustomPushEVSEDataAcknowledgementParser))
                                    {

                                        Counters.PushEVSEData.IncResponses_OK();

                                        result = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Success(
                                                     Request,
                                                     acknowledgement!,
                                                     processId
                                                 );

                                    }

                                }
                                catch (Exception e)
                                {

                                    result = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<PushEVSEDataRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     processId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     Request,
                                                     HTTPResponse,
                                                     false
                                                 )
                                             );

                                }

                            }

                            TransmissionRetry = Byte.MaxValue - 1;
                            break;

                        }

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.BadRequest)
                        {

                            if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                                HTTPResponse.HTTPBody?.Length > 0)
                            {

                                // HTTP/1.1 400
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

                                if (ValidationErrorList.TryParse(JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String() ?? ""),
                                                                 out var validationErrors,
                                                                 out var errorResponse))
                                {

                                    result = OICPResult<Acknowledgement<PushEVSEDataRequest>>.BadRequest(
                                                 Request,
                                                 validationErrors,
                                                 processId
                                             );

                                }

                            }

                            break;

                        }

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                        {

                            // HTTP/1.1 403 Forbidden
                            // Server:          nginx/1.18.0 (Ubuntu)
                            // Date:            Thu, 15 Apr 2021 22:47:22 GMT
                            // Content-Type:    text/html
                            // Content-Length:  162
                            // Connection:      keep-alive
                            // 
                            // <html>
                            // <head><title>403 Forbidden</title></head>
                            // <body>
                            // <center><h1>403 Forbidden</h1></center>
                            // <hr><center>nginx/1.18.0 (Ubuntu)</center>
                            // </body>
                            // </html>

                            statusDescription = "Hubject firewall problem!";
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

                            statusDescription = "Operator/provider identification is not linked to the TLS client certificate!";

                            if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                                HTTPResponse.HTTPBody?.Length > 0)
                            {

                                try
                                {

                                    var json = JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String());

                                    if (json is not null &&
                                        json["StatusCode"] is JObject JSONObject &&
                                        StatusCode.TryParse(JSONObject,
                                                            out StatusCode? statusCode,
                                                            out String? ErrorResponse,
                                                            CustomStatusCodeParser))
                                    {

                                        result = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                                                     Request,
                                                     new Acknowledgement<PushEVSEDataRequest>(
                                                         HTTPResponse.Timestamp,
                                                         HTTPResponse.EventTrackingId,
                                                         processId,
                                                         HTTPResponse.Runtime,
                                                         statusCode!,
                                                         Request
                                                     ),
                                                     processId
                                                 );

                                    }

                                }
                                catch (Exception e)
                                {

                                    result = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<PushEVSEDataRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     processId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     Request,
                                                     HTTPResponse,
                                                     false
                                                 )
                                             );

                                }

                            }

                            break;

                        }

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                        {

                            // HTTP/1.1 404 NotFound
                            // Server:          nginx/1.18.0 (Ubuntu)
                            // Date:            Wed, 03 Mar 2021 01:00:15 GMT
                            // Content-Type:    application/json;charset=UTF-8
                            // Content-Length:  85
                            // Connection:      keep-alive
                            // Process-ID:      7bb86bc9-659f-4e57-8136-a7eb9ebc9c1d
                            // 
                            // {
                            //     "StatusCode": {
                            //         "Code":            "300",
                            //         "Description":     "Partner not found",
                            //         "AdditionalInfo":   null
                            //     }
                            // }

                            if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                                HTTPResponse.HTTPBody?.Length > 0)
                            {

                                try
                                {

                                    var json = JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String());

                                    if (json is not null &&
                                        json["StatusCode"] is JObject JSONObject &&
                                        StatusCode.TryParse(JSONObject,
                                                            out StatusCode? statusCode,
                                                            out String? ErrorResponse,
                                                            CustomStatusCodeParser))
                                    {

                                        result = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                                                     Request,
                                                     new Acknowledgement<PushEVSEDataRequest>(
                                                         HTTPResponse.Timestamp,
                                                         HTTPResponse.EventTrackingId,
                                                         processId,
                                                         HTTPResponse.Runtime,
                                                         statusCode!,
                                                         Request
                                                     ),
                                                     processId
                                                 );

                                    }

                                }
                                catch (Exception e)
                                {

                                    result = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<PushEVSEDataRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     processId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     Request,
                                                     HTTPResponse,
                                                     false
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

                    result = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                                 Request,
                                 new Acknowledgement<PushEVSEDataRequest>(
                                     Timestamp.Now,
                                     Request.EventTrackingId ?? EventTracking_Id.New,
                                     Process_Id.NewRandom(),
                                     Timestamp.Now - Request.Timestamp,
                                     new StatusCode(
                                         StatusCodes.SystemError,
                                         e.Message,
                                         e.StackTrace
                                     ),
                                     Request,
                                     null,
                                     false
                                 )
                             );

                }

                result ??= OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                               Request,
                               new Acknowledgement<PushEVSEDataRequest>(
                                   Timestamp.Now,
                                   Request.EventTrackingId ?? EventTracking_Id.New,
                                   Process_Id.NewRandom(),
                                   Timestamp.Now - Request.Timestamp,
                                   new StatusCode(
                                       StatusCodes.SystemError,
                                       statusDescription ?? "HTTP request failed!"
                                   ),
                                   Request,
                                   null,
                                   false
                               )
                           );

                if (result.IsNotSuccessful)
                    Counters.PushEVSEData.IncResponses_Error();

            }


            #region Send OnPushEVSEDataResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnPushEVSEDataResponse is not null)
                    await Task.WhenAll(OnPushEVSEDataResponse.GetInvocationList().
                                       Cast<OnPushEVSEDataResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnPushEVSEDataResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region PushEVSEStatus                  (Request)

        /// <summary>
        /// Upload the given EVSE status records.
        /// </summary>
        /// <param name="Request">A PushEVSEStatus request.</param>
        public async Task<OICPResult<Acknowledgement<PushEVSEStatusRequest>>>

            PushEVSEStatus(PushEVSEStatusRequest Request)

        {

            #region Initial checks

            //Request = _CustomPushEVSEStatusRequestMapper(Request);

            Byte                                                 TransmissionRetry   = 0;
            OICPResult<Acknowledgement<PushEVSEStatusRequest>>?  result              = null;

            #endregion

            #region Send OnPushEVSEStatusRequest event

            var startTime = Timestamp.Now;

            Counters.PushEVSEStatus.IncRequests_OK();

            try
            {

                if (OnPushEVSEStatusRequest is not null)
                    await Task.WhenAll(OnPushEVSEStatusRequest.GetInvocationList().
                                       Cast<OnPushEVSEStatusRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnPushEVSEStatusRequest));
            }

            #endregion


            // Apply EVSE filter!

            #region No EVSE status to push?

            if (!Request.EVSEStatusRecords.Any())
            {

                result = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Success(
                             Request,
                             Acknowledgement<PushEVSEStatusRequest>.Success(
                                 Request,
                                 StatusCodeDescription: "No EVSE status to push"
                             )
                         );

            }

            #endregion

            else
            {

                var statusDescription = "HTTP request failed!";

                try
                {

                    do
                    {

                        #region Upstream HTTP request...

                        var HTTPResponse = await HTTPClientFactory.Create(RemoteURL,
                                                                          VirtualHostname,
                                                                          Description,
                                                                          PreferIPv4,
                                                                          RemoteCertificateValidator,
                                                                          LocalCertificateSelector,
                                                                          ClientCert,
                                                                          TLSProtocol,
                                                                          HTTPUserAgent,
                                                                          HTTPAuthentication,
                                                                          RequestTimeout,
                                                                          TransmissionRetryDelay,
                                                                          MaxNumberOfRetries,
                                                                          InternalBufferSize,
                                                                          UseHTTPPipelining,
                                                                          DisableLogging,
                                                                          null,
                                                                          DNSClient).

                                                  Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/evsepush/v21/operators/" + Request.OperatorId.ToString().Replace("*", "%2A") + "/status-records"),
                                                                                       requestbuilder => {
                                                                                           requestbuilder.Accept?.Add(HTTPContentType.Application.JSON_UTF8);
                                                                                           requestbuilder.ContentType  = HTTPContentType.Application.JSON_UTF8;
                                                                                           requestbuilder.Content      = Request.ToJSON(CustomPushEVSEStatusRequestSerializer,
                                                                                                                                        CustomOperatorEVSEStatusSerializer,
                                                                                                                                        CustomEVSEStatusRecordSerializer).
                                                                                                                                 ToString(JSONFormatting).
                                                                                                                                 ToUTF8Bytes();
                                                                                           requestbuilder.Connection   = "close";
                                                                                       }),

                                                          RequestLogDelegate:   OnPushEVSEStatusHTTPRequest,
                                                          ResponseLogDelegate:  OnPushEVSEStatusHTTPResponse,
                                                          CancellationToken:    Request.CancellationToken,
                                                          EventTrackingId:      Request.EventTrackingId,
                                                          RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                                  ConfigureAwait(false);

                        #endregion


                        var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse) ?? Process_Id.NewRandom();

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                        {

                            if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                                HTTPResponse.HTTPBody?.Length > 0)
                            {

                                try
                                {

                                    // HTTP/1.1 200 OK
                                    // Server:             nginx/1.18.0 (Ubuntu)
                                    // Date:               Tue, 02 Mar 2021 17:51:14 GMT
                                    // Content-Type:       application/json;charset=utf-8
                                    // Transfer-Encoding:  chunked
                                    // Connection:         keep-alive
                                    // Process-ID:         332c9d01-2ea4-4d15-9d4a-bb9f5abd097c
                                    // 
                                    // {
                                    //     "Result":               true,
                                    //     "StatusCode": {
                                    //         "Code":             "000",
                                    //         "Description":      null,
                                    //         "AdditionalInfo":   null
                                    //     },
                                    //     "SessionID":            null,
                                    //     "CPOPartnerSessionID":  null,
                                    //     "EMPPartnerSessionID":  null
                                    // }

                                    if (Acknowledgement<PushEVSEStatusRequest>.TryParse(Request,
                                                                                        JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String() ?? ""),
                                                                                        out var acknowledgement,
                                                                                        out var ErrorResponse,
                                                                                        HTTPResponse,
                                                                                        HTTPResponse.Timestamp,
                                                                                        HTTPResponse.EventTrackingId,
                                                                                        HTTPResponse.Runtime,
                                                                                        processId,
                                                                                        CustomPushEVSEStatusAcknowledgementParser))
                                    {

                                        Counters.PushEVSEStatus.IncResponses_OK();

                                        result = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Success(
                                                     Request,
                                                     acknowledgement!,
                                                     processId
                                                 );

                                    }

                                }
                                catch (Exception e)
                                {

                                    result = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<PushEVSEStatusRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     processId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     Request,
                                                     HTTPResponse,
                                                     false
                                                 )
                                             );

                                }

                            }

                            TransmissionRetry = Byte.MaxValue - 1;
                            break;

                        }

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.BadRequest)
                        {

                            if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                                HTTPResponse.HTTPBody?.Length > 0)
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
                                //     "message": "Error parsing/validating JSON.",
                                //     "validationErrors": [
                                //         {
                                //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].hotlinePhoneNumber",
                                //             "errorMessage":    "must match \"^\\+[0-9]{5,15}$\""
                                //         },
                                //         {
                                //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].geoCoordinates",
                                //             "errorMessage":    "may not be null"
                                //         },
                                //         {
                                //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].chargingStationNames",
                                //             "errorMessage":    "may not be empty"
                                //         },
                                //         {
                                //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].plugs",
                                //             "errorMessage":    "may not be empty"
                                //         }
                                //     ]
                                // }

                                if (ValidationErrorList.TryParse(JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String() ?? ""),
                                                                 out var validationErrors,
                                                                 out var errorResponse))
                                {

                                    result = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.BadRequest(
                                                 Request,
                                                 validationErrors,
                                                 processId
                                             );

                                }

                            }

                        }

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                        {

                            // HTTP/1.1 403 Forbidden
                            // Server:          nginx/1.18.0 (Ubuntu)
                            // Date:            Thu, 15 Apr 2021 22:47:22 GMT
                            // Content-Type:    text/html
                            // Content-Length:  162
                            // Connection:      keep-alive
                            // 
                            // <html>
                            // <head><title>403 Forbidden</title></head>
                            // <body>
                            // <center><h1>403 Forbidden</h1></center>
                            // <hr><center>nginx/1.18.0 (Ubuntu)</center>
                            // </body>
                            // </html>

                            statusDescription = "Hubject firewall problem!";
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

                            statusDescription = "Operator/provider identification is not linked to the TLS client certificate!";

                            if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                                HTTPResponse.HTTPBody?.Length > 0)
                            {

                                try
                                {

                                    var json = JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String());

                                    if (json is not null &&
                                        json["StatusCode"] is JObject JSONObject &&
                                        StatusCode.TryParse(JSONObject,
                                                            out StatusCode? statusCode,
                                                            out String? ErrorResponse,
                                                            CustomStatusCodeParser))
                                    {

                                        result = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                                                     Request,
                                                     new Acknowledgement<PushEVSEStatusRequest>(
                                                         HTTPResponse.Timestamp,
                                                         HTTPResponse.EventTrackingId,
                                                         processId,
                                                         HTTPResponse.Runtime,
                                                         statusCode!,
                                                         Request
                                                     ),
                                                     processId
                                                 );

                                    }

                                }
                                catch (Exception e)
                                {

                                    result = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<PushEVSEStatusRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     processId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     Request,
                                                     HTTPResponse,
                                                     false
                                                 )
                                             );

                                }

                            }

                            break;

                        }

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                        {

                            // HTTP/1.1 404 NotFound
                            // Server: nginx/1.18.0 (Ubuntu)
                            // Date: Wed, 03 Mar 2021 01:00:15 GMT
                            // Content-Type: application/json;charset=UTF-8
                            // Content-Length: 85
                            // Connection: keep-alive
                            // Process-ID: 7bb86bc9-659f-4e57-8136-a7eb9ebc9c1d
                            // 
                            // {
                            //     "StatusCode": {
                            //         "Code":            "300",
                            //         "Description":     "Partner not found",
                            //         "AdditionalInfo":   null
                            //     }
                            // }

                            if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                                HTTPResponse.HTTPBody?.Length > 0)
                            {

                                try
                                {

                                    var json = JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String());

                                    if (json is not null &&
                                        json["StatusCode"] is JObject JSONObject &&
                                        StatusCode.TryParse(JSONObject,
                                                            out StatusCode? statusCode,
                                                            out String? ErrorResponse,
                                                            CustomStatusCodeParser))
                                    {

                                        result = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                                                     Request,
                                                     new Acknowledgement<PushEVSEStatusRequest>(
                                                         HTTPResponse.Timestamp,
                                                         HTTPResponse.EventTrackingId,
                                                         processId,
                                                         HTTPResponse.Runtime,
                                                         statusCode!,
                                                         Request
                                                     ),
                                                     processId
                                                 );

                                    }

                                }
                                catch (Exception e)
                                {

                                    result = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<PushEVSEStatusRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     processId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     Request,
                                                     HTTPResponse,
                                                     false
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

                    result = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                                 Request,
                                 new Acknowledgement<PushEVSEStatusRequest>(
                                     Timestamp.Now,
                                     Request.EventTrackingId ?? EventTracking_Id.New,
                                     Process_Id.NewRandom(),
                                     Timestamp.Now - Request.Timestamp,
                                     new StatusCode(
                                         StatusCodes.SystemError,
                                         e.Message,
                                         e.StackTrace
                                     ),
                                     Request,
                                     null,
                                     false
                                 )
                             );

                }

                result ??= OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                               Request,
                               new Acknowledgement<PushEVSEStatusRequest>(
                                   Timestamp.Now,
                                   Request.EventTrackingId ?? EventTracking_Id.New,
                                   Process_Id.NewRandom(),
                                   Timestamp.Now - Request.Timestamp,
                                   new StatusCode(
                                       StatusCodes.SystemError,
                                       statusDescription ?? "HTTP request failed!"
                                   ),
                                   Request,
                                   null,
                                   false
                               )
                           );

                if (result.IsNotSuccessful)
                    Counters.PushEVSEStatus.IncResponses_Error();

            }


            #region Send OnPushEVSEStatusResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnPushEVSEStatusResponse is not null)
                    await Task.WhenAll(OnPushEVSEStatusResponse.GetInvocationList().
                                       Cast<OnPushEVSEStatusResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnPushEVSEStatusResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region PushPricingProductData          (Request)

        /// <summary>
        /// Upload the given pricing product data.
        /// </summary>
        /// <param name="Request">A PushPricingProductData request.</param>
        public async Task<OICPResult<Acknowledgement<PushPricingProductDataRequest>>>

            PushPricingProductData(PushPricingProductDataRequest Request)

        {

            #region Initial checks

            //Request = _CustomPushPricingProductDataRequestMapper(Request);

            Byte                                                         TransmissionRetry   = 0;
            OICPResult<Acknowledgement<PushPricingProductDataRequest>>?  result              = null;

            #endregion

            #region Send OnPushPricingProductDataRequest event

            var startTime = Timestamp.Now;

            Counters.PushPricingProductData.IncRequests_OK();

            try
            {

                if (OnPushPricingProductDataRequest is not null)
                    await Task.WhenAll(OnPushPricingProductDataRequest.GetInvocationList().
                                       Cast<OnPushPricingProductDataRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnPushPricingProductDataRequest));
            }

            #endregion


            // Apply EVSE filter!

            #region No EVSE data to push?

            if (!Request.PricingProductDataRecords.Any())
            {

                result = OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Success(
                             Request,
                             Acknowledgement<PushPricingProductDataRequest>.Success(Request,
                                                                                    StatusCodeDescription: "No EVSE data to push")
                         );

            }

            #endregion

            else
            {

                var statusDescription = "HTTP request failed!";

                try
                {

                    do
                    {

                        #region Upstream HTTP request...

                        var HTTPResponse = await HTTPClientFactory.Create(RemoteURL,
                                                                          VirtualHostname,
                                                                          Description,
                                                                          PreferIPv4,
                                                                          RemoteCertificateValidator,
                                                                          LocalCertificateSelector,
                                                                          ClientCert,
                                                                          TLSProtocol,
                                                                          HTTPUserAgent,
                                                                          HTTPAuthentication,
                                                                          RequestTimeout,
                                                                          TransmissionRetryDelay,
                                                                          MaxNumberOfRetries,
                                                                          InternalBufferSize,
                                                                          UseHTTPPipelining,
                                                                          DisableLogging,
                                                                          null,
                                                                          DNSClient).

                                                  Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/dynamicpricing/v10/operators/" + Request.OperatorId.ToString().Replace("*", "%2A") + "/pricing-products"),
                                                                                       requestbuilder => {
                                                                                           requestbuilder.Accept?.Add(HTTPContentType.Application.JSON_UTF8);
                                                                                           requestbuilder.ContentType  = HTTPContentType.Application.JSON_UTF8;
                                                                                           requestbuilder.Content      = Request.ToJSON(CustomPushPricingProductDataRequestSerializer,
                                                                                                                                        CustomPricingProductDataSerializer,
                                                                                                                                        CustomPricingProductDataRecordSerializer).
                                                                                                                                 ToString(JSONFormatting).
                                                                                                                                 ToUTF8Bytes();
                                                                                           requestbuilder.Connection   = "close";
                                                                                       }),

                                                          RequestLogDelegate:   OnPushPricingProductDataHTTPRequest,
                                                          ResponseLogDelegate:  OnPushPricingProductDataHTTPResponse,
                                                          CancellationToken:    Request.CancellationToken,
                                                          EventTrackingId:      Request.EventTrackingId,
                                                          RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                                  ConfigureAwait(false);

                        #endregion


                        var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse) ?? Process_Id.NewRandom();

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                        {

                            if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                                HTTPResponse.HTTPBody?.Length > 0)
                            {

                                try
                                {

                                    // HTTP/1.1 200 OK
                                    // Server:            nginx/1.18.0
                                    // Date:              Sat, 09 Jan 2021 06:53:50 GMT
                                    // Content-Type:      application/json;charset=utf-8
                                    // Transfer-Encoding: chunked
                                    // Connection:        keep-alive
                                    // Process-ID:        d8d4583c-ff9b-44dd-bc92-b341f15f644e
                                    // cd .
                                    // {
                                    //     "Result":               true,
                                    //     "StatusCode": {
                                    //         "Code":             "000",
                                    //         "Description":      null,
                                    //         "AdditionalInfo":   null
                                    //     },
                                    //     "SessionID":            null,
                                    //     "CPOPartnerSessionID":  null,
                                    //     "EMPPartnerSessionID":  null
                                    // }

                                    if (Acknowledgement<PushPricingProductDataRequest>.TryParse(Request,
                                                                                                JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String() ?? ""),
                                                                                                out var acknowledgement,
                                                                                                out var ErrorResponse,
                                                                                                HTTPResponse,
                                                                                                HTTPResponse.Timestamp,
                                                                                                HTTPResponse.EventTrackingId,
                                                                                                HTTPResponse.Runtime,
                                                                                                processId,
                                                                                                CustomPushPricingProductDataAcknowledgementParser))
                                    {

                                        Counters.PushPricingProductData.IncResponses_OK();

                                        result = OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Success(
                                                     Request,
                                                     acknowledgement!,
                                                     processId
                                                 );

                                    }

                                }
                                catch (Exception e)
                                {

                                    result = OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<PushPricingProductDataRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     processId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     Request,
                                                     HTTPResponse,
                                                     false
                                                 )
                                             );

                                }

                            }

                            TransmissionRetry = Byte.MaxValue - 1;
                            break;

                        }

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.BadRequest)
                        {

                            if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                                HTTPResponse.HTTPBody?.Length > 0)
                            {

                                // HTTP/1.1 400
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

                                if (ValidationErrorList.TryParse(JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String() ?? ""),
                                                                 out var validationErrors,
                                                                 out var errorResponse))
                                {

                                    result = OICPResult<Acknowledgement<PushPricingProductDataRequest>>.BadRequest(
                                                 Request,
                                                 validationErrors,
                                                 processId
                                             );

                                }

                            }

                            break;

                        }

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                        {

                            // HTTP/1.1 403 Forbidden
                            // Server:          nginx/1.18.0 (Ubuntu)
                            // Date:            Thu, 15 Apr 2021 22:47:22 GMT
                            // Content-Type:    text/html
                            // Content-Length:  162
                            // Connection:      keep-alive
                            // 
                            // <html>
                            // <head><title>403 Forbidden</title></head>
                            // <body>
                            // <center><h1>403 Forbidden</h1></center>
                            // <hr><center>nginx/1.18.0 (Ubuntu)</center>
                            // </body>
                            // </html>

                            statusDescription = "Hubject firewall problem!";
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

                            statusDescription = "Operator/provider identification is not linked to the TLS client certificate!";

                            if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                                HTTPResponse.HTTPBody?.Length > 0)
                            {

                                try
                                {

                                    var json = JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String());

                                    if (json is not null &&
                                        json["StatusCode"] is JObject JSONObject &&
                                        StatusCode.TryParse(JSONObject,
                                                            out StatusCode? statusCode,
                                                            out String? ErrorResponse,
                                                            CustomStatusCodeParser))
                                    {

                                        result = OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Failed(
                                                     Request,
                                                     new Acknowledgement<PushPricingProductDataRequest>(
                                                         HTTPResponse.Timestamp,
                                                         HTTPResponse.EventTrackingId,
                                                         processId,
                                                         HTTPResponse.Runtime,
                                                         statusCode!,
                                                         Request
                                                     ),
                                                     processId
                                                 );

                                    }

                                }
                                catch (Exception e)
                                {

                                    result = OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<PushPricingProductDataRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     processId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     Request,
                                                     HTTPResponse,
                                                     false
                                                 )
                                             );

                                }

                            }

                            break;

                        }

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                        {

                            // HTTP/1.1 404 NotFound
                            // Server:          nginx/1.18.0 (Ubuntu)
                            // Date:            Wed, 03 Mar 2021 01:00:15 GMT
                            // Content-Type:    application/json;charset=UTF-8
                            // Content-Length:  85
                            // Connection:      keep-alive
                            // Process-ID:      7bb86bc9-659f-4e57-8136-a7eb9ebc9c1d
                            // 
                            // {
                            //     "StatusCode": {
                            //         "Code":            "300",
                            //         "Description":     "Partner not found",
                            //         "AdditionalInfo":   null
                            //     }
                            // }

                            if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                                HTTPResponse.HTTPBody?.Length > 0)
                            {

                                try
                                {

                                    var json = JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String());

                                    if (json is not null &&
                                        json["StatusCode"] is JObject JSONObject &&
                                        StatusCode.TryParse(JSONObject,
                                                            out StatusCode? statusCode,
                                                            out String? ErrorResponse,
                                                            CustomStatusCodeParser))
                                    {

                                        result = OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Failed(
                                                     Request,
                                                     new Acknowledgement<PushPricingProductDataRequest>(
                                                         HTTPResponse.Timestamp,
                                                         HTTPResponse.EventTrackingId,
                                                         processId,
                                                         HTTPResponse.Runtime,
                                                         statusCode!,
                                                         Request
                                                     ),
                                                     processId
                                                 );

                                    }

                                }
                                catch (Exception e)
                                {

                                    result = OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<PushPricingProductDataRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     processId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     Request,
                                                     HTTPResponse,
                                                     false
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

                    result = OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Failed(
                                 Request,
                                 new Acknowledgement<PushPricingProductDataRequest>(
                                     Timestamp.Now,
                                     Request.EventTrackingId ?? EventTracking_Id.New,
                                     Process_Id.NewRandom(),
                                     Timestamp.Now - Request.Timestamp,
                                     new StatusCode(
                                         StatusCodes.SystemError,
                                         e.Message,
                                         e.StackTrace
                                     ),
                                     Request,
                                     null,
                                     false
                                 )
                             );

                }

                result ??= OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Failed(
                               Request,
                               new Acknowledgement<PushPricingProductDataRequest>(
                                   Timestamp.Now,
                                   Request.EventTrackingId ?? EventTracking_Id.New,
                                   Process_Id.NewRandom(),
                                   Timestamp.Now - Request.Timestamp,
                                   new StatusCode(
                                       StatusCodes.SystemError,
                                       statusDescription ?? "HTTP request failed!"
                                   ),
                                   Request,
                                   null,
                                   false
                               )
                           );

                if (result.IsNotSuccessful)
                    Counters.PushPricingProductData.IncResponses_Error();

            }


            #region Send OnPushPricingProductDataResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnPushPricingProductDataResponse is not null)
                    await Task.WhenAll(OnPushPricingProductDataResponse.GetInvocationList().
                                       Cast<OnPushPricingProductDataResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnPushPricingProductDataResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region PushEVSEPricing                 (Request)

        /// <summary>
        /// Upload the given EVSE pricing data.
        /// </summary>
        /// <param name="Request">A PushEVSEPricing request.</param>
        public async Task<OICPResult<Acknowledgement<PushEVSEPricingRequest>>>

            PushEVSEPricing(PushEVSEPricingRequest Request)

        {

            #region Initial checks

            //Request = _CustomPushEVSEPricingRequestMapper(Request);

            Byte                                                  TransmissionRetry   = 0;
            OICPResult<Acknowledgement<PushEVSEPricingRequest>>?  result              = null;

            #endregion

            #region Send OnPushEVSEPricingRequest event

            var startTime = Timestamp.Now;

            Counters.PushEVSEPricing.IncRequests_OK();

            try
            {

                if (OnPushEVSEPricingRequest is not null)
                    await Task.WhenAll(OnPushEVSEPricingRequest.GetInvocationList().
                                       Cast<OnPushEVSEPricingRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnPushEVSEPricingRequest));
            }

            #endregion


            // Apply EVSE filter!

            #region No EVSE data to push?

            if (!Request.EVSEPricing.Any())
            {

                result = OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Success(
                             Request,
                             Acknowledgement<PushEVSEPricingRequest>.Success(Request,
                                                                             StatusCodeDescription: "No EVSE data to push")
                         );

            }

            #endregion

            else
            {

                var statusDescription = "HTTP request failed!";

                try
                {

                    do
                    {

                        #region Upstream HTTP request...

                        var HTTPResponse = await HTTPClientFactory.Create(RemoteURL,
                                                                          VirtualHostname,
                                                                          Description,
                                                                          PreferIPv4,
                                                                          RemoteCertificateValidator,
                                                                          LocalCertificateSelector,
                                                                          ClientCert,
                                                                          TLSProtocol,
                                                                          HTTPUserAgent,
                                                                          HTTPAuthentication,
                                                                          RequestTimeout,
                                                                          TransmissionRetryDelay,
                                                                          MaxNumberOfRetries,
                                                                          InternalBufferSize,
                                                                          UseHTTPPipelining,
                                                                          DisableLogging,
                                                                          null,
                                                                          DNSClient).

                                                  Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/dynamicpricing/v10/operators/" + Request.OperatorId.ToString().Replace("*", "%2A") + "/evse-pricing"),
                                                                                       requestbuilder => {
                                                                                           requestbuilder.Accept?.Add(HTTPContentType.Application.JSON_UTF8);
                                                                                           requestbuilder.ContentType  = HTTPContentType.Application.JSON_UTF8;
                                                                                           requestbuilder.Content      = Request.ToJSON(CustomPushEVSEPricingRequestSerializer,
                                                                                                                                        CustomEVSEPricingSerializer).
                                                                                                                                 ToString(JSONFormatting).
                                                                                                                                 ToUTF8Bytes();
                                                                                           requestbuilder.Connection   = "close";
                                                                                       }),

                                                          RequestLogDelegate:   OnPushEVSEPricingHTTPRequest,
                                                          ResponseLogDelegate:  OnPushEVSEPricingHTTPResponse,
                                                          CancellationToken:    Request.CancellationToken,
                                                          EventTrackingId:      Request.EventTrackingId,
                                                          RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                                  ConfigureAwait(false);

                        #endregion


                        var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse) ?? Process_Id.NewRandom();

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                        {

                            if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                                HTTPResponse.HTTPBody?.Length > 0)
                            {

                                try
                                {

                                    // HTTP/1.1 200 OK
                                    // Server:            nginx/1.18.0
                                    // Date:              Sat, 09 Jan 2021 06:53:50 GMT
                                    // Content-Type:      application/json;charset=utf-8
                                    // Transfer-Encoding: chunked
                                    // Connection:        keep-alive
                                    // Process-ID:        d8d4583c-ff9b-44dd-bc92-b341f15f644e
                                    // cd .
                                    // {
                                    //     "Result":               true,
                                    //     "StatusCode": {
                                    //         "Code":             "000",
                                    //         "Description":      null,
                                    //         "AdditionalInfo":   null
                                    //     },
                                    //     "SessionID":            null,
                                    //     "CPOPartnerSessionID":  null,
                                    //     "EMPPartnerSessionID":  null
                                    // }

                                    if (Acknowledgement<PushEVSEPricingRequest>.TryParse(Request,
                                                                                         JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String() ?? ""),
                                                                                         out var acknowledgement,
                                                                                         out var ErrorResponse,
                                                                                         HTTPResponse,
                                                                                         HTTPResponse.Timestamp,
                                                                                         HTTPResponse.EventTrackingId,
                                                                                         HTTPResponse.Runtime,
                                                                                         processId,
                                                                                         CustomPushEVSEPricingAcknowledgementParser))
                                    {

                                        Counters.PushEVSEPricing.IncResponses_OK();

                                        result = OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Success(
                                                     Request,
                                                     acknowledgement!,
                                                     processId
                                                 );

                                    }

                                }
                                catch (Exception e)
                                {

                                    result = OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<PushEVSEPricingRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     processId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     Request,
                                                     HTTPResponse,
                                                     false
                                                 )
                                             );

                                }

                            }

                            TransmissionRetry = Byte.MaxValue - 1;
                            break;

                        }

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.BadRequest)
                        {

                            if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                                HTTPResponse.HTTPBody?.Length > 0)
                            {

                                // HTTP/1.1 400
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

                                if (ValidationErrorList.TryParse(JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String() ?? ""),
                                                                 out var validationErrors,
                                                                 out var errorResponse))
                                {

                                    result = OICPResult<Acknowledgement<PushEVSEPricingRequest>>.BadRequest(
                                                 Request,
                                                 validationErrors,
                                                 processId
                                             );

                                }

                            }

                            break;

                        }

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                        {

                            // HTTP/1.1 403 Forbidden
                            // Server:          nginx/1.18.0 (Ubuntu)
                            // Date:            Thu, 15 Apr 2021 22:47:22 GMT
                            // Content-Type:    text/html
                            // Content-Length:  162
                            // Connection:      keep-alive
                            // 
                            // <html>
                            // <head><title>403 Forbidden</title></head>
                            // <body>
                            // <center><h1>403 Forbidden</h1></center>
                            // <hr><center>nginx/1.18.0 (Ubuntu)</center>
                            // </body>
                            // </html>

                            statusDescription = "Hubject firewall problem!";
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

                            statusDescription = "Operator/provider identification is not linked to the TLS client certificate!";

                            if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                                HTTPResponse.HTTPBody?.Length > 0)
                            {

                                try
                                {

                                    var json = JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String());

                                    if (json is not null &&
                                        json["StatusCode"] is JObject JSONObject &&
                                        StatusCode.TryParse(JSONObject,
                                                            out StatusCode? statusCode,
                                                            out String? ErrorResponse,
                                                            CustomStatusCodeParser))
                                    {

                                        result = OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Failed(
                                                     Request,
                                                     new Acknowledgement<PushEVSEPricingRequest>(
                                                         HTTPResponse.Timestamp,
                                                         HTTPResponse.EventTrackingId,
                                                         processId,
                                                         HTTPResponse.Runtime,
                                                         statusCode!,
                                                         Request
                                                     ),
                                                     processId
                                                 );

                                    }

                                }
                                catch (Exception e)
                                {

                                    result = OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<PushEVSEPricingRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     processId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     Request,
                                                     HTTPResponse,
                                                     false
                                                 )
                                             );

                                }

                            }

                            break;

                        }

                        if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                        {

                            // HTTP/1.1 404 NotFound
                            // Server:          nginx/1.18.0 (Ubuntu)
                            // Date:            Wed, 03 Mar 2021 01:00:15 GMT
                            // Content-Type:    application/json;charset=UTF-8
                            // Content-Length:  85
                            // Connection:      keep-alive
                            // Process-ID:      7bb86bc9-659f-4e57-8136-a7eb9ebc9c1d
                            // 
                            // {
                            //     "StatusCode": {
                            //         "Code":            "300",
                            //         "Description":     "Partner not found",
                            //         "AdditionalInfo":   null
                            //     }
                            // }

                            if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                                HTTPResponse.HTTPBody?.Length > 0)
                            {

                                try
                                {

                                    var json = JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String());

                                    if (json is not null &&
                                        json["StatusCode"] is JObject JSONObject &&
                                        StatusCode.TryParse(JSONObject,
                                                            out StatusCode? statusCode,
                                                            out String? ErrorResponse,
                                                            CustomStatusCodeParser))
                                    {

                                        result = OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Failed(
                                                     Request,
                                                     new Acknowledgement<PushEVSEPricingRequest>(
                                                         HTTPResponse.Timestamp,
                                                         HTTPResponse.EventTrackingId,
                                                         processId,
                                                         HTTPResponse.Runtime,
                                                         statusCode!,
                                                         Request
                                                     ),
                                                     processId
                                                 );

                                    }

                                }
                                catch (Exception e)
                                {

                                    result = OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<PushEVSEPricingRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     processId,
                                                     HTTPResponse.Runtime,
                                                     new StatusCode(
                                                         StatusCodes.SystemError,
                                                         e.Message,
                                                         e.StackTrace),
                                                     Request,
                                                     HTTPResponse,
                                                     false
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

                    result = OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Failed(
                                 Request,
                                 new Acknowledgement<PushEVSEPricingRequest>(
                                     Timestamp.Now,
                                     Request.EventTrackingId ?? EventTracking_Id.New,
                                     Process_Id.NewRandom(),
                                     Timestamp.Now - Request.Timestamp,
                                     new StatusCode(
                                         StatusCodes.SystemError,
                                         e.Message,
                                         e.StackTrace
                                     ),
                                     Request,
                                     null,
                                     false
                                 )
                             );

                }

                result ??= OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Failed(
                               Request,
                               new Acknowledgement<PushEVSEPricingRequest>(
                                   Timestamp.Now,
                                   Request.EventTrackingId ?? EventTracking_Id.New,
                                   Process_Id.NewRandom(),
                                   Timestamp.Now - Request.Timestamp,
                                   new StatusCode(
                                       StatusCodes.SystemError,
                                       statusDescription ?? "HTTP request failed!"
                                   ),
                                   Request,
                                   null,
                                   false
                               )
                           );

                if (result.IsNotSuccessful)
                    Counters.PushEVSEPricing.IncResponses_Error();

            }


            #region Send OnPushEVSEPricingResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnPushEVSEPricingResponse is not null)
                    await Task.WhenAll(OnPushEVSEPricingResponse.GetInvocationList().
                                       Cast<OnPushEVSEPricingResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnPushEVSEPricingResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region PullAuthenticationData          (Request)  [Obsolete!]

        /// <summary>
        /// Download provider authentication data.
        /// </summary>
        /// <param name="Request">A PullAuthenticationData request.</param>
        [Obsolete("PullAuthenticationData was removed from OICP.")]
        public async Task<OICPResult<PullAuthenticationDataResponse>>

            PullAuthenticationData(PullAuthenticationDataRequest Request)

        {

            #region Initial checks

            //Request = _CustomPullAuthenticationDataRequestMapper(Request);

            Byte                                         TransmissionRetry   = 0;
            OICPResult<PullAuthenticationDataResponse>?  result              = null;

            #endregion

            #region Send OnPullAuthenticationDataRequest event

            var startTime = Timestamp.Now;

            Counters.PullAuthenticationData.IncRequests_OK();

            try
            {

                if (OnPullAuthenticationDataRequest is not null)
                    await Task.WhenAll(OnPullAuthenticationDataRequest.GetInvocationList().
                                       Cast<OnPullAuthenticationDataRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnPullAuthenticationDataRequest));
            }

            #endregion


            var statusDescription = "HTTP request failed!";

            try
            {

                do
                {

                    #region Upstream HTTP request...

                    var HTTPResponse = await HTTPClientFactory.Create(RemoteURL,
                                                                      VirtualHostname,
                                                                      Description,
                                                                      PreferIPv4,
                                                                      RemoteCertificateValidator,
                                                                      LocalCertificateSelector,
                                                                      ClientCert,
                                                                      TLSProtocol,
                                                                      HTTPUserAgent,
                                                                      HTTPAuthentication,
                                                                      RequestTimeout,
                                                                      TransmissionRetryDelay,
                                                                      MaxNumberOfRetries,
                                                                      InternalBufferSize,
                                                                      UseHTTPPipelining,
                                                                      DisableLogging,
                                                                      null,
                                                                      DNSClient).

                                                Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/authdata/v21/operators/" + Request.OperatorId.ToString().Replace("*", "%2A") + "/pull-request"),
                                                                                     requestbuilder => {
                                                                                         requestbuilder.Accept?.Add(HTTPContentType.Application.JSON_UTF8);
                                                                                         requestbuilder.ContentType  = HTTPContentType.Application.JSON_UTF8;
                                                                                         requestbuilder.Content      = Request.ToJSON(CustomPullAuthenticationDataRequestSerializer).
                                                                                                                               ToString(JSONFormatting).
                                                                                                                               ToUTF8Bytes();
                                                                                         requestbuilder.Connection   = "close";
                                                                                     }),

                                                        RequestLogDelegate:   OnPullAuthenticationDataHTTPRequest,
                                                        ResponseLogDelegate:  OnPullAuthenticationDataHTTPResponse,
                                                        CancellationToken:    Request.CancellationToken,
                                                        EventTrackingId:      Request.EventTrackingId,
                                                        RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                                ConfigureAwait(false);

                    #endregion


                    var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse) ?? Process_Id.NewRandom();

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

                                // HTTP/1.1 200 OK
                                // Server:            nginx/1.18.0
                                // Date:              Sat, 09 Jan 2021 06:53:50 GMT
                                // Content-Type:      application/json;charset=utf-8
                                // Transfer-Encoding: chunked
                                // Connection:        keep-alive
                                // Process-ID:        d8d4583c-ff9b-44dd-bc92-b341f15f644e
                                // cd .
                                // {
                                //     "Result":               true,
                                //     "StatusCode": {
                                //         "Code":             "000",
                                //         "Description":      null,
                                //         "AdditionalInfo":   null
                                //     },
                                //     "SessionID":            null,
                                //     "CPOPartnerSessionID":  null,
                                //     "EMPPartnerSessionID":  null
                                // }

                                if (PullAuthenticationDataResponse.TryParse(Request,
                                                                            JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String() ?? ""),
                                                                            HTTPResponse.Timestamp,
                                                                            HTTPResponse.EventTrackingId,
                                                                            HTTPResponse.Runtime,
                                                                            out var pullAuthenticationDataResponse,
                                                                            out var ErrorResponse,
                                                                            processId,
                                                                            HTTPResponse,
                                                                            CustomPullAuthenticationDataResponseParser))
                                {

                                    Counters.PullAuthenticationData.IncResponses_OK();

                                    result = OICPResult<PullAuthenticationDataResponse>.Success(
                                                 Request,
                                                 pullAuthenticationDataResponse!,
                                                 processId
                                             );

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<PullAuthenticationDataResponse>.Failed(
                                             Request,
                                             new PullAuthenticationDataResponse(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 [],
                                                 Request,
                                                 StatusCode: new StatusCode(
                                                                 StatusCodes.SystemError,
                                                                 e.Message,
                                                                 e.StackTrace)
                                                             )
                                         );

                            }

                        }

                        TransmissionRetry = Byte.MaxValue - 1;
                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.BadRequest)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
                        {

                            // HTTP/1.1 400
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

                            if (ValidationErrorList.TryParse(JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String() ?? ""),
                                                             out var validationErrors,
                                                             out var errorResponse))
                            {

                                result = OICPResult<PullAuthenticationDataResponse>.BadRequest(
                                             Request,
                                             validationErrors,
                                             processId
                                         );

                            }

                        }

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                    {

                        // HTTP/1.1 403
                        // Server:          nginx/1.18.0 (Ubuntu)
                        // Date:            Sat, 06 Aug 2022 07:30:58 GMT
                        // Content-Type:    application/json;charset=ISO-8859-1
                        // Content-Length:  96
                        // Connection:      close
                        // Process-ID:      14673a14-b4fd-4ecc-93b2-422a9b9745de
                        // 
                        // {
                        //   "StatusCode": {
                        //     "Code":           "210",
                        //     "Description":    "No active subscription found",
                        //     "AdditionalInfo":  null
                        //   }
                        // }

                        // HTTP/1.1 403 Forbidden
                        // Server:          nginx/1.18.0 (Ubuntu)
                        // Date:            Thu, 15 Apr 2021 22:47:22 GMT
                        // Content-Type:    text/html
                        // Content-Length:  162
                        // Connection:      keep-alive
                        // 
                        // <html>
                        // <head><title>403 Forbidden</title></head>
                        // <body>
                        // <center><h1>403 Forbidden</h1></center>
                        // <hr><center>nginx/1.18.0 (Ubuntu)</center>
                        // </body>
                        // </html>

                        statusDescription = "Hubject firewall problem!";

                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

                                var json = JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String());

                                if (json is not null &&
                                    json["StatusCode"] is JObject JSONObject &&
                                    StatusCode.TryParse(JSONObject,
                                                        out StatusCode? statusCode,
                                                        out String? ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<PullAuthenticationDataResponse>.Failed(
                                                 Request,
                                                 new PullAuthenticationDataResponse(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     processId,
                                                     HTTPResponse.Runtime,
                                                     [],
                                                     Request,
                                                     StatusCode: statusCode
                                                 ),
                                                 processId
                                             );

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<PullAuthenticationDataResponse>.Failed(
                                             Request,
                                             new PullAuthenticationDataResponse(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 [],
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

                        statusDescription = "Operator/provider identification is not linked to the TLS client certificate!";

                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

                                var json = JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String());

                                if (json is not null &&
                                    json["StatusCode"] is JObject JSONObject &&
                                    StatusCode.TryParse(JSONObject,
                                                        out StatusCode? statusCode,
                                                        out String? ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<PullAuthenticationDataResponse>.Failed(
                                                 Request,
                                                 new PullAuthenticationDataResponse(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     processId,
                                                     HTTPResponse.Runtime,
                                                     [],
                                                     Request,
                                                     StatusCode: statusCode
                                                 ),
                                                 processId
                                             );

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<PullAuthenticationDataResponse>.Failed(
                                             Request,
                                             new PullAuthenticationDataResponse(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 [],
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

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                    {

                        // HTTP/1.1 404 NotFound
                        // Server:          nginx/1.18.0 (Ubuntu)
                        // Date:            Wed, 03 Mar 2021 01:00:15 GMT
                        // Content-Type:    application/json;charset=UTF-8
                        // Content-Length:  85
                        // Connection:      keep-alive
                        // Process-ID:      7bb86bc9-659f-4e57-8136-a7eb9ebc9c1d
                        // 
                        // {
                        //     "StatusCode": {
                        //         "Code":            "300",
                        //         "Description":     "Partner not found",
                        //         "AdditionalInfo":   null
                        //     }
                        // }

                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

                                var json = JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String());

                                if (json is not null &&
                                    json["StatusCode"] is JObject JSONObject &&
                                    StatusCode.TryParse(JSONObject,
                                                        out StatusCode? statusCode,
                                                        out String? ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<PullAuthenticationDataResponse>.Failed(
                                                 Request,
                                                 new PullAuthenticationDataResponse(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     processId,
                                                     HTTPResponse.Runtime,
                                                     [],
                                                     Request,
                                                     StatusCode: statusCode
                                                 ),
                                                 processId
                                             );

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<PullAuthenticationDataResponse>.Failed(
                                             Request,
                                             new PullAuthenticationDataResponse(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 [],
                                                 Request,
                                                 StatusCode: new StatusCode(
                                                                 StatusCodes.SystemError,
                                                                 e.Message,
                                                                 e.StackTrace)
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

                result = OICPResult<PullAuthenticationDataResponse>.Failed(
                             Request,
                             new PullAuthenticationDataResponse(
                                 Timestamp.Now,
                                 Request.EventTrackingId ?? EventTracking_Id.New,
                                 Process_Id.NewRandom(),
                                 Timestamp.Now - Request.Timestamp,
                                 [],
                                 Request,
                                 StatusCode: new StatusCode(
                                                 StatusCodes.SystemError,
                                                 e.Message,
                                                 e.StackTrace
                                             )
                             )
                         );

            }

            result ??= OICPResult<PullAuthenticationDataResponse>.Failed(
                            Request,
                            new PullAuthenticationDataResponse(
                                Timestamp.Now,
                                Request.EventTrackingId ?? EventTracking_Id.New,
                                Process_Id.NewRandom(),
                                Timestamp.Now - Request.Timestamp,
                                [],
                                Request,
                                StatusCode: new StatusCode(
                                                StatusCodes.SystemError,
                                                statusDescription ?? "HTTP request failed!"
                                            )
                            )
                        );

            if (result.IsNotSuccessful)
                Counters.PullAuthenticationData.IncResponses_Error();


            #region Send OnPullAuthenticationDataResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnPullAuthenticationDataResponse is not null)
                    await Task.WhenAll(OnPullAuthenticationDataResponse.GetInvocationList().
                                       Cast<OnPullAuthenticationDataResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnPullAuthenticationDataResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region AuthorizeStart                  (Request)

        /// <summary>
        /// Authorize for starting a charging session.
        /// </summary>
        /// <param name="Request">An AuthorizeStart request.</param>
        public async Task<OICPResult<AuthorizationStartResponse>>

            AuthorizeStart(AuthorizeStartRequest Request)

        {

            #region Initial checks

            //Request = _CustomAuthorizeStartRequestMapper(Request);

            Byte                                     TransmissionRetry   = 0;
            OICPResult<AuthorizationStartResponse>?  result              = null;

            #endregion

            #region Send OnAuthorizeStartRequest event

            var startTime = Timestamp.Now;

            Counters.AuthorizeStart.IncRequests_OK();

            try
            {

                if (OnAuthorizeStartRequest is not null)
                    await Task.WhenAll(OnAuthorizeStartRequest.GetInvocationList().
                                       Cast<OnAuthorizeStartRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnAuthorizeStartRequest));
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
                                                                      PreferIPv4,
                                                                      RemoteCertificateValidator,
                                                                      LocalCertificateSelector,
                                                                      ClientCert,
                                                                      TLSProtocol,
                                                                      HTTPUserAgent,
                                                                      HTTPAuthentication,
                                                                      RequestTimeout,
                                                                      TransmissionRetryDelay,
                                                                      MaxNumberOfRetries,
                                                                      InternalBufferSize,
                                                                      UseHTTPPipelining,
                                                                      DisableLogging,
                                                                      null,
                                                                      DNSClient).

                                              Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/charging/v21/operators/" + Request.OperatorId.ToString().Replace("*", "%2A") + "/authorize/start"),
                                                                                   requestbuilder => {
                                                                                       requestbuilder.Accept?.Add(HTTPContentType.Application.JSON_UTF8);
                                                                                       requestbuilder.ContentType  = HTTPContentType.Application.JSON_UTF8;
                                                                                       requestbuilder.Content      = Request.ToJSON(CustomAuthorizeStartRequestSerializer,
                                                                                                                                    CustomIdentificationSerializer).
                                                                                                                             ToString(JSONFormatting).
                                                                                                                             ToUTF8Bytes();
                                                                                       requestbuilder.Connection   = "close";
                                                                                   }),

                                                      RequestLogDelegate:   OnAuthorizeStartHTTPRequest,
                                                      ResponseLogDelegate:  OnAuthorizeStartHTTPResponse,
                                                      CancellationToken:    Request.CancellationToken,
                                                      EventTrackingId:      Request.EventTrackingId,
                                                      RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                              ConfigureAwait(false);

                    #endregion


                    var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse) ?? Process_Id.NewRandom();

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

                                // HTTP/1.1 200
                                // Server:             nginx/1.18.0 (Ubuntu)
                                // Date:               Tue, 02 Mar 2021 21:58:48 GMT
                                // Content-Type:       application/json;charset=utf-8
                                // Transfer-Encoding:  chunked
                                // Connection:         keep-alive
                                // Process-ID:         64d20013-dce6-4039-bbaf-d56651ec597f
                                // 
                                // {
                                //     "SessionID":                          null,
                                //     "CPOPartnerSessionID":                "73bbf19b-c468-470b-b186-e79f10a9e950",
                                //     "EMPPartnerSessionID":                null,
                                //     "ProviderID":                         null,
                                //     "AuthorizationStatus":                "NotAuthorized",
                                //     "StatusCode": {
                                //         "Code":                               "210",
                                //         "Description":                        null,
                                //         "AdditionalInfo":                     null
                                //     },
                                //     "AuthorizationStopIdentifications":   null
                                // }

                                // {
                                //     "SessionID":            "8dd819d6-82e8-492c-afc9-8e5cdac35e5a",
                                //     "CPOPartnerSessionID":  "7e2d0869-2ed3-4d7b-9b0b-73bc37bd0e02",
                                //     "EMPPartnerSessionID":  "842bd7b3-bd3f-41ef-bfef-0c2b2cf81cba",
                                //     "ProviderID":           "DE-XXX",
                                //     "AuthorizationStatus":  "Authorized",
                                //     "StatusCode": {
                                //         "Code":                  "000",
                                //         "Description":           "Nice to meet you!",
                                //         "AdditionalInfo":        "Happy charging!"
                                //     },
                                //     "AuthorizationStopIdentifications": [{
                                //         "RFIDMifareFamilyIdentification": {
                                //             "UID": "99887766"
                                //         }
                                //     }, {
                                //         "RFIDMifareFamilyIdentification": {
                                //             "UID": "77665544"
                                //         }
                                //     }]
                                // }

                                // 210 (No valid contract)                         => No valid contact with any EMP!
                                // 102 (RFID Authentication failed – invalid UID)  => No positive authorization from any EMP!

                                if (AuthorizationStartResponse.TryParse(Request,
                                                                        JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String()),
                                                                        out var authorizationStartResponse,
                                                                        out var ErrorResponse,
                                                                        HTTPResponse.Timestamp,
                                                                        HTTPResponse.EventTrackingId,
                                                                        HTTPResponse.Runtime,
                                                                        processId,
                                                                        HTTPResponse,
                                                                        CustomAuthorizationStartResponseParser))
                                {

                                    Counters.AuthorizeStart.IncResponses_OK();

                                    result = OICPResult<AuthorizationStartResponse>.Success(
                                                 Request,
                                                 authorizationStartResponse!,
                                                 processId
                                             );

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<AuthorizationStartResponse>.Failed(
                                             Request,
                                             AuthorizationStartResponse.NotAuthorized(
                                                 Request,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 )
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

                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
                        {

                            // HTTP/1.1 400
                            // Server:             nginx/1.18.0
                            // Date:               Fri, 08 Jan 2021 14:19:25 GMT
                            // Content-Type:       application/json;charset=utf-8
                            // Transfer-Encoding:  chunked
                            // Connection:         keep-alive
                            // Process-ID:         b87fd67b-2d74-4318-86cf-0d2c2c50cabb
                            // 
                            // {
                            //     "message": "Error parsing/validating JSON.",
                            //     "validationErrors": [
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].hotlinePhoneNumber",
                            //             "errorMessage":    "must match \"^\\+[0-9]{5,15}$\""
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].geoCoordinates",
                            //             "errorMessage":    "may not be null"
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].chargingStationNames",
                            //             "errorMessage":    "may not be empty"
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].plugs",
                            //             "errorMessage":    "may not be empty"
                            //         }
                            //     ]
                            // }

                            if (ValidationErrorList.TryParse(JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String() ?? ""),
                                                             out var validationErrors,
                                                             out var errorResponse))
                            {

                                result = OICPResult<AuthorizationStartResponse>.BadRequest(
                                             Request,
                                             validationErrors,
                                             processId
                                         );

                            }

                        }

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                    {

                        // Hubject firewall problem!
                        // Only HTML response!
                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Unauthorized)
                    {

                        // HTTP/1.1 401
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

                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

                                var json = JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String());

                                if (json is not null &&
                                    json["StatusCode"] is JObject JSONObject &&
                                    StatusCode.TryParse(JSONObject,
                                                        out var statusCode,
                                                        out var errorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<AuthorizationStartResponse>.Failed(
                                                 Request,
                                                 AuthorizationStartResponse.NotAuthorized(
                                                     Request,
                                                     statusCode!,
                                                     ProcessId: processId
                                                 ),
                                                 processId
                                             );

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<AuthorizationStartResponse>.Failed(
                                             Request,
                                             AuthorizationStartResponse.NotAuthorized(
                                                 Request,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 ),
                                                 ProcessId: processId
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

                result = OICPResult<AuthorizationStartResponse>.Failed(
                             Request,
                             AuthorizationStartResponse.NotAuthorized(
                                 Request,
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     e.Message,
                                     e.StackTrace
                                 )
                             )
                         );

            }

            result ??= OICPResult<AuthorizationStartResponse>.Failed(
                           Request,
                           AuthorizationStartResponse.NotAuthorized(
                               Request,
                               new StatusCode(
                                   StatusCodes.SystemError,
                                   "HTTP request failed!"
                               )
                           )
                       );

            if (result.IsNotSuccessful)
                Counters.AuthorizeStart.IncResponses_Error();


            #region Send OnAuthorizeStartResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnAuthorizeStartResponse is not null)
                    await Task.WhenAll(OnAuthorizeStartResponse.GetInvocationList().
                                       Cast<OnAuthorizeStartResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnAuthorizeStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop                   (Request)

        /// <summary>
        /// Authorize for stopping a charging session.
        /// </summary>
        /// <param name="Request">An AuthorizeStop request.</param>
        public async Task<OICPResult<AuthorizationStopResponse>>

            AuthorizeStop(AuthorizeStopRequest Request)

        {

            #region Initial checks

            //Request = _CustomAuthorizeStopRequestMapper(Request);

            Byte                                    TransmissionRetry   = 0;
            OICPResult<AuthorizationStopResponse>?  result              = null;

            #endregion

            #region Send OnAuthorizeStopRequest event

            var startTime = Timestamp.Now;

            Counters.AuthorizeStop.IncRequests_OK();

            try
            {

                if (OnAuthorizeStopRequest is not null)
                    await Task.WhenAll(OnAuthorizeStopRequest.GetInvocationList().
                                       Cast<OnAuthorizeStopRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnAuthorizeStopRequest));
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
                                                                      PreferIPv4,
                                                                      RemoteCertificateValidator,
                                                                      LocalCertificateSelector,
                                                                      ClientCert,
                                                                      TLSProtocol,
                                                                      HTTPUserAgent,
                                                                      HTTPAuthentication,
                                                                      RequestTimeout,
                                                                      TransmissionRetryDelay,
                                                                      MaxNumberOfRetries,
                                                                      InternalBufferSize,
                                                                      UseHTTPPipelining,
                                                                      DisableLogging,
                                                                      null,
                                                                      DNSClient).

                                              Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/charging/v21/operators/" + Request.OperatorId.ToString().Replace("*", "%2A") + "/authorize/stop"),
                                                                                   requestbuilder => {
                                                                                       requestbuilder.Accept?.Add(HTTPContentType.Application.JSON_UTF8);
                                                                                       requestbuilder.ContentType  = HTTPContentType.Application.JSON_UTF8;
                                                                                       requestbuilder.Content      = Request.ToJSON(CustomAuthorizeStopRequestSerializer,
                                                                                                                                    CustomIdentificationSerializer).
                                                                                                                             ToString(JSONFormatting).
                                                                                                                             ToUTF8Bytes();
                                                                                       requestbuilder.Connection   = "close";
                                                                                   }),

                                                      RequestLogDelegate:   OnAuthorizeStopHTTPRequest,
                                                      ResponseLogDelegate:  OnAuthorizeStopHTTPResponse,
                                                      CancellationToken:    Request.CancellationToken,
                                                      EventTrackingId:      Request.EventTrackingId,
                                                      RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                              ConfigureAwait(false);

                    #endregion


                    var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse) ?? Process_Id.NewRandom();

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

                                if (AuthorizationStopResponse.TryParse(Request,
                                                                       JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String()),
                                                                       out var authorizationStopResponse,
                                                                       out var ErrorResponse,
                                                                       HTTPResponse.Timestamp,
                                                                       HTTPResponse.EventTrackingId,
                                                                       HTTPResponse.Runtime,
                                                                       processId,
                                                                       HTTPResponse,
                                                                       CustomAuthorizationStopResponseParser))
                                {

                                    Counters.AuthorizeStop.IncResponses_OK();

                                    result = OICPResult<AuthorizationStopResponse>.Success(
                                                 Request,
                                                 authorizationStopResponse!,
                                                 processId
                                             );

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<AuthorizationStopResponse>.Failed(
                                             Request,
                                             AuthorizationStopResponse.NotAuthorized(
                                                 Request,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 ),
                                                 ProcessId: processId
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

                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
                        {

                            // HTTP/1.1 400
                            // Server:             nginx/1.18.0
                            // Date:               Fri, 08 Jan 2021 14:19:25 GMT
                            // Content-Type:       application/json;charset=utf-8
                            // Transfer-Encoding:  chunked
                            // Connection:         keep-alive
                            // Process-ID:         b87fd67b-2d74-4318-86cf-0d2c2c50cabb
                            // 
                            // {
                            //     "message": "Error parsing/validating JSON.",
                            //     "validationErrors": [
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].hotlinePhoneNumber",
                            //             "errorMessage":    "must match \"^\\+[0-9]{5,15}$\""
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].geoCoordinates",
                            //             "errorMessage":    "may not be null"
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].chargingStationNames",
                            //             "errorMessage":    "may not be empty"
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].plugs",
                            //             "errorMessage":    "may not be empty"
                            //         }
                            //     ]
                            // }

                            if (ValidationErrorList.TryParse(JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String() ?? ""),
                                                             out var validationErrors,
                                                             out var errorResponse))
                            {

                                result = OICPResult<AuthorizationStopResponse>.BadRequest(
                                             Request,
                                             validationErrors,
                                             processId
                                         );

                            }

                        }

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                    {

                        // Hubject firewall problem!
                        // Only HTML response!
                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Unauthorized)
                    {

                        // HTTP/1.1 401
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

                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

                                var json = JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String());

                                if (json is not null &&
                                    json["StatusCode"] is JObject JSONObject &&
                                    StatusCode.TryParse(JSONObject,
                                                        out StatusCode? statusCode,
                                                        out String?     ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<AuthorizationStopResponse>.Failed(
                                                 Request,
                                                 AuthorizationStopResponse.NotAuthorized(
                                                     Request,
                                                     statusCode!,
                                                     ProcessId: processId
                                                 ),
                                                 processId
                                             );

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<AuthorizationStopResponse>.Failed(
                                             Request,
                                             AuthorizationStopResponse.NotAuthorized(
                                                 Request,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 ),
                                                 ProcessId: processId
                                             )
                                         );

                            }

                        }

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                    {

                        // HTTP/1.1 401
                        // Server:          nginx/1.18.0 (Ubuntu)
                        // Date:            Tue, 02 Mar 2021 23:09:35 GMT
                        // Content-Type:    application/json;charset=UTF-8
                        // Content-Length:  87
                        // Connection:      keep-alive
                        // Process-ID:      cefd3dfc-8807-4160-8913-d3153dfea8ab
                        // 
                        // {
                        //     "StatusCode": {
                        //         "Code":            "400",
                        //         "Description":     "Session is not valid",
                        //         "AdditionalInfo":   null
                        //     }
                        // }

                        // Operator/provider identification is not linked to the TLS client certificate!

                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

                                var json = JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String());

                                if (json is not null &&
                                    json["StatusCode"] is JObject JSONObject &&
                                    StatusCode.TryParse(JSONObject,
                                                        out StatusCode? statusCode,
                                                        out String?     ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<AuthorizationStopResponse>.Failed(
                                                 Request,
                                                 AuthorizationStopResponse.NotAuthorized(
                                                     Request,
                                                     statusCode!,
                                                     ProcessId: processId
                                                 ),
                                                 processId
                                             );

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<AuthorizationStopResponse>.Failed(
                                             Request,
                                             AuthorizationStopResponse.NotAuthorized(
                                                 Request,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace
                                                 ),
                                                 ProcessId: processId
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

                result = OICPResult<AuthorizationStopResponse>.Failed(
                             Request,
                             AuthorizationStopResponse.NotAuthorized(
                                 Request,
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     e.Message,
                                     e.StackTrace
                                 )
                             )
                         );

            }

            result ??= OICPResult<AuthorizationStopResponse>.Failed(
                           Request,
                           AuthorizationStopResponse.NotAuthorized(
                               Request,
                               new StatusCode(
                                   StatusCodes.SystemError,
                                   "HTTP request failed!"
                               )
                           )
                       );

            if (result.IsNotSuccessful)
                Counters.AuthorizeStop.IncResponses_Error();


            #region Send OnAuthorizeStopResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnAuthorizeStopResponse is not null)
                    await Task.WhenAll(OnAuthorizeStopResponse.GetInvocationList().
                                       Cast<OnAuthorizeStopResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnAuthorizeStopResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region SendChargingStartNotification   (Request)

        /// <summary>
        /// Send a charging start notification.
        /// </summary>
        /// <param name="Request">A ChargingStartNotification request.</param>
        public async Task<OICPResult<Acknowledgement<ChargingStartNotificationRequest>>>

            SendChargingStartNotification(ChargingStartNotificationRequest Request)

        {

            #region Initial checks

            //Request = _CustomSendChargingStartNotificationRequestMapper(Request);

            Byte                                                            TransmissionRetry   = 0;
            OICPResult<Acknowledgement<ChargingStartNotificationRequest>>?  result              = null;

            #endregion

            #region  OnChargingStartNotificationRequest event

            var startTime = Timestamp.Now;

            Counters.SendChargingStartNotification.IncRequests_OK();

            try
            {

                if (OnChargingStartNotificationRequest is not null)
                    await Task.WhenAll(OnChargingStartNotificationRequest.GetInvocationList().
                                       Cast<OnChargingStartNotificationRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnChargingStartNotificationRequest));
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
                                                                      PreferIPv4,
                                                                      RemoteCertificateValidator,
                                                                      LocalCertificateSelector,
                                                                      ClientCert,
                                                                      TLSProtocol,
                                                                      HTTPUserAgent,
                                                                      HTTPAuthentication,
                                                                      RequestTimeout,
                                                                      TransmissionRetryDelay,
                                                                      MaxNumberOfRetries,
                                                                      InternalBufferSize,
                                                                      UseHTTPPipelining,
                                                                      DisableLogging,
                                                                      null,
                                                                      DNSClient).

                                                 Execute(client => client.POSTRequest(RemoteURL.Path + "/api/oicp/notificationmgmt/v11/charging-notifications",
                                                                                      requestbuilder => {
                                                                                          requestbuilder.Accept?.Add(HTTPContentType.Application.JSON_UTF8);
                                                                                          requestbuilder.ContentType  = HTTPContentType.Application.JSON_UTF8;
                                                                                          requestbuilder.Content      = Request.ToJSON(CustomChargingStartNotificationRequestSerializer,
                                                                                                                                       CustomIdentificationSerializer).
                                                                                                                                ToString(JSONFormatting).
                                                                                                                                ToUTF8Bytes();
                                                                                          requestbuilder.Connection   = "close";
                                                                                      }),

                                                         RequestLogDelegate:   OnChargingStartNotificationHTTPRequest,
                                                         ResponseLogDelegate:  OnChargingStartNotificationHTTPResponse,
                                                         CancellationToken:    Request.CancellationToken,
                                                         EventTrackingId:      Request.EventTrackingId,
                                                         RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                                 ConfigureAwait(false);

                    #endregion


                    var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse) ?? Process_Id.NewRandom();

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

                                // HTTP/1.1 200
                                // Server:             nginx/1.18.0 (Ubuntu)
                                // Date:               Tue, 02 Mar 2021 17:51:14 GMT
                                // Content-Type:       application/json;charset=utf-8
                                // Transfer-Encoding:  chunked
                                // Connection:         keep-alive
                                // Process-ID:         332c9d01-2ea4-4d15-9d4a-bb9f5abd097c
                                // 
                                // {
                                //     "Result":               true,
                                //     "StatusCode": {
                                //         "Code":             "000",
                                //         "Description":      null,
                                //         "AdditionalInfo":   null
                                //     },
                                //     "SessionID":            null,
                                //     "CPOPartnerSessionID":  null,
                                //     "EMPPartnerSessionID":  null
                                // }

                                if (Acknowledgement<ChargingStartNotificationRequest>.TryParse(Request,
                                                                                               JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String()),
                                                                                               out var acknowledgement,
                                                                                               out var ErrorResponse,
                                                                                               HTTPResponse,
                                                                                               HTTPResponse.Timestamp,
                                                                                               HTTPResponse.EventTrackingId,
                                                                                               HTTPResponse.Runtime,
                                                                                               processId,
                                                                                               CustomChargingStartNotificationAcknowledgementParser))
                                {

                                    Counters.SendChargingStartNotification.IncResponses_OK();

                                    result = OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Success(
                                                 Request,
                                                 acknowledgement!,
                                                 processId
                                             );

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<ChargingStartNotificationRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace),
                                                 Request,
                                                 HTTPResponse,
                                                 false
                                             )
                                         );

                            }

                        }

                        TransmissionRetry = Byte.MaxValue - 1;
                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.BadRequest)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
                        {

                            // HTTP/1.1 400
                            // Server:             nginx/1.18.0
                            // Date:               Fri, 08 Jan 2021 14:19:25 GMT
                            // Content-Type:       application/json;charset=utf-8
                            // Transfer-Encoding:  chunked
                            // Connection:         keep-alive
                            // Process-ID:         b87fd67b-2d74-4318-86cf-0d2c2c50cabb
                            // 
                            // {
                            //     "message": "Error parsing/validating JSON.",
                            //     "validationErrors": [
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].hotlinePhoneNumber",
                            //             "errorMessage":    "must match \"^\\+[0-9]{5,15}$\""
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].geoCoordinates",
                            //             "errorMessage":    "may not be null"
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].chargingStationNames",
                            //             "errorMessage":    "may not be empty"
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].plugs",
                            //             "errorMessage":    "may not be empty"
                            //         }
                            //     ]
                            // }

                            if (ValidationErrorList.TryParse(JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String() ?? ""),
                                                             out var validationErrors,
                                                             out var errorResponse))
                            {

                                result = OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.BadRequest(
                                             Request,
                                             validationErrors,
                                             processId
                                         );

                            }

                        }

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                    {

                        // Hubject firewall problem!
                        // Only HTML response!
                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Unauthorized)
                    {

                        // HTTP/1.1 401
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

                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

                                var json = JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String());

                                if (json is not null &&
                                    json["StatusCode"] is JObject JSONObject &&
                                    StatusCode.TryParse(JSONObject,
                                                        out StatusCode? statusCode,
                                                        out String?     ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<ChargingStartNotificationRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     processId,
                                                     HTTPResponse.Runtime,
                                                     statusCode!,
                                                     Request
                                                 ),
                                                 processId
                                             );

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<ChargingStartNotificationRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace),
                                                 Request,
                                                 HTTPResponse,
                                                 false
                                             )
                                         );

                            }

                        }

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                    {

                        // HTTP/1.1 404
                        // Server: nginx/1.18.0 (Ubuntu)
                        // Date: Wed, 03 Mar 2021 01:00:15 GMT
                        // Content-Type: application/json;charset=UTF-8
                        // Content-Length: 85
                        // Connection: keep-alive
                        // Process-ID: 7bb86bc9-659f-4e57-8136-a7eb9ebc9c1d
                        // 
                        // {
                        //     "StatusCode": {
                        //         "Code":            "300",
                        //         "Description":     "Partner not found",
                        //         "AdditionalInfo":   null
                        //     }
                        // }

                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

                                var json = JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String());

                                if (json is not null &&
                                    json["StatusCode"] is JObject JSONObject &&
                                    StatusCode.TryParse(JSONObject,
                                                        out StatusCode? statusCode,
                                                        out String?     ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<ChargingStartNotificationRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     processId,
                                                     HTTPResponse.Runtime,
                                                     statusCode!,
                                                     Request
                                                 ),
                                                 processId
                                             );

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Failed(
                                                Request,
                                                new Acknowledgement<ChargingStartNotificationRequest>(
                                                    HTTPResponse.Timestamp,
                                                    HTTPResponse.EventTrackingId,
                                                    processId,
                                                    HTTPResponse.Runtime,
                                                    new StatusCode(
                                                        StatusCodes.SystemError,
                                                        e.Message,
                                                        e.StackTrace),
                                                    Request,
                                                    HTTPResponse,
                                                    false
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

                result = OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Failed(
                             Request,
                             new Acknowledgement<ChargingStartNotificationRequest>(
                                 Timestamp.Now,
                                 Request.EventTrackingId ?? EventTracking_Id.New,
                                 Process_Id.NewRandom(),
                                 Timestamp.Now - Request.Timestamp,
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     e.Message,
                                     e.StackTrace
                                 ),
                                 Request,
                                 null,
                                 false
                             )
                         );

            }

            result ??= OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Failed(
                           Request,
                           new Acknowledgement<ChargingStartNotificationRequest>(
                               Timestamp.Now,
                               Request.EventTrackingId ?? EventTracking_Id.New,
                               Process_Id.NewRandom(),
                               Timestamp.Now - Request.Timestamp,
                               new StatusCode(
                                   StatusCodes.SystemError,
                                   "HTTP request failed!",
                                   null
                               ),
                               Request,
                               null,
                               false
                           )
                       );

            if (result.IsNotSuccessful)
                Counters.SendChargingStartNotification.IncResponses_Error();


            #region  OnChargingStartNotificationResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnChargingStartNotificationResponse is not null)
                    await Task.WhenAll(OnChargingStartNotificationResponse.GetInvocationList().
                                       Cast<OnChargingStartNotificationResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnChargingStartNotificationResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region SendChargingProgressNotification(Request)

        /// <summary>
        /// Send a charging progress notification.
        /// </summary>
        /// <param name="Request">A ChargingProgressNotification request.</param>
        public async Task<OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>>

            SendChargingProgressNotification(ChargingProgressNotificationRequest Request)

        {

            #region Initial checks

            //Request = _CustomSendChargingProgressNotificationRequestMapper(Request);

            Byte                                                               TransmissionRetry   = 0;
            OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>?  result              = null;

            #endregion

            #region  OnChargingProgressNotificationRequest event

            var startTime = Timestamp.Now;

            Counters.SendChargingProgressNotification.IncRequests_OK();

            try
            {

                if (OnChargingProgressNotificationRequest is not null)
                    await Task.WhenAll(OnChargingProgressNotificationRequest.GetInvocationList().
                                       Cast<OnChargingProgressNotificationRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnChargingProgressNotificationRequest));
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
                                                                      PreferIPv4,
                                                                      RemoteCertificateValidator,
                                                                      LocalCertificateSelector,
                                                                      ClientCert,
                                                                      TLSProtocol,
                                                                      HTTPUserAgent,
                                                                      HTTPAuthentication,
                                                                      RequestTimeout,
                                                                      TransmissionRetryDelay,
                                                                      MaxNumberOfRetries,
                                                                      InternalBufferSize,
                                                                      UseHTTPPipelining,
                                                                      DisableLogging,
                                                                      null,
                                                                      DNSClient).

                                                 Execute(client => client.POSTRequest(RemoteURL.Path + "/api/oicp/notificationmgmt/v11/charging-notifications",
                                                                                      requestbuilder => {
                                                                                          requestbuilder.Accept?.Add(HTTPContentType.Application.JSON_UTF8);
                                                                                          requestbuilder.ContentType  = HTTPContentType.Application.JSON_UTF8;
                                                                                          requestbuilder.Content      = Request.ToJSON(CustomChargingProgressNotificationRequestSerializer,
                                                                                                                                       CustomIdentificationSerializer).
                                                                                                                                ToString(JSONFormatting).
                                                                                                                                ToUTF8Bytes();
                                                                                          requestbuilder.Connection   = "close";
                                                                                      }),

                                                         RequestLogDelegate:   OnChargingProgressNotificationHTTPRequest,
                                                         ResponseLogDelegate:  OnChargingProgressNotificationHTTPResponse,
                                                         CancellationToken:    Request.CancellationToken,
                                                         EventTrackingId:      Request.EventTrackingId,
                                                         RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                                 ConfigureAwait(false);

                    #endregion


                    var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse) ?? Process_Id.NewRandom();

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

                                // HTTP/1.1 200
                                // Server:             nginx/1.18.0 (Ubuntu)
                                // Date:               Tue, 02 Mar 2021 17:51:14 GMT
                                // Content-Type:       application/json;charset=utf-8
                                // Transfer-Encoding:  chunked
                                // Connection:         keep-alive
                                // Process-ID:         332c9d01-2ea4-4d15-9d4a-bb9f5abd097c
                                // 
                                // {
                                //     "Result":               true,
                                //     "StatusCode": {
                                //         "Code":             "000",
                                //         "Description":      null,
                                //         "AdditionalInfo":   null
                                //     },
                                //     "SessionID":            null,
                                //     "CPOPartnerSessionID":  null,
                                //     "EMPPartnerSessionID":  null
                                // }

                                if (Acknowledgement<ChargingProgressNotificationRequest>.TryParse(Request,
                                                                                                  JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String()),
                                                                                                  out var acknowledgement,
                                                                                                  out var ErrorResponse,
                                                                                                  HTTPResponse,
                                                                                                  HTTPResponse.Timestamp,
                                                                                                  HTTPResponse.EventTrackingId,
                                                                                                  HTTPResponse.Runtime,
                                                                                                  processId,
                                                                                                  CustomChargingProgressNotificationAcknowledgementParser))
                                {

                                    Counters.SendChargingProgressNotification.IncResponses_OK();

                                    result = OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Success(
                                                 Request,
                                                 acknowledgement!,
                                                 processId
                                             );

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<ChargingProgressNotificationRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace),
                                                 Request,
                                                 HTTPResponse,
                                                 false
                                             )
                                         );

                            }

                        }

                        TransmissionRetry = Byte.MaxValue - 1;
                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.BadRequest)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
                        {

                            // HTTP/1.1 400
                            // Server:             nginx/1.18.0
                            // Date:               Fri, 08 Jan 2021 14:19:25 GMT
                            // Content-Type:       application/json;charset=utf-8
                            // Transfer-Encoding:  chunked
                            // Connection:         keep-alive
                            // Process-ID:         b87fd67b-2d74-4318-86cf-0d2c2c50cabb
                            // 
                            // {
                            //     "message": "Error parsing/validating JSON.",
                            //     "validationErrors": [
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].hotlinePhoneNumber",
                            //             "errorMessage":    "must match \"^\\+[0-9]{5,15}$\""
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].geoCoordinates",
                            //             "errorMessage":    "may not be null"
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].chargingStationNames",
                            //             "errorMessage":    "may not be empty"
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].plugs",
                            //             "errorMessage":    "may not be empty"
                            //         }
                            //     ]
                            // }

                            if (ValidationErrorList.TryParse(JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String() ?? ""),
                                                             out var validationErrors,
                                                             out var errorResponse))
                            {

                                result = OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.BadRequest(
                                             Request,
                                             validationErrors,
                                             processId
                                         );

                            }

                        }

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                    {

                        // Hubject firewall problem!
                        // Only HTML response!
                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Unauthorized)
                    {

                        // HTTP/1.1 401
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

                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

                                var json = JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String());

                                if (json is not null &&
                                    json["StatusCode"] is JObject JSONObject &&
                                    StatusCode.TryParse(JSONObject,
                                                        out StatusCode? statusCode,
                                                        out String?     ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<ChargingProgressNotificationRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     processId,
                                                     HTTPResponse.Runtime,
                                                     statusCode!,
                                                     Request
                                                 ),
                                                 processId
                                             );

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<ChargingProgressNotificationRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace),
                                                 Request,
                                                 HTTPResponse,
                                                 false
                                             )
                                         );

                            }

                        }

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                    {

                        // HTTP/1.1 404
                        // Server: nginx/1.18.0 (Ubuntu)
                        // Date: Wed, 03 Mar 2021 01:00:15 GMT
                        // Content-Type: application/json;charset=UTF-8
                        // Content-Length: 85
                        // Connection: keep-alive
                        // Process-ID: 7bb86bc9-659f-4e57-8136-a7eb9ebc9c1d
                        // 
                        // {
                        //     "StatusCode": {
                        //         "Code":            "300",
                        //         "Description":     "Partner not found",
                        //         "AdditionalInfo":   null
                        //     }
                        // }

                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

                                var json = JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String());

                                if (json is not null &&
                                    json["StatusCode"] is JObject JSONObject &&
                                    StatusCode.TryParse(JSONObject,
                                                        out StatusCode? statusCode,
                                                        out String?     ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<ChargingProgressNotificationRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     processId,
                                                     HTTPResponse.Runtime,
                                                     statusCode!,
                                                     Request
                                                 ),
                                                 processId
                                             );

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<ChargingProgressNotificationRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace),
                                                 Request,
                                                 HTTPResponse,
                                                 false
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

                result = OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Failed(
                             Request,
                             new Acknowledgement<ChargingProgressNotificationRequest>(
                                 Timestamp.Now,
                                 Request.EventTrackingId ?? EventTracking_Id.New,
                                 Process_Id.NewRandom(),
                                 Timestamp.Now - Request.Timestamp,
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     e.Message,
                                     e.StackTrace
                                 ),
                                 Request,
                                 null,
                                 false
                             )
                         );

            }

            result ??= OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Failed(
                           Request,
                           new Acknowledgement<ChargingProgressNotificationRequest>(
                               Timestamp.Now,
                               Request.EventTrackingId ?? EventTracking_Id.New,
                               Process_Id.NewRandom(),
                               Timestamp.Now - Request.Timestamp,
                               new StatusCode(
                                   StatusCodes.SystemError,
                                   "HTTP request failed!",
                                   null
                               ),
                               Request,
                               null,
                               false
                           )
                       );

            if (result.IsNotSuccessful)
                Counters.SendChargingProgressNotification.IncResponses_Error();


            #region  OnChargingProgressNotificationResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnChargingProgressNotificationResponse is not null)
                    await Task.WhenAll(OnChargingProgressNotificationResponse.GetInvocationList().
                                       Cast<OnChargingProgressNotificationResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnChargingProgressNotificationResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region SendChargingEndNotification     (Request)

        /// <summary>
        /// Send a charging start notification.
        /// </summary>
        /// <param name="Request">A ChargingEndNotification request.</param>
        public async Task<OICPResult<Acknowledgement<ChargingEndNotificationRequest>>>

            SendChargingEndNotification(ChargingEndNotificationRequest Request)

        {

            #region Initial checks

            //Request = _CustomSendChargingEndNotificationRequestMapper(Request);

            Byte                                                          TransmissionRetry   = 0;
            OICPResult<Acknowledgement<ChargingEndNotificationRequest>>?  result              = null;

            #endregion

            #region  OnChargingEndNotificationRequest event

            var startTime = Timestamp.Now;

            Counters.SendChargingEndNotification.IncRequests_OK();

            try
            {

                if (OnChargingEndNotificationRequest is not null)
                    await Task.WhenAll(OnChargingEndNotificationRequest.GetInvocationList().
                                       Cast<OnChargingEndNotificationRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnChargingEndNotificationRequest));
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
                                                                      PreferIPv4,
                                                                      RemoteCertificateValidator,
                                                                      LocalCertificateSelector,
                                                                      ClientCert,
                                                                      TLSProtocol,
                                                                      HTTPUserAgent,
                                                                      HTTPAuthentication,
                                                                      RequestTimeout,
                                                                      TransmissionRetryDelay,
                                                                      MaxNumberOfRetries,
                                                                      InternalBufferSize,
                                                                      UseHTTPPipelining,
                                                                      DisableLogging,
                                                                      null,
                                                                      DNSClient).

                                                 Execute(client => client.POSTRequest(RemoteURL.Path + "/api/oicp/notificationmgmt/v11/charging-notifications",
                                                                                      requestbuilder => {
                                                                                          requestbuilder.Accept?.Add(HTTPContentType.Application.JSON_UTF8);
                                                                                          requestbuilder.ContentType  = HTTPContentType.Application.JSON_UTF8;
                                                                                          requestbuilder.Content      = Request.ToJSON(CustomChargingEndNotificationRequestSerializer,
                                                                                                                                       CustomIdentificationSerializer).
                                                                                                                                ToString(JSONFormatting).
                                                                                                                                ToUTF8Bytes();
                                                                                          requestbuilder.Connection   = "close";
                                                                                      }),

                                                         RequestLogDelegate:   OnChargingEndNotificationHTTPRequest,
                                                         ResponseLogDelegate:  OnChargingEndNotificationHTTPResponse,
                                                         CancellationToken:    Request.CancellationToken,
                                                         EventTrackingId:      Request.EventTrackingId,
                                                         RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                                 ConfigureAwait(false);

                    #endregion


                    var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse) ?? Process_Id.NewRandom();

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

                                // HTTP/1.1 200 OK
                                // Server:             nginx/1.18.0 (Ubuntu)
                                // Date:               Tue, 02 Mar 2021 17:51:14 GMT
                                // Content-Type:       application/json;charset=utf-8
                                // Transfer-Encoding:  chunked
                                // Connection:         keep-alive
                                // Process-ID:         332c9d01-2ea4-4d15-9d4a-bb9f5abd097c
                                // 
                                // {
                                //     "Result":               true,
                                //     "StatusCode": {
                                //         "Code":             "000",
                                //         "Description":      null,
                                //         "AdditionalInfo":   null
                                //     },
                                //     "SessionID":            null,
                                //     "CPOPartnerSessionID":  null,
                                //     "EMPPartnerSessionID":  null
                                // }

                                if (Acknowledgement<ChargingEndNotificationRequest>.TryParse(Request,
                                                                                             JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String()),
                                                                                             out var acknowledgement,
                                                                                             out var ErrorResponse,
                                                                                             HTTPResponse,
                                                                                             HTTPResponse.Timestamp,
                                                                                             HTTPResponse.EventTrackingId,
                                                                                             HTTPResponse.Runtime,
                                                                                             processId,
                                                                                             CustomChargingEndNotificationAcknowledgementParser))
                                {

                                    Counters.SendChargingEndNotification.IncResponses_OK();

                                    result = OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Success(
                                                 Request,
                                                 acknowledgement!,
                                                 processId
                                             );

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<ChargingEndNotificationRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace),
                                                 Request,
                                                 HTTPResponse,
                                                 false
                                             )
                                         );

                            }

                        }

                        TransmissionRetry = Byte.MaxValue - 1;
                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.BadRequest)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
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
                            //     "message": "Error parsing/validating JSON.",
                            //     "validationErrors": [
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].hotlinePhoneNumber",
                            //             "errorMessage":    "must match \"^\\+[0-9]{5,15}$\""
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].geoCoordinates",
                            //             "errorMessage":    "may not be null"
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].chargingStationNames",
                            //             "errorMessage":    "may not be empty"
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].plugs",
                            //             "errorMessage":    "may not be empty"
                            //         }
                            //     ]
                            // }

                            if (ValidationErrorList.TryParse(JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String() ?? ""),
                                                             out var validationErrors,
                                                             out var errorResponse))
                            {

                                result = OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.BadRequest(
                                             Request,
                                             validationErrors,
                                             processId
                                         );

                            }

                        }

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

                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

                                var json = JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String());

                                if (json is not null &&
                                    json["StatusCode"] is JObject JSONObject &&
                                    StatusCode.TryParse(JSONObject,
                                                        out StatusCode? statusCode,
                                                        out String? ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<ChargingEndNotificationRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     processId,
                                                     HTTPResponse.Runtime,
                                                     statusCode!,
                                                     Request
                                                 ),
                                                 processId
                                             );

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<ChargingEndNotificationRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace),
                                                 Request,
                                                 HTTPResponse,
                                                 false
                                             )
                                         );

                            }

                        }

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                    {

                        // HTTP/1.1 404 NotFound
                        // Server: nginx/1.18.0 (Ubuntu)
                        // Date: Wed, 03 Mar 2021 01:00:15 GMT
                        // Content-Type: application/json;charset=UTF-8
                        // Content-Length: 85
                        // Connection: keep-alive
                        // Process-ID: 7bb86bc9-659f-4e57-8136-a7eb9ebc9c1d
                        // 
                        // {
                        //     "StatusCode": {
                        //         "Code":            "300",
                        //         "Description":     "Partner not found",
                        //         "AdditionalInfo":   null
                        //     }
                        // }

                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

                                var json = JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String());

                                if (json is not null &&
                                    json["StatusCode"] is JObject JSONObject &&
                                    StatusCode.TryParse(JSONObject,
                                                        out StatusCode? statusCode,
                                                        out String? ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<ChargingEndNotificationRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     processId,
                                                     HTTPResponse.Runtime,
                                                     statusCode!,
                                                     Request
                                                 ),
                                                 processId
                                             );

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<ChargingEndNotificationRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace),
                                                 Request,
                                                 HTTPResponse,
                                                 false
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

                result = OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Failed(
                             Request,
                             new Acknowledgement<ChargingEndNotificationRequest>(
                                 Timestamp.Now,
                                 Request.EventTrackingId ?? EventTracking_Id.New,
                                 Process_Id.NewRandom(),
                                 Timestamp.Now - Request.Timestamp,
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     e.Message,
                                     e.StackTrace
                                 ),
                                 Request,
                                 null,
                                 false
                             )
                         );

            }

            result ??= OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Failed(
                           Request,
                           new Acknowledgement<ChargingEndNotificationRequest>(
                               Timestamp.Now,
                               Request.EventTrackingId ?? EventTracking_Id.New,
                               Process_Id.NewRandom(),
                               Timestamp.Now - Request.Timestamp,
                               new StatusCode(
                                   StatusCodes.SystemError,
                                   "HTTP request failed!",
                                   null
                               ),
                               Request,
                               null,
                               false
                           )
                       );

            if (result.IsNotSuccessful)
                Counters.SendChargingEndNotification.IncResponses_Error();


            #region  OnChargingEndNotificationResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnChargingEndNotificationResponse is not null)
                    await Task.WhenAll(OnChargingEndNotificationResponse.GetInvocationList().
                                       Cast<OnChargingEndNotificationResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnChargingEndNotificationResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region SendChargingErrorNotification   (Request)

        /// <summary>
        /// Send a charging error notification.
        /// </summary>
        /// <param name="Request">A ChargingErrorNotification request.</param>
        public async Task<OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>>

            SendChargingErrorNotification(ChargingErrorNotificationRequest Request)

        {

            #region Initial checks

            //Request = _CustomSendChargingErrorNotificationRequestMapper(Request);

            Byte                                                            TransmissionRetry   = 0;
            OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>?  result              = null;

            #endregion

            #region  OnChargingErrorNotificationRequest event

            var startTime = Timestamp.Now;

            Counters.SendChargingErrorNotification.IncRequests_OK();

            try
            {

                if (OnChargingErrorNotificationRequest is not null)
                    await Task.WhenAll(OnChargingErrorNotificationRequest.GetInvocationList().
                                       Cast<OnChargingErrorNotificationRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnChargingErrorNotificationRequest));
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
                                                                      PreferIPv4,
                                                                      RemoteCertificateValidator,
                                                                      LocalCertificateSelector,
                                                                      ClientCert,
                                                                      TLSProtocol,
                                                                      HTTPUserAgent,
                                                                      HTTPAuthentication,
                                                                      RequestTimeout,
                                                                      TransmissionRetryDelay,
                                                                      MaxNumberOfRetries,
                                                                      InternalBufferSize,
                                                                      UseHTTPPipelining,
                                                                      DisableLogging,
                                                                      null,
                                                                      DNSClient).

                                                 Execute(client => client.POSTRequest(RemoteURL.Path + "/api/oicp/notificationmgmt/v11/charging-notifications",
                                                                                      requestbuilder => {
                                                                                          requestbuilder.Accept?.Add(HTTPContentType.Application.JSON_UTF8);
                                                                                          requestbuilder.ContentType  = HTTPContentType.Application.JSON_UTF8;
                                                                                          requestbuilder.Content      = Request.ToJSON(CustomChargingErrorNotificationRequestSerializer,
                                                                                                                                       CustomIdentificationSerializer).
                                                                                                                                ToString(JSONFormatting).
                                                                                                                                ToUTF8Bytes();
                                                                                          requestbuilder.Connection   = "close";
                                                                                      }),

                                                         RequestLogDelegate:   OnChargingErrorNotificationHTTPRequest,
                                                         ResponseLogDelegate:  OnChargingErrorNotificationHTTPResponse,
                                                         CancellationToken:    Request.CancellationToken,
                                                         EventTrackingId:      Request.EventTrackingId,
                                                         RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                                 ConfigureAwait(false);

                    #endregion


                    var processId = HTTPResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse) ?? Process_Id.NewRandom();

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

                                // HTTP/1.1 200 OK
                                // Server:             nginx/1.18.0 (Ubuntu)
                                // Date:               Tue, 02 Mar 2021 17:51:14 GMT
                                // Content-Type:       application/json;charset=utf-8
                                // Transfer-Encoding:  chunked
                                // Connection:         keep-alive
                                // Process-ID:         332c9d01-2ea4-4d15-9d4a-bb9f5abd097c
                                // 
                                // {
                                //     "Result":               true,
                                //     "StatusCode": {
                                //         "Code":             "000",
                                //         "Description":      null,
                                //         "AdditionalInfo":   null
                                //     },
                                //     "SessionID":            null,
                                //     "CPOPartnerSessionID":  null,
                                //     "EMPPartnerSessionID":  null
                                // }

                                if (Acknowledgement<ChargingErrorNotificationRequest>.TryParse(Request,
                                                                                               JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String()),
                                                                                               out var acknowledgement,
                                                                                               out var ErrorResponse,
                                                                                               HTTPResponse,
                                                                                               HTTPResponse.Timestamp,
                                                                                               HTTPResponse.EventTrackingId,
                                                                                               HTTPResponse.Runtime,
                                                                                               processId,
                                                                                               CustomChargingErrorNotificationAcknowledgementParser))
                                {

                                    Counters.SendChargingErrorNotification.IncResponses_OK();

                                    result = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Success(
                                                 Request,
                                                 acknowledgement!,
                                                 processId
                                             );

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<ChargingErrorNotificationRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace),
                                                 Request,
                                                 HTTPResponse,
                                                 false
                                             )
                                         );

                            }

                        }

                        TransmissionRetry = Byte.MaxValue - 1;
                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.BadRequest)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
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
                            //     "message": "Error parsing/validating JSON.",
                            //     "validationErrors": [
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].hotlinePhoneNumber",
                            //             "errorMessage":    "must match \"^\\+[0-9]{5,15}$\""
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].geoCoordinates",
                            //             "errorMessage":    "may not be null"
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].chargingStationNames",
                            //             "errorMessage":    "may not be empty"
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].plugs",
                            //             "errorMessage":    "may not be empty"
                            //         }
                            //     ]
                            // }

                            if (ValidationErrorList.TryParse(JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String() ?? ""),
                                                             out var validationErrors,
                                                             out var errorResponse))
                            {

                                result = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.BadRequest(
                                             Request,
                                             validationErrors,
                                             processId
                                         );

                            }

                        }

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

                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

                                var json = JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String());

                                if (json is not null &&
                                    json["StatusCode"] is JObject JSONObject &&
                                    StatusCode.TryParse(JSONObject,
                                                        out StatusCode? statusCode,
                                                        out String?     ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<ChargingErrorNotificationRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     processId,
                                                     HTTPResponse.Runtime,
                                                     statusCode!,
                                                     Request
                                                 ),
                                                 processId
                                             );

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<ChargingErrorNotificationRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace),
                                                 Request,
                                                 HTTPResponse,
                                                 false
                                             )
                                         );

                            }

                        }

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                    {

                        // HTTP/1.1 404 NotFound
                        // Server: nginx/1.18.0 (Ubuntu)
                        // Date: Wed, 03 Mar 2021 01:00:15 GMT
                        // Content-Type: application/json;charset=UTF-8
                        // Content-Length: 85
                        // Connection: keep-alive
                        // Process-ID: 7bb86bc9-659f-4e57-8136-a7eb9ebc9c1d
                        // 
                        // {
                        //     "StatusCode": {
                        //         "Code":            "300",
                        //         "Description":     "Partner not found",
                        //         "AdditionalInfo":   null
                        //     }
                        // }

                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

                                var json = JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String());

                                if (json is not null &&
                                    json["StatusCode"] is JObject JSONObject &&
                                    StatusCode.TryParse(JSONObject,
                                                        out StatusCode? statusCode,
                                                        out String?     ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<ChargingErrorNotificationRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     processId,
                                                     HTTPResponse.Runtime, 
                                                     statusCode!,
                                                     Request
                                                 ),
                                                 processId
                                             );

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Failed(
                                                Request,
                                                new Acknowledgement<ChargingErrorNotificationRequest>(
                                                    HTTPResponse.Timestamp,
                                                    HTTPResponse.EventTrackingId,
                                                    processId,
                                                    HTTPResponse.Runtime,
                                                    new StatusCode(
                                                        StatusCodes.SystemError,
                                                        e.Message,
                                                        e.StackTrace),
                                                    Request,
                                                    HTTPResponse,
                                                    false
                                                )
                                            );

                            }

                        }

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.RequestTimeout)
                    { }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.InternalServerError)
                    {

                        // HTTP/1.1 500
                        // Server:          nginx/1.18.0 (Ubuntu)
                        // Date:            Sat, 06 Aug 2022 06:33:53 GMT
                        // Content-Type:    application/json;charset=ISO-8859-1
                        // Content-Length:  108
                        // Connection:      close
                        // Process-ID:      90163b8e-3301-4538-a7d9-82d7734879b4
                        // 
                        // {"StatusCode":{"Code":"001","Description":"Unexpected error checking partners: null","AdditionalInfo":null}}


                        if (HTTPResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            HTTPResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

                                var json = JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String());

                                if (json is not null &&
                                    json["StatusCode"] is JObject JSONObject &&
                                    StatusCode.TryParse(JSONObject,
                                                        out StatusCode? statusCode,
                                                        out String?     ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<ChargingErrorNotificationRequest>(
                                                     HTTPResponse.Timestamp,
                                                     HTTPResponse.EventTrackingId,
                                                     processId,
                                                     HTTPResponse.Runtime,
                                                     statusCode!,
                                                     Request
                                                 ),
                                                 processId
                                             );

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<ChargingErrorNotificationRequest>(
                                                 HTTPResponse.Timestamp,
                                                 HTTPResponse.EventTrackingId,
                                                 processId,
                                                 HTTPResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace),
                                                 Request,
                                                 HTTPResponse,
                                                 false
                                             )
                                         );

                            }

                        }

                        break;

                    }

                }
                while (TransmissionRetry++ < MaxNumberOfRetries);

            }
            catch (Exception e)
            {

                result = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Failed(
                             Request,
                             new Acknowledgement<ChargingErrorNotificationRequest>(
                                 Timestamp.Now,
                                 Request.EventTrackingId ?? EventTracking_Id.New,
                                 Process_Id.NewRandom(),
                                 Timestamp.Now - Request.Timestamp,
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     e.Message,
                                     e.StackTrace
                                 ),
                                 Request,
                                 null,
                                 false
                             )
                         );

            }

            result ??= OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Failed(
                           Request,
                           new Acknowledgement<ChargingErrorNotificationRequest>(
                               Timestamp.Now,
                               Request.EventTrackingId ?? EventTracking_Id.New,
                               Process_Id.NewRandom(),
                               Timestamp.Now - Request.Timestamp,
                               new StatusCode(
                                   StatusCodes.SystemError,
                                   "HTTP request failed!",
                                   null
                               ),
                               Request,
                               null,
                               false
                           )
                       );

            if (result.IsNotSuccessful)
                Counters.SendChargingErrorNotification.IncResponses_Error();


            #region  OnChargingErrorNotificationResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnChargingErrorNotificationResponse is not null)
                    await Task.WhenAll(OnChargingErrorNotificationResponse.GetInvocationList().
                                       Cast<OnChargingErrorNotificationResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnChargingErrorNotificationResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region SendChargeDetailRecord          (Request)

        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="Request">A SendChargeDetailRecord request.</param>
        public async Task<OICPResult<Acknowledgement<ChargeDetailRecordRequest>>>

            SendChargeDetailRecord(ChargeDetailRecordRequest Request)

        {

            #region Initial checks

            //Request = _CustomSendChargeDetailRecordRequestMapper(Request);

            Byte                                                     TransmissionRetry   = 0;
            OICPResult<Acknowledgement<ChargeDetailRecordRequest>>?  result              = null;

            #endregion

            #region Send OnSendChargeDetailRecord event

            var startTime = Timestamp.Now;

            Counters.SendChargeDetailRecord.IncRequests_OK();

            try
            {

                if (OnSendChargeDetailRecordRequest is not null)
                    await Task.WhenAll(OnSendChargeDetailRecordRequest.GetInvocationList().
                                       Cast<OnSendChargeDetailRecordRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnSendChargeDetailRecordRequest));
            }

            #endregion


            try
            {

                do
                {

                    #region Upstream HTTP request...

                    var httpResponse = await HTTPClientFactory.Create(RemoteURL,
                                                                      VirtualHostname,
                                                                      Description,
                                                                      PreferIPv4,
                                                                      RemoteCertificateValidator,
                                                                      LocalCertificateSelector,
                                                                      ClientCert,
                                                                      TLSProtocol,
                                                                      HTTPUserAgent,
                                                                      HTTPAuthentication,
                                                                      RequestTimeout,
                                                                      TransmissionRetryDelay,
                                                                      MaxNumberOfRetries,
                                                                      InternalBufferSize,
                                                                      UseHTTPPipelining,
                                                                      DisableLogging,
                                                                      null,
                                                                      DNSClient).

                                              Execute(client => client.POSTRequest(RemoteURL.Path + ("/api/oicp/cdrmgmt/v22/operators/" + Request.OperatorId.ToString().Replace("*", "%2A") + "/charge-detail-record"),
                                                                                   requestbuilder => {
                                                                                       requestbuilder.Accept?.Add(HTTPContentType.Application.JSON_UTF8);
                                                                                       requestbuilder.ContentType  = HTTPContentType.Application.JSON_UTF8;
                                                                                       requestbuilder.Content      = Request.ToJSON(CustomChargeDetailRecordRequestSerializer,
                                                                                                                                    CustomChargeDetailRecordSerializer,
                                                                                                                                    CustomIdentificationSerializer,
                                                                                                                                    CustomSignedMeteringValueSerializer,
                                                                                                                                    CustomCalibrationLawVerificationSerializer).
                                                                                                                             ToString(JSONFormatting).
                                                                                                                             ToUTF8Bytes();
                                                                                       requestbuilder.Connection   = "close";
                                                                                   }),

                                                      RequestLogDelegate:   OnSendChargeDetailRecordHTTPRequest,
                                                      ResponseLogDelegate:  OnSendChargeDetailRecordHTTPResponse,
                                                      CancellationToken:    Request.CancellationToken,
                                                      EventTrackingId:      Request.EventTrackingId,
                                                      RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                              ConfigureAwait(false);

                    #endregion


                    var processId = httpResponse.TryParseHeaderField<Process_Id>("Process-ID", Process_Id.TryParse) ?? Process_Id.NewRandom();

                    if (httpResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (httpResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            httpResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

                                if (Acknowledgement<ChargeDetailRecordRequest>.TryParse(Request,
                                                                                        JObject.Parse(httpResponse.HTTPBody.ToUTF8String()),
                                                                                        out var acknowledgement,
                                                                                        out var ErrorResponse,
                                                                                        httpResponse,
                                                                                        httpResponse.Timestamp,
                                                                                        httpResponse.EventTrackingId,
                                                                                        httpResponse.Runtime,
                                                                                        processId,
                                                                                        CustomSendChargeDetailRecordAcknowledgementParser))
                                {

                                    Counters.SendChargeDetailRecord.IncResponses_OK();

                                    result = OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Success(
                                                 Request,
                                                 acknowledgement!,
                                                 processId
                                             );

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<ChargeDetailRecordRequest>(
                                                 httpResponse.Timestamp,
                                                 httpResponse.EventTrackingId,
                                                 processId,
                                                 httpResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace),
                                                 Request,
                                                 httpResponse,
                                                 false
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

                            // HTTP/1.1 400 OK
                            // Server:             nginx/1.18.0
                            // Date:               Fri, 08 Jan 2021 14:19:25 GMT
                            // Content-Type:       application/json;charset=utf-8
                            // Transfer-Encoding:  chunked
                            // Connection:         keep-alive
                            // Process-ID:         b87fd67b-2d74-4318-86cf-0d2c2c50cabb
                            // 
                            // {
                            //     "message": "Error parsing/validating JSON.",
                            //     "validationErrors": [
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].hotlinePhoneNumber",
                            //             "errorMessage":    "must match \"^\\+[0-9]{5,15}$\""
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].geoCoordinates",
                            //             "errorMessage":    "may not be null"
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].chargingStationNames",
                            //             "errorMessage":    "may not be empty"
                            //         },
                            //         {
                            //             "fieldReference":  "operatorEvseStatus.evseStatusRecord[0].plugs",
                            //             "errorMessage":    "may not be empty"
                            //         }
                            //     ]
                            // }

                            if (ValidationErrorList.TryParse(JObject.Parse(httpResponse.HTTPBody.ToUTF8String() ?? ""),
                                                             out var validationErrors,
                                                             out var errorResponse))
                            {

                                result = OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.BadRequest(
                                             Request,
                                             validationErrors,
                                             processId
                                         );

                            }

                        }

                    }

                    if (httpResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                    {

                        // Hubject firewall problem!
                        // Only HTML response!
                        break;

                    }

                    if (httpResponse.HTTPStatusCode == HTTPStatusCode.Unauthorized ||
                        httpResponse.HTTPStatusCode == HTTPStatusCode.NotFound)
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

                        // => Operator/provider identification is not linked to the TLS client certificate!


                        // HTTP/1.1 404 Not Found
                        // Server:             nginx/1.18.0 (Ubuntu)
                        // Date:               Tue, 01 Jun 2021 21:22:45 GMT
                        // Content-Type:       application/json
                        // Transfer-Encoding:  chunked
                        // Connection:         close
                        // Process-ID:         51c4bb66-052c-4c1c-a288-31902dc81fd1
                        // 
                        // {
                        //     "StatusCode": {
                        //         "Code":            "400",
                        //         "Description":     "Session found but status is not valid CLOSED!",
                        //         "AdditionalInfo":   null
                        //     }
                        // }

                        if (httpResponse.ContentType == HTTPContentType.Application.JSON_UTF8 &&
                            httpResponse.HTTPBody?.Length > 0)
                        {

                            try
                            {

                                var json = JObject.Parse(httpResponse.HTTPBody.ToUTF8String());

                                if (json is not null &&
                                    json["StatusCode"] is JObject JSONObject &&
                                    StatusCode.TryParse(JSONObject,
                                                        out StatusCode? statusCode,
                                                        out String? ErrorResponse,
                                                        CustomStatusCodeParser))
                                {

                                    result = OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Failed(
                                                 Request,
                                                 new Acknowledgement<ChargeDetailRecordRequest>(
                                                     httpResponse.Timestamp,
                                                     httpResponse.EventTrackingId,
                                                     processId,
                                                     httpResponse.Runtime,
                                                     statusCode!,
                                                     Request
                                                 ),
                                                 processId
                                             );

                                }

                            }
                            catch (Exception e)
                            {

                                result = OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Failed(
                                             Request,
                                             new Acknowledgement<ChargeDetailRecordRequest>(
                                                 httpResponse.Timestamp,
                                                 httpResponse.EventTrackingId,
                                                 processId,
                                                 httpResponse.Runtime,
                                                 new StatusCode(
                                                     StatusCodes.SystemError,
                                                     e.Message,
                                                     e.StackTrace),
                                                 Request,
                                                 httpResponse,
                                                 false
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
                             new Acknowledgement<ChargeDetailRecordRequest>(
                                 Timestamp.Now,
                                 Request.EventTrackingId ?? EventTracking_Id.New,
                                 Process_Id.NewRandom(),
                                 Timestamp.Now - Request.Timestamp,
                                 new StatusCode(
                                     StatusCodes.SystemError,
                                     e.Message,
                                     e.StackTrace
                                 ),
                                 Request,
                                 null,
                                 false
                             )
                         );

            }

            result ??= OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Failed(
                           Request,
                           new Acknowledgement<ChargeDetailRecordRequest>(
                               Timestamp.Now,
                               Request.EventTrackingId ?? EventTracking_Id.New,
                               Process_Id.NewRandom(),
                               Timestamp.Now - Request.Timestamp,
                               new StatusCode(
                                   StatusCodes.SystemError,
                                   "HTTP request failed!",
                                   null
                               ),
                               Request,
                               null,
                               false
                           )
                       );

            if (result.IsNotSuccessful)
                Counters.SendChargeDetailRecord.IncResponses_Error();


            #region Send OnChargeDetailRecordSent event

            var endtime = Timestamp.Now;

            try
            {

                if (OnSendChargeDetailRecordResponse is not null)
                    await Task.WhenAll(OnSendChargeDetailRecordResponse.GetInvocationList().
                                       Cast<OnSendChargeDetailRecordResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     result,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnSendChargeDetailRecordResponse));
            }

            #endregion

            return result;

        }

        #endregion


    }

}
