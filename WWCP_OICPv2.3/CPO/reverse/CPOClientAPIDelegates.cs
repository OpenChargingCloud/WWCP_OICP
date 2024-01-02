/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    #region OnPushEVSEDataAPI                    (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a PushEVSEData request was received.
    /// </summary>
    public delegate Task

        OnPushEVSEDataAPIRequestDelegate (DateTime                                            Timestamp,
                                          CPOClientAPI                                        Sender,
                                          PushEVSEDataRequest                                 Request);


    /// <summary>
    /// Receive a PushEVSEData request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<Acknowledgement<PushEVSEDataRequest>>>

        OnPushEVSEDataAPIDelegate        (DateTime                                            Timestamp,
                                          CPOClientAPI                                        Sender,
                                          PushEVSEDataRequest                                 Request);


    /// <summary>
    /// A delegate called whenever a PushEVSEData response was sent.
    /// </summary>
    public delegate Task

        OnPushEVSEDataAPIResponseDelegate(DateTime                                            Timestamp,
                                          CPOClientAPI                                        Sender,
                                          PushEVSEDataRequest                                 Request,
                                          OICPResult<Acknowledgement<PushEVSEDataRequest>>    Response,
                                          TimeSpan                                            Runtime);

    #endregion

    #region OnPushEVSEStatusAPI                  (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a PushEVSEStatus request was received.
    /// </summary>
    public delegate Task

        OnPushEVSEStatusAPIRequestDelegate (DateTime                                              Timestamp,
                                            CPOClientAPI                                          Sender,
                                            PushEVSEStatusRequest                                 Request);


    /// <summary>
    /// Receive a PushEVSEStatus request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<Acknowledgement<PushEVSEStatusRequest>>>

        OnPushEVSEStatusAPIDelegate        (DateTime                                              Timestamp,
                                            CPOClientAPI                                          Sender,
                                            PushEVSEStatusRequest                                 Request);


    /// <summary>
    /// A delegate called whenever a PushEVSEStatus response was sent.
    /// </summary>
    public delegate Task

        OnPushEVSEStatusAPIResponseDelegate(DateTime                                              Timestamp,
                                            CPOClientAPI                                          Sender,
                                            PushEVSEStatusRequest                                 Request,
                                            OICPResult<Acknowledgement<PushEVSEStatusRequest>>    Response,
                                            TimeSpan                                              Runtime);

    #endregion


    #region OnPushPricingProductDataAPI          (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a PushPricingProductData request was received.
    /// </summary>
    public delegate Task

        OnPushPricingProductDataAPIRequestDelegate (DateTime                                                      Timestamp,
                                                    CPOClientAPI                                                  Sender,
                                                    PushPricingProductDataRequest                                 Request);


    /// <summary>
    /// Receive a PushPricingProductData request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<Acknowledgement<PushPricingProductDataRequest>>>

        OnPushPricingProductDataAPIDelegate        (DateTime                                                      Timestamp,
                                                    CPOClientAPI                                                  Sender,
                                                    PushPricingProductDataRequest                                 Request);


    /// <summary>
    /// A delegate called whenever a PushPricingProductData response was sent.
    /// </summary>
    public delegate Task

        OnPushPricingProductDataAPIResponseDelegate(DateTime                                                      Timestamp,
                                                    CPOClientAPI                                                  Sender,
                                                    PushPricingProductDataRequest                                 Request,
                                                    OICPResult<Acknowledgement<PushPricingProductDataRequest>>    Response,
                                                    TimeSpan                                                      Runtime);

    #endregion

    #region OnPushEVSEPricingAPI                 (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a PushEVSEPricing request was received.
    /// </summary>
    public delegate Task

        OnPushEVSEPricingAPIRequestDelegate (DateTime                                               Timestamp,
                                             CPOClientAPI                                           Sender,
                                             PushEVSEPricingRequest                                 Request);


    /// <summary>
    /// Receive a PushEVSEPricing request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<Acknowledgement<PushEVSEPricingRequest>>>

        OnPushEVSEPricingAPIDelegate        (DateTime                                               Timestamp,
                                             CPOClientAPI                                           Sender,
                                             PushEVSEPricingRequest                                 Request);


    /// <summary>
    /// A delegate called whenever a PushEVSEPricing response was sent.
    /// </summary>
    public delegate Task

        OnPushEVSEPricingAPIResponseDelegate(DateTime                                               Timestamp,
                                             CPOClientAPI                                           Sender,
                                             PushEVSEPricingRequest                                 Request,
                                             OICPResult<Acknowledgement<PushEVSEPricingRequest>>    Response,
                                             TimeSpan                                               Runtime);

    #endregion


    #region OnPullAuthenticationDataAPI          (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a PullAuthenticationData request was received.
    /// </summary>
    public delegate Task

        OnPullAuthenticationDataAPIRequestDelegate (DateTime                                      Timestamp,
                                                    CPOClientAPI                                  Sender,
                                                    PullAuthenticationDataRequest                 Request);


    /// <summary>
    /// Receive a PullAuthenticationData request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<PullAuthenticationDataResponse>>

        OnPullAuthenticationDataAPIDelegate        (DateTime                                      Timestamp,
                                                    CPOClientAPI                                  Sender,
                                                    PullAuthenticationDataRequest                 Request);


    /// <summary>
    /// A delegate called whenever a PullAuthenticationData response was sent.
    /// </summary>
    public delegate Task

        OnPullAuthenticationDataAPIResponseDelegate(DateTime                                      Timestamp,
                                                    CPOClientAPI                                  Sender,
                                                    PullAuthenticationDataRequest                 Request,
                                                    OICPResult<PullAuthenticationDataResponse>    Response,
                                                    TimeSpan                                      Runtime);

    #endregion


    #region OnAuthorizeStartAPI                  (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a AuthorizeStart request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeStartAPIRequestDelegate (DateTime                                  Timestamp,
                                            CPOClientAPI                              Sender,
                                            AuthorizeStartRequest                     Request);


    /// <summary>
    /// Receive a AuthorizeStart request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<AuthorizationStartResponse>>

        OnAuthorizeStartAPIDelegate        (DateTime                                  Timestamp,
                                            CPOClientAPI                              Sender,
                                            AuthorizeStartRequest                     Request);


    /// <summary>
    /// A delegate called whenever a AuthorizationStart response was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeStartAPIResponseDelegate(DateTime                                  Timestamp,
                                            CPOClientAPI                              Sender,
                                            AuthorizeStartRequest                     Request,
                                            OICPResult<AuthorizationStartResponse>    Response,
                                            TimeSpan                                  Runtime);

    #endregion

    #region OnAuthorizeStopAPI                   (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a AuthorizeStop request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeStopAPIRequestDelegate (DateTime                                 Timestamp,
                                           CPOClientAPI                             Sender,
                                           AuthorizeStopRequest                     Request);


    /// <summary>
    /// Receive a AuthorizeStop request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<AuthorizationStopResponse>>

        OnAuthorizeStopAPIDelegate        (DateTime                                 Timestamp,
                                           CPOClientAPI                             Sender,
                                           AuthorizeStopRequest                     Request);


    /// <summary>
    /// A delegate called whenever a AuthorizationStop response was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeStopAPIResponseDelegate(DateTime                                 Timestamp,
                                           CPOClientAPI                             Sender,
                                           AuthorizeStopRequest                     Request,
                                           OICPResult<AuthorizationStopResponse>    Response,
                                           TimeSpan                                 Runtime);

    #endregion


    #region OnChargingStartNotificationAPI       (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a ChargingStartNotification request was received.
    /// </summary>
    public delegate Task

        OnChargingStartNotificationAPIRequestDelegate (DateTime                                                          Timestamp,
                                                       CPOClientAPI                                                      Sender,
                                                       ChargingStartNotificationRequest                                  Request);


    /// <summary>
    /// Receive a ChargingStartNotification request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="er">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<Acknowledgement<ChargingStartNotificationRequest>>>

        OnChargingStartNotificationAPIDelegate        (DateTime                                                         Timestamp,
                                                       CPOClientAPI                                                     Sender,
                                                       ChargingStartNotificationRequest                                 Request);


    /// <summary>
    /// A delegate called whenever a ChargingStartNotification response was sent.
    /// </summary>
    public delegate Task

        OnChargingStartNotificationAPIResponseDelegate(DateTime                                                         Timestamp,
                                                       CPOClientAPI                                                     Sender,
                                                       ChargingStartNotificationRequest                                 Request,
                                                       OICPResult<Acknowledgement<ChargingStartNotificationRequest>>    Response,
                                                       TimeSpan                                                         Runtime);

    #endregion

    #region OnChargingProgressNotificationAPI    (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a ChargingProgressNotification request was received.
    /// </summary>
    public delegate Task

        OnChargingProgressNotificationAPIRequestDelegate (DateTime                                                            Timestamp,
                                                          CPOClientAPI                                                        Sender,
                                                          ChargingProgressNotificationRequest                                 Request);


    /// <summary>
    /// Receive a ChargingProgressNotification request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="er">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>>

        OnChargingProgressNotificationAPIDelegate        (DateTime                                                            Timestamp,
                                                          CPOClientAPI                                                        Sender,
                                                          ChargingProgressNotificationRequest                                 Request);


    /// <summary>
    /// A delegate called whenever a ChargingProgressNotification response was sent.
    /// </summary>
    public delegate Task

        OnChargingProgressNotificationAPIResponseDelegate(DateTime                                                            Timestamp,
                                                          CPOClientAPI                                                        Sender,
                                                          ChargingProgressNotificationRequest                                 Request,
                                                          OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>    Response,
                                                          TimeSpan                                                            Runtime);

    #endregion

    #region OnChargingEndNotificationAPI         (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a ChargingEndNotification request was received.
    /// </summary>
    public delegate Task

        OnChargingEndNotificationAPIRequestDelegate (DateTime                                                        Timestamp,
                                                     CPOClientAPI                                                    Sender,
                                                     ChargingEndNotificationRequest                                  Request);


    /// <summary>
    /// Receive a ChargingEndNotification request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="er">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<Acknowledgement<ChargingEndNotificationRequest>>>

        OnChargingEndNotificationAPIDelegate        (DateTime                                                       Timestamp,
                                                     CPOClientAPI                                                   Sender,
                                                     ChargingEndNotificationRequest                                 Request);


    /// <summary>
    /// A delegate called whenever a ChargingEndNotification response was sent.
    /// </summary>
    public delegate Task

        OnChargingEndNotificationAPIResponseDelegate(DateTime                                                       Timestamp,
                                                     CPOClientAPI                                                   Sender,
                                                     ChargingEndNotificationRequest                                 Request,
                                                     OICPResult<Acknowledgement<ChargingEndNotificationRequest>>    Response,
                                                     TimeSpan                                                       Runtime);

    #endregion

    #region OnChargingErrorNotificationAPI       (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a ChargingErrorNotification request was received.
    /// </summary>
    public delegate Task

        OnChargingErrorNotificationAPIRequestDelegate (DateTime                                                          Timestamp,
                                                       CPOClientAPI                                                      Sender,
                                                       ChargingErrorNotificationRequest                                  Request);


    /// <summary>
    /// Receive a ChargingErrorNotification request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="er">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>>

        OnChargingErrorNotificationAPIDelegate        (DateTime                                                         Timestamp,
                                                       CPOClientAPI                                                     Sender,
                                                       ChargingErrorNotificationRequest                                 Request);


    /// <summary>
    /// A delegate called whenever a ChargingErrorNotification response was sent.
    /// </summary>
    public delegate Task

        OnChargingErrorNotificationAPIResponseDelegate(DateTime                                                         Timestamp,
                                                       CPOClientAPI                                                     Sender,
                                                       ChargingErrorNotificationRequest                                 Request,
                                                       OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>    Response,
                                                       TimeSpan                                                         Runtime);

    #endregion


    #region OnChargeDetailRecordAPI              (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a ChargeDetailRecord request was received.
    /// </summary>
    public delegate Task

        OnChargeDetailRecordAPIRequestDelegate (DateTime                                                   Timestamp,
                                                CPOClientAPI                                               Sender,
                                                ChargeDetailRecordRequest                                  Request);


    /// <summary>
    /// Receive a ChargeDetailRecord request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<Acknowledgement<ChargeDetailRecordRequest>>>

        OnChargeDetailRecordAPIDelegate        (DateTime                                                  Timestamp,
                                                CPOClientAPI                                              Sender,
                                                ChargeDetailRecordRequest                                 Request);


    /// <summary>
    /// A delegate called whenever a ChargeDetailRecord response was sent.
    /// </summary>
    public delegate Task

        OnChargeDetailRecordAPIResponseDelegate(DateTime                                                  Timestamp,
                                                CPOClientAPI                                              Sender,
                                                ChargeDetailRecordRequest                                 Request,
                                                OICPResult<Acknowledgement<ChargeDetailRecordRequest>>    Response,
                                                TimeSpan                                                  Runtime);

    #endregion


}
