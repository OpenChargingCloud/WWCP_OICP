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
    /// An OICP Authorization Start response.
    /// </summary>
    public class MobileAuthorizationStart
    {

        #region Properties

        /// <summary>
        /// The authorization status, e.g. "Authorized".
        /// </summary>
        public AuthorizationStatusType  AuthorizationStatus     { get; }

        /// <summary>
        /// The geo coordinate of the EVSE.
        /// </summary>
        public GeoCoordinate            GeoCoordinates          { get; }

        /// <summary>
        /// The name of the charging station.
        /// </summary>
        public I18NString               ChargingStationName     { get; }

        /// <summary>
        /// The address of the EVSE.
        /// </summary>
        public Address                  Address                 { get; }

        /// <summary>
        /// The Hubject session identification.
        /// </summary>
        public ChargingSession_Id       SessionId               { get; }

        /// <summary>
        /// The status code of the request.
        /// </summary>
        public StatusCode               StatusCode              { get; }

        /// <summary>
        /// The terms of use.
        /// </summary>
        public I18NString               TermsOfUse              { get; }

        /// <summary>
        /// Additional information.
        /// </summary>
        public I18NString               AdditionalInfo          { get; }

        #endregion

        #region Constructor(s)

        #region MobileAuthorizationStart(AuthorizationStatus, GeoCoordinates, SessionId  = null, StatusCode = null, TermsOfUse = null, Address = null, AdditionalInfo = null, ChargingStationName = null)

        /// <summary>
        /// Create a new OICP Authorization Start response.
        /// </summary>
        /// <param name="AuthorizationStatus">The status of the mobile authorization request.</param>
        /// <param name="GeoCoordinates">The geo coordinate of the EVSE.</param>
        /// <param name="ChargingStationName">An optional charging station name of the EVSE.</param>
        /// <param name="Address">An optional address of the EVSE.</param>
        /// <param name="SessionId">An optional session identification of the mobile authorization request.</param>
        /// <param name="StatusCode">An optional status code for the mobile authorization request.</param>
        /// <param name="TermsOfUse">An optional multilingual term-of-use text.</param>
        /// <param name="AdditionalInfo">Optional additional information.</param>
        public MobileAuthorizationStart(AuthorizationStatusType  AuthorizationStatus,
                                        GeoCoordinate            GeoCoordinates,
                                        I18NString               ChargingStationName  = null,
                                        Address                  Address              = null,
                                        ChargingSession_Id       SessionId            = null,
                                        StatusCode               StatusCode           = null,
                                        I18NString               TermsOfUse           = null,
                                        I18NString               AdditionalInfo       = null)
        {

            #region Initial checks

            if (GeoCoordinates == null)
                throw new ArgumentNullException(nameof(GeoCoordinates),  "The given geo coordinates must not be null!");

            #endregion

            this.AuthorizationStatus  = AuthorizationStatus;
            this.GeoCoordinates       = GeoCoordinates;
            this.ChargingStationName  = ChargingStationName ?? new I18NString();
            this.Address              = Address;
            this.SessionId            = SessionId;
            this.StatusCode           = StatusCode          ?? new StatusCode(StatusCodes.Success);
            this.TermsOfUse           = TermsOfUse          ?? new I18NString();
            this.AdditionalInfo       = AdditionalInfo      ?? new I18NString();

        }

        #endregion

        #region MobileAuthorizationStart(StatusCode, ...)

        /// <summary>
        /// Create a new OICP Authorization Start response.
        /// </summary>
        /// <pparam name="StatusCode">The status code for this request.</pparam>
        public MobileAuthorizationStart(StatusCodes  StatusCode,
                                        String       Description     = null,
                                        String       AdditionalInfo  = null)
        {

            this.AuthorizationStatus  = AuthorizationStatusType.NotAuthorized;
            this.StatusCode           = new StatusCode(StatusCode, Description, AdditionalInfo);
            this.GeoCoordinates       = GeoCoordinate.Zero;

        }

        #endregion

        #endregion


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

        #region (static) Parse(MobileAuthorizationStartXML)

        /// <summary>
        /// Parse the givem XML as an OICP Authorization Start response.
        /// </summary>
        /// <param name="MobileAuthorizationStartXML">A XML representation of an OICP Authorization Start response.</param>
        public static MobileAuthorizationStart Parse(XElement MobileAuthorizationStartXML)
        {

            if (MobileAuthorizationStartXML.Name != OICPNS.MobileAuthorization + "eRoamingMobileAuthorizationStart")
                throw new Exception("Invalid eRoamingMobileAuthorizationStart XML!");

            var ChargingStationName = new I18NString();
            MobileAuthorizationStartXML.UseValue(OICPNS.MobileAuthorization + "ChargingStationName",   v => ChargingStationName.Add(Languages.de, v));
            MobileAuthorizationStartXML.UseValue(OICPNS.MobileAuthorization + "EnChargingStationName", v => ChargingStationName.Add(Languages.en, v));

            var AdditionalInfo = new I18NString();
            MobileAuthorizationStartXML.UseValue(OICPNS.MobileAuthorization + "AdditionalInfo",        v => AdditionalInfo.Add(Languages.de, v));
            MobileAuthorizationStartXML.UseValue(OICPNS.MobileAuthorization + "EnAdditionalInfo",      v => AdditionalInfo.Add(Languages.en, v));

            return new MobileAuthorizationStart((AuthorizationStatusType) Enum.Parse(typeof(AuthorizationStatusType), MobileAuthorizationStartXML.ElementValueOrFail(OICPNS.MobileAuthorization + "AuthorizationStatus")),
                                                GeoCoordinates:       XMLMethods.ParseGeoCoordinatesXML(MobileAuthorizationStartXML.ElementOrFail(OICPNS.MobileAuthorization + "GeoCoordinates", "Missing 'GeoCoordinates'-XML tag!")),
                                                ChargingStationName:  ChargingStationName,
                                                Address:              MobileAuthorizationStartXML.MapElement       (OICPNS.MobileAuthorization + "Address",    XMLMethods.ParseAddressXML, null),
                                                SessionId:            MobileAuthorizationStartXML.MapValueOrDefault(OICPNS.MobileAuthorization + "SessionID",  ChargingSession_Id.Parse,   null),
                                                StatusCode:           MobileAuthorizationStartXML.MapElement       (OICPNS.MobileAuthorization + "StatusCode", StatusCode.Parse,           null),
                                                TermsOfUse:           MobileAuthorizationStartXML.MapValueOrDefault(OICPNS.MobileAuthorization + "TermsOfUse", s => new I18NString(Languages.de, s), null),
                                                AdditionalInfo:       AdditionalInfo);

        }

        #endregion


    }

}
