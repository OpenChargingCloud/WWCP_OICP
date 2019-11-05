/*
 * Copyright (c) 2014-2019 GraphDefined GmbH
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
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2
{

    /// <summary>
    /// An OICP Electric Vehicle Supply Equipment (EVSE).
    /// This is meant to be one electrical circuit which can charge an electric vehicle.
    /// </summary>
    public class EVSEDataRecord : ACustomData,
                                  IEquatable <EVSEDataRecord>,
                                  IComparable<EVSEDataRecord>,
                                  IComparable
    {

        #region Data

        private static readonly Regex HotlinePhoneNumberRegExpr  = new Regex("\\+[^0-9]");

        #endregion

        #region Properties

        /// <summary>
        /// The unique identification of the Electric Vehicle Supply Equipment (EVSE).
        /// </summary>
        public EVSE_Id                        Id                          { get; }


        /// <summary>
        /// The delta type when this EVSE data record was just downloaded.
        /// </summary>
        public DeltaTypes?                    DeltaType                   { get; }

        /// <summary>
        /// The last update timestamp of this EVSE data record.
        /// </summary>
        public DateTime?                      LastUpdate                  { get; }


        /// <summary>
        /// The identification of the charging station hosting this EVSE.
        /// </summary>
        public ChargingStation_Id?            ChargingStationId           { get; }

        /// <summary>
        /// The identification of the charging pool hosting this EVSE.
        /// </summary>
        public ChargingPool_Id?               ChargingPoolId              { get; }

        /// <summary>
        /// The multi-language name of the charging station hosting this EVSE.
        /// </summary>
        public I18NString                     ChargingStationName         { get; }

        /// <summary>
        /// The address of this EVSE.
        /// </summary>
        public Address                        Address                     { get; }

        /// <summary>
        /// The geo coordinate of this EVSE.
        /// </summary>
        public GeoCoordinate?                 GeoCoordinate               { get; }

        /// <summary>
        /// The types of charging plugs attached to this EVSE.
        /// </summary>
        public PlugTypes                      Plugs                       { get; }

        /// <summary>
        /// The charging facilities at this EVSE.
        /// </summary>
        public IEnumerable<ChargingFacility>  ChargingFacilities          { get; }

        /// <summary>
        /// The charging modes this EVSE supports.
        /// </summary>
        public ChargingModes                  ChargingModes               { get; }

        /// <summary>
        /// The authentication modes this EVSE supports.
        /// </summary>
        public AuthenticationModes            AuthenticationModes         { get; }

        /// <summary>
        /// The maximum capacity this EVSE provides.
        /// </summary>
        public Single?                        MaxCapacity                 { get; }

        /// <summary>
        /// The payment options this EVSE supports.
        /// </summary>
        public PaymentOptions                 PaymentOptions              { get; }

        /// <summary>
        /// A list of "value added services" this EVSE supports.
        /// </summary>
        public ValueAddedServices             ValueAddedServices          { get; }

        /// <summary>
        /// The accessibility of this EVSE.
        /// </summary>
        public AccessibilityTypes             Accessibility               { get; }

        /// <summary>
        /// The phone number of the Charging Station Operators hotline.
        /// </summary>
        public String                         HotlinePhoneNumber          { get; }

        /// <summary>
        /// Additional multi-language information about this EVSE.
        /// </summary>
        public I18NString                     AdditionalInfo              { get; }

        /// <summary>
        /// The geo coordinate of the entrance to this EVSE.
        /// </summary>
        public GeoCoordinate?                 GeoChargingPointEntrance    { get; }

        /// <summary>
        /// Whether this EVSE is open 24/7.
        /// </summary>
        public Boolean                        IsOpen24Hours               { get; }

        /// <summary>
        /// The opening times of this EVSE.
        /// </summary>
        public String                         OpeningTimes                 { get; }

        /// <summary>
        /// An optional hub operator of this EVSE.
        /// </summary>
        public HubOperator_Id?                HubOperatorId               { get; }

        /// <summary>
        /// An optional clearing house of this EVSE.
        /// </summary>
        public ClearingHouse_Id?              ClearingHouseId             { get; }

        /// <summary>
        /// Whether this EVSE is Hubject compatible.
        /// </summary>
        public Boolean                        IsHubjectCompatible         { get; }

        /// <summary>
        /// Whether this EVSE provides dynamic status information.
        /// </summary>
        public Boolean                        DynamicInfoAvailable        { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EVSE data record.
        /// </summary>
        /// <param name="Id">A unique EVSE identification.</param>
        /// <param name="DeltaType">The delta type when this EVSE data record was just downloaded.</param>
        /// <param name="LastUpdate">The last update timestamp of this EVSE data record.</param>
        /// 
        /// <param name="ChargingStationId">The identification of the charging station hosting this EVSE.</param>
        /// <param name="ChargingPoolId">The identification of the charging pool hosting this EVSE.</param>
        /// <param name="ChargingStationName">The multi-language name of the charging station hosting this EVSE.</param>
        /// <param name="Address">The address of this EVSE.</param>
        /// <param name="GeoCoordinate">The geo coordinate of this EVSE.</param>
        /// <param name="Plugs">The types of charging plugs attached to this EVSE.</param>
        /// <param name="ChargingFacilities">The charging facilities at this EVSE.</param>
        /// <param name="ChargingModes">The charging modes this EVSE supports.</param>
        /// <param name="AuthenticationModes">The authentication modes this EVSE supports.</param>
        /// <param name="MaxCapacity">The maximum capacity this EVSE provides.</param>
        /// <param name="PaymentOptions">The payment options this EVSE supports.</param>
        /// <param name="ValueAddedServices">A list of "value added services" this EVSE supports.</param>
        /// <param name="Accessibility">The accessibility of this EVSE.</param>
        /// <param name="HotlinePhoneNumber">The phone number of the Charging Station Operators hotline.</param>
        /// <param name="AdditionalInfo">Additional multi-language information about this EVSE.</param>
        /// <param name="GeoChargingPointEntrance">The geo coordinate of the entrance to this EVSE.</param>
        /// <param name="IsOpen24Hours">Whether this EVSE is open 24/7.</param>
        /// <param name="OpeningTimes">The opening times of this EVSE.</param>
        /// <param name="HubOperatorId">An optional hub operator of this EVSE.</param>
        /// <param name="ClearingHouseId">An optional clearing house of this EVSE.</param>
        /// <param name="IsHubjectCompatible">Whether this EVSE is Hubject compatible.</param>
        /// <param name="DynamicInfoAvailable">Whether this EVSE provides dynamic status information.</param>
        /// 
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public EVSEDataRecord(EVSE_Id                              Id,
                              DeltaTypes?                          DeltaType,
                              DateTime?                            LastUpdate,

                              ChargingStation_Id?                  ChargingStationId           = null,
                              ChargingPool_Id?                     ChargingPoolId              = null,
                              I18NString                           ChargingStationName         = null,
                              Address                              Address                     = null,
                              GeoCoordinate?                       GeoCoordinate               = null,
                              PlugTypes                            Plugs                       = PlugTypes.Unspecified,
                              IEnumerable<ChargingFacility>        ChargingFacilities          = null,
                              ChargingModes                        ChargingModes               = ChargingModes.Unspecified,
                              AuthenticationModes                  AuthenticationModes         = AuthenticationModes.Unknown,
                              Single?                              MaxCapacity                 = null,
                              PaymentOptions                       PaymentOptions              = PaymentOptions.Unspecified,
                              ValueAddedServices                   ValueAddedServices          = ValueAddedServices.None,
                              AccessibilityTypes                   Accessibility               = AccessibilityTypes.Free_publicly_accessible,
                              String                               HotlinePhoneNumber          = null,
                              I18NString                           AdditionalInfo              = null,
                              GeoCoordinate?                       GeoChargingPointEntrance    = null,
                              Boolean                              IsOpen24Hours               = true,
                              String                               OpeningTimes                = null,
                              HubOperator_Id?                      HubOperatorId               = null,
                              ClearingHouse_Id?                    ClearingHouseId             = null,
                              Boolean                              IsHubjectCompatible         = true,
                              Boolean                              DynamicInfoAvailable        = true,

                              IReadOnlyDictionary<String, Object>  CustomData                  = null)

            : base(CustomData)

        {

            #region Initial checks

            if (Address == null)
                throw new ArgumentNullException(nameof(Address),              "The given address must not be null!");

            if (GeoCoordinate == null)
                throw new ArgumentNullException(nameof(GeoCoordinate),        "The given geo coordinate must not be null!");

            if (Plugs == PlugTypes.Unspecified)
                throw new ArgumentNullException(nameof(Plugs),                "The given plugs must not be empty!");

            if (AuthenticationModes == AuthenticationModes.Unknown)
                throw new ArgumentNullException(nameof(AuthenticationModes),  "The given authentication modes must not be null or empty!");

            if (HotlinePhoneNumber == null || HotlinePhoneNumber.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(HotlinePhoneNumber),   "The given hotline phone number must not be null or empty!");

            #endregion

            this.Id                         = Id;

            this.DeltaType                  = DeltaType;
            this.LastUpdate                 = LastUpdate;

            this.ChargingStationId          = ChargingStationId;
            this.ChargingPoolId             = ChargingPoolId;
            this.ChargingStationName        = ChargingStationName              ?? new I18NString();
            this.Address                    = Address;
            this.GeoCoordinate              = GeoCoordinate;
            this.Plugs                      = Plugs;
            this.ChargingModes              = ChargingModes;
            this.ChargingFacilities         = ChargingFacilities               ?? new ChargingFacility[0];
            this.AuthenticationModes        = AuthenticationModes;
            this.MaxCapacity                = MaxCapacity;
            this.PaymentOptions             = PaymentOptions;
            this.ValueAddedServices         = ValueAddedServices;
            this.Accessibility              = Accessibility;
            this.HotlinePhoneNumber         = HotlinePhoneNumber;
            this.AdditionalInfo             = AdditionalInfo.SubstringMax(200) ?? new I18NString();
            this.GeoChargingPointEntrance   = GeoChargingPointEntrance;
            this.IsOpen24Hours              = IsOpen24Hours;
            this.OpeningTimes               = OpeningTimes;
            this.HubOperatorId              = HubOperatorId;
            this.ClearingHouseId            = ClearingHouseId;
            this.IsHubjectCompatible        = IsHubjectCompatible;
            this.DynamicInfoAvailable       = DynamicInfoAvailable;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv      = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:EVSEData     = "http://www.hubject.com/b2b/services/evsedata/EVSEData/v2.1"
        //                   xmlns:CommonTypes  = "http://www.hubject.com/b2b/services/commontypes/EVSEData/v2.0">
        //
        // [...]
        //
        //    <EVSEData:eRoamingEvseDataRecord deltaType="?" lastUpdate="?">
        //
        //       <EVSEData:EvseId>?</EVSEData:EvseId>
        //
        //       <!--Optional:-->
        //       <EVSEData:ChargingStationId>?</EVSEData:ChargingStationId>
        //       <!--Optional:-->
        //       <EVSEData:ChargingStationName>?</EVSEData:ChargingStationName>
        //       <!--Optional:-->
        //       <EVSEData:EnChargingStationName>?</EVSEData:EnChargingStationName>
        //
        //       <EVSEData:Address>
        //          <CommonTypes:Country>?</CommonTypes:Country>
        //          <CommonTypes:City>?</CommonTypes:City>
        //          <CommonTypes:Street>?</CommonTypes:Street>
        //          <!--Optional:-->
        //          <CommonTypes:PostalCode>?</CommonTypes:PostalCode>
        //          <!--Optional:-->
        //          <CommonTypes:HouseNum>?</CommonTypes:HouseNum>
        //          <!--Optional:-->
        //          <CommonTypes:Floor>?</CommonTypes:Floor>
        //          <!--Optional:-->
        //          <CommonTypes:Region>?</CommonTypes:Region>
        //          <!--Optional:-->
        //          <CommonTypes:TimeZone>?</CommonTypes:TimeZone>
        //       </EVSEData:Address>
        //
        //       <EVSEData:GeoCoordinates>
        //          <!--You have a CHOICE of the next 3 items at this level-->
        //          <CommonTypes:Google>
        //             <CommonTypes:Coordinates>?</CommonTypes:Coordinates>
        //          </CommonTypes:Google>
        //          <CommonTypes:DecimalDegree>
        //             <CommonTypes:Longitude>?</CommonTypes:Longitude>
        //             <CommonTypes:Latitude>?</CommonTypes:Latitude>
        //          </CommonTypes:DecimalDegree>
        //          <CommonTypes:DegreeMinuteSeconds>
        //             <CommonTypes:Longitude>?</CommonTypes:Longitude>
        //             <CommonTypes:Latitude>?</CommonTypes:Latitude>
        //          </CommonTypes:DegreeMinuteSeconds>
        //       </EVSEData:GeoCoordinates>
        //
        //       <EVSEData:Plugs>
        //          <!--1 or more repetitions:-->
        //          <EVSEData:Plug>?</EVSEData:Plug>
        //       </EVSEData:Plugs>
        //
        //       <!--Optional:-->
        //       <EVSEData:ChargingFacilities>
        //          <!--1 or more repetitions:-->
        //          <EVSEData:ChargingFacility>?</EVSEData:ChargingFacility>
        //       </EVSEData:ChargingFacilities>
        //
        //       <!--Optional:-->
        //       <EVSEData:ChargingModes>
        //          <!--1 or more repetitions:-->
        //          <EVSEData:ChargingMode>?</EVSEData:ChargingMode>
        //       </EVSEData:ChargingModes>
        //
        //       <EVSEData:AuthenticationModes>
        //          <!--1 or more repetitions:-->
        //          <EVSEData:AuthenticationMode>?</EVSEData:AuthenticationMode>
        //       </EVSEData:AuthenticationModes>
        //
        //       <!--Optional:-->
        //       <EVSEData:MaxCapacity>?</EVSEData:MaxCapacity>
        //
        //       <!--Optional:-->
        //       <EVSEData:PaymentOptions>
        //          <!--1 or more repetitions:-->
        //          <EVSEData:PaymentOption>?</EVSEData:PaymentOption>
        //       </EVSEData:PaymentOptions>
        //
        //       <EVSEData:ValueAddedServices>
        //          <!--1 or more repetitions:-->
        //          <EVSEData:ValueAddedService>?</EVSEData:ValueAddedService>
        //       </EVSEData:ValueAddedServices>
        //
        //       <EVSEData:Accessibility>?</EVSEData:Accessibility>
        //       <EVSEData:HotlinePhoneNum>?</EVSEData:HotlinePhoneNum>
        //
        //       <!--Optional:-->
        //       <EVSEData:AdditionalInfo>?</EVSEData:AdditionalInfo>
        //
        //       <!--Optional:-->
        //       <EVSEData:EnAdditionalInfo>?</EVSEData:EnAdditionalInfo>
        //
        //       <!--Optional:-->
        //       <EVSEData:GeoChargingPointEntrance>
        //          <!--You have a CHOICE of the next 3 items at this level-->
        //          <CommonTypes:Google>
        //             <CommonTypes:Coordinates>?</CommonTypes:Coordinates>
        //          </CommonTypes:Google>
        //          <CommonTypes:DecimalDegree>
        //             <CommonTypes:Longitude>?</CommonTypes:Longitude>
        //             <CommonTypes:Latitude>?</CommonTypes:Latitude>
        //          </CommonTypes:DecimalDegree>
        //          <CommonTypes:DegreeMinuteSeconds>
        //             <CommonTypes:Longitude>?</CommonTypes:Longitude>
        //             <CommonTypes:Latitude>?</CommonTypes:Latitude>
        //          </CommonTypes:DegreeMinuteSeconds>
        //       </EVSEData:GeoChargingPointEntrance>
        //
        //       <EVSEData:IsOpen24Hours>?</EVSEData:IsOpen24Hours>
        //       <!--Optional:-->
        //       <EVSEData:OpeningTime>?</EVSEData:OpeningTime>
        //
        //       <!--Optional:-->
        //       <EVSEData:HubOperatorID>?</EVSEData:HubOperatorID>
        //
        //       <!--Optional:-->
        //       <EVSEData:ClearinghouseID>?</EVSEData:ClearinghouseID>
        //       <EVSEData:IsHubjectCompatible>?</EVSEData:IsHubjectCompatible>
        //       <EVSEData:DynamicInfoAvailable>?</EVSEData:DynamicInfoAvailable>
        //
        //    </EVSEData:eRoamingEvseDataRecord>
        //
        // [...]
        //
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (EVSEDataRecordXML,  ..., OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OICP EVSE data record.
        /// </summary>
        /// <param name="EVSEDataRecordXML">The XML to parse.</param>
        /// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSEDataRecord XML elements.</param>
        /// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        /// <param name="CustomChargingFacilityParser">A delegate to parse custom ChargingFacility XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static EVSEDataRecord Parse(XElement                                   EVSEDataRecordXML,
                                           CustomXMLParserDelegate<EVSEDataRecord>    CustomEVSEDataRecordParser     = null,
                                           CustomXMLParserDelegate<Address>           CustomAddressParser            = null,
                                           CustomXMLParserDelegate<ChargingFacility>  CustomChargingFacilityParser   = null,
                                           OnExceptionDelegate                        OnException                    = null)
        {

            if (TryParse(EVSEDataRecordXML,
                         out EVSEDataRecord evseDataRecord,
                         CustomEVSEDataRecordParser,
                         CustomAddressParser,
                         CustomChargingFacilityParser,
                         OnException))

                return evseDataRecord;

            return null;

        }

        #endregion

        #region (static) Parse   (EVSEDataRecordText, ..., OnException = null)

        /// <summary>
        /// Parse the given text representation of an OICP EVSE data record.
        /// </summary>
        /// <param name="EVSEDataRecordText">The text to parse.</param>
        /// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSEDataRecord XML elements.</param>
        /// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        /// <param name="CustomChargingFacilityParser">A delegate to parse custom ChargingFacility XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static EVSEDataRecord Parse(String                                     EVSEDataRecordText,
                                           CustomXMLParserDelegate<EVSEDataRecord>    CustomEVSEDataRecordParser     = null,
                                           CustomXMLParserDelegate<Address>           CustomAddressParser            = null,
                                           CustomXMLParserDelegate<ChargingFacility>  CustomChargingFacilityParser   = null,
                                           OnExceptionDelegate                        OnException                    = null)
        {

            if (TryParse(EVSEDataRecordText,
                         out EVSEDataRecord evseDataRecord,
                         CustomEVSEDataRecordParser,
                         CustomAddressParser,
                         CustomChargingFacilityParser,
                         OnException))

                return evseDataRecord;

            return null;

        }

        #endregion

        #region (static) TryParse(EVSEDataRecordXML,  out EVSEDataRecord, ..., OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OICP EVSE data record.
        /// </summary>
        /// <param name="EVSEDataRecordXML">The XML to parse.</param>
        /// <param name="EVSEDataRecord">The parsed EVSE data record.</param>
        /// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSEDataRecord XML elements.</param>
        /// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        /// <param name="CustomChargingFacilityParser">A delegate to parse custom ChargingFacility XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                                   EVSEDataRecordXML,
                                       out EVSEDataRecord                         EVSEDataRecord,
                                       CustomXMLParserDelegate<EVSEDataRecord>    CustomEVSEDataRecordParser     = null,
                                       CustomXMLParserDelegate<Address>           CustomAddressParser            = null,
                                       CustomXMLParserDelegate<ChargingFacility>  CustomChargingFacilityParser   = null,
                                       OnExceptionDelegate                        OnException                    = null)
        {

            try
            {

                if (EVSEDataRecordXML.Name != OICPNS.EVSEData + "EvseDataRecord")
                {
                    EVSEDataRecord = null;
                    return false;
                }

                var EVSEId = EVSE_Id.Parse(EVSEDataRecordXML.ElementValueOrFail(OICPNS.EVSEData + "EvseId", "Missing 'EvseId'-XML tag!"));

                #region XML Attribute: LastUpdate

                DateTime.TryParse(EVSEDataRecordXML.AttributeValueOrDefault(XName.Get("lastUpdate"), ""), out DateTime _LastUpdate);

                #endregion

                #region ChargingStationName

                var _ChargingStationName = new I18NString();

                EVSEDataRecordXML.IfValueIsNotNullOrEmpty(OICPNS.EVSEData + "ChargingStationName",
                                                          v => _ChargingStationName.Add(Languages.deu, v));

                EVSEDataRecordXML.IfValueIsNotNullOrEmpty(OICPNS.EVSEData + "EnChargingStationName",
                                                          v => _ChargingStationName.Add(Languages.eng, v));

                #endregion

                #region MaxCapacity in kWh

                var _MaxCapacity_kWh = EVSEDataRecordXML.
                                           ElementValueOrDefault(OICPNS.EVSEData + "MaxCapacity", String.Empty).
                                           Trim();

                Single _MaxCapacity = 0.0f;

                if (_MaxCapacity_kWh.IsNotNullOrEmpty())
                    Single.TryParse(_MaxCapacity_kWh, out _MaxCapacity);

                #endregion

                #region AdditionalInfo

                var _AdditionalInfo = new I18NString();

                foreach (var infoTextXML in EVSEDataRecordXML.
                                                Element (OICPNS.EVSEData + "AdditionalInfo").
                                                Elements(OICPNS.EVSEData + "InfoText"))
                {

                    var lang = infoTextXML.Attribute("lang").Value?.Trim().ToLower();

                    if (lang.Length == 2 && Enum.TryParse(lang, out Languages Language))
                        _AdditionalInfo.Add(Language, infoTextXML.Value);

                }

                #endregion


                EVSEDataRecord = new EVSEDataRecord(

                    EVSEId,

                    XML_IO.AsDeltaType(EVSEDataRecordXML.AttributeValueOrDefault(XName.Get("deltaType"), "")),

                    _LastUpdate,

                    EVSEDataRecordXML.MapValueOrNullable(OICPNS.EVSEData + "ChargingStationId",
                                                         ChargingStation_Id.Parse),

                    EVSEDataRecordXML.MapValueOrNullable(OICPNS.EVSEData + "ChargingPoolId",
                                                         ChargingPool_Id.Parse),

                    _ChargingStationName,

                    EVSEDataRecordXML.MapElementOrFail  (OICPNS.EVSEData + "Address",
                                                         (xml, e) => Address.Parse(xml,
                                                                                   CustomAddressParser,
                                                                                   e),
                                                         OnException),

                    XML_IO.ParseGeoCoordinatesXML(EVSEDataRecordXML.ElementOrFail(OICPNS.EVSEData + "GeoCoordinates", "Missing 'GeoCoordinates'-XML tag!")),

                    EVSEDataRecordXML.MapValuesOrFail   (OICPNS.EVSEData + "Plugs",
                                                         OICPNS.EVSEData + "Plug",
                                                         XML_IO.AsPlugType).
                                                         Reduce(),

                    EVSEDataRecordXML.MapValuesOrDefault(OICPNS.EVSEData + "ChargingFacilities",
                                                         OICPNS.EVSEData + "ChargingFacility",
                                                         xml => ChargingFacility.Parse(xml,
                                                                                       CustomChargingFacilityParser,
                                                                                       OnException),
                                                         null), //new ChargingFacility[0] as IEnumerable<ChargingFacility>),

                    EVSEDataRecordXML.MapValuesOrDefault(OICPNS.EVSEData + "ChargingModes",
                                                         OICPNS.EVSEData + "ChargingMode",
                                                         XML_IO.AsChargingMode,
                                                         ChargingModes.Unspecified).
                                                         Reduce(),

                    EVSEDataRecordXML.MapValuesOrFail(OICPNS.EVSEData + "AuthenticationModes",
                                                         OICPNS.EVSEData + "AuthenticationMode",
                                                         XML_IO.AsAuthenticationMode).
                                                         Reduce(),

                    _MaxCapacity,

                    EVSEDataRecordXML.MapValuesOrDefault(OICPNS.EVSEData + "PaymentOptions",
                                                         OICPNS.EVSEData + "PaymentOption",
                                                         XML_IO.AsPaymetOption,
                                                         PaymentOptions.Unspecified).
                                                         Reduce(),

                    EVSEDataRecordXML.MapValuesOrDefault(OICPNS.EVSEData + "ValueAddedServices",
                                                         OICPNS.EVSEData + "ValueAddedService",
                                                         XML_IO.AsValueAddedService,
                                                         ValueAddedServices.None).
                                                         Reduce(),

                    XML_IO.AsAccessibilityType(EVSEDataRecordXML.
                                                   ElementValueOrFail(OICPNS.EVSEData + "Accessibility").
                                                   Trim()),

                    EVSEDataRecordXML.ElementValueOrDefault(OICPNS.EVSEData + "HotlinePhoneNum").
                                      Trim(),


                    _AdditionalInfo,

                    EVSEDataRecordXML.MapElement(OICPNS.CommonTypes + "GeoChargingPointEntrance",
                                                 XML_IO.ParseGeoCoordinatesXML),

                    EVSEDataRecordXML.MapValueOrFail(OICPNS.EVSEData + "IsOpen24Hours",
                                                     s => s == "true"),

                    //ToDo!!!!!!!!!!!!!!!!!!!!
                    EVSEDataRecordXML.ElementValueOrDefault(OICPNS.EVSEData + "OpeningTimes"),

                    EVSEDataRecordXML.MapValueOrNullable(OICPNS.EVSEData + "HubOperatorID",
                                                         HubOperator_Id.Parse),

                    EVSEDataRecordXML.MapValueOrNullable(OICPNS.EVSEData + "ClearinghouseID",
                                                         ClearingHouse_Id.Parse),

                    EVSEDataRecordXML.ElementValueOrFail(OICPNS.EVSEData + "IsHubjectCompatible").
                                      Trim() == "true",

                    EVSEDataRecordXML.ElementValueOrFail(OICPNS.EVSEData + "DynamicInfoAvailable").
                                      Trim() != "false"

                );

                if (CustomEVSEDataRecordParser != null)
                    EVSEDataRecord = CustomEVSEDataRecordParser(EVSEDataRecordXML, EVSEDataRecord);

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, EVSEDataRecordXML, e);

                EVSEDataRecord = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(EVSEDataRecordText, out EVSEDataRecord, ..., OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OICP EVSE data record.
        /// </summary>
        /// <param name="EVSEDataRecordText">The text to parse.</param>
        /// <param name="EVSEDataRecord">The parsed EVSE data record.</param>
        /// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSEDataRecord XML elements.</param>
        /// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                     EVSEDataRecordText,
                                       out EVSEDataRecord                         EVSEDataRecord,
                                       CustomXMLParserDelegate<EVSEDataRecord>    CustomEVSEDataRecordParser     = null,
                                       CustomXMLParserDelegate<Address>           CustomAddressParser            = null,
                                       CustomXMLParserDelegate<ChargingFacility>  CustomChargingFacilityParser   = null,
                                       OnExceptionDelegate                        OnException                    = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(EVSEDataRecordText).Root,
                             out EVSEDataRecord,
                             CustomEVSEDataRecordParser,
                             CustomAddressParser,
                             CustomChargingFacilityParser,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, EVSEDataRecordText, e);
            }

            EVSEDataRecord = null;
            return false;

        }

        #endregion

        #region ToXML(XName = null, IncludeMetadata = false, CustomEVSEDataRecordSerializer = null, CustomAddressSerializer = null)

        /// <summary>
        /// Return an XML representation of this EVSE data record.
        /// </summary>
        /// <param name="XName">The XML name to use.</param>
        /// <param name="IncludeMetadata">Include deltaType and lastUpdate meta data.</param>
        /// <param name="CustomEVSEDataRecordSerializer">A delegate to serialize custom EVSEDataRecord XML elements.</param>
        /// <param name="CustomAddressSerializer">A delegate to serialize custom Address XML elements.</param>
        /// <param name="CustomChargingFacilitySerializer">A delegate to serialize custom ChargingFacility XML elements.</param>
        public XElement ToXML(XName                                          XName                              = null,
                              Boolean                                        IncludeMetadata                    = false,
                              CustomXMLSerializerDelegate<EVSEDataRecord>    CustomEVSEDataRecordSerializer     = null,
                              CustomXMLSerializerDelegate<Address>           CustomAddressSerializer            = null,
                              CustomXMLSerializerDelegate<ChargingFacility>  CustomChargingFacilitySerializer   = null)

        {

            var XML = new XElement(OICPNS.EVSEData + "EvseDataRecord",

                          IncludeMetadata && DeltaType.HasValue
                              ? new XAttribute(OICPNS.EVSEData + "deltaType",  DeltaType.ToString())
                              : null,

                          IncludeMetadata && LastUpdate.HasValue
                              ? new XAttribute(OICPNS.EVSEData + "lastUpdate", LastUpdate.ToString())
                              : null,

                          new XElement(OICPNS.EVSEData + "EvseID",                Id.ToString()),
                          new XElement(OICPNS.EVSEData + "ChargingStationID",     ChargingStationId),
                          new XElement(OICPNS.EVSEData + "ChargingStationName",   ChargingStationName[Languages.deu].SubstringMax(50)),
                          new XElement(OICPNS.EVSEData + "EnChargingStationName", ChargingStationName[Languages.eng].SubstringMax(50)),

                          ChargingPoolId.HasValue
                              ? new XElement(OICPNS.EVSEData + "ChargingPoolID",  ChargingPoolId.Value.ToString())
                              : null,

                          Address.ToXML(OICPNS.EVSEData + "Address",
                                        CustomAddressSerializer),

                          new XElement(OICPNS.EVSEData + "GeoCoordinates",
                              new XElement(OICPNS.CommonTypes + "DecimalDegree",  // Force 0.00... (dot) format!
                                  new XElement(OICPNS.CommonTypes + "Longitude",  GeoCoordinate.Value.Longitude.ToString("{0:0.######}").Replace(",", ".")),// CultureInfo.InvariantCulture.NumberFormat)),
                                  new XElement(OICPNS.CommonTypes + "Latitude",   GeoCoordinate.Value.Latitude. ToString("{0:0.######}").Replace(",", ".")) // CultureInfo.InvariantCulture.NumberFormat))
                              )
                          ),

                          new XElement(OICPNS.EVSEData + "Plugs",
                              Plugs.ToEnumeration().SafeSelect(Plug               => new XElement(OICPNS.EVSEData + "Plug",               XML_IO.AsString(Plug)))
                          ),

                          ChargingFacilities.IsNeitherNullNorEmpty()
                              ? new XElement(OICPNS.EVSEData + "ChargingFacilities",
                                    ChargingFacilities.Select(chargingFacility    => chargingFacility.ToXML(CustomChargingFacilitySerializer)))
                              : null,

                          ChargingModes.ToEnumeration().IsNeitherNullNorEmpty()
                              ? new XElement(OICPNS.EVSEData + "ChargingModes",
                                    ChargingModes.ToEnumeration().Select(ChargingMode => new XElement(OICPNS.EVSEData + "ChargingMode", XML_IO.AsString(ChargingMode))))
                              : null,

                          new XElement(OICPNS.EVSEData + "AuthenticationModes",
                              AuthenticationModes.     ToEnumeration().
                                                       Select(AuthenticationMode => XML_IO.AsString(AuthenticationMode)).
                                                       Where (AuthenticationMode => AuthenticationMode != "Unknown").
                                                       Select(AuthenticationMode => new XElement(OICPNS.EVSEData + "AuthenticationMode", AuthenticationMode))
                          ),

                          MaxCapacity.HasValue
                              ? new XElement(OICPNS.EVSEData + "MaxCapacity", MaxCapacity)
                              : null,

                          PaymentOptions.ToEnumeration().IsNeitherNullNorEmpty()
                              ? new XElement(OICPNS.EVSEData + "PaymentOptions",
                                    PaymentOptions.ToEnumeration().SafeSelect(PaymentOption      => new XElement(OICPNS.EVSEData + "PaymentOption",      XML_IO.AsString(PaymentOption)))
                                )
                              : null,

                          ValueAddedServices.ToEnumeration().IsNeitherNullNorEmpty()
                              ? new XElement(OICPNS.EVSEData + "ValueAddedServices",
                                    ValueAddedServices.ToEnumeration().SafeSelect(ValueAddedService => new XElement(OICPNS.EVSEData + "ValueAddedService", XML_IO.AsString(ValueAddedService)))
                                )
                              : new XElement(OICPNS.EVSEData + "ValueAddedServices", new XElement(OICPNS.EVSEData + "ValueAddedService", "None")),

                          new XElement(OICPNS.EVSEData + "Accessibility",     Accessibility.ToString().Replace("_", " ")),

                          HotlinePhoneNumber.IsNotNullOrEmpty()
                              ? new XElement(OICPNS.EVSEData + "HotlinePhoneNumber", HotlinePhoneNumberRegExpr.Replace(HotlinePhoneNumber, ""))  // RegEx: \+[0-9]{5,15}
                              : null,

                          AdditionalInfo.IsNeitherNullNorEmpty()
                              ? new XElement(OICPNS.EVSEData + "AdditionalInfo",
                                             AdditionalInfo.
                                                 Select(info => new XElement(OICPNS.EVSEData + "InfoText",
                                                                             new XAttribute("lang", info.Language), //ToDo: ISO 639-3 codes for languages is not what OICP expects! Pattern: [a-z]{2,3}(-[A-Z]{2,3}(-[a-zA-Z]{4})?)?(-x-[a-zA-Z0-9]{1,8})?
                                                                             info.Text)))
                              : null,

                          GeoChargingPointEntrance != null
                              ? new XElement(OICPNS.EVSEData + "GeoChargingPointEntrance",
                                  new XElement(OICPNS.CommonTypes + "DecimalDegree",  // Force 0.00... (dot) format!
                                      new XElement(OICPNS.CommonTypes + "Longitude", GeoChargingPointEntrance.Value.Longitude.ToString("{0:0.######}").Replace(",", ".")),// CultureInfo.InvariantCulture.NumberFormat)),
                                      new XElement(OICPNS.CommonTypes + "Latitude",  GeoChargingPointEntrance.Value.Latitude. ToString("{0:0.######}").Replace(",", ".")) // CultureInfo.InvariantCulture.NumberFormat))
                                  )
                              )
                              : null,

                          new XElement(OICPNS.EVSEData + "IsOpen24Hours",         IsOpen24Hours ? "true" : "false"),

                          //ToDo: Implement new OpeningTimes complex type!
                          //OpeningTimes.IsNotNullOrEmpty()
                          //    ? new XElement(OICPNS.EVSEData + "OpeningTime",     OpeningTimes)
                          //    : null,

                          HubOperatorId != null
                              ? new XElement(OICPNS.EVSEData + "HubOperatorID",   HubOperatorId.ToString())
                              : null,

                          ClearingHouseId != null
                              ? new XElement(OICPNS.EVSEData + "ClearinghouseID", ClearingHouseId.ToString())
                              : null,

                          new XElement(OICPNS.EVSEData + "IsHubjectCompatible",   IsHubjectCompatible  ? "true" : "false"),
                          new XElement(OICPNS.EVSEData + "DynamicInfoAvailable",  DynamicInfoAvailable ? "true" : "false")

                   );

            return CustomEVSEDataRecordSerializer != null
                       ? CustomEVSEDataRecordSerializer(this, XML)
                       : XML;

        }

        #endregion


        #region Operator overloading

        #region Operator == (EVSEDataRecord1, EVSEDataRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEDataRecord1">An EVSE data record.</param>
        /// <param name="EVSEDataRecord2">Another EVSE data record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSEDataRecord EVSEDataRecord1, EVSEDataRecord EVSEDataRecord2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EVSEDataRecord1, EVSEDataRecord2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSEDataRecord1 == null) || ((Object) EVSEDataRecord2 == null))
                return false;

            return EVSEDataRecord1.Equals(EVSEDataRecord2);

        }

        #endregion

        #region Operator != (EVSEDataRecord1, EVSEDataRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEDataRecord1">An EVSE data record.</param>
        /// <param name="EVSEDataRecord2">Another EVSE data record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSEDataRecord EVSEDataRecord1, EVSEDataRecord EVSEDataRecord2)
            => !(EVSEDataRecord1 == EVSEDataRecord2);

        #endregion

        #region Operator <  (EVSEDataRecord1, EVSEDataRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEDataRecord1">An EVSE data record.</param>
        /// <param name="EVSEDataRecord2">Another EVSE data record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSEDataRecord EVSEDataRecord1, EVSEDataRecord EVSEDataRecord2)
        {

            if ((Object) EVSEDataRecord1 == null)
                throw new ArgumentNullException(nameof(EVSEDataRecord1), "The given EVSEDataRecord1 must not be null!");

            return EVSEDataRecord1.CompareTo(EVSEDataRecord2) < 0;

        }

        #endregion

        #region Operator <= (EVSEDataRecord1, EVSEDataRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEDataRecord1">An EVSE data record.</param>
        /// <param name="EVSEDataRecord2">Another EVSE data record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSEDataRecord EVSEDataRecord1, EVSEDataRecord EVSEDataRecord2)
            => !(EVSEDataRecord1 > EVSEDataRecord2);

        #endregion

        #region Operator >  (EVSEDataRecord1, EVSEDataRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEDataRecord1">An EVSE data record.</param>
        /// <param name="EVSEDataRecord2">Another EVSE data record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSEDataRecord EVSEDataRecord1, EVSEDataRecord EVSEDataRecord2)
        {

            if ((Object) EVSEDataRecord1 == null)
                throw new ArgumentNullException(nameof(EVSEDataRecord1), "The given EVSEDataRecord1 must not be null!");

            return EVSEDataRecord1.CompareTo(EVSEDataRecord2) > 0;

        }

        #endregion

        #region Operator >= (EVSEDataRecord1, EVSEDataRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEDataRecord1">An EVSE data record.</param>
        /// <param name="EVSEDataRecord2">Another EVSE data record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSEDataRecord EVSEDataRecord1, EVSEDataRecord EVSEDataRecord2)
            => !(EVSEDataRecord1 < EVSEDataRecord2);

        #endregion

        #endregion

        #region IComparable<EVSEDataRecord> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            var EVSEDataRecord = Object as EVSEDataRecord;
            if ((Object) EVSEDataRecord == null)
                throw new ArgumentException("The given object is not an EVSE data record identification!", nameof(Object));

            return CompareTo(EVSEDataRecord);

        }

        #endregion

        #region CompareTo(EVSEDataRecord)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEDataRecord">An object to compare with.</param>
        public Int32 CompareTo(EVSEDataRecord EVSEDataRecord)
        {

            if ((Object) EVSEDataRecord == null)
                throw new ArgumentNullException(nameof(EVSEDataRecord), "The given EVSE data record must not be null!");

            return Id.CompareTo(EVSEDataRecord.Id);

        }

        #endregion

        #endregion

        #region IEquatable<EVSEDataRecord> Members

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

            var EVSEDataRecord = Object as EVSEDataRecord;
            if ((Object) EVSEDataRecord == null)
                return false;

            return Equals(EVSEDataRecord);

        }

        #endregion

        #region Equals(EVSEDataRecord)

        /// <summary>
        /// Compares two EVSE data records for equality.
        /// </summary>
        /// <param name="EVSEDataRecord">An EVSE data record to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSEDataRecord EVSEDataRecord)
        {

            if ((Object) EVSEDataRecord == null)
                return false;

            return Id.Equals(EVSEDataRecord.Id);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
            => Id.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
            => Id.ToString();

        #endregion


        #region ToBuilder(NewEVSEId = null)

        /// <summary>
        /// Return a builder for this EVSE data record.
        /// </summary>
        /// <param name="NewEVSEId">An optional new EVSE identification.</param>
        public Builder ToBuilder(EVSE_Id? NewEVSEId = null)

            => new Builder(
                   NewEVSEId ?? Id,

                   DeltaType,
                   LastUpdate,

                   ChargingStationId,
                   ChargingPoolId,
                   ChargingStationName,
                   Address,
                   GeoCoordinate,
                   Plugs,
                   ChargingFacilities,
                   ChargingModes,
                   AuthenticationModes,
                   MaxCapacity,
                   PaymentOptions,
                   ValueAddedServices,
                   Accessibility,
                   HotlinePhoneNumber,
                   AdditionalInfo,
                   GeoChargingPointEntrance,
                   IsOpen24Hours,
                   OpeningTimes,
                   HubOperatorId,
                   ClearingHouseId,
                   IsHubjectCompatible,
                   DynamicInfoAvailable,
                   CustomData);

        #endregion

        #region (class) Builder

        /// <summary>
        /// An OICP Electric Vehicle Supply Equipment (EVSE).
        /// This is meant to be one electrical circuit which can charge a electric vehicle.
        /// </summary>
        public new class Builder : ACustomData.Builder
        {

            #region Properties

            /// <summary>
            /// The unique identification of the Electric Vehicle Supply Equipment (EVSE).
            /// </summary>
            public EVSE_Id                        Id                          { get; }


            /// <summary>
            /// The delta type when this EVSE data record was just downloaded.
            /// </summary>
            public DeltaTypes?                    DeltaType                   { get; set; }

            /// <summary>
            /// The last update timestamp of this EVSE data record.
            /// </summary>
            public DateTime?                      LastUpdate                  { get; set; }


            /// <summary>
            /// The identification of the charging station hosting this EVSE.
            /// </summary>
            public ChargingStation_Id?            ChargingStationId           { get; set; }

            /// <summary>
            /// The identification of the charging pool hosting this EVSE.
            /// </summary>
            public ChargingPool_Id?               ChargingPoolId              { get; set; }

            /// <summary>
            /// The multi-language name of the charging station hosting this EVSE.
            /// </summary>
            public I18NString                     ChargingStationName         { get; set; }

            /// <summary>
            /// The address of this EVSE.
            /// </summary>
            public Address                        Address                     { get; set; }

            /// <summary>
            /// The geo coordinate of this EVSE.
            /// </summary>
            public GeoCoordinate?                 GeoCoordinate               { get; set; }

            /// <summary>
            /// The types of charging plugs attached to this EVSE.
            /// </summary>
            public PlugTypes                      Plugs                       { get; set; }

            /// <summary>
            /// The charging facilities at this EVSE.
            /// </summary>
            public IEnumerable<ChargingFacility>  ChargingFacilities          { get; set; }

            /// <summary>
            /// The charging modes this EVSE supports.
            /// </summary>
            public ChargingModes                  ChargingModes               { get; set; }

            /// <summary>
            /// The authentication modes this EVSE supports.
            /// </summary>
            public AuthenticationModes            AuthenticationModes         { get; set; }

            /// <summary>
            /// The maximum capacity this EVSE provides.
            /// </summary>
            public Single?                        MaxCapacity                 { get; set; }

            /// <summary>
            /// The payment options this EVSE supports.
            /// </summary>
            public PaymentOptions                 PaymentOptions              { get; set; }

            /// <summary>
            /// A list of "value added services" this EVSE supports.
            /// </summary>
            public ValueAddedServices             ValueAddedServices          { get; set; }

            /// <summary>
            /// The accessibility of this EVSE.
            /// </summary>
            public AccessibilityTypes             Accessibility               { get; set; }

            /// <summary>
            /// The phone number of the Charging Station Operators hotline.
            /// </summary>
            public String                         HotlinePhoneNumber          { get; set; }

            /// <summary>
            /// Additional multi-language information about this EVSE.
            /// </summary>
            public I18NString                     AdditionalInfo              { get; set; }

            /// <summary>
            /// The geo coordinate of the entrance to this EVSE.
            /// </summary>
            public GeoCoordinate?                 GeoChargingPointEntrance    { get; set; }

            /// <summary>
            /// Whether this EVSE is open 24/7.
            /// </summary>
            public Boolean                        IsOpen24Hours               { get; set; }

            /// <summary>
            /// The opening times of this EVSE.
            /// </summary>
            public String                         OpeningTime                 { get; set; }

            /// <summary>
            /// An optional hub operator of this EVSE.
            /// </summary>
            public HubOperator_Id?                HubOperatorId               { get; set; }

            /// <summary>
            /// An optional clearing house of this EVSE.
            /// </summary>
            public ClearingHouse_Id?              ClearingHouseId             { get; set; }

            /// <summary>
            /// Whether this EVSE is Hubject compatible.
            /// </summary>
            public Boolean                        IsHubjectCompatible         { get; set; }

            /// <summary>
            /// Whether this EVSE provides dynamic status information.
            /// </summary>
            public Boolean                        DynamicInfoAvailable        { get; set; }

            #endregion

            #region Constructor(s)

            /// <summary>
            /// Create a new EVSE data record builder.
            /// </summary>
            /// <param name="Id">A unique EVSE identification.</param>
            /// <param name="DeltaType">The delta type when this EVSE data record was just downloaded.</param>
            /// <param name="LastUpdate">The last update timestamp of this EVSE data record.</param>
            /// <param name="ChargingStationId">The identification of the charging station hosting this EVSE.</param>
            /// <param name="ChargingStationName">The multi-language name of the charging station hosting this EVSE.</param>
            /// <param name="Address">The address of this EVSE.</param>
            /// <param name="GeoCoordinate">The geo coordinate of this EVSE.</param>
            /// <param name="Plugs">The types of charging plugs attached to this EVSE.</param>
            /// <param name="ChargingFacilities">The charging facilities at this EVSE.</param>
            /// <param name="ChargingModes">The charging modes this EVSE supports.</param>
            /// <param name="AuthenticationModes">The authentication modes this EVSE supports.</param>
            /// <param name="MaxCapacity">The maximum capacity this EVSE provides.</param>
            /// <param name="PaymentOptions">The payment options this EVSE supports.</param>
            /// <param name="ValueAddedServices">A list of "value added services" this EVSE supports.</param>
            /// <param name="Accessibility">The accessibility of this EVSE.</param>
            /// <param name="HotlinePhoneNumber">The phone number of the Charging Station Operators hotline.</param>
            /// <param name="AdditionalInfo">Additional multi-language information about this EVSE.</param>
            /// <param name="GeoChargingPointEntrance">The geo coordinate of the entrance to this EVSE.</param>
            /// <param name="IsOpen24Hours">Whether this EVSE is open 24/7.</param>
            /// <param name="OpeningTime">The opening times of this EVSE.</param>
            /// <param name="HubOperatorId">An optional hub operator of this EVSE.</param>
            /// <param name="ClearingHouseId">An optional clearing house of this EVSE.</param>
            /// <param name="IsHubjectCompatible">Whether this EVSE is Hubject compatible.</param>
            /// <param name="DynamicInfoAvailable">Whether this EVSE provides dynamic status information.</param>
            public Builder(EVSE_Id                        Id,

                           DeltaTypes?                    DeltaType                  = null,
                           DateTime?                      LastUpdate                 = null,

                           ChargingStation_Id?            ChargingStationId          = null,
                           ChargingPool_Id?               ChargingPoolId             = null,
                           I18NString                     ChargingStationName        = null,
                           Address                        Address                    = null,
                           GeoCoordinate?                 GeoCoordinate              = null,
                           PlugTypes                      Plugs                      = PlugTypes.Unspecified,
                           IEnumerable<ChargingFacility>  ChargingFacilities         = null,
                           ChargingModes                  ChargingModes              = ChargingModes.Unspecified,
                           AuthenticationModes            AuthenticationModes        = AuthenticationModes.Unknown,
                           Single?                        MaxCapacity                = null,
                           PaymentOptions                 PaymentOptions             = PaymentOptions.Unspecified,
                           ValueAddedServices             ValueAddedServices         = ValueAddedServices.None,
                           AccessibilityTypes             Accessibility              = AccessibilityTypes.Free_publicly_accessible,
                           String                         HotlinePhoneNumber         = null,
                           I18NString                     AdditionalInfo             = null,
                           GeoCoordinate?                 GeoChargingPointEntrance   = null,
                           Boolean                        IsOpen24Hours              = true,
                           String                         OpeningTime                = null,
                           HubOperator_Id?                HubOperatorId              = null,
                           ClearingHouse_Id?              ClearingHouseId            = null,
                           Boolean                        IsHubjectCompatible        = true,
                           Boolean                        DynamicInfoAvailable       = true,

                           Dictionary<String, Object>     CustomData                 = null)

                : base(CustomData)

            {

                #region Initial checks

                if (Id == null)
                    throw new ArgumentNullException(nameof(Id),                   "The given unique EVSE identification must not be null!");

                if (Address == null)
                    throw new ArgumentNullException(nameof(Address),              "The given address must not be null!");

                if (GeoCoordinate == null)
                    throw new ArgumentNullException(nameof(GeoCoordinate),        "The given geo coordinate must not be null!");

                if (Plugs == PlugTypes.Unspecified)
                    throw new ArgumentNullException(nameof(Plugs),                "The given plugs must not be empty!");

                if (AuthenticationModes == AuthenticationModes.Unknown)
                    throw new ArgumentNullException(nameof(AuthenticationModes),  "The given authentication modes must not be null or empty!");

                if (HotlinePhoneNumber == null || HotlinePhoneNumber.IsNullOrEmpty())
                    throw new ArgumentNullException(nameof(HotlinePhoneNumber),   "The given hotline phone number must not be null or empty!");

                #endregion

                this.Id                        = Id;

                this.DeltaType                 = DeltaType;
                this.LastUpdate                = LastUpdate;

                this.ChargingStationId         = ChargingStationId;
                this.ChargingPoolId            = ChargingPoolId;
                this.ChargingStationName       = ChargingStationName ?? new I18NString();
                this.Address                   = Address;
                this.GeoCoordinate             = GeoCoordinate;
                this.Plugs                     = Plugs;
                this.ChargingModes             = ChargingModes;
                this.ChargingFacilities        = ChargingFacilities  ?? new ChargingFacility[0];
                this.AuthenticationModes       = AuthenticationModes;
                this.MaxCapacity               = MaxCapacity;
                this.PaymentOptions            = PaymentOptions;
                this.ValueAddedServices        = ValueAddedServices;
                this.Accessibility             = Accessibility;
                this.HotlinePhoneNumber        = HotlinePhoneNumber;
                this.AdditionalInfo            = AdditionalInfo      ?? new I18NString();
                this.GeoChargingPointEntrance  = GeoChargingPointEntrance;
                this.IsOpen24Hours             = IsOpen24Hours;
                this.OpeningTime               = OpeningTime;
                this.HubOperatorId             = HubOperatorId;
                this.ClearingHouseId           = ClearingHouseId;
                this.IsHubjectCompatible       = IsHubjectCompatible;
                this.DynamicInfoAvailable      = DynamicInfoAvailable;

            }

            #endregion


            #region Build()

            /// <summary>
            /// Return an immutable version of the EVSE data record.
            /// </summary>
            public EVSEDataRecord Build()

                => new EVSEDataRecord(Id,
                                      DeltaType,
                                      LastUpdate,
                                      ChargingStationId,
                                      ChargingPoolId,
                                      ChargingStationName,
                                      Address,
                                      GeoCoordinate,
                                      Plugs,
                                      ChargingFacilities,
                                      ChargingModes,
                                      AuthenticationModes,
                                      MaxCapacity,
                                      PaymentOptions,
                                      ValueAddedServices,
                                      Accessibility,
                                      HotlinePhoneNumber,
                                      AdditionalInfo,
                                      GeoChargingPointEntrance,
                                      IsOpen24Hours,
                                      OpeningTime,
                                      HubOperatorId,
                                      ClearingHouseId,
                                      IsHubjectCompatible,
                                      DynamicInfoAvailable,
                                      CustomData);

            #endregion

        }

        #endregion

    }

}
