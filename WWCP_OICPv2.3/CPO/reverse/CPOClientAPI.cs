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

using System.Net.Security;
using System.Security.Authentication;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Sockets;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    /// <summary>
    /// The CPO HTTP Client API.
    /// </summary>
    public partial class CPOClientAPI : HTTPAPI
    {

        #region (class) APICounters

        public class APICounters
        {

            public APICounterValues  PushEVSEData                    { get; }
            public APICounterValues  PushEVSEStatus                  { get; }

            public APICounterValues  PushPricingProductData          { get; }
            public APICounterValues  PushEVSEPricing                 { get; }

            public APICounterValues  PullAuthenticationData          { get; }

            public APICounterValues  AuthorizeStart                  { get; }
            public APICounterValues  AuthorizeStop                   { get; }


            public APICounterValues  ChargingNotifications           { get; }
            public APICounterValues  ChargingStartNotification       { get; }
            public APICounterValues  ChargingProgressNotification    { get; }
            public APICounterValues  ChargingEndNotification         { get; }
            public APICounterValues  ChargingErrorNotification       { get; }

            public APICounterValues  ChargeDetailRecord              { get; }


            public APICounters(APICounterValues? PushEVSEData                   = null,
                               APICounterValues? PushEVSEStatus                 = null,

                               APICounterValues? PushPricingProductData         = null,
                               APICounterValues? PushEVSEPricing                = null,

                               APICounterValues? PullAuthenticationData         = null,

                               APICounterValues? AuthorizeStart                 = null,
                               APICounterValues? AuthorizeStop                  = null,

                               APICounterValues? ChargingNotifications          = null,
                               APICounterValues? ChargingStartNotification      = null,
                               APICounterValues? ChargingProgressNotification   = null,
                               APICounterValues? ChargingEndNotification        = null,
                               APICounterValues? ChargingErrorNotification      = null,

                               APICounterValues? ChargeDetailRecord             = null)
            {

                this.PushEVSEData                  = PushEVSEData                 ?? new APICounterValues();
                this.PushEVSEStatus                = PushEVSEStatus               ?? new APICounterValues();

                this.PushPricingProductData        = PushPricingProductData       ?? new APICounterValues();
                this.PushEVSEPricing               = PushEVSEPricing              ?? new APICounterValues();

                this.PullAuthenticationData        = PullAuthenticationData       ?? new APICounterValues();

                this.AuthorizeStart                = AuthorizeStart               ?? new APICounterValues();
                this.AuthorizeStop                 = AuthorizeStop                ?? new APICounterValues();

                this.ChargingNotifications         = ChargingNotifications        ?? new APICounterValues();
                this.ChargingStartNotification     = ChargingStartNotification    ?? new APICounterValues();
                this.ChargingProgressNotification  = ChargingProgressNotification ?? new APICounterValues();
                this.ChargingEndNotification       = ChargingEndNotification      ?? new APICounterValues();
                this.ChargingErrorNotification     = ChargingErrorNotification    ?? new APICounterValues();

                this.ChargeDetailRecord            = ChargeDetailRecord           ?? new APICounterValues();

            }

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
        public new const String  DefaultHTTPServerName   = "GraphDefined OICP " + Version.Number + " CPO HTTP API";

        /// <summary>
        /// The default HTTP service name.
        /// </summary>
        public new const String  DefaultHTTPServiceName  = "GraphDefined OICP " + Version.Number + " CPO HTTP API";

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
        /// CPO Client API counters.
        /// </summary>
        public APICounters                 Counters          { get; }

        public Newtonsoft.Json.Formatting  JSONFormatting    { get; set; }

        #endregion

        #region Custom JSON parsers
        public CustomJObjectParserDelegate<PushEVSEDataRequest>?                     CustomPushEVSEDataRequestParser                    { get; set; }
        public CustomJObjectParserDelegate<PushEVSEStatusRequest>?                   CustomPushEVSEStatusRequestParser                  { get; set; }


        public CustomJObjectParserDelegate<PushPricingProductDataRequest>?           CustomPushPricingProductDataRequestParser          { get; set; }
        public CustomJObjectParserDelegate<PushEVSEPricingRequest>?                  CustomPushEVSEPricingRequestParser                 { get; set; }

        public CustomJObjectParserDelegate<PullAuthenticationDataRequest>?           CustomPullAuthenticationDataRequestParser          { get; set; }

        public CustomJObjectParserDelegate<AuthorizeStartRequest>?                   CustomAuthorizeStartRequestParser                  { get; set; }
        public CustomJObjectParserDelegate<AuthorizeStopRequest>?                    CustomAuthorizeStopRequestParser                   { get; set; }


        public CustomJObjectParserDelegate<ChargingStartNotificationRequest>?        CustomChargingStartNotificationRequestParser       { get; set; }
        public CustomJObjectParserDelegate<ChargingProgressNotificationRequest>?     CustomChargingProgressNotificationRequestParser    { get; set; }
        public CustomJObjectParserDelegate<ChargingEndNotificationRequest>?          CustomChargingEndNotificationRequestParser         { get; set; }
        public CustomJObjectParserDelegate<ChargingErrorNotificationRequest>?        CustomChargingErrorNotificationRequestParser       { get; set; }

        public CustomJObjectParserDelegate<ChargeDetailRecordRequest>?               CustomChargeDetailRecordRequestParser              { get; set; }

        #endregion

        #region Custom JSON serializers
        public CustomJObjectSerializerDelegate<Acknowledgement>?                     CustomAcknowledgementSerializer                    { get; set; }
        public CustomJObjectSerializerDelegate<StatusCode>?                          CustomStatusCodeSerializer                         { get; set; }

        public CustomJObjectSerializerDelegate<PullAuthenticationDataResponse>?      CustomPullAuthenticationDataResponseSerializer     { get; set; }
        public CustomJObjectSerializerDelegate<ProviderAuthenticationData>?          CustomProviderAuthenticationDataSerializer         { get; set; }

        public CustomJObjectSerializerDelegate<AuthorizationStartResponse>?          CustomAuthorizationStartSerializer                 { get; set; }
        public CustomJObjectSerializerDelegate<Identification>?                      CustomIdentificationSerializer                     { get; set; }
        public CustomJObjectSerializerDelegate<AuthorizationStopResponse>?           CustomAuthorizationStopSerializer                  { get; set; }

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
        protected internal Task logPushEVSEDataHTTPRequest(DateTime     Timestamp,
                                                           HTTPAPI      API,
                                                           HTTPRequest  Request)

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
        protected internal Task logPushEVSEDataHTTPResponse(DateTime      Timestamp,
                                                            HTTPAPI       API,
                                                            HTTPRequest   Request,
                                                            HTTPResponse  Response)

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
        protected internal Task logPushEVSEStatusHTTPRequest(DateTime     Timestamp,
                                                             HTTPAPI      API,
                                                             HTTPRequest  Request)

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
        protected internal Task logPushEVSEStatusHTTPResponse(DateTime      Timestamp,
                                                              HTTPAPI       API,
                                                              HTTPRequest   Request,
                                                              HTTPResponse  Response)

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
        protected internal Task logPushPricingProductDataHTTPRequest(DateTime     Timestamp,
                                                                     HTTPAPI      API,
                                                                     HTTPRequest  Request)

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
        protected internal Task logPushPricingProductDataHTTPResponse(DateTime      Timestamp,
                                                                      HTTPAPI       API,
                                                                      HTTPRequest   Request,
                                                                      HTTPResponse  Response)

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
        protected internal Task logPushEVSEPricingHTTPRequest(DateTime     Timestamp,
                                                              HTTPAPI      API,
                                                              HTTPRequest  Request)

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
        protected internal Task logPushEVSEPricingHTTPResponse(DateTime      Timestamp,
                                                               HTTPAPI       API,
                                                               HTTPRequest   Request,
                                                               HTTPResponse  Response)

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
        protected internal Task logPullAuthenticationDataHTTPRequest(DateTime     Timestamp,
                                                                     HTTPAPI      API,
                                                                     HTTPRequest  Request)

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
        protected internal Task logPullAuthenticationDataHTTPResponse(DateTime      Timestamp,
                                                                      HTTPAPI       API,
                                                                      HTTPRequest   Request,
                                                                      HTTPResponse  Response)

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
        protected internal Task logAuthorizeStartHTTPRequest(DateTime     Timestamp,
                                                             HTTPAPI      API,
                                                             HTTPRequest  Request)

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
        protected internal Task logAuthorizationStartHTTPResponse(DateTime      Timestamp,
                                                                  HTTPAPI       API,
                                                                  HTTPRequest   Request,
                                                                  HTTPResponse  Response)

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
        protected internal Task logAuthorizeStopHTTPRequest(DateTime     Timestamp,
                                                            HTTPAPI      API,
                                                            HTTPRequest  Request)

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
        protected internal Task logAuthorizationStopHTTPResponse(DateTime      Timestamp,
                                                                 HTTPAPI       API,
                                                                 HTTPRequest   Request,
                                                                 HTTPResponse  Response)

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
        protected internal Task logChargingNotificationHTTPRequest(DateTime     Timestamp,
                                                                   HTTPAPI      API,
                                                                   HTTPRequest  Request)

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
        protected internal Task logChargingNotificationHTTPResponse(DateTime      Timestamp,
                                                                    HTTPAPI       API,
                                                                    HTTPRequest   Request,
                                                                    HTTPResponse  Response)

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
        protected internal Task logChargeDetailRecordHTTPRequest(DateTime     Timestamp,
                                                                 HTTPAPI      API,
                                                                 HTTPRequest  Request)

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
        protected internal Task logChargeDetailRecordHTTPResponse(DateTime      Timestamp,
                                                                  HTTPAPI       API,
                                                                  HTTPRequest   Request,
                                                                  HTTPResponse  Response)

            => OnChargeDetailRecordHTTPResponse.WhenAll(Timestamp,
                                                        API ?? this,
                                                        Request,
                                                        Response);

        #endregion

        #endregion

        #region Constructor(s)

        #region CPOClientAPI(HTTPAPI, URLPathPrefix = null, ...)

        public CPOClientAPI(HTTPAPI    HTTPAPI,
                            HTTPPath?  URLPathPrefix    = null,

                            String     LoggingPath      = DefaultHTTPAPI_LoggingPath,
                            String     LoggingContext   = DefaultLoggingContext,
                            String     LogfileName      = DefaultHTTPAPI_LogfileName)

            : base(HTTPAPI)

        {

            this.URLPathPrefix  = base.URLPathPrefix + (URLPathPrefix ?? HTTPPath.Root);

            this.Counters       = new APICounters();

            this.HTTPLogger     = DisableLogging == false
                                      ? new Logger(this,
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
        /// <param name="ExternalDNSName">The offical URL/DNS name of this service, e.g. for sending e-mails.</param>
        /// <param name="HTTPServerPort">A TCP port to listen on.</param>
        /// <param name="BasePath">When the API is served from an optional subdirectory path.</param>
        /// <param name="HTTPServerName">The default HTTP servername, used whenever no HTTP Host-header has been given.</param>
        /// 
        /// <param name="URLPathPrefix">A common prefix for all URLs.</param>
        /// <param name="HTTPServiceName">The name of the HTTP service.</param>
        /// <param name="APIVersionHashes">The API version hashes (git commit hash values).</param>
        /// 
        /// <param name="ServerCertificateSelector">An optional delegate to select a SSL/TLS server certificate.</param>
        /// <param name="ClientCertificateValidator">An optional delegate to verify the SSL/TLS client certificate used for authentication.</param>
        /// <param name="ClientCertificateSelector">An optional delegate to select the SSL/TLS client certificate used for authentication.</param>
        /// <param name="AllowedTLSProtocols">The SSL/TLS protocol(s) allowed for this connection.</param>
        /// 
        /// <param name="ServerThreadName">The optional name of the TCP server thread.</param>
        /// <param name="ServerThreadPriority">The optional priority of the TCP server thread.</param>
        /// <param name="ServerThreadIsBackground">Whether the TCP server thread is a background thread or not.</param>
        /// <param name="ConnectionIdBuilder">An optional delegate to build a connection identification based on IP socket information.</param>
        /// <param name="ConnectionThreadsNameBuilder">An optional delegate to set the name of the TCP connection threads.</param>
        /// <param name="ConnectionThreadsPriorityBuilder">An optional delegate to set the priority of the TCP connection threads.</param>
        /// <param name="ConnectionThreadsAreBackground">Whether the TCP connection threads are background threads or not (default: yes).</param>
        /// <param name="ConnectionTimeout">The TCP client timeout for all incoming client connections in seconds (default: 30 sec).</param>
        /// <param name="MaxClientConnections">The maximum number of concurrent TCP client connections (default: 4096).</param>
        /// 
        /// <param name="DisableMaintenanceTasks">Disable all maintenance tasks.</param>
        /// <param name="MaintenanceInitialDelay">The initial delay of the maintenance tasks.</param>
        /// <param name="MaintenanceEvery">The maintenance intervall.</param>
        /// 
        /// <param name="DisableWardenTasks">Disable all warden tasks.</param>
        /// <param name="WardenInitialDelay">The initial delay of the warden tasks.</param>
        /// <param name="WardenCheckEvery">The warden intervall.</param>
        /// 
        /// <param name="IsDevelopment">This HTTP API runs in development mode.</param>
        /// <param name="DevelopmentServers">An enumeration of server names which will imply to run this service in development mode.</param>
        /// <param name="DisableLogging">Disable the log file.</param>
        /// <param name="LoggingPath">The path for all logfiles.</param>
        /// <param name="LoggingContext">The context of all logfiles.</param>
        /// <param name="LogfileCreator">A delegate for creating the name of the logfile for this API.</param>
        /// <param name="DNSClient">The DNS client of the API.</param>
        /// <param name="Autostart">Whether to start the API automatically.</param>
        public CPOClientAPI(HTTPHostname?                         HTTPHostname                       = null,
                            String?                               ExternalDNSName                    = null,
                            IPPort?                               HTTPServerPort                     = null,
                            HTTPPath?                             BasePath                           = null,
                            String                                HTTPServerName                     = DefaultHTTPServerName,

                            HTTPPath?                             URLPathPrefix                      = null,
                            String                                HTTPServiceName                    = DefaultHTTPServiceName,
                            Boolean                               RegisterRootService                = true,
                            JObject?                              APIVersionHashes                   = null,

                            ServerCertificateSelectorDelegate?    ServerCertificateSelector          = null,
                            LocalCertificateSelectionCallback?    ClientCertificateSelector          = null,
                            RemoteCertificateValidationCallback?  ClientCertificateValidator         = null,
                            SslProtocols?                         AllowedTLSProtocols                = null,
                            Boolean?                              ClientCertificateRequired          = null,
                            Boolean?                              CheckCertificateRevocation         = null,

                            String?                               ServerThreadName                   = null,
                            ThreadPriority?                       ServerThreadPriority               = null,
                            Boolean?                              ServerThreadIsBackground           = null,
                            ConnectionIdBuilder?                  ConnectionIdBuilder                = null,
                            ConnectionThreadsNameBuilder?         ConnectionThreadsNameBuilder       = null,
                            ConnectionThreadsPriorityBuilder?     ConnectionThreadsPriorityBuilder   = null,
                            Boolean?                              ConnectionThreadsAreBackground     = null,
                            TimeSpan?                             ConnectionTimeout                  = null,
                            UInt32?                               MaxClientConnections               = null,

                            Boolean?                              DisableMaintenanceTasks            = false,
                            TimeSpan?                             MaintenanceInitialDelay            = null,
                            TimeSpan?                             MaintenanceEvery                   = null,

                            Boolean?                              DisableWardenTasks                 = false,
                            TimeSpan?                             WardenInitialDelay                 = null,
                            TimeSpan?                             WardenCheckEvery                   = null,

                            Boolean?                              IsDevelopment                      = null,
                            IEnumerable<String>?                  DevelopmentServers                 = null,
                            Boolean                               DisableLogging                     = false,
                            String                                LoggingPath                        = DefaultHTTPAPI_LoggingPath,
                            String                                LoggingContext                     = DefaultLoggingContext,
                            String                                LogfileName                        = DefaultHTTPAPI_LogfileName,
                            LogfileCreatorDelegate?               LogfileCreator                     = null,
                            DNSClient?                            DNSClient                          = null,
                            Boolean                               Autostart                          = false)

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

                   ServerThreadName,
                   ServerThreadPriority,
                   ServerThreadIsBackground,
                   ConnectionIdBuilder,
                   ConnectionThreadsNameBuilder,
                   ConnectionThreadsPriorityBuilder,
                   ConnectionThreadsAreBackground,
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
                   false) //Autostart)

        {

            this.URLPathPrefix  = base.URLPathPrefix;

            this.Counters       = new APICounters();

            this.HTTPLogger     = DisableLogging == false
                                      ? new Logger(this,
                                                   LoggingPath,
                                                   LoggingContext ?? DefaultLoggingContext,
                                                   LogfileCreator)
                                      : null;

            RegisterURLTemplates(RegisterRootService);

            if (Autostart)
                Start();

        }

        #endregion

        #endregion


        #region (private) RegisterURLTemplates(RegisterRootService = true)

        private void RegisterURLTemplates(Boolean RegisterRootService = true)
        {

            #region ~/ (HTTPRoot)

            if (RegisterRootService)
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
                                                         Date            = Timestamp.Now,
                                                         ContentType     = HTTPContentType.TEXT_UTF8,
                                                         Content         = "This is an OICP v2.3 CPO Client HTTP/JSON endpoint!".ToUTF8Bytes(),
                                                         CacheControl    = "public, max-age=300",
                                                         Connection      = "close"
                                                     }.AsImmutable);
                                             },
                                             AllowReplacement: URLReplacement.Allow);

            #endregion


            //Note: OperatorId is the remote CPO sending an authorize start/stop request!

            #region POST  ~/api/oicp/evsepull/v23/operators/{operatorId}/data-records

            // -----------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/evsepush/v23/operators/DE-GEF/data-records
            // -----------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/evsepush/v23/operators/{operatorId}/data-records",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logPushEVSEDataHTTPRequest,
                                         HTTPResponseLogger:  logPushEVSEDataHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime  = Timestamp.Now;
                                             var processId  = Process_Id.NewRandom;

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

                                                 else if (PushEVSEDataRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                       //     operatorId ????
                                                                                       out PushEVSEDataRequest?          pullEVSEDataRequest,
                                                                                       out String?                       errorResponse,
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
                                                        AccessControlAllowMethods  = "POST",
                                                        AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                        ContentType                = HTTPContentType.JSON_UTF8,
                                                        Content                    = pullEVSEDataResponse.Response?.ToJSON(CustomAcknowledgementSerializer,
                                                                                                                           CustomStatusCodeSerializer).
                                                                                                                    ToString(JSONFormatting).
                                                                                                                    ToUTF8Bytes()
                                                                                                          ?? Array.Empty<Byte>(),
                                                        ProcessID                  = processId.ToString(),
                                                        Connection                 = "close"
                                                    }.AsImmutable;

                                          }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region POST  ~/api/oicp/evsepull/v21/operators/{operatorId}/status-records

            // -------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/evsepush/v21/operators/DE-GEF/status-records
            // -------------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/evsepush/v21/operators/{operatorId}/status-records",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logPushEVSEStatusHTTPRequest,
                                         HTTPResponseLogger:  logPushEVSEStatusHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime  = Timestamp.Now;
                                             var processId  = Process_Id.NewRandom;

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

                                                 else if (PushEVSEStatusRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                         //     operatorId ????
                                                                                         out PushEVSEStatusRequest?          pullEVSEStatusRequest,
                                                                                         out String?                         errorResponse,
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
                                                        AccessControlAllowMethods  = "POST",
                                                        AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                        ContentType                = HTTPContentType.JSON_UTF8,
                                                        Content                    = pullEVSEStatusResponse.Response?.ToJSON(CustomAcknowledgementSerializer,
                                                                                                                             CustomStatusCodeSerializer).
                                                                                                                      ToString(JSONFormatting).
                                                                                                                      ToUTF8Bytes()
                                                                                                            ?? Array.Empty<Byte>(),
                                                        ProcessID                  = processId.ToString(),
                                                        Connection                 = "close"
                                                    }.AsImmutable;

                                          }, AllowReplacement: URLReplacement.Allow);

            #endregion


            #region POST  ~/api/oicp/dynamicpricing/v10/operators/{operatorId}/pricing-products

            // -----------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/dynamicpricing/v10/operators/DE*GEF/pricing-products
            // -----------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/dynamicpricing/v10/operators/{operatorId}/pricing-products",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logPushPricingProductDataHTTPRequest,
                                         HTTPResponseLogger:  logPushPricingProductDataHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime  = Timestamp.Now;
                                             var processId  = Process_Id.NewRandom;

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

                                                 else if (PushPricingProductDataRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                                 //     operatorId ????
                                                                                                 out PushPricingProductDataRequest?          pullEVSEDataRequest,
                                                                                                 out String?                                 errorResponse,
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
                                                        AccessControlAllowMethods  = "POST",
                                                        AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                        ContentType                = HTTPContentType.JSON_UTF8,
                                                        Content                    = pushPricingProductDataResponse.Response?.ToJSON(CustomAcknowledgementSerializer,
                                                                                                                                     CustomStatusCodeSerializer).
                                                                                                                              ToString(JSONFormatting).
                                                                                                                              ToUTF8Bytes()
                                                                                                                    ?? Array.Empty<Byte>(),
                                                        ProcessID                  = processId.ToString(),
                                                        Connection                 = "close"
                                                    }.AsImmutable;

                                          }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region POST  ~/api/oicp/dynamicpricing/v10/operators/{operatorId}/evse-pricing

            // -----------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/dynamicpricing/v10/operators/DE-GEF/evse-pricing
            // -----------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/dynamicpricing/v10/operators/{operatorId}/evse-pricing",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logPushEVSEPricingHTTPRequest,
                                         HTTPResponseLogger:  logPushEVSEPricingHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime  = Timestamp.Now;
                                             var processId  = Process_Id.NewRandom;

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

                                                 else if (PushEVSEPricingRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                          operatorId,
                                                                                          out PushEVSEPricingRequest?          pullEVSEDataRequest,
                                                                                          out String?                          errorResponse,
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
                                                        AccessControlAllowMethods  = "POST",
                                                        AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                        ContentType                = HTTPContentType.JSON_UTF8,
                                                        Content                    = pushEVSEPricingResponse.Response?.ToJSON(CustomAcknowledgementSerializer,
                                                                                                                              CustomStatusCodeSerializer).
                                                                                                                       ToString(JSONFormatting).
                                                                                                                       ToUTF8Bytes()
                                                                                                             ?? Array.Empty<Byte>(),
                                                        ProcessID                  = processId.ToString(),
                                                        Connection                 = "close"
                                                    }.AsImmutable;

                                          }, AllowReplacement: URLReplacement.Allow);

            #endregion


            #region POST  ~/api/oicp/authdata/v21/operators/{operatorId}/pull-request

            // -----------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/authdata/v21/operators/DE*GEF/pull-request
            // -----------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/authdata/v21/operators/{operatorId}/pull-request",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logPullAuthenticationDataHTTPRequest,
                                         HTTPResponseLogger:  logPullAuthenticationDataHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime  = Timestamp.Now;
                                             var processId  = Process_Id.NewRandom;

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
                                                                                              Array.Empty<ProviderAuthenticationData>(),
                                                                                              StatusCode: new StatusCode(
                                                                                                              StatusCodes.SystemError,
                                                                                                              "The expected 'operatorId' URL parameter could not be parsed!"
                                                                                                          )
                                                                                          )
                                                                                      );

                                                 }

                                                 #endregion

                                                 else if (PullAuthenticationDataRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                                 //operatorId,
                                                                                                 out PullAuthenticationDataRequest?          pullAuthenticationDataRequest,
                                                                                                 out String?                                 errorResponse,
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
                                                                                                      Array.Empty<ProviderAuthenticationData>(),
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
                                                                                                  Array.Empty<ProviderAuthenticationData>(),
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
                                                                                              Array.Empty<ProviderAuthenticationData>(),
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
                                                                                          Array.Empty<ProviderAuthenticationData>(),
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
                                                        AccessControlAllowMethods  = "POST",
                                                        AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                        ContentType                = HTTPContentType.JSON_UTF8,
                                                        Content                    = pullAuthenticationDataResponse.Response?.ToJSON(CustomPullAuthenticationDataResponseSerializer,
                                                                                                                                     CustomProviderAuthenticationDataSerializer,
                                                                                                                                     CustomIdentificationSerializer,
                                                                                                                                     CustomStatusCodeSerializer).
                                                                                                                              ToString(JSONFormatting).
                                                                                                                              ToUTF8Bytes()
                                                                                                                    ?? Array.Empty<Byte>(),
                                                        ProcessID                  = processId.ToString(),
                                                        Connection                 = "close"
                                                    }.AsImmutable;

                                          }, AllowReplacement: URLReplacement.Allow);

            #endregion


            #region POST  ~/api/oicp/charging/v21/operators/{operatorId}/authorize/start

            // --------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/charging/v21/operators/DE*GEF/authorize/start
            // --------------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/charging/v21/operators/{operatorId}/authorize/start",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logAuthorizeStartHTTPRequest,
                                         HTTPResponseLogger:  logAuthorizationStartHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime  = Timestamp.Now;
                                             var processId  = Process_Id.NewRandom;

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

                                                 else if (AuthorizeStartRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                         operatorId,
                                                                                         out AuthorizeStartRequest?          pullEVSEDataRequest,
                                                                                         out String?                         errorResponse,
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
                                                                                              pullEVSEDataRequest!))).
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
                                                                                                                                                  pullEVSEDataRequest!))))?.FirstOrDefault();

                                                             Counters.AuthorizeStart.IncResponses_OK();

                                                         }
                                                         catch (Exception e)
                                                         {

                                                             Counters.AuthorizeStart.IncResponses_Error();

                                                             authorizationStartResponse = OICPResult<AuthorizationStartResponse>.Failed(
                                                                                              this,
                                                                                              AuthorizationStartResponse.DataError(
                                                                                                  pullEVSEDataRequest,
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
                                                                                              pullEVSEDataRequest,
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
                                                                                          pullEVSEDataRequest,
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
                                                        AccessControlAllowMethods  = "POST",
                                                        AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                        ContentType                = HTTPContentType.JSON_UTF8,
                                                        Content                    = authorizationStartResponse.Response?.ToJSON(CustomAuthorizationStartSerializer,
                                                                                                                                 CustomStatusCodeSerializer,
                                                                                                                                 CustomIdentificationSerializer).
                                                                                                                          ToString(JSONFormatting).
                                                                                                                          ToUTF8Bytes()
                                                                                                                ?? Array.Empty<Byte>(),
                                                        ProcessID                  = processId.ToString(),
                                                        Connection                 = "close"
                                                    }.AsImmutable;

                                          }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region POST  ~/api/oicp/charging/v21/operators/{operatorId}/authorize/stop

            // --------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/charging/v21/operators/DE*GEF/authorize/stop
            // --------------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/charging/v21/operators/{operatorId}/authorize/stop",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logAuthorizeStopHTTPRequest,
                                         HTTPResponseLogger:  logAuthorizationStopHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime  = Timestamp.Now;
                                             var processId  = Process_Id.NewRandom;

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

                                                 else if (AuthorizeStopRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                        operatorId,
                                                                                        out AuthorizeStopRequest?          pullEVSEDataRequest,
                                                                                        out String?                        errorResponse,
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
                                                                                              pullEVSEDataRequest!))).
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
                                                                                                                                                pullEVSEDataRequest!))))?.FirstOrDefault();

                                                             Counters.AuthorizeStop.IncResponses_OK();

                                                         }
                                                         catch (Exception e)
                                                         {

                                                             Counters.AuthorizeStop.IncResponses_Error();

                                                             authorizationStopResponse = OICPResult<AuthorizationStopResponse>.Failed(
                                                                                             this,
                                                                                             AuthorizationStopResponse.DataError(
                                                                                                 pullEVSEDataRequest,
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
                                                                                             pullEVSEDataRequest,
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
                                                                                         pullEVSEDataRequest,
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
                                                        AccessControlAllowMethods  = "POST",
                                                        AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                        ContentType                = HTTPContentType.JSON_UTF8,
                                                        Content                    = authorizationStopResponse.Response?.ToJSON(CustomAuthorizationStopSerializer,
                                                                                                                                CustomStatusCodeSerializer).
                                                                                                                         ToString(JSONFormatting).
                                                                                                                         ToUTF8Bytes()
                                                                                                               ?? Array.Empty<Byte>(),
                                                        ProcessID                  = processId.ToString(),
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
                                         HTTPRequestLogger:   logChargingNotificationHTTPRequest,
                                         HTTPResponseLogger:  logChargingNotificationHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime  = Timestamp.Now;
                                             var processId  = Process_Id.NewRandom;

                                             OICPResult<Acknowledgement>? chargingNotificationResponse = null;

                                             try
                                             {

                                                 if (Request.TryParseJObjectRequestBody(out JObject JSONRequest, out _) &&
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
                                                        AccessControlAllowMethods  = "POST",
                                                        AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                        ContentType                = HTTPContentType.JSON_UTF8,
                                                        Content                    = chargingNotificationResponse.Response?.ToJSON(CustomAcknowledgementSerializer,
                                                                                                                                   CustomStatusCodeSerializer).
                                                                                                                            ToString(JSONFormatting).
                                                                                                                            ToUTF8Bytes()
                                                                                                                  ?? Array.Empty<Byte>(),
                                                        ProcessID                  = processId.ToString(),
                                                        Connection                 = "close"
                                                    }.AsImmutable;

                                          }, AllowReplacement: URLReplacement.Allow);

            #endregion


            #region POST  ~/api/oicp/cdrmgmt/v22/operators/{operatorId}/charge-detail-record

            // ------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/cdrmgmt/v22/operators/DE-GEF/charge-detail-record
            // ------------------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/cdrmgmt/v22/operators/{operatorId}/charge-detail-record",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logChargeDetailRecordHTTPRequest,
                                         HTTPResponseLogger:  logChargeDetailRecordHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime  = Timestamp.Now;
                                             var processId  = Process_Id.NewRandom;

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

                                                 else if (ChargeDetailRecordRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                             operatorId,
                                                                                             out ChargeDetailRecordRequest?          chargeDetailRecordRequest,
                                                                                             out String?                             errorResponse,
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
                                                        AccessControlAllowMethods  = "POST",
                                                        AccessControlAllowHeaders  = "Content-Type, Accept, Authorization",
                                                        ContentType                = HTTPContentType.JSON_UTF8,
                                                        Content                    = chargeDetailRecordResponse.Response?.ToJSON(CustomAcknowledgementSerializer,
                                                                                                                                 CustomStatusCodeSerializer).
                                                                                                                          ToString(JSONFormatting).
                                                                                                                          ToUTF8Bytes()
                                                                                                                ?? Array.Empty<Byte>(),
                                                        ProcessID                  = processId.ToString(),
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
        public override void Dispose()
        { }

        #endregion

    }

}
