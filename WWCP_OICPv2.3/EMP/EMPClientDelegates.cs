﻿/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace cloud.charging.open.protocols.OICPv2_3.EMP
{

    #region OnPullEVSEData                             (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a PullEVSEData request will be send.
    /// </summary>
    public delegate Task OnPullEVSEDataRequestDelegate   (DateTime                            Timestamp,
                                                          IEMPClient                          Sender,
                                                          PullEVSEDataRequest                 Request);

    /// <summary>
    /// A delegate called whenever a response for a PullEVSEData request had been received.
    /// </summary>
    public delegate Task OnPullEVSEDataResponseDelegate  (DateTime                            Timestamp,
                                                          IEMPClient                          Sender,
                                                          PullEVSEDataRequest                 Request,
                                                          OICPResult<PullEVSEDataResponse>    Response,
                                                          TimeSpan                            Runtime);

    #endregion

    #region OnPullEVSEStatus                           (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a PullEVSEStatus request will be send.
    /// </summary>
    public delegate Task OnPullEVSEStatusRequestDelegate (DateTime                              Timestamp,
                                                          IEMPClient                            Sender,
                                                          PullEVSEStatusRequest                 Request);

    /// <summary>
    /// A delegate called whenever a response for a PullEVSEStatus request had been received.
    /// </summary>
    public delegate Task OnPullEVSEStatusResponseDelegate(DateTime                              Timestamp,
                                                          IEMPClient                            Sender,
                                                          PullEVSEStatusRequest                 Request,
                                                          OICPResult<PullEVSEStatusResponse>    Response,
                                                          TimeSpan                              Runtime);

    #endregion

    #region OnPullEVSEStatusById                       (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a PullEVSEStatusById request will be send.
    /// </summary>
    public delegate Task OnPullEVSEStatusByIdRequestDelegate (DateTime                                  Timestamp,
                                                              IEMPClient                                Sender,
                                                              PullEVSEStatusByIdRequest                 Request);

    /// <summary>
    /// A delegate called whenever a response for a PullEVSEStatusById request had been received.
    /// </summary>
    public delegate Task OnPullEVSEStatusByIdResponseDelegate(DateTime                                  Timestamp,
                                                              IEMPClient                                Sender,
                                                              PullEVSEStatusByIdRequest                 Request,
                                                              OICPResult<PullEVSEStatusByIdResponse>    Response,
                                                              TimeSpan                                  Runtime);

    #endregion

    #region OnPullEVSEStatusByOperatorId               (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a PullEVSEStatusByOperatorId request will be send.
    /// </summary>
    public delegate Task OnPullEVSEStatusByOperatorIdRequestDelegate (DateTime                                          Timestamp,
                                                                      IEMPClient                                        Sender,
                                                                      PullEVSEStatusByOperatorIdRequest                 Request);

    /// <summary>
    /// A delegate called whenever a response for a PullEVSEStatusByOperatorId request had been received.
    /// </summary>
    public delegate Task OnPullEVSEStatusByOperatorIdResponseDelegate(DateTime                                          Timestamp,
                                                                      IEMPClient                                        Sender,
                                                                      PullEVSEStatusByOperatorIdRequest                 Request,
                                                                      OICPResult<PullEVSEStatusByOperatorIdResponse>    Response,
                                                                      TimeSpan                                          Runtime);

    #endregion


    #region OnPullPricingProductData                   (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a PullPricingProductData request will be send.
    /// </summary>
    public delegate Task OnPullPricingProductDataRequestDelegate (DateTime                                      Timestamp,
                                                                  IEMPClient                                    Sender,
                                                                  PullPricingProductDataRequest                 Request);

    /// <summary>
    /// A delegate called whenever a response for a PullPricingProductData request had been received.
    /// </summary>
    public delegate Task OnPullPricingProductDataResponseDelegate(DateTime                                      Timestamp,
                                                                  IEMPClient                                    Sender,
                                                                  PullPricingProductDataRequest                 Request,
                                                                  OICPResult<PullPricingProductDataResponse>    Response,
                                                                  TimeSpan                                      Runtime);

    #endregion

    #region OnPullEVSEPricing                          (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a PullEVSEPricing request will be send.
    /// </summary>
    public delegate Task OnPullEVSEPricingRequestDelegate (DateTime                               Timestamp,
                                                           IEMPClient                             Sender,
                                                           PullEVSEPricingRequest                 Request);

    /// <summary>
    /// A delegate called whenever a response for a PullEVSEPricing request had been received.
    /// </summary>
    public delegate Task OnPullEVSEPricingResponseDelegate(DateTime                               Timestamp,
                                                           IEMPClient                             Sender,
                                                           PullEVSEPricingRequest                 Request,
                                                           OICPResult<PullEVSEPricingResponse>    Response,
                                                           TimeSpan                               Runtime);

    #endregion


    #region OnPushAuthenticationDataAPI                (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a PushAuthenticationData request request will be send.
    /// </summary>
    public delegate Task

        OnPushAuthenticationDataRequestDelegate (DateTime                                                      Timestamp,
                                                 IEMPClient                                                    Sender,
                                                 PushAuthenticationDataRequest                                 Request);

    /// <summary>
    /// A delegate called whenever a response for a PushAuthenticationData request had been received.
    /// </summary>
    public delegate Task

        OnPushAuthenticationDataResponseDelegate(DateTime                                                      Timestamp,
                                                 IEMPClient                                                    Sender,
                                                 PushAuthenticationDataRequest                                 Request,
                                                 OICPResult<Acknowledgement<PushAuthenticationDataRequest>>    Response,
                                                 TimeSpan                                                      Runtime);

    #endregion


    #region OnAuthorizeRemoteReservationStart/-Stop    (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever an AuthorizeRemoteReservationStart request will be send.
    /// </summary>
    public delegate Task OnAuthorizeRemoteReservationStartRequestDelegate (DateTime                                                               Timestamp,
                                                                           IEMPClient                                                             Sender,
                                                                           AuthorizeRemoteReservationStartRequest                                 Request);

    /// <summary>
    /// A delegate called whenever a response for an AuthorizeRemoteReservationStart request had been received.
    /// </summary>
    public delegate Task OnAuthorizeRemoteReservationStartResponseDelegate(DateTime                                                               Timestamp,
                                                                           IEMPClient                                                             Sender,
                                                                           AuthorizeRemoteReservationStartRequest                                 Request,
                                                                           OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>    Response,
                                                                           TimeSpan                                                               Runtime);



    /// <summary>
    /// A delegate called whenever an AuthorizeRemoteReservationStop request will be send.
    /// </summary>
    public delegate Task OnAuthorizeRemoteReservationStopRequestDelegate  (DateTime                                                               Timestamp,
                                                                           IEMPClient                                                             Sender,
                                                                           AuthorizeRemoteReservationStopRequest                                  Request);

    /// <summary>
    /// A delegate called whenever a response for an AuthorizeRemoteReservationStop request had been received.
    /// </summary>
    public delegate Task OnAuthorizeRemoteReservationStopResponseDelegate (DateTime                                                               Timestamp,
                                                                           IEMPClient                                                             Sender,
                                                                           AuthorizeRemoteReservationStopRequest                                  Request,
                                                                           OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>     Response,
                                                                           TimeSpan                                                               Runtime);

    #endregion

    #region OnAuthorizeRemoteStart/-Stop               (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever an AuthorizeRemoteStart request will be send.
    /// </summary>
    public delegate Task OnAuthorizeRemoteStartRequestDelegate (DateTime                                                    Timestamp,
                                                                IEMPClient                                                  Sender,
                                                                AuthorizeRemoteStartRequest                                 Request);

    /// <summary>
    /// A delegate called whenever a response for an AuthorizeRemoteStart request had been received.
    /// </summary>
    public delegate Task OnAuthorizeRemoteStartResponseDelegate(DateTime                                                    Timestamp,
                                                                IEMPClient                                                  Sender,
                                                                AuthorizeRemoteStartRequest                                 Request,
                                                                OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>    Response,
                                                                TimeSpan                                                    Runtime);



    /// <summary>
    /// A delegate called whenever an AuthorizeRemoteStop request will be send.
    /// </summary>
    public delegate Task OnAuthorizeRemoteStopRequestDelegate  (DateTime                                                    Timestamp,
                                                                IEMPClient                                                  Sender,
                                                                AuthorizeRemoteStopRequest                                  Request);

    /// <summary>
    /// A delegate called whenever a response for an AuthorizeRemoteStop request had been received.
    /// </summary>
    public delegate Task OnAuthorizeRemoteStopResponseDelegate (DateTime                                                    Timestamp,
                                                                IEMPClient                                                  Sender,
                                                                AuthorizeRemoteStopRequest                                  Request,
                                                                OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>     Response,
                                                                TimeSpan                                                    Runtime);

    #endregion


    #region OnGetChargeDetailRecords                   (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a GetChargeDetailRecords request will be send.
    /// </summary>
    public delegate Task OnGetChargeDetailRecordsRequestDelegate (DateTime                                      Timestamp,
                                                                  IEMPClient                                    Sender,
                                                                  GetChargeDetailRecordsRequest                 Request);

    /// <summary>
    /// A delegate called whenever a response for a GetChargeDetailRecords request had been received.
    /// </summary>
    public delegate Task OnGetChargeDetailRecordsResponseDelegate(DateTime                                      Timestamp,
                                                                  IEMPClient                                    Sender,
                                                                  GetChargeDetailRecordsRequest                 Request,
                                                                  OICPResult<GetChargeDetailRecordsResponse>    Response,
                                                                  TimeSpan                                      Runtime);

    #endregion

}
