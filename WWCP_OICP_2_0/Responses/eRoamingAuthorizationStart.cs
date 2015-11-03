/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/WorldWideCharging/WWCP_OICP>
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

namespace org.GraphDefined.WWCP.OICP_2_0
{

    /// <summary>
    /// An authorization start result.
    /// </summary>
    public class eRoamingAuthorizationStart
    {

        #region Properties

        #region SessionId

        private readonly ChargingSession_Id _SessionId;

        /// <summary>
        /// The Hubject session identification.
        /// </summary>
        public ChargingSession_Id SessionId
        {
            get
            {
                return _SessionId;
            }
        }

        #endregion

        #region PartnerSessionId

        private readonly ChargingSession_Id _PartnerSessionId;

        /// <summary>
        /// Your own session identification.
        /// </summary>
        public ChargingSession_Id PartnerSessionId
        {
            get
            {
                return _PartnerSessionId;
            }
        }

        #endregion

        #region ProviderId

        private readonly EVSP_Id _ProviderId;

        /// <summary>
        /// The provider identification, e.g. BMW.
        /// </summary>
        public EVSP_Id ProviderId
        {
            get
            {
                return _ProviderId;
            }
        }

        #endregion

        #region AuthorizationStatus

        private readonly AuthorizationStatusType _AuthorizationStatus;

        /// <summary>
        /// The authorization status, e.g. "Authorized".
        /// </summary>
        public AuthorizationStatusType AuthorizationStatus
        {
            get
            {
                return _AuthorizationStatus;
            }
        }

        #endregion

        #region StatusCode

        private readonly StatusCode _StatusCode;

        /// <summary>
        /// The authorization status code.
        /// </summary>
        public StatusCode StatusCode
        {
            get
            {
                return _StatusCode;
            }
        }

        #endregion

        #region AuthorizationIdentifications

        private readonly IEnumerable<AuthorizationIdentification> _AuthorizationIdentifications;

        /// <summary>
        /// An enumeration of authorization identifications.
        /// </summary>
        public IEnumerable<AuthorizationIdentification> AuthorizationIdentifications
        {
            get
            {
                return _AuthorizationIdentifications;
            }
        }

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new authorization start result.
        /// </summary>
        public eRoamingAuthorizationStart(AuthorizationStatusType                   AuthorizationStatus,
                                          ChargingSession_Id                        SessionId                     = null,
                                          ChargingSession_Id                        PartnerSessionId              = null,
                                          EVSP_Id                                   ProviderId                    = null,
                                          StatusCode                                StatusCode                    = null,
                                          IEnumerable<AuthorizationIdentification>  AuthorizationIdentifications  = null)
        {

            this._AuthorizationStatus           = AuthorizationStatus;
            this._SessionId                     = SessionId;
            this._PartnerSessionId              = PartnerSessionId;
            this._ProviderId                    = ProviderId;
            this._StatusCode                    = StatusCode;
            this._AuthorizationIdentifications  = AuthorizationIdentifications;

        }

        #endregion

        #region (static) Parse(eRoamingAuthorizationStartXML)

        public static eRoamingAuthorizationStart Parse(XElement eRoamingAuthorizationStartXML)
        {

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

            if (eRoamingAuthorizationStartXML.Name != OICPNS.Authorization + "eRoamingAuthorizationStart")
                throw new ArgumentException("Invalid eRoamingAuthorizationStart XML");

            return new eRoamingAuthorizationStart(
                           (AuthorizationStatusType) Enum.Parse(typeof(AuthorizationStatusType), eRoamingAuthorizationStartXML.ElementValueOrFail(OICPNS.Authorization + "AuthorizationStatus")),
                           eRoamingAuthorizationStartXML.MapValueOrNull(OICPNS.Authorization + "SessionID",                        ChargingSession_Id.         Parse),
                           eRoamingAuthorizationStartXML.MapValueOrNull(OICPNS.Authorization + "PartnerSessionID",                 ChargingSession_Id.         Parse),
                           eRoamingAuthorizationStartXML.MapValueOrNull(OICPNS.Authorization + "ProviderID",                       EVSP_Id.                    Parse),
                           eRoamingAuthorizationStartXML.MapElement    (OICPNS.Authorization + "StatusCode",                       StatusCode.                 Parse),
                           eRoamingAuthorizationStartXML.MapElements   (OICPNS.Authorization + "AuthorizationStopIdentifications", (XML, e) => AuthorizationIdentification.Parse(XML, e))
                       );

        }

        #endregion

    }

}
