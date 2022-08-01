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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    /// <summary>
    /// Extension methods for the CPO client interface.
    /// </summary>
    public static class ICPOClientExtensions
    {

        #region PushEVSEData(OperatorEVSEData, Action = fullLoad, ...)

        /// <summary>
        /// Upload the given EVSE data records.
        /// </summary>
        /// <param name="CPOClient">A CPO client.</param>
        /// 
        /// <param name="OperatorEVSEData">An operator EVSE data.</param>
        /// <param name="Action">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<OICPResult<Acknowledgement<PushEVSEDataRequest>>>

            PushEVSEData(this ICPOClient     CPOClient,
                         OperatorEVSEData    OperatorEVSEData,
                         ActionTypes         Action              = ActionTypes.FullLoad,
                         JObject?            CustomData          = null,

                         DateTime?           Timestamp           = null,
                         CancellationToken?  CancellationToken   = null,
                         EventTracking_Id?   EventTrackingId     = null,
                         TimeSpan?           RequestTimeout      = null)


                => CPOClient.PushEVSEData(
                       new PushEVSEDataRequest(
                           OperatorEVSEData,
                           Action,
                           null,
                           CustomData,

                           Timestamp,
                           CancellationToken,
                           EventTrackingId,
                           RequestTimeout ?? CPOClient.RequestTimeout));

        #endregion

        #region PushEVSEData(EVSEDataRecords, OperatorId, OperatorName = null, Action = fullLoad, ...)

        /// <summary>
        /// Upload the given EVSE data records.
        /// </summary>
        /// <param name="CPOClient">A CPO client.</param>
        /// 
        /// <param name="EVSEDataRecords">An enumeration of EVSE data records.</param>
        /// <param name="OperatorId">The unqiue identification of the charging station operator maintaining the given EVSE data records.</param>
        /// <param name="OperatorName">The name of the charging station operator maintaining the given EVSE data records.</param>
        /// <param name="Action">The server-side data management operation.</param>
        /// <param name="IncludeEVSEDataRecords">An optional delegate for filtering EVSE data records before pushing them to the server.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<OICPResult<Acknowledgement<PushEVSEDataRequest>>>

            PushEVSEData(this ICPOClient                  CPOClient,
                         IEnumerable<EVSEDataRecord>      EVSEDataRecords,
                         Operator_Id                      OperatorId,
                         String                           OperatorName,
                         ActionTypes                      Action                   = ActionTypes.FullLoad,
                         IncludeEVSEDataRecordsDelegate?  IncludeEVSEDataRecords   = null,
                         JObject?                         CustomData               = null,

                         DateTime?                        Timestamp                = null,
                         CancellationToken?               CancellationToken        = null,
                         EventTracking_Id?                EventTrackingId          = null,
                         TimeSpan?                        RequestTimeout           = null)


                => CPOClient.PushEVSEData(
                       new PushEVSEDataRequest(
                           new OperatorEVSEData(IncludeEVSEDataRecords != null
                                                    ? EVSEDataRecords.Where(evsedatarecord => IncludeEVSEDataRecords(evsedatarecord))
                                                    : EVSEDataRecords,
                                                OperatorId,
                                                OperatorName),
                           Action,
                           null,
                           CustomData,

                           Timestamp,
                           CancellationToken,
                           EventTrackingId,
                           RequestTimeout ?? CPOClient.RequestTimeout));

        #endregion

        #region PushEVSEData(EVSEDataRecord,  OperatorId, OperatorName = null, Action = insert, ...)

        /// <summary>
        /// Upload the given EVSE data records.
        /// </summary>
        /// <param name="CPOClient">A CPO client.</param>
        /// 
        /// <param name="EVSEDataRecord">An EVSE data record.</param>
        /// <param name="Action">The server-side data management operation.</param>
        /// <param name="OperatorId">The unqiue identification of the charging station operator maintaining the given EVSE data records.</param>
        /// <param name="OperatorName">The name of the charging station operator maintaining the given EVSE data records.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<OICPResult<Acknowledgement<PushEVSEDataRequest>>>

            PushEVSEData(this ICPOClient     CPOClient,
                         EVSEDataRecord      EVSEDataRecord,
                         Operator_Id         OperatorId,
                         String              OperatorName,
                         ActionTypes         Action              = ActionTypes.Insert,
                         JObject?            CustomData          = null,

                         DateTime?           Timestamp           = null,
                         CancellationToken?  CancellationToken   = null,
                         EventTracking_Id?   EventTrackingId     = null,
                         TimeSpan?           RequestTimeout      = null)


                => CPOClient.PushEVSEData(
                       new PushEVSEDataRequest(
                           new OperatorEVSEData(new EVSEDataRecord[] { EVSEDataRecord },
                                                OperatorId,
                                                OperatorName),
                           Action,
                           null,
                           CustomData,

                           Timestamp,
                           CancellationToken,
                           EventTrackingId,
                           RequestTimeout ?? CPOClient.RequestTimeout));

        #endregion

        #region PushEVSEData(OperatorId, Action, params EVSEDataRecords)

        /// <summary>
        /// Upload the given EVSE data records.
        /// </summary>
        /// <param name="CPOClient">A CPO client.</param>
        /// 
        /// <param name="OperatorId">The unqiue identification of the charging station operator maintaining the given EVSE data records.</param>
        /// <param name="OperatorName">The name of the EVSE operator maintaining the given EVSE data records.</param>
        /// <param name="Action">The server-side data management operation.</param>
        /// <param name="EVSEDataRecords">An array of EVSE data records.</param>
        public static Task<OICPResult<Acknowledgement<PushEVSEDataRequest>>>

            PushEVSEData(this ICPOClient          CPOClient,
                         Operator_Id              OperatorId,
                         String                   OperatorName,
                         ActionTypes              Action,
                         params EVSEDataRecord[]  EVSEDataRecords)


            => CPOClient.PushEVSEData(
                   new PushEVSEDataRequest(
                       new OperatorEVSEData(
                           EVSEDataRecords,
                           OperatorId,
                           OperatorName
                       ),
                       Action,
                       RequestTimeout: CPOClient.RequestTimeout));

        #endregion


        #region PushEVSEStatus(OperatorEVSEStatus, Action = update, ...)

        /// <summary>
        /// Upload the given EVSE status records.
        /// </summary>
        /// <param name="CPOClient">A CPO client.</param>
        /// 
        /// <param name="OperatorEVSEStatus">An operator EVSE status.</param>
        /// <param name="Action">The server-side status management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<OICPResult<Acknowledgement<PushEVSEStatusRequest>>>

            PushEVSEStatus(this ICPOClient     CPOClient,
                           OperatorEVSEStatus  OperatorEVSEStatus,
                           ActionTypes         Action              = ActionTypes.Update,
                           JObject?            CustomData          = null,

                           DateTime?           Timestamp           = null,
                           CancellationToken?  CancellationToken   = null,
                           EventTracking_Id?   EventTrackingId     = null,
                           TimeSpan?           RequestTimeout      = null)


                => CPOClient.PushEVSEStatus(
                       new PushEVSEStatusRequest(
                           OperatorEVSEStatus,
                           Action,
                           null,
                           CustomData,

                           Timestamp,
                           CancellationToken,
                           EventTrackingId,
                           RequestTimeout ?? CPOClient.RequestTimeout));

        #endregion

        #region PushEVSEStatus(EVSEStatusRecords, OperatorId, OperatorName = null, Action = update, ...)

        /// <summary>
        /// Upload the given EVSE status records.
        /// </summary>
        /// <param name="CPOClient">A CPO client.</param>
        /// 
        /// <param name="EVSEStatusRecords">An enumeration of EVSE status records.</param>
        /// <param name="OperatorId">The unqiue identification of the charging station operator maintaining the given EVSE status records.</param>
        /// <param name="OperatorName">The name of the charging station operator maintaining the given EVSE status records.</param>
        /// <param name="Action">The server-side status management operation.</param>
        /// <param name="IncludeEVSEStatusRecords">An optional delegate for filtering EVSE status records before pushing them to the server.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<OICPResult<Acknowledgement<PushEVSEStatusRequest>>>

            PushEVSEStatus(this ICPOClient                    CPOClient,
                           IEnumerable<EVSEStatusRecord>      EVSEStatusRecords,
                           Operator_Id                        OperatorId,
                           String                             OperatorName,
                           ActionTypes                        Action                     = ActionTypes.Update,
                           IncludeEVSEStatusRecordsDelegate?  IncludeEVSEStatusRecords   = null,
                           JObject?                           CustomData                 = null,

                           DateTime?                          Timestamp                  = null,
                           CancellationToken?                 CancellationToken          = null,
                           EventTracking_Id?                  EventTrackingId            = null,
                           TimeSpan?                          RequestTimeout             = null)


                => CPOClient.PushEVSEStatus(
                       new PushEVSEStatusRequest(
                           new OperatorEVSEStatus(
                               IncludeEVSEStatusRecords != null
                                   ? EVSEStatusRecords.Where(evsestatusrecord => IncludeEVSEStatusRecords(evsestatusrecord))
                                   : EVSEStatusRecords,
                               OperatorId,
                               OperatorName
                           ),
                           Action,
                           null,
                           CustomData,

                           Timestamp,
                           CancellationToken,
                           EventTrackingId,
                           RequestTimeout ?? CPOClient.RequestTimeout));

        #endregion

        #region PushEVSEStatus(EVSEStatusRecord,  OperatorId, OperatorName = null, Action = insert, ...)

        /// <summary>
        /// Upload the given EVSE status records.
        /// </summary>
        /// <param name="CPOClient">A CPO client.</param>
        /// 
        /// <param name="EVSEStatusRecord">An EVSE status record.</param>
        /// <param name="Action">The server-side status management operation.</param>
        /// <param name="OperatorId">The unqiue identification of the charging station operator maintaining the given EVSE status records.</param>
        /// <param name="OperatorName">The name of the charging station operator maintaining the given EVSE status records.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<OICPResult<Acknowledgement<PushEVSEStatusRequest>>>

            PushEVSEStatus(this ICPOClient     CPOClient,
                           EVSEStatusRecord    EVSEStatusRecord,
                           Operator_Id         OperatorId,
                           String              OperatorName,
                           ActionTypes         Action              = ActionTypes.Update,
                           JObject?            CustomData          = null,

                           DateTime?           Timestamp           = null,
                           CancellationToken?  CancellationToken   = null,
                           EventTracking_Id?   EventTrackingId     = null,
                           TimeSpan?           RequestTimeout      = null)


                => CPOClient.PushEVSEStatus(
                       new PushEVSEStatusRequest(
                           new OperatorEVSEStatus(
                               new EVSEStatusRecord[] { EVSEStatusRecord },
                               OperatorId,
                               OperatorName
                           ),
                           Action,
                           null,
                           CustomData,

                           Timestamp,
                           CancellationToken,
                           EventTrackingId,
                           RequestTimeout ?? CPOClient.RequestTimeout));

        #endregion

        #region PushEVSEStatus(OperatorId, Action, params EVSEStatusRecords)

        /// <summary>
        /// Upload the given EVSE status records.
        /// </summary>
        /// <param name="CPOClient">A CPO client.</param>
        /// 
        /// <param name="OperatorId">The unqiue identification of the charging station operator maintaining the given EVSE status records.</param>
        /// <param name="OperatorName">The name of the EVSE operator maintaining the given EVSE data records.</param>
        /// <param name="Action">The server-side status management operation.</param>
        /// <param name="EVSEStatusRecords">An array of EVSE status records.</param>
        public static Task<OICPResult<Acknowledgement<PushEVSEStatusRequest>>>

            PushEVSEStatus(this ICPOClient            CPOClient,
                           Operator_Id                OperatorId,
                           String                     OperatorName,
                           ActionTypes                Action,
                           params EVSEStatusRecord[]  EVSEStatusRecords)


            => CPOClient.PushEVSEStatus(
                   new PushEVSEStatusRequest(
                       new OperatorEVSEStatus(
                           EVSEStatusRecords,
                           OperatorId,
                           OperatorName
                       ),
                       Action,
                       RequestTimeout: CPOClient.RequestTimeout));

        #endregion


        #region PushPricingProductData           (PricingProductData, Action = fullLoad, ...)

        /// <summary>
        /// Upload the given pricing product data.
        /// </summary>
        /// <param name="PricingProductData">The pricing product data record.</param>
        /// <param name="Action">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public static Task<OICPResult<Acknowledgement<PushPricingProductDataRequest>>>

            PushPricingProductData(this ICPOClient     CPOClient,
                                   PricingProductData  PricingProductData,
                                   ActionTypes         Action              = ActionTypes.FullLoad,
                                   Process_Id?         ProcessId           = null,
                                   JObject?            CustomData          = null,

                                   DateTime?           Timestamp           = null,
                                   CancellationToken?  CancellationToken   = null,
                                   EventTracking_Id?   EventTrackingId     = null,
                                   TimeSpan?           RequestTimeout      = null)

                => CPOClient.PushPricingProductData(
                       new PushPricingProductDataRequest(
                           PricingProductData,
                           Action,
                           ProcessId,
                           CustomData,

                           Timestamp,
                           CancellationToken,
                           EventTrackingId,
                           RequestTimeout ?? CPOClient.RequestTimeout));


        #endregion

        #region PushEVSEPricing                  (OperatorId, EVSEPricing, Action = fullLoad, ...)

        /// <summary>
        /// Upload the given EVSE pricing data.
        /// </summary>
        /// <param name="EVSEPricing">The EVSE pricing data.</param>
        /// <param name="Action">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public static Task<OICPResult<Acknowledgement<PushEVSEPricingRequest>>>

            PushEVSEPricing(this ICPOClient           CPOClient,
                            Operator_Id               OperatorId,
                            IEnumerable<EVSEPricing>  EVSEPricing,
                            ActionTypes               Action              = ActionTypes.FullLoad,
                            Process_Id?               ProcessId           = null,
                            JObject?                  CustomData          = null,

                            DateTime?                 Timestamp           = null,
                            CancellationToken?        CancellationToken   = null,
                            EventTracking_Id?         EventTrackingId     = null,
                            TimeSpan?                 RequestTimeout      = null)

                => CPOClient.PushEVSEPricing(
                       new PushEVSEPricingRequest(
                           OperatorId,
                           EVSEPricing,
                           Action,
                           ProcessId,
                           CustomData,

                           Timestamp,
                           CancellationToken,
                           EventTrackingId,
                           RequestTimeout ?? CPOClient.RequestTimeout));


        #endregion


        #region AuthorizeStart                   (OperatorId, Identification, EVSEId = null, ...)

        /// <summary>
        /// Authorize for starting a charging session.
        /// </summary>
        /// <param name="CPOClient">A CPO client.</param>
        /// <param name="OperatorId">The unqiue identification of the charging station operator.</param>
        /// <param name="Identification">Authentication data used to authorize the user or the car.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="PartnerProductId">An optional partner product identification (for identifying a charging tariff).</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<OICPResult<AuthorizationStartResponse>>

            AuthorizeStart(this ICPOClient        CPOClient,
                           Operator_Id            OperatorId,
                           Identification         Identification,
                           EVSE_Id?               EVSEId                = null,
                           PartnerProduct_Id?     PartnerProductId      = null,
                           CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                           JObject?               CustomData            = null,

                           DateTime?              Timestamp             = null,
                           CancellationToken?     CancellationToken     = null,
                           EventTracking_Id?      EventTrackingId       = null,
                           TimeSpan?              RequestTimeout        = null)

            => CPOClient.AuthorizeStart(
                   new AuthorizeStartRequest(
                       OperatorId,
                       Identification,
                       EVSEId,
                       PartnerProductId, // PartnerProductId will not be shown in the Hubject portal!
                       null,             // SessionId will be ignored by Hubject!
                       CPOPartnerSessionId,
                       null,             // EMPPartnerSessionId does not make much sense here!
                       null,
                       CustomData,

                       Timestamp,
                       CancellationToken,
                       EventTrackingId,
                       RequestTimeout ?? CPOClient.RequestTimeout));

        #endregion

        #region AuthorizeStop                    (OperatorId, SessionId, Identification, EVSEId = null, ...)

        /// <summary>
        /// Authorize for stopping a charging session.
        /// </summary>
        /// <param name="CPOClient">A CPO client.</param>
        /// <param name="OperatorId">The unqiue identification of the charging station operator.</param>
        /// <param name="SessionId">The charging session identification.</param>
        /// <param name="Identification">Authentication data used to authorize the user or the car.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<OICPResult<AuthorizationStopResponse>>

            AuthorizeStop(this ICPOClient        CPOClient,
                          Operator_Id            OperatorId,
                          Session_Id             SessionId,
                          Identification         Identification,
                          EVSE_Id?               EVSEId                = null,
                          CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                          EMPPartnerSession_Id?  EMPPartnerSessionId   = null,
                          JObject?               CustomData            = null,

                          DateTime?              Timestamp             = null,
                          CancellationToken?     CancellationToken     = null,
                          EventTracking_Id?      EventTrackingId       = null,
                          TimeSpan?              RequestTimeout        = null)

            => CPOClient.AuthorizeStop(
                   new AuthorizeStopRequest(
                       OperatorId,
                       SessionId,
                       Identification,
                       EVSEId,
                       CPOPartnerSessionId,
                       EMPPartnerSessionId,
                       null,
                       CustomData,

                       Timestamp,
                       CancellationToken,
                       EventTrackingId,
                       RequestTimeout ?? CPOClient.RequestTimeout));

        #endregion


        #region SendChargingNotificationsStart   (SessionId, Identification, EVSEId, ChargingStart, ...)

        /// <summary>
        /// Send a charging start notification.
        /// </summary>
        /// <param name="CPOClient">A CPO client.</param>
        /// 
        /// <param name="SessionId">The Hubject session identification, that identifies the charging process.</param>
        /// <param name="Identification">The authentication data used to authorize the user or the car.</param>
        /// <param name="EVSEId">The EVSE identification, that identifies the location of the charging process.</param>
        /// <param name="ChargingStart">The timestamp when the charging process started.</param>
        /// 
        /// <param name="CPOPartnerSessionId">An optional session identification assinged by the CPO partner.</param>
        /// <param name="EMPPartnerSessionId">An optional session identification assinged by the EMP partner.</param>
        /// <param name="SessionStart">An optional timestamp when the charging session started.</param>
        /// <param name="MeterValueStart">An optional starting value of the energy meter [kWh].</param>
        /// <param name="OperatorId">An optional operator identification of the hub operator.</param>
        /// <param name="PartnerProductId">An optional pricing product name (for identifying a tariff) that must be unique.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<OICPResult<Acknowledgement<ChargingStartNotificationRequest>>>

            SendChargingNotificationsStart(this ICPOClient        CPOClient,
                                           Session_Id             SessionId,
                                           Identification         Identification,
                                           EVSE_Id                EVSEId,
                                           DateTime               ChargingStart,

                                           CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                                           EMPPartnerSession_Id?  EMPPartnerSessionId   = null,
                                           DateTime?              SessionStart          = null,
                                           Decimal?               MeterValueStart       = null,
                                           Operator_Id?           OperatorId            = null,
                                           PartnerProduct_Id?     PartnerProductId      = null,
                                           JObject?               CustomData            = null,

                                           DateTime?              Timestamp             = null,
                                           CancellationToken?     CancellationToken     = null,
                                           EventTracking_Id?      EventTrackingId       = null,
                                           TimeSpan?              RequestTimeout        = null)


                => CPOClient.SendChargingStartNotification(
                       new ChargingStartNotificationRequest(
                           SessionId,
                           Identification,
                           EVSEId,
                           ChargingStart,

                           CPOPartnerSessionId,
                           EMPPartnerSessionId,
                           SessionStart,
                           MeterValueStart,
                           OperatorId,
                           PartnerProductId,
                           null,
                           CustomData,

                           Timestamp,
                           CancellationToken,
                           EventTrackingId,
                           RequestTimeout ?? CPOClient.RequestTimeout));

        #endregion

        #region SendChargingNotificationsProgress(SessionId, Identification, EVSEId, ChargingStart, EventOcurred, ...)

        /// <summary>
        /// Send a charging progress notification.
        /// </summary>
        /// <param name="CPOClient">A CPO client.</param>
        /// 
        /// <param name="SessionId">The Hubject session identification, that identifies the charging process.</param>
        /// <param name="Identification">The authentication data used to authorize the user or the car.</param>
        /// <param name="EVSEId">The EVSE identification, that identifies the location of the charging process.</param>
        /// <param name="ChargingStart">The timestamp when the charging process started.</param>
        /// <param name="EventOcurred">The timestamp when the charging progress parameters had been captured.</param>
        /// 
        /// <param name="CPOPartnerSessionId">An optional session identification assinged by the CPO partner.</param>
        /// <param name="EMPPartnerSessionId">An optional session identification assinged by the EMP partner.</param>
        /// <param name="ChargingDuration">Charging Duration = EventOccurred - Charging Duration.</param>
        /// <param name="SessionStart">An optional timestamp when the charging session started.</param>
        /// <param name="ConsumedEnergyProgress">The optional consumed energy till now.</param>
        /// <param name="MeterValueStart">An optional starting value of the energy meter [kWh].</param>
        /// <param name="MeterValuesInBetween">An optional enumeration of meter values during the charging session.</param>
        /// <param name="OperatorId">An optional operator identification of the hub operator.</param>
        /// <param name="PartnerProductId">An optional pricing product name (for identifying a tariff) that must be unique.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>>

            SendChargingNotificationsProgress(this ICPOClient        CPOClient,
                                              Session_Id             SessionId,
                                              Identification         Identification,
                                              EVSE_Id                EVSEId,
                                              DateTime               ChargingStart,
                                              DateTime               EventOcurred,

                                              CPOPartnerSession_Id?  CPOPartnerSessionId      = null,
                                              EMPPartnerSession_Id?  EMPPartnerSessionId      = null,
                                              TimeSpan?              ChargingDuration         = null,
                                              DateTime?              SessionStart             = null,
                                              Decimal?               ConsumedEnergyProgress   = null,
                                              Decimal?               MeterValueStart          = null,
                                              IEnumerable<Decimal>?  MeterValuesInBetween     = null,
                                              Operator_Id?           OperatorId               = null,
                                              PartnerProduct_Id?     PartnerProductId         = null,
                                              JObject?               CustomData               = null,

                                              DateTime?              Timestamp                = null,
                                              CancellationToken?     CancellationToken        = null,
                                              EventTracking_Id?      EventTrackingId          = null,
                                              TimeSpan?              RequestTimeout           = null)


                => CPOClient.SendChargingProgressNotification(
                       new ChargingProgressNotificationRequest(
                           SessionId,
                           Identification,
                           EVSEId,
                           ChargingStart,
                           EventOcurred,

                           CPOPartnerSessionId,
                           EMPPartnerSessionId,
                           ChargingDuration,
                           SessionStart,
                           ConsumedEnergyProgress,
                           MeterValueStart,
                           MeterValuesInBetween,
                           OperatorId,
                           PartnerProductId,
                           null,
                           CustomData,

                           Timestamp,
                           CancellationToken,
                           EventTrackingId,
                           RequestTimeout ?? CPOClient.RequestTimeout));

        #endregion

        #region SendChargingNotificationsEnd     (SessionId, Identification, EVSEId, ChargingStart, ChargingEnd, ...)

        /// <summary>
        /// Send a charging end notification.
        /// </summary>
        /// <param name="CPOClient">A CPO client.</param>
        /// 
        /// <param name="SessionId">The Hubject session identification, that identifies the charging process.</param>
        /// <param name="Identification">The authentication data used to authorize the user or the car.</param>
        /// <param name="EVSEId">The EVSE identification, that identifies the location of the charging process.</param>
        /// <param name="ChargingStart">The timestamp when the charging process started.</param>
        /// <param name="ChargingEnd">The timestamp when the charging process stopped.</param>
        /// 
        /// <param name="CPOPartnerSessionId">An optional session identification assinged by the CPO partner.</param>
        /// <param name="EMPPartnerSessionId">An optional session identification assinged by the EMP partner.</param>
        /// <param name="SessionStart">An optional timestamp when the charging session started.</param>
        /// <param name="SessionEnd">An optional timestamp when the charging session stopped.</param>
        /// <param name="ConsumedEnergy">The optional consumed energy.</param>
        /// <param name="MeterValueStart">An optional starting value of the energy meter [kWh].</param>
        /// <param name="MeterValueEnd">An optional ending value of the energy meter [kWh].</param>
        /// <param name="MeterValuesInBetween">An optional enumeration of meter values during the charging session.</param>
        /// <param name="OperatorId">An optional operator identification of the hub operator.</param>
        /// <param name="PartnerProductId">An optional pricing product name (for identifying a tariff) that must be unique.</param>
        /// <param name="PenaltyTimeStart">An optional timestamp when the penalty time start after the grace period.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<OICPResult<Acknowledgement<ChargingEndNotificationRequest>>>

            SendChargingNotificationsEnd(this ICPOClient         CPOClient,
                                         Session_Id              SessionId,
                                         Identification          Identification,
                                         EVSE_Id                 EVSEId,
                                         DateTime                ChargingStart,
                                         DateTime                ChargingEnd,

                                         CPOPartnerSession_Id?   CPOPartnerSessionId      = null,
                                         EMPPartnerSession_Id?   EMPPartnerSessionId      = null,
                                         DateTime?               SessionStart             = null,
                                         DateTime?               SessionEnd               = null,
                                         Decimal?                ConsumedEnergy           = null,
                                         Decimal?                MeterValueStart          = null,
                                         Decimal?                MeterValueEnd            = null,
                                         IEnumerable<Decimal>?   MeterValuesInBetween     = null,
                                         Operator_Id?            OperatorId               = null,
                                         PartnerProduct_Id?      PartnerProductId         = null,
                                         DateTime?               PenaltyTimeStart         = null,
                                         JObject?                CustomData               = null,

                                         DateTime?               Timestamp                = null,
                                         CancellationToken?      CancellationToken        = null,
                                         EventTracking_Id?       EventTrackingId          = null,
                                         TimeSpan?               RequestTimeout           = null)


                => CPOClient.SendChargingEndNotification(
                       new ChargingEndNotificationRequest(
                           SessionId,
                           Identification,
                           EVSEId,
                           ChargingStart,
                           ChargingEnd,

                           CPOPartnerSessionId,
                           EMPPartnerSessionId,
                           SessionStart,
                           SessionEnd,
                           ConsumedEnergy,
                           MeterValueStart,
                           MeterValueEnd,
                           MeterValuesInBetween,
                           OperatorId,
                           PartnerProductId,
                           PenaltyTimeStart,
                           null,
                           CustomData,

                           Timestamp,
                           CancellationToken,
                           EventTrackingId,
                           RequestTimeout ?? CPOClient.RequestTimeout));

        #endregion

        #region SendChargingNotificationsError   (SessionId, Identification, EVSEId, ErrorType, ...)

        /// <summary>
        /// Send a charging error notification.
        /// </summary>
        /// <param name="CPOClient">A CPO client.</param>
        /// 
        /// <param name="SessionId">The Hubject session identification, that identifies the charging process.</param>
        /// <param name="Identification">The authentication data used to authorize the user or the car.</param>
        /// <param name="EVSEId">The EVSE identification, that identifies the location of the charging process.</param>
        /// <param name="ErrorType">The error class.</param>
        /// 
        /// <param name="CPOPartnerSessionId">An optional session identification assinged by the CPO partner.</param>
        /// <param name="EMPPartnerSessionId">An optional session identification assinged by the EMP partner.</param>
        /// <param name="ErrorAdditionalInfo">Additional information about the error.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>>

            SendChargingNotificationsError(this ICPOClient        CPOClient,
                                           Session_Id             SessionId,
                                           Identification         Identification,
                                           EVSE_Id                EVSEId,
                                           ErrorClassTypes        ErrorType,

                                           CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                                           EMPPartnerSession_Id?  EMPPartnerSessionId   = null,
                                           String?                ErrorAdditionalInfo   = null,
                                           JObject?               CustomData            = null,

                                           DateTime?              Timestamp             = null,
                                           CancellationToken?     CancellationToken     = null,
                                           EventTracking_Id?      EventTrackingId       = null,
                                           TimeSpan?              RequestTimeout        = null)


                => CPOClient.SendChargingErrorNotification(
                       new ChargingErrorNotificationRequest(
                           SessionId,
                           Identification,
                           EVSEId,
                           ErrorType,

                           CPOPartnerSessionId,
                           EMPPartnerSessionId,
                           ErrorAdditionalInfo,
                           null,
                           CustomData,

                           Timestamp,
                           CancellationToken,
                           EventTrackingId,
                           RequestTimeout ?? CPOClient.RequestTimeout));

        #endregion


        #region SendChargeDetailRecord           (ChargeDetailRecord, OperatorId, ...)

        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="CPOClient">A CPO client.</param>
        /// 
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="OperatorId">The unqiue identification of the operator sending the given charge detail record (not the suboperator or the operator of the EVSE).</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<OICPResult<Acknowledgement<ChargeDetailRecordRequest>>>

            SendChargeDetailRecord(this ICPOClient     CPOClient,
                                   ChargeDetailRecord  ChargeDetailRecord,
                                   Operator_Id         OperatorId,
                                   JObject?            CustomData         = null,

                                   DateTime?           Timestamp          = null,
                                   CancellationToken?  CancellationToken  = null,
                                   EventTracking_Id?   EventTrackingId    = null,
                                   TimeSpan?           RequestTimeout     = null)


                => CPOClient.SendChargeDetailRecord(
                       new ChargeDetailRecordRequest(
                           ChargeDetailRecord,
                           OperatorId,
                           null,
                           CustomData,

                           Timestamp,
                           CancellationToken,
                           EventTrackingId,
                           RequestTimeout ?? CPOClient.RequestTimeout));

        #endregion


    }

}
