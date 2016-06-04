/*
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
using System.Xml.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
{

    /// <summary>
    /// An authorization stop result.
    /// </summary>
    public class eRoamingAuthorizationStop
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

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new authorization stop result.
        /// </summary>
        public eRoamingAuthorizationStop(AuthorizationStatusType  AuthorizationStatus,
                                         ChargingSession_Id       SessionId         = null,
                                         ChargingSession_Id       PartnerSessionId  = null,
                                         EVSP_Id                  ProviderId        = null,
                                         StatusCode               StatusCode        = null)
        {

            this._AuthorizationStatus  = AuthorizationStatus;
            this._SessionId            = SessionId;
            this._PartnerSessionId     = PartnerSessionId;
            this._ProviderId           = ProviderId;
            this._StatusCode           = StatusCode;

        }

        #endregion


        #region (static) Parse(eRoamingAuthorizationStopXML)

        public static eRoamingAuthorizationStop Parse(XElement eRoamingAuthorizationStopXML)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv       = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:Authorization = "http://www.hubject.com/b2b/services/authorization/v2.0"
            //                   xmlns:CommonTypes   = "http://www.hubject.com/b2b/services/commontypes/v2.0">
            //
            // [...]
            //
            //    <Authorization:eRoamingAuthorizationStop>
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
            //    </Authorization:eRoamingAuthorizationStop>
            //
            // [...]
            //
            // </soapenv:Envelope>



            // <Authorization:eRoamingAuthorizationStop>
            //
            //   <Authorization:AuthorizationStatus>NotAuthorized</Authorization:AuthorizationStatus>
            //
            //   <Authorization:StatusCode>
            //     <CommonTypes:Code>102</CommonTypes:Code>
            //     <CommonTypes:Description>RFID Authentication failed – invalid UID</CommonTypes:Description>
            //   </Authorization:StatusCode>
            //
            // </Authorization:eRoamingAuthorizationStop>



            // <Authorization:eRoamingAuthorizationStop>
            //
            //   <Authorization:SessionID>8f9cbd74-0a88-1296-1078-6e9cca762de2</Authorization:SessionID>
            //   <Authorization:PartnerSessionID>0815</Authorization:PartnerSessionID>
            //   <Authorization:AuthorizationStatus>NotAuthorized</Authorization:AuthorizationStatus>
            //
            //   <Authorization:StatusCode>
            //     <CommonTypes:Code>400</CommonTypes:Code>
            //     <CommonTypes:Description>Session is invalid</CommonTypes:Description>
            //   </Authorization:StatusCode>
            //
            // </Authorization:eRoamingAuthorizationStop>

            #endregion

            if (eRoamingAuthorizationStopXML.Name != OICPNS.Authorization + "eRoamingAuthorizationStop")
                throw new ArgumentException("Invalid eRoamingAuthorizationStop XML");

            return new eRoamingAuthorizationStop(
                           (AuthorizationStatusType) Enum.Parse(typeof(AuthorizationStatusType), eRoamingAuthorizationStopXML.ElementValueOrFail(OICPNS.Authorization + "AuthorizationStatus")),
                           eRoamingAuthorizationStopXML.MapValueOrNull(OICPNS.Authorization + "SessionID",                        ChargingSession_Id.         Parse),
                           eRoamingAuthorizationStopXML.MapValueOrNull(OICPNS.Authorization + "PartnerSessionID",                 ChargingSession_Id.         Parse),
                           eRoamingAuthorizationStopXML.MapValueOrNull(OICPNS.Authorization + "ProviderID",                       EVSP_Id.                    Parse),
                           eRoamingAuthorizationStopXML.MapElement    (OICPNS.Authorization + "StatusCode",                       StatusCode.                 Parse)
                       );

        }

        #endregion


    }

}
