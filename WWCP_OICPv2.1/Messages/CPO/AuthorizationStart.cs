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
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.CPO
{

    /// <summary>
    /// An OICP AuthorizationStart result.
    /// </summary>
    public class AuthorizationStart : AResponse<AuthorizeStartRequest,
                                                AuthorizationStart>
    {

        #region Properties

        /// <summary>
        /// The charging session identification.
        /// </summary>
        public Session_Id?                  SessionId                           { get; }

        /// <summary>
        /// An optional partner charging session identification.
        /// </summary>
        public PartnerSession_Id?           PartnerSessionId                    { get; }

        /// <summary>
        /// The e-mobility provider identification.
        /// </summary>
        public Provider_Id?                 ProviderId                          { get; }

        /// <summary>
        /// The authorization status, e.g. "Authorized".
        /// </summary>
        public AuthorizationStatusTypes     AuthorizationStatus                 { get; }

        /// <summary>
        /// The authorization status code.
        /// </summary>
        public StatusCode                   StatusCode                          { get; }

        /// <summary>
        /// An enumeration of authorization identifications.
        /// </summary>
        public IEnumerable<Identification>  AuthorizationStopIdentifications    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OICP Authorization Start result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="AuthorizationStatus">The authorization status.</param>
        /// <param name="StatusCode">A status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        /// <param name="AuthorizationStopIdentifications">Optional authorization stop identifications.</param>
        /// <param name="CustomData">Optional custom data.</param>
        private AuthorizationStart(AuthorizeStartRequest                Request,
                                   AuthorizationStatusTypes             AuthorizationStatus,
                                   StatusCode                           StatusCode,
                                   Session_Id?                          SessionId                          = null,
                                   PartnerSession_Id?                   PartnerSessionId                   = null,
                                   Provider_Id?                         ProviderId                         = null,
                                   IEnumerable<Identification>          AuthorizationStopIdentifications   = null,
                                   IReadOnlyDictionary<String, Object>  CustomData                         = null)

            : base(Request,
                   CustomData)

        {

            this.AuthorizationStatus               = AuthorizationStatus;
            this.StatusCode                        = StatusCode;
            this.SessionId                         = SessionId;
            this.PartnerSessionId                  = PartnerSessionId;
            this.ProviderId                        = ProviderId;
            this.AuthorizationStopIdentifications  = AuthorizationStopIdentifications ?? new Identification[0];

        }

        #endregion


        #region (static) Authorized               (Request, SessionId = null, PartnerSessionId = null, ProviderId = null, ...)

        /// <summary>
        /// Create a new OICP 'Authorized' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="SessionId">The charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="AuthorizationStopIdentifications">Optional authorization stop identifications.</param>
        public static AuthorizationStart Authorized(AuthorizeStartRequest                     Request,
                                                    Session_Id?                               SessionId                         = null,
                                                    PartnerSession_Id?                        PartnerSessionId                  = null,
                                                    Provider_Id?                              ProviderId                        = null,
                                                    String                                    StatusCodeDescription             = null,
                                                    String                                    StatusCodeAdditionalInfo          = null,
                                                    IEnumerable<Identification>  AuthorizationStopIdentifications  = null)


            => new AuthorizationStart(Request,
                                      AuthorizationStatusTypes.Authorized,
                                      new StatusCode(
                                          StatusCodes.Success,
                                          StatusCodeDescription,
                                          StatusCodeAdditionalInfo
                                      ),
                                      SessionId,
                                      PartnerSessionId,
                                      ProviderId,
                                      AuthorizationStopIdentifications);

        #endregion

        #region (static) NotAuthorized            (Request, StatusCode, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new OICP 'NotAuthorized' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCode">The status code of the operation.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStart NotAuthorized(AuthorizeStartRequest  Request,
                                                       StatusCodes            StatusCode,
                                                       String                 StatusCodeDescription      = null,
                                                       String                 StatusCodeAdditionalInfo   = null,
                                                       Session_Id?            SessionId                  = null,
                                                       PartnerSession_Id?     PartnerSessionId           = null,
                                                       Provider_Id?           ProviderId                 = null)

            => new AuthorizationStart(Request,
                                      AuthorizationStatusTypes.NotAuthorized,
                                      new StatusCode(
                                          StatusCode,
                                          StatusCodeDescription,
                                          StatusCodeAdditionalInfo
                                      ),
                                      SessionId,
                                      PartnerSessionId,
                                      ProviderId);

        #endregion

        #region (static) SessionIsInvalid         (Request, StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'SessionIsInvalid' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStart SessionIsInvalid(AuthorizeStartRequest  Request,
                                                          String                 StatusCodeDescription      = null,
                                                          String                 StatusCodeAdditionalInfo   = null,
                                                          Session_Id?            SessionId                  = null,
                                                          PartnerSession_Id?     PartnerSessionId           = null,
                                                          Provider_Id?           ProviderId                 = null)

            => new AuthorizationStart(Request,
                                      AuthorizationStatusTypes.NotAuthorized,
                                      new StatusCode(
                                          StatusCodes.SessionIsInvalid,
                                          StatusCodeDescription ?? "Session is invalid",
                                          StatusCodeAdditionalInfo
                                      ),
                                      SessionId,
                                      PartnerSessionId,
                                      ProviderId);

        #endregion

        #region (static) CommunicationToEVSEFailed(Request, StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'CommunicationToEVSEFailed' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStart CommunicationToEVSEFailed(AuthorizeStartRequest  Request,
                                                                   String                 StatusCodeDescription      = null,
                                                                   String                 StatusCodeAdditionalInfo   = null,
                                                                   Session_Id?            SessionId                  = null,
                                                                   PartnerSession_Id?     PartnerSessionId           = null,
                                                                   Provider_Id?           ProviderId                 = null)

            => new AuthorizationStart(Request,
                                      AuthorizationStatusTypes.NotAuthorized,
                                      new StatusCode(
                                          StatusCodes.CommunicationToEVSEFailed,
                                          StatusCodeDescription ?? "Communication to EVSE failed!",
                                          StatusCodeAdditionalInfo
                                      ),
                                      SessionId,
                                      PartnerSessionId,
                                      ProviderId);

        #endregion

        #region (static) NoEVConnectedToEVSE      (Request, StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'NoEVConnectedToEVSE' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStart NoEVConnectedToEVSE(AuthorizeStartRequest  Request,
                                                             String                 StatusCodeDescription      = null,
                                                             String                 StatusCodeAdditionalInfo   = null,
                                                             Session_Id?            SessionId                  = null,
                                                             PartnerSession_Id?     PartnerSessionId           = null,
                                                             Provider_Id?           ProviderId                 = null)

            => new AuthorizationStart(Request,
                                      AuthorizationStatusTypes.NotAuthorized,
                                      new StatusCode(
                                          StatusCodes.NoEVConnectedToEVSE,
                                          StatusCodeDescription ?? "No EV connected to EVSE!",
                                          StatusCodeAdditionalInfo
                                      ),
                                      SessionId,
                                      PartnerSessionId,
                                      ProviderId);

        #endregion

        #region (static) EVSEAlreadyReserved      (Request, StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'EVSEAlreadyReserved' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStart EVSEAlreadyReserved(AuthorizeStartRequest  Request,
                                                             String                 StatusCodeDescription      = null,
                                                             String                 StatusCodeAdditionalInfo   = null,
                                                             Session_Id?            SessionId                  = null,
                                                             PartnerSession_Id?     PartnerSessionId           = null,
                                                             Provider_Id?           ProviderId                 = null)

            => new AuthorizationStart(Request,
                                      AuthorizationStatusTypes.NotAuthorized,
                                      new StatusCode(
                                          StatusCodes.EVSEAlreadyReserved,
                                          StatusCodeDescription ?? "EVSE already reserved!",
                                          StatusCodeAdditionalInfo
                                      ),
                                      SessionId,
                                      PartnerSessionId,
                                      ProviderId);

        #endregion

        #region (static) UnknownEVSEID            (Request, StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'UnknownEVSEID' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStart UnknownEVSEID(AuthorizeStartRequest  Request,
                                                       String                 StatusCodeDescription      = null,
                                                       String                 StatusCodeAdditionalInfo   = null,
                                                       Session_Id?            SessionId                  = null,
                                                       PartnerSession_Id?     PartnerSessionId           = null,
                                                       Provider_Id?           ProviderId                 = null)

            => new AuthorizationStart(Request,
                                      AuthorizationStatusTypes.NotAuthorized,
                                      new StatusCode(
                                          StatusCodes.UnknownEVSEID,
                                          StatusCodeDescription ?? "Unknown EVSE ID!",
                                          StatusCodeAdditionalInfo
                                      ),
                                      SessionId,
                                      PartnerSessionId,
                                      ProviderId);

        #endregion

        #region (static) EVSEOutOfService         (Request, StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'EVSEOutOfService' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStart EVSEOutOfService(AuthorizeStartRequest  Request,
                                                          String                 StatusCodeDescription      = null,
                                                          String                 StatusCodeAdditionalInfo   = null,
                                                          Session_Id?            SessionId                  = null,
                                                          PartnerSession_Id?     PartnerSessionId           = null,
                                                          Provider_Id?           ProviderId                 = null)

            => new AuthorizationStart(Request,
                                      AuthorizationStatusTypes.NotAuthorized,
                                      new StatusCode(
                                          StatusCodes.EVSEOutOfService,
                                          StatusCodeDescription ?? "EVSE out of service!",
                                          StatusCodeAdditionalInfo
                                      ),
                                      SessionId,
                                      PartnerSessionId,
                                      ProviderId);

        #endregion

        #region (static) ServiceNotAvailable      (Request, StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'ServiceNotAvailable' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStart ServiceNotAvailable(AuthorizeStartRequest  Request,
                                                             String                 StatusCodeDescription      = null,
                                                             String                 StatusCodeAdditionalInfo   = null,
                                                             Session_Id?            SessionId                  = null,
                                                             PartnerSession_Id?     PartnerSessionId           = null,
                                                             Provider_Id?           ProviderId                 = null)

            => new AuthorizationStart(Request,
                                      AuthorizationStatusTypes.NotAuthorized,
                                      new StatusCode(
                                          StatusCodes.ServiceNotAvailable,
                                          StatusCodeDescription ?? "Service not available!",
                                          StatusCodeAdditionalInfo
                                      ),
                                      SessionId,
                                      PartnerSessionId,
                                      ProviderId);

        #endregion

        #region (static) DataError                (Request, StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'DataError' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStart DataError(AuthorizeStartRequest  Request,
                                                   String                 StatusCodeDescription      = null,
                                                   String                 StatusCodeAdditionalInfo   = null,
                                                   Session_Id?            SessionId                  = null,
                                                   PartnerSession_Id?     PartnerSessionId           = null,
                                                   Provider_Id?           ProviderId                 = null)

            => new AuthorizationStart(Request,
                                      AuthorizationStatusTypes.NotAuthorized,
                                      new StatusCode(
                                          StatusCodes.DataError,
                                          StatusCodeDescription ?? "Data Error!",
                                          StatusCodeAdditionalInfo
                                      ),
                                      SessionId,
                                      PartnerSessionId,
                                      ProviderId);

        #endregion

        #region (static) SystemError              (Request, StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'SystemError' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStart SystemError(AuthorizeStartRequest  Request,
                                                     String                 StatusCodeDescription      = null,
                                                     String                 StatusCodeAdditionalInfo   = null,
                                                     Session_Id?            SessionId                  = null,
                                                     PartnerSession_Id?     PartnerSessionId           = null,
                                                     Provider_Id?           ProviderId                 = null)

            => new AuthorizationStart(Request,
                                      AuthorizationStatusTypes.NotAuthorized,
                                      new StatusCode(
                                          StatusCodes.SystemError,
                                          StatusCodeDescription ?? "System Error!",
                                          StatusCodeAdditionalInfo
                                      ),
                                      SessionId,
                                      PartnerSessionId,
                                      ProviderId);

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv       = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:Authorization = "http://www.hubject.com/b2b/services/authorization/v2.0"
        //                   xmlns:CommonTypes   = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        //
        //    <soapenv:Header/>
        //
        //    <soapenv:Body>
        //
        //       <Authorization:eRoamingAuthorizationStart>
        //
        //          <!--Optional:-->
        //          <Authorization:SessionID>?</Authorization:SessionID>
        //
        //          <!--Optional:-->
        //          <Authorization:PartnerSessionID>?</Authorization:PartnerSessionID>
        //
        //          <!--Optional:-->
        //          <Authorization:ProviderID>?</Authorization:ProviderID>
        //
        //          <Authorization:AuthorizationStatus>?</Authorization:AuthorizationStatus>
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
        //          <!--Optional:-->
        //          <Authorization:AuthorizationStopIdentifications>
        //
        //             <!--Zero or more repetitions:-->
        //             <Authorization:Identification>
        //
        //                <!--You have a CHOICE of the next 4 items at this level-->
        //                <CommonTypes:RFIDmifarefamilyIdentification>
        //                   <CommonTypes:UID>?</CommonTypes:UID>
        //                </CommonTypes:RFIDmifarefamilyIdentification>
        //
        //                <CommonTypes:QRCodeIdentification>
        //
        //                   <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
        //
        //                   <!--You have a CHOICE of the next 2 items at this level-->
        //                   <CommonTypes:PIN>?</CommonTypes:PIN>
        //
        //                   <CommonTypes:HashedPIN>
        //                      <CommonTypes:Value>?</CommonTypes:Value>
        //                      <CommonTypes:Function>?</CommonTypes:Function>
        //                      <CommonTypes:Salt>?</CommonTypes:Salt>
        //                   </CommonTypes:HashedPIN>
        //
        //                </CommonTypes:QRCodeIdentification>
        //
        //                <CommonTypes:PlugAndChargeIdentification>
        //                   <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
        //                </CommonTypes:PlugAndChargeIdentification>
        //
        //                <CommonTypes:RemoteIdentification>
        //                   <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
        //                </CommonTypes:RemoteIdentification>
        //
        //             </Authorization:Identification>
        //          </Authorization:AuthorizationStopIdentifications>
        //
        //       </Authorization:eRoamingAuthorizationStart>
        //    </soapenv:Body>
        //
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (Request, AuthorizationStartXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP EVSE statuses request.
        /// </summary>
        /// <param name="Request">An PullAuthorizationStart request.</param>
        /// <param name="AuthorizationStartXML">The XML to parse.</param>
        /// <param name="CustomAuthorizationStartParser">A delegate to parse custom AuthorizationStart XML elements.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AuthorizationStart Parse(AuthorizeStartRequest                        Request,
                                               XElement                                     AuthorizationStartXML,
                                               CustomXMLParserDelegate<AuthorizationStart>  CustomAuthorizationStartParser   = null,
                                               CustomXMLParserDelegate<Identification>      CustomIdentificationParser       = null,
                                               CustomXMLParserDelegate<StatusCode>          CustomStatusCodeParser           = null,
                                               OnExceptionDelegate                          OnException                      = null)
        {

            AuthorizationStart _AuthorizationStart;

            if (TryParse(Request,
                         AuthorizationStartXML,
                         out _AuthorizationStart,
                         CustomAuthorizationStartParser,
                         CustomIdentificationParser,
                         CustomStatusCodeParser,
                         OnException))

                return _AuthorizationStart;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, AuthorizationStartText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP EVSE statuses request.
        /// </summary>
        /// <param name="Request">An PullAuthorizationStart request.</param>
        /// <param name="AuthorizationStartText">The text to parse.</param>
        /// <param name="CustomAuthorizationStartParser">A delegate to parse custom AuthorizationStart XML elements.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AuthorizationStart Parse(AuthorizeStartRequest                        Request,
                                               String                                       AuthorizationStartText,
                                               CustomXMLParserDelegate<AuthorizationStart>  CustomAuthorizationStartParser   = null,
                                               CustomXMLParserDelegate<Identification>      CustomIdentificationParser       = null,
                                               CustomXMLParserDelegate<StatusCode>          CustomStatusCodeParser           = null,
                                               OnExceptionDelegate                          OnException                      = null)
        {

            AuthorizationStart _AuthorizationStart;

            if (TryParse(Request,
                         AuthorizationStartText,
                         out _AuthorizationStart,
                         CustomAuthorizationStartParser,
                         CustomIdentificationParser,
                         CustomStatusCodeParser,
                         OnException))

                return _AuthorizationStart;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, AuthorizationStartXML,  out AuthorizationStart, ..., OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP EVSE statuses request.
        /// </summary>
        /// <param name="Request">An PullAuthorizationStart request.</param>
        /// <param name="AuthorizationStartXML">The XML to parse.</param>
        /// <param name="AuthorizationStart">The parsed AuthorizationStart request.</param>
        /// <param name="CustomAuthorizationStartParser">A delegate to parse custom AuthorizationStart XML elements.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(AuthorizeStartRequest                        Request,
                                       XElement                                     AuthorizationStartXML,
                                       out AuthorizationStart                       AuthorizationStart,
                                       CustomXMLParserDelegate<AuthorizationStart>  CustomAuthorizationStartParser   = null,
                                       CustomXMLParserDelegate<Identification>      CustomIdentificationParser       = null,
                                       CustomXMLParserDelegate<StatusCode>          CustomStatusCodeParser           = null,
                                       OnExceptionDelegate                          OnException                      = null)
        {

            try
            {

                if (AuthorizationStartXML.Name != OICPNS.Authorization + "eRoamingAuthorizationStart")
                {
                    AuthorizationStart = null;
                    return false;
                }

                AuthorizationStart = new AuthorizationStart(

                                         Request,

                                         AuthorizationStartXML.MapValueOrFail    (OICPNS.Authorization + "AuthorizationStatus",
                                                                                  s => (AuthorizationStatusTypes) Enum.Parse(typeof(AuthorizationStatusTypes), s)),

                                         AuthorizationStartXML.MapElement        (OICPNS.Authorization + "StatusCode",
                                                                                  (XML, e) => StatusCode.Parse(XML,
                                                                                                               CustomStatusCodeParser,
                                                                                                               e),
                                                                                  OnException),

                                         AuthorizationStartXML.MapValueOrNullable(OICPNS.Authorization + "SessionID",
                                                                                  Session_Id.       Parse),

                                         AuthorizationStartXML.MapValueOrNullable(OICPNS.Authorization + "PartnerSessionID",
                                                                                  PartnerSession_Id.Parse),

                                         AuthorizationStartXML.MapValueOrNullable(OICPNS.Authorization + "ProviderID",
                                                                                  Provider_Id.      Parse),

                                         AuthorizationStartXML.MapElements       (OICPNS.Authorization + "AuthorizationStopIdentifications",
                                                                                  (XML, e) => Identification.Parse(XML,
                                                                                                                   CustomIdentificationParser,
                                                                                                                   e),
                                                                                  OnException)

                                     );


                if (CustomAuthorizationStartParser != null)
                    AuthorizationStart = CustomAuthorizationStartParser(AuthorizationStartXML,
                                                                        AuthorizationStart);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, AuthorizationStartXML, e);

                AuthorizationStart = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, AuthorizationStartText, out AuthorizationStart, ..., OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP EVSE statuses request.
        /// </summary>
        /// <param name="Request">An PullAuthorizationStart request.</param>
        /// <param name="AuthorizationStartText">The text to parse.</param>
        /// <param name="AuthorizationStart">The parsed EVSE statuses request.</param>
        /// <param name="CustomAuthorizationStartParser">A delegate to parse custom AuthorizationStart XML elements.</param>
        /// <param name="CustomIdentificationParser">A delegate to parse custom Identification XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(AuthorizeStartRequest                        Request,
                                       String                                       AuthorizationStartText,
                                       out AuthorizationStart                       AuthorizationStart,
                                       CustomXMLParserDelegate<AuthorizationStart>  CustomAuthorizationStartParser   = null,
                                       CustomXMLParserDelegate<Identification>      CustomIdentificationParser       = null,
                                       CustomXMLParserDelegate<StatusCode>          CustomStatusCodeParser           = null,
                                       OnExceptionDelegate                          OnException                      = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(AuthorizationStartText).Root,
                             out AuthorizationStart,
                             CustomAuthorizationStartParser,
                             CustomIdentificationParser,
                             CustomStatusCodeParser,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, AuthorizationStartText, e);
            }

            AuthorizationStart = null;
            return false;

        }

        #endregion

        #region ToXML(CustomAuthorizationStartSerializer = null, CustomStatusCodeSerializer = null, CustomIdentificationSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomAuthorizationStartSerializer">A delegate to customize the serialization of AuthorizationStart respones.</param>
        /// <param name="CustomStatusCodeSerializer">A delegate to serialize custom StatusCode XML elements.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<AuthorizationStart>  CustomAuthorizationStartSerializer   = null,
                              CustomXMLSerializerDelegate<StatusCode>          CustomStatusCodeSerializer           = null,
                              CustomXMLSerializerDelegate<Identification>      CustomIdentificationSerializer       = null)

        {

            var XML = new XElement(OICPNS.Authorization + "eRoamingAuthorizationStart",

                          SessionId.HasValue
                              ? new XElement(OICPNS.Authorization + "SessionID",         SessionId.ToString())
                              : null,

                          PartnerSessionId.HasValue
                              ? new XElement(OICPNS.Authorization + "PartnerSessionID",  PartnerSessionId.ToString())
                              : null,

                          ProviderId.HasValue
                              ? new XElement(OICPNS.Authorization + "ProviderID",        ProviderId.ToString())
                              : null,

                          new XElement(OICPNS.Authorization + "AuthorizationStatus",     AuthorizationStatus.ToString()),


                          StatusCode.ToXML(CustomStatusCodeSerializer: CustomStatusCodeSerializer),

                          AuthorizationStopIdentifications.Any()
                              ? new XElement(OICPNS.Authorization + "AuthorizationStopIdentifications",
                                    AuthorizationStopIdentifications.Select(identification => identification.ToXML(CustomIdentificationSerializer: CustomIdentificationSerializer))
                                )
                              : null

                      );

            return CustomAuthorizationStartSerializer != null
                       ? CustomAuthorizationStartSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(AuthorizationStatus,
                             StatusCode.HasResult
                                 ? ", " + StatusCode
                                 : "");

        #endregion


        public override Boolean Equals(AuthorizationStart AResponse)
        {
            throw new NotImplementedException();
        }


        #region ToBuilder

        /// <summary>
        /// Return a response builder.
        /// </summary>
        public Builder ToBuilder
            => new Builder(this);

        #endregion

        #region (class) Builder

        /// <summary>
        /// An AuthorizationStart response builder.
        /// </summary>
        public class Builder : AResponseBuilder<AuthorizeStartRequest,
                                                AuthorizationStart>
        {

            #region Properties

            public AuthorizeStartRequest              Request                             { get; set; }

            /// <summary>
            /// The charging session identification.
            /// </summary>
            public Session_Id?                        SessionId                           { get; set; }

            /// <summary>
            /// An optional partner charging session identification.
            /// </summary>
            public PartnerSession_Id?                 PartnerSessionId                    { get; set; }

            /// <summary>
            /// The e-mobility provider identification.
            /// </summary>
            public Provider_Id?                       ProviderId                          { get; set; }

            /// <summary>
            /// The authorization status, e.g. "Authorized".
            /// </summary>
            public AuthorizationStatusTypes           AuthorizationStatus                 { get; set; }

            /// <summary>
            /// The authorization status code.
            /// </summary>
            public StatusCode                         StatusCode                          { get; set; }

            /// <summary>
            /// An enumeration of authorization identifications.
            /// </summary>
            public List<Identification>  AuthorizationStopIdentifications    { get; set; }

            ///// <summary>
            ///// The result of the operation.
            ///// </summary>
            //public Result                             Result                              { get; set; }

            #endregion

            #region Constructor(s)

            #region Builder(Request,            CustomData = null)

            /// <summary>
            /// Create a new AuthorizationStart response builder.
            /// </summary>
            /// <param name="Request">An AuthorizeStart request.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(AuthorizeStartRequest                Request,
                           IReadOnlyDictionary<String, Object>  CustomData  = null)

                : base(Request,
                       CustomData)

            { }

            #endregion

            #region Builder(AuthorizationStart, CustomData = null)

            /// <summary>
            /// Create a new AuthorizationStart response builder.
            /// </summary>
            /// <param name="AuthorizationStart">An AuthorizeStart response.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(AuthorizationStart                   AuthorizationStart,
                           IReadOnlyDictionary<String, Object>  CustomData  = null)

                : base(AuthorizationStart?.Request,
                       AuthorizationStart.HasCustomData
                           ? CustomData != null && CustomData.Any()
                                 ? AuthorizationStart.CustomValues.Concat(CustomData)
                                 : AuthorizationStart.CustomValues
                           : CustomData)

            {

                if (AuthorizationStart != null)
                {

                    this.SessionId                         = AuthorizationStart.SessionId;
                    this.PartnerSessionId                  = AuthorizationStart.PartnerSessionId;
                    this.ProviderId                        = AuthorizationStart.ProviderId;
                    this.AuthorizationStatus               = AuthorizationStart.AuthorizationStatus;
                    this.StatusCode                        = AuthorizationStart.StatusCode;
                    this.AuthorizationStopIdentifications  = AuthorizationStart.AuthorizationStopIdentifications != null
                                                                 ? new List<Identification>(AuthorizationStart.AuthorizationStopIdentifications)
                                                                 : new List<Identification>();

                }

            }

            #endregion

            #endregion



            public override Boolean Equals(AuthorizationStart AResponse)
            {
                throw new NotImplementedException();
            }

            public override AuthorizationStart ToImmutable

                => new AuthorizationStart(Request,
                                          AuthorizationStatus,
                                          StatusCode,
                                          SessionId,
                                          PartnerSessionId,
                                          ProviderId,
                                          AuthorizationStopIdentifications,
                                          ImmutableCustomData);

        }

        #endregion

    }

}
