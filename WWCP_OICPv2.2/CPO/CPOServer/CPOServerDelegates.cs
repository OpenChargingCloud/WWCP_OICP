/*
 * Copyright (c) 2014-2019 GraphDefined GmbH
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2.CPO
{

    #region OnAuthorizeRemoteReservationStartDelegate

    /// <summary>
    /// A delegate called whenever an 'authorize remote reservation start' request was received.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the logging request.</param>
    /// <param name="RequestTimestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="SenderId">The unique identification of the sender.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="PartnerProductId">The unique identification of the choosen charging product at the given EVSE.</param>
    /// <param name="SessionId">The unique identification of this charging session.</param>
    /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
    /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
    /// <param name="Identification">The unique identification of the e-mobility account.</param>
    /// <param name="RequestTimeout">The timeout of this request.</param>
    public delegate Task

        OnAuthorizeRemoteReservationStartRequestDelegate(DateTime                LogTimestamp,
                                                         DateTime                RequestTimestamp,
                                                         CPOServer               Sender,
                                                         String                  SenderId,
                                                         EventTracking_Id        EventTrackingId,
                                                         EVSE_Id                 EVSEId,
                                                         PartnerProduct_Id?      PartnerProductId,
                                                         Session_Id?             SessionId,
                                                         CPOPartnerSession_Id?   CPOPartnerSessionId,
                                                         EMPPartnerSession_Id?   EMPPartnerSessionId,
                                                         Provider_Id?            ProviderId,
                                                         Identification          Identification,
                                                         TimeSpan?               RequestTimeout);


    /// <summary>
    /// Initiate a remote reservation start at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">An 'authorize remote reservation start' request.</param>
    public delegate Task<Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>>

        OnAuthorizeRemoteReservationStartDelegate(DateTime                                    Timestamp,
                                                  CPOServer                                   Sender,
                                                  EMP.AuthorizeRemoteReservationStartRequest  Request);


    /// <summary>
    /// A delegate called whenever a response to an 'authorize remote reservation start' request was sent.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="SenderId">The unique identification of the sender.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="PartnerProductId">The unique identification of the choosen charging product at the given EVSE.</param>
    /// <param name="SessionId">The unique identification of this charging session.</param>
    /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
    /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
    /// <param name="Identification">The unique identification of the e-mobility account.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    /// <param name="Result">The result of the request.</param>
    /// <param name="Duration">The time between request and response.</param>
    public delegate Task

        OnAuthorizeRemoteReservationStartResponseDelegate(DateTime                                                       Timestamp,
                                                          CPOServer                                                      Sender,
                                                          String                                                         SenderId,
                                                          EventTracking_Id                                               EventTrackingId,
                                                          EVSE_Id                                                        EVSEId,
                                                          PartnerProduct_Id?                                             PartnerProductId,
                                                          Session_Id?                                                    SessionId,
                                                          CPOPartnerSession_Id?                                          CPOPartnerSessionId,
                                                          EMPPartnerSession_Id?                                          EMPPartnerSessionId,
                                                          Provider_Id?                                                   ProviderId,
                                                          Identification                                                 Identification,
                                                          TimeSpan                                                       RequestTimeout,
                                                          Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>    Result,
                                                          TimeSpan                                                       Duration);

    #endregion

    #region OnAuthorizeRemoteReservationStopDelegate

    /// <summary>
    /// A delegate called whenever an 'authorize remote reservation stop' request was received.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the logging request.</param>
    /// <param name="RequestTimestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="SenderId">The unique identification of the sender.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="SessionId">The unique identification of this charging session.</param>
    /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
    /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
    /// <param name="RequestTimeout">The timeout of this request.</param>
    public delegate Task

        OnAuthorizeRemoteReservationStopRequestDelegate(DateTime                LogTimestamp,
                                                        DateTime                RequestTimestamp,
                                                        CPOServer               Sender,
                                                        String                  SenderId,
                                                        EventTracking_Id        EventTrackingId,
                                                        EVSE_Id                 EVSEId,
                                                        Session_Id?             SessionId,
                                                        CPOPartnerSession_Id?   CPOPartnerSessionId,
                                                        EMPPartnerSession_Id?   EMPPartnerSessionId,
                                                        Provider_Id?            ProviderId,
                                                        TimeSpan?               RequestTimeout);


    /// <summary>
    /// Initiate a remote reservation stop of the given charging session at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">An 'authorize remote reservation stop' request.</param>
    public delegate Task<Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>>

        OnAuthorizeRemoteReservationStopDelegate(DateTime                                   Timestamp,
                                                 CPOServer                                  Sender,
                                                 EMP.AuthorizeRemoteReservationStopRequest  Request);


    /// <summary>
    /// A delegate called whenever a response to an 'authorize remote reservation stop' request was sent.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="SenderId">The unique identification of the sender.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="SessionId">The unique identification of this charging session.</param>
    /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
    /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    /// <param name="Result">The result of the request.</param>
    /// <param name="Duration">The time between request and response.</param>
    public delegate Task

        OnAuthorizeRemoteReservationStopResponseDelegate(DateTime                                                      Timestamp,
                                                         CPOServer                                                     Sender,
                                                         String                                                        SenderId,
                                                         EventTracking_Id                                              EventTrackingId,
                                                         EVSE_Id                                                       EVSEId,
                                                         Session_Id?                                                   SessionId,
                                                         CPOPartnerSession_Id?                                         CPOPartnerSessionId,
                                                         EMPPartnerSession_Id?                                         EMPPartnerSessionId,
                                                         Provider_Id?                                                  ProviderId,
                                                         TimeSpan                                                      RequestTimeout,
                                                         Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>    Result,
                                                         TimeSpan                                                      Duration);

    #endregion


    #region OnAuthorizeRemoteStartDelegate

    /// <summary>
    /// A delegate called whenever an 'authorize remote start' request was received.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the logging request.</param>
    /// <param name="RequestTimestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="SenderId">The unique identification of the sender.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="PartnerProductId">The unique identification of the choosen charging product at the given EVSE.</param>
    /// <param name="SessionId">The unique identification of this charging session.</param>
    /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
    /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
    /// <param name="EVCOId">The unique identification of the e-mobility account.</param>
    /// <param name="RequestTimeout">The timeout of this request.</param>
    public delegate Task

        OnAuthorizeRemoteStartRequestDelegate(DateTime                LogTimestamp,
                                              DateTime                RequestTimestamp,
                                              CPOServer               Sender,
                                              String                  SenderId,
                                              EventTracking_Id        EventTrackingId,
                                              EVSE_Id                 EVSEId,
                                              PartnerProduct_Id?      PartnerProductId,
                                              Session_Id?             SessionId,
                                              CPOPartnerSession_Id?   CPOPartnerSessionId,
                                              EMPPartnerSession_Id?   EMPPartnerSessionId,
                                              Provider_Id?            ProviderId,
                                              EVCO_Id?                EVCOId,
                                              TimeSpan?               RequestTimeout);


    /// <summary>
    /// Initiate a remote start at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">An 'authorize remote reservation start' request.</param>
    public delegate Task<Acknowledgement<EMP.AuthorizeRemoteStartRequest>>

        OnAuthorizeRemoteStartDelegate(DateTime                         Timestamp,
                                       CPOServer                        Sender,
                                       EMP.AuthorizeRemoteStartRequest  Request);


    /// <summary>
    /// A delegate called whenever a response to an 'authorize remote start' request was sent.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="SenderId">The unique identification of the sender.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="PartnerProductId">The unique identification of the choosen charging product at the given EVSE.</param>
    /// <param name="SessionId">The unique identification of this charging session.</param>
    /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
    /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
    /// <param name="EVCOId">The unique identification of the e-mobility account.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    /// <param name="Result">The result of the request.</param>
    /// <param name="Duration">The time between request and response.</param>
    public delegate Task

        OnAuthorizeRemoteStartResponseDelegate(DateTime                                            Timestamp,
                                               CPOServer                                           Sender,
                                               String                                              SenderId,
                                               EventTracking_Id                                    EventTrackingId,
                                               EVSE_Id                                             EVSEId,
                                               PartnerProduct_Id?                                  PartnerProductId,
                                               Session_Id?                                         SessionId,
                                               CPOPartnerSession_Id?                               CPOPartnerSessionId,
                                               EMPPartnerSession_Id?                               EMPPartnerSessionId,
                                               Provider_Id?                                        ProviderId,
                                               EVCO_Id?                                            EVCOId,
                                               TimeSpan                                            RequestTimeout,
                                               Acknowledgement<EMP.AuthorizeRemoteStartRequest>    Result,
                                               TimeSpan                                            Duration);

    #endregion

    #region OnAuthorizeRemoteStopDelegate

    /// <summary>
    /// A delegate called whenever an 'authorize remote stop' request was received.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the logging request.</param>
    /// <param name="RequestTimestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="SenderId">The unique identification of the sender.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="SessionId">The unique identification of this charging session.</param>
    /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
    /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
    /// <param name="RequestTimeout">The timeout of this request.</param>
    public delegate Task

        OnAuthorizeRemoteStopRequestDelegate(DateTime                LogTimestamp,
                                             DateTime                RequestTimestamp,
                                             CPOServer               Sender,
                                             String                  SenderId,
                                             EventTracking_Id        EventTrackingId,
                                             EVSE_Id                 EVSEId,
                                             Session_Id?             SessionId,
                                             CPOPartnerSession_Id?   CPOPartnerSessionId,
                                             EMPPartnerSession_Id?   EMPPartnerSessionId,
                                             Provider_Id?            ProviderId,
                                             TimeSpan?               RequestTimeout);


    /// <summary>
    /// Initiate a remote stop of the given charging session at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">An 'authorize remote reservation stop' request.</param>
    public delegate Task<Acknowledgement<EMP.AuthorizeRemoteStopRequest>>

        OnAuthorizeRemoteStopDelegate(DateTime                        Timestamp,
                                      CPOServer                       Sender,
                                      EMP.AuthorizeRemoteStopRequest  Request);


    /// <summary>
    /// A delegate called whenever a response to an 'authorize remote stop' request was sent.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="SenderId">The unique identification of the sender.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="EVSEId">The unique identification of an EVSE.</param>
    /// <param name="SessionId">The unique identification of this charging session.</param>
    /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
    /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    /// <param name="Result">The result of the request.</param>
    /// <param name="Duration">The time between request and response.</param>
    public delegate Task

        OnAuthorizeRemoteStopResponseDelegate(DateTime                                           Timestamp,
                                              CPOServer                                          Sender,
                                              String                                             SenderId,
                                              EventTracking_Id                                   EventTrackingId,
                                              EVSE_Id                                            EVSEId,
                                              Session_Id?                                        SessionId,
                                              CPOPartnerSession_Id?                              CPOPartnerSessionId,
                                              EMPPartnerSession_Id?                              EMPPartnerSessionId,
                                              Provider_Id?                                       ProviderId,
                                              TimeSpan                                           RequestTimeout,
                                              Acknowledgement<EMP.AuthorizeRemoteStopRequest>    Result,
                                              TimeSpan                                           Duration);

    #endregion

}
