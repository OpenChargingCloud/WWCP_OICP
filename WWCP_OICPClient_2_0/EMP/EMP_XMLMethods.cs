/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICPClient <https://github.com/WorldWideCharging/WWCP_OICPClient>
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
using System.Globalization;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPClient_2_0
{

    /// <summary>
    /// OICP v2.0 EMP management methods.
    /// </summary>
    public static class EMP_XMLMethods
    {

        #region SearchRequestXML(ProviderId, SearchCenter = null, DistanceKM = 0, Address = null, Plug = null, ChargingFacility = null)

        /// <summary>
        /// Create a new EVSE Search request.
        /// </summary>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="Address">An optional address of the charging stations.</param>
        /// <param name="Plug">Optional plugs of the charging station.</param>
        /// <param name="ChargingFacility">Optional charging facilities of the charging station.</param>
        public static XElement SearchRequestXML(EVSP_Id              ProviderId,
                                                GeoCoordinate        SearchCenter      = null,
                                                UInt64               DistanceKM        = 0,
                                                Address              Address           = null,
                                                PlugTypes?           Plug              = null,
                                                ChargingFacilities?  ChargingFacility  = null)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:v2      = "http://www.hubject.com/b2b/services/evsesearch/v2.0"
            //                   xmlns:v21     = "http://www.hubject.com/b2b/services/commontypes/v2.0">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <v2:eRoamingSearchEvse>
            //
            //          <!--Optional:-->
            //          <v2:GeoCoordinates>
            //             <!--You have a CHOICE of the next 3 items at this level-->
            //             <v21:Google>
            //                <v21:Coordinates>?</v21:Coordinates>
            //             </v21:Google>
            //             <v21:DecimalDegree>
            //                <v21:Longitude>?</v21:Longitude>
            //                <v21:Latitude>?</v21:Latitude>
            //             </v21:DecimalDegree>
            //             <v21:DegreeMinuteSeconds>
            //                <v21:Longitude>?</v21:Longitude>
            //                <v21:Latitude>?</v21:Latitude>
            //             </v21:DegreeMinuteSeconds>
            //          </v2:GeoCoordinates>
            //
            //          <!--Optional:-->
            //          <v2:Address>
            //             <v21:Country>?</v21:Country>
            //             <v21:City>?</v21:City>
            //             <v21:Street>?</v21:Street>
            //             <!--Optional:-->
            //             <v21:PostalCode>?</v21:PostalCode>
            //             <!--Optional:-->
            //             <v21:HouseNum>?</v21:HouseNum>
            //             <!--Optional:-->
            //             <v21:Floor>?</v21:Floor>
            //             <!--Optional:-->
            //             <v21:Region>?</v21:Region>
            //             <!--Optional:-->
            //             <v21:TimeZone>?</v21:TimeZone>
            //          </v2:Address>
            //
            //          <v2:ProviderID>?</v2:ProviderID>
            //          <v2:Range>?</v2:Range>
            //          <!--Optional:-->
            //          <v2:Plug>?</v2:Plug>
            //          <!--Optional:-->
            //          <v2:ChargingFacility>?</v2:ChargingFacility>
            //
            //       </v2:eRoamingSearchEvse>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (ProviderId == null)
                throw new ArgumentNullException("ProviderId", "The given parameter must not be null!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.EVSESearch + "eRoamingSearchEvse",

                                          (SearchCenter != null && DistanceKM > 0)
                                              ? new XElement(OICPNS.EVSEData + "SearchCenter",
                                                  new XElement(OICPNS.CommonTypes + "GeoCoordinates",
                                                      new XElement(OICPNS.CommonTypes + "DecimalDegree",
                                                          new XElement(OICPNS.CommonTypes + "Longitude", SearchCenter.Longitude.ToString(CultureInfo.InvariantCulture.NumberFormat)),
                                                          new XElement(OICPNS.CommonTypes + "Latitude",  SearchCenter.Latitude. ToString(CultureInfo.InvariantCulture.NumberFormat)))
                                                  ))
                                              : null,

                                          (Address         != null            &&
                                           Address.Country != Country.unknown &&
                                           Address.City.  IsNotNullOrEmpty()  &&
                                           Address.Street.IsNotNullOrEmpty())
                                              ? new XElement(OICPNS.EVSEData + "Address",

                                                    new XElement(OICPNS.CommonTypes + "Country" + Address.Country.ToString()),
                                                    new XElement(OICPNS.CommonTypes + "City"    + Address.City),
                                                    new XElement(OICPNS.CommonTypes + "Street"  + Address.Street),

                                                    Address.PostalCode.IsNotNullOrEmpty()
                                                        ? new XElement(OICPNS.CommonTypes + "PostalCode" + Address.PostalCode)
                                                        : null,

                                                    Address.HouseNumber.IsNotNullOrEmpty()
                                                        ? new XElement(OICPNS.CommonTypes + "HouseNum"   + Address.HouseNumber)
                                                        : null,

                                                    Address.FloorLevel.IsNotNullOrEmpty()
                                                        ? new XElement(OICPNS.CommonTypes + "Floor"      + Address.FloorLevel)
                                                        : null

                                                    // <!--Optional:-->
                                                    // <v21:Region>?</v21:Region>

                                                    // <!--Optional:-->
                                                    // <v21:TimeZone>?</v21:TimeZone>

                                                    )
                                              : null,

                                          new XElement(OICPNS.EVSESearch + "ProviderID", ProviderId),

                                          (SearchCenter     != null && DistanceKM > 0)
                                              ? new XElement(OICPNS.EVSESearch + "Range", DistanceKM)
                                              : null,

                                          (Plug             != null && Plug.HasValue)
                                              ? new XElement(OICPNS.EVSESearch + "Plug", Plug.ToString())
                                              : null,

                                          (ChargingFacility != null && ChargingFacility.HasValue)
                                              ? new XElement(OICPNS.EVSESearch + "ChargingFacility", ChargingFacility.ToString())
                                              : null

                                     ));

        }

        #endregion


        #region ParseEVSEDataRecord(EVSEDataRecordXML)

        public static EVSEDataRecord ParseEVSEDataRecord(XElement  EVSEDataRecordXML)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:v2      = "http://www.hubject.com/b2b/services/evsedata/v2.0"
            //                   xmlns:v21     = "http://www.hubject.com/b2b/services/commontypes/v2.0">

            // <v2:eRoamingEvseDataRecord deltaType="?" lastUpdate="?">
            //
            //    <v2:EvseId>?</v2:EvseId>
            //
            //    <!--Optional:-->
            //    <v2:ChargingStationId>?</v2:ChargingStationId>
            //    <!--Optional:-->
            //    <v2:ChargingStationName>?</v2:ChargingStationName>
            //    <!--Optional:-->
            //    <v2:EnChargingStationName>?</v2:EnChargingStationName>
            //
            //    <v2:Address>
            //       <v21:Country>?</v21:Country>
            //       <v21:City>?</v21:City>
            //       <v21:Street>?</v21:Street>
            //       <!--Optional:-->
            //       <v21:PostalCode>?</v21:PostalCode>
            //       <!--Optional:-->
            //       <v21:HouseNum>?</v21:HouseNum>
            //       <!--Optional:-->
            //       <v21:Floor>?</v21:Floor>
            //       <!--Optional:-->
            //       <v21:Region>?</v21:Region>
            //       <!--Optional:-->
            //       <v21:TimeZone>?</v21:TimeZone>
            //    </v2:Address>
            //
            //    <v2:GeoCoordinates>
            //       <!--You have a CHOICE of the next 3 items at this level-->
            //       <v21:Google>
            //          <v21:Coordinates>?</v21:Coordinates>
            //       </v21:Google>
            //       <v21:DecimalDegree>
            //          <v21:Longitude>?</v21:Longitude>
            //          <v21:Latitude>?</v21:Latitude>
            //       </v21:DecimalDegree>
            //       <v21:DegreeMinuteSeconds>
            //          <v21:Longitude>?</v21:Longitude>
            //          <v21:Latitude>?</v21:Latitude>
            //       </v21:DegreeMinuteSeconds>
            //    </v2:GeoCoordinates>
            //
            //    <v2:Plugs>
            //       <!--1 or more repetitions:-->
            //       <v2:Plug>?</v2:Plug>
            //    </v2:Plugs>
            //
            //    <!--Optional:-->
            //    <v2:ChargingFacilities>
            //       <!--1 or more repetitions:-->
            //       <v2:ChargingFacility>?</v2:ChargingFacility>
            //    </v2:ChargingFacilities>
            //
            //    <!--Optional:-->
            //    <v2:ChargingModes>
            //       <!--1 or more repetitions:-->
            //       <v2:ChargingMode>?</v2:ChargingMode>
            //    </v2:ChargingModes>
            //
            //    <v2:AuthenticationModes>
            //       <!--1 or more repetitions:-->
            //       <v2:AuthenticationMode>?</v2:AuthenticationMode>
            //    </v2:AuthenticationModes>
            //
            //    <!--Optional:-->
            //    <v2:MaxCapacity>?</v2:MaxCapacity>
            //
            //    <!--Optional:-->
            //    <v2:PaymentOptions>
            //       <!--1 or more repetitions:-->
            //       <v2:PaymentOption>?</v2:PaymentOption>
            //    </v2:PaymentOptions>
            //
            //    <v2:Accessibility>?</v2:Accessibility>
            //    <v2:HotlinePhoneNum>?</v2:HotlinePhoneNum>
            //
            //    <!--Optional:-->
            //    <v2:AdditionalInfo>?</v2:AdditionalInfo>
            //
            //    <!--Optional:-->
            //    <v2:EnAdditionalInfo>?</v2:EnAdditionalInfo>
            //
            //    <!--Optional:-->
            //    <v2:GeoChargingPointEntrance>
            //       <!--You have a CHOICE of the next 3 items at this level-->
            //       <v21:Google>
            //          <v21:Coordinates>?</v21:Coordinates>
            //       </v21:Google>
            //       <v21:DecimalDegree>
            //          <v21:Longitude>?</v21:Longitude>
            //          <v21:Latitude>?</v21:Latitude>
            //       </v21:DecimalDegree>
            //       <v21:DegreeMinuteSeconds>
            //          <v21:Longitude>?</v21:Longitude>
            //          <v21:Latitude>?</v21:Latitude>
            //       </v21:DegreeMinuteSeconds>
            //    </v2:GeoChargingPointEntrance>
            //
            //    <v2:IsOpen24Hours>?</v2:IsOpen24Hours>
            //    <!--Optional:-->
            //    <v2:OpeningTime>?</v2:OpeningTime>
            //
            //    <!--Optional:-->
            //    <v2:HubOperatorID>?</v2:HubOperatorID>
            //
            //    <!--Optional:-->
            //    <v2:ClearinghouseID>?</v2:ClearinghouseID>
            //    <v2:IsHubjectCompatible>?</v2:IsHubjectCompatible>
            //    <v2:DynamicInfoAvailable>?</v2:DynamicInfoAvailable>
            //
            // </v2:eRoamingEvseDataRecord>

            #endregion


            var EVSEDataRecord = new EVSEDataRecord(EVSE_Id.Parse(EVSEDataRecordXML.ElementValue(OICPNS.EVSEData + "EvseId", "Missing EvseId!")));

            #region XML Attribute: DeltaType

            EVSEDataRecord.DeltaType  = EVSEDataRecordXML.Attribute("deltaType").Value;

            #endregion

            #region XML Attribute: LastUpdate

            DateTime _LastUpdate;
            if (DateTime.TryParse(EVSEDataRecordXML.Attribute("lastUpdate").Value, out _LastUpdate))
                EVSEDataRecord.LastUpdate  = _LastUpdate;

            #endregion


            #region ChargingStationId

            EVSEDataRecord.ChargingStationId  = EVSEDataRecordXML.
                                                    ElementValueOrDefault(OICPNS.EVSEData + "ChargingStationId", "").
                                                    Trim();

            #endregion

            #region ChargingStationName

            EVSEDataRecord.ChargingStationName    = EVSEDataRecordXML.
                                                        ElementValueOrDefault(OICPNS.EVSEData + "ChargingStationName", "").
                                                        Trim();

            EVSEDataRecord.EnChargingStationName  = EVSEDataRecordXML.
                                                        ElementValueOrDefault(OICPNS.EVSEData + "EnChargingStationName", "").
                                                        Trim();

            #endregion

            #region Get address of the EVSE...

            var AddressXML   = EVSEDataRecordXML.ElementOrFail(OICPNS.EVSEData + "Address", "Missing 'Address'-XML tag!");

            var _CountryTXT  = AddressXML.ElementValue(OICPNS.CommonTypes + "Country", "Missing 'Country'-XML tag!").Trim();

            Country _Country;
            if (!Country.TryParseAlpha3Code(_CountryTXT, out _Country))
            {

                if (_CountryTXT.ToUpper() == "UNKNOWN")
                    _Country = Country.unknown;

                else
                    throw new Exception("'" + _CountryTXT + "' is an unknown country name!");

            }

            EVSEDataRecord.Address = new Address(AddressXML.ElementValue         (OICPNS.CommonTypes + "Street",     "Missing 'Street'-XML tag!").Trim(),
                                                 AddressXML.ElementValueOrDefault(OICPNS.CommonTypes + "HouseNum",   "").Trim(),
                                                 AddressXML.ElementValueOrDefault(OICPNS.CommonTypes + "Floor",      "").Trim(),
                                                 AddressXML.ElementValueOrDefault(OICPNS.CommonTypes + "PostalCode", "").Trim(),
                                                 "",
                                                 AddressXML.ElementValue         (OICPNS.CommonTypes + "City",       "Missing 'City'-XML tag!").Trim(),
                                                 _Country);

            // Currently not used OICP address information!
            //var _Region       = AddressXML.       ElementValueOrDefault(OICPNS.OICPv2_0CommonTypes + "Region",     "").Trim();
            //var _Timezone     = AddressXML.       ElementValueOrDefault(OICPNS.OICPv2_0CommonTypes + "Timezone",   "").Trim();

            #endregion

            #region GeoCoordinate

            var GeoCoordinatesXML = EVSEDataRecordXML.ElementOrFail(OICPNS.EVSEData    + "GeoCoordinates", "Missing 'GeoCoordinates'-XML tag!");
            var DecimalDegreeXML  = GeoCoordinatesXML.ElementOrFail(OICPNS.CommonTypes + "DecimalDegree",  "Missing 'DecimalDegree'-XML tag!");
            var LatitudeString    = DecimalDegreeXML. ElementValue (OICPNS.CommonTypes + "Latitude",       "Missing 'Latitude'-XML tag!");
            var LongitudeString   = DecimalDegreeXML. ElementValue (OICPNS.CommonTypes + "Longitude",      "Missing 'Longitude'-XML tag!");

            Latitude LatitudeValue;
            if (!Latitude.TryParse(LatitudeString,   out LatitudeValue))
                throw new Exception("Invalid 'latitude'-XML tag value for 'GeoCoordinate'!");

            Longitude LongitudeValue;
            if (!Longitude.TryParse(LongitudeString, out LongitudeValue))
                throw new Exception("Invalid 'longitude'-XML tag value for 'GeoCoordinate'!");

            EVSEDataRecord.GeoCoordinate  = new GeoCoordinate(LatitudeValue, LongitudeValue);

            #endregion

            #region Plugs

            EVSEDataRecord.Plugs = new ReactiveSet<PlugTypes>(EVSEDataRecordXML.
                                                                  ElementOrFail(OICPNS.EVSEData + "Plugs", "Missing 'Plugs'-XML tag!").
                                                                  Elements     (OICPNS.EVSEData + "Plug").
                                                                  Select(xml => OICPMapper.AsPlugType(xml.Value.Trim())));

            #endregion

            #region ChargingFacilities

            EVSEDataRecord.ChargingFacilities = new ReactiveSet<ChargingFacilities>(EVSEDataRecordXML.
                                                                                        IfElementExists(OICPNS.EVSEData + "ChargingFacilities",
                                                                                                        XML => XML.Elements(OICPNS.EVSEData + "ChargingFacility").
                                                                                                                   Select(xml => OICPMapper.AsChargingFacility(xml.Value.Trim())),
                                                                                                        org.GraphDefined.WWCP.ChargingFacilities.Unspecified));

            #endregion

            #region ChargingModes

            EVSEDataRecord.ChargingModes = new ReactiveSet<ChargingModes>(EVSEDataRecordXML.
                                                                              IfElementExists(OICPNS.EVSEData + "ChargingModes",
                                                                                              XML => XML.Elements(OICPNS.EVSEData + "ChargingMode").
                                                                                                         Select(xml => OICPMapper.AsChargingMode(xml.Value.Trim())),
                                                                                              org.GraphDefined.WWCP.ChargingModes.Unspecified));

            #endregion

            #region AuthenticationModes

            EVSEDataRecord.AuthenticationModes = new ReactiveSet<AuthenticationModes>(EVSEDataRecordXML.
                                                                                          ElementOrFail(OICPNS.EVSEData + "AuthenticationModes", "Missing 'AuthenticationModes'-XML tag!").
                                                                                          Elements(OICPNS.EVSEData + "AuthenticationMode").
                                                                                          Select(xml => OICPMapper.AsAuthenticationMode(xml.Value.Trim())));

            #endregion

            #region MaxCapacity in kWh

            var _MaxCapacity_kWh = EVSEDataRecordXML.
                                       ElementValueOrDefault(OICPNS.EVSEData + "MaxCapacity", String.Empty).
                                       Trim();

            Double _MaxCapacity;
            if (_MaxCapacity_kWh.IsNotNullOrEmpty())
                if (Double.TryParse(_MaxCapacity_kWh, out _MaxCapacity))
                    EVSEDataRecord.MaxCapacity = _MaxCapacity;

            #endregion

            #region PaymentOptions

            EVSEDataRecord.PaymentOptions = new ReactiveSet<PaymentOptions>(EVSEDataRecordXML.
                                                                                IfElementExists(OICPNS.EVSEData + "PaymentOptions",
                                                                                                XML => XML.Elements(OICPNS.EVSEData + "PaymentOption").
                                                                                                           Select(xml => OICPMapper.AsPaymetOption(xml.Value.Trim())),
                                                                                                PaymentOptions.Unspecified));

            #endregion

            #region Accessibility

            EVSEDataRecord.Accessibility = OICPMapper.AsAccessibilityType(EVSEDataRecordXML.
                                                                              ElementValue(OICPNS.EVSEData + "Accessibility", "Missing 'Accessibility'-XML tag!").
                                                                              Trim());

            #endregion

            #region HotlinePhoneNum

            EVSEDataRecord.HotlinePhoneNum = EVSEDataRecordXML.
                                                 ElementValue(OICPNS.EVSEData + "HotlinePhoneNum", "Missing 'HotlinePhoneNum '-XML tag!").
                                                 Trim();

            #endregion

            #region AdditionalInfo

            EVSEDataRecord.AdditionalInfo  = EVSEDataRecordXML.ElementValueOrDefault(OICPNS.EVSEData + "AdditionalInfo",   String.Empty).Trim();

            #endregion

            #region EnAdditionalInfo

            // EnAdditionalInfo not parsed as OICP v2.0 multi-language string!
            EVSEDataRecord.EnAdditionalInfo  = new I18NString(Languages.en, EVSEDataRecordXML.ElementValueOrDefault(OICPNS.EVSEData + "EnAdditionalInfo", String.Empty).Trim());

            #endregion

            #region Get geo coordinate of the charging pool entrance...

            var GeoChargingPointEntranceXML = EVSEDataRecordXML.Element(OICPNS.CommonTypes + "GeoChargingPointEntrance");
            if (GeoChargingPointEntranceXML != null)
            {

                var Entr_DecimalDegreeXML  = GeoChargingPointEntranceXML.ElementOrFail     (OICPNS.CommonTypes + "DecimalDegree", "Missing 'DecimalDegree'-XML tag for 'GeoChargingPointEntrance'!");
                var Entr_LatitudeString    = Entr_DecimalDegreeXML.      ElementValue(OICPNS.CommonTypes + "Latitude",      "Missing 'Latitude'-XML tag for 'GeoChargingPointEntrance'!");
                var Entr_LongitudeString   = Entr_DecimalDegreeXML.      ElementValue(OICPNS.CommonTypes + "Longitude",     "Missing 'Longitude'-XML tag for 'GeoChargingPointEntrance'!");

                Latitude Entr_LatitudeValue;
                if (!Latitude.TryParse(Entr_LatitudeString, out Entr_LatitudeValue))
                    throw new Exception("Invalid 'Latitude'-XML tag value for 'GeoChargingPointEntrance'!");

                Longitude Entr_LongitudeValue;
                if (!Longitude.TryParse(Entr_LongitudeString, out Entr_LongitudeValue))
                    throw new Exception("Invalid 'Longitude'-XML tag value for 'GeoChargingPointEntrance'!");

                EVSEDataRecord.GeoChargingPointEntrance = new GeoCoordinate(Entr_LatitudeValue, Entr_LongitudeValue);

            }

            #endregion

            #region IsOpen24Hours / OpeningTime

            if (EVSEDataRecordXML.ElementValue(OICPNS.EVSEData + "IsOpen24Hours", "Missing 'IsOpen24Hours'-XML tag!") == "true")
            {
                EVSEDataRecord.OpeningTime = OpeningTime.Is24Hours;
            }

            else
            {

                var OpeningTimeXML = EVSEDataRecordXML.Element(OICPNS.EVSEData + "OpeningTime");
                if (OpeningTimeXML != null)
                {
                    EVSEDataRecord.OpeningTime = new OpeningTime(OpeningTimeXML.Value.Trim());
                }

            }

            #endregion

            #region HubOperatorID

            EVSEOperator_Id _HubOperatorId;
            if (EVSEOperator_Id.TryParse(EVSEDataRecordXML.
                                             ElementValueOrDefault(OICPNS.EVSEData + "HubOperatorID", String.Empty).
                                             Trim(),
                                         out _HubOperatorId))
                EVSEDataRecord.HubOperatorID = _HubOperatorId;

            #endregion

            #region ClearinghouseID

            RoamingProvider_Id _ClearinghouseID;
            if (RoamingProvider_Id.TryParse(EVSEDataRecordXML.
                                                ElementValueOrDefault(OICPNS.EVSEData + "ClearinghouseID", String.Empty).
                                                Trim(),
                                                out _ClearinghouseID))
                EVSEDataRecord.ClearinghouseID = _ClearinghouseID;

            #endregion

            #region IsHubjectCompatible

            EVSEDataRecord.IsHubjectCompatible  = EVSEDataRecordXML.
                                                      ElementValue(OICPNS.EVSEData + "IsHubjectCompatible", "Missing 'IsHubjectCompatible '-XML tag!").
                                                      Trim() == "true";

            #endregion

            #region DynamicInfoAvailable

            EVSEDataRecord.DynamicInfoAvailable  = EVSEDataRecordXML.
                                                       ElementValue(OICPNS.EVSEData + "DynamicInfoAvailable", "Missing 'DynamicInfoAvailable '-XML tag!").
                                                       Trim() != "false";

            #endregion

            // Currently not used OICP address information!
            //var _Region       = AddressXML.       ElementValueOrDefault(OICPNS.OICPv2_0CommonTypes + "Region",     "").Trim();
            //var _Timezone     = AddressXML.       ElementValueOrDefault(OICPNS.OICPv2_0CommonTypes + "Timezone",   "").Trim();

            // EnAdditionalInfo not parsed as OICP v2.0 multi-language string!

            return EVSEDataRecord;

        }

        #endregion

        #region GetEVSEByIdRequestXML(EVSEId)

        /// <summary>
        /// Create a new Get-EVSE-By-Id request.
        /// </summary>
        /// <param name="EVSEId">An unique EVSE identification.</param>
        public static XElement GetEVSEByIdRequestXML(EVSE_Id  EVSEId)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:v2      = "http://www.hubject.com/b2b/services/evsedata/v2.0">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <v2:eRoamingGetEvseById>
            //
            //          <v2:EvseId>?</v2:EvseId>
            //
            //       </v2:eRoamingGetEvseById>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentNullException("EVSEId", "The given parameter must not be null!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.EVSEData + "eRoamingGetEvseById",
                                          new XElement(OICPNS.EVSEData + "EvseId", EVSEId.OriginId)
                                     ));

        }

        #endregion

        #region PullEVSEDataRequestXML(ProviderId, SearchCenter = null, DistanceKM = 0, LastCall = null, GeoCoordinatesResponseFormat = DecimalDegree)

        /// <summary>
        /// Create a new Pull EVSE Data request.
        /// </summary>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="LastCall">An optional timestamp of the last call.</param>
        /// <param name="GeoCoordinatesResponseFormat">An optional response format for the geo coordinates [default: DecimalDegree]</param>
        public static XElement PullEVSEDataRequestXML(EVSP_Id                            ProviderId,
                                                      GeoCoordinate                      SearchCenter                  = null,
                                                      UInt64                             DistanceKM                    = 0,
                                                      DateTime?                          LastCall                      = null,
                                                      GeoCoordinatesResponseFormatTypes  GeoCoordinatesResponseFormat  = GeoCoordinatesResponseFormatTypes.DecimalDegree)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:v2      = "http://www.hubject.com/b2b/services/evsedata/v2.0"
            //                   xmlns:v21     = "http://www.hubject.com/b2b/services/commontypes/v2.0">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <v2:eRoamingPullEvseData>
            //
            //          <v2:ProviderID>?</v2:ProviderID>
            //
            //          <!--You have a CHOICE of the next 2 items at this level-->
            //          <!--Optional:-->
            //          <v2:SearchCenter>
            //             <v21:GeoCoordinates>
            //                <!--You have a CHOICE of the next 3 items at this level-->
            //                <v21:Google>
            //                   <v21:Coordinates>?</v21:Coordinates>
            //                </v21:Google>
            //                <v21:DecimalDegree>
            //                   <v21:Longitude>?</v21:Longitude>
            //                   <v21:Latitude>?</v21:Latitude>
            //                </v21:DecimalDegree>
            //                <v21:DegreeMinuteSeconds>
            //                   <v21:Longitude>?</v21:Longitude>
            //                   <v21:Latitude>?</v21:Latitude>
            //                </v21:DegreeMinuteSeconds>
            //             </v21:GeoCoordinates>
            //             <v21:Radius>?</v21:Radius>
            //          </v2:SearchCenter>
            //
            //          <!--Optional:-->
            //          <v2:LastCall>?</v2:LastCall>
            //          <v2:GeoCoordinatesResponseFormat>?</v2:GeoCoordinatesResponseFormat>
            //
            //       </v2:eRoamingPullEvseData>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (ProviderId == null)
                throw new ArgumentNullException("ProviderId", "The given parameter must not be null!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.EVSEData + "eRoamingPullEVSEData",

                                          new XElement(OICPNS.EVSEData + "ProviderID", ProviderId.ToString()),

                                          (SearchCenter != null && DistanceKM > 0)
                                              ? new XElement(OICPNS.EVSEData + "SearchCenter",
                                                  new XElement(OICPNS.CommonTypes + "GeoCoordinates",
                                                      new XElement(OICPNS.CommonTypes + "DecimalDegree",
                                                          new XElement(OICPNS.CommonTypes + "Longitude", SearchCenter.Longitude.ToString(CultureInfo.InvariantCulture.NumberFormat)),
                                                          new XElement(OICPNS.CommonTypes + "Latitude",  SearchCenter.Latitude. ToString(CultureInfo.InvariantCulture.NumberFormat)))
                                                  ))
                                              : null,

                                          (LastCall != null && LastCall.HasValue)
                                              ? new XElement(OICPNS.EVSEData + "LastCall",  LastCall.Value.ToIso8601())
                                              : null,

                                          new XElement(OICPNS.EVSEData + "GeoCoordinatesResponseFormat", GeoCoordinatesResponseFormat.ToString())

                                     ));

        }

        #endregion


        #region PullEVSEStatusByIdRequestXML(ProviderId, EVSEIds)

        /// <summary>
        /// Create a new Pull EVSE Status-By-Id request.
        /// </summary>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="EVSEIds">Up to 100 EVSE Ids.</param>
        public static XElement PullEVSEStatusByIdRequestXML(EVSP_Id               ProviderId,
                                                            IEnumerable<EVSE_Id>  EVSEIds)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:v2      = "http://www.hubject.com/b2b/services/evsestatus/v2.0">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <v2:eRoamingPullEvseStatusById>
            //
            //          <v2:ProviderID>?</v2:ProviderID>
            //
            //          <!--1 to 100 repetitions:-->
            //          <v2:EvseId>?</v2:EvseId>
            //
            //       </v2:eRoamingPullEvseStatusById>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (ProviderId == null)
                throw new ArgumentNullException("ProviderId", "The given parameter must not be null!");

            if (EVSEIds == null)
                throw new ArgumentNullException("EVSEIds", "The given parameter must not be null!");

            var _EVSEIds = EVSEIds.ToArray();

            if (_EVSEIds.Length == 0)
                throw new ArgumentNullException("EVSEIds", "The given enumeration must not be empty!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.EVSEStatus + "eRoamingPullEvseStatusById",

                                          new XElement(OICPNS.EVSEStatus + "ProviderID", ProviderId.ToString()),

                                          EVSEIds.Select(EVSEId => new XElement(OICPNS.EVSEStatus + "EvseId", EVSEId.OriginId)).
                                                  ToArray()

                                     ));

        }

        #endregion

        #region PullEVSEStatusRequestXML(ProviderId, SearchCenter = null, DistanceKM = 0, EVSEStatus = null)

        /// <summary>
        /// Create a new Pull EVSE Status request.
        /// </summary>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="EVSEStatus">An optional EVSE status as filter criteria.</param>
        public static XElement PullEVSEStatusRequestXML(EVSP_Id           ProviderId,
                                                        GeoCoordinate     SearchCenter  = null,
                                                        UInt64            DistanceKM    = 0,
                                                        HubjectEVSEState? EVSEStatus    = null)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:v2      = "http://www.hubject.com/b2b/services/evsestatus/v2.0"
            //                   xmlns:v21     = "http://www.hubject.com/b2b/services/commontypes/v2.0">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <v2:eRoamingPullEvseStatus>
            //
            //          <v2:ProviderID>?</v2:ProviderID>
            //
            //          <!--Optional:-->
            //          <v2:SearchCenter>
            //
            //             <v21:GeoCoordinates>
            //                <!--You have a CHOICE of the next 3 items at this level-->
            //                <v21:Google>
            //                   <v21:Coordinates>?</v21:Coordinates>
            //                </v21:Google>
            //                <v21:DecimalDegree>
            //                   <v21:Longitude>?</v21:Longitude>
            //                   <v21:Latitude>?</v21:Latitude>
            //                </v21:DecimalDegree>
            //                <v21:DegreeMinuteSeconds>
            //                   <v21:Longitude>?</v21:Longitude>
            //                   <v21:Latitude>?</v21:Latitude>
            //                </v21:DegreeMinuteSeconds>
            //             </v21:GeoCoordinates>
            //
            //             <v21:Radius>?</v21:Radius>
            //
            //          </v2:SearchCenter>
            //
            //          <!--Optional:-->
            //          <v2:EvseStatus>?</v2:EvseStatus>
            //
            //       </v2:eRoamingPullEvseStatus>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (ProviderId == null)
                throw new ArgumentNullException("ProviderId", "The given parameter must not be null!");

            #endregion

            return SOAP.Encapsulation(new XElement(OICPNS.EVSEStatus + "eRoamingPullEvseStatus",

                                          new XElement(OICPNS.EVSEStatus + "ProviderID", ProviderId.ToString()),

                                          (SearchCenter != null && DistanceKM > 0)
                                              ? new XElement(OICPNS.EVSEStatus + "SearchCenter",

                                                    new XElement(OICPNS.CommonTypes + "GeoCoordinates",
                                                        new XElement(OICPNS.CommonTypes + "DecimalDegree",
                                                           new XElement(OICPNS.CommonTypes + "Longitude", SearchCenter.Longitude.ToString(CultureInfo.InvariantCulture.NumberFormat)),
                                                           new XElement(OICPNS.CommonTypes + "Latitude",  SearchCenter.Latitude. ToString(CultureInfo.InvariantCulture.NumberFormat))
                                                        )
                                                    ),

                                                    new XElement(OICPNS.CommonTypes + "Radius", DistanceKM)

                                                )
                                              : null,

                                          (EVSEStatus != null && EVSEStatus.HasValue)
                                              ? new XElement(OICPNS.EVSEStatus + "EvseStatus",  EVSEStatus.Value)
                                              : null

                                     ));

        }

        #endregion


        #region MobileAuthorizeStartXML(EVSEId, EVCOId, PIN, PartnerProductId = null)

        /// <summary>
        /// Create a new mobile AuthorizeStart request.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="EVCOId"></param>
        /// <param name="PIN"></param>
        /// <param name="PartnerProductId">Your charging product identification (optional).</param>
        public static XElement MobileAuthorizeStartXML(EVSE_Id  EVSEId,
                                                       eMA_Id   EVCOId,
                                                       String   PIN,
                                                       String   PartnerProductId = null)
        {

            return SOAP.Encapsulation(new XElement(OICPNS.MobileAuthorization + "eRoamingMobileAuthorizeStart",

                                          new XElement(OICPNS.MobileAuthorization + "EvseID", EVSEId.ToString()),

                                          new XElement(OICPNS.MobileAuthorization + "QRCodeIdentification",
                                              new XElement(OICPNS.CommonTypes + "EVCOID", EVCOId.ToString()),
                                              new XElement(OICPNS.CommonTypes + "PIN",    PIN)
                                          ),

                                          (PartnerProductId != null)
                                              ? new XElement(OICPNS.MobileAuthorization + "PartnerProductID", PartnerProductId)
                                              : null

                                     ));

        }

        #endregion

        #region MobileRemoteStartXML(SessionId = null)

        /// <summary>
        /// Create a new mobile AuthorizeStart request.
        /// </summary>
        /// <param name="SessionId">An OICP session identification from the MobileAuthorizationStart response.</param>
        public static XElement MobileRemoteStartXML(ChargingSession_Id  SessionId = null)
        {

            return SOAP.Encapsulation(new XElement(OICPNS.MobileAuthorization + "eRoamingMobileRemoteStart",
                                          new XElement(OICPNS.EVSESearch + "SessionID", (SessionId != null) ? SessionId : ChargingSession_Id.New)
                                     ));

        }

        #endregion

        #region MobileRemoteStopXML(SessionId = null)

        /// <summary>
        /// Create a new mobile AuthorizeStop request.
        /// </summary>
        /// <param name="SessionId">The OICP session identification from the MobileAuthorizationStart response.</param>
        public static XElement MobileRemoteStopXML(ChargingSession_Id SessionId = null)
        {

            return SOAP.Encapsulation(new XElement(OICPNS.MobileAuthorization + "eRoamingMobileRemoteStop",
                                          new XElement(OICPNS.EVSESearch + "SessionID", (SessionId != null) ? SessionId : ChargingSession_Id.New)
                                     ));

        }

        #endregion

    }

}
