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

    #region OnPullEVSEDataRequest/-Response

    /// <summary>
    /// A delegate called whenever a PullEVSEData request will be send.
    /// </summary>
    public delegate Task OnPullEVSEDataRequestDelegate   (DateTime                                Timestamp,
                                                          IEMPClient                              Sender,
                                                          String                                  SenderId,
                                                          PullEVSEDataRequest                     Request);

    /// <summary>
    /// A delegate called whenever a response for a PullEVSEData request had been received.
    /// </summary>
    public delegate Task OnPullEVSEDataResponseDelegate  (DateTime                                Timestamp,
                                                          IEMPClient                              Sender,
                                                          String                                  SenderId,
                                                          PullEVSEDataRequest                     Request,
                                                          PullEVSEDataResponse                    Response);

    #endregion

    #region OnPullEVSEStatusRequest/-Response

    /// <summary>
    /// A delegate called whenever a PullEVSEStatus request will be send.
    /// </summary>
    public delegate Task OnPullEVSEStatusRequestDelegate (DateTime                                Timestamp,
                                                          IEMPClient                              Sender,
                                                          String                                  SenderId,
                                                          PullEVSEStatusRequest                   Request);

    /// <summary>
    /// A delegate called whenever a response for a PullEVSEStatus request had been received.
    /// </summary>
    public delegate Task OnPullEVSEStatusResponseDelegate(DateTime                                Timestamp,
                                                          EMPClient                               Sender,
                                                          String                                  SenderId,
                                                          PullEVSEStatusRequest                   Request,
                                                          PullEVSEStatusResponse                  Response);

    #endregion


    #region OnAuthorizeRemoteReservationStart/-Stop

    /// <summary>
    /// A delegate called whenever an AuthorizeRemoteReservationStart request will be send.
    /// </summary>
    public delegate Task OnAuthorizeRemoteReservationStartRequestDelegate (DateTime                                                  Timestamp,
                                                                           IEMPClient                                                Sender,
                                                                           String                                                    SenderId,
                                                                           AuthorizeRemoteReservationStartRequest                    Request);

    /// <summary>
    /// A delegate called whenever a response for an AuthorizeRemoteReservationStart request had been received.
    /// </summary>
    public delegate Task OnAuthorizeRemoteReservationStartResponseDelegate(DateTime                                                  Timestamp,
                                                                           IEMPClient                                                Sender,
                                                                           String                                                    SenderId,
                                                                           AuthorizeRemoteReservationStartRequest                    Request,
                                                                           Acknowledgement<AuthorizeRemoteReservationStartRequest>   Response);


    /// <summary>
    /// A delegate called whenever an AuthorizeRemoteReservationStop request will be send.
    /// </summary>
    public delegate Task OnAuthorizeRemoteReservationStopRequestDelegate  (DateTime                                                  Timestamp,
                                                                           IEMPClient                                                Sender,
                                                                           String                                                    SenderId,
                                                                           AuthorizeRemoteReservationStopRequest                     Request);

    /// <summary>
    /// A delegate called whenever a response for an AuthorizeRemoteReservationStop request had been received.
    /// </summary>
    public delegate Task OnAuthorizeRemoteReservationStopResponseDelegate (DateTime                                                  Timestamp,
                                                                           IEMPClient                                                Sender,
                                                                           String                                                    SenderId,
                                                                           AuthorizeRemoteReservationStopRequest                     Request,
                                                                           Acknowledgement<AuthorizeRemoteReservationStopRequest>    Response);

    #endregion

    #region OnAuthorizeRemoteStart/-Stop

    /// <summary>
    /// A delegate called whenever an AuthorizeRemoteStart request will be send.
    /// </summary>
    public delegate Task OnAuthorizeRemoteStartRequestDelegate (DateTime                                       Timestamp,
                                                                IEMPClient                                     Sender,
                                                                String                                         SenderId,
                                                                AuthorizeRemoteStartRequest                    Request);

    /// <summary>
    /// A delegate called whenever a response for an AuthorizeRemoteStart request had been received.
    /// </summary>
    public delegate Task OnAuthorizeRemoteStartResponseDelegate(DateTime                                       Timestamp,
                                                                IEMPClient                                     Sender,
                                                                String                                         SenderId,
                                                                AuthorizeRemoteStartRequest                    Request,
                                                                Acknowledgement<AuthorizeRemoteStartRequest>   Response);


    /// <summary>
    /// A delegate called whenever an AuthorizeRemoteStop request will be send.
    /// </summary>
    public delegate Task OnAuthorizeRemoteStopRequestDelegate  (DateTime                                       Timestamp,
                                                                IEMPClient                                     Sender,
                                                                String                                         SenderId,
                                                                AuthorizeRemoteStopRequest                     Request);

    /// <summary>
    /// A delegate called whenever a response for an AuthorizeRemoteStop request had been received.
    /// </summary>
    public delegate Task OnAuthorizeRemoteStopResponseDelegate (DateTime                                       Timestamp,
                                                                IEMPClient                                     Sender,
                                                                String                                         SenderId,
                                                                AuthorizeRemoteStopRequest                     Request,
                                                                Acknowledgement<AuthorizeRemoteStopRequest>    Response);

    #endregion


    #region OnGetChargeDetailRecords

    /// <summary>
    /// A delegate called whenever a GetChargeDetailRecords request will be send.
    /// </summary>
    public delegate Task OnGetChargeDetailRecordsRequestDelegate (DateTime                                         Timestamp,
                                                                  IEMPClient                                       Sender,
                                                                  String                                           SenderId,
                                                                  GetChargeDetailRecordsRequest                    Request);

    /// <summary>
    /// A delegate called whenever a response for a GetChargeDetailRecords request had been received.
    /// </summary>
    public delegate Task OnGetChargeDetailRecordsResponseDelegate(DateTime                                         Timestamp,
                                                                  IEMPClient                                       Sender,
                                                                  String                                           SenderId,
                                                                  GetChargeDetailRecordsRequest                    Request,
                                                                  Acknowledgement<GetChargeDetailRecordsRequest>   Response);

    #endregion

}
