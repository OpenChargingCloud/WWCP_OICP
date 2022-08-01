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
        Task<OICPResult<Acknowledgement<PushEVSEDataRequest>>>                  PushEVSEData                    (PushEVSEDataRequest                  Request);

        /// <summary>
        /// Upload the given EVSE status records.
        /// </summary>
        /// <param name="Request">A PushEVSEStatus request.</param>
        Task<OICPResult<Acknowledgement<PushEVSEStatusRequest>>>                PushEVSEStatus                  (PushEVSEStatusRequest                Request);


        /// <summary>
        /// Upload the given pricing product data.
        /// </summary>
        /// <param name="Request">A PushPricingProductData request.</param>
        Task<OICPResult<Acknowledgement<PushPricingProductDataRequest>>>        PushPricingProductData          (PushPricingProductDataRequest        Request);

        /// <summary>
        /// Upload the given EVSE pricing data.
        /// </summary>
        /// <param name="Request">A PushEVSEPricing request.</param>
        Task<OICPResult<Acknowledgement<PushEVSEPricingRequest>>>               PushEVSEPricing                 (PushEVSEPricingRequest               Request);


        /// <summary>
        /// Authorize for starting a charging session.
        /// </summary>
        /// <param name="Request">An AuthorizeStart request.</param>
        Task<OICPResult<AuthorizationStartResponse>>                            AuthorizeStart                  (AuthorizeStartRequest                Request);

        /// <summary>
        /// Authorize for stopping a charging session.
        /// </summary>
        /// <param name="Request">An AuthorizeStop request.</param>
        Task<OICPResult<AuthorizationStopResponse>>                             AuthorizeStop                   (AuthorizeStopRequest                 Request);


        /// <summary>
        /// Send a charging start notification.
        /// </summary>
        /// <param name="Request">A ChargingStartNotification request.</param>
        Task<OICPResult<Acknowledgement<ChargingStartNotificationRequest>>>     SendChargingStartNotification   (ChargingStartNotificationRequest     Request);

        /// <summary>
        /// Send a charging progress notification.
        /// </summary>
        /// <param name="Request">A ChargingProgressNotification request.</param>
        Task<OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>>  SendChargingProgressNotification(ChargingProgressNotificationRequest  Request);

        /// <summary>
        /// Send a charging end notification.
        /// </summary>
        /// <param name="Request">A ChargingEndNotification request.</param>
        Task<OICPResult<Acknowledgement<ChargingEndNotificationRequest>>>       SendChargingEndNotification     (ChargingEndNotificationRequest       Request);

        /// <summary>
        /// Send a charging error notification.
        /// </summary>
        /// <param name="Request">A ChargingErrorNotification request.</param>
        Task<OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>>     SendChargingErrorNotification   (ChargingErrorNotificationRequest     Request);


        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="Request">A SendChargeDetailRecord request.</param>
        Task<OICPResult<Acknowledgement<ChargeDetailRecordRequest>>>            SendChargeDetailRecord          (ChargeDetailRecordRequest            Request);

    }

}
