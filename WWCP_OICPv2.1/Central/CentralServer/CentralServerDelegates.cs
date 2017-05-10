/*
 * Copyright (c) 2014-2017 GraphDefined GmbH
 * This file is part of WWCP OCHP <https://github.com/OpenChargingCloud/WWCP_OCHP>
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

using org.GraphDefined.WWCP.OICPv2_1.CPO;
using org.GraphDefined.WWCP.OICPv2_1.EMP;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.Central
{

    // EMP event delegates...

    #region OnPullEVSEData      (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a PullEVSEData request was received.
    /// </summary>
    public delegate Task

        OnPullEVSEDataRequestDelegate (DateTime                         LogTimestamp,
                                       DateTime                         RequestTimestamp,
                                       CentralServer                    Sender,
                                       String                           SenderId,
                                       EventTracking_Id                 EventTrackingId,
                                       Provider_Id                      ProviderId,
                                       GeoCoordinate?                   SearchCenter,
                                       Single                           DistanceKM,
                                       DateTime?                        LastCall,
                                       GeoCoordinatesResponseFormats?   GeoCoordinatesResponseFormat,
                                       TimeSpan                         RequestTimeout);


    /// <summary>
    /// Send a PullEVSEData request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<EVSEData>

        OnPullEVSEDataDelegate(DateTime             Timestamp,
                               CentralServer        Sender,
                               PullEVSEDataRequest  Request);


    /// <summary>
    /// A delegate called whenever a PullEVSEData response was sent.
    /// </summary>
    public delegate Task

        OnPullEVSEDataResponseDelegate(DateTime                         Timestamp,
                                       CentralServer                    Sender,
                                       String                           SenderId,
                                       EventTracking_Id                 EventTrackingId,
                                       Provider_Id                      ProviderId,
                                       GeoCoordinate?                   SearchCenter,
                                       Single                           DistanceKM,
                                       DateTime?                        LastCall,
                                       GeoCoordinatesResponseFormats?   GeoCoordinatesResponseFormat,
                                       TimeSpan                         RequestTimeout,
                                       EVSEData                         Result,
                                       TimeSpan                         Duration);

    #endregion

    #region OnPullEVSEStatus    (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a PullEVSEStatus request was received.
    /// </summary>
    public delegate Task

        OnPullEVSEStatusRequestDelegate (DateTime                         LogTimestamp,
                                         DateTime                         RequestTimestamp,
                                         CentralServer                    Sender,
                                         String                           SenderId,
                                         EventTracking_Id                 EventTrackingId,
                                         Provider_Id                      ProviderId,
                                         GeoCoordinate?                   SearchCenter,
                                         Single                           DistanceKM,
                                         EVSEStatusTypes?                 EVSEStatusFilter,
                                         TimeSpan                         RequestTimeout);


    /// <summary>
    /// Send a PullEVSEStatus request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<EVSEStatus>

        OnPullEVSEStatusDelegate(DateTime               Timestamp,
                                 CentralServer          Sender,
                                 PullEVSEStatusRequest  Request);


    /// <summary>
    /// A delegate called whenever a PullEVSEStatus response was sent.
    /// </summary>
    public delegate Task

        OnPullEVSEStatusResponseDelegate(DateTime                         Timestamp,
                                         CentralServer                    Sender,
                                         String                           SenderId,
                                         EventTracking_Id                 EventTrackingId,
                                         Provider_Id                      ProviderId,
                                         GeoCoordinate?                   SearchCenter,
                                         Single                           DistanceKM,
                                         EVSEStatusTypes?                 EVSEStatusFilter,
                                         TimeSpan                         RequestTimeout,
                                         EVSEStatus                       Result,
                                         TimeSpan                         Duration);

    #endregion

    #region OnPullEVSEStatusById(Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a PullEVSEStatusById request was received.
    /// </summary>
    public delegate Task

        OnPullEVSEStatusByIdRequestDelegate (DateTime                LogTimestamp,
                                             DateTime                RequestTimestamp,
                                             CentralServer           Sender,
                                             String                  SenderId,
                                             EventTracking_Id        EventTrackingId,
                                             Provider_Id             ProviderId,
                                             IEnumerable<EVSE_Id>    EVSEIds,
                                             TimeSpan                RequestTimeout);


    /// <summary>
    /// Send a PullEVSEStatusById request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<EVSEStatusById>

        OnPullEVSEStatusByIdDelegate(DateTime                   Timestamp,
                                     CentralServer              Sender,
                                     PullEVSEStatusByIdRequest  Request);


    /// <summary>
    /// A delegate called whenever a PullEVSEStatusById response was sent.
    /// </summary>
    public delegate Task

        OnPullEVSEStatusByIdResponseDelegate(DateTime                Timestamp,
                                             CentralServer           Sender,
                                             String                  SenderId,
                                             EventTracking_Id        EventTrackingId,
                                             Provider_Id             ProviderId,
                                             IEnumerable<EVSE_Id>    EVSEIds,
                                             TimeSpan                RequestTimeout,
                                             EVSEStatusById          Result,
                                             TimeSpan                Duration);

    #endregion


    #region OnPushAuthenticationData(Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a PushAuthenticationData request was received.
    /// </summary>
    public delegate Task

        OnPushAuthenticationDataRequestDelegate (DateTime                                          LogTimestamp,
                                                 DateTime                                          RequestTimestamp,
                                                 CentralServer                                     Sender,
                                                 String                                            SenderId,
                                                 EventTracking_Id                                  EventTrackingId,
                                                 ProviderAuthenticationData                        ProviderAuthenticationData,
                                                 ActionTypes                                       OICPAction,
                                                 TimeSpan                                          RequestTimeout);


    /// <summary>
    /// Send a PushAuthenticationData request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<Acknowledgement<PushAuthenticationDataRequest>>

        OnPushAuthenticationDataDelegate        (DateTime                                          Timestamp,
                                                 CentralServer                                     Sender,
                                                 PushAuthenticationDataRequest                     Request);


    /// <summary>
    /// A delegate called whenever a PushAuthenticationData response was sent.
    /// </summary>
    public delegate Task

        OnPushAuthenticationDataResponseDelegate(DateTime                                          Timestamp,
                                                 CentralServer                                     Sender,
                                                 String                                            SenderId,
                                                 EventTracking_Id                                  EventTrackingId,
                                                 ProviderAuthenticationData                        ProviderAuthenticationData,
                                                 ActionTypes                                       OICPAction,
                                                 TimeSpan                                          RequestTimeout,
                                                 Acknowledgement<PushAuthenticationDataRequest>    Result,
                                                 TimeSpan                                          Duration);

    #endregion


    #region OnAuthorizeRemoteReservationStart(Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a AuthorizeRemoteReservationStart request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteReservationStartRequestDelegate (DateTime                                                   LogTimestamp,
                                                          DateTime                                                   RequestTimestamp,
                                                          CentralServer                                              Sender,
                                                          String                                                     SenderId,
                                                          EventTracking_Id                                           EventTrackingId,
                                                          Provider_Id                                                ProviderId,
                                                          EVSE_Id                                                    EVSEId,
                                                          EVCO_Id                                                    EVCOId,
                                                          Session_Id?                                                SessionId,
                                                          PartnerSession_Id?                                         PartnerSessionId,
                                                          PartnerProduct_Id?                                         PartnerProductId,
                                                          TimeSpan                                                   RequestTimeout);


    /// <summary>
    /// Send a AuthorizeRemoteReservationStart request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<Acknowledgement<AuthorizeRemoteReservationStartRequest>>

        OnAuthorizeRemoteReservationStartDelegate        (DateTime                                                   Timestamp,
                                                          CentralServer                                              Sender,
                                                          AuthorizeRemoteReservationStartRequest                     Request);


    /// <summary>
    /// A delegate called whenever a AuthorizeRemoteReservationStart response was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteReservationStartResponseDelegate(DateTime                                                   Timestamp,
                                                          CentralServer                                              Sender,
                                                          String                                                     SenderId,
                                                          EventTracking_Id                                           EventTrackingId,
                                                          Provider_Id                                                ProviderId,
                                                          EVSE_Id                                                    EVSEId,
                                                          EVCO_Id                                                    EVCOId,
                                                          Session_Id?                                                SessionId,
                                                          PartnerSession_Id?                                         PartnerSessionId,
                                                          PartnerProduct_Id?                                         PartnerProductId,
                                                          TimeSpan                                                   RequestTimeout,
                                                          Acknowledgement<AuthorizeRemoteReservationStartRequest>    Result,
                                                          TimeSpan                                                   Duration);

    #endregion

    #region OnAuthorizeRemoteReservationStop (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a AuthorizeRemoteReservationStop request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteReservationStopRequestDelegate (DateTime                                                  LogTimestamp,
                                                         DateTime                                                  RequestTimestamp,
                                                         CentralServer                                             Sender,
                                                         String                                                    SenderId,
                                                         EventTracking_Id                                          EventTrackingId,
                                                         Session_Id                                                SessionId,
                                                         Provider_Id                                               ProviderId,
                                                         EVSE_Id                                                   EVSEId,
                                                         PartnerSession_Id?                                        PartnerSessionId,
                                                         TimeSpan                                                  RequestTimeout);


    /// <summary>
    /// Send a AuthorizeRemoteReservationStop request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<Acknowledgement<AuthorizeRemoteReservationStopRequest>>

        OnAuthorizeRemoteReservationStopDelegate        (DateTime                                                  Timestamp,
                                                         CentralServer                                             Sender,
                                                         AuthorizeRemoteReservationStopRequest                     Request);


    /// <summary>
    /// A delegate called whenever a AuthorizeRemoteReservationStop response was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteReservationStopResponseDelegate(DateTime                                                  Timestamp,
                                                         CentralServer                                             Sender,
                                                         String                                                    SenderId,
                                                         EventTracking_Id                                          EventTrackingId,
                                                         Session_Id                                                SessionId,
                                                         Provider_Id                                               ProviderId,
                                                         EVSE_Id                                                   EVSEId,
                                                         PartnerSession_Id?                                        PartnerSessionId,
                                                         TimeSpan                                                  RequestTimeout,
                                                         Acknowledgement<AuthorizeRemoteReservationStopRequest>    Result,
                                                         TimeSpan                                                  Duration);

    #endregion

    #region OnAuthorizeRemoteStart           (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a AuthorizeRemoteStart request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteStartRequestDelegate (DateTime                                        LogTimestamp,
                                               DateTime                                        RequestTimestamp,
                                               CentralServer                                   Sender,
                                               String                                          SenderId,
                                               EventTracking_Id                                EventTrackingId,
                                               Provider_Id                                     ProviderId,
                                               EVSE_Id                                         EVSEId,
                                               EVCO_Id                                         EVCOId,
                                               Session_Id?                                     SessionId,
                                               PartnerSession_Id?                              PartnerSessionId,
                                               PartnerProduct_Id?                              PartnerProductId,
                                               TimeSpan                                        RequestTimeout);


    /// <summary>
    /// Send a AuthorizeRemoteStart request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<Acknowledgement<AuthorizeRemoteStartRequest>>

        OnAuthorizeRemoteStartDelegate        (DateTime                                        Timestamp,
                                               CentralServer                                   Sender,
                                               AuthorizeRemoteStartRequest                     Request);


    /// <summary>
    /// A delegate called whenever a AuthorizeRemoteStart response was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteStartResponseDelegate(DateTime                                        Timestamp,
                                               CentralServer                                   Sender,
                                               String                                          SenderId,
                                               EventTracking_Id                                EventTrackingId,
                                               Provider_Id                                     ProviderId,
                                               EVSE_Id                                         EVSEId,
                                               EVCO_Id                                         EVCOId,
                                               Session_Id?                                     SessionId,
                                               PartnerSession_Id?                              PartnerSessionId,
                                               PartnerProduct_Id?                              PartnerProductId,
                                               TimeSpan                                        RequestTimeout,
                                               Acknowledgement<AuthorizeRemoteStartRequest>    Result,
                                               TimeSpan                                        Duration);

    #endregion

    #region OnAuthorizeRemoteStop            (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a AuthorizeRemoteStop request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteStopRequestDelegate (DateTime                                       LogTimestamp,
                                              DateTime                                       RequestTimestamp,
                                              CentralServer                                  Sender,
                                              String                                         SenderId,
                                              EventTracking_Id                               EventTrackingId,
                                              Session_Id                                     SessionId,
                                              Provider_Id                                    ProviderId,
                                              EVSE_Id                                        EVSEId,
                                              PartnerSession_Id?                             PartnerSessionId,
                                              TimeSpan                                       RequestTimeout);


    /// <summary>
    /// Send a AuthorizeRemoteStop request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<Acknowledgement<AuthorizeRemoteStopRequest>>

        OnAuthorizeRemoteStopDelegate        (DateTime                                       Timestamp,
                                              CentralServer                                  Sender,
                                              AuthorizeRemoteStopRequest                     Request);


    /// <summary>
    /// A delegate called whenever a AuthorizeRemoteStop response was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeRemoteStopResponseDelegate(DateTime                                       Timestamp,
                                              CentralServer                                  Sender,
                                              String                                         SenderId,
                                              EventTracking_Id                               EventTrackingId,
                                              Session_Id                                     SessionId,
                                              Provider_Id                                    ProviderId,
                                              EVSE_Id                                        EVSEId,
                                              PartnerSession_Id?                             PartnerSessionId,
                                              TimeSpan                                       RequestTimeout,
                                              Acknowledgement<AuthorizeRemoteStopRequest>    Result,
                                              TimeSpan                                       Duration);

    #endregion


    #region OnGetChargeDetailRecords(Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a GetChargeDetailRecords request was received.
    /// </summary>
    public delegate Task

        OnGetChargeDetailRecordsRequestDelegate (DateTime                          LogTimestamp,
                                                 DateTime                          RequestTimestamp,
                                                 CentralServer                     Sender,
                                                 String                            SenderId,
                                                 EventTracking_Id                  EventTrackingId,
                                                 Provider_Id                       ProviderId,
                                                 DateTime                          From,
                                                 DateTime                          To,
                                                 TimeSpan                          RequestTimeout);


    /// <summary>
    /// Send a GetChargeDetailRecords request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<GetChargeDetailRecordsResponse>

        OnGetChargeDetailRecordsDelegate        (DateTime                          Timestamp,
                                                 CentralServer                     Sender,
                                                 GetChargeDetailRecordsRequest     Request);


    /// <summary>
    /// A delegate called whenever a GetChargeDetailRecords response was sent.
    /// </summary>
    public delegate Task

        OnGetChargeDetailRecordsResponseDelegate(DateTime                          Timestamp,
                                                 CentralServer                     Sender,
                                                 String                            SenderId,
                                                 EventTracking_Id                  EventTrackingId,
                                                 Provider_Id                       ProviderId,
                                                 DateTime                          From,
                                                 DateTime                          To,
                                                 TimeSpan                          RequestTimeout,
                                                 GetChargeDetailRecordsResponse    Result,
                                                 TimeSpan                          Duration);

    #endregion


    // CPO event delegates...

    #region OnPushEVSEData      (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a PushEVSEData request was received.
    /// </summary>
    public delegate Task

        OnPushEVSEDataRequestDelegate (DateTime                                LogTimestamp,
                                       DateTime                                RequestTimestamp,
                                       CentralServer                           Sender,
                                       String                                  SenderId,
                                       EventTracking_Id                        EventTrackingId,
                                       OperatorEVSEData                        OperatorEVSEData,
                                       ActionTypes                             Action,
                                       TimeSpan                                RequestTimeout);


    /// <summary>
    /// Send a PushEVSEData request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<Acknowledgement<PushEVSEDataRequest>>

        OnPushEVSEDataDelegate        (DateTime                                Timestamp,
                                       CentralServer                           Sender,
                                       PushEVSEDataRequest                     Request);


    /// <summary>
    /// A delegate called whenever a PushEVSEData response was sent.
    /// </summary>
    public delegate Task

        OnPushEVSEDataResponseDelegate(DateTime                                Timestamp,
                                       CentralServer                           Sender,
                                       String                                  SenderId,
                                       EventTracking_Id                        EventTrackingId,
                                       OperatorEVSEData                        OperatorEVSEData,
                                       ActionTypes                             Action,
                                       TimeSpan                                RequestTimeout,
                                       Acknowledgement<PushEVSEDataRequest>    Result,
                                       TimeSpan                                Duration);

    #endregion

    #region OnPushEVSEStatus    (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a PushEVSEStatus request was received.
    /// </summary>
    public delegate Task

        OnPushEVSEStatusRequestDelegate (DateTime                                  LogTimestamp,
                                         DateTime                                  RequestTimestamp,
                                         CentralServer                             Sender,
                                         String                                    SenderId,
                                         EventTracking_Id                          EventTrackingId,
                                         OperatorEVSEStatus                        OperatorEVSEStatus,
                                         ActionTypes                               Action,
                                         TimeSpan                                  RequestTimeout);


    /// <summary>
    /// Send a PushEVSEStatus request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<Acknowledgement<PushEVSEStatusRequest>>

        OnPushEVSEStatusDelegate        (DateTime                                  Timestamp,
                                         CentralServer                             Sender,
                                         PushEVSEStatusRequest                     Request);


    /// <summary>
    /// A delegate called whenever a PushEVSEStatus response was sent.
    /// </summary>
    public delegate Task

        OnPushEVSEStatusResponseDelegate(DateTime                                  Timestamp,
                                         CentralServer                             Sender,
                                         String                                    SenderId,
                                         EventTracking_Id                          EventTrackingId,
                                         OperatorEVSEStatus                        OperatorEVSEStatus,
                                         ActionTypes                               Action,
                                         TimeSpan                                  RequestTimeout,
                                         Acknowledgement<PushEVSEStatusRequest>    Result,
                                         TimeSpan                                  Duration);

    #endregion


    #region OnAuthorizeStart        (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a AuthorizeStart request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeStartRequestDelegate (DateTime                LogTimestamp,
                                         DateTime                RequestTimestamp,
                                         CentralServer           Sender,
                                         String                  SenderId,
                                         EventTracking_Id        EventTrackingId,
                                         Operator_Id             OperatorId,
                                         UID                     UID,
                                         EVSE_Id?                EVSEId,
                                         PartnerProduct_Id?      PartnerProductId,
                                         Session_Id?             SessionId,
                                         PartnerSession_Id?      PartnerSessionId,
                                         TimeSpan                RequestTimeout);


    /// <summary>
    /// Send a AuthorizeStart request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<AuthorizationStart>

        OnAuthorizeStartDelegate        (DateTime                Timestamp,
                                         CentralServer           Sender,
                                         AuthorizeStartRequest   Request);


    /// <summary>
    /// A delegate called whenever a AuthorizeStart response was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeStartResponseDelegate(DateTime                Timestamp,
                                         CentralServer           Sender,
                                         String                  SenderId,
                                         EventTracking_Id        EventTrackingId,
                                         Operator_Id             OperatorId,
                                         UID                     UID,
                                         EVSE_Id?                EVSEId,
                                         PartnerProduct_Id?      PartnerProductId,
                                         Session_Id?             SessionId,
                                         PartnerSession_Id?      PartnerSessionId,
                                         TimeSpan                RequestTimeout,
                                         AuthorizationStart      Result,
                                         TimeSpan                Duration);

    #endregion

    #region OnAuthorizeStop         (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a AuthorizeStop request was received.
    /// </summary>
    public delegate Task

        OnAuthorizeStopRequestDelegate (DateTime                LogTimestamp,
                                        DateTime                RequestTimestamp,
                                        CentralServer           Sender,
                                        String                  SenderId,
                                        EventTracking_Id        EventTrackingId,
                                        Operator_Id             OperatorId,
                                        Session_Id              SessionId,
                                        UID                     UID,
                                        EVSE_Id?                EVSEId,
                                        PartnerSession_Id?      PartnerSessionId,
                                        TimeSpan                RequestTimeout);


    /// <summary>
    /// Send a AuthorizeStop request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<AuthorizationStop>

        OnAuthorizeStopDelegate        (DateTime                Timestamp,
                                        CentralServer           Sender,
                                        AuthorizeStopRequest    Request);


    /// <summary>
    /// A delegate called whenever a AuthorizeStop response was sent.
    /// </summary>
    public delegate Task

        OnAuthorizeStopResponseDelegate(DateTime                Timestamp,
                                        CentralServer           Sender,
                                        String                  SenderId,
                                        EventTracking_Id        EventTrackingId,
                                        Operator_Id             OperatorId,
                                        Session_Id              SessionId,
                                        UID                     UID,
                                        EVSE_Id?                EVSEId,
                                        PartnerSession_Id?      PartnerSessionId,
                                        TimeSpan                RequestTimeout,
                                        AuthorizationStop       Result,
                                        TimeSpan                Duration);

    #endregion

    #region OnSendChargeDetailRecord(Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a SendChargeDetailRecord request was received.
    /// </summary>
    public delegate Task

        OnSendChargeDetailRecordRequestDelegate (DateTime                                          LogTimestamp,
                                                 DateTime                                          RequestTimestamp,
                                                 CentralServer                                     Sender,
                                                 String                                            SenderId,
                                                 EventTracking_Id                                  EventTrackingId,
                                                 ChargeDetailRecord                                ChargeDetailRecord,
                                                 TimeSpan                                          RequestTimeout);


    /// <summary>
    /// Send a SendChargeDetailRecord request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<Acknowledgement<SendChargeDetailRecordRequest>>

        OnSendChargeDetailRecordDelegate        (DateTime                                          Timestamp,
                                                 CentralServer                                     Sender,
                                                 SendChargeDetailRecordRequest                     Request);


    /// <summary>
    /// A delegate called whenever a SendChargeDetailRecord response was sent.
    /// </summary>
    public delegate Task

        OnSendChargeDetailRecordResponseDelegate(DateTime                                          Timestamp,
                                                 CentralServer                                     Sender,
                                                 String                                            SenderId,
                                                 EventTracking_Id                                  EventTrackingId,
                                                 ChargeDetailRecord                                ChargeDetailRecord,
                                                 TimeSpan                                          RequestTimeout,
                                                 Acknowledgement<SendChargeDetailRecordRequest>    Result,
                                                 TimeSpan                                          Duration);

    #endregion


    #region OnPullAuthenticationData(Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a PullAuthenticationData request was received.
    /// </summary>
    public delegate Task

        OnPullAuthenticationDataRequestDelegate (DateTime                         LogTimestamp,
                                                 DateTime                         RequestTimestamp,
                                                 CentralServer                    Sender,
                                                 String                           SenderId,
                                                 EventTracking_Id                 EventTrackingId,
                                                 Operator_Id                      OperatorId,
                                                 TimeSpan                         RequestTimeout);


    /// <summary>
    /// Send a PullAuthenticationData request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<AuthenticationData>

        OnPullAuthenticationDataDelegate        (DateTime                         Timestamp,
                                                 CentralServer                    Sender,
                                                 PullAuthenticationDataRequest    Request);


    /// <summary>
    /// A delegate called whenever a PullAuthenticationData response was sent.
    /// </summary>
    public delegate Task

        OnPullAuthenticationDataResponseDelegate(DateTime                         Timestamp,
                                                 CentralServer                    Sender,
                                                 String                           SenderId,
                                                 EventTracking_Id                 EventTrackingId,
                                                 Operator_Id                      OperatorId,
                                                 TimeSpan                         RequestTimeout,
                                                 AuthenticationData               Result,
                                                 TimeSpan                         Duration);

    #endregion


    // Mobile event delegates...

    #region OnMobileAuthorizeStart      (Request|Response)Delegate

    /// <summary>
    /// A delegate called whenever a MobileAuthorizeStart request was received.
    /// </summary>
    public delegate Task

        OnMobileAuthorizeStartRequestDelegate (DateTime                              LogTimestamp,
                                               DateTime                              RequestTimestamp,
                                               CentralServer                         Sender,
                                               String                                SenderId,
                                               EventTracking_Id                      EventTrackingId,
                                               EVSE_Id                               EVSEId,
                                               QRCodeIdentification                  QRCodeIdentification,
                                               PartnerProduct_Id?                    PartnerProductId,
                                               Boolean?                              GetNewSession,
                                               TimeSpan                              RequestTimeout);


    /// <summary>
    /// Send a MobileAuthorizeStart request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task<Mobile.MobileAuthorizationStart>

        OnMobileAuthorizeStartDelegate        (DateTime                              Timestamp,
                                               CentralServer                         Sender,
                                               Mobile.MobileAuthorizeStartRequest    Request);


    /// <summary>
    /// A delegate called whenever a MobileAuthorizeStart response was sent.
    /// </summary>
    public delegate Task

        OnMobileAuthorizeStartResponseDelegate(DateTime                              Timestamp,
                                               CentralServer                         Sender,
                                               String                                SenderId,
                                               EventTracking_Id                      EventTrackingId,
                                               EVSE_Id                               EVSEId,
                                               QRCodeIdentification                  QRCodeIdentification,
                                               PartnerProduct_Id?                    PartnerProductId,
                                               Boolean?                              GetNewSession,
                                               TimeSpan                              RequestTimeout,
                                               Mobile.MobileAuthorizationStart       Result,
                                               TimeSpan                              Duration);

    #endregion


}
