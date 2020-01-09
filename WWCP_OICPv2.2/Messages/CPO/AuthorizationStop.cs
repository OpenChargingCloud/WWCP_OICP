/*
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
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2.CPO
{

    /// <summary>
    /// An OICP Authorization Stop result.
    /// </summary>
    public class AuthorizationStop : AResponse<AuthorizeStopRequest,
                                               AuthorizationStop>
    {

        #region Properties

        /// <summary>
        /// The charging session identification.
        /// </summary>
        public Session_Id?               SessionId              { get; }

        /// <summary>
        /// An optional CPO partner session identification.
        /// </summary>
        public CPOPartnerSession_Id?     CPOPartnerSessionId    { get; }

        /// <summary>
        /// An optional EMP partner session identification.
        /// </summary>
        public EMPPartnerSession_Id?     EMPPartnerSessionId    { get; }

        /// <summary>
        /// The e-mobility provider identification.
        /// </summary>
        public Provider_Id?              ProviderId             { get; }

        /// <summary>
        /// The authorization status, e.g. "Authorized".
        /// </summary>
        public AuthorizationStatusTypes  AuthorizationStatus    { get; }

        /// <summary>
        /// The authorization status code.
        /// </summary>
        public StatusCode                StatusCode             { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OICP Authorization Stop result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="AuthorizationStatus">The authorization status.</param>
        /// <param name="StatusCode">An optional status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        /// <param name="CustomData">Optional custom data.</param>
        private AuthorizationStop(AuthorizeStopRequest                 Request,
                                  AuthorizationStatusTypes             AuthorizationStatus,
                                  StatusCode                           StatusCode,
                                  Session_Id?                          SessionId             = null,
                                  CPOPartnerSession_Id?                CPOPartnerSessionId   = null,
                                  EMPPartnerSession_Id?                EMPPartnerSessionId   = null,
                                  Provider_Id?                         ProviderId            = null,
                                  IReadOnlyDictionary<String, Object>  CustomData            = null)

            : base(Request,
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


        #region (static) Authorized               (Request, SessionId = null, PartnerSessionId = null, ProviderId = null, ...)

        /// <summary>
        /// Create a new OICP 'Authorized' AuthorizationStop result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="SessionId">The charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        public static AuthorizationStop Authorized(AuthorizeStopRequest   Request,
                                                   Session_Id?            SessionId                  = null,
                                                   CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                   EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                   Provider_Id?           ProviderId                 = null,
                                                   String                 StatusCodeDescription      = null,
                                                   String                 StatusCodeAdditionalInfo   = null)


            => new AuthorizationStop(Request,
                                     AuthorizationStatusTypes.Authorized,
                                     new StatusCode(
                                         StatusCodes.Success,
                                         StatusCodeDescription,
                                         StatusCodeAdditionalInfo
                                     ),
                                     SessionId,
                                     CPOPartnerSessionId,
                                     EMPPartnerSessionId,
                                     ProviderId);

        #endregion

        #region (static) NotAuthorized            (Request, StatusCode, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new OICP 'NotAuthorized' AuthorizationStop result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCode">The status code of the operation.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStop NotAuthorized(AuthorizeStopRequest   Request,
                                                      StatusCodes            StatusCode,
                                                      String                 StatusCodeDescription      = null,
                                                      String                 StatusCodeAdditionalInfo   = null,
                                                      Session_Id?            SessionId                  = null,
                                                      CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                      EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                      Provider_Id?           ProviderId                 = null)

            => new AuthorizationStop(Request,
                                     AuthorizationStatusTypes.NotAuthorized,
                                     new StatusCode(
                                         StatusCode,
                                         StatusCodeDescription,
                                         StatusCodeAdditionalInfo
                                     ),
                                     SessionId,
                                     CPOPartnerSessionId,
                                     EMPPartnerSessionId,
                                     ProviderId);

        #endregion

        #region (static) SessionIsInvalid         (Request, StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'SessionIsInvalid' AuthorizationStop result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStop SessionIsInvalid(AuthorizeStopRequest   Request,
                                                         String                 StatusCodeDescription      = null,
                                                         String                 StatusCodeAdditionalInfo   = null,
                                                         Session_Id?            SessionId                  = null,
                                                         CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                         EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                         Provider_Id?           ProviderId                 = null)

            => new AuthorizationStop(Request,
                                     AuthorizationStatusTypes.NotAuthorized,
                                     new StatusCode(
                                         StatusCodes.SessionIsInvalid,
                                         StatusCodeDescription ?? "Session is invalid",
                                         StatusCodeAdditionalInfo
                                     ),
                                     SessionId,
                                     CPOPartnerSessionId,
                                     EMPPartnerSessionId,
                                     ProviderId);

        #endregion

        #region (static) CommunicationToEVSEFailed(Request, StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'CommunicationToEVSEFailed' AuthorizationStop result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStop CommunicationToEVSEFailed(AuthorizeStopRequest   Request,
                                                                  String                 StatusCodeDescription      = null,
                                                                  String                 StatusCodeAdditionalInfo   = null,
                                                                  Session_Id?            SessionId                  = null,
                                                                  CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                                  EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                                  Provider_Id?           ProviderId                 = null)

            => new AuthorizationStop(Request,
                                     AuthorizationStatusTypes.NotAuthorized,
                                     new StatusCode(
                                         StatusCodes.CommunicationToEVSEFailed,
                                         StatusCodeDescription ?? "Communication to EVSE failed!",
                                         StatusCodeAdditionalInfo
                                     ),
                                     SessionId,
                                     CPOPartnerSessionId,
                                     EMPPartnerSessionId,
                                     ProviderId);

        #endregion

        #region (static) NoEVConnectedToEVSE      (Request, StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'NoEVConnectedToEVSE' AuthorizationStop result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStop NoEVConnectedToEVSE(AuthorizeStopRequest   Request,
                                                            String                 StatusCodeDescription      = null,
                                                            String                 StatusCodeAdditionalInfo   = null,
                                                            Session_Id?            SessionId                  = null,
                                                            CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                            EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                            Provider_Id?           ProviderId                 = null)

            => new AuthorizationStop(Request,
                                     AuthorizationStatusTypes.NotAuthorized,
                                     new StatusCode(
                                         StatusCodes.NoEVConnectedToEVSE,
                                         StatusCodeDescription ?? "No EV connected to EVSE!",
                                         StatusCodeAdditionalInfo
                                     ),
                                     SessionId,
                                     CPOPartnerSessionId,
                                     EMPPartnerSessionId,
                                     ProviderId);

        #endregion

        #region (static) EVSEAlreadyReserved      (Request, StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'EVSEAlreadyReserved' AuthorizationStop result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStop EVSEAlreadyReserved(AuthorizeStopRequest   Request,
                                                            String                 StatusCodeDescription      = null,
                                                            String                 StatusCodeAdditionalInfo   = null,
                                                            Session_Id?            SessionId                  = null,
                                                            CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                            EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                            Provider_Id?           ProviderId                 = null)

            => new AuthorizationStop(Request,
                                     AuthorizationStatusTypes.NotAuthorized,
                                     new StatusCode(
                                         StatusCodes.EVSEAlreadyReserved,
                                         StatusCodeDescription ?? "EVSE already reserved!",
                                         StatusCodeAdditionalInfo
                                     ),
                                     SessionId,
                                     CPOPartnerSessionId,
                                     EMPPartnerSessionId,
                                     ProviderId);

        #endregion

        #region (static) UnknownEVSEID            (Request, StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'UnknownEVSEID' AuthorizationStop result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStop UnknownEVSEID(AuthorizeStopRequest   Request,
                                                      String                 StatusCodeDescription      = null,
                                                      String                 StatusCodeAdditionalInfo   = null,
                                                      Session_Id?            SessionId                  = null,
                                                      CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                      EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                      Provider_Id?           ProviderId                 = null)

            => new AuthorizationStop(Request,
                                     AuthorizationStatusTypes.NotAuthorized,
                                     new StatusCode(
                                         StatusCodes.UnknownEVSEID,
                                         StatusCodeDescription ?? "Unknown EVSE ID!",
                                         StatusCodeAdditionalInfo
                                     ),
                                     SessionId,
                                     CPOPartnerSessionId,
                                     EMPPartnerSessionId,
                                     ProviderId);

        #endregion

        #region (static) EVSEOutOfService         (Request, StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'EVSEOutOfService' AuthorizationStop result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStop EVSEOutOfService(AuthorizeStopRequest   Request,
                                                         String                 StatusCodeDescription      = null,
                                                         String                 StatusCodeAdditionalInfo   = null,
                                                         Session_Id?            SessionId                  = null,
                                                         CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                         EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                         Provider_Id?           ProviderId                 = null)

            => new AuthorizationStop(Request,
                                     AuthorizationStatusTypes.NotAuthorized,
                                     new StatusCode(
                                         StatusCodes.EVSEOutOfService,
                                         StatusCodeDescription ?? "EVSE out of service!",
                                         StatusCodeAdditionalInfo
                                     ),
                                     SessionId,
                                     CPOPartnerSessionId,
                                     EMPPartnerSessionId,
                                     ProviderId);

        #endregion

        #region (static) ServiceNotAvailable      (Request, StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'ServiceNotAvailable' AuthorizationStop result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStop ServiceNotAvailable(AuthorizeStopRequest   Request,
                                                            String                 StatusCodeDescription      = null,
                                                            String                 StatusCodeAdditionalInfo   = null,
                                                            Session_Id?            SessionId                  = null,
                                                            CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                            EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                            Provider_Id?           ProviderId                 = null)

            => new AuthorizationStop(Request,
                                     AuthorizationStatusTypes.NotAuthorized,
                                     new StatusCode(
                                         StatusCodes.ServiceNotAvailable,
                                         StatusCodeDescription ?? "Service not available!",
                                         StatusCodeAdditionalInfo
                                     ),
                                     SessionId,
                                     CPOPartnerSessionId,
                                     EMPPartnerSessionId,
                                     ProviderId);

        #endregion

        #region (static) DataError                (StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'DataError' AuthorizationStop result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStop DataError(AuthorizeStopRequest   Request,
                                                  String                 StatusCodeDescription      = null,
                                                  String                 StatusCodeAdditionalInfo   = null,
                                                  Session_Id?            SessionId                  = null,
                                                  CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                  EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                  Provider_Id?           ProviderId                 = null)

            => new AuthorizationStop(Request,
                                     AuthorizationStatusTypes.NotAuthorized,
                                     new StatusCode(
                                         StatusCodes.DataError,
                                         StatusCodeDescription ?? "Data Error!",
                                         StatusCodeAdditionalInfo
                                     ),
                                     SessionId,
                                     CPOPartnerSessionId,
                                     EMPPartnerSessionId,
                                     ProviderId);

        #endregion

        #region (static) SystemError              (StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'SystemError' AuthorizationStop result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStop SystemError(AuthorizeStopRequest   Request,
                                                    String                 StatusCodeDescription      = null,
                                                    String                 StatusCodeAdditionalInfo   = null,
                                                    Session_Id?            SessionId                  = null,
                                                    CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                    EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                    Provider_Id?           ProviderId                 = null)

            => new AuthorizationStop(Request,
                                     AuthorizationStatusTypes.NotAuthorized,
                                     new StatusCode(
                                         StatusCodes.SystemError,
                                         StatusCodeDescription ?? "System Error!",
                                         StatusCodeAdditionalInfo
                                     ),
                                     SessionId,
                                     CPOPartnerSessionId,
                                     EMPPartnerSessionId,
                                     ProviderId);

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv        = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:Authorization  = "http://www.hubject.com/b2b/services/authorization/v2.0"
        //                   xmlns:CommonTypes    = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        //
        //    <soapenv:Header/>
        //
        //    <soapenv:Body>
        //       <Authorization:eRoamingAuthorizationStop>
        //
        //          <!--Optional:-->
        //          <Authorization:SessionID>de164e08-1c88-1293-537b-be355041070e</Authorization:SessionID>
        //
        //          <!--Optional:-->
        //          <Authorization:CPOPartnerSessionID>?</Authorization:CPOPartnerSessionID>
        //
        //          <!--Optional:-->
        //          <Authorization:EMPPartnerSessionID>?</Authorization:EMPPartnerSessionID>
        //
        //          <!--Optional:-->
        //          <Authorization:ProviderID>DE*GDF</Authorization:ProviderID>
        //
        //          <Authorization:AuthorizationStatus>Authorized|NotAuthorized</Authorization:AuthorizationStatus>
        //
        //          <Authorization:StatusCode>
        //
        //             <CommonTypes:Code>?</CommonTypes:Code>
        //
        //             <!--Optional:-->
        //             <CommonTypes:Description>?</CommonTypes:Description>
        //
        //             <!--Optional:-->
        //             <CommonTypes:AdditionalInfo>?</CommonTypes:AdditionalInfo>
        //
        //          </Authorization:StatusCode>
        //
        //       </Authorization:eRoamingAuthorizationStop>
        //    </soapenv:Body>
        //
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (Request, AuthorizationStopXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP authorization stop request.
        /// </summary>
        /// <param name="Request">An AuthorizeStopRequest request.</param>
        /// <param name="AuthorizationStopXML">The XML to parse.</param>
        /// <param name="CustomAuthorizationStopParser">A delegate to parse custom AuthorizationStop XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AuthorizationStop Parse(AuthorizeStopRequest                        Request,
                                              XElement                                    AuthorizationStopXML,
                                              CustomXMLParserDelegate<AuthorizationStop>  CustomAuthorizationStopParser   = null,
                                              CustomXMLParserDelegate<StatusCode>         CustomStatusCodeParser          = null,
                                              OnExceptionDelegate                         OnException                     = null)
        {

            if (TryParse(Request,
                         AuthorizationStopXML,
                         out AuthorizationStop _AuthorizationStop,
                         CustomAuthorizationStopParser,
                         CustomStatusCodeParser,
                         OnException))

                return _AuthorizationStop;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, AuthorizationStopText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP authorization stop request.
        /// </summary>
        /// <param name="Request">An AuthorizeStopRequest request.</param>
        /// <param name="AuthorizationStopText">The text to parse.</param>
        /// <param name="CustomAuthorizationStopParser">A delegate to parse custom AuthorizationStop XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AuthorizationStop Parse(AuthorizeStopRequest                        Request,
                                              String                                      AuthorizationStopText,
                                              CustomXMLParserDelegate<AuthorizationStop>  CustomAuthorizationStopParser   = null,
                                              CustomXMLParserDelegate<StatusCode>         CustomStatusCodeParser          = null,
                                              OnExceptionDelegate                         OnException                     = null)
        {

            if (TryParse(Request,
                         AuthorizationStopText,
                         out AuthorizationStop _AuthorizationStop,
                         CustomAuthorizationStopParser,
                         CustomStatusCodeParser,
                         OnException))

                return _AuthorizationStop;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, AuthorizationStopXML,  out AuthorizationStop, ..., OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP authorization stop request.
        /// </summary>
        /// <param name="Request">An AuthorizeStopRequest request.</param>
        /// <param name="AuthorizationStopXML">The XML to parse.</param>
        /// <param name="AuthorizationStop">The parsed AuthorizationStop request.</param>
        /// <param name="CustomAuthorizationStopParser">A delegate to parse custom AuthorizationStop XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(AuthorizeStopRequest                        Request,
                                       XElement                                    AuthorizationStopXML,
                                       out AuthorizationStop                       AuthorizationStop,
                                       CustomXMLParserDelegate<AuthorizationStop>  CustomAuthorizationStopParser   = null,
                                       CustomXMLParserDelegate<StatusCode>         CustomStatusCodeParser          = null,
                                       OnExceptionDelegate                         OnException                     = null)
        {

            try
            {

                if (AuthorizationStopXML.Name != OICPNS.Authorization + "eRoamingAuthorizationStop")
                {
                    AuthorizationStop = null;
                    return false;
                }

                AuthorizationStop = new AuthorizationStop(

                                        Request,

                                        AuthorizationStopXML.MapValueOrFail    (OICPNS.Authorization + "AuthorizationStatus",
                                                                                s => (AuthorizationStatusTypes) Enum.Parse(typeof(AuthorizationStatusTypes), s)),

                                        AuthorizationStopXML.MapElement        (OICPNS.Authorization + "StatusCode",
                                                                                (xml, e) => StatusCode.Parse(xml,
                                                                                                             CustomStatusCodeParser,
                                                                                                             e)),

                                        AuthorizationStopXML.MapValueOrNullable(OICPNS.Authorization + "SessionID",
                                                                                Session_Id.       Parse),

                                        AuthorizationStopXML.MapValueOrNullable(OICPNS.Authorization + "CPOPartnerSessionID",
                                                                                CPOPartnerSession_Id.Parse),

                                        AuthorizationStopXML.MapValueOrNullable(OICPNS.Authorization + "EMPPartnerSessionID",
                                                                                EMPPartnerSession_Id.Parse),

                                        AuthorizationStopXML.MapValueOrNullable(OICPNS.Authorization + "ProviderID",
                                                                                Provider_Id.      Parse)

                                    );


                if (CustomAuthorizationStopParser != null)
                    AuthorizationStop = CustomAuthorizationStopParser(AuthorizationStopXML,
                                                                      AuthorizationStop);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, AuthorizationStopXML, e);

                AuthorizationStop = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, AuthorizationStopText, out AuthorizationStop, ..., OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP authorization stop request.
        /// </summary>
        /// <param name="Request">An AuthorizeStopRequest request.</param>
        /// <param name="AuthorizationStopText">The text to parse.</param>
        /// <param name="AuthorizationStop">The parsed authorization stop request.</param>
        /// <param name="CustomAuthorizationStopParser">A delegate to parse custom AuthorizationStop XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(AuthorizeStopRequest                        Request,
                                       String                                      AuthorizationStopText,
                                       out AuthorizationStop                       AuthorizationStop,
                                       CustomXMLParserDelegate<AuthorizationStop>  CustomAuthorizationStopParser   = null,
                                       CustomXMLParserDelegate<StatusCode>         CustomStatusCodeParser          = null,
                                       OnExceptionDelegate                         OnException                     = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(AuthorizationStopText).Root,
                             out AuthorizationStop,
                             CustomAuthorizationStopParser,
                             CustomStatusCodeParser,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, AuthorizationStopText, e);
            }

            AuthorizationStop = null;
            return false;

        }

        #endregion

        #region ToXML(CustomAuthorizationStopSerializer = null, CustomStatusCodeSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomAuthorizationStopSerializer">A delegate to customize the serialization of AuthorizationStop respones.</param>
        /// <param name="CustomStatusCodeSerializer">A delegate to serialize custom StatusCode XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<AuthorizationStop>  CustomAuthorizationStopSerializer   = null,
                              CustomXMLSerializerDelegate<StatusCode>         CustomStatusCodeSerializer          = null)

        {

            var XML = new XElement(OICPNS.Authorization + "eRoamingAuthorizationStop",

                          SessionId != null
                              ? new XElement(OICPNS.Authorization + "SessionID",            SessionId.          ToString())
                              : null,

                          CPOPartnerSessionId.HasValue
                              ? new XElement(OICPNS.Authorization + "CPOPartnerSessionID",  CPOPartnerSessionId.ToString())
                              : null,

                          EMPPartnerSessionId.HasValue
                              ? new XElement(OICPNS.Authorization + "EMPPartnerSessionID",  EMPPartnerSessionId.ToString())
                              : null,

                          ProviderId != null
                              ? new XElement(OICPNS.Authorization + "ProviderID",           ProviderId.         ToString())
                              : null,

                          new XElement(OICPNS.Authorization + "AuthorizationStatus",    AuthorizationStatus.ToString()),

                          StatusCode.ToXML(OICPNS.Authorization + "StatusCode",
                                           CustomStatusCodeSerializer)

                      );

            return CustomAuthorizationStopSerializer != null
                       ? CustomAuthorizationStopSerializer(this, XML)
                       : XML;

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
        public static Boolean operator == (AuthorizationStop AuthorizationStop1, AuthorizationStop AuthorizationStop2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(AuthorizationStop1, AuthorizationStop2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) AuthorizationStop1 == null) || ((Object) AuthorizationStop2 == null))
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
        public static Boolean operator != (AuthorizationStop AuthorizationStop1, AuthorizationStop AuthorizationStop2)
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
        {

            if (Object == null)
                return false;

            if (!(Object is AuthorizationStop AuthorizationStop))
                return false;

            return Equals(AuthorizationStop);

        }

        #endregion

        #region Equals(AuthorizationStop)

        /// <summary>
        /// Compares two authorize stop requests for equality.
        /// </summary>
        /// <param name="AuthorizationStop">An authorize stop request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(AuthorizationStop AuthorizationStop)
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

                return AuthorizationStatus.GetHashCode() * 13 ^
                       StatusCode.         GetHashCode() * 11 ^

                       (SessionId != null
                            ? SessionId.          GetHashCode() * 7
                            : 0) ^

                       (CPOPartnerSessionId != null
                            ? CPOPartnerSessionId.GetHashCode() * 5
                            : 0) ^

                       (EMPPartnerSessionId != null
                            ? EMPPartnerSessionId.GetHashCode() * 3
                            : 0) ^

                       (ProviderId != null
                            ? ProviderId.         GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(AuthorizationStatus,

                             StatusCode != null
                                 ? "; " + StatusCode.Code +

                                       (StatusCode.Description.IsNotNullOrEmpty()
                                            ? " / " + StatusCode.Description
                                            : "") +

                                       (StatusCode.AdditionalInfo.IsNotNullOrEmpty()
                                            ? " / " + StatusCode.AdditionalInfo
                                            : "")
                                 : "");

        #endregion



        #region ToBuilder

        /// <summary>
        /// Return a response builder.
        /// </summary>
        public Builder ToBuilder
            => new Builder(this);

        #endregion

        #region (class) Builder

        /// <summary>
        /// An AuthorizationStop response builder.
        /// </summary>
        public class Builder : AResponseBuilder<AuthorizeStopRequest,
                                                AuthorizationStop>
        {

            #region Properties

            /// <summary>
            /// The charging session identification.
            /// </summary>
            public Session_Id?                 SessionId              { get; set; }

            /// <summary>
            /// An optional EMP partner charging session identification.
            /// </summary>
            public CPOPartnerSession_Id?       CPOPartnerSessionId    { get; set; }

            /// <summary>
            /// An optional CPO partner charging session identification.
            /// </summary>
            public EMPPartnerSession_Id?       EMPPartnerSessionId    { get; set; }

            /// <summary>
            /// The e-mobility provider identification.
            /// </summary>
            public Provider_Id?                ProviderId             { get; set; }

            /// <summary>
            /// The authorization status, e.g. "Authorized".
            /// </summary>
            public AuthorizationStatusTypes    AuthorizationStatus    { get; set; }

            /// <summary>
            /// The authorization status code.
            /// </summary>
            public StatusCode                  StatusCode             { get; set; }

            #endregion

            #region Constructor(s)

            #region Builder(Request,           CustomData = null)

            /// <summary>
            /// Create a new AuthorizationStop response builder.
            /// </summary>
            /// <param name="Request">The request leading to this response.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(AuthorizeStopRequest                 Request,
                           IReadOnlyDictionary<String, Object>  CustomData  = null)

                : base(Request,
                       CustomData)

            { }

            #endregion

            #region Builder(AuthorizationStop, CustomData = null)

            /// <summary>
            /// Create a new AuthorizationStop response builder.
            /// </summary>
            /// <param name="AuthorizationStop">An AuthorizeStop response.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(AuthorizationStop                    AuthorizationStop,
                           IReadOnlyDictionary<String, Object>  CustomData  = null)

                : base(AuthorizationStop?.Request,
                       AuthorizationStop.HasCustomData
                           ? CustomData != null && CustomData.Any()
                                 ? AuthorizationStop.CustomData.Concat(CustomData)
                                 : AuthorizationStop.CustomData
                           : CustomData)

            {

                if (AuthorizationStop != null)
                {

                    this.AuthorizationStatus  = AuthorizationStop.AuthorizationStatus;
                    this.StatusCode           = AuthorizationStop.StatusCode;
                    this.SessionId            = AuthorizationStop.SessionId;
                    this.CPOPartnerSessionId  = AuthorizationStop.CPOPartnerSessionId;
                    this.EMPPartnerSessionId  = AuthorizationStop.EMPPartnerSessionId;
                    this.ProviderId           = AuthorizationStop.ProviderId;

                }

            }

            #endregion

            #endregion


            #region Operator overloading

            #region Operator == (AuthorizationStop1, AuthorizationStop2)

            /// <summary>
            /// Compares two AuthorizationStop requests for equality.
            /// </summary>
            /// <param name="AuthorizationStop1">An authorize stop request.</param>
            /// <param name="AuthorizationStop2">Another authorize stop request.</param>
            /// <returns>True if both match; False otherwise.</returns>
            public static Boolean operator == (Builder AuthorizationStop1, Builder AuthorizationStop2)
            {

                // If both are null, or both are same instance, return true.
                if (Object.ReferenceEquals(AuthorizationStop1, AuthorizationStop2))
                    return true;

                // If one is null, but not both, return false.
                if (((Object) AuthorizationStop1 == null) || ((Object) AuthorizationStop2 == null))
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
            public static Boolean operator != (Builder AuthorizationStop1, Builder AuthorizationStop2)
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
            {

                if (Object == null)
                    return false;

                if (!(Object is Builder AuthorizationStop))
                    return false;

                return Equals(AuthorizationStop);

            }

            #endregion

            #region Equals(AuthorizationStop)

            /// <summary>
            /// Compares two authorize stop requests for equality.
            /// </summary>
            /// <param name="AuthorizationStop">An authorize stop request to compare with.</param>
            /// <returns>True if both match; False otherwise.</returns>
            public override Boolean Equals(AuthorizationStop AuthorizationStop)
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

                    return AuthorizationStatus.GetHashCode() * 13 ^
                           StatusCode.         GetHashCode() * 11 ^

                           (SessionId != null
                                ? SessionId.          GetHashCode() * 7
                                : 0) ^

                           (CPOPartnerSessionId != null
                                ? CPOPartnerSessionId.GetHashCode() * 5
                                : 0) ^

                           (EMPPartnerSessionId != null
                                ? EMPPartnerSessionId.GetHashCode() * 3
                                : 0) ^

                           (ProviderId != null
                                ? ProviderId.         GetHashCode()
                                : 0);

                }
            }

            #endregion


            public override AuthorizationStop ToImmutable

                => new AuthorizationStop(Request,
                                         AuthorizationStatus,
                                         StatusCode,
                                         SessionId,
                                         CPOPartnerSessionId,
                                         EMPPartnerSessionId,
                                         ProviderId,
                                         CustomData);

        }

        #endregion

    }

}
