/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/GraphDefined/WWCP_OICP>
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
using System.Collections.Generic;

using org.GraphDefined.WWCP;
using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using System.Xml.Linq;
using System.Text.RegularExpressions;

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    /// <summary>
    /// A OICP v2.0 Electric Vehicle Supply Equipment (EVSE).
    /// This is meant to be one electrical circuit which can charge a electric vehicle.
    /// </summary>
    public class EVSEDataRecord
    {

        #region Data

        private static Regex HotlinePhoneNumberRegExpr = new Regex("\\+[^0-9]");

        #endregion

        #region Properties

        #region EVSEId

        private readonly EVSE_Id _EVSEId;

        /// <summary>
        /// The unique identification of the Electric Vehicle Supply Equipment.
        /// </summary>
        public EVSE_Id EVSEId
        {
            get
            {
                return _EVSEId;
            }
        }

        #endregion

        #region DeltaType

        private String _DeltaType;

        public String DeltaType
        {

            get
            {
                return _DeltaType;
            }

            set
            {
                if (value != null)
                    _DeltaType = value;
            }

        }

        #endregion

        #region LastUpdate

        private DateTime? _LastUpdate;

        public DateTime? LastUpdate
        {

            get
            {
                return _LastUpdate;
            }

            set
            {
                if (value != null)
                    _LastUpdate = value;
            }

        }

        #endregion


        #region ChargingStationId

        private String _ChargingStationId;

        public String ChargingStationId
        {

            get
            {
                return _ChargingStationId;
            }

            set
            {
                if (value != null)
                    _ChargingStationId = value;
            }

        }

        #endregion

        #region ChargingStationName

        private I18NString _ChargingStationName;

        public I18NString ChargingStationName
        {

            get
            {
                return _ChargingStationName;
            }

            set
            {
                if (value != null)
                    _ChargingStationName = value;
            }

        }

        #endregion

        #region Address

        private Address _Address;

        public Address Address
        {

            get
            {
                return _Address;
            }

            set
            {
                if (value != null)
                    _Address = value;
            }

        }

        #endregion

        #region GeoCoordinate

        private GeoCoordinate _GeoCoordinate;

        public GeoCoordinate GeoCoordinate
        {

            get
            {
                return _GeoCoordinate;
            }

            set
            {
                if (value != null)
                    _GeoCoordinate = value;
            }

        }

        #endregion

        #region Plugs

        private IEnumerable<PlugTypes> _Plugs;

        /// <summary>
        /// The types of the charging plugs.
        /// </summary>
        public IEnumerable<PlugTypes> Plugs
        {

            get
            {
                return _Plugs;
            }

            set
            {
                if (value != null)
                    _Plugs = value;
            }

        }

        #endregion

        #region ChargingFacilities

        private IEnumerable<ChargingFacilities> _ChargingFacilities;

        public IEnumerable<ChargingFacilities> ChargingFacilities
        {

            get
            {
                return _ChargingFacilities;
            }

            set
            {
                if (value != null)
                    _ChargingFacilities = value;
            }

        }

        #endregion

        #region ChargingModes

        private IEnumerable<ChargingModes> _ChargingModes;

        public IEnumerable<ChargingModes> ChargingModes
        {

            get
            {
                return _ChargingModes;
            }

            set
            {
                if (value != null)
                    _ChargingModes = value;
            }

        }

        #endregion

        #region AuthenticationModes

        private IEnumerable<AuthenticationModes> _AuthenticationModes;

        public IEnumerable<AuthenticationModes> AuthenticationModes
        {

            get
            {
                return _AuthenticationModes;
            }

            set
            {
                if (value != null)
                    _AuthenticationModes = value;
            }

        }

        #endregion

        #region MaxCapacity

        private Double? _MaxCapacity_kWh;

        public Double? MaxCapacity
        {

            get
            {
                return _MaxCapacity_kWh;
            }

            set
            {
                if (value != null)
                    _MaxCapacity_kWh = value;
            }

        }

        #endregion

        #region PaymentOptions

        private IEnumerable<PaymentOptions> _PaymentOptions;

        public IEnumerable<PaymentOptions> PaymentOptions
        {

            get
            {
                return _PaymentOptions;
            }

            set
            {
                if (value != null)
                    _PaymentOptions = value;
            }

        }

        #endregion

        #region Accessibility

        private AccessibilityTypes _Accessibility;

        public AccessibilityTypes Accessibility
        {

            get
            {
                return _Accessibility;
            }

            set
            {
                if (value != null)
                    _Accessibility = value;
            }

        }

        #endregion

        #region HotlinePhoneNumber

        private String _HotlinePhoneNumber;

        public String HotlinePhoneNumber
        {

            get
            {
                return _HotlinePhoneNumber;
            }

            set
            {
                if (value != null)
                    _HotlinePhoneNumber = value;
            }

        }

        #endregion

        #region AdditionalInfo

        private I18NString _AdditionalInfo;

        public I18NString AdditionalInfo
        {

            get
            {
                return _AdditionalInfo;
            }

            set
            {
                if (value != null)
                    _AdditionalInfo = value;
            }

        }

        #endregion

        #region GeoChargingPointEntrance

        private GeoCoordinate _GeoChargingPointEntrance;

        public GeoCoordinate GeoChargingPointEntrance
        {

            get
            {
                return _GeoChargingPointEntrance;
            }

            set
            {
                if (value != null)
                    _GeoChargingPointEntrance = value;
            }

        }

        #endregion

        #region IsOpen24Hours

        public Boolean IsOpen24Hours
        {
            get
            {

                if (_OpeningTime == null)
                    return true;

                return _OpeningTime.IsOpen24Hours;

            }
        }

        #endregion

        #region OpeningTime

        private OpeningTime _OpeningTime;

        public OpeningTime OpeningTime
        {

            get
            {
                return _OpeningTime;
            }

            set
            {
                if (value != null)
                    _OpeningTime = value;
            }

        }

        #endregion

        #region HubOperatorId

        private HubOperator_Id _HubOperatorId;

        public HubOperator_Id HubOperatorId
        {

            get
            {
                return _HubOperatorId;
            }

            set
            {
                if (value != null)
                    _HubOperatorId = value;
            }

        }

        #endregion

        #region ClearingHouseId

        private RoamingProvider_Id _ClearingHouseId;

        public RoamingProvider_Id ClearingHouseId
        {

            get
            {
                return _ClearingHouseId;
            }

            set
            {
                if (value != null)
                    _ClearingHouseId = value;
            }

        }

        #endregion

        #region IsHubjectCompatible

        private Boolean _IsHubjectCompatible;

        public Boolean IsHubjectCompatible
        {

            get
            {
                return _IsHubjectCompatible;
            }

            set
            {
                _IsHubjectCompatible = value;
            }

        }

        #endregion

        #region DynamicInfoAvailable

        private Boolean _DynamicInfoAvailable;

        public Boolean DynamicInfoAvailable
        {

            get
            {
                return _DynamicInfoAvailable;
            }

            set
            {
                _DynamicInfoAvailable = value;
            }

        }

        #endregion

        #endregion

        #region Constructor(s)

        #region EVSEDataRecord(EVSEId)

        public EVSEDataRecord(EVSE_Id  EVSEId)

        {

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentNullException("EVSEId", "The given parameter must not be null!");

            #endregion

            this._EVSEId               = EVSEId;
            this._ChargingStationName  = new I18NString();
            this._AdditionalInfo       = new I18NString();

        }

        #endregion

        #region EVSEDataRecord(EVSEId, ...)

        public EVSEDataRecord(EVSE_Id                           EVSEId,
                              String                            ChargingStationId           = null,
                              I18NString                        ChargingStationName         = null,
                              Address                           Address                     = null,
                              GeoCoordinate                     GeoCoordinate               = null,
                              IEnumerable<PlugTypes>            Plugs                       = null,
                              IEnumerable<ChargingFacilities>   ChargingFacilities          = null,
                              IEnumerable<ChargingModes>        ChargingModes               = null,
                              IEnumerable<AuthenticationModes>  AuthenticationModes         = null,
                              Int32?                            MaxCapacity                 = null,
                              IEnumerable<PaymentOptions>       PaymentOptions              = null,
                              AccessibilityTypes                Accessibility               = AccessibilityTypes.Free_publicly_accessible,
                              String                            HotlinePhoneNumber          = null,
                              I18NString                        AdditionalInfo              = null,
                              GeoCoordinate                     GeoChargingPointEntrance    = null,
                              Boolean?                          IsOpen24Hours               = null,
                              OpeningTime                       OpeningTime                 = null,
                              HubOperator_Id                    HubOperatorId               = null,
                              RoamingProvider_Id                ClearingHouseId             = null,
                              Boolean                           IsHubjectCompatible         = true,
                              Boolean                           DynamicInfoAvailable        = true)

            : this(EVSEId)

        {

            if (Address == null)
                throw new ArgumentNullException("The given parameter for 'Address' must not be null!");

            if (GeoCoordinate == null)
                throw new ArgumentNullException("The given parameter for 'GeoCoordinates' must not be null!");

            if (Plugs == null)
                throw new ArgumentNullException("The given parameter for 'Plugs' must not be null!");

            if (Plugs.Count() < 1)
                throw new ArgumentNullException("The given parameter for 'Plugs' must not be empty!");

            if (AuthenticationModes == null)
                throw new ArgumentNullException("The given parameter for 'AuthenticationModes' must not be null!");

            if (AuthenticationModes.Count() < 1)
                throw new ArgumentNullException("The given parameter for 'AuthenticationModes' must not be empty!");

            if (HotlinePhoneNumber == null || HotlinePhoneNumber.IsNullOrEmpty())
                throw new ArgumentNullException("The given parameter for 'HotlinePhoneNumber' must not be null or empty!");


            this._ChargingStationId         = ChargingStationId;
            this._ChargingStationName       = ChargingStationName != null ? ChargingStationName : new I18NString();
            this._Address                   = Address;
            this._GeoCoordinate             = GeoCoordinate;
            this._Plugs                     = Plugs;
            this._ChargingModes             = ChargingModes;
            this._ChargingFacilities        = ChargingFacilities;
            this._AuthenticationModes       = AuthenticationModes;
            this._MaxCapacity_kWh           = MaxCapacity;
            this._PaymentOptions            = PaymentOptions;
            this._Accessibility             = Accessibility;
            this._HotlinePhoneNumber        = HotlinePhoneNumber;
            this._AdditionalInfo            = AdditionalInfo      != null ? AdditionalInfo      : new I18NString();
            this._GeoChargingPointEntrance  = GeoChargingPointEntrance;
            this._HubOperatorId             = HubOperatorId;
            this._ClearingHouseId           = ClearingHouseId;
            this._IsHubjectCompatible       = IsHubjectCompatible;
            this._DynamicInfoAvailable      = DynamicInfoAvailable;


            if (IsOpen24Hours != null && IsOpen24Hours.HasValue && IsOpen24Hours.Value)
                this._OpeningTime  = OpeningTime.Open24Hours;

            if (OpeningTime != null)
                this._OpeningTime  = OpeningTime;

            if (OpeningTime == null && (IsOpen24Hours == null || !IsOpen24Hours.HasValue))
                this._OpeningTime = OpeningTime.Open24Hours;

        }

        #endregion

        #endregion


        #region Parse(EVSEDataRecordXML, OnException = null)

        public static EVSEDataRecord Parse(XElement             EVSEDataRecordXML,
                                           OnExceptionDelegate  OnException = null)
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


            var EVSEDataRecord = new EVSEDataRecord(EVSE_Id.Parse(EVSEDataRecordXML.ElementValueOrFail(OICPNS.EVSEData + "EvseId", "Missing 'EvseId'-XML tag!")));

            #region XML Attribute: DeltaType

            EVSEDataRecord.DeltaType  = EVSEDataRecordXML.AttributeValueOrDefault(XName.Get("deltaType"),  "");

            #endregion

            #region XML Attribute: LastUpdate

            DateTime _LastUpdate;
            if (DateTime.TryParse(EVSEDataRecordXML.AttributeValueOrDefault(XName.Get("lastUpdate"), ""), out _LastUpdate))
                EVSEDataRecord.LastUpdate = _LastUpdate;

            #endregion


            #region ChargingStationId, ChargingStationName

            EVSEDataRecordXML.IfElementIsDefined(OICPNS.EVSEData + "ChargingStationId",
                                                 v => EVSEDataRecord.ChargingStationId = v);

            EVSEDataRecordXML.IfElementIsDefined(OICPNS.EVSEData + "ChargingStationName",
                                                 v => EVSEDataRecord.ChargingStationName.Add(Languages.de, v));

            EVSEDataRecordXML.IfElementIsDefined(OICPNS.EVSEData + "EnChargingStationName",
                                                 v => EVSEDataRecord.ChargingStationName.Add(Languages.en, v));

            #endregion

            #region Address

            var AddressXML   = EVSEDataRecordXML.ElementOrFail(OICPNS.EVSEData + "Address", "Missing 'Address'-XML tag!");

            var _CountryTXT  = AddressXML.ElementValueOrFail(OICPNS.CommonTypes + "Country", "Missing 'Country'-XML tag!").Trim();

            Country _Country;
            if (!Country.TryParse(_CountryTXT, out _Country))
            {

                if (_CountryTXT.ToUpper() == "UNKNOWN")
                    _Country = Country.unknown;

                else
                    throw new Exception("'" + _CountryTXT + "' is an unknown country name!");

            }

            EVSEDataRecord.Address = new Address(AddressXML.ElementValueOrFail   (OICPNS.CommonTypes + "Street",     "Missing 'Street'-XML tag!").Trim(),
                                                 AddressXML.ElementValueOrDefault(OICPNS.CommonTypes + "HouseNum",   "").Trim(),
                                                 AddressXML.ElementValueOrDefault(OICPNS.CommonTypes + "Floor",      "").Trim(),
                                                 AddressXML.ElementValueOrDefault(OICPNS.CommonTypes + "PostalCode", "").Trim(),
                                                 "",
                                                 I18NString.Create(Languages.unknown, AddressXML.ElementValueOrFail(OICPNS.CommonTypes + "City", "Missing 'City'-XML tag!").Trim()),
                                                 _Country);

            // Currently not used OICP address information!
            //var _Region       = AddressXML.       ElementValueOrDefault(OICPNS.OICPv2_0CommonTypes + "Region",     "").Trim();
            //var _Timezone     = AddressXML.       ElementValueOrDefault(OICPNS.OICPv2_0CommonTypes + "Timezone",   "").Trim();

            #endregion

            #region GeoCoordinate

            EVSEDataRecord.GeoCoordinate = XMLMethods.ParseGeoCoordinatesXML(EVSEDataRecordXML.ElementOrFail(OICPNS.EVSEData + "GeoCoordinates", "Missing 'GeoCoordinates'-XML tag!"));

            #endregion

            #region Plugs

            EVSEDataRecord.Plugs = new ReactiveSet<PlugTypes>(EVSEDataRecordXML.
                                                                  MapValuesOrFail(OICPNS.EVSEData + "Plugs", "Missing 'Plugs'-XML tag!",
                                                                                  OICPNS.EVSEData + "Plug",
                                                                                  OICPMapper.AsPlugType));

            #endregion

            #region ChargingFacilities

            EVSEDataRecord.ChargingFacilities = new ReactiveSet<ChargingFacilities>(EVSEDataRecordXML.
                                                                                        MapValuesOrDefault(OICPNS.EVSEData + "ChargingFacilities",
                                                                                                           OICPNS.EVSEData + "ChargingFacility",
                                                                                                           OICPMapper.AsChargingFacility,
                                                                                                           org.GraphDefined.WWCP.ChargingFacilities.Unspecified));

            #endregion

            #region ChargingModes

            EVSEDataRecord.ChargingModes = new ReactiveSet<ChargingModes>(EVSEDataRecordXML.
                                                                              MapValuesOrDefault(OICPNS.EVSEData + "ChargingModes",
                                                                                                 OICPNS.EVSEData + "ChargingMode",
                                                                                                 OICPMapper.AsChargingMode,
                                                                                                 org.GraphDefined.WWCP.ChargingModes.Unspecified));

            #endregion

            #region AuthenticationModes

            EVSEDataRecord.AuthenticationModes = new ReactiveSet<AuthenticationModes>(EVSEDataRecordXML.
                                                                                          MapValuesOrFail(OICPNS.EVSEData + "AuthenticationModes", "Missing 'AuthenticationModes'-XML tag!",
                                                                                                          OICPNS.EVSEData + "AuthenticationMode",
                                                                                                          OICPMapper.AsAuthenticationMode));

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
                                                                                MapValuesOrDefault(OICPNS.EVSEData + "PaymentOptions",
                                                                                                   OICPNS.EVSEData + "PaymentOption",
                                                                                                   OICPMapper.AsPaymetOption,
                                                                                                   WWCP.PaymentOptions.Unspecified));

            #endregion

            #region Accessibility

            EVSEDataRecord.Accessibility = OICPMapper.AsAccessibilityType(EVSEDataRecordXML.
                                                                              ElementValueOrFail(OICPNS.EVSEData + "Accessibility", "Missing 'Accessibility'-XML tag!").
                                                                              Trim());

            #endregion

            #region HotlinePhoneNum

            EVSEDataRecord.HotlinePhoneNumber = EVSEDataRecordXML.
                                                 ElementValueOrFail(OICPNS.EVSEData + "HotlinePhoneNum", "Missing 'HotlinePhoneNum '-XML tag!").
                                                 Trim();

            #endregion

            #region AdditionalInfo

            EVSEDataRecordXML.IfElementIsDefined(OICPNS.EVSEData + "AdditionalInfo",
                                                 v => EVSEDataRecord.AdditionalInfo.Add(Languages.de, v));

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
                                                                     EVSEDataRecord.AdditionalInfo.Add((Languages) Enum.Parse(typeof(Languages),
                                                                                                                              Country.ParseAlpha3Code(I18NTokens[0]).Alpha2Code.ToLower()),
                                                                                                       I18NTokens[1]);
                                                             }
                                                             catch (Exception e)
                                                             {
                                                                 DebugX.Log("Could not parse 'EnAdditionalInfo': " + I18NTokens + Environment.NewLine + e.Message);
                                                             }

                                                         }

                                                     }

                                                     else
                                                         EVSEDataRecord.AdditionalInfo.Add(Languages.en, EnAdditionalInfo);

                                                 });

            #endregion

            #region Get geo coordinate of the charging pool entrance...

            var GeoChargingPointEntranceXML = EVSEDataRecordXML.Element(OICPNS.CommonTypes + "GeoChargingPointEntrance");
            if (GeoChargingPointEntranceXML != null)
                EVSEDataRecord.GeoChargingPointEntrance = XMLMethods.ParseGeoCoordinatesXML(GeoChargingPointEntranceXML);

            #endregion

            #region IsOpen24Hours / OpeningTime

            if (EVSEDataRecordXML.ElementValueOrFail(OICPNS.EVSEData + "IsOpen24Hours", "Missing 'IsOpen24Hours'-XML tag!") == "true")
            {
                EVSEDataRecord.OpeningTime = OpeningTime.Open24Hours;
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

            HubOperator_Id _HubOperatorId;
            if (HubOperator_Id.TryParse(EVSEDataRecordXML.
                                            ElementValueOrDefault(OICPNS.EVSEData + "HubOperatorID", String.Empty).
                                            Trim(),
                                        out _HubOperatorId))
                EVSEDataRecord.HubOperatorId = _HubOperatorId;

            #endregion

            #region ClearinghouseID

            RoamingProvider_Id _ClearinghouseID;
            if (RoamingProvider_Id.TryParse(EVSEDataRecordXML.
                                                ElementValueOrDefault(OICPNS.EVSEData + "ClearinghouseID", String.Empty).
                                                Trim(),
                                                out _ClearinghouseID))
                EVSEDataRecord.ClearingHouseId = _ClearinghouseID;

            #endregion

            #region IsHubjectCompatible

            EVSEDataRecord.IsHubjectCompatible  = EVSEDataRecordXML.
                                                      ElementValueOrFail(OICPNS.EVSEData + "IsHubjectCompatible", "Missing 'IsHubjectCompatible '-XML tag!").
                                                      Trim() == "true";

            #endregion

            #region DynamicInfoAvailable

            EVSEDataRecord.DynamicInfoAvailable  = EVSEDataRecordXML.
                                                       ElementValueOrFail(OICPNS.EVSEData + "DynamicInfoAvailable", "Missing 'DynamicInfoAvailable '-XML tag!").
                                                       Trim() != "false";

            #endregion

            // Currently not used OICP address information!
            //var _Region       = AddressXML.       ElementValueOrDefault(OICPNS.OICPv2_0CommonTypes + "Region",     "").Trim();
            //var _Timezone     = AddressXML.       ElementValueOrDefault(OICPNS.OICPv2_0CommonTypes + "Timezone",   "").Trim();

            // EnAdditionalInfo not parsed as OICP v2.0 multi-language string!

            return EVSEDataRecord;

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

                new XElement(OICPNS.EVSEData + "EvseId",                _EVSEId.OriginId),
                new XElement(OICPNS.EVSEData + "ChargingStationId",     _ChargingStationId),
                new XElement(OICPNS.EVSEData + "ChargingStationName",   _ChargingStationName[Languages.de].SubstringMax(50)),
                new XElement(OICPNS.EVSEData + "EnChargingStationName", _ChargingStationName[Languages.en].SubstringMax(50)),

                new XElement(OICPNS.EVSEData + "Address",
                    new XElement(OICPNS.CommonTypes + "Country",        _Address.Country.Alpha3Code),
                    new XElement(OICPNS.CommonTypes + "City",           _Address.City),
                    new XElement(OICPNS.CommonTypes + "Street",         _Address.Street), // OICPEVSEData.0 requires at least 5 characters!

                    _Address.PostalCode. IsNotNullOrEmpty()
                        ? new XElement(OICPNS.CommonTypes + "PostalCode", _Address.PostalCode)
                        : null,

                    _Address.HouseNumber.IsNotNullOrEmpty()
                        ? new XElement(OICPNS.CommonTypes + "HouseNum",   _Address.HouseNumber)
                        : null,

                    _Address.FloorLevel. IsNotNullOrEmpty()
                        ? new XElement(OICPNS.CommonTypes + "Floor",      _Address.FloorLevel)
                        : null

                    // <!--Optional:-->
                    // <v11:Region>?</v11:Region>

                    // <!--Optional:-->
                    // <v11:TimeZone>?</v11:TimeZone>

                ),

                new XElement(OICPNS.EVSEData + "GeoCoordinates",
                    new XElement(OICPNS.CommonTypes + "DecimalDegree",  // Force 0.00... (dot) format!
                        new XElement(OICPNS.CommonTypes + "Longitude",  _GeoCoordinate.Longitude.ToString("{0:0.######}").Replace(",", ".")),// CultureInfo.InvariantCulture.NumberFormat)),
                        new XElement(OICPNS.CommonTypes + "Latitude",   _GeoCoordinate.Latitude. ToString("{0:0.######}").Replace(",", ".")) // CultureInfo.InvariantCulture.NumberFormat))
                    )
                ),

                new XElement(OICPNS.EVSEData + "Plugs",
                    _Plugs.                   Select(Plug               => new XElement(OICPNS.EVSEData + "Plug",               OICPMapper.AsString(Plug)))
                ),

                _ChargingFacilities.NotNullAny()
                    ? new XElement(OICPNS.EVSEData + "ChargingFacilities",
                          _ChargingFacilities.Select(ChargingFacility   => new XElement(OICPNS.EVSEData + "ChargingFacility",   ChargingFacility.  ToString().Replace("_", " "))))
                    : null,

                _ChargingModes.NotNullAny()
                    ? new XElement(OICPNS.EVSEData + "ChargingModes",
                          _ChargingModes.     Select(ChargingMode       => new XElement(OICPNS.EVSEData + "ChargingMode",       ChargingMode.      ToString())))
                    : null,

                new XElement(OICPNS.EVSEData + "AuthenticationModes",
                    _AuthenticationModes.     Select(AuthenticationMode => new XElement(OICPNS.EVSEData + "AuthenticationMode", AuthenticationMode.ToString().Replace("_", " ")))
                ),

                _MaxCapacity_kWh.HasValue
                    ? new XElement(OICPNS.EVSEData + "MaxCapacity", _MaxCapacity_kWh)
                    : null,

                new XElement(OICPNS.EVSEData + "PaymentOptions",
                    _PaymentOptions.          Select(PaymentOption      => new XElement(OICPNS.EVSEData + "PaymentOption",      PaymentOption.     ToString().Replace("_", " ")))
                ),

                new XElement(OICPNS.EVSEData + "Accessibility",     _Accessibility.ToString().Replace("_", " ")),
                new XElement(OICPNS.EVSEData + "HotlinePhoneNum",   HotlinePhoneNumberRegExpr.Replace(_HotlinePhoneNumber, "")),  // RegEx: \+[0-9]{5,15}

                _AdditionalInfo.has(Languages.de)
                    ? new XElement(OICPNS.EVSEData + "AdditionalInfo", _AdditionalInfo)
                    : null,

                _AdditionalInfo.has(Languages.en)
                    ? new XElement(OICPNS.EVSEData + "EnAdditionalInfo", _AdditionalInfo[Languages.en])
                    : null,

                _GeoChargingPointEntrance != null
                    ? new XElement(OICPNS.EVSEData + "GeoChargingPointEntrance",
                        new XElement(OICPNS.CommonTypes + "DecimalDegree",  // Force 0.00... (dot) format!
                            new XElement(OICPNS.CommonTypes + "Longitude", _GeoChargingPointEntrance.Longitude.ToString("{0:0.######}").Replace(",", ".")),// CultureInfo.InvariantCulture.NumberFormat)),
                            new XElement(OICPNS.CommonTypes + "Latitude",  _GeoChargingPointEntrance.Latitude. ToString("{0:0.######}").Replace(",", ".")) // CultureInfo.InvariantCulture.NumberFormat))
                        )
                    )
                    : null,

                new XElement(OICPNS.EVSEData + "IsOpen24Hours",         _OpeningTime.IsOpen24Hours ? "true" : "false"),

                _OpeningTime.IsOpen24Hours
                    ? null
                    : new XElement(OICPNS.EVSEData + "OpeningTime",     _OpeningTime.Text),

                _HubOperatorId != null
                    ? new XElement(OICPNS.EVSEData + "HubOperatorID",   _HubOperatorId.ToString())
                    : null,

                _ClearingHouseId != null
                    ? new XElement(OICPNS.EVSEData + "ClearinghouseID", _ClearingHouseId.ToString())
                    : null,

                new XElement(OICPNS.EVSEData + "IsHubjectCompatible",   _IsHubjectCompatible  ? "true" : "false"),
                new XElement(OICPNS.EVSEData + "DynamicInfoAvailable",  _DynamicInfoAvailable ? "true" : "false")

            );

        }

        #endregion

    }

}
