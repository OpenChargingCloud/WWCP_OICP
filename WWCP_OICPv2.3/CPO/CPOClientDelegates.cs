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
    public delegate Task OnPushEVSEDataRequestDelegate (DateTime                                           Timestamp,
                                                        CPOClient                                          Sender,
                                                        String                                             SenderDescription,
                                                        PushEVSEDataRequest                                Request);

    /// <summary>
    /// A delegate called whenever new EVSE data record had been send upstream.
    /// </summary>
    public delegate Task OnPushEVSEDataResponseDelegate(DateTime                                           Timestamp,
                                                        CPOClient                                          Sender,
                                                        String                                             SenderDescription,
                                                        PushEVSEDataRequest                                Request,
                                                        OICPResult<Acknowledgement<PushEVSEDataRequest>>   Result);

    #endregion

    #region OnPushEVSEStatusRequest/-Response

    /// <summary>
    /// A delegate called whenever new EVSE status record will be send upstream.
    /// </summary>
    public delegate Task OnPushEVSEStatusRequestDelegate (DateTime                                             Timestamp,
                                                          CPOClient                                            Sender,
                                                          String                                               SenderDescription,
                                                          PushEVSEStatusRequest                                Request);

    /// <summary>
    /// A delegate called whenever new EVSE status record had been send upstream.
    /// </summary>
    public delegate Task OnPushEVSEStatusResponseDelegate(DateTime                                             Timestamp,
                                                          CPOClient                                            Sender,
                                                          String                                               SenderDescription,
                                                          PushEVSEStatusRequest                                Request,
                                                          OICPResult<Acknowledgement<PushEVSEStatusRequest>>   Result);

    #endregion


    #region OnAuthorizeStartRequest/-Response

    /// <summary>
    /// A delegate called whenever an AuthorizeStart request will be send.
    /// </summary>
    public delegate Task OnAuthorizeStartRequestDelegate (DateTime                                 Timestamp,
                                                          CPOClient                                Sender,
                                                          String                                   SenderDescription,
                                                          AuthorizeStartRequest                    Request);

    /// <summary>
    /// A delegate called whenever a response for an AuthorizeStart request had been received.
    /// </summary>
    public delegate Task OnAuthorizeStartResponseDelegate(DateTime                                 Timestamp,
                                                          CPOClient                                Sender,
                                                          String                                   SenderDescription,
                                                          AuthorizeStartRequest                    Request,
                                                          OICPResult<AuthorizationStartResponse>   Result);

    #endregion

    #region OnAuthorizeStopRequest/-Response

    /// <summary>
    /// A delegate called whenever an AuthorizeStop request will be send.
    /// </summary>
    public delegate Task OnAuthorizeStopRequestDelegate (DateTime                                Timestamp,
                                                         CPOClient                               Sender,
                                                         String                                  SenderDescription,
                                                         AuthorizeStopRequest                    Request);

    /// <summary>
    /// A delegate called whenever a response for an AuthorizeStop request had been received.
    /// </summary>
    public delegate Task OnAuthorizeStopResponseDelegate(DateTime                                Timestamp,
                                                         CPOClient                               Sender,
                                                         String                                  SenderDescription,
                                                         AuthorizeStopRequest                    Request,
                                                         OICPResult<AuthorizationStopResponse>   Result);

    #endregion


    #region OnChargingNotificationsStart

    /// <summary>
    /// A delegate called whenever a ChargingNotificationsStart will be send.
    /// </summary>
    public delegate Task OnChargingNotificationsStartRequestDelegate (DateTime                                                         Timestamp,
                                                                      CPOClient                                                        Sender,
                                                                      String                                                           SenderDescription,
                                                                      ChargingNotificationsStartRequest                                Request);

    /// <summary>
    /// A delegate called whenever a response for a ChargingNotificationsStart had been received.
    /// </summary>
    public delegate Task OnChargingNotificationsStartResponseDelegate(DateTime                                                         Timestamp,
                                                                      CPOClient                                                        Sender,
                                                                      String                                                           SenderDescription,
                                                                      ChargingNotificationsStartRequest                                Request,
                                                                      OICPResult<Acknowledgement<ChargingNotificationsStartRequest>>   Result);

    #endregion

    #region OnChargingNotificationsProgress

    /// <summary>
    /// A delegate called whenever a ChargingNotificationsProgress will be send.
    /// </summary>
    public delegate Task OnChargingNotificationsProgressRequestDelegate (DateTime                                                            Timestamp,
                                                                         CPOClient                                                           Sender,
                                                                         String                                                              SenderDescription,
                                                                         ChargingNotificationsProgressRequest                                Request);

    /// <summary>
    /// A delegate called whenever a response for a ChargingNotificationsProgress had been received.
    /// </summary>
    public delegate Task OnChargingNotificationsProgressResponseDelegate(DateTime                                                            Timestamp,
                                                                         CPOClient                                                           Sender,
                                                                         String                                                              SenderDescription,
                                                                         ChargingNotificationsProgressRequest                                Request,
                                                                         OICPResult<Acknowledgement<ChargingNotificationsProgressRequest>>   Result);

    #endregion

    #region OnChargingNotificationsEnd

    /// <summary>
    /// A delegate called whenever a ChargingNotificationsEnd will be send.
    /// </summary>
    public delegate Task OnChargingNotificationsEndRequestDelegate (DateTime                                                       Timestamp,
                                                                    CPOClient                                                      Sender,
                                                                    String                                                         SenderDescription,
                                                                    ChargingNotificationsEndRequest                                Request);

    /// <summary>
    /// A delegate called whenever a response for a ChargingNotificationsEnd had been received.
    /// </summary>
    public delegate Task OnChargingNotificationsEndResponseDelegate(DateTime                                                       Timestamp,
                                                                    CPOClient                                                      Sender,
                                                                    String                                                         SenderDescription,
                                                                    ChargingNotificationsEndRequest                                Request,
                                                                    OICPResult<Acknowledgement<ChargingNotificationsEndRequest>>   Result);

    #endregion

    #region OnChargingNotificationsError

    /// <summary>
    /// A delegate called whenever a ChargingNotificationsError will be send.
    /// </summary>
    public delegate Task OnChargingNotificationsErrorRequestDelegate (DateTime                                                         Timestamp,
                                                                      CPOClient                                                        Sender,
                                                                      String                                                           SenderDescription,
                                                                      ChargingNotificationsErrorRequest                                Request);

    /// <summary>
    /// A delegate called whenever a response for a ChargingNotificationsError had been received.
    /// </summary>
    public delegate Task OnChargingNotificationsErrorResponseDelegate(DateTime                                                         Timestamp,
                                                                      CPOClient                                                        Sender,
                                                                      String                                                           SenderDescription,
                                                                      ChargingNotificationsErrorRequest                                Request,
                                                                      OICPResult<Acknowledgement<ChargingNotificationsErrorRequest>>   Result);

    #endregion


    #region OnSendChargeDetailRecord

    /// <summary>
    /// A delegate called whenever a SendChargeDetailRecord request will be send.
    /// </summary>
    public delegate Task OnSendChargeDetailRecordRequestDelegate (DateTime                                                     Timestamp,
                                                                  CPOClient                                                    Sender,
                                                                  String                                                       SenderDescription,
                                                                  SendChargeDetailRecordRequest                                Request);

    /// <summary>
    /// A delegate called whenever a response for a SendChargeDetailRecord request had been received.
    /// </summary>
    public delegate Task OnSendChargeDetailRecordResponseDelegate(DateTime                                                     Timestamp,
                                                                  CPOClient                                                    Sender,
                                                                  String                                                       SenderDescription,
                                                                  SendChargeDetailRecordRequest                                Request,
                                                                  OICPResult<Acknowledgement<SendChargeDetailRecordRequest>>   Result);

    #endregion

}
