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
using Newtonsoft.Json.Linq;
using org.GraphDefined.Vanaheimr.Hermod.JSON;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2.CPO
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
        public CPOPartnerSession_Id?        CPOPartnerSessionId                 { get; }

        /// <summary>
        /// An optional partner charging session identification.
        /// </summary>
        public EMPPartnerSession_Id?        EMPPartnerSessionId                 { get; }

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
        /// <param name="CPOPartnerSessionId">An optional EMP partner charging session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional CPO partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        /// <param name="AuthorizationStopIdentifications">Optional authorization stop identifications.</param>
        /// <param name="CustomData">Optional custom data.</param>
        private AuthorizationStart(AuthorizeStartRequest                Request,
                                   AuthorizationStatusTypes             AuthorizationStatus,
                                   StatusCode                           StatusCode,
                                   Session_Id?                          SessionId                          = null,
                                   CPOPartnerSession_Id?                CPOPartnerSessionId                = null,
                                   EMPPartnerSession_Id?                EMPPartnerSessionId                = null,
                                   Provider_Id?                         ProviderId                         = null,
                                   IEnumerable<Identification>          AuthorizationStopIdentifications   = null,
                                   IReadOnlyDictionary<String, Object>  CustomData                         = null)

            : base(Request,
                   CustomData)

        {

            this.AuthorizationStatus               = AuthorizationStatus;
            this.StatusCode                        = StatusCode;
            this.SessionId                         = SessionId;
            this.CPOPartnerSessionId               = CPOPartnerSessionId;
            this.EMPPartnerSessionId               = EMPPartnerSessionId;
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
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="AuthorizationStopIdentifications">Optional authorization stop identifications.</param>
        public static AuthorizationStart Authorized(AuthorizeStartRequest                     Request,
                                                    Session_Id?                               SessionId                         = null,
                                                    CPOPartnerSession_Id?                     CPOPartnerSessionId               = null,
                                                    EMPPartnerSession_Id?                     EMPPartnerSessionId               = null,
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
                                      CPOPartnerSessionId,
                                      EMPPartnerSessionId,
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
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStart NotAuthorized(AuthorizeStartRequest  Request,
                                                       StatusCodes            StatusCode,
                                                       String                 StatusCodeDescription      = null,
                                                       String                 StatusCodeAdditionalInfo   = null,
                                                       Session_Id?            SessionId                  = null,
                                                       CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                       EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                       Provider_Id?           ProviderId                 = null)

            => new AuthorizationStart(Request,
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
        /// Create a new OICP 'SessionIsInvalid' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStart SessionIsInvalid(AuthorizeStartRequest  Request,
                                                          String                 StatusCodeDescription      = null,
                                                          String                 StatusCodeAdditionalInfo   = null,
                                                          Session_Id?            SessionId                  = null,
                                                          CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                          EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                          Provider_Id?           ProviderId                 = null)

            => new AuthorizationStart(Request,
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
        /// Create a new OICP 'CommunicationToEVSEFailed' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStart CommunicationToEVSEFailed(AuthorizeStartRequest  Request,
                                                                   String                 StatusCodeDescription      = null,
                                                                   String                 StatusCodeAdditionalInfo   = null,
                                                                   Session_Id?            SessionId                  = null,
                                                                   CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                                   EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                                   Provider_Id?           ProviderId                 = null)

            => new AuthorizationStart(Request,
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
        /// Create a new OICP 'NoEVConnectedToEVSE' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStart NoEVConnectedToEVSE(AuthorizeStartRequest  Request,
                                                             String                 StatusCodeDescription      = null,
                                                             String                 StatusCodeAdditionalInfo   = null,
                                                             Session_Id?            SessionId                  = null,
                                                             CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                             EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                             Provider_Id?           ProviderId                 = null)

            => new AuthorizationStart(Request,
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
        /// Create a new OICP 'EVSEAlreadyReserved' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStart EVSEAlreadyReserved(AuthorizeStartRequest  Request,
                                                             String                 StatusCodeDescription      = null,
                                                             String                 StatusCodeAdditionalInfo   = null,
                                                             Session_Id?            SessionId                  = null,
                                                             CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                             EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                             Provider_Id?           ProviderId                 = null)

            => new AuthorizationStart(Request,
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
        /// Create a new OICP 'UnknownEVSEID' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStart UnknownEVSEID(AuthorizeStartRequest  Request,
                                                       String                 StatusCodeDescription      = null,
                                                       String                 StatusCodeAdditionalInfo   = null,
                                                       Session_Id?            SessionId                  = null,
                                                       CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                       EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                       Provider_Id?           ProviderId                 = null)

            => new AuthorizationStart(Request,
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
        /// Create a new OICP 'EVSEOutOfService' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStart EVSEOutOfService(AuthorizeStartRequest  Request,
                                                          String                 StatusCodeDescription      = null,
                                                          String                 StatusCodeAdditionalInfo   = null,
                                                          Session_Id?            SessionId                  = null,
                                                          CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                          EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                          Provider_Id?           ProviderId                 = null)

            => new AuthorizationStart(Request,
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
        /// Create a new OICP 'ServiceNotAvailable' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStart ServiceNotAvailable(AuthorizeStartRequest  Request,
                                                             String                 StatusCodeDescription      = null,
                                                             String                 StatusCodeAdditionalInfo   = null,
                                                             Session_Id?            SessionId                  = null,
                                                             CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                             EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                             Provider_Id?           ProviderId                 = null)

            => new AuthorizationStart(Request,
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

        #region (static) DataError                (Request, StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'DataError' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStart DataError(AuthorizeStartRequest  Request,
                                                   String                 StatusCodeDescription      = null,
                                                   String                 StatusCodeAdditionalInfo   = null,
                                                   Session_Id?            SessionId                  = null,
                                                   CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                   EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                   Provider_Id?           ProviderId                 = null)

            => new AuthorizationStart(Request,
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

        #region (static) SystemError              (Request, StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'SystemError' AuthorizationStart result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional CPO partner session identification.</param>
        /// <param name="EMPPartnerSessionId">An optional EMP partner session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStart SystemError(AuthorizeStartRequest  Request,
                                                     String                 StatusCodeDescription      = null,
                                                     String                 StatusCodeAdditionalInfo   = null,
                                                     Session_Id?            SessionId                  = null,
                                                     CPOPartnerSession_Id?  CPOPartnerSessionId        = null,
                                                     EMPPartnerSession_Id?  EMPPartnerSessionId        = null,
                                                     Provider_Id?           ProviderId                 = null)

            => new AuthorizationStart(Request,
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
        //          <Authorization:CPOPartnerSessionID>?</Authorization:CPOPartnerSessionID>
        //
        //          <!--Optional:-->
        //          <Authorization:EMPPartnerSessionID>?</Authorization:EMPPartnerSessionID>
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
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identification XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AuthorizationStart Parse(AuthorizeStartRequest                        Request,
                                               XElement                                     AuthorizationStartXML,
                                               CustomXMLParserDelegate<AuthorizationStart>  CustomAuthorizationStartParser   = null,
                                               CustomXMLParserDelegate<Identification>      CustomIdentificationParser       = null,
                                               CustomXMLParserDelegate<RFIDIdentification>  CustomRFIDIdentificationParser   = null,
                                               CustomXMLParserDelegate<StatusCode>          CustomStatusCodeParser           = null,
                                               OnExceptionDelegate                          OnException                      = null)
        {

            if (TryParse(Request,
                         AuthorizationStartXML,
                         out AuthorizationStart _AuthorizationStart,
                         CustomAuthorizationStartParser,
                         CustomIdentificationParser,
                         CustomRFIDIdentificationParser,
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
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identification XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AuthorizationStart Parse(AuthorizeStartRequest                        Request,
                                               String                                       AuthorizationStartText,
                                               CustomXMLParserDelegate<AuthorizationStart>  CustomAuthorizationStartParser   = null,
                                               CustomXMLParserDelegate<Identification>      CustomIdentificationParser       = null,
                                               CustomXMLParserDelegate<RFIDIdentification>  CustomRFIDIdentificationParser   = null,
                                               CustomXMLParserDelegate<StatusCode>          CustomStatusCodeParser           = null,
                                               OnExceptionDelegate                          OnException                      = null)
        {

            if (TryParse(Request,
                         AuthorizationStartText,
                         out AuthorizationStart _AuthorizationStart,
                         CustomAuthorizationStartParser,
                         CustomIdentificationParser,
                         CustomRFIDIdentificationParser,
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
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identification XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(AuthorizeStartRequest                        Request,
                                       XElement                                     AuthorizationStartXML,
                                       out AuthorizationStart                       AuthorizationStart,
                                       CustomXMLParserDelegate<AuthorizationStart>  CustomAuthorizationStartParser   = null,
                                       CustomXMLParserDelegate<Identification>      CustomIdentificationParser       = null,
                                       CustomXMLParserDelegate<RFIDIdentification>  CustomRFIDIdentificationParser   = null,
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

                                         AuthorizationStartXML.MapValueOrNullable(OICPNS.Authorization + "CPOPartnerSessionID",
                                                                                  CPOPartnerSession_Id.Parse),

                                         AuthorizationStartXML.MapValueOrNullable(OICPNS.Authorization + "EMPPartnerSessionID",
                                                                                  EMPPartnerSession_Id.Parse),

                                         AuthorizationStartXML.MapValueOrNullable(OICPNS.Authorization + "ProviderID",
                                                                                  Provider_Id.      Parse),

                                         AuthorizationStartXML.MapElements       (OICPNS.Authorization + "AuthorizationStopIdentifications",
                                                                                  (XML, e) => Identification.Parse(XML,
                                                                                                                   CustomIdentificationParser,
                                                                                                                   CustomRFIDIdentificationParser,
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

                OnException?.Invoke(DateTime.UtcNow, AuthorizationStartXML, e);

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
        /// <param name="CustomRFIDIdentificationParser">A delegate to parse custom RFID identification XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(AuthorizeStartRequest                        Request,
                                       String                                       AuthorizationStartText,
                                       out AuthorizationStart                       AuthorizationStart,
                                       CustomXMLParserDelegate<AuthorizationStart>  CustomAuthorizationStartParser   = null,
                                       CustomXMLParserDelegate<Identification>      CustomIdentificationParser       = null,
                                       CustomXMLParserDelegate<RFIDIdentification>  CustomRFIDIdentificationParser   = null,
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
                             CustomRFIDIdentificationParser,
                             CustomStatusCodeParser,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, AuthorizationStartText, e);
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
                              ? new XElement(OICPNS.Authorization + "SessionID",            SessionId.          ToString())
                              : null,

                          CPOPartnerSessionId.HasValue
                              ? new XElement(OICPNS.Authorization + "CPOPartnerSessionID",  CPOPartnerSessionId.ToString())
                              : null,

                          EMPPartnerSessionId.HasValue
                              ? new XElement(OICPNS.Authorization + "EMPPartnerSessionID",  EMPPartnerSessionId.ToString())
                              : null,

                          ProviderId.HasValue
                              ? new XElement(OICPNS.Authorization + "ProviderID",           ProviderId.         ToString())
                              : null,

                          new XElement(OICPNS.Authorization + "AuthorizationStatus",    AuthorizationStatus.ToString()),


                          StatusCode.ToXML(OICPNS.Authorization + "StatusCode",
                                           CustomStatusCodeSerializer),

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


        #region ToJSON(CustomAuthorizationStartSerializer = null, CustomStatusCodeSerializer = null, CustomIdentificationSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomAuthorizationStartSerializer">A delegate to customize the serialization of AuthorizationStart respones.</param>
        /// <param name="CustomStatusCodeSerializer">A delegate to serialize custom StatusCode JSON objects.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<AuthorizationStart>  CustomAuthorizationStartSerializer   = null,
                              CustomJObjectSerializerDelegate<StatusCode>          CustomStatusCodeSerializer           = null,
                              CustomJObjectSerializerDelegate<Identification>      CustomIdentificationSerializer       = null)

        {

            var JSON = JSONObject.Create(

                           SessionId.HasValue
                               ? new JProperty("sessionID",            SessionId.          ToString())
                               : null,

                           CPOPartnerSessionId.HasValue
                               ? new JProperty("CPOPartnerSessionID",  CPOPartnerSessionId.ToString())
                               : null,

                           EMPPartnerSessionId.HasValue
                               ? new JProperty("EMPPartnerSessionID",  EMPPartnerSessionId.ToString())
                               : null,

                           ProviderId.HasValue
                               ? new JProperty("providerID",           ProviderId.         ToString())
                               : null,

                           new JProperty("authorizationStatus",        AuthorizationStatus.ToString()),


                           new JProperty("statusCode",                 StatusCode.         ToJSON(CustomStatusCodeSerializer)),

                           AuthorizationStopIdentifications.Any()
                               ? new JProperty("authorizationStopIdentifications", new JArray(AuthorizationStopIdentifications.Select(identification => identification.ToJSON(CustomIdentificationSerializer: CustomIdentificationSerializer))))
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
        public static Boolean operator == (AuthorizationStart AuthorizationStart1, AuthorizationStart AuthorizationStart2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(AuthorizationStart1, AuthorizationStart2))
                return true;

            // If one is null, but not both, return false.
            if ((AuthorizationStart1 is null) || (AuthorizationStart2 is null))
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
        public static Boolean operator != (AuthorizationStart AuthorizationStart1, AuthorizationStart AuthorizationStart2)
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
        {

            if (Object == null)
                return false;

            if (!(Object is AuthorizationStart AuthorizationStart))
                return false;

            return Equals(AuthorizationStart);

        }

        #endregion

        #region Equals(AuthorizationStart)

        /// <summary>
        /// Compares two authorize start requests for equality.
        /// </summary>
        /// <param name="AuthorizationStart">An authorize start request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(AuthorizationStart AuthorizationStart)
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
                             StatusCode.HasResult
                                 ? ", " + StatusCode
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
        /// An AuthorizationStart response builder.
        /// </summary>
        public class Builder : AResponseBuilder<AuthorizeStartRequest,
                                                AuthorizationStart>
        {

            #region Properties

            /// <summary>
            /// The charging session identification.
            /// </summary>
            public Session_Id?                        SessionId                           { get; set; }

            /// <summary>
            /// An optional EMP partner charging session identification.
            /// </summary>
            public CPOPartnerSession_Id?              CPOPartnerSessionId                 { get; set; }

            /// <summary>
            /// An optional CPO partner charging session identification.
            /// </summary>
            public EMPPartnerSession_Id?              EMPPartnerSessionId                 { get; set; }

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
            public List<Identification>               AuthorizationStopIdentifications    { get; set; }

            #endregion

            #region Constructor(s)

            #region Builder(Request,            CustomData = null)

            /// <summary>
            /// Create a new AuthorizationStart response builder.
            /// </summary>
            /// <param name="Request">The request leading to this response.</param>
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
                                 ? AuthorizationStart.CustomData.Concat(CustomData)
                                 : AuthorizationStart.CustomData
                           : CustomData)

            {

                if (AuthorizationStart != null)
                {

                    this.SessionId                         = AuthorizationStart.SessionId;
                    this.CPOPartnerSessionId               = AuthorizationStart.CPOPartnerSessionId;
                    this.EMPPartnerSessionId               = AuthorizationStart.EMPPartnerSessionId;
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


            #region Operator overloading

            #region Operator == (AuthorizationStart1, AuthorizationStart2)

            /// <summary>
            /// Compares two AuthorizationStart requests for equality.
            /// </summary>
            /// <param name="AuthorizationStart1">An authorize start request.</param>
            /// <param name="AuthorizationStart2">Another authorize start request.</param>
            /// <returns>True if both match; False otherwise.</returns>
            public static Boolean operator == (Builder AuthorizationStart1, Builder AuthorizationStart2)
            {

                // If both are null, or both are same instance, return true.
                if (ReferenceEquals(AuthorizationStart1, AuthorizationStart2))
                    return true;

                // If one is null, but not both, return false.
                if (((Object) AuthorizationStart1 == null) || ((Object) AuthorizationStart2 == null))
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
            public static Boolean operator != (Builder AuthorizationStart1, Builder AuthorizationStart2)
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
            {

                if (Object == null)
                    return false;

                if (!(Object is AuthorizationStart AuthorizationStart))
                    return false;

                return Equals(AuthorizationStart);

            }

            #endregion

            #region Equals(AuthorizationStart)

            /// <summary>
            /// Compares two authorize start requests for equality.
            /// </summary>
            /// <param name="AuthorizationStart">An authorize start request to compare with.</param>
            /// <returns>True if both match; False otherwise.</returns>
            public override Boolean Equals(AuthorizationStart AuthorizationStart)
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


            public override AuthorizationStart ToImmutable

                => new AuthorizationStart(Request,
                                          AuthorizationStatus,
                                          StatusCode,
                                          SessionId,
                                          CPOPartnerSessionId,
                                          EMPPartnerSessionId,
                                          ProviderId,
                                          AuthorizationStopIdentifications,
                                          CustomData);

        }

        #endregion

    }

}
