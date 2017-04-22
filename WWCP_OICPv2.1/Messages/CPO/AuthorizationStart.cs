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
        public Session_Id?                               SessionId                           { get; }

        /// <summary>
        /// An optional partner charging session identification.
        /// </summary>
        public PartnerSession_Id?                        PartnerSessionId                    { get; }

        /// <summary>
        /// The e-mobility provider identification.
        /// </summary>
        public Provider_Id?                              ProviderId                          { get; }

        /// <summary>
        /// The authorization status, e.g. "Authorized".
        /// </summary>
        public AuthorizationStatusTypes                  AuthorizationStatus                 { get; }

        /// <summary>
        /// The authorization status code.
        /// </summary>
        public StatusCode                                StatusCode                          { get; }

        /// <summary>
        /// An enumeration of authorization identifications.
        /// </summary>
        public IEnumerable<AuthorizationIdentification>  AuthorizationStopIdentifications    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OICP Authorization Start result.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="AuthorizationStatus">The authorization status.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        /// <param name="StatusCode">An optional status code.</param>
        /// <param name="AuthorizationStopIdentifications">Optional authorization stop identifications.</param>
        private AuthorizationStart(AuthorizeStartRequest                     Request,
                                   AuthorizationStatusTypes                  AuthorizationStatus,
                                   Session_Id?                               SessionId                          = null,
                                   PartnerSession_Id?                        PartnerSessionId                   = null,
                                   Provider_Id?                              ProviderId                         = null,
                                   StatusCode                                StatusCode                         = null,
                                   IEnumerable<AuthorizationIdentification>  AuthorizationStopIdentifications   = null)

            : base(Request)

        {

            this.AuthorizationStatus               = AuthorizationStatus;
            this.SessionId                         = SessionId;
            this.PartnerSessionId                  = PartnerSessionId;
            this.ProviderId                        = ProviderId;
            this.StatusCode                        = StatusCode;
            this.AuthorizationStopIdentifications  = AuthorizationStopIdentifications;

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
                                                    IEnumerable<AuthorizationIdentification>  AuthorizationStopIdentifications  = null)


            => new AuthorizationStart(Request,
                                      AuthorizationStatusTypes.Authorized,
                                      SessionId,
                                      PartnerSessionId,
                                      ProviderId,
                                      new StatusCode(
                                          StatusCodes.Success,
                                          StatusCodeDescription,
                                          StatusCodeAdditionalInfo
                                      ),
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
                                      SessionId,
                                      PartnerSessionId,
                                      ProviderId,
                                      new StatusCode(
                                          StatusCode,
                                          StatusCodeDescription,
                                          StatusCodeAdditionalInfo
                                      ));

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
                                      SessionId,
                                      PartnerSessionId,
                                      ProviderId,
                                      new StatusCode(
                                          StatusCodes.SessionIsInvalid,
                                          StatusCodeDescription ?? "Session is invalid",
                                          StatusCodeAdditionalInfo
                                      ));

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
                                      SessionId,
                                      PartnerSessionId,
                                      ProviderId,
                                      new StatusCode(
                                          StatusCodes.CommunicationToEVSEFailed,
                                          StatusCodeDescription ?? "Communication to EVSE failed!",
                                          StatusCodeAdditionalInfo
                                      ));

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
                                      SessionId,
                                      PartnerSessionId,
                                      ProviderId,
                                      new StatusCode(
                                          StatusCodes.NoEVConnectedToEVSE,
                                          StatusCodeDescription ?? "No EV connected to EVSE!",
                                          StatusCodeAdditionalInfo
                                      ));

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
                                      SessionId,
                                      PartnerSessionId,
                                      ProviderId,
                                      new StatusCode(
                                          StatusCodes.EVSEAlreadyReserved,
                                          StatusCodeDescription ?? "EVSE already reserved!",
                                          StatusCodeAdditionalInfo
                                      ));

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
                                      SessionId,
                                      PartnerSessionId,
                                      ProviderId,
                                      new StatusCode(
                                          StatusCodes.UnknownEVSEID,
                                          StatusCodeDescription ?? "Unknown EVSE ID!",
                                          StatusCodeAdditionalInfo
                                      ));

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
                                      SessionId,
                                      PartnerSessionId,
                                      ProviderId,
                                      new StatusCode(
                                          StatusCodes.EVSEOutOfService,
                                          StatusCodeDescription ?? "EVSE out of service!",
                                          StatusCodeAdditionalInfo
                                      ));

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
                                      SessionId,
                                      PartnerSessionId,
                                      ProviderId,
                                      new StatusCode(
                                          StatusCodes.ServiceNotAvailable,
                                          StatusCodeDescription ?? "Service not available!",
                                          StatusCodeAdditionalInfo
                                      ));

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
                                      SessionId,
                                      PartnerSessionId,
                                      ProviderId,
                                      new StatusCode(
                                          StatusCodes.DataError,
                                          StatusCodeDescription ?? "Data Error!",
                                          StatusCodeAdditionalInfo
                                      ));

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
                                      SessionId,
                                      PartnerSessionId,
                                      ProviderId,
                                      new StatusCode(
                                          StatusCodes.SystemError,
                                          StatusCodeDescription ?? "System Error!",
                                          StatusCodeAdditionalInfo
                                      ));

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv       = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:Authorization = "http://www.hubject.com/b2b/services/authorization/v2.0"
        //                   xmlns:CommonTypes   = "http://www.hubject.com/b2b/services/commontypes/v2.0">
        //
        // [...]
        //
        //    <Authorization:eRoamingAuthorizationStart>
        //
        //       <!--Optional:-->
        //       <Authorization:SessionID>?</Authorization:SessionID>
        //
        //       <!--Optional:-->
        //       <Authorization:PartnerSessionID>?</Authorization:PartnerSessionID>
        //
        //       <!--Optional:-->
        //       <Authorization:ProviderID>?</Authorization:ProviderID>
        //
        //       <Authorization:AuthorizationStatus>?</Authorization:AuthorizationStatus>
        //
        //       <Authorization:StatusCode>
        //
        //          <CommonTypes:Code>?</CommonTypes:Code>
        //
        //          <!--Optional:-->
        //          <CommonTypes:Description>?</CommonTypes:Description>
        //
        //          <!--Optional:-->
        //          <CommonTypes:AdditionalInfo>?</CommonTypes:AdditionalInfo>
        //
        //       </Authorization:StatusCode>
        //
        //       <!--Optional:-->
        //       <Authorization:AuthorizationStopIdentifications>
        //
        //          <!--Zero or more repetitions:-->
        //          <Authorization:Identification>
        //
        //             <!--You have a CHOICE of the next 4 items at this level-->
        //             <CommonTypes:RFIDmifarefamilyIdentification>
        //                <CommonTypes:UID>?</CommonTypes:UID>
        //             </CommonTypes:RFIDmifarefamilyIdentification>
        //
        //             <CommonTypes:QRCodeIdentification>
        //
        //                <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
        //
        //                <!--You have a CHOICE of the next 2 items at this level-->
        //                <CommonTypes:PIN>?</CommonTypes:PIN>
        //
        //                <CommonTypes:HashedPIN>
        //                   <CommonTypes:Value>?</CommonTypes:Value>
        //                   <CommonTypes:Function>?</CommonTypes:Function>
        //                   <CommonTypes:Salt>?</CommonTypes:Salt>
        //                </CommonTypes:HashedPIN>
        //
        //             </CommonTypes:QRCodeIdentification>
        //
        //             <CommonTypes:PlugAndChargeIdentification>
        //                <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
        //             </CommonTypes:PlugAndChargeIdentification>
        //
        //             <CommonTypes:RemoteIdentification>
        //                <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
        //             </CommonTypes:RemoteIdentification>
        //
        //          </Authorization:Identification>
        //       </Authorization:AuthorizationStopIdentifications>
        //
        //    </Authorization:eRoamingAuthorizationStart>
        //
        // [...]
        //
        // </soapenv:Envelope>


        // <Authorization:eRoamingAuthorizationStart>
        //
        //   <Authorization:AuthorizationStatus>NotAuthorized</Authorization:AuthorizationStatus>
        //
        //   <Authorization:StatusCode>
        //     <CommonTypes:Code>102</CommonTypes:Code>
        //     <CommonTypes:Description>RFID Authentication failed – invalid UID</CommonTypes:Description>
        //   </Authorization:StatusCode>
        //
        // </Authorization:eRoamingAuthorizationStart>


        // <Authorization:eRoamingAuthorizationStart>
        //
        //   <Authorization:SessionID>8fade8bd-0a88-1296-0f2f-41ae8a80af1b</Authorization:SessionID>
        //   <Authorization:PartnerSessionID>0815</Authorization:PartnerSessionID>
        //   <Authorization:ProviderID>BMW</Authorization:ProviderID>
        //   <Authorization:AuthorizationStatus>Authorized</Authorization:AuthorizationStatus>
        //
        //   <Authorization:StatusCode>
        //     <CommonTypes:Code>000</CommonTypes:Code>
        //     <CommonTypes:Description>Success</CommonTypes:Description>
        //   </Authorization:StatusCode>
        //
        // </Authorization:eRoamingAuthorizationStart>


        // <Authorization:eRoamingAuthorizationStart>
        //
        //   <Authorization:SessionID>8f9cbd74-0a88-1296-1078-6e9cca762de2</Authorization:SessionID>
        //   <Authorization:PartnerSessionID>0815</Authorization:PartnerSessionID>
        //   <Authorization:AuthorizationStatus>NotAuthorized</Authorization:AuthorizationStatus>
        //
        //   <Authorization:StatusCode>
        //     <CommonTypes:Code>017</CommonTypes:Code>
        //     <CommonTypes:Description>Unauthorized Access</CommonTypes:Description>
        //     <CommonTypes:AdditionalInfo>The identification criterion for the provider/operator with the ID "812" doesn't match the given identification information "/C=DE/ST=Thueringen/L=Jena/O=Hubject/OU=GraphDefined GmbH/CN=GraphDefined Software Development/emailAddress=achim.friedland@graphdefined.com" from the certificate.</CommonTypes:AdditionalInfo>
        //   </Authorization:StatusCode>
        //
        // </Authorization:eRoamingAuthorizationStart>


        // <Authorization:eRoamingAuthorizationStart>
        //
        //   <Authorization:PartnerSessionID>0815</Authorization:PartnerSessionID>
        //   <Authorization:AuthorizationStatus>NotAuthorized</Authorization:AuthorizationStatus>
        //
        //   <Authorization:StatusCode>
        //     <CommonTypes:Code>320</CommonTypes:Code>
        //     <CommonTypes:Description>Service not available</CommonTypes:Description>
        //   </Authorization:StatusCode>
        //
        // </Authorization:eRoamingAuthorizationStart>

        // <Authorization:eRoamingAuthorizationStart>
        //
        //   <Authorization:PartnerSessionID>0815</Authorization:PartnerSessionID>
        //   <Authorization:AuthorizationStatus>NotAuthorized</Authorization:AuthorizationStatus>
        //
        //   <Authorization:StatusCode>
        //     <CommonTypes:Code>102</CommonTypes:Code>
        //     <CommonTypes:Description>RFID Authentication failed – invalid UID</CommonTypes:Description>
        //   </Authorization:StatusCode>
        //
        // </Authorization:eRoamingAuthorizationStart>

        #endregion

        #region (static) Parse   (Request, AuthorizationStartXML, CustomMapper = null, OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP authorization start result.
        /// </summary>
        /// <param name="Request">An AuthorizeStartRequest request.</param>
        /// <param name="AuthorizationStartXML">The XML to parse.</param>
        public static AuthorizationStart Parse(AuthorizeStartRequest                              Request,
                                               XElement                                           AuthorizationStartXML,
                                               CustomMapperDelegate<AuthorizationStart, Builder>  CustomMapper  = null,
                                               OnExceptionDelegate                                OnException   = null)


        {

            AuthorizationStart _AuthorizationStart;

            if (TryParse(Request, AuthorizationStartXML, out _AuthorizationStart, CustomMapper, OnException))
                return _AuthorizationStart;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, AuthorizationStartXML, out AuthorizationStart, CustomMapper = null, OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP authorization start result.
        /// </summary>
        /// <param name="Request">An AuthorizeStartRequest request.</param>
        /// <param name="AuthorizationStartXML">The XML to parse.</param>
        public static Boolean TryParse(AuthorizeStartRequest                              Request,
                                       XElement                                           AuthorizationStartXML,
                                       out AuthorizationStart                             AuthorizationStart,
                                       CustomMapperDelegate<AuthorizationStart, Builder>  CustomMapper  = null,
                                       OnExceptionDelegate                                OnException   = null)


        {


            try
            {

                if (AuthorizationStartXML.Name != OICPNS.Authorization + "eRoamingAuthorizationStart")
                    throw new ArgumentException("Invalid eRoamingAuthorizationStart XML");

                AuthorizationStart = new AuthorizationStart(
                                         Request,
                                         (AuthorizationStatusTypes) Enum.Parse(typeof(AuthorizationStatusTypes), AuthorizationStartXML.ElementValueOrFail(OICPNS.Authorization + "AuthorizationStatus")),
                                         AuthorizationStartXML.MapValueOrNullable(OICPNS.Authorization + "SessionID",                        Session_Id.       Parse),
                                         AuthorizationStartXML.MapValueOrNullable(OICPNS.Authorization + "PartnerSessionID",                 PartnerSession_Id.Parse),
                                         AuthorizationStartXML.MapValueOrNullable(OICPNS.Authorization + "ProviderID",                       Provider_Id.      Parse),
                                         AuthorizationStartXML.MapElement        (OICPNS.Authorization + "StatusCode",                       StatusCode.       Parse),
                                         AuthorizationStartXML.MapElements       (OICPNS.Authorization + "AuthorizationStopIdentifications", (XML, e) => AuthorizationIdentification.Parse(XML))
                                     );

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

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OICPNS.Authorization + "eRoamingAuthorizationStart",

                SessionId != null
                    ? new XElement(OICPNS.Authorization + "SessionID",         SessionId.ToString())
                    : null,

                PartnerSessionId != null
                    ? new XElement(OICPNS.Authorization + "PartnerSessionID",  PartnerSessionId.ToString())
                    : null,

                ProviderId != null
                    ? new XElement(OICPNS.Authorization + "ProviderID",        ProviderId.ToString())
                    : null,

                new XElement(OICPNS.Authorization + "AuthorizationStatus",     AuthorizationStatus.ToString()),

                StatusCode.ToXML(),

                AuthorizationStopIdentifications.Any()
                    ? new XElement(OICPNS.Authorization + "AuthorizationStopIdentifications",
                          AuthorizationStopIdentifications.Select(identification => new XElement(OICPNS.Authorization + "Identification", identification.ToXML(OICPNS.Authorization)))
                      )
                    : null

            );

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("AuthorizationStart: " + Result + "; " + StatusCode.Code, " / ", StatusCode.Description, " / ", StatusCode.AdditionalInfo);

        #endregion


        public override bool Equals(AuthorizationStart AResponse)
        {
            throw new NotImplementedException();
        }


        public class Builder : ABuilder
        {

            #region Properties

            public AuthorizeStartRequest                     Request                             { get; set; }

            /// <summary>
            /// The charging session identification.
            /// </summary>
            public Session_Id?                               SessionId                           { get; set; }

            /// <summary>
            /// An optional partner charging session identification.
            /// </summary>
            public PartnerSession_Id?                        PartnerSessionId                    { get; set; }

            /// <summary>
            /// The e-mobility provider identification.
            /// </summary>
            public Provider_Id?                              ProviderId                          { get; set; }

            /// <summary>
            /// The authorization status, e.g. "Authorized".
            /// </summary>
            public AuthorizationStatusTypes                  AuthorizationStatus                 { get; set; }

            /// <summary>
            /// The authorization status code.
            /// </summary>
            public StatusCode                                StatusCode                          { get; set; }

            /// <summary>
            /// An enumeration of authorization identifications.
            /// </summary>
            public IEnumerable<AuthorizationIdentification>  AuthorizationStopIdentifications    { get; set; }

            /// <summary>
            /// The result of the operation.
            /// </summary>
            public Result                                    Result                              { get; set; }

            public Dictionary<String, Object>                CustomData                          { get; set; }

            #endregion

            public Builder(AuthorizationStart AuthorizationStart = null)
            {

                if (AuthorizationStart != null)
                {

                    this.Request                           = AuthorizationStart.Request;
                    this.SessionId                         = AuthorizationStart.SessionId;
                    this.PartnerSessionId                  = AuthorizationStart.PartnerSessionId;
                    this.ProviderId                        = AuthorizationStart.ProviderId;
                    this.AuthorizationStatus               = AuthorizationStart.AuthorizationStatus;
                    this.StatusCode                        = AuthorizationStart.StatusCode;
                    this.AuthorizationStopIdentifications  = AuthorizationStart.AuthorizationStopIdentifications;
                    this.Result                            = AuthorizationStart.Result;
                    this.CustomData                        = new Dictionary<String, Object>();

                    if (AuthorizationStart.CustomData != null)
                        foreach (var item in AuthorizationStart.CustomData)
                            CustomData.Add(item.Key, item.Value);

                }

            }


            //public Acknowledgement<T> ToImmutable()

            //    => new Acknowledgement<T>(Request,
            //                              Result,
            //                              StatusCode,
            //                              SessionId,
            //                              PartnerSessionId,
            //                              CustomData);

        }


    }

}
