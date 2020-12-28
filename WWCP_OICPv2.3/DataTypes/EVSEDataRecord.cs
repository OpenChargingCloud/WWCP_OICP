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
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// An Electric Vehicle Supply Equipment (EVSE).
    /// This is meant to be one electrical circuit which can charge an electric vehicle.
    /// </summary>
    public class EVSEDataRecord : IEquatable<EVSEDataRecord>,
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
        public I18NString                           ChargingStationName                    { get; }

        /// <summary>
        /// Optional name of the EVSE manufacturer.
        /// </summary>
        [Optional]
        public String                               HardwareManufacturer                   { get; }

        /// <summary>
        /// Optional URL to an image of the EVSE.
        /// </summary>
        [Optional]
        public URL?                                 ChargingStationImageURL                { get; }

        /// <summary>
        /// Optional name of the sub operator owning the EVSE.
        /// </summary>
        [Optional]
        public String                               SubOperatorName                        { get; }

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
        [Optional]
        public GeoCoordinates?                      GeoCoordinates                          { get; }

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
        public IEnumerable<EnergySource>            EnergySources                          { get; }

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
        [Mandatory]
        public Phone_Number                         HotlinePhoneNumber                     { get; }

        /// <summary>
        /// Optional multi-language information about the EVSE.
        /// </summary>
        [Optional]
        public I18NString                           AdditionalInfo                         { get; }

        /// <summary>
        /// Optional last meters information regarding the location of the EVSE.
        /// </summary>
        [Optional]
        public I18NString                           ChargingStationLocationReference       { get; }

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
        public IEnumerable<OpeningTime>             OpeningTimes                           { get; }

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

        /// <summary>
        /// Optional custom data, e.g. in combination with custom parsers and serializers.
        /// </summary>
        [Optional]
        public JObject                              CustomData                             { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EVSE data record.
        /// </summary>
        /// <param name="Id">A unique EVSE identification.</param>
        /// 
        /// <param name="Address">The address of the EVSE.</param>
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
        /// <param name="ChargingStationName">The multi-language name of the charging station hosting the EVSE.</param>
        /// <param name="HardwareManufacturer">Optional name of the EVSE manufacturer.</param>
        /// <param name="ChargingStationImageURL">Optional URL to an image of the EVSE.</param>
        /// <param name="SubOperatorName">Optional name of the sub operator owning the EVSE.</param>
        /// <param name="GeoCoordinates">The geo coordinate of the EVSE.</param>
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
        /// <param name="CustomData">Optional custom data, e.g. in combination with custom parsers and serializers.</param>
        public EVSEDataRecord(EVSE_Id                           Id,

                              Address                           Address,
                              IEnumerable<PlugTypes>            PlugTypes,
                              IEnumerable<ChargingFacility>     ChargingFacilities,
                              Boolean                           RenewableEnergy,
                              CalibrationLawDataAvailabilities  CalibrationLawDataAvailability,
                              IEnumerable<AuthenticationModes>  AuthenticationModes,
                              IEnumerable<PaymentOptions>       PaymentOptions,
                              IEnumerable<ValueAddedServices>   ValueAddedServices,
                              AccessibilityTypes                Accessibility,
                              Phone_Number                      HotlinePhoneNumber,
                              Boolean                           IsOpen24Hours,
                              Boolean                           IsHubjectCompatible,
                              FalseTrueAuto                     DynamicInfoAvailable,

                              DeltaTypes?                       DeltaType                          = null,
                              DateTime?                         LastUpdate                         = null,

                              ChargingStation_Id?               ChargingStationId                  = null,
                              ChargingPool_Id?                  ChargingPoolId                     = null,
                              I18NString                        ChargingStationName                = null,
                              String                            HardwareManufacturer               = null,
                              URL?                              ChargingStationImageURL            = null,
                              String                            SubOperatorName                    = null,
                              GeoCoordinates?                   GeoCoordinates                     = null,
                              Boolean?                          DynamicPowerLevel                  = null,
                              IEnumerable<EnergySource>         EnergySources                      = null,
                              EnvironmentalImpact?              EnvironmentalImpact                = null,
                              UInt32?                           MaxCapacity                        = null,
                              AccessibilityLocationTypes?       AccessibilityLocationType          = null,
                              I18NString                        AdditionalInfo                     = null,
                              I18NString                        ChargingStationLocationReference   = null,
                              GeoCoordinates?                   GeoChargingPointEntrance           = null,
                              IEnumerable<OpeningTime>          OpeningTimes                       = null,
                              Operator_Id?                      HubOperatorId                      = null,
                              ClearingHouse_Id?                 ClearingHouseId                    = null,

                              JObject                           CustomData                         = null)

        {

            #region Initial checks

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

            this.Address                           = Address                ?? throw new ArgumentNullException(nameof(Address), "The given address must not be null!");
            this.PlugTypes                         = PlugTypes?.          Distinct();
            this.ChargingFacilities                = ChargingFacilities?. Distinct();
            this.RenewableEnergy                   = RenewableEnergy;
            this.CalibrationLawDataAvailability    = CalibrationLawDataAvailability;
            this.AuthenticationModes               = AuthenticationModes?.Distinct();
            this.PaymentOptions                    = PaymentOptions?.     Distinct();
            this.ValueAddedServices                = ValueAddedServices?. Distinct();
            this.Accessibility                     = Accessibility;
            this.HotlinePhoneNumber                = HotlinePhoneNumber;
            this.IsOpen24Hours                     = IsOpen24Hours;
            this.IsHubjectCompatible               = IsHubjectCompatible;
            this.DynamicInfoAvailable              = DynamicInfoAvailable;

            this.DeltaType                         = DeltaType;
            this.LastUpdate                        = LastUpdate;

            this.ChargingStationId                 = ChargingStationId;
            this.ChargingPoolId                    = ChargingPoolId;
            this.ChargingStationName               = ChargingStationName;
            this.HardwareManufacturer              = HardwareManufacturer;
            this.ChargingStationImageURL           = ChargingStationImageURL;
            this.SubOperatorName                   = SubOperatorName;
            this.GeoCoordinates                    = GeoCoordinates;
            this.DynamicPowerLevel                 = DynamicPowerLevel;
            this.EnergySources                     = EnergySources?.      Distinct();
            this.EnvironmentalImpact               = EnvironmentalImpact;
            this.MaxCapacity                       = MaxCapacity;
            this.AccessibilityLocationType         = AccessibilityLocationType;
            this.AdditionalInfo                    = AdditionalInfo;
            this.ChargingStationLocationReference  = ChargingStationLocationReference;
            this.GeoChargingPointEntrance          = GeoChargingPointEntrance;
            this.OpeningTimes                      = OpeningTimes;
            this.HubOperatorId                     = HubOperatorId;
            this.ClearingHouseId                   = ClearingHouseId;

            this.CustomData                        = CustomData;

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
        public static EVSEDataRecord Parse(JObject                                      JSON,
                                           CustomJObjectParserDelegate<EVSEDataRecord>  CustomEVSEDataRecordParser   = null)
        {

            if (TryParse(JSON,
                         out EVSEDataRecord evseDataRecord,
                         out String         ErrorResponse,
                         CustomEVSEDataRecordParser))
            {
                return evseDataRecord;
            }

            throw new ArgumentException("The given JSON representation of an EVSE data record is invalid: " + ErrorResponse, nameof(JSON));

        }

        #endregion

        #region (static) Parse   (Text, CustomEVSEDataRecordParser = null)

        /// <summary>
        /// Parse the given text representation of an EVSE data record.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSE data records JSON objects.</param>
        public static EVSEDataRecord Parse(String                                       Text,
                                           CustomJObjectParserDelegate<EVSEDataRecord>  CustomEVSEDataRecordParser   = null)
        {

            if (TryParse(Text,
                         out EVSEDataRecord evseDataRecord,
                         out String         ErrorResponse,
                         CustomEVSEDataRecordParser))
            {
                return evseDataRecord;
            }

            throw new ArgumentException("The given text representation of an EVSE data record is invalid: " + ErrorResponse, nameof(Text));

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
        public static Boolean TryParse(JObject             JSON,
                                       out EVSEDataRecord  EVSEDataRecord,
                                       out String          ErrorResponse)

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
        public static Boolean TryParse(JObject                                      JSON,
                                       out EVSEDataRecord                           EVSEDataRecord,
                                       out String                                   ErrorResponse,
                                       CustomJObjectParserDelegate<EVSEDataRecord>  CustomEVSEDataRecordParser)
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

                #region Parse Address                           [mandatory]

                if (!JSON.ParseMandatoryJSON2("Address",
                                              "address",
                                              OICPv2_3.Address.TryParse,
                                              out Address Address,
                                              out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse PlugTypes                         [mandatory]

                if (!JSON.ParseMandatory("PlugTypes",
                                         "plug types",
                                         PlugTypesExtentions.TryParse,
                                         out IEnumerable<PlugTypes> PlugTypes,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse ChargingFacilities                [mandatory]

                if (!JSON.ParseMandatoryJSON("ChargingFacilities",
                                             "charging facilities",
                                             ChargingFacility.TryParse,
                                             out IEnumerable<ChargingFacility> ChargingFacilities,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse RenewableEnergy                   [mandatory]

                if (!JSON.ParseMandatory("RenewableEnergy",
                                         "renewable energy",
                                         out Boolean RenewableEnergy,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse CalibrationLawDataAvailability    [mandatory]

                if (!JSON.ParseMandatory("CalibrationLawDataAvailability",
                                         "calibration law data availability",
                                         CalibrationLawDataAvailabilitiesExtentions.TryParse,
                                         out CalibrationLawDataAvailabilities CalibrationLawDataAvailability,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse AuthenticationModes               [mandatory]

                if (!JSON.ParseMandatory("AuthenticationModes",
                                         "address",
                                         AuthenticationModesExtentions.TryParse,
                                         out IEnumerable<AuthenticationModes> AuthenticationModes,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse PaymentOptions                    [mandatory]

                if (!JSON.ParseMandatory("PaymentOptions",
                                         "payment options",
                                         PaymentOptionsExtentions.TryParse,
                                         out IEnumerable<PaymentOptions> PaymentOptions,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse ValueAddedServices                [mandatory]

                if (!JSON.ParseMandatory("ValueAddedServices",
                                         "value added services",
                                         ValueAddedServicesExtentions.TryParse,
                                         out IEnumerable<ValueAddedServices> ValueAddedServices,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Accessibility                     [mandatory]

                if (!JSON.ParseMandatory("Accessibility",
                                         "accessibility",
                                         AccessibilityTypesExtentions.TryParse,
                                         out AccessibilityTypes Accessibility,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse HotlinePhoneNumber                [mandatory]

                if (!JSON.ParseMandatory("HotlinePhoneNumber",
                                         "hotline phone number",
                                         Phone_Number.TryParse,
                                         out Phone_Number HotlinePhoneNumber,
                                         out ErrorResponse))
                {
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
                                         FalseTrueAuto.TryParse,
                                         out FalseTrueAuto DynamicInfoAvailable,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion


                #region Parse DeltaType                         [optional]

                if (JSON.ParseOptionalStruct("deltaType",
                                             "delta type",
                                             DeltaTypesExtentions.TryParse,
                                             out DeltaTypes? DeltaType,
                                             out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse LastUpdate                        [optional]

                if (JSON.ParseOptional("lastUpdate",
                                       "last update",
                                       out DateTime? LastUpdate,
                                       out ErrorResponse))
                {
                    if (ErrorResponse != null)
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
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse ChargingPoolId                    [optional]

                if (JSON.ParseOptionalStruct("ChargingPoolID",
                                             "charging pool identification",
                                             ChargingPool_Id.TryParse,
                                             out ChargingPool_Id? ChargingPoolId,
                                             out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                // ChargingStationName

                #region HardwareManufacturer                    [optional]

                var HardwareManufacturer = JSON["HardwareManufacturer"]?.Value<String>();

                #endregion

                #region Parse ChargingStationImageURL           [optional]

                if (JSON.ParseOptionalStruct("ChargingStationImage",
                                             "charging station image URL",
                                             URL.TryParse,
                                             out URL? ChargingStationImageURL,
                                             out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region SubOperatorName                         [optional]

                var SubOperatorName = JSON["SubOperatorName"]?.Value<String>();

                #endregion

                #region Parse GeoCoordinates                    [optional]

                if (JSON.ParseOptionalStruct("GeoCoordinates",
                                             "geo coordinates",
                                             OICPv2_3.GeoCoordinates.TryParse,
                                             out GeoCoordinates? GeoCoordinates,
                                             out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse DynamicPowerLevel                 [optional]

                if (JSON.ParseOptional("DynamicPowerLevel",
                                       "dynamic power level",
                                       out Boolean? DynamicPowerLevel,
                                       out ErrorResponse))
                {
                    if (ErrorResponse != null)
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
                    if (ErrorResponse != null)
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
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse MaxCapacity                       [optional]

                if (JSON.ParseOptional("MaxCapacity",
                                       "max capacity",
                                       out UInt32? MaxCapacity,
                                       out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                #region Parse AccessibilityLocationType         [optional]

                if (JSON.ParseOptionalStruct("AccessibilityLocation",
                                             "accessibility location",
                                             AccessibilityLocationTypesExtentions.TryParse,
                                             out AccessibilityLocationTypes? AccessibilityLocationType,
                                             out ErrorResponse))
                {
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion

                // AdditionalInfo
                // ChargingStationLocationReference

                #region Parse GeoChargingPointEntrance          [optional]

                if (JSON.ParseOptionalStruct("GeoChargingPointEntrance",
                                             "geo charging point entrance",
                                             OICPv2_3.GeoCoordinates.TryParse,
                                             out GeoCoordinates? GeoChargingPointEntrance,
                                             out ErrorResponse))
                {
                    if (ErrorResponse != null)
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
                    if (ErrorResponse != null)
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
                    if (ErrorResponse != null)
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
                    if (ErrorResponse != null)
                        return false;
                }

                #endregion


                #region Parse Custom Data                       [optional]

                var CustomData = JSON["CustomData"] as JObject;

                #endregion


                EVSEDataRecord = new EVSEDataRecord(EVSEId,
                                                    Address,
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
                                                    null,
                                                    HardwareManufacturer,
                                                    ChargingStationImageURL,
                                                    SubOperatorName,
                                                    GeoCoordinates,
                                                    DynamicPowerLevel,
                                                    EnergySources,
                                                    EnvironmentalImpact,
                                                    MaxCapacity,
                                                    AccessibilityLocationType,
                                                    null,
                                                    null,
                                                    GeoChargingPointEntrance,
                                                    OpeningTimes,
                                                    HubOperatorId,
                                                    ClearingHouseId,

                                                    CustomData);


                if (CustomEVSEDataRecordParser != null)
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

        #region (static) TryParse(Text, out EVSEDataRecord, out ErrorResponse, CustomEVSEDataRecordParser = null)

        /// <summary>
        /// Try to parse the given text representation of an EVSE data record.
        /// </summary>
        /// <param name="Text">The text to parse.</param>
        /// <param name="EVSEDataRecord">The parsed EVSE data record.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSE data records JSON objects.</param>
        public static Boolean TryParse(String                                       Text,
                                       out EVSEDataRecord                           EVSEDataRecord,
                                       out String                                   ErrorResponse,
                                       CustomJObjectParserDelegate<EVSEDataRecord>  CustomEVSEDataRecordParser)
        {

            try
            {

                return TryParse(JObject.Parse(Text),
                                out EVSEDataRecord,
                                out ErrorResponse,
                                CustomEVSEDataRecordParser);

            }
            catch (Exception e)
            {
                EVSEDataRecord  = default;
                ErrorResponse   = "The given text representation of an EVSE data record is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomEVSEDataRecordSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomEVSEDataRecordSerializer">A delegate to serialize custom EVSE data record JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<EVSEDataRecord> CustomEVSEDataRecordSerializer = null)
        {

            var JSON = JSONObject.Create(

                           new JProperty("EvseID",                                  Id.                            ToString()),

                           new JProperty("Address",                                 Address.                       ToJSON()),
                           new JProperty("PlugTypes",                               new JArray(PlugTypes.          SafeSelect(plugType           => plugType.          AsString()))),
                           new JProperty("ChargingFacilities",                      new JArray(ChargingFacilities. SafeSelect(chargingFacility   => chargingFacility.  ToString()))),
                           new JProperty("RenewableEnergy",                         RenewableEnergy),
                           new JProperty("CalibrationLawDataAvailability",          CalibrationLawDataAvailability.AsString()),
                           new JProperty("AuthenticationModes",                     new JArray(AuthenticationModes.SafeSelect(authenticationMode => authenticationMode.AsString()))),
                           new JProperty("PaymentOptions",                          new JArray(PaymentOptions.     SafeSelect(paymentOption      => paymentOption.     AsString()))),
                           new JProperty("ValueAddedServices",                      new JArray(ValueAddedServices. SafeSelect(valueAddedService  => valueAddedService. AsString()))),
                           new JProperty("Accessibility",                           Accessibility.                 AsString()),
                           new JProperty("HotlinePhoneNumber",                      HotlinePhoneNumber.            ToString()),
                           new JProperty("IsOpen24Hours",                           IsOpen24Hours),
                           new JProperty("IsHubjectCompatible",                     IsHubjectCompatible),
                           new JProperty("DynamicInfoAvailable",                    DynamicInfoAvailable.          ToString()),


                           DeltaType.                       HasValue
                               ? new JProperty("deltaType",                         DeltaType.               Value)
                               : null,

                           LastUpdate.                      HasValue
                               ? new JProperty("lastUpdate",                        LastUpdate.              Value.ToIso8601())
                               : null,


                           ChargingStationId.               HasValue
                               ? new JProperty("ChargingStationID",                 ChargingStationId.       Value)
                               : null,

                           ChargingPoolId.                  HasValue
                               ? new JProperty("ChargingPoolID",                    ChargingPoolId.          Value)
                               : null,

                           ChargingStationName.             IsNeitherNullNorEmpty()
                               ? new JProperty("ChargingStationNames",              new JArray(ChargingStationName.Select(i18n => new JObject(
                                                                                                                                      new JProperty("lang",  i18n.Language.ToString()),
                                                                                                                                      new JProperty("value", i18n.Text)
                                                                                                                                  ))))
                               : null,

                           HardwareManufacturer.            IsNeitherNullNorEmpty()
                               ? new JProperty("HardwareManufacturer",              HardwareManufacturer)
                               : null,

                           ChargingStationImageURL.         HasValue
                               ? new JProperty("ChargingStationImage",              ChargingStationImageURL. Value.ToString())
                               : null,

                           SubOperatorName.                 IsNeitherNullNorEmpty()
                               ? new JProperty("SubOperatorName",                   SubOperatorName)
                               : null,

                           GeoCoordinates.                  HasValue
                               ? new JProperty("GeoCoordinates",                    GeoCoordinates.          Value.ToJSON())
                               : null,

                           DynamicPowerLevel.               HasValue
                               ? new JProperty("DynamicPowerLevel",                 DynamicPowerLevel.       Value)
                               : null,

                           EnergySources.                   IsNeitherNullNorEmpty()
                               ? new JProperty("EnergySource",                      new JArray(EnergySources.Select(energySource => energySource.ToJSON())))
                               : null,

                           EnvironmentalImpact.             HasValue
                               ? new JProperty("EnvironmentalImpact",               EnvironmentalImpact.     Value.ToJSON())
                               : null,

                           MaxCapacity.                     HasValue
                               ? new JProperty("MaxCapacity",                       MaxCapacity.             Value)
                               : null,

                           AccessibilityLocationType.           HasValue
                               ? new JProperty("AccessibilityLocation",             AccessibilityLocationType.   Value.AsString())
                               : null,

                           AdditionalInfo.                  IsNeitherNullNorEmpty()
                               ? new JProperty("AdditionalInfo",                    new JArray(AdditionalInfo.Select(i18n => new JObject(
                                                                                                                                 new JProperty("lang",  i18n.Language.ToString()),
                                                                                                                                 new JProperty("value", i18n.Text)
                                                                                                                             ))))
                               : null,

                           ChargingStationLocationReference.IsNeitherNullNorEmpty()
                               ? new JProperty("ChargingStationLocationReference",  new JArray(ChargingStationLocationReference.Select(i18n => new JObject(
                                                                                                                                                   new JProperty("lang",  i18n.Language.ToString()),
                                                                                                                                                   new JProperty("value", i18n.Text)
                                                                                                                                               ))))
                               : null,

                           GeoChargingPointEntrance.        HasValue
                               ? new JProperty("GeoChargingPointEntrance",          GeoChargingPointEntrance.Value.ToJSON())
                               : null,

                           OpeningTimes.                    IsNeitherNullNorEmpty()
                               ? new JProperty("OpeningTimes",                      new JArray(OpeningTimes.Select(openingTime => openingTime.ToJSON())))
                               : null,

                           HubOperatorId.                   HasValue
                               ? new JProperty("HubOperatorID",                     HubOperatorId.           Value.ToString())
                               : null,

                           ClearingHouseId.                 HasValue
                               ? new JProperty("ClearinghouseID",                   ClearingHouseId.         Value.ToString())
                               : null,

                           CustomData != null
                               ? new JProperty("CustomData",                        CustomData)
                               : null

                       );

            return CustomEVSEDataRecordSerializer != null
                       ? CustomEVSEDataRecordSerializer(this, JSON)
                       : JSON;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this dynamic data of an EVSE.
        /// </summary>
        public EVSEDataRecord Clone

            => new EVSEDataRecord(Id,

                                  Address,
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
                                  ChargingStationName,
                                  HardwareManufacturer,
                                  ChargingStationImageURL,
                                  SubOperatorName,
                                  GeoCoordinates,
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

                                  JObject.Parse(CustomData.ToString(Newtonsoft.Json.Formatting.None)));

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
        public Int32 CompareTo(Object Object)

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
        public Int32 CompareTo(EVSEDataRecord EVSEDataRecord)
        {

            if (EVSEDataRecord is null)
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

            => Object is EVSEDataRecord evseDataRecord &&
                   Equals(evseDataRecord);

        #endregion

        #region Equals(EVSEDataRecord)

        /// <summary>
        /// Compares two EVSE data records for equality.
        /// </summary>
        /// <param name="EVSEDataRecord">An EVSE data record to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSEDataRecord EVSEDataRecord)
        {

            if (EVSEDataRecord is null)
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
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()
            => Id.ToString();

        #endregion


        #region ToBuilder(NewEVSEId = null)

        ///// <summary>
        ///// Return a builder for the EVSE data record.
        ///// </summary>
        ///// <param name="NewEVSEId">An optional new EVSE identification.</param>
        //public Builder ToBuilder(EVSE_Id? NewEVSEId = null)

        //    => new Builder(
        //           NewEVSEId ?? Id,
        //           DeltaType,
        //           LastUpdate,

        //           Address,
        //           GeoCoordinate,
        //           PlugTypes,
        //           AuthenticationModes,
        //           ValueAddedServices,
        //           Accessibility,
        //           HotlinePhoneNumber,
        //           IsOpen24Hours,
        //           IsHubjectCompatible,
        //           DynamicInfoAvailable,

        //           ChargingStationId,
        //           ChargingPoolId,
        //           ChargingStationName,
        //           ChargingFacilities,
        //           ChargingModes,
        //           MaxCapacity,
        //           PaymentOptions,
        //           AdditionalInfo,
        //           GeoChargingPointEntrance,
        //           OpeningTimes,
        //           HubOperatorId,
        //           ClearingHouseId,

        //           CustomData);

        #endregion

        #region (class) Builder

        ///// <summary>
        ///// An Electric Vehicle Supply Equipment (EVSE).
        ///// This is meant to be one electrical circuit which can charge a electric vehicle.
        ///// </summary>
        //public new class Builder : ACustomData.Builder
        //{

        //    #region Properties

        //    /// <summary>
        //    /// The unique identification of the Electric Vehicle Supply Equipment (EVSE).
        //    /// </summary>
        //    public EVSE_Id                           Id                          { get; }


        //    /// <summary>
        //    /// The delta type when the EVSE data record was just downloaded.
        //    /// </summary>
        //    public DeltaTypes?                       DeltaType                   { get; set; }

        //    /// <summary>
        //    /// The last update timestamp of the EVSE data record.
        //    /// </summary>
        //    public DateTime?                         LastUpdate                  { get; set; }


        //    /// <summary>
        //    /// The identification of the charging station hosting the EVSE.
        //    /// </summary>
        //    public ChargingStation_Id?               ChargingStationId           { get; set; }

        //    /// <summary>
        //    /// The identification of the charging pool hosting the EVSE.
        //    /// </summary>
        //    public ChargingPool_Id?                  ChargingPoolId              { get; set; }

        //    /// <summary>
        //    /// The multi-language name of the charging station hosting the EVSE.
        //    /// </summary>
        //    public I18NString                        ChargingStationName         { get; set; }

        //    /// <summary>
        //    /// The address of the EVSE.
        //    /// </summary>
        //    public Address                           Address                     { get; set; }

        //    /// <summary>
        //    /// The geo coordinate of the EVSE.
        //    /// </summary>
        //    public GeoCoordinates?                    GeoCoordinate               { get; set; }

        //    /// <summary>
        //    /// The types of charging plugs attached to the EVSE.
        //    /// </summary>
        //    public ReactiveSet<PlugTypes>            PlugTypes                   { get; }

        //    /// <summary>
        //    /// The charging facilities at the EVSE.
        //    /// </summary>
        //    public ReactiveSet<ChargingFacility>     ChargingFacilities          { get; }

        //    /// <summary>
        //    /// The charging modes the EVSE supports.
        //    /// </summary>
        //    public ReactiveSet<ChargingModes>        ChargingModes               { get; }

        //    /// <summary>
        //    /// The authentication modes the EVSE supports.
        //    /// </summary>
        //    public ReactiveSet<AuthenticationModes>  AuthenticationModes         { get; }

        //    /// <summary>
        //    /// The maximum capacity the EVSE provides.
        //    /// </summary>
        //    public Single?                           MaxCapacity                 { get; set; }

        //    /// <summary>
        //    /// The payment options the EVSE supports.
        //    /// </summary>
        //    public ReactiveSet<PaymentOptions>       PaymentOptions              { get; }

        //    /// <summary>
        //    /// A list of "value added services" the EVSE supports.
        //    /// </summary>
        //    public ReactiveSet<ValueAddedServices>   ValueAddedServices          { get; }

        //    /// <summary>
        //    /// The accessibility of the EVSE.
        //    /// </summary>
        //    public AccessibilityTypes?               Accessibility               { get; set; }

        //    /// <summary>
        //    /// The phone number of the Charging Station Operators hotline.
        //    /// </summary>
        //    public String                            HotlinePhoneNumber          { get; set; }

        //    /// <summary>
        //    /// Additional multi-language information about the EVSE.
        //    /// </summary>
        //    public I18NString                        AdditionalInfo              { get; set; }

        //    /// <summary>
        //    /// The geo coordinate of the entrance to the EVSE.
        //    /// </summary>
        //    public GeoCoordinates?                    GeoChargingPointEntrance    { get; set; }

        //    /// <summary>
        //    /// Whether the EVSE is open 24/7.
        //    /// </summary>
        //    public Boolean?                          IsOpen24Hours               { get; set; }

        //    /// <summary>
        //    /// The opening times of the EVSE.
        //    /// </summary>
        //    public String                            OpeningTimes                { get; set; }

        //    /// <summary>
        //    /// An optional hub operator of the EVSE.
        //    /// </summary>
        //    public HubOperator_Id?                   HubOperatorId               { get; set; }

        //    /// <summary>
        //    /// An optional clearing house of the EVSE.
        //    /// </summary>
        //    public ClearingHouse_Id?                 ClearingHouseId             { get; set; }

        //    /// <summary>
        //    /// Whether the EVSE is Hubject compatible.
        //    /// </summary>
        //    public Boolean?                          IsHubjectCompatible         { get; set; }

        //    /// <summary>
        //    /// Whether the EVSE provides dynamic data information.
        //    /// </summary>
        //    public Boolean?                          DynamicInfoAvailable        { get; set; }

        //    #endregion

        //    #region Constructor(s)

        //    /// <summary>
        //    /// Create a new EVSE data record builder.
        //    /// </summary>
        //    /// <param name="Id">A unique EVSE identification.</param>
        //    /// <param name="DeltaType">The delta type when the EVSE data record was just downloaded.</param>
        //    /// <param name="LastUpdate">The last update timestamp of the EVSE data record.</param>
        //    /// 
        //    /// <param name="Address">The address of the EVSE.</param>
        //    /// <param name="GeoCoordinate">The geo coordinate of the EVSE.</param>
        //    /// <param name="PlugTypes">The types of charging plugs attached to the EVSE.</param>
        //    /// <param name="AuthenticationModes">The authentication modes the EVSE supports.</param>
        //    /// <param name="ValueAddedServices">A list of "value added services" the EVSE supports.</param>
        //    /// <param name="Accessibility">The accessibility of the EVSE.</param>
        //    /// <param name="HotlinePhoneNumber">The phone number of the charging station operators hotline.</param>
        //    /// <param name="IsOpen24Hours">Whether the EVSE is open 24/7.</param>
        //    /// <param name="IsHubjectCompatible">Whether the EVSE is Hubject compatible.</param>
        //    /// <param name="DynamicInfoAvailable">Whether the EVSE provides dynamic data information.</param>
        //    /// 
        //    /// <param name="ChargingStationId">The identification of the charging station hosting the EVSE.</param>
        //    /// <param name="ChargingPoolId">The identification of the charging pool hosting the EVSE.</param>
        //    /// <param name="ChargingStationName">The multi-language name of the charging station hosting the EVSE.</param>
        //    /// <param name="ChargingFacilities">The charging facilities at the EVSE.</param>
        //    /// <param name="ChargingModes">The charging modes the EVSE supports.</param>
        //    /// <param name="MaxCapacity">The maximum capacity the EVSE provides.</param>
        //    /// <param name="PaymentOptions">The payment options the EVSE supports.</param>
        //    /// <param name="AdditionalInfo">Additional multi-language information about the EVSE.</param>
        //    /// <param name="GeoChargingPointEntrance">The geo coordinate of the entrance to the EVSE.</param>
        //    /// <param name="OpeningTimes">The opening times of the EVSE.</param>
        //    /// <param name="HubOperatorId">An optional hub operator of the EVSE.</param>
        //    /// <param name="ClearingHouseId">An optional clearing house of the EVSE.</param>
        //    /// 
        //    /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        //    public Builder(EVSE_Id                                    Id,
        //                   DeltaTypes?                                DeltaType                  = null,
        //                   DateTime?                                  LastUpdate                 = null,

        //                   Address                                    Address                    = null,
        //                   GeoCoordinates?                             GeoCoordinate              = null,
        //                   IEnumerable<PlugTypes>                     PlugTypes                  = null,
        //                   IEnumerable<AuthenticationModes>           AuthenticationModes        = null,
        //                   IEnumerable<ValueAddedServices>            ValueAddedServices         = null,
        //                   AccessibilityTypes?                        Accessibility              = null,
        //                   String                                     HotlinePhoneNumber         = null,
        //                   Boolean?                                   IsOpen24Hours              = null,
        //                   Boolean?                                   IsHubjectCompatible        = null,
        //                   Boolean?                                   DynamicInfoAvailable       = null,

        //                   ChargingStation_Id?                        ChargingStationId          = null,
        //                   ChargingPool_Id?                           ChargingPoolId             = null,
        //                   I18NString                                 ChargingStationName        = null,
        //                   IEnumerable<ChargingFacility>              ChargingFacilities         = null,
        //                   IEnumerable<ChargingModes>                 ChargingModes              = null,
        //                   Single?                                    MaxCapacity                = null,
        //                   IEnumerable<PaymentOptions>                PaymentOptions             = null,
        //                   I18NString                                 AdditionalInfo             = null,
        //                   GeoCoordinates?                             GeoChargingPointEntrance   = null,
        //                   String                                     OpeningTimes               = null,
        //                   HubOperator_Id?                            HubOperatorId              = null,
        //                   ClearingHouse_Id?                          ClearingHouseId            = null,

        //                   IEnumerable<KeyValuePair<String, Object>>  CustomData                 = null)

        //        : base(CustomData)

        //    {

        //        this.Id                        = Id;
        //        this.DeltaType                 = DeltaType;
        //        this.LastUpdate                = LastUpdate;

        //        this.Address                   = Address                          ?? throw new ArgumentNullException(nameof(Address), "The given address must not be null!");
        //        this.GeoCoordinate             = GeoCoordinate;
        //        this.PlugTypes                 = PlugTypes           != null ? new ReactiveSet<PlugTypes>          (PlugTypes)           : new ReactiveSet<PlugTypes>();
        //        this.AuthenticationModes       = AuthenticationModes != null ? new ReactiveSet<AuthenticationModes>(AuthenticationModes) : new ReactiveSet<AuthenticationModes>();
        //        this.ValueAddedServices        = ValueAddedServices  != null ? new ReactiveSet<ValueAddedServices> (ValueAddedServices)  : new ReactiveSet<ValueAddedServices>();
        //        this.Accessibility             = Accessibility;
        //        this.HotlinePhoneNumber        = HotlinePhoneNumber?.Trim();
        //        this.IsOpen24Hours             = IsOpen24Hours;
        //        this.IsHubjectCompatible       = IsHubjectCompatible;
        //        this.DynamicInfoAvailable      = DynamicInfoAvailable;

        //        this.ChargingStationId         = ChargingStationId;
        //        this.ChargingPoolId            = ChargingPoolId;
        //        this.ChargingStationName       = ChargingStationName              ?? new I18NString();
        //        this.ChargingModes             = ChargingModes        != null ? new ReactiveSet<ChargingModes>     (ChargingModes)       : new ReactiveSet<ChargingModes>();
        //        this.ChargingFacilities        = ChargingFacilities   != null ? new ReactiveSet<ChargingFacility>  (ChargingFacilities)  : new ReactiveSet<ChargingFacility>();
        //        this.MaxCapacity               = MaxCapacity;
        //        this.PaymentOptions            = PaymentOptions       != null ? new ReactiveSet<PaymentOptions>    (PaymentOptions)      : new ReactiveSet<PaymentOptions>();
        //        this.AdditionalInfo            = AdditionalInfo.SubstringMax(200) ?? new I18NString();
        //        this.GeoChargingPointEntrance  = GeoChargingPointEntrance;
        //        this.OpeningTimes              = OpeningTimes;
        //        this.HubOperatorId             = HubOperatorId;
        //        this.ClearingHouseId           = ClearingHouseId;

        //    }

        //    #endregion


        //    #region Build()

        //    /// <summary>
        //    /// Return an immutable version of the EVSE data record.
        //    /// </summary>
        //    public EVSEDataRecord Build()

        //        => new EVSEDataRecord(Id,

        //                              Address,
        //                              GeoCoordinate        ?? Vanaheimr.Aegir.GeoCoordinate.Parse(0, 0),
        //                              PlugTypes,
        //                              AuthenticationModes,
        //                              ValueAddedServices,
        //                              Accessibility        ?? AccessibilityTypes.Test_Station,
        //                              HotlinePhoneNumber,
        //                              IsOpen24Hours        ?? true,
        //                              IsHubjectCompatible  ?? false,
        //                              DynamicInfoAvailable ?? false,

        //                              DeltaType,
        //                              LastUpdate,

        //                              ChargingStationId,
        //                              ChargingPoolId,
        //                              ChargingStationName,
        //                              ChargingFacilities,
        //                              ChargingModes,
        //                              MaxCapacity,
        //                              PaymentOptions,
        //                              AdditionalInfo,
        //                              GeoChargingPointEntrance,
        //                              OpeningTimes,
        //                              HubOperatorId,
        //                              ClearingHouseId,

        //                              CustomData);

        //    #endregion

        //}

        #endregion

    }

}
