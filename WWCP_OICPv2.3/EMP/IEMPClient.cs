/*
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

using System.Threading.Tasks;

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
        /// Upload the given EVSE data records.
        /// </summary>
        /// <param name="Request">A PullEVSEData request.</param>
        Task<OICPResult<PullEVSEDataResponse>>                                     PullEVSEData                   (PullEVSEDataRequest                     Request);

        /// <summary>
        /// Upload the given EVSE status records.
        /// </summary>
        /// <param name="Request">A PullEVSEStatus request.</param>
        Task<OICPResult<PullEVSEStatusResponse>>                                   PullEVSEStatus                 (PullEVSEStatusRequest                   Request);

        /// <summary>
        /// Create a new task requesting the current status of up to 100 EVSEs by their EVSE Ids.
        /// </summary>
        /// <param name="Request">A PullEVSEStatusById request.</param>
        Task<OICPResult<PullEVSEStatusByIdResponse>>                               PullEVSEStatusById             (PullEVSEStatusByIdRequest               Request);

        /// <summary>
        /// Create a new task requesting the current status of up to 100 EVSEs by their EVSE Ids.
        /// </summary>
        /// <param name="Request">A PullEVSEStatusByOperatorId request.</param>
        Task<OICPResult<PullEVSEStatusByOperatorIdResponse>>                       PullEVSEStatusByOperatorId     (PullEVSEStatusByOperatorIdRequest       Request);

        ///// <summary>
        ///// Create a new task pushing provider authentication data records onto the OICP server.
        ///// </summary>
        ///// <param name="Request">An PushAuthenticationData request.</param>
        //Task<OICPResult<Acknowledgement<PushAuthenticationDataRequest>>> PushAuthenticationData(PushAuthenticationDataRequest Request);

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteReservationStart request.</param>
        Task<OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>>  AuthorizeRemoteReservationStart(AuthorizeRemoteReservationStartRequest  Request);

        /// <summary>
        /// Stop the given charging session at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteReservationStop request.</param>
        Task<OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>>   AuthorizeRemoteReservationStop (AuthorizeRemoteReservationStopRequest   Request);

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteStart request.</param>
        Task<OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>>             AuthorizeRemoteStart           (AuthorizeRemoteStartRequest             Request);

        /// <summary>
        /// Stop the given charging session at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteStop request.</param>
        Task<OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>>              AuthorizeRemoteStop            (AuthorizeRemoteStopRequest              Request);

        /// <summary>
        /// Create a new task querying charge detail records from the OICP server.
        /// </summary>
        /// <param name="Request">An GetChargeDetailRecords request.</param>
        Task<OICPResult<GetChargeDetailRecordsResponse>>                           GetChargeDetailRecords         (GetChargeDetailRecordsRequest           Request);

    }

}
