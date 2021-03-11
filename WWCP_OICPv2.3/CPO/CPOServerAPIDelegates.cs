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

    #region OnAuthorizeRemoteStart(Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever an AuthorizeRemoteStart request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteStartRequestDelegate(DateTime                       Timestamp,
                                              CPOServerAPI                   Sender,
                                              AuthorizeRemoteStartRequest    Request);


    /// <summary>
    /// Initiate a remote start at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">An AuthorizeStart request.</param>
    public delegate Task<Acknowledgement<AuthorizeRemoteStartRequest>>

        OnAuthorizeRemoteStartDelegate(DateTime                       Timestamp,
                                       CPOServerAPI                   Sender,
                                       AuthorizeRemoteStartRequest    Request);


    /// <summary>
    /// A delegate called whenever a response for an AuthorizeRemoteStart request was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteStartResponseDelegate(DateTime                                        Timestamp,
                                               CPOServerAPI                                    Sender,
                                               Acknowledgement<AuthorizeRemoteStartRequest>    Response,
                                               TimeSpan                                        Runtime);

    #endregion

    #region OnAuthorizeRemoteStop (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever an AuthorizeRemoteStop request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteStopRequestDelegate(DateTime                      Timestamp,
                                             CPOServerAPI                  Sender,
                                             AuthorizeRemoteStopRequest    Request);


    /// <summary>
    /// Initiate a remote stop at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">An AuthorizeStop request.</param>
    public delegate Task<Acknowledgement<AuthorizeRemoteStopRequest>>

        OnAuthorizeRemoteStopDelegate(DateTime                      Timestamp,
                                      CPOServerAPI                  Sender,
                                      AuthorizeRemoteStopRequest    Request);


    /// <summary>
    /// A delegate called whenever a response for an AuthorizeRemoteStop request was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteStopResponseDelegate(DateTime                                       Timestamp,
                                              CPOServerAPI                                   Sender,
                                              Acknowledgement<AuthorizeRemoteStopRequest>    Response,
                                              TimeSpan                                       Runtime);

    #endregion

}
