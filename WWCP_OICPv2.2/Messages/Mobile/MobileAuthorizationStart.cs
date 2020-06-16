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

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2.Mobile
{

    /// <summary>
    /// An OICP Authorization Start response.
    /// </summary>
    public class MobileAuthorizationStart : AResponse<MobileAuthorizeStartRequest,
                                                      MobileAuthorizationStart>
    {

        #region Properties

        /// <summary>
        /// The authorization status, e.g. "Authorized".
        /// </summary>
        public AuthorizationStatusTypes  AuthorizationStatus     { get; }

        /// <summary>
        /// The geo coordinate of the EVSE.
        /// </summary>
        public GeoCoordinate             GeoCoordinates          { get; }

        /// <summary>
        /// The name of the charging station.
        /// </summary>
        public I18NString                ChargingStationName     { get; }

        /// <summary>
        /// The address of the EVSE.
        /// </summary>
        public Address                   Address                 { get; }

        /// <summary>
        /// The Hubject session identification.
        /// </summary>
        public Session_Id?               SessionId               { get; }

        /// <summary>
        /// The status code of the request.
        /// </summary>
        public StatusCode?               StatusCode              { get; }

        /// <summary>
        /// The terms of use.
        /// </summary>
        public I18NString                TermsOfUse              { get; }

        /// <summary>
        /// Additional information.
        /// </summary>
        public I18NString                AdditionalInfo          { get; }

        #endregion

        #region Constructor(s)

        #region MobileAuthorizationStart(Request, AuthorizationStatus, GeoCoordinates, SessionId  = null, StatusCode = null, TermsOfUse = null, Address = null, AdditionalInfo = null, ChargingStationName = null)

        /// <summary>
        /// Create a new OICP Authorization Start response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="AuthorizationStatus">The status of the mobile authorization request.</param>
        /// <param name="GeoCoordinates">The geo coordinate of the EVSE.</param>
        /// <param name="ChargingStationName">An optional charging station name of the EVSE.</param>
        /// <param name="Address">An optional address of the EVSE.</param>
        /// <param name="SessionId">An optional session identification of the mobile authorization request.</param>
        /// <param name="StatusCode">An optional status code for the mobile authorization request.</param>
        /// <param name="TermsOfUse">An optional multilingual term-of-use text.</param>
        /// <param name="AdditionalInfo">Optional additional information.</param>
        /// <param name="CustomData">Optional custom data.</param>
        public MobileAuthorizationStart(MobileAuthorizeStartRequest          Request,
                                        AuthorizationStatusTypes             AuthorizationStatus,
                                        GeoCoordinate                        GeoCoordinates,
                                        I18NString                           ChargingStationName  = null,
                                        Address                              Address              = null,
                                        Session_Id?                          SessionId            = null,
                                        StatusCode?                          StatusCode           = null,
                                        I18NString                           TermsOfUse           = null,
                                        I18NString                           AdditionalInfo       = null,
                                        IReadOnlyDictionary<String, Object>  CustomData           = null)

            : base(Request,
                   CustomData)

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
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="StatusCode">The status code for this request.</pparam>
        /// <param name="Description">An optional description of the status code.</param>
        /// <param name="AdditionalInfo">An optional additional information to the status code.</param>
        /// <param name="CustomData">Optional custom data.</param>
        public MobileAuthorizationStart(MobileAuthorizeStartRequest          Request,
                                        StatusCodes                          StatusCode,
                                        String                               Description     = null,
                                        String                               AdditionalInfo  = null,
                                        IReadOnlyDictionary<String, Object>  CustomData      = null)

            : base(Request,
                   CustomData)

        {

            this.AuthorizationStatus  = AuthorizationStatusTypes.NotAuthorized;

            this.StatusCode           = new StatusCode(StatusCode,
                                                       Description,
                                                       AdditionalInfo);

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

        #region (static) Parse   (Request, MobileAuthorizationStartXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP mobile authorization startes request.
        /// </summary>
        /// <param name="Request">An PullMobileAuthorizationStart request.</param>
        /// <param name="MobileAuthorizationStartXML">The XML to parse.</param>
        /// <param name="CustomMobileAuthorizationStartParser">A delegate to customize the deserialization of MobileAuthorizationStart responses.</param>
        /// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static MobileAuthorizationStart

            Parse(MobileAuthorizeStartRequest                        Request,
                  XElement                                           MobileAuthorizationStartXML,
                  CustomXMLParserDelegate<MobileAuthorizationStart>  CustomMobileAuthorizationStartParser   = null,
                  CustomXMLParserDelegate<Address>                   CustomAddressParser                    = null,
                  CustomXMLParserDelegate<StatusCode>                CustomStatusCodeParser                 = null,
                  OnExceptionDelegate                                OnException                            = null)

        {

            if (TryParse(Request,
                         MobileAuthorizationStartXML,
                         out MobileAuthorizationStart _MobileAuthorizationStart,
                         CustomMobileAuthorizationStartParser,
                         CustomAddressParser,
                         CustomStatusCodeParser,
                         OnException))

                return _MobileAuthorizationStart;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, MobileAuthorizationStartText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP mobile authorization startes request.
        /// </summary>
        /// <param name="Request">An PullMobileAuthorizationStart request.</param>
        /// <param name="MobileAuthorizationStartText">The text to parse.</param>
        /// <param name="CustomMobileAuthorizationStartParser">A delegate to customize the deserialization of MobileAuthorizationStart responses.</param>
        /// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static MobileAuthorizationStart

            Parse(MobileAuthorizeStartRequest                        Request,
                  String                                             MobileAuthorizationStartText,
                  CustomXMLParserDelegate<MobileAuthorizationStart>  CustomMobileAuthorizationStartParser   = null,
                  CustomXMLParserDelegate<Address>                   CustomAddressParser                    = null,
                  CustomXMLParserDelegate<StatusCode>                CustomStatusCodeParser                 = null,
                  OnExceptionDelegate                                OnException                            = null)

        {

            if (TryParse(Request,
                         MobileAuthorizationStartText,
                         out MobileAuthorizationStart _MobileAuthorizationStart,
                         CustomMobileAuthorizationStartParser,
                         CustomAddressParser,
                         CustomStatusCodeParser,
                         OnException))

                return _MobileAuthorizationStart;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, MobileAuthorizationStartXML,  out MobileAuthorizationStart, ..., OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP mobile authorization startes request.
        /// </summary>
        /// <param name="Request">An PullMobileAuthorizationStart request.</param>
        /// <param name="MobileAuthorizationStartXML">The XML to parse.</param>
        /// <param name="MobileAuthorizationStart">The parsed MobileAuthorizationStart request.</param>
        /// <param name="CustomMobileAuthorizationStartParser">A delegate to customize the deserialization of MobileAuthorizationStart responses.</param>
        /// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(MobileAuthorizeStartRequest                        Request,
                                       XElement                                           MobileAuthorizationStartXML,
                                       out MobileAuthorizationStart                       MobileAuthorizationStart,
                                       CustomXMLParserDelegate<MobileAuthorizationStart>  CustomMobileAuthorizationStartParser   = null,
                                       CustomXMLParserDelegate<Address>                   CustomAddressParser                    = null,
                                       CustomXMLParserDelegate<StatusCode>                CustomStatusCodeParser                 = null,
                                       OnExceptionDelegate                                OnException                            = null)
        {

            try
            {

                if (MobileAuthorizationStartXML.Name != OICPNS.MobileAuthorization + "eRoamingMobileAuthorizationStart")
                {
                    MobileAuthorizationStart = null;
                    return false;
                }

                var ChargingStationName  = new I18NString();
                MobileAuthorizationStartXML.IfValueIsNotNullOrEmpty(OICPNS.MobileAuthorization + "ChargingStationName",
                                                                    v => ChargingStationName.Add(Languages.deu, v));
                MobileAuthorizationStartXML.IfValueIsNotNullOrEmpty(OICPNS.MobileAuthorization + "EnChargingStationName",
                                                                    v => ChargingStationName.Add(Languages.eng, v));

                var AdditionalInfo       = new I18NString();
                MobileAuthorizationStartXML.IfValueIsNotNullOrEmpty(OICPNS.MobileAuthorization + "AdditionalInfo",
                                                                    v => AdditionalInfo.     Add(Languages.deu, v));
                MobileAuthorizationStartXML.IfValueIsNotNullOrEmpty(OICPNS.MobileAuthorization + "EnAdditionalInfo",
                                                                    v => AdditionalInfo.     Add(Languages.eng, v));


                MobileAuthorizationStart  = new MobileAuthorizationStart(

                                                Request,

                                                (AuthorizationStatusTypes) Enum.Parse(typeof(AuthorizationStatusTypes),

                                                MobileAuthorizationStartXML.ElementValueOrFail(OICPNS.MobileAuthorization + "AuthorizationStatus")),

                                                MobileAuthorizationStartXML.MapElementOrFail  (OICPNS.MobileAuthorization + "GeoCoordinates",
                                                                                               XML_IO.ParseGeoCoordinatesXML),

                                                ChargingStationName,

                                                MobileAuthorizationStartXML.MapElement        (OICPNS.MobileAuthorization + "Address",
                                                                                               (xml, e) => Address.Parse(xml,
                                                                                                                         CustomAddressParser,
                                                                                                                         e),
                                                                                               OnException),

                                                MobileAuthorizationStartXML.MapValueOrNullable(OICPNS.MobileAuthorization + "SessionID",
                                                                                               Session_Id.Parse),

                                                MobileAuthorizationStartXML.MapElement        (OICPNS.MobileAuthorization + "StatusCode",
                                                                                               (xml, e) => OICPv2_2.StatusCode.Parse(xml,
                                                                                                                                     CustomStatusCodeParser,
                                                                                                                                     e),
                                                                                               OnException),

                                                MobileAuthorizationStartXML.MapValueOrDefault (OICPNS.MobileAuthorization + "TermsOfUse",
                                                                                               s => new I18NString(Languages.unknown, s)),

                                                AdditionalInfo);


                if (CustomMobileAuthorizationStartParser != null)
                    MobileAuthorizationStart = CustomMobileAuthorizationStartParser(MobileAuthorizationStartXML,
                                                        MobileAuthorizationStart);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, MobileAuthorizationStartXML, e);

                MobileAuthorizationStart = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, MobileAuthorizationStartText, out MobileAuthorizationStart, ..., OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP mobile authorization startes request.
        /// </summary>
        /// <param name="Request">An PullMobileAuthorizationStart request.</param>
        /// <param name="MobileAuthorizationStartText">The text to parse.</param>
        /// <param name="MobileAuthorizationStart">The parsed mobile authorization startes request.</param>
        /// <param name="CustomMobileAuthorizationStartParser">A delegate to customize the deserialization of MobileAuthorizationStart responses.</param>
        /// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        /// <param name="CustomStatusCodeParser">A delegate to parse custom StatusCode XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(MobileAuthorizeStartRequest                        Request,
                                       String                                             MobileAuthorizationStartText,
                                       out MobileAuthorizationStart                       MobileAuthorizationStart,
                                       CustomXMLParserDelegate<MobileAuthorizationStart>  CustomMobileAuthorizationStartParser   = null,
                                       CustomXMLParserDelegate<Address>                   CustomAddressParser                    = null,
                                       CustomXMLParserDelegate<StatusCode>                CustomStatusCodeParser                 = null,
                                       OnExceptionDelegate                                OnException                            = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(MobileAuthorizationStartText).Root,
                             out MobileAuthorizationStart,
                             CustomMobileAuthorizationStartParser,
                             CustomAddressParser,
                             CustomStatusCodeParser,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, MobileAuthorizationStartText, e);
            }

            MobileAuthorizationStart = null;
            return false;

        }

        #endregion

        #region ToXML(CustomAuthorizationStopSerializer = null, CustomStatusCodeSerializer = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="CustomAuthorizationStopSerializer">A delegate to customize the serialization of AuthorizationStop respones.</param>
        /// <param name="CustomStatusCodeSerializer">A delegate to serialize custom StatusCode XML elements.</param>
        /// <param name="CustomIdentificationSerializer">A delegate to serialize custom Identification XML elements.</param>
        public XElement ToXML(CustomXMLSerializerDelegate<MobileAuthorizationStart>  CustomMobileAuthorizationStartSerializer   = null,
                              CustomXMLSerializerDelegate<StatusCode>                CustomStatusCodeSerializer                 = null,
                              CustomXMLSerializerDelegate<Address>                   CustomAddressSerializer                    = null)

        {

            var XML = new XElement(OICPNS.MobileAuthorization + "eRoamingMobileAuthorizationStart",

                          SessionId.HasValue
                              ? new XElement(OICPNS.MobileAuthorization + "SessionID",         SessionId.ToString())
                              : null,

                          new XElement(OICPNS.MobileAuthorization + "AuthorizationStatus",     AuthorizationStatus.ToString()),

                          StatusCode.HasValue
                              ? StatusCode.Value.ToXML(CustomStatusCodeSerializer: CustomStatusCodeSerializer)
                              : null,

                          TermsOfUse.IsNeitherNullNorEmpty()
                              ? new XElement(OICPNS.MobileAuthorization + "TermsOfUse",        TermsOfUse.FirstText())
                              : null,

                          new XElement(OICPNS.EVSEData + "GeoCoordinates",
                              new XElement(OICPNS.CommonTypes + "DecimalDegree",  // Force 0.00... (dot) format!
                                  new XElement(OICPNS.CommonTypes + "Longitude",  GeoCoordinates.Longitude.ToString("{0:0.######}").Replace(",", ".")),// CultureInfo.InvariantCulture.NumberFormat)),
                                  new XElement(OICPNS.CommonTypes + "Latitude",   GeoCoordinates.Latitude. ToString("{0:0.######}").Replace(",", ".")) // CultureInfo.InvariantCulture.NumberFormat))
                              )
                          ),

                          Address != null
                              ? Address.ToXML(OICPNS.MobileAuthorization + "Address",
                                              CustomAddressSerializer)
                              : null,

                          AdditionalInfo.IsNeitherNullNorEmpty()
                              ? new XElement(OICPNS.MobileAuthorization + "AdditionalInfo",        AdditionalInfo.FirstText())
                              : null,

                          //AdditionalInfo.IsNotNullOrEmpty() && AdditionalInfo.Count() > 1
                          //    ? new XElement(OICPNS.MobileAuthorization + "EnAdditionalInfo",      AdditionalInfo.FirstText())
                          //    : null

                          ChargingStationName.IsNeitherNullNorEmpty()
                              ? new XElement(OICPNS.MobileAuthorization + "ChargingStationName",   ChargingStationName.FirstText())
                              : null

                      );

            return CustomMobileAuthorizationStartSerializer != null
                       ? CustomMobileAuthorizationStartSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (MobileAuthorizationStart1, MobileAuthorizationStart2)

        /// <summary>
        /// Compares two mobile authorization starts for equality.
        /// </summary>
        /// <param name="MobileAuthorizationStart1">A mobile authorization start.</param>
        /// <param name="MobileAuthorizationStart2">Another mobile authorization start.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (MobileAuthorizationStart MobileAuthorizationStart1, MobileAuthorizationStart MobileAuthorizationStart2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(MobileAuthorizationStart1, MobileAuthorizationStart2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) MobileAuthorizationStart1 == null) || ((Object) MobileAuthorizationStart2 == null))
                return false;

            return MobileAuthorizationStart1.Equals(MobileAuthorizationStart2);

        }

        #endregion

        #region Operator != (MobileAuthorizationStart1, MobileAuthorizationStart2)

        /// <summary>
        /// Compares two mobile authorization starts for inequality.
        /// </summary>
        /// <param name="MobileAuthorizationStart1">A mobile authorization start.</param>
        /// <param name="MobileAuthorizationStart2">Another mobile authorization start.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (MobileAuthorizationStart MobileAuthorizationStart1, MobileAuthorizationStart MobileAuthorizationStart2)

            => !(MobileAuthorizationStart1 == MobileAuthorizationStart2);

        #endregion

        #endregion

        #region IEquatable<MobileAuthorizationStart> Members

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

            var MobileAuthorizationStart = Object as MobileAuthorizationStart;
            if ((Object) MobileAuthorizationStart == null)
                return false;

            return Equals(MobileAuthorizationStart);

        }

        #endregion

        #region Equals(MobileAuthorizationStart)

        /// <summary>
        /// Compares two mobile authorization start for equality.
        /// </summary>
        /// <param name="MobileAuthorizationStart">A mobile authorization start to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(MobileAuthorizationStart MobileAuthorizationStart)
        {

            if ((Object) MobileAuthorizationStart == null)
                return false;

            return  AuthorizationStatus.Equals(MobileAuthorizationStart.AuthorizationStatus) &&
                    GeoCoordinates.     Equals(MobileAuthorizationStart.GeoCoordinates)      &&

                    ((ChargingStationName != null && MobileAuthorizationStart.ChargingStationName != null) ||
                     (ChargingStationName == null && MobileAuthorizationStart.ChargingStationName == null && ChargingStationName.Equals(MobileAuthorizationStart.ChargingStationName))) &&

                    ((Address             != null && MobileAuthorizationStart.Address             != null) ||
                     (Address             == null && MobileAuthorizationStart.Address             == null && Address.            Equals(MobileAuthorizationStart.Address))) &&

                    ((!SessionId.HasValue         && !MobileAuthorizationStart.SessionId.HasValue) ||
                      (SessionId.HasValue         &&  MobileAuthorizationStart.SessionId.HasValue         && SessionId.Value.    Equals(MobileAuthorizationStart.SessionId.Value))) &&

                    ((StatusCode           != null && MobileAuthorizationStart.StatusCode          != null) ||
                     (StatusCode           == null && MobileAuthorizationStart.StatusCode          == null && StatusCode.        Equals(MobileAuthorizationStart.StatusCode))) &&

                    ((TermsOfUse           != null && MobileAuthorizationStart.TermsOfUse          != null) ||
                     (TermsOfUse           == null && MobileAuthorizationStart.TermsOfUse          == null && TermsOfUse.        Equals(MobileAuthorizationStart.TermsOfUse))) &&

                    ((AdditionalInfo       != null && MobileAuthorizationStart.AdditionalInfo      != null) ||
                     (AdditionalInfo       == null && MobileAuthorizationStart.AdditionalInfo      == null && AdditionalInfo.    Equals(MobileAuthorizationStart.AdditionalInfo)));

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

                return AuthorizationStatus.      GetHashCode() * 23 ^
                       GeoCoordinates.           GetHashCode() * 19 ^

                       (ChargingStationName != null
                           ? ChargingStationName.GetHashCode() * 17
                           : 0) ^

                       (Address != null
                           ? Address.            GetHashCode() * 13
                           : 0) ^

                       (SessionId.HasValue
                           ? SessionId.Value.    GetHashCode() * 11
                           : 0) ^

                       (StatusCode != null
                           ? StatusCode.         GetHashCode() * 7
                           : 0) ^

                       (TermsOfUse != null
                           ? TermsOfUse.         GetHashCode() * 5
                           : 0) ^

                       (AdditionalInfo != null
                           ? AdditionalInfo.     GetHashCode() * 3
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
                             StatusCode.HasValue
                                 ? " -> " + StatusCode.Value.Code
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
        /// An MobileAuthorizationStart response builder.
        /// </summary>
        public class Builder : AResponseBuilder<MobileAuthorizeStartRequest,
                                                MobileAuthorizationStart>
        {

            #region Properties

            /// <summary>
            /// The authorization status, e.g. "Authorized".
            /// </summary>
            public AuthorizationStatusTypes  AuthorizationStatus     { get; set; }

            /// <summary>
            /// The geo coordinate of the EVSE.
            /// </summary>
            public GeoCoordinate             GeoCoordinates          { get; set; }

            /// <summary>
            /// The name of the charging station.
            /// </summary>
            public I18NString                ChargingStationName     { get; set; }

            /// <summary>
            /// The address of the EVSE.
            /// </summary>
            public Address                   Address                 { get; set; }

            /// <summary>
            /// The Hubject session identification.
            /// </summary>
            public Session_Id?               SessionId               { get; set; }

            /// <summary>
            /// The status code of the request.
            /// </summary>
            public StatusCode?               StatusCode              { get; set; }

            /// <summary>
            /// The terms of use.
            /// </summary>
            public I18NString                TermsOfUse              { get; set; }

            /// <summary>
            /// Additional information.
            /// </summary>
            public I18NString                AdditionalInfo          { get; set; }

            #endregion

            #region Constructor(s)

            #region Builder(Request,    CustomData = null)

            /// <summary>
            /// Create a new MobileAuthorizationStart response builder.
            /// </summary>
            /// <param name="Request">A MobileAuthorizeStart request.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(MobileAuthorizeStartRequest          Request,
                           IReadOnlyDictionary<String, Object>  CustomData  = null)

                : base(Request,
                       CustomData)

            { }

            #endregion

            #region Builder(MobileAuthorizationStart, CustomData = null)

            /// <summary>
            /// Create a new MobileAuthorizationStart response builder.
            /// </summary>
            /// <param name="MobileAuthorizationStart">An MobileAuthorizationStart response.</param>
            /// <param name="CustomData">Optional custom data.</param>
            public Builder(MobileAuthorizationStart             MobileAuthorizationStart,
                           IReadOnlyDictionary<String, Object>  CustomData  = null)

                : base(MobileAuthorizationStart?.Request,
                       MobileAuthorizationStart.HasCustomData
                           ? CustomData != null && CustomData.Any()
                                 ? MobileAuthorizationStart.CustomData.Concat(CustomData)
                                 : MobileAuthorizationStart.CustomData
                           : CustomData)

            {

                if (MobileAuthorizationStart != null)
                {

                    this.AuthorizationStatus  = MobileAuthorizationStart.AuthorizationStatus;
                    this.GeoCoordinates       = MobileAuthorizationStart.GeoCoordinates;
                    this.ChargingStationName  = MobileAuthorizationStart.ChargingStationName;
                    this.Address              = MobileAuthorizationStart.Address;
                    this.SessionId            = MobileAuthorizationStart.SessionId;
                    this.StatusCode           = MobileAuthorizationStart.StatusCode;
                    this.TermsOfUse           = MobileAuthorizationStart.TermsOfUse;
                    this.AdditionalInfo       = MobileAuthorizationStart.AdditionalInfo;

                }

            }

            #endregion

            #endregion


            #region Equals(MobileAuthorizationStart)

            /// <summary>
            /// Compares two mobile authorization start for equality.
            /// </summary>
            /// <param name="MobileAuthorizationStart">A mobile authorization start to compare with.</param>
            /// <returns>True if both match; False otherwise.</returns>
            public override Boolean Equals(MobileAuthorizationStart MobileAuthorizationStart)
            {

                if ((Object) MobileAuthorizationStart == null)
                    return false;

                return  AuthorizationStatus.Equals(MobileAuthorizationStart.AuthorizationStatus) &&
                        GeoCoordinates.     Equals(MobileAuthorizationStart.GeoCoordinates)      &&

                        ((ChargingStationName != null && MobileAuthorizationStart.ChargingStationName != null) ||
                         (ChargingStationName == null && MobileAuthorizationStart.ChargingStationName == null && ChargingStationName.Equals(MobileAuthorizationStart.ChargingStationName))) &&

                        ((Address             != null && MobileAuthorizationStart.Address             != null) ||
                         (Address             == null && MobileAuthorizationStart.Address             == null && Address.            Equals(MobileAuthorizationStart.Address))) &&

                        ((!SessionId.HasValue         && !MobileAuthorizationStart.SessionId.HasValue) ||
                          (SessionId.HasValue         &&  MobileAuthorizationStart.SessionId.HasValue         && SessionId.Value.    Equals(MobileAuthorizationStart.SessionId.Value))) &&

                        ((StatusCode           != null && MobileAuthorizationStart.StatusCode          != null) ||
                         (StatusCode           == null && MobileAuthorizationStart.StatusCode          == null && StatusCode.        Equals(MobileAuthorizationStart.StatusCode))) &&

                        ((TermsOfUse           != null && MobileAuthorizationStart.TermsOfUse          != null) ||
                         (TermsOfUse           == null && MobileAuthorizationStart.TermsOfUse          == null && TermsOfUse.        Equals(MobileAuthorizationStart.TermsOfUse))) &&

                        ((AdditionalInfo       != null && MobileAuthorizationStart.AdditionalInfo      != null) ||
                         (AdditionalInfo       == null && MobileAuthorizationStart.AdditionalInfo      == null && AdditionalInfo.    Equals(MobileAuthorizationStart.AdditionalInfo)));

            }

            #endregion

            public override MobileAuthorizationStart ToImmutable

                => new MobileAuthorizationStart(Request,
                                                AuthorizationStatus,
                                                GeoCoordinates,
                                                ChargingStationName,
                                                Address,
                                                SessionId,
                                                StatusCode,
                                                TermsOfUse,
                                                AdditionalInfo);

        }

        #endregion

    }

}
