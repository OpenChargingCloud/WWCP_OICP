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

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// An abstract Hubject Authorization result.
    /// </summary>
    public class eRoamingMobileAuthorizationStart
    {

        #region Properties

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

        #region GeoCoordinates

        private readonly GeoCoordinate _GeoCoordinates;

        /// <summary>
        /// The geo coordinate of the EVSE.
        /// </summary>
        public GeoCoordinate GeoCoordinates
        {
            get
            {
                return _GeoCoordinates;
            }
        }

        #endregion

        #region ChargingStationName

        private readonly I18NString _ChargingStationName;

        /// <summary>
        /// The name of the charging station.
        /// </summary>
        public I18NString ChargingStationName
        {
            get
            {
                return _ChargingStationName;
            }
        }

        #endregion

        #region Address

        private readonly Address _Address;

        /// <summary>
        /// The address of the EVSE.
        /// </summary>
        public Address Address
        {
            get
            {
                return _Address;
            }
        }

        #endregion

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

        #region StatusCode

        private readonly StatusCode _StatusCode;

        /// <summary>
        /// The status code of the request.
        /// </summary>
        public StatusCode StatusCode
        {
            get
            {
                return _StatusCode;
            }
        }

        #endregion

        #region TermsOfUse

        private readonly I18NString _TermsOfUse;

        public I18NString TermsOfUse
        {
            get
            {
                return _TermsOfUse;
            }
        }

        #endregion

        #region AdditionalInfo

        private readonly I18NString _AdditionalInfo;

        public I18NString AdditionalInfo
        {
            get
            {
                return _AdditionalInfo;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region eRoamingMobileAuthorizationStart(AuthorizationStatus, GeoCoordinates, SessionId  = null, StatusCode = null, TermsOfUse = null, Address = null, AdditionalInfo = null, ChargingStationName = null)

        /// <summary>
        /// Create a new OICP v2.0 AuthorizationStart response.
        /// </summary>
        /// <param name="AuthorizationStatus">The status of the mobile authorization request.</param>
        /// <param name="GeoCoordinates">The geo coordinate of the EVSE.</param>
        /// <param name="ChargingStationName">An optional charging station name of the EVSE.</param>
        /// <param name="Address">An optional address of the EVSE.</param>
        /// <param name="SessionId">An optional session identification of the mobile authorization request.</param>
        /// <param name="StatusCode">An optional status code for the mobile authorization request.</param>
        /// <param name="TermsOfUse">An optional multilingual term-of-use text.</param>
        /// <param name="AdditionalInfo">Optional additional information.</param>
        public eRoamingMobileAuthorizationStart(AuthorizationStatusType  AuthorizationStatus,
                                                GeoCoordinate            GeoCoordinates,
                                                I18NString               ChargingStationName  = null,
                                                Address                  Address              = null,
                                                ChargingSession_Id       SessionId            = null,
                                                StatusCode               StatusCode           = null,
                                                I18NString               TermsOfUse           = null,
                                                I18NString               AdditionalInfo       = null)
        {

            #region Initial checks

            if (AuthorizationStatus == null)
                throw new ArgumentNullException("AuthorizationStatus", "The given parameter must not be null!");

            if (GeoCoordinates == null)
                throw new ArgumentNullException("GeoCoordinates", "The given parameter must not be null!");

            #endregion

            this._AuthorizationStatus  = AuthorizationStatus;
            this._GeoCoordinates       = GeoCoordinates;
            this._ChargingStationName  = ChargingStationName != null ? ChargingStationName : new I18NString();
            this._Address              = Address;
            this._SessionId            = SessionId;
            this._StatusCode           = StatusCode          != null ? StatusCode          : new StatusCode(0);
            this._TermsOfUse           = TermsOfUse          != null ? TermsOfUse          : new I18NString();
            this._AdditionalInfo       = AdditionalInfo      != null ? AdditionalInfo      : new I18NString();

        }

        #endregion

        #region eRoamingMobileAuthorizationStart(StatusCode)

        /// <summary>
        /// Create a new group of OICP v2.0 operator EVSE status records or a status code.
        /// </summary>
        /// <pparam name="StatusCode">The status code for this request.</pparam>
        public eRoamingMobileAuthorizationStart(Int16  Code,
                                                String Description     = null,
                                                String AdditionalInfo  = null)
        {

            this._AuthorizationStatus  = AuthorizationStatusType.NotAuthorized;
            this._StatusCode           = new StatusCode(Code, Description, AdditionalInfo);
            this._GeoCoordinates       = GeoCoordinate.Zero;

        }

        #endregion

        #endregion


        #region (static) Parse(eRoamingMobileAuthorizationStartXML)

        public static eRoamingMobileAuthorizationStart Parse(XElement eRoamingMobileAuthorizationStartXML)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv             = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:MobileAuthorization = "http://www.hubject.com/b2b/services/mobileauthorization/v2.0"
            //                   xmlns:CommonTypes         = "http://www.hubject.com/b2b/services/commontypes/v2.0">
            // 
            //    <soapenv:Header/>
            // 
            //    <soapenv:Body>
            //       <MobileAuthorization:eRoamingMobileAuthorizationStart>
            // 
            //          <!--Optional:-->
            //          <MobileAuthorization:SessionID>?</MobileAuthorization:SessionID>
            // 
            //          <MobileAuthorization:AuthorizationStatus>?</MobileAuthorization:AuthorizationStatus>
            // 
            //          <!--Optional:-->
            //          <MobileAuthorization:StatusCode>
            // 
            //             <CommonTypes:Code>?</CommonTypes:Code>
            // 
            //             <!--Optional:-->
            //             <CommonTypes:Description>?</CommonTypes:Description>
            // 
            //             <!--Optional:-->
            //             <CommonTypes:AdditionalInfo>?</CommonTypes:AdditionalInfo>
            // 
            //          </MobileAuthorization:StatusCode>
            // 
            //          <!--Optional:-->
            //          <MobileAuthorization:TermsOfUse>?</MobileAuthorization:TermsOfUse>
            // 
            //          <MobileAuthorization:GeoCoordinates>
            //             <!--You have a CHOICE of the next 3 items at this level. All are WGS84.-->
            // 
            //             <CommonTypes:Google>
            //                <!-- latitude longitude: -?1?\d{1,2}\.\d{1,6}\s*\,?\s*-?1?\d{1,2}\.\d{1,6} -->
            //                <CommonTypes:Coordinates>50.931844 11.625214</CommonTypes:Coordinates>
            //             </CommonTypes:Google>
            // 
            //             <CommonTypes:DecimalDegree>
            //                <!-- -?1?\d{1,2}\.\d{1,6} -->
            //                <CommonTypes:Longitude>11.625214</CommonTypes:Longitude>
            //                <CommonTypes:Latitude >50.931844</CommonTypes:Latitude>
            //             </CommonTypes:DecimalDegree>
            // 
            //             <CommonTypes:DegreeMinuteSeconds>
            //                <!-- -?1?\d{1,2}°[ ]?\d{1,2}'[ ]?\d{1,2}\.\d+'' -->
            //                <CommonTypes:Longitude>11° 37' 30.7704''</CommonTypes:Longitude>
            //                <CommonTypes:Latitude >50° 55' 54.6384''</CommonTypes:Latitude>
            //             </CommonTypes:DegreeMinuteSeconds>
            // 
            //          </MobileAuthorization:GeoCoordinates>
            // 
            //          <!--Optional:-->
            //          <MobileAuthorization:Address>
            // 
            //             <CommonTypes:Country>DE</CommonTypes:Country>
            //             <CommonTypes:City>Jena</CommonTypes:City>
            //             <CommonTypes:Street>Biberweg</CommonTypes:Street>
            // 
            //             <!--Optional:-->
            //             <CommonTypes:PostalCode>07749</CommonTypes:PostalCode>
            //             <!--Optional:-->
            //             <CommonTypes:HouseNum>18</CommonTypes:HouseNum>
            //             <!--Optional:-->
            //             <CommonTypes:Floor>2</CommonTypes:Floor>
            //             <!--Optional:-->
            //             <CommonTypes:Region>?</CommonTypes:Region>
            //             <!--Optional:-->
            //             <CommonTypes:TimeZone>?</CommonTypes:TimeZone>
            // 
            //          </MobileAuthorization:Address>
            // 
            //          <!--Optional:-->
            //          <MobileAuthorization:AdditionalInfo>Nur zum Laden von Spielzeugautos geeignet.</MobileAuthorization:AdditionalInfo>
            //          <!--Optional:-->
            //          <MobileAuthorization:EnAdditionalInfo>For charging electric toy cars only.</MobileAuthorization:EnAdditionalInfo>
            // 
            //          <!--Optional:-->
            //          <MobileAuthorization:ChargingStationName>Testbox1</MobileAuthorization:ChargingStationName>
            //          <!--Optional:-->
            //          <MobileAuthorization:EnChargingStationName>Testbox One</MobileAuthorization:EnChargingStationName>
            // 
            //       </MobileAuthorization:eRoamingMobileAuthorizationStart>
            //    </soapenv:Body>
            // 
            // </soapenv:Envelope>

            #endregion


            if (eRoamingMobileAuthorizationStartXML.Name != OICPNS.MobileAuthorization + "eRoamingMobileAuthorizationStart")
                throw new Exception("Invalid eRoamingMobileAuthorizationStart XML!");

            var ChargingStationName = new I18NString();
            eRoamingMobileAuthorizationStartXML.UseValue(OICPNS.MobileAuthorization + "ChargingStationName",   v => ChargingStationName.Add(Languages.de, v));
            eRoamingMobileAuthorizationStartXML.UseValue(OICPNS.MobileAuthorization + "EnChargingStationName", v => ChargingStationName.Add(Languages.en, v));

            var AdditionalInfo = new I18NString();
            eRoamingMobileAuthorizationStartXML.UseValue(OICPNS.MobileAuthorization + "AdditionalInfo",        v => AdditionalInfo.Add(Languages.de, v));
            eRoamingMobileAuthorizationStartXML.UseValue(OICPNS.MobileAuthorization + "EnAdditionalInfo",      v => AdditionalInfo.Add(Languages.en, v));

            return new eRoamingMobileAuthorizationStart((AuthorizationStatusType) Enum.Parse(typeof(AuthorizationStatusType), eRoamingMobileAuthorizationStartXML.ElementValueOrFail(OICPNS.MobileAuthorization + "AuthorizationStatus")),
                                                        GeoCoordinates:       XMLMethods.ParseGeoCoordinatesXML(eRoamingMobileAuthorizationStartXML.ElementOrFail(OICPNS.MobileAuthorization + "GeoCoordinates", "Missing 'GeoCoordinates'-XML tag!")),
                                                        ChargingStationName:  ChargingStationName,
                                                        Address:              eRoamingMobileAuthorizationStartXML.MapElement       (OICPNS.MobileAuthorization + "Address",    XMLMethods.ParseAddressXML, null),
                                                        SessionId:            eRoamingMobileAuthorizationStartXML.MapValueOrDefault(OICPNS.MobileAuthorization + "SessionID",  ChargingSession_Id.Parse,   null),
                                                        StatusCode:           eRoamingMobileAuthorizationStartXML.MapElement       (OICPNS.MobileAuthorization + "StatusCode", StatusCode.Parse,           null),
                                                        TermsOfUse:           eRoamingMobileAuthorizationStartXML.MapValueOrDefault(OICPNS.MobileAuthorization + "TermsOfUse", s => new I18NString(Languages.de, s), null),
                                                        AdditionalInfo:       AdditionalInfo);

        }

        #endregion


    }

}
