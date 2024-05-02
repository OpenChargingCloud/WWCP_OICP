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

using System.Net.Security;
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
        public new const String  DefaultHTTPServerName   = "GraphDefined OICP " + Version.String + " EMP HTTP API";

        /// <summary>
        /// The default HTTP service name.
        /// </summary>
        public new const String  DefaultHTTPServiceName  = "GraphDefined OICP " + Version.String + " EMP HTTP API";

        /// <summary>
        /// The default logging context.
        /// </summary>
        public     const String  DefaultLoggingContext   = "EMPServerAPI";

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
        public EMPClientAPILogger?         Logger            { get; }

        /// <summary>
        /// EMP Client API counters.
        /// </summary>
        public APICounters                 Counters          { get; }

        public Newtonsoft.Json.Formatting  JSONFormatting    { get; set; }

        #endregion

        #region Custom JSON parsers

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

        #endregion

        #region Custom JSON serializers

        public CustomJObjectSerializerDelegate<PullEVSEDataResponse>?                CustomPullEVSEDataResponseSerializer                  { get; set; }
        public CustomJObjectSerializerDelegate<EVSEDataRecord>?                      CustomEVSEDataRecordSerializer                        { get; set; }
        public CustomJObjectSerializerDelegate<Address>?                             CustomAddressSerializer                               { get; set; }
        public CustomJObjectSerializerDelegate<ChargingFacility>?                    CustomChargingFacilitySerializer                      { get; set; }
        public CustomJObjectSerializerDelegate<GeoCoordinates>?                      CustomGeoCoordinatesSerializer                        { get; set; }
        public CustomJObjectSerializerDelegate<EnergyMeter>?                         CustomEnergyMeterSerializer                           { get; set; }
        public CustomJObjectSerializerDelegate<TransparencySoftwareStatus>?          CustomTransparencySoftwareStatusSerializer            { get; set; }
        public CustomJObjectSerializerDelegate<TransparencySoftware>?                CustomTransparencySoftwareSerializer                  { get; set; }
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



        #endregion

        #region Custom request/response logging converters

        #region PullEVSEData                   (Request/Response)Converter

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

        #region PullEVSEStatus                 (Request/Response)Converter

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

        #region PullEVSEStatusById             (Request/Response)Converter

        public Func<DateTime, Object, PullEVSEStatusByIdRequest, String>
            PullEVSEStatusByIdRequestConverter               { get; set; }

            = (timestamp, sender, pullEVSEStatusByIdRequest)
            => String.Concat(pullEVSEStatusByIdRequest.ProviderId, ", ids: " + pullEVSEStatusByIdRequest.EVSEIds.Count());

        public Func<DateTime, Object, PullEVSEStatusByIdRequest, OICPResult<PullEVSEStatusByIdResponse>, TimeSpan, String>
            PullEVSEStatusByIdResponseConverter              { get; set; }

            = (timestamp, sender, pullEVSEStatusByIdRequest, pullEVSEStatusByIdResponse, runtime)
            => String.Concat(pullEVSEStatusByIdRequest.ProviderId, ", ids: " + pullEVSEStatusByIdRequest.EVSEIds.Count(),
                             " => ",
                             pullEVSEStatusByIdResponse.Response?.StatusCode?.ToString() ?? "failed!", " ", pullEVSEStatusByIdResponse.Response?.EVSEStatusRecords.Count() ?? 0, " evse status record(s)");

        #endregion

        #region PullEVSEStatusByOperatorId     (Request/Response)Converter

        public Func<DateTime, Object, PullEVSEStatusByOperatorIdRequest, String>
            PullEVSEStatusByOperatorIdRequestConverter       { get; set; }

            = (timestamp, sender, pullEVSEStatusByOperatorIdRequest)
            => String.Concat(pullEVSEStatusByOperatorIdRequest.ProviderId, ", operator ids: " + pullEVSEStatusByOperatorIdRequest.OperatorIds.Count());

        public Func<DateTime, Object, PullEVSEStatusByOperatorIdRequest, OICPResult<PullEVSEStatusByOperatorIdResponse>, TimeSpan, String>
            PullEVSEStatusByOperatorIdResponseConverter      { get; set; }

            = (timestamp, sender, pullEVSEStatusByOperatorIdRequest, pullEVSEStatusByOperatorIdResponse, runtime)
            => String.Concat(pullEVSEStatusByOperatorIdRequest.ProviderId, ", operator ids: " + pullEVSEStatusByOperatorIdRequest.OperatorIds.Count(),
                             " => ",
                             pullEVSEStatusByOperatorIdResponse.Response?.StatusCode?.ToString() ?? "failed!", " ", pullEVSEStatusByOperatorIdResponse.Response?.OperatorEVSEStatus.Count() ?? 0, " operator evse status record(s), ", pullEVSEStatusByOperatorIdResponse.Response?.OperatorEVSEStatus.Sum(cc => cc.EVSEStatusRecords.Count()) ?? 0, " evse status record(s)");

        #endregion


        #region PullPricingProductData         (Request/Response)Converter

        public Func<DateTime, Object, PullPricingProductDataRequest, String>
            PullPricingProductDataRequestConverter           { get; set; }

            = (timestamp, sender, pullPricingProductDataRequest)
            => String.Concat(pullPricingProductDataRequest.ProviderId, pullPricingProductDataRequest.LastCall.HasValue ? ", last call: " + pullPricingProductDataRequest.LastCall.Value.ToLocalTime().ToString() : "");

        public Func<DateTime, Object, PullPricingProductDataRequest, OICPResult<PullPricingProductDataResponse>, TimeSpan, String>
            PullPricingProductDataResponseConverter          { get; set; }

            = (timestamp, sender, pullPricingProductDataRequest, pullPricingProductDataResponse, runtime)
            => String.Concat(pullPricingProductDataRequest.ProviderId, pullPricingProductDataRequest.LastCall.HasValue ? ", last call: " + pullPricingProductDataRequest.LastCall.Value.ToLocalTime().ToString() : "",
                             " => ",
                             pullPricingProductDataResponse.Response?.StatusCode?.ToString() ?? "failed!", " ", pullPricingProductDataResponse.Response?.NumberOfElements ?? 0, " pricing product data record(s)");

        #endregion

        #region PullEVSEPricing                (Request/Response)Converter

        public Func<DateTime, Object, PullEVSEPricingRequest, String>
            PullEVSEPricingRequestConverter                  { get; set; }

            = (timestamp, sender, pullEVSEPricingRequest)
            => String.Concat(pullEVSEPricingRequest.ProviderId, pullEVSEPricingRequest.LastCall.HasValue ? ", last call: " + pullEVSEPricingRequest.LastCall.Value.ToLocalTime().ToString() : "");

        public Func<DateTime, Object, PullEVSEPricingRequest, OICPResult<PullEVSEPricingResponse>, TimeSpan, String>
            PullEVSEPricingResponseConverter                 { get; set; }

            = (timestamp, sender, pullEVSEPricingRequest, pullEVSEPricingResponse, runtime)
            => String.Concat(pullEVSEPricingRequest.ProviderId, pullEVSEPricingRequest.LastCall.HasValue ? ", last call: " + pullEVSEPricingRequest.LastCall.Value.ToLocalTime().ToString() : "",
                             " => ",
                             pullEVSEPricingResponse.Response?.StatusCode?.ToString() ?? "failed!", " ", pullEVSEPricingResponse.Response?.NumberOfElements ?? 0, " evse data record(s)");

        #endregion


        #region PushAuthenticationData         (Request/Response)Converter

        public Func<DateTime, Object, PushAuthenticationDataRequest, String>
            PushAuthenticationDataRequestConverter           { get; set; }

            = (timestamp, sender, pushAuthenticationDataRequest)
            => String.Concat(pushAuthenticationDataRequest.Action, " of ", pushAuthenticationDataRequest.ProviderAuthenticationData.Identifications.Count(), " identifications(s)");

        public Func<DateTime, Object, PushAuthenticationDataRequest, OICPResult<Acknowledgement<PushAuthenticationDataRequest>>, TimeSpan, String>
            PushAuthenticationDataResponseConverter          { get; set; }

            = (timestamp, sender, pushAuthenticationDataRequest, pushAuthenticationDataResponse, runtime)
            => String.Concat(pushAuthenticationDataRequest.Action, " of ", pushAuthenticationDataRequest.ProviderAuthenticationData.Identifications.Count(), " identifications(s)",
                             " => ",
                             pushAuthenticationDataResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion


        #region AuthorizeRemoteReservationStart(Request/Response)Converter

        public Func<DateTime, Object, AuthorizeRemoteReservationStartRequest, String>
            AuthorizeRemoteReservationStartRequestConverter           { get; set; }

            = (timestamp, sender, authorizeRemoteReservationStartRequest)
            => String.Concat(authorizeRemoteReservationStartRequest.Identification, " at ", authorizeRemoteReservationStartRequest.EVSEId,
                                                                                            authorizeRemoteReservationStartRequest.PartnerProductId.HasValue
                                                                                                ? " (" + authorizeRemoteReservationStartRequest.PartnerProductId.Value.ToString() + ")"
                                                                                                : "");

        public Func<DateTime, Object, AuthorizeRemoteReservationStartRequest, OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>, TimeSpan, String>
            AuthorizeRemoteReservationStartResponseConverter          { get; set; }

            = (timestamp, sender, authorizeRemoteReservationStartRequest, authorizeRemoteReservationStartResponse, runtime)
            => String.Concat(authorizeRemoteReservationStartRequest.Identification, " at ", authorizeRemoteReservationStartRequest.EVSEId,
                                                                                            authorizeRemoteReservationStartRequest.PartnerProductId.HasValue
                                                                                                ? " (" + authorizeRemoteReservationStartRequest.PartnerProductId.Value.ToString() + ")"
                                                                                                : "",
                             " => ",
                             authorizeRemoteReservationStartResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion

        #region AuthorizeRemoteReservationStop (Request/Response)Converter

        public Func<DateTime, Object, AuthorizeRemoteReservationStopRequest, String>
            AuthorizeRemoteReservationStopRequestConverter           { get; set; }

            = (timestamp, sender, authorizeRemoteReservationStopRequest)
            => String.Concat(authorizeRemoteReservationStopRequest.SessionId, " at ", authorizeRemoteReservationStopRequest.EVSEId);

        public Func<DateTime, Object, AuthorizeRemoteReservationStopRequest, OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>, TimeSpan, String>
            AuthorizeRemoteReservationStopResponseConverter          { get; set; }

            = (timestamp, sender, authorizeRemoteReservationStopRequest, authorizeRemoteReservationStopResponse, runtime)
            => String.Concat(authorizeRemoteReservationStopRequest.SessionId, " at ", authorizeRemoteReservationStopRequest.EVSEId,
                             " => ",
                             authorizeRemoteReservationStopResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion

        #region AuthorizeRemoteStart           (Request/Response)Converter

        public Func<DateTime, Object, AuthorizeRemoteStartRequest, String>
            AuthorizeRemoteStartRequestConverter           { get; set; }

            = (timestamp, sender, authorizeRemoteStartRequest)
            => String.Concat(authorizeRemoteStartRequest.Identification, " at ", authorizeRemoteStartRequest.EVSEId,
                                                                                            authorizeRemoteStartRequest.PartnerProductId.HasValue
                                                                                                ? " (" + authorizeRemoteStartRequest.PartnerProductId.Value.ToString() + ")"
                                                                                                : "");

        public Func<DateTime, Object, AuthorizeRemoteStartRequest, OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>, TimeSpan, String>
            AuthorizeRemoteStartResponseConverter          { get; set; }

            = (timestamp, sender, authorizeRemoteStartRequest, authorizeRemoteStartResponse, runtime)
            => String.Concat(authorizeRemoteStartRequest.Identification, " at ", authorizeRemoteStartRequest.EVSEId,
                                                                                            authorizeRemoteStartRequest.PartnerProductId.HasValue
                                                                                                ? " (" + authorizeRemoteStartRequest.PartnerProductId.Value.ToString() + ")"
                                                                                                : "",
                             " => ",
                             authorizeRemoteStartResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion

        #region AuthorizeRemoteStop            (Request/Response)Converter

        public Func<DateTime, Object, AuthorizeRemoteStopRequest, String>
            AuthorizeRemoteStopRequestConverter           { get; set; }

            = (timestamp, sender, authorizeRemoteStopRequest)
            => String.Concat(authorizeRemoteStopRequest.SessionId, " at ", authorizeRemoteStopRequest.EVSEId);

        public Func<DateTime, Object, AuthorizeRemoteStopRequest, OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>, TimeSpan, String>
            AuthorizeRemoteStopResponseConverter          { get; set; }

            = (timestamp, sender, authorizeRemoteStopRequest, authorizeRemoteStopResponse, runtime)
            => String.Concat(authorizeRemoteStopRequest.SessionId, " at ", authorizeRemoteStopRequest.EVSEId,
                             " => ",
                             authorizeRemoteStopResponse.Response?.StatusCode.ToString() ?? "failed!");

        #endregion


        #region GetChargeDetailRecords         (Request/Response)Converter

        public Func<DateTime, Object, GetChargeDetailRecordsRequest, String>
            GetChargeDetailRecordsRequestConverter                     { get; set; }

            = (timestamp, sender, getChargeDetailRecordsRequest)
            => String.Concat(getChargeDetailRecordsRequest.ProviderId, " from ", getChargeDetailRecordsRequest.From.ToLocalTime().ToString(), " to " + getChargeDetailRecordsRequest.To.ToLocalTime().ToString());

        public Func<DateTime, Object, GetChargeDetailRecordsRequest, OICPResult<GetChargeDetailRecordsResponse>, TimeSpan, String>
            GetChargeDetailRecordsResponseConverter                    { get; set; }

            = (timestamp, sender, getChargeDetailRecordsRequest, getChargeDetailRecordsResponse, runtime)
            => String.Concat(getChargeDetailRecordsRequest.ProviderId, " from ", getChargeDetailRecordsRequest.From.ToLocalTime().ToString(), " to " + getChargeDetailRecordsRequest.To.ToLocalTime().ToString(),
                             " => ",
                             getChargeDetailRecordsResponse.Response?.StatusCode?.ToString() ?? "failed!", " ", getChargeDetailRecordsResponse.Response?.NumberOfElements ?? 0, " charge detail record(s)");

        #endregion

        #endregion

        #region Events

        #region (protected internal) OnPullEVSEDataHTTPRequest

        /// <summary>
        /// An event sent whenever an PullEVSEData HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnPullEVSEDataHTTPRequest = new ();

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
        public HTTPResponseLogEvent OnPullEVSEDataHTTPResponse = new ();

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
        public HTTPRequestLogEvent OnPullEVSEStatusHTTPRequest = new ();

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
        public HTTPResponseLogEvent OnPullEVSEStatusHTTPResponse = new ();

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
        public HTTPRequestLogEvent OnPullEVSEStatusByIdHTTPRequest = new ();

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
        public HTTPResponseLogEvent OnPullEVSEStatusByIdHTTPResponse = new ();

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
        public HTTPRequestLogEvent OnPullEVSEStatusByOperatorIdHTTPRequest = new ();

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
        public HTTPResponseLogEvent OnPullEVSEStatusByOperatorIdHTTPResponse = new ();

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
        public HTTPRequestLogEvent OnPullPricingProductDataHTTPRequest = new ();

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
        public HTTPResponseLogEvent OnPullPricingProductDataHTTPResponse = new ();

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
        public HTTPRequestLogEvent OnPullEVSEPricingHTTPRequest = new ();

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
        public HTTPResponseLogEvent OnPullEVSEPricingHTTPResponse = new ();

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
        public HTTPRequestLogEvent OnPushAuthenticationDataHTTPRequest = new ();

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
        public HTTPResponseLogEvent OnPushAuthenticationDataHTTPResponse = new ();

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
        public HTTPRequestLogEvent OnAuthorizeRemoteReservationStartHTTPRequest = new ();

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
        public HTTPResponseLogEvent OnAuthorizeRemoteReservationStartHTTPResponse = new ();

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
        public HTTPRequestLogEvent OnAuthorizeRemoteReservationStopHTTPRequest = new ();

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
        public HTTPResponseLogEvent OnAuthorizeRemoteReservationStopHTTPResponse = new ();

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
        public HTTPRequestLogEvent OnAuthorizeRemoteStartHTTPRequest = new ();

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
        public HTTPResponseLogEvent OnAuthorizeRemoteStartHTTPResponse = new ();

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
        public HTTPRequestLogEvent OnAuthorizeRemoteStopHTTPRequest = new ();

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
        public HTTPResponseLogEvent OnAuthorizeRemoteStopHTTPResponse = new ();

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
        public HTTPRequestLogEvent OnGetChargeDetailRecordsHTTPRequest = new ();

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
        public HTTPResponseLogEvent OnGetChargeDetailRecordsHTTPResponse = new ();

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

        public EMPClientAPI(HTTPAPI                  HTTPAPI,
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
                                       ? new EMPClientAPILogger(this,
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
        /// <param name="AutoStart">Whether to start the API automatically.</param>
        public EMPClientAPI(HTTPHostname?                                              HTTPHostname                 = null,
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
                                       ? new EMPClientAPILogger(this,
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
                                              ContentType     = HTTPContentType.Text.PLAIN,
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
            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.POST,
                              URLPathPrefix + "/api/oicp/evsepull/v23/providers/{providerId}/data-records",
                              HTTPContentType.Application.JSON_UTF8,
                              HTTPRequestLogger:   logPullEVSEDataHTTPRequest,
                              HTTPResponseLogger:  logPullEVSEDataHTTPResponse,
                              HTTPDelegate:        async Request => {

                                  var startTime  = Timestamp.Now;
                                  var page       = Request.QueryString.GetUInt32 ("page");
                                  var size       = Request.QueryString.GetUInt32 ("size");
                                  var sortOrder  = Request.QueryString.GetStrings("sortOrder");
                                  var processId  = Process_Id.NewRandom();

                                  OICPResult<PullEVSEDataResponse>? pullEVSEDataResponse = null;

                                  try
                                  {

                                      #region Try to parse ProviderId URL parameter

                                      if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                      {

                                          Counters.PullEVSEData.IncRequests_Error();

                                          pullEVSEDataResponse = OICPResult<PullEVSEDataResponse>.Failed(
                                                                     this,
                                                                     new PullEVSEDataResponse(
                                                                         Timestamp.Now,
                                                                         Request.EventTrackingId,
                                                                         processId,
                                                                         Timestamp.Now - Request.Timestamp,
                                                                         [],
                                                                         StatusCode: new StatusCode(
                                                                                         StatusCodes.SystemError,
                                                                                         "The expected 'providerId' URL parameter could not be parsed!"
                                                                                     )
                                                                     )
                                                                 );

                                      }

                                      #endregion

                                      else if (PullEVSEDataRequest.TryParse(JObject.Parse(Request.HTTPBody?.ToUTF8String() ?? ""),
                                                                            //     providerId ????
                                                                            out var                           pullEVSEDataRequest,
                                                                            out var                           errorResponse,
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

                                                  Counters.PullEVSEData.IncResponses_Error();

                                                  pullEVSEDataResponse = OICPResult<PullEVSEDataResponse>.Failed(
                                                                             this,
                                                                             new PullEVSEDataResponse(
                                                                                 Timestamp.Now,
                                                                                 Request.EventTrackingId,
                                                                                 processId,
                                                                                 Timestamp.Now - Request.Timestamp,
                                                                                 [],
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

                                          if (pullEVSEDataResponse is null)
                                          {

                                              Counters.PullEVSEData.IncResponses_Error();

                                              pullEVSEDataResponse = OICPResult<PullEVSEDataResponse>.Failed(
                                                                         this,
                                                                         new PullEVSEDataResponse(
                                                                             Timestamp.Now,
                                                                             Request.EventTrackingId,
                                                                             processId,
                                                                             Timestamp.Now - Request.Timestamp,
                                                                             [],
                                                                             pullEVSEDataRequest,
                                                                             StatusCode: new StatusCode(
                                                                                             StatusCodes.SystemError,
                                                                                             "Could not process the received PullEVSEData request!"
                                                                                         )
                                                                         )
                                                                     );

                                          }

                                          #endregion

                                          #region Send OnPullEVSEDataResponse event

                                          try
                                          {

                                              if (OnPullEVSEDataResponse is not null)
                                                  await Task.WhenAll(OnPullEVSEDataResponse.GetInvocationList().
                                                                     Cast<OnPullEVSEDataAPIResponseDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   pullEVSEDataRequest!,
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
                                      {

                                          Counters.PullEVSEData.IncRequests_Error();

                                          pullEVSEDataResponse = OICPResult<PullEVSEDataResponse>.Failed(
                                                                     this,
                                                                     new PullEVSEDataResponse(
                                                                         Timestamp.Now,
                                                                         Request.EventTrackingId,
                                                                         processId,
                                                                         Timestamp.Now - Request.Timestamp,
                                                                         [],
                                                                         pullEVSEDataRequest,
                                                                         StatusCode: new StatusCode(
                                                                                         StatusCodes.DataError,
                                                                                         "We could not parse the given PullEVSEData request!",
                                                                                         errorResponse
                                                                                     )
                                                                     )
                                                                 );

                                      }

                                  }
                                  catch (Exception e)
                                  {

                                      Counters.PullEVSEData.IncResponses_Error();

                                      pullEVSEDataResponse = OICPResult<PullEVSEDataResponse>.Failed(
                                                                 this,
                                                                 new PullEVSEDataResponse(
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
                                             Content                    = pullEVSEDataResponse.Response?.ToJSON(CustomPullEVSEDataResponseSerializer,
                                                                                                                CustomEVSEDataRecordSerializer,
                                                                                                                CustomAddressSerializer,
                                                                                                                CustomChargingFacilitySerializer,
                                                                                                                CustomGeoCoordinatesSerializer,
                                                                                                                CustomEnergyMeterSerializer,
                                                                                                                CustomTransparencySoftwareStatusSerializer,
                                                                                                                CustomTransparencySoftwareSerializer,
                                                                                                                CustomEnergySourceSerializer,
                                                                                                                CustomEnvironmentalImpactSerializer,
                                                                                                                CustomOpeningTimesSerializer,
                                                                                                                CustomStatusCodeSerializer).
                                                                                                         ToString(JSONFormatting).
                                                                                                         ToUTF8Bytes()
                                                                                               ?? [],
                                             ProcessID                  = processId.ToString(),
                                             Connection                 = "close"
                                         }.AsImmutable;

                               }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region POST  ~/api/oicp/evsepull/v21/providers/{providerId}/status-records

            // -------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/evsepull/v21/providers/DE-GDF/status-records
            // -------------------------------------------------------------------------------------------------------------------------------------
            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.POST,
                              URLPathPrefix + "/api/oicp/evsepull/v21/providers/{providerId}/status-records",
                              HTTPContentType.Application.JSON_UTF8,
                              HTTPRequestLogger:   logPullEVSEStatusHTTPRequest,
                              HTTPResponseLogger:  logPullEVSEStatusHTTPResponse,
                              HTTPDelegate:        async Request => {

                                  var startTime  = Timestamp.Now;
                                  var processId  = Process_Id.NewRandom();

                                  OICPResult<PullEVSEStatusResponse>? pullEVSEStatusResponse = null;

                                  try
                                  {

                                      #region Try to parse ProviderId URL parameter

                                      if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                      {

                                          Counters.PullEVSEStatus.IncRequests_Error();

                                          pullEVSEStatusResponse = OICPResult<PullEVSEStatusResponse>.Failed(
                                                                       this,
                                                                       new PullEVSEStatusResponse(
                                                                           Timestamp.Now,
                                                                           Request.EventTrackingId,
                                                                           processId,
                                                                           Timestamp.Now - Request.Timestamp,
                                                                           [],
                                                                           StatusCode: new StatusCode(
                                                                                           StatusCodes.SystemError,
                                                                                           "The expected 'providerId' URL parameter could not be parsed!"
                                                                                       )
                                                                       )
                                                                   );

                                      }

                                      #endregion

                                      else if (PullEVSEStatusRequest.TryParse(JObject.Parse(Request.HTTPBody?.ToUTF8String() ?? ""),
                                                                              //     providerId ????
                                                                              out var                             pullEVSEStatusRequest,
                                                                              out var                             errorResponse,
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

                                                  Counters.PullEVSEStatus.IncResponses_Error();

                                                  pullEVSEStatusResponse = OICPResult<PullEVSEStatusResponse>.Failed(
                                                                             this,
                                                                             new PullEVSEStatusResponse(
                                                                                 Timestamp.Now,
                                                                                 Request.EventTrackingId,
                                                                                 processId,
                                                                                 Timestamp.Now - Request.Timestamp,
                                                                                 [],
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

                                          if (pullEVSEStatusResponse is null)
                                          {

                                              Counters.PullEVSEStatus.IncResponses_Error();

                                              pullEVSEStatusResponse = OICPResult<PullEVSEStatusResponse>.Failed(
                                                                           this,
                                                                           new PullEVSEStatusResponse(
                                                                               Timestamp.Now,
                                                                               Request.EventTrackingId,
                                                                               processId,
                                                                               Timestamp.Now - Request.Timestamp,
                                                                               [],
                                                                               pullEVSEStatusRequest,
                                                                               StatusCode: new StatusCode(
                                                                                               StatusCodes.SystemError,
                                                                                               "Could not process the received PullEVSEStatus request!"
                                                                                           )
                                                                           )
                                                                       );

                                          }

                                          #endregion

                                          #region Send OnPullEVSEStatusResponse event

                                          try
                                          {

                                              if (OnPullEVSEStatusResponse is not null)
                                                  await Task.WhenAll(OnPullEVSEStatusResponse.GetInvocationList().
                                                                     Cast<OnPullEVSEStatusAPIResponseDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   pullEVSEStatusRequest!,
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
                                      {

                                          Counters.PullEVSEStatus.IncRequests_Error();

                                          pullEVSEStatusResponse = OICPResult<PullEVSEStatusResponse>.Failed(
                                                                       this,
                                                                       new PullEVSEStatusResponse(
                                                                           Timestamp.Now,
                                                                           Request.EventTrackingId,
                                                                           processId,
                                                                           Timestamp.Now - Request.Timestamp,
                                                                           [],
                                                                           pullEVSEStatusRequest,
                                                                           StatusCode: new StatusCode(
                                                                                           StatusCodes.DataError,
                                                                                           "We could not parse the given PullEVSEStatus request!",
                                                                                           errorResponse
                                                                                       )
                                                                       )
                                                                   );

                                      }

                                  }
                                  catch (Exception e)
                                  {

                                      Counters.PullEVSEStatus.IncResponses_Error();

                                      pullEVSEStatusResponse = OICPResult<PullEVSEStatusResponse>.Failed(
                                                                   this,
                                                                   new PullEVSEStatusResponse(
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
                                             Content                    = pullEVSEStatusResponse.Response?.ToJSON(CustomPullEVSEStatusResponseSerializer,
                                                                                                                  CustomOperatorEVSEStatusSerializer,
                                                                                                                  CustomEVSEStatusRecordSerializer,
                                                                                                                  CustomStatusCodeSerializer).
                                                                                                           ToString(JSONFormatting).
                                                                                                           ToUTF8Bytes()
                                                                                                 ?? [],
                                             ProcessID                  = processId.ToString(),
                                             Connection                 = "close"
                                         }.AsImmutable;

                               }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region POST  ~/api/oicp/evsepull/v21/providers/{providerId}/status-records-by-id

            // -------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/evsepull/v21/providers/DE-GDF/status-records-by-id
            // -------------------------------------------------------------------------------------------------------------------------------------------
            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.POST,
                              URLPathPrefix + "/api/oicp/evsepull/v21/providers/{providerId}/status-records-by-id",
                              HTTPContentType.Application.JSON_UTF8,
                              HTTPRequestLogger:   logPullEVSEStatusByIdHTTPRequest,
                              HTTPResponseLogger:  logPullEVSEStatusByIdHTTPResponse,
                              HTTPDelegate:        async Request => {

                                  var startTime  = Timestamp.Now;
                                  var processId  = Process_Id.NewRandom();

                                  OICPResult<PullEVSEStatusByIdResponse>? pullEVSEStatusByIdResponse = null;

                                  try
                                  {

                                      #region Try to parse ProviderId URL parameter

                                      if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                      {

                                          Counters.PullEVSEStatusById.IncRequests_Error();

                                          pullEVSEStatusByIdResponse = OICPResult<PullEVSEStatusByIdResponse>.Failed(
                                                                       this,
                                                                       new PullEVSEStatusByIdResponse(
                                                                           Timestamp.Now,
                                                                           Request.EventTrackingId,
                                                                           processId,
                                                                           Timestamp.Now - Request.Timestamp,
                                                                           [],
                                                                           StatusCode: new StatusCode(
                                                                                           StatusCodes.SystemError,
                                                                                           "The expected 'providerId' URL parameter could not be parsed!"
                                                                                       )
                                                                       )
                                                                   );

                                      }

                                      #endregion

                                      else if (PullEVSEStatusByIdRequest.TryParse(JObject.Parse(Request.HTTPBody?.ToUTF8String() ?? ""),
                                                                                  //     providerId ????
                                                                                  out var                                 pullEVSEStatusByIdRequest,
                                                                                  out var                                 errorResponse,
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

                                                  Counters.PullEVSEStatusById.IncResponses_Error();

                                                  pullEVSEStatusByIdResponse = OICPResult<PullEVSEStatusByIdResponse>.Failed(
                                                                                   this,
                                                                                   new PullEVSEStatusByIdResponse(
                                                                                       Timestamp.Now,
                                                                                       Request.EventTrackingId,
                                                                                       processId,
                                                                                       Timestamp.Now - Request.Timestamp,
                                                                                       [],
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

                                          if (pullEVSEStatusByIdResponse is null)
                                          {

                                              Counters.PullEVSEStatusById.IncResponses_Error();

                                              pullEVSEStatusByIdResponse = OICPResult<PullEVSEStatusByIdResponse>.Failed(
                                                                               this,
                                                                               new PullEVSEStatusByIdResponse(
                                                                                   Timestamp.Now,
                                                                                   Request.EventTrackingId,
                                                                                   processId,
                                                                                   Timestamp.Now - Request.Timestamp,
                                                                                   [],
                                                                                   pullEVSEStatusByIdRequest,
                                                                                   StatusCode: new StatusCode(
                                                                                                   StatusCodes.SystemError,
                                                                                                   "Could not process the received PullEVSEStatusById request!"
                                                                                               )
                                                                               )
                                                                           );

                                          }

                                          #endregion

                                          #region Send OnPullEVSEStatusByIdResponse event

                                          try
                                          {

                                              if (OnPullEVSEStatusByIdResponse is not null)
                                                  await Task.WhenAll(OnPullEVSEStatusByIdResponse.GetInvocationList().
                                                                     Cast<OnPullEVSEStatusByIdAPIResponseDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   pullEVSEStatusByIdRequest!,
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
                                      {

                                          Counters.PullEVSEStatusById.IncRequests_Error();

                                          pullEVSEStatusByIdResponse = OICPResult<PullEVSEStatusByIdResponse>.Failed(
                                                                           this,
                                                                           new PullEVSEStatusByIdResponse(
                                                                               Timestamp.Now,
                                                                               Request.EventTrackingId,
                                                                               processId,
                                                                               Timestamp.Now - Request.Timestamp,
                                                                               [],
                                                                               pullEVSEStatusByIdRequest,
                                                                               StatusCode: new StatusCode(
                                                                                               StatusCodes.DataError,
                                                                                               "We could not parse the given PullEVSEStatusById request!",
                                                                                               errorResponse
                                                                                           )
                                                                           )
                                                                       );

                                      }

                                  }
                                  catch (Exception e)
                                  {

                                      Counters.PullEVSEStatusById.IncResponses_Error();

                                      pullEVSEStatusByIdResponse = OICPResult<PullEVSEStatusByIdResponse>.Failed(
                                                                       this,
                                                                       new PullEVSEStatusByIdResponse(
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
                                             Content                    = pullEVSEStatusByIdResponse.Response?.ToJSON(CustomPullEVSEStatusByIdResponseSerializer,
                                                                                                                      CustomEVSEStatusRecordSerializer,
                                                                                                                      CustomStatusCodeSerializer).
                                                                                                               ToString(JSONFormatting).
                                                                                                               ToUTF8Bytes()
                                                                                                     ?? [],
                                             ProcessID                  = processId.ToString(),
                                             Connection                 = "close"
                                         }.AsImmutable;

                               }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region POST  ~/api/oicp/evsepull/v21/providers/{providerId}/status-records-by-operator-id

            // ----------------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/evsepull/v21/providers/DE-GDF/status-records-by-operator-id
            // ----------------------------------------------------------------------------------------------------------------------------------------------------
            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.POST,
                              URLPathPrefix + "/api/oicp/evsepull/v21/providers/{providerId}/status-records-by-operator-id",
                              HTTPContentType.Application.JSON_UTF8,
                              HTTPRequestLogger:   logPullEVSEStatusByOperatorIdHTTPRequest,
                              HTTPResponseLogger:  logPullEVSEStatusByOperatorIdHTTPResponse,
                              HTTPDelegate:        async Request => {

                                  var startTime  = Timestamp.Now;
                                  var processId  = Process_Id.NewRandom();

                                  OICPResult<PullEVSEStatusByOperatorIdResponse>? pullEVSEStatusByOperatorIdResponse = null;

                                  try
                                  {

                                      #region Try to parse ProviderId URL parameter

                                      if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                      {

                                          Counters.PullEVSEStatusByOperatorId.IncRequests_Error();

                                          pullEVSEStatusByOperatorIdResponse = OICPResult<PullEVSEStatusByOperatorIdResponse>.Failed(
                                                                                   this,
                                                                                   new PullEVSEStatusByOperatorIdResponse(
                                                                                       Timestamp.Now,
                                                                                       Request.EventTrackingId,
                                                                                       Process_Id.NewRandom(),
                                                                                       Timestamp.Now - Request.Timestamp,
                                                                                       [],
                                                                                       StatusCode: new StatusCode(
                                                                                                       StatusCodes.SystemError,
                                                                                                       "The expected 'providerId' URL parameter could not be parsed!"
                                                                                                   )
                                                                                   )
                                                                               );

                                      }

                                      #endregion

                                      else if (PullEVSEStatusByOperatorIdRequest.TryParse(JObject.Parse(Request.HTTPBody?.ToUTF8String() ?? ""),
                                                                                          //     providerId ????
                                                                                          out var                                         pullEVSEStatusByOperatorIdRequest,
                                                                                          out var                                         errorResponse,
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

                                                  Counters.PullEVSEStatusByOperatorId.IncResponses_Error();

                                                  pullEVSEStatusByOperatorIdResponse = OICPResult<PullEVSEStatusByOperatorIdResponse>.Failed(
                                                                                           this,
                                                                                           new PullEVSEStatusByOperatorIdResponse(
                                                                                               Timestamp.Now,
                                                                                               Request.EventTrackingId,
                                                                                               processId,
                                                                                               Timestamp.Now - Request.Timestamp,
                                                                                               [],
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

                                          if (pullEVSEStatusByOperatorIdResponse is null)
                                          {

                                              Counters.PullEVSEStatusByOperatorId.IncResponses_Error();

                                              pullEVSEStatusByOperatorIdResponse = OICPResult<PullEVSEStatusByOperatorIdResponse>.Failed(
                                                                                       this,
                                                                                       new PullEVSEStatusByOperatorIdResponse(
                                                                                           Timestamp.Now,
                                                                                           Request.EventTrackingId,
                                                                                           processId,
                                                                                           Timestamp.Now - Request.Timestamp,
                                                                                           [],
                                                                                           pullEVSEStatusByOperatorIdRequest,
                                                                                           StatusCode: new StatusCode(
                                                                                                           StatusCodes.SystemError,
                                                                                                           "Could not process the received PullEVSEStatusByOperatorId request!"
                                                                                                       )
                                                                                       )
                                                                                   );

                                          }

                                          #endregion

                                          #region Send OnPullEVSEStatusByOperatorIdResponse event

                                          try
                                          {

                                              if (OnPullEVSEStatusByOperatorIdResponse is not null)
                                                  await Task.WhenAll(OnPullEVSEStatusByOperatorIdResponse.GetInvocationList().
                                                                     Cast<OnPullEVSEStatusByOperatorIdAPIResponseDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   pullEVSEStatusByOperatorIdRequest!,
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
                                      {

                                          Counters.PullEVSEStatusByOperatorId.IncRequests_Error();

                                          pullEVSEStatusByOperatorIdResponse = OICPResult<PullEVSEStatusByOperatorIdResponse>.Failed(
                                                                                   this,
                                                                                   new PullEVSEStatusByOperatorIdResponse(
                                                                                       Timestamp.Now,
                                                                                       Request.EventTrackingId,
                                                                                       processId,
                                                                                       Timestamp.Now - Request.Timestamp,
                                                                                       [],
                                                                                       pullEVSEStatusByOperatorIdRequest,
                                                                                       StatusCode: new StatusCode(
                                                                                                       StatusCodes.DataError,
                                                                                                       "We could not parse the given PullEVSEStatusByOperatorId request!",
                                                                                                       errorResponse
                                                                                                   )
                                                                                   )
                                                                               );

                                      }

                                  }
                                  catch (Exception e)
                                  {

                                      Counters.PullEVSEStatusByOperatorId.IncResponses_Error();

                                      pullEVSEStatusByOperatorIdResponse = OICPResult<PullEVSEStatusByOperatorIdResponse>.Failed(
                                                                               this,
                                                                               new PullEVSEStatusByOperatorIdResponse(
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
                                             Content                    = pullEVSEStatusByOperatorIdResponse.Response?.ToJSON(CustomPullEVSEStatusByOperatorIdResponseSerializer,
                                                                                                                              CustomOperatorEVSEStatusSerializer,
                                                                                                                              CustomEVSEStatusRecordSerializer,
                                                                                                                              CustomStatusCodeSerializer).
                                                                                                                       ToString(JSONFormatting).
                                                                                                                       ToUTF8Bytes()
                                                                                                             ?? [],
                                             ProcessID                  = processId.ToString(),
                                             Connection                 = "close"
                                         }.AsImmutable;

                               }, AllowReplacement: URLReplacement.Allow);

            #endregion


            #region POST  ~/api/oicp/dynamicpricing/v10/providers/{providerId}/pricing-products

            // ---------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/dynamicpricing/v10/providers/DE-GDF/pricing-products
            // ---------------------------------------------------------------------------------------------------------------------------------------------
            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.POST,
                              URLPathPrefix + "/api/oicp/dynamicpricing/v10/providers/{providerId}/pricing-products",
                              HTTPContentType.Application.JSON_UTF8,
                              HTTPRequestLogger:   logPullPricingProductDataHTTPRequest,
                              HTTPResponseLogger:  logPullPricingProductDataHTTPResponse,
                              HTTPDelegate:        async Request => {

                                  var startTime  = Timestamp.Now;
                                  var page       = Request.QueryString.GetUInt32 ("page");
                                  var size       = Request.QueryString.GetUInt32 ("size");
                                  var sortOrder  = Request.QueryString.GetStrings("sortOrder");
                                  var processId  = Process_Id.NewRandom();

                                  OICPResult<PullPricingProductDataResponse>? pullPricingProductDataResponse = null;

                                  try
                                  {

                                      #region Try to parse ProviderId URL parameter

                                      if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                      {

                                          Counters.PullPricingProductData.IncRequests_Error();

                                          pullPricingProductDataResponse = OICPResult<PullPricingProductDataResponse>.Failed(
                                                                               this,
                                                                               new PullPricingProductDataResponse(
                                                                                   Timestamp.Now,
                                                                                   Request.EventTrackingId,
                                                                                   processId,
                                                                                   Timestamp.Now - Request.Timestamp,
                                                                                   [],
                                                                                   StatusCode: new StatusCode(
                                                                                                   StatusCodes.SystemError,
                                                                                                   "The expected 'providerId' URL parameter could not be parsed!"
                                                                                               )
                                                                               )
                                                                           );

                                      }

                                      #endregion

                                      else if (PullPricingProductDataRequest.TryParse(JObject.Parse(Request.HTTPBody?.ToUTF8String() ?? ""),
                                                                                      providerId,
                                                                                      out var                                     pullPricingProductDataRequest,
                                                                                      out var                                     errorResponse,
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
                                                                                   pullPricingProductDataRequest!))).
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
                                                                                                                                                   pullPricingProductDataRequest!))))?.FirstOrDefault();

                                                  Counters.PullPricingProductData.IncResponses_OK();

                                              }
                                              catch (Exception e)
                                              {

                                                  Counters.PullPricingProductData.IncResponses_Error();

                                                  pullPricingProductDataResponse = OICPResult<PullPricingProductDataResponse>.Failed(
                                                                                       this,
                                                                                       new PullPricingProductDataResponse(
                                                                                           Timestamp.Now,
                                                                                           Request.EventTrackingId,
                                                                                           processId,
                                                                                           Timestamp.Now - Request.Timestamp,
                                                                                           [],
                                                                                           pullPricingProductDataRequest,
                                                                                           StatusCode: new StatusCode(
                                                                                                           StatusCodes.DataError,
                                                                                                           e.Message,
                                                                                                           e.StackTrace
                                                                                                       )
                                                                                       )
                                                                                   );

                                              }
                                          }

                                          if (pullPricingProductDataResponse is null)
                                          {

                                              Counters.PullPricingProductData.IncResponses_Error();

                                              pullPricingProductDataResponse = OICPResult<PullPricingProductDataResponse>.Failed(
                                                                                   this,
                                                                                   new PullPricingProductDataResponse(
                                                                                       Timestamp.Now,
                                                                                       Request.EventTrackingId,
                                                                                       processId,
                                                                                       Timestamp.Now - Request.Timestamp,
                                                                                       [],
                                                                                       pullPricingProductDataRequest,
                                                                                       StatusCode: new StatusCode(
                                                                                                       StatusCodes.SystemError,
                                                                                                       "Could not process the received PullPricingProductData request!"
                                                                                                   )
                                                                                   )
                                                                               );

                                          }

                                          #endregion

                                          #region Send OnPullPricingProductDataResponse event

                                          try
                                          {

                                              if (OnPullPricingProductDataResponse is not null)
                                                  await Task.WhenAll(OnPullPricingProductDataResponse.GetInvocationList().
                                                                     Cast<OnPullPricingProductDataAPIResponseDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   pullPricingProductDataRequest!,
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
                                      {

                                          Counters.PullPricingProductData.IncRequests_Error();

                                          pullPricingProductDataResponse = OICPResult<PullPricingProductDataResponse>.Failed(
                                                                               this,
                                                                               new PullPricingProductDataResponse(
                                                                                   Timestamp.Now,
                                                                                   Request.EventTrackingId,
                                                                                   processId,
                                                                                   Timestamp.Now - Request.Timestamp,
                                                                                   [],
                                                                                   pullPricingProductDataRequest,
                                                                                   StatusCode: new StatusCode(
                                                                                                   StatusCodes.DataError,
                                                                                                   "We could not parse the given PullPricingProductData request!",
                                                                                                   errorResponse
                                                                                               )
                                                                               )
                                                                           );

                                      }

                                  }
                                  catch (Exception e)
                                  {

                                      Counters.PullPricingProductData.IncResponses_Error();

                                      pullPricingProductDataResponse = OICPResult<PullPricingProductDataResponse>.Failed(
                                                                           this,
                                                                           new PullPricingProductDataResponse(
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
                                             Content                    = pullPricingProductDataResponse.Response?.ToJSON(CustomPullPricingProductDataResponseSerializer,
                                                                                                                          CustomPricingProductDataSerializer,
                                                                                                                          CustomPricingProductDataRecordSerializer,
                                                                                                                          CustomStatusCodeSerializer).
                                                                                                                   ToString(JSONFormatting).
                                                                                                                   ToUTF8Bytes()
                                                                                                         ?? [],
                                             ProcessID                  = processId.ToString(),
                                             Connection                 = "close"
                                         }.AsImmutable;

                               }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region POST  ~/api/oicp/dynamicpricing/v10/providers/{providerId}/evse-pricing

            // -----------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/dynamicpricing/v10/providers/DE-GDF/evse-pricing
            // -----------------------------------------------------------------------------------------------------------------------------------------
            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.POST,
                              URLPathPrefix + "/api/oicp/dynamicpricing/v10/providers/{providerId}/evse-pricing",
                              HTTPContentType.Application.JSON_UTF8,
                              HTTPRequestLogger:   logPullEVSEPricingHTTPRequest,
                              HTTPResponseLogger:  logPullEVSEPricingHTTPResponse,
                              HTTPDelegate:        async Request => {

                                  var startTime  = Timestamp.Now;
                                  var page       = Request.QueryString.GetUInt32 ("page");
                                  var size       = Request.QueryString.GetUInt32 ("size");
                                  var sortOrder  = Request.QueryString.GetStrings("sortOrder");
                                  var processId  = Process_Id.NewRandom();

                                  OICPResult<PullEVSEPricingResponse>? pullEVSEPricingResponse = null;

                                  try
                                  {

                                      #region Try to parse ProviderId URL parameter

                                      if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                      {

                                          Counters.PullEVSEPricing.IncRequests_Error();

                                          pullEVSEPricingResponse = OICPResult<PullEVSEPricingResponse>.Failed(
                                                                        this,
                                                                        new PullEVSEPricingResponse(
                                                                            Timestamp.Now,
                                                                            Request.EventTrackingId,
                                                                            processId,
                                                                            Timestamp.Now - Request.Timestamp,
                                                                            [],
                                                                            StatusCode: new StatusCode(
                                                                                            StatusCodes.SystemError,
                                                                                            "The expected 'providerId' URL parameter could not be parsed!"
                                                                                        )
                                                                        )
                                                                    );

                                      }

                                      #endregion

                                      else if (PullEVSEPricingRequest.TryParse(JObject.Parse(Request.HTTPBody?.ToUTF8String() ?? ""),
                                                                               //     providerId ????
                                                                               out var                              pullEVSEPricingRequest,
                                                                               out var                              errorResponse,
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
                                                                                   pullEVSEPricingRequest!))).
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
                                                                                                                                     pullEVSEPricingRequest!))))?.FirstOrDefault();

                                                  Counters.PullEVSEPricing.IncResponses_OK();

                                              }
                                              catch (Exception e)
                                              {

                                                  Counters.PullEVSEPricing.IncResponses_Error();

                                                  pullEVSEPricingResponse = OICPResult<PullEVSEPricingResponse>.Failed(
                                                                                this,
                                                                                new PullEVSEPricingResponse(
                                                                                    Timestamp.Now,
                                                                                    Request.EventTrackingId,
                                                                                    processId,
                                                                                    Timestamp.Now - Request.Timestamp,
                                                                                    [],
                                                                                    pullEVSEPricingRequest,
                                                                                    StatusCode: new StatusCode(
                                                                                                    StatusCodes.DataError,
                                                                                                    e.Message,
                                                                                                    e.StackTrace
                                                                                                )
                                                                                )
                                                                            );

                                              }
                                          }

                                          if (pullEVSEPricingResponse is null)
                                          {

                                              Counters.PullEVSEPricing.IncResponses_Error();

                                              pullEVSEPricingResponse = OICPResult<PullEVSEPricingResponse>.Failed(
                                                                            this,
                                                                            new PullEVSEPricingResponse(
                                                                                Timestamp.Now,
                                                                                Request.EventTrackingId,
                                                                                processId,
                                                                                Timestamp.Now - Request.Timestamp,
                                                                                [],
                                                                                pullEVSEPricingRequest,
                                                                                StatusCode: new StatusCode(
                                                                                                StatusCodes.SystemError,
                                                                                                "Could not process the received PullEVSEPricing request!"
                                                                                            )
                                                                            )
                                                                        );

                                          }

                                          #endregion

                                          #region Send OnPullEVSEPricingResponse event

                                          try
                                          {

                                              if (OnPullEVSEPricingResponse is not null)
                                                  await Task.WhenAll(OnPullEVSEPricingResponse.GetInvocationList().
                                                                     Cast<OnPullEVSEPricingAPIResponseDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   pullEVSEPricingRequest!,
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
                                      {

                                          Counters.PullEVSEPricing.IncRequests_Error();

                                          pullEVSEPricingResponse = OICPResult<PullEVSEPricingResponse>.Failed(
                                                                        this,
                                                                        new PullEVSEPricingResponse(
                                                                            Timestamp.Now,
                                                                            Request.EventTrackingId,
                                                                            processId,
                                                                            Timestamp.Now - Request.Timestamp,
                                                                            [],
                                                                            pullEVSEPricingRequest,
                                                                            StatusCode: new StatusCode(
                                                                                            StatusCodes.DataError,
                                                                                            "We could not parse the given PullEVSEPricing request!",
                                                                                            errorResponse
                                                                                        )
                                                                        )
                                                                    );

                                      }

                                  }
                                  catch (Exception e)
                                  {

                                      Counters.PullEVSEPricing.IncResponses_Error();

                                      pullEVSEPricingResponse = OICPResult<PullEVSEPricingResponse>.Failed(
                                                                    this,
                                                                    new PullEVSEPricingResponse(
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
                                             Content                    = pullEVSEPricingResponse.Response?.ToJSON(CustomPullEVSEPricingResponseSerializer,
                                                                                                                   CustomOperatorEVSEPricingSerializer,
                                                                                                                   CustomEVSEPricingSerializer,
                                                                                                                   CustomStatusCodeSerializer).
                                                                                                            ToString(JSONFormatting).
                                                                                                            ToUTF8Bytes()
                                                                                                  ?? [],
                                             ProcessID                  = processId.ToString(),
                                             Connection                 = "close"
                                         }.AsImmutable;

                               }, AllowReplacement: URLReplacement.Allow);

            #endregion


            #region POST  ~/api/oicp/authdata/v21/providers/{providerId}/push-request

            // -----------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/evsepull/v23/providers/DE-GDF/push-request
            // -----------------------------------------------------------------------------------------------------------------------------------
            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.POST,
                              URLPathPrefix + "/api/oicp/authdata/v21/providers/{providerId}/push-request",
                              HTTPContentType.Application.JSON_UTF8,
                              HTTPRequestLogger:   logPushAuthenticationDataHTTPRequest,
                              HTTPResponseLogger:  logPushAuthenticationDataHTTPResponse,
                              HTTPDelegate:        async Request => {

                                  var startTime  = Timestamp.Now;
                                  var processId  = Process_Id.NewRandom();

                                  OICPResult<Acknowledgement<PushAuthenticationDataRequest>>? pushAuthenticationDataResponse = null;

                                  try
                                  {

                                      #region Try to parse ProviderId URL parameter

                                      if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                      {

                                          Counters.PushAuthenticationData.IncRequests_Error();

                                          pushAuthenticationDataResponse = OICPResult<Acknowledgement<PushAuthenticationDataRequest>>.Failed(
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

                                      }

                                      #endregion

                                      else if (PushAuthenticationDataRequest.TryParse(JObject.Parse(Request.HTTPBody?.ToUTF8String() ?? ""),
                                                                                      //     providerId ????
                                                                                      out var                                     pushAuthenticationDataRequest,
                                                                                      out var                                     errorResponse,

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
                                                                                   pushAuthenticationDataRequest!))).
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

                                                  pushAuthenticationDataResponse = (await Task.WhenAll(OnPushAuthenticationDataLocal.GetInvocationList().
                                                                                                                                     Cast<OnPushAuthenticationDataAPIDelegate>().
                                                                                                                                     Select(e => e(Timestamp.Now,
                                                                                                                                                   this,
                                                                                                                                                   pushAuthenticationDataRequest!))))?.FirstOrDefault();

                                                  Counters.PushAuthenticationData.IncResponses_OK();

                                              }
                                              catch (Exception e)
                                              {

                                                  Counters.PushAuthenticationData.IncResponses_Error();

                                                  pushAuthenticationDataResponse = OICPResult<Acknowledgement<PushAuthenticationDataRequest>>.Failed(
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

                                          if (pushAuthenticationDataResponse is null)
                                          {

                                              pushAuthenticationDataResponse = OICPResult<Acknowledgement<PushAuthenticationDataRequest>>.Failed(
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

                                          }

                                          #endregion

                                          #region Send OnPushAuthenticationDataResponse event

                                          try
                                          {

                                              if (OnPushAuthenticationDataResponse is not null)
                                                  await Task.WhenAll(OnPushAuthenticationDataResponse.GetInvocationList().
                                                                     Cast<OnPushAuthenticationDataAPIResponseDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   pushAuthenticationDataRequest!,
                                                                                   pushAuthenticationDataResponse,
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
                                      {

                                          Counters.PushAuthenticationData.IncRequests_Error();

                                          pushAuthenticationDataResponse = OICPResult<Acknowledgement<PushAuthenticationDataRequest>>.Failed(
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

                                  }
                                  catch (Exception e)
                                  {

                                      Counters.PushAuthenticationData.IncResponses_Error();

                                      pushAuthenticationDataResponse = OICPResult<Acknowledgement<PushAuthenticationDataRequest>>.Failed(
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
                                             AccessControlAllowMethods  = [ "POST" ],
                                             AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                             ContentType                = HTTPContentType.Application.JSON_UTF8,
                                             Content                    = pushAuthenticationDataResponse.Response?.ToJSON(CustomAcknowledgementSerializer,
                                                                                                                          CustomStatusCodeSerializer).
                                                                                                                   ToString(JSONFormatting).
                                                                                                                   ToUTF8Bytes()
                                                                                                         ?? [],
                                             ProcessID                  = processId.ToString(),
                                             Connection                 = "close"
                                         }.AsImmutable;

                               }, AllowReplacement: URLReplacement.Allow);

            #endregion


            #region POST  ~/api/oicp/charging/v21/providers/{providerId}/authorize-remote-reservation/start

            // ---------------------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/charging/v21/providers/DE-GDF/authorize-remote-reservation/start
            // ---------------------------------------------------------------------------------------------------------------------------------------------------------
            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.POST,
                              URLPathPrefix + "/api/oicp/charging/v21/providers/{providerId}/authorize-remote-reservation/start",
                              HTTPContentType.Application.JSON_UTF8,
                              HTTPRequestLogger:   logAuthorizeRemoteReservationStartHTTPRequest,
                              HTTPResponseLogger:  logAuthorizeRemoteReservationStartHTTPResponse,
                              HTTPDelegate:        async Request => {

                                  var startTime  = Timestamp.Now;
                                  var processId  = Process_Id.NewRandom();

                                  OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>? authorizeRemoteReservationStartResponse = null;

                                  try
                                  {

                                      #region Try to parse ProviderId URL parameter

                                      if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                      {

                                          Counters.AuthorizeRemoteReservationStart.IncRequests_Error();

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

                                      }

                                      #endregion

                                      else if (AuthorizeRemoteReservationStartRequest.TryParse(JObject.Parse(Request.HTTPBody?.ToUTF8String() ?? ""),
                                                                                               providerId,
                                                                                               out var                                              authorizeRemoteReservationStartRequest,
                                                                                               out var                                              errorResponse,
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
                                                                                   authorizeRemoteReservationStartRequest!))).
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
                                                                                                                                                                     authorizeRemoteReservationStartRequest!))))?.FirstOrDefault();

                                                  Counters.AuthorizeRemoteReservationStart.IncResponses_OK();

                                              }
                                              catch (Exception e)
                                              {

                                                  Counters.AuthorizeRemoteReservationStart.IncResponses_Error();

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

                                          if (authorizeRemoteReservationStartResponse is null)
                                          {

                                              Counters.AuthorizeRemoteReservationStart.IncResponses_Error();

                                              authorizeRemoteReservationStartResponse = OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>.Failed(
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

                                          }

                                          #endregion

                                          #region Send OnAuthorizeRemoteReservationStartResponse event

                                          try
                                          {

                                              if (OnAuthorizeRemoteReservationStartResponse is not null)
                                                  await Task.WhenAll(OnAuthorizeRemoteReservationStartResponse.GetInvocationList().
                                                                     Cast<OnAuthorizeRemoteReservationStartAPIResponseDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   authorizeRemoteReservationStartRequest!,
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
                                      {

                                          Counters.AuthorizeRemoteReservationStart.IncRequests_Error();

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

                                  }
                                  catch (Exception e)
                                  {

                                      Counters.AuthorizeRemoteReservationStart.IncResponses_Error();

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
                                             AccessControlAllowMethods  = [ "POST" ],
                                             AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                             ContentType                = HTTPContentType.Application.JSON_UTF8,
                                             Content                    = authorizeRemoteReservationStartResponse.Response?.ToJSON(CustomAcknowledgementSerializer,
                                                                                                                                   CustomStatusCodeSerializer).
                                                                                                                            ToString(JSONFormatting).
                                                                                                                            ToUTF8Bytes()
                                                                                                                  ?? [],
                                             ProcessID                  = processId.ToString(),
                                             Connection                 = "close"
                                         }.AsImmutable;

                               }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region POST  ~/api/oicp/charging/v21/providers/{providerId}/authorize-remote-reservation/stop

            // --------------------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/charging/v21/providers/DE-GDF/authorize-remote-reservation/stop
            // --------------------------------------------------------------------------------------------------------------------------------------------------------
            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.POST,
                              URLPathPrefix + "/api/oicp/charging/v21/providers/{providerId}/authorize-remote-reservation/stop",
                              HTTPContentType.Application.JSON_UTF8,
                              HTTPRequestLogger:   logAuthorizeRemoteReservationStopHTTPRequest,
                              HTTPResponseLogger:  logAuthorizeRemoteReservationStopHTTPResponse,
                              HTTPDelegate:        async Request => {

                                  var startTime  = Timestamp.Now;
                                  var processId  = Process_Id.NewRandom();

                                  OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>? authorizeRemoteReservationStopResponse = null;

                                  try
                                  {

                                      #region Try to parse ProviderId URL parameter

                                      if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                      {

                                          Counters.AuthorizeRemoteReservationStop.IncRequests_Error();

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

                                      }

                                      #endregion

                                      else if (AuthorizeRemoteReservationStopRequest.TryParse(JObject.Parse(Request.HTTPBody?.ToUTF8String() ?? ""),
                                                                                              providerId,
                                                                                              out var                                             authorizeRemoteReservationStopRequest,
                                                                                              out var                                             errorResponse,
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
                                                                                   authorizeRemoteReservationStopRequest!))).
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
                                                                                                                                                                   authorizeRemoteReservationStopRequest!))))?.FirstOrDefault();

                                                  Counters.AuthorizeRemoteReservationStop.IncResponses_OK();

                                              }
                                              catch (Exception e)
                                              {

                                                  Counters.AuthorizeRemoteReservationStop.IncResponses_Error();

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

                                          if (authorizeRemoteReservationStopResponse is null)
                                          {

                                              Counters.AuthorizeRemoteReservationStop.IncResponses_Error();

                                              authorizeRemoteReservationStopResponse = OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>.Failed(
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

                                          }

                                          #endregion

                                          #region Send OnAuthorizeRemoteReservationStopResponse event

                                          try
                                          {

                                              if (OnAuthorizeRemoteReservationStopResponse is not null)
                                                  await Task.WhenAll(OnAuthorizeRemoteReservationStopResponse.GetInvocationList().
                                                                     Cast<OnAuthorizeRemoteReservationStopAPIResponseDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   authorizeRemoteReservationStopRequest!,
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
                                      {

                                          Counters.AuthorizeRemoteReservationStop.IncRequests_Error();

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

                                  }
                                  catch (Exception e)
                                  {

                                      Counters.AuthorizeRemoteReservationStop.IncResponses_Error();

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
                                             AccessControlAllowMethods  = [ "POST" ],
                                             AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                             ContentType                = HTTPContentType.Application.JSON_UTF8,
                                             Content                    = authorizeRemoteReservationStopResponse.Response?.ToJSON(CustomAcknowledgementSerializer,
                                                                                                                                  CustomStatusCodeSerializer).
                                                                                                                           ToString(JSONFormatting).
                                                                                                                           ToUTF8Bytes()
                                                                                                                 ?? [],
                                             ProcessID                  = processId.ToString(),
                                             Connection                 = "close"
                                         }.AsImmutable;

                               }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region POST  ~/api/oicp/charging/v21/providers/{providerId}/authorize-remote/start

            // ---------------------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/charging/v21/providers/DE-GDF/authorize-remote/start
            // ---------------------------------------------------------------------------------------------------------------------------------------------------------
            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.POST,
                              URLPathPrefix + "/api/oicp/charging/v21/providers/{providerId}/authorize-remote/start",
                              HTTPContentType.Application.JSON_UTF8,
                              HTTPRequestLogger:   logAuthorizeRemoteStartHTTPRequest,
                              HTTPResponseLogger:  logAuthorizeRemoteStartHTTPResponse,
                              HTTPDelegate:        async Request => {

                                  var startTime  = Timestamp.Now;
                                  var processId  = Process_Id.NewRandom();

                                  OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>? authorizeRemoteStartResponse = null;

                                  try
                                  {

                                      #region Try to parse ProviderId URL parameter

                                      if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                      {

                                          Counters.AuthorizeRemoteStart.IncRequests_Error();

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

                                      }

                                      #endregion

                                      else if (AuthorizeRemoteStartRequest.TryParse(JObject.Parse(Request.HTTPBody?.ToUTF8String() ?? ""),
                                                                                    providerId,
                                                                                    out var                                   authorizeRemoteStartRequest,
                                                                                    out var                                   errorResponse,
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
                                                                                   authorizeRemoteStartRequest!))).
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
                                                                                                                                               authorizeRemoteStartRequest!))))?.FirstOrDefault();

                                                  Counters.AuthorizeRemoteStart.IncResponses_OK();

                                              }
                                              catch (Exception e)
                                              {

                                                  Counters.AuthorizeRemoteStart.IncResponses_Error();

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

                                          if (authorizeRemoteStartResponse is null)
                                          {

                                              Counters.AuthorizeRemoteStart.IncResponses_Error();

                                              authorizeRemoteStartResponse = OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>.Failed(
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

                                          }

                                          #endregion

                                          #region Send OnAuthorizeRemoteStartResponse event

                                          try
                                          {

                                              if (OnAuthorizeRemoteStartResponse is not null)
                                                  await Task.WhenAll(OnAuthorizeRemoteStartResponse.GetInvocationList().
                                                                     Cast<OnAuthorizeRemoteStartAPIResponseDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   authorizeRemoteStartRequest!,
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
                                      {

                                          Counters.AuthorizeRemoteStart.IncRequests_Error();

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

                                  }
                                  catch (Exception e)
                                  {

                                      Counters.AuthorizeRemoteStart.IncResponses_Error();

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
                                             AccessControlAllowMethods  = [ "POST" ],
                                             AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                             ContentType                = HTTPContentType.Application.JSON_UTF8,
                                             Content                    = authorizeRemoteStartResponse.Response?.ToJSON(CustomAcknowledgementSerializer,
                                                                                                                        CustomStatusCodeSerializer).
                                                                                                                 ToString(JSONFormatting).
                                                                                                                 ToUTF8Bytes()
                                                                                                       ?? [],
                                             ProcessID                  = processId.ToString(),
                                             Connection                 = "close"
                                         }.AsImmutable;

                               }, AllowReplacement: URLReplacement.Allow);

            #endregion

            #region POST  ~/api/oicp/charging/v21/providers/{providerId}/authorize-remote/stop

            // --------------------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/charging/v21/providers/DE-GDF/authorize-remote/stop
            // --------------------------------------------------------------------------------------------------------------------------------------------------------
            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.POST,
                              URLPathPrefix + "/api/oicp/charging/v21/providers/{providerId}/authorize-remote/stop",
                              HTTPContentType.Application.JSON_UTF8,
                              HTTPRequestLogger:   logAuthorizeRemoteStopHTTPRequest,
                              HTTPResponseLogger:  logAuthorizeRemoteStopHTTPResponse,
                              HTTPDelegate:        async Request => {

                                  var startTime  = Timestamp.Now;
                                  var processId  = Process_Id.NewRandom();

                                  OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>? authorizeRemoteStopResponse = null;

                                  try
                                  {

                                      #region Try to parse ProviderId URL parameter

                                      if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                      {

                                          Counters.AuthorizeRemoteStop.IncRequests_Error();

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

                                      }

                                      #endregion

                                      else if (AuthorizeRemoteStopRequest.TryParse(JObject.Parse(Request.HTTPBody?.ToUTF8String() ?? ""),
                                                                                   providerId,
                                                                                   out var                                  authorizeRemoteStopRequest,
                                                                                   out var                                  errorResponse,
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
                                                                                   authorizeRemoteStopRequest!))).
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
                                                                                                                                             authorizeRemoteStopRequest!))))?.FirstOrDefault();

                                                  Counters.AuthorizeRemoteStop.IncResponses_OK();

                                              }
                                              catch (Exception e)
                                              {

                                                  Counters.AuthorizeRemoteStop.IncResponses_Error();

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

                                          if (authorizeRemoteStopResponse is null)
                                          {

                                              Counters.AuthorizeRemoteStop.IncResponses_Error();

                                              authorizeRemoteStopResponse = OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>.Failed(
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

                                          }

                                          #endregion

                                          #region Send OnAuthorizeRemoteStopResponse event

                                          try
                                          {

                                              if (OnAuthorizeRemoteStopResponse is not null)
                                                  await Task.WhenAll(OnAuthorizeRemoteStopResponse.GetInvocationList().
                                                                     Cast<OnAuthorizeRemoteStopAPIResponseDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   authorizeRemoteStopRequest!,
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
                                      {

                                          Counters.AuthorizeRemoteStop.IncRequests_Error();

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

                                  }
                                  catch (Exception e)
                                  {

                                      Counters.AuthorizeRemoteStop.IncResponses_Error();

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
                                             AccessControlAllowMethods  = [ "POST" ],
                                             AccessControlAllowHeaders  = [ "Content-Type", "Accept", "Authorization" ],
                                             ContentType                = HTTPContentType.Application.JSON_UTF8,
                                             Content                    = authorizeRemoteStopResponse.Response?.ToJSON(CustomAcknowledgementSerializer,
                                                                                                                       CustomStatusCodeSerializer).
                                                                                                                ToString(JSONFormatting).
                                                                                                                ToUTF8Bytes()
                                                                                                      ?? [],
                                             ProcessID                  = processId.ToString(),
                                             Connection                 = "close"
                                         }.AsImmutable;

                               }, AllowReplacement: URLReplacement.Allow);

            #endregion


            #region POST  ~/api/oicp/cdrmgmt/v22/providers/{providerId}/get-charge-detail-records-request

            // -------------------------------------------------------------------------------------------------------------------------------------------------------
            // curl -v -X POST -H "Accept: application/json" -d "test" http://127.0.0.1:3002/api/oicp/cdrmgmt/v22/providers/DE-GDF/get-charge-detail-records-request
            // -------------------------------------------------------------------------------------------------------------------------------------------------------
            AddMethodCallback(HTTPHostname.Any,
                              HTTPMethod.POST,
                              URLPathPrefix + "/api/oicp/cdrmgmt/v22/providers/{providerId}/get-charge-detail-records-request",
                              HTTPContentType.Application.JSON_UTF8,
                              HTTPRequestLogger:   logGetChargeDetailRecordsHTTPRequest,
                              HTTPResponseLogger:  logGetChargeDetailRecordsHTTPResponse,
                              HTTPDelegate:        async Request => {

                                  var startTime  = Timestamp.Now;
                                  var page       = Request.QueryString.GetUInt32 ("page");
                                  var size       = Request.QueryString.GetUInt32 ("size");
                                  var sortOrder  = Request.QueryString.GetStrings("sortOrder");
                                  var processId  = Process_Id.NewRandom();

                                  OICPResult<GetChargeDetailRecordsResponse>? getChargeDetailRecordsResponse = null;

                                  try
                                  {

                                      #region Try to parse ProviderId URL parameter

                                      if (Request.ParsedURLParameters.Length != 1 || !Provider_Id.TryParse(HTTPTools.URLDecode(Request.ParsedURLParameters[0]), out Provider_Id providerId))
                                      {

                                          Counters.GetChargeDetailRecords.IncRequests_Error();

                                          getChargeDetailRecordsResponse = OICPResult<GetChargeDetailRecordsResponse>.Failed(
                                                                               this,
                                                                               new GetChargeDetailRecordsResponse(
                                                                                   Timestamp.Now,
                                                                                   Request.EventTrackingId,
                                                                                   processId,
                                                                                   Timestamp.Now - Request.Timestamp,
                                                                                   [],
                                                                                   StatusCode: new StatusCode(
                                                                                                   StatusCodes.SystemError,
                                                                                                   "The expected 'providerId' URL parameter could not be parsed!"
                                                                                               )
                                                                               )
                                                                           );

                                      }

                                      #endregion

                                      else if (GetChargeDetailRecordsRequest.TryParse(JObject.Parse(Request.HTTPBody?.ToUTF8String() ?? ""),
                                                                                      //     providerId ????
                                                                                      out var                                     getChargeDetailRecordsRequest,
                                                                                      out var                                     errorResponse,
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
                                                                                   getChargeDetailRecordsRequest!))).
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
                                                                                                                                                   getChargeDetailRecordsRequest!))))?.FirstOrDefault();

                                                  Counters.GetChargeDetailRecords.IncResponses_OK();

                                              }
                                              catch (Exception e)
                                              {

                                                  Counters.GetChargeDetailRecords.IncResponses_Error();

                                                  getChargeDetailRecordsResponse = OICPResult<GetChargeDetailRecordsResponse>.Failed(
                                                                                       this,
                                                                                       new GetChargeDetailRecordsResponse(
                                                                                           Timestamp.Now,
                                                                                           Request.EventTrackingId,
                                                                                           processId,
                                                                                           Timestamp.Now - Request.Timestamp,
                                                                                           [],
                                                                                           getChargeDetailRecordsRequest,
                                                                                           StatusCode: new StatusCode(
                                                                                                           StatusCodes.DataError,
                                                                                                           e.Message,
                                                                                                           e.StackTrace
                                                                                                       )
                                                                                       )
                                                                                   );

                                              }
                                          }

                                          if (getChargeDetailRecordsResponse is null)
                                          {

                                              Counters.GetChargeDetailRecords.IncResponses_Error();

                                              getChargeDetailRecordsResponse = OICPResult<GetChargeDetailRecordsResponse>.Failed(
                                                                                   this,
                                                                                   new GetChargeDetailRecordsResponse(
                                                                                       Timestamp.Now,
                                                                                       Request.EventTrackingId,
                                                                                       processId,
                                                                                       Timestamp.Now - Request.Timestamp,
                                                                                       [],
                                                                                       getChargeDetailRecordsRequest,
                                                                                       StatusCode: new StatusCode(
                                                                                                       StatusCodes.SystemError,
                                                                                                       "Could not process the received GetChargeDetailRecords request!"
                                                                                                   )
                                                                                   )
                                                                               );

                                          }

                                          #endregion

                                          #region Send OnGetChargeDetailRecordsResponse event

                                          try
                                          {

                                              if (OnGetChargeDetailRecordsResponse is not null)
                                                  await Task.WhenAll(OnGetChargeDetailRecordsResponse.GetInvocationList().
                                                                     Cast<OnGetChargeDetailRecordsAPIResponseDelegate>().
                                                                     Select(e => e(Timestamp.Now,
                                                                                   this,
                                                                                   getChargeDetailRecordsRequest!,
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
                                      {

                                          Counters.GetChargeDetailRecords.IncRequests_Error();

                                          getChargeDetailRecordsResponse = OICPResult<GetChargeDetailRecordsResponse>.Failed(
                                                                               this,
                                                                               new GetChargeDetailRecordsResponse(
                                                                                   Timestamp.Now,
                                                                                   Request.EventTrackingId,
                                                                                   processId,
                                                                                   Timestamp.Now - Request.Timestamp,
                                                                                   [],
                                                                                   getChargeDetailRecordsRequest,
                                                                                   StatusCode: new StatusCode(
                                                                                                   StatusCodes.DataError,
                                                                                                   "We could not parse the given GetChargeDetailRecords request!",
                                                                                                   errorResponse
                                                                                               )
                                                                               )
                                                                           );

                                      }

                                  }
                                  catch (Exception e)
                                  {

                                      Counters.GetChargeDetailRecords.IncResponses_Error();

                                      getChargeDetailRecordsResponse = OICPResult<GetChargeDetailRecordsResponse>.Failed(
                                                                           this,
                                                                           new GetChargeDetailRecordsResponse(
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
                                             Content                    = getChargeDetailRecordsResponse.Response?.ToJSON(CustomGetChargeDetailRecordsResponseSerializer,
                                                                                                                          CustomIPagedResponseSerializer,
                                                                                                                          CustomChargeDetailRecordSerializer,
                                                                                                                          CustomIdentificationSerializer,
                                                                                                                          CustomSignedMeteringValueSerializer,
                                                                                                                          CustomCalibrationLawVerificationSerializer,
                                                                                                                          CustomStatusCodeSerializer).
                                                                                                                   ToString(JSONFormatting).
                                                                                                                   ToUTF8Bytes()
                                                                                                         ?? [],
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
