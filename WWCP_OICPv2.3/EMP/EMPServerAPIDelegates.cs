﻿/*
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

    #region OnAuthorizeStart    (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a authorize start request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeStartRequestDelegate(DateTime                 Timestamp,
                                        EMPServerAPI             Sender,
                                        AuthorizeStartRequest    Request);


    /// <summary>
    /// Initiate an AuthorizeStart for the given AuthToken at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">An AuthorizeStart request.</param>
    public delegate Task<AuthorizationStartResponse>

        OnAuthorizeStartDelegate(DateTime                 Timestamp,
                                 EMPServerAPI             Sender,
                                 AuthorizeStartRequest    Request);


    /// <summary>
    /// A delegate called whenever a authorize start response was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeStartResponseDelegate(DateTime                      Timestamp,
                                         EMPServerAPI                  Sender,
                                         AuthorizationStartResponse    Response,
                                         TimeSpan                      Runtime);

    #endregion

    #region OnAuthorizeStop     (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a authorize stop request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeStopRequestDelegate(DateTime                 Timestamp,
                                       EMPServerAPI             Sender,
                                       AuthorizeStopRequest     Request);


    /// <summary>
    /// Initiate a remote stop of the given charging session at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<AuthorizationStopResponse>

        OnAuthorizeStopDelegate(DateTime                Timestamp,
                                EMPServerAPI            Sender,
                                AuthorizeStopRequest    Request);


    /// <summary>
    /// A delegate called whenever a authorize stop response was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeStopResponseDelegate(DateTime                     Timestamp,
                                        EMPServerAPI                 Sender,
                                        AuthorizationStopResponse    Response,
                                        TimeSpan                     Runtime);

    #endregion

    #region OnChargeDetailRecord(Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a charge detail record request was received.
    /// </summary>
    public delegate Task

        OnChargeDetailRecordRequestDelegate(DateTime              Timestamp,
                                            EMPServerAPI          Sender,
                                            ChargeDetailRecord    ChargeDetailRecord);


    /// <summary>
    /// Send a charge detail record.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<Acknowledgement<SendChargeDetailRecordRequest>>

        OnChargeDetailRecordDelegate       (DateTime                       Timestamp,
                                            EMPServerAPI                   Sender,
                                            SendChargeDetailRecordRequest  Request);


    /// <summary>
    /// A delegate called whenever a charge detail record response was sent.
    /// </summary>
    public delegate Task

        OnChargeDetailRecordResponseDelegate(DateTime                                          Timestamp,
                                             EMPServerAPI                                      Sender,
                                             Acknowledgement<SendChargeDetailRecordRequest>    Response,
                                             TimeSpan                                          Runtime);

    #endregion

}
