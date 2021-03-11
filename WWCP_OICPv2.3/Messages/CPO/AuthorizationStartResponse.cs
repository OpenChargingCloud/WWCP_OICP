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
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

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
        public AuthorizationStatusTypes     AuthorizationStatus                 { get; }

        /// <summary>
        /// The authorization status code.
        /// </summary>
        [Mandatory]
        public StatusCode                   StatusCode                          { get; }

        /// <summary>
        /// The e-mobility provider identification.
        /// </summary>
        [Optional]
        public Provider_Id?                 ProviderId                          { get; }

        /// <summary>
        /// The charging session identification.
        /// </summary>
        [Optional]
        public Session_Id?                  SessionId                           { get; }

        /// <summary>
        /// An optional partner charging session identification.
        /// </summary>
        [Optional]
        public CPOPartnerSession_Id?        CPOPartnerSessionId                 { get; }

        /// <summary>
        /// An optional partner charging session identification.
        /// </summary>
        [Optional]
        public EMPPartnerSession_Id?        EMPPartnerSessionId                 { get; }

        /// <summary>
        /// An enumeration of authorization identifications.
        /// </summary>
        [Optional]
        public IEnumerable<Identification>  AuthorizationStopIdentifications    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new AuthorizationStart response.
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
        /// <param name="CustomData">Optional custom data, e.g. in combination with custom parsers and serializers.</param>
        private AuthorizationStartResponse(AuthorizeStartRequest        Request,
                                           DateTime                     ResponseTimestamp,
                                           EventTracking_Id             EventTrackingId,
                                           TimeSpan                     Runtime,
                                           AuthorizationStatusTypes     AuthorizationStatus,
                                           StatusCode                   StatusCode,
                                           Session_Id?                  SessionId                          = null,
                                           CPOPartnerSession_Id?        CPOPartnerSessionId                = null,
                                           EMPPartnerSession_Id?        EMPPartnerSessionId                = null,
                                           Provider_Id?                 ProviderId                         = null,
                                           IEnumerable<Identification>  AuthorizationStopIdentifications   = null,
                                           Process_Id?                  ProcessId                          = null,
                                           JObject                      CustomData                         = null)

            : base(Request,
                   ResponseTimestamp,
                   EventTrackingId,
                   Runtime,
                   ProcessId,
                   CustomData)

        {

            this.AuthorizationStatus               = AuthorizationStatus;
            this.StatusCode                        = StatusCode;
            this.SessionId                         = SessionId;
            this.CPOPartnerSessionId               = CPOPartnerSessionId;
            this.EMPPartnerSessionId               = EMPPartnerSessionId;
            this.ProviderId                        = ProviderId;
            this.AuthorizationStopIdentifications  = AuthorizationStopIdentifications?.Distinct() ?? new Identification[0];

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
        /// <param name="CustomData">Optional custom data, e.g. in combination with custom parsers and serializers.</param>
        public static AuthorizationStartResponse Authorized(AuthorizeStartRequest        Request,
                                                            Session_Id?                  SessionId                          = null,
                                                            CPOPartnerSession_Id?        CPOPartnerSessionId                = null,
                                                            EMPPartnerSession_Id?        EMPPartnerSessionId                = null,
                                                            Provider_Id?                 ProviderId                         = null,
                                                            String                       StatusCodeDescription              = null,
                                                            String                       StatusCodeAdditionalInfo           = null,
                                                            IEnumerable<Identification>  AuthorizationStopIdentifications   = null,
                                                            DateTime?                    ResponseTimestamp                  = null,
                                                            EventTracking_Id             EventTrackingId                    = null,
                                                            TimeSpan?                    Runtime                            = null,
                                                            Process_Id?                  ProcessId                          = null,
                                                            JObject                      CustomData                         = null)


            => new AuthorizationStartResponse(Request,
                                              ResponseTimestamp ?? DateTime.UtcNow,
                                              EventTrackingId   ?? EventTracking_Id.New,
                                              Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                              AuthorizationStatusTypes.Authorized,
                                              new StatusCode(
                                                  StatusCodes.Success,
                                                  StatusCodeDescription,
                                                  StatusCodeAdditionalInfo
                                              ),
                                              SessionId,
                                              CPOPartnerSessionId,
                                              EMPPartnerSessionId,
                                              ProviderId,
                                              AuthorizationStopIdentifications,
                                              ProcessId,
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
        /// <param name="CustomData">Optional custom data, e.g. in combination with custom parsers and serializers.</param>
        public static AuthorizationStartResponse NotAuthorized(AuthorizeStartRequest  Request,
                                                               StatusCode             StatusCode,
                                                               Session_Id?            SessionId             = null,
                                                               CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                                                               EMPPartnerSession_Id?  EMPPartnerSessionId   = null,
                                                               Provider_Id?           ProviderId            = null,
                                                               DateTime?              ResponseTimestamp     = null,
                                                               EventTracking_Id       EventTrackingId       = null,
                                                               TimeSpan?              Runtime               = null,
                                                               Process_Id?            ProcessId             = null,
                                                               JObject                CustomData            = null)

            => new AuthorizationStartResponse(Request,
                                              ResponseTimestamp ?? DateTime.UtcNow,
                                              EventTrackingId   ?? EventTracking_Id.New,
                                              Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                              AuthorizationStatusTypes.NotAuthorized,
                                              StatusCode,
                                              SessionId,
                                              CPOPartnerSessionId,
                                              EMPPartnerSessionId,
                                              ProviderId,
                                              null,
                                              ProcessId,
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
        /// <param name="CustomData">Optional custom data, e.g. in combination with custom parsers and serializers.</param>
        public static AuthorizationStartResponse SessionIsInvalid(AuthorizeStartRequest  Request,
                                                                  String                 StatusCodeDescription      = null,
                                                                  String                 StatusCodeAdditionalInfo   = null,
                                                                  Session_Id?            SessionId                  = null,
                                                                  CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                                  EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                                  Provider_Id?           ProviderId                 = null,
                                                                  DateTime?              ResponseTimestamp          = null,
                                                                  EventTracking_Id       EventTrackingId            = null,
                                                                  TimeSpan?              Runtime                    = null,
                                                                  Process_Id?            ProcessId                  = null,
                                                                  JObject                CustomData                 = null)

            => new AuthorizationStartResponse(Request,
                                              ResponseTimestamp ?? DateTime.UtcNow,
                                              EventTrackingId   ?? EventTracking_Id.New,
                                              Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                              AuthorizationStatusTypes.NotAuthorized,
                                              new StatusCode(
                                                  StatusCodes.SessionIsInvalid,
                                                  StatusCodeDescription ?? "Session is invalid",
                                                  StatusCodeAdditionalInfo
                                              ),
                                              SessionId,
                                              CPOPartnerSessionId,
                                              EMPPartnerSessionId,
                                              ProviderId,
                                              null,
                                              ProcessId,
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
        /// <param name="CustomData">Optional custom data, e.g. in combination with custom parsers and serializers.</param>
        public static AuthorizationStartResponse CommunicationToEVSEFailed(AuthorizeStartRequest  Request,
                                                                           String                 StatusCodeDescription      = null,
                                                                           String                 StatusCodeAdditionalInfo   = null,
                                                                           Session_Id?            SessionId                  = null,
                                                                           CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                                           EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                                           Provider_Id?           ProviderId                 = null,
                                                                           DateTime?              ResponseTimestamp          = null,
                                                                           EventTracking_Id       EventTrackingId            = null,
                                                                           TimeSpan?              Runtime                    = null,
                                                                           Process_Id?            ProcessId                  = null,
                                                                           JObject                CustomData                 = null)

            => new AuthorizationStartResponse(Request,
                                              ResponseTimestamp ?? DateTime.UtcNow,
                                              EventTrackingId   ?? EventTracking_Id.New,
                                              Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                              AuthorizationStatusTypes.NotAuthorized,
                                              new StatusCode(
                                                  StatusCodes.CommunicationToEVSEFailed,
                                                  StatusCodeDescription ?? "Communication to EVSE failed!",
                                                  StatusCodeAdditionalInfo
                                              ),
                                              SessionId,
                                              CPOPartnerSessionId,
                                              EMPPartnerSessionId,
                                              ProviderId,
                                              null,
                                              ProcessId,
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
        /// <param name="CustomData">Optional custom data, e.g. in combination with custom parsers and serializers.</param>
        public static AuthorizationStartResponse NoEVConnectedToEVSE(AuthorizeStartRequest  Request,
                                                                     String                 StatusCodeDescription      = null,
                                                                     String                 StatusCodeAdditionalInfo   = null,
                                                                     Session_Id?            SessionId                  = null,
                                                                     CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                                     EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                                     Provider_Id?           ProviderId                 = null,
                                                                     DateTime?              ResponseTimestamp          = null,
                                                                     EventTracking_Id       EventTrackingId            = null,
                                                                     TimeSpan?              Runtime                    = null,
                                                                     Process_Id?            ProcessId                  = null,
                                                                     JObject                CustomData                 = null)

            => new AuthorizationStartResponse(Request,
                                              ResponseTimestamp ?? DateTime.UtcNow,
                                              EventTrackingId   ?? EventTracking_Id.New,
                                              Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                              AuthorizationStatusTypes.NotAuthorized,
                                              new StatusCode(
                                                  StatusCodes.NoEVConnectedToEVSE,
                                                  StatusCodeDescription ?? "No EV connected to EVSE!",
                                                  StatusCodeAdditionalInfo
                                              ),
                                              SessionId,
                                              CPOPartnerSessionId,
                                              EMPPartnerSessionId,
                                              ProviderId,
                                              null,
                                              ProcessId,
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
        /// <param name="CustomData">Optional custom data, e.g. in combination with custom parsers and serializers.</param>
        public static AuthorizationStartResponse EVSEAlreadyReserved(AuthorizeStartRequest  Request,
                                                                     String                 StatusCodeDescription      = null,
                                                                     String                 StatusCodeAdditionalInfo   = null,
                                                                     Session_Id?            SessionId                  = null,
                                                                     CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                                     EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                                     Provider_Id?           ProviderId                 = null,
                                                                     DateTime?              ResponseTimestamp          = null,
                                                                     EventTracking_Id       EventTrackingId            = null,
                                                                     TimeSpan?              Runtime                    = null,
                                                                     Process_Id?            ProcessId                  = null,
                                                                     JObject                CustomData                 = null)

            => new AuthorizationStartResponse(Request,
                                              ResponseTimestamp ?? DateTime.UtcNow,
                                              EventTrackingId   ?? EventTracking_Id.New,
                                              Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                              AuthorizationStatusTypes.NotAuthorized,
                                              new StatusCode(
                                                  StatusCodes.EVSEAlreadyReserved,
                                                  StatusCodeDescription ?? "EVSE already reserved!",
                                                  StatusCodeAdditionalInfo
                                              ),
                                              SessionId,
                                              CPOPartnerSessionId,
                                              EMPPartnerSessionId,
                                              ProviderId,
                                              null,
                                              ProcessId,
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
        /// <param name="CustomData">Optional custom data, e.g. in combination with custom parsers and serializers.</param>
        public static AuthorizationStartResponse UnknownEVSEID(AuthorizeStartRequest  Request,
                                                               String                 StatusCodeDescription      = null,
                                                               String                 StatusCodeAdditionalInfo   = null,
                                                               Session_Id?            SessionId                  = null,
                                                               CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                               EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                               Provider_Id?           ProviderId                 = null,
                                                               DateTime?              ResponseTimestamp          = null,
                                                               EventTracking_Id       EventTrackingId            = null,
                                                               TimeSpan?              Runtime                    = null,
                                                               Process_Id?            ProcessId                  = null,
                                                               JObject                CustomData                 = null)

            => new AuthorizationStartResponse(Request,
                                              ResponseTimestamp ?? DateTime.UtcNow,
                                              EventTrackingId   ?? EventTracking_Id.New,
                                              Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                              AuthorizationStatusTypes.NotAuthorized,
                                              new StatusCode(
                                                  StatusCodes.UnknownEVSEID,
                                                  StatusCodeDescription ?? "Unknown EVSE ID!",
                                                  StatusCodeAdditionalInfo
                                              ),
                                              SessionId,
                                              CPOPartnerSessionId,
                                              EMPPartnerSessionId,
                                              ProviderId,
                                              null,
                                              ProcessId,
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
        /// <param name="CustomData">Optional custom data, e.g. in combination with custom parsers and serializers.</param>
        public static AuthorizationStartResponse EVSEOutOfService(AuthorizeStartRequest  Request,
                                                                  String                 StatusCodeDescription      = null,
                                                                  String                 StatusCodeAdditionalInfo   = null,
                                                                  Session_Id?            SessionId                  = null,
                                                                  CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                                  EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                                  Provider_Id?           ProviderId                 = null,
                                                                  DateTime?              ResponseTimestamp          = null,
                                                                  EventTracking_Id       EventTrackingId            = null,
                                                                  TimeSpan?              Runtime                    = null,
                                                                  Process_Id?            ProcessId                  = null,
                                                                  JObject                CustomData                 = null)

            => new AuthorizationStartResponse(Request,
                                              ResponseTimestamp ?? DateTime.UtcNow,
                                              EventTrackingId   ?? EventTracking_Id.New,
                                              Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                              AuthorizationStatusTypes.NotAuthorized,
                                              new StatusCode(
                                                  StatusCodes.EVSEOutOfService,
                                                  StatusCodeDescription ?? "EVSE out of service!",
                                                  StatusCodeAdditionalInfo
                                              ),
                                              SessionId,
                                              CPOPartnerSessionId,
                                              EMPPartnerSessionId,
                                              ProviderId,
                                              null,
                                              ProcessId,
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
        /// <param name="CustomData">Optional custom data, e.g. in combination with custom parsers and serializers.</param>
        public static AuthorizationStartResponse ServiceNotAvailable(AuthorizeStartRequest  Request,
                                                                     String                 StatusCodeDescription      = null,
                                                                     String                 StatusCodeAdditionalInfo   = null,
                                                                     Session_Id?            SessionId                  = null,
                                                                     CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                                     EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                                     Provider_Id?           ProviderId                 = null,
                                                                     DateTime?              ResponseTimestamp          = null,
                                                                     EventTracking_Id       EventTrackingId            = null,
                                                                     TimeSpan?              Runtime                    = null,
                                                                     Process_Id?            ProcessId                  = null,
                                                                     JObject                CustomData                 = null)

            => new AuthorizationStartResponse(Request,
                                              ResponseTimestamp ?? DateTime.UtcNow,
                                              EventTrackingId   ?? EventTracking_Id.New,
                                              Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                              AuthorizationStatusTypes.NotAuthorized,
                                              new StatusCode(
                                                  StatusCodes.ServiceNotAvailable,
                                                  StatusCodeDescription ?? "Service not available!",
                                                  StatusCodeAdditionalInfo
                                              ),
                                              SessionId,
                                              CPOPartnerSessionId,
                                              EMPPartnerSessionId,
                                              ProviderId,
                                              null,
                                              ProcessId,
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
        /// <param name="CustomData">Optional custom data, e.g. in combination with custom parsers and serializers.</param>
        public static AuthorizationStartResponse DataError(AuthorizeStartRequest  Request,
                                                           String                 StatusCodeDescription      = null,
                                                           String                 StatusCodeAdditionalInfo   = null,
                                                           Session_Id?            SessionId                  = null,
                                                           CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                           EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                           Provider_Id?           ProviderId                 = null,
                                                           DateTime?              ResponseTimestamp          = null,
                                                           EventTracking_Id       EventTrackingId            = null,
                                                           TimeSpan?              Runtime                    = null,
                                                           Process_Id?            ProcessId                  = null,
                                                           JObject                CustomData                 = null)

            => new AuthorizationStartResponse(Request,
                                              ResponseTimestamp ?? DateTime.UtcNow,
                                              EventTrackingId   ?? EventTracking_Id.New,
                                              Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                              AuthorizationStatusTypes.NotAuthorized,
                                              new StatusCode(
                                                  StatusCodes.DataError,
                                                  StatusCodeDescription ?? "Data Error!",
                                                  StatusCodeAdditionalInfo
                                              ),
                                              SessionId,
                                              CPOPartnerSessionId,
                                              EMPPartnerSessionId,
                                              ProviderId,
                                              null,
                                              ProcessId,
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
        /// <param name="CustomData">Optional custom data, e.g. in combination with custom parsers and serializers.</param>
        public static AuthorizationStartResponse SystemError(AuthorizeStartRequest  Request,
                                                             String                 StatusCodeDescription      = null,
                                                             String                 StatusCodeAdditionalInfo   = null,
                                                             Session_Id?            SessionId                  = null,
                                                             CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                             EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                             Provider_Id?           ProviderId                 = null,
                                                             DateTime?              ResponseTimestamp          = null,
                                                             EventTracking_Id       EventTrackingId            = null,
                                                             TimeSpan?              Runtime                    = null,
                                                             Process_Id?            ProcessId                  = null,
                                                             JObject                CustomData                 = null)

            => new AuthorizationStartResponse(Request,
                                              ResponseTimestamp ?? DateTime.UtcNow,
                                              EventTrackingId   ?? EventTracking_Id.New,
                                              Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                              AuthorizationStatusTypes.NotAuthorized,
                                              new StatusCode(
                                                  StatusCodes.SystemError,
                                                  StatusCodeDescription ?? "System Error!",
                                                  StatusCodeAdditionalInfo
                                              ),
                                              SessionId,
                                              CPOPartnerSessionId,
                                              EMPPartnerSessionId,
                                              ProviderId,
                                              null,
                                              ProcessId,
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
        /// <param name="CustomAuthorizationStartResponseParser">A delegate to parse custom AuthorizationStart response JSON objects.</param>
        public static AuthorizationStartResponse Parse(AuthorizeStartRequest                                    Request,
                                                       JObject                                                  JSON,
                                                       DateTime?                                                ResponseTimestamp                        = null,
                                                       EventTracking_Id                                         EventTrackingId                          = null,
                                                       TimeSpan?                                                Runtime                                  = null,
                                                       Process_Id?                                              ProcessId                                = null,
                                                       CustomJObjectParserDelegate<AuthorizationStartResponse>  CustomAuthorizationStartResponseParser   = null)
        {

            if (TryParse(Request,
                         JSON,
                         out AuthorizationStartResponse  authorizationStartResponse,
                         out String                      ErrorResponse,
                         ResponseTimestamp,
                         EventTrackingId,
                         Runtime,
                         ProcessId,
                         CustomAuthorizationStartResponseParser))
            {
                return authorizationStartResponse;
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
        /// <param name="CustomAuthorizationStartResponseParser">A delegate to parse custom AuthorizationStart response JSON objects.</param>
        public static AuthorizationStartResponse Parse(AuthorizeStartRequest                                    Request,
                                                       String                                                   Text,
                                                       DateTime?                                                ResponseTimestamp                        = null,
                                                       EventTracking_Id                                         EventTrackingId                          = null,
                                                       TimeSpan?                                                Runtime                                  = null,
                                                       Process_Id?                                              ProcessId                                = null,
                                                       CustomJObjectParserDelegate<AuthorizationStartResponse>  CustomAuthorizationStartResponseParser   = null)
        {

            if (TryParse(Request,
                         Text,
                         out AuthorizationStartResponse  authorizationStartResponse,
                         out String                      ErrorResponse,
                         ResponseTimestamp,
                         EventTrackingId,
                         Runtime,
                         ProcessId,
                         CustomAuthorizationStartResponseParser))
            {
                return authorizationStartResponse;
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
        /// <param name="CustomAuthorizationStartResponseParser">A delegate to parse custom AuthorizationStart response JSON objects.</param>
        public static Boolean TryParse(AuthorizeStartRequest                                    Request,
                                       JObject                                                  JSON,
                                       out AuthorizationStartResponse                           AuthorizationStartResponse,
                                       out String                                               ErrorResponse,
                                       DateTime?                                                ResponseTimestamp                        = null,
                                       EventTracking_Id                                         EventTrackingId                          = null,
                                       TimeSpan?                                                Runtime                                  = null,
                                       Process_Id?                                              ProcessId                                = null,
                                       CustomJObjectParserDelegate<AuthorizationStartResponse>  CustomAuthorizationStartResponseParser   = null)
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
                                         AuthorizationStatusTypesExtentions.TryParse,
                                         out AuthorizationStatusTypes AuthorizationStatus,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse StatusCode                            [mandatory]

                if (!JSON.ParseMandatoryJSON2("StatusCode",
                                              "status code",
                                              OICPv2_3.StatusCode.TryParse,
                                              out StatusCode StatusCode,
                                              out ErrorResponse))
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
                    if (ErrorResponse != null)
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
                    if (ErrorResponse != null)
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
                    if (ErrorResponse != null)
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
                    if (ErrorResponse != null)
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
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse Custom Data                           [optional]

                var CustomData = JSON["CustomData"] as JObject;

                #endregion


                AuthorizationStartResponse = new AuthorizationStartResponse(Request,
                                                                            ResponseTimestamp ?? DateTime.UtcNow,
                                                                            EventTrackingId   ?? Request.EventTrackingId,
                                                                            Runtime           ?? DateTime.UtcNow - Request.Timestamp,
                                                                            AuthorizationStatus,
                                                                            StatusCode,
                                                                            SessionId,
                                                                            CPOPartnerSessionId,
                                                                            EMPPartnerSessionId,
                                                                            ProviderId,
                                                                            AuthorizationStopIdentifications,
                                                                            ProcessId,
                                                                            CustomData);

                if (CustomAuthorizationStartResponseParser != null)
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
        /// <param name="CustomAuthorizationStartResponseParser">A delegate to parse custom AuthorizationStart response JSON objects.</param>
        public static Boolean TryParse(AuthorizeStartRequest                                    Request,
                                       String                                                   Text,
                                       out AuthorizationStartResponse                           AuthorizationStartResponse,
                                       out String                                               ErrorResponse,
                                       DateTime?                                                ResponseTimestamp                        = null,
                                       EventTracking_Id                                         EventTrackingId                          = null,
                                       TimeSpan?                                                Runtime                                  = null,
                                       Process_Id?                                              ProcessId                                = null,
                                       CustomJObjectParserDelegate<AuthorizationStartResponse>  CustomAuthorizationStartResponseParser   = null)
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
        public JObject ToJSON(CustomJObjectSerializerDelegate<AuthorizationStartResponse>  CustomAuthorizationStartSerializer   = null,
                              CustomJObjectSerializerDelegate<StatusCode>                  CustomStatusCodeSerializer           = null,
                              CustomJObjectSerializerDelegate<Identification>              CustomIdentificationSerializer       = null)

        {

            var JSON = JSONObject.Create(

                           new JProperty("AuthorizationStatus",                     AuthorizationStatus.ToString()),

                           new JProperty("StatusCode",                              StatusCode.         ToJSON(CustomStatusCodeSerializer)),

                           ProviderId.HasValue
                               ? new JProperty("ProviderID",                        ProviderId.         ToString())
                               : null,

                           SessionId.HasValue
                               ? new JProperty("SessionID",                         SessionId.          ToString())
                               : null,

                           CPOPartnerSessionId.HasValue
                               ? new JProperty("CPOPartnerSessionID",               CPOPartnerSessionId.ToString())
                               : null,

                           EMPPartnerSessionId.HasValue
                               ? new JProperty("EMPPartnerSessionID",               EMPPartnerSessionId.ToString())
                               : null,

                           AuthorizationStopIdentifications.SafeAny()
                               ? new JProperty("AuthorizationStopIdentifications",  new JArray(AuthorizationStopIdentifications.Select(identification => identification.ToJSON(CustomIdentificationSerializer))))
                               : null,

                           CustomData != null
                               ? new JProperty("CustomData",                        CustomData)
                               : null

                       );

            return CustomAuthorizationStartSerializer != null
                       ? CustomAuthorizationStartSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region Operator overloading

        #region Operator == (AuthorizationStart1, AuthorizationStart2)

        /// <summary>
        /// Compares two AuthorizationStart requests for equality.
        /// </summary>
        /// <param name="AuthorizationStart1">An authorize start request.</param>
        /// <param name="AuthorizationStart2">Another authorize start request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (AuthorizationStartResponse AuthorizationStart1,
                                           AuthorizationStartResponse AuthorizationStart2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(AuthorizationStart1, AuthorizationStart2))
                return true;

            // If one is null, but not both, return false.
            if (AuthorizationStart1 is null || AuthorizationStart2 is null)
                return false;

            return AuthorizationStart1.Equals(AuthorizationStart2);

        }

        #endregion

        #region Operator != (AuthorizationStart1, AuthorizationStart2)

        /// <summary>
        /// Compares two AuthorizationStart requests for inequality.
        /// </summary>
        /// <param name="AuthorizationStart1">An authorize start request.</param>
        /// <param name="AuthorizationStart2">Another authorize start request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (AuthorizationStartResponse AuthorizationStart1,
                                           AuthorizationStartResponse AuthorizationStart2)

            => !(AuthorizationStart1 == AuthorizationStart2);

        #endregion

        #endregion

        #region IEquatable<AuthorizationStart> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is AuthorizationStartResponse authorizationStartResponse &&
                   Equals(authorizationStartResponse);

        #endregion

        #region Equals(AuthorizationStart)

        /// <summary>
        /// Compares two authorize start requests for equality.
        /// </summary>
        /// <param name="AuthorizationStart">An authorize start request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(AuthorizationStartResponse AuthorizationStart)
        {

            if (AuthorizationStart is null)
                return false;

            return AuthorizationStatus.Equals(AuthorizationStart.AuthorizationStatus) &&
                   StatusCode.         Equals(AuthorizationStart.StatusCode)          &&
                   //AuthorizationStopIdentifications

                   ((!SessionId.          HasValue && !AuthorizationStart.SessionId.          HasValue) ||
                     (SessionId.          HasValue &&  AuthorizationStart.SessionId.          HasValue && SessionId.          Value.Equals(AuthorizationStart.SessionId.          Value))) &&

                   ((!CPOPartnerSessionId.HasValue && !AuthorizationStart.CPOPartnerSessionId.HasValue) ||
                     (CPOPartnerSessionId.HasValue &&  AuthorizationStart.CPOPartnerSessionId.HasValue && CPOPartnerSessionId.Value.Equals(AuthorizationStart.CPOPartnerSessionId.Value))) &&

                   ((!EMPPartnerSessionId.HasValue && !AuthorizationStart.EMPPartnerSessionId.HasValue) ||
                     (EMPPartnerSessionId.HasValue &&  AuthorizationStart.EMPPartnerSessionId.HasValue && EMPPartnerSessionId.Value.Equals(AuthorizationStart.EMPPartnerSessionId.Value))) &&

                   ((!ProviderId.         HasValue && !AuthorizationStart.ProviderId.         HasValue) ||
                     (ProviderId.         HasValue &&  AuthorizationStart.ProviderId.         HasValue && ProviderId.         Value.Equals(AuthorizationStart.ProviderId.         Value)));

        }

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
        /// Return a text-representation of this object.
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

            => new Builder(Request,
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
            public StatusCode                 StatusCode                          { get; set; }

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
            /// <param name="CustomData">Optional custom data, e.g. in combination with custom parsers and serializers.</param>
            public Builder(AuthorizeStartRequest        Request                            = null,
                           DateTime?                    ResponseTimestamp                  = null,
                           EventTracking_Id             EventTrackingId                    = null,
                           TimeSpan?                    Runtime                            = null,
                           AuthorizationStatusTypes?    AuthorizationStatus                = null,
                           StatusCode                   StatusCode                         = null,
                           Session_Id?                  SessionId                          = null,
                           CPOPartnerSession_Id?        CPOPartnerSessionId                = null,
                           EMPPartnerSession_Id?        EMPPartnerSessionId                = null,
                           Provider_Id?                 ProviderId                         = null,
                           IEnumerable<Identification>  AuthorizationStopIdentifications   = null,
                           Process_Id?                  ProcessId                          = null,
                           JObject                      CustomData                         = null)

                : base(Request,
                       ResponseTimestamp,
                       EventTrackingId,
                       Runtime,
                       ProcessId,
                       CustomData)

            {

                this.AuthorizationStatus               = AuthorizationStatus;
                this.StatusCode                        = StatusCode;
                this.SessionId                         = SessionId;
                this.CPOPartnerSessionId               = CPOPartnerSessionId;
                this.EMPPartnerSessionId               = EMPPartnerSessionId;
                this.ProviderId                        = ProviderId;
                this.AuthorizationStopIdentifications  = AuthorizationStopIdentifications != null
                                                             ? new HashSet<Identification>(AuthorizationStopIdentifications)
                                                             : new HashSet<Identification>();

            }

            #endregion

            #region ToImmutable()

            /// <summary>
            /// Return an immutable version of the AuthorizationStart response.
            /// </summary>
            /// <param name="Builder">A EVSE data record builder.</param>
            public static implicit operator AuthorizationStartResponse(Builder Builder)

                => Builder?.ToImmutable();


            /// <summary>
            /// Return an immutable version of the AuthorizationStart response.
            /// </summary>
            public override AuthorizationStartResponse ToImmutable()
            {

                #region Check mandatory parameters

                if (!AuthorizationStatus.HasValue)
                    throw new ArgumentException("The given authorization status must not be null!", nameof(AuthorizationStatus));

                #endregion

                return new AuthorizationStartResponse(Request           ?? throw new ArgumentNullException(nameof(Request), "The given request must not be null!"),
                                                      ResponseTimestamp ?? DateTime.UtcNow,
                                                      EventTrackingId   ?? EventTracking_Id.New,
                                                      Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                                      AuthorizationStatus.Value,
                                                      StatusCode,
                                                      SessionId,
                                                      CPOPartnerSessionId,
                                                      EMPPartnerSessionId,
                                                      ProviderId,
                                                      AuthorizationStopIdentifications,
                                                      ProcessId,
                                                      CustomData);

            }

            #endregion

        }

        #endregion

    }

}
