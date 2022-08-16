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

namespace cloud.charging.open.protocols.OICPv2_3.p2p.CPO
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
                                                        ICPOPeer                                           Sender,
                                                        PushEVSEDataRequest                                Request);

    /// <summary>
    /// A delegate called whenever new EVSE data record had been send upstream.
    /// </summary>
    public delegate Task OnPushEVSEDataResponseDelegate(DateTime                                           Timestamp,
                                                        ICPOPeer                                           Sender,
                                                        PushEVSEDataRequest                                Request,
                                                        OICPResult<Acknowledgement<PushEVSEDataRequest>>   Result);

    #endregion

    #region OnPushEVSEStatusRequest/-Response

    /// <summary>
    /// A delegate called whenever new EVSE status record will be send upstream.
    /// </summary>
    public delegate Task OnPushEVSEStatusRequestDelegate (DateTime                                             Timestamp,
                                                          ICPOPeer                                             Sender,
                                                          PushEVSEStatusRequest                                Request);

    /// <summary>
    /// A delegate called whenever new EVSE status record had been send upstream.
    /// </summary>
    public delegate Task OnPushEVSEStatusResponseDelegate(DateTime                                             Timestamp,
                                                          ICPOPeer                                             Sender,
                                                          PushEVSEStatusRequest                                Request,
                                                          OICPResult<Acknowledgement<PushEVSEStatusRequest>>   Result);

    #endregion


    #region OnPushPricingProductDataRequest/-Response

    /// <summary>
    /// A delegate called whenever new PricingProductData will be send upstream.
    /// </summary>
    public delegate Task OnPushPricingProductDataRequestDelegate (DateTime                                                     Timestamp,
                                                                  ICPOPeer                                                     Sender,
                                                                  PushPricingProductDataRequest                                Request);

    /// <summary>
    /// A delegate called whenever new PricingProductData had been send upstream.
    /// </summary>
    public delegate Task OnPushPricingProductDataResponseDelegate(DateTime                                                     Timestamp,
                                                                  ICPOPeer                                                     Sender,
                                                                  PushPricingProductDataRequest                                Request,
                                                                  OICPResult<Acknowledgement<PushPricingProductDataRequest>>   Result);

    #endregion

    #region OnPushEVSEPricingRequest/-Response

    /// <summary>
    /// A delegate called whenever new EVSEPricing will be send upstream.
    /// </summary>
    public delegate Task OnPushEVSEPricingRequestDelegate (DateTime                                              Timestamp,
                                                           ICPOPeer                                              Sender,
                                                           PushEVSEPricingRequest                                Request);

    /// <summary>
    /// A delegate called whenever new EVSEPricing had been send upstream.
    /// </summary>
    public delegate Task OnPushEVSEPricingResponseDelegate(DateTime                                              Timestamp,
                                                           ICPOPeer                                              Sender,
                                                           PushEVSEPricingRequest                                Request,
                                                           OICPResult<Acknowledgement<PushEVSEPricingRequest>>   Result);

    #endregion


    #region OnPullAuthenticationDataRequest/-Response

    /// <summary>
    /// A delegate called whenever a PullAuthenticationData request will be send.
    /// </summary>
    public delegate Task OnPullAuthenticationDataRequestDelegate (DateTime                                     Timestamp,
                                                                  ICPOPeer                                     Sender,
                                                                  PullAuthenticationDataRequest                Request);

    /// <summary>
    /// A delegate called whenever a response for a PullAuthenticationData request had been received.
    /// </summary>
    public delegate Task OnPullAuthenticationDataResponseDelegate(DateTime                                     Timestamp,
                                                                  ICPOPeer                                     Sender,
                                                                  PullAuthenticationDataRequest                Request,
                                                                  OICPResult<PullAuthenticationDataResponse>   Result);

    #endregion


    #region OnAuthorizeStartRequest/-Response

    /// <summary>
    /// A delegate called whenever an AuthorizeStart request will be send.
    /// </summary>
    public delegate Task OnAuthorizeStartRequestDelegate (DateTime                                 Timestamp,
                                                          ICPOPeer                                 Sender,
                                                          AuthorizeStartRequest                    Request);

    /// <summary>
    /// A delegate called whenever a response for an AuthorizeStart request had been received.
    /// </summary>
    public delegate Task OnAuthorizeStartResponseDelegate(DateTime                                 Timestamp,
                                                          ICPOPeer                                 Sender,
                                                          AuthorizeStartRequest                    Request,
                                                          OICPResult<AuthorizationStartResponse>   Result);

    #endregion

    #region OnAuthorizeStopRequest/-Response

    /// <summary>
    /// A delegate called whenever an AuthorizeStop request will be send.
    /// </summary>
    public delegate Task OnAuthorizeStopRequestDelegate (DateTime                                Timestamp,
                                                         ICPOPeer                                Sender,
                                                         AuthorizeStopRequest                    Request);

    /// <summary>
    /// A delegate called whenever a response for an AuthorizeStop request had been received.
    /// </summary>
    public delegate Task OnAuthorizeStopResponseDelegate(DateTime                                Timestamp,
                                                         ICPOPeer                                Sender,
                                                         AuthorizeStopRequest                    Request,
                                                         OICPResult<AuthorizationStopResponse>   Result);

    #endregion


    #region OnChargingStartNotificationRequest/-Response

    /// <summary>
    /// A delegate called whenever a ChargingStartNotification will be send.
    /// </summary>
    public delegate Task OnChargingStartNotificationRequestDelegate (DateTime                                                         Timestamp,
                                                                     ICPOPeer                                                         Sender,
                                                                     ChargingStartNotificationRequest                                 Request);

    /// <summary>
    /// A delegate called whenever a response for a ChargingStartNotification had been received.
    /// </summary>
    public delegate Task OnChargingStartNotificationResponseDelegate(DateTime                                                         Timestamp,
                                                                     ICPOPeer                                                         Sender,
                                                                     ChargingStartNotificationRequest                                 Request,
                                                                     OICPResult<Acknowledgement<ChargingStartNotificationRequest>>    Result);

    #endregion

    #region OnChargingProgressNotificationRequest/-Response

    /// <summary>
    /// A delegate called whenever a ChargingProgressNotification will be send.
    /// </summary>
    public delegate Task OnChargingProgressNotificationRequestDelegate (DateTime                                                            Timestamp,
                                                                        ICPOPeer                                                            Sender,
                                                                        ChargingProgressNotificationRequest                                 Request);

    /// <summary>
    /// A delegate called whenever a response for a ChargingProgressNotification had been received.
    /// </summary>
    public delegate Task OnChargingProgressNotificationResponseDelegate(DateTime                                                            Timestamp,
                                                                        ICPOPeer                                                            Sender,
                                                                        ChargingProgressNotificationRequest                                 Request,
                                                                        OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>    Result);

    #endregion

    #region OnChargingEndNotificationRequest/-Response

    /// <summary>
    /// A delegate called whenever a ChargingEndNotification will be send.
    /// </summary>
    public delegate Task OnChargingEndNotificationRequestDelegate (DateTime                                                       Timestamp,
                                                                   ICPOPeer                                                       Sender,
                                                                   ChargingEndNotificationRequest                                 Request);

    /// <summary>
    /// A delegate called whenever a response for a ChargingEndNotification had been received.
    /// </summary>
    public delegate Task OnChargingEndNotificationResponseDelegate(DateTime                                                       Timestamp,
                                                                   ICPOPeer                                                       Sender,
                                                                   ChargingEndNotificationRequest                                 Request,
                                                                   OICPResult<Acknowledgement<ChargingEndNotificationRequest>>    Result);

    #endregion

    #region OnChargingErrorNotificationRequest/-Response

    /// <summary>
    /// A delegate called whenever a ChargingErrorNotification will be send.
    /// </summary>
    public delegate Task OnChargingErrorNotificationRequestDelegate (DateTime                                                         Timestamp,
                                                                     ICPOPeer                                                         Sender,
                                                                     ChargingErrorNotificationRequest                                 Request);

    /// <summary>
    /// A delegate called whenever a response for a ChargingErrorNotification had been received.
    /// </summary>
    public delegate Task OnChargingErrorNotificationResponseDelegate(DateTime                                                         Timestamp,
                                                                     ICPOPeer                                                         Sender,
                                                                     ChargingErrorNotificationRequest                                 Request,
                                                                     OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>    Result);

    #endregion


    #region OnSendChargeDetailRecord

    /// <summary>
    /// A delegate called whenever a SendChargeDetailRecord request will be send.
    /// </summary>
    public delegate Task OnSendChargeDetailRecordRequestDelegate (DateTime                                                 Timestamp,
                                                                  ICPOPeer                                                 Sender,
                                                                  ChargeDetailRecordRequest                                Request);

    /// <summary>
    /// A delegate called whenever a response for a SendChargeDetailRecord request had been received.
    /// </summary>
    public delegate Task OnSendChargeDetailRecordResponseDelegate(DateTime                                                 Timestamp,
                                                                  ICPOPeer                                                 Sender,
                                                                  ChargeDetailRecordRequest                                Request,
                                                                  OICPResult<Acknowledgement<ChargeDetailRecordRequest>>   Result);

    #endregion

}
