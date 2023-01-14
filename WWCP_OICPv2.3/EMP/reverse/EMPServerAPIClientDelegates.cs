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

    #region OnAuthorizeStart              (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever an AuthorizeStart request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeStartClientRequestDelegate (DateTime                                  Timestamp,
                                               EMPServerAPIClient                        Sender,
                                               AuthorizeStartRequest                     Request);


    /// <summary>
    /// Initiate an start authorization for the given AuthToken at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">An AuthorizeStart request.</param>
    public delegate Task<OICPResult<AuthorizationStartResponse>>

        OnAuthorizeStartClientDelegate        (DateTime                                  Timestamp,
                                               EMPServerAPIClient                        Sender,
                                               AuthorizeStartRequest                     Request);


    /// <summary>
    /// A delegate called whenever a response for an AuthorizeStart request was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeStartClientResponseDelegate(DateTime                                  Timestamp,
                                               EMPServerAPIClient                        Sender,
                                               AuthorizeStartRequest                     Request,
                                               OICPResult<AuthorizationStartResponse>    Response,
                                               TimeSpan                                  Runtime);

    #endregion

    #region OnAuthorizeStop               (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever an AuthorizeStop request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeStopClientRequestDelegate (DateTime                                  Timestamp,
                                              EMPServerAPIClient                        Sender,
                                              AuthorizeStopRequest                      Request);


    /// <summary>
    /// Initiate an stop authorization for the given AuthToken at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<AuthorizationStopResponse>>

        OnAuthorizeStopClientDelegate        (DateTime                                 Timestamp,
                                              EMPServerAPIClient                       Sender,
                                              AuthorizeStopRequest                     Request);


    /// <summary>
    /// A delegate called whenever a response for an AuthorizeStop request was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeStopClientResponseDelegate(DateTime                                 Timestamp,
                                              EMPServerAPIClient                       Sender,
                                              AuthorizeStopRequest                     Request,
                                              OICPResult<AuthorizationStopResponse>    Response,
                                              TimeSpan                                 Runtime);

    #endregion


    #region OnChargingStartNotification   (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a charging start notification request was received.
    /// </summary>
    public delegate Task

        OnChargingStartNotificationClientRequestDelegate (DateTime                                                         Timestamp,
                                                          EMPServerAPIClient                                               Sender,
                                                          ChargingStartNotificationRequest                                 Request);


    /// <summary>
    /// Send a charging start notification.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<Acknowledgement<ChargingStartNotificationRequest>>>

        OnChargingStartNotificationClientDelegate        (DateTime                                                         Timestamp,
                                                          EMPServerAPIClient                                               Sender,
                                                          ChargingStartNotificationRequest                                 Request);


    /// <summary>
    /// A delegate called whenever a charging start notification response was sent.
    /// </summary>
    public delegate Task

        OnChargingStartNotificationClientResponseDelegate(DateTime                                                         Timestamp,
                                                          EMPServerAPIClient                                               Sender,
                                                          ChargingStartNotificationRequest                                 Request,
                                                          OICPResult<Acknowledgement<ChargingStartNotificationRequest>>    Response,
                                                          TimeSpan                                                         Runtime);

    #endregion

    #region OnChargingProgressNotification(Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a charging progress notification request was received.
    /// </summary>
    public delegate Task

        OnChargingProgressNotificationClientRequestDelegate (DateTime                                                            Timestamp,
                                                             EMPServerAPIClient                                                  Sender,
                                                             ChargingProgressNotificationRequest                                 Request);


    /// <summary>
    /// Send a charging progress notification.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>>

        OnChargingProgressNotificationClientDelegate        (DateTime                                                            Timestamp,
                                                             EMPServerAPIClient                                                  Sender,
                                                             ChargingProgressNotificationRequest                                 Request);


    /// <summary>
    /// A delegate called whenever a charging progress notification response was sent.
    /// </summary>
    public delegate Task

        OnChargingProgressNotificationClientResponseDelegate(DateTime                                                            Timestamp,
                                                             EMPServerAPIClient                                                  Sender,
                                                             ChargingProgressNotificationRequest                                 Request,
                                                             OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>    Response,
                                                             TimeSpan                                                            Runtime);

    #endregion

    #region OnChargingEndNotification     (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a charging end notification request was received.
    /// </summary>
    public delegate Task

        OnChargingEndNotificationClientRequestDelegate (DateTime                                                       Timestamp,
                                                        EMPServerAPIClient                                             Sender,
                                                        ChargingEndNotificationRequest                                 Request);


    /// <summary>
    /// Send a charging end notification.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<Acknowledgement<ChargingEndNotificationRequest>>>

        OnChargingEndNotificationClientDelegate        (DateTime                                                       Timestamp,
                                                        EMPServerAPIClient                                             Sender,
                                                        ChargingEndNotificationRequest                                 Request);


    /// <summary>
    /// A delegate called whenever a charging end notification response was sent.
    /// </summary>
    public delegate Task

        OnChargingEndNotificationClientResponseDelegate(DateTime                                                       Timestamp,
                                                        EMPServerAPIClient                                             Sender,
                                                        ChargingEndNotificationRequest                                 Request,
                                                        OICPResult<Acknowledgement<ChargingEndNotificationRequest>>    Response,
                                                        TimeSpan                                                       Runtime);

    #endregion

    #region OnChargingErrorNotification   (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a charging error notification request was received.
    /// </summary>
    public delegate Task

        OnChargingErrorNotificationClientRequestDelegate (DateTime                                                         Timestamp,
                                                          EMPServerAPIClient                                               Sender,
                                                          ChargingErrorNotificationRequest                                 Request);


    /// <summary>
    /// Send a charging error notification.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<Acknowledgement<ChargeDetailRecordRequest>>>

        OnChargingErrorNotificationClientDelegate        (DateTime                                                         Timestamp,
                                                          EMPServerAPIClient                                               Sender,
                                                          ChargingErrorNotificationRequest                                 Request);


    /// <summary>
    /// A delegate called whenever a charging error notification response was sent.
    /// </summary>
    public delegate Task

        OnChargingErrorNotificationClientResponseDelegate(DateTime                                                         Timestamp,
                                                          EMPServerAPIClient                                               Sender,
                                                          ChargingErrorNotificationRequest                                 Request,
                                                          OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>    Response,
                                                          TimeSpan                                                         Runtime);

    #endregion


    #region OnChargeDetailRecord          (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a charge detail record request was received.
    /// </summary>
    public delegate Task

        OnChargeDetailRecordClientRequestDelegate (DateTime                                                  Timestamp,
                                                   EMPServerAPIClient                                        Sender,
                                                   ChargeDetailRecordRequest                                 Request);


    /// <summary>
    /// Send a charge detail record.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<Acknowledgement<ChargeDetailRecordRequest>>>

        OnChargeDetailRecordClientDelegate        (DateTime                                                  Timestamp,
                                                   EMPServerAPIClient                                        Sender,
                                                   ChargeDetailRecordRequest                                 Request);


    /// <summary>
    /// A delegate called whenever a charge detail record response was sent.
    /// </summary>
    public delegate Task

        OnChargeDetailRecordClientResponseDelegate(DateTime                                                  Timestamp,
                                                   EMPServerAPIClient                                        Sender,
                                                   ChargeDetailRecordRequest                                 Request,
                                                   OICPResult<Acknowledgement<ChargeDetailRecordRequest>>    Response,
                                                   TimeSpan                                                  Runtime);

    #endregion

}
