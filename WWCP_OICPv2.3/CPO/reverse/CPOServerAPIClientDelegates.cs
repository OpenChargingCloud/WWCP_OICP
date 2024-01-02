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

    #region AuthorizeRemoteReservationStart(Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever an AuthorizeRemoteReservationStart request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteReservationStartClientRequestDelegate (DateTime                                                               Timestamp,
                                                                CPOServerAPIClient                                                     Sender,
                                                                AuthorizeRemoteReservationStartRequest                                 Request);


    /// <summary>
    /// Initiate an AuthorizeRemoteReservationStart for the given identification at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<AuthorizationStartResponse>>

        OnAuthorizeRemoteReservationStartClientDelegate        (DateTime                                                               Timestamp,
                                                                CPOServerAPIClient                                                     Sender,
                                                                AuthorizeRemoteReservationStartRequest                                 Request);


    /// <summary>
    /// A delegate called whenever a response for an AuthorizeRemoteReservationStart request was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteReservationStartClientResponseDelegate(DateTime                                                               Timestamp,
                                                                CPOServerAPIClient                                                     Sender,
                                                                AuthorizeRemoteReservationStartRequest                                 Request,
                                                                OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>    Response,
                                                                TimeSpan                                                               Runtime);

    #endregion

    #region AuthorizeRemoteReservationStop (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever an AuthorizeRemoteReservationStop request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteReservationStopClientRequestDelegate (DateTime                                                              Timestamp,
                                                               CPOServerAPIClient                                                    Sender,
                                                               AuthorizeRemoteReservationStopRequest                                 Request);


    /// <summary>
    /// Initiate an AuthorizeRemoteReservationStop for the given identification at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<AuthorizationStopResponse>>

        OnAuthorizeRemoteReservationStopClientDelegate        (DateTime                                                              Timestamp,
                                                               CPOServerAPIClient                                                    Sender,
                                                               AuthorizeRemoteReservationStopRequest                                 Request);


    /// <summary>
    /// A delegate called whenever a response for an AuthorizeRemoteReservationStop request was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteReservationStopClientResponseDelegate(DateTime                                                              Timestamp,
                                                               CPOServerAPIClient                                                    Sender,
                                                               AuthorizeRemoteReservationStopRequest                                 Request,
                                                               OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>    Response,
                                                               TimeSpan                                                              Runtime);

    #endregion


    #region AuthorizeRemoteStart(Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever an AuthorizeRemoteStart request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteStartClientRequestDelegate (DateTime                                                    Timestamp,
                                                     CPOServerAPIClient                                          Sender,
                                                     AuthorizeRemoteStartRequest                                 Request);


    /// <summary>
    /// Initiate an AuthorizeRemoteStart for the given identification at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<AuthorizationStartResponse>>

        OnAuthorizeRemoteStartClientDelegate        (DateTime                                                    Timestamp,
                                                     CPOServerAPIClient                                          Sender,
                                                     AuthorizeRemoteStartRequest                                 Request);


    /// <summary>
    /// A delegate called whenever a response for an AuthorizeRemoteStart request was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteStartClientResponseDelegate(DateTime                                                    Timestamp,
                                                     CPOServerAPIClient                                          Sender,
                                                     AuthorizeRemoteStartRequest                                 Request,
                                                     OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>    Response,
                                                     TimeSpan                                                    Runtime);

    #endregion

    #region AuthorizeRemoteStop (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever an AuthorizeRemoteStop request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteStopClientRequestDelegate (DateTime                                                   Timestamp,
                                                    CPOServerAPIClient                                         Sender,
                                                    AuthorizeRemoteStopRequest                                 Request);


    /// <summary>
    /// Initiate an AuthorizeRemoteStop for the given identification at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<OICPResult<AuthorizationStopResponse>>

        OnAuthorizeRemoteStopClientDelegate        (DateTime                                                   Timestamp,
                                                    CPOServerAPIClient                                         Sender,
                                                    AuthorizeRemoteStopRequest                                 Request);


    /// <summary>
    /// A delegate called whenever a response for an AuthorizeRemoteStop request was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteStopClientResponseDelegate(DateTime                                                   Timestamp,
                                                    CPOServerAPIClient                                         Sender,
                                                    AuthorizeRemoteStopRequest                                 Request,
                                                    OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>    Response,
                                                    TimeSpan                                                   Runtime);

    #endregion

}
