/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
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
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.Central
{

    /// <summary>
    /// Extention methods for the Central client interface.
    /// </summary>
    public static class ICentralClientExtentions
    {

        #region AuthorizeRemoteReservationStart(ProviderId, EVSEId, Identification, Duration = null, SessionId = null, PartnerSessionId = null, PartnerProductId = null, ...)

        /// <summary>
        /// Create an OICP AuthorizeRemoteReservationStart XML/SOAP request.
        /// </summary>
        /// <param name="ProviderId">An e-mobility provider identification.</param>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="Identification">An identification, e.g. an electric vehicle contract identification.</param>
        /// <param name="Duration">The duration of the reservation (max. 99 minutes).</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="PartnerProductId">An optional partner product identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>>>

            AuthorizeRemoteReservationStart(this ICentralClient   ICentralClient,
                                            Provider_Id           ProviderId,
                                            EVSE_Id               EVSEId,
                                            Identification        Identification,
                                            TimeSpan?             Duration            = null,
                                            Session_Id?           SessionId           = null,
                                            PartnerSession_Id?    PartnerSessionId    = null,
                                            PartnerProduct_Id?    PartnerProductId    = null,

                                            DateTime?             Timestamp           = null,
                                            CancellationToken?    CancellationToken   = null,
                                            EventTracking_Id      EventTrackingId     = null,
                                            TimeSpan?             RequestTimeout      = null)


                => ICentralClient.AuthorizeRemoteReservationStart(new EMP.AuthorizeRemoteReservationStartRequest(ProviderId,
                                                                                                                 EVSEId,
                                                                                                                 Identification,
                                                                                                                 Duration,
                                                                                                                 SessionId,
                                                                                                                 PartnerSessionId,
                                                                                                                 PartnerProductId,

                                                                                                                 Timestamp,
                                                                                                                 CancellationToken,
                                                                                                                 EventTrackingId,
                                                                                                                 RequestTimeout.HasValue
                                                                                                                     ? RequestTimeout.Value
                                                                                                                     : ICentralClient.RequestTimeout));

        #endregion

        #region AuthorizeRemoteReservationStop (SessionId, ProviderId, EVSEId, PartnerSessionId = null, ...)

        /// <summary>
        /// Create an OICP AuthorizeRemoteReservationStop XML/SOAP request.
        /// </summary>
        /// <param name="SessionId">A charging session identification.</param>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        public static Task<HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>>>

            AuthorizeRemoteReservationStop(this ICentralClient  ICentralClient,
                                           Session_Id           SessionId,
                                           Provider_Id          ProviderId,
                                           EVSE_Id              EVSEId,
                                           PartnerSession_Id?   PartnerSessionId    = null,

                                           DateTime?            Timestamp           = null,
                                           CancellationToken?   CancellationToken   = null,
                                           EventTracking_Id     EventTrackingId     = null,
                                           TimeSpan?            RequestTimeout      = null)


                => ICentralClient.AuthorizeRemoteReservationStop(new EMP.AuthorizeRemoteReservationStopRequest(SessionId,
                                                                                                               ProviderId,
                                                                                                               EVSEId,
                                                                                                               PartnerSessionId,

                                                                                                               Timestamp,
                                                                                                               CancellationToken,
                                                                                                               EventTrackingId,
                                                                                                               RequestTimeout.HasValue
                                                                                                                   ? RequestTimeout.Value
                                                                                                                   : ICentralClient.RequestTimeout));

        #endregion


        #region AuthorizeRemoteStart(ProviderId, EVSEId, EVCOId, SessionId = null, PartnerSessionId = null, PartnerProductId = null, ...)

        /// <summary>
        /// Create an OICP AuthorizeRemoteStart XML/SOAP request.
        /// </summary>
        /// <param name="ProviderId">An e-mobility provider identification.</param>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="EVCOId">An electric vehicle contract identification.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="PartnerProductId">An optional partner product identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteStartRequest>>>

            AuthorizeRemoteStart(this ICentralClient   ICentralClient,
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


                => ICentralClient.AuthorizeRemoteStart(new EMP.AuthorizeRemoteStartRequest(ProviderId,
                                                                                           EVSEId,
                                                                                           EVCOId,
                                                                                           SessionId,
                                                                                           PartnerSessionId,
                                                                                           PartnerProductId,

                                                                                           Timestamp,
                                                                                           CancellationToken,
                                                                                           EventTrackingId,
                                                                                           RequestTimeout.HasValue
                                                                                               ? RequestTimeout.Value
                                                                                               : ICentralClient.RequestTimeout));

        #endregion

        #region AuthorizeRemoteStop (SessionId, ProviderId, EVSEId, PartnerSessionId = null, ...)

        /// <summary>
        /// Create an OICP AuthorizeRemoteStop XML/SOAP request.
        /// </summary>
        /// <param name="SessionId">A charging session identification.</param>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        public static Task<HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteStopRequest>>>

            AuthorizeRemoteStop(this ICentralClient  ICentralClient,
                                Session_Id           SessionId,
                                Provider_Id          ProviderId,
                                EVSE_Id              EVSEId,
                                PartnerSession_Id?   PartnerSessionId    = null,

                                DateTime?            Timestamp           = null,
                                CancellationToken?   CancellationToken   = null,
                                EventTracking_Id     EventTrackingId     = null,
                                TimeSpan?            RequestTimeout      = null)


                => ICentralClient.AuthorizeRemoteStop(new EMP.AuthorizeRemoteStopRequest(SessionId,
                                                                                         ProviderId,
                                                                                         EVSEId,
                                                                                         PartnerSessionId,

                                                                                         Timestamp,
                                                                                         CancellationToken,
                                                                                         EventTrackingId,
                                                                                         RequestTimeout.HasValue
                                                                                             ? RequestTimeout.Value
                                                                                             : ICentralClient.RequestTimeout));

        #endregion

    }

}
