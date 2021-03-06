﻿/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.EMP
{

    /// <summary>
    /// Extention methods for the EMP client interface.
    /// </summary>
    public static class IEMPClientExtentions
    {

        #region PullEVSEData              (ProviderId, ...)

        /// <summary>
        /// Create a new task querying EVSE data from the OICP server.
        /// The request might either have none, 'SearchCenter + DistanceKM' or 'LastCall' parameters.
        /// Because of limitations at Hubject the SearchCenter and LastCall parameters can not be used at the same time!
        /// </summary>
        /// <param name="EMPClient">An EMP client.</param>
        /// 
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="LastCall">An optional timestamp of the last call. Cannot be combined with 'SearchCenter'.</param>
        /// 
        /// <param name="GeoCoordinatesResponseFormat">An optional response format for representing geo coordinates.</param>
        /// <param name="OperatorIdFilter">Only return EVSEs belonging to the given optional enumeration of EVSE operators.</param>
        /// <param name="CountryCodeFilter">An optional enumeration of countries whose EVSE's a provider wants to retrieve.</param>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// 
        /// <param name="Page">An optional page number of the request page.</param>
        /// <param name="Size">An optional size of a request page.</param>
        /// <param name="SortOrder">Optional sorting criteria in the format: property(,asc|desc).</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public static Task<OICPResult<PullEVSEDataResponse>>

            PullEVSEData(this IEMPClient                                EMPClient,
                         Provider_Id                                    ProviderId,
                         DateTime?                                      LastCall                               = null,

                         IEnumerable<Operator_Id>                       OperatorIdFilter                       = null,
                         IEnumerable<Country>                           CountryCodeFilter                      = null,
                         IEnumerable<AccessibilityTypes>                AccessibilityFilter                    = null,
                         IEnumerable<AuthenticationModes>               AuthenticationModeFilter               = null,
                         IEnumerable<CalibrationLawDataAvailabilities>  CalibrationLawDataAvailabilityFilter   = null,
                         Boolean?                                       RenewableEnergyFilter                  = null,
                         Boolean?                                       IsHubjectCompatibleFilter              = null,
                         Boolean?                                       IsOpen24HoursFilter                    = null,

                         GeoCoordinates?                                SearchCenter                           = null,
                         Single?                                        DistanceKM                             = null,
                         GeoCoordinatesFormats?                         GeoCoordinatesResponseFormat           = GeoCoordinatesFormats.DecimalDegree,

                         UInt32?                                        Page                                   = null,
                         UInt32?                                        Size                                   = null,
                         IEnumerable<String>                            SortOrder                              = null,
                         JObject                                        CustomData                             = null,

                         DateTime?                                      Timestamp                              = null,
                         CancellationToken?                             CancellationToken                      = null,
                         EventTracking_Id                               EventTrackingId                        = null,
                         TimeSpan?                                      RequestTimeout                         = null)

                => EMPClient.PullEVSEData(
                       new PullEVSEDataRequest(
                           ProviderId,
                           LastCall,

                           OperatorIdFilter,
                           CountryCodeFilter,
                           AccessibilityFilter,
                           AuthenticationModeFilter,
                           CalibrationLawDataAvailabilityFilter,
                           RenewableEnergyFilter,
                           IsHubjectCompatibleFilter,
                           IsOpen24HoursFilter,

                           SearchCenter,
                           DistanceKM,
                           GeoCoordinatesResponseFormat,

                           Page,
                           Size,
                           SortOrder,
                           CustomData,

                           Timestamp,
                           CancellationToken,
                           EventTrackingId,
                           RequestTimeout ?? EMPClient.RequestTimeout));

        #endregion

        #region PullEVSEStatus            (ProviderId, ...)

        /// <summary>
        /// Create a new task requesting the current status of all EVSEs (within an optional search radius and status).
        /// </summary>
        /// <param name="EMPClient">An EMP client.</param>
        /// 
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="EVSEStatusFilter">An optional EVSE status as filter criteria.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public static Task<OICPResult<PullEVSEStatusResponse>>

            PullEVSEStatus(this IEMPClient      EMPClient,
                           Provider_Id          ProviderId,
                           GeoCoordinates?      SearchCenter        = null,
                           Single?              DistanceKM          = null,
                           EVSEStatusTypes?     EVSEStatusFilter    = null,
                           JObject              CustomData          = null,

                           DateTime?            Timestamp           = null,
                           CancellationToken?   CancellationToken   = null,
                           EventTracking_Id     EventTrackingId     = null,
                           TimeSpan?            RequestTimeout      = null)

                => EMPClient.PullEVSEStatus(
                       new PullEVSEStatusRequest(
                           ProviderId,
                           SearchCenter,
                           DistanceKM,
                           EVSEStatusFilter,
                           CustomData,

                           Timestamp,
                           CancellationToken,
                           EventTrackingId,
                           RequestTimeout ?? EMPClient.RequestTimeout));

        #endregion

        #region PullEVSEStatusById        (ProviderId, EVSEIds, ...)

        /// <summary>
        /// Create a new task requesting the current status of up to 100 EVSEs by their EVSE identifications.
        /// </summary>
        /// <param name="EMPClient">An EMP client.</param>
        /// 
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="EVSEIds">An enumeration of up to 100 EVSE identifications.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public static Task<OICPResult<PullEVSEStatusByIdResponse>>

            PullEVSEStatusById(this IEMPClient       EMPClient,
                               Provider_Id           ProviderId,
                               IEnumerable<EVSE_Id>  EVSEIds,
                               JObject               CustomData          = null,

                               DateTime?             Timestamp           = null,
                               CancellationToken?    CancellationToken   = null,
                               EventTracking_Id      EventTrackingId     = null,
                               TimeSpan?             RequestTimeout      = null)

                => EMPClient.PullEVSEStatusById(
                       new PullEVSEStatusByIdRequest(
                           ProviderId,
                           EVSEIds,
                           CustomData,

                           Timestamp,
                           CancellationToken,
                           EventTrackingId,
                           RequestTimeout ?? EMPClient.RequestTimeout));

        #endregion

        #region PullEVSEStatusByOperatorId(ProviderId, OperatorIds, ...)

        /// <summary>
        /// Create a new task requesting the current EVSE status of the given charging station operators.
        /// </summary>
        /// <param name="EMPClient">An EMP client.</param>
        /// 
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="OperatorIds">An enumeration of up to 100 operator identifications.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public static Task<OICPResult<PullEVSEStatusByOperatorIdResponse>>

            PullEVSEStatusByOperatorId(this IEMPClient           EMPClient,
                                       Provider_Id               ProviderId,
                                       IEnumerable<Operator_Id>  OperatorIds,
                                       JObject                   CustomData          = null,

                                       DateTime?                 Timestamp           = null,
                                       CancellationToken?        CancellationToken   = null,
                                       EventTracking_Id          EventTrackingId     = null,
                                       TimeSpan?                 RequestTimeout      = null)

                => EMPClient.PullEVSEStatusByOperatorId(
                       new PullEVSEStatusByOperatorIdRequest(
                           ProviderId,
                           OperatorIds,
                           CustomData,

                           Timestamp,
                           CancellationToken,
                           EventTrackingId,
                           RequestTimeout ?? EMPClient.RequestTimeout));

        #endregion


        #region PushAuthenticationData    (Request)

        ///// <summary>
        ///// Create a new task pushing provider authentication data records onto the OICP server.
        ///// </summary>
        ///// <param name="Request">An PushAuthenticationData request.</param>
        //public Task<OICPResult<Acknowledgement<PushAuthenticationDataRequest>>>

        //    PushAuthenticationData(PushAuthenticationDataRequest Request)

        //        => EMPClient.PushAuthenticationData(Request);

        #endregion


        #region ReservationStart          (ProviderId, EVSEId, Identification, ...)

        /// <summary>
        /// Create a reservation at the given EVSE.
        /// </summary>
        /// <param name="EMPClient">An EMP client.</param>
        /// 
        /// <param name="ProviderId">An e-mobility provider identification.</param>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="Identification">The user or contract identification.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="PartnerProductId">An optional partner product identification.</param>
        /// <param name="Duration">The optional duration of reservation.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public static Task<OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>>

            AuthorizeRemoteReservationStart(this IEMPClient        EMPClient,
                                            Provider_Id            ProviderId,
                                            EVSE_Id                EVSEId,
                                            Identification         Identification,
                                            Session_Id?            SessionId             = null,
                                            CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                                            EMPPartnerSession_Id?  EMPPartnerSessionId   = null,
                                            PartnerProduct_Id?     PartnerProductId      = null,
                                            TimeSpan?              Duration              = null,
                                            JObject                CustomData            = null,

                                            DateTime?              Timestamp             = null,
                                            CancellationToken?     CancellationToken     = null,
                                            EventTracking_Id       EventTrackingId       = null,
                                            TimeSpan?              RequestTimeout        = null)


                => EMPClient.AuthorizeRemoteReservationStart(
                       new AuthorizeRemoteReservationStartRequest(
                           ProviderId,
                           EVSEId,
                           Identification,
                           SessionId,
                           CPOPartnerSessionId,
                           EMPPartnerSessionId,
                           PartnerProductId,
                           Duration,
                           CustomData,

                           Timestamp,
                           CancellationToken,
                           EventTrackingId,
                           RequestTimeout ?? EMPClient.RequestTimeout));

        #endregion

        #region ReservationStop           (ProviderId, EVSEId, SessionId, ...)

        /// <summary>
        /// Delete a reservation at the given EVSE.
        /// </summary>
        /// <param name="EMPClient">An EMP client.</param>
        /// 
        /// <param name="ProviderId">An e-mobility provider identification.</param>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="SessionId">An charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public static Task<OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>>

            AuthorizeRemoteReservationStop(this IEMPClient        EMPClient,
                                           Provider_Id            ProviderId,
                                           EVSE_Id                EVSEId,
                                           Session_Id             SessionId,
                                           CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                                           EMPPartnerSession_Id?  EMPPartnerSessionId   = null,
                                           JObject                CustomData            = null,

                                           DateTime?              Timestamp             = null,
                                           CancellationToken?     CancellationToken     = null,
                                           EventTracking_Id       EventTrackingId       = null,
                                           TimeSpan?              RequestTimeout        = null)

                => EMPClient.AuthorizeRemoteReservationStop(
                       new AuthorizeRemoteReservationStopRequest(
                           ProviderId,
                           EVSEId,
                           SessionId,
                           CPOPartnerSessionId,
                           EMPPartnerSessionId,
                           CustomData,

                           Timestamp,
                           CancellationToken,
                           EventTrackingId,
                           RequestTimeout ?? EMPClient.RequestTimeout));

        #endregion

        #region RemoteStart               (ProviderId, EVSEId, Identification, ...)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="EMPClient">An EMP client.</param>
        /// 
        /// <param name="ProviderId">An e-mobility provider identification.</param>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="Identification">An user or contract identification.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="PartnerProductId">An optional partner product identification.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public static Task<OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>>

            AuthorizeRemoteStart(this IEMPClient        EMPClient,
                                 Provider_Id            ProviderId,
                                 EVSE_Id                EVSEId,
                                 Identification         Identification,
                                 Session_Id?            SessionId             = null,
                                 CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                                 EMPPartnerSession_Id?  EMPPartnerSessionId   = null,
                                 PartnerProduct_Id?     PartnerProductId      = null,
                                 JObject                CustomData            = null,

                                 DateTime?              Timestamp             = null,
                                 CancellationToken?     CancellationToken     = null,
                                 EventTracking_Id       EventTrackingId       = null,
                                 TimeSpan?              RequestTimeout        = null)

                => EMPClient.AuthorizeRemoteStart(
                       new AuthorizeRemoteStartRequest(
                           ProviderId,
                           EVSEId,
                           Identification,
                           SessionId,
                           CPOPartnerSessionId,
                           EMPPartnerSessionId,
                           PartnerProductId,
                           CustomData,

                           Timestamp,
                           CancellationToken,
                           EventTrackingId,
                           RequestTimeout ?? EMPClient.RequestTimeout));

        #endregion

        #region RemoteStop                (ProviderId, EVSEId, SessionId, ...)

        /// <summary>
        /// Stop the given charging session at the given EVSE.
        /// </summary>
        /// <param name="EMPClient">An EMP client.</param>
        /// 
        /// <param name="ProviderId">An e-mobility provider identification.</param>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="SessionId">A charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public static Task<OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>>

            AuthorizeRemoteStop(this IEMPClient        EMPClient,
                                Provider_Id            ProviderId,
                                EVSE_Id                EVSEId,
                                Session_Id             SessionId,
                                CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                                EMPPartnerSession_Id?  EMPPartnerSessionId   = null,
                                JObject                CustomData            = null,

                                DateTime?              Timestamp             = null,
                                CancellationToken?     CancellationToken     = null,
                                EventTracking_Id       EventTrackingId       = null,
                                TimeSpan?              RequestTimeout        = null)

                => EMPClient.AuthorizeRemoteStop(
                       new AuthorizeRemoteStopRequest(
                           ProviderId,
                           EVSEId,
                           SessionId,
                           CPOPartnerSessionId,
                           EMPPartnerSessionId,
                           CustomData,

                           Timestamp,
                           CancellationToken,
                           EventTrackingId,
                           RequestTimeout ?? EMPClient.RequestTimeout));

        #endregion


        #region GetChargeDetailRecords    (ProviderId, From, To, ...)

        /// <summary>
        /// Create a new task querying charge detail records from the OICP server.
        /// </summary>
        /// <param name="EMPClient">An EMP client.</param>
        /// 
        /// <param name="ProviderId">An e-mobility provider identification.</param>
        /// <param name="From">The start of the requested time range.</param>
        /// <param name="To">The end of the requested time range.</param>
        /// 
        /// <param name="SessionIds">An optional enumeration of charging session identifications.</param>
        /// <param name="OperatorIds">An optional enumeration of operator identifications.</param>
        /// <param name="CDRForwarded">Whether the CDR was successfuly forwarded to the EMP or not.</param>
        /// 
        /// <param name="Page">An optional page number of the request page.</param>
        /// <param name="Size">An optional size of a request page.</param>
        /// <param name="SortOrder">Optional sorting criteria in the format: property(,asc|desc).</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public static Task<OICPResult<GetChargeDetailRecordsResponse>>

            GetChargeDetailRecords(this IEMPClient           EMPClient,
                                   Provider_Id               ProviderId,
                                   DateTime                  From,
                                   DateTime                  To,
                                   IEnumerable<Session_Id>   SessionIds          = null,
                                   IEnumerable<Operator_Id>  OperatorIds         = null,
                                   Boolean?                  CDRForwarded        = null,

                                   UInt32?                   Page                = null,
                                   UInt32?                   Size                = null,
                                   IEnumerable<String>       SortOrder           = null,
                                   JObject                   CustomData          = null,

                                   DateTime?                 Timestamp           = null,
                                   CancellationToken?        CancellationToken   = null,
                                   EventTracking_Id          EventTrackingId     = null,
                                   TimeSpan?                 RequestTimeout      = null)

                => EMPClient.GetChargeDetailRecords(
                       new GetChargeDetailRecordsRequest(
                           ProviderId,
                           From,
                           To,
                           SessionIds,
                           OperatorIds,
                           CDRForwarded,

                           Page,
                           Size,
                           SortOrder,
                           CustomData,

                           Timestamp,
                           CancellationToken,
                           EventTrackingId,
                           RequestTimeout ?? EMPClient.RequestTimeout));

        #endregion

    }

}
