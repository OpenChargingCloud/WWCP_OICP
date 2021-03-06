﻿/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2.EMP
{

    #region OnPullEVSEData

    /// <summary>
    /// A delegate called whenever a 'PullEVSEData' request will be send.
    /// </summary>
    public delegate Task OnPullEVSEDataRequestHandler (DateTime                        LogTimestamp,
                                                       DateTime                        RequestTimestamp,
                                                       IEMPClient                      Sender,
                                                       String                          SenderId,
                                                       EventTracking_Id                EventTrackingId,
                                                       Provider_Id                     ProviderId,
                                                       GeoCoordinate?                  SearchCenter,
                                                       Single                          DistanceKM,
                                                       DateTime?                       LastCall,
                                                       GeoCoordinatesResponseFormats   GeoCoordinatesResponseFormat,
                                                       TimeSpan                        RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response for a 'PullEVSEData' request had been received.
    /// </summary>
    public delegate Task OnPullEVSEDataResponseHandler(DateTime                        Timestamp,
                                                       IEMPClient                      Sender,
                                                       String                          SenderId,
                                                       EventTracking_Id                EventTrackingId,
                                                       Provider_Id                     ProviderId,
                                                       GeoCoordinate?                  SearchCenter,
                                                       Single                          DistanceKM,
                                                       DateTime?                       LastCall,
                                                       GeoCoordinatesResponseFormats   GeoCoordinatesResponseFormat,
                                                       TimeSpan                        RequestTimeout,
                                                       EVSEData                        EVSEData,
                                                       StatusCode?                     StatusCode,
                                                       TimeSpan                        Runtime);

    #endregion

    #region OnPullEVSEStatus

    /// <summary>
    /// A delegate called whenever a 'pull EVSE status' request will be send.
    /// </summary>
    public delegate Task OnPullEVSEStatusRequestHandler (DateTime             LogTimestamp,
                                                         DateTime             RequestTimestamp,
                                                         EMPClient            Sender,
                                                         String               SenderId,
                                                         EventTracking_Id     EventTrackingId,
                                                         Provider_Id          ProviderId,
                                                         GeoCoordinate?       SearchCenter,
                                                         Single               DistanceKM,
                                                         EVSEStatusTypes?     EVSEStatusFilter,
                                                         TimeSpan             RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response for a 'pull EVSE status' request had been received.
    /// </summary>
    public delegate Task OnPullEVSEStatusResponseHandler(DateTime             Timestamp,
                                                         EMPClient            Sender,
                                                         String               SenderId,
                                                         EventTracking_Id     EventTrackingId,
                                                         Provider_Id          ProviderId,
                                                         GeoCoordinate?       SearchCenter,
                                                         Single               DistanceKM,
                                                         EVSEStatusTypes?     EVSEStatusFilter,
                                                         TimeSpan             RequestTimeout,
                                                         EVSEStatus           EVSEStatus,
                                                         TimeSpan             Runtime);

    #endregion

    #region OnPullEVSEStatusById

    /// <summary>
    /// A delegate called whenever a 'pull EVSE status by id' request will be send.
    /// </summary>
    public delegate Task OnPullEVSEStatusByIdRequestHandler (DateTime                 LogTimestamp,
                                                             DateTime                 RequestTimestamp,
                                                             EMPClient                Sender,
                                                             String                   SenderId,
                                                             EventTracking_Id         EventTrackingId,
                                                             Provider_Id              ProviderId,
                                                             IEnumerable<EVSE_Id>     EVSEIds,
                                                             TimeSpan                 RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response for a 'pull EVSE status by id' request had been received.
    /// </summary>
    public delegate Task OnPullEVSEStatusByIdResponseHandler(DateTime                 Timestamp,
                                                             EMPClient                Sender,
                                                             String                   SenderId,
                                                             EventTracking_Id         EventTrackingId,
                                                             Provider_Id              ProviderId,
                                                             IEnumerable<EVSE_Id>     EVSEIds,
                                                             TimeSpan                 RequestTimeout,
                                                             EVSEStatusById           EVSEStatusById,
                                                             TimeSpan                 Runtime);

    #endregion

    #region OnPullEVSEStatusByOperatorId

    /// <summary>
    /// A delegate called whenever a 'pull EVSE status by operator id' request will be send.
    /// </summary>
    public delegate Task OnPullEVSEStatusByOperatorIdRequestHandler (DateTime                  LogTimestamp,
                                                                     DateTime                  RequestTimestamp,
                                                                     EMPClient                 Sender,
                                                                     String                    SenderId,
                                                                     EventTracking_Id          EventTrackingId,
                                                                     Provider_Id               ProviderId,
                                                                     IEnumerable<Operator_Id>  OperatorIds,
                                                                     TimeSpan                  RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response for a 'pull EVSE status by operator id' request had been received.
    /// </summary>
    public delegate Task OnPullEVSEStatusByOperatorIdResponseHandler(DateTime                  Timestamp,
                                                                     EMPClient                 Sender,
                                                                     String                    SenderId,
                                                                     EventTracking_Id          EventTrackingId,
                                                                     Provider_Id               ProviderId,
                                                                     IEnumerable<Operator_Id>  OperatorIds,
                                                                     TimeSpan                  RequestTimeout,
                                                                     EVSEStatus2               EVSEStatus,
                                                                     TimeSpan                  Runtime);

    #endregion


    #region OnPushAuthenticationData

    /// <summary>
    /// A delegate called whenever a 'push authentication data' request will be send.
    /// </summary>
    public delegate Task OnPushAuthenticationDataRequestHandler (DateTime                                         LogTimestamp,
                                                                 DateTime                                         RequestTimestamp,
                                                                 EMPClient                                        Sender,
                                                                 String                                           SenderId,
                                                                 EventTracking_Id                                 EventTrackingId,
                                                                 ProviderAuthenticationData                       ProviderAuthenticationData,
                                                                 ActionTypes                                      OICPAction,
                                                                 TimeSpan                                         RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response for a 'push authentication data' request had been received.
    /// </summary>
    public delegate Task OnPushAuthenticationDataResponseHandler(DateTime                                         Timestamp,
                                                                 EMPClient                                        Sender,
                                                                 String                                           SenderId,
                                                                 EventTracking_Id                                 EventTrackingId,
                                                                 ProviderAuthenticationData                       ProviderAuthenticationData,
                                                                 ActionTypes                                      OICPAction,
                                                                 TimeSpan                                         RequestTimeout,
                                                                 Acknowledgement<PushAuthenticationDataRequest>   Acknowledgement,
                                                                 TimeSpan                                         Runtime);

    #endregion


    #region OnAuthorizeRemoteReservationStart/-Stop

    /// <summary>
    /// A delegate called whenever a 'reservation start' request will be send.
    /// </summary>
    public delegate Task OnAuthorizeRemoteReservationStartRequestHandler (DateTime                                                  LogTimestamp,
                                                                          DateTime                                                  RequestTimestamp,
                                                                          EMPClient                                                 Sender,
                                                                          String                                                    SenderId,
                                                                          EventTracking_Id                                          EventTrackingId,
                                                                          Provider_Id                                               ProviderId,
                                                                          EVSE_Id                                                   EVSEId,
                                                                          Identification                                            Identification,
                                                                          Session_Id?                                               SessionId,
                                                                          CPOPartnerSession_Id?                                     CPOPartnerSessionId,
                                                                          EMPPartnerSession_Id?                                     EMPPartnerSessionId,
                                                                          PartnerProduct_Id?                                        PartnerProductId,
                                                                          TimeSpan                                                  RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response for a 'reservation start' request had been received.
    /// </summary>
    public delegate Task OnAuthorizeRemoteReservationStartResponseHandler(DateTime                                                  Timestamp,
                                                                          EMPClient                                                 Sender,
                                                                          String                                                    SenderId,
                                                                          EventTracking_Id                                          EventTrackingId,
                                                                          Provider_Id                                               ProviderId,
                                                                          EVSE_Id                                                   EVSEId,
                                                                          Identification                                            Identification,
                                                                          Session_Id?                                               SessionId,
                                                                          CPOPartnerSession_Id?                                     CPOPartnerSessionId,
                                                                          EMPPartnerSession_Id?                                     EMPPartnerSessionId,
                                                                          PartnerProduct_Id?                                        PartnerProductId,
                                                                          TimeSpan                                                  RequestTimeout,
                                                                          Acknowledgement<AuthorizeRemoteReservationStartRequest>   Acknowledgement,
                                                                          TimeSpan                                                  Runtime);


    /// <summary>
    /// A delegate called whenever a reservation stop request will be send.
    /// </summary>
    public delegate Task OnAuthorizeRemoteReservationStopRequestHandler  (DateTime                                                  LogTimestamp,
                                                                          DateTime                                                  RequestTimestamp,
                                                                          EMPClient                                                 Sender,
                                                                          String                                                    SenderId,
                                                                          EventTracking_Id                                          EventTrackingId,
                                                                          Session_Id                                                SessionId,
                                                                          Provider_Id                                               ProviderId,
                                                                          EVSE_Id                                                   EVSEId,
                                                                          CPOPartnerSession_Id?                                     CPOPartnerSessionId,
                                                                          EMPPartnerSession_Id?                                     EMPPartnerSessionId,
                                                                          TimeSpan                                                  RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response for a reservation stop request had been received.
    /// </summary>
    public delegate Task OnAuthorizeRemoteReservationStopResponseHandler (DateTime                                                  Timestamp,
                                                                          EMPClient                                                 Sender,
                                                                          String                                                    SenderId,
                                                                          EventTracking_Id                                          EventTrackingId,
                                                                          Session_Id                                                SessionId,
                                                                          Provider_Id                                               ProviderId,
                                                                          EVSE_Id                                                   EVSEId,
                                                                          CPOPartnerSession_Id?                                     CPOPartnerSessionId,
                                                                          EMPPartnerSession_Id?                                     EMPPartnerSessionId,
                                                                          TimeSpan                                                  RequestTimeout,
                                                                          Acknowledgement<AuthorizeRemoteReservationStopRequest>    Acknowledgement,
                                                                          TimeSpan                                                  Runtime);

    #endregion

    #region OnAuthorizeRemoteStart/-Stop

    /// <summary>
    /// A delegate called whenever an 'authorize remote start' request will be send.
    /// </summary>
    public delegate Task OnAuthorizeRemoteStartRequestHandler (DateTime                                       LogTimestamp,
                                                               DateTime                                       RequestTimestamp,
                                                               EMPClient                                      Sender,
                                                               String                                         SenderId,
                                                               EventTracking_Id                               EventTrackingId,
                                                               Provider_Id                                    ProviderId,
                                                               EVSE_Id                                        EVSEId,
                                                               Identification                                 Identification,
                                                               Session_Id?                                    SessionId,
                                                               CPOPartnerSession_Id?                          CPOPartnerSessionId,
                                                               EMPPartnerSession_Id?                          EMPPartnerSessionId,
                                                               PartnerProduct_Id?                             PartnerProductId,
                                                               TimeSpan                                       RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response for an 'authorize remote start' request had been received.
    /// </summary>
    public delegate Task OnAuthorizeRemoteStartResponseHandler(DateTime                                       Timestamp,
                                                               EMPClient                                      Sender,
                                                               String                                         SenderId,
                                                               EventTracking_Id                               EventTrackingId,
                                                               Provider_Id                                    ProviderId,
                                                               EVSE_Id                                        EVSEId,
                                                               Identification                                 Identification,
                                                               Session_Id?                                    SessionId,
                                                               CPOPartnerSession_Id?                          CPOPartnerSessionId,
                                                               EMPPartnerSession_Id?                          EMPPartnerSessionId,
                                                               PartnerProduct_Id?                             PartnerProductId,
                                                               TimeSpan                                       RequestTimeout,
                                                               Acknowledgement<AuthorizeRemoteStartRequest>   Acknowledgement,
                                                               TimeSpan                                       Runtime);


    /// <summary>
    /// A delegate called whenever an 'authorize remote stop' request will be send.
    /// </summary>
    public delegate Task OnAuthorizeRemoteStopRequestHandler  (DateTime                                       LogTimestamp,
                                                               DateTime                                       RequestTimestamp,
                                                               EMPClient                                      Sender,
                                                               String                                         SenderId,
                                                               EventTracking_Id                               EventTrackingId,
                                                               Session_Id                                     SessionId,
                                                               Provider_Id                                    ProviderId,
                                                               EVSE_Id                                        EVSEId,
                                                               CPOPartnerSession_Id?                          CPOPartnerSessionId,
                                                               EMPPartnerSession_Id?                          EMPPartnerSessionId,
                                                               TimeSpan                                       RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response for an 'authorize remote stop' request had been received.
    /// </summary>
    public delegate Task OnAuthorizeRemoteStopResponseHandler (DateTime                                       Timestamp,
                                                               EMPClient                                      Sender,
                                                               String                                         SenderId,
                                                               EventTracking_Id                               EventTrackingId,
                                                               Session_Id                                     SessionId,
                                                               Provider_Id                                    ProviderId,
                                                               EVSE_Id                                        EVSEId,
                                                               CPOPartnerSession_Id?                          CPOPartnerSessionId,
                                                               EMPPartnerSession_Id?                          EMPPartnerSessionId,
                                                               TimeSpan                                       RequestTimeout,
                                                               Acknowledgement<AuthorizeRemoteStopRequest>    Acknowledgement,
                                                               TimeSpan                                       Runtime);

    #endregion


    #region OnGetChargeDetailRecords

    /// <summary>
    /// A delegate called whenever a 'get charge detail records' request will be send.
    /// </summary>
    public delegate Task OnGetChargeDetailRecordsRequestHandler (DateTime                         LogTimestamp,
                                                                 DateTime                         RequestTimestamp,
                                                                 EMPClient                        Sender,
                                                                 String                           SenderId,
                                                                 EventTracking_Id                 EventTrackingId,
                                                                 Provider_Id                      ProviderId,
                                                                 DateTime                         From,
                                                                 DateTime                         To,
                                                                 TimeSpan                         RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response for a 'get charge detail records' request had been received.
    /// </summary>
    public delegate Task OnGetChargeDetailRecordsResponseHandler(DateTime                         Timestamp,
                                                                 EMPClient                        Sender,
                                                                 String                           SenderId,
                                                                 EventTracking_Id                 EventTrackingId,
                                                                 Provider_Id                      ProviderId,
                                                                 DateTime                         From,
                                                                 DateTime                         To,
                                                                 TimeSpan                         RequestTimeout,
                                                                 IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
                                                                 StatusCode?                      StatusCode,
                                                                 TimeSpan                         Runtime);

    #endregion

}
