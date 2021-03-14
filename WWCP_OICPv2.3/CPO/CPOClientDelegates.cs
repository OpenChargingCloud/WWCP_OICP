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
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO
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


    #region OnPushEVSEDataRequest/-Response

    /// <summary>
    /// A delegate called whenever new EVSE data record will be send upstream.
    /// </summary>
    public delegate Task OnPushEVSEDataRequestDelegate (DateTime                                            LogTimestamp,
                                                        DateTime                                            RequestTimestamp,
                                                        CPOClient                                           Sender,
                                                        //String                                              SenderId,
                                                        EventTracking_Id                                    EventTrackingId,
                                                        ActionTypes                                         Action,
                                                        UInt64                                              NumberOfEVSEDataRecords,
                                                        IEnumerable<EVSEDataRecord>                         EVSEDataRecords,
                                                        TimeSpan                                            RequestTimeout);

    /// <summary>
    /// A delegate called whenever new EVSE data record had been send upstream.
    /// </summary>
    public delegate Task OnPushEVSEDataResponseDelegate(DateTime                                            LogTimestamp,
                                                        DateTime                                            RequestTimestamp,
                                                        CPOClient                                           Sender,
                                                        //String                                              SenderId,
                                                        EventTracking_Id                                    EventTrackingId,
                                                        ActionTypes                                         Action,
                                                        UInt64                                              NumberOfEVSEDataRecords,
                                                        IEnumerable<EVSEDataRecord>                         EVSEDataRecords,
                                                        TimeSpan                                            RequestTimeout,
                                                        OICPResult<Acknowledgement<PushEVSEDataRequest>>    Result,
                                                        TimeSpan                                            Runtime);

    #endregion

    #region OnPushEVSEStatusRequest/-Response

    /// <summary>
    /// A delegate called whenever new EVSE status record will be send upstream.
    /// </summary>
    public delegate Task OnPushEVSEStatusRequestDelegate (DateTime                                              LogTimestamp,
                                                          DateTime                                              RequestTimestamp,
                                                          CPOClient                                             Sender,
                                                          //String                                                SenderId,
                                                          EventTracking_Id                                      EventTrackingId,
                                                          ActionTypes                                           Action,
                                                          UInt64                                                NumberOfEVSEStatusRecords,
                                                          IEnumerable<EVSEStatusRecord>                         EVSEStatusRecords,
                                                          TimeSpan                                              RequestTimeout);

    /// <summary>
    /// A delegate called whenever new EVSE status record had been send upstream.
    /// </summary>
    public delegate Task OnPushEVSEStatusResponseDelegate(DateTime                                              LogTimestamp,
                                                          DateTime                                              RequestTimestamp,
                                                          CPOClient                                             Sender,
                                                          //String                                                SenderId,
                                                          EventTracking_Id                                      EventTrackingId,
                                                          ActionTypes                                           Action,
                                                          UInt64                                                NumberOfEVSEStatusRecords,
                                                          IEnumerable<EVSEStatusRecord>                         EVSEStatusRecords,
                                                          TimeSpan                                              RequestTimeout,
                                                          OICPResult<Acknowledgement<PushEVSEStatusRequest>>    Result,
                                                          TimeSpan                                              Runtime);

    #endregion


    #region OnAuthorizeStartRequest/-Response

    /// <summary>
    /// A delegate called whenever an 'authorize start' request will be send.
    /// </summary>
    public delegate Task OnAuthorizeStartRequestHandler (DateTime                                  LogTimestamp,
                                                         DateTime                                  RequestTimestamp,
                                                         CPOClient                                 Sender,
                                                         //String                                    SenderId,
                                                         EventTracking_Id                          EventTrackingId,
                                                         Operator_Id                               OperatorId,
                                                         Identification                            Identification,
                                                         EVSE_Id?                                  EVSEId,
                                                         Session_Id?                               SessionId,
                                                         PartnerProduct_Id?                        PartnerProductId,
                                                         CPOPartnerSession_Id?                     CPOPartnerSessionId,
                                                         EMPPartnerSession_Id?                     EMPPartnerSessionId,
                                                         TimeSpan                                  RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response to an 'authorize start' request had been received.
    /// </summary>
    public delegate Task OnAuthorizeStartResponseHandler(DateTime                                  LogTimestamp,
                                                         DateTime                                  RequestTimestamp,
                                                         CPOClient                                 Sender,
                                                         //String                                    SenderId,
                                                         EventTracking_Id                          EventTrackingId,
                                                         Operator_Id                               OperatorId,
                                                         Identification                            Identification,
                                                         EVSE_Id?                                  EVSEId,
                                                         Session_Id?                               SessionId,
                                                         PartnerProduct_Id?                        PartnerProductId,
                                                         CPOPartnerSession_Id?                     CPOPartnerSessionId,
                                                         EMPPartnerSession_Id?                     EMPPartnerSessionId,
                                                         TimeSpan                                  RequestTimeout,
                                                         OICPResult<AuthorizationStartResponse>    Result,
                                                         TimeSpan                                  Runtime);

    #endregion

    #region OnAuthorizeStopRequest/-Response

    /// <summary>
    /// A delegate called whenever an 'authorize stop' request will be send.
    /// </summary>
    public delegate Task OnAuthorizeStopRequestHandler (DateTime                      LogTimestamp,
                                                        DateTime                      RequestTimestamp,
                                                        CPOClient                     Sender,
                                                        String                        SenderId,
                                                        EventTracking_Id              EventTrackingId,
                                                        Operator_Id                   OperatorId,
                                                        Session_Id                    SessionId,
                                                        Identification                Identification,
                                                        EVSE_Id?                      EVSEId,
                                                        CPOPartnerSession_Id?         CPOPartnerSessionId,
                                                        EMPPartnerSession_Id?         EMPPartnerSessionId,
                                                        TimeSpan                      RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response to an 'authorize stop' request had been received.
    /// </summary>
    public delegate Task OnAuthorizeStopResponseHandler(DateTime                      LogTimestamp,
                                                        DateTime                      RequestTimestamp,
                                                        CPOClient                    Sender,
                                                        String                        SenderId,
                                                        EventTracking_Id              EventTrackingId,
                                                        Operator_Id                   OperatorId,
                                                        Session_Id                    SessionId,
                                                        Identification                Identification,
                                                        EVSE_Id?                      EVSEId,
                                                        CPOPartnerSession_Id?         CPOPartnerSessionId,
                                                        EMPPartnerSession_Id?         EMPPartnerSessionId,
                                                        TimeSpan                      RequestTimeout,
                                                        AuthorizationStopResponse     Result,
                                                        TimeSpan                      Runtime);

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
                                                                 TimeSpan                                         Runtime);

    #endregion

}
