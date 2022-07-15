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

using Newtonsoft.Json.Linq;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// The AuthorizationStop response.
    /// </summary>
    public class AuthorizationStopResponse : AResponse<AuthorizeStopRequest,
                                                        AuthorizationStopResponse>
    {

        #region Properties

        /// <summary>
        /// The authorization status, e.g. "Authorized".
        /// </summary>
        [Mandatory]
        public AuthorizationStatusTypes  AuthorizationStatus    { get; }

        /// <summary>
        /// The authorization status code.
        /// </summary>
        [Mandatory]
        public StatusCode                StatusCode             { get; }

        /// <summary>
        /// The e-mobility provider identification.
        /// </summary>
        [Optional]
        public Provider_Id?              ProviderId             { get; }

        /// <summary>
        /// The charging session identification.
        /// </summary>
        [Optional]
        public Session_Id?               SessionId              { get; }

        /// <summary>
        /// An optional partner charging session identification.
        /// </summary>
        [Optional]
        public CPOPartnerSession_Id?     CPOPartnerSessionId    { get; }

        /// <summary>
        /// An optional partner charging session identification.
        /// </summary>
        [Optional]
        public EMPPartnerSession_Id?     EMPPartnerSessionId    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new AuthorizationStop response.
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
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        private AuthorizationStopResponse(AuthorizeStopRequest         Request,
                                          DateTime                     ResponseTimestamp,
                                          EventTracking_Id             EventTrackingId,
                                          TimeSpan                     Runtime,
                                          AuthorizationStatusTypes     AuthorizationStatus,
                                          StatusCode                   StatusCode,
                                          Session_Id?                  SessionId                          = null,
                                          CPOPartnerSession_Id?        CPOPartnerSessionId                = null,
                                          EMPPartnerSession_Id?        EMPPartnerSessionId                = null,
                                          Provider_Id?                 ProviderId                         = null,
                                          Process_Id?                  ProcessId                          = null,
                                          HTTPResponse                 HTTPResponse                       = null,
                                          JObject                      CustomData                         = null)

            : base(ResponseTimestamp,
                   EventTrackingId,
                   Runtime,
                   Request,
                   HTTPResponse,
                   ProcessId,
                   CustomData)

        {

            this.AuthorizationStatus  = AuthorizationStatus;
            this.StatusCode           = StatusCode;
            this.SessionId            = SessionId;
            this.CPOPartnerSessionId  = CPOPartnerSessionId;
            this.EMPPartnerSessionId  = EMPPartnerSessionId;
            this.ProviderId           = ProviderId;

        }

        #endregion


        #region (static) Authorized               (Request, ...)

        /// <summary>
        /// Create a new 'Authorized' AuthorizationStop result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="SessionId">The charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public static AuthorizationStopResponse Authorized(AuthorizeStopRequest   Request,
                                                           Session_Id?            SessionId                          = null,
                                                           CPOPartnerSession_Id?  CPOPartnerSessionId                = null,
                                                           EMPPartnerSession_Id?  EMPPartnerSessionId                = null,
                                                           Provider_Id?           ProviderId                         = null,
                                                           String                 StatusCodeDescription              = null,
                                                           String                 StatusCodeAdditionalInfo           = null,
                                                           DateTime?              ResponseTimestamp                  = null,
                                                           EventTracking_Id       EventTrackingId                    = null,
                                                           TimeSpan?              Runtime                            = null,
                                                           Process_Id?            ProcessId                          = null,
                                                           HTTPResponse           HTTPResponse                       = null,
                                                           JObject                CustomData                         = null)


            => new AuthorizationStopResponse(Request,
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
                                             ProcessId,
                                             HTTPResponse,
                                             CustomData);

        #endregion

        #region (static) NotAuthorized            (Request, StatusCode, ...)

        /// <summary>
        /// Create a new 'NotAuthorized' AuthorizationStop result.
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
        public static AuthorizationStopResponse NotAuthorized(AuthorizeStopRequest   Request,
                                                              StatusCode             StatusCode,
                                                              Session_Id?            SessionId             = null,
                                                              CPOPartnerSession_Id?  CPOPartnerSessionId   = null,
                                                              EMPPartnerSession_Id?  EMPPartnerSessionId   = null,
                                                              Provider_Id?           ProviderId            = null,
                                                              DateTime?              ResponseTimestamp     = null,
                                                              EventTracking_Id       EventTrackingId       = null,
                                                              TimeSpan?              Runtime               = null,
                                                              Process_Id?            ProcessId             = null,
                                                              HTTPResponse           HTTPResponse          = null,
                                                              JObject                CustomData            = null)

            => new AuthorizationStopResponse(Request,
                                             ResponseTimestamp ?? DateTime.UtcNow,
                                             EventTrackingId   ?? EventTracking_Id.New,
                                             Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                             AuthorizationStatusTypes.NotAuthorized,
                                             StatusCode,
                                             SessionId,
                                             CPOPartnerSessionId,
                                             EMPPartnerSessionId,
                                             ProviderId,
                                             ProcessId,
                                             HTTPResponse,
                                             CustomData);

        #endregion

        #region (static) SessionIsInvalid         (Request, ...)

        /// <summary>
        /// Create a new 'SessionIsInvalid' AuthorizationStop result.
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
        public static AuthorizationStopResponse SessionIsInvalid(AuthorizeStopRequest   Request,
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
                                                                 HTTPResponse           HTTPResponse               = null,
                                                                 JObject                CustomData                 = null)

            => new AuthorizationStopResponse(Request,
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
                                             ProcessId,
                                             HTTPResponse,
                                             CustomData);

        #endregion

        #region (static) CommunicationToEVSEFailed(Request, ...)

        /// <summary>
        /// Create a new 'CommunicationToEVSEFailed' AuthorizationStop result.
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
        public static AuthorizationStopResponse CommunicationToEVSEFailed(AuthorizeStopRequest   Request,
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
                                                                          HTTPResponse           HTTPResponse               = null,
                                                                          JObject                CustomData                 = null)

            => new AuthorizationStopResponse(Request,
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
                                             ProcessId,
                                             HTTPResponse,
                                             CustomData);

        #endregion

        #region (static) NoEVConnectedToEVSE      (Request, ...)

        /// <summary>
        /// Create a new 'NoEVConnectedToEVSE' AuthorizationStop result.
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
        public static AuthorizationStopResponse NoEVConnectedToEVSE(AuthorizeStopRequest   Request,
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
                                                                    HTTPResponse           HTTPResponse               = null,
                                                                    JObject                CustomData                 = null)

            => new AuthorizationStopResponse(Request,
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
                                             ProcessId,
                                             HTTPResponse,
                                             CustomData);

        #endregion

        #region (static) EVSEAlreadyReserved      (Request, ...)

        /// <summary>
        /// Create a new 'EVSEAlreadyReserved' AuthorizationStop result.
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
        public static AuthorizationStopResponse EVSEAlreadyReserved(AuthorizeStopRequest   Request,
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
                                                                    HTTPResponse           HTTPResponse               = null,
                                                                    JObject                CustomData                 = null)

            => new AuthorizationStopResponse(Request,
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
                                             ProcessId,
                                             HTTPResponse,
                                             CustomData);

        #endregion

        #region (static) UnknownEVSEID            (Request, ...)

        /// <summary>
        /// Create a new 'UnknownEVSEID' AuthorizationStop result.
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
        public static AuthorizationStopResponse UnknownEVSEID(AuthorizeStopRequest   Request,
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
                                                              HTTPResponse           HTTPResponse               = null,
                                                              JObject                CustomData                 = null)

            => new AuthorizationStopResponse(Request,
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
                                             ProcessId,
                                             HTTPResponse,
                                             CustomData);

        #endregion

        #region (static) EVSEOutOfService         (Request, ...)

        /// <summary>
        /// Create a new 'EVSEOutOfService' AuthorizationStop result.
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
        public static AuthorizationStopResponse EVSEOutOfService(AuthorizeStopRequest   Request,
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
                                                                 HTTPResponse           HTTPResponse               = null,
                                                                 JObject                CustomData                 = null)

            => new AuthorizationStopResponse(Request,
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
                                             ProcessId,
                                             HTTPResponse,
                                             CustomData);

        #endregion

        #region (static) ServiceNotAvailable      (Request, ...)

        /// <summary>
        /// Create a new 'ServiceNotAvailable' AuthorizationStop result.
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
        public static AuthorizationStopResponse ServiceNotAvailable(AuthorizeStopRequest   Request,
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
                                                                    HTTPResponse           HTTPResponse               = null,
                                                                    JObject                CustomData                 = null)

            => new AuthorizationStopResponse(Request,
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
                                             ProcessId,
                                             HTTPResponse,
                                             CustomData);

        #endregion

        #region (static) DataError                (Request, ...)

        /// <summary>
        /// Create a new 'DataError' AuthorizationStop result.
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
        public static AuthorizationStopResponse DataError(AuthorizeStopRequest   Request,
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
                                                          HTTPResponse           HTTPResponse               = null,
                                                          JObject                CustomData                 = null)

            => new AuthorizationStopResponse(Request,
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
                                             ProcessId,
                                             HTTPResponse,
                                             CustomData);

        #endregion

        #region (static) SystemError              (Request, ...)

        /// <summary>
        /// Create a new 'SystemError' AuthorizationStop result.
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
        public static AuthorizationStopResponse SystemError(AuthorizeStopRequest   Request,
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
                                                            HTTPResponse           HTTPResponse               = null,
                                                            JObject                CustomData                 = null)

            => new AuthorizationStopResponse(Request,
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
                                             ProcessId,
                                             HTTPResponse,
                                             CustomData);

        #endregion


        #region Documentation

        // https://github.com/hubject/oicp/blob/master/OICP-2.3/OICP%202.3%20CPO/02_CPO_Services_and_Operations.asciidoc#eRoamingAuthorizationStop

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

        #region (static) Parse   (JSON, CustomAuthorizationStopResponseParser = null)

        /// <summary>
        /// Parse the given JSON representation of a AuthorizationStop response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomAuthorizationStopResponseParser">A delegate to parse custom AuthorizationStop response JSON objects.</param>
        public static AuthorizationStopResponse Parse(AuthorizeStopRequest                                    Request,
                                                      JObject                                                 JSON,
                                                      DateTime?                                               ResponseTimestamp                       = null,
                                                      EventTracking_Id                                        EventTrackingId                         = null,
                                                      TimeSpan?                                               Runtime                                 = null,
                                                      Process_Id?                                             ProcessId                               = null,
                                                      HTTPResponse                                            HTTPResponse                            = null,
                                                      CustomJObjectParserDelegate<AuthorizationStopResponse>  CustomAuthorizationStopResponseParser   = null)
        {

            if (TryParse(Request,
                         JSON,
                         out AuthorizationStopResponse  authorizationStopResponse,
                         out String                     ErrorResponse,
                         ResponseTimestamp,
                         EventTrackingId,
                         Runtime,
                         ProcessId,
                         HTTPResponse,
                         CustomAuthorizationStopResponseParser))
            {
                return authorizationStopResponse;
            }

            throw new ArgumentException("The given JSON representation of a AuthorizationStop response is invalid: " + ErrorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, CustomAuthorizationStopResponseParser = null)

        /// <summary>
        /// Parse the given text representation of a AuthorizationStop response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="Text">The text to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomAuthorizationStopResponseParser">A delegate to parse custom AuthorizationStop response JSON objects.</param>
        public static AuthorizationStopResponse Parse(AuthorizeStopRequest                                    Request,
                                                      String                                                  Text,
                                                      DateTime?                                               ResponseTimestamp                       = null,
                                                      EventTracking_Id                                        EventTrackingId                         = null,
                                                      TimeSpan?                                               Runtime                                 = null,
                                                      Process_Id?                                             ProcessId                               = null,
                                                      HTTPResponse                                            HTTPResponse                            = null,
                                                      CustomJObjectParserDelegate<AuthorizationStopResponse>  CustomAuthorizationStopResponseParser   = null)
        {

            if (TryParse(Request,
                         Text,
                         out AuthorizationStopResponse  authorizationStopResponse,
                         out String                     ErrorResponse,
                         ResponseTimestamp,
                         EventTrackingId,
                         Runtime,
                         ProcessId,
                         HTTPResponse,
                         CustomAuthorizationStopResponseParser))
            {
                return authorizationStopResponse;
            }

            throw new ArgumentException("The given text representation of a AuthorizationStop response is invalid: " + ErrorResponse, nameof(Text));

        }

        #endregion

        #region (static) TryParse(JSON, out AuthorizationStopResponse, out ErrorResponse, CustomAuthorizationStopResponseParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a AuthorizationStop response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="AuthorizationStopResponse">The parsed AuthorizationStop response.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomAuthorizationStopResponseParser">A delegate to parse custom AuthorizationStop response JSON objects.</param>
        public static Boolean TryParse(AuthorizeStopRequest                                    Request,
                                       JObject                                                 JSON,
                                       out AuthorizationStopResponse                           AuthorizationStopResponse,
                                       out String                                              ErrorResponse,
                                       DateTime?                                               ResponseTimestamp                       = null,
                                       EventTracking_Id                                        EventTrackingId                         = null,
                                       TimeSpan?                                               Runtime                                 = null,
                                       Process_Id?                                             ProcessId                               = null,
                                       HTTPResponse                                            HTTPResponse                            = null,
                                       CustomJObjectParserDelegate<AuthorizationStopResponse>  CustomAuthorizationStopResponseParser   = null)
        {

            try
            {

                AuthorizationStopResponse = default;

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

                #region Parse CustomData                            [optional]

                var CustomData = JSON["CustomData"] as JObject;

                #endregion


                AuthorizationStopResponse  = new AuthorizationStopResponse(Request,
                                                                           ResponseTimestamp ?? DateTime.UtcNow,
                                                                           EventTrackingId   ?? Request.EventTrackingId,
                                                                           Runtime           ?? DateTime.UtcNow - Request.Timestamp,
                                                                           AuthorizationStatus,
                                                                           StatusCode,
                                                                           SessionId,
                                                                           CPOPartnerSessionId,
                                                                           EMPPartnerSessionId,
                                                                           ProviderId,
                                                                           ProcessId,
                                                                           HTTPResponse,
                                                                           CustomData);

                if (CustomAuthorizationStopResponseParser != null)
                    AuthorizationStopResponse = CustomAuthorizationStopResponseParser(JSON,
                                                                                      AuthorizationStopResponse);

                return true;

            }
            catch (Exception e)
            {
                AuthorizationStopResponse  = default;
                ErrorResponse              = "The given JSON representation of a AuthorizationStop response is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Text, out AuthorizationStopResponse, out ErrorResponse, CustomAuthorizationStopResponseParser = null)

        /// <summary>
        /// Try to parse the given text representation of a AuthorizationStop response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="Text">The text to parse.</param>
        /// <param name="AuthorizationStopResponse">The parsed AuthorizationStop response.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomAuthorizationStopResponseParser">A delegate to parse custom AuthorizationStop response JSON objects.</param>
        public static Boolean TryParse(AuthorizeStopRequest                                    Request,
                                       String                                                  Text,
                                       out AuthorizationStopResponse                           AuthorizationStopResponse,
                                       out String                                              ErrorResponse,
                                       DateTime?                                               ResponseTimestamp                       = null,
                                       EventTracking_Id                                        EventTrackingId                         = null,
                                       TimeSpan?                                               Runtime                                 = null,
                                       Process_Id?                                             ProcessId                               = null,
                                       HTTPResponse                                            HTTPResponse                            = null,
                                       CustomJObjectParserDelegate<AuthorizationStopResponse>  CustomAuthorizationStopResponseParser   = null)
        {

            try
            {

                return TryParse(Request,
                                JObject.Parse(Text),
                                out AuthorizationStopResponse,
                                out ErrorResponse,
                                ResponseTimestamp,
                                EventTrackingId,
                                Runtime,
                                ProcessId,
                                HTTPResponse,
                                CustomAuthorizationStopResponseParser);

            }
            catch (Exception e)
            {
                AuthorizationStopResponse  = default;
                ErrorResponse              = "The given text representation of a AuthorizationStop response is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomAuthorizationStopSerializer = null, CustomStatusCodeSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomAuthorizationStopSerializer">A delegate to customize the serialization of AuthorizationStop respones.</param>
        /// <param name="CustomStatusCodeSerializer">A delegate to serialize custom StatusCode JSON objects.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<AuthorizationStopResponse>  CustomAuthorizationStopSerializer   = null,
                              CustomJObjectSerializerDelegate<StatusCode>                  CustomStatusCodeSerializer           = null,
                              CustomJObjectSerializerDelegate<Identification>              CustomIdentificationSerializer       = null)

        {

            var JSON = JSONObject.Create(

                           new JProperty("AuthorizationStatus",        AuthorizationStatus.ToString()),

                           new JProperty("StatusCode",                 StatusCode.         ToJSON(CustomStatusCodeSerializer)),

                           ProviderId.HasValue
                               ? new JProperty("ProviderID",           ProviderId.         ToString())
                               : null,

                           SessionId.HasValue
                               ? new JProperty("SessionID",            SessionId.          ToString())
                               : null,

                           CPOPartnerSessionId.HasValue
                               ? new JProperty("CPOPartnerSessionID",  CPOPartnerSessionId.ToString())
                               : null,

                           EMPPartnerSessionId.HasValue
                               ? new JProperty("EMPPartnerSessionID",  EMPPartnerSessionId.ToString())
                               : null,

                           CustomData != null
                               ? new JProperty("CustomData",           CustomData)
                               : null

                       );

            return CustomAuthorizationStopSerializer != null
                       ? CustomAuthorizationStopSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region Operator overloading

        #region Operator == (AuthorizationStop1, AuthorizationStop2)

        /// <summary>
        /// Compares two AuthorizationStop requests for equality.
        /// </summary>
        /// <param name="AuthorizationStop1">An authorize stop request.</param>
        /// <param name="AuthorizationStop2">Another authorize stop request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (AuthorizationStopResponse AuthorizationStop1,
                                           AuthorizationStopResponse AuthorizationStop2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(AuthorizationStop1, AuthorizationStop2))
                return true;

            // If one is null, but not both, return false.
            if (AuthorizationStop1 is null || AuthorizationStop2 is null)
                return false;

            return AuthorizationStop1.Equals(AuthorizationStop2);

        }

        #endregion

        #region Operator != (AuthorizationStop1, AuthorizationStop2)

        /// <summary>
        /// Compares two AuthorizationStop requests for inequality.
        /// </summary>
        /// <param name="AuthorizationStop1">An authorize stop request.</param>
        /// <param name="AuthorizationStop2">Another authorize stop request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (AuthorizationStopResponse AuthorizationStop1,
                                           AuthorizationStopResponse AuthorizationStop2)

            => !(AuthorizationStop1 == AuthorizationStop2);

        #endregion

        #endregion

        #region IEquatable<AuthorizationStop> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is AuthorizationStopResponse authorizationStopResponse &&
                   Equals(authorizationStopResponse);

        #endregion

        #region Equals(AuthorizationStop)

        /// <summary>
        /// Compares two authorize stop requests for equality.
        /// </summary>
        /// <param name="AuthorizationStop">An authorize stop request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(AuthorizationStopResponse AuthorizationStop)
        {

            if (AuthorizationStop is null)
                return false;

            return AuthorizationStatus.Equals(AuthorizationStop.AuthorizationStatus) &&
                   StatusCode.         Equals(AuthorizationStop.StatusCode)          &&
                   //AuthorizationStopIdentifications

                   ((!SessionId.          HasValue && !AuthorizationStop.SessionId.          HasValue) ||
                     (SessionId.          HasValue &&  AuthorizationStop.SessionId.          HasValue && SessionId.          Value.Equals(AuthorizationStop.SessionId.          Value))) &&

                   ((!CPOPartnerSessionId.HasValue && !AuthorizationStop.CPOPartnerSessionId.HasValue) ||
                     (CPOPartnerSessionId.HasValue &&  AuthorizationStop.CPOPartnerSessionId.HasValue && CPOPartnerSessionId.Value.Equals(AuthorizationStop.CPOPartnerSessionId.Value))) &&

                   ((!EMPPartnerSessionId.HasValue && !AuthorizationStop.EMPPartnerSessionId.HasValue) ||
                     (EMPPartnerSessionId.HasValue &&  AuthorizationStop.EMPPartnerSessionId.HasValue && EMPPartnerSessionId.Value.Equals(AuthorizationStop.EMPPartnerSessionId.Value))) &&

                   ((!ProviderId.         HasValue && !AuthorizationStop.ProviderId.         HasValue) ||
                     (ProviderId.         HasValue &&  AuthorizationStop.ProviderId.         HasValue && ProviderId.         Value.Equals(AuthorizationStop.ProviderId.         Value)));

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
                           ProcessId,
                           HTTPResponse,
                           CustomData);

        #endregion

        #region (class) Builder

        /// <summary>
        /// The AuthorizationStop response builder.
        /// </summary>
        public new class Builder : AResponse<AuthorizeStopRequest,
                                             AuthorizationStopResponse>.Builder
        {

            #region Properties

            /// <summary>
            /// The authorization status, e.g. "Authorized".
            /// </summary>
            [Mandatory]
            public AuthorizationStatusTypes?  AuthorizationStatus    { get; set; }

            /// <summary>
            /// The authorization status code.
            /// </summary>
            [Mandatory]
            public StatusCode                 StatusCode             { get; set; }

            /// <summary>
            /// The e-mobility provider identification.
            /// </summary>
            [Optional]
            public Provider_Id?               ProviderId             { get; set; }

            /// <summary>
            /// The charging session identification.
            /// </summary>
            [Optional]
            public Session_Id?                SessionId              { get; set; }

            /// <summary>
            /// An optional partner charging session identification.
            /// </summary>
            [Optional]
            public CPOPartnerSession_Id?      CPOPartnerSessionId    { get; set; }

            /// <summary>
            /// An optional partner charging session identification.
            /// </summary>
            [Optional]
            public EMPPartnerSession_Id?      EMPPartnerSessionId    { get; set; }

            #endregion

            #region Constructor(s)

            /// <summary>
            /// Create a new AuthorizationStop response builder.
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
            /// <param name="ProcessId">The optional Hubject process identification of the request.</param>
            /// <param name="HTTPResponse">The optional HTTP response.</param>
            /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
            public Builder(AuthorizeStopRequest       Request               = null,
                           DateTime?                  ResponseTimestamp     = null,
                           EventTracking_Id           EventTrackingId       = null,
                           TimeSpan?                  Runtime               = null,
                           AuthorizationStatusTypes?  AuthorizationStatus   = null,
                           StatusCode                 StatusCode            = null,
                           Session_Id?                SessionId             = null,
                           CPOPartnerSession_Id?      CPOPartnerSessionId   = null,
                           EMPPartnerSession_Id?      EMPPartnerSessionId   = null,
                           Provider_Id?               ProviderId            = null,
                           Process_Id?                ProcessId             = null,
                           HTTPResponse               HTTPResponse          = null,
                           JObject                    CustomData            = null)

                : base(ResponseTimestamp,
                       EventTrackingId,
                       Runtime,
                       Request,
                       HTTPResponse,
                       ProcessId,
                       CustomData)

            {

                this.AuthorizationStatus  = AuthorizationStatus;
                this.StatusCode           = StatusCode;
                this.SessionId            = SessionId;
                this.CPOPartnerSessionId  = CPOPartnerSessionId;
                this.EMPPartnerSessionId  = EMPPartnerSessionId;
                this.ProviderId           = ProviderId;

            }

            #endregion

            #region ToImmutable()

            /// <summary>
            /// Return an immutable version of the AuthorizationStop response.
            /// </summary>
            /// <param name="Builder">A EVSE data record builder.</param>
            public static implicit operator AuthorizationStopResponse(Builder Builder)

                => Builder?.ToImmutable();


            /// <summary>
            /// Return an immutable version of the AuthorizationStop response.
            /// </summary>
            public override AuthorizationStopResponse ToImmutable()
            {

                #region Check mandatory parameters

                if (!AuthorizationStatus.HasValue)
                    throw new ArgumentException("The given authorization status must not be null!", nameof(AuthorizationStatus));

                #endregion

                return new AuthorizationStopResponse(Request           ?? throw new ArgumentNullException(nameof(Request), "The given request must not be null!"),
                                                     ResponseTimestamp ?? DateTime.UtcNow,
                                                     EventTrackingId   ?? EventTracking_Id.New,
                                                     Runtime           ?? (DateTime.UtcNow - Request.Timestamp),
                                                     AuthorizationStatus.Value,
                                                     StatusCode,
                                                     SessionId,
                                                     CPOPartnerSessionId,
                                                     EMPPartnerSessionId,
                                                     ProviderId,
                                                     ProcessId,
                                                     HTTPResponse,
                                                     CustomData);

            }

            #endregion

        }

        #endregion

    }

}
