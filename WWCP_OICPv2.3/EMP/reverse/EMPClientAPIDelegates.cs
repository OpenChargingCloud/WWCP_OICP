/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.EMP
{

    #region OnPullEVSEDataAPI                  (Request|Response)Delegate

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
                                          OICPResult<PullEVSEDataResponse>    Response,
                                          TimeSpan                            Runtime);

    #endregion

    #region OnPullEVSEStatusAPI                (Request|Response)Delegate

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
                                            OICPResult<PullEVSEStatusResponse>    Response,
                                            TimeSpan                              Runtime);

    #endregion

    #region OnPullEVSEStatusByIdAPI            (Request|Response)Delegate

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
                                                OICPResult<PullEVSEStatusByIdResponse>    Response,
                                                TimeSpan                                  Runtime);

    #endregion

    #region OnPullEVSEStatusByOperatorIdAPI    (Request|Response)Delegate

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
                                                        OICPResult<PullEVSEStatusByOperatorIdResponse>    Response,
                                                        TimeSpan                                          Runtime);

    #endregion


    #region OnPushAuthenticationDataAPI        (Request|Response)Delegate

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
                                                    OICPResult<Acknowledgement<PushAuthenticationDataRequest>>    Response,
                                                    TimeSpan                                                      Runtime);

    #endregion


}
