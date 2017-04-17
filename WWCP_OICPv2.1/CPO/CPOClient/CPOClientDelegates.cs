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
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.CPO
{

    /// <summary>
    /// A delegate for filtering EVSE data records.
    /// </summary>
    /// <param name="EVSEDataRecord">An EVSE data record.</param>
    public delegate Boolean IncludeEVSEDataRecordsDelegate  (EVSEDataRecord    EVSEDataRecord);

    /// <summary>
    /// A delegate for filtering EVSE status records.
    /// </summary>
    /// <param name="EVSEStatusRecord">An EVSE status record.</param>
    public delegate Boolean IncludeEVSEStatusRecordsDelegate(EVSEStatusRecord  EVSEStatusRecord);


    #region OnPushEVSEData/-Status

    /// <summary>
    /// A delegate called whenever new EVSE data record will be send upstream.
    /// </summary>
    public delegate Task OnPushEVSEDataRequestDelegate (DateTime                                LogTimestamp,
                                                        DateTime                                RequestTimestamp,
                                                        ICPOClient                              Sender,
                                                        String                                  SenderId,
                                                        EventTracking_Id                        EventTrackingId,
                                                        ActionTypes                             Action,
                                                        UInt64                                  NumberOfEVSEDataRecords,
                                                        IEnumerable<EVSEDataRecord>             EVSEDataRecords,
                                                        TimeSpan                                RequestTimeout);

    /// <summary>
    /// A delegate called whenever new EVSE data record had been send upstream.
    /// </summary>
    public delegate Task OnPushEVSEDataResponseDelegate(DateTime                                LogTimestamp,
                                                        DateTime                                RequestTimestamp,
                                                        ICPOClient                              Sender,
                                                        String                                  SenderId,
                                                        EventTracking_Id                        EventTrackingId,
                                                        ActionTypes                             Action,
                                                        UInt64                                  NumberOfEVSEDataRecords,
                                                        IEnumerable<EVSEDataRecord>             EVSEDataRecords,
                                                        TimeSpan                                RequestTimeout,
                                                        Acknowledgement<PushEVSEDataRequest>    Result,
                                                        TimeSpan                                Duration);


    /// <summary>
    /// A delegate called whenever new EVSE status will be send upstream.
    /// </summary>
    public delegate Task OnPushEVSEStatusRequestDelegate (DateTime                                 LogTimestamp,
                                                          DateTime                                 RequestTimestamp,
                                                          ICPOClient                               Sender,
                                                          String                                   SenderId,
                                                          EventTracking_Id                         EventTrackingId,
                                                          ActionTypes                              Action,
                                                          UInt64                                   NumberOfEVSEStatus,
                                                          IEnumerable<EVSEStatusRecord>            EVSEStatusRecords,
                                                          TimeSpan                                 RequestTimeout);

    /// <summary>
    /// A delegate called whenever new EVSE status had been send upstream.
    /// </summary>
    public delegate Task OnPushEVSEStatusResponseDelegate(DateTime                                 LogTimestamp,
                                                          DateTime                                 RequestTimestamp,
                                                          ICPOClient                               Sender,
                                                          String                                   SenderId,
                                                          EventTracking_Id                         EventTrackingId,
                                                          ActionTypes                              Action,
                                                          UInt64                                   NumberOfEVSEStatus,
                                                          IEnumerable<EVSEStatusRecord>            EVSEStatusRecords,
                                                          TimeSpan                                 RequestTimeout,
                                                          Acknowledgement<PushEVSEStatusRequest>   Result,
                                                          TimeSpan                                 Duration);

    #endregion

    #region OnAuthorizeStart/-Stop

    /// <summary>
    /// A delegate called whenever an 'authorize start' request will be send.
    /// </summary>
    public delegate Task OnAuthorizeStartHandler  (DateTime                       LogTimestamp,
                                                   DateTime                       RequestTimestamp,
                                                   CPOClient                      Sender,
                                                   String                         SenderId,
                                                   Operator_Id                    OperatorId,
                                                   UID                            UID,
                                                   EVSE_Id?                       EVSEId,
                                                   Session_Id?                    SessionId,
                                                   PartnerProduct_Id?             PartnerProductId,
                                                   PartnerSession_Id?             PartnerSessionId,
                                                   TimeSpan                       RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response to a 'authorize start' request had been received.
    /// </summary>
    public delegate Task OnAuthorizeStartedHandler(DateTime                     Timestamp,
                                                   CPOClient                    Sender,
                                                   String                       SenderId,
                                                   Operator_Id                  OperatorId,
                                                   UID                          UID,
                                                   EVSE_Id?                     EVSEId,
                                                   Session_Id?                  SessionId,
                                                   PartnerProduct_Id?           PartnerProductId,
                                                   PartnerSession_Id?           PartnerSessionId,
                                                   TimeSpan                     RequestTimeout,
                                                   AuthorizationStart           Result,
                                                   TimeSpan                     Duration);


    /// <summary>
    /// A delegate called whenever an 'authorize stop' request will be send.
    /// </summary>
    public delegate Task OnAuthorizeStopRequestHandler(DateTime                     LogTimestamp,
                                                       DateTime                     RequestTimestamp,
                                                       CPOClient                    Sender,
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
                                                        CPOClient                   Sender,
                                                        String                      SenderId,
                                                        Operator_Id                 OperatorId,
                                                        Session_Id                  SessionId,
                                                        UID                         UID,
                                                        EVSE_Id?                    EVSEId,
                                                        PartnerSession_Id?          PartnerSessionId,
                                                        TimeSpan                    RequestTimeout,
                                                        AuthorizationStop           Result,
                                                        TimeSpan                    Duration);

    #endregion

    #region OnSendChargeDetailRecord

    /// <summary>
    /// A delegate called whenever a 'charge detail record' will be send.
    /// </summary>
    public delegate Task OnSendChargeDetailRecordRequestHandler (DateTime                                         LogTimestamp,
                                                                 DateTime                                         RequestTimestamp,
                                                                 CPOClient                                        Sender,
                                                                 String                                           SenderId,
                                                                 EventTracking_Id                                 EventTrackingId,
                                                                 ChargeDetailRecord                               ChargeDetailRecord,
                                                                 TimeSpan                                         RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response for a sent 'charge detail record' had been received.
    /// </summary>
    public delegate Task OnSendChargeDetailRecordResponseHandler(DateTime                                         Timestamp,
                                                                 DateTime                                         RequestTimestamp,
                                                                 CPOClient                                        Sender,
                                                                 String                                           SenderId,
                                                                 EventTracking_Id                                 EventTrackingId,
                                                                 ChargeDetailRecord                               ChargeDetailRecord,
                                                                 TimeSpan                                         RequestTimeout,
                                                                 Acknowledgement<SendChargeDetailRecordRequest>   Result,
                                                                 TimeSpan                                         Duration);

    #endregion

    #region OnPullAuthenticationData

    /// <summary>
    /// A delegate called whenever a 'pull authentication data' request will be send.
    /// </summary>
    public delegate Task OnPullAuthenticationDataRequestHandler (DateTime                     LogTimestamp,
                                                                 DateTime                     RequestTimestamp,
                                                                 CPOClient                    Sender,
                                                                 String                       SenderId,
                                                                 EventTracking_Id             EventTrackingId,
                                                                 Operator_Id                  OperatorId,
                                                                 TimeSpan                     RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response for a 'pull authentication data' request had been received.
    /// </summary>
    public delegate Task OnPullAuthenticationDataResponseHandler(DateTime                     Timestamp,
                                                                 CPOClient                    Sender,
                                                                 String                       SenderId,
                                                                 EventTracking_Id             EventTrackingId,
                                                                 Operator_Id                  OperatorId,
                                                                 TimeSpan                     RequestTimeout,
                                                                 AuthenticationData           Result,
                                                                 TimeSpan                     Duration);

    #endregion

}
