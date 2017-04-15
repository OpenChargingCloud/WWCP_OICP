/*
 * Copyright (c) 2014-2017 GraphDefined GmbH
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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.EMP
{

    #region OnPullEVSEData

    /// <summary>
    /// A delegate called whenever a 'pull EVSE data' request will be send.
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
                                                       TimeSpan?                       RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response for a 'pull EVSE data' request had been received.
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
                                                       TimeSpan?                       RequestTimeout,
                                                       EVSEData                        Result,
                                                       TimeSpan                        Duration);

    #endregion

    #region OnSearchEVSE

    /// <summary>
    /// A delegate called whenever a 'search EVSE' request will be send.
    /// </summary>
    public delegate Task OnSearchEVSERequestHandler (DateTime                   LogTimestamp,
                                                     DateTime                   RequestTimestamp,
                                                     EMPClient                  Sender,
                                                     String                     SenderId,
                                                     EventTracking_Id           EventTrackingId,
                                                     Provider_Id                ProviderId,
                                                     GeoCoordinate?             SearchCenter,
                                                     Single                     DistanceKM,
                                                     Address                    Address,
                                                     PlugTypes?                 Plug,
                                                     ChargingFacilities?        ChargingFacility,
                                                     TimeSpan?                  RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response for a 'search EVSE' request had been received.
    /// </summary>
    public delegate Task OnSearchEVSEResponseHandler(DateTime                   Timestamp,
                                                     EMPClient                  Sender,
                                                     String                     SenderId,
                                                     EventTracking_Id           EventTrackingId,
                                                     Provider_Id                ProviderId,
                                                     GeoCoordinate?             SearchCenter,
                                                     Single                     DistanceKM,
                                                     Address                    Address,
                                                     PlugTypes?                 Plug,
                                                     ChargingFacilities?        ChargingFacility,
                                                     TimeSpan?                  RequestTimeout,
                                                     EVSESearchResult           Result,
                                                     TimeSpan                   Duration);

    #endregion

    #region OnPullEVSEStatusById

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
                                                         TimeSpan?            RequestTimeout);

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
                                                         TimeSpan?            RequestTimeout,
                                                         EVSEStatus           Result,
                                                         TimeSpan             Duration);

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
                                                             TimeSpan?                RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response for a 'pull EVSE status by id' request had been received.
    /// </summary>
    public delegate Task OnPullEVSEStatusByIdResponseHandler(DateTime                 Timestamp,
                                                             EMPClient                Sender,
                                                             String                   SenderId,
                                                             EventTracking_Id         EventTrackingId,
                                                             Provider_Id              ProviderId,
                                                             IEnumerable<EVSE_Id>     EVSEIds,
                                                             TimeSpan?                RequestTimeout,
                                                             EVSEStatusById           Result,
                                                             TimeSpan                 Duration);

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
                                                                 IEnumerable<AuthorizationIdentification>         AuthorizationIdentifications,
                                                                 Provider_Id                                      ProviderId,
                                                                 ActionTypes                                      OICPAction,
                                                                 TimeSpan?                                        RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response for a 'push authentication data' request had been received.
    /// </summary>
    public delegate Task OnPushAuthenticationDataResponseHandler(DateTime                                         Timestamp,
                                                                 EMPClient                                        Sender,
                                                                 String                                           SenderId,
                                                                 EventTracking_Id                                 EventTrackingId,
                                                                 IEnumerable<AuthorizationIdentification>         AuthorizationIdentifications,
                                                                 Provider_Id                                      ProviderId,
                                                                 ActionTypes                                      OICPAction,
                                                                 TimeSpan?                                        RequestTimeout,
                                                                 Acknowledgement<PushAuthenticationDataRequest>   Result,
                                                                 TimeSpan                                         Duration);

    #endregion

    #region OnReservationStart/-Stop

    /// <summary>
    /// A delegate called whenever a 'reservation start' request will be send.
    /// </summary>
    public delegate Task OnReservationStartRequestHandler (DateTime                                                  LogTimestamp,
                                                           DateTime                                                  RequestTimestamp,
                                                           EMPClient                                                 Sender,
                                                           String                                                    SenderId,
                                                           EventTracking_Id                                          EventTrackingId,
                                                           Provider_Id                                               ProviderId,
                                                           EVSE_Id                                                   EVSEId,
                                                           EVCO_Id                                                   EVCOId,
                                                           Session_Id?                                               SessionId,
                                                           PartnerSession_Id?                                        PartnerSessionId,
                                                           PartnerProduct_Id?                                        PartnerProductId,
                                                           TimeSpan?                                                 RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response for a 'reservation start' request had been received.
    /// </summary>
    public delegate Task OnReservationStartResponseHandler(DateTime                                                  Timestamp,
                                                           EMPClient                                                 Sender,
                                                           String                                                    SenderId,
                                                           EventTracking_Id                                          EventTrackingId,
                                                           Provider_Id                                               ProviderId,
                                                           EVSE_Id                                                   EVSEId,
                                                           EVCO_Id                                                   EVCOId,
                                                           Session_Id?                                               SessionId,
                                                           PartnerSession_Id?                                        PartnerSessionId,
                                                           PartnerProduct_Id?                                        PartnerProductId,
                                                           TimeSpan?                                                 RequestTimeout,
                                                           Acknowledgement<AuthorizeRemoteReservationStartRequest>   Result,
                                                           TimeSpan                                                  Duration);


    /// <summary>
    /// A delegate called whenever a reservation stop request will be send.
    /// </summary>
    public delegate Task OnReservationStopRequestHandler  (DateTime                                                  LogTimestamp,
                                                           DateTime                                                  RequestTimestamp,
                                                           EMPClient                                                 Sender,
                                                           String                                                    SenderId,
                                                           EventTracking_Id                                          EventTrackingId,
                                                           Session_Id                                                SessionId,
                                                           Provider_Id                                               ProviderId,
                                                           EVSE_Id                                                   EVSEId,
                                                           PartnerSession_Id?                                        PartnerSessionId,
                                                           TimeSpan?                                                 RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response for a reservation stop request had been received.
    /// </summary>
    public delegate Task OnReservationStopResponseHandler (DateTime                                                  Timestamp,
                                                           EMPClient                                                 Sender,
                                                           String                                                    SenderId,
                                                           EventTracking_Id                                          EventTrackingId,
                                                           Session_Id                                                SessionId,
                                                           Provider_Id                                               ProviderId,
                                                           EVSE_Id                                                   EVSEId,
                                                           PartnerSession_Id?                                        PartnerSessionId,
                                                           TimeSpan?                                                 RequestTimeout,
                                                           Acknowledgement<AuthorizeRemoteReservationStopRequest>    Result,
                                                           TimeSpan                                                  Duration);

    #endregion

    #region OnRemoteStart/-Stop

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
                                                               EVCO_Id                                        EVCOId,
                                                               Session_Id?                                    SessionId,
                                                               PartnerSession_Id?                             PartnerSessionId,
                                                               PartnerProduct_Id?                             PartnerProductId,
                                                               TimeSpan?                                      RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response for an 'authorize remote start' request had been received.
    /// </summary>
    public delegate Task OnAuthorizeRemoteStartResponseHandler(DateTime                                       Timestamp,
                                                               EMPClient                                      Sender,
                                                               String                                         SenderId,
                                                               EventTracking_Id                               EventTrackingId,
                                                               Provider_Id                                    ProviderId,
                                                               EVSE_Id                                        EVSEId,
                                                               EVCO_Id                                        EVCOId,
                                                               Session_Id?                                    SessionId,
                                                               PartnerSession_Id?                             PartnerSessionId,
                                                               PartnerProduct_Id?                             PartnerProductId,
                                                               TimeSpan?                                      RequestTimeout,
                                                               Acknowledgement<AuthorizeRemoteStartRequest>   Result,
                                                               TimeSpan                                       Duration);


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
                                                               PartnerSession_Id?                             PartnerSessionId,
                                                               TimeSpan?                                      RequestTimeout);

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
                                                               PartnerSession_Id?                             PartnerSessionId,
                                                               TimeSpan?                                      RequestTimeout,
                                                               Acknowledgement<AuthorizeRemoteStopRequest>    Result,
                                                               TimeSpan                                       Duration);

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
                                                                 TimeSpan?                        RequestTimeout);

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
                                                                 TimeSpan?                        RequestTimeout,
                                                                 GetChargeDetailRecordsResponse   Result,
                                                                 TimeSpan                         Duration);

    #endregion

}
