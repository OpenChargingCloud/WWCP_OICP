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

namespace cloud.charging.open.protocols.OICPv2_3.EMP
{

    /// <summary>
    /// The EMP HTTP Client API.
    /// </summary>
    public partial class EMPClientAPI : HTTPAPI
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


            public APICounters(APICounterValues? PullEVSEData                      = null,
                               APICounterValues? PullEVSEStatus                    = null,
                               APICounterValues? PullEVSEStatusById                = null,
                               APICounterValues? PullEVSEStatusByOperatorId        = null,

                               APICounterValues? PullPricingProductData            = null,
                               APICounterValues? PullEVSEPricing                   = null,

                               APICounterValues? PushAuthenticationData            = null,

                               APICounterValues? AuthorizeRemoteReservationStart   = null,
                               APICounterValues? AuthorizeRemoteReservationStop    = null,
                               APICounterValues? AuthorizeRemoteStart              = null,
                               APICounterValues? AuthorizeRemoteStop               = null,

                               APICounterValues? GetChargeDetailRecords            = null)
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
        /// The default HTTP server name.
        /// </summary>
        public new const String  DefaultHTTPServerName   = "GraphDefined OICP " + Version.Number + " EMP HTTP API";

        /// <summary>
        /// The default HTTP service name.
        /// </summary>
        public new const String  DefaultHTTPServiceName  = "GraphDefined OICP " + Version.Number + " EMP HTTP API";

        /// <summary>
        /// The default logging context.
        /// </summary>
        public     const String  DefaultLoggingContext   = "EMPServerAPI";

        #endregion

        #region Properties

        public APICounters                                                           Counters                                              { get; }


        // Custom JSON parsers

        public CustomJObjectParserDelegate<PullEVSEDataRequest>?                     CustomPullEVSEDataRequestParser                       { get; set; }
        public CustomJObjectParserDelegate<PullEVSEStatusRequest>?                   CustomPullEVSEStatusRequestParser                     { get; set; }
        public CustomJObjectParserDelegate<PullEVSEStatusByIdRequest>?               CustomPullEVSEStatusByIdRequestParser                 { get; set; }
        public CustomJObjectParserDelegate<PullEVSEStatusByOperatorIdRequest>?       CustomPullEVSEStatusByOperatorIdRequestParser         { get; set; }

        public CustomJObjectParserDelegate<PullPricingProductDataRequest>?           CustomPullPricingProductDataRequestParser             { get; set; }
        public CustomJObjectParserDelegate<PullEVSEPricingRequest>?                  CustomPullEVSEPricingRequestParser                    { get; set; }

        public CustomJObjectParserDelegate<PushAuthenticationDataRequest>?           CustomPushAuthenticationDataRequestParser             { get; set; }

        public CustomJObjectParserDelegate<AuthorizeRemoteReservationStartRequest>?  CustomAuthorizeRemoteReservationStartRequestParser    { get; set; }
        public CustomJObjectParserDelegate<AuthorizeRemoteReservationStopRequest>?   CustomAuthorizeRemoteReservationStopRequestParser     { get; set; }
        public CustomJObjectParserDelegate<AuthorizeRemoteStartRequest>?             CustomAuthorizeRemoteStartRequestParser               { get; set; }
        public CustomJObjectParserDelegate<AuthorizeRemoteStopRequest>?              CustomAuthorizeRemoteStopRequestParser                { get; set; }

        public CustomJObjectParserDelegate<GetChargeDetailRecordsRequest>?           CustomGetChargeDetailRecordsRequestParser             { get; set; }



        // Custom JSON serializers

        public CustomJObjectSerializerDelegate<PullEVSEDataResponse>?                CustomPullEVSEDataResponseSerializer                  { get; set; }
        public CustomJObjectSerializerDelegate<EVSEDataRecord>?                      CustomEVSEDataRecordSerializer                        { get; set; }
        public CustomJObjectSerializerDelegate<Address>?                             CustomAddressSerializer                               { get; set; }
        public CustomJObjectSerializerDelegate<ChargingFacility>?                    CustomChargingFacilitySerializer                      { get; set; }
        public CustomJObjectSerializerDelegate<GeoCoordinates>?                      CustomGeoCoordinatesSerializer                        { get; set; }
        public CustomJObjectSerializerDelegate<EnergySource>?                        CustomEnergySourceSerializer                          { get; set; }
        public CustomJObjectSerializerDelegate<EnvironmentalImpact>?                 CustomEnvironmentalImpactSerializer                   { get; set; }
        public CustomJObjectSerializerDelegate<OpeningTime>?                         CustomOpeningTimesSerializer                          { get; set; }
        public CustomJObjectSerializerDelegate<StatusCode>?                          CustomStatusCodeSerializer                            { get; set; }


        public CustomJObjectSerializerDelegate<PullEVSEStatusResponse>?              CustomPullEVSEStatusResponseSerializer                { get; set; }
        public CustomJObjectSerializerDelegate<OperatorEVSEStatus>?                  CustomOperatorEVSEStatusSerializer                    { get; set; }
        public CustomJObjectSerializerDelegate<EVSEStatusRecord>?                    CustomEVSEStatusRecordSerializer                      { get; set; }

        public CustomJObjectSerializerDelegate<PullEVSEStatusByIdResponse>?          CustomPullEVSEStatusByIdResponseSerializer            { get; set; }
        public CustomJObjectSerializerDelegate<PullEVSEStatusByOperatorIdResponse>?  CustomPullEVSEStatusByOperatorIdResponseSerializer    { get; set; }


        public CustomJObjectSerializerDelegate<PullPricingProductDataResponse>?      CustomPullPricingProductDataResponseSerializer        { get; set; }
        public CustomJObjectSerializerDelegate<PricingProductData>?                  CustomPricingProductDataSerializer                    { get; set; }
        public CustomJObjectSerializerDelegate<PricingProductDataRecord>?            CustomPricingProductDataRecordSerializer              { get; set; }

        public CustomJObjectSerializerDelegate<PullEVSEPricingResponse>?             CustomPullEVSEPricingResponseSerializer               { get; set; }
        public CustomJObjectSerializerDelegate<OperatorEVSEPricing>?                 CustomOperatorEVSEPricingSerializer                   { get; set; }
        public CustomJObjectSerializerDelegate<EVSEPricing>?                         CustomEVSEPricingSerializer                           { get; set; }


        public CustomJObjectSerializerDelegate<GetChargeDetailRecordsResponse>?      CustomGetChargeDetailRecordsResponseSerializer        { get; set; }
        public CustomJObjectSerializerDelegate<IPagedResponse>?                      CustomIPagedResponseSerializer                        { get; set; }
        public CustomJObjectSerializerDelegate<ChargeDetailRecord>?                  CustomChargeDetailRecordSerializer                    { get; set; }
        public CustomJObjectSerializerDelegate<Identification>?                      CustomIdentificationSerializer                        { get; set; }
        public CustomJObjectSerializerDelegate<SignedMeteringValue>?                 CustomSignedMeteringValueSerializer                   { get; set; }
        public CustomJObjectSerializerDelegate<CalibrationLawVerification>?          CustomCalibrationLawVerificationSerializer            { get; set; }

        public CustomJObjectSerializerDelegate<Acknowledgement>?                     CustomAcknowledgementSerializer                       { get; set; }


        public Newtonsoft.Json.Formatting                                            JSONFormatting                                        { get; set; }

        #endregion

        #region Events

        #region (protected internal) OnPullEVSEDataHTTPRequest

