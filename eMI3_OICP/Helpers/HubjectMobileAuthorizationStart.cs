/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@belectric.com>
 * This file is part of eMI3 OICP <http://www.github.com/eMI3/OICP-Bindings>
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

#endregion

namespace org.emi3group.IO.OICP
{

    #region HubjectMobileAuthorization

    /// <summary>
    /// An abstract Hubject Authorization result.
    /// </summary>
    public class HubjectMobileAuthorizationStart
    {

        #region Properties

        #region SessionID

        private readonly SessionId _SessionID;

        /// <summary>
        /// The Hubject session identification.
        /// </summary>
        public SessionId SessionID
        {
            get
            {
                return _SessionID;
            }
        }

        #endregion

        #region PartnerSessionID

        private readonly String _PartnerSessionID;

        /// <summary>
        /// Your own session identification.
        /// </summary>
        public String PartnerSessionID
        {
            get
            {
                return _PartnerSessionID;
            }
        }

        #endregion

        #region ProviderID

        private readonly String _ProviderID;

        /// <summary>
        /// The provider identification, e.g. BMW.
        /// </summary>
        public String ProviderID
        {
            get
            {
                return _ProviderID;
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

        #region Code

        private readonly UInt16 _Code;

        /// <summary>
        /// The result code.
        /// </summary>
        public UInt16 Code
        {
            get
            {
                return _Code;
            }
        }

        #endregion

        #region Description

        private readonly String _Description;

        /// <summary>
        /// A description of the result.
        /// </summary>
        public String Description
        {
            get
            {
                return _Description;
            }
        }

        #endregion

        #region AdditionalInfo

        private readonly String _AdditionalInfo;

        /// <summary>
        /// An additional information on the result.
        /// </summary>
        public String AdditionalInfo
        {
            get
            {
                return _AdditionalInfo;
            }
        }

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new abstract Hubject Authorization Start result.
        /// </summary>
        /// <param name="AuthorizationType">The type of authorization [start|stop].</param>
        /// <param name="XML">The XML to parse.</param>
        public HubjectMobileAuthorizationStart(XElement XML)
        {

            var HubjectMobileAuthorizationStart = XML.Descendants(NS.OICPv1MobileAuthorization + "HubjectMobileAuthorizationStart").FirstOrDefault();


            // <?xml version='1.0' encoding='UTF-8'?>
            // <isns:Envelope xmlns:cmn  = "http://www.inubit.com/eMobility/SBP/CommonTypes"
            //                xmlns:isns = "http://schemas.xmlsoap.org/soap/envelope/"
            //                xmlns:ns   = "http://www.hubject.com/b2b/services/commontypes/v1"
            //                xmlns:sbp  = "http://www.inubit.com/eMobility/SBP"
            //                xmlns:tns  = "http://www.hubject.com/b2b/services/evsedata/v1"
            //                xmlns:v1   = "http://www.hubject.com/b2b/services/commontypes/v1"
            //                xmlns:wsc  = "http://www.hubject.com/b2b/services/mobileauthorization/v1">
            //
            //   <isns:Body>
            //     <wsc:HubjectMobileAuthorizationStart>
            //
            //       <wsc:AuthorizationStatus>NotAuthorized</wsc:AuthorizationStatus>
            //
            //       <wsc:StatusCode>
            //         <v1:Code>101</v1:Code>
            //         <v1:Description>QR-Code Authentication failed - Invalid Credentials</v1:Description>
            //       </wsc:StatusCode>
            //
            //       <wsc:GeoCoordinates>
            //         <v1:DecimalDegree>
            //           <v1:Longitude>0.000000</v1:Longitude>
            //           <v1:Latitude>0.000000</v1:Latitude>
            //         </v1:DecimalDegree>
            //       </wsc:GeoCoordinates>
            //
            //     </wsc:HubjectMobileAuthorizationStart>
            //   </isns:Body>
            //
            // </isns:Envelope>

            // <?xml version='1.0' encoding='UTF-8'?>
            // <isns:Envelope xmlns:cmn  = "http://www.inubit.com/eMobility/SBP/CommonTypes"
            //                xmlns:isns = "http://schemas.xmlsoap.org/soap/envelope/"
            //                xmlns:ns   = "http://www.hubject.com/b2b/services/commontypes/v1"
            //                xmlns:sbp  = "http://www.inubit.com/eMobility/SBP"
            //                xmlns:tns  = "http://www.hubject.com/b2b/services/evsedata/v1"
            //                xmlns:v1   = "http://www.hubject.com/b2b/services/commontypes/v1"
            //                xmlns:wsc  = "http://www.hubject.com/b2b/services/mobileauthorization/v1">
            //
            //   <isns:Body>
            //     <wsc:HubjectMobileAuthorizationStart>
            //
            //       <wsc:SessionID>2cfc3548-0a88-1296-7141-df2c5e1864d3</wsc:SessionID>
            //       <wsc:AuthorizationStatus>Authorized</wsc:AuthorizationStatus>
            //
            //       <wsc:StatusCode>
            //         <v1:Code>000</v1:Code>
            //         <v1:Description>Success</v1:Description>
            //       </wsc:StatusCode>
            //
            //       <wsc:GeoCoordinates>
            //         <v1:DecimalDegree>
            //           <v1:Longitude>10.144537</v1:Longitude>
            //           <v1:Latitude>49.729122</v1:Latitude>
            //         </v1:DecimalDegree>
            //       </wsc:GeoCoordinates>
            //
            //       <wsc:Address>
            //         <v1:Country>DEU</v1:Country>
            //         <v1:City>Kitzingen</v1:City>
            //         <v1:Street>Steigweg</v1:Street>
            //         <v1:PostalCode>97318</v1:PostalCode>
            //         <v1:HouseNum>24</v1:HouseNum>
            //       </wsc:Address>
            //
            //       <wsc:ChargingStationName>Innopark Kitzingen</wsc:ChargingStationName>
            //       <wsc:EnChargingStationName>Innopark Kitzingen</wsc:EnChargingStationName>
            //
            //     </wsc:HubjectMobileAuthorizationStart>
            //   </isns:Body>
            // </isns:Envelope>

            this._SessionID = SessionId.Parse((HubjectMobileAuthorizationStart.Element(NS.OICPv1MobileAuthorization + "SessionID") != null)
                                             ? HubjectMobileAuthorizationStart.Element(NS.OICPv1MobileAuthorization + "SessionID").Value
                                             : "");

            var AuthorizationStatus = HubjectMobileAuthorizationStart.Element(NS.OICPv1MobileAuthorization + "AuthorizationStatus");




            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:v1      = "http://www.hubject.com/b2b/services/mobileauthorization/v1"
            //                   xmlns:v11     = "http://www.hubject.com/b2b/services/commontypes/v1">
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <v1:HubjectMobileAuthorizationStart>
            //          <!--Optional:-->
            //          <v1:SessionID>?</v1:SessionID>
            //          <v1:AuthorizationStatus>?</v1:AuthorizationStatus>
            //          <!--Optional:-->
            //          <v1:StatusCode>
            //             <v11:Code>?</v11:Code>
            //             <!--Optional:-->
            //             <v11:Description>?</v11:Description>
            //             <!--Optional:-->
            //             <v11:AdditionalInfo>?</v11:AdditionalInfo>
            //          </v1:StatusCode>
            //          <!--Optional:-->
            //          <v1:TermsOfUse>?</v1:TermsOfUse>
            //          <v1:GeoCoordinates>
            //             <!--You have a CHOICE of the next 3 items at this level-->
            //             <v11:Google>
            //                <v11:Coordinates>?</v11:Coordinates>
            //             </v11:Google>
            //             <v11:DecimalDegree>
            //                <v11:Longitude>?</v11:Longitude>
            //                <v11:Latitude>?</v11:Latitude>
            //             </v11:DecimalDegree>
            //             <v11:DegreeMinuteSeconds>
            //                <v11:Longitude>?</v11:Longitude>
            //                <v11:Latitude>?</v11:Latitude>
            //             </v11:DegreeMinuteSeconds>
            //          </v1:GeoCoordinates>
            //          <!--Optional:-->
            //          <v1:Address>
            //             <v11:Country>?</v11:Country>
            //             <v11:City>?</v11:City>
            //             <v11:Street>?</v11:Street>
            //             <!--Optional:-->
            //             <v11:PostalCode>?</v11:PostalCode>
            //             <!--Optional:-->
            //             <v11:HouseNum>?</v11:HouseNum>
            //             <!--Optional:-->
            //             <v11:Floor>?</v11:Floor>
            //             <!--Optional:-->
            //             <v11:Region>?</v11:Region>
            //             <!--Optional:-->
            //             <v11:TimeZone>?</v11:TimeZone>
            //          </v1:Address>
            //          <!--Optional:-->
            //          <v1:AdditionalInfo>?</v1:AdditionalInfo>
            //          <!--Optional:-->
            //          <v1:EnAdditionalInfo>?</v1:EnAdditionalInfo>
            //          <!--Optional:-->
            //          <v1:ChargingStationName>?</v1:ChargingStationName>
            //          <!--Optional:-->
            //          <v1:EnChargingStationName>?</v1:EnChargingStationName>
            //       </v1:HubjectMobileAuthorizationStart>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            //this._SessionID           = SessionId .Parse((ack.Element(NS.OICPv1Authorization + "SessionID") != null)  ? ack.Element(NS.OICPv1Authorization + "SessionID"). Value : "");
            //this._PartnerSessionID    =  ack.Element(NS.OICPv1Authorization + "PartnerSessionID").Value;
            //this._ProviderID          = (ack.Element(NS.OICPv1Authorization + "ProviderID") != null) ? ack.Element(NS.OICPv1Authorization + "ProviderID").Value : "";
            //this._AuthorizationStatus = (ack.Element(NS.OICPv1Authorization + "AuthorizationStatus").Value.ToLower() == "authorized") ? AuthorizationStatusType.Authorized : AuthorizationStatusType.NotAuthorized;

            //var StatusCode            = ack.Element(NS.OICPv1Authorization + "StatusCode");
            //this._Code                = UInt16.Parse(StatusCode.Element(NS.OICPv1CommonTypes + "Code").Value);
            //this._Description         =  StatusCode.Element(NS.OICPv1CommonTypes + "Description").Value;
            //this._AdditionalInfo      = (StatusCode.Element(NS.OICPv1CommonTypes + "AdditionalInfo") != null) ? StatusCode.Element(NS.OICPv1CommonTypes + "AdditionalInfo").Value : String.Empty;

        }

        #endregion

        #region (static) Parse(XML)

        /// <summary>
        /// Create a new Hubject Mobile Authorization Start result.
        /// </summary>
        /// <param name="XML">The XML to parse.</param>
        public static HubjectMobileAuthorizationStart Parse(XElement XML)
        {
            try
            {
                return new HubjectMobileAuthorizationStart(XML);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return Description;
        }

        #endregion

    }

    #endregion

}
