/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.EMP
{

    /// <summary>
    /// The common interface for all EMP clients.
    /// </summary>
    public interface IEMPClient : IHTTPClient
    {

        /// <summary>
        /// Download EVSE data records.
        /// The request might either have none, 'SearchCenter + DistanceKM' or 'LastCall' parameters.
        /// Because of limitations at Hubject the SearchCenter and LastCall parameters can not be used at the same time!
        /// </summary>
        /// <param name="Request">A PullEVSEData request.</param>
        Task<OICPResult<PullEVSEDataResponse>>                                     PullEVSEData                   (PullEVSEDataRequest                     Request);

        /// <summary>
        /// Download EVSE status records.
        /// The request might have an optional search radius and/or status filter.
        /// </summary>
        /// <param name="Request">A PullEVSEStatus request.</param>
        Task<OICPResult<PullEVSEStatusResponse>>                                   PullEVSEStatus                 (PullEVSEStatusRequest                   Request);

        /// <summary>
        /// Download the current status of up to 100 EVSEs.
        /// </summary>
        /// <param name="Request">A PullEVSEStatusById request.</param>
        Task<OICPResult<PullEVSEStatusByIdResponse>>                               PullEVSEStatusById             (PullEVSEStatusByIdRequest               Request);

        /// <summary>
        /// Download the current EVSE status of the given charge point operators.
        /// </summary>
        /// <param name="Request">A PullEVSEStatusByOperatorId request.</param>
        Task<OICPResult<PullEVSEStatusByOperatorIdResponse>>                       PullEVSEStatusByOperatorId     (PullEVSEStatusByOperatorIdRequest       Request);



        /// <summary>
        /// Download pricing product data.
        /// </summary>
        /// <param name="Request">A PullPricingProductData request.</param>
        Task<OICPResult<PullPricingProductDataResponse>>                           PullPricingProductData         (PullPricingProductDataRequest           Request);

        /// <summary>
        /// Download EVSE pricing data.
        /// </summary>
        /// <param name="Request">A PullEVSEPricing request.</param></param>
        Task<OICPResult<PullEVSEPricingResponse>>                                  PullEVSEPricing                (PullEVSEPricingRequest                  Request);



        /// <summary>
        /// Upload provider authentication data records.
        /// </summary>
        /// <param name="Request">A PushAuthenticationData request.</param>
        Task<OICPResult<Acknowledgement<PushAuthenticationDataRequest>>>           PushAuthenticationData         (PushAuthenticationDataRequest           Request);



        /// <summary>
        /// Create a charging reservation at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteReservationStart request.</param>
        Task<OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>>  AuthorizeRemoteReservationStart(AuthorizeRemoteReservationStartRequest  Request);

        /// <summary>
        /// Stop the given charging reservation.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteReservationStop request.</param>
        Task<OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>>   AuthorizeRemoteReservationStop (AuthorizeRemoteReservationStopRequest   Request);


        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteStart request.</param>
        Task<OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>>             AuthorizeRemoteStart           (AuthorizeRemoteStartRequest             Request);

        /// <summary>
        /// Stop the given charging session.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteStop request.</param>
        Task<OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>>              AuthorizeRemoteStop            (AuthorizeRemoteStopRequest              Request);



        /// <summary>
        /// Download charge detail records.
        /// </summary>
        /// <param name="Request">An GetChargeDetailRecords request.</param>
        Task<OICPResult<GetChargeDetailRecordsResponse>>                           GetChargeDetailRecords         (GetChargeDetailRecordsRequest           Request);

    }

}