        /// <summary>
        /// An event sent whenever an PullEVSEData HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnPullEVSEDataHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an PullEVSEData HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logPullEVSEDataHTTPRequest(DateTime     Timestamp,
                                                           HTTPAPI      API,
                                                           HTTPRequest  Request)

            => OnPullEVSEDataHTTPRequest.WhenAll(Timestamp,
                                                 API ?? this,
                                                 Request);

        #endregion

        #region (event)              OnPullEVSEData(Request-/Response)

        /// <summary>
        /// An event send whenever a PullEVSEData request was received.
        /// </summary>
        public event OnPullEVSEDataAPIRequestDelegate?   OnPullEVSEDataRequest;

        /// <summary>
        /// An event send whenever a PullEVSEData request was received.
        /// </summary>
        public event OnPullEVSEDataAPIDelegate?          OnPullEVSEData;

        /// <summary>
        /// An event send whenever a response to a PullEVSEData request was sent.
        /// </summary>
        public event OnPullEVSEDataAPIResponseDelegate?  OnPullEVSEDataResponse;

        #endregion

        #region (protected internal) OnPullEVSEDataHTTPResponse

        /// <summary>
        /// An event sent whenever an PullEVSEData HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnPullEVSEDataHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an PullEVSEData HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logPullEVSEDataHTTPResponse(DateTime      Timestamp,
                                                            HTTPAPI       API,
                                                            HTTPRequest   Request,
                                                            HTTPResponse  Response)

            => OnPullEVSEDataHTTPResponse.WhenAll(Timestamp,
                                                  API ?? this,
                                                  Request,
                                                  Response);

        #endregion


        #region (protected internal) OnPullEVSEStatusHTTPRequest

        /// <summary>
        /// An event sent whenever an PullEVSEStatus HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnPullEVSEStatusHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an PullEVSEStatus HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logPullEVSEStatusHTTPRequest(DateTime     Timestamp,
                                                             HTTPAPI      API,
                                                             HTTPRequest  Request)

            => OnPullEVSEStatusHTTPRequest.WhenAll(Timestamp,
                                                   API ?? this,
                                                   Request);

        #endregion

        #region (event)              OnPullEVSEStatus(Request-/Response)

        /// <summary>
        /// An event send whenever a PullEVSEStatus request was received.
        /// </summary>
        public event OnPullEVSEStatusAPIRequestDelegate?   OnPullEVSEStatusRequest;

        /// <summary>
        /// An event send whenever a PullEVSEStatus request was received.
        /// </summary>
        public event OnPullEVSEStatusAPIDelegate?          OnPullEVSEStatus;

        /// <summary>
        /// An event send whenever a response to a PullEVSEStatus request was sent.
        /// </summary>
        public event OnPullEVSEStatusAPIResponseDelegate?  OnPullEVSEStatusResponse;

        #endregion

        #region (protected internal) OnPullEVSEStatusHTTPResponse

        /// <summary>
        /// An event sent whenever an PullEVSEStatus HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnPullEVSEStatusHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an PullEVSEStatus HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logPullEVSEStatusHTTPResponse(DateTime      Timestamp,
                                                              HTTPAPI       API,
                                                              HTTPRequest   Request,
                                                              HTTPResponse  Response)

            => OnPullEVSEStatusHTTPResponse.WhenAll(Timestamp,
                                                    API ?? this,
                                                    Request,
                                                    Response);

        #endregion


        #region (protected internal) OnPullEVSEStatusByIdHTTPRequest

        /// <summary>
        /// An event sent whenever an PullEVSEStatusById HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnPullEVSEStatusByIdHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an PullEVSEStatusById HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logPullEVSEStatusByIdHTTPRequest(DateTime     Timestamp,
                                                                 HTTPAPI      API,
                                                                 HTTPRequest  Request)

            => OnPullEVSEStatusByIdHTTPRequest.WhenAll(Timestamp,
                                                       API ?? this,
                                                       Request);

        #endregion

        #region (event)              OnPullEVSEStatusById(Request-/Response)

        /// <summary>
        /// An event send whenever a PullEVSEStatusById request was received.
        /// </summary>
        public event OnPullEVSEStatusByIdAPIRequestDelegate?   OnPullEVSEStatusByIdRequest;

        /// <summary>
        /// An event send whenever a PullEVSEStatusById request was received.
        /// </summary>
        public event OnPullEVSEStatusByIdAPIDelegate?          OnPullEVSEStatusById;

        /// <summary>
        /// An event send whenever a response to a PullEVSEStatusById request was sent.
        /// </summary>
        public event OnPullEVSEStatusByIdAPIResponseDelegate?  OnPullEVSEStatusByIdResponse;

        #endregion

        #region (protected internal) OnPullEVSEStatusByIdHTTPResponse

        /// <summary>
        /// An event sent whenever an PullEVSEStatusById HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnPullEVSEStatusByIdHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an PullEVSEStatusById HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logPullEVSEStatusByIdHTTPResponse(DateTime      Timestamp,
                                                                  HTTPAPI       API,
                                                                  HTTPRequest   Request,
                                                                  HTTPResponse  Response)

            => OnPullEVSEStatusByIdHTTPResponse.WhenAll(Timestamp,
                                                        API ?? this,
                                                        Request,
                                                        Response);

        #endregion


        #region (protected internal) OnPullEVSEStatusByOperatorIdHTTPRequest

        /// <summary>
        /// An event sent whenever an PullEVSEStatusByOperatorId HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnPullEVSEStatusByOperatorIdHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an PullEVSEStatusByOperatorId HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logPullEVSEStatusByOperatorIdHTTPRequest(DateTime     Timestamp,
                                                                         HTTPAPI      API,
                                                                         HTTPRequest  Request)

            => OnPullEVSEStatusByOperatorIdHTTPRequest.WhenAll(Timestamp,
                                                               API ?? this,
                                                               Request);

        #endregion

        #region (event)              OnPullEVSEStatusByOperatorId(Request-/Response)

        /// <summary>
        /// An event send whenever a PullEVSEStatusByOperatorId request was received.
        /// </summary>
        public event OnPullEVSEStatusByOperatorIdAPIRequestDelegate?   OnPullEVSEStatusByOperatorIdRequest;

        /// <summary>
        /// An event send whenever a PullEVSEStatusByOperatorId request was received.
        /// </summary>
        public event OnPullEVSEStatusByOperatorIdAPIDelegate?          OnPullEVSEStatusByOperatorId;

        /// <summary>
        /// An event send whenever a response to a PullEVSEStatusByOperatorId request was sent.
        /// </summary>
        public event OnPullEVSEStatusByOperatorIdAPIResponseDelegate?  OnPullEVSEStatusByOperatorIdResponse;

        #endregion

        #region (protected internal) OnPullEVSEStatusByOperatorIdHTTPResponse

        /// <summary>
        /// An event sent whenever an PullEVSEStatusByOperatorId HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnPullEVSEStatusByOperatorIdHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an PullEVSEStatusByOperatorId HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logPullEVSEStatusByOperatorIdHTTPResponse(DateTime      Timestamp,
                                                                          HTTPAPI       API,
                                                                          HTTPRequest   Request,
                                                                          HTTPResponse  Response)

            => OnPullEVSEStatusByOperatorIdHTTPResponse.WhenAll(Timestamp,
                                                                API ?? this,
                                                                Request,
                                                                Response);

        #endregion



        #region (protected internal) OnPullPricingProductDataHTTPRequest

        /// <summary>
        /// An event sent whenever an PullPricingProductData HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnPullPricingProductDataHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an PullPricingProductData HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logPullPricingProductDataHTTPRequest(DateTime     Timestamp,
                                                                     HTTPAPI      API,
                                                                     HTTPRequest  Request)

            => OnPullPricingProductDataHTTPRequest.WhenAll(Timestamp,
                                                           API ?? this,
                                                           Request);

        #endregion

        #region (event)              OnPullPricingProductData(Request-/Response)

        /// <summary>
        /// An event send whenever a PullPricingProductData request was received.
        /// </summary>
        public event OnPullPricingProductDataAPIRequestDelegate?   OnPullPricingProductDataRequest;

        /// <summary>
        /// An event send whenever a PullPricingProductData request was received.
        /// </summary>
        public event OnPullPricingProductDataAPIDelegate?          OnPullPricingProductData;

        /// <summary>
        /// An event send whenever a response to a PullPricingProductData request was sent.
        /// </summary>
        public event OnPullPricingProductDataAPIResponseDelegate?  OnPullPricingProductDataResponse;

        #endregion

        #region (protected internal) OnPullPricingProductDataHTTPResponse

        /// <summary>
        /// An event sent whenever an PullPricingProductData HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnPullPricingProductDataHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an PullPricingProductData HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logPullPricingProductDataHTTPResponse(DateTime      Timestamp,
                                                                      HTTPAPI       API,
                                                                      HTTPRequest   Request,
                                                                      HTTPResponse  Response)

            => OnPullPricingProductDataHTTPResponse.WhenAll(Timestamp,
                                                            API ?? this,
                                                            Request,
                                                            Response);

        #endregion


        #region (protected internal) OnPullEVSEPricingHTTPRequest

        /// <summary>
        /// An event sent whenever an PullEVSEPricing HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnPullEVSEPricingHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an PullEVSEPricing HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logPullEVSEPricingHTTPRequest(DateTime     Timestamp,
                                                              HTTPAPI      API,
                                                              HTTPRequest  Request)

            => OnPullEVSEPricingHTTPRequest.WhenAll(Timestamp,
                                                    API ?? this,
                                                    Request);

        #endregion

        #region (event)              OnPullEVSEPricing(Request-/Response)

        /// <summary>
        /// An event send whenever a PullEVSEPricing request was received.
        /// </summary>
        public event OnPullEVSEPricingAPIRequestDelegate?   OnPullEVSEPricingRequest;

        /// <summary>
        /// An event send whenever a PullEVSEPricing request was received.
        /// </summary>
        public event OnPullEVSEPricingAPIDelegate?          OnPullEVSEPricing;

        /// <summary>
        /// An event send whenever a response to a PullEVSEPricing request was sent.
        /// </summary>
        public event OnPullEVSEPricingAPIResponseDelegate?  OnPullEVSEPricingResponse;

        #endregion

        #region (protected internal) OnPullEVSEPricingHTTPResponse

        /// <summary>
        /// An event sent whenever an PullEVSEPricing HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnPullEVSEPricingHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an PullEVSEPricing HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logPullEVSEPricingHTTPResponse(DateTime      Timestamp,
                                                               HTTPAPI       API,
                                                               HTTPRequest   Request,
                                                               HTTPResponse  Response)

            => OnPullEVSEPricingHTTPResponse.WhenAll(Timestamp,
                                                     API ?? this,
                                                     Request,
                                                     Response);

        #endregion



        #region (protected internal) OnPushAuthenticationDataHTTPRequest

        /// <summary>
        /// An event sent whenever an PushAuthenticationData HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnPushAuthenticationDataHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an PushAuthenticationData HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logPushAuthenticationDataHTTPRequest(DateTime     Timestamp,
                                                                     HTTPAPI      API,
                                                                     HTTPRequest  Request)

            => OnPushAuthenticationDataHTTPRequest.WhenAll(Timestamp,
                                                           API ?? this,
                                                           Request);

        #endregion

        #region (event)              OnPushAuthenticationData(Request-/Response)

        /// <summary>
        /// An event send whenever a PushAuthenticationData request was received.
        /// </summary>
        public event OnPushAuthenticationDataAPIRequestDelegate?   OnPushAuthenticationDataRequest;

        /// <summary>
        /// An event send whenever a PushAuthenticationData request was received.
        /// </summary>
        public event OnPushAuthenticationDataAPIDelegate?          OnPushAuthenticationData;

        /// <summary>
        /// An event send whenever a response to a PushAuthenticationData request was sent.
        /// </summary>
        public event OnPushAuthenticationDataAPIResponseDelegate?  OnPushAuthenticationDataResponse;

        #endregion

        #region (protected internal) OnPushAuthenticationDataHTTPResponse

        /// <summary>
        /// An event sent whenever an PushAuthenticationData HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnPushAuthenticationDataHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an PushAuthenticationData HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logPushAuthenticationDataHTTPResponse(DateTime      Timestamp,
                                                                      HTTPAPI       API,
                                                                      HTTPRequest   Request,
                                                                      HTTPResponse  Response)

            => OnPushAuthenticationDataHTTPResponse.WhenAll(Timestamp,
                                                            API ?? this,
                                                            Request,
                                                            Response);

        #endregion



        #region (protected internal) OnAuthorizeRemoteReservationStartHTTPRequest

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteReservationStart HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnAuthorizeRemoteReservationStartHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteReservationStart HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logAuthorizeRemoteReservationStartHTTPRequest(DateTime     Timestamp,
                                                                              HTTPAPI      API,
                                                                              HTTPRequest  Request)

            => OnAuthorizeRemoteReservationStartHTTPRequest.WhenAll(Timestamp,
                                                                    API ?? this,
                                                                    Request);

        #endregion

        #region (event)              OnAuthorizeRemoteReservationStart(Request-/Response)

        /// <summary>
        /// An event send whenever a AuthorizeRemoteReservationStart request was received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStartAPIRequestDelegate?   OnAuthorizeRemoteReservationStartRequest;

        /// <summary>
        /// An event send whenever a AuthorizeRemoteReservationStart request was received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStartAPIDelegate?          OnAuthorizeRemoteReservationStart;

        /// <summary>
        /// An event send whenever a response to a AuthorizeRemoteReservationStart request was sent.
        /// </summary>
        public event OnAuthorizeRemoteReservationStartAPIResponseDelegate?  OnAuthorizeRemoteReservationStartResponse;

        #endregion

        #region (protected internal) OnAuthorizeRemoteReservationStartHTTPResponse

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteReservationStart HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnAuthorizeRemoteReservationStartHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteReservationStart HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logAuthorizeRemoteReservationStartHTTPResponse(DateTime      Timestamp,
                                                                               HTTPAPI       API,
                                                                               HTTPRequest   Request,
                                                                               HTTPResponse  Response)

            => OnAuthorizeRemoteReservationStartHTTPResponse.WhenAll(Timestamp,
                                                                     API ?? this,
                                                                     Request,
                                                                     Response);

        #endregion


        #region (protected internal) OnAuthorizeRemoteReservationStopHTTPRequest

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteReservationStop HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnAuthorizeRemoteReservationStopHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteReservationStop HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logAuthorizeRemoteReservationStopHTTPRequest(DateTime     Timestamp,
                                                                             HTTPAPI      API,
                                                                             HTTPRequest  Request)

            => OnAuthorizeRemoteReservationStopHTTPRequest.WhenAll(Timestamp,
                                                                   API ?? this,
                                                                   Request);

        #endregion

        #region (event)              OnAuthorizeRemoteReservationStop(Request-/Response)

        /// <summary>
        /// An event send whenever a AuthorizeRemoteReservationStop request was received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStopAPIRequestDelegate?   OnAuthorizeRemoteReservationStopRequest;

        /// <summary>
        /// An event send whenever a AuthorizeRemoteReservationStop request was received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStopAPIDelegate?          OnAuthorizeRemoteReservationStop;

        /// <summary>
        /// An event send whenever a response to a AuthorizeRemoteReservationStop request was sent.
        /// </summary>
        public event OnAuthorizeRemoteReservationStopAPIResponseDelegate?  OnAuthorizeRemoteReservationStopResponse;

        #endregion

        #region (protected internal) OnAuthorizeRemoteReservationStopHTTPResponse

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteReservationStop HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnAuthorizeRemoteReservationStopHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteReservationStop HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logAuthorizeRemoteReservationStopHTTPResponse(DateTime      Timestamp,
                                                                              HTTPAPI       API,
                                                                              HTTPRequest   Request,
                                                                              HTTPResponse  Response)

            => OnAuthorizeRemoteReservationStopHTTPResponse.WhenAll(Timestamp,
                                                                    API ?? this,
                                                                    Request,
                                                                    Response);

        #endregion


        #region (protected internal) OnAuthorizeRemoteStartHTTPRequest

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteStart HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnAuthorizeRemoteStartHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteStart HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logAuthorizeRemoteStartHTTPRequest(DateTime     Timestamp,
                                                                   HTTPAPI      API,
                                                                   HTTPRequest  Request)

            => OnAuthorizeRemoteStartHTTPRequest.WhenAll(Timestamp,
                                                         API ?? this,
                                                         Request);

        #endregion

        #region (event)              OnAuthorizeRemoteStart(Request-/Response)

        /// <summary>
        /// An event send whenever a AuthorizeRemoteStart request was received.
        /// </summary>
        public event OnAuthorizeRemoteStartAPIRequestDelegate?   OnAuthorizeRemoteStartRequest;

        /// <summary>
        /// An event send whenever a AuthorizeRemoteStart request was received.
        /// </summary>
        public event OnAuthorizeRemoteStartAPIDelegate?          OnAuthorizeRemoteStart;

        /// <summary>
        /// An event send whenever a response to a AuthorizeRemoteStart request was sent.
        /// </summary>
        public event OnAuthorizeRemoteStartAPIResponseDelegate?  OnAuthorizeRemoteStartResponse;

        #endregion

        #region (protected internal) OnAuthorizeRemoteStartHTTPResponse

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteStart HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnAuthorizeRemoteStartHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteStart HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logAuthorizeRemoteStartHTTPResponse(DateTime      Timestamp,
                                                                    HTTPAPI       API,
                                                                    HTTPRequest   Request,
                                                                    HTTPResponse  Response)

            => OnAuthorizeRemoteStartHTTPResponse.WhenAll(Timestamp,
                                                          API ?? this,
                                                          Request,
                                                          Response);

        #endregion


        #region (protected internal) OnAuthorizeRemoteStopHTTPRequest

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteStop HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnAuthorizeRemoteStopHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteStop HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logAuthorizeRemoteStopHTTPRequest(DateTime     Timestamp,
                                                                  HTTPAPI      API,
                                                                  HTTPRequest  Request)

            => OnAuthorizeRemoteStopHTTPRequest.WhenAll(Timestamp,
                                                        API ?? this,
                                                        Request);

        #endregion

        #region (event)              OnAuthorizeRemoteStop(Request-/Response)

        /// <summary>
        /// An event send whenever a AuthorizeRemoteStop request was received.
        /// </summary>
        public event OnAuthorizeRemoteStopAPIRequestDelegate?   OnAuthorizeRemoteStopRequest;

        /// <summary>
        /// An event send whenever a AuthorizeRemoteStop request was received.
        /// </summary>
        public event OnAuthorizeRemoteStopAPIDelegate?          OnAuthorizeRemoteStop;

        /// <summary>
        /// An event send whenever a response to a AuthorizeRemoteStop request was sent.
        /// </summary>
        public event OnAuthorizeRemoteStopAPIResponseDelegate?  OnAuthorizeRemoteStopResponse;

        #endregion

        #region (protected internal) OnAuthorizeRemoteStopHTTPResponse

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteStop HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnAuthorizeRemoteStopHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an AuthorizeRemoteStop HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logAuthorizeRemoteStopHTTPResponse(DateTime      Timestamp,
                                                                   HTTPAPI       API,
                                                                   HTTPRequest   Request,
                                                                   HTTPResponse  Response)

            => OnAuthorizeRemoteStopHTTPResponse.WhenAll(Timestamp,
                                                         API ?? this,
                                                         Request,
                                                         Response);

        #endregion



        #region (protected internal) OnGetChargeDetailRecordsHTTPRequest

        /// <summary>
        /// An event sent whenever an GetChargeDetailRecords HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnGetChargeDetailRecordsHTTPRequest = new();

        /// <summary>
        /// An event sent whenever an GetChargeDetailRecords HTTP request was received.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        protected internal Task logGetChargeDetailRecordsHTTPRequest(DateTime     Timestamp,
                                                                     HTTPAPI      API,
                                                                     HTTPRequest  Request)

            => OnGetChargeDetailRecordsHTTPRequest.WhenAll(Timestamp,
                                                           API ?? this,
                                                           Request);

        #endregion

        #region (event)              OnGetChargeDetailRecords(Request-/Response)

        /// <summary>
        /// An event send whenever a GetChargeDetailRecords request was received.
        /// </summary>
        public event OnGetChargeDetailRecordsAPIRequestDelegate?   OnGetChargeDetailRecordsRequest;

        /// <summary>
        /// An event send whenever a GetChargeDetailRecords request was received.
        /// </summary>
        public event OnGetChargeDetailRecordsAPIDelegate?          OnGetChargeDetailRecords;

        /// <summary>
        /// An event send whenever a response to a GetChargeDetailRecords request was sent.
        /// </summary>
        public event OnGetChargeDetailRecordsAPIResponseDelegate?  OnGetChargeDetailRecordsResponse;

        #endregion

        #region (protected internal) OnGetChargeDetailRecordsHTTPResponse

        /// <summary>
        /// An event sent whenever an GetChargeDetailRecords HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnGetChargeDetailRecordsHTTPResponse = new();

        /// <summary>
        /// An event sent whenever an GetChargeDetailRecords HTTP response was sent.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="API">The EMP Client HTTP API.</param>
        /// <param name="Request">The HTTP request.</param>
        /// <param name="Response">The HTTP response.</param>
        protected internal Task logGetChargeDetailRecordsHTTPResponse(DateTime      Timestamp,
                                                                      HTTPAPI       API,
                                                                      HTTPRequest   Request,
                                                                      HTTPResponse  Response)

            => OnGetChargeDetailRecordsHTTPResponse.WhenAll(Timestamp,
                                                            API ?? this,
                                                            Request,
                                                            Response);

        #endregion

        #endregion

        #region Constructor(s)

        #region EMPClientAPI(HTTPAPI, ...)

        public EMPClientAPI(HTTPAPI  HTTPAPI,
                            String   LoggingPath      = DefaultHTTPAPI_LoggingPath,
                            String   LoggingContext   = DefaultLoggingContext,
                            String   LogfileName      = DefaultHTTPAPI_LogfileName)

            : base(HTTPAPI)

        {

            this.Counters    = new APICounters();

            this.HTTPLogger  = DisableLogging == false
                                   ? new Logger(this,
                                                LoggingPath,
                                                LoggingContext ?? DefaultLoggingContext,
                                                LogfileCreator)
                                   : null;

            RegisterURLTemplates(false);

        }

        #endregion

        #region EMPClientAPI(HTTPHostname, ...)

        /// <summary>
        /// Create a new EMP HTTP Client API.
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
        public EMPClientAPI(HTTPHostname?                         HTTPHostname                       = null,
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

            this.Counters    = new APICounters();

            this.HTTPLogger  = DisableLogging == false
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

            #region / (HTTPRoot)

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
                                                         Content         = "This is an OICP v2.3 EMP Client HTTP/JSON endpoint!".ToUTF8Bytes(),
                                                         CacheControl    = "public, max-age=300",
                                                         Connection      = "close"
                                                     }.AsImmutable);
                                             },
                                             AllowReplacement: URLReplacement.Allow);

            #endregion


            #region POST  ~/api/oicp/evsepull/v23/providers/{providerId}/data-records

            // -----------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/evsepull/v23/providers/DE-GDF/data-records
            // -----------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/evsepull/v23/providers/{providerId}/data-records",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logPullEVSEDataHTTPRequest,
                                         HTTPResponseLogger:  logPullEVSEDataHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime  = Timestamp.Now;
                                             var page       = Request.QueryString.GetUInt32 ("page");
                                             var size       = Request.QueryString.GetUInt32 ("size");
                                             var sortOrder  = Request.QueryString.GetStrings("sortOrder");
                                             var processId  = Process_Id.NewRandom;

                                             OICPResult<PullEVSEDataResponse>? pullEVSEDataResponse = null;

                                             try
                                             {

                                                 #region Try to parse ProviderId URL parameter

                                                 if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                                     pullEVSEDataResponse = OICPResult<PullEVSEDataResponse>.Failed(
                                                                                this,
                                                                                new PullEVSEDataResponse(
                                                                                    Timestamp.Now,
                                                                                    Request.EventTrackingId,
                                                                                    processId,
                                                                                    Timestamp.Now - Request.Timestamp,
                                                                                    Array.Empty<EVSEDataRecord>(),
                                                                                    StatusCode: new StatusCode(
                                                                                                    StatusCodes.SystemError,
                                                                                                    "The expected 'providerId' URL parameter could not be parsed!"
                                                                                                )
                                                                                )
                                                                            );

                                                 #endregion

                                                 else if (PullEVSEDataRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                       //     providerId ????
                                                                                       out PullEVSEDataRequest?          pullEVSEDataRequest,
                                                                                       out String?                       errorResponse,
                                                                                       ProcessId:                        processId,
                                                                                       Page:                             page,
                                                                                       Size:                             size,
                                                                                       SortOrder:                        sortOrder,

                                                                                       Timestamp:                        Request.Timestamp,
                                                                                       CancellationToken:                Request.CancellationToken,
                                                                                       EventTrackingId:                  Request.EventTrackingId,
                                                                                       RequestTimeout:                   Request.Timeout ?? DefaultRequestTimeout,

                                                                                       CustomPullEVSEDataRequestParser:  CustomPullEVSEDataRequestParser))
                                                 {

                                                     Counters.PullEVSEData.IncRequests_OK();

                                                     #region Send OnPullEVSEDataRequest event

                                                     try
                                                     {

                                                         if (OnPullEVSEDataRequest is not null)
                                                             await Task.WhenAll(OnPullEVSEDataRequest.GetInvocationList().
                                                                                Cast<OnPullEVSEDataAPIRequestDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              pullEVSEDataRequest!))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPClientAPI) + "." + nameof(OnPullEVSEDataRequest));
                                                     }

                                                     #endregion

                                                     #region Call async subscribers

                                                     var OnPullEVSEDataLocal = OnPullEVSEData;
                                                     if (OnPullEVSEDataLocal is not null)
                                                     {
                                                         try
                                                         {

                                                             pullEVSEDataResponse = (await Task.WhenAll(OnPullEVSEDataLocal.GetInvocationList().
                                                                                                                            Cast<OnPullEVSEDataAPIDelegate>().
                                                                                                                            Select(e => e(Timestamp.Now,
                                                                                                                                          this,
                                                                                                                                          pullEVSEDataRequest!))))?.FirstOrDefault();

                                                             Counters.PullEVSEData.IncResponses_OK();

                                                         }
                                                         catch (Exception e)
                                                         {
                                                             pullEVSEDataResponse = OICPResult<PullEVSEDataResponse>.Failed(
                                                                                        this,
                                                                                        new PullEVSEDataResponse(
                                                                                            Timestamp.Now,
                                                                                            Request.EventTrackingId,
                                                                                            processId,
                                                                                            Timestamp.Now - Request.Timestamp,
                                                                                            Array.Empty<EVSEDataRecord>(),
                                                                                            pullEVSEDataRequest,
                                                                                            StatusCode: new StatusCode(
                                                                                                            StatusCodes.DataError,
                                                                                                            e.Message,
                                                                                                            e.StackTrace
                                                                                                        )
                                                                                        )
                                                                                    );
                                                         }
                                                     }

                                                     pullEVSEDataResponse ??= OICPResult<PullEVSEDataResponse>.Failed(
                                                                                  this,
                                                                                  new PullEVSEDataResponse(
                                                                                      Timestamp.Now,
                                                                                      Request.EventTrackingId,
                                                                                      processId,
                                                                                      Timestamp.Now - Request.Timestamp,
                                                                                      Array.Empty<EVSEDataRecord>(),
                                                                                      pullEVSEDataRequest,
                                                                                      StatusCode: new StatusCode(
                                                                                                      StatusCodes.SystemError,
                                                                                                      "Could not process the received PullEVSEData request!"
                                                                                                  )
                                                                                  )
                                                                              );

                                                     #endregion

                                                     #region Send OnPullEVSEDataResponse event

                                                     try
                                                     {

                                                         if (OnPullEVSEDataResponse is not null)
                                                             await Task.WhenAll(OnPullEVSEDataResponse.GetInvocationList().
                                                                                Cast<OnPullEVSEDataAPIResponseDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              pullEVSEDataResponse,
                                                                                              Timestamp.Now - startTime))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPClientAPI) + "." + nameof(OnPullEVSEDataResponse));
                                                     }

                                                     #endregion

                                                 }
                                                 else
                                                     pullEVSEDataResponse = OICPResult<PullEVSEDataResponse>.Failed(
                                                                                this,
                                                                                new PullEVSEDataResponse(
                                                                                    Timestamp.Now,
                                                                                    Request.EventTrackingId,
                                                                                    processId,
                                                                                    Timestamp.Now - Request.Timestamp,
                                                                                    Array.Empty<EVSEDataRecord>(),
                                                                                    pullEVSEDataRequest,
                                                                                    StatusCode: new StatusCode(
                                                                                                    StatusCodes.DataError,
                                                                                                    "We could not parse the given PullEVSEData request!",
                                                                                                    errorResponse
                                                                                                )
                                                                                )
                                                                            );

                                             }
                                             catch (Exception e)
                                             {
                                                 pullEVSEDataResponse = OICPResult<PullEVSEDataResponse>.Failed(
                                                                            this,
                                                                            new PullEVSEDataResponse(
                                                                                Timestamp.Now,
                                                                                Request.EventTrackingId,
                                                                                processId,
                                                                                Timestamp.Now - Request.Timestamp,
                                                                                Array.Empty<EVSEDataRecord>(),
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
                                                        Content                    = pullEVSEDataResponse.Response?.ToJSON(CustomPullEVSEDataResponseSerializer,
                                                                                                                           CustomEVSEDataRecordSerializer,
                                                                                                                           CustomAddressSerializer,
                                                                                                                           CustomChargingFacilitySerializer,
                                                                                                                           CustomGeoCoordinatesSerializer,
                                                                                                                           CustomEnergySourceSerializer,
                                                                                                                           CustomEnvironmentalImpactSerializer,
                                                                                                                           CustomOpeningTimesSerializer,
                                                                                                                           CustomStatusCodeSerializer).
                                                                                                                    ToString(JSONFormatting).
                                                                                                                    ToUTF8Bytes()
                                                                                                          ?? Array.Empty<Byte>(),
                                                        ProcessID                  = processId.ToString(),
                                                        Connection                 = "close"
                                                    }.AsImmutable;

                                          }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region POST  ~/api/oicp/evsepull/v21/providers/{providerId}/status-records

            // -------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/evsepull/v21/providers/DE-GDF/status-records
            // -------------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/evsepull/v21/providers/{providerId}/status-records",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logPullEVSEStatusHTTPRequest,
                                         HTTPResponseLogger:  logPullEVSEStatusHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime  = Timestamp.Now;
                                             var processId  = Process_Id.NewRandom;

                                             OICPResult<PullEVSEStatusResponse>? pullEVSEStatusResponse = null;

                                             try
                                             {

                                                 #region Try to parse ProviderId URL parameter

                                                 if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                                     pullEVSEStatusResponse = OICPResult<PullEVSEStatusResponse>.Failed(
                                                                                  this,
                                                                                  new PullEVSEStatusResponse(
                                                                                      Timestamp.Now,
                                                                                      Request.EventTrackingId,
                                                                                      processId,
                                                                                      Timestamp.Now - Request.Timestamp,
                                                                                      Array.Empty<OperatorEVSEStatus>(),
                                                                                      StatusCode: new StatusCode(
                                                                                                      StatusCodes.SystemError,
                                                                                                      "The expected 'providerId' URL parameter could not be parsed!"
                                                                                                  )
                                                                                  )
                                                                              );

                                                 #endregion

                                                 else if (PullEVSEStatusRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                         //     providerId ????
                                                                                         out PullEVSEStatusRequest?          pullEVSEStatusRequest,
                                                                                         out String?                         errorResponse,
                                                                                         ProcessId:                          processId,

                                                                                         Timestamp:                          Request.Timestamp,
                                                                                         CancellationToken:                  Request.CancellationToken,
                                                                                         EventTrackingId:                    Request.EventTrackingId,
                                                                                         RequestTimeout:                     Request.Timeout ?? DefaultRequestTimeout,

                                                                                         CustomPullEVSEStatusRequestParser:  CustomPullEVSEStatusRequestParser))
                                                 {

                                                     Counters.PullEVSEStatus.IncRequests_OK();

                                                     #region Send OnPullEVSEStatusRequest event

                                                     try
                                                     {

                                                         if (OnPullEVSEStatusRequest is not null)
                                                             await Task.WhenAll(OnPullEVSEStatusRequest.GetInvocationList().
                                                                                Cast<OnPullEVSEStatusAPIRequestDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              pullEVSEStatusRequest!))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPClientAPI) + "." + nameof(OnPullEVSEStatusRequest));
                                                     }

                                                     #endregion

                                                     #region Call async subscribers

                                                     var OnPullEVSEStatusLocal = OnPullEVSEStatus;
                                                     if (OnPullEVSEStatusLocal is not null)
                                                     {
                                                         try
                                                         {

                                                             pullEVSEStatusResponse = (await Task.WhenAll(OnPullEVSEStatusLocal.GetInvocationList().
                                                                                                                                Cast<OnPullEVSEStatusAPIDelegate>().
                                                                                                                                Select(e => e(Timestamp.Now,
                                                                                                                                              this,
                                                                                                                                              pullEVSEStatusRequest!))))?.FirstOrDefault();

                                                             Counters.PullEVSEStatus.IncResponses_OK();

                                                         }
                                                         catch (Exception e)
                                                         {
                                                             pullEVSEStatusResponse = OICPResult<PullEVSEStatusResponse>.Failed(
                                                                                        this,
                                                                                        new PullEVSEStatusResponse(
                                                                                            Timestamp.Now,
                                                                                            Request.EventTrackingId,
                                                                                            processId,
                                                                                            Timestamp.Now - Request.Timestamp,
                                                                                            Array.Empty<OperatorEVSEStatus>(),
                                                                                            pullEVSEStatusRequest,
                                                                                            StatusCode: new StatusCode(
                                                                                                            StatusCodes.DataError,
                                                                                                            e.Message,
                                                                                                            e.StackTrace
                                                                                                        )
                                                                                        )
                                                                                    );
                                                         }
                                                     }

                                                     pullEVSEStatusResponse ??= OICPResult<PullEVSEStatusResponse>.Failed(
                                                                                    this,
                                                                                    new PullEVSEStatusResponse(
                                                                                        Timestamp.Now,
                                                                                        Request.EventTrackingId,
                                                                                        processId,
                                                                                        Timestamp.Now - Request.Timestamp,
                                                                                        Array.Empty<OperatorEVSEStatus>(),
                                                                                        pullEVSEStatusRequest,
                                                                                        StatusCode: new StatusCode(
                                                                                                        StatusCodes.SystemError,
                                                                                                        "Could not process the received PullEVSEStatus request!"
                                                                                                    )
                                                                                    )
                                                                                );

                                                     #endregion

                                                     #region Send OnPullEVSEStatusResponse event

                                                     try
                                                     {

                                                         if (OnPullEVSEStatusResponse is not null)
                                                             await Task.WhenAll(OnPullEVSEStatusResponse.GetInvocationList().
                                                                                Cast<OnPullEVSEStatusAPIResponseDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              pullEVSEStatusResponse,
                                                                                              Timestamp.Now - startTime))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPClientAPI) + "." + nameof(OnPullEVSEStatusResponse));
                                                     }

                                                     #endregion

                                                 }
                                                 else
                                                     pullEVSEStatusResponse = OICPResult<PullEVSEStatusResponse>.Failed(
                                                                                this,
                                                                                new PullEVSEStatusResponse(
                                                                                    Timestamp.Now,
                                                                                    Request.EventTrackingId,
                                                                                    processId,
                                                                                    Timestamp.Now - Request.Timestamp,
                                                                                    Array.Empty<OperatorEVSEStatus>(),
                                                                                    pullEVSEStatusRequest,
                                                                                    StatusCode: new StatusCode(
                                                                                                    StatusCodes.DataError,
                                                                                                    "We could not parse the given PullEVSEStatus request!",
                                                                                                    errorResponse
                                                                                                )
                                                                                )
                                                                            );

                                             }
                                             catch (Exception e)
                                             {
                                                 pullEVSEStatusResponse = OICPResult<PullEVSEStatusResponse>.Failed(
                                                                            this,
                                                                            new PullEVSEStatusResponse(
                                                                                Timestamp.Now,
                                                                                Request.EventTrackingId,
                                                                                processId,
                                                                                Timestamp.Now - Request.Timestamp,
                                                                                Array.Empty<OperatorEVSEStatus>(),
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
                                                        Content                    = pullEVSEStatusResponse.Response?.ToJSON(CustomPullEVSEStatusResponseSerializer,
                                                                                                                             CustomOperatorEVSEStatusSerializer,
                                                                                                                             CustomEVSEStatusRecordSerializer,
                                                                                                                             CustomStatusCodeSerializer).
                                                                                                                      ToString(JSONFormatting).
                                                                                                                      ToUTF8Bytes()
                                                                                                            ?? Array.Empty<Byte>(),
                                                        ProcessID                  = processId.ToString(),
                                                        Connection                 = "close"
                                                    }.AsImmutable;

                                          }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region POST  ~/api/oicp/evsepull/v21/providers/{providerId}/status-records-by-id

            // -------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/evsepull/v21/providers/DE-GDF/status-records-by-id
            // -------------------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/evsepull/v21/providers/{providerId}/status-records-by-id",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logPullEVSEStatusByIdHTTPRequest,
                                         HTTPResponseLogger:  logPullEVSEStatusByIdHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime  = Timestamp.Now;
                                             var processId  = Process_Id.NewRandom;

                                             OICPResult<PullEVSEStatusByIdResponse>? pullEVSEStatusByIdResponse = null;

                                             try
                                             {

                                                 #region Try to parse ProviderId URL parameter

                                                 if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                                     pullEVSEStatusByIdResponse = OICPResult<PullEVSEStatusByIdResponse>.Failed(
                                                                                  this,
                                                                                  new PullEVSEStatusByIdResponse(
                                                                                      Timestamp.Now,
                                                                                      Request.EventTrackingId,
                                                                                      processId,
                                                                                      Timestamp.Now - Request.Timestamp,
                                                                                      Array.Empty<EVSEStatusRecord>(),
                                                                                      StatusCode: new StatusCode(
                                                                                                      StatusCodes.SystemError,
                                                                                                      "The expected 'providerId' URL parameter could not be parsed!"
                                                                                                  )
                                                                                  )
                                                                              );

                                                 #endregion

                                                 else if (PullEVSEStatusByIdRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                             //     providerId ????
                                                                                             out PullEVSEStatusByIdRequest?          pullEVSEStatusByIdRequest,
                                                                                             out String?                             errorResponse,
                                                                                             ProcessId:                              processId,

                                                                                             Timestamp:                              Request.Timestamp,
                                                                                             CancellationToken:                      Request.CancellationToken,
                                                                                             EventTrackingId:                        Request.EventTrackingId,
                                                                                             RequestTimeout:                         Request.Timeout ?? DefaultRequestTimeout,

                                                                                             CustomPullEVSEStatusByIdRequestParser:  CustomPullEVSEStatusByIdRequestParser))
                                                 {

                                                     Counters.PullEVSEStatusById.IncRequests_OK();

                                                     #region Send OnPullEVSEStatusByIdRequest event

                                                     try
                                                     {

                                                         if (OnPullEVSEStatusByIdRequest is not null)
                                                             await Task.WhenAll(OnPullEVSEStatusByIdRequest.GetInvocationList().
                                                                                Cast<OnPullEVSEStatusByIdAPIRequestDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              pullEVSEStatusByIdRequest!))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPClientAPI) + "." + nameof(OnPullEVSEStatusByIdRequest));
                                                     }

                                                     #endregion

                                                     #region Call async subscribers

                                                     var OnPullEVSEStatusByIdLocal = OnPullEVSEStatusById;
                                                     if (OnPullEVSEStatusByIdLocal is not null)
                                                     {
                                                         try
                                                         {

                                                             pullEVSEStatusByIdResponse = (await Task.WhenAll(OnPullEVSEStatusByIdLocal.GetInvocationList().
                                                                                                                                        Cast<OnPullEVSEStatusByIdAPIDelegate>().
                                                                                                                                        Select(e => e(Timestamp.Now,
                                                                                                                                                      this,
                                                                                                                                                      pullEVSEStatusByIdRequest!))))?.FirstOrDefault();

                                                             Counters.PullEVSEStatusById.IncResponses_OK();

                                                         }
                                                         catch (Exception e)
                                                         {
                                                             pullEVSEStatusByIdResponse = OICPResult<PullEVSEStatusByIdResponse>.Failed(
                                                                                              this,
                                                                                              new PullEVSEStatusByIdResponse(
                                                                                                  Timestamp.Now,
                                                                                                  Request.EventTrackingId,
                                                                                                  processId,
                                                                                                  Timestamp.Now - Request.Timestamp,
                                                                                                  Array.Empty<EVSEStatusRecord>(),
                                                                                                  pullEVSEStatusByIdRequest,
                                                                                                  StatusCode: new StatusCode(
                                                                                                                  StatusCodes.DataError,
                                                                                                                  e.Message,
                                                                                                                  e.StackTrace
                                                                                                              )
                                                                                              )
                                                                                          );
                                                         }
                                                     }

                                                     pullEVSEStatusByIdResponse = OICPResult<PullEVSEStatusByIdResponse>.Failed(
                                                                                      this,
                                                                                      new PullEVSEStatusByIdResponse(
                                                                                          Timestamp.Now,
                                                                                          Request.EventTrackingId,
                                                                                          processId,
                                                                                          Timestamp.Now - Request.Timestamp,
                                                                                          Array.Empty<EVSEStatusRecord>(),
                                                                                          pullEVSEStatusByIdRequest,
                                                                                          StatusCode: new StatusCode(
                                                                                                          StatusCodes.SystemError,
                                                                                                          "Could not process the received PullEVSEStatusById request!"
                                                                                                      )
                                                                                      )
                                                                                  );

                                                     #endregion

                                                     #region Send OnPullEVSEStatusByIdResponse event

                                                     try
                                                     {

                                                         if (OnPullEVSEStatusByIdResponse is not null)
                                                             await Task.WhenAll(OnPullEVSEStatusByIdResponse.GetInvocationList().
                                                                                Cast<OnPullEVSEStatusByIdAPIResponseDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              pullEVSEStatusByIdResponse,
                                                                                              Timestamp.Now - startTime))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPClientAPI) + "." + nameof(OnPullEVSEStatusByIdResponse));
                                                     }

                                                     #endregion

                                                 }
                                                 else
                                                     pullEVSEStatusByIdResponse = OICPResult<PullEVSEStatusByIdResponse>.Failed(
                                                                                      this,
                                                                                      new PullEVSEStatusByIdResponse(
                                                                                          Timestamp.Now,
                                                                                          Request.EventTrackingId,
                                                                                          processId,
                                                                                          Timestamp.Now - Request.Timestamp,
                                                                                          Array.Empty<EVSEStatusRecord>(),
                                                                                          pullEVSEStatusByIdRequest,
                                                                                          StatusCode: new StatusCode(
                                                                                                          StatusCodes.DataError,
                                                                                                          "We could not parse the given PullEVSEStatusById request!",
                                                                                                          errorResponse
                                                                                                      )
                                                                                      )
                                                                                  );

                                             }
                                             catch (Exception e)
                                             {
                                                 pullEVSEStatusByIdResponse = OICPResult<PullEVSEStatusByIdResponse>.Failed(
                                                                                  this,
                                                                                  new PullEVSEStatusByIdResponse(
                                                                                      Timestamp.Now,
                                                                                      Request.EventTrackingId,
                                                                                      processId,
                                                                                      Timestamp.Now - Request.Timestamp,
                                                                                      Array.Empty<EVSEStatusRecord>(),
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
                                                        Content                    = pullEVSEStatusByIdResponse.Response?.ToJSON(CustomPullEVSEStatusByIdResponseSerializer,
                                                                                                                                 CustomEVSEStatusRecordSerializer,
                                                                                                                                 CustomStatusCodeSerializer).
                                                                                                                          ToString(JSONFormatting).
                                                                                                                          ToUTF8Bytes()
                                                                                                                ?? Array.Empty<Byte>(),
                                                        ProcessID                  = processId.ToString(),
                                                        Connection                 = "close"
                                                    }.AsImmutable;

                                          }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region POST  ~/api/oicp/evsepull/v21/providers/{providerId}/status-records-by-operator-id

            // ----------------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/evsepull/v21/providers/DE-GDF/status-records-by-operator-id
            // ----------------------------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/evsepull/v21/providers/{providerId}/status-records-by-operator-id",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logPullEVSEStatusByOperatorIdHTTPRequest,
                                         HTTPResponseLogger:  logPullEVSEStatusByOperatorIdHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime  = Timestamp.Now;
                                             var processId  = Process_Id.NewRandom;

                                             OICPResult<PullEVSEStatusByOperatorIdResponse>? pullEVSEStatusByOperatorIdResponse = null;

                                             try
                                             {

                                                 #region Try to parse ProviderId URL parameter

                                                 if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                                     pullEVSEStatusByOperatorIdResponse = OICPResult<PullEVSEStatusByOperatorIdResponse>.Failed(
                                                                                              this,
                                                                                              new PullEVSEStatusByOperatorIdResponse(
                                                                                                  Timestamp.Now,
                                                                                                  Request.EventTrackingId,
                                                                                                  Process_Id.NewRandom,
                                                                                                  Timestamp.Now - Request.Timestamp,
                                                                                                  Array.Empty<OperatorEVSEStatus>(),
                                                                                                  StatusCode: new StatusCode(
                                                                                                                  StatusCodes.SystemError,
                                                                                                                  "The expected 'providerId' URL parameter could not be parsed!"
                                                                                                              )
                                                                                              )
                                                                                          );

                                                 #endregion

                                                 else if (PullEVSEStatusByOperatorIdRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                                     //     providerId ????
                                                                                                     out PullEVSEStatusByOperatorIdRequest?          pullEVSEStatusByOperatorIdRequest,
                                                                                                     out String?                                     errorResponse,
                                                                                                     ProcessId:                                      processId,

                                                                                                     Timestamp:                                      Request.Timestamp,
                                                                                                     CancellationToken:                              Request.CancellationToken,
                                                                                                     EventTrackingId:                                Request.EventTrackingId,
                                                                                                     RequestTimeout:                                 Request.Timeout ?? DefaultRequestTimeout,

                                                                                                     CustomPullEVSEStatusByOperatorIdRequestParser:  CustomPullEVSEStatusByOperatorIdRequestParser))
                                                 {

                                                     Counters.PullEVSEStatusByOperatorId.IncRequests_OK();

                                                     #region Send OnPullEVSEStatusByOperatorIdRequest event

                                                     try
                                                     {

                                                         if (OnPullEVSEStatusByOperatorIdRequest is not null)
                                                             await Task.WhenAll(OnPullEVSEStatusByOperatorIdRequest.GetInvocationList().
                                                                                Cast<OnPullEVSEStatusByOperatorIdAPIRequestDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              pullEVSEStatusByOperatorIdRequest!))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPClientAPI) + "." + nameof(OnPullEVSEStatusByOperatorIdRequest));
                                                     }

                                                     #endregion

                                                     #region Call async subscribers

                                                     var OnPullEVSEStatusByOperatorIdLocal = OnPullEVSEStatusByOperatorId;
                                                     if (OnPullEVSEStatusByOperatorIdLocal is not null)
                                                     {
                                                         try
                                                         {

                                                             pullEVSEStatusByOperatorIdResponse = (await Task.WhenAll(OnPullEVSEStatusByOperatorIdLocal.GetInvocationList().
                                                                                                                                                        Cast<OnPullEVSEStatusByOperatorIdAPIDelegate>().
                                                                                                                                                        Select(e => e(Timestamp.Now,
                                                                                                                                                                      this,
                                                                                                                                                                      pullEVSEStatusByOperatorIdRequest!))))?.FirstOrDefault();

                                                             Counters.PullEVSEStatusByOperatorId.IncResponses_OK();

                                                         }
                                                         catch (Exception e)
                                                         {
                                                             pullEVSEStatusByOperatorIdResponse = OICPResult<PullEVSEStatusByOperatorIdResponse>.Failed(
                                                                                                      this,
                                                                                                      new PullEVSEStatusByOperatorIdResponse(
                                                                                                          Timestamp.Now,
                                                                                                          Request.EventTrackingId,
                                                                                                          processId,
                                                                                                          Timestamp.Now - Request.Timestamp,
                                                                                                          Array.Empty<OperatorEVSEStatus>(),
                                                                                                          pullEVSEStatusByOperatorIdRequest,
                                                                                                          StatusCode: new StatusCode(
                                                                                                                          StatusCodes.DataError,
                                                                                                                          e.Message,
                                                                                                                          e.StackTrace
                                                                                                                      )
                                                                                                      )
                                                                                                  );
                                                         }
                                                     }

                                                     pullEVSEStatusByOperatorIdResponse ??= OICPResult<PullEVSEStatusByOperatorIdResponse>.Failed(
                                                                                                this,
                                                                                                new PullEVSEStatusByOperatorIdResponse(
                                                                                                    Timestamp.Now,
                                                                                                    Request.EventTrackingId,
                                                                                                    processId,
                                                                                                    Timestamp.Now - Request.Timestamp,
                                                                                                    Array.Empty<OperatorEVSEStatus>(),
                                                                                                    pullEVSEStatusByOperatorIdRequest,
                                                                                                    StatusCode: new StatusCode(
                                                                                                                    StatusCodes.SystemError,
                                                                                                                    "Could not process the received PullEVSEStatusByOperatorId request!"
                                                                                                                )
                                                                                                )
                                                                                            );

                                                     #endregion

                                                     #region Send OnPullEVSEStatusByOperatorIdResponse event

                                                     try
                                                     {

                                                         if (OnPullEVSEStatusByOperatorIdResponse is not null)
                                                             await Task.WhenAll(OnPullEVSEStatusByOperatorIdResponse.GetInvocationList().
                                                                                Cast<OnPullEVSEStatusByOperatorIdAPIResponseDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              pullEVSEStatusByOperatorIdResponse,
                                                                                              Timestamp.Now - startTime))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPClientAPI) + "." + nameof(OnPullEVSEStatusByOperatorIdResponse));
                                                     }

                                                     #endregion

                                                 }
                                                 else
                                                     pullEVSEStatusByOperatorIdResponse = OICPResult<PullEVSEStatusByOperatorIdResponse>.Failed(
                                                                                              this,
                                                                                              new PullEVSEStatusByOperatorIdResponse(
                                                                                                  Timestamp.Now,
                                                                                                  Request.EventTrackingId,
                                                                                                  processId,
                                                                                                  Timestamp.Now - Request.Timestamp,
                                                                                                  Array.Empty<OperatorEVSEStatus>(),
                                                                                                  pullEVSEStatusByOperatorIdRequest,
                                                                                                  StatusCode: new StatusCode(
                                                                                                                  StatusCodes.DataError,
                                                                                                                  "We could not parse the given PullEVSEStatusByOperatorId request!",
                                                                                                                  errorResponse
                                                                                                              )
                                                                                              )
                                                                                          );

                                             }
                                             catch (Exception e)
                                             {
                                                 pullEVSEStatusByOperatorIdResponse = OICPResult<PullEVSEStatusByOperatorIdResponse>.Failed(
                                                                                          this,
                                                                                          new PullEVSEStatusByOperatorIdResponse(
                                                                                              Timestamp.Now,
                                                                                              Request.EventTrackingId,
                                                                                              processId,
                                                                                              Timestamp.Now - Request.Timestamp,
                                                                                              Array.Empty<OperatorEVSEStatus>(),
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
                                                        Content                    = pullEVSEStatusByOperatorIdResponse.Response?.ToJSON(CustomPullEVSEStatusByOperatorIdResponseSerializer,
                                                                                                                                         CustomOperatorEVSEStatusSerializer,
                                                                                                                                         CustomEVSEStatusRecordSerializer,
                                                                                                                                         CustomStatusCodeSerializer).
                                                                                                                                  ToString(JSONFormatting).
                                                                                                                                  ToUTF8Bytes()
                                                                                                                        ?? Array.Empty<Byte>(),
                                                        ProcessID                  = processId.ToString(),
                                                        Connection                 = "close"
                                                    }.AsImmutable;

                                          }, AllowReplacement: URLReplacement.Allow);

            #endregion


            #region POST  ~/api/oicp/dynamicpricing/v10/providers/{providerId}/pricing-products

            // ---------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/dynamicpricing/v10/providers/DE-GDF/pricing-products
            // ---------------------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/dynamicpricing/v10/providers/{providerId}/pricing-products",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logPullPricingProductDataHTTPRequest,
                                         HTTPResponseLogger:  logPullPricingProductDataHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime  = Timestamp.Now;
                                             var page       = Request.QueryString.GetUInt32 ("page");
                                             var size       = Request.QueryString.GetUInt32 ("size");
                                             var sortOrder  = Request.QueryString.GetStrings("sortOrder");
                                             var processId  = Process_Id.NewRandom;

                                             OICPResult<PullPricingProductDataResponse>? pullPricingProductDataResponse = null;

                                             try
                                             {

                                                 #region Try to parse ProviderId URL parameter

                                                 if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                                     pullPricingProductDataResponse = OICPResult<PullPricingProductDataResponse>.Failed(
                                                                                          this,
                                                                                          new PullPricingProductDataResponse(
                                                                                              Timestamp.Now,
                                                                                              Request.EventTrackingId,
                                                                                              processId,
                                                                                              Timestamp.Now - Request.Timestamp,
                                                                                              Array.Empty<PricingProductData>(),
                                                                                              StatusCode: new StatusCode(
                                                                                                              StatusCodes.SystemError,
                                                                                                              "The expected 'providerId' URL parameter could not be parsed!"
                                                                                                          )
                                                                                          )
                                                                                      );

                                                 #endregion

                                                 else if (PullPricingProductDataRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                                 providerId,
                                                                                                 out PullPricingProductDataRequest?          pullEVSEDataRequest,
                                                                                                 out String?                                 errorResponse,
                                                                                                 ProcessId:                                  processId,
                                                                                                 Page:                                       page,
                                                                                                 Size:                                       size,
                                                                                                 SortOrder:                                  sortOrder,

                                                                                                 Timestamp:                                  Request.Timestamp,
                                                                                                 CancellationToken:                          Request.CancellationToken,
                                                                                                 EventTrackingId:                            Request.EventTrackingId,
                                                                                                 RequestTimeout:                             Request.Timeout ?? DefaultRequestTimeout,

                                                                                                 CustomPullPricingProductDataRequestParser:  CustomPullPricingProductDataRequestParser))
                                                 {

                                                     Counters.PullPricingProductData.IncRequests_OK();

                                                     #region Send OnPullPricingProductDataRequest event

                                                     try
                                                     {

                                                         if (OnPullPricingProductDataRequest is not null)
                                                             await Task.WhenAll(OnPullPricingProductDataRequest.GetInvocationList().
                                                                                Cast<OnPullPricingProductDataAPIRequestDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              pullEVSEDataRequest!))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPClientAPI) + "." + nameof(OnPullPricingProductDataRequest));
                                                     }

                                                     #endregion

                                                     #region Call async subscribers

                                                     var OnPullPricingProductDataLocal = OnPullPricingProductData;
                                                     if (OnPullPricingProductDataLocal is not null)
                                                     {
                                                         try
                                                         {

                                                             pullPricingProductDataResponse = (await Task.WhenAll(OnPullPricingProductDataLocal.GetInvocationList().
                                                                                                                                                Cast<OnPullPricingProductDataAPIDelegate>().
                                                                                                                                                Select(e => e(Timestamp.Now,
                                                                                                                                                              this,
                                                                                                                                                              pullEVSEDataRequest!))))?.FirstOrDefault();

                                                             Counters.PullPricingProductData.IncResponses_OK();

                                                         }
                                                         catch (Exception e)
                                                         {
                                                             pullPricingProductDataResponse = OICPResult<PullPricingProductDataResponse>.Failed(
                                                                                                  this,
                                                                                                  new PullPricingProductDataResponse(
                                                                                                      Timestamp.Now,
                                                                                                      Request.EventTrackingId,
                                                                                                      processId,
                                                                                                      Timestamp.Now - Request.Timestamp,
                                                                                                      Array.Empty<PricingProductData>(),
                                                                                                      pullEVSEDataRequest,
                                                                                                      StatusCode: new StatusCode(
                                                                                                                      StatusCodes.DataError,
                                                                                                                      e.Message,
                                                                                                                      e.StackTrace
                                                                                                                  )
                                                                                                  )
                                                                                              );
                                                         }
                                                     }

                                                     pullPricingProductDataResponse ??= OICPResult<PullPricingProductDataResponse>.Failed(
                                                                                            this,
                                                                                            new PullPricingProductDataResponse(
                                                                                                Timestamp.Now,
                                                                                                Request.EventTrackingId,
                                                                                                processId,
                                                                                                Timestamp.Now - Request.Timestamp,
                                                                                                Array.Empty<PricingProductData>(),
                                                                                                pullEVSEDataRequest,
                                                                                                StatusCode: new StatusCode(
                                                                                                                StatusCodes.SystemError,
                                                                                                                "Could not process the received PullPricingProductData request!"
                                                                                                            )
                                                                                            )
                                                                                        );

                                                     #endregion

                                                     #region Send OnPullPricingProductDataResponse event

                                                     try
                                                     {

                                                         if (OnPullPricingProductDataResponse is not null)
                                                             await Task.WhenAll(OnPullPricingProductDataResponse.GetInvocationList().
                                                                                Cast<OnPullPricingProductDataAPIResponseDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              pullPricingProductDataResponse,
                                                                                              Timestamp.Now - startTime))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPClientAPI) + "." + nameof(OnPullPricingProductDataResponse));
                                                     }

                                                     #endregion

                                                 }
                                                 else
                                                     pullPricingProductDataResponse = OICPResult<PullPricingProductDataResponse>.Failed(
                                                                                          this,
                                                                                          new PullPricingProductDataResponse(
                                                                                              Timestamp.Now,
                                                                                              Request.EventTrackingId,
                                                                                              processId,
                                                                                              Timestamp.Now - Request.Timestamp,
                                                                                              Array.Empty<PricingProductData>(),
                                                                                              pullEVSEDataRequest,
                                                                                              StatusCode: new StatusCode(
                                                                                                              StatusCodes.DataError,
                                                                                                              "We could not parse the given PullPricingProductData request!",
                                                                                                              errorResponse
                                                                                                          )
                                                                                          )
                                                                                      );

                                             }
                                             catch (Exception e)
                                             {
                                                 pullPricingProductDataResponse = OICPResult<PullPricingProductDataResponse>.Failed(
                                                                                      this,
                                                                                      new PullPricingProductDataResponse(
                                                                                          Timestamp.Now,
                                                                                          Request.EventTrackingId,
                                                                                          processId,
                                                                                          Timestamp.Now - Request.Timestamp,
                                                                                          Array.Empty<PricingProductData>(),
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
                                                        Content                    = pullPricingProductDataResponse.Response?.ToJSON(CustomPullPricingProductDataResponseSerializer,
                                                                                                                                     CustomPricingProductDataSerializer,
                                                                                                                                     CustomPricingProductDataRecordSerializer,
                                                                                                                                     CustomStatusCodeSerializer).
                                                                                                                              ToString(JSONFormatting).
                                                                                                                              ToUTF8Bytes()
                                                                                                                    ?? Array.Empty<Byte>(),
                                                        ProcessID                  = processId.ToString(),
                                                        Connection                 = "close"
                                                    }.AsImmutable;

                                          }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region POST  ~/api/oicp/dynamicpricing/v10/providers/{providerId}/evse-pricing

            // -----------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/dynamicpricing/v10/providers/DE-GDF/evse-pricing
            // -----------------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/dynamicpricing/v10/providers/{providerId}/evse-pricing",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logPullEVSEPricingHTTPRequest,
                                         HTTPResponseLogger:  logPullEVSEPricingHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime  = Timestamp.Now;
                                             var page       = Request.QueryString.GetUInt32 ("page");
                                             var size       = Request.QueryString.GetUInt32 ("size");
                                             var sortOrder  = Request.QueryString.GetStrings("sortOrder");
                                             var processId  = Process_Id.NewRandom;

                                             OICPResult<PullEVSEPricingResponse>? pullEVSEPricingResponse = null;

                                             try
                                             {

                                                 #region Try to parse ProviderId URL parameter

                                                 if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                                     pullEVSEPricingResponse = OICPResult<PullEVSEPricingResponse>.Failed(
                                                                                   this,
                                                                                   new PullEVSEPricingResponse(
                                                                                       Timestamp.Now,
                                                                                       Request.EventTrackingId,
                                                                                       processId,
                                                                                       Timestamp.Now - Request.Timestamp,
                                                                                       Array.Empty<OperatorEVSEPricing>(),
                                                                                       StatusCode: new StatusCode(
                                                                                                       StatusCodes.SystemError,
                                                                                                       "The expected 'providerId' URL parameter could not be parsed!"
                                                                                                   )
                                                                                   )
                                                                               );

                                                 #endregion

                                                 else if (PullEVSEPricingRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                          //     providerId ????
                                                                                          out PullEVSEPricingRequest?          pullEVSEDataRequest,
                                                                                          out String?                          errorResponse,
                                                                                          ProcessId:                           processId,
                                                                                          Page:                                page,
                                                                                          Size:                                size,
                                                                                          SortOrder:                           sortOrder,

                                                                                          Timestamp:                           Request.Timestamp,
                                                                                          CancellationToken:                   Request.CancellationToken,
                                                                                          EventTrackingId:                     Request.EventTrackingId,
                                                                                          RequestTimeout:                      Request.Timeout ?? DefaultRequestTimeout,

                                                                                          CustomPullEVSEPricingRequestParser:  CustomPullEVSEPricingRequestParser))
                                                 {

                                                     Counters.PullEVSEPricing.IncRequests_OK();

                                                     #region Send OnPullEVSEPricingRequest event

                                                     try
                                                     {

                                                         if (OnPullEVSEPricingRequest is not null)
                                                             await Task.WhenAll(OnPullEVSEPricingRequest.GetInvocationList().
                                                                                Cast<OnPullEVSEPricingAPIRequestDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              pullEVSEDataRequest!))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPClientAPI) + "." + nameof(OnPullEVSEPricingRequest));
                                                     }

                                                     #endregion

                                                     #region Call async subscribers

                                                     var OnPullEVSEPricingLocal = OnPullEVSEPricing;
                                                     if (OnPullEVSEPricingLocal is not null)
                                                     {
                                                         try
                                                         {

                                                             pullEVSEPricingResponse = (await Task.WhenAll(OnPullEVSEPricingLocal.GetInvocationList().
                                                                                                                                  Cast<OnPullEVSEPricingAPIDelegate>().
                                                                                                                                  Select(e => e(Timestamp.Now,
                                                                                                                                                this,
                                                                                                                                                pullEVSEDataRequest!))))?.FirstOrDefault();

                                                             Counters.PullEVSEPricing.IncResponses_OK();

                                                         }
                                                         catch (Exception e)
                                                         {
                                                             pullEVSEPricingResponse = OICPResult<PullEVSEPricingResponse>.Failed(
                                                                                           this,
                                                                                           new PullEVSEPricingResponse(
                                                                                               Timestamp.Now,
                                                                                               Request.EventTrackingId,
                                                                                               processId,
                                                                                               Timestamp.Now - Request.Timestamp,
                                                                                               Array.Empty<OperatorEVSEPricing>(),
                                                                                               pullEVSEDataRequest,
                                                                                               StatusCode: new StatusCode(
                                                                                                               StatusCodes.DataError,
                                                                                                               e.Message,
                                                                                                               e.StackTrace
                                                                                                           )
                                                                                           )
                                                                                       );
                                                         }
                                                     }

                                                     pullEVSEPricingResponse ??= OICPResult<PullEVSEPricingResponse>.Failed(
                                                                                     this,
                                                                                     new PullEVSEPricingResponse(
                                                                                         Timestamp.Now,
                                                                                         Request.EventTrackingId,
                                                                                         processId,
                                                                                         Timestamp.Now - Request.Timestamp,
                                                                                         Array.Empty<OperatorEVSEPricing>(),
                                                                                         pullEVSEDataRequest,
                                                                                         StatusCode: new StatusCode(
                                                                                                         StatusCodes.SystemError,
                                                                                                         "Could not process the received PullEVSEPricing request!"
                                                                                                     )
                                                                                     )
                                                                                 );

                                                     #endregion

                                                     #region Send OnPullEVSEPricingResponse event

                                                     try
                                                     {

                                                         if (OnPullEVSEPricingResponse is not null)
                                                             await Task.WhenAll(OnPullEVSEPricingResponse.GetInvocationList().
                                                                                Cast<OnPullEVSEPricingAPIResponseDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              pullEVSEPricingResponse,
                                                                                              Timestamp.Now - startTime))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPClientAPI) + "." + nameof(OnPullEVSEPricingResponse));
                                                     }

                                                     #endregion

                                                 }
                                                 else
                                                     pullEVSEPricingResponse = OICPResult<PullEVSEPricingResponse>.Failed(
                                                                                   this,
                                                                                   new PullEVSEPricingResponse(
                                                                                       Timestamp.Now,
                                                                                       Request.EventTrackingId,
                                                                                       processId,
                                                                                       Timestamp.Now - Request.Timestamp,
                                                                                       Array.Empty<OperatorEVSEPricing>(),
                                                                                       pullEVSEDataRequest,
                                                                                       StatusCode: new StatusCode(
                                                                                                       StatusCodes.DataError,
                                                                                                       "We could not parse the given PullEVSEPricing request!",
                                                                                                       errorResponse
                                                                                                   )
                                                                                   )
                                                                               );

                                             }
                                             catch (Exception e)
                                             {
                                                 pullEVSEPricingResponse = OICPResult<PullEVSEPricingResponse>.Failed(
                                                                               this,
                                                                               new PullEVSEPricingResponse(
                                                                                   Timestamp.Now,
                                                                                   Request.EventTrackingId,
                                                                                   processId,
                                                                                   Timestamp.Now - Request.Timestamp,
                                                                                   Array.Empty<OperatorEVSEPricing>(),
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
                                                        Content                    = pullEVSEPricingResponse.Response?.ToJSON(CustomPullEVSEPricingResponseSerializer,
                                                                                                                              CustomOperatorEVSEPricingSerializer,
                                                                                                                              CustomEVSEPricingSerializer,
                                                                                                                              CustomStatusCodeSerializer).
                                                                                                                       ToString(JSONFormatting).
                                                                                                                       ToUTF8Bytes()
                                                                                                             ?? Array.Empty<Byte>(),
                                                        ProcessID                  = processId.ToString(),
                                                        Connection                 = "close"
                                                    }.AsImmutable;

                                          }, AllowReplacement: URLReplacement.Allow);

            #endregion


            #region POST  ~/api/oicp/authdata/v21/providers/{providerId}/push-request

            // -----------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/evsepull/v23/providers/DE-GDF/push-request
            // -----------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/authdata/v21/providers/{providerId}/push-request",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logPushAuthenticationDataHTTPRequest,
                                         HTTPResponseLogger:  logPushAuthenticationDataHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime  = Timestamp.Now;
                                             var processId  = Process_Id.NewRandom;

                                             OICPResult<Acknowledgement<PushAuthenticationDataRequest>>? pullEVSEDataResponse = null;

                                             try
                                             {

                                                 #region Try to parse ProviderId URL parameter

                                                 if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                                     pullEVSEDataResponse = OICPResult<Acknowledgement<PushAuthenticationDataRequest>>.Failed(
                                                                                this,
                                                                                new Acknowledgement<PushAuthenticationDataRequest>(
                                                                                    Timestamp.Now,
                                                                                    Request.EventTrackingId,
                                                                                    processId,
                                                                                    Timestamp.Now - Request.Timestamp,
                                                                                    StatusCode: new StatusCode(
                                                                                                    StatusCodes.SystemError,
                                                                                                    "The expected 'providerId' URL parameter could not be parsed!"
                                                                                                )
                                                                                )
                                                                            );

                                                 #endregion

                                                 else if (PushAuthenticationDataRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                                 //     providerId ????
                                                                                                 out PushAuthenticationDataRequest?          pullEVSEDataRequest,
                                                                                                 out String?                                 errorResponse,

                                                                                                 Timestamp:                                  Request.Timestamp,
                                                                                                 CancellationToken:                          Request.CancellationToken,
                                                                                                 EventTrackingId:                            Request.EventTrackingId,
                                                                                                 RequestTimeout:                             Request.Timeout ?? DefaultRequestTimeout,

                                                                                                 CustomPushAuthenticationDataRequestParser:  CustomPushAuthenticationDataRequestParser))
                                                 {

                                                     Counters.PushAuthenticationData.IncRequests_OK();

                                                     #region Send OnPushAuthenticationDataRequest event

                                                     try
                                                     {

                                                         if (OnPushAuthenticationDataRequest is not null)
                                                             await Task.WhenAll(OnPushAuthenticationDataRequest.GetInvocationList().
                                                                                Cast<OnPushAuthenticationDataAPIRequestDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              pullEVSEDataRequest!))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPClientAPI) + "." + nameof(OnPushAuthenticationDataRequest));
                                                     }

                                                     #endregion

                                                     #region Call async subscribers

                                                     var OnPushAuthenticationDataLocal = OnPushAuthenticationData;
                                                     if (OnPushAuthenticationDataLocal is not null)
                                                     {
                                                         try
                                                         {

                                                             pullEVSEDataResponse = (await Task.WhenAll(OnPushAuthenticationDataLocal.GetInvocationList().
                                                                                                                                      Cast<OnPushAuthenticationDataAPIDelegate>().
                                                                                                                                      Select(e => e(Timestamp.Now,
                                                                                                                                                    this,
                                                                                                                                                    pullEVSEDataRequest!))))?.FirstOrDefault();

                                                             Counters.PushAuthenticationData.IncResponses_OK();

                                                         }
                                                         catch (Exception e)
                                                         {
                                                             pullEVSEDataResponse = OICPResult<Acknowledgement<PushAuthenticationDataRequest>>.Failed(
                                                                                        this,
                                                                                        new Acknowledgement<PushAuthenticationDataRequest>(
                                                                                            Timestamp.Now,
                                                                                            Request.EventTrackingId,
                                                                                            processId,
                                                                                            Timestamp.Now - Request.Timestamp,
                                                                                            StatusCode: new StatusCode(
                                                                                                            StatusCodes.DataError,
                                                                                                            e.Message,
                                                                                                            e.StackTrace
                                                                                                        )
                                                                                        )
                                                                                    );
                                                         }
                                                     }

                                                     pullEVSEDataResponse ??= OICPResult<Acknowledgement<PushAuthenticationDataRequest>>.Failed(
                                                                                  this,
                                                                                  new Acknowledgement<PushAuthenticationDataRequest>(
                                                                                      Timestamp.Now,
                                                                                      Request.EventTrackingId,
                                                                                      processId,
                                                                                      Timestamp.Now - Request.Timestamp,
                                                                                      StatusCode: new StatusCode(
                                                                                                      StatusCodes.SystemError,
                                                                                                      "Could not process the received PushAuthenticationData request!"
                                                                                                  )
                                                                                  )
                                                                              );

                                                     #endregion

                                                     #region Send OnPushAuthenticationDataResponse event

                                                     try
                                                     {

                                                         if (OnPushAuthenticationDataResponse is not null)
                                                             await Task.WhenAll(OnPushAuthenticationDataResponse.GetInvocationList().
                                                                                Cast<OnPushAuthenticationDataAPIResponseDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              pullEVSEDataResponse,
                                                                                              Timestamp.Now - startTime))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPClientAPI) + "." + nameof(OnPushAuthenticationDataResponse));
                                                     }

                                                     #endregion

                                                 }
                                                 else
                                                     pullEVSEDataResponse = OICPResult<Acknowledgement<PushAuthenticationDataRequest>>.Failed(
                                                                                this,
                                                                                new Acknowledgement<PushAuthenticationDataRequest>(
                                                                                    Timestamp.Now,
                                                                                    Request.EventTrackingId,
                                                                                    processId,
                                                                                    Timestamp.Now - Request.Timestamp,
                                                                                    StatusCode: new StatusCode(
                                                                                                    StatusCodes.DataError,
                                                                                                    "We could not parse the given PushAuthenticationData request!",
                                                                                                    errorResponse
                                                                                                )
                                                                                )
                                                                            );

                                             }
                                             catch (Exception e)
                                             {
                                                 pullEVSEDataResponse = OICPResult<Acknowledgement<PushAuthenticationDataRequest>>.Failed(
                                                                            this,
                                                                            new Acknowledgement<PushAuthenticationDataRequest>(
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


            #region POST  ~/api/oicp/charging/v21/providers/{providerId}/authorize-remote-reservation/start

            // ---------------------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/charging/v21/providers/DE-GDF/authorize-remote-reservation/start
            // ---------------------------------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/charging/v21/providers/{providerId}/authorize-remote-reservation/start",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logAuthorizeRemoteReservationStartHTTPRequest,
                                         HTTPResponseLogger:  logAuthorizeRemoteReservationStartHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime  = Timestamp.Now;
                                             var processId  = Process_Id.NewRandom;

                                             OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>? authorizeRemoteReservationStartResponse = null;

                                             try
                                             {

                                                 #region Try to parse ProviderId URL parameter

                                                 if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                                     authorizeRemoteReservationStartResponse = OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>.Failed(
                                                                                                   this,
                                                                                                   new Acknowledgement<AuthorizeRemoteReservationStartRequest>(
                                                                                                       Timestamp.Now,
                                                                                                       Request.EventTrackingId,
                                                                                                       processId,
                                                                                                       Timestamp.Now - Request.Timestamp,
                                                                                                       StatusCode: new StatusCode(
                                                                                                                       StatusCodes.SystemError,
                                                                                                                       "The expected 'providerId' URL parameter could not be parsed!"
                                                                                                                   )
                                                                                                   )
                                                                                               );

                                                 #endregion

                                                 else if (AuthorizeRemoteReservationStartRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                                          providerId,
                                                                                                          out AuthorizeRemoteReservationStartRequest?          pullEVSEDataRequest,
                                                                                                          out String?                                          errorResponse,
                                                                                                          ProcessId:                                           processId,

                                                                                                          Timestamp:                                           Request.Timestamp,
                                                                                                          CancellationToken:                                   Request.CancellationToken,
                                                                                                          EventTrackingId:                                     Request.EventTrackingId,
                                                                                                          RequestTimeout:                                      Request.Timeout ?? DefaultRequestTimeout,

                                                                                                          CustomAuthorizeRemoteReservationStartRequestParser:  CustomAuthorizeRemoteReservationStartRequestParser))
                                                 {

                                                     Counters.AuthorizeRemoteReservationStart.IncRequests_OK();

                                                     #region Send OnAuthorizeRemoteReservationStartRequest event

                                                     try
                                                     {

                                                         if (OnAuthorizeRemoteReservationStartRequest is not null)
                                                             await Task.WhenAll(OnAuthorizeRemoteReservationStartRequest.GetInvocationList().
                                                                                Cast<OnAuthorizeRemoteReservationStartAPIRequestDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              pullEVSEDataRequest!))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPClientAPI) + "." + nameof(OnAuthorizeRemoteReservationStartRequest));
                                                     }

                                                     #endregion

                                                     #region Call async subscribers

                                                     var OnAuthorizeRemoteReservationStartLocal = OnAuthorizeRemoteReservationStart;
                                                     if (OnAuthorizeRemoteReservationStartLocal is not null)
                                                     {
                                                         try
                                                         {

                                                             authorizeRemoteReservationStartResponse = (await Task.WhenAll(OnAuthorizeRemoteReservationStartLocal.GetInvocationList().
                                                                                                                                                                  Cast<OnAuthorizeRemoteReservationStartAPIDelegate>().
                                                                                                                                                                  Select(e => e(Timestamp.Now,
                                                                                                                                                                                this,
                                                                                                                                                                                pullEVSEDataRequest!))))?.FirstOrDefault();

                                                             Counters.AuthorizeRemoteReservationStart.IncResponses_OK();

                                                         }
                                                         catch (Exception e)
                                                         {
                                                             authorizeRemoteReservationStartResponse = OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>.Failed(
                                                                                                           this,
                                                                                                           new Acknowledgement<AuthorizeRemoteReservationStartRequest>(
                                                                                                               Timestamp.Now,
                                                                                                               Request.EventTrackingId,
                                                                                                               processId,
                                                                                                               Timestamp.Now - Request.Timestamp,
                                                                                                               StatusCode: new StatusCode(
                                                                                                                               StatusCodes.DataError,
                                                                                                                               e.Message,
                                                                                                                               e.StackTrace
                                                                                                                           )
                                                                                                           )
                                                                                                       );
                                                         }
                                                     }

                                                     authorizeRemoteReservationStartResponse ??= OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>.Failed(
                                                                                                     this,
                                                                                                     new Acknowledgement<AuthorizeRemoteReservationStartRequest>(
                                                                                                         Timestamp.Now,
                                                                                                         Request.EventTrackingId,
                                                                                                         processId,
                                                                                                         Timestamp.Now - Request.Timestamp,
                                                                                                         StatusCode: new StatusCode(
                                                                                                                         StatusCodes.SystemError,
                                                                                                                         "Could not process the received AuthorizeRemoteReservationStart request!"
                                                                                                                     )
                                                                                                     )
                                                                                                 );

                                                     #endregion

                                                     #region Send OnAuthorizeRemoteReservationStartResponse event

                                                     try
                                                     {

                                                         if (OnAuthorizeRemoteReservationStartResponse is not null)
                                                             await Task.WhenAll(OnAuthorizeRemoteReservationStartResponse.GetInvocationList().
                                                                                Cast<OnAuthorizeRemoteReservationStartAPIResponseDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              authorizeRemoteReservationStartResponse,
                                                                                              Timestamp.Now - startTime))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPClientAPI) + "." + nameof(OnAuthorizeRemoteReservationStartResponse));
                                                     }

                                                     #endregion

                                                 }
                                                 else
                                                     authorizeRemoteReservationStartResponse = OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>.Failed(
                                                                                                   this,
                                                                                                   new Acknowledgement<AuthorizeRemoteReservationStartRequest>(
                                                                                                       Timestamp.Now,
                                                                                                       Request.EventTrackingId,
                                                                                                       processId,
                                                                                                       Timestamp.Now - Request.Timestamp,
                                                                                                       StatusCode: new StatusCode(
                                                                                                                       StatusCodes.DataError,
                                                                                                                       "We could not parse the given AuthorizeRemoteReservationStart request!",
                                                                                                                       errorResponse
                                                                                                                   )
                                                                                                   )
                                                                                               );

                                             }
                                             catch (Exception e)
                                             {
                                                 authorizeRemoteReservationStartResponse = OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>.Failed(
                                                                                               this,
                                                                                               new Acknowledgement<AuthorizeRemoteReservationStartRequest>(
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
                                                        Content                    = authorizeRemoteReservationStartResponse.Response?.ToJSON(CustomAcknowledgementSerializer,
                                                                                                                                              CustomStatusCodeSerializer).
                                                                                                                                       ToString(JSONFormatting).
                                                                                                                                       ToUTF8Bytes()
                                                                                                                             ?? Array.Empty<Byte>(),
                                                        ProcessID                  = processId.ToString(),
                                                        Connection                 = "close"
                                                    }.AsImmutable;

                                          }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region POST  ~/api/oicp/charging/v21/providers/{providerId}/authorize-remote-reservation/stop

            // --------------------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/charging/v21/providers/DE-GDF/authorize-remote-reservation/stop
            // --------------------------------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/charging/v21/providers/{providerId}/authorize-remote-reservation/stop",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logAuthorizeRemoteReservationStopHTTPRequest,
                                         HTTPResponseLogger:  logAuthorizeRemoteReservationStopHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime  = Timestamp.Now;
                                             var processId  = Process_Id.NewRandom;

                                             OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>? authorizeRemoteReservationStopResponse = null;

                                             try
                                             {

                                                 #region Try to parse ProviderId URL parameter

                                                 if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                                     authorizeRemoteReservationStopResponse = OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>.Failed(
                                                                                                  this,
                                                                                                  new Acknowledgement<AuthorizeRemoteReservationStopRequest>(
                                                                                                      Timestamp.Now,
                                                                                                      Request.EventTrackingId,
                                                                                                      processId,
                                                                                                      Timestamp.Now - Request.Timestamp,
                                                                                                      StatusCode: new StatusCode(
                                                                                                                      StatusCodes.SystemError,
                                                                                                                      "The expected 'providerId' URL parameter could not be parsed!"
                                                                                                                  )
                                                                                                  )
                                                                                              );

                                                 #endregion

                                                 else if (AuthorizeRemoteReservationStopRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                                         providerId,
                                                                                                         out AuthorizeRemoteReservationStopRequest?          pullEVSEDataRequest,
                                                                                                         out String?                                         errorResponse,
                                                                                                         ProcessId:                                          processId,

                                                                                                         Timestamp:                                          Request.Timestamp,
                                                                                                         CancellationToken:                                  Request.CancellationToken,
                                                                                                         EventTrackingId:                                    Request.EventTrackingId,
                                                                                                         RequestTimeout:                                     Request.Timeout ?? DefaultRequestTimeout,

                                                                                                         CustomAuthorizeRemoteReservationStopRequestParser:  CustomAuthorizeRemoteReservationStopRequestParser))
                                                 {

                                                     Counters.AuthorizeRemoteReservationStop.IncRequests_OK();

                                                     #region Send OnAuthorizeRemoteReservationStopRequest event

                                                     try
                                                     {

                                                         if (OnAuthorizeRemoteReservationStopRequest is not null)
                                                             await Task.WhenAll(OnAuthorizeRemoteReservationStopRequest.GetInvocationList().
                                                                                Cast<OnAuthorizeRemoteReservationStopAPIRequestDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              pullEVSEDataRequest!))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPClientAPI) + "." + nameof(OnAuthorizeRemoteReservationStopRequest));
                                                     }

                                                     #endregion

                                                     #region Call async subscribers

                                                     var OnAuthorizeRemoteReservationStopLocal = OnAuthorizeRemoteReservationStop;
                                                     if (OnAuthorizeRemoteReservationStopLocal is not null)
                                                     {
                                                         try
                                                         {

                                                             authorizeRemoteReservationStopResponse = (await Task.WhenAll(OnAuthorizeRemoteReservationStopLocal.GetInvocationList().
                                                                                                                                                                Cast<OnAuthorizeRemoteReservationStopAPIDelegate>().
                                                                                                                                                                Select(e => e(Timestamp.Now,
                                                                                                                                                                              this,
                                                                                                                                                                              pullEVSEDataRequest!))))?.FirstOrDefault();

                                                             Counters.AuthorizeRemoteReservationStop.IncResponses_OK();

                                                         }
                                                         catch (Exception e)
                                                         {
                                                             authorizeRemoteReservationStopResponse = OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>.Failed(
                                                                                                          this,
                                                                                                          new Acknowledgement<AuthorizeRemoteReservationStopRequest>(
                                                                                                              Timestamp.Now,
                                                                                                              Request.EventTrackingId,
                                                                                                              processId,
                                                                                                              Timestamp.Now - Request.Timestamp,
                                                                                                              StatusCode: new StatusCode(
                                                                                                                              StatusCodes.DataError,
                                                                                                                              e.Message,
                                                                                                                              e.StackTrace
                                                                                                                          )
                                                                                                          )
                                                                                                      );
                                                         }
                                                     }

                                                     authorizeRemoteReservationStopResponse ??= OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>.Failed(
                                                                                                    this,
                                                                                                    new Acknowledgement<AuthorizeRemoteReservationStopRequest>(
                                                                                                        Timestamp.Now,
                                                                                                        Request.EventTrackingId,
                                                                                                        processId,
                                                                                                        Timestamp.Now - Request.Timestamp,
                                                                                                        StatusCode: new StatusCode(
                                                                                                                        StatusCodes.SystemError,
                                                                                                                        "Could not process the received AuthorizeRemoteReservationStop request!"
                                                                                                                    )
                                                                                                    )
                                                                                                );

                                                     #endregion

                                                     #region Send OnAuthorizeRemoteReservationStopResponse event

                                                     try
                                                     {

                                                         if (OnAuthorizeRemoteReservationStopResponse is not null)
                                                             await Task.WhenAll(OnAuthorizeRemoteReservationStopResponse.GetInvocationList().
                                                                                Cast<OnAuthorizeRemoteReservationStopAPIResponseDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              authorizeRemoteReservationStopResponse,
                                                                                              Timestamp.Now - startTime))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPClientAPI) + "." + nameof(OnAuthorizeRemoteReservationStopResponse));
                                                     }

                                                     #endregion

                                                 }
                                                 else
                                                     authorizeRemoteReservationStopResponse = OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>.Failed(
                                                                                                  this,
                                                                                                  new Acknowledgement<AuthorizeRemoteReservationStopRequest>(
                                                                                                      Timestamp.Now,
                                                                                                      Request.EventTrackingId,
                                                                                                      processId,
                                                                                                      Timestamp.Now - Request.Timestamp,
                                                                                                      StatusCode: new StatusCode(
                                                                                                                      StatusCodes.DataError,
                                                                                                                      "We could not parse the given AuthorizeRemoteReservationStop request!",
                                                                                                                      errorResponse
                                                                                                                  )
                                                                                                  )
                                                                                              );

                                             }
                                             catch (Exception e)
                                             {
                                                 authorizeRemoteReservationStopResponse = OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>.Failed(
                                                                                              this,
                                                                                              new Acknowledgement<AuthorizeRemoteReservationStopRequest>(
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
                                                        Content                    = authorizeRemoteReservationStopResponse.Response?.ToJSON(CustomAcknowledgementSerializer,
                                                                                                                                             CustomStatusCodeSerializer).
                                                                                                                                      ToString(JSONFormatting).
                                                                                                                                      ToUTF8Bytes()
                                                                                                                            ?? Array.Empty<Byte>(),
                                                        ProcessID                  = processId.ToString(),
                                                        Connection                 = "close"
                                                    }.AsImmutable;

                                          }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region POST  ~/api/oicp/charging/v21/providers/{providerId}/authorize-remote/start

            // ---------------------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/charging/v21/providers/DE-GDF/authorize-remote/start
            // ---------------------------------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/charging/v21/providers/{providerId}/authorize-remote/start",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logAuthorizeRemoteStartHTTPRequest,
                                         HTTPResponseLogger:  logAuthorizeRemoteStartHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime  = Timestamp.Now;
                                             var processId  = Process_Id.NewRandom;

                                             OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>? authorizeRemoteStartResponse = null;

                                             try
                                             {

                                                 #region Try to parse ProviderId URL parameter

                                                 if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                                     authorizeRemoteStartResponse = OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>.Failed(
                                                                                        this,
                                                                                        new Acknowledgement<AuthorizeRemoteStartRequest>(
                                                                                            Timestamp.Now,
                                                                                            Request.EventTrackingId,
                                                                                            processId,
                                                                                            Timestamp.Now - Request.Timestamp,
                                                                                            StatusCode: new StatusCode(
                                                                                                            StatusCodes.SystemError,
                                                                                                            "The expected 'providerId' URL parameter could not be parsed!"
                                                                                                        )
                                                                                        )
                                                                                    );

                                                 #endregion

                                                 else if (AuthorizeRemoteStartRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                               providerId,
                                                                                               out AuthorizeRemoteStartRequest?          pullEVSEDataRequest,
                                                                                               out String?                               errorResponse,
                                                                                               ProcessId:                                processId,

                                                                                               Timestamp:                                Request.Timestamp,
                                                                                               CancellationToken:                        Request.CancellationToken,
                                                                                               EventTrackingId:                          Request.EventTrackingId,
                                                                                               RequestTimeout:                           Request.Timeout ?? DefaultRequestTimeout,

                                                                                               CustomAuthorizeRemoteStartRequestParser:  CustomAuthorizeRemoteStartRequestParser))
                                                 {

                                                     Counters.AuthorizeRemoteStart.IncRequests_OK();

                                                     #region Send OnAuthorizeRemoteStartRequest event

                                                     try
                                                     {

                                                         if (OnAuthorizeRemoteStartRequest is not null)
                                                             await Task.WhenAll(OnAuthorizeRemoteStartRequest.GetInvocationList().
                                                                                Cast<OnAuthorizeRemoteStartAPIRequestDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              pullEVSEDataRequest!))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPClientAPI) + "." + nameof(OnAuthorizeRemoteStartRequest));
                                                     }

                                                     #endregion

                                                     #region Call async subscribers

                                                     var OnAuthorizeRemoteStartLocal = OnAuthorizeRemoteStart;
                                                     if (OnAuthorizeRemoteStartLocal is not null)
                                                     {
                                                         try
                                                         {

                                                             authorizeRemoteStartResponse = (await Task.WhenAll(OnAuthorizeRemoteStartLocal.GetInvocationList().
                                                                                                                                            Cast<OnAuthorizeRemoteStartAPIDelegate>().
                                                                                                                                            Select(e => e(Timestamp.Now,
                                                                                                                                                          this,
                                                                                                                                                          pullEVSEDataRequest!))))?.FirstOrDefault();

                                                             Counters.AuthorizeRemoteStart.IncResponses_OK();

                                                         }
                                                         catch (Exception e)
                                                         {
                                                             authorizeRemoteStartResponse = OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>.Failed(
                                                                                                this,
                                                                                                new Acknowledgement<AuthorizeRemoteStartRequest>(
                                                                                                    Timestamp.Now,
                                                                                                    Request.EventTrackingId,
                                                                                                    processId,
                                                                                                    Timestamp.Now - Request.Timestamp,
                                                                                                    StatusCode: new StatusCode(
                                                                                                                    StatusCodes.DataError,
                                                                                                                    e.Message,
                                                                                                                    e.StackTrace
                                                                                                                )
                                                                                                )
                                                                                            );
                                                         }
                                                     }

                                                     authorizeRemoteStartResponse ??= OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>.Failed(
                                                                                          this,
                                                                                          new Acknowledgement<AuthorizeRemoteStartRequest>(
                                                                                              Timestamp.Now,
                                                                                              Request.EventTrackingId,
                                                                                              processId,
                                                                                              Timestamp.Now - Request.Timestamp,
                                                                                              StatusCode: new StatusCode(
                                                                                                              StatusCodes.SystemError,
                                                                                                              "Could not process the received AuthorizeRemoteStart request!"
                                                                                                          )
                                                                                          )
                                                                                      );

                                                     #endregion

                                                     #region Send OnAuthorizeRemoteStartResponse event

                                                     try
                                                     {

                                                         if (OnAuthorizeRemoteStartResponse is not null)
                                                             await Task.WhenAll(OnAuthorizeRemoteStartResponse.GetInvocationList().
                                                                                Cast<OnAuthorizeRemoteStartAPIResponseDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              authorizeRemoteStartResponse,
                                                                                              Timestamp.Now - startTime))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPClientAPI) + "." + nameof(OnAuthorizeRemoteStartResponse));
                                                     }

                                                     #endregion

                                                 }
                                                 else
                                                     authorizeRemoteStartResponse = OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>.Failed(
                                                                                        this,
                                                                                        new Acknowledgement<AuthorizeRemoteStartRequest>(
                                                                                            Timestamp.Now,
                                                                                            Request.EventTrackingId,
                                                                                            processId,
                                                                                            Timestamp.Now - Request.Timestamp,
                                                                                            StatusCode: new StatusCode(
                                                                                                            StatusCodes.DataError,
                                                                                                            "We could not parse the given AuthorizeRemoteStart request!",
                                                                                                            errorResponse
                                                                                                        )
                                                                                        )
                                                                                    );

                                             }
                                             catch (Exception e)
                                             {
                                                 authorizeRemoteStartResponse = OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>.Failed(
                                                                                    this,
                                                                                    new Acknowledgement<AuthorizeRemoteStartRequest>(
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
                                                        Content                    = authorizeRemoteStartResponse.Response?.ToJSON(CustomAcknowledgementSerializer,
                                                                                                                                   CustomStatusCodeSerializer).
                                                                                                                            ToString(JSONFormatting).
                                                                                                                            ToUTF8Bytes()
                                                                                                                  ?? Array.Empty<Byte>(),
                                                        ProcessID                  = processId.ToString(),
                                                        Connection                 = "close"
                                                    }.AsImmutable;

                                          }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region POST  ~/api/oicp/charging/v21/providers/{providerId}/authorize-remote/stop

            // --------------------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/charging/v21/providers/DE-GDF/authorize-remote/stop
            // --------------------------------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/charging/v21/providers/{providerId}/authorize-remote/stop",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logAuthorizeRemoteStopHTTPRequest,
                                         HTTPResponseLogger:  logAuthorizeRemoteStopHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime  = Timestamp.Now;
                                             var processId  = Process_Id.NewRandom;

                                             OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>? authorizeRemoteStopResponse = null;

                                             try
                                             {

                                                 #region Try to parse ProviderId URL parameter

                                                 if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                                     authorizeRemoteStopResponse = OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>.Failed(
                                                                                       this,
                                                                                       new Acknowledgement<AuthorizeRemoteStopRequest>(
                                                                                           Timestamp.Now,
                                                                                           Request.EventTrackingId,
                                                                                           processId,
                                                                                           Timestamp.Now - Request.Timestamp,
                                                                                           StatusCode: new StatusCode(
                                                                                                           StatusCodes.SystemError,
                                                                                                           "The expected 'providerId' URL parameter could not be parsed!"
                                                                                                       )
                                                                                       )
                                                                                   );

                                                 #endregion

                                                 else if (AuthorizeRemoteStopRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                              providerId,
                                                                                              out AuthorizeRemoteStopRequest?          pullEVSEDataRequest,
                                                                                              out String?                              errorResponse,
                                                                                              ProcessId:                               processId,

                                                                                              Timestamp:                               Request.Timestamp,
                                                                                              CancellationToken:                       Request.CancellationToken,
                                                                                              EventTrackingId:                         Request.EventTrackingId,
                                                                                              RequestTimeout:                          Request.Timeout ?? DefaultRequestTimeout,

                                                                                              CustomAuthorizeRemoteStopRequestParser:  CustomAuthorizeRemoteStopRequestParser))
                                                 {

                                                     Counters.AuthorizeRemoteStop.IncRequests_OK();

                                                     #region Send OnAuthorizeRemoteStopRequest event

                                                     try
                                                     {

                                                         if (OnAuthorizeRemoteStopRequest is not null)
                                                             await Task.WhenAll(OnAuthorizeRemoteStopRequest.GetInvocationList().
                                                                                Cast<OnAuthorizeRemoteStopAPIRequestDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              pullEVSEDataRequest!))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPClientAPI) + "." + nameof(OnAuthorizeRemoteStopRequest));
                                                     }

                                                     #endregion

                                                     #region Call async subscribers

                                                     var OnAuthorizeRemoteStopLocal = OnAuthorizeRemoteStop;
                                                     if (OnAuthorizeRemoteStopLocal is not null)
                                                     {
                                                         try
                                                         {

                                                             authorizeRemoteStopResponse = (await Task.WhenAll(OnAuthorizeRemoteStopLocal.GetInvocationList().
                                                                                                                                          Cast<OnAuthorizeRemoteStopAPIDelegate>().
                                                                                                                                          Select(e => e(Timestamp.Now,
                                                                                                                                                        this,
                                                                                                                                                        pullEVSEDataRequest!))))?.FirstOrDefault();

                                                             Counters.AuthorizeRemoteStop.IncResponses_OK();

                                                         }
                                                         catch (Exception e)
                                                         {
                                                             authorizeRemoteStopResponse = OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>.Failed(
                                                                                               this,
                                                                                               new Acknowledgement<AuthorizeRemoteStopRequest>(
                                                                                                   Timestamp.Now,
                                                                                                   Request.EventTrackingId,
                                                                                                   processId,
                                                                                                   Timestamp.Now - Request.Timestamp,
                                                                                                   StatusCode: new StatusCode(
                                                                                                                   StatusCodes.DataError,
                                                                                                                   e.Message,
                                                                                                                   e.StackTrace
                                                                                                               )
                                                                                               )
                                                                                           );
                                                         }
                                                     }

                                                     authorizeRemoteStopResponse ??= OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>.Failed(
                                                                                         this,
                                                                                         new Acknowledgement<AuthorizeRemoteStopRequest>(
                                                                                             Timestamp.Now,
                                                                                             Request.EventTrackingId,
                                                                                             processId,
                                                                                             Timestamp.Now - Request.Timestamp,
                                                                                             StatusCode: new StatusCode(
                                                                                                             StatusCodes.SystemError,
                                                                                                             "Could not process the received AuthorizeRemoteStop request!"
                                                                                                         )
                                                                                         )
                                                                                     );

                                                     #endregion

                                                     #region Send OnAuthorizeRemoteStopResponse event

                                                     try
                                                     {

                                                         if (OnAuthorizeRemoteStopResponse is not null)
                                                             await Task.WhenAll(OnAuthorizeRemoteStopResponse.GetInvocationList().
                                                                                Cast<OnAuthorizeRemoteStopAPIResponseDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              authorizeRemoteStopResponse,
                                                                                              Timestamp.Now - startTime))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPClientAPI) + "." + nameof(OnAuthorizeRemoteStopResponse));
                                                     }

                                                     #endregion

                                                 }
                                                 else
                                                     authorizeRemoteStopResponse = OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>.Failed(
                                                                                       this,
                                                                                       new Acknowledgement<AuthorizeRemoteStopRequest>(
                                                                                           Timestamp.Now,
                                                                                           Request.EventTrackingId,
                                                                                           processId,
                                                                                           Timestamp.Now - Request.Timestamp,
                                                                                           StatusCode: new StatusCode(
                                                                                                           StatusCodes.DataError,
                                                                                                           "We could not parse the given AuthorizeRemoteStop request!",
                                                                                                           errorResponse
                                                                                                       )
                                                                                       )
                                                                                   );

                                             }
                                             catch (Exception e)
                                             {
                                                 authorizeRemoteStopResponse = OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>.Failed(
                                                                                   this,
                                                                                   new Acknowledgement<AuthorizeRemoteStopRequest>(
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
                                                        Content                    = authorizeRemoteStopResponse.Response?.ToJSON(CustomAcknowledgementSerializer,
                                                                                                                                  CustomStatusCodeSerializer).
                                                                                                                           ToString(JSONFormatting).
                                                                                                                           ToUTF8Bytes()
                                                                                                                 ?? Array.Empty<Byte>(),
                                                        ProcessID                  = processId.ToString(),
                                                        Connection                 = "close"
                                                    }.AsImmutable;

                                          }, AllowReplacement: URLReplacement.Allow);

            #endregion


            #region POST  ~/api/oicp/cdrmgmt/v22/providers/{providerId}/get-charge-detail-records-request

            // -------------------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/cdrmgmt/v22/providers/DE-GDF/get-charge-detail-records-request
            // -------------------------------------------------------------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URLPathPrefix + "/api/oicp/cdrmgmt/v22/providers/{providerId}/get-charge-detail-records-request",
                                         HTTPContentType.JSON_UTF8,
                                         HTTPRequestLogger:   logGetChargeDetailRecordsHTTPRequest,
                                         HTTPResponseLogger:  logGetChargeDetailRecordsHTTPResponse,
                                         HTTPDelegate:        async Request => {

                                             var startTime  = Timestamp.Now;
                                             var page       = Request.QueryString.GetUInt32 ("page");
                                             var size       = Request.QueryString.GetUInt32 ("size");
                                             var sortOrder  = Request.QueryString.GetStrings("sortOrder");
                                             var processId  = Process_Id.NewRandom;

                                             OICPResult<GetChargeDetailRecordsResponse>? getChargeDetailRecordsResponse = null;

                                             try
                                             {

                                                 #region Try to parse ProviderId URL parameter

                                                 if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                                     getChargeDetailRecordsResponse = OICPResult<GetChargeDetailRecordsResponse>.Failed(
                                                                                          this,
                                                                                          new GetChargeDetailRecordsResponse(
                                                                                              Timestamp.Now,
                                                                                              Request.EventTrackingId,
                                                                                              processId,
                                                                                              Timestamp.Now - Request.Timestamp,
                                                                                              Array.Empty<ChargeDetailRecord>(),
                                                                                              StatusCode: new StatusCode(
                                                                                                              StatusCodes.SystemError,
                                                                                                              "The expected 'providerId' URL parameter could not be parsed!"
                                                                                                          )
                                                                                          )
                                                                                      );

                                                 #endregion

                                                 else if (GetChargeDetailRecordsRequest.TryParse(Request.HTTPBody.ToUTF8String(),
                                                                                                 //     providerId ????
                                                                                                 out GetChargeDetailRecordsRequest?          pullEVSEDataRequest,
                                                                                                 out String?                                 errorResponse,
                                                                                                 ProcessId:                                  processId,
                                                                                                 Page:                                       page,
                                                                                                 Size:                                       size,
                                                                                                 SortOrder:                                  sortOrder,

                                                                                                 Timestamp:                                  Request.Timestamp,
                                                                                                 CancellationToken:                          Request.CancellationToken,
                                                                                                 EventTrackingId:                            Request.EventTrackingId,
                                                                                                 RequestTimeout:                             Request.Timeout ?? DefaultRequestTimeout,

                                                                                                 CustomGetChargeDetailRecordsRequestParser:  CustomGetChargeDetailRecordsRequestParser))
                                                 {

                                                     Counters.GetChargeDetailRecords.IncRequests_OK();

                                                     #region Send OnGetChargeDetailRecordsRequest event

                                                     try
                                                     {

                                                         if (OnGetChargeDetailRecordsRequest is not null)
                                                             await Task.WhenAll(OnGetChargeDetailRecordsRequest.GetInvocationList().
                                                                                Cast<OnGetChargeDetailRecordsAPIRequestDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              pullEVSEDataRequest!))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPClientAPI) + "." + nameof(OnGetChargeDetailRecordsRequest));
                                                     }

                                                     #endregion

                                                     #region Call async subscribers

                                                     var OnGetChargeDetailRecordsLocal = OnGetChargeDetailRecords;
                                                     if (OnGetChargeDetailRecordsLocal is not null)
                                                     {
                                                         try
                                                         {

                                                             getChargeDetailRecordsResponse = (await Task.WhenAll(OnGetChargeDetailRecordsLocal.GetInvocationList().
                                                                                                                                                Cast<OnGetChargeDetailRecordsAPIDelegate>().
                                                                                                                                                Select(e => e(Timestamp.Now,
                                                                                                                                                              this,
                                                                                                                                                              pullEVSEDataRequest!))))?.FirstOrDefault();

                                                             Counters.GetChargeDetailRecords.IncResponses_OK();

                                                         }
                                                         catch (Exception e)
                                                         {
                                                             getChargeDetailRecordsResponse = OICPResult<GetChargeDetailRecordsResponse>.Failed(
                                                                                                  this,
                                                                                                  new GetChargeDetailRecordsResponse(
                                                                                                      Timestamp.Now,
                                                                                                      Request.EventTrackingId,
                                                                                                      processId,
                                                                                                      Timestamp.Now - Request.Timestamp,
                                                                                                      Array.Empty<ChargeDetailRecord>(),
                                                                                                      pullEVSEDataRequest,
                                                                                                      StatusCode: new StatusCode(
                                                                                                                      StatusCodes.DataError,
                                                                                                                      e.Message,
                                                                                                                      e.StackTrace
                                                                                                                  )
                                                                                                  )
                                                                                              );
                                                         }
                                                     }

                                                     getChargeDetailRecordsResponse ??= OICPResult<GetChargeDetailRecordsResponse>.Failed(
                                                                                            this,
                                                                                            new GetChargeDetailRecordsResponse(
                                                                                                Timestamp.Now,
                                                                                                Request.EventTrackingId,
                                                                                                processId,
                                                                                                Timestamp.Now - Request.Timestamp,
                                                                                                Array.Empty<ChargeDetailRecord>(),
                                                                                                pullEVSEDataRequest,
                                                                                                StatusCode: new StatusCode(
                                                                                                                StatusCodes.SystemError,
                                                                                                                "Could not process the received GetChargeDetailRecords request!"
                                                                                                            )
                                                                                            )
                                                                                        );

                                                     #endregion

                                                     #region Send OnGetChargeDetailRecordsResponse event

                                                     try
                                                     {

                                                         if (OnGetChargeDetailRecordsResponse is not null)
                                                             await Task.WhenAll(OnGetChargeDetailRecordsResponse.GetInvocationList().
                                                                                Cast<OnGetChargeDetailRecordsAPIResponseDelegate>().
                                                                                Select(e => e(Timestamp.Now,
                                                                                              this,
                                                                                              getChargeDetailRecordsResponse,
                                                                                              Timestamp.Now - startTime))).
                                                                                ConfigureAwait(false);

                                                     }
                                                     catch (Exception e)
                                                     {
                                                         DebugX.LogException(e, nameof(EMPClientAPI) + "." + nameof(OnGetChargeDetailRecordsResponse));
                                                     }

                                                     #endregion

                                                 }
                                                 else
                                                     getChargeDetailRecordsResponse = OICPResult<GetChargeDetailRecordsResponse>.Failed(
                                                                                          this,
                                                                                          new GetChargeDetailRecordsResponse(
                                                                                              Timestamp.Now,
                                                                                              Request.EventTrackingId,
                                                                                              processId,
                                                                                              Timestamp.Now - Request.Timestamp,
                                                                                              Array.Empty<ChargeDetailRecord>(),
                                                                                              pullEVSEDataRequest,
                                                                                              StatusCode: new StatusCode(
                                                                                                              StatusCodes.DataError,
                                                                                                              "We could not parse the given GetChargeDetailRecords request!",
                                                                                                              errorResponse
                                                                                                          )
                                                                                          )
                                                                                      );

                                             }
                                             catch (Exception e)
                                             {
                                                 getChargeDetailRecordsResponse = OICPResult<GetChargeDetailRecordsResponse>.Failed(
                                                                                      this,
                                                                                      new GetChargeDetailRecordsResponse(
                                                                                          Timestamp.Now,
                                                                                          Request.EventTrackingId,
                                                                                          processId,
                                                                                          Timestamp.Now - Request.Timestamp,
                                                                                          Array.Empty<ChargeDetailRecord>(),
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
                                                        Content                    = getChargeDetailRecordsResponse.Response?.ToJSON(CustomGetChargeDetailRecordsResponseSerializer,
                                                                                                                                     CustomIPagedResponseSerializer,
                                                                                                                                     CustomChargeDetailRecordSerializer,
                                                                                                                                     CustomIdentificationSerializer,
                                                                                                                                     CustomSignedMeteringValueSerializer,
                                                                                                                                     CustomCalibrationLawVerificationSerializer,
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
