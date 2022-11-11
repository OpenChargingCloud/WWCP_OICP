/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// An Electric Vehicle Supply Equipment (EVSE) data record.
    /// This is meant to be one electrical circuit which can charge an electric vehicle.
    /// </summary>
    public class EVSEDataRecord : AInternalData,
                                  IEquatable<EVSEDataRecord>,
                                  IComparable<EVSEDataRecord>,
                                  IComparable
    {

        #region Properties

        /// <summary>
        /// The unique identification of the Electric Vehicle Supply Equipment (EVSE).
        /// </summary>
        [Mandatory]
        public EVSE_Id                              Id                                     { get; }


        /// <summary>
        /// The delta type when the EVSE data record was just downloaded.
        /// </summary>
        [Optional]
        public DeltaTypes?                          DeltaType                              { get; }

        /// <summary>
        /// The last update timestamp of the EVSE data record.
        /// </summary>
        [Optional]
        public DateTime?                            LastUpdate                             { get; }


        /// <summary>
        /// The unique identification of the operator of the EVSE.
        /// </summary>
        [Mandatory]
        public Operator_Id                          OperatorId                             { get; }

        /// <summary>
        /// The name of the operator of the EVSE.
        /// </summary>
        [Mandatory]
        public String                               OperatorName                           { get; }

        /// <summary>
        /// The identification of the charging station hosting the EVSE.
        /// </summary>
        [Optional]
        public ChargingStation_Id?                  ChargingStationId                      { get; }

        /// <summary>
        /// The identification of the charging pool hosting the EVSE.
        /// </summary>
        [Optional]
        public ChargingPool_Id?                     ChargingPoolId                         { get; }

        /// <summary>
        /// The multi-language name of the charging station hosting the EVSE.
        /// </summary>
        [Mandatory]
        public I18NText                             ChargingStationName                    { get; }

        /// <summary>
        /// Optional name of the EVSE manufacturer.
        /// </summary>
        [Optional]
        public String?                              HardwareManufacturer                   { get; }

        /// <summary>
        /// Optional URL to an image of the EVSE.
        /// </summary>
        [Optional]
        public URL?                                 ChargingStationImageURL                { get; }

        /// <summary>
        /// Optional name of the sub operator owning the EVSE.
        /// </summary>
        [Optional]
        public String?                              SubOperatorName                        { get; }

        /// <summary>
        /// Whether the EVSE is able to deliver different power outputs.
        /// </summary>
        [Optional]
        public Boolean?                             DynamicPowerLevel                      { get; }

        /// <summary>
        /// The address of the EVSE.
        /// </summary>
        [Mandatory]
        public Address                              Address                                { get; }

        /// <summary>
        /// The geo coordinate of the EVSE.
        /// </summary>
        [Mandatory]
        public GeoCoordinates                       GeoCoordinates                         { get; }

        /// <summary>
        /// The types of charging plugs attached to the EVSE.
        /// </summary>
        [Mandatory]
        public IEnumerable<PlugTypes>               PlugTypes                              { get; }

        /// <summary>
        /// An enumeration of supported charging facilities at the EVSE.
        /// </summary>
        [Mandatory]
        public IEnumerable<ChargingFacility>        ChargingFacilities                     { get; }


        /// <summary>
        /// This field gives the information how the charging station provides metering law data.
        /// </summary>
        [Mandatory]
        public CalibrationLawDataAvailabilities     CalibrationLawDataAvailability         { get; }

        /// <summary>
        /// The authentication modes the EVSE supports.
        /// </summary>
        [Mandatory]
        public IEnumerable<AuthenticationModes>     AuthenticationModes                    { get; }

        /// <summary>
        /// If the EVSE provides only renewable energy then the value MUST be” true”,
        /// if it use grey energy then value MUST be “false”.
        /// </summary>
        [Mandatory]
        public Boolean                              RenewableEnergy                        { get; }

        /// <summary>
        /// Optional enumeration of energy sources that the EVSE uses to supply electric energy.
        /// </summary>
        [Optional]
        public IEnumerable<EnergySource>?           EnergySources                          { get; }

        /// <summary>
        /// Optional environmental impact produced by the energy sources used by the EVSE.
        /// </summary>
        [Optional]
        public EnvironmentalImpact?                 EnvironmentalImpact                    { get; }

        /// <summary>
        /// The maximum in kWh capacity the EVSE provides.
        /// </summary>
        [Optional]
        public UInt32?                              MaxCapacity                            { get; }

        /// <summary>
        /// An enumeration of payment options that are supported.
        /// </summary>
        [Mandatory]
        public IEnumerable<PaymentOptions>          PaymentOptions                         { get; }

        /// <summary>
        /// An enumeration of "value added services" the EVSE supports.
        /// </summary>
        [Mandatory]
        public IEnumerable<ValueAddedServices>      ValueAddedServices                     { get; }

        /// <summary>
        /// Specifies how the charging station can be accessed.
        /// </summary>
        [Mandatory]
        public AccessibilityTypes                   Accessibility                          { get; }

        /// <summary>
        /// Optional information where the EVSE could be accessed.
        /// </summary>
        [Optional]
        public AccessibilityLocationTypes?          AccessibilityLocationType              { get; }

        /// <summary>
        /// The phone number of the charging station operator's hotline.
        /// </summary>
        [Optional]
        public Phone_Number?                        HotlinePhoneNumber                     { get; }

        /// <summary>
        /// Optional multi-language information about the EVSE.
        /// </summary>
        [Optional]
        public I18NText?                            AdditionalInfo                         { get; }

        /// <summary>
        /// Optional last meters information regarding the location of the EVSE.
        /// </summary>
        [Optional]
        public I18NText?                            ChargingStationLocationReference       { get; }

        /// <summary>
        /// In case that the EVSE is part of a bigger facility (e.g. parking place),
        /// this optional attribute specifies the facilities entrance coordinates.
        /// </summary>
        [Optional]
        public GeoCoordinates?                      GeoChargingPointEntrance               { get; }

        /// <summary>
        /// Whether the EVSE is open 24/7.
        /// </summary>
        [Mandatory]
        public Boolean                              IsOpen24Hours                          { get; }

        /// <summary>
        /// Optional opening times in case that the EVSE cannot be accessed around the clock.
        /// </summary>
        [Optional]
        public IEnumerable<OpeningTime>?            OpeningTimes                           { get; }

        /// <summary>
        /// The optional hub operator of the EVSE.
        /// </summary>
        [Optional]
        public Operator_Id?                         HubOperatorId                          { get; }

        /// <summary>
        /// Optional clearing house for all charging sessions at the EVSE.
        /// </summary>
        [Optional]
        public ClearingHouse_Id?                    ClearingHouseId                        { get; }

        /// <summary>
        /// Whether ev roaming via Intercharge at the EVSE is possible.
        /// If set to "false" the EVSE will not be started/stopped remotely via Hubject.
        /// </summary>
        [Mandatory]
        public Boolean                              IsHubjectCompatible                    { get; }

        /// <summary>
        /// Whether the CPO provides dynamic EVSE data information.
        /// </summary>
        [Mandatory]
        public FalseTrueAuto                        DynamicInfoAvailable                   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new Electric Vehicle Supply Equipment (EVSE) data record.
        /// </summary>
        /// <param name="Id">A unique EVSE identification.</param>
        /// <param name="OperatorId">The unique identification of the operator of the EVSE.</param>
        /// <param name="OperatorName">The name of the operator of the EVSE.</param>
        /// <param name="ChargingStationName">The multi-language name of the charging station hosting the EVSE.</param>
        /// <param name="Address">The address of the EVSE.</param>
        /// <param name="GeoCoordinates">The geo coordinate of the EVSE.</param>
        /// <param name="PlugTypes">The types of charging plugs attached to the EVSE.</param>
        /// <param name="ChargingFacilities">An enumeration of supported charging facilities at the EVSE.</param>
        /// <param name="RenewableEnergy">If the EVSE provides only renewable energy then the value MUST be” true”, if it use grey energy then value MUST be “false”.</param>
        /// <param name="CalibrationLawDataAvailability">This field gives the information how the charging station provides metering law data.</param>
        /// <param name="AuthenticationModes">The authentication modes the EVSE supports.</param>
        /// <param name="PaymentOptions">An enumeration of payment options that are supported.</param>
        /// <param name="ValueAddedServices">An enumeration of "value added services" the EVSE supports.</param>
        /// <param name="Accessibility">Specifies how the charging station can be accessed.</param>
        /// <param name="HotlinePhoneNumber">The phone number of the charging station operator's hotline.</param>
        /// <param name="IsOpen24Hours">Whether the EVSE is open 24/7.</param>
        /// <param name="IsHubjectCompatible">Whether ev roaming via Intercharge at the EVSE is possible. If set to "false" the EVSE will not be started/stopped remotely via Hubject.</param>
        /// <param name="DynamicInfoAvailable">Whether the CPO provides dynamic EVSE data information.</param>
        /// 
        /// <param name="DeltaType">The delta type when the EVSE data record was just downloaded.</param>
        /// <param name="LastUpdate">The last update timestamp of the EVSE data record.</param>
        /// 
        /// <param name="ChargingStationId">The identification of the charging station hosting the EVSE.</param>
        /// <param name="ChargingPoolId">The identification of the charging pool hosting the EVSE.</param>
        /// <param name="HardwareManufacturer">Optional name of the EVSE manufacturer.</param>
        /// <param name="ChargingStationImageURL">Optional URL to an image of the EVSE.</param>
        /// <param name="SubOperatorName">Optional name of the sub operator owning the EVSE.</param>
        /// <param name="DynamicPowerLevel">Whether the EVSE is able to deliver different power outputs.</param>
        /// <param name="EnergySources">Optional enumeration of energy sources that the EVSE uses to supply electric energy.</param>
        /// <param name="EnvironmentalImpact">Optional environmental impact produced by the energy sources used by the EVSE.</param>
        /// <param name="MaxCapacity">The maximum in kWh capacity the EVSE provides.</param>
        /// <param name="AccessibilityLocationType">Optional information where the EVSE could be accessed.</param>
        /// <param name="AdditionalInfo">Optional multi-language information about the EVSE.</param>
        /// <param name="ChargingStationLocationReference">Optional last meters information regarding the location of the EVSE.</param>
        /// <param name="GeoChargingPointEntrance">In case that the EVSE is part of a bigger facility (e.g. parking place), this optional attribute specifies the facilities entrance coordinates.</param>
        /// <param name="OpeningTimes">Optional opening times in case that the EVSE cannot be accessed around the clock.</param>
        /// <param name="HubOperatorId">The optional hub operator of the EVSE.</param>
        /// <param name="ClearingHouseId">Optional clearing house for all charging sessions at the EVSE.</param>
        /// 
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// <param name="InternalData">Optional internal customer specific data, e.g. in combination with custom parsers and serializers.</param>
        public EVSEDataRecord(EVSE_Id                           Id,
                              Operator_Id                       OperatorId,
                              String                            OperatorName,
                              I18NText                          ChargingStationName,
                              Address                           Address,
                              GeoCoordinates                    GeoCoordinates,
                              IEnumerable<PlugTypes>            PlugTypes,
                              IEnumerable<ChargingFacility>     ChargingFacilities,
                              Boolean                           RenewableEnergy,
                              CalibrationLawDataAvailabilities  CalibrationLawDataAvailability,
                              IEnumerable<AuthenticationModes>  AuthenticationModes,
                              IEnumerable<PaymentOptions>       PaymentOptions,
                              IEnumerable<ValueAddedServices>   ValueAddedServices,
                              AccessibilityTypes                Accessibility,
                              Phone_Number?                     HotlinePhoneNumber,                // mandatory => optional, because of Hubject data quality issues!
                              Boolean                           IsOpen24Hours,
                              Boolean                           IsHubjectCompatible,
                              FalseTrueAuto                     DynamicInfoAvailable,

                              DeltaTypes?                       DeltaType                          = null,
                              DateTime?                         LastUpdate                         = null,

                              ChargingStation_Id?               ChargingStationId                  = null,
                              ChargingPool_Id?                  ChargingPoolId                     = null,
                              String?                           HardwareManufacturer               = null,
                              URL?                              ChargingStationImageURL            = null,
                              String?                           SubOperatorName                    = null,
                              Boolean?                          DynamicPowerLevel                  = null,
                              IEnumerable<EnergySource>?        EnergySources                      = null,
                              EnvironmentalImpact?              EnvironmentalImpact                = null,
                              UInt32?                           MaxCapacity                        = null,
                              AccessibilityLocationTypes?       AccessibilityLocationType          = null,
                              I18NText?                         AdditionalInfo                     = null,
                              I18NText?                         ChargingStationLocationReference   = null,
                              GeoCoordinates?                   GeoChargingPointEntrance           = null,
                              IEnumerable<OpeningTime>?         OpeningTimes                       = null,
                              Operator_Id?                      HubOperatorId                      = null,
                              ClearingHouse_Id?                 ClearingHouseId                    = null,

                              JObject?                          CustomData                         = null,
                              UserDefinedDictionary?            InternalData                       = null)

            : base(CustomData,
                   InternalData)

        {

            #region Initial checks

            if (ChargingStationName.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(ChargingStationName),  "The given charging station name must not be null or empty!");

            if (Address is null)
                throw new ArgumentNullException(nameof(Address),              "The given address must not be null!");

            if (PlugTypes.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(PlugTypes),            "The given enumeration of plug types must not be null or empty!");

            if (ChargingFacilities.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(ChargingFacilities),   "The given enumeration of charging facilities must not be null or empty!");

            if (AuthenticationModes.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(AuthenticationModes),  "The given enumeration of authentication modes must not be null or empty!");

            if (ValueAddedServices.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(ValueAddedServices),   "The given enumeration of value added services must not be null or empty!");

            #endregion

            this.Id                                = Id;
            this.OperatorId                        = OperatorId;
            this.OperatorName                      = OperatorName;
            this.ChargingStationName               = ChargingStationName;
            this.Address                           = Address;
            this.GeoCoordinates                    = GeoCoordinates;
            this.PlugTypes                         = PlugTypes.          Distinct(); // mandatory!
            this.ChargingFacilities                = ChargingFacilities. Distinct(); // mandatory!
            this.RenewableEnergy                   = RenewableEnergy;
            this.CalibrationLawDataAvailability    = CalibrationLawDataAvailability;
            this.AuthenticationModes               = AuthenticationModes.Distinct(); // mandatory!
            this.PaymentOptions                    = PaymentOptions.     Distinct(); // mandatory!
            this.ValueAddedServices                = ValueAddedServices. Distinct(); // mandatory!
            this.Accessibility                     = Accessibility;
            this.HotlinePhoneNumber                = HotlinePhoneNumber;
            this.IsOpen24Hours                     = IsOpen24Hours;
            this.IsHubjectCompatible               = IsHubjectCompatible;
            this.DynamicInfoAvailable              = DynamicInfoAvailable;

            this.DeltaType                         = DeltaType;
            this.LastUpdate                        = LastUpdate;

            this.ChargingStationId                 = ChargingStationId;
            this.ChargingPoolId                    = ChargingPoolId;
            this.HardwareManufacturer              = HardwareManufacturer;
            this.ChargingStationImageURL           = ChargingStationImageURL;
            this.SubOperatorName                   = SubOperatorName;
            this.DynamicPowerLevel                 = DynamicPowerLevel;
            this.EnergySources                     = EnergySources?.      Distinct() ?? Array.Empty<EnergySource>();
            this.EnvironmentalImpact               = EnvironmentalImpact;
            this.MaxCapacity                       = MaxCapacity;
            this.AccessibilityLocationType         = AccessibilityLocationType;
            this.AdditionalInfo                    = AdditionalInfo;
            this.ChargingStationLocationReference  = ChargingStationLocationReference;
            this.GeoChargingPointEntrance          = GeoChargingPointEntrance;
            this.OpeningTimes                      = OpeningTimes;
            this.HubOperatorId                     = HubOperatorId;
            this.ClearingHouseId                   = ClearingHouseId;

        }

        #endregion


        #region Documentation

        // {
        //   "Accessibility":           "Unspecified",
        //   "AccessibilityLocation":   "OnStreet",
        //   "AdditionalInfo": [
        //     {
        //       "lang":                "string",
        //       "value":               "string"
        //     }
        //   ],
        //   "Address": {
        //     "City":                  "string",
        //     "Country":               "string",
        //     "Floor":                 "string",
        //     "HouseNum":              "string",
        //     "ParkingFacility":        false,
        //     "ParkingSpot":           "string",
        //     "PostalCode":            "string",
        //     "Region":                "string",
        //     "Street":                "string",
        //     "TimeZone":              "string"
        //   },
        //   "AuthenticationModes": [
        //     "NFC RFID Classic"
        //   ],
        //   "CalibrationLawDataAvailability":  "Local",
        //   "ChargingFacilities": [
        //     {
        //       "Amperage": 0,
        //       "ChargingModes": [
        //         "Mode_1"
        //       ],
        //       "PowerType": "AC_1_PHASE",
        //       "Voltage": 0,
        //       "Power": 0
        //     }
        //   ],
        //   "ChargingPoolID": "string",
        //   "ChargingStationID": "string",
        //   "ChargingStationImage": "string",
        //   "ChargingStationLocationReference": [
        //     {
        //       "lang": "string",
        //       "value": "string"
        //     }
        //   ],
        //   "ChargingStationNames": [
        //     {
        //       "lang": "string",
        //       "value": "string"
        //     }
        //   ],
        //   "ClearinghouseID": "string",
        //   "DynamicInfoAvailable": "true",
        //   "DynamicPowerLevel": true,
        //   "EnergySource": [
        //     {
        //       "Energy": "Solar",
        //       "Percentage": 0
        //     }
        //   ],
        //   "EnvironmentalImpact": {
        //     "CO2Emission": 0,
        //     "NuclearWaste": 0
        //   },
        //   "EvseID": "string",
        //   "GeoChargingPointEntrance": {
        //     "Google": {
        //       "Coordinates": "string"
        //     },
        //     "DecimalDegree": {
        //       "Latitude": "string",
        //       "Longitude": "string"
        //     },
        //     "DegreeMinuteSeconds": {
        //       "Latitude": "string",
        //       "Longitude": "string"
        //     }
        //   },
        //   "GeoCoordinates": {
        //     "Google": {
        //       "Coordinates": "string"
        //     },
        //     "DecimalDegree": {
        //       "Latitude": "string",
        //       "Longitude": "string"
        //     },
        //     "DegreeMinuteSeconds": {
        //       "Latitude": "string",
        //       "Longitude": "string"
        //     }
        //   },
        //   "HardwareManufacturer": "string",
        //   "HotlinePhoneNumber": "string",
        //   "HubOperatorID": "string",
        //   "IsHubjectCompatible": false,
        //   "IsOpen24Hours": false,
        //   "MaxCapacity": 0,
        //   "OpeningTimes": [
        //     {
        //       "Period": [
        //         {
        //           "begin": "string",
        //           "end": "string"
        //         }
        //       ],
        //       "on": "Everyday",
        //       "unstructuredOpeningTime": "string"
        //     }
        //   ],
        //   "OperatorID": "string",
        //   "OperatorName": "string",
        //   "PaymentOptions": [
        //     "No Payment"
        //   ],
        //   "Plugs": [
        //     "Small Paddle Inductive"
        //   ],
        //   "RenewableEnergy": true,
        //   "SubOperatorName": "string",
        //   "ValueAddedServices": [
        //     "Reservation"
        //   ],
        //   "deltaType": "insert",
        //   "lastUpdate": "2020-12-20T23:39:33.562Z"
        // }

        #endregion

        #region (static) Parse   (JSON, CustomEVSEDataRecordParser = null)

        /// <summary>
        /// Parse the given JSON representation of an EVSE data record.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSE data records JSON objects.</param>
        public static EVSEDataRecord Parse(JObject                                       JSON,
                                           CustomJObjectParserDelegate<EVSEDataRecord>?  CustomEVSEDataRecordParser   = null)
        {

            if (TryParse(JSON,
                         out EVSEDataRecord?  evseDataRecord,
                         out String?          errorResponse,
                         CustomEVSEDataRecordParser))
            {
                return evseDataRecord!;
            }

            throw new ArgumentException("The given JSON representation of an EVSE data record is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out EVSEDataRecord, out ErrorResponse, CustomEVSEDataRecordParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of an EVSE data record.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="EVSEDataRecord">The parsed EVSE data record.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject              JSON,
                                       out EVSEDataRecord?  EVSEDataRecord,
                                       out String?          ErrorResponse)

            => TryParse(JSON,
                        out EVSEDataRecord,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of an EVSE data record.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="EVSEDataRecord">The parsed EVSE data record.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSE data records JSON objects.</param>
        public static Boolean TryParse(JObject                                       JSON,
                                       out EVSEDataRecord?                           EVSEDataRecord,
                                       out String?                                   ErrorResponse,
                                       CustomJObjectParserDelegate<EVSEDataRecord>?  CustomEVSEDataRecordParser)
        {

            try
            {

                EVSEDataRecord = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse EVSEId                            [mandatory]

                if (!JSON.ParseMandatory("EvseID",
                                         "EVSE identification",
                                         EVSE_Id.TryParse,
                                         out EVSE_Id EVSEId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse OperatorId                        [mandatory]

                if (!JSON.ParseMandatory("OperatorID",
                                         "operator identification",
                                         Operator_Id.TryParse,
                                         out Operator_Id OperatorId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse OperatorName                      [mandatory]

                if (!JSON.ParseMandatoryText("OperatorName",
                                             "operator name",
                                             out String OperatorName,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse ChargingStationName               [mandatory => optional, because of Hubject data quality issues!]

                if (!JSON.ParseMandatoryJSONArray2("ChargingStationNames",
                                                   "multi-language charging station name",
                                                   I18NText.TryParse,
                                                   out I18NText ChargingStationName,
                                                   out ErrorResponse))
                {

                    ChargingStationName = new I18NText(LanguageCode.en, "Unnamed station");

                    //return false;

                }

                #endregion

                #region Parse Address                           [mandatory]

                if (!JSON.ParseMandatoryJSON2("Address",
                                              "address",
                                              OICPv2_3.Address.TryParse,
                                              out Address? Address,
                                              out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse GeoCoordinates                    [mandatory]

                if (!JSON.ParseMandatoryJSON2("GeoCoordinates",
                                              "geo coordinates",
                                              OICPv2_3.GeoCoordinates.TryParse,
                                              out GeoCoordinates? geoCoordinates,
                                              out ErrorResponse))
                {
                    return false;
                }

                var GeoCoordinates = geoCoordinates!.Value;

                #endregion

                #region Parse PlugTypes                         [mandatory]

                if (!JSON.ParseMandatory("Plugs",
                                         "plug types",
                                         PlugTypesExtensions.TryParse,
                                         out IEnumerable<PlugTypes> PlugTypes,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse ChargingFacilities                [mandatory => optional, because of Hubject data quality issues!]

                if (!JSON.ParseMandatoryJSON("ChargingFacilities",
                                             "charging facilities",
                                             ChargingFacility.TryParse,
                                             out IEnumerable<ChargingFacility> ChargingFacilities,
                                             out ErrorResponse))
                {

                    ChargingFacilities = new ChargingFacility[] {
                                             new ChargingFacility(
                                                 PowerTypes.Unspecified,
                                                 0
                                             )
                                         };

                    //return false;

                }

                #endregion

                #region Parse RenewableEnergy                   [mandatory => optional, because of Hubject data quality issues!]

                if (!JSON.ParseMandatory("RenewableEnergy",
                                         "renewable energy",
                                         out Boolean RenewableEnergy,
                                         out ErrorResponse))
                {

                    RenewableEnergy = false;

                    //return false;

                }

                #endregion

                #region Parse CalibrationLawDataAvailability    [mandatory => optional, because of Hubject data quality issues!]

                if (!JSON.ParseMandatory("CalibrationLawDataAvailability",
                                         "calibration law data availability",
                                         CalibrationLawDataAvailabilitiesExtensions.TryParse,
                                         out CalibrationLawDataAvailabilities CalibrationLawDataAvailability,
                                         out ErrorResponse))
                {

                    CalibrationLawDataAvailability = CalibrationLawDataAvailabilities.NotAvailable;

                    //return false;

                }

                #endregion

                #region Parse AuthenticationModes               [mandatory]

                if (!JSON.ParseMandatory("AuthenticationModes",
                                         "address",
                                         AuthenticationModesExtensions.TryParse,
                                         out IEnumerable<AuthenticationModes> AuthenticationModes,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse PaymentOptions                    [mandatory => optional, because of Hubject data quality issues!]

                if (!JSON.ParseMandatory("PaymentOptions",
                                         "payment options",
                                         PaymentOptionsExtensions.TryParse,
                                         out IEnumerable<PaymentOptions> PaymentOptions,
                                         out ErrorResponse))
                {

                    PaymentOptions = new PaymentOptions[] {
                                         OICPv2_3.PaymentOptions.Contract
                                     };

                    //return false;

                }

                #endregion

                #region Parse ValueAddedServices                [mandatory]

                if (!JSON.ParseMandatory("ValueAddedServices",
                                         "value added services",
                                         ValueAddedServicesExtensions.TryParse,
                                         out IEnumerable<ValueAddedServices> ValueAddedServices,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Accessibility                     [mandatory => optional, because of Hubject data quality issues!]

                if (!JSON.ParseMandatory("Accessibility",
                                         "accessibility",
                                         AccessibilityTypesExtensions.TryParse,
                                         out AccessibilityTypes Accessibility,
                                         out ErrorResponse))
                {

                    Accessibility = AccessibilityTypes.Unspecified;

                    //return false;

                }

                #endregion

                #region Parse HotlinePhoneNumber                [mandatory => optional, because of Hubject data quality issues!]

                if (JSON.ParseOptional("HotlinePhoneNumber",
                                       "hotline phone number",
                                       Phone_Number.TryParse,
                                       out Phone_Number? HotlinePhoneNumber,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse IsOpen24Hours                     [mandatory]

                if (!JSON.ParseMandatory("IsOpen24Hours",
                                         "is open 24 hours",
                                         out Boolean IsOpen24Hours,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse IsHubjectCompatible               [mandatory]

                if (!JSON.ParseMandatory("IsHubjectCompatible",
                                         "is hubject compatible",
                                         out Boolean IsHubjectCompatible,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse DynamicInfoAvailable              [mandatory]

                if (!JSON.ParseMandatory("DynamicInfoAvailable",
                                         "dynamic info available",
                                         FalseTrueAutoExtensions.TryParse,
                                         out FalseTrueAuto DynamicInfoAvailable,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion


                #region Parse DeltaType                         [optional]

                if (JSON.ParseOptionalStruct("deltaType",
                                             "delta type",
                                             DeltaTypesExtensions.TryParse,
                                             out DeltaTypes? DeltaType,
                                             out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse LastUpdate                        [optional]

                if (JSON.ParseOptional("lastUpdate",
                                       "last update",
                                       out DateTime? LastUpdate,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                #region Parse ChargingStationId                 [optional]

                if (JSON.ParseOptionalStruct("ChargingStationID",
                                             "charging station identification",
                                             ChargingStation_Id.TryParse,
                                             out ChargingStation_Id? ChargingStationId,
                                             out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                    {

                        if (JSON["ChargingStationID"]?.Value<String>() == String.Empty)
                        {
                            // Allow "", because of Hubject data quality issues!
                        }

                        else
                            return false;

                    }
                }

                #endregion

                #region Parse ChargingPoolId                    [optional]

                if (JSON.ParseOptionalStruct("ChargingPoolID",
                                             "charging pool identification",
                                             ChargingPool_Id.TryParse,
                                             out ChargingPool_Id? ChargingPoolId,
                                             out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                    {

                        if (JSON["ChargingPoolID"]?.Value<String>() == String.Empty)
                        {
                            // Allow "", because of Hubject data quality issues!
                        }

                        else
                            return false;

                    }
                }

                #endregion

                #region HardwareManufacturer                    [optional]

                var hardwareManufacturer = JSON[nameof(HardwareManufacturer)]?.Value<String>();

                #endregion

                #region Parse ChargingStationImageURL           [optional]

                if (JSON.ParseOptionalStruct("ChargingStationImage",
                                             "charging station image URL",
                                             URL.TryParse,
                                             out URL? ChargingStationImageURL,
                                             out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region SubOperatorName                         [optional]

                var subOperatorName = JSON[nameof(SubOperatorName)]?.Value<String>();

                #endregion

                #region Parse DynamicPowerLevel                 [optional]

                if (JSON.ParseOptional("DynamicPowerLevel",
                                       "dynamic power level",
                                       out Boolean? DynamicPowerLevel,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse EnergySources                     [optional]

                if (JSON.ParseOptionalJSON("EnergySource",
                                           "delta type",
                                           EnergySource.TryParse,
                                           out IEnumerable<EnergySource> EnergySources,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse EnvironmentalImpact               [optional]

                if (JSON.ParseOptionalJSON("EnvironmentalImpact",
                                           "environmental impact",
                                           OICPv2_3.EnvironmentalImpact.TryParse,
                                           out EnvironmentalImpact? EnvironmentalImpact,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse MaxCapacity                       [optional]

                if (JSON.ParseOptional("MaxCapacity",
                                       "max capacity",
                                       out UInt32? MaxCapacity,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse AccessibilityLocationType         [optional]

                if (JSON.ParseOptionalStruct("AccessibilityLocation",
                                             "accessibility location",
                                             AccessibilityLocationTypesExtensions.TryParse,
                                             out AccessibilityLocationTypes? AccessibilityLocationType,
                                             out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse AdditionalInfo                    [optional]

                if (JSON.ParseOptionalJSONArray("AdditionalInfo",
                                                "additional info",
                                                I18NText.TryParse,
                                                out I18NText AdditionalInfo,
                                                out ErrorResponse))
                {
                    // Allow "[]", because of Hubject data quality issues!
                    if (ErrorResponse is not null &&
                        JSON[nameof(AdditionalInfo)] is JArray array &&
                        array.Count == 0)
                    {
                        AdditionalInfo = I18NText.Empty;
                    }
                }

                #endregion

                #region Parse ChargingStationLocationReference  [optional]

                if (JSON.ParseOptionalJSONArray("ChargingStationLocationReference",
                                                "charging station location reference",
                                                I18NText.TryParse,
                                                out I18NText ChargingStationLocationReference,
                                                out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse GeoChargingPointEntrance          [optional]

                if (JSON.ParseOptionalJSON("GeoChargingPointEntrance",
                                           "geo charging point entrance",
                                           OICPv2_3.GeoCoordinates.TryParse,
                                           out GeoCoordinates? GeoChargingPointEntrance,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse OpeningTimes                      [optional]

                if (JSON.ParseOptionalJSON("OpeningTimes",
                                           "opening times",
                                           OpeningTime.TryParse,
                                           out IEnumerable<OpeningTime> OpeningTimes,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse HubOperatorId                     [optional]

                if (JSON.ParseOptionalStruct("HubOperatorID",
                                             "hub operator identification",
                                             Operator_Id.TryParse,
                                             out Operator_Id? HubOperatorId,
                                             out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse ClearingHouseId                   [optional]

                if (JSON.ParseOptionalStruct("ClearingHouseID",
                                             "clearing house identification",
                                             ClearingHouse_Id.TryParse,
                                             out ClearingHouse_Id? ClearingHouseId,
                                             out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                #region Parse CustomData                        [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                EVSEDataRecord = new EVSEDataRecord(EVSEId,
                                                    OperatorId,
                                                    OperatorName,
                                                    ChargingStationName,
                                                    Address!,
                                                    GeoCoordinates,
                                                    PlugTypes,
                                                    ChargingFacilities,
                                                    RenewableEnergy,
                                                    CalibrationLawDataAvailability,
                                                    AuthenticationModes,
                                                    PaymentOptions,
                                                    ValueAddedServices,
                                                    Accessibility,
                                                    HotlinePhoneNumber,
                                                    IsOpen24Hours,
                                                    IsHubjectCompatible,
                                                    DynamicInfoAvailable,

                                                    DeltaType,
                                                    LastUpdate,

                                                    ChargingStationId,
                                                    ChargingPoolId,
                                                    hardwareManufacturer,
                                                    ChargingStationImageURL,
                                                    subOperatorName,
                                                    DynamicPowerLevel,
                                                    EnergySources,
                                                    EnvironmentalImpact,
                                                    MaxCapacity,
                                                    AccessibilityLocationType,
                                                    AdditionalInfo,
                                                    ChargingStationLocationReference,
                                                    GeoChargingPointEntrance,
                                                    OpeningTimes,
                                                    HubOperatorId,
                                                    ClearingHouseId,

                                                    customData);


                if (CustomEVSEDataRecordParser is not null)
                    EVSEDataRecord = CustomEVSEDataRecordParser(JSON,
                                                                EVSEDataRecord);

                return true;

            }
            catch (Exception e)
            {
                EVSEDataRecord  = default;
                ErrorResponse   = "The given JSON representation of an EVSE data record is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomEVSEDataRecordSerializer = null, CustomAddressSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomEVSEDataRecordSerializer">A delegate to serialize custom EVSE data record JSON objects.</param>
        /// <param name="CustomAddressSerializer">A delegate to serialize custom address JSON objects.</param>
        /// <param name="CustomChargingFacilitySerializer">A delegate to serialize custom charging facility JSON objects.</param>
        /// <param name="CustomGeoCoordinatesSerializer">A delegate to serialize custom geo coordinates JSON objects.</param>
        /// <param name="CustomEnergySourceSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomEnvironmentalImpactSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomOpeningTimesSerializer">A delegate to serialize custom opening time JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<EVSEDataRecord>?       CustomEVSEDataRecordSerializer        = null,
                              CustomJObjectSerializerDelegate<Address>?              CustomAddressSerializer               = null,
                              CustomJObjectSerializerDelegate<ChargingFacility>?     CustomChargingFacilitySerializer      = null,
                              CustomJObjectSerializerDelegate<GeoCoordinates>?       CustomGeoCoordinatesSerializer        = null,
                              CustomJObjectSerializerDelegate<EnergySource>?         CustomEnergySourceSerializer          = null,
                              CustomJObjectSerializerDelegate<EnvironmentalImpact>?  CustomEnvironmentalImpactSerializer   = null,
                              CustomJObjectSerializerDelegate<OpeningTime>?          CustomOpeningTimesSerializer          = null)
        {

            var JSON = JSONObject.Create(

                           new JProperty("EvseID",                                  Id.                                ToString()),
                           new JProperty("ChargingStationNames",                    ChargingStationName.               ToJSON()),
                           new JProperty("Address",                                 Address.                           ToJSON(CustomAddressSerializer)),
                           new JProperty("GeoCoordinates",                          GeoCoordinates.                    ToJSON(CustomGeoCoordinatesSerializer)),
                           new JProperty("Plugs",                                   new JArray(PlugTypes.          SafeSelect(plugType           => plugType.          AsString()))),
                           new JProperty("ChargingFacilities",                      new JArray(ChargingFacilities. SafeSelect(chargingFacility   => chargingFacility.  ToJSON(CustomChargingFacilitySerializer)))),
                           new JProperty("RenewableEnergy",                         RenewableEnergy),
                           new JProperty("CalibrationLawDataAvailability",          CalibrationLawDataAvailability.    AsString()),
                           new JProperty("AuthenticationModes",                     new JArray(AuthenticationModes.SafeSelect(authenticationMode => authenticationMode.AsString()))),
                           new JProperty("PaymentOptions",                          new JArray(PaymentOptions.     SafeSelect(paymentOption      => paymentOption.     AsString()))),
                           new JProperty("ValueAddedServices",                      new JArray(ValueAddedServices. SafeSelect(valueAddedService  => valueAddedService. AsString()))),
                           new JProperty("Accessibility",                           Accessibility.                     AsString()),
                           new JProperty("HotlinePhoneNumber",                      HotlinePhoneNumber?.               ToString()),
                           new JProperty("IsOpen24Hours",                           IsOpen24Hours),
                           new JProperty("IsHubjectCompatible",                     IsHubjectCompatible),
                           new JProperty("DynamicInfoAvailable",                    DynamicInfoAvailable.              AsString()),
                           new JProperty("OperatorID",                              OperatorId.                        ToString()),
                           new JProperty("OperatorName",                            OperatorName),

                           DeltaType.                       HasValue
                               ? new JProperty("deltaType",                         DeltaType.                   Value.ToString())
                               : null,

                           LastUpdate.                      HasValue
                               ? new JProperty("lastUpdate",                        LastUpdate.                  Value.ToIso8601())
                               : null,


                           ChargingStationId.               HasValue
                               ? new JProperty("ChargingStationID",                 ChargingStationId.           Value.ToString())
                               : null,

                           ChargingPoolId.                  HasValue
                               ? new JProperty("ChargingPoolID",                    ChargingPoolId.              Value.ToString())
                               : null,

                           HardwareManufacturer             is not null && HardwareManufacturer.IsNeitherNullNorEmpty()
                               ? new JProperty("HardwareManufacturer",              HardwareManufacturer)
                               : null,

                           ChargingStationImageURL.         HasValue
                               ? new JProperty("ChargingStationImage",              ChargingStationImageURL.     Value.ToString())
                               : null,

                           SubOperatorName                  is not null && SubOperatorName.IsNeitherNullNorEmpty()
                               ? new JProperty("SubOperatorName",                   SubOperatorName)
                               : null,

                           DynamicPowerLevel.               HasValue
                               ? new JProperty("DynamicPowerLevel",                 DynamicPowerLevel.           Value)
                               : null,

                           EnergySources                    is not null && EnergySources.Any()
                               ? new JProperty("EnergySource",                      new JArray(EnergySources.Select(energySource => energySource.ToJSON(CustomEnergySourceSerializer))))
                               : null,

                           EnvironmentalImpact.             HasValue
                               ? new JProperty("EnvironmentalImpact",               EnvironmentalImpact.         Value.ToJSON(CustomEnvironmentalImpactSerializer))
                               : null,

                           MaxCapacity.                     HasValue
                               ? new JProperty("MaxCapacity",                       MaxCapacity.                 Value)
                               : null,

                           AccessibilityLocationType.       HasValue
                               ? new JProperty("AccessibilityLocation",             AccessibilityLocationType.   Value.AsString())
                               : null,

                           AdditionalInfo                   is not null && AdditionalInfo.Any()
                               ? new JProperty("AdditionalInfo",                    AdditionalInfo.                    ToJSON())
                               : null,

                           ChargingStationLocationReference is not null && ChargingStationLocationReference.Any()
                               ? new JProperty("ChargingStationLocationReference",  ChargingStationLocationReference.  ToJSON())
                               : null,

                           GeoChargingPointEntrance.        HasValue
                               ? new JProperty("GeoChargingPointEntrance",          GeoChargingPointEntrance.    Value.ToJSON(CustomGeoCoordinatesSerializer))
                               : null,

                           !IsOpen24Hours && OpeningTimes is not null && OpeningTimes.Any()
                               ? new JProperty("OpeningTimes",                      new JArray(OpeningTimes.Select(openingTime => openingTime.ToJSON(CustomOpeningTimesSerializer))))
                               : null,

                           HubOperatorId.                   HasValue
                               ? new JProperty("HubOperatorID",                     HubOperatorId.               Value.ToString())
                               : null,

                           ClearingHouseId.                 HasValue
                               ? new JProperty("ClearinghouseID",                   ClearingHouseId.             Value.ToString())
                               : null,

                           CustomData?.HasValues == true
                               ? new JProperty("CustomData",                        CustomData)
                               : null

                       );

            return CustomEVSEDataRecordSerializer is not null
                       ? CustomEVSEDataRecordSerializer(this, JSON)
                       : JSON;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this object.
        /// </summary>
        public EVSEDataRecord Clone

            => new (Id.                               Clone,
                    OperatorId.                       Clone,
                    new String(OperatorName.ToCharArray()),
                    ChargingStationName.              Clone,
                    Address.                          Clone,
                    GeoCoordinates.                   Clone,
                    PlugTypes.                        ToArray(),
                    ChargingFacilities.               ToArray(),
                    RenewableEnergy,
                    CalibrationLawDataAvailability,
                    AuthenticationModes.              ToArray(),
                    PaymentOptions.                   ToArray(),
                    ValueAddedServices.               ToArray(),
                    Accessibility,
                    HotlinePhoneNumber?.              Clone,
                    IsOpen24Hours,
                    IsHubjectCompatible,
                    DynamicInfoAvailable,

                    DeltaType,
                    LastUpdate,

                    ChargingStationId?.               Clone,
                    ChargingPoolId?.                  Clone,
                    HardwareManufacturer is not null
                        ? new String(HardwareManufacturer.ToCharArray())
                        : null,
                    ChargingStationImageURL?.         Clone,
                    SubOperatorName      is not null
                        ? new String(SubOperatorName.     ToCharArray())
                        : null,
                    DynamicPowerLevel,
                    EnergySources        is not null
                        ? EnergySources.SafeSelect(enerygSource => enerygSource.Clone).ToArray()
                        : null,
                    EnvironmentalImpact?.             Clone,
                    MaxCapacity,
                    AccessibilityLocationType,
                    AdditionalInfo?.                  Clone,
                    ChargingStationLocationReference?.Clone,
                    GeoChargingPointEntrance?.        Clone,
                    OpeningTimes         is not null
                        ? OpeningTimes. SafeSelect(openingTime  => openingTime. Clone).ToArray()
                        : null,
                    HubOperatorId?.                   Clone,
                    ClearingHouseId?.                 Clone,

                    CustomData           is not null
                        ? JObject.Parse(CustomData.ToString(Newtonsoft.Json.Formatting.None))
                        : null);

        #endregion


        #region Operator overloading

        #region Operator == (EVSEDataRecord1, EVSEDataRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEDataRecord1">An EVSE data record.</param>
        /// <param name="EVSEDataRecord2">Another EVSE data record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSEDataRecord EVSEDataRecord1,
                                           EVSEDataRecord EVSEDataRecord2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(EVSEDataRecord1, EVSEDataRecord2))
                return true;

            // If one is null, but not both, return false.
            if (EVSEDataRecord1 is null || EVSEDataRecord2 is null)
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
        public static Boolean operator != (EVSEDataRecord EVSEDataRecord1,
                                           EVSEDataRecord EVSEDataRecord2)

            => !(EVSEDataRecord1 == EVSEDataRecord2);

        #endregion

        #region Operator <  (EVSEDataRecord1, EVSEDataRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEDataRecord1">An EVSE data record.</param>
        /// <param name="EVSEDataRecord2">Another EVSE data record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSEDataRecord EVSEDataRecord1,
                                          EVSEDataRecord EVSEDataRecord2)
        {

            if (EVSEDataRecord1 is null)
                throw new ArgumentNullException(nameof(EVSEDataRecord1), "The given EVSE data record must not be null!");

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
        public static Boolean operator <= (EVSEDataRecord EVSEDataRecord1,
                                           EVSEDataRecord EVSEDataRecord2)

            => !(EVSEDataRecord1 > EVSEDataRecord2);

        #endregion

        #region Operator >  (EVSEDataRecord1, EVSEDataRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEDataRecord1">An EVSE data record.</param>
        /// <param name="EVSEDataRecord2">Another EVSE data record.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSEDataRecord EVSEDataRecord1,
                                          EVSEDataRecord EVSEDataRecord2)
        {

            if (EVSEDataRecord1 is null)
                throw new ArgumentNullException(nameof(EVSEDataRecord1), "The given EVSE data record must not be null!");

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
        public static Boolean operator >= (EVSEDataRecord EVSEDataRecord1,
                                           EVSEDataRecord EVSEDataRecord2)

            => !(EVSEDataRecord1 < EVSEDataRecord2);

        #endregion

        #endregion

        #region IComparable<EVSEDataRecord> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EVSEDataRecord evseDataRecord
                   ? CompareTo(evseDataRecord)
                   : throw new ArgumentException("The given object is not an EVSE data record!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EVSEDataRecord)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEDataRecord">An object to compare with.</param>
        public Int32 CompareTo(EVSEDataRecord? EVSEDataRecord)
        {

            if (EVSEDataRecord is null)
                throw new ArgumentNullException(nameof(EVSEDataRecord),
                                                "The given EVSE data record must not be null!");

            return Id.CompareTo(EVSEDataRecord.Id);

            //ToDo: Compare more properties!

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
        public override Boolean Equals(Object? Object)

            => Object is EVSEDataRecord evseDataRecord &&
                   Equals(evseDataRecord);

        #endregion

        #region Equals(EVSEDataRecord)

        /// <summary>
        /// Compares two EVSE data records for equality.
        /// </summary>
        /// <param name="EVSEDataRecord">An EVSE data record to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSEDataRecord? EVSEDataRecord)

            => EVSEDataRecord is not null &&
                   Id.Equals(EVSEDataRecord.Id);

        //ToDo: Compare more properties!

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()
            => Id.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()
            => Id.ToString();

        #endregion


        #region ToBuilder(NewEVSEId = null)

        /// <summary>
        /// Return an EVSE data record builder.
        /// </summary>
        /// <param name="NewEVSEId">An optional new EVSE identification.</param>
        public Builder ToBuilder(EVSE_Id? NewEVSEId = null)

            => new (NewEVSEId ?? Id,
                    OperatorId,
                    OperatorName,
                    ChargingStationName,
                    Address,
                    GeoCoordinates,
                    PlugTypes,
                    ChargingFacilities,
                    RenewableEnergy,
                    CalibrationLawDataAvailability,
                    AuthenticationModes,
                    PaymentOptions,
                    ValueAddedServices,
                    Accessibility,
                    HotlinePhoneNumber,
                    IsOpen24Hours,
                    IsHubjectCompatible,
                    DynamicInfoAvailable,

                    DeltaType,
                    LastUpdate,

                    ChargingStationId,
                    ChargingPoolId,
                    HardwareManufacturer,
                    ChargingStationImageURL,
                    SubOperatorName,
                    DynamicPowerLevel,
                    EnergySources,
                    EnvironmentalImpact,
                    MaxCapacity,
                    AccessibilityLocationType,
                    AdditionalInfo,
                    ChargingStationLocationReference,
                    GeoChargingPointEntrance,
                    OpeningTimes,
                    HubOperatorId,
                    ClearingHouseId,

                    CustomData,
                    InternalData);

        #endregion

        #region (class) Builder

        /// <summary>
        /// An Electric Vehicle Supply Equipment (EVSE) data record builder.
        /// This is meant to be one electrical circuit which can charge a electric vehicle.
        /// </summary>
        public new class Builder : AInternalData.Builder
        {

            #region Properties

            /// <summary>
            /// The unique identification of the Electric Vehicle Supply Equipment (EVSE).
            /// </summary>
            [Mandatory]
            public EVSE_Id?                           Id                                  { get; set; }


            /// <summary>
            /// The delta type when the EVSE data record was just downloaded.
            /// </summary>
            [Optional]
            public DeltaTypes?                        DeltaType                           { get; set; }

            /// <summary>
            /// The last update timestamp of the EVSE data record.
            /// </summary>
            [Optional]
            public DateTime?                          LastUpdate                          { get; set; }


            /// <summary>
            /// The unique identification of the operator of the EVSE.
            /// </summary>
            [Mandatory]
            public Operator_Id?                       OperatorId                          { get; set; }

            /// <summary>
            /// The name of the operator of the EVSE.
            /// </summary>
            [Mandatory]
            public String?                            OperatorName                        { get; set; }

            /// <summary>
            /// The identification of the charging station hosting the EVSE.
            /// </summary>
            [Optional]
            public ChargingStation_Id?                ChargingStationId                   { get; set; }

            /// <summary>
            /// The identification of the charging pool hosting the EVSE.
            /// </summary>
            [Optional]
            public ChargingPool_Id?                   ChargingPoolId                      { get; set; }

            /// <summary>
            /// The multi-language name of the charging station hosting the EVSE.
            /// </summary>
            [Mandatory]
            public I18NText?                          ChargingStationName                 { get; set; }

            /// <summary>
            /// Optional name of the EVSE manufacturer.
            /// </summary>
            [Optional]
            public String?                            HardwareManufacturer                { get; set; }

            /// <summary>
            /// Optional URL to an image of the EVSE.
            /// </summary>
            [Optional]
            public URL?                               ChargingStationImageURL             { get; set; }

            /// <summary>
            /// Optional name of the sub operator owning the EVSE.
            /// </summary>
            [Optional]
            public String?                            SubOperatorName                     { get; set; }

            /// <summary>
            /// Whether the EVSE is able to deliver different power outputs.
            /// </summary>
            [Optional]
            public Boolean?                           DynamicPowerLevel                   { get; set; }

            /// <summary>
            /// The address of the EVSE.
            /// </summary>
            [Mandatory]
            public Address?                           Address                             { get; set; }

            /// <summary>
            /// The geo coordinate of the EVSE.
            /// </summary>
            [Mandatory]
            public GeoCoordinates?                    GeoCoordinates                      { get; set; }

            /// <summary>
            /// The types of charging plugs attached to the EVSE.
            /// </summary>
            [Mandatory]
            public HashSet<PlugTypes>                 PlugTypes                           { get; }

            /// <summary>
            /// An enumeration of supported charging facilities at the EVSE.
            /// </summary>
            [Mandatory]
            public HashSet<ChargingFacility>          ChargingFacilities                  { get; }


            /// <summary>
            /// This field gives the information how the charging station provides metering law data.
            /// </summary>
            [Mandatory]
            public CalibrationLawDataAvailabilities?  CalibrationLawDataAvailability      { get; set; }

            /// <summary>
            /// The authentication modes the EVSE supports.
            /// </summary>
            [Mandatory]
            public HashSet<AuthenticationModes>       AuthenticationModes                 { get; }

            /// <summary>
            /// If the EVSE provides only renewable energy then the value MUST be” true”,
            /// if it use grey energy then value MUST be “false”.
            /// </summary>
            [Mandatory]
            public Boolean?                           RenewableEnergy                     { get; set; }

            /// <summary>
            /// Optional enumeration of energy sources that the EVSE uses to supply electric energy.
            /// </summary>
            public HashSet<EnergySource>              EnergySources                       { get; }

            /// <summary>
            /// Optional environmental impact produced by the energy sources used by the EVSE.
            /// </summary>
            [Optional]
            public EnvironmentalImpact?               EnvironmentalImpact                 { get; set; }

            /// <summary>
            /// The maximum in kWh capacity the EVSE provides.
            /// </summary>
            [Optional]
            public UInt32?                            MaxCapacity                         { get; set; }

            /// <summary>
            /// An enumeration of payment options that are supported.
            /// </summary>
            [Mandatory]
            public HashSet<PaymentOptions>            PaymentOptions                      { get; }

            /// <summary>
            /// An enumeration of "value added services" the EVSE supports.
            /// </summary>
            [Mandatory]
            public HashSet<ValueAddedServices>        ValueAddedServices                  { get; }

            /// <summary>
            /// Specifies how the charging station can be accessed.
            /// </summary>
            [Mandatory]
            public AccessibilityTypes?                Accessibility                       { get; set; }

            /// <summary>
            /// Optional information where the EVSE could be accessed.
            /// </summary>
            [Optional]
            public AccessibilityLocationTypes?        AccessibilityLocationType           { get; set; }

            /// <summary>
            /// The phone number of the charging station operator's hotline.
            /// </summary>
            [Mandatory]
            public Phone_Number?                      HotlinePhoneNumber                  { get; set; }

            /// <summary>
            /// Optional multi-language information about the EVSE.
            /// </summary>
            [Optional]
            public I18NText?                          AdditionalInfo                      { get; set; }

            /// <summary>
            /// Optional last meters information regarding the location of the EVSE.
            /// </summary>
            [Optional]
            public I18NText?                          ChargingStationLocationReference    { get; set; }

            /// <summary>
            /// In case that the EVSE is part of a bigger facility (e.g. parking place),
            /// this optional attribute specifies the facilities entrance coordinates.
            /// </summary>
            [Optional]
            public GeoCoordinates?                    GeoChargingPointEntrance            { get; set; }

            /// <summary>
            /// Whether the EVSE is open 24/7.
            /// </summary>
            [Mandatory]
            public Boolean?                           IsOpen24Hours                       { get; set; }

            /// <summary>
            /// Optional opening times in case that the EVSE cannot be accessed around the clock.
            /// </summary>
            [Optional]
            public HashSet<OpeningTime>               OpeningTimes                        { get; }

            /// <summary>
            /// The optional hub operator of the EVSE.
            /// </summary>
            [Optional]
            public Operator_Id?                       HubOperatorId                       { get; set; }

            /// <summary>
            /// Optional clearing house for all charging sessions at the EVSE.
            /// </summary>
            [Optional]
            public ClearingHouse_Id?                  ClearingHouseId                     { get; set; }

            /// <summary>
            /// Whether ev roaming via Intercharge at the EVSE is possible.
            /// If set to "false" the EVSE will not be started/stopped remotely via Hubject.
            /// </summary>
            [Mandatory]
            public Boolean?                           IsHubjectCompatible                 { get; set; }

            /// <summary>
            /// Whether the CPO provides dynamic EVSE data information.
            /// </summary>
            [Mandatory]
            public FalseTrueAuto?                     DynamicInfoAvailable                { get; set; }

            #endregion

            #region Constructor(s)

            /// <summary>
            /// Create a new EVSE data record builder.
            /// </summary>
            /// <param name="Id">A unique EVSE identification.</param>
            /// <param name="OperatorId">The unique identification of the operator of the EVSE.</param>
            /// <param name="OperatorName">The name of the operator of the EVSE.</param>
            /// <param name="ChargingStationName">The multi-language name of the charging station hosting the EVSE.</param>
            /// <param name="Address">The address of the EVSE.</param>
            /// <param name="GeoCoordinates">The geo coordinate of the EVSE.</param>
            /// <param name="PlugTypes">The types of charging plugs attached to the EVSE.</param>
            /// <param name="ChargingFacilities">An enumeration of supported charging facilities at the EVSE.</param>
            /// <param name="RenewableEnergy">If the EVSE provides only renewable energy then the value MUST be” true”, if it use grey energy then value MUST be “false”.</param>
            /// <param name="CalibrationLawDataAvailability">This field gives the information how the charging station provides metering law data.</param>
            /// <param name="AuthenticationModes">The authentication modes the EVSE supports.</param>
            /// <param name="PaymentOptions">An enumeration of payment options that are supported.</param>
            /// <param name="ValueAddedServices">An enumeration of "value added services" the EVSE supports.</param>
            /// <param name="Accessibility">Specifies how the charging station can be accessed.</param>
            /// <param name="HotlinePhoneNumber">The phone number of the charging station operator's hotline.</param>
            /// <param name="IsOpen24Hours">Whether the EVSE is open 24/7.</param>
            /// <param name="IsHubjectCompatible">Whether ev roaming via Intercharge at the EVSE is possible. If set to "false" the EVSE will not be started/stopped remotely via Hubject.</param>
            /// <param name="DynamicInfoAvailable">Whether the CPO provides dynamic EVSE data information.</param>
            /// 
            /// <param name="DeltaType">The delta type when the EVSE data record was just downloaded.</param>
            /// <param name="LastUpdate">The last update timestamp of the EVSE data record.</param>
            /// 
            /// <param name="ChargingStationId">The identification of the charging station hosting the EVSE.</param>
            /// <param name="ChargingPoolId">The identification of the charging pool hosting the EVSE.</param>
            /// <param name="HardwareManufacturer">Optional name of the EVSE manufacturer.</param>
            /// <param name="ChargingStationImageURL">Optional URL to an image of the EVSE.</param>
            /// <param name="SubOperatorName">Optional name of the sub operator owning the EVSE.</param>
            /// <param name="DynamicPowerLevel">Whether the EVSE is able to deliver different power outputs.</param>
            /// <param name="EnergySources">Optional enumeration of energy sources that the EVSE uses to supply electric energy.</param>
            /// <param name="EnvironmentalImpact">Optional environmental impact produced by the energy sources used by the EVSE.</param>
            /// <param name="MaxCapacity">The maximum in kWh capacity the EVSE provides.</param>
            /// <param name="AccessibilityLocationType">Optional information where the EVSE could be accessed.</param>
            /// <param name="AdditionalInfo">Optional multi-language information about the EVSE.</param>
            /// <param name="ChargingStationLocationReference">Optional last meters information regarding the location of the EVSE.</param>
            /// <param name="GeoChargingPointEntrance">In case that the EVSE is part of a bigger facility (e.g. parking place), this optional attribute specifies the facilities entrance coordinates.</param>
            /// <param name="OpeningTimes">Optional opening times in case that the EVSE cannot be accessed around the clock.</param>
            /// <param name="HubOperatorId">The optional hub operator of the EVSE.</param>
            /// <param name="ClearingHouseId">Optional clearing house for all charging sessions at the EVSE.</param>
            /// 
            /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
            /// <param name="InternalData">Optional internal customer specific data, e.g. in combination with custom parsers and serializers.</param>
            public Builder(EVSE_Id?                           Id                                 = null,
                           Operator_Id?                       OperatorId                         = null,
                           String?                            OperatorName                       = null,
                           I18NText?                          ChargingStationName                = null,
                           Address?                           Address                            = null,
                           GeoCoordinates?                    GeoCoordinates                     = null,
                           IEnumerable<PlugTypes>?            PlugTypes                          = null,
                           IEnumerable<ChargingFacility>?     ChargingFacilities                 = null,
                           Boolean?                           RenewableEnergy                    = null,
                           CalibrationLawDataAvailabilities?  CalibrationLawDataAvailability     = null,
                           IEnumerable<AuthenticationModes>?  AuthenticationModes                = null,
                           IEnumerable<PaymentOptions>?       PaymentOptions                     = null,
                           IEnumerable<ValueAddedServices>?   ValueAddedServices                 = null,
                           AccessibilityTypes?                Accessibility                      = null,
                           Phone_Number?                      HotlinePhoneNumber                 = null,
                           Boolean?                           IsOpen24Hours                      = null,
                           Boolean?                           IsHubjectCompatible                = null,
                           FalseTrueAuto?                     DynamicInfoAvailable               = null,

                           DeltaTypes?                        DeltaType                          = null,
                           DateTime?                          LastUpdate                         = null,

                           ChargingStation_Id?                ChargingStationId                  = null,
                           ChargingPool_Id?                   ChargingPoolId                     = null,
                           String?                            HardwareManufacturer               = null,
                           URL?                               ChargingStationImageURL            = null,
                           String?                            SubOperatorName                    = null,
                           Boolean?                           DynamicPowerLevel                  = null,
                           IEnumerable<EnergySource>?         EnergySources                      = null,
                           EnvironmentalImpact?               EnvironmentalImpact                = null,
                           UInt32?                            MaxCapacity                        = null,
                           AccessibilityLocationTypes?        AccessibilityLocationType          = null,
                           I18NText?                          AdditionalInfo                     = null,
                           I18NText?                          ChargingStationLocationReference   = null,
                           GeoCoordinates?                    GeoChargingPointEntrance           = null,
                           IEnumerable<OpeningTime>?          OpeningTimes                       = null,
                           Operator_Id?                       HubOperatorId                      = null,
                           ClearingHouse_Id?                  ClearingHouseId                    = null,

                           JObject?                           CustomData                         = null,
                           UserDefinedDictionary?             InternalData                       = null)

                : base(CustomData,
                       InternalData)

            {

                this.Id                                = Id;
                this.OperatorId                        = OperatorId;
                this.OperatorName                      = OperatorName;
                this.ChargingStationName               = ChargingStationName;
                this.Address                           = Address;
                this.PlugTypes                         = PlugTypes           is not null && PlugTypes.          Any() ? new HashSet<PlugTypes>          (PlugTypes.          Distinct()) : new HashSet<PlugTypes>();
                this.ChargingFacilities                = ChargingFacilities  is not null && ChargingFacilities. Any() ? new HashSet<ChargingFacility>   (ChargingFacilities. Distinct()) : new HashSet<ChargingFacility>();
                this.RenewableEnergy                   = RenewableEnergy;
                this.CalibrationLawDataAvailability    = CalibrationLawDataAvailability;
                this.AuthenticationModes               = AuthenticationModes is not null && AuthenticationModes.Any() ? new HashSet<AuthenticationModes>(AuthenticationModes.Distinct()) : new HashSet<AuthenticationModes>();
                this.PaymentOptions                    = PaymentOptions      is not null && PaymentOptions.     Any() ? new HashSet<PaymentOptions>     (PaymentOptions.     Distinct()) : new HashSet<PaymentOptions>();
                this.ValueAddedServices                = ValueAddedServices  is not null && ValueAddedServices. Any() ? new HashSet<ValueAddedServices> (ValueAddedServices. Distinct()) : new HashSet<ValueAddedServices>();
                this.Accessibility                     = Accessibility;
                this.HotlinePhoneNumber                = HotlinePhoneNumber;
                this.IsOpen24Hours                     = IsOpen24Hours;
                this.IsHubjectCompatible               = IsHubjectCompatible;
                this.DynamicInfoAvailable              = DynamicInfoAvailable;

                this.DeltaType                         = DeltaType;
                this.LastUpdate                        = LastUpdate;

                this.ChargingStationId                 = ChargingStationId;
                this.ChargingPoolId                    = ChargingPoolId;
                this.HardwareManufacturer              = HardwareManufacturer;
                this.ChargingStationImageURL           = ChargingStationImageURL;
                this.SubOperatorName                   = SubOperatorName;
                this.GeoCoordinates                    = GeoCoordinates;
                this.DynamicPowerLevel                 = DynamicPowerLevel;
                this.EnergySources                     = EnergySources       is not null && EnergySources.      Any() ? new HashSet<EnergySource>       (EnergySources.      Distinct()) : new HashSet<EnergySource>();
                this.EnvironmentalImpact               = EnvironmentalImpact;
                this.MaxCapacity                       = MaxCapacity;
                this.AccessibilityLocationType         = AccessibilityLocationType;
                this.AdditionalInfo                    = AdditionalInfo;
                this.ChargingStationLocationReference  = ChargingStationLocationReference;
                this.GeoChargingPointEntrance          = GeoChargingPointEntrance;
                this.OpeningTimes                      = OpeningTimes        is not null && OpeningTimes.       Any() ? new HashSet<OpeningTime>        (OpeningTimes.       Distinct()) : new HashSet<OpeningTime>();
                this.HubOperatorId                     = HubOperatorId;
                this.ClearingHouseId                   = ClearingHouseId;

            }

            #endregion

            #region ToImmutable()

            /// <summary>
            /// Return an immutable version of the EVSE data record.
            /// </summary>
            /// <param name="Builder">An EVSEDataRecord builder.</param>
            public static implicit operator EVSEDataRecord(Builder Builder)

                => Builder.ToImmutable();


            /// <summary>
            /// Return an immutable version of the EVSE data record.
            /// </summary>
            public EVSEDataRecord ToImmutable()
            {

                #region Check mandatory parameters

                if (!Id.                            HasValue)
                    throw new ArgumentException("The given EVSE identification must not be null!",               nameof(Id));

                if (!OperatorId.                    HasValue)
                    throw new ArgumentException("The given operator identification must not be null!",           nameof(OperatorId));

                if (ChargingStationName is null || ChargingStationName.IsNullOrEmpty())
                    throw new ArgumentException("The given charging station name must not be null or empty!",    nameof(ChargingStationName));

                if (Address is null)
                    throw new ArgumentException("The given address must not be null!",                           nameof(Address));

                if (!GeoCoordinates.                HasValue)
                    throw new ArgumentException("The given geo coordinates must not be null!",                   nameof(GeoCoordinates));

                if (!RenewableEnergy.               HasValue)
                    throw new ArgumentException("The given renewable energy must not be null!",                  nameof(RenewableEnergy));

                if (!CalibrationLawDataAvailability.HasValue)
                    throw new ArgumentException("The given calibration law data availability must not be null!", nameof(CalibrationLawDataAvailability));

                if (!Accessibility.                 HasValue)
                    throw new ArgumentException("The given accessibility must not be null!",                     nameof(Accessibility));

                if (!IsOpen24Hours.                 HasValue)
                    throw new ArgumentException("The given 'is open 24 hours' must not be null!",                nameof(IsOpen24Hours));

                if (!IsHubjectCompatible.           HasValue)
                    throw new ArgumentException("The given 'is hubject compatible' must not be null!",           nameof(IsHubjectCompatible));

                if (!DynamicInfoAvailable.          HasValue)
                    throw new ArgumentException("The given 'dynamic info available' must not be null!",          nameof(DynamicInfoAvailable));

                #endregion

                return new EVSEDataRecord(Id.                            Value,
                                          OperatorId.                    Value,
                                          OperatorName                   ?? "",
                                          ChargingStationName,
                                          Address,
                                          GeoCoordinates.                Value,
                                          PlugTypes,
                                          ChargingFacilities,
                                          RenewableEnergy.               Value,
                                          CalibrationLawDataAvailability.Value,
                                          AuthenticationModes,
                                          PaymentOptions,
                                          ValueAddedServices,
                                          Accessibility.                 Value,
                                          HotlinePhoneNumber,
                                          IsOpen24Hours.                 Value,
                                          IsHubjectCompatible.           Value,
                                          DynamicInfoAvailable.          Value,

                                          DeltaType,
                                          LastUpdate,

                                          ChargingStationId,
                                          ChargingPoolId,
                                          HardwareManufacturer,
                                          ChargingStationImageURL,
                                          SubOperatorName,
                                          DynamicPowerLevel,
                                          EnergySources,
                                          EnvironmentalImpact,
                                          MaxCapacity,
                                          AccessibilityLocationType,
                                          AdditionalInfo,
                                          ChargingStationLocationReference,
                                          GeoChargingPointEntrance,
                                          OpeningTimes,
                                          HubOperatorId,
                                          ClearingHouseId,

                                          CustomData,
                                          InternalData);

            }

            #endregion

        }

        #endregion

    }

}
