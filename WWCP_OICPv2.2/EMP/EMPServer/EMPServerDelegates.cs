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
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2.EMP
{

    #region OnAuthorizeStart    (Request|Response)Handler

    /// <summary>
    /// A delegate called whenever a authorize start request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeStartRequestDelegate (DateTime                 LogTimestamp,
                                         DateTime                 RequestTimestamp,
                                         EMPServer                Sender,
                                         String                   SenderId,
                                         EventTracking_Id         EventTrackingId,
                                         Operator_Id              OperatorId,
                                         Identification           Identification,
                                         EVSE_Id?                 EVSEId,
                                         Session_Id?              SessionId,
                                         PartnerProduct_Id?       PartnerProductId,
                                         CPOPartnerSession_Id?    CPOPartnerSessionId,
                                         EMPPartnerSession_Id?    EMPPartnerSessionId,
                                         TimeSpan?                RequestTimeout);


    /// <summary>
    /// Initiate an AuthorizeStart for the given AuthToken at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">An AuthorizeStart request.</param>
    public delegate Task<CPO.AuthorizationStart>

        OnAuthorizeStartDelegate(DateTime                   Timestamp,
                                 EMPServer                  Sender,
                                 CPO.AuthorizeStartRequest  Request);


    /// <summary>
    /// A delegate called whenever a authorize start response was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeStartResponseDelegate(DateTime                 Timestamp,
                                         EMPServer                Sender,
                                         String                   SenderId,
                                         EventTracking_Id         EventTrackingId,
                                         Operator_Id              OperatorId,
                                         Identification           Identification,
                                         EVSE_Id?                 EVSEId,
                                         Session_Id?              SessionId,
                                         PartnerProduct_Id?       PartnerProductId,
                                         CPOPartnerSession_Id?    CPOPartnerSessionId,
                                         EMPPartnerSession_Id?    EMPPartnerSessionId,
                                         TimeSpan?                RequestTimeout,
                                         CPO.AuthorizationStart   Result,
                                         TimeSpan                 Duration);

    #endregion

    #region OnAuthorizeStop     (Request|Response)Handler

    /// <summary>
    /// A delegate called whenever a authorize stop request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeStopRequestHandler (DateTime               LogTimestamp,
                                       DateTime               RequestTimestamp,
                                       EMPServer              Sender,
                                       String                 SenderId,
                                       EventTracking_Id       EventTrackingId,
                                       Session_Id?            SessionId,
                                       CPOPartnerSession_Id?  CPOPartnerSessionId,
                                       EMPPartnerSession_Id?  EMPPartnerSessionId,
                                       Operator_Id            OperatorId,
                                       EVSE_Id?               EVSEId,
                                       Identification         Identification,
                                       TimeSpan               RequestTimeout);


    /// <summary>
    /// Initiate a remote stop of the given charging session at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<CPO.AuthorizationStop>

        OnAuthorizeStopDelegate(DateTime                  Timestamp,
                                EMPServer                 Sender,
                                CPO.AuthorizeStopRequest  Request);


    /// <summary>
    /// A delegate called whenever a authorize stop response was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeStopResponseHandler(DateTime                Timestamp,
                                       EMPServer               Sender,
                                       String                  SenderId,
                                       EventTracking_Id        EventTrackingId,
                                       Session_Id?             SessionId,
                                       CPOPartnerSession_Id?   CPOPartnerSessionId,
                                       EMPPartnerSession_Id?   EMPPartnerSessionId,
                                       Operator_Id             OperatorId,
                                       EVSE_Id?                EVSEId,
                                       Identification          Identification,
                                       TimeSpan                RequestTimeout,
                                       CPO.AuthorizationStop   Result,
                                       TimeSpan                Duration);

    #endregion

    #region OnChargeDetailRecord(Request|Response)Handler

    /// <summary>
    /// A delegate called whenever a charge detail record request was received.
    /// </summary>
    public delegate Task

        OnChargeDetailRecordRequestHandler (DateTime              LogTimestamp,
                                            DateTime              RequestTimestamp,
                                            EMPServer             Sender,
                                            String                SenderId,
                                            EventTracking_Id      EventTrackingId,
                                            ChargeDetailRecord    ChargeDetailRecord,
                                            TimeSpan              RequestTimeout);


    /// <summary>
    /// Send a charge detail record.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<Acknowledgement<CPO.SendChargeDetailRecordRequest>>

        OnChargeDetailRecordDelegate       (DateTime                           Timestamp,
                                            EMPServer                          Sender,
                                            CPO.SendChargeDetailRecordRequest  Request);


    /// <summary>
    /// A delegate called whenever a charge detail record response was sent.
    /// </summary>
    public delegate Task

        OnChargeDetailRecordResponseHandler(DateTime                                              Timestamp,
                                            EMPServer                                             Sender,
                                            String                                                SenderId,
                                            EventTracking_Id                                      EventTrackingId,
                                            ChargeDetailRecord                                    ChargeDetailRecord,
                                            TimeSpan                                              RequestTimeout,
                                            Acknowledgement<CPO.SendChargeDetailRecordRequest>    Result,
                                            TimeSpan                                              Duration);

    #endregion

}
