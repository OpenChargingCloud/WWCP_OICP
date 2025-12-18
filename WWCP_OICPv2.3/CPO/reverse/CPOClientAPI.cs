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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Sockets;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;
using org.GraphDefined.Vanaheimr.Hermod.Logging;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    /// <summary>
    /// The CPO HTTP Client API.
    /// </summary>
    public partial class CPOClientAPI : HTTPAPI
    {

        #region (class) APICounters

        public class APICounters(APICounterValues?  PushEVSEData                   = null,
                                 APICounterValues?  PushEVSEStatus                 = null,

                                 APICounterValues?  PushPricingProductData         = null,
                                 APICounterValues?  PushEVSEPricing                = null,

                                 APICounterValues?  PullAuthenticationData         = null,

                                 APICounterValues?  AuthorizeStart                 = null,
                                 APICounterValues?  AuthorizeStop                  = null,

                                 APICounterValues?  ChargingNotifications          = null,
                                 APICounterValues?  ChargingStartNotification      = null,
                                 APICounterValues?  ChargingProgressNotification   = null,
                                 APICounterValues?  ChargingEndNotification        = null,
                                 APICounterValues?  ChargingErrorNotification      = null,

                                 APICounterValues?  ChargeDetailRecord             = null)
        {

            public APICounterValues PushEVSEData                    { get; } = PushEVSEData                 ?? new APICounterValues();
            public APICounterValues PushEVSEStatus                  { get; } = PushEVSEStatus               ?? new APICounterValues();

            public APICounterValues PushPricingProductData          { get; } = PushPricingProductData       ?? new APICounterValues();
            public APICounterValues PushEVSEPricing                 { get; } = PushEVSEPricing              ?? new APICounterValues();

            public APICounterValues PullAuthenticationData          { get; } = PullAuthenticationData       ?? new APICounterValues();

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
                       new JProperty("PushEVSEData",                  PushEVSEData.                ToJSON()),
                       new JProperty("PushEVSEStatus",                PushEVSEStatus.              ToJSON()),

                       new JProperty("PushPricingProductData",        PushPricingProductData.      ToJSON()),
                       new JProperty("PushEVSEPricing",               PushEVSEPricing.             ToJSON()),

                       new JProperty("PullAuthenticationData",        PullAuthenticationData.      ToJSON()),

                       new JProperty("AuthorizeStart",                AuthorizeStart.              ToJSON()),
                       new JProperty("AuthorizeStop",                 AuthorizeStop.               ToJSON()),

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
        public new const String  DefaultHTTPServerName   = "GraphDefined OICP " + Version.String + " CPO HTTP API";

        /// <summary>
        /// The default HTTP service name.
        /// </summary>
        public new const String  DefaultHTTPServiceName  = "GraphDefined OICP " + Version.String + " CPO HTTP API";

        /// <summary>
        /// The default logging context.
        /// </summary>
        public     const String  DefaultLoggingContext   = "CPOServerAPI";

        #endregion

        #region Properties

        /// <summary>
        /// The optional URL path prefix, used when defining URL templates.
        /// </summary>
        public new HTTPPath                URLPathPrefix     { get; }

        /// <summary>
        /// The attached HTTP logger.
        /// </summary>
        public new HTTP_Logger             HTTPLogger
#pragma warning disable CS8603 // Possible null reference return.
            => base.HTTPLogger as HTTP_Logger;
#pragma warning restore CS8603 // Possible null reference return.

        /// <summary>
        /// The attached Client API logger.
        /// </summary>
        public CPOClientAPILogger?         Logger            { get; }

        /// <summary>
        /// CPO Client API counters.
        /// </summary>
        public APICounters                 Counters          { get; }

        public Newtonsoft.Json.Formatting  JSONFormatting    { get; set; }

        #endregion

        #region Custom JSON parsers
        public CustomJObjectParserDelegate<PushEVSEDataRequest>?                  CustomPushEVSEDataRequestParser                    { get; set; }
        public CustomJObjectParserDelegate<PushEVSEStatusRequest>?                CustomPushEVSEStatusRequestParser                  { get; set; }


        public CustomJObjectParserDelegate<PushPricingProductDataRequest>?        CustomPushPricingProductDataRequestParser          { get; set; }
        public CustomJObjectParserDelegate<PushEVSEPricingRequest>?               CustomPushEVSEPricingRequestParser                 { get; set; }

        public CustomJObjectParserDelegate<PullAuthenticationDataRequest>?        CustomPullAuthenticationDataRequestParser          { get; set; }

        public CustomJObjectParserDelegate<AuthorizeStartRequest>?                CustomAuthorizeStartRequestParser                  { get; set; }
        public CustomJObjectParserDelegate<AuthorizeStopRequest>?                 CustomAuthorizeStopRequestParser                   { get; set; }


        public CustomJObjectParserDelegate<ChargingStartNotificationRequest>?     CustomChargingStartNotificationRequestParser       { get; set; }
        public CustomJObjectParserDelegate<ChargingProgressNotificationRequest>?  CustomChargingProgressNotificationRequestParser    { get; set; }
        public CustomJObjectParserDelegate<ChargingEndNotificationRequest>?       CustomChargingEndNotificationRequestParser         { get; set; }
        public CustomJObjectParserDelegate<ChargingErrorNotificationRequest>?     CustomChargingErrorNotificationRequestParser       { get; set; }

        public CustomJObjectParserDelegate<ChargeDetailRecordRequest>?            CustomChargeDetailRecordRequestParser              { get; set; }

        #endregion

        #region Custom JSON serializers
        public CustomJObjectSerializerDelegate<Acknowledgement>?                  CustomAcknowledgementSerializer                    { get; set; }
        public CustomJObjectSerializerDelegate<StatusCode>?                       CustomStatusCodeSerializer                         { get; set; }

        public CustomJObjectSerializerDelegate<PullAuthenticationDataResponse>?   CustomPullAuthenticationDataResponseSerializer     { get; set; }
        public CustomJObjectSerializerDelegate<ProviderAuthenticationData>?       CustomProviderAuthenticationDataSerializer         { get; set; }

        public CustomJObjectSerializerDelegate<AuthorizationStartResponse>?       CustomAuthorizationStartSerializer                 { get; set; }
        public CustomJObjectSerializerDelegate<Identification>?                   CustomIdentificationSerializer                     { get; set; }
        public CustomJObjectSerializerDelegate<AuthorizationStopResponse>?        CustomAuthorizationStopSerializer                  { get; set; }

        #endregion

        #region Custom request/response logging converters

        #region PushEVSEData                (Request/Response)Converter

        public Func<DateTimeOffset, Object, PushEVSEDataRequest, String>
            PushEVSEDataRequestConverter                     { get; set; }

            = (timestamp, sender, pushEVSEDataRequest)
            => String.Concat(pushEVSEDataRequest.Action, " of ", pushEVSEDataRequest.EVSEDataRecords.Count(), " evse(s)");

        public Func<DateTimeOffset, Object, PushEVSEDataRequest, OICPResult<Acknowledgement<PushEVSEDataRequest>>, TimeSpan, String>
            PushEVSEDataResponseConverter                    { get; set; }

            = (timestamp, sender, pushEVSEDataRequest, pushEVSEDataResponse, runtime)
            => String.Concat(pushEVSEDataRequest.Action, " of ", pushEVSEDataRequest.EVSEDataRecords.Count(), " evse(s) => ", pushEVSEDataResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion

        #region PushEVSEStatus              (Request/Response)Converter

        public Func<DateTimeOffset, Object, PushEVSEStatusRequest, String>
            PushEVSEStatusRequestConverter                   { get; set; }

            = (timestamp, sender, pushEVSEStatusRequest)
            => String.Concat(pushEVSEStatusRequest.Action, " of ", pushEVSEStatusRequest.EVSEStatusRecords.Count(), " evse status");

        public Func<DateTimeOffset, Object, PushEVSEStatusRequest, OICPResult<Acknowledgement<PushEVSEStatusRequest>>, TimeSpan, String>
            PushEVSEStatusResponseConverter                  { get; set; }

            = (timestamp, sender, pushEVSEStatusRequest, pushEVSEStatusResponse, runtime)
            => String.Concat(pushEVSEStatusRequest.Action, " of ", pushEVSEStatusRequest.EVSEStatusRecords.Count(), " evse status => ", pushEVSEStatusResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion


        #region PushPricingProductData      (Request/Response)Converter

        public Func<DateTimeOffset, Object, PushPricingProductDataRequest, String>
            PushPricingProductDataRequestConverter                     { get; set; }

            = (timestamp, sender, pushPricingProductDataRequest)
            => String.Concat(pushPricingProductDataRequest.Action, " of ", pushPricingProductDataRequest.PricingProductData.PricingProductDataRecords.Count(), " pricing product data record(s)");

        public Func<DateTimeOffset, Object, PushPricingProductDataRequest, OICPResult<Acknowledgement<PushPricingProductDataRequest>>, TimeSpan, String>
            PushPricingProductDataResponseConverter                    { get; set; }

            = (timestamp, sender, pushPricingProductDataRequest, pushPricingProductDataResponse, runtime)
            => String.Concat(pushPricingProductDataRequest.Action, " of ", pushPricingProductDataRequest.PricingProductData.PricingProductDataRecords.Count(), " pricing product data record(s) => ", pushPricingProductDataResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion

        #region PushEVSEPricing             (Request/Response)Converter

        public Func<DateTimeOffset, Object, PushEVSEPricingRequest, String>
            PushEVSEPricingRequestConverter                   { get; set; }

            = (timestamp, sender, pushEVSEPricingRequest)
            => String.Concat(pushEVSEPricingRequest.Action, " of ", pushEVSEPricingRequest.EVSEPricing.Count(), " evse pricing record(s)");

        public Func<DateTimeOffset, Object, PushEVSEPricingRequest, OICPResult<Acknowledgement<PushEVSEPricingRequest>>, TimeSpan, String>
            PushEVSEPricingResponseConverter                  { get; set; }

            = (timestamp, sender, pushEVSEPricingRequest, pushEVSEPricingResponse, runtime)
            => String.Concat(pushEVSEPricingRequest.Action, " of ", pushEVSEPricingRequest.EVSEPricing.Count(), " evse pricing record(s) => ", pushEVSEPricingResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion


        #region AuthorizeStart              (Request/Response)Converter

        public Func<DateTimeOffset, Object, AuthorizeStartRequest, String>
            AuthorizeStartRequestConverter                   { get; set; }

            = (timestamp, sender, authorizeStartRequest)
            => String.Concat(authorizeStartRequest.Identification, " at ", authorizeStartRequest.EVSEId);

        public Func<DateTimeOffset, Object, AuthorizeStartRequest, OICPResult<AuthorizationStartResponse>, TimeSpan, String>
            AuthorizationStartResponseConverter              { get; set; }

            = (timestamp, sender, authorizeStartRequest, authorizationStartResponse, runtime)
            => String.Concat(authorizeStartRequest.Identification, " at ", authorizeStartRequest.EVSEId, " => ", authorizationStartResponse.Response?.AuthorizationStatus.ToString() ?? "failed!");

        #endregion

        #region AuthorizeStop               (Request/Response)Converter

        public Func<DateTimeOffset, Object, AuthorizeStopRequest, String>
            AuthorizeStopRequestConverter                    { get; set; }

            = (timestamp, sender, authorizeStopRequest)
            => String.Concat(authorizeStopRequest.Identification, " at ", authorizeStopRequest.EVSEId);

        public Func<DateTimeOffset, Object, AuthorizeStopRequest, OICPResult<AuthorizationStopResponse>, TimeSpan, String>
            AuthorizationStopResponseConverter               { get; set; }

            = (timestamp, sender, authorizeStopRequest, authorizationStopResponse, runtime)
            => String.Concat(authorizeStopRequest.Identification, " at ", authorizeStopRequest.EVSEId, " => ", authorizationStopResponse.Response?.AuthorizationStatus.ToString() ?? "failed!");

        #endregion


        #region ChargingStartNotification   (Request/Response)Converter

        public Func<DateTimeOffset, Object, ChargingStartNotificationRequest, String>
            ChargingStartNotificationRequestConverter        { get; set; }

            = (timestamp, sender, chargingStartNotificationRequest)
            => String.Concat(chargingStartNotificationRequest.Identification, " at ", chargingStartNotificationRequest.EVSEId);

        public Func<DateTimeOffset, Object, ChargingStartNotificationRequest, OICPResult<Acknowledgement<ChargingStartNotificationRequest>>, TimeSpan, String>
            ChargingStartNotificationResponseConverter       { get; set; }

            = (timestamp, sender, chargingStartNotificationRequest, chargingStartNotificationResponse, runtime)
            => String.Concat(chargingStartNotificationRequest.Identification, " at ", chargingStartNotificationRequest.EVSEId, " => ", chargingStartNotificationResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion

        #region ChargingProgressNotification(Request/Response)Converter

        public Func<DateTimeOffset, Object, ChargingProgressNotificationRequest, String>
            ChargingProgressNotificationRequestConverter     { get; set; }

            = (timestamp, sender, chargingProgressNotificationRequest)
            => String.Concat(chargingProgressNotificationRequest.Identification, " at ", chargingProgressNotificationRequest.EVSEId);

        public Func<DateTimeOffset, Object, ChargingProgressNotificationRequest, OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>, TimeSpan, String>
            ChargingProgressNotificationResponseConverter    { get; set; }

            = (timestamp, sender, chargingProgressNotificationRequest, chargingProgressNotificationResponse, runtime)
            => String.Concat(chargingProgressNotificationRequest.Identification, " at ", chargingProgressNotificationRequest.EVSEId, " => ", chargingProgressNotificationResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion

        #region ChargingEndNotification     (Request/Response)Converter

        public Func<DateTimeOffset, Object, ChargingEndNotificationRequest, String>
            ChargingEndNotificationRequestConverter          { get; set; }

            = (timestamp, sender, chargingEndNotificationRequest)
            => String.Concat(chargingEndNotificationRequest.Identification, " at ", chargingEndNotificationRequest.EVSEId);

        public Func<DateTimeOffset, Object, ChargingEndNotificationRequest, OICPResult<Acknowledgement<ChargingEndNotificationRequest>>, TimeSpan, String>
            ChargingEndNotificationResponseConverter         { get; set; }

            = (timestamp, sender, chargingEndNotificationRequest, chargingEndNotificationResponse, runtime)
            => String.Concat(chargingEndNotificationRequest.Identification, " at ", chargingEndNotificationRequest.EVSEId, " => ", chargingEndNotificationResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion

        #region ChargingErrorNotification   (Request/Response)Converter

        public Func<DateTimeOffset, Object, ChargingErrorNotificationRequest, String>
            ChargingErrorNotificationRequestConverter        { get; set; }

            = (timestamp, sender, chargingErrorNotificationRequest)
            => String.Concat(chargingErrorNotificationRequest.Identification, " at ", chargingErrorNotificationRequest.EVSEId);

        public Func<DateTimeOffset, Object, ChargingErrorNotificationRequest, OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>, TimeSpan, String>
            ChargingErrorNotificationResponseConverter       { get; set; }

            = (timestamp, sender, chargingErrorNotificationRequest, chargingErrorNotificationResponse, runtime)
            => String.Concat(chargingErrorNotificationRequest.Identification, " at ", chargingErrorNotificationRequest.EVSEId, " => ", chargingErrorNotificationResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion


        #region SendChargeDetailRecord      (Request/Response)Converter

        public Func<DateTimeOffset, Object, ChargeDetailRecordRequest, String>
            SendChargeDetailRecordRequestConverter           { get; set; }

            = (timestamp, sender, chargeDetailRecordRequest)
            => String.Concat(chargeDetailRecordRequest.ChargeDetailRecord.Identification, " at ", chargeDetailRecordRequest.ChargeDetailRecord.EVSEId, " (", chargeDetailRecordRequest.ChargeDetailRecord.SessionId, ")");

        public Func<DateTimeOffset, Object, ChargeDetailRecordRequest, OICPResult<Acknowledgement<ChargeDetailRecordRequest>>, TimeSpan, String>
            SendChargeDetailRecordResponseConverter          { get; set; }

            = (timestamp, sender, chargeDetailRecordRequest, chargeDetailRecordResponse, runtime)
            => String.Concat(chargeDetailRecordRequest.ChargeDetailRecord.Identification, " at ", chargeDetailRecordRequest.ChargeDetailRecord.EVSEId, " (", chargeDetailRecordRequest.ChargeDetailRecord.SessionId, ") => ", chargeDetailRecordResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion

        #endregion

        #region Events

        #region (protected internal) OnPushEVSEDataHTTPRequest

        /// <summary>
        /// An event sent whenever an PushEVSEData HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnPushEVSEDataHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an PushEVSEData HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logPushEVSEDataHTTPRequest(DateTimeOffset  Timestamp,
                                                           HTTPAPI         API,
                                                           HTTPRequest     Request)

            => OnPushEVSEDataHTTPRequest.WhenAll(Timestamp,
                                                 API ?? this,
                                                 Request);

        #endregion

        #region (event)              OnPushEVSEData(Request-/Response)

        /// <summary>
        /// An event send whenever a PushEVSEData request was received.
        /// </summary>
        public event OnPushEVSEDataAPIRequestDelegate?   OnPushEVSEDataRequest;

        /// <summary>
        /// An event send whenever a PushEVSEData request was received.
        /// </summary>
        public event OnPushEVSEDataAPIDelegate?          OnPushEVSEData;

        /// <summary>
        /// An event send whenever a response to a PushEVSEData request was sent.
        /// </summary>
        public event OnPushEVSEDataAPIResponseDelegate?  OnPushEVSEDataResponse;

        #endregion

        #region (protected internal) OnPushEVSEDataHTTPResponse

        /// <summary>
        /// An event sent whenever an PushEVSEData HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnPushEVSEDataHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an PushEVSEData HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logPushEVSEDataHTTPResponse(DateTimeOffset  Timestamp,
                                                            HTTPAPI         API,
                                                            HTTPRequest     Request,
                                                            HTTPResponse    Response)

            => OnPushEVSEDataHTTPResponse.WhenAll(Timestamp,
                                                  API ?? this,
                                                  Request,
                                                  Response);

        #endregion


        #region (protected internal) OnPushEVSEStatusHTTPRequest

        /// <summary>
        /// An event sent whenever an PushEVSEStatus HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnPushEVSEStatusHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an PushEVSEStatus HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logPushEVSEStatusHTTPRequest(DateTimeOffset  Timestamp,
                                                             HTTPAPI         API,
                                                             HTTPRequest     Request)

            => OnPushEVSEStatusHTTPRequest.WhenAll(Timestamp,
                                                   API ?? this,
                                                   Request);

        #endregion

        #region (event)              OnPushEVSEStatus(Request-/Response)

        /// <summary>
        /// An event send whenever a PushEVSEStatus request was received.
        /// </summary>
        public event OnPushEVSEStatusAPIRequestDelegate?   OnPushEVSEStatusRequest;

        /// <summary>
        /// An event send whenever a PushEVSEStatus request was received.
        /// </summary>
        public event OnPushEVSEStatusAPIDelegate?          OnPushEVSEStatus;

        /// <summary>
        /// An event send whenever a response to a PushEVSEStatus request was sent.
        /// </summary>
        public event OnPushEVSEStatusAPIResponseDelegate?  OnPushEVSEStatusResponse;

        #endregion

        #region (protected internal) OnPushEVSEStatusHTTPResponse

        /// <summary>
        /// An event sent whenever an PushEVSEStatus HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnPushEVSEStatusHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an PushEVSEStatus HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logPushEVSEStatusHTTPResponse(DateTimeOffset  Timestamp,
                                                              HTTPAPI         API,
                                                              HTTPRequest     Request,
                                                              HTTPResponse    Response)

            => OnPushEVSEStatusHTTPResponse.WhenAll(Timestamp,
                                                    API ?? this,
                                                    Request,
                                                    Response);

        #endregion



        #region (protected internal) OnPushPricingProductDataHTTPRequest

        /// <summary>
        /// An event sent whenever an PushPricingProductData HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnPushPricingProductDataHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an PushPricingProductData HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logPushPricingProductDataHTTPRequest(DateTimeOffset  Timestamp,
                                                                     HTTPAPI         API,
                                                                     HTTPRequest     Request)

            => OnPushPricingProductDataHTTPRequest.WhenAll(Timestamp,
                                                           API ?? this,
                                                           Request);

        #endregion

        #region (event)              OnPushPricingProductData(Request-/Response)

        /// <summary>
        /// An event send whenever a PushPricingProductData request was received.
        /// </summary>
        public event OnPushPricingProductDataAPIRequestDelegate?   OnPushPricingProductDataRequest;

        /// <summary>
        /// An event send whenever a PushPricingProductData request was received.
        /// </summary>
        public event OnPushPricingProductDataAPIDelegate?          OnPushPricingProductData;

        /// <summary>
        /// An event send whenever a response to a PushPricingProductData request was sent.
        /// </summary>
        public event OnPushPricingProductDataAPIResponseDelegate?  OnPushPricingProductDataResponse;

        #endregion

        #region (protected internal) OnPushPricingProductDataHTTPResponse

        /// <summary>
        /// An event sent whenever an PushPricingProductData HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnPushPricingProductDataHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an PushPricingProductData HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logPushPricingProductDataHTTPResponse(DateTimeOffset  Timestamp,
                                                                      HTTPAPI         API,
                                                                      HTTPRequest     Request,
                                                                      HTTPResponse    Response)

            => OnPushPricingProductDataHTTPResponse.WhenAll(Timestamp,
                                                            API ?? this,
                                                            Request,
                                                            Response);

        #endregion


        #region (protected internal) OnPushEVSEPricingHTTPRequest

        /// <summary>
        /// An event sent whenever an PushEVSEPricing HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnPushEVSEPricingHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an PushEVSEPricing HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logPushEVSEPricingHTTPRequest(DateTimeOffset  Timestamp,
                                                              HTTPAPI         API,
                                                              HTTPRequest     Request)

            => OnPushEVSEPricingHTTPRequest.WhenAll(Timestamp,
                                                    API ?? this,
                                                    Request);

        #endregion

        #region (event)              OnPushEVSEPricing(Request-/Response)

        /// <summary>
        /// An event send whenever a PushEVSEPricing request was received.
        /// </summary>
        public event OnPushEVSEPricingAPIRequestDelegate?   OnPushEVSEPricingRequest;

        /// <summary>
        /// An event send whenever a PushEVSEPricing request was received.
        /// </summary>
        public event OnPushEVSEPricingAPIDelegate?          OnPushEVSEPricing;

        /// <summary>
        /// An event send whenever a response to a PushEVSEPricing request was sent.
        /// </summary>
        public event OnPushEVSEPricingAPIResponseDelegate?  OnPushEVSEPricingResponse;

        #endregion

        #region (protected internal) OnPushEVSEPricingHTTPResponse

        /// <summary>
        /// An event sent whenever an PushEVSEPricing HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnPushEVSEPricingHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an PushEVSEPricing HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logPushEVSEPricingHTTPResponse(DateTimeOffset  Timestamp,
                                                               HTTPAPI         API,
                                                               HTTPRequest     Request,
                                                               HTTPResponse    Response)

            => OnPushEVSEPricingHTTPResponse.WhenAll(Timestamp,
                                                     API ?? this,
                                                     Request,
                                                     Response);

        #endregion



        #region (protected internal) OnPullAuthenticationDataHTTPRequest

        /// <summary>
        /// An event sent whenever an PullAuthenticationData HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnPullAuthenticationDataHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an PullAuthenticationData HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logPullAuthenticationDataHTTPRequest(DateTimeOffset  Timestamp,
                                                                     HTTPAPI         API,
                                                                     HTTPRequest     Request)

            => OnPullAuthenticationDataHTTPRequest.WhenAll(Timestamp,
                                                           API ?? this,
                                                           Request);

        #endregion

        #region (event)              OnPullAuthenticationData(Request-/Response)

        /// <summary>
        /// An event send whenever a PullAuthenticationData request was received.
        /// </summary>
        public event OnPullAuthenticationDataAPIRequestDelegate?   OnPullAuthenticationDataRequest;

        /// <summary>
        /// An event send whenever a PullAuthenticationData request was received.
        /// </summary>
        public event OnPullAuthenticationDataAPIDelegate?          OnPullAuthenticationData;

        /// <summary>
        /// An event send whenever a response to a PullAuthenticationData request was sent.
        /// </summary>
        public event OnPullAuthenticationDataAPIResponseDelegate?  OnPullAuthenticationDataResponse;

        #endregion

        #region (protected internal) OnPullAuthenticationDataHTTPResponse

        /// <summary>
        /// An event sent whenever an PullAuthenticationData HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnPullAuthenticationDataHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an PullAuthenticationData HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logPullAuthenticationDataHTTPResponse(DateTimeOffset  Timestamp,
                                                                      HTTPAPI         API,
                                                                      HTTPRequest     Request,
                                                                      HTTPResponse    Response)

            => OnPullAuthenticationDataHTTPResponse.WhenAll(Timestamp,
                                                            API ?? this,
                                                            Request,
                                                            Response);

        #endregion



        #region (protected internal) OnAuthorizeStartHTTPRequest

        /// <summary>
        /// An event sent whenever an AuthorizeStart HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnAuthorizeStartHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an AuthorizeStart HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logAuthorizeStartHTTPRequest(DateTimeOffset  Timestamp,
                                                             HTTPAPI         API,
                                                             HTTPRequest     Request)

            => OnAuthorizeStartHTTPRequest.WhenAll(Timestamp,
                                                   API ?? this,
                                                   Request);

        #endregion

        #region (event)              OnAuthorizeStart(Request-/Response)

        /// <summary>
        /// An event send whenever a AuthorizeStart request was received.
        /// </summary>
        public event OnAuthorizeStartAPIRequestDelegate?   OnAuthorizeStartRequest;

        /// <summary>
        /// An event send whenever a AuthorizeStart request was received.
        /// </summary>
        public event OnAuthorizeStartAPIDelegate?          OnAuthorizeStart;

        /// <summary>
        /// An event send whenever a response to a AuthorizeStart request was sent.
        /// </summary>
        public event OnAuthorizeStartAPIResponseDelegate?  OnAuthorizeStartResponse;

        #endregion

        #region (protected internal) OnAuthorizationStartHTTPResponse

        /// <summary>
        /// An event sent whenever an AuthorizationStart HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnAuthorizationStartHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an AuthorizationStart HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logAuthorizationStartHTTPResponse(DateTimeOffset  Timestamp,
                                                                  HTTPAPI         API,
                                                                  HTTPRequest     Request,
                                                                  HTTPResponse    Response)

            => OnAuthorizationStartHTTPResponse.WhenAll(Timestamp,
                                                        API ?? this,
                                                        Request,
                                                        Response);

        #endregion


        #region (protected internal) OnAuthorizeStopHTTPRequest

        /// <summary>
        /// An event sent whenever an AuthorizeStop HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnAuthorizeStopHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an AuthorizeStop HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logAuthorizeStopHTTPRequest(DateTimeOffset  Timestamp,
                                                            HTTPAPI         API,
                                                            HTTPRequest     Request)

            => OnAuthorizeStopHTTPRequest.WhenAll(Timestamp,
                                                  API ?? this,
                                                  Request);

        #endregion

        #region (event)              OnAuthorizeStop(Request-/Response)

        /// <summary>
        /// An event send whenever a AuthorizeStop request was received.
        /// </summary>
        public event OnAuthorizeStopAPIRequestDelegate?   OnAuthorizeStopRequest;

        /// <summary>
        /// An event send whenever a AuthorizeStop request was received.
        /// </summary>
        public event OnAuthorizeStopAPIDelegate?          OnAuthorizeStop;

        /// <summary>
        /// An event send whenever a response to a AuthorizeStop request was sent.
        /// </summary>
        public event OnAuthorizeStopAPIResponseDelegate?  OnAuthorizeStopResponse;

        #endregion

        #region (protected internal) OnAuthorizationStopHTTPResponse

        /// <summary>
        /// An event sent whenever an AuthorizationStop HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnAuthorizationStopHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an AuthorizationStop HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logAuthorizationStopHTTPResponse(DateTimeOffset  Timestamp,
                                                                 HTTPAPI         API,
                                                                 HTTPRequest     Request,
                                                                 HTTPResponse    Response)

            => OnAuthorizationStopHTTPResponse.WhenAll(Timestamp,
                                                       API ?? this,
                                                       Request,
                                                       Response);

        #endregion



        #region (protected internal) OnChargingNotificationHTTPRequest

        /// <summary>
        /// An event sent whenever an ChargingNotification HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnChargingNotificationHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an ChargingNotification HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logChargingNotificationHTTPRequest(DateTimeOffset  Timestamp,
                                                                   HTTPAPI         API,
                                                                   HTTPRequest     Request)

            => OnChargingNotificationHTTPRequest.WhenAll(Timestamp,
                                                         API ?? this,
                                                         Request);

        #endregion

        #region (event)              OnChargingStartNotification   (Request-/Response)

        /// <summary>
        /// An event send whenever a ChargingStartNotification request was received.
        /// </summary>
        public event OnChargingStartNotificationAPIRequestDelegate?   OnChargingStartNotificationRequest;

        /// <summary>
        /// An event send whenever a ChargingStartNotification request was received.
        /// </summary>
        public event OnChargingStartNotificationAPIDelegate?          OnChargingStartNotification;

        /// <summary>
        /// An event send whenever a response to a ChargingStartNotification request was sent.
        /// </summary>
        public event OnChargingStartNotificationAPIResponseDelegate?  OnChargingStartNotificationResponse;

        #endregion

        #region (event)              OnChargingProgressNotification(Request-/Response)

        /// <summary>
        /// An event send whenever a ChargingProgressNotification request was received.
        /// </summary>
        public event OnChargingProgressNotificationAPIRequestDelegate?   OnChargingProgressNotificationRequest;

        /// <summary>
        /// An event send whenever a ChargingProgressNotification request was received.
        /// </summary>
        public event OnChargingProgressNotificationAPIDelegate?          OnChargingProgressNotification;

        /// <summary>
        /// An event send whenever a response to a ChargingProgressNotification request was sent.
        /// </summary>
        public event OnChargingProgressNotificationAPIResponseDelegate?  OnChargingProgressNotificationResponse;

        #endregion

        #region (event)              OnChargingEndNotification     (Request-/Response)

        /// <summary>
        /// An event send whenever a ChargingEndNotification request was received.
        /// </summary>
        public event OnChargingEndNotificationAPIRequestDelegate?   OnChargingEndNotificationRequest;

        /// <summary>
        /// An event send whenever a ChargingEndNotification request was received.
        /// </summary>
        public event OnChargingEndNotificationAPIDelegate?          OnChargingEndNotification;

        /// <summary>
        /// An event send whenever a response to a ChargingEndNotification request was sent.
        /// </summary>
        public event OnChargingEndNotificationAPIResponseDelegate?  OnChargingEndNotificationResponse;

        #endregion

        #region (event)              OnChargingErrorNotification   (Request-/Response)

        /// <summary>
        /// An event send whenever a ChargingErrorNotification request was received.
        /// </summary>
        public event OnChargingErrorNotificationAPIRequestDelegate?   OnChargingErrorNotificationRequest;

        /// <summary>
        /// An event send whenever a ChargingErrorNotification request was received.
        /// </summary>
        public event OnChargingErrorNotificationAPIDelegate?          OnChargingErrorNotification;

        /// <summary>
        /// An event send whenever a response to a ChargingErrorNotification request was sent.
        /// </summary>
        public event OnChargingErrorNotificationAPIResponseDelegate?  OnChargingErrorNotificationResponse;

        #endregion

        #region (protected internal) OnChargingNotificationHTTPResponse

        /// <summary>
        /// An event sent whenever an ChargingNotification HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnChargingNotificationHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an ChargingNotification HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logChargingNotificationHTTPResponse(DateTimeOffset  Timestamp,
                                                                    HTTPAPI         API,
                                                                    HTTPRequest     Request,
                                                                    HTTPResponse    Response)

            => OnChargingNotificationHTTPResponse.WhenAll(Timestamp,
                                                          API ?? this,
                                                          Request,
                                                          Response);

        #endregion



        #region (protected internal) OnChargeDetailRecordHTTPRequest

        /// <summary>
        /// An event sent whenever an ChargeDetailRecord HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnChargeDetailRecordHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an ChargeDetailRecord HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logChargeDetailRecordHTTPRequest(DateTimeOffset  Timestamp,
                                                                 HTTPAPI         API,
                                                                 HTTPRequest     Request)

            => OnChargeDetailRecordHTTPRequest.WhenAll(Timestamp,
                                                       API ?? this,
                                                       Request);

        #endregion

        #region (event)              OnChargeDetailRecord(Request-/Response)

        /// <summary>
        /// An event send whenever a ChargeDetailRecord request was received.
        /// </summary>
        public event OnChargeDetailRecordAPIRequestDelegate?   OnChargeDetailRecordRequest;

        /// <summary>
        /// An event send whenever a ChargeDetailRecord request was received.
        /// </summary>
        public event OnChargeDetailRecordAPIDelegate?          OnChargeDetailRecord;

        /// <summary>
        /// An event send whenever a response to a ChargeDetailRecord request was sent.
        /// </summary>
        public event OnChargeDetailRecordAPIResponseDelegate?  OnChargeDetailRecordResponse;

        #endregion

        #region (protected internal) OnChargeDetailRecordHTTPResponse

        /// <summary>
        /// An event sent whenever an ChargeDetailRecord HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnChargeDetailRecordHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an ChargeDetailRecord HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The CPO Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logChargeDetailRecordHTTPResponse(DateTimeOffset  Timestamp,
                                                                  HTTPAPI         API,
                                                                  HTTPRequest     Request,
                                                                  HTTPResponse    Response)

            => OnChargeDetailRecordHTTPResponse.WhenAll(Timestamp,
                                                        API ?? this,
                                                        Request,
                                                        Response);

        #endregion

        #endregion

        #region Constructor(s)

        #region CPOClientAPI(HTTPAPI, URLPathPrefix = null, ...)

        public CPOClientAPI(HTTPAPI                  HTTPAPI,
                            HTTPPath?                URLPathPrefix    = null,

                            String                   LoggingPath      = DefaultHTTPAPI_LoggingPath,
                            String                   LoggingContext   = DefaultLoggingContext,
                            LogfileCreatorDelegate?  LogfileCreator   = null)

            : base(HTTPAPI)

        {

            this.URLPathPrefix   = base.URLPathPrefix + (URLPathPrefix ?? HTTPPath.Root);

            this.Counters        = new APICounters();

            this.JSONFormatting  = Newtonsoft.Json.Formatting.None;

            base.HTTPLogger      = this.DisableLogging == false
                                       ? new HTTP_Logger(this,
                                                         LoggingPath,
                                                         LoggingContext ?? DefaultLoggingContext,
                                                         LogfileCreator)
                                       : null;

            this.Logger          = this.DisableLogging == false
                                       ? new CPOClientAPILogger(this,
                                                                LoggingPath,
                                                                LoggingContext ?? DefaultLoggingContext,
                                                                LogfileCreator)
                                       : null;

            RegisterURLTemplates(false);

        }

        #endregion

        #region CPOClientAPI(HTTPHostname, ...)

        /// <summary>
        /// Create a new CPO HTTP Client API.
        /// </summary>
        /// <param name="HTTPHostname">The HTTP hostname for all URLs within this API.</param>
        /// <param name="ExternalDNSName">The official URL/DNS name of this service, e.g. for sending e-mails.</param>
        /// <param name="HTTPServerPort">A TCP port to listen on.</param>
        /// <param name="BasePath">When the API is served from an optional subdirectory path.</param>
        /// <param name="HTTPServerName">The default HTTP server name, used whenever no HTTP Host-header has been given.</param>
        /// 
        /// <param name="URLPathPrefix">A common prefix for all URLs.</param>
        /// <param name="HTTPServiceName">The name of the HTTP service.</param>
        /// <param name="APIVersionHashes">The API version hashes (git commit hash values).</param>
        /// 
        /// <param name="ServerCertificateSelector">An optional delegate to select a TLS server certificate.</param>
        /// <param name="ClientCertificateValidator">An optional delegate to verify the TLS client certificate used for authentication.</param>
        /// <param name="ClientCertificateSelector">An optional delegate to select the TLS client certificate used for authentication.</param>
        /// <param name="AllowedTLSProtocols">The TLS protocol(s) allowed for this connection.</param>
        /// 
        /// <param name="ServerThreadName">The optional name of the TCP server thread.</param>
        /// <param name="ServerThreadPriority">The optional priority of the TCP server thread.</param>
        /// <param name="ServerThreadIsBackground">Whether the TCP server thread is a background thread or not.</param>
        /// <param name="ConnectionIdBuilder">An optional delegate to build a connection identification based on IP socket information.</param>
        /// <param name="ConnectionTimeout">The TCP client timeout for all incoming client connections in seconds (default: 30 sec).</param>
        /// <param name="MaxClientConnections">The maximum number of concurrent TCP client connections (default: 4096).</param>
        /// 
        /// <param name="DisableMaintenanceTasks">Disable all maintenance tasks.</param>
        /// <param name="MaintenanceInitialDelay">The initial delay of the maintenance tasks.</param>
        /// <param name="MaintenanceEvery">The maintenance interval.</param>
        /// 
        /// <param name="DisableWardenTasks">Disable all warden tasks.</param>
        /// <param name="WardenInitialDelay">The initial delay of the warden tasks.</param>
        /// <param name="WardenCheckEvery">The warden interval.</param>
        /// 
        /// <param name="IsDevelopment">This HTTP API runs in development mode.</param>
        /// <param name="DevelopmentServers">An enumeration of server names which will imply to run this service in development mode.</param>
        /// <param name="DisableLogging">Disable the log file.</param>
        /// <param name="LoggingPath">The path for all logfiles.</param>
        /// <param name="LoggingContext">The context of all logfiles.</param>
        /// <param name="LogfileCreator">A delegate for creating the name of the logfile for this API.</param>
        /// <param name="DNSClient">The DNS client of the API.</param>
        /// <param name="AutoStart">Whether to start the API automatically.</param>
        public CPOClientAPI(HTTPHostname?                                              HTTPHostname                 = null,
                            String?                                                    ExternalDNSName              = null,
                            IPPort?                                                    HTTPServerPort               = null,
                            HTTPPath?                                                  BasePath                     = null,
                            String                                                     HTTPServerName               = DefaultHTTPServerName,

                            HTTPPath?                                                  URLPathPrefix                = null,
                            String                                                     HTTPServiceName              = DefaultHTTPServiceName,
                            Boolean                                                    RegisterRootService          = true,
                            JObject?                                                   APIVersionHashes             = null,

                            ServerCertificateSelectorDelegate?                         ServerCertificateSelector    = null,
                            RemoteTLSClientCertificateValidationHandler<IHTTPServer>?  ClientCertificateValidator   = null,
                            LocalCertificateSelectionHandler?                          ClientCertificateSelector    = null,
                            SslProtocols?                                              AllowedTLSProtocols          = null,
                            Boolean?                                                   ClientCertificateRequired    = null,
                            Boolean?                                                   CheckCertificateRevocation   = null,

                            ServerThreadNameCreatorDelegate?                           ServerThreadNameCreator      = null,
                            ServerThreadPriorityDelegate?                              ServerThreadPrioritySetter   = null,
                            Boolean?                                                   ServerThreadIsBackground     = null,
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
                            String?                                                    Description                  = null,
                            Boolean                                                    AutoStart                    = false)

            : base(HTTPHostname,
                   ExternalDNSName,
                   HTTPServerPort,
                   BasePath,
                   HTTPServerName,

                   URLPathPrefix,
                   HTTPServiceName,
                   null, //HTMLTemplate,
                   APIVersionHashes,

                   ServerCertificateSelector,
                   ClientCertificateValidator,
                   ClientCertificateSelector,
                   AllowedTLSProtocols,
                   ClientCertificateRequired,
                   CheckCertificateRevocation,

                   ServerThreadNameCreator,
                   ServerThreadPrioritySetter,
                   ServerThreadIsBackground,
                   ConnectionIdBuilder,
                   ConnectionTimeout,
                   MaxClientConnections,

                   DisableMaintenanceTasks,
                   MaintenanceInitialDelay,
                   MaintenanceEvery,

                   DisableWardenTasks,
                   WardenInitialDelay,
                   WardenCheckEvery,

                   IsDevelopment,
                   DevelopmentServers,
                   DisableLogging,
                   LoggingPath,
                   LogfileName,
                   LogfileCreator,
                   DNSClient,
                   Description,
                   false) //AutoStart)

        {

            this.URLPathPrefix   =  base.URLPathPrefix + (URLPathPrefix ?? HTTPPath.Root);

            this.Counters        = new APICounters();

            this.JSONFormatting  = Newtonsoft.Json.Formatting.None;

            base.HTTPLogger      = this.DisableLogging == false
                                       ? new HTTP_Logger(this,
                                                         LoggingPath,
                                                         LoggingContext ?? DefaultLoggingContext,
                                                         LogfileCreator)
                                       : null;

            this.Logger          = this.DisableLogging == false
                                       ? new CPOClientAPILogger(this,
                                                                LoggingPath,
                                                                LoggingContext ?? DefaultLoggingContext,
                                                                LogfileCreator)
                                       : null;

            RegisterURLTemplates(RegisterRootService);

            if (AutoStart)
                Start();

        }

        #endregion

        #endregion


        #region (private) RegisterURLTemplates(RegisterRootService = true)

        private void RegisterURLTemplates(Boolean RegisterRootService = true)
        {

            #region ~/ (HTTPRoot)

            if (RegisterRootService)
                AddMethodCallback(HTTPHostname.Any,
                                  HTTPMethod.GET,
                                  [
                                      URLPathPrefix + "/",
                                      URLPathPrefix + "/{FileName}"
                                  ],
                                  HTTPDelegate: Request => {
                                      return Task.FromResult(
                                          new HTTPResponse.Builder(Request) {
                                              HTTPStatusCode  = HTTPStatusCode.OK,
                                              Server          = HTTPServer.DefaultServerName,
                                              Date            = Timestamp.Now,
                                              ContentType     = HTTPContentType.Text.PLAIN,
                                              Content         = "This is an OICP v2.3 CPO Client HTTP/JSON endpoint!".ToUTF8Bytes(),
                                              CacheControl    = "public, max-age=300",
                                              Connection      = ConnectionType.Close
                                          }.AsImmutable);
                                  },
                                  AllowReplacement: URLReplacement.Allow);

            #endregion


            //Note: OperatorId is the remote CPO sending an authorize start/stop request!

            #region POST  ~/api/oicp/evsepull/v23/operators/{operatorId}/data-records

            // -----------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/evsepush/v23/operators/DE-GEF/data-records
            // -----------------------------------------------------------------------------------------------------------------------------------
            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.POST,
                              URLPathPrefix + "/api/oicp/evsepush/v23/operators/{operatorId}/data-records",
                              HTTPContentType.Application.JSON_UTF8,
                              HTTPRequestLogger:   logPushEVSEDataHTTPRequest,
                              HTTPResponseLogger:  logPushEVSEDataHTTPResponse,
                              HTTPDelegate:        async Request => {

                                  var startTime  = Timestamp.Now;
                                  var processId  = Process_Id.NewRandom();

                                  OICPResult<Acknowledgement<PushEVSEDataRequest>>? pullEVSEDataResponse = null;

                                  try
                                  {

                                      #region Try to parse ProviderId URL parameter

                                      if (Request.ParsedURLParameters.Length != 1 || !Operator_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Operator_Id operatorId))
                                      {

                                          Counters.PushEVSEData.IncRequests_Error();

                                          pullEVSEDataResponse = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                                                                     this,
                                                                     new Acknowledgement<PushEVSEDataRequest>(
                                                                         Timestamp.Now,
                                                                         Request.EventTrackingId,
                                                                         processId,
                                                                         Timestamp.Now - Request.Timestamp,
                                                                         StatusCode: new StatusCode(
                                                                                         StatusCodes.SystemError,
                                                                                         "The expected 'operatorId' URL parameter could not be parsed!"
                                                                                     )
                                                                     )
                                                                 );

                                      }

                                      #endregion

                                      else if (PushEVSEDataRequest.TryParse(JObject.Parse(Request.HTTPBody?.ToUTF8String() ?? ""),
                                                                            //     operatorId ????
                                                                            out var                           pullEVSEDataRequest,
                                                                            out var                           errorResponse,
                                                                            ProcessId:                        processId,

                                                                            Timestamp:                        Request.Timestamp,
                                                                            CancellationToken:                Request.CancellationToken,
                                                                            EventTrackingId:                  Request.EventTrackingId,
                                                                            RequestTimeout:                   Request.Timeout ?? DefaultRequestTimeout,

                                                                            CustomPushEVSEDataRequestParser:  CustomPushEVSEDataRequestParser))
                                      {

                                          Counters.PushEVSEData.IncRequests_OK();

                                          #region Send OnPushEVSEDataRequest event

                                          try
                                          {

                                              if (OnPushEVSEDataRequest is not null)
                                                  await Task.WhenAll(OnPushEVSEDataRequest.GetInvocationList().
                                                                     Cast<OnPushEVSEDataAPIRequestDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   pullEVSEDataRequest!))).
                                                                     ConfigureAwait(false);

                                          }
                                          catch (Exception e)
                                          {
                                              DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnPushEVSEDataRequest));
                                          }

                                          #endregion

                                          #region Call async subscribers

                                          var OnPushEVSEDataLocal = OnPushEVSEData;
                                          if (OnPushEVSEDataLocal is not null)
                                          {
                                              try
                                              {

                                                  pullEVSEDataResponse = (await Task.WhenAll(OnPushEVSEDataLocal.GetInvocationList().
                                                                                                                 Cast<OnPushEVSEDataAPIDelegate>().
                                                                                                                 Select(e => e(Timestamp.Now,
                                                                                                                               this,
                                                                                                                               pullEVSEDataRequest!))))?.FirstOrDefault();

                                                  Counters.PushEVSEData.IncResponses_OK();

                                              }
                                              catch (Exception e)
                                              {

                                                  Counters.PushEVSEData.IncResponses_Error();

                                                  pullEVSEDataResponse = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                                                                             this,
                                                                             new Acknowledgement<PushEVSEDataRequest>(
                                                                                 Timestamp.Now,
                                                                                 Request.EventTrackingId,
                                                                                 processId,
                                                                                 Timestamp.Now - Request.Timestamp,
                                                                                 StatusCode: new StatusCode(
                                                                                                 StatusCodes.DataError,
                                                                                                 e.Message,
                                                                                                 e.StackTrace
                                                                                             ),
                                                                                 pullEVSEDataRequest
                                                                             )
                                                                         );

                                              }
                                          }

                                          if (pullEVSEDataResponse is null)
                                          {

                                              Counters.PushEVSEData.IncResponses_Error();

                                              pullEVSEDataResponse = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                                                                         this,
                                                                         new Acknowledgement<PushEVSEDataRequest>(
                                                                             Timestamp.Now,
                                                                             Request.EventTrackingId,
                                                                             processId,
                                                                             Timestamp.Now - Request.Timestamp,
                                                                             StatusCode: new StatusCode(
                                                                                             StatusCodes.SystemError,
                                                                                             "Could not process the received PushEVSEData request!"
                                                                                         ),
                                                                             pullEVSEDataRequest
                                                                         )
                                                                     );

                                          }

                                          #endregion

                                          #region Send OnPushEVSEDataResponse event

                                          try
                                          {

                                              if (OnPushEVSEDataResponse is not null)
                                                  await Task.WhenAll(OnPushEVSEDataResponse.GetInvocationList().
                                                                     Cast<OnPushEVSEDataAPIResponseDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   pullEVSEDataRequest!,
                                                                                   pullEVSEDataResponse,
                                                                                   Timestamp.Now - startTime))).
                                                                     ConfigureAwait(false);

                                          }
                                          catch (Exception e)
                                          {
                                              DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnPushEVSEDataResponse));
                                          }

                                          #endregion

                                      }
                                      else
                                      {

                                          Counters.PushEVSEData.IncRequests_Error();

                                          pullEVSEDataResponse = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                                                                     this,
                                                                     new Acknowledgement<PushEVSEDataRequest>(
                                                                         Timestamp.Now,
                                                                         Request.EventTrackingId,
                                                                         processId,
                                                                         Timestamp.Now - Request.Timestamp,
                                                                         StatusCode: new StatusCode(
                                                                                         StatusCodes.DataError,
                                                                                         "We could not parse the given PushEVSEData request!",
                                                                                         errorResponse
                                                                                     ),
                                                                         pullEVSEDataRequest
                                                                     )
                                                                 );

                                      }

                                  }
                                  catch (Exception e)
                                  {

                                      Counters.PushEVSEData.IncResponses_Error();

                                      pullEVSEDataResponse = OICPResult<Acknowledgement<PushEVSEDataRequest>>.Failed(
                                                                 this,
                                                                 new Acknowledgement<PushEVSEDataRequest>(
                                                                     Timestamp.Now,
                                                                     Request.EventTrackingId,
                                                                     processId,
                                                                     Timestamp.Now - Request.Timestamp,
                                                                     StatusCode: new StatusCode(
                                                                                     StatusCodes.SystemError,
                                                                                     e.Message,
                                                                                     e.StackTrace
                                                                                 )
                                                                 )
                                                             );

                                  }


                                  return new HTTPResponse.Builder(Request) {
                                             HTTPStatusCode             = HTTPStatusCode.OK,
                                             Server                     = HTTPServer.DefaultServerName,
                                             Date                       = Timestamp.Now,
                                             AccessControlAllowOrigin   = "*",
                                             AccessControlAllowMethods  = [ "POST" ],
                                             AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                             ContentType                = HTTPContentType.Application.JSON_UTF8,
                                             Content                    = pullEVSEDataResponse.Response?.ToJSON(CustomAcknowledgementSerializer,
                                                                                                                CustomStatusCodeSerializer).
                                                                                                         ToString(JSONFormatting).
                                                                                                         ToUTF8Bytes()
                                                                                               ?? [],
                                             ProcessID                  = processId.ToString(),
                                             Connection                 = ConnectionType.Close
                                         }.AsImmutable;

                               }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region POST  ~/api/oicp/evsepull/v21/operators/{operatorId}/status-records

            // -------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/evsepush/v21/operators/DE-GEF/status-records
            // -------------------------------------------------------------------------------------------------------------------------------------
            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.POST,
                              URLPathPrefix + "/api/oicp/evsepush/v21/operators/{operatorId}/status-records",
                              HTTPContentType.Application.JSON_UTF8,
                              HTTPRequestLogger:   logPushEVSEStatusHTTPRequest,
                              HTTPResponseLogger:  logPushEVSEStatusHTTPResponse,
                              HTTPDelegate:        async Request => {

                                  var startTime  = Timestamp.Now;
                                  var processId  = Process_Id.NewRandom();

                                  OICPResult<Acknowledgement<PushEVSEStatusRequest>>? pullEVSEStatusResponse = null;

                                  try
                                  {

                                      #region Try to parse ProviderId URL parameter

                                      if (Request.ParsedURLParameters.Length != 1 || !Operator_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Operator_Id operatorId))
                                      {

                                          Counters.PushEVSEStatus.IncRequests_Error();

                                          pullEVSEStatusResponse = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                                                                       this,
                                                                       new Acknowledgement<PushEVSEStatusRequest>(
                                                                           Timestamp.Now,
                                                                           Request.EventTrackingId,
                                                                           processId,
                                                                           Timestamp.Now - Request.Timestamp,
                                                                           StatusCode: new StatusCode(
                                                                                           StatusCodes.SystemError,
                                                                                           "The expected 'operatorId' URL parameter could not be parsed!"
                                                                                       )
                                                                       )
                                                                   );

                                      }

                                      #endregion

                                      else if (PushEVSEStatusRequest.TryParse(JObject.Parse(Request.HTTPBody?.ToUTF8String() ?? ""),
                                                                              //     operatorId ????
                                                                              out var                             pullEVSEStatusRequest,
                                                                              out var                             errorResponse,
                                                                              ProcessId:                          processId,

                                                                              Timestamp:                          Request.Timestamp,
                                                                              CancellationToken:                  Request.CancellationToken,
                                                                              EventTrackingId:                    Request.EventTrackingId,
                                                                              RequestTimeout:                     Request.Timeout ?? DefaultRequestTimeout,

                                                                              CustomPushEVSEStatusRequestParser:  CustomPushEVSEStatusRequestParser))
                                      {

                                          Counters.PushEVSEStatus.IncRequests_OK();

                                          #region Send OnPushEVSEStatusRequest event

                                          try
                                          {

                                              if (OnPushEVSEStatusRequest is not null)
                                                  await Task.WhenAll(OnPushEVSEStatusRequest.GetInvocationList().
                                                                     Cast<OnPushEVSEStatusAPIRequestDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   pullEVSEStatusRequest!))).
                                                                     ConfigureAwait(false);

                                          }
                                          catch (Exception e)
                                          {
                                              DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnPushEVSEStatusRequest));
                                          }

                                          #endregion

                                          #region Call async subscribers

                                          var OnPushEVSEStatusLocal = OnPushEVSEStatus;
                                          if (OnPushEVSEStatusLocal is not null)
                                          {
                                              try
                                              {

                                                  pullEVSEStatusResponse = (await Task.WhenAll(OnPushEVSEStatusLocal.GetInvocationList().
                                                                                                                     Cast<OnPushEVSEStatusAPIDelegate>().
                                                                                                                     Select(e => e(Timestamp.Now,
                                                                                                                                   this,
                                                                                                                                   pullEVSEStatusRequest!))))?.FirstOrDefault();

                                                  Counters.PushEVSEStatus.IncResponses_OK();

                                              }
                                              catch (Exception e)
                                              {

                                                  Counters.PushEVSEStatus.IncResponses_Error();

                                                  pullEVSEStatusResponse = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                                                                               this,
                                                                               new Acknowledgement<PushEVSEStatusRequest>(
                                                                                   Timestamp.Now,
                                                                                   Request.EventTrackingId,
                                                                                   processId,
                                                                                   Timestamp.Now - Request.Timestamp,
                                                                                   StatusCode: new StatusCode(
                                                                                                   StatusCodes.DataError,
                                                                                                   e.Message,
                                                                                                   e.StackTrace
                                                                                               ),
                                                                                   pullEVSEStatusRequest
                                                                               )
                                                                           );

                                              }
                                          }

                                          if (pullEVSEStatusResponse is null)
                                          {

                                              Counters.PushEVSEStatus.IncResponses_Error();

                                              pullEVSEStatusResponse = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                                                                           this,
                                                                           new Acknowledgement<PushEVSEStatusRequest>(
                                                                               Timestamp.Now,
                                                                               Request.EventTrackingId,
                                                                               processId,
                                                                               Timestamp.Now - Request.Timestamp,
                                                                               StatusCode: new StatusCode(
                                                                                               StatusCodes.SystemError,
                                                                                               "Could not process the received PushEVSEStatus request!"
                                                                                           ),
                                                                               pullEVSEStatusRequest
                                                                           )
                                                                       );

                                          }

                                          #endregion

                                          #region Send OnPushEVSEStatusResponse event

                                          try
                                          {

                                              if (OnPushEVSEStatusResponse is not null)
                                                  await Task.WhenAll(OnPushEVSEStatusResponse.GetInvocationList().
                                                                     Cast<OnPushEVSEStatusAPIResponseDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   pullEVSEStatusRequest!,
                                                                                   pullEVSEStatusResponse,
                                                                                   Timestamp.Now - startTime))).
                                                                     ConfigureAwait(false);

                                          }
                                          catch (Exception e)
                                          {
                                              DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnPushEVSEStatusResponse));
                                          }

                                          #endregion

                                      }
                                      else
                                      {

                                          Counters.PushEVSEStatus.IncRequests_Error();

                                          pullEVSEStatusResponse = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                                                                       this,
                                                                       new Acknowledgement<PushEVSEStatusRequest>(
                                                                           Timestamp.Now,
                                                                           Request.EventTrackingId,
                                                                           processId,
                                                                           Timestamp.Now - Request.Timestamp,
                                                                           StatusCode: new StatusCode(
                                                                                           StatusCodes.DataError,
                                                                                           "We could not parse the given PushEVSEStatus request!",
                                                                                           errorResponse
                                                                                       ),
                                                                           pullEVSEStatusRequest
                                                                       )
                                                                   );

                                      }

                                  }
                                  catch (Exception e)
                                  {

                                      Counters.PushEVSEStatus.IncResponses_Error();

                                      pullEVSEStatusResponse = OICPResult<Acknowledgement<PushEVSEStatusRequest>>.Failed(
                                                                   this,
                                                                   new Acknowledgement<PushEVSEStatusRequest>(
                                                                       Timestamp.Now,
                                                                       Request.EventTrackingId,
                                                                       processId,
                                                                       Timestamp.Now - Request.Timestamp,
                                                                       StatusCode: new StatusCode(
                                                                                       StatusCodes.SystemError,
                                                                                       e.Message,
                                                                                       e.StackTrace
                                                                                   )
                                                                   )
                                                               );

                                  }


                                  return new HTTPResponse.Builder(Request) {
                                             HTTPStatusCode             = HTTPStatusCode.OK,
                                             Server                     = HTTPServer.DefaultServerName,
                                             Date                       = Timestamp.Now,
                                             AccessControlAllowOrigin   = "*",
                                             AccessControlAllowMethods  = [ "POST" ],
                                             AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                             ContentType                = HTTPContentType.Application.JSON_UTF8,
                                             Content                    = pullEVSEStatusResponse.Response?.ToJSON(CustomAcknowledgementSerializer,
                                                                                                                  CustomStatusCodeSerializer).
                                                                                                           ToString(JSONFormatting).
                                                                                                           ToUTF8Bytes()
                                                                                                 ?? [],
                                             ProcessID                  = processId.ToString(),
                                             Connection                 = ConnectionType.Close
                                         }.AsImmutable;

                               }, AllowReplacement: URLReplacement.Allow);

            #endregion


            #region POST  ~/api/oicp/dynamicpricing/v10/operators/{operatorId}/pricing-products

            // -----------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/dynamicpricing/v10/operators/DE*GEF/pricing-products
            // -----------------------------------------------------------------------------------------------------------------------------------
            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.POST,
                              URLPathPrefix + "/api/oicp/dynamicpricing/v10/operators/{operatorId}/pricing-products",
                              HTTPContentType.Application.JSON_UTF8,
                              HTTPRequestLogger:   logPushPricingProductDataHTTPRequest,
                              HTTPResponseLogger:  logPushPricingProductDataHTTPResponse,
                              HTTPDelegate:        async Request => {

                                  var startTime  = Timestamp.Now;
                                  var processId  = Process_Id.NewRandom();

                                  OICPResult<Acknowledgement<PushPricingProductDataRequest>>? pushPricingProductDataResponse = null;

                                  try
                                  {

                                      #region Try to parse ProviderId URL parameter

                                      if (Request.ParsedURLParameters.Length != 1 || !Operator_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Operator_Id operatorId))
                                      {

                                          Counters.PushPricingProductData.IncRequests_Error();

                                          pushPricingProductDataResponse = OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Failed(
                                                                               this,
                                                                               new Acknowledgement<PushPricingProductDataRequest>(
                                                                                   Timestamp.Now,
                                                                                   Request.EventTrackingId,
                                                                                   processId,
                                                                                   Timestamp.Now - Request.Timestamp,
                                                                                   StatusCode: new StatusCode(
                                                                                                   StatusCodes.SystemError,
                                                                                                   "The expected 'operatorId' URL parameter could not be parsed!"
                                                                                               )
                                                                               )
                                                                           );

                                      }

                                      #endregion

                                      else if (PushPricingProductDataRequest.TryParse(JObject.Parse(Request.HTTPBody?.ToUTF8String() ?? ""),
                                                                                      //     operatorId ????
                                                                                      out var                                     pullEVSEDataRequest,
                                                                                      out var                                     errorResponse,
                                                                                      ProcessId:                                  processId,

                                                                                      Timestamp:                                  Request.Timestamp,
                                                                                      CancellationToken:                          Request.CancellationToken,
                                                                                      EventTrackingId:                            Request.EventTrackingId,
                                                                                      RequestTimeout:                             Request.Timeout ?? DefaultRequestTimeout,

                                                                                      CustomPushPricingProductDataRequestParser:  CustomPushPricingProductDataRequestParser))
                                      {

                                          Counters.PushPricingProductData.IncRequests_OK();

                                          #region Send OnPushPricingProductDataRequest event

                                          try
                                          {

                                              if (OnPushPricingProductDataRequest is not null)
                                                  await Task.WhenAll(OnPushPricingProductDataRequest.GetInvocationList().
                                                                     Cast<OnPushPricingProductDataAPIRequestDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   pullEVSEDataRequest!))).
                                                                     ConfigureAwait(false);

                                          }
                                          catch (Exception e)
                                          {
                                              DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnPushPricingProductDataRequest));
                                          }

                                          #endregion

                                          #region Call async subscribers

                                          var OnPushPricingProductDataLocal = OnPushPricingProductData;
                                          if (OnPushPricingProductDataLocal is not null)
                                          {
                                              try
                                              {

                                                  pushPricingProductDataResponse = (await Task.WhenAll(OnPushPricingProductDataLocal.GetInvocationList().
                                                                                                                                     Cast<OnPushPricingProductDataAPIDelegate>().
                                                                                                                                     Select(e => e(Timestamp.Now,
                                                                                                                                                   this,
                                                                                                                                                   pullEVSEDataRequest!))))?.FirstOrDefault();

                                                  Counters.PushPricingProductData.IncResponses_OK();

                                              }
                                              catch (Exception e)
                                              {

                                                  Counters.PushPricingProductData.IncResponses_Error();

                                                  pushPricingProductDataResponse = OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Failed(
                                                                                       this,
                                                                                       new Acknowledgement<PushPricingProductDataRequest>(
                                                                                           Timestamp.Now,
                                                                                           Request.EventTrackingId,
                                                                                           processId,
                                                                                           Timestamp.Now - Request.Timestamp,
                                                                                           StatusCode: new StatusCode(
                                                                                                           StatusCodes.DataError,
                                                                                                           e.Message,
                                                                                                           e.StackTrace
                                                                                                       ),
                                                                                           pullEVSEDataRequest
                                                                                       )
                                                                                   );

                                              }
                                          }

                                          if (pushPricingProductDataResponse is null)
                                          {

                                              Counters.PushPricingProductData.IncResponses_Error();

                                              pushPricingProductDataResponse = OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Failed(
                                                                                   this,
                                                                                   new Acknowledgement<PushPricingProductDataRequest>(
                                                                                       Timestamp.Now,
                                                                                       Request.EventTrackingId,
                                                                                       processId,
                                                                                       Timestamp.Now - Request.Timestamp,
                                                                                       StatusCode: new StatusCode(
                                                                                                       StatusCodes.SystemError,
                                                                                                       "Could not process the received PushPricingProductData request!"
                                                                                                   ),
                                                                                       pullEVSEDataRequest
                                                                                   )
                                                                               );

                                          }

                                          #endregion

                                          #region Send OnPushPricingProductDataResponse event

                                          try
                                          {

                                              if (OnPushPricingProductDataResponse is not null)
                                                  await Task.WhenAll(OnPushPricingProductDataResponse.GetInvocationList().
                                                                     Cast<OnPushPricingProductDataAPIResponseDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   pullEVSEDataRequest!,
                                                                                   pushPricingProductDataResponse,
                                                                                   Timestamp.Now - startTime))).
                                                                     ConfigureAwait(false);

                                          }
                                          catch (Exception e)
                                          {
                                              DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnPushPricingProductDataResponse));
                                          }

                                          #endregion

                                      }
                                      else
                                      {

                                          Counters.PushPricingProductData.IncRequests_Error();

                                          pushPricingProductDataResponse = OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Failed(
                                                                               this,
                                                                               new Acknowledgement<PushPricingProductDataRequest>(
                                                                                   Timestamp.Now,
                                                                                   Request.EventTrackingId,
                                                                                   processId,
                                                                                   Timestamp.Now - Request.Timestamp,
                                                                                   StatusCode: new StatusCode(
                                                                                                   StatusCodes.DataError,
                                                                                                   "We could not parse the given PushPricingProductData request!",
                                                                                                   errorResponse
                                                                                               ),
                                                                                   pullEVSEDataRequest
                                                                               )
                                                                           );

                                      }

                                  }
                                  catch (Exception e)
                                  {

                                      Counters.PushPricingProductData.IncResponses_Error();

                                      pushPricingProductDataResponse = OICPResult<Acknowledgement<PushPricingProductDataRequest>>.Failed(
                                                                           this,
                                                                           new Acknowledgement<PushPricingProductDataRequest>(
                                                                               Timestamp.Now,
                                                                               Request.EventTrackingId,
                                                                               processId,
                                                                               Timestamp.Now - Request.Timestamp,
                                                                               StatusCode: new StatusCode(
                                                                                               StatusCodes.SystemError,
                                                                                               e.Message,
                                                                                               e.StackTrace
                                                                                           )
                                                                           )
                                                                       );

                                  }


                                  return new HTTPResponse.Builder(Request) {
                                             HTTPStatusCode             = HTTPStatusCode.OK,
                                             Server                     = HTTPServer.DefaultServerName,
                                             Date                       = Timestamp.Now,
                                             AccessControlAllowOrigin   = "*",
                                             AccessControlAllowMethods  = [ "POST" ],
                                             AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                             ContentType                = HTTPContentType.Application.JSON_UTF8,
                                             Content                    = pushPricingProductDataResponse.Response?.ToJSON(CustomAcknowledgementSerializer,
                                                                                                                          CustomStatusCodeSerializer).
                                                                                                                   ToString(JSONFormatting).
                                                                                                                   ToUTF8Bytes()
                                                                                                         ?? [],
                                             ProcessID                  = processId.ToString(),
                                             Connection                 = ConnectionType.Close
                                         }.AsImmutable;

                               }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region POST  ~/api/oicp/dynamicpricing/v10/operators/{operatorId}/evse-pricing

            // -----------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/dynamicpricing/v10/operators/DE-GEF/evse-pricing
            // -----------------------------------------------------------------------------------------------------------------------------------
            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.POST,
                              URLPathPrefix + "/api/oicp/dynamicpricing/v10/operators/{operatorId}/evse-pricing",
                              HTTPContentType.Application.JSON_UTF8,
                              HTTPRequestLogger:   logPushEVSEPricingHTTPRequest,
                              HTTPResponseLogger:  logPushEVSEPricingHTTPResponse,
                              HTTPDelegate:        async Request => {

                                  var startTime  = Timestamp.Now;
                                  var processId  = Process_Id.NewRandom();

                                  OICPResult<Acknowledgement<PushEVSEPricingRequest>>? pushEVSEPricingResponse = null;

                                  try
                                  {

                                      #region Try to parse ProviderId URL parameter

                                      if (Request.ParsedURLParameters.Length != 1 || !Operator_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Operator_Id operatorId))
                                      {

                                          Counters.PushEVSEPricing.IncRequests_Error();

                                          pushEVSEPricingResponse = OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Failed(
                                                                        this,
                                                                        new Acknowledgement<PushEVSEPricingRequest>(
                                                                            Timestamp.Now,
                                                                            Request.EventTrackingId,
                                                                            processId,
                                                                            Timestamp.Now - Request.Timestamp,
                                                                            StatusCode: new StatusCode(
                                                                                            StatusCodes.SystemError,
                                                                                            "The expected 'operatorId' URL parameter could not be parsed!"
                                                                                        )
                                                                        )
                                                                    );

                                      }

                                      #endregion

                                      else if (PushEVSEPricingRequest.TryParse(JObject.Parse(Request.HTTPBody?.ToUTF8String() ?? ""),
                                                                               operatorId,
                                                                               out var                              pullEVSEDataRequest,
                                                                               out var                              errorResponse,
                                                                               ProcessId:                           processId,

                                                                               Timestamp:                           Request.Timestamp,
                                                                               CancellationToken:                   Request.CancellationToken,
                                                                               EventTrackingId:                     Request.EventTrackingId,
                                                                               RequestTimeout:                      Request.Timeout ?? DefaultRequestTimeout,

                                                                               CustomPushEVSEPricingRequestParser:  CustomPushEVSEPricingRequestParser))
                                      {

                                          Counters.PushEVSEPricing.IncRequests_OK();

                                          #region Send OnPushEVSEPricingRequest event

                                          try
                                          {

                                              if (OnPushEVSEPricingRequest is not null)
                                                  await Task.WhenAll(OnPushEVSEPricingRequest.GetInvocationList().
                                                                     Cast<OnPushEVSEPricingAPIRequestDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   pullEVSEDataRequest!))).
                                                                     ConfigureAwait(false);

                                          }
                                          catch (Exception e)
                                          {
                                              DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnPushEVSEPricingRequest));
                                          }

                                          #endregion

                                          #region Call async subscribers

                                          var OnPushEVSEPricingLocal = OnPushEVSEPricing;
                                          if (OnPushEVSEPricingLocal is not null)
                                          {
                                              try
                                              {

                                                  pushEVSEPricingResponse = (await Task.WhenAll(OnPushEVSEPricingLocal.GetInvocationList().
                                                                                                                       Cast<OnPushEVSEPricingAPIDelegate>().
                                                                                                                       Select(e => e(Timestamp.Now,
                                                                                                                                     this,
                                                                                                                                     pullEVSEDataRequest!))))?.FirstOrDefault();

                                                  Counters.PushEVSEPricing.IncResponses_OK();

                                              }
                                              catch (Exception e)
                                              {

                                                  Counters.PushEVSEPricing.IncResponses_Error();

                                                  pushEVSEPricingResponse = OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Failed(
                                                                                this,
                                                                                new Acknowledgement<PushEVSEPricingRequest>(
                                                                                    Timestamp.Now,
                                                                                    Request.EventTrackingId,
                                                                                    processId,
                                                                                    Timestamp.Now - Request.Timestamp,
                                                                                    StatusCode: new StatusCode(
                                                                                                    StatusCodes.DataError,
                                                                                                    e.Message,
                                                                                                    e.StackTrace
                                                                                                ),
                                                                                    pullEVSEDataRequest!
                                                                                )
                                                                            );

                                              }
                                          }

                                          if (pushEVSEPricingResponse is null)
                                          {

                                              Counters.PushEVSEPricing.IncResponses_Error();

                                              pushEVSEPricingResponse = OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Failed(
                                                                            this,
                                                                            new Acknowledgement<PushEVSEPricingRequest>(
                                                                                Timestamp.Now,
                                                                                Request.EventTrackingId,
                                                                                processId,
                                                                                Timestamp.Now - Request.Timestamp,
                                                                                StatusCode: new StatusCode(
                                                                                                StatusCodes.SystemError,
                                                                                                "Could not process the received PushEVSEPricing request!"
                                                                                            ),
                                                                                pullEVSEDataRequest!
                                                                            )
                                                                        );

                                          }

                                          #endregion

                                          #region Send OnPushEVSEPricingResponse event

                                          try
                                          {

                                              if (OnPushEVSEPricingResponse is not null)
                                                  await Task.WhenAll(OnPushEVSEPricingResponse.GetInvocationList().
                                                                     Cast<OnPushEVSEPricingAPIResponseDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   pullEVSEDataRequest!,
                                                                                   pushEVSEPricingResponse,
                                                                                   Timestamp.Now - startTime))).
                                                                     ConfigureAwait(false);

                                          }
                                          catch (Exception e)
                                          {
                                              DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnPushEVSEPricingResponse));
                                          }

                                          #endregion

                                      }
                                      else
                                      {

                                          Counters.PushEVSEPricing.IncRequests_Error();

                                          pushEVSEPricingResponse = OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Failed(
                                                                        this,
                                                                        new Acknowledgement<PushEVSEPricingRequest>(
                                                                            Timestamp.Now,
                                                                            Request.EventTrackingId,
                                                                            processId,
                                                                            Timestamp.Now - Request.Timestamp,
                                                                            StatusCode: new StatusCode(
                                                                                            StatusCodes.DataError,
                                                                                            "We could not parse the given PushEVSEPricing request!",
                                                                                            errorResponse
                                                                                        ),
                                                                            pullEVSEDataRequest!
                                                                        )
                                                                    );

                                      }

                                  }
                                  catch (Exception e)
                                  {

                                      Counters.PushEVSEPricing.IncResponses_Error();

                                      pushEVSEPricingResponse = OICPResult<Acknowledgement<PushEVSEPricingRequest>>.Failed(
                                                                    this,
                                                                    new Acknowledgement<PushEVSEPricingRequest>(
                                                                        Timestamp.Now,
                                                                        Request.EventTrackingId,
                                                                        processId,
                                                                        Timestamp.Now - Request.Timestamp,
                                                                        StatusCode: new StatusCode(
                                                                                        StatusCodes.SystemError,
                                                                                        e.Message,
                                                                                        e.StackTrace
                                                                                    )
                                                                    )
                                                                );

                                  }


                                  return new HTTPResponse.Builder(Request) {
                                             HTTPStatusCode             = HTTPStatusCode.OK,
                                             Server                     = HTTPServer.DefaultServerName,
                                             Date                       = Timestamp.Now,
                                             AccessControlAllowOrigin   = "*",
                                             AccessControlAllowMethods  = [ "POST" ],
                                             AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                             ContentType                = HTTPContentType.Application.JSON_UTF8,
                                             Content                    = pushEVSEPricingResponse.Response?.ToJSON(CustomAcknowledgementSerializer,
                                                                                                                   CustomStatusCodeSerializer).
                                                                                                            ToString(JSONFormatting).
                                                                                                            ToUTF8Bytes()
                                                                                                  ?? [],
                                             ProcessID                  = processId.ToString(),
                                             Connection                 = ConnectionType.Close
                                         }.AsImmutable;

                               }, AllowReplacement: URLReplacement.Allow);

            #endregion


            #region POST  ~/api/oicp/authdata/v21/operators/{operatorId}/pull-request

            // -----------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/authdata/v21/operators/DE*GEF/pull-request
            // -----------------------------------------------------------------------------------------------------------------------------------
            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.POST,
                              URLPathPrefix + "/api/oicp/authdata/v21/operators/{operatorId}/pull-request",
                              HTTPContentType.Application.JSON_UTF8,
                              HTTPRequestLogger:   logPullAuthenticationDataHTTPRequest,
                              HTTPResponseLogger:  logPullAuthenticationDataHTTPResponse,
                              HTTPDelegate:        async Request => {

                                  var startTime  = Timestamp.Now;
                                  var processId  = Process_Id.NewRandom();

                                  OICPResult<PullAuthenticationDataResponse>? pullAuthenticationDataResponse = null;

                                  try
                                  {

                                      #region Try to parse ProviderId URL parameter

                                      if (Request.ParsedURLParameters.Length != 1 || !Operator_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Operator_Id operatorId))
                                      {

                                          Counters.PullAuthenticationData.IncRequests_Error();

                                          pullAuthenticationDataResponse = OICPResult<PullAuthenticationDataResponse>.Failed(
                                                                               this,
                                                                               new PullAuthenticationDataResponse(
                                                                                   Timestamp.Now,
                                                                                   Request.EventTrackingId,
                                                                                   processId,
                                                                                   Timestamp.Now - Request.Timestamp,
                                                                                   [],
                                                                                   StatusCode: new StatusCode(
                                                                                                   StatusCodes.SystemError,
                                                                                                   "The expected 'operatorId' URL parameter could not be parsed!"
                                                                                               )
                                                                               )
                                                                           );

                                      }

                                      #endregion

                                      else if (PullAuthenticationDataRequest.TryParse(JObject.Parse(Request.HTTPBody?.ToUTF8String() ?? ""),
                                                                                      //operatorId,
                                                                                      out var                                     pullAuthenticationDataRequest,
                                                                                      out var                                     errorResponse,
                                                                                      ProcessId:                                  processId,

                                                                                      Timestamp:                                  Request.Timestamp,
                                                                                      CancellationToken:                          Request.CancellationToken,
                                                                                      EventTrackingId:                            Request.EventTrackingId,
                                                                                      RequestTimeout:                             Request.Timeout ?? DefaultRequestTimeout,

                                                                                      CustomPullAuthenticationDataRequestParser:  CustomPullAuthenticationDataRequestParser))
                                      {

                                          Counters.PullAuthenticationData.IncRequests_OK();

                                          #region Send OnPullAuthenticationDataRequest event

                                          try
                                          {

                                              if (OnPullAuthenticationDataRequest is not null)
                                                  await Task.WhenAll(OnPullAuthenticationDataRequest.GetInvocationList().
                                                                     Cast<OnPullAuthenticationDataAPIRequestDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   pullAuthenticationDataRequest!))).
                                                                     ConfigureAwait(false);

                                          }
                                          catch (Exception e)
                                          {
                                              DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnPullAuthenticationDataRequest));
                                          }

                                          #endregion

                                          #region Call async subscribers

                                          var OnPullAuthenticationDataLocal = OnPullAuthenticationData;
                                          if (OnPullAuthenticationDataLocal is not null)
                                          {
                                              try
                                              {

                                                  pullAuthenticationDataResponse = (await Task.WhenAll(OnPullAuthenticationDataLocal.GetInvocationList().
                                                                                                                                     Cast<OnPullAuthenticationDataAPIDelegate>().
                                                                                                                                     Select(e => e(Timestamp.Now,
                                                                                                                                                   this,
                                                                                                                                                   pullAuthenticationDataRequest!))))?.FirstOrDefault();

                                                  Counters.PullAuthenticationData.IncResponses_OK();

                                              }
                                              catch (Exception e)
                                              {

                                                  Counters.PullAuthenticationData.IncResponses_Error();

                                                  pullAuthenticationDataResponse = OICPResult<PullAuthenticationDataResponse>.Failed(
                                                                                       this,
                                                                                       new PullAuthenticationDataResponse(
                                                                                           Timestamp.Now,
                                                                                           Request.EventTrackingId,
                                                                                           processId,
                                                                                           Timestamp.Now - Request.Timestamp,
                                                                                           [],
                                                                                           pullAuthenticationDataRequest,
                                                                                           StatusCode: new StatusCode(
                                                                                                           StatusCodes.DataError,
                                                                                                           e.Message,
                                                                                                           e.StackTrace
                                                                                                       )
                                                                                       )
                                                                                   );

                                              }
                                          }

                                          if (pullAuthenticationDataResponse is null)
                                          {

                                              Counters.PullAuthenticationData.IncResponses_Error();

                                              pullAuthenticationDataResponse = OICPResult<PullAuthenticationDataResponse>.Failed(
                                                                                   this,
                                                                                   new PullAuthenticationDataResponse(
                                                                                       Timestamp.Now,
                                                                                       Request.EventTrackingId,
                                                                                       processId,
                                                                                       Timestamp.Now - Request.Timestamp,
                                                                                       [],
                                                                                       pullAuthenticationDataRequest,
                                                                                       StatusCode: new StatusCode(
                                                                                                       StatusCodes.SystemError,
                                                                                                       "Could not process the received PullAuthenticationData request!"
                                                                                                   )
                                                                                   )
                                                                               );

                                          }

                                          #endregion

                                          #region Send OnPullAuthenticationDataResponse event

                                          try
                                          {

                                              if (OnPullAuthenticationDataResponse is not null)
                                                  await Task.WhenAll(OnPullAuthenticationDataResponse.GetInvocationList().
                                                                     Cast<OnPullAuthenticationDataAPIResponseDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   pullAuthenticationDataRequest!,
                                                                                   pullAuthenticationDataResponse,
                                                                                   Timestamp.Now - startTime))).
                                                                     ConfigureAwait(false);

                                          }
                                          catch (Exception e)
                                          {
                                              DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnPullAuthenticationDataResponse));
                                          }

                                          #endregion

                                      }
                                      else
                                      {

                                          Counters.PullAuthenticationData.IncRequests_Error();

                                          pullAuthenticationDataResponse = OICPResult<PullAuthenticationDataResponse>.Failed(
                                                                               this,
                                                                               new PullAuthenticationDataResponse(
                                                                                   Timestamp.Now,
                                                                                   Request.EventTrackingId,
                                                                                   processId,
                                                                                   Timestamp.Now - Request.Timestamp,
                                                                                   [],
                                                                                   pullAuthenticationDataRequest,
                                                                                   StatusCode: new StatusCode(
                                                                                                   StatusCodes.DataError,
                                                                                                   "We could not parse the given PullAuthenticationData request!",
                                                                                                   errorResponse
                                                                                               )
                                                                               )
                                                                           );

                                      }

                                  }
                                  catch (Exception e)
                                  {

                                      Counters.PullAuthenticationData.IncResponses_Error();

                                      pullAuthenticationDataResponse = OICPResult<PullAuthenticationDataResponse>.Failed(
                                                                           this,
                                                                           new PullAuthenticationDataResponse(
                                                                               Timestamp.Now,
                                                                               Request.EventTrackingId,
                                                                               processId,
                                                                               Timestamp.Now - Request.Timestamp,
                                                                               [],
                                                                               StatusCode: new StatusCode(
                                                                                               StatusCodes.SystemError,
                                                                                               e.Message,
                                                                                               e.StackTrace
                                                                                           )
                                                                           )
                                                                       );

                                  }


                                  return new HTTPResponse.Builder(Request) {
                                             HTTPStatusCode             = HTTPStatusCode.OK,
                                             Server                     = HTTPServer.DefaultServerName,
                                             Date                       = Timestamp.Now,
                                             AccessControlAllowOrigin   = "*",
                                             AccessControlAllowMethods  = [ "POST" ],
                                             AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                             ContentType                = HTTPContentType.Application.JSON_UTF8,
                                             Content                    = pullAuthenticationDataResponse.Response?.ToJSON(CustomPullAuthenticationDataResponseSerializer,
                                                                                                                          CustomProviderAuthenticationDataSerializer,
                                                                                                                          CustomIdentificationSerializer,
                                                                                                                          CustomStatusCodeSerializer).
                                                                                                                   ToString(JSONFormatting).
                                                                                                                   ToUTF8Bytes()
                                                                                                         ?? [],
                                             ProcessID                  = processId.ToString(),
                                             Connection                 = ConnectionType.Close
                                         }.AsImmutable;

                               }, AllowReplacement: URLReplacement.Allow);

            #endregion


            #region POST  ~/api/oicp/charging/v21/operators/{operatorId}/authorize/start

            // --------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/charging/v21/operators/DE*GEF/authorize/start
            // --------------------------------------------------------------------------------------------------------------------------------------
            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.POST,
                              URLPathPrefix + "/api/oicp/charging/v21/operators/{operatorId}/authorize/start",
                              HTTPContentType.Application.JSON_UTF8,
                              HTTPRequestLogger:   logAuthorizeStartHTTPRequest,
                              HTTPResponseLogger:  logAuthorizationStartHTTPResponse,
                              HTTPDelegate:        async Request => {

                                  var startTime  = Timestamp.Now;
                                  var processId  = Process_Id.NewRandom();

                                  OICPResult<AuthorizationStartResponse>? authorizationStartResponse = null;

                                  try
                                  {

                                      #region Try to parse ProviderId URL parameter

                                      if (Request.ParsedURLParameters.Length != 1 || !Operator_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Operator_Id operatorId))
                                      {

                                          Counters.AuthorizeStart.IncRequests_Error();

                                          authorizationStartResponse = OICPResult<AuthorizationStartResponse>.Failed(
                                                                           this,
                                                                           AuthorizationStartResponse.SystemError(
                                                                               null,
                                                                               "The expected 'operatorId' URL parameter could not be parsed!",
                                                                               ResponseTimestamp:  Timestamp.Now,
                                                                               EventTrackingId:    Request.EventTrackingId,
                                                                               Runtime:            Timestamp.Now - Request.Timestamp
                                                                           )
                                                                       );

                                      }

                                      #endregion

                                      else if (AuthorizeStartRequest.TryParse(JObject.Parse(Request.HTTPBody?.ToUTF8String() ?? ""),
                                                                              operatorId,
                                                                              out var                             authorizeStartRequest,
                                                                              out var                             errorResponse,
                                                                              ProcessId:                          processId,

                                                                              Timestamp:                          Request.Timestamp,
                                                                              CancellationToken:                  Request.CancellationToken,
                                                                              EventTrackingId:                    Request.EventTrackingId,
                                                                              RequestTimeout:                     Request.Timeout ?? DefaultRequestTimeout,

                                                                              CustomAuthorizeStartRequestParser:  CustomAuthorizeStartRequestParser))
                                      {

                                          Counters.AuthorizeStart.IncRequests_OK();

                                          #region Send OnAuthorizeStartRequest event

                                          try
                                          {

                                              if (OnAuthorizeStartRequest is not null)
                                                  await Task.WhenAll(OnAuthorizeStartRequest.GetInvocationList().
                                                                     Cast<OnAuthorizeStartAPIRequestDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   authorizeStartRequest!))).
                                                                     ConfigureAwait(false);

                                          }
                                          catch (Exception e)
                                          {
                                              DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnAuthorizeStartRequest));
                                          }

                                          #endregion

                                          #region Call async subscribers

                                          var OnAuthorizeStartLocal = OnAuthorizeStart;
                                          if (OnAuthorizeStartLocal is not null)
                                          {
                                              try
                                              {

                                                  authorizationStartResponse = (await Task.WhenAll(OnAuthorizeStartLocal.GetInvocationList().
                                                                                                                         Cast<OnAuthorizeStartAPIDelegate>().
                                                                                                                         Select(e => e(Timestamp.Now,
                                                                                                                                       this,
                                                                                                                                       authorizeStartRequest!))))?.FirstOrDefault();

                                                  Counters.AuthorizeStart.IncResponses_OK();

                                              }
                                              catch (Exception e)
                                              {

                                                  Counters.AuthorizeStart.IncResponses_Error();

                                                  authorizationStartResponse = OICPResult<AuthorizationStartResponse>.Failed(
                                                                                   this,
                                                                                   AuthorizationStartResponse.DataError(
                                                                                       authorizeStartRequest,
                                                                                       e.Message,
                                                                                       e.StackTrace,
                                                                                       ResponseTimestamp:  Timestamp.Now,
                                                                                       EventTrackingId:    Request.EventTrackingId,
                                                                                       ProcessId:          processId,
                                                                                       Runtime:            Timestamp.Now - Request.Timestamp
                                                                                   )
                                                                               );

                                              }
                                          }

                                          if (authorizationStartResponse is null)
                                          {

                                              Counters.AuthorizeStart.IncResponses_Error();

                                              authorizationStartResponse = OICPResult<AuthorizationStartResponse>.Failed(
                                                                               this,
                                                                               AuthorizationStartResponse.SystemError(
                                                                                   authorizeStartRequest,
                                                                                   "Could not process the received AuthorizeStart request!",
                                                                                   ResponseTimestamp:  Timestamp.Now,
                                                                                   EventTrackingId:    Request.EventTrackingId,
                                                                                   ProcessId:          processId,
                                                                                   Runtime:            Timestamp.Now - Request.Timestamp
                                                                               )
                                                                           );

                                          }

                                          #endregion

                                          #region Send OnAuthorizeStartResponse event

                                          try
                                          {

                                              if (OnAuthorizeStartResponse is not null)
                                                  await Task.WhenAll(OnAuthorizeStartResponse.GetInvocationList().
                                                                     Cast<OnAuthorizeStartAPIResponseDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   authorizeStartRequest!,
                                                                                   authorizationStartResponse,
                                                                                   Timestamp.Now - startTime))).
                                                                     ConfigureAwait(false);

                                          }
                                          catch (Exception e)
                                          {
                                              DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnAuthorizeStartResponse));
                                          }

                                          #endregion

                                      }
                                      else
                                      {

                                          Counters.AuthorizeStart.IncRequests_Error();

                                          authorizationStartResponse = OICPResult<AuthorizationStartResponse>.Failed(
                                                                           this,
                                                                           AuthorizationStartResponse.DataError(
                                                                               authorizeStartRequest,
                                                                               "We could not parse the given AuthorizeStart request!",
                                                                               errorResponse,
                                                                               ResponseTimestamp:  Timestamp.Now,
                                                                               EventTrackingId:    Request.EventTrackingId,
                                                                               ProcessId:          processId,
                                                                               Runtime:            Timestamp.Now - Request.Timestamp
                                                                           )
                                                                       );

                                      }

                                  }
                                  catch (Exception e)
                                  {

                                      Counters.AuthorizeStart.IncResponses_Error();

                                      authorizationStartResponse = OICPResult<AuthorizationStartResponse>.Failed(
                                                                       this,
                                                                       AuthorizationStartResponse.SystemError(
                                                                           null,
                                                                           e.Message,
                                                                           e.StackTrace,
                                                                           ResponseTimestamp:  Timestamp.Now,
                                                                           EventTrackingId:    Request.EventTrackingId,
                                                                           ProcessId:          processId,
                                                                           Runtime:            Timestamp.Now - Request.Timestamp
                                                                       )
                                                                   );

                                  }


                                  return new HTTPResponse.Builder(Request) {
                                             HTTPStatusCode             = HTTPStatusCode.OK,
                                             Server                     = HTTPServer.DefaultServerName,
                                             Date                       = Timestamp.Now,
                                             AccessControlAllowOrigin   = "*",
                                             AccessControlAllowMethods  = [ "POST" ],
                                             AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                             ContentType                = HTTPContentType.Application.JSON_UTF8,
                                             Content                    = authorizationStartResponse.Response?.ToJSON(CustomAuthorizationStartSerializer,
                                                                                                                      CustomStatusCodeSerializer,
                                                                                                                      CustomIdentificationSerializer).
                                                                                                               ToString(JSONFormatting).
                                                                                                               ToUTF8Bytes()
                                                                                                     ?? [],
                                             ProcessID                  = processId.ToString(),
                                             Connection                 = ConnectionType.Close
                                         }.AsImmutable;

                               }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region POST  ~/api/oicp/charging/v21/operators/{operatorId}/authorize/stop

            // --------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/charging/v21/operators/DE*GEF/authorize/stop
            // --------------------------------------------------------------------------------------------------------------------------------------
            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.POST,
                              URLPathPrefix + "/api/oicp/charging/v21/operators/{operatorId}/authorize/stop",
                              HTTPContentType.Application.JSON_UTF8,
                              HTTPRequestLogger:   logAuthorizeStopHTTPRequest,
                              HTTPResponseLogger:  logAuthorizationStopHTTPResponse,
                              HTTPDelegate:        async Request => {

                                  var startTime  = Timestamp.Now;
                                  var processId  = Process_Id.NewRandom();

                                  OICPResult<AuthorizationStopResponse>? authorizationStopResponse = null;

                                  try
                                  {

                                      #region Try to parse ProviderId URL parameter

                                      if (Request.ParsedURLParameters.Length != 1 || !Operator_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Operator_Id operatorId))
                                      {

                                          Counters.AuthorizeStop.IncRequests_Error();

                                          authorizationStopResponse = OICPResult<AuthorizationStopResponse>.Failed(
                                                                          this,
                                                                          AuthorizationStopResponse.SystemError(
                                                                              null,
                                                                              "The expected 'operatorId' URL parameter could not be parsed!",
                                                                              ResponseTimestamp:  Timestamp.Now,
                                                                              EventTrackingId:    Request.EventTrackingId,
                                                                              Runtime:            Timestamp.Now - Request.Timestamp
                                                                          )
                                                                      );

                                      }

                                      #endregion

                                      else if (AuthorizeStopRequest.TryParse(JObject.Parse(Request.HTTPBody?.ToUTF8String() ?? ""),
                                                                             operatorId,
                                                                             out var                            authorizeStopRequest,
                                                                             out var                            errorResponse,
                                                                             ProcessId:                         processId,

                                                                             Timestamp:                         Request.Timestamp,
                                                                             CancellationToken:                 Request.CancellationToken,
                                                                             EventTrackingId:                   Request.EventTrackingId,
                                                                             RequestTimeout:                    Request.Timeout ?? DefaultRequestTimeout,

                                                                             CustomAuthorizeStopRequestParser:  CustomAuthorizeStopRequestParser))
                                      {

                                          Counters.AuthorizeStop.IncRequests_OK();

                                          #region Send OnAuthorizeStopRequest event

                                          try
                                          {

                                              if (OnAuthorizeStopRequest is not null)
                                                  await Task.WhenAll(OnAuthorizeStopRequest.GetInvocationList().
                                                                     Cast<OnAuthorizeStopAPIRequestDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   authorizeStopRequest!))).
                                                                     ConfigureAwait(false);

                                          }
                                          catch (Exception e)
                                          {
                                              DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnAuthorizeStopRequest));
                                          }

                                          #endregion

                                          #region Call async subscribers

                                          var OnAuthorizeStopLocal = OnAuthorizeStop;
                                          if (OnAuthorizeStopLocal is not null)
                                          {
                                              try
                                              {

                                                  authorizationStopResponse = (await Task.WhenAll(OnAuthorizeStopLocal.GetInvocationList().
                                                                                                                       Cast<OnAuthorizeStopAPIDelegate>().
                                                                                                                       Select(e => e(Timestamp.Now,
                                                                                                                                     this,
                                                                                                                                     authorizeStopRequest!))))?.FirstOrDefault();

                                                  Counters.AuthorizeStop.IncResponses_OK();

                                              }
                                              catch (Exception e)
                                              {

                                                  Counters.AuthorizeStop.IncResponses_Error();

                                                  authorizationStopResponse = OICPResult<AuthorizationStopResponse>.Failed(
                                                                                  this,
                                                                                  AuthorizationStopResponse.DataError(
                                                                                      authorizeStopRequest,
                                                                                      e.Message,
                                                                                      e.StackTrace,
                                                                                      ResponseTimestamp:  Timestamp.Now,
                                                                                      EventTrackingId:    Request.EventTrackingId,
                                                                                      ProcessId:          processId,
                                                                                      Runtime:            Timestamp.Now - Request.Timestamp
                                                                                  )
                                                                              );

                                              }
                                          }

                                          if (authorizationStopResponse is null)
                                          {

                                              Counters.AuthorizeStop.IncResponses_Error();

                                              authorizationStopResponse = OICPResult<AuthorizationStopResponse>.Failed(
                                                                              this,
                                                                              AuthorizationStopResponse.SystemError(
                                                                                  authorizeStopRequest,
                                                                                  "Could not process the received AuthorizeStop request!",
                                                                                  ResponseTimestamp: Timestamp.Now,
                                                                                  EventTrackingId: Request.EventTrackingId,
                                                                                  ProcessId: processId,
                                                                                  Runtime: Timestamp.Now - Request.Timestamp
                                                                              )
                                                                          );

                                          }

                                          #endregion

                                          #region Send OnAuthorizeStopResponse event

                                          try
                                          {

                                              if (OnAuthorizeStopResponse is not null)
                                                  await Task.WhenAll(OnAuthorizeStopResponse.GetInvocationList().
                                                                     Cast<OnAuthorizeStopAPIResponseDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   authorizeStopRequest!,
                                                                                   authorizationStopResponse,
                                                                                   Timestamp.Now - startTime))).
                                                                     ConfigureAwait(false);

                                          }
                                          catch (Exception e)
                                          {
                                              DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnAuthorizeStopResponse));
                                          }

                                          #endregion

                                      }
                                      else
                                      {

                                          Counters.AuthorizeStop.IncRequests_Error();

                                          authorizationStopResponse = OICPResult<AuthorizationStopResponse>.Failed(
                                                                          this,
                                                                          AuthorizationStopResponse.DataError(
                                                                              authorizeStopRequest,
                                                                              "We could not parse the given AuthorizeStop request!",
                                                                              errorResponse,
                                                                              ResponseTimestamp:  Timestamp.Now,
                                                                              EventTrackingId:    Request.EventTrackingId,
                                                                              ProcessId:          processId,
                                                                              Runtime:            Timestamp.Now - Request.Timestamp
                                                                          )
                                                                      );

                                      }

                                  }
                                  catch (Exception e)
                                  {

                                      Counters.AuthorizeStop.IncResponses_Error();

                                      authorizationStopResponse = OICPResult<AuthorizationStopResponse>.Failed(
                                                                      this,
                                                                      AuthorizationStopResponse.SystemError(
                                                                          null,
                                                                          e.Message,
                                                                          e.StackTrace,
                                                                          ResponseTimestamp:  Timestamp.Now,
                                                                          EventTrackingId:    Request.EventTrackingId,
                                                                          ProcessId:          processId,
                                                                          Runtime:            Timestamp.Now - Request.Timestamp
                                                                      )
                                                                  );

                                  }


                                  return new HTTPResponse.Builder(Request) {
                                             HTTPStatusCode             = HTTPStatusCode.OK,
                                             Server                     = HTTPServer.DefaultServerName,
                                             Date                       = Timestamp.Now,
                                             AccessControlAllowOrigin   = "*",
                                             AccessControlAllowMethods  = [ "POST" ],
                                             AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                             ContentType                = HTTPContentType.Application.JSON_UTF8,
                                             Content                    = authorizationStopResponse.Response?.ToJSON(CustomAuthorizationStopSerializer,
                                                                                                                     CustomStatusCodeSerializer).
                                                                                                              ToString(JSONFormatting).
                                                                                                              ToUTF8Bytes()
                                                                                                    ?? [],
                                             ProcessID                  = processId.ToString(),
                                             Connection                 = ConnectionType.Close
                                         }.AsImmutable;

                               }, AllowReplacement: URLReplacement.Allow);

            #endregion


            #region POST  ~/api/oicp/notificationmgmt/v11/charging-notifications

            // ------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/notificationmgmt/v11/charging-notifications
            // ------------------------------------------------------------------------------------------------------------------------------------
            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.POST,
                              URLPathPrefix + "/api/oicp/notificationmgmt/v11/charging-notifications",
                              HTTPContentType.Application.JSON_UTF8,
                              HTTPRequestLogger:   logChargingNotificationHTTPRequest,
                              HTTPResponseLogger:  logChargingNotificationHTTPResponse,
                              HTTPDelegate:        async Request => {

                                  var startTime  = Timestamp.Now;
                                  var processId  = Process_Id.NewRandom();

                                  OICPResult<Acknowledgement>? chargingNotificationResponse = null;

                                  try
                                  {

                                      if (Request.TryParseJSONObjectRequestBody(out var JSONRequest, out _) &&
                                          JSONRequest.ParseMandatory("Type",
                                                                     "charging notification type",
                                                                     ChargingNotificationTypesExtensions.TryParse,
                                                                     out ChargingNotificationTypes chargingNotificationType,
                                                                     out _))
                                      {

                                          switch (chargingNotificationType)
                                          {

                                              #region Start

                                              case ChargingNotificationTypes.Start:

                                                  if (ChargingStartNotificationRequest.TryParse(JSONRequest,
                                                                                                out ChargingStartNotificationRequest?          chargingStartNotificationRequest,
                                                                                                out String?                                    errorResponse,
                                                                                                ProcessId:                                     processId,

                                                                                                Timestamp:                                     Request.Timestamp,
                                                                                                CancellationToken:                             Request.CancellationToken,
                                                                                                EventTrackingId:                               Request.EventTrackingId,
                                                                                                RequestTimeout:                                Request.Timeout ?? DefaultRequestTimeout,

                                                                                                CustomChargingStartNotificationRequestParser:  CustomChargingStartNotificationRequestParser))
                                                  {

                                                      Counters.ChargingNotifications.    IncRequests_OK();
                                                      Counters.ChargingStartNotification.IncRequests_OK();

                                                      #region Send OnChargingStartNotificationRequest event

                                                      try
                                                      {

                                                          if (OnChargingStartNotificationRequest is not null)
                                                              await Task.WhenAll(OnChargingStartNotificationRequest.GetInvocationList().
                                                                                 Cast<OnChargingStartNotificationAPIRequestDelegate>().
                                                                                 Select(e => e(Timestamp.Now,
                                                                                               this,
                                                                                               chargingStartNotificationRequest!))).
                                                                                 ConfigureAwait(false);

                                                      }
                                                      catch (Exception e)
                                                      {
                                                          DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnChargingStartNotificationRequest));
                                                      }

                                                      #endregion

                                                      #region Call async subscribers

                                                      OICPResult<Acknowledgement<ChargingStartNotificationRequest>>? chargingStartNotificationResponse = null;

                                                      var OnChargingStartNotificationLocal = OnChargingStartNotification;
                                                      if (OnChargingStartNotificationLocal is not null)
                                                      {
                                                          try
                                                          {

                                                              chargingStartNotificationResponse = (await Task.WhenAll(OnChargingStartNotificationLocal.GetInvocationList().
                                                                                                                                                       Cast<OnChargingStartNotificationAPIDelegate>().
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

                                                              chargingStartNotificationResponse = OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Failed(
                                                                                                      this,
                                                                                                      new Acknowledgement<ChargingStartNotificationRequest>(
                                                                                                          Timestamp.Now,
                                                                                                          Request.EventTrackingId,
                                                                                                          processId,
                                                                                                          Timestamp.Now - Request.Timestamp,
                                                                                                          StatusCode: new StatusCode(
                                                                                                                          StatusCodes.DataError,
                                                                                                                          e.Message,
                                                                                                                          e.StackTrace
                                                                                                                      ),
                                                                                                          chargingStartNotificationRequest
                                                                                                      )
                                                                                                  );

                                                          }
                                                      }

                                                      if (chargingStartNotificationResponse is null)
                                                      {

                                                          Counters.ChargingNotifications.    IncResponses_Error();
                                                          Counters.ChargingStartNotification.IncResponses_Error();

                                                          chargingStartNotificationResponse = OICPResult<Acknowledgement<ChargingStartNotificationRequest>>.Failed(
                                                                                                  this,
                                                                                                  new Acknowledgement<ChargingStartNotificationRequest>(
                                                                                                      Timestamp.Now,
                                                                                                      Request.EventTrackingId,
                                                                                                      processId,
                                                                                                      Timestamp.Now - Request.Timestamp,
                                                                                                      StatusCode: new StatusCode(
                                                                                                                      StatusCodes.SystemError,
                                                                                                                      "Could not process the received ChargingStartNotification request!"
                                                                                                                  ),
                                                                                                      chargingStartNotificationRequest
                                                                                                  )
                                                                                              );

                                                      }

                                                      #endregion

                                                      #region Send OnChargingStartNotificationResponse event

                                                      try
                                                      {

                                                          if (OnChargingStartNotificationResponse is not null)
                                                              await Task.WhenAll(OnChargingStartNotificationResponse.GetInvocationList().
                                                                                 Cast<OnChargingStartNotificationAPIResponseDelegate>().
                                                                                 Select(e => e(Timestamp.Now,
                                                                                               this,
                                                                                               chargingStartNotificationRequest!,
                                                                                               chargingStartNotificationResponse,
                                                                                               Timestamp.Now - startTime))).
                                                                                 ConfigureAwait(false);

                                                      }
                                                      catch (Exception e)
                                                      {
                                                          DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnChargingStartNotificationResponse));
                                                      }

                                                      #endregion


                                                      chargingNotificationResponse = new OICPResult<Acknowledgement>(
                                                                                         Request,
                                                                                         chargingStartNotificationResponse.Response,
                                                                                         chargingStartNotificationResponse.IsSuccessful,
                                                                                         chargingStartNotificationResponse.ValidationErrors,
                                                                                         chargingStartNotificationResponse.ProcessId);

                                                  }
                                                  else
                                                  {

                                                      Counters.ChargingNotifications.    IncRequests_Error();
                                                      Counters.ChargingStartNotification.IncRequests_Error();

                                                      chargingNotificationResponse = OICPResult<Acknowledgement>.Failed(
                                                                                         this,
                                                                                         new Acknowledgement(
                                                                                             Request.Timestamp,
                                                                                             Timestamp.Now,
                                                                                             Request.EventTrackingId,
                                                                                             Timestamp.Now - Request.Timestamp,
                                                                                             new StatusCode(
                                                                                                 StatusCodes.DataError,
                                                                                                 "We could not parse the given ChargingStartNotification request!",
                                                                                                 errorResponse
                                                                                             )
                                                                                         )
                                                                                     );

                                                  }

                                                  break;

                                              #endregion

                                              #region Progress

                                              case ChargingNotificationTypes.Progress:

                                                  if (ChargingProgressNotificationRequest.TryParse(JSONRequest,
                                                                                                   out ChargingProgressNotificationRequest?          chargingProgressNotificationRequest,
                                                                                                   out                                               errorResponse,
                                                                                                   ProcessId:                                        processId,

                                                                                                   Timestamp:                                        Request.Timestamp,
                                                                                                   CancellationToken:                                Request.CancellationToken,
                                                                                                   EventTrackingId:                                  Request.EventTrackingId,
                                                                                                   RequestTimeout:                                   Request.Timeout ?? DefaultRequestTimeout,

                                                                                                   CustomChargingProgressNotificationRequestParser:  CustomChargingProgressNotificationRequestParser))
                                                  {

                                                      Counters.ChargingNotifications.       IncRequests_OK();
                                                      Counters.ChargingProgressNotification.IncRequests_OK();

                                                      #region Send OnChargingProgressNotificationRequest event

                                                      try
                                                      {

                                                          if (OnChargingProgressNotificationRequest is not null)
                                                              await Task.WhenAll(OnChargingProgressNotificationRequest.GetInvocationList().
                                                                                 Cast<OnChargingProgressNotificationAPIRequestDelegate>().
                                                                                 Select(e => e(Timestamp.Now,
                                                                                               this,
                                                                                               chargingProgressNotificationRequest!))).
                                                                                 ConfigureAwait(false);

                                                      }
                                                      catch (Exception e)
                                                      {
                                                          DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnChargingProgressNotificationRequest));
                                                      }

                                                      #endregion

                                                      #region Call async subscribers

                                                      OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>? chargingProgressNotificationResponse = null;

                                                      var OnChargingProgressNotificationLocal = OnChargingProgressNotification;
                                                      if (OnChargingProgressNotificationLocal is not null)
                                                      {
                                                          try
                                                          {

                                                              chargingProgressNotificationResponse = (await Task.WhenAll(OnChargingProgressNotificationLocal.GetInvocationList().
                                                                                                                                                             Cast<OnChargingProgressNotificationAPIDelegate>().
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

                                                              chargingProgressNotificationResponse = OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Failed(
                                                                                                         this,
                                                                                                         new Acknowledgement<ChargingProgressNotificationRequest>(
                                                                                                             Timestamp.Now,
                                                                                                             Request.EventTrackingId,
                                                                                                             processId,
                                                                                                             Timestamp.Now - Request.Timestamp,
                                                                                                             StatusCode: new StatusCode(
                                                                                                                             StatusCodes.DataError,
                                                                                                                             e.Message,
                                                                                                                             e.StackTrace
                                                                                                                         ),
                                                                                                             chargingProgressNotificationRequest
                                                                                                         )
                                                                                                     );

                                                          }
                                                      }

                                                      if (chargingProgressNotificationResponse is null)
                                                      {

                                                          Counters.ChargingNotifications.       IncResponses_Error();
                                                          Counters.ChargingProgressNotification.IncResponses_Error();

                                                          chargingProgressNotificationResponse = OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>.Failed(
                                                                                                     this,
                                                                                                     new Acknowledgement<ChargingProgressNotificationRequest>(
                                                                                                         Timestamp.Now,
                                                                                                         Request.EventTrackingId,
                                                                                                         processId,
                                                                                                         Timestamp.Now - Request.Timestamp,
                                                                                                         StatusCode: new StatusCode(
                                                                                                                         StatusCodes.SystemError,
                                                                                                                         "Could not process the received ChargingProgressNotification request!"
                                                                                                                     ),
                                                                                                         chargingProgressNotificationRequest
                                                                                                     )
                                                                                                 );

                                                      }

                                                      #endregion

                                                      #region Send OnChargingProgressNotificationResponse event

                                                      try
                                                      {

                                                          if (OnChargingProgressNotificationResponse is not null)
                                                              await Task.WhenAll(OnChargingProgressNotificationResponse.GetInvocationList().
                                                                                 Cast<OnChargingProgressNotificationAPIResponseDelegate>().
                                                                                 Select(e => e(Timestamp.Now,
                                                                                               this,
                                                                                               chargingProgressNotificationRequest!,
                                                                                               chargingProgressNotificationResponse,
                                                                                               Timestamp.Now - startTime))).
                                                                                 ConfigureAwait(false);

                                                      }
                                                      catch (Exception e)
                                                      {
                                                          DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnChargingProgressNotificationResponse));
                                                      }

                                                      #endregion


                                                      chargingNotificationResponse = new OICPResult<Acknowledgement>(
                                                                                         Request,
                                                                                         chargingProgressNotificationResponse.Response,
                                                                                         chargingProgressNotificationResponse.IsSuccessful,
                                                                                         chargingProgressNotificationResponse.ValidationErrors,
                                                                                         chargingProgressNotificationResponse.ProcessId);

                                                  }
                                                  else
                                                  {

                                                      Counters.ChargingNotifications.       IncRequests_Error();
                                                      Counters.ChargingProgressNotification.IncRequests_Error();

                                                      chargingNotificationResponse = OICPResult<Acknowledgement>.Failed(
                                                                                         this,
                                                                                         new Acknowledgement(
                                                                                             Request.Timestamp,
                                                                                             Timestamp.Now,
                                                                                             Request.EventTrackingId,
                                                                                             Timestamp.Now - Request.Timestamp,
                                                                                             new StatusCode(
                                                                                                 StatusCodes.DataError,
                                                                                                 "We could not parse the given ChargingProgressNotification request!",
                                                                                                 errorResponse
                                                                                             )
                                                                                         )
                                                                                     );

                                                  }

                                                  break;

                                              #endregion

                                              #region End

                                              case ChargingNotificationTypes.End:

                                                  if (ChargingEndNotificationRequest.TryParse(JSONRequest,
                                                                                              out ChargingEndNotificationRequest?          chargingEndNotificationRequest,
                                                                                              out                                          errorResponse,
                                                                                              ProcessId:                                   processId,

                                                                                              Timestamp:                                   Request.Timestamp,
                                                                                              CancellationToken:                           Request.CancellationToken,
                                                                                              EventTrackingId:                             Request.EventTrackingId,
                                                                                              RequestTimeout:                              Request.Timeout ?? DefaultRequestTimeout,

                                                                                              CustomChargingEndNotificationRequestParser:  CustomChargingEndNotificationRequestParser))
                                                  {

                                                      Counters.ChargingNotifications.  IncRequests_OK();
                                                      Counters.ChargingEndNotification.IncRequests_OK();

                                                      #region Send OnChargingEndNotificationRequest event

                                                      try
                                                      {

                                                          if (OnChargingEndNotificationRequest is not null)
                                                              await Task.WhenAll(OnChargingEndNotificationRequest.GetInvocationList().
                                                                                 Cast<OnChargingEndNotificationAPIRequestDelegate>().
                                                                                 Select(e => e(Timestamp.Now,
                                                                                               this,
                                                                                               chargingEndNotificationRequest!))).
                                                                                 ConfigureAwait(false);

                                                      }
                                                      catch (Exception e)
                                                      {
                                                          DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnChargingEndNotificationRequest));
                                                      }

                                                      #endregion

                                                      #region Call async subscribers

                                                      OICPResult<Acknowledgement<ChargingEndNotificationRequest>>? chargingEndNotificationResponse = null;

                                                      var OnChargingEndNotificationLocal = OnChargingEndNotification;
                                                      if (OnChargingEndNotificationLocal is not null)
                                                      {
                                                          try
                                                          {

                                                              chargingEndNotificationResponse = (await Task.WhenAll(OnChargingEndNotificationLocal.GetInvocationList().
                                                                                                                                                   Cast<OnChargingEndNotificationAPIDelegate>().
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

                                                              chargingEndNotificationResponse = OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Failed(
                                                                                                    this,
                                                                                                    new Acknowledgement<ChargingEndNotificationRequest>(
                                                                                                        Timestamp.Now,
                                                                                                        Request.EventTrackingId,
                                                                                                        processId,
                                                                                                        Timestamp.Now - Request.Timestamp,
                                                                                                        StatusCode: new StatusCode(
                                                                                                                        StatusCodes.DataError,
                                                                                                                        e.Message,
                                                                                                                        e.StackTrace
                                                                                                                    ),
                                                                                                        chargingEndNotificationRequest
                                                                                                    )
                                                                                                );

                                                          }
                                                      }

                                                      if (chargingEndNotificationResponse is null)
                                                      {

                                                          Counters.ChargingNotifications.  IncResponses_Error();
                                                          Counters.ChargingEndNotification.IncResponses_Error();

                                                          chargingEndNotificationResponse = OICPResult<Acknowledgement<ChargingEndNotificationRequest>>.Failed(
                                                                                                this,
                                                                                                new Acknowledgement<ChargingEndNotificationRequest>(
                                                                                                    Timestamp.Now,
                                                                                                    Request.EventTrackingId,
                                                                                                    processId,
                                                                                                    Timestamp.Now - Request.Timestamp,
                                                                                                    StatusCode: new StatusCode(
                                                                                                                    StatusCodes.SystemError,
                                                                                                                    "Could not process the received ChargingEndNotification request!"
                                                                                                                ),
                                                                                                    chargingEndNotificationRequest
                                                                                                )
                                                                                            );

                                                      }

                                                      #endregion

                                                      #region Send OnChargingEndNotificationResponse event

                                                      try
                                                      {

                                                          if (OnChargingEndNotificationResponse is not null)
                                                              await Task.WhenAll(OnChargingEndNotificationResponse.GetInvocationList().
                                                                                 Cast<OnChargingEndNotificationAPIResponseDelegate>().
                                                                                 Select(e => e(Timestamp.Now,
                                                                                               this,
                                                                                               chargingEndNotificationRequest!,
                                                                                               chargingEndNotificationResponse,
                                                                                               Timestamp.Now - startTime))).
                                                                                 ConfigureAwait(false);

                                                      }
                                                      catch (Exception e)
                                                      {
                                                          DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnChargingEndNotificationResponse));
                                                      }

                                                      #endregion


                                                      chargingNotificationResponse = new OICPResult<Acknowledgement>(
                                                                                         Request,
                                                                                         chargingEndNotificationResponse.Response,
                                                                                         chargingEndNotificationResponse.IsSuccessful,
                                                                                         chargingEndNotificationResponse.ValidationErrors,
                                                                                         chargingEndNotificationResponse.ProcessId);

                                                  }
                                                  else
                                                  {

                                                      Counters.ChargingNotifications.  IncRequests_Error();
                                                      Counters.ChargingEndNotification.IncRequests_Error();

                                                      chargingNotificationResponse = OICPResult<Acknowledgement>.Failed(
                                                                                         this,
                                                                                         new Acknowledgement(
                                                                                             Request.Timestamp,
                                                                                             Timestamp.Now,
                                                                                             Request.EventTrackingId,
                                                                                             Timestamp.Now - Request.Timestamp,
                                                                                             new StatusCode(
                                                                                                 StatusCodes.DataError,
                                                                                                 "We could not parse the given ChargingEndNotification request!",
                                                                                                 errorResponse
                                                                                             )
                                                                                         )
                                                                                     );

                                                  }

                                                  break;

                                              #endregion

                                              #region Error

                                              case ChargingNotificationTypes.Error:

                                                  if (ChargingErrorNotificationRequest.TryParse(JSONRequest,
                                                                                                out ChargingErrorNotificationRequest?          chargingErrorNotificationRequest,
                                                                                                out                                            errorResponse,
                                                                                                ProcessId:                                     processId,

                                                                                                Timestamp:                                     Request.Timestamp,
                                                                                                CancellationToken:                             Request.CancellationToken,
                                                                                                EventTrackingId:                               Request.EventTrackingId,
                                                                                                RequestTimeout:                                Request.Timeout ?? DefaultRequestTimeout,

                                                                                                CustomChargingErrorNotificationRequestParser:  CustomChargingErrorNotificationRequestParser))
                                                  {

                                                      Counters.ChargingNotifications.    IncRequests_OK();
                                                      Counters.ChargingErrorNotification.IncRequests_OK();

                                                      #region Send OnChargingErrorNotificationRequest event

                                                      try
                                                      {

                                                          if (OnChargingErrorNotificationRequest is not null)
                                                              await Task.WhenAll(OnChargingErrorNotificationRequest.GetInvocationList().
                                                                                 Cast<OnChargingErrorNotificationAPIRequestDelegate>().
                                                                                 Select(e => e(Timestamp.Now,
                                                                                               this,
                                                                                               chargingErrorNotificationRequest!))).
                                                                                 ConfigureAwait(false);

                                                      }
                                                      catch (Exception e)
                                                      {
                                                          DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnChargingErrorNotificationRequest));
                                                      }

                                                      #endregion

                                                      #region Call async subscribers

                                                      OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>? chargingErrorNotificationResponse = null;

                                                      var OnChargingErrorNotificationLocal = OnChargingErrorNotification;
                                                      if (OnChargingErrorNotificationLocal is not null)
                                                      {
                                                          try
                                                          {

                                                              chargingErrorNotificationResponse = (await Task.WhenAll(OnChargingErrorNotificationLocal.GetInvocationList().
                                                                                                                                          Cast<OnChargingErrorNotificationAPIDelegate>().
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

                                                              chargingErrorNotificationResponse = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Failed(
                                                                                         this,
                                                                                         new Acknowledgement<ChargingErrorNotificationRequest>(
                                                                                             Timestamp.Now,
                                                                                             Request.EventTrackingId,
                                                                                             processId,
                                                                                             Timestamp.Now - Request.Timestamp,
                                                                                             StatusCode: new StatusCode(
                                                                                                             StatusCodes.DataError,
                                                                                                             e.Message,
                                                                                                             e.StackTrace
                                                                                                         ),
                                                                                             chargingErrorNotificationRequest
                                                                                         )
                                                                                     );

                                                          }
                                                      }

                                                      if (chargingErrorNotificationResponse is null)
                                                      {

                                                          Counters.ChargingNotifications.    IncResponses_Error();
                                                          Counters.ChargingErrorNotification.IncResponses_Error();

                                                          chargingErrorNotificationResponse = OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>.Failed(
                                                                                                  this,
                                                                                                  new Acknowledgement<ChargingErrorNotificationRequest>(
                                                                                                      Timestamp.Now,
                                                                                                      Request.EventTrackingId,
                                                                                                      processId,
                                                                                                      Timestamp.Now - Request.Timestamp,
                                                                                                      StatusCode: new StatusCode(
                                                                                                                      StatusCodes.SystemError,
                                                                                                                      "Could not process the received ChargingErrorNotification request!"
                                                                                                                  ),
                                                                                                      chargingErrorNotificationRequest
                                                                                                  )
                                                                                              );

                                                      }

                                                      #endregion

                                                      #region Send OnChargingErrorNotificationResponse event

                                                      try
                                                      {

                                                          if (OnChargingErrorNotificationResponse is not null)
                                                              await Task.WhenAll(OnChargingErrorNotificationResponse.GetInvocationList().
                                                                                 Cast<OnChargingErrorNotificationAPIResponseDelegate>().
                                                                                 Select(e => e(Timestamp.Now,
                                                                                               this,
                                                                                               chargingErrorNotificationRequest!,
                                                                                               chargingErrorNotificationResponse,
                                                                                               Timestamp.Now - startTime))).
                                                                                 ConfigureAwait(false);

                                                      }
                                                      catch (Exception e)
                                                      {
                                                          DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnChargingErrorNotificationResponse));
                                                      }

                                                      #endregion


                                                      chargingNotificationResponse = new OICPResult<Acknowledgement>(
                                                                                         Request,
                                                                                         chargingErrorNotificationResponse.Response,
                                                                                         chargingErrorNotificationResponse.IsSuccessful,
                                                                                         chargingErrorNotificationResponse.ValidationErrors,
                                                                                         chargingErrorNotificationResponse.ProcessId);

                                                  }
                                                  else
                                                  {

                                                      Counters.ChargingNotifications.IncRequests_Error();
                                                      Counters.ChargingEndNotification.IncRequests_Error();

                                                      chargingNotificationResponse = OICPResult<Acknowledgement>.Failed(
                                                                                         this,
                                                                                         new Acknowledgement(
                                                                                             Request.Timestamp,
                                                                                             Timestamp.Now,
                                                                                             Request.EventTrackingId,
                                                                                             Timestamp.Now - Request.Timestamp,
                                                                                             new StatusCode(
                                                                                                 StatusCodes.DataError,
                                                                                                 "We could not parse the given ChargingErrorNotification request!",
                                                                                                 errorResponse
                                                                                             )
                                                                                         )
                                                                                     );

                                                  }

                                                  break;

                                              #endregion

                                              #region ...or default

                                              default:

                                                  Counters.ChargingNotifications.IncRequests_Error();

                                                  chargingNotificationResponse = OICPResult<Acknowledgement>.Failed(
                                                                                     this,
                                                                                     new Acknowledgement(
                                                                                         Request.Timestamp,
                                                                                         Timestamp.Now,
                                                                                         Request.EventTrackingId,
                                                                                         Timestamp.Now - Request.Timestamp,
                                                                                         StatusCode: new StatusCode(
                                                                                                         StatusCodes.DataError,
                                                                                                         "Unknown or invalid charging notification type '" + chargingNotificationType.ToString() + "'!"
                                                                                                         //errorResponse
                                                                                                     )
                                                                                     )
                                                                                 );

                                                  break;

                                              #endregion

                                          }

                                      }
                                      else
                                      {

                                          Counters.ChargingNotifications.IncRequests_Error();

                                          chargingNotificationResponse = OICPResult<Acknowledgement>.Failed(
                                                                             this,
                                                                             new Acknowledgement(
                                                                                 Request.Timestamp,
                                                                                 Timestamp.Now,
                                                                                 Request.EventTrackingId,
                                                                                 Timestamp.Now - Request.Timestamp,
                                                                                 StatusCode: new StatusCode(
                                                                                                 StatusCodes.DataError,
                                                                                                 "We could not parse the given ChargingNotification request!"
                                                                                                 //errorResponse
                                                                                             )
                                                                             )
                                                                         );

                                      }

                                  }
                                  catch (Exception e)
                                  {

                                      Counters.ChargingNotifications.IncResponses_Error();

                                      chargingNotificationResponse = OICPResult<Acknowledgement>.Failed(
                                                                         this,
                                                                         new Acknowledgement(
                                                                             Request.Timestamp,
                                                                             Timestamp.Now,
                                                                             Request.EventTrackingId,
                                                                             Timestamp.Now - Request.Timestamp,
                                                                             StatusCode: new StatusCode(
                                                                                             StatusCodes.SystemError,
                                                                                             e.Message,
                                                                                             e.StackTrace
                                                                                         )
                                                                         )
                                                                     );

                                  }


                                  return new HTTPResponse.Builder(Request) {
                                             HTTPStatusCode             = HTTPStatusCode.OK,
                                             Server                     = HTTPServer.DefaultServerName,
                                             Date                       = Timestamp.Now,
                                             AccessControlAllowOrigin   = "*",
                                             AccessControlAllowMethods  = [ "POST" ],
                                             AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                             ContentType                = HTTPContentType.Application.JSON_UTF8,
                                             Content                    = chargingNotificationResponse.Response?.ToJSON(CustomAcknowledgementSerializer,
                                                                                                                        CustomStatusCodeSerializer).
                                                                                                                 ToString(JSONFormatting).
                                                                                                                 ToUTF8Bytes()
                                                                                                       ?? [],
                                             ProcessID                  = processId.ToString(),
                                             Connection                 = ConnectionType.Close
                                         }.AsImmutable;

                               }, AllowReplacement: URLReplacement.Allow);

            #endregion


            #region POST  ~/api/oicp/cdrmgmt/v22/operators/{operatorId}/charge-detail-record

            // ------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/cdrmgmt/v22/operators/DE-GEF/charge-detail-record
            // ------------------------------------------------------------------------------------------------------------------------------------------
            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.POST,
                              URLPathPrefix + "/api/oicp/cdrmgmt/v22/operators/{operatorId}/charge-detail-record",
                              HTTPContentType.Application.JSON_UTF8,
                              HTTPRequestLogger:   logChargeDetailRecordHTTPRequest,
                              HTTPResponseLogger:  logChargeDetailRecordHTTPResponse,
                              HTTPDelegate:        async Request => {

                                  var startTime  = Timestamp.Now;
                                  var processId  = Process_Id.NewRandom();

                                  OICPResult<Acknowledgement<ChargeDetailRecordRequest>>? chargeDetailRecordResponse = null;

                                  try
                                  {

                                      #region Try to parse ProviderId URL parameter

                                      if (Request.ParsedURLParameters.Length != 1 || !Operator_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Operator_Id operatorId))
                                      {

                                          Counters.ChargeDetailRecord.IncRequests_Error();

                                          chargeDetailRecordResponse = OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Failed(
                                                                           this,
                                                                           new Acknowledgement<ChargeDetailRecordRequest>(
                                                                               Timestamp.Now,
                                                                               Request.EventTrackingId,
                                                                               processId,
                                                                               Timestamp.Now - Request.Timestamp,
                                                                               StatusCode: new StatusCode(
                                                                                               StatusCodes.SystemError,
                                                                                               "The expected 'operatorId' URL parameter could not be parsed!"
                                                                                           )
                                                                           )
                                                                       );

                                      }

                                      #endregion

                                      else if (ChargeDetailRecordRequest.TryParse(JObject.Parse(Request.HTTPBody?.ToUTF8String() ?? ""),
                                                                                  operatorId,
                                                                                  out var                                 chargeDetailRecordRequest,
                                                                                  out var                                 errorResponse,
                                                                                  ProcessId:                              processId,

                                                                                  Timestamp:                              Request.Timestamp,
                                                                                  CancellationToken:                      Request.CancellationToken,
                                                                                  EventTrackingId:                        Request.EventTrackingId,
                                                                                  RequestTimeout:                         Request.Timeout ?? DefaultRequestTimeout,

                                                                                  CustomChargeDetailRecordRequestParser:  CustomChargeDetailRecordRequestParser))
                                      {

                                          Counters.ChargeDetailRecord.IncRequests_OK();

                                          #region Send OnChargeDetailRecordRequest event

                                          try
                                          {

                                              if (OnChargeDetailRecordRequest is not null)
                                                  await Task.WhenAll(OnChargeDetailRecordRequest.GetInvocationList().
                                                                     Cast<OnChargeDetailRecordAPIRequestDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   chargeDetailRecordRequest!))).
                                                                     ConfigureAwait(false);

                                          }
                                          catch (Exception e)
                                          {
                                              DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnChargeDetailRecordRequest));
                                          }

                                          #endregion

                                          #region Call async subscribers

                                          var OnChargeDetailRecordLocal = OnChargeDetailRecord;
                                          if (OnChargeDetailRecordLocal is not null)
                                          {
                                              try
                                              {

                                                  chargeDetailRecordResponse = (await Task.WhenAll(OnChargeDetailRecordLocal.GetInvocationList().
                                                                                                                             Cast<OnChargeDetailRecordAPIDelegate>().
                                                                                                                             Select(e => e(Timestamp.Now,
                                                                                                                                           this,
                                                                                                                                           chargeDetailRecordRequest!))))?.FirstOrDefault();

                                                  Counters.ChargeDetailRecord.IncResponses_OK();

                                              }
                                              catch (Exception e)
                                              {

                                                  Counters.ChargeDetailRecord.IncResponses_Error();

                                                  chargeDetailRecordResponse = OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Failed(
                                                                                   this,
                                                                                   new Acknowledgement<ChargeDetailRecordRequest>(
                                                                                       Timestamp.Now,
                                                                                       Request.EventTrackingId,
                                                                                       processId,
                                                                                       Timestamp.Now - Request.Timestamp,
                                                                                       StatusCode: new StatusCode(
                                                                                                       StatusCodes.DataError,
                                                                                                       e.Message,
                                                                                                       e.StackTrace
                                                                                                   ),
                                                                                       chargeDetailRecordRequest
                                                                                   )
                                                                               );

                                              }
                                          }

                                          if (chargeDetailRecordResponse is null)
                                          {

                                              Counters.ChargeDetailRecord.IncResponses_Error();

                                              chargeDetailRecordResponse = OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Failed(
                                                                               this,
                                                                               new Acknowledgement<ChargeDetailRecordRequest>(
                                                                                   Timestamp.Now,
                                                                                   Request.EventTrackingId,
                                                                                   processId,
                                                                                   Timestamp.Now - Request.Timestamp,
                                                                                   StatusCode: new StatusCode(
                                                                                                   StatusCodes.SystemError,
                                                                                                   "Could not process the received ChargeDetailRecord request!"
                                                                                               ),
                                                                                   chargeDetailRecordRequest
                                                                               )
                                                                           );

                                          }

                                          #endregion

                                          #region Send OnChargeDetailRecordResponse event

                                          try
                                          {

                                              if (OnChargeDetailRecordResponse is not null)
                                                  await Task.WhenAll(OnChargeDetailRecordResponse.GetInvocationList().
                                                                     Cast<OnChargeDetailRecordAPIResponseDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   chargeDetailRecordRequest!,
                                                                                   chargeDetailRecordResponse,
                                                                                   Timestamp.Now - startTime))).
                                                                     ConfigureAwait(false);

                                          }
                                          catch (Exception e)
                                          {
                                              DebugX.LogException(e, nameof(CPOClientAPI) + "." + nameof(OnChargeDetailRecordResponse));
                                          }

                                          #endregion

                                      }
                                      else
                                      {

                                          Counters.ChargeDetailRecord.IncRequests_Error();

                                          chargeDetailRecordResponse = OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Failed(
                                                                           this,
                                                                           new Acknowledgement<ChargeDetailRecordRequest>(
                                                                               Timestamp.Now,
                                                                               Request.EventTrackingId,
                                                                               processId,
                                                                               Timestamp.Now - Request.Timestamp,
                                                                               StatusCode: new StatusCode(
                                                                                               StatusCodes.DataError,
                                                                                               "We could not parse the given ChargeDetailRecord request!",
                                                                                               errorResponse
                                                                                           ),
                                                                               chargeDetailRecordRequest
                                                                           )
                                                                       );

                                      }

                                  }
                                  catch (Exception e)
                                  {

                                      Counters.ChargeDetailRecord.IncResponses_Error();

                                      chargeDetailRecordResponse = OICPResult<Acknowledgement<ChargeDetailRecordRequest>>.Failed(
                                                                       this,
                                                                       new Acknowledgement<ChargeDetailRecordRequest>(
                                                                           Timestamp.Now,
                                                                           Request.EventTrackingId,
                                                                           processId,
                                                                           Timestamp.Now - Request.Timestamp,
                                                                           StatusCode: new StatusCode(
                                                                                           StatusCodes.SystemError,
                                                                                           e.Message,
                                                                                           e.StackTrace
                                                                                       )
                                                                       )
                                                                   );

                                  }


                                  return new HTTPResponse.Builder(Request) {
                                             HTTPStatusCode             = HTTPStatusCode.OK,
                                             Server                     = HTTPServer.DefaultServerName,
                                             Date                       = Timestamp.Now,
                                             AccessControlAllowOrigin   = "*",
                                             AccessControlAllowMethods  = [ "POST" ],
                                             AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                             ContentType                = HTTPContentType.Application.JSON_UTF8,
                                             Content                    = chargeDetailRecordResponse.Response?.ToJSON(CustomAcknowledgementSerializer,
                                                                                                                      CustomStatusCodeSerializer).
                                                                                                               ToString(JSONFormatting).
                                                                                                               ToUTF8Bytes()
                                                                                                     ?? [],
                                             ProcessID                  = processId.ToString(),
                                             Connection                 = ConnectionType.Close
                                         }.AsImmutable;

                               }, AllowReplacement: URLReplacement.Allow);

            #endregion

        }

        #endregion


        #region Dispose()

        /// <summary>
        /// Dispose this object.
        /// </summary>
        public override void Dispose()
        { }

        #endregion

    }

}
