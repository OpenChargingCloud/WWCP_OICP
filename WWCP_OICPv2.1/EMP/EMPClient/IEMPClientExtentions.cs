/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.EMP
{

    /// <summary>
    /// Extention methods for the EMP client interface.
    /// </summary>
    public static class IEMPClientExtentions
    {

        #region PushAuthenticationData(ProviderAuthenticationDataRecords, ProviderId, Action = fullLoad, ...)

        /// <summary>
        /// Create a new task pushing provider authentication data records onto the OICP server.
        /// </summary>
        /// <param name="AuthorizationIdentifications">An enumeration of authorization identifications.</param>
        /// <param name="ProviderId">An e-mobility provider identification.</param>
        /// <param name="Action">An optional OICP action.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static async Task<HTTPResponse<Acknowledgement<PushAuthenticationDataRequest>>>

            PushAuthenticationData(this IEMPClient                           IEMPClient,
                                   IEnumerable<AuthorizationIdentification>  AuthorizationIdentifications,
                                   Provider_Id                               ProviderId,
                                   ActionTypes                               Action              = ActionTypes.fullLoad,

                                   DateTime?                                 Timestamp           = null,
                                   CancellationToken?                        CancellationToken   = null,
                                   EventTracking_Id                          EventTrackingId     = null,
                                   TimeSpan?                                 RequestTimeout      = null)


            => await IEMPClient.PushAuthenticationData(new PushAuthenticationDataRequest(AuthorizationIdentifications,
                                                                                         ProviderId,
                                                                                         Action,

                                                                                         Timestamp,
                                                                                         CancellationToken,
                                                                                         EventTrackingId,
                                                                                         RequestTimeout.HasValue ? RequestTimeout.Value : IEMPClient.RequestTimeout));

        #endregion


        #region ReservationStart(ProviderId, EVSEId, eMAId, SessionId = null, PartnerSessionId = null, PartnerProductId = null, ...)

        /// <summary>
        /// Create a reservation at the given EVSE.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="EVSEId">The unique identification of the EVSE to be started.</param>
        /// <param name="EVCOId">The unique identification of the e-mobility account.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="PartnerProductId">The unique identification of the choosen charging product.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static async Task<HTTPResponse<Acknowledgement<AuthorizeRemoteReservationStartRequest>>>

            ReservationStart(this IEMPClient       IEMPClient,
                             Provider_Id           ProviderId,
                             EVSE_Id               EVSEId,
                             EVCO_Id               EVCOId,
                             Session_Id?           SessionId           = null,
                             PartnerSession_Id?    PartnerSessionId    = null,
                             PartnerProduct_Id?    PartnerProductId    = null,

                             DateTime?             Timestamp           = null,
                             CancellationToken?    CancellationToken   = null,
                             EventTracking_Id      EventTrackingId     = null,
                             TimeSpan?             RequestTimeout      = null)


            => await IEMPClient.ReservationStart(new AuthorizeRemoteReservationStartRequest(ProviderId,
                                                                                            EVSEId,
                                                                                            EVCOId,
                                                                                            SessionId,
                                                                                            PartnerSessionId,
                                                                                            PartnerProductId,

                                                                                            Timestamp,
                                                                                            CancellationToken,
                                                                                            EventTrackingId,
                                                                                            RequestTimeout.HasValue ? RequestTimeout.Value : IEMPClient.RequestTimeout));

        #endregion

        #region ReservationStop(SessionId, ProviderId, EVSEId, PartnerSessionId = null, ...)

        /// <summary>
        /// Delete a reservation at the given EVSE.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the EVSE to be stopped.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static async Task<HTTPResponse<Acknowledgement<AuthorizeRemoteReservationStopRequest>>>

            ReservationStop(this IEMPClient       IEMPClient,
                            Session_Id            SessionId,
                            Provider_Id           ProviderId,
                            EVSE_Id               EVSEId,
                            PartnerSession_Id?    PartnerSessionId    = null,

                            DateTime?             Timestamp           = null,
                            CancellationToken?    CancellationToken   = null,
                            EventTracking_Id      EventTrackingId     = null,
                            TimeSpan?             RequestTimeout      = null)


            => await IEMPClient.ReservationStop(new AuthorizeRemoteReservationStopRequest(SessionId,
                                                                                          ProviderId,
                                                                                          EVSEId,
                                                                                          PartnerSessionId,

                                                                                          Timestamp,
                                                                                          CancellationToken,
                                                                                          EventTrackingId,
                                                                                          RequestTimeout.HasValue ? RequestTimeout.Value : IEMPClient.RequestTimeout));

        #endregion


        #region RemoteStart(ProviderId, EVSEId, eMAId, SessionId = null, PartnerSessionId = null, PartnerProductId = null, ...)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="EVSEId">The unique identification of the EVSE to be started.</param>
        /// <param name="EVCOId">The unique identification of the e-mobility account.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="PartnerProductId">The unique identification of the choosen charging product.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static async Task<HTTPResponse<Acknowledgement<AuthorizeRemoteStartRequest>>>

            RemoteStart(this IEMPClient       IEMPClient,
                        Provider_Id           ProviderId,
                        EVSE_Id               EVSEId,
                        EVCO_Id               EVCOId,
                        Session_Id?           SessionId           = null,
                        PartnerSession_Id?    PartnerSessionId    = null,
                        PartnerProduct_Id?    PartnerProductId    = null,

                        DateTime?             Timestamp           = null,
                        CancellationToken?    CancellationToken   = null,
                        EventTracking_Id      EventTrackingId     = null,
                        TimeSpan?             RequestTimeout      = null)

            => await IEMPClient.RemoteStart(new AuthorizeRemoteStartRequest(ProviderId,
                                                                            EVSEId,
                                                                            EVCOId,
                                                                            SessionId,
                                                                            PartnerSessionId,
                                                                            PartnerProductId,

                                                                            Timestamp,
                                                                            CancellationToken,
                                                                            EventTrackingId,
                                                                            RequestTimeout.HasValue ? RequestTimeout.Value : IEMPClient.RequestTimeout));

        #endregion

        #region RemoteStop(EVSEId, SessionId, ReservationHandling, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Stop the given charging session at the given EVSE.
        /// </summary>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="EVSEId">The unique identification of the EVSE to be stopped.</param>
        /// <param name="PartnerSessionId">The unique identification for the partner charging session.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static async Task<HTTPResponse<Acknowledgement<AuthorizeRemoteStopRequest>>>

            RemoteStop(this IEMPClient       IEMPClient,
                       Session_Id            SessionId,
                       Provider_Id           ProviderId,
                       EVSE_Id               EVSEId,
                       PartnerSession_Id?    PartnerSessionId    = null,

                       DateTime?             Timestamp           = null,
                       CancellationToken?    CancellationToken   = null,
                       EventTracking_Id      EventTrackingId     = null,
                       TimeSpan?             RequestTimeout      = null)


            => await IEMPClient.RemoteStop(new AuthorizeRemoteStopRequest(SessionId,
                                                                          ProviderId,
                                                                          EVSEId,
                                                                          PartnerSessionId,

                                                                          Timestamp,
                                                                          CancellationToken,
                                                                          EventTrackingId,
                                                                          RequestTimeout.HasValue ? RequestTimeout.Value : IEMPClient.RequestTimeout));

        #endregion


        #region GetChargeDetailRecords(ProviderId, From, To, ...)

        /// <summary>
        /// Create a new task querying charge detail records from the OICP server.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="From">The starting time.</param>
        /// <param name="To">An optional end time. [default: current time].</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static async Task<HTTPResponse<GetChargeDetailRecordsResponse>>

            GetChargeDetailRecords(this IEMPClient     IEMPClient,
                                   Provider_Id         ProviderId,
                                   DateTime            From,
                                   DateTime            To,

                                   DateTime?           Timestamp           = null,
                                   CancellationToken?  CancellationToken   = null,
                                   EventTracking_Id    EventTrackingId     = null,
                                   TimeSpan?           RequestTimeout      = null)


            => await IEMPClient.GetChargeDetailRecords(new GetChargeDetailRecordsRequest(ProviderId,
                                                                                         From,
                                                                                         To,

                                                                                         Timestamp,
                                                                                         CancellationToken,
                                                                                         EventTrackingId,
                                                                                         RequestTimeout.HasValue ? RequestTimeout.Value : IEMPClient.RequestTimeout));

        #endregion

    }

}
