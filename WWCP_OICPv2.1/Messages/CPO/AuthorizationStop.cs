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
using System.Xml.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// An OICP Authorization Stop result.
    /// </summary>
    public class AuthorizationStop
    {

        #region Properties

        /// <summary>
        /// The charging session identification.
        /// </summary>
        public Session_Id?               SessionId             { get; }

        /// <summary>
        /// An optional partner charging session identification.
        /// </summary>
        public PartnerSession_Id?        PartnerSessionId      { get; }

        /// <summary>
        /// The e-mobility provider identification.
        /// </summary>
        public Provider_Id?              ProviderId            { get; }

        /// <summary>
        /// The authorization status, e.g. "Authorized".
        /// </summary>
        public AuthorizationStatusTypes  AuthorizationStatus   { get; }

        /// <summary>
        /// The authorization status code.
        /// </summary>
        public StatusCode                StatusCode            { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OICP Authorization Stop result.
        /// </summary>
        /// <param name="AuthorizationStatus">The authorization status.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        /// <param name="StatusCode">An optional status code.</param>
        private AuthorizationStop(AuthorizationStatusTypes  AuthorizationStatus,
                                  Session_Id?               SessionId          = null,
                                  PartnerSession_Id?        PartnerSessionId   = null,
                                  Provider_Id?              ProviderId         = null,
                                  StatusCode                StatusCode         = null)
        {

            this.AuthorizationStatus  = AuthorizationStatus;
            this.SessionId            = SessionId;
            this.PartnerSessionId     = PartnerSessionId;
            this.ProviderId           = ProviderId;
            this.StatusCode           = StatusCode;

        }

        #endregion


        #region (static) Authorized               (SessionId = null, PartnerSessionId = null, ProviderId = null, ...)

        /// <summary>
        /// Create a new OICP 'Authorized' AuthorizationStop result.
        /// </summary>
        /// <param name="SessionId">The charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        public static AuthorizationStop Authorized(Session_Id?         SessionId                  = null,
                                                   PartnerSession_Id?  PartnerSessionId           = null,
                                                   Provider_Id?        ProviderId                 = null,
                                                   String              StatusCodeDescription      = null,
                                                   String              StatusCodeAdditionalInfo   = null)


            => new AuthorizationStop(AuthorizationStatusTypes.Authorized,
                                     SessionId,
                                     PartnerSessionId,
                                     ProviderId,
                                     new StatusCode(
                                         StatusCodes.Success,
                                         StatusCodeDescription,
                                         StatusCodeAdditionalInfo
                                     ));

        #endregion

        #region (static) NotAuthorized            (StatusCode, StatusCodeDescription = null, ...)

        /// <summary>
        /// Create a new OICP 'NotAuthorized' AuthorizationStop result.
        /// </summary>
        /// <param name="StatusCode">The status code of the operation.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStop NotAuthorized(StatusCodes         StatusCode,
                                                      String              StatusCodeDescription      = null,
                                                      String              StatusCodeAdditionalInfo   = null,
                                                      Session_Id?         SessionId                  = null,
                                                      PartnerSession_Id?  PartnerSessionId           = null,
                                                      Provider_Id?        ProviderId                 = null)

            => new AuthorizationStop(AuthorizationStatusTypes.NotAuthorized,
                                     SessionId,
                                     PartnerSessionId,
                                     ProviderId,
                                     new StatusCode(
                                         StatusCode,
                                         StatusCodeDescription,
                                         StatusCodeAdditionalInfo
                                     ));

        #endregion

        #region (static) SessionIsInvalid         (StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'SessionIsInvalid' AuthorizationStop result.
        /// </summary>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStop SessionIsInvalid(String              StatusCodeDescription      = null,
                                                         String              StatusCodeAdditionalInfo   = null,
                                                         Session_Id?         SessionId                  = null,
                                                         PartnerSession_Id?  PartnerSessionId           = null,
                                                         Provider_Id?        ProviderId                 = null)

            => new AuthorizationStop(AuthorizationStatusTypes.NotAuthorized,
                                     SessionId,
                                     PartnerSessionId,
                                     ProviderId,
                                     new StatusCode(
                                         StatusCodes.SessionIsInvalid,
                                         StatusCodeDescription ?? "Session is invalid",
                                         StatusCodeAdditionalInfo
                                     ));

        #endregion

        #region (static) CommunicationToEVSEFailed(StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'CommunicationToEVSEFailed' AuthorizationStop result.
        /// </summary>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStop CommunicationToEVSEFailed(String              StatusCodeDescription      = null,
                                                                  String              StatusCodeAdditionalInfo   = null,
                                                                  Session_Id?         SessionId                  = null,
                                                                  PartnerSession_Id?  PartnerSessionId           = null,
                                                                  Provider_Id?        ProviderId                 = null)

            => new AuthorizationStop(AuthorizationStatusTypes.NotAuthorized,
                                     SessionId,
                                     PartnerSessionId,
                                     ProviderId,
                                     new StatusCode(
                                         StatusCodes.CommunicationToEVSEFailed,
                                         StatusCodeDescription ?? "Communication to EVSE failed!",
                                         StatusCodeAdditionalInfo
                                     ));

        #endregion

        #region (static) NoEVConnectedToEVSE      (StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'NoEVConnectedToEVSE' AuthorizationStop result.
        /// </summary>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStop NoEVConnectedToEVSE(String              StatusCodeDescription      = null,
                                                            String              StatusCodeAdditionalInfo   = null,
                                                            Session_Id?         SessionId                  = null,
                                                            PartnerSession_Id?  PartnerSessionId           = null,
                                                            Provider_Id?        ProviderId                 = null)

            => new AuthorizationStop(AuthorizationStatusTypes.NotAuthorized,
                                     SessionId,
                                     PartnerSessionId,
                                     ProviderId,
                                     new StatusCode(
                                         StatusCodes.NoEVConnectedToEVSE,
                                         StatusCodeDescription ?? "No EV connected to EVSE!",
                                         StatusCodeAdditionalInfo
                                     ));

        #endregion

        #region (static) EVSEAlreadyReserved      (StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'EVSEAlreadyReserved' AuthorizationStop result.
        /// </summary>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStop EVSEAlreadyReserved(String              StatusCodeDescription      = null,
                                                            String              StatusCodeAdditionalInfo   = null,
                                                            Session_Id?         SessionId                  = null,
                                                            PartnerSession_Id?  PartnerSessionId           = null,
                                                            Provider_Id?        ProviderId                 = null)

            => new AuthorizationStop(AuthorizationStatusTypes.NotAuthorized,
                                     SessionId,
                                     PartnerSessionId,
                                     ProviderId,
                                     new StatusCode(
                                         StatusCodes.EVSEAlreadyReserved,
                                         StatusCodeDescription ?? "EVSE already reserved!",
                                         StatusCodeAdditionalInfo
                                     ));

        #endregion

        #region (static) UnknownEVSEID            (StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'UnknownEVSEID' AuthorizationStop result.
        /// </summary>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStop UnknownEVSEID(String              StatusCodeDescription      = null,
                                                      String              StatusCodeAdditionalInfo   = null,
                                                      Session_Id?         SessionId                  = null,
                                                      PartnerSession_Id?  PartnerSessionId           = null,
                                                      Provider_Id?        ProviderId                 = null)

            => new AuthorizationStop(AuthorizationStatusTypes.NotAuthorized,
                                     SessionId,
                                     PartnerSessionId,
                                     ProviderId,
                                     new StatusCode(
                                         StatusCodes.UnknownEVSEID,
                                         StatusCodeDescription ?? "Unknown EVSE ID!",
                                         StatusCodeAdditionalInfo
                                     ));

        #endregion

        #region (static) EVSEOutOfService         (StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'EVSEOutOfService' AuthorizationStop result.
        /// </summary>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStop EVSEOutOfService(String              StatusCodeDescription      = null,
                                                         String              StatusCodeAdditionalInfo   = null,
                                                         Session_Id?         SessionId                  = null,
                                                         PartnerSession_Id?  PartnerSessionId           = null,
                                                         Provider_Id?        ProviderId                 = null)

            => new AuthorizationStop(AuthorizationStatusTypes.NotAuthorized,
                                     SessionId,
                                     PartnerSessionId,
                                     ProviderId,
                                     new StatusCode(
                                         StatusCodes.EVSEOutOfService,
                                         StatusCodeDescription ?? "EVSE out of service!",
                                         StatusCodeAdditionalInfo
                                     ));

        #endregion

        #region (static) ServiceNotAvailable      (StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'ServiceNotAvailable' AuthorizationStop result.
        /// </summary>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStop ServiceNotAvailable(String              StatusCodeDescription      = null,
                                                            String              StatusCodeAdditionalInfo   = null,
                                                            Session_Id?         SessionId                  = null,
                                                            PartnerSession_Id?  PartnerSessionId           = null,
                                                            Provider_Id?        ProviderId                 = null)

            => new AuthorizationStop(AuthorizationStatusTypes.NotAuthorized,
                                     SessionId,
                                     PartnerSessionId,
                                     ProviderId,
                                     new StatusCode(
                                         StatusCodes.ServiceNotAvailable,
                                         StatusCodeDescription ?? "Service not available!",
                                         StatusCodeAdditionalInfo
                                     ));

        #endregion

        #region (static) DataError                (StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'DataError' AuthorizationStop result.
        /// </summary>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStop DataError(String              StatusCodeDescription      = null,
                                                  String              StatusCodeAdditionalInfo   = null,
                                                  Session_Id?         SessionId                  = null,
                                                  PartnerSession_Id?  PartnerSessionId           = null,
                                                  Provider_Id?        ProviderId                 = null)

            => new AuthorizationStop(AuthorizationStatusTypes.NotAuthorized,
                                     SessionId,
                                     PartnerSessionId,
                                     ProviderId,
                                     new StatusCode(
                                         StatusCodes.DataError,
                                         StatusCodeDescription ?? "Data Error!",
                                         StatusCodeAdditionalInfo
                                     ));

        #endregion

        #region (static) SystemError              (StatusCodeDescription = null, StatusCodeAdditionalInfo = null, ...)

        /// <summary>
        /// Create a new OICP 'SystemError' AuthorizationStop result.
        /// </summary>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public static AuthorizationStop SystemError(String              StatusCodeDescription      = null,
                                                    String              StatusCodeAdditionalInfo   = null,
                                                    Session_Id?         SessionId                  = null,
                                                    PartnerSession_Id?  PartnerSessionId           = null,
                                                    Provider_Id?        ProviderId                 = null)

            => new AuthorizationStop(AuthorizationStatusTypes.NotAuthorized,
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
        //    <soapenv:Header/>
        // 
        //    <soapenv:Body>
        //       <Authorization:eRoamingAuthorizationStop>
        // 
        //          <!--Optional:-->
        //          <Authorization:SessionID>de164e08-1c88-1293-537b-be355041070e</Authorization:SessionID>
        // 
        //          <!--Optional:-->
        //          <Authorization:PartnerSessionID>0815</Authorization:PartnerSessionID>
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

        #region (static) Parse(AuthorizationStopXML)

        /// <summary>
        /// Parse the given XML representation of an OICP authorization stop result.
        /// </summary>
        /// <param name="AuthorizationStopXML">The XML to parse.</param>
        public static AuthorizationStop Parse(XElement AuthorizationStopXML)
        {

            if (AuthorizationStopXML.Name != OICPNS.Authorization + "eRoamingAuthorizationStop")
                throw new ArgumentException("Invalid eRoamingAuthorizationStop XML");

            return new AuthorizationStop(
                           (AuthorizationStatusTypes) Enum.Parse(typeof(AuthorizationStatusTypes), AuthorizationStopXML.ElementValueOrFail(OICPNS.Authorization + "AuthorizationStatus")),
                           AuthorizationStopXML.MapValueOrNullable(OICPNS.Authorization + "SessionID",         Session_Id.       Parse),
                           AuthorizationStopXML.MapValueOrNullable(OICPNS.Authorization + "PartnerSessionID",  PartnerSession_Id.Parse),
                           AuthorizationStopXML.MapValueOrNullable(OICPNS.Authorization + "ProviderID",        Provider_Id.      Parse),
                           AuthorizationStopXML.MapElement        (OICPNS.Authorization + "StatusCode",        StatusCode.       Parse)
                       );

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OICPNS.Authorization + "eRoamingAuthorizationStop",

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

                StatusCode.ToXML()

            );

        #endregion

    }

}
