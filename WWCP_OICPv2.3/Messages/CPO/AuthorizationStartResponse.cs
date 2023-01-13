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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// The AuthorizationStart response.
    /// </summary>
    public class AuthorizationStartResponse : AResponse<AuthorizeStartRequest,
                                                        AuthorizationStartResponse>
    {

        #region Properties

        /// <summary>
        /// The authorization status, e.g. "Authorized".
        /// </summary>
        [Mandatory]
        public AuthorizationStatusTypes      AuthorizationStatus                 { get; }

        /// <summary>
        /// The authorization status code.
        /// </summary>
        [Mandatory]
        public StatusCode                    StatusCode                          { get; }

        /// <summary>
        /// The e-mobility provider identification.
        /// </summary>
        [Optional]
        public Provider_Id?                  ProviderId                          { get; }

        /// <summary>
        /// The charging session identification.
        /// </summary>
        [Optional]
        public Session_Id?                   SessionId                           { get; }

        /// <summary>
        /// An optional partner charging session identification.
        /// </summary>
        [Optional]
        public CPOPartnerSession_Id?         CPOPartnerSessionId                 { get; }

        /// <summary>
        /// An optional partner charging session identification.
        /// </summary>
        [Optional]
        public EMPPartnerSession_Id?         EMPPartnerSessionId                 { get; }

        /// <summary>
        /// An enumeration of authorization identifications.
        /// </summary>
        [Optional]
        public IEnumerable<Identification>?  AuthorizationStopIdentifications    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new AuthorizationStart response.
        /// </summary>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="ProcessId">The server side process identification of the request.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="AuthorizationStatus">The authorization status.</param>
        /// <param name="StatusCode">A status code.</param>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        /// <param name="AuthorizationStopIdentifications">Optional authorization stop identifications.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        private AuthorizationStartResponse(DateTime                      ResponseTimestamp,
                                           EventTracking_Id              EventTrackingId,
                                           Process_Id                    ProcessId,
                                           TimeSpan                      Runtime,
                                           AuthorizationStatusTypes      AuthorizationStatus,
                                           StatusCode                    StatusCode,

                                           AuthorizeStartRequest?        Request                            = null,
                                           Session_Id?                   SessionId                          = null,
                                           CPOPartnerSession_Id?         CPOPartnerSessionId                = null,
                                           EMPPartnerSession_Id?         EMPPartnerSessionId                = null,
                                           Provider_Id?                  ProviderId                         = null,
                                           IEnumerable<Identification>?  AuthorizationStopIdentifications   = null,
                                           HTTPResponse?                 HTTPResponse                       = null,
                                           JObject?                      CustomData                         = null)

            : base(ResponseTimestamp,
                   EventTrackingId,
                   ProcessId,
                   Runtime,
                   Request,
                   HTTPResponse,
                   CustomData)

        {

            this.AuthorizationStatus               = AuthorizationStatus;
            this.StatusCode                        = StatusCode;
            this.SessionId                         = SessionId;
            this.CPOPartnerSessionId               = CPOPartnerSessionId;
            this.EMPPartnerSessionId               = EMPPartnerSessionId;
            this.ProviderId                        = ProviderId;
            this.AuthorizationStopIdentifications  = AuthorizationStopIdentifications?.Distinct() ?? Array.Empty<Identification>();

        }

        #endregion


        #region (static) Authorized               (Request, ...)

        /// <summary>
        /// Create a new 'Authorized' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="SessionId">The charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="AuthorizationStopIdentifications">Optional authorization stop identifications.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public static AuthorizationStartResponse Authorized(AuthorizeStartRequest         Request,
                                                            Session_Id?                   SessionId                          = null,
                                                            CPOPartnerSession_Id?         CPOPartnerSessionId                = null,
                                                            EMPPartnerSession_Id?         EMPPartnerSessionId                = null,
                                                            Provider_Id?                  ProviderId                         = null,
                                                            String?                       StatusCodeDescription              = null,
                                                            String?                       StatusCodeAdditionalInfo           = null,
                                                            IEnumerable<Identification>?  AuthorizationStopIdentifications   = null,
                                                            DateTime?                     ResponseTimestamp                  = null,
                                                            EventTracking_Id?             EventTrackingId                    = null,
                                                            TimeSpan?                     Runtime                            = null,
                                                            Process_Id?                   ProcessId                          = null,
                                                            HTTPResponse?                 HTTPResponse                       = null,
                                                            JObject?                      CustomData                         = null)


            => new (ResponseTimestamp ?? Timestamp.Now,
                    EventTrackingId   ?? EventTracking_Id.New,
                    ProcessId         ?? Process_Id.NewRandom(),
                    Runtime           ?? (Timestamp.Now - Request.Timestamp),
                    AuthorizationStatusTypes.Authorized,
                    new StatusCode(
                        StatusCodes.Success,
                        StatusCodeDescription,
                        StatusCodeAdditionalInfo
                    ),
                    Request,
                    SessionId,
                    CPOPartnerSessionId,
                    EMPPartnerSessionId,
                    ProviderId,
                    AuthorizationStopIdentifications,
                    HTTPResponse,
                    CustomData);

        #endregion

        #region (static) NotAuthorized            (Request, StatusCode, ...)

        /// <summary>
        /// Create a new 'NotAuthorized' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCode">The status code of the operation.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public static AuthorizationStartResponse NotAuthorized(AuthorizeStartRequest  Request,
                                                               StatusCode             StatusCode,
                                                               Session_Id?            SessionId             = null,
                                                               CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                                                               EMPPartnerSession_Id?  EMPPartnerSessionId   = null,
                                                               Provider_Id?           ProviderId            = null,
                                                               DateTime?              ResponseTimestamp     = null,
                                                               EventTracking_Id?      EventTrackingId       = null,
                                                               TimeSpan?              Runtime               = null,
                                                               Process_Id?            ProcessId             = null,
                                                               HTTPResponse?          HTTPResponse          = null,
                                                               JObject?               CustomData            = null)

            => new (ResponseTimestamp ?? Timestamp.Now,
                    EventTrackingId   ?? EventTracking_Id.New,
                    ProcessId         ?? Process_Id.NewRandom(),
                    Runtime           ?? (Timestamp.Now - Request.Timestamp),
                    AuthorizationStatusTypes.NotAuthorized,
                    StatusCode,
                    Request,
                    SessionId,
                    CPOPartnerSessionId,
                    EMPPartnerSessionId,
                    ProviderId,
                    null,
                    HTTPResponse,
                    CustomData);

        #endregion

        #region (static) SessionIsInvalid         (Request, ...)

        /// <summary>
        /// Create a new 'SessionIsInvalid' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public static AuthorizationStartResponse SessionIsInvalid(AuthorizeStartRequest  Request,
                                                                  String?                StatusCodeDescription      = null,
                                                                  String?                StatusCodeAdditionalInfo   = null,
                                                                  Session_Id?            SessionId                  = null,
                                                                  CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                                  EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                                  Provider_Id?           ProviderId                 = null,
                                                                  DateTime?              ResponseTimestamp          = null,
                                                                  EventTracking_Id?      EventTrackingId            = null,
                                                                  TimeSpan?              Runtime                    = null,
                                                                  Process_Id?            ProcessId                  = null,
                                                                  HTTPResponse?          HTTPResponse               = null,
                                                                  JObject?               CustomData                 = null)

            => new (ResponseTimestamp ?? Timestamp.Now,
                    EventTrackingId   ?? EventTracking_Id.New,
                    ProcessId         ?? Process_Id.NewRandom(),
                    Runtime           ?? (Timestamp.Now - Request.Timestamp),
                    AuthorizationStatusTypes.NotAuthorized,
                    new StatusCode(
                        StatusCodes.SessionIsInvalid,
                        StatusCodeDescription ?? "Session is invalid",
                        StatusCodeAdditionalInfo
                    ),
                    Request,
                    SessionId,
                    CPOPartnerSessionId,
                    EMPPartnerSessionId,
                    ProviderId,
                    null,
                    HTTPResponse,
                    CustomData);

        #endregion

        #region (static) CommunicationToEVSEFailed(Request, ...)

        /// <summary>
        /// Create a new 'CommunicationToEVSEFailed' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public static AuthorizationStartResponse CommunicationToEVSEFailed(AuthorizeStartRequest  Request,
                                                                           String?                StatusCodeDescription      = null,
                                                                           String?                StatusCodeAdditionalInfo   = null,
                                                                           Session_Id?            SessionId                  = null,
                                                                           CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                                           EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                                           Provider_Id?           ProviderId                 = null,
                                                                           DateTime?              ResponseTimestamp          = null,
                                                                           EventTracking_Id?      EventTrackingId            = null,
                                                                           TimeSpan?              Runtime                    = null,
                                                                           Process_Id?            ProcessId                  = null,
                                                                           HTTPResponse?          HTTPResponse               = null,
                                                                           JObject?               CustomData                 = null)

            => new (ResponseTimestamp ?? Timestamp.Now,
                    EventTrackingId   ?? EventTracking_Id.New,
                    ProcessId         ?? Process_Id.NewRandom(),
                    Runtime           ?? (Timestamp.Now - Request.Timestamp),
                    AuthorizationStatusTypes.NotAuthorized,
                    new StatusCode(
                        StatusCodes.CommunicationToEVSEFailed,
                        StatusCodeDescription ?? "Communication to EVSE failed!",
                        StatusCodeAdditionalInfo
                    ),
                    Request,
                    SessionId,
                    CPOPartnerSessionId,
                    EMPPartnerSessionId,
                    ProviderId,
                    null,
                    HTTPResponse,
                    CustomData);

        #endregion

        #region (static) NoEVConnectedToEVSE      (Request, ...)

        /// <summary>
        /// Create a new 'NoEVConnectedToEVSE' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public static AuthorizationStartResponse NoEVConnectedToEVSE(AuthorizeStartRequest  Request,
                                                                     String?                StatusCodeDescription      = null,
                                                                     String?                StatusCodeAdditionalInfo   = null,
                                                                     Session_Id?            SessionId                  = null,
                                                                     CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                                     EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                                     Provider_Id?           ProviderId                 = null,
                                                                     DateTime?              ResponseTimestamp          = null,
                                                                     EventTracking_Id?      EventTrackingId            = null,
                                                                     TimeSpan?              Runtime                    = null,
                                                                     Process_Id?            ProcessId                  = null,
                                                                     HTTPResponse?          HTTPResponse               = null,
                                                                     JObject?               CustomData                 = null)

            => new (ResponseTimestamp ?? Timestamp.Now,
                    EventTrackingId   ?? EventTracking_Id.New,
                    ProcessId         ?? Process_Id.NewRandom(),
                    Runtime           ?? (Timestamp.Now - Request.Timestamp),
                    AuthorizationStatusTypes.NotAuthorized,
                    new StatusCode(
                        StatusCodes.NoEVConnectedToEVSE,
                        StatusCodeDescription ?? "No EV connected to EVSE!",
                        StatusCodeAdditionalInfo
                    ),
                    Request,
                    SessionId,
                    CPOPartnerSessionId,
                    EMPPartnerSessionId,
                    ProviderId,
                    null,
                    HTTPResponse,
                    CustomData);

        #endregion

        #region (static) EVSEAlreadyReserved      (Request, ...)

        /// <summary>
        /// Create a new 'EVSEAlreadyReserved' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public static AuthorizationStartResponse EVSEAlreadyReserved(AuthorizeStartRequest  Request,
                                                                     String?                StatusCodeDescription      = null,
                                                                     String?                StatusCodeAdditionalInfo   = null,
                                                                     Session_Id?            SessionId                  = null,
                                                                     CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                                     EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                                     Provider_Id?           ProviderId                 = null,
                                                                     DateTime?              ResponseTimestamp          = null,
                                                                     EventTracking_Id?      EventTrackingId            = null,
                                                                     TimeSpan?              Runtime                    = null,
                                                                     Process_Id?            ProcessId                  = null,
                                                                     HTTPResponse?          HTTPResponse               = null,
                                                                     JObject?               CustomData                 = null)

            => new (ResponseTimestamp ?? Timestamp.Now,
                    EventTrackingId   ?? EventTracking_Id.New,
                    ProcessId         ?? Process_Id.NewRandom(),
                    Runtime           ?? (Timestamp.Now - Request.Timestamp),
                    AuthorizationStatusTypes.NotAuthorized,
                    new StatusCode(
                        StatusCodes.EVSEAlreadyReserved,
                        StatusCodeDescription ?? "EVSE already reserved!",
                        StatusCodeAdditionalInfo
                    ),
                    Request,
                    SessionId,
                    CPOPartnerSessionId,
                    EMPPartnerSessionId,
                    ProviderId,
                    null,
                    HTTPResponse,
                    CustomData);

        #endregion

        #region (static) UnknownEVSEID            (Request, ...)

        /// <summary>
        /// Create a new 'UnknownEVSEID' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public static AuthorizationStartResponse UnknownEVSEID(AuthorizeStartRequest  Request,
                                                               String?                StatusCodeDescription      = null,
                                                               String?                StatusCodeAdditionalInfo   = null,
                                                               Session_Id?            SessionId                  = null,
                                                               CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                               EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                               Provider_Id?           ProviderId                 = null,
                                                               DateTime?              ResponseTimestamp          = null,
                                                               EventTracking_Id?      EventTrackingId            = null,
                                                               TimeSpan?              Runtime                    = null,
                                                               Process_Id?            ProcessId                  = null,
                                                               HTTPResponse?          HTTPResponse               = null,
                                                               JObject?               CustomData                 = null)

            => new (ResponseTimestamp ?? Timestamp.Now,
                    EventTrackingId   ?? EventTracking_Id.New,
                    ProcessId         ?? Process_Id.NewRandom(),
                    Runtime           ?? (Timestamp.Now - Request.Timestamp),
                    AuthorizationStatusTypes.NotAuthorized,
                    new StatusCode(
                        StatusCodes.UnknownEVSEID,
                        StatusCodeDescription ?? "Unknown EVSE ID!",
                        StatusCodeAdditionalInfo
                    ),
                    Request,
                    SessionId,
                    CPOPartnerSessionId,
                    EMPPartnerSessionId,
                    ProviderId,
                    null,
                    HTTPResponse,
                    CustomData);

        #endregion

        #region (static) EVSEOutOfService         (Request, ...)

        /// <summary>
        /// Create a new 'EVSEOutOfService' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public static AuthorizationStartResponse EVSEOutOfService(AuthorizeStartRequest  Request,
                                                                  String?                StatusCodeDescription      = null,
                                                                  String?                StatusCodeAdditionalInfo   = null,
                                                                  Session_Id?            SessionId                  = null,
                                                                  CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                                  EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                                  Provider_Id?           ProviderId                 = null,
                                                                  DateTime?              ResponseTimestamp          = null,
                                                                  EventTracking_Id?      EventTrackingId            = null,
                                                                  TimeSpan?              Runtime                    = null,
                                                                  Process_Id?            ProcessId                  = null,
                                                                  HTTPResponse?          HTTPResponse               = null,
                                                                  JObject?               CustomData                 = null)

            => new (ResponseTimestamp ?? Timestamp.Now,
                    EventTrackingId   ?? EventTracking_Id.New,
                    ProcessId         ?? Process_Id.NewRandom(),
                    Runtime           ?? (Timestamp.Now - Request.Timestamp),
                    AuthorizationStatusTypes.NotAuthorized,
                    new StatusCode(
                        StatusCodes.EVSEOutOfService,
                        StatusCodeDescription ?? "EVSE out of service!",
                        StatusCodeAdditionalInfo
                    ),
                    Request,
                    SessionId,
                    CPOPartnerSessionId,
                    EMPPartnerSessionId,
                    ProviderId,
                    null,
                    HTTPResponse,
                    CustomData);

        #endregion

        #region (static) ServiceNotAvailable      (Request, ...)

        /// <summary>
        /// Create a new 'ServiceNotAvailable' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public static AuthorizationStartResponse ServiceNotAvailable(AuthorizeStartRequest  Request,
                                                                     String?                StatusCodeDescription      = null,
                                                                     String?                StatusCodeAdditionalInfo   = null,
                                                                     Session_Id?            SessionId                  = null,
                                                                     CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                                     EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                                     Provider_Id?           ProviderId                 = null,
                                                                     DateTime?              ResponseTimestamp          = null,
                                                                     EventTracking_Id?      EventTrackingId            = null,
                                                                     TimeSpan?              Runtime                    = null,
                                                                     Process_Id?            ProcessId                  = null,
                                                                     HTTPResponse?          HTTPResponse               = null,
                                                                     JObject?               CustomData                 = null)

            => new (ResponseTimestamp ?? Timestamp.Now,
                    EventTrackingId   ?? EventTracking_Id.New,
                    ProcessId         ?? Process_Id.NewRandom(),
                    Runtime           ?? (Timestamp.Now - Request.Timestamp),
                    AuthorizationStatusTypes.NotAuthorized,
                    new StatusCode(
                        StatusCodes.ServiceNotAvailable,
                        StatusCodeDescription ?? "Service not available!",
                        StatusCodeAdditionalInfo
                    ),
                    Request,
                    SessionId,
                    CPOPartnerSessionId,
                    EMPPartnerSessionId,
                    ProviderId,
                    null,
                    HTTPResponse,
                    CustomData);

        #endregion

        #region (static) DataError                (Request, ...)

        /// <summary>
        /// Create a new 'DataError' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public static AuthorizationStartResponse DataError(AuthorizeStartRequest?  Request                    = null,
                                                           String?                 StatusCodeDescription      = null,
                                                           String?                 StatusCodeAdditionalInfo   = null,
                                                           Session_Id?             SessionId                  = null,
                                                           CPOPartnerSession_Id?   CPOPartnerSessionId        = null,
                                                           EMPPartnerSession_Id?   EMPPartnerSessionId        = null,
                                                           Provider_Id?            ProviderId                 = null,
                                                           DateTime?               ResponseTimestamp          = null,
                                                           EventTracking_Id?       EventTrackingId            = null,
                                                           TimeSpan?               Runtime                    = null,
                                                           Process_Id?             ProcessId                  = null,
                                                           HTTPResponse?           HTTPResponse               = null,
                                                           JObject?                CustomData                 = null)

            => new (ResponseTimestamp ?? Timestamp.Now,
                    EventTrackingId   ?? EventTracking_Id.New,
                    ProcessId         ?? Process_Id.NewRandom(),
                    Runtime           ?? (Request is not null
                                              ? Timestamp.Now - Request.Timestamp
                                              : TimeSpan.Zero),
                    AuthorizationStatusTypes.NotAuthorized,
                    new StatusCode(
                        StatusCodes.DataError,
                        StatusCodeDescription ?? "Data Error!",
                        StatusCodeAdditionalInfo
                    ),
                    Request,
                    SessionId,
                    CPOPartnerSessionId,
                    EMPPartnerSessionId,
                    ProviderId,
                    null,
                    HTTPResponse,
                    CustomData);

        #endregion

        #region (static) SystemError              (Request, ...)

        /// <summary>
        /// Create a new 'SystemError' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public static AuthorizationStartResponse SystemError(AuthorizeStartRequest?  Request                    = null,
                                                             String?                 StatusCodeDescription      = null,
                                                             String?                 StatusCodeAdditionalInfo   = null,
                                                             Session_Id?             SessionId                  = null,
                                                             CPOPartnerSession_Id?   CPOPartnerSessionId        = null,
                                                             EMPPartnerSession_Id?   EMPPartnerSessionId        = null,
                                                             Provider_Id?            ProviderId                 = null,
                                                             DateTime?               ResponseTimestamp          = null,
                                                             EventTracking_Id?       EventTrackingId            = null,
                                                             TimeSpan?               Runtime                    = null,
                                                             Process_Id?             ProcessId                  = null,
                                                             HTTPResponse?           HTTPResponse               = null,
                                                             JObject?                CustomData                 = null)

            => new (ResponseTimestamp ?? Timestamp.Now,
                    EventTrackingId   ?? EventTracking_Id.New,
                    ProcessId         ?? Process_Id.NewRandom(),
                    Runtime           ?? (Request is not null
                                              ? Timestamp.Now - Request.Timestamp
                                              : TimeSpan.Zero),
                    AuthorizationStatusTypes.NotAuthorized,
                    new StatusCode(
                        StatusCodes.SystemError,
                        StatusCodeDescription ?? "System Error!",
                        StatusCodeAdditionalInfo
                    ),
                    Request,
                    SessionId,
                    CPOPartnerSessionId,
                    EMPPartnerSessionId,
                    ProviderId,
                    null,
                    HTTPResponse,
                    CustomData);

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/02_CPO_Services_and_Operations.asciidoc#eRoamingAuthorizationStart

        // {
        //   "AuthorizationStatus":     "Authorized",
        //   "StatusCode": {
        //     "AdditionalInfo":        "string",
        //     "Code":                  "000",
        //     "Description":           "string"
        //   },
        //   "ProviderID":              "string",
        //   "SessionID":               "string",
        //   "CPOPartnerSessionID":     "string",
        //   "EMPPartnerSessionID":     "string",
        //   "AuthorizationStopIdentifications": [
        //     {
        //       "RFIDMifareFamilyIdentification": {
        //         "UID":               "string"
        //       },
        //       "QRCodeIdentification": {
        //         "EvcoID":            "string",
        //         "HashedPIN": {
        //           "Function":        "Bcrypt",
        //           "LegacyHashData": {
        //             "Function":      "MD5",
        //             "Salt":          "string",
        //             "Value":         "string"
        //           },
        //           "Value":           "string"
        //         },
        //         "PIN":               "string"
        //       },
        //       "PlugAndChargeIdentification": {
        //     "EvcoID":                "string"
        //       },
        //       "RemoteIdentification": {
        //     "EvcoID":                "string"
        //       },
        //       "RFIDIdentification": {
        //     "EvcoID":                "string",
        //         "ExpiryDate":        "2021-01-10T17:17:43.613Z",
        //         "PrintedNumber":     "string",
        //         "RFID":              "mifareCls",
        //         "UID":               "string"
        //       }
        //     }
        //   ]
        // }

        #endregion

        #region (static) Parse   (JSON, CustomAuthorizationStartResponseParser = null)

        /// <summary>
        /// Parse the given JSON representation of a AuthorizationStart response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomAuthorizationStartResponseParser">A delegate to parse custom AuthorizationStart response JSON objects.</param>
        public static AuthorizationStartResponse Parse(AuthorizeStartRequest                                     Request,
                                                       JObject                                                   JSON,
                                                       DateTime?                                                 ResponseTimestamp                        = null,
                                                       EventTracking_Id?                                         EventTrackingId                          = null,
                                                       TimeSpan?                                                 Runtime                                  = null,
                                                       Process_Id?                                               ProcessId                                = null,
                                                       HTTPResponse?                                             HTTPResponse                             = null,
                                                       CustomJObjectParserDelegate<AuthorizationStartResponse>?  CustomAuthorizationStartResponseParser   = null)
        {

            if (TryParse(Request,
                         JSON,
                         out AuthorizationStartResponse?  authorizationStartResponse,
                         out String?                      ErrorResponse,
                         ResponseTimestamp,
                         EventTrackingId,
                         Runtime,
                         ProcessId,
                         HTTPResponse,
                         CustomAuthorizationStartResponseParser))
            {
                return authorizationStartResponse!;
            }

            throw new ArgumentException("The given JSON representation of a AuthorizationStart response is invalid: " + ErrorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, CustomAuthorizationStartResponseParser = null)

        /// <summary>
        /// Parse the given text representation of a AuthorizationStart response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="Text">The text to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomAuthorizationStartResponseParser">A delegate to parse custom AuthorizationStart response JSON objects.</param>
        public static AuthorizationStartResponse Parse(AuthorizeStartRequest                                     Request,
                                                       String                                                    Text,
                                                       DateTime?                                                 ResponseTimestamp                        = null,
                                                       EventTracking_Id?                                         EventTrackingId                          = null,
                                                       TimeSpan?                                                 Runtime                                  = null,
                                                       Process_Id?                                               ProcessId                                = null,
                                                       HTTPResponse?                                             HTTPResponse                             = null,
                                                       CustomJObjectParserDelegate<AuthorizationStartResponse>?  CustomAuthorizationStartResponseParser   = null)
        {

            if (TryParse(Request,
                         Text,
                         out AuthorizationStartResponse?  authorizationStartResponse,
                         out String?                      ErrorResponse,
                         ResponseTimestamp,
                         EventTrackingId,
                         Runtime,
                         ProcessId,
                         HTTPResponse,
                         CustomAuthorizationStartResponseParser))
            {
                return authorizationStartResponse!;
            }

            throw new ArgumentException("The given text representation of a AuthorizationStart response is invalid: " + ErrorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParse(JSON, out AuthorizationStartResponse, out ErrorResponse, CustomAuthorizationStartResponseParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a AuthorizationStart response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="AuthorizationStartResponse">The parsed AuthorizationStart response.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomAuthorizationStartResponseParser">A delegate to parse custom AuthorizationStart response JSON objects.</param>
        public static Boolean TryParse(AuthorizeStartRequest                                     Request,
                                       JObject                                                   JSON,
                                       out AuthorizationStartResponse?                           AuthorizationStartResponse,
                                       out String?                                               ErrorResponse,
                                       DateTime?                                                 ResponseTimestamp                        = null,
                                       EventTracking_Id?                                         EventTrackingId                          = null,
                                       TimeSpan?                                                 Runtime                                  = null,
                                       Process_Id?                                               ProcessId                                = null,
                                       HTTPResponse?                                             HTTPResponse                             = null,
                                       CustomJObjectParserDelegate<AuthorizationStartResponse>?  CustomAuthorizationStartResponseParser   = null)
        {

            try
            {

                AuthorizationStartResponse = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse AuthorizationStatus                   [mandatory]

                if (!JSON.ParseMandatory("AuthorizationStatus",
                                         "authorization status",
                                         AuthorizationStatusTypesExtensions.TryParse,
                                         out AuthorizationStatusTypes AuthorizationStatus,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse StatusCode                            [mandatory]

                if (!JSON.ParseMandatoryJSON("StatusCode",
                                             "status code",
                                             OICPv2_3.StatusCode.TryParse,
                                             out StatusCode? StatusCode,
                                             out ErrorResponse) ||
                     StatusCode is null)
                {
                    return false;
                }

                #endregion

                #region Parse SessionId                             [optional]

                if (JSON.ParseOptional("SessionID",
                                       "session identification",
                                       Session_Id.TryParse,
                                       out Session_Id? SessionId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse CPOPartnerSessionId                   [optional]

                if (JSON.ParseOptional("CPOPartnerSessionID",
                                       "CPO product session identification",
                                       CPOPartnerSession_Id.TryParse,
                                       out CPOPartnerSession_Id? CPOPartnerSessionId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse EMPPartnerSessionId                   [optional]

                if (JSON.ParseOptional("EMPPartnerSessionID",
                                       "EMP product session identification",
                                       EMPPartnerSession_Id.TryParse,
                                       out EMPPartnerSession_Id? EMPPartnerSessionId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse ProviderId                            [optional]

                if (JSON.ParseOptional("ProviderID",
                                       "provider identification",
                                       Provider_Id.TryParse,
                                       out Provider_Id? ProviderId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse AuthorizationStopIdentifications      [optional]

                if (JSON.ParseOptionalJSON("AuthorizationStopIdentifications",
                                           "authorization stop identifications",
                                           Identification.TryParse,
                                           out IEnumerable<Identification> AuthorizationStopIdentifications,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse CustomData                            [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                AuthorizationStartResponse = new AuthorizationStartResponse(ResponseTimestamp ?? Timestamp.Now,
                                                                            EventTrackingId   ?? Request.EventTrackingId,
                                                                            ProcessId         ?? Process_Id.NewRandom(),
                                                                            Runtime           ?? Timestamp.Now - Request.Timestamp,
                                                                            AuthorizationStatus,
                                                                            StatusCode!,
                                                                            Request,
                                                                            SessionId,
                                                                            CPOPartnerSessionId,
                                                                            EMPPartnerSessionId,
                                                                            ProviderId,
                                                                            AuthorizationStopIdentifications,
                                                                            HTTPResponse,
                                                                            customData);

                if (CustomAuthorizationStartResponseParser is not null)
                    AuthorizationStartResponse = CustomAuthorizationStartResponseParser(JSON,
                                                                                        AuthorizationStartResponse);

                return true;

            }
            catch (Exception e)
            {
                AuthorizationStartResponse  = default;
                ErrorResponse               = "The given JSON representation of a AuthorizationStart response is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, out AuthorizationStartResponse, out ErrorResponse, CustomAuthorizationStartResponseParser = null)

        /// <summary>
        /// Try to parse the given text representation of a AuthorizationStart response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="Text">The text to parse.</param>
        /// <param name="AuthorizationStartResponse">The parsed AuthorizationStart response.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomAuthorizationStartResponseParser">A delegate to parse custom AuthorizationStart response JSON objects.</param>
        public static Boolean TryParse(AuthorizeStartRequest                                     Request,
                                       String                                                    Text,
                                       out AuthorizationStartResponse?                           AuthorizationStartResponse,
                                       out String?                                               ErrorResponse,
                                       DateTime?                                                 ResponseTimestamp                        = null,
                                       EventTracking_Id?                                         EventTrackingId                          = null,
                                       TimeSpan?                                                 Runtime                                  = null,
                                       Process_Id?                                               ProcessId                                = null,
                                       HTTPResponse?                                             HTTPResponse                             = null,
                                       CustomJObjectParserDelegate<AuthorizationStartResponse>?  CustomAuthorizationStartResponseParser   = null)
        {

            try
            {

                return TryParse(Request,
                                JObject.Parse(Text),
                                out AuthorizationStartResponse,
                                out ErrorResponse,
                                ResponseTimestamp,
                                EventTrackingId,
                                Runtime,
                                ProcessId,
                                HTTPResponse,
                                CustomAuthorizationStartResponseParser);

            }
            catch (Exception e)
            {
                AuthorizationStartResponse  = default;
                ErrorResponse               = "The given text representation of a AuthorizationStart response is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomAuthorizationStartSerializer = null, CustomStatusCodeSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomAuthorizationStartSerializer">A delegate to customize the serialization of AuthorizationStart respones.</param>
        /// <param name="CustomStatusCodeSerializer">A delegate to serialize custom StatusCode JSON objects.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<AuthorizationStartResponse>?  CustomAuthorizationStartSerializer   = null,
                              CustomJObjectSerializerDelegate<StatusCode>?                  CustomStatusCodeSerializer           = null,
                              CustomJObjectSerializerDelegate<Identification>?              CustomIdentificationSerializer       = null)

        {

            var JSON = JSONObject.Create(

                           new JProperty("AuthorizationStatus",                     AuthorizationStatus.      ToString()),

                           new JProperty("StatusCode",                              StatusCode.               ToJSON(CustomStatusCodeSerializer)),

                           ProviderId.HasValue
                               ? new JProperty("ProviderID",                        ProviderId.         Value.ToString())
                               : null,

                           SessionId.HasValue
                               ? new JProperty("SessionID",                         SessionId.          Value.ToString())
                               : null,

                           CPOPartnerSessionId.HasValue
                               ? new JProperty("CPOPartnerSessionID",               CPOPartnerSessionId.Value.ToString())
                               : null,

                           EMPPartnerSessionId.HasValue
                               ? new JProperty("EMPPartnerSessionID",               EMPPartnerSessionId.Value.ToString())
                               : null,

                           AuthorizationStopIdentifications is not null && AuthorizationStopIdentifications.Any()
                               ? new JProperty("AuthorizationStopIdentifications",  new JArray(AuthorizationStopIdentifications.Select(identification => identification.ToJSON(CustomIdentificationSerializer))))
                               : null,

                           CustomData is not null
                               ? new JProperty("CustomData",                        CustomData)
                               : null

                       );

            return CustomAuthorizationStartSerializer is not null
                       ? CustomAuthorizationStartSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region Operator overloading

        #region Operator == (AuthorizationStartResponse1, AuthorizationStartResponse2)

        /// <summary>
        /// Compares two authorization start responses for equality.
        /// </summary>
        /// <param name="AuthorizationStartResponse1">An authorize start response.</param>
        /// <param name="AuthorizationStartResponse2">Another authorize start response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (AuthorizationStartResponse AuthorizationStartResponse1,
                                           AuthorizationStartResponse AuthorizationStartResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(AuthorizationStartResponse1, AuthorizationStartResponse2))
                return true;

            // If one is null, but not both, return false.
            if (AuthorizationStartResponse1 is null || AuthorizationStartResponse2 is null)
                return false;

            return AuthorizationStartResponse1.Equals(AuthorizationStartResponse2);

        }

        #endregion

        #region Operator != (AuthorizationStartResponse1, AuthorizationStartResponse2)

        /// <summary>
        /// Compares two authorization start responses for inequality.
        /// </summary>
        /// <param name="AuthorizationStartResponse1">An authorize start response.</param>
        /// <param name="AuthorizationStartResponse2">Another authorize start response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (AuthorizationStartResponse AuthorizationStartResponse1,
                                           AuthorizationStartResponse AuthorizationStartResponse2)

            => !(AuthorizationStartResponse1 == AuthorizationStartResponse2);

        #endregion

        #endregion

        #region IEquatable<AuthorizationStartResponse> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two authorize start responses for equality.
        /// </summary>
        /// <param name="Object">An authorize start response to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is AuthorizationStartResponse authorizationStartResponse &&
                   Equals(authorizationStartResponse);

        #endregion

        #region Equals(AuthorizationStartResponse)

        /// <summary>
        /// Compares two authorize start responses for equality.
        /// </summary>
        /// <param name="AuthorizationStartResponse">An authorize start response to compare with.</param>
        public override Boolean Equals(AuthorizationStartResponse? AuthorizationStartResponse)

            => AuthorizationStartResponse is not null &&
               AuthorizationStatus.Equals(AuthorizationStartResponse.AuthorizationStatus) &&
               StatusCode.         Equals(AuthorizationStartResponse.StatusCode)          &&
               //ToDo: AuthorizationStopIdentifications

               ((!SessionId.          HasValue && !AuthorizationStartResponse.SessionId.          HasValue) ||
                 (SessionId.          HasValue &&  AuthorizationStartResponse.SessionId.          HasValue && SessionId.          Value.Equals(AuthorizationStartResponse.SessionId.          Value))) &&

               ((!CPOPartnerSessionId.HasValue && !AuthorizationStartResponse.CPOPartnerSessionId.HasValue) ||
                 (CPOPartnerSessionId.HasValue &&  AuthorizationStartResponse.CPOPartnerSessionId.HasValue && CPOPartnerSessionId.Value.Equals(AuthorizationStartResponse.CPOPartnerSessionId.Value))) &&

               ((!EMPPartnerSessionId.HasValue && !AuthorizationStartResponse.EMPPartnerSessionId.HasValue) ||
                 (EMPPartnerSessionId.HasValue &&  AuthorizationStartResponse.EMPPartnerSessionId.HasValue && EMPPartnerSessionId.Value.Equals(AuthorizationStartResponse.EMPPartnerSessionId.Value))) &&

               ((!ProviderId.         HasValue && !AuthorizationStartResponse.ProviderId.         HasValue) ||
                 (ProviderId.         HasValue &&  AuthorizationStartResponse.ProviderId.         HasValue && ProviderId.         Value.Equals(AuthorizationStartResponse.ProviderId.         Value)));

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return AuthorizationStatus. GetHashCode()       * 13 ^
                       StatusCode.          GetHashCode()       * 11 ^

                      (SessionId?.          GetHashCode() ?? 0) *  7 ^
                      (CPOPartnerSessionId?.GetHashCode() ?? 0) *  5 ^
                      (EMPPartnerSessionId?.GetHashCode() ?? 0) *  3 ^
                      (ProviderId?.         GetHashCode() ?? 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(AuthorizationStatus,
                             StatusCode.HasResult
                                 ? ", " + StatusCode
                                 : "");

        #endregion


        #region ToBuilder

        /// <summary>
        /// Return a response builder.
        /// </summary>
        public Builder ToBuilder

            => new (Request,
                    ResponseTimestamp,
                    EventTrackingId,
                    Runtime,
                    AuthorizationStatus,
                    StatusCode,
                    SessionId,
                    CPOPartnerSessionId,
                    EMPPartnerSessionId,
                    ProviderId,
                    AuthorizationStopIdentifications,
                    ProcessId,
                    HTTPResponse,
                    CustomData);

        #endregion

        #region (class) Builder

        /// <summary>
        /// The AuthorizationStart response builder.
        /// </summary>
        public new class Builder : AResponse<AuthorizeStartRequest,
                                             AuthorizationStartResponse>.Builder
        {

            #region Properties

            /// <summary>
            /// The authorization status, e.g. "Authorized".
            /// </summary>
            [Mandatory]
            public AuthorizationStatusTypes?  AuthorizationStatus                 { get; set; }

            /// <summary>
            /// The authorization status code.
            /// </summary>
            [Mandatory]
            public StatusCode?                StatusCode                          { get; set; }

            /// <summary>
            /// The e-mobility provider identification.
            /// </summary>
            [Optional]
            public Provider_Id?               ProviderId                          { get; set; }

            /// <summary>
            /// The charging session identification.
            /// </summary>
            [Optional]
            public Session_Id?                SessionId                           { get; set; }

            /// <summary>
            /// An optional partner charging session identification.
            /// </summary>
            [Optional]
            public CPOPartnerSession_Id?      CPOPartnerSessionId                 { get; set; }

            /// <summary>
            /// An optional partner charging session identification.
            /// </summary>
            [Optional]
            public EMPPartnerSession_Id?      EMPPartnerSessionId                 { get; set; }

            /// <summary>
            /// An enumeration of authorization identifications.
            /// </summary>
            [Optional]
            public HashSet<Identification>    AuthorizationStopIdentifications    { get; }

            #endregion

            #region Constructor(s)

            /// <summary>
            /// Create a new AuthorizationStart response builder.
            /// </summary>
            /// <param name="Request">The request leading to this response.</param>
            /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
            /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
            /// <param name="Runtime">The runtime of the request/response.</param>
            /// <param name="AuthorizationStatus">The authorization status.</param>
            /// <param name="StatusCode">A status code.</param>
            /// <param name="SessionId">An optional charging session identification.</param>
            /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
            /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
            /// <param name="ProviderId">An optional e-mobility provider identification.</param>
            /// <param name="AuthorizationStopIdentifications">Optional authorization stop identifications.</param>
            /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
            /// <param name="HTTPResponse">The optional HTTP response.</param>
            /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
            public Builder(AuthorizeStartRequest?        Request                            = null,
                           DateTime?                     ResponseTimestamp                  = null,
                           EventTracking_Id?             EventTrackingId                    = null,
                           TimeSpan?                     Runtime                            = null,
                           AuthorizationStatusTypes?     AuthorizationStatus                = null,
                           StatusCode?                   StatusCode                         = null,
                           Session_Id?                   SessionId                          = null,
                           CPOPartnerSession_Id?         CPOPartnerSessionId                = null,
                           EMPPartnerSession_Id?         EMPPartnerSessionId                = null,
                           Provider_Id?                  ProviderId                         = null,
                           IEnumerable<Identification>?  AuthorizationStopIdentifications   = null,
                           Process_Id?                   ProcessId                          = null,
                           HTTPResponse?                 HTTPResponse                       = null,
                           JObject?                      CustomData                         = null)

                : base(ResponseTimestamp,
                       EventTrackingId,
                       Runtime,
                       Request,
                       HTTPResponse,
                       ProcessId,
                       CustomData)

            {

                this.AuthorizationStatus               = AuthorizationStatus;
                this.StatusCode                        = StatusCode;
                this.SessionId                         = SessionId;
                this.CPOPartnerSessionId               = CPOPartnerSessionId;
                this.EMPPartnerSessionId               = EMPPartnerSessionId;
                this.ProviderId                        = ProviderId;
                this.AuthorizationStopIdentifications  = AuthorizationStopIdentifications is not null
                                                             ? new HashSet<Identification>(AuthorizationStopIdentifications)
                                                             : new HashSet<Identification>();

            }

            #endregion

            #region ToImmutable()

            /// <summary>
            /// Return an immutable version of the AuthorizationStart response.
            /// </summary>
            /// <param name="Builder">An AuthorizationStartResponse builder.</param>
            public static implicit operator AuthorizationStartResponse(Builder Builder)

                => Builder.ToImmutable();


            /// <summary>
            /// Return an immutable version of the AuthorizationStart response.
            /// </summary>
            public override AuthorizationStartResponse ToImmutable()
            {

                #region Check mandatory parameters

                if (!AuthorizationStatus.HasValue)
                    throw new ArgumentException("The given authorization status must not be null!", nameof(AuthorizationStatus));

                if (StatusCode is null)
                    throw new ArgumentException("The given status code must not be null!",          nameof(StatusCode));

                #endregion

                return new AuthorizationStartResponse(ResponseTimestamp ?? Timestamp.Now,
                                                      EventTrackingId   ?? EventTracking_Id.New,
                                                      ProcessId         ?? Process_Id.NewRandom(),
                                                      Runtime           ?? (Request is not null
                                                                                ? Timestamp.Now - Request.Timestamp
                                                                                : TimeSpan.Zero),
                                                      AuthorizationStatus.Value,
                                                      StatusCode,
                                                      Request,
                                                      SessionId,
                                                      CPOPartnerSessionId,
                                                      EMPPartnerSessionId,
                                                      ProviderId,
                                                      AuthorizationStopIdentifications,
                                                      HTTPResponse,
                                                      CustomData);

            }

            #endregion

        }

        #endregion

    }

}
