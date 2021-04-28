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

using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    /// <summary>
    /// The common interface for all CPO clients.
    /// </summary>
    public interface ICPOClient : IHTTPClient
    {

        /// <summary>
        /// Upload the given EVSE data records.
        /// </summary>
        /// <param name="Request">A PushEVSEData request.</param>
        Task<OICPResult<Acknowledgement<PushEVSEDataRequest>>>                   PushEVSEData                     (PushEVSEDataRequest                   Request);

        /// <summary>
        /// Upload the given EVSE status records.
        /// </summary>
        /// <param name="Request">A PushEVSEStatus request.</param>
        Task<OICPResult<Acknowledgement<PushEVSEStatusRequest>>>                 PushEVSEStatus                   (PushEVSEStatusRequest                 Request);


        /// <summary>
        /// Create an AuthorizeStart request.
        /// </summary>
        /// <param name="Request">An AuthorizeStart request.</param>
        Task<OICPResult<AuthorizationStartResponse>>                             AuthorizeStart                   (AuthorizeStartRequest                 Request);

        /// <summary>
        /// Create an AuthorizeStop request.
        /// </summary>
        /// <param name="Request">An AuthorizeStop request.</param>
        Task<OICPResult<AuthorizationStopResponse>>                              AuthorizeStop                    (AuthorizeStopRequest                  Request);


        /// <summary>
        /// Send a charging start notification.
        /// </summary>
        /// <param name="Request">A ChargingNotificationsStart request.</param>
        Task<OICPResult<Acknowledgement<ChargingNotificationsStartRequest>>>     SendChargingNotificationsStart   (ChargingNotificationsStartRequest     Request);

        /// <summary>
        /// Send a charging progress notification.
        /// </summary>
        /// <param name="Request">A ChargingNotificationsProgress request.</param>
        Task<OICPResult<Acknowledgement<ChargingNotificationsProgressRequest>>>  SendChargingNotificationsProgress(ChargingNotificationsProgressRequest  Request);

        /// <summary>
        /// Send a charging end notification.
        /// </summary>
        /// <param name="Request">A ChargingNotificationsEnd request.</param>
        Task<OICPResult<Acknowledgement<ChargingNotificationsEndRequest>>>       SendChargingNotificationsEnd     (ChargingNotificationsEndRequest       Request);

        /// <summary>
        /// Send a charging error notification.
        /// </summary>
        /// <param name="Request">A ChargingNotificationsError request.</param>
        Task<OICPResult<Acknowledgement<ChargingNotificationsErrorRequest>>>     SendChargingNotificationsError   (ChargingNotificationsErrorRequest     Request);


        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="Request">A SendChargeDetailRecord request.</param>
        Task<OICPResult<Acknowledgement<SendChargeDetailRecordRequest>>>         SendChargeDetailRecord           (SendChargeDetailRecordRequest         Request);

    }

}
