/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

        OnAuthorizeStartRequestDelegate (DateTimeOffset                Timestamp,
                                         EMPServerAPI                  Sender,
                                         AuthorizeStartRequest         Request);


    /// <summary>
    /// Initiate an start authorization for the given AuthToken at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">An AuthorizeStart request.</param>
    public delegate Task<AuthorizationStartResponse>

        OnAuthorizeStartDelegate        (DateTimeOffset                Timestamp,
                                         EMPServerAPI                  Sender,
                                         AuthorizeStartRequest         Request);


    /// <summary>
    /// A delegate called whenever a response for an AuthorizeStart request was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeStartResponseDelegate(DateTimeOffset                Timestamp,
                                         EMPServerAPI                  Sender,
                                         AuthorizeStartRequest         Request,
                                         AuthorizationStartResponse    Response,
                                         TimeSpan                      Runtime);

    #endregion

    #region OnAuthorizeStop               (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever an AuthorizeStop request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeStopRequestDelegate (DateTimeOffset               Timestamp,
                                        EMPServerAPI                 Sender,
                                        AuthorizeStopRequest         Request);


    /// <summary>
    /// Initiate an stop authorization for the given AuthToken at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<AuthorizationStopResponse>

        OnAuthorizeStopDelegate        (DateTimeOffset               Timestamp,
                                        EMPServerAPI                 Sender,
                                        AuthorizeStopRequest         Request);


    /// <summary>
    /// A delegate called whenever a response for an AuthorizeStop request was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeStopResponseDelegate(DateTimeOffset               Timestamp,
                                        EMPServerAPI                 Sender,
                                        AuthorizeStopRequest         Request,
                                        AuthorizationStopResponse    Response,
                                        TimeSpan                     Runtime);

    #endregion


    #region OnChargingStartNotification   (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a charging start notification request was received.
    /// </summary>
    public delegate Task

        OnChargingStartNotificationRequestDelegate (DateTimeOffset                                       Timestamp,
                                                    EMPServerAPI                                         Sender,
                                                    ChargingStartNotificationRequest                     ChargingStartNotification);


    /// <summary>
    /// Send a charging start notification.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the notification.</param>
    /// <param name="Sender">The sender of the notification.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<Acknowledgement<ChargingStartNotificationRequest>>

        OnChargingStartNotificationDelegate        (DateTimeOffset                                       Timestamp,
                                                    EMPServerAPI                                         Sender,
                                                    ChargingStartNotificationRequest                     Request);


    /// <summary>
    /// A delegate called whenever a charging start notification response was sent.
    /// </summary>
    public delegate Task

        OnChargingStartNotificationResponseDelegate(DateTimeOffset                                       Timestamp,
                                                    EMPServerAPI                                         Sender,
                                                    ChargingStartNotificationRequest                     Request,
                                                    Acknowledgement<ChargingStartNotificationRequest>    Response,
                                                    TimeSpan                                             Runtime);

    #endregion

    #region OnChargingProgressNotification(Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a charging progress notification request was received.
    /// </summary>
    public delegate Task

        OnChargingProgressNotificationRequestDelegate (DateTimeOffset                                          Timestamp,
                                                       EMPServerAPI                                            Sender,
                                                       ChargingProgressNotificationRequest                     ChargingProgressNotification);


    /// <summary>
    /// Send a charging progress notification.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the notification.</param>
    /// <param name="Sender">The sender of the notification.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<Acknowledgement<ChargingProgressNotificationRequest>>

        OnChargingProgressNotificationDelegate        (DateTimeOffset                                          Timestamp,
                                                       EMPServerAPI                                            Sender,
                                                       ChargingProgressNotificationRequest                     Request);


    /// <summary>
    /// A delegate called whenever a charging progress notification response was sent.
    /// </summary>
    public delegate Task

        OnChargingProgressNotificationResponseDelegate(DateTimeOffset                                          Timestamp,
                                                       EMPServerAPI                                            Sender,
                                                       ChargingProgressNotificationRequest                     Request,
                                                       Acknowledgement<ChargingProgressNotificationRequest>    Response,
                                                       TimeSpan                                                Runtime);

    #endregion

    #region OnChargingEndNotification     (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a charging end notification request was received.
    /// </summary>
    public delegate Task

        OnChargingEndNotificationRequestDelegate (DateTimeOffset                                     Timestamp,
                                                  EMPServerAPI                                       Sender,
                                                  ChargingEndNotificationRequest                     ChargingEndNotification);


    /// <summary>
    /// Send a charging end notification.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the notification.</param>
    /// <param name="Sender">The sender of the notification.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<Acknowledgement<ChargingEndNotificationRequest>>

        OnChargingEndNotificationDelegate        (DateTimeOffset                                     Timestamp,
                                                  EMPServerAPI                                       Sender,
                                                  ChargingEndNotificationRequest                     Request);


    /// <summary>
    /// A delegate called whenever a charging end notification response was sent.
    /// </summary>
    public delegate Task

        OnChargingEndNotificationResponseDelegate(DateTimeOffset                                     Timestamp,
                                                  EMPServerAPI                                       Sender,
                                                  ChargingEndNotificationRequest                     Request,
                                                  Acknowledgement<ChargingEndNotificationRequest>    Response,
                                                  TimeSpan                                           Runtime);

    #endregion

    #region OnChargingErrorNotification   (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a charging error notification request was received.
    /// </summary>
    public delegate Task

        OnChargingErrorNotificationRequestDelegate (DateTimeOffset                                       Timestamp,
                                                    EMPServerAPI                                         Sender,
                                                    ChargingErrorNotificationRequest                     ChargingErrorNotification);


    /// <summary>
    /// Send a charging error notification.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the notification.</param>
    /// <param name="Sender">The sender of the notification.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<Acknowledgement<ChargingErrorNotificationRequest>>

        OnChargingErrorNotificationDelegate        (DateTimeOffset                                       Timestamp,
                                                    EMPServerAPI                                         Sender,
                                                    ChargingErrorNotificationRequest                     Request);


    /// <summary>
    /// A delegate called whenever a charging error notification response was sent.
    /// </summary>
    public delegate Task

        OnChargingErrorNotificationResponseDelegate(DateTimeOffset                                       Timestamp,
                                                    EMPServerAPI                                         Sender,
                                                    ChargingErrorNotificationRequest                     Request,
                                                    Acknowledgement<ChargingErrorNotificationRequest>    Response,
                                                    TimeSpan                                             Runtime);

    #endregion


    #region OnChargeDetailRecord          (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a charge detail record request was received.
    /// </summary>
    public delegate Task

        OnChargeDetailRecordRequestDelegate (DateTimeOffset                                Timestamp,
                                             EMPServerAPI                                  Sender,
                                             ChargeDetailRecordRequest                     Request);


    /// <summary>
    /// Send a charge detail record.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<Acknowledgement<ChargeDetailRecordRequest>>

        OnChargeDetailRecordDelegate        (DateTimeOffset                                Timestamp,
                                             EMPServerAPI                                  Sender,
                                             ChargeDetailRecordRequest                     Request);


    /// <summary>
    /// A delegate called whenever a charge detail record response was sent.
    /// </summary>
    public delegate Task

        OnChargeDetailRecordResponseDelegate(DateTimeOffset                                Timestamp,
                                             EMPServerAPI                                  Sender,
                                             ChargeDetailRecordRequest                     Request,
                                             Acknowledgement<ChargeDetailRecordRequest>    Response,
                                             TimeSpan                                      Runtime);

    #endregion

}
