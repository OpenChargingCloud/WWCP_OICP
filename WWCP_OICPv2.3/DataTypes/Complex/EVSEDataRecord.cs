﻿/*
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

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
        public String                               ChargingStationImageURL                { get; }

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
        public GeoCoordinates?                      GeoCoordinate                          { get; }

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
        public AccessibilityLocationTypes?          AccessibilityLocation                  { get; }

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
        /// Whether the CPO provides dynamic EVSE status information.
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
        /// <param name="DynamicInfoAvailable">Whether the CPO provides dynamic EVSE status information.</param>
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
        /// <param name="GeoCoordinate">The geo coordinate of the EVSE.</param>
        /// <param name="DynamicPowerLevel">Whether the EVSE is able to deliver different power outputs.</param>
        /// <param name="EnergySources">Optional enumeration of energy sources that the EVSE uses to supply electric energy.</param>
        /// <param name="EnvironmentalImpact">Optional environmental impact produced by the energy sources used by the EVSE.</param>
        /// <param name="MaxCapacity">The maximum in kWh capacity the EVSE provides.</param>
        /// <param name="AccessibilityLocation">Optional information where the EVSE could be accessed.</param>
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
                              String                            ChargingStationImageURL            = null,
                              String                            SubOperatorName                    = null,
                              GeoCoordinates?                   GeoCoordinate                      = null,
                              Boolean?                          DynamicPowerLevel                  = null,
                              IEnumerable<EnergySource>         EnergySources                      = null,
                              EnvironmentalImpact?              EnvironmentalImpact                = null,
                              UInt32?                           MaxCapacity                        = null,
                              AccessibilityLocationTypes?       AccessibilityLocation              = null,
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
            this.GeoCoordinate                     = GeoCoordinate;
            this.DynamicPowerLevel                 = DynamicPowerLevel;
            this.EnergySources                     = EnergySources?.      Distinct();
            this.EnvironmentalImpact               = EnvironmentalImpact;
            this.MaxCapacity                       = MaxCapacity;
            this.AccessibilityLocation             = AccessibilityLocation;
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

        //#region (static) Parse   (EVSEDataRecordXML,  ..., OnException = null)

        ///// <summary>
        ///// Parse the given XML representation of an OICP EVSE data record.
        ///// </summary>
        ///// <param name="EVSEDataRecordXML">The XML to parse.</param>
        ///// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSEDataRecord XML elements.</param>
        ///// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        ///// <param name="CustomChargingFacilityParser">A delegate to parse custom ChargingFacility XML elements.</param>
        ///// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        //public static EVSEDataRecord Parse(XElement                                   EVSEDataRecordXML,
        //                                   CustomXMLParserDelegate<EVSEDataRecord>    CustomEVSEDataRecordParser     = null,
        //                                   CustomXMLParserDelegate<Address>           CustomAddressParser            = null,
        //                                   CustomXMLParserDelegate<ChargingFacility>  CustomChargingFacilityParser   = null,
        //                                   OnExceptionDelegate                        OnException                    = null)
        //{

        //    if (TryParse(EVSEDataRecordXML,
        //                 out EVSEDataRecord evseDataRecord,
        //                 CustomEVSEDataRecordParser,
        //                 CustomAddressParser,
        //                 CustomChargingFacilityParser,
        //                 OnException))

        //        return evseDataRecord;

        //    return null;

        //}

        //#endregion

        //#region (static) Parse   (EVSEDataRecordText, ..., OnException = null)

        ///// <summary>
        ///// Parse the given text-representation of an OICP EVSE data record.
        ///// </summary>
        ///// <param name="EVSEDataRecordText">The text to parse.</param>
        ///// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSEDataRecord XML elements.</param>
        ///// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        ///// <param name="CustomChargingFacilityParser">A delegate to parse custom ChargingFacility XML elements.</param>
        ///// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        //public static EVSEDataRecord Parse(String                                     EVSEDataRecordText,
        //                                   CustomXMLParserDelegate<EVSEDataRecord>    CustomEVSEDataRecordParser     = null,
        //                                   CustomXMLParserDelegate<Address>           CustomAddressParser            = null,
        //                                   CustomXMLParserDelegate<ChargingFacility>  CustomChargingFacilityParser   = null,
        //                                   OnExceptionDelegate                        OnException                    = null)
        //{

        //    if (TryParse(EVSEDataRecordText,
        //                 out EVSEDataRecord evseDataRecord,
        //                 CustomEVSEDataRecordParser,
        //                 CustomAddressParser,
        //                 CustomChargingFacilityParser,
        //                 OnException))

        //        return evseDataRecord;

        //    return null;

        //}

        //#endregion

        //#region (static) TryParse(EVSEDataRecordXML,  out EVSEDataRecord, ..., OnException = null)

        ///// <summary>
        ///// Try to parse the given XML representation of an OICP EVSE data record.
        ///// </summary>
        ///// <param name="EVSEDataRecordXML">The XML to parse.</param>
        ///// <param name="EVSEDataRecord">The parsed EVSE data record.</param>
        ///// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSEDataRecord XML elements.</param>
        ///// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        ///// <param name="CustomChargingFacilityParser">A delegate to parse custom ChargingFacility XML elements.</param>
        ///// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        //public static Boolean TryParse(XElement                                   EVSEDataRecordXML,
        //                               out EVSEDataRecord                         EVSEDataRecord,
        //                               CustomXMLParserDelegate<EVSEDataRecord>    CustomEVSEDataRecordParser     = null,
        //                               CustomXMLParserDelegate<Address>           CustomAddressParser            = null,
        //                               CustomXMLParserDelegate<ChargingFacility>  CustomChargingFacilityParser   = null,
        //                               OnExceptionDelegate                        OnException                    = null)
        //{

        //    try
        //    {

        //        if (EVSEDataRecordXML.Name != OICPNS.EVSEData + "EvseDataRecord")
        //        {
        //            EVSEDataRecord = null;
        //            return false;
        //        }

        //        var EVSEId = EVSE_Id.Parse(EVSEDataRecordXML.ElementValueOrFail(OICPNS.EVSEData + "EvseId", "Missing 'EvseId'-XML tag!"));

        //        #region XML Attribute: LastUpdate

        //        DateTime.TryParse(EVSEDataRecordXML.AttributeValueOrDefault(XName.Get("lastUpdate"), ""), out DateTime _LastUpdate);

        //        #endregion

        //        #region ChargingStationName

        //        var _ChargingStationName = new I18NString();

        //        EVSEDataRecordXML.IfValueIsNotNullOrEmpty(OICPNS.EVSEData + "ChargingStationName",
        //                                                  v => _ChargingStationName.Add(Languages.de, v));

        //        EVSEDataRecordXML.IfValueIsNotNullOrEmpty(OICPNS.EVSEData + "EnChargingStationName",
        //                                                  v => _ChargingStationName.Add(Languages.en, v));

        //        #endregion

        //        #region MaxCapacity in kWh

        //        var _MaxCapacity_kWh = EVSEDataRecordXML.
        //                                   ElementValueOrDefault(OICPNS.EVSEData + "MaxCapacity", String.Empty).
        //                                   Trim();

        //        Single _MaxCapacity = 0.0f;

        //        if (_MaxCapacity_kWh.IsNotNullOrEmpty())
        //            Single.TryParse(_MaxCapacity_kWh, out _MaxCapacity);

        //        #endregion

        //        #region AdditionalInfo

        //        var _AdditionalInfo = new I18NString();

        //        foreach (var infoTextXML in EVSEDataRecordXML.
        //                                        Element (OICPNS.EVSEData + "AdditionalInfo").
        //                                        Elements(OICPNS.EVSEData + "InfoText"))
        //        {

        //            var lang = infoTextXML.Attribute("lang").Value?.Trim().ToLower();

        //            if (lang.Length == 2 && Enum.TryParse(lang, out Languages Language))
        //                _AdditionalInfo.Add(Language, infoTextXML.Value);

        //        }

        //        #endregion


        //        EVSEDataRecord = new EVSEDataRecord(

        //            EVSEId,

        //            EVSEDataRecordXML.MapElementOrFail  (OICPNS.EVSEData + "Address",
        //                                                 (xml, e) => Address.Parse(xml,
        //                                                                           CustomAddressParser,
        //                                                                           e),
        //                                                 OnException),

        //            XML_IO.ParseGeoCoordinatesXML(EVSEDataRecordXML.ElementOrFail(OICPNS.EVSEData + "GeoCoordinates", "Missing 'GeoCoordinates'-XML tag!")),

        //            EVSEDataRecordXML.MapValuesOrFail   (OICPNS.EVSEData + "Plugs",
        //                                                 OICPNS.EVSEData + "Plug",
        //                                                 PlugTypesExtentions.Parse),

        //            EVSEDataRecordXML.MapValuesOrFail   (OICPNS.EVSEData + "AuthenticationModes",
        //                                                 OICPNS.EVSEData + "AuthenticationMode",
        //                                                 AuthenticationModesExtentions.Parse),

        //            EVSEDataRecordXML.MapValuesOrFail   (OICPNS.EVSEData + "ValueAddedServices",
        //                                                 OICPNS.EVSEData + "ValueAddedService",
        //                                                 ValueAddedServicesExtentions.Parse),

        //            AccessibilityTypesExtentions.Parse  (EVSEDataRecordXML.
        //                                                 ElementValueOrFail(OICPNS.EVSEData + "Accessibility").
        //                                                 Trim()),


        //            EVSEDataRecordXML.                   ElementValueOrDefault(OICPNS.EVSEData + "HotlinePhoneNum").
        //                                                 Trim(),

        //            EVSEDataRecordXML.MapValueOrFail    (OICPNS.EVSEData + "IsOpen24Hours",
        //                                                 s => s == "true"),

        //            EVSEDataRecordXML.ElementValueOrFail(OICPNS.EVSEData + "IsHubjectCompatible").
        //                              Trim() == "true",

        //            EVSEDataRecordXML.ElementValueOrFail(OICPNS.EVSEData + "DynamicInfoAvailable").
        //                              Trim() != "false",



        //            DeltaTypesExtentions.Parse(EVSEDataRecordXML.AttributeValueOrDefault(XName.Get("deltaType"), "")),

        //            _LastUpdate,



        //            EVSEDataRecordXML.MapValueOrNullable(OICPNS.EVSEData + "ChargingStationId",
        //                                                 ChargingStation_Id.Parse),

        //            EVSEDataRecordXML.MapValueOrNullable(OICPNS.EVSEData + "ChargingPoolId",
        //                                                 ChargingPool_Id.Parse),

        //            _ChargingStationName,

        //            HardwareManufacturer,

        //            EVSEDataRecordXML.MapValues         (OICPNS.EVSEData + "ChargingFacilities",
        //                                                 OICPNS.EVSEData + "ChargingFacility",
        //                                                 xml => ChargingFacility.Parse(xml,
        //                                                                               CustomChargingFacilityParser,
        //                                                                               OnException)),

        //            EVSEDataRecordXML.MapValues         (OICPNS.EVSEData + "ChargingModes",
        //                                                 OICPNS.EVSEData + "ChargingMode",
        //                                                 ChargingModesExtentions.Parse),

        //            _MaxCapacity,

        //            EVSEDataRecordXML.MapValues         (OICPNS.EVSEData + "PaymentOptions",
        //                                                 OICPNS.EVSEData + "PaymentOption",
        //                                                 PaymentOptionsExtentions.Parse),

        //            _AdditionalInfo,

        //            EVSEDataRecordXML.MapElement(OICPNS.CommonTypes + "GeoChargingPointEntrance",
        //                                         XML_IO.ParseGeoCoordinatesXML),

        //            //ToDo!!!!!!!!!!!!!!!!!!!!
        //            EVSEDataRecordXML.ElementValueOrDefault(OICPNS.EVSEData + "OpeningTimes"),

        //            EVSEDataRecordXML.MapValueOrNullable(OICPNS.EVSEData + "HubOperatorID",
        //                                                 HubOperator_Id.Parse),

        //            EVSEDataRecordXML.MapValueOrNullable(OICPNS.EVSEData + "ClearinghouseID",
        //                                                 ClearingHouse_Id.Parse)

        //        );

        //        if (CustomEVSEDataRecordParser != null)
        //            EVSEDataRecord = CustomEVSEDataRecordParser(EVSEDataRecordXML, EVSEDataRecord);

        //        return true;

        //    }
        //    catch (Exception e)
        //    {

        //        OnException?.Invoke(DateTime.UtcNow, EVSEDataRecordXML, e);

        //        EVSEDataRecord = null;
        //        return false;

        //    }

        //}

        //#endregion

        //#region (static) TryParse(EVSEDataRecordText, out EVSEDataRecord, ..., OnException = null)

        ///// <summary>
        ///// Try to parse the given text-representation of an OICP EVSE data record.
        ///// </summary>
        ///// <param name="EVSEDataRecordText">The text to parse.</param>
        ///// <param name="EVSEDataRecord">The parsed EVSE data record.</param>
        ///// <param name="CustomEVSEDataRecordParser">A delegate to parse custom EVSEDataRecord XML elements.</param>
        ///// <param name="CustomAddressParser">A delegate to parse custom Address XML elements.</param>
        ///// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        //public static Boolean TryParse(String                                     EVSEDataRecordText,
        //                               out EVSEDataRecord                         EVSEDataRecord,
        //                               CustomXMLParserDelegate<EVSEDataRecord>    CustomEVSEDataRecordParser     = null,
        //                               CustomXMLParserDelegate<Address>           CustomAddressParser            = null,
        //                               CustomXMLParserDelegate<ChargingFacility>  CustomChargingFacilityParser   = null,
        //                               OnExceptionDelegate                        OnException                    = null)
        //{

        //    try
        //    {

        //        if (TryParse(XDocument.Parse(EVSEDataRecordText).Root,
        //                     out EVSEDataRecord,
        //                     CustomEVSEDataRecordParser,
        //                     CustomAddressParser,
        //                     CustomChargingFacilityParser,
        //                     OnException))

        //            return true;

        //    }
        //    catch (Exception e)
        //    {
        //        OnException?.Invoke(DateTime.UtcNow, EVSEDataRecordText, e);
        //    }

        //    EVSEDataRecord = null;
        //    return false;

        //}

        //#endregion

        //#region ToXML(XName = null, IncludeMetadata = false, CustomEVSEDataRecordSerializer = null, CustomAddressSerializer = null)

        ///// <summary>
        ///// Return a XML representation of the EVSE data record.
        ///// </summary>
        ///// <param name="XName">The XML name to use.</param>
        ///// <param name="IncludeMetadata">Include deltaType and lastUpdate meta data.</param>
        ///// <param name="CustomEVSEDataRecordSerializer">A delegate to serialize custom EVSEDataRecord XML elements.</param>
        ///// <param name="CustomAddressSerializer">A delegate to serialize custom Address XML elements.</param>
        ///// <param name="CustomChargingFacilitySerializer">A delegate to serialize custom ChargingFacility XML elements.</param>
        //public XElement ToXML(XName                                          XName                              = null,
        //                      Boolean                                        IncludeMetadata                    = false,
        //                      CustomXMLSerializerDelegate<EVSEDataRecord>    CustomEVSEDataRecordSerializer     = null,
        //                      CustomXMLSerializerDelegate<Address>           CustomAddressSerializer            = null,
        //                      CustomXMLSerializerDelegate<ChargingFacility>  CustomChargingFacilitySerializer   = null)

        //{

        //    var XML = new XElement(OICPNS.EVSEData + "EvseDataRecord",

        //                  IncludeMetadata && DeltaType.HasValue
        //                      ? new XAttribute(OICPNS.EVSEData + "deltaType",  DeltaType.ToString())
        //                      : null,

        //                  IncludeMetadata && LastUpdate.HasValue
        //                      ? new XAttribute(OICPNS.EVSEData + "lastUpdate", LastUpdate.ToString())
        //                      : null,

        //                  new XElement(OICPNS.EVSEData + "EvseID",                Id.ToString()),

        //                  ChargingStationId.HasValue
        //                      ? new XElement(OICPNS.EVSEData + "ChargingStationID",     ChargingStationId.Value.ToString())
        //                      : null,

        //                  ChargingStationName[Languages.de] != null
        //                      ? new XElement(OICPNS.EVSEData + "ChargingStationName",   ChargingStationName[Languages.de].SubstringMax(50))
        //                      : null,

        //                  HardwareManufacturer,

        //                  ChargingStationName[Languages.en] != null
        //                      ? new XElement(OICPNS.EVSEData + "EnChargingStationName", ChargingStationName[Languages.en].SubstringMax(50))
        //                      : null,

        //                  ChargingPoolId.HasValue
        //                      ? new XElement(OICPNS.EVSEData + "ChargingPoolID", ChargingPoolId.Value.ToString())
        //                      : null,

        //                  Address.ToXML(OICPNS.EVSEData + "Address",
        //                                CustomAddressSerializer),

        //                  new XElement(OICPNS.EVSEData + "GeoCoordinates",
        //                      new XElement(OICPNS.CommonTypes + "DecimalDegree",  // Force 0.00... (dot) format!
        //                          new XElement(OICPNS.CommonTypes + "Longitude",  GeoCoordinate.Longitude.ToString("{0:0.######}").Replace(",", ".")),// CultureInfo.InvariantCulture.NumberFormat)),
        //                          new XElement(OICPNS.CommonTypes + "Latitude",   GeoCoordinate.Latitude. ToString("{0:0.######}").Replace(",", ".")) // CultureInfo.InvariantCulture.NumberFormat))
        //                      )
        //                  ),

        //                  new XElement(OICPNS.EVSEData + "Plugs",
        //                      PlugTypes.SafeSelect(Plug => new XElement(OICPNS.EVSEData + "Plug", PlugTypesExtentions.AsString(Plug)))
        //                  ),

        //                  ChargingFacilities.SafeAny()
        //                      ? new XElement(OICPNS.EVSEData + "ChargingFacilities",
        //                            ChargingFacilities.Select(chargingFacility    => chargingFacility.ToXML(CustomChargingFacilitySerializer)))
        //                      : null,

        //                  ChargingModes.SafeAny()
        //                      ? new XElement(OICPNS.EVSEData + "ChargingModes",
        //                            ChargingModes.Select(ChargingMode => new XElement(OICPNS.EVSEData + "ChargingMode", ChargingModesExtentions.AsString(ChargingMode))))
        //                      : null,

        //                  new XElement(OICPNS.EVSEData + "AuthenticationModes",
        //                      AuthenticationModes.SafeSelect(AuthenticationMode => AuthenticationModesExtentions.AsString(AuthenticationMode)).
        //                                          Where     (AuthenticationMode => AuthenticationMode != "Unknown").
        //                                          Select    (AuthenticationMode => new XElement(OICPNS.EVSEData + "AuthenticationMode", AuthenticationMode))
        //                  ),

        //                  MaxCapacity.HasValue
        //                      ? new XElement(OICPNS.EVSEData + "MaxCapacity", MaxCapacity)
        //                      : null,

        //                  PaymentOptions.SafeAny()
        //                      ? new XElement(OICPNS.EVSEData + "PaymentOptions",
        //                            PaymentOptions.SafeSelect(PaymentOption      => new XElement(OICPNS.EVSEData + "PaymentOption", PaymentOptionsExtentions.AsString(PaymentOption)))
        //                        )
        //                      : null,

        //                  ValueAddedServices.SafeAny()
        //                      ? new XElement(OICPNS.EVSEData + "ValueAddedServices",
        //                            ValueAddedServices.SafeSelect(ValueAddedService => new XElement(OICPNS.EVSEData + "ValueAddedService", ValueAddedServicesExtentions.AsString(ValueAddedService)))
        //                        )
        //                      : new XElement(OICPNS.EVSEData + "ValueAddedServices", new XElement(OICPNS.EVSEData + "ValueAddedService", "None")),

        //                  new XElement(OICPNS.EVSEData + "Accessibility",     Accessibility.AsString()),

        //                  HotlinePhoneNumber.IsNotNullOrEmpty()
        //                      ? new XElement(OICPNS.EVSEData + "HotlinePhoneNumber", HotlinePhoneNumberRegExpr.Replace(HotlinePhoneNumber, ""))  // RegEx: \+[0-9]{5,15}
        //                      : null,

        //                  AdditionalInfo.IsNeitherNullNorEmpty()
        //                      ? new XElement(OICPNS.EVSEData + "AdditionalInfo",
        //                                     AdditionalInfo.
        //                                         Select(info => new XElement(OICPNS.EVSEData + "InfoText",
        //                                                                     new XAttribute("lang", info.Language), //ToDo: ISO 639-3 codes for languages is not what OICP expects! Pattern: [a-z]{2,3}(-[A-Z]{2,3}(-[a-zA-Z]{4})?)?(-x-[a-zA-Z0-9]{1,8})?
        //                                                                     info.Text)))
        //                      : null,

        //                  GeoChargingPointEntrance != null
        //                      ? new XElement(OICPNS.EVSEData + "GeoChargingPointEntrance",
        //                          new XElement(OICPNS.CommonTypes + "DecimalDegree",  // Force 0.00... (dot) format!
        //                              new XElement(OICPNS.CommonTypes + "Longitude", GeoChargingPointEntrance.Value.Longitude.ToString("{0:0.######}").Replace(",", ".")),// CultureInfo.InvariantCulture.NumberFormat)),
        //                              new XElement(OICPNS.CommonTypes + "Latitude",  GeoChargingPointEntrance.Value.Latitude. ToString("{0:0.######}").Replace(",", ".")) // CultureInfo.InvariantCulture.NumberFormat))
        //                          )
        //                      )
        //                      : null,

        //                  new XElement(OICPNS.EVSEData + "IsOpen24Hours",         IsOpen24Hours ? "true" : "false"),

        //                  //ToDo: Implement new OpeningTimes complex type!
        //                  //OpeningTimes.IsNotNullOrEmpty()
        //                  //    ? new XElement(OICPNS.EVSEData + "OpeningTime",     OpeningTimes)
        //                  //    : null,

        //                  HubOperatorId != null
        //                      ? new XElement(OICPNS.EVSEData + "HubOperatorID",   HubOperatorId.ToString())
        //                      : null,

        //                  ClearingHouseId != null
        //                      ? new XElement(OICPNS.EVSEData + "ClearinghouseID", ClearingHouseId.ToString())
        //                      : null,

        //                  new XElement(OICPNS.EVSEData + "IsHubjectCompatible",   IsHubjectCompatible  ? "true" : "false"),
        //                  new XElement(OICPNS.EVSEData + "DynamicInfoAvailable",  DynamicInfoAvailable ? "true" : "false")

        //           );

        //    return CustomEVSEDataRecordSerializer != null
        //               ? CustomEVSEDataRecordSerializer(this, XML)
        //               : XML;

        //}

        //#endregion


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
            if (ReferenceEquals(EVSEDataRecord1, EVSEDataRecord2))
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
        //    /// Whether the EVSE provides dynamic status information.
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
        //    /// <param name="DynamicInfoAvailable">Whether the EVSE provides dynamic status information.</param>
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
