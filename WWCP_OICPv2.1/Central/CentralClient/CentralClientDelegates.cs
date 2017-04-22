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
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.Central
{

    // Towards CPOs

    #region OnAuthorizeRemoteReservationStart/-Stop

    /// <summary>
    /// A delegate called whenever a 'reservation start' request will be send.
    /// </summary>
    public delegate Task OnAuthorizeRemoteReservationStartRequestHandler (DateTime                                                      LogTimestamp,
                                                                          DateTime                                                      RequestTimestamp,
                                                                          CentralClient                                                 Sender,
                                                                          String                                                        SenderId,
                                                                          EventTracking_Id                                              EventTrackingId,
                                                                          Provider_Id                                                   ProviderId,
                                                                          EVSE_Id                                                       EVSEId,
                                                                          EVCO_Id                                                       EVCOId,
                                                                          Session_Id?                                                   SessionId,
                                                                          PartnerSession_Id?                                            PartnerSessionId,
                                                                          PartnerProduct_Id?                                            PartnerProductId,
                                                                          TimeSpan                                                      RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response for a 'reservation start' request had been received.
    /// </summary>
    public delegate Task OnAuthorizeRemoteReservationStartResponseHandler(DateTime                                                      Timestamp,
                                                                          CentralClient                                                 Sender,
                                                                          String                                                        SenderId,
                                                                          EventTracking_Id                                              EventTrackingId,
                                                                          Provider_Id                                                   ProviderId,
                                                                          EVSE_Id                                                       EVSEId,
                                                                          EVCO_Id                                                       EVCOId,
                                                                          Session_Id?                                                   SessionId,
                                                                          PartnerSession_Id?                                            PartnerSessionId,
                                                                          PartnerProduct_Id?                                            PartnerProductId,
                                                                          TimeSpan                                                      RequestTimeout,
                                                                          Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>   Result,
                                                                          TimeSpan                                                      Duration);


    /// <summary>
    /// A delegate called whenever a reservation stop request will be send.
    /// </summary>
    public delegate Task OnAuthorizeRemoteReservationStopRequestHandler  (DateTime                                                      LogTimestamp,
                                                                          DateTime                                                      RequestTimestamp,
                                                                          CentralClient                                                 Sender,
                                                                          String                                                        SenderId,
                                                                          EventTracking_Id                                              EventTrackingId,
                                                                          Session_Id                                                    SessionId,
                                                                          Provider_Id                                                   ProviderId,
                                                                          EVSE_Id                                                       EVSEId,
                                                                          PartnerSession_Id?                                            PartnerSessionId,
                                                                          TimeSpan                                                      RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response for a reservation stop request had been received.
    /// </summary>
    public delegate Task OnAuthorizeRemoteReservationStopResponseHandler (DateTime                                                      Timestamp,
                                                                          CentralClient                                                 Sender,
                                                                          String                                                        SenderId,
                                                                          EventTracking_Id                                              EventTrackingId,
                                                                          Session_Id                                                    SessionId,
                                                                          Provider_Id                                                   ProviderId,
                                                                          EVSE_Id                                                       EVSEId,
                                                                          PartnerSession_Id?                                            PartnerSessionId,
                                                                          TimeSpan                                                      RequestTimeout,
                                                                          Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>    Result,
                                                                          TimeSpan                                                      Duration);

    #endregion



    // Towards EMPs

    #region OnAuthorizeStart/-Stop

    /// <summary>
    /// A delegate called whenever an 'authorize start' request will be send.
    /// </summary>
    public delegate Task OnAuthorizeStartHandler  (DateTime                     LogTimestamp,
                                                   DateTime                     RequestTimestamp,
                                                   CentralClient                Sender,
                                                   String                       SenderId,
                                                   Operator_Id                  OperatorId,
                                                   UID                          UID,
                                                   EVSE_Id?                     EVSEId,
                                                   Session_Id?                  SessionId,
                                                   PartnerProduct_Id?           PartnerProductId,
                                                   PartnerSession_Id?           PartnerSessionId,
                                                   TimeSpan                     RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response to a 'authorize start' request had been received.
    /// </summary>
    public delegate Task OnAuthorizeStartedHandler(DateTime                     Timestamp,
                                                   CentralClient                Sender,
                                                   String                       SenderId,
                                                   Operator_Id                  OperatorId,
                                                   UID                          UID,
                                                   EVSE_Id?                     EVSEId,
                                                   Session_Id?                  SessionId,
                                                   PartnerProduct_Id?           PartnerProductId,
                                                   PartnerSession_Id?           PartnerSessionId,
                                                   TimeSpan                     RequestTimeout,
                                                   CPO.AuthorizationStart       Result,
                                                   TimeSpan                     Duration);


    /// <summary>
    /// A delegate called whenever an 'authorize stop' request will be send.
    /// </summary>
    public delegate Task OnAuthorizeStopRequestHandler (DateTime                     LogTimestamp,
                                                        DateTime                     RequestTimestamp,
                                                        CentralClient               Sender,
                                                        String                       SenderId,
                                                        Operator_Id                  OperatorId,
                                                        Session_Id                   SessionId,
                                                        UID                          UID,
                                                        EVSE_Id?                     EVSEId,
                                                        PartnerSession_Id?           PartnerSessionId,
                                                        TimeSpan                     RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response to a 'authorize stop' request had been received.
    /// </summary>
    public delegate Task OnAuthorizeStopResponseHandler(DateTime                    Timestamp,
                                                        CentralClient               Sender,
                                                        String                      SenderId,
                                                        Operator_Id                 OperatorId,
                                                        Session_Id                  SessionId,
                                                        UID                         UID,
                                                        EVSE_Id?                    EVSEId,
                                                        PartnerSession_Id?          PartnerSessionId,
                                                        TimeSpan                    RequestTimeout,
                                                        CPO.AuthorizationStop       Result,
                                                        TimeSpan                    Duration);

    #endregion

    #region OnSendChargeDetailRecord

    /// <summary>
    /// A delegate called whenever a 'send charge detail record' request will be send.
    /// </summary>
    public delegate Task OnSendChargeDetailRecordRequestHandler (DateTime                                             LogTimestamp,
                                                                 DateTime                                             RequestTimestamp,
                                                                 ICentralClient                                       Sender,
                                                                 String                                               SenderId,
                                                                 EventTracking_Id                                     EventTrackingId,
                                                                 ChargeDetailRecord                                   ChargeDetailRecord,
                                                                 TimeSpan                                             RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response for a 'send charge detail record' request had been received.
    /// </summary>
    public delegate Task OnSendChargeDetailRecordResponseHandler(DateTime                                             Timestamp,
                                                                 ICentralClient                                       Sender,
                                                                 String                                               SenderId,
                                                                 EventTracking_Id                                     EventTrackingId,
                                                                 ChargeDetailRecord                                   ChargeDetailRecord,
                                                                 TimeSpan                                             RequestTimeout,
                                                                 Acknowledgement<CPO.SendChargeDetailRecordRequest>   Result,
                                                                 TimeSpan                                             Duration);

    #endregion

}
