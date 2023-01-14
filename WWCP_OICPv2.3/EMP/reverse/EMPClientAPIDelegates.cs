/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
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

    #region OnPullEVSEData                     (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a PullEVSEData request was received.
    /// </summary>
    public delegate Task

        OnPullEVSEDataAPIRequestDelegate (DateTime                            Timestamp,
                                          EMPClientAPI                        Sender,
                                          PullEVSEDataRequest                 Request);


    /// <summary>
    /// Send a PullEVSEData.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<PullEVSEDataResponse>>

        OnPullEVSEDataAPIDelegate        (DateTime                            Timestamp,
                                          EMPClientAPI                        Sender,
                                          PullEVSEDataRequest                 Request);


    /// <summary>
    /// A delegate called whenever a PullEVSEData response was sent.
    /// </summary>
    public delegate Task

        OnPullEVSEDataAPIResponseDelegate(DateTime                            Timestamp,
                                          EMPClientAPI                        Sender,
                                          PullEVSEDataRequest                 Request,
                                          OICPResult<PullEVSEDataResponse>    Response,
                                          TimeSpan                            Runtime);

    #endregion

    #region OnPullEVSEStatus                   (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a PullEVSEStatus request was received.
    /// </summary>
    public delegate Task

        OnPullEVSEStatusAPIRequestDelegate (DateTime                              Timestamp,
                                            EMPClientAPI                          Sender,
                                            PullEVSEStatusRequest                 Request);


    /// <summary>
    /// Send a PullEVSEStatus.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<PullEVSEStatusResponse>>

        OnPullEVSEStatusAPIDelegate        (DateTime                              Timestamp,
                                            EMPClientAPI                          Sender,
                                            PullEVSEStatusRequest                 Request);


    /// <summary>
    /// A delegate called whenever a PullEVSEStatus response was sent.
    /// </summary>
    public delegate Task

        OnPullEVSEStatusAPIResponseDelegate(DateTime                              Timestamp,
                                            EMPClientAPI                          Sender,
                                            PullEVSEStatusRequest                 Request,
                                            OICPResult<PullEVSEStatusResponse>    Response,
                                            TimeSpan                              Runtime);

    #endregion

    #region OnPullEVSEStatusById               (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a PullEVSEStatusById request was received.
    /// </summary>
    public delegate Task

        OnPullEVSEStatusByIdAPIRequestDelegate (DateTime                                  Timestamp,
                                                EMPClientAPI                              Sender,
                                                PullEVSEStatusByIdRequest                 Request);


    /// <summary>
    /// Send a PullEVSEStatusById.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<PullEVSEStatusByIdResponse>>

        OnPullEVSEStatusByIdAPIDelegate        (DateTime                                  Timestamp,
                                                EMPClientAPI                              Sender,
                                                PullEVSEStatusByIdRequest                 Request);


    /// <summary>
    /// A delegate called whenever a PullEVSEStatusById response was sent.
    /// </summary>
    public delegate Task

        OnPullEVSEStatusByIdAPIResponseDelegate(DateTime                                  Timestamp,
                                                EMPClientAPI                              Sender,
                                                PullEVSEStatusByIdRequest                 Request,
                                                OICPResult<PullEVSEStatusByIdResponse>    Response,
                                                TimeSpan                                  Runtime);

    #endregion

    #region OnPullEVSEStatusByOperatorId       (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a PullEVSEStatusByOperatorId request was received.
    /// </summary>
    public delegate Task

        OnPullEVSEStatusByOperatorIdAPIRequestDelegate (DateTime                                          Timestamp,
                                                        EMPClientAPI                                      Sender,
                                                        PullEVSEStatusByOperatorIdRequest                 Request);


    /// <summary>
    /// Send a PullEVSEStatusByOperatorId.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<PullEVSEStatusByOperatorIdResponse>>

        OnPullEVSEStatusByOperatorIdAPIDelegate        (DateTime                                          Timestamp,
                                                        EMPClientAPI                                      Sender,
                                                        PullEVSEStatusByOperatorIdRequest                 Request);


    /// <summary>
    /// A delegate called whenever a PullEVSEStatusByOperatorId response was sent.
    /// </summary>
    public delegate Task

        OnPullEVSEStatusByOperatorIdAPIResponseDelegate(DateTime                                          Timestamp,
                                                        EMPClientAPI                                      Sender,
                                                        PullEVSEStatusByOperatorIdRequest                 Request,
                                                        OICPResult<PullEVSEStatusByOperatorIdResponse>    Response,
                                                        TimeSpan                                          Runtime);

    #endregion


    #region OnPullPricingProductData           (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a PullPricingProductData request was received.
    /// </summary>
    public delegate Task

        OnPullPricingProductDataAPIRequestDelegate (DateTime                                      Timestamp,
                                                    EMPClientAPI                                  Sender,
                                                    PullPricingProductDataRequest                 Request);


    /// <summary>
    /// Send a PullPricingProductData.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<PullPricingProductDataResponse>>

        OnPullPricingProductDataAPIDelegate        (DateTime                                      Timestamp,
                                                    EMPClientAPI                                  Sender,
                                                    PullPricingProductDataRequest                 Request);


    /// <summary>
    /// A delegate called whenever a PullPricingProductData response was sent.
    /// </summary>
    public delegate Task

        OnPullPricingProductDataAPIResponseDelegate(DateTime                                      Timestamp,
                                                    EMPClientAPI                                  Sender,
                                                    PullPricingProductDataRequest                 Request,
                                                    OICPResult<PullPricingProductDataResponse>    Response,
                                                    TimeSpan                                      Runtime);

    #endregion

    #region OnPullEVSEPricing                  (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a PullEVSEPricing request was received.
    /// </summary>
    public delegate Task

        OnPullEVSEPricingAPIRequestDelegate (DateTime                               Timestamp,
                                             EMPClientAPI                           Sender,
                                             PullEVSEPricingRequest                 Request);


    /// <summary>
    /// Send a PullEVSEPricing.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<PullEVSEPricingResponse>>

        OnPullEVSEPricingAPIDelegate        (DateTime                               Timestamp,
                                             EMPClientAPI                           Sender,
                                             PullEVSEPricingRequest                 Request);


    /// <summary>
    /// A delegate called whenever a PullEVSEPricing response was sent.
    /// </summary>
    public delegate Task

        OnPullEVSEPricingAPIResponseDelegate(DateTime                               Timestamp,
                                             EMPClientAPI                           Sender,
                                             PullEVSEPricingRequest                 Request,
                                             OICPResult<PullEVSEPricingResponse>    Response,
                                             TimeSpan                               Runtime);

    #endregion


    #region OnPushAuthenticationData           (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a PushAuthenticationData request was received.
    /// </summary>
    public delegate Task

        OnPushAuthenticationDataAPIRequestDelegate(DateTime                                                       Timestamp,
                                                   EMPClientAPI                                                   Sender,
                                                   PushAuthenticationDataRequest                                  Request);


    /// <summary>
    /// Send a PushAuthenticationData.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<Acknowledgement<PushAuthenticationDataRequest>>>

        OnPushAuthenticationDataAPIDelegate        (DateTime                                                      Timestamp,
                                                    EMPClientAPI                                                  Sender,
                                                    PushAuthenticationDataRequest                                 Request);


    /// <summary>
    /// A delegate called whenever a PushAuthenticationData response was sent.
    /// </summary>
    public delegate Task

        OnPushAuthenticationDataAPIResponseDelegate(DateTime                                                      Timestamp,
                                                    EMPClientAPI                                                  Sender,
                                                    PushAuthenticationDataRequest                                 Request,
                                                    OICPResult<Acknowledgement<PushAuthenticationDataRequest>>    Response,
                                                    TimeSpan                                                      Runtime);

    #endregion


    #region OnAuthorizeRemoteReservationStart  (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a AuthorizeRemoteReservationStart request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteReservationStartAPIRequestDelegate(DateTime                                                                Timestamp,
                                                            EMPClientAPI                                                            Sender,
                                                            AuthorizeRemoteReservationStartRequest                                  Request);


    /// <summary>
    /// Send a AuthorizeRemoteReservationStart.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>>

        OnAuthorizeRemoteReservationStartAPIDelegate        (DateTime                                                               Timestamp,
                                                             EMPClientAPI                                                           Sender,
                                                             AuthorizeRemoteReservationStartRequest                                 Request);


    /// <summary>
    /// A delegate called whenever a AuthorizeRemoteReservationStart response was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteReservationStartAPIResponseDelegate(DateTime                                                               Timestamp,
                                                             EMPClientAPI                                                           Sender,
                                                             AuthorizeRemoteReservationStartRequest                                 Request,
                                                             OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>    Response,
                                                             TimeSpan                                                               Runtime);

    #endregion

    #region OnAuthorizeRemoteReservationStop   (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a AuthorizeRemoteReservationStop request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteReservationStopAPIRequestDelegate(DateTime                                                               Timestamp,
                                                           EMPClientAPI                                                           Sender,
                                                           AuthorizeRemoteReservationStopRequest                                  Request);


    /// <summary>
    /// Send a AuthorizeRemoteReservationStop.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>>

        OnAuthorizeRemoteReservationStopAPIDelegate        (DateTime                                                              Timestamp,
                                                            EMPClientAPI                                                          Sender,
                                                            AuthorizeRemoteReservationStopRequest                                 Request);


    /// <summary>
    /// A delegate called whenever a AuthorizeRemoteReservationStop response was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteReservationStopAPIResponseDelegate(DateTime                                                              Timestamp,
                                                            EMPClientAPI                                                          Sender,
                                                            AuthorizeRemoteReservationStopRequest                                 Request,
                                                            OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>    Response,
                                                            TimeSpan                                                              Runtime);

    #endregion

    #region OnAuthorizeRemoteStart             (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a AuthorizeRemoteStart request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteStartAPIRequestDelegate(DateTime                                                     Timestamp,
                                                 EMPClientAPI                                                 Sender,
                                                 AuthorizeRemoteStartRequest                                  Request);


    /// <summary>
    /// Send a AuthorizeRemoteStart.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>>

        OnAuthorizeRemoteStartAPIDelegate        (DateTime                                                    Timestamp,
                                                  EMPClientAPI                                                Sender,
                                                  AuthorizeRemoteStartRequest                                 Request);


    /// <summary>
    /// A delegate called whenever a AuthorizeRemoteStart response was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteStartAPIResponseDelegate(DateTime                                                    Timestamp,
                                                  EMPClientAPI                                                Sender,
                                                  AuthorizeRemoteStartRequest                                 Request,
                                                  OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>    Response,
                                                  TimeSpan                                                    Runtime);

    #endregion

    #region OnAuthorizeRemoteStop              (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a AuthorizeRemoteStop request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteStopAPIRequestDelegate(DateTime                                                    Timestamp,
                                                EMPClientAPI                                                Sender,
                                                AuthorizeRemoteStopRequest                                  Request);


    /// <summary>
    /// Send a AuthorizeRemoteStop.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>>

        OnAuthorizeRemoteStopAPIDelegate        (DateTime                                                   Timestamp,
                                                 EMPClientAPI                                               Sender,
                                                 AuthorizeRemoteStopRequest                                 Request);


    /// <summary>
    /// A delegate called whenever a AuthorizeRemoteStop response was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteStopAPIResponseDelegate(DateTime                                                   Timestamp,
                                                 EMPClientAPI                                               Sender,
                                                 AuthorizeRemoteStopRequest                                 Request,
                                                 OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>    Response,
                                                 TimeSpan                                                   Runtime);

    #endregion


    #region OnGetChargeDetailRecords           (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a GetChargeDetailRecords request was received.
    /// </summary>
    public delegate Task

        OnGetChargeDetailRecordsAPIRequestDelegate(DateTime                                       Timestamp,
                                                   EMPClientAPI                                   Sender,
                                                   GetChargeDetailRecordsRequest                  Request);


    /// <summary>
    /// Send a GetChargeDetailRecords.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<GetChargeDetailRecordsResponse>>

        OnGetChargeDetailRecordsAPIDelegate        (DateTime                                      Timestamp,
                                                    EMPClientAPI                                  Sender,
                                                    GetChargeDetailRecordsRequest                 Request);


    /// <summary>
    /// A delegate called whenever a GetChargeDetailRecords response was sent.
    /// </summary>
    public delegate Task

        OnGetChargeDetailRecordsAPIResponseDelegate(DateTime                                      Timestamp,
                                                    EMPClientAPI                                  Sender,
                                                    GetChargeDetailRecordsRequest                 Request,
                                                    OICPResult<GetChargeDetailRecordsResponse>    Response,
                                                    TimeSpan                                      Runtime);

    #endregion

}
