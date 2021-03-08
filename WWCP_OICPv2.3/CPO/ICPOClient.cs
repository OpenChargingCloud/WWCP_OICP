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

using System;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    /// <summary>
    /// Extention methods for the CPO client interface.
    /// </summary>
    public static class ICPOClientExtentions
    {

        #region AuthorizeStart(...)

        /// <summary>
        /// Create a new AuthorizeStart request.
        /// </summary>
        /// <param name="CPOClient">A CPO client.</param>
        /// <param name="OperatorId">The unqiue identification of the charging station operator.</param>
        /// <param name="Identification">Authentication data used to authorize the user or the car.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="PartnerProductId">An optional partner product identification (for identifying a charging tariff).</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="CustomData">Optional custom data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static async Task<OICPResult<AuthorizationStartResponse>> AuthorizeStart(this ICPOClient        CPOClient,
                                                                                        Operator_Id            OperatorId,
                                                                                        Identification         Identification,
                                                                                        EVSE_Id?               EVSEId                = null,
                                                                                        PartnerProduct_Id?     PartnerProductId      = null,
                                                                                        CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                                                                                        JObject                CustomData            = null,

                                                                                        DateTime?              Timestamp             = null,
                                                                                        CancellationToken?     CancellationToken     = null,
                                                                                        EventTracking_Id       EventTrackingId       = null,
                                                                                        TimeSpan?              RequestTimeout        = null)
        {

            return await CPOClient.AuthorizeStart(new AuthorizeStartRequest(
                                                      OperatorId,
                                                      Identification,
                                                      EVSEId,
                                                      PartnerProductId, // PartnerProductId will not be shown in the Hubject portal!
                                                      null,             // SessionId will be ignored by Hubject!
                                                      CPOPartnerSessionId,
                                                      null,             // EMPPartnerSessionId does not make much sense here!
                                                      CustomData,

                                                      Timestamp,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      RequestTimeout));

        }

        #endregion

        #region AuthorizeStop (...)

        /// <summary>
        /// Create a new AuthorizeStop request.
        /// </summary>
        /// <param name="CPOClient">A CPO client.</param>
        /// <param name="OperatorId">The unqiue identification of the charging station operator.</param>
        /// <param name="SessionId">The charging session identification.</param>
        /// <param name="Identification">Authentication data used to authorize the user or the car.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="CustomData">Optional custom data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static async Task<OICPResult<AuthorizationStopResponse>> AuthorizeStop(this ICPOClient        CPOClient,
                                                                                      Operator_Id            OperatorId,
                                                                                      Session_Id             SessionId,
                                                                                      Identification         Identification,
                                                                                      EVSE_Id?               EVSEId                = null,
                                                                                      CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                                                                                      EMPPartnerSession_Id?  EMPPartnerSessionId   = null,
                                                                                      JObject                CustomData            = null,

                                                                                      DateTime?              Timestamp             = null,
                                                                                      CancellationToken?     CancellationToken     = null,
                                                                                      EventTracking_Id       EventTrackingId       = null,
                                                                                      TimeSpan?              RequestTimeout        = null)
        {

            return await CPOClient.AuthorizeStop(new AuthorizeStopRequest(
                                                     OperatorId,
                                                     SessionId,
                                                     Identification,
                                                     EVSEId,
                                                     CPOPartnerSessionId,
                                                     EMPPartnerSessionId,
                                                     CustomData,

                                                     Timestamp,
                                                     CancellationToken,
                                                     EventTrackingId,
                                                     RequestTimeout));

        }

        #endregion


    }


    /// <summary>
    /// The common interface for all CPO clients.
    /// </summary>
    public interface ICPOClient
    {

        /// <summary>
        /// Upload the given EVSE data records.
        /// </summary>
        /// <param name="Request">A PushEVSEData request.</param>
        Task<OICPResult<Acknowledgement<PushEVSEDataRequest>>>            PushEVSEData          (PushEVSEDataRequest           Request);

        /// <summary>
        /// Upload the given EVSE status records.
        /// </summary>
        /// <param name="Request">A PushEVSEStatus request.</param>
        Task<OICPResult<Acknowledgement<PushEVSEStatusRequest>>>          PushEVSEStatus        (PushEVSEStatusRequest         Request);


        /// <summary>
        /// Create an AuthorizeStart request.
        /// </summary>
        /// <param name="Request">An AuthorizeStart request.</param>
        Task<OICPResult<AuthorizationStartResponse>>                      AuthorizeStart        (AuthorizeStartRequest         Request);

        /// <summary>
        /// Create an AuthorizeStop request.
        /// </summary>
        /// <param name="Request">An AuthorizeStop request.</param>
        Task<OICPResult<AuthorizationStopResponse>>                       AuthorizeStop         (AuthorizeStopRequest          Request);

        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="Request">A SendChargeDetailRecord request.</param>
        Task<OICPResult<Acknowledgement<SendChargeDetailRecordRequest>>>  SendChargeDetailRecord(SendChargeDetailRecordRequest Request);

    }

}
