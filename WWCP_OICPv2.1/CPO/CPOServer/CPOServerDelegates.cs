/*
 * Copyright (c) 2014-2017 GraphDefined GmbH
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.CPO
{

    /// <summary>
    /// Initiate a remote start of the given charging session at the given EVSE
    /// and for the given Provider/eMAId.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="PartnerProductId">The unique identification of the choosen charging product at the given EVSE.</param>
    /// <param name="SessionId">The unique identification of this charging session.</param>
    /// <param name="PartnerSessionId">The unique identification of this charging session on the partner side.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
    /// <param name="EVCOId">The unique identification of the e-mobility account.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    public delegate Task<Acknowledgement>

        OnRemoteReservationStartDelegate(DateTime               Timestamp,
                                         CPOServer              Sender,
                                         CancellationToken      CancellationToken,
                                         EventTracking_Id       EventTrackingId,
                                         EVSE_Id                EVSEId,
                                         PartnerProduct_Id?     PartnerProductId,
                                         Session_Id?            SessionId,
                                         PartnerSession_Id?     PartnerSessionId,
                                         Provider_Id?           ProviderId,
                                         EVCO_Id?               EVCOId,
                                         TimeSpan?              RequestTimeout  = null);


    /// <summary>
    /// Initiate a remote stop of the given charging session at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="SessionId">The unique identification of this charging session.</param>
    /// <param name="PartnerSessionId">The unique identification of this charging session on the partner side.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender..</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    public delegate Task<Acknowledgement>

        OnRemoteReservationStopDelegate(DateTime               Timestamp,
                                        CPOServer              Sender,
                                        CancellationToken      CancellationToken,
                                        EventTracking_Id       EventTrackingId,
                                        EVSE_Id                EVSEId,
                                        Session_Id?            SessionId,
                                        PartnerSession_Id?     PartnerSessionId,
                                        Provider_Id?           ProviderId,
                                        TimeSpan?              RequestTimeout  = null);

    /// <summary>
    /// Initiate a remote start of the given charging session at the given EVSE
    /// and for the given Provider/eMAId.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="PartnerProductId">The unique identification of the choosen charging product at the given EVSE.</param>
    /// <param name="SessionId">The unique identification of this charging session.</param>
    /// <param name="PartnerSessionId">The unique identification of this charging session on the partner side.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
    /// <param name="EVCOId">The unique identification of the e-mobility account.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    public delegate Task<Acknowledgement>

        OnRemoteStartDelegate(DateTime               Timestamp,
                              CPOServer              Sender,
                              CancellationToken      CancellationToken,
                              EventTracking_Id       EventTrackingId,
                              EVSE_Id                EVSEId,
                              PartnerProduct_Id?     PartnerProductId,
                              Session_Id?            SessionId,
                              PartnerSession_Id?     PartnerSessionId,
                              Provider_Id?           ProviderId,
                              EVCO_Id?               EVCOId,
                              TimeSpan?              RequestTimeout  = null);


    /// <summary>
    /// Initiate a remote stop of the given charging session at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="SessionId">The unique identification of this charging session.</param>
    /// <param name="PartnerSessionId">The unique identification of this charging session on the partner side.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender..</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    public delegate Task<Acknowledgement>

        OnRemoteStopDelegate(DateTime               Timestamp,
                             CPOServer              Sender,
                             CancellationToken      CancellationToken,
                             EventTracking_Id       EventTrackingId,
                             EVSE_Id                EVSEId,
                             Session_Id             SessionId,
                             PartnerSession_Id?     PartnerSessionId,
                             Provider_Id?           ProviderId,
                             TimeSpan?              RequestTimeout  = null);

}
