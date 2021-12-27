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

namespace cloud.charging.open.protocols.OICPv2_3.EMP
{

    #region OnAuthorizeStart              (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever an AuthorizeStart request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeStartRequestDelegate (DateTime                      Timestamp,
                                         EMPServerAPI                  Sender,
                                         AuthorizeStartRequest         Request);


    /// <summary>
    /// Initiate an start authorization for the given AuthToken at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">An AuthorizeStart request.</param>
    public delegate Task<AuthorizationStartResponse>

        OnAuthorizeStartDelegate        (DateTime                      Timestamp,
                                         EMPServerAPI                  Sender,
                                         AuthorizeStartRequest         Request);


    /// <summary>
    /// A delegate called whenever a response for an AuthorizeStart request was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeStartResponseDelegate(DateTime                      Timestamp,
                                         EMPServerAPI                  Sender,
                                         AuthorizationStartResponse    Response,
                                         TimeSpan                      Runtime);

    #endregion

    #region OnAuthorizeStop               (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever an AuthorizeStop request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeStopRequestDelegate (DateTime                     Timestamp,
                                        EMPServerAPI                 Sender,
                                        AuthorizeStopRequest         Request);


    /// <summary>
    /// Initiate an stop authorization for the given AuthToken at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<AuthorizationStopResponse>

        OnAuthorizeStopDelegate        (DateTime                     Timestamp,
                                        EMPServerAPI                 Sender,
                                        AuthorizeStopRequest         Request);


    /// <summary>
    /// A delegate called whenever a response for an AuthorizeStop request was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeStopResponseDelegate(DateTime                     Timestamp,
                                        EMPServerAPI                 Sender,
                                        AuthorizationStopResponse    Response,
                                        TimeSpan                     Runtime);

    #endregion


    #region OnChargingStartNotification   (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a charging start notification request was received.
    /// </summary>
    public delegate Task

        OnChargingStartNotificationRequestDelegate (DateTime                                             Timestamp,
                                                    EMPServerAPI                                         Sender,
                                                    ChargingStartNotificationRequest                     ChargingStartNotification);


    /// <summary>
    /// Send a charging start notification.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the notification.</param>
    /// <param name="Sender">The sender of the notification.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<Acknowledgement<ChargingStartNotificationRequest>>

        OnChargingStartNotificationDelegate        (DateTime                                             Timestamp,
                                                    EMPServerAPI                                         Sender,
                                                    ChargingStartNotificationRequest                     Request);


    /// <summary>
    /// A delegate called whenever a charging start notification response was sent.
    /// </summary>
    public delegate Task

        OnChargingStartNotificationResponseDelegate(DateTime                                             Timestamp,
                                                    EMPServerAPI                                         Sender,
                                                    Acknowledgement<ChargingStartNotificationRequest>    Response,
                                                    TimeSpan                                             Runtime);

    #endregion

    #region OnChargingProgressNotification(Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a charging progress notification request was received.
    /// </summary>
    public delegate Task

        OnChargingProgressNotificationRequestDelegate (DateTime                                                Timestamp,
                                                       EMPServerAPI                                            Sender,
                                                       ChargingProgressNotificationRequest                     ChargingProgressNotification);


    /// <summary>
    /// Send a charging progress notification.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the notification.</param>
    /// <param name="Sender">The sender of the notification.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<Acknowledgement<ChargingProgressNotificationRequest>>

        OnChargingProgressNotificationDelegate        (DateTime                                                Timestamp,
                                                       EMPServerAPI                                            Sender,
                                                       ChargingProgressNotificationRequest                     Request);


    /// <summary>
    /// A delegate called whenever a charging progress notification response was sent.
    /// </summary>
    public delegate Task

        OnChargingProgressNotificationResponseDelegate(DateTime                                                Timestamp,
                                                       EMPServerAPI                                            Sender,
                                                       Acknowledgement<ChargingProgressNotificationRequest>    Response,
                                                       TimeSpan                                                Runtime);

    #endregion

    #region OnChargingEndNotification     (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a charging end notification request was received.
    /// </summary>
    public delegate Task

        OnChargingEndNotificationRequestDelegate (DateTime                                           Timestamp,
                                                  EMPServerAPI                                       Sender,
                                                  ChargingEndNotificationRequest                     ChargingEndNotification);


    /// <summary>
    /// Send a charging end notification.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the notification.</param>
    /// <param name="Sender">The sender of the notification.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<Acknowledgement<ChargingEndNotificationRequest>>

        OnChargingEndNotificationDelegate        (DateTime                                           Timestamp,
                                                  EMPServerAPI                                       Sender,
                                                  ChargingEndNotificationRequest                     Request);


    /// <summary>
    /// A delegate called whenever a charging end notification response was sent.
    /// </summary>
    public delegate Task

        OnChargingEndNotificationResponseDelegate(DateTime                                           Timestamp,
                                                  EMPServerAPI                                       Sender,
                                                  Acknowledgement<ChargingEndNotificationRequest>    Response,
                                                  TimeSpan                                           Runtime);

    #endregion

    #region OnChargingErrorNotification   (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a charging error notification request was received.
    /// </summary>
    public delegate Task

        OnChargingErrorNotificationRequestDelegate (DateTime                                             Timestamp,
                                                    EMPServerAPI                                         Sender,
                                                    ChargingErrorNotificationRequest                     ChargingErrorNotification);


    /// <summary>
    /// Send a charging error notification.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the notification.</param>
    /// <param name="Sender">The sender of the notification.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<Acknowledgement<ChargingErrorNotificationRequest>>

        OnChargingErrorNotificationDelegate        (DateTime                                             Timestamp,
                                                    EMPServerAPI                                         Sender,
                                                    ChargingErrorNotificationRequest                     Request);


    /// <summary>
    /// A delegate called whenever a charging error notification response was sent.
    /// </summary>
    public delegate Task

        OnChargingErrorNotificationResponseDelegate(DateTime                                             Timestamp,
                                                    EMPServerAPI                                         Sender,
                                                    Acknowledgement<ChargingErrorNotificationRequest>    Response,
                                                    TimeSpan                                             Runtime);

    #endregion


    #region OnChargeDetailRecord          (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a charge detail record request was received.
    /// </summary>
    public delegate Task

        OnChargeDetailRecordRequestDelegate (DateTime                                          Timestamp,
                                             EMPServerAPI                                      Sender,
                                             ChargeDetailRecordRequest                     Request);


    /// <summary>
    /// Send a charge detail record.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<Acknowledgement<ChargeDetailRecordRequest>>

        OnChargeDetailRecordDelegate        (DateTime                                          Timestamp,
                                             EMPServerAPI                                      Sender,
                                             ChargeDetailRecordRequest                     Request);


    /// <summary>
    /// A delegate called whenever a charge detail record response was sent.
    /// </summary>
    public delegate Task

        OnChargeDetailRecordResponseDelegate(DateTime                                          Timestamp,
                                             EMPServerAPI                                      Sender,
                                             Acknowledgement<ChargeDetailRecordRequest>    Response,
                                             TimeSpan                                          Runtime);

    #endregion

}
