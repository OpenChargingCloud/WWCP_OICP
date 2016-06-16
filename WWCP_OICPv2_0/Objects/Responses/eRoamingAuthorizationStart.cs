﻿/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
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

namespace org.GraphDefined.WWCP.OICPv2_0
{

    /// <summary>
    /// An OICP authorization start result.
    /// </summary>
    public class eRoamingAuthorizationStart
    {

        #region Properties

        /// <summary>
        /// The charging session identification.
        /// </summary>
        public ChargingSession_Id                       SessionId                       { get; }

        /// <summary>
        /// An optional partner charging session identification.
        /// </summary>
        public ChargingSession_Id                       PartnerSessionId                { get; }

        /// <summary>
        /// The e-mobility provider identification.
        /// </summary>
        public EVSP_Id                                  ProviderId                      { get; }

        /// <summary>
        /// The authorization status, e.g. "Authorized".
        /// </summary>
        public AuthorizationStatusType                  AuthorizationStatus             { get; }

        /// <summary>
        /// The authorization status code.
        /// </summary>
        public StatusCode                               StatusCode                      { get; }

        /// <summary>
        /// An enumeration of authorization identifications.
        /// </summary>
        public IEnumerable<AuthorizationIdentification> AuthorizationStopIdentifications    { get; }

        #endregion

        #region Constructor(s)

        #region (private) eRoamingAuthorizationStart(AuthorizationStatus, ...)

        /// <summary>
        /// Create a new OICP authorization start result.
        /// </summary>
        /// <param name="AuthorizationStatus">The authorization status.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        /// <param name="StatusCode">An optional status code.</param>
        /// <param name="AuthorizationStopIdentifications">Optional authorization stop identifications.</param>
        private eRoamingAuthorizationStart(AuthorizationStatusType                   AuthorizationStatus,
                                           ChargingSession_Id                        SessionId                         = null,
                                           ChargingSession_Id                        PartnerSessionId                  = null,
                                           EVSP_Id                                   ProviderId                        = null,
                                           StatusCode                                StatusCode                        = null,                                           
                                           IEnumerable<AuthorizationIdentification>  AuthorizationStopIdentifications  = null)
        {

            this.AuthorizationStatus               = AuthorizationStatus;
            this.SessionId                         = SessionId;
            this.PartnerSessionId                  = PartnerSessionId;
            this.ProviderId                        = ProviderId;
            this.StatusCode                        = StatusCode;
            this.AuthorizationStopIdentifications  = AuthorizationStopIdentifications;

        }

        #endregion

        #region eRoamingAuthorizationStart(SessionId, ...)

        /// <summary>
        /// Create a new OICP 'Authorized' authorization start result.
        /// </summary>
        /// <param name="SessionId">The charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="AuthorizationStopIdentifications">Optional authorization stop identifications.</param>
        public eRoamingAuthorizationStart(ChargingSession_Id                        SessionId,
                                          ChargingSession_Id                        PartnerSessionId                  = null,
                                          EVSP_Id                                   ProviderId                        = null,
                                          String                                    StatusCodeDescription             = null,
                                          String                                    StatusCodeAdditionalInfo          = null,
                                          IEnumerable<AuthorizationIdentification>  AuthorizationStopIdentifications  = null)

            : this(AuthorizationStatusType.Authorized,
                   SessionId,
                   PartnerSessionId,
                   ProviderId,
                   new StatusCode(StatusCodes.Success,
                                  StatusCodeDescription,
                                  StatusCodeAdditionalInfo),
                   AuthorizationStopIdentifications)

        { }

        #endregion

        #region eRoamingAuthorizationStart(StatusCode, ...)

        /// <summary>
        /// Create a new OICP 'NotAuthorized' authorization start result.
        /// </summary>
        /// <param name="StatusCode">The status code of the operation.</param>
        /// <param name="StatusCodeDescription">An optional description of the status code.</param>
        /// <param name="StatusCodeAdditionalInfo">An optional additional information for the status code.</param>
        /// <param name="SessionId">An optional charging session identification.</param>
        /// <param name="PartnerSessionId">An optional partner charging session identification.</param>
        /// <param name="ProviderId">An optional e-mobility provider identification.</param>
        public eRoamingAuthorizationStart(StatusCodes         StatusCode,
                                          String              StatusCodeDescription     = null,
                                          String              StatusCodeAdditionalInfo  = null,
                                          ChargingSession_Id  SessionId                 = null,
                                          ChargingSession_Id  PartnerSessionId          = null,
                                          EVSP_Id             ProviderId                = null)

            : this(AuthorizationStatusType.NotAuthorized,
                   SessionId,
                   PartnerSessionId,
                   ProviderId,
                   new StatusCode(StatusCode,
                                  StatusCodeDescription,
                                  StatusCodeAdditionalInfo))

        { }

        #endregion

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
        //       <!--Optional:-->
        //       <Authorization:PartnerSessionID>?</Authorization:PartnerSessionID>
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

        #region (static) Parse(eRoamingAuthorizationStartXML)

        /// <summary>
        /// Parse the given XML representation of an OICP authorization start result.
        /// </summary>
        /// <param name="eRoamingAuthorizationStartXML">The XML to parse.</param>
        public static eRoamingAuthorizationStart Parse(XElement eRoamingAuthorizationStartXML)
        {



            if (eRoamingAuthorizationStartXML.Name != OICPNS.Authorization + "eRoamingAuthorizationStart")
                throw new ArgumentException("Invalid eRoamingAuthorizationStart XML");

            return new eRoamingAuthorizationStart(
                           (AuthorizationStatusType) Enum.Parse(typeof(AuthorizationStatusType), eRoamingAuthorizationStartXML.ElementValueOrFail(OICPNS.Authorization + "AuthorizationStatus")),
                           eRoamingAuthorizationStartXML.MapValueOrNull(OICPNS.Authorization + "SessionID",                        ChargingSession_Id.         Parse),
                           eRoamingAuthorizationStartXML.MapValueOrNull(OICPNS.Authorization + "PartnerSessionID",                 ChargingSession_Id.         Parse),
                           eRoamingAuthorizationStartXML.MapValueOrNull(OICPNS.Authorization + "ProviderID",                       EVSP_Id.                    Parse),
                           eRoamingAuthorizationStartXML.MapElement    (OICPNS.Authorization + "StatusCode",                       StatusCode.                 Parse),
                           eRoamingAuthorizationStartXML.MapElements   (OICPNS.Authorization + "AuthorizationStopIdentifications", (XML, e) => AuthorizationIdentification.Parse(XML))
                       );

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

    }

}
