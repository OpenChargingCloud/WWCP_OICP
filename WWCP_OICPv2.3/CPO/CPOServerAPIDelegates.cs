/*
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

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    #region OnAuthorizeRemoteReservationStart(Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever an AuthorizeRemoteReservationStart request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteReservationStartRequestDelegate(DateTimeOffset                            Timestamp,
                                                         CPOServerAPI                              Sender,
                                                         AuthorizeRemoteReservationStartRequest    Request);


    /// <summary>
    /// Initiate a remote reservation start at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">An AuthorizeStart request.</param>
    public delegate Task<Acknowledgement<AuthorizeRemoteReservationStartRequest>>

        OnAuthorizeRemoteReservationStartDelegate(DateTimeOffset                            Timestamp,
                                                  CPOServerAPI                              Sender,
                                                  AuthorizeRemoteReservationStartRequest    Request);


    /// <summary>
    /// A delegate called whenever a response for an AuthorizeRemoteReservationStart request was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteReservationStartResponseDelegate(DateTimeOffset                                             Timestamp,
                                                          CPOServerAPI                                               Sender,
                                                          AuthorizeRemoteReservationStartRequest                     Request,
                                                          Acknowledgement<AuthorizeRemoteReservationStartRequest>    Response,
                                                          TimeSpan                                                   Runtime);

    #endregion

    #region OnAuthorizeRemoteReservationStop (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever an AuthorizeRemoteReservationStop request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteReservationStopRequestDelegate(DateTimeOffset                           Timestamp,
                                                        CPOServerAPI                             Sender,
                                                        AuthorizeRemoteReservationStopRequest    Request);


    /// <summary>
    /// Initiate a remote reservation stop at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">An AuthorizeStop request.</param>
    public delegate Task<Acknowledgement<AuthorizeRemoteReservationStopRequest>>

        OnAuthorizeRemoteReservationStopDelegate(DateTimeOffset                           Timestamp,
                                                 CPOServerAPI                             Sender,
                                                 AuthorizeRemoteReservationStopRequest    Request);


    /// <summary>
    /// A delegate called whenever a response for an AuthorizeRemoteReservationStop request was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteReservationStopResponseDelegate(DateTimeOffset                                            Timestamp,
                                                         CPOServerAPI                                              Sender,
                                                         AuthorizeRemoteReservationStopRequest                     Request,
                                                         Acknowledgement<AuthorizeRemoteReservationStopRequest>    Response,
                                                         TimeSpan                                                  Runtime);

    #endregion


    #region OnAuthorizeRemoteStart(Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever an AuthorizeRemoteStart request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteStartRequestDelegate(DateTimeOffset                 Timestamp,
                                              CPOServerAPI                   Sender,
                                              AuthorizeRemoteStartRequest    Request);


    /// <summary>
    /// Initiate a remote start at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">An AuthorizeStart request.</param>
    public delegate Task<Acknowledgement<AuthorizeRemoteStartRequest>>

        OnAuthorizeRemoteStartDelegate(DateTimeOffset                 Timestamp,
                                       CPOServerAPI                   Sender,
                                       AuthorizeRemoteStartRequest    Request);


    /// <summary>
    /// A delegate called whenever a response for an AuthorizeRemoteStart request was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteStartResponseDelegate(DateTimeOffset                                  Timestamp,
                                               CPOServerAPI                                    Sender,
                                               AuthorizeRemoteStartRequest                     Request,
                                               Acknowledgement<AuthorizeRemoteStartRequest>    Response,
                                               TimeSpan                                        Runtime);

    #endregion

    #region OnAuthorizeRemoteStop (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever an AuthorizeRemoteStop request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteStopRequestDelegate(DateTimeOffset                Timestamp,
                                             CPOServerAPI                  Sender,
                                             AuthorizeRemoteStopRequest    Request);


    /// <summary>
    /// Initiate a remote stop at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">An AuthorizeStop request.</param>
    public delegate Task<Acknowledgement<AuthorizeRemoteStopRequest>>

        OnAuthorizeRemoteStopDelegate(DateTimeOffset                Timestamp,
                                      CPOServerAPI                  Sender,
                                      AuthorizeRemoteStopRequest    Request);


    /// <summary>
    /// A delegate called whenever a response for an AuthorizeRemoteStop request was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteStopResponseDelegate(DateTimeOffset                                 Timestamp,
                                              CPOServerAPI                                   Sender,
                                              AuthorizeRemoteStopRequest                     Request,
                                              Acknowledgement<AuthorizeRemoteStopRequest>    Response,
                                              TimeSpan                                       Runtime);

    #endregion

}
