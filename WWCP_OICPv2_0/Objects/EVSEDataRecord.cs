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
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
{

    /// <summary>
    /// An OICP Electric Vehicle Supply Equipment (EVSE).
    /// This is meant to be one electrical circuit which can charge a electric vehicle.
    /// </summary>
    public class EVSEDataRecord
    {

        #region Data

        private static readonly Regex HotlinePhoneNumberRegExpr = new Regex("\\+[^0-9]");

        #endregion

        #region Properties

        /// <summary>
        /// The unique identification of the Electric Vehicle Supply Equipment (EVSE).
        /// </summary>
        public EVSE_Id              Id                          { get; }

        /// <summary>
        /// The related WWCP Electric Vehicle Supply Equipment (EVSE).
        /// </summary>
        public EVSE                 EVSE                        { get; }


        /// <summary>
        /// The delta type when this EVSE data record was just downloaded.
        /// </summary>
        public String               DeltaType                   { get; }

        /// <summary>
        /// The last update timestamp of this EVSE data record.
        /// </summary>
        public DateTime?            LastUpdate                  { get; }


        /// <summary>
        /// The identification of the charging station hosting this EVSE.
        /// </summary>
        public String               ChargingStationId           { get; }

        /// <summary>
        /// The multi-language name of the charging station hosting this EVSE.
        /// </summary>
        public I18NString           ChargingStationName         { get; }

        /// <summary>
        /// The address of this EVSE.
        /// </summary>
        public Address              Address                     { get; }

        /// <summary>
        /// The geo coordinate of this EVSE.
        /// </summary>
        public GeoCoordinate        GeoCoordinate               { get; }

        /// <summary>
        /// The types of charging plugs attached to this EVSE.
        /// </summary>
        public PlugTypes            Plugs                       { get; }

        /// <summary>
        /// The charging facilities at this EVSE.
        /// </summary>
        public ChargingFacilities   ChargingFacilities          { get; }

        /// <summary>
        /// The charging modes this EVSE supports.
        /// </summary>
        public ChargingModes        ChargingModes               { get; }

        /// <summary>
        /// The authentication modes this EVSE supports.
        /// </summary>
        public AuthenticationModes  AuthenticationModes         { get; }

        /// <summary>
        /// The maximum capacity this EVSE provides.
        /// </summary>
        public Double?              MaxCapacity                 { get; }

        /// <summary>
        /// The payment options this EVSE supports.
        /// </summary>
        public PaymentOptions       PaymentOptions              { get; }

        /// <summary>
        /// The accessibility of this EVSE.
        /// </summary>
        public AccessibilityTypes   Accessibility               { get; }

        /// <summary>
        /// The phone number of the Charging Station Operators hotline.
        /// </summary>
        public String               HotlinePhoneNumber          { get; }

        /// <summary>
        /// Additional multi-language information about this EVSE.
        /// </summary>
        public I18NString           AdditionalInfo              { get; }

        /// <summary>
        /// The geo coordinate of the entrance to this EVSE.
        /// </summary>
        public GeoCoordinate        GeoChargingPointEntrance    { get; }

        #region IsOpen24Hours

        /// <summary>
        /// Whether this EVSE is open 24/7.
        /// </summary>
        public Boolean IsOpen24Hours
        {
            get
            {

                if (OpeningTime == null)
                    return true;

                return OpeningTime.IsOpen24Hours;

            }
        }

        #endregion

        /// <summary>
        /// The opening times of this EVSE.
        /// </summary>
        public OpeningTimes         OpeningTime                 { get; }

        /// <summary>
        /// An optional hub operator of this EVSE.
        /// </summary>
        public HubOperator_Id       HubOperatorId               { get; }

        /// <summary>
        /// An optional clearing house of this EVSE.
        /// </summary>
        public RoamingProvider_Id   ClearingHouseId             { get; }

        /// <summary>
        /// Whether this EVSE is Hubject compatible.
        /// </summary>
        public Boolean              IsHubjectCompatible         { get; }

        /// <summary>
        /// Whether this EVSE provides dynamic status information.
        /// </summary>
        public Boolean              DynamicInfoAvailable        { get; }

        #endregion

        #region Constructor(s)

        #region (internal) EVSEDataRecord(EVSE, ...)

        /// <summary>
        /// Create a new EVSE data record.
        /// </summary>
        /// <param name="EVSE">A WWCP EVSE.</param>
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
        internal EVSEDataRecord(EVSE                              EVSE,
                                String                            ChargingStationId           = null,
                                I18NString                        ChargingStationName         = null,
                                Address                           Address                     = null,
                                GeoCoordinate                     GeoCoordinate               = null,
                                PlugTypes                         Plugs                       = PlugTypes.Unspecified,
                                ChargingFacilities                ChargingFacilities          = ChargingFacilities.Unspecified,
                                ChargingModes                     ChargingModes               = ChargingModes.Unspecified,
                                AuthenticationModes               AuthenticationModes         = AuthenticationModes.Unkown,
                                Int32?                            MaxCapacity                 = null,
                                PaymentOptions                    PaymentOptions              = PaymentOptions.Unspecified,
                                AccessibilityTypes                Accessibility               = AccessibilityTypes.Free_publicly_accessible,
                                String                            HotlinePhoneNumber          = null,
                                I18NString                        AdditionalInfo              = null,
                                GeoCoordinate                     GeoChargingPointEntrance    = null,
                                Boolean?                          IsOpen24Hours               = null,
                                OpeningTimes                      OpeningTime                 = null,
                                HubOperator_Id                    HubOperatorId               = null,
                                RoamingProvider_Id                ClearingHouseId             = null,
                                Boolean                           IsHubjectCompatible         = true,
                                Boolean                           DynamicInfoAvailable        = true)

            : this(EVSE.Id,
                   "",
                   DateTime.Now,
                   ChargingStationId,
                   ChargingStationName,
                   Address,
                   GeoCoordinate,
                   Plugs,
                   ChargingFacilities,
                   ChargingModes,
                   AuthenticationModes,
                   MaxCapacity,
                   PaymentOptions,
                   Accessibility,
                   HotlinePhoneNumber,
                   AdditionalInfo,
                   GeoChargingPointEntrance,
                   IsOpen24Hours,
                   OpeningTime,
                   HubOperatorId,
                   ClearingHouseId,
                   IsHubjectCompatible,
                   DynamicInfoAvailable)

        {

            this.EVSE = EVSE;

        }

        #endregion

        #region EVSEDataRecord(Id)

        /// <summary>
        /// Create a new EVSE data record.
        /// </summary>
        /// <param name="Id">The unique identification of an EVSE.</param>
        public EVSEDataRecord(EVSE_Id  Id)
        {

            #region Initial checks

            if (Id == null)
                throw new ArgumentNullException(nameof(Id),  "The given unique EVSE identification must not be null!");

            #endregion

            this.Id                   = Id;
            this.ChargingStationName  = new I18NString();
            this.AdditionalInfo       = new I18NString();

        }

        #endregion

        #region EVSEDataRecord(Id, ...)

        /// <summary>
        /// Create a new EVSE data record.
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
        public EVSEDataRecord(EVSE_Id                           Id,
                              String                            DeltaType,
                              DateTime?                         LastUpdate,
                              String                            ChargingStationId           = null,
                              I18NString                        ChargingStationName         = null,
                              Address                           Address                     = null,
                              GeoCoordinate                     GeoCoordinate               = null,
                              PlugTypes                         Plugs                       = PlugTypes.Unspecified,
                              ChargingFacilities                ChargingFacilities          = ChargingFacilities.Unspecified,
                              ChargingModes                     ChargingModes               = ChargingModes.Unspecified,
                              AuthenticationModes               AuthenticationModes         = AuthenticationModes.Unkown,
                              Double?                           MaxCapacity                 = null,
                              PaymentOptions                    PaymentOptions              = PaymentOptions.Unspecified,
                              AccessibilityTypes                Accessibility               = AccessibilityTypes.Free_publicly_accessible,
                              String                            HotlinePhoneNumber          = null,
                              I18NString                        AdditionalInfo              = null,
                              GeoCoordinate                     GeoChargingPointEntrance    = null,
                              Boolean?                          IsOpen24Hours               = null,
                              OpeningTimes                      OpeningTime                 = null,
                              HubOperator_Id                    HubOperatorId               = null,
                              RoamingProvider_Id                ClearingHouseId             = null,
                              Boolean                           IsHubjectCompatible         = true,
                              Boolean                           DynamicInfoAvailable        = true)

            : this(Id)

        {

            #region Initial checks

            if (Address == null)
                throw new ArgumentNullException(nameof(Address),              "The given address must not be null!");

            if (GeoCoordinate == null)
                throw new ArgumentNullException(nameof(GeoCoordinate),        "The given geo coordinate must not be null!");

            if (Plugs == PlugTypes.Unspecified)
                throw new ArgumentNullException(nameof(Plugs),                "The given plugs must not be empty!");

            if (AuthenticationModes == AuthenticationModes.Unkown)
                throw new ArgumentNullException(nameof(AuthenticationModes),  "The given authentication modes must not be null or empty!");

            if (HotlinePhoneNumber == null || HotlinePhoneNumber.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(HotlinePhoneNumber),   "The given hotline phone number must not be null or empty!");

            #endregion

            this.DeltaType                 = DeltaType;
            this.LastUpdate                = LastUpdate;

            this.ChargingStationId         = ChargingStationId;
            this.ChargingStationName       = ChargingStationName ?? new I18NString();
            this.Address                   = Address;
            this.GeoCoordinate             = GeoCoordinate;
            this.Plugs                     = Plugs;
            this.ChargingModes             = ChargingModes;
            this.ChargingFacilities        = ChargingFacilities;
            this.AuthenticationModes       = AuthenticationModes;
            this.MaxCapacity               = MaxCapacity;
            this.PaymentOptions            = PaymentOptions;
            this.Accessibility             = Accessibility;
            this.HotlinePhoneNumber        = HotlinePhoneNumber;
            this.AdditionalInfo            = AdditionalInfo ?? new I18NString();
            this.GeoChargingPointEntrance  = GeoChargingPointEntrance;
            this.HubOperatorId             = HubOperatorId;
            this.ClearingHouseId           = ClearingHouseId;
            this.IsHubjectCompatible       = IsHubjectCompatible;
            this.DynamicInfoAvailable      = DynamicInfoAvailable;


            if (IsOpen24Hours != null && IsOpen24Hours.HasValue && IsOpen24Hours.Value)
                this.OpeningTime  = OpeningTimes.Open24Hours;

            if (OpeningTime != null)
                this.OpeningTime  = OpeningTime;

            if (OpeningTime == null && (IsOpen24Hours == null || !IsOpen24Hours.HasValue))
                this.OpeningTime  = OpeningTimes.Open24Hours;

        }

        #endregion

        #endregion


        #region Parse(EVSEDataRecordXML, OnException = null)

        public static EVSEDataRecord Parse(XElement             EVSEDataRecordXML,
                                           OnExceptionDelegate  OnException = null)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv      = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:EVSEData     = "http://www.hubject.com/b2b/services/evsedata/EVSEData/v2.1"
            //                   xmlns:CommonTypes  = "http://www.hubject.com/b2b/services/commontypes/EVSEData/v2.0">

            // <EVSEData:eRoamingEvseDataRecord deltaType="?" lastUpdate="?">
            //
            //    <EVSEData:EvseId>?</EVSEData:EvseId>
            //
            //    <!--Optional:-->
            //    <EVSEData:ChargingStationId>?</EVSEData:ChargingStationId>
            //    <!--Optional:-->
            //    <EVSEData:ChargingStationName>?</EVSEData:ChargingStationName>
            //    <!--Optional:-->
            //    <EVSEData:EnChargingStationName>?</EVSEData:EnChargingStationName>
            //
            //    <EVSEData:Address>
            //       <CommonTypes:Country>?</CommonTypes:Country>
            //       <CommonTypes:City>?</CommonTypes:City>
            //       <CommonTypes:Street>?</CommonTypes:Street>
            //       <!--Optional:-->
            //       <CommonTypes:PostalCode>?</CommonTypes:PostalCode>
            //       <!--Optional:-->
            //       <CommonTypes:HouseNum>?</CommonTypes:HouseNum>
            //       <!--Optional:-->
            //       <CommonTypes:Floor>?</CommonTypes:Floor>
            //       <!--Optional:-->
            //       <CommonTypes:Region>?</CommonTypes:Region>
            //       <!--Optional:-->
            //       <CommonTypes:TimeZone>?</CommonTypes:TimeZone>
            //    </EVSEData:Address>
            //
            //    <EVSEData:GeoCoordinates>
            //       <!--You have a CHOICE of the next 3 items at this level-->
            //       <CommonTypes:Google>
            //          <CommonTypes:Coordinates>?</CommonTypes:Coordinates>
            //       </CommonTypes:Google>
            //       <CommonTypes:DecimalDegree>
            //          <CommonTypes:Longitude>?</CommonTypes:Longitude>
            //          <CommonTypes:Latitude>?</CommonTypes:Latitude>
            //       </CommonTypes:DecimalDegree>
            //       <CommonTypes:DegreeMinuteSeconds>
            //          <CommonTypes:Longitude>?</CommonTypes:Longitude>
            //          <CommonTypes:Latitude>?</CommonTypes:Latitude>
            //       </CommonTypes:DegreeMinuteSeconds>
            //    </EVSEData:GeoCoordinates>
            //
            //    <EVSEData:Plugs>
            //       <!--1 or more repetitions:-->
            //       <EVSEData:Plug>?</EVSEData:Plug>
            //    </EVSEData:Plugs>
            //
            //    <!--Optional:-->
            //    <EVSEData:ChargingFacilities>
            //       <!--1 or more repetitions:-->
            //       <EVSEData:ChargingFacility>?</EVSEData:ChargingFacility>
            //    </EVSEData:ChargingFacilities>
            //
            //    <!--Optional:-->
            //    <EVSEData:ChargingModes>
            //       <!--1 or more repetitions:-->
            //       <EVSEData:ChargingMode>?</EVSEData:ChargingMode>
            //    </EVSEData:ChargingModes>
            //
            //    <EVSEData:AuthenticationModes>
            //       <!--1 or more repetitions:-->
            //       <EVSEData:AuthenticationMode>?</EVSEData:AuthenticationMode>
            //    </EVSEData:AuthenticationModes>
            //
            //    <!--Optional:-->
            //    <EVSEData:MaxCapacity>?</EVSEData:MaxCapacity>
            //
            //    <!--Optional:-->
            //    <EVSEData:PaymentOptions>
            //       <!--1 or more repetitions:-->
            //       <EVSEData:PaymentOption>?</EVSEData:PaymentOption>
            //    </EVSEData:PaymentOptions>
            //
            //    <EVSEData:ValueAddedServices>
            //       <!--1 or more repetitions:-->
            //       <EVSEData:ValueAddedService>?</EVSEData:ValueAddedService>
            //    </EVSEData:ValueAddedServices>
            //
            //    <EVSEData:Accessibility>?</EVSEData:Accessibility>
            //    <EVSEData:HotlinePhoneNum>?</EVSEData:HotlinePhoneNum>
            //
            //    <!--Optional:-->
            //    <EVSEData:AdditionalInfo>?</EVSEData:AdditionalInfo>
            //
            //    <!--Optional:-->
            //    <EVSEData:EnAdditionalInfo>?</EVSEData:EnAdditionalInfo>
            //
            //    <!--Optional:-->
            //    <EVSEData:GeoChargingPointEntrance>
            //       <!--You have a CHOICE of the next 3 items at this level-->
            //       <CommonTypes:Google>
            //          <CommonTypes:Coordinates>?</CommonTypes:Coordinates>
            //       </CommonTypes:Google>
            //       <CommonTypes:DecimalDegree>
            //          <CommonTypes:Longitude>?</CommonTypes:Longitude>
            //          <CommonTypes:Latitude>?</CommonTypes:Latitude>
            //       </CommonTypes:DecimalDegree>
            //       <CommonTypes:DegreeMinuteSeconds>
            //          <CommonTypes:Longitude>?</CommonTypes:Longitude>
            //          <CommonTypes:Latitude>?</CommonTypes:Latitude>
            //       </CommonTypes:DegreeMinuteSeconds>
            //    </EVSEData:GeoChargingPointEntrance>
            //
            //    <EVSEData:IsOpen24Hours>?</EVSEData:IsOpen24Hours>
            //    <!--Optional:-->
            //    <EVSEData:OpeningTime>?</EVSEData:OpeningTime>
            //
            //    <!--Optional:-->
            //    <EVSEData:HubOperatorID>?</EVSEData:HubOperatorID>
            //
            //    <!--Optional:-->
            //    <EVSEData:ClearinghouseID>?</EVSEData:ClearinghouseID>
            //    <EVSEData:IsHubjectCompatible>?</EVSEData:IsHubjectCompatible>
            //    <EVSEData:DynamicInfoAvailable>?</EVSEData:DynamicInfoAvailable>
            //
            // </EVSEData:eRoamingEvseDataRecord>

            #endregion


            #region XML Attribute: LastUpdate

            DateTime _LastUpdate;
            DateTime.TryParse(EVSEDataRecordXML.AttributeValueOrDefault(XName.Get("lastUpdate"), ""), out _LastUpdate);

            #endregion

            #region ChargingStationName

            var _ChargingStationName = new I18NString();

            EVSEDataRecordXML.IfElementIsDefined(OICPNS.EVSEData + "ChargingStationName",
                                                 v => _ChargingStationName.Add(Languages.de, v));

            EVSEDataRecordXML.IfElementIsDefined(OICPNS.EVSEData + "EnChargingStationName",
                                                 v => _ChargingStationName.Add(Languages.en, v));

            #endregion

            #region Address

            var AddressXML  = EVSEDataRecordXML.ElementOrFail(OICPNS.EVSEData + "Address", "Missing 'Address'-XML tag!");

            var _CountryTXT = AddressXML.ElementValueOrFail(OICPNS.CommonTypes + "Country", "Missing 'Country'-XML tag!").Trim();

            Country _Country;
            if (!Country.TryParse(_CountryTXT, out _Country))
            {

                if (_CountryTXT.ToUpper() == "UNKNOWN")
                    _Country = Country.unknown;

                else
                    throw new Exception("'" + _CountryTXT + "' is an unknown country name!");

            }

            // Currently not used OICP address information!
            //var _Region       = AddressXML.       ElementValueOrDefault(OICPNS.OICPv2_0CommonTypes + "Region",     "").Trim();
            //var _Timezone     = AddressXML.       ElementValueOrDefault(OICPNS.OICPv2_0CommonTypes + "Timezone",   "").Trim();

            #endregion

            #region MaxCapacity in kWh

            var _MaxCapacity_kWh = EVSEDataRecordXML.
                                       ElementValueOrDefault(OICPNS.EVSEData + "MaxCapacity", String.Empty).
                                       Trim();

            Double _MaxCapacity = 0.0;
            if (_MaxCapacity_kWh.IsNotNullOrEmpty())
                Double.TryParse(_MaxCapacity_kWh, out _MaxCapacity);

            #endregion

            #region AdditionalInfo

            var _AdditionalInfo = new I18NString();

            EVSEDataRecordXML.IfElementIsDefined(OICPNS.EVSEData + "AdditionalInfo",
                                                 v => _AdditionalInfo.Add(Languages.de, v));

            // EnAdditionalInfo not parsed as OICP v2.0 multi-language string!
            EVSEDataRecordXML.IfElementIsDefined(OICPNS.EVSEData + "EnAdditionalInfo",
                                                 EnAdditionalInfo => {

                                                     // The section must end with the separator string "|||"
                                                     // Example: "DEU:Inhalt|||GBR:Content|||FRA:Objet|||"
                                                     if (EnAdditionalInfo.Contains("|||"))
                                                     {

                                                         foreach (var Token1 in EnAdditionalInfo.Split(new String[] { "|||" }, StringSplitOptions.RemoveEmptyEntries))
                                                         {

                                                             var I18NTokens = Token1.Split(':');

                                                             try
                                                             {
                                                                 if (I18NTokens.Length == 2)
                                                                 {
                                                                     _AdditionalInfo.Add((Languages)Enum.Parse(typeof(Languages),
                                                                                                               Country.ParseAlpha3Code(I18NTokens[0]).Alpha2Code.ToLower()),
                                                                                        I18NTokens[1]);
                                                                 }
                                                             }
                                                             catch (Exception e)
                                                             {
                                                                 DebugX.Log("Could not parse 'EnAdditionalInfo': " + I18NTokens + Environment.NewLine + e.Message);
                                                             }

                                                         }

                                                     }

                                                     else
                                                         _AdditionalInfo.Add(Languages.en, EnAdditionalInfo);

                                                 });

            #endregion

            #region IsOpen24Hours / OpeningTime

            OpeningTimes _OpeningTime = null;

            if (EVSEDataRecordXML.ElementValueOrFail(OICPNS.EVSEData + "IsOpen24Hours", "Missing 'IsOpen24Hours'-XML tag!") == "true")
            {
                _OpeningTime = OpeningTimes.Open24Hours;
            }

            else
            {

                var OpeningTimeXML = EVSEDataRecordXML.Element(OICPNS.EVSEData + "OpeningTime");
                if (OpeningTimeXML != null)
                {
                    _OpeningTime = OpeningTimes.FromFreeText(OpeningTimeXML.Value.Trim());
                }

            }

            #endregion


            return new EVSEDataRecord(

                EVSE_Id.Parse(EVSEDataRecordXML.ElementValueOrFail(OICPNS.EVSEData + "EvseId", "Missing 'EvseId'-XML tag!")),

                EVSEDataRecordXML.AttributeValueOrDefault(XName.Get("deltaType"), ""),

                _LastUpdate,

           //     null,

                EVSEDataRecordXML.ElementValueOrDefault(OICPNS.EVSEData + "ChargingStationId", ""),
                _ChargingStationName,

                new Address(AddressXML.ElementValueOrFail(OICPNS.CommonTypes + "Street", "Missing 'Street'-XML tag!").Trim(),
                                                 AddressXML.ElementValueOrDefault(OICPNS.CommonTypes + "HouseNum", "").Trim(),
                                                 AddressXML.ElementValueOrDefault(OICPNS.CommonTypes + "Floor", "").Trim(),
                                                 AddressXML.ElementValueOrDefault(OICPNS.CommonTypes + "PostalCode", "").Trim(),
                                                 "",
                                                 I18NString.Create(Languages.unknown, AddressXML.ElementValueOrFail(OICPNS.CommonTypes + "City", "Missing 'City'-XML tag!").Trim()),
                                                 _Country),

                XMLMethods.ParseGeoCoordinatesXML(EVSEDataRecordXML.ElementOrFail(OICPNS.EVSEData + "GeoCoordinates", "Missing 'GeoCoordinates'-XML tag!")),

                EVSEDataRecordXML.MapValuesOrFail   (OICPNS.EVSEData + "Plugs", "Missing 'Plugs'-XML tag!",
                                                     OICPNS.EVSEData + "Plug",
                                                     OICPMapper.AsPlugType,
                                                     PlugTypes.Unspecified).
                                                     Reduce(),

                EVSEDataRecordXML.MapValuesOrDefault(OICPNS.EVSEData + "ChargingFacilities",
                                                     OICPNS.EVSEData + "ChargingFacility",
                                                     OICPMapper.AsChargingFacility,
                                                     ChargingFacilities.Unspecified).
                                                     Reduce(),

                EVSEDataRecordXML.MapValuesOrDefault(OICPNS.EVSEData + "ChargingModes",
                                                     OICPNS.EVSEData + "ChargingMode",
                                                     OICPMapper.AsChargingMode,
                                                     ChargingModes.Unspecified).
                                                     Reduce(),

                EVSEDataRecordXML.MapValuesOrFail   (OICPNS.EVSEData + "AuthenticationModes", "Missing 'AuthenticationModes'-XML tag!",
                                                     OICPNS.EVSEData + "AuthenticationMode",
                                                     OICPMapper.AsAuthenticationMode).
                                                     Reduce(),

                _MaxCapacity,

                EVSEDataRecordXML.MapValuesOrDefault(OICPNS.EVSEData + "PaymentOptions",
                                                     OICPNS.EVSEData + "PaymentOption",
                                                     OICPMapper.AsPaymetOption,
                                                     PaymentOptions.Unspecified).
                                                     Reduce(),

                OICPMapper.AsAccessibilityType(EVSEDataRecordXML.
                                               ElementValueOrFail(OICPNS.EVSEData + "Accessibility", "Missing 'Accessibility'-XML tag!").
                                               Trim()),

                EVSEDataRecordXML.ElementValueOrFail(OICPNS.EVSEData + "HotlinePhoneNum", "Missing 'HotlinePhoneNum '-XML tag!").
                                  Trim(),

                _AdditionalInfo,

                EVSEDataRecordXML.MapElement(OICPNS.CommonTypes + "GeoChargingPointEntrance",
                                             XMLMethods.ParseGeoCoordinatesXML),

                _OpeningTime.IsOpen24Hours,
                _OpeningTime,

                EVSEDataRecordXML.MapValueOrNull(OICPNS.EVSEData + "HubOperatorID",
                                                 HubOperator_Id.Parse),

                EVSEDataRecordXML.MapValueOrNull(OICPNS.EVSEData + "ClearinghouseID",
                                                 RoamingProvider_Id.Parse),

                EVSEDataRecordXML.ElementValueOrFail(OICPNS.EVSEData + "IsHubjectCompatible", "Missing 'IsHubjectCompatible '-XML tag!").
                                  Trim() == "true",

                EVSEDataRecordXML.ElementValueOrFail(OICPNS.EVSEData + "DynamicInfoAvailable", "Missing 'DynamicInfoAvailable '-XML tag!").
                                  Trim() != "false"

            );


        }

        #endregion

        #region ToXML()

        public XElement ToXML()
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:EVSEData      = "http://www.hubject.com/b2b/services/evsedata/EVSEData.0"
            //                   xmlns:CommonTypes     = "http://www.hubject.com/b2b/services/commontypes/EVSEData.0">

            // <EVSEData:EvseDataRecord>
            //
            //    <EVSEData:EvseId>?</EVSEData:EvseId>
            //
            //    <!--Optional:-->
            //    <EVSEData:ChargingStationId>?</EVSEData:ChargingStationId>
            //    <!--Optional:-->
            //    <EVSEData:ChargingStationName>?</EVSEData:ChargingStationName>
            //    <!--Optional:-->
            //    <EVSEData:EnChargingStationName>?</EVSEData:EnChargingStationName>
            //
            //    <EVSEData:Address>
            //       <CommonTypes:Country>?</CommonTypes:Country>
            //       <CommonTypes:City>?</CommonTypes:City>
            //       <CommonTypes:Street>?</CommonTypes:Street>
            //       <!--Optional:-->
            //       <CommonTypes:PostalCode>?</CommonTypes:PostalCode>
            //       <!--Optional:-->
            //       <CommonTypes:HouseNum>?</CommonTypes:HouseNum>
            //       <!--Optional:-->
            //       <CommonTypes:Floor>?</CommonTypes:Floor>
            //       <!--Optional:-->
            //       <CommonTypes:Region>?</CommonTypes:Region>
            //       <!--Optional:-->
            //       <CommonTypes:TimeZone>?</CommonTypes:TimeZone>
            //    </EVSEData:Address>
            //
            //    <EVSEData:GeoCoordinates>
            //       <!--You have a CHOICE of the next 3 items at this level-->
            //       <CommonTypes:Google>
            //          <CommonTypes:Coordinates>?</CommonTypes:Coordinates>
            //       </CommonTypes:Google>
            //       <CommonTypes:DecimalDegree>
            //          <CommonTypes:Longitude>?</CommonTypes:Longitude>
            //          <CommonTypes:Latitude>?</CommonTypes:Latitude>
            //       </CommonTypes:DecimalDegree>
            //       <CommonTypes:DegreeMinuteSeconds>
            //          <CommonTypes:Longitude>?</CommonTypes:Longitude>
            //          <CommonTypes:Latitude>?</CommonTypes:Latitude>
            //       </CommonTypes:DegreeMinuteSeconds>
            //    </EVSEData:GeoCoordinates>
            //
            //    <EVSEData:Plugs>
            //       <!--1 or more repetitions:-->
            //       <EVSEData:Plug>?</EVSEData:Plug>
            //    </EVSEData:Plugs>
            //
            //    <!--Optional:-->
            //    <EVSEData:ChargingFacilities>
            //       <!--1 or more repetitions:-->
            //       <EVSEData:ChargingFacility>?</EVSEData:ChargingFacility>
            //    </EVSEData:ChargingFacilities>
            //
            //    <!--Optional:-->
            //    <EVSEData:ChargingModes>
            //       <!--1 or more repetitions:-->
            //       <EVSEData:ChargingMode>?</EVSEData:ChargingMode>
            //    </EVSEData:ChargingModes>
            //
            //    <EVSEData:AuthenticationModes>
            //       <!--1 or more repetitions:-->
            //       <EVSEData:AuthenticationMode>?</EVSEData:AuthenticationMode>
            //    </EVSEData:AuthenticationModes>
            //
            //    <!--Optional:-->
            //    <EVSEData:MaxCapacity>?</EVSEData:MaxCapacity>
            //
            //    <!--Optional:-->
            //    <EVSEData:PaymentOptions>
            //       <!--1 or more repetitions:-->
            //       <EVSEData:PaymentOption>?</EVSEData:PaymentOption>
            //    </EVSEData:PaymentOptions>
            //
            //    <EVSEData:Accessibility>?</EVSEData:Accessibility>
            //    <EVSEData:HotlinePhoneNum>?</EVSEData:HotlinePhoneNum>

            //    <!--Optional:-->
            //    <EVSEData:AdditionalInfo>?</EVSEData:AdditionalInfo>
            //
            //    <!--Optional:-->
            //    <EVSEData:EnAdditionalInfo>?</EVSEData:EnAdditionalInfo>
            //
            //    <!--Optional:-->
            //    <EVSEData:GeoChargingPointEntrance>
            //       <!--You have a CHOICE of the next 3 items at this level-->
            //       <CommonTypes:Google>
            //          <CommonTypes:Coordinates>?</CommonTypes:Coordinates>
            //       </CommonTypes:Google>
            //       <CommonTypes:DecimalDegree>
            //          <CommonTypes:Longitude>?</CommonTypes:Longitude>
            //          <CommonTypes:Latitude>?</CommonTypes:Latitude>
            //       </CommonTypes:DecimalDegree>
            //       <CommonTypes:DegreeMinuteSeconds>
            //          <CommonTypes:Longitude>?</CommonTypes:Longitude>
            //          <CommonTypes:Latitude>?</CommonTypes:Latitude>
            //       </CommonTypes:DegreeMinuteSeconds>
            //    </EVSEData:GeoChargingPointEntrance>
            //
            //    <EVSEData:IsOpen24Hours>?</EVSEData:IsOpen24Hours>
            //    <!--Optional:-->
            //    <EVSEData:OpeningTime>?</EVSEData:OpeningTime>
            //
            //    <!--Optional:-->
            //    <EVSEData:HubOperatorID>?</EVSEData:HubOperatorID>
            //
            //    <!--Optional:-->
            //    <EVSEData:ClearinghouseID>?</EVSEData:ClearinghouseID>
            //
            //    <EVSEData:IsHubjectCompatible>?</EVSEData:IsHubjectCompatible>
            //    <EVSEData:DynamicInfoAvailable>?</EVSEData:DynamicInfoAvailable>
            //
            // </EVSEData:EvseDataRecord>

            #endregion

            return new XElement(OICPNS.EVSEData + "EvseDataRecord",

                new XElement(OICPNS.EVSEData + "EvseId",                Id.OriginId),
                new XElement(OICPNS.EVSEData + "ChargingStationId",     ChargingStationId),
                new XElement(OICPNS.EVSEData + "ChargingStationName",   ChargingStationName[Languages.de].SubstringMax(50)),
                new XElement(OICPNS.EVSEData + "EnChargingStationName", ChargingStationName[Languages.en].SubstringMax(50)),

                new XElement(OICPNS.EVSEData + "Address",
                    new XElement(OICPNS.CommonTypes + "Country",        Address.Country.Alpha3Code),
                    new XElement(OICPNS.CommonTypes + "City",           Address.City.FirstText),
                    new XElement(OICPNS.CommonTypes + "Street",         Address.Street), // OICPEVSEData.0 requires at least 5 characters!

                    Address.PostalCode. IsNotNullOrEmpty()
                        ? new XElement(OICPNS.CommonTypes + "PostalCode", Address.PostalCode)
                        : null,

                    Address.HouseNumber.IsNotNullOrEmpty()
                        ? new XElement(OICPNS.CommonTypes + "HouseNum",   Address.HouseNumber)
                        : null,

                    Address.FloorLevel. IsNotNullOrEmpty()
                        ? new XElement(OICPNS.CommonTypes + "Floor",      Address.FloorLevel)
                        : null

                    // <!--Optional:-->
                    // <v11:Region>?</v11:Region>

                    // <!--Optional:-->
                    // <v11:TimeZone>?</v11:TimeZone>

                ),

                new XElement(OICPNS.EVSEData + "GeoCoordinates",
                    new XElement(OICPNS.CommonTypes + "DecimalDegree",  // Force 0.00... (dot) format!
                        new XElement(OICPNS.CommonTypes + "Longitude",  GeoCoordinate.Longitude.ToString("{0:0.######}").Replace(",", ".")),// CultureInfo.InvariantCulture.NumberFormat)),
                        new XElement(OICPNS.CommonTypes + "Latitude",   GeoCoordinate.Latitude. ToString("{0:0.######}").Replace(",", ".")) // CultureInfo.InvariantCulture.NumberFormat))
                    )
                ),

                new XElement(OICPNS.EVSEData + "Plugs",
                    Plugs.ToEnumeration().SafeSelect(Plug               => new XElement(OICPNS.EVSEData + "Plug",               OICPMapper.AsString(Plug)))
                ),

                ChargingFacilities.ToEnumeration().NotNullAny()
                    ? new XElement(OICPNS.EVSEData + "ChargingFacilities",
                          ChargingFacilities.ToEnumeration().SafeSelect(ChargingFacility   => new XElement(OICPNS.EVSEData + "ChargingFacility",   OICPMapper.AsString(ChargingFacility))))
                    : null,

                ChargingModes.ToEnumeration().NotNullAny()
                    ? new XElement(OICPNS.EVSEData + "ChargingModes",
                          ChargingModes.ToEnumeration().Select(ChargingMode     => new XElement(OICPNS.EVSEData + "ChargingMode",       OICPMapper.AsString(ChargingMode))))
                    : null,

                new XElement(OICPNS.EVSEData + "AuthenticationModes",
                    AuthenticationModes.     ToEnumeration().
                                             Select(AuthenticationMode => OICPMapper.AsString(AuthenticationMode)).
                                             Where (AuthenticationMode => AuthenticationMode != "Unknown").
                                             Select(AuthenticationMode => new XElement(OICPNS.EVSEData + "AuthenticationMode", AuthenticationMode))
                ),

                MaxCapacity.HasValue
                    ? new XElement(OICPNS.EVSEData + "MaxCapacity", MaxCapacity)
                    : null,

                PaymentOptions.ToEnumeration().NotNullAny()
                    ? new XElement(OICPNS.EVSEData + "PaymentOptions",
                          PaymentOptions.ToEnumeration().SafeSelect(PaymentOption      => new XElement(OICPNS.EVSEData + "PaymentOption",      OICPMapper.AsString(PaymentOption)))
                      )
                    : null,

                new XElement(OICPNS.EVSEData + "Accessibility",     Accessibility.ToString().Replace("_", " ")),
                new XElement(OICPNS.EVSEData + "HotlinePhoneNum",   HotlinePhoneNumberRegExpr.Replace(HotlinePhoneNumber, "")),  // RegEx: \+[0-9]{5,15}

                AdditionalInfo.has(Languages.de)
                    ? new XElement(OICPNS.EVSEData + "AdditionalInfo", AdditionalInfo[Languages.de])
                    : null,

                AdditionalInfo.has(Languages.en)
                    ? new XElement(OICPNS.EVSEData + "EnAdditionalInfo", AdditionalInfo[Languages.en])
                    : null,

                GeoChargingPointEntrance != null
                    ? new XElement(OICPNS.EVSEData + "GeoChargingPointEntrance",
                        new XElement(OICPNS.CommonTypes + "DecimalDegree",  // Force 0.00... (dot) format!
                            new XElement(OICPNS.CommonTypes + "Longitude", GeoChargingPointEntrance.Longitude.ToString("{0:0.######}").Replace(",", ".")),// CultureInfo.InvariantCulture.NumberFormat)),
                            new XElement(OICPNS.CommonTypes + "Latitude",  GeoChargingPointEntrance.Latitude. ToString("{0:0.######}").Replace(",", ".")) // CultureInfo.InvariantCulture.NumberFormat))
                        )
                    )
                    : null,

                new XElement(OICPNS.EVSEData + "IsOpen24Hours",         OpeningTime.IsOpen24Hours ? "true" : "false"),

                OpeningTime.IsOpen24Hours
                    ? null
                    : new XElement(OICPNS.EVSEData + "OpeningTime",     OpeningTime.FreeText),

                HubOperatorId != null
                    ? new XElement(OICPNS.EVSEData + "HubOperatorID",   HubOperatorId.ToString())
                    : null,

                ClearingHouseId != null
                    ? new XElement(OICPNS.EVSEData + "ClearinghouseID", ClearingHouseId.ToString())
                    : null,

                new XElement(OICPNS.EVSEData + "IsHubjectCompatible",   IsHubjectCompatible  ? "true" : "false"),
                new XElement(OICPNS.EVSEData + "DynamicInfoAvailable",  DynamicInfoAvailable ? "true" : "false")

            );

        }

        #endregion


        public Builder ToBuilder(EVSE_Id NewEVSEId = null)
        {

            return new Builder(NewEVSEId ?? this.Id)
            {

                DeltaType                 = DeltaType,
                LastUpdate                = LastUpdate,

                ChargingStationId         = ChargingStationId,
                ChargingStationName       = ChargingStationName,
                Address                   = Address,
                GeoCoordinate             = GeoCoordinate,
                Plugs                     = Plugs,
                ChargingModes             = ChargingModes,
                ChargingFacilities        = ChargingFacilities,
                AuthenticationModes       = AuthenticationModes,
                MaxCapacity               = MaxCapacity,
                PaymentOptions            = PaymentOptions,
                Accessibility             = Accessibility,
                HotlinePhoneNumber        = HotlinePhoneNumber,
                AdditionalInfo            = AdditionalInfo,
                GeoChargingPointEntrance  = GeoChargingPointEntrance,
                HubOperatorId             = HubOperatorId,
                ClearingHouseId           = ClearingHouseId,
                IsHubjectCompatible       = IsHubjectCompatible,
                DynamicInfoAvailable      = DynamicInfoAvailable,

                OpeningTime               = OpeningTime

            };

        }


        /// <summary>
        /// An OICP Electric Vehicle Supply Equipment (EVSE).
        /// This is meant to be one electrical circuit which can charge a electric vehicle.
        /// </summary>
        public class Builder
        {

            #region Data

            private static readonly Regex HotlinePhoneNumberRegExpr = new Regex("\\+[^0-9]");

            #endregion

            #region Properties

            /// <summary>
            /// The related the Electric Vehicle Supply Equipment (EVSE).
            /// </summary>
            public EVSE EVSE { get; set; }

            /// <summary>
            /// The unique identification of the Electric Vehicle Supply Equipment (EVSE).
            /// </summary>
            public EVSE_Id EVSEId { get; }

            /// <summary>
            /// An Charging Station Operator.
            /// </summary>
            public ChargingStationOperator EVSEOperator { get; set; }


            public String DeltaType { get; set; }
            public DateTime? LastUpdate { get; set; }

            public String ChargingStationId { get; set; }
            public I18NString ChargingStationName { get; set; }
            public Address Address { get; set; }
            public GeoCoordinate GeoCoordinate { get; set; }

            /// <summary>
            /// The types of the charging plugs.
            /// </summary>
            public PlugTypes Plugs { get; set; }

            public ChargingFacilities ChargingFacilities { get; set; }
            public ChargingModes ChargingModes { get; set; }
            public AuthenticationModes AuthenticationModes { get; set; }
            public Double? MaxCapacity { get; set; }
            public PaymentOptions PaymentOptions { get; set; }
            public AccessibilityTypes Accessibility { get; set; }
            public String HotlinePhoneNumber { get; set; }
            public I18NString AdditionalInfo { get; set; }
            public GeoCoordinate GeoChargingPointEntrance { get; set; }

            #region IsOpen24Hours

            public Boolean IsOpen24Hours
            {
                get
                {

                    if (OpeningTime == null)
                        return true;

                    return OpeningTime.IsOpen24Hours;

                }
            }

            #endregion


            public OpeningTimes OpeningTime { get; set; }

            public HubOperator_Id HubOperatorId { get; set; }

            public RoamingProvider_Id ClearingHouseId { get; set; }
            public Boolean IsHubjectCompatible { get; set; }
            public Boolean DynamicInfoAvailable { get; set; }

            #endregion

            #region Constructor(s)

            #region (internal) Builder(EVSE, ...)

            /// <summary>
            /// Create a new EVSE data record.
            /// </summary>
            /// <param name="EVSE">An EVSE identification.</param>
            internal Builder(EVSE                              EVSE,
                                    String                            ChargingStationId           = null,
                                    I18NString                        ChargingStationName         = null,
                                    Address                           Address                     = null,
                                    GeoCoordinate                     GeoCoordinate               = null,
                                    PlugTypes                         Plugs                       = PlugTypes.Unspecified,
                                    ChargingFacilities                ChargingFacilities          = ChargingFacilities.Unspecified,
                                    ChargingModes                     ChargingModes               = ChargingModes.Unspecified,
                                    AuthenticationModes               AuthenticationModes         = AuthenticationModes.Unkown,
                                    Int32?                            MaxCapacity                 = null,
                                    PaymentOptions                    PaymentOptions              = PaymentOptions.Unspecified,
                                    AccessibilityTypes                Accessibility               = AccessibilityTypes.Free_publicly_accessible,
                                    String                            HotlinePhoneNumber          = null,
                                    I18NString                        AdditionalInfo              = null,
                                    GeoCoordinate                     GeoChargingPointEntrance    = null,
                                    Boolean?                          IsOpen24Hours               = null,
                                    OpeningTimes                      OpeningTime                 = null,
                                    HubOperator_Id                    HubOperatorId               = null,
                                    RoamingProvider_Id                ClearingHouseId             = null,
                                    Boolean                           IsHubjectCompatible         = true,
                                    Boolean                           DynamicInfoAvailable        = true)

                : this(EVSE.Id,
                       "",
                       DateTime.Now,
                       EVSE.Operator,
                       ChargingStationId,
                       ChargingStationName,
                       Address,
                       GeoCoordinate,
                       Plugs,
                       ChargingFacilities,
                       ChargingModes,
                       AuthenticationModes,
                       MaxCapacity,
                       PaymentOptions,
                       Accessibility,
                       HotlinePhoneNumber,
                       AdditionalInfo,
                       GeoChargingPointEntrance,
                       IsOpen24Hours,
                       OpeningTime,
                       HubOperatorId,
                       ClearingHouseId,
                       IsHubjectCompatible,
                       DynamicInfoAvailable)

            {

                this.EVSE = EVSE;

            }

            #endregion

            #region Builder(EVSEId, EVSEOperator = null)

            /// <summary>
            /// Create a new EVSE data record.
            /// </summary>
            /// <param name="EVSEId">A unique EVSE identification.</param>
            /// <param name="EVSEOperator">An Charging Station Operator.</param>
            public Builder(EVSE_Id       EVSEId,
                           ChargingStationOperator  EVSEOperator = null)
            {

                #region Initial checks

                if (EVSEId == null)
                    throw new ArgumentNullException(nameof(EVSEId),  "The given unique EVSE identification must not be null!");

                #endregion

                this.EVSEId               = EVSEId;
                this.EVSEOperator         = EVSEOperator;
                this.ChargingStationName  = new I18NString();
                this.AdditionalInfo       = new I18NString();

            }

            #endregion

            #region Builder(EVSEId, EVSEOperator, ...)

            /// <summary>
            /// Create a new EVSE data record.
            /// </summary>
            /// <param name="EVSEId">A unique EVSE identification.</param>
            /// <param name="EVSEOperator">An Charging Station Operator.</param>
            public Builder(EVSE_Id                           EVSEId,
                                  String                            DeltaType,
                                  DateTime?                         LastUpdate,
                                  ChargingStationOperator                      EVSEOperator,
                                  String                            ChargingStationId           = null,
                                  I18NString                        ChargingStationName         = null,
                                  Address                           Address                     = null,
                                  GeoCoordinate                     GeoCoordinate               = null,
                                  PlugTypes                         Plugs                       = PlugTypes.Unspecified,
                                  ChargingFacilities                ChargingFacilities          = ChargingFacilities.Unspecified,
                                  ChargingModes                     ChargingModes               = ChargingModes.Unspecified,
                                  AuthenticationModes               AuthenticationModes         = AuthenticationModes.Unkown,
                                  Double?                           MaxCapacity                 = null,
                                  PaymentOptions                    PaymentOptions              = PaymentOptions.Unspecified,
                                  AccessibilityTypes                Accessibility               = AccessibilityTypes.Free_publicly_accessible,
                                  String                            HotlinePhoneNumber          = null,
                                  I18NString                        AdditionalInfo              = null,
                                  GeoCoordinate                     GeoChargingPointEntrance    = null,
                                  Boolean?                          IsOpen24Hours               = null,
                                  OpeningTimes                      OpeningTime                 = null,
                                  HubOperator_Id                    HubOperatorId               = null,
                                  RoamingProvider_Id                ClearingHouseId             = null,
                                  Boolean                           IsHubjectCompatible         = true,
                                  Boolean                           DynamicInfoAvailable        = true)

                : this(EVSEId, EVSEOperator)

            {

                #region Initial checks

                if (Address == null)
                    throw new ArgumentNullException(nameof(Address),              "The given address must not be null!");

                if (GeoCoordinate == null)
                    throw new ArgumentNullException(nameof(GeoCoordinate),        "The given geo coordinate must not be null!");

                if (Plugs == PlugTypes.Unspecified)
                    throw new ArgumentNullException(nameof(Plugs),                "The given plugs must not be empty!");

                if (AuthenticationModes == AuthenticationModes.Unkown)
                    throw new ArgumentNullException(nameof(AuthenticationModes),  "The given authentication modes must not be null or empty!");

                if (HotlinePhoneNumber == null || HotlinePhoneNumber.IsNullOrEmpty())
                    throw new ArgumentNullException(nameof(HotlinePhoneNumber),   "The given hotline phone number must not be null or empty!");

                #endregion

                this.DeltaType                 = DeltaType;
                this.LastUpdate                = LastUpdate;

                this.ChargingStationId         = ChargingStationId;
                this.ChargingStationName       = ChargingStationName ?? new I18NString();
                this.Address                   = Address;
                this.GeoCoordinate             = GeoCoordinate;
                this.Plugs                     = Plugs;
                this.ChargingModes             = ChargingModes;
                this.ChargingFacilities        = ChargingFacilities;
                this.AuthenticationModes       = AuthenticationModes;
                this.MaxCapacity               = MaxCapacity;
                this.PaymentOptions            = PaymentOptions;
                this.Accessibility             = Accessibility;
                this.HotlinePhoneNumber        = HotlinePhoneNumber;
                this.AdditionalInfo            = AdditionalInfo ?? new I18NString();
                this.GeoChargingPointEntrance  = GeoChargingPointEntrance;
                this.HubOperatorId             = HubOperatorId;
                this.ClearingHouseId           = ClearingHouseId;
                this.IsHubjectCompatible       = IsHubjectCompatible;
                this.DynamicInfoAvailable      = DynamicInfoAvailable;


                if (IsOpen24Hours != null && IsOpen24Hours.HasValue && IsOpen24Hours.Value)
                    this.OpeningTime  = OpeningTimes.Open24Hours;

                if (OpeningTime != null)
                    this.OpeningTime  = OpeningTime;

                if (OpeningTime == null && (IsOpen24Hours == null || !IsOpen24Hours.HasValue))
                    this.OpeningTime  = OpeningTimes.Open24Hours;

            }

            #endregion

            #endregion


            public EVSEDataRecord Build()
            {

                return new EVSEDataRecord(EVSEId,
                                          DeltaType,
                                          LastUpdate,
                                          ChargingStationId,
                                          ChargingStationName,
                                          Address,
                                          GeoCoordinate,
                                          Plugs,
                                          ChargingFacilities,
                                          ChargingModes,
                                          AuthenticationModes,
                                          MaxCapacity,
                                          PaymentOptions,
                                          Accessibility,
                                          HotlinePhoneNumber,
                                          AdditionalInfo,
                                          GeoChargingPointEntrance,
                                          IsOpen24Hours,
                                          OpeningTime,
                                          HubOperatorId,
                                          ClearingHouseId,
                                          IsHubjectCompatible,
                                          DynamicInfoAvailable);

          }

        }

    }

}
