﻿/*
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
using System.Collections.Generic;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// Helper methods to map OICP data type values to
    /// WWCP data type values and vice versa.
    /// </summary>
    public static class OICPMapper
    {

        #region AsChargingMode(ChargingMode)

        /// <summary>
        /// Maps an OICP charging mode to a WWCP charging mode.
        /// </summary>
        /// <param name="ChargingMode">A charging mode.</param>
        public static ChargingModes AsChargingMode(String ChargingMode)
        {

            switch (ChargingMode.Trim())
            {

                case "Mode_1":   return ChargingModes.Mode_1;
                case "Mode_2":   return ChargingModes.Mode_2;
                case "Mode_3":   return ChargingModes.Mode_3;
                case "Mode_4":   return ChargingModes.Mode_4;
                case "CHAdeMO":  return ChargingModes.CHAdeMO;

                default: return ChargingModes.Unspecified;

            }

        }

        #endregion

        #region AsChargingFacility(Text)

        /// <summary>
        /// Maps an OICP charging facility to a WWCP charging facility.
        /// </summary>
        /// <param name="Text">A charging facility.</param>
        public static ChargingFacilities AsChargingFacility(String Text)
        {

            switch (Text.Trim())
            {

                case "100 - 120V, 1-Phase ≤10A":   return ChargingFacilities.CF_100_120V_1Phase_lessOrEquals10A;
                case "100 - 120V, 1-Phase ≤16A":   return ChargingFacilities.CF_100_120V_1Phase_lessOrEquals16A;
                case "100 - 120V, 1-Phase ≤32A":   return ChargingFacilities.CF_100_120V_1Phase_lessOrEquals32A;
                case "200 - 240V, 1-Phase ≤10A":   return ChargingFacilities.CF_200_240V_1Phase_lessOrEquals10A;
                case "200 - 240V, 1-Phase ≤16A":   return ChargingFacilities.CF_200_240V_1Phase_lessOrEquals16A;
                case "200 - 240V, 1-Phase ≤32A":   return ChargingFacilities.CF_200_240V_1Phase_lessOrEquals32A;
                case "200 - 240V, 1-Phase >32A":   return ChargingFacilities.CF_200_240V_1Phase_moreThan32A;
                case "380 - 480V, 3-Phase ≤16A":   return ChargingFacilities.CF_380_480V_3Phase_lessOrEquals16A;
                case "380 - 480V, 3-Phase ≤32A":   return ChargingFacilities.CF_380_480V_3Phase_lessOrEquals32A;
                case "380 - 480V, 3-Phase ≤63A":   return ChargingFacilities.CF_380_480V_3Phase_lessOrEquals63A;
                case "Battery exchange":           return ChargingFacilities.Battery_exchange;
                case "DC Charging ≤20kW":          return ChargingFacilities.DCCharging_lessOrEquals20kW;
                case "DC Charging ≤50kW":          return ChargingFacilities.DCCharging_lessOrEquals50kW;
                case "DC Charging >50kW":          return ChargingFacilities.DCCharging_moreThan50kW;

                default: return ChargingFacilities.Unspecified;

            }

        }

        #endregion

        #region AsChargingFacilities(WWCP.EVSE)

        /// <summary>
        /// Maps a WWCP EVSE to an OICP charging facility.
        /// </summary>
        /// <param name="EVSE">An WWCP EVSE.</param>
        public static IEnumerable<ChargingFacilities> AsChargingFacilities(WWCP.EVSE EVSE)
        {

            var _ChargingFacilities = new List<ChargingFacilities>();

            #region AC, 1 phase

            if (EVSE.CurrentType == CurrentTypes.AC_OnePhase)
            {

                if (EVSE.AverageVoltage >= 100.0 &&
                    EVSE.AverageVoltage <= 120.0)
                {

                    if (EVSE.MaxCurrent <= 10)
                        _ChargingFacilities.Add(ChargingFacilities.CF_100_120V_1Phase_lessOrEquals10A);

                    else if (EVSE.MaxCurrent <= 16)
                        _ChargingFacilities.Add(ChargingFacilities.CF_100_120V_1Phase_lessOrEquals16A);

                    else if (EVSE.MaxCurrent <= 32)
                        _ChargingFacilities.Add(ChargingFacilities.CF_100_120V_1Phase_lessOrEquals32A);

                }

                if (EVSE.AverageVoltage >= 200.0 &&
                    EVSE.AverageVoltage <= 240.0)
                {

                    if (EVSE.MaxCurrent <= 10)
                        _ChargingFacilities.Add(ChargingFacilities.CF_200_240V_1Phase_lessOrEquals10A);

                    else if (EVSE.MaxCurrent <= 16)
                        _ChargingFacilities.Add(ChargingFacilities.CF_200_240V_1Phase_lessOrEquals16A);

                    else if (EVSE.MaxCurrent <= 32)
                        _ChargingFacilities.Add(ChargingFacilities.CF_200_240V_1Phase_lessOrEquals32A);

                    else if (EVSE.MaxCurrent > 32)
                        _ChargingFacilities.Add(ChargingFacilities.CF_200_240V_1Phase_moreThan32A);

                }

            }

            #endregion

            #region AC, 3 phases

            else if (EVSE.CurrentType == CurrentTypes.AC_ThreePhases)
            {

                if (EVSE.AverageVoltage >= 380.0 &&
                    EVSE.AverageVoltage <= 480.0)
                {

                    if (EVSE.MaxCurrent <= 16)
                        _ChargingFacilities.Add(ChargingFacilities.CF_380_480V_3Phase_lessOrEquals16A);

                    else if (EVSE.MaxCurrent <= 32)
                        _ChargingFacilities.Add(ChargingFacilities.CF_380_480V_3Phase_lessOrEquals32A);

                    else if (EVSE.MaxCurrent <= 63)
                        _ChargingFacilities.Add(ChargingFacilities.CF_380_480V_3Phase_lessOrEquals63A);

                }

            }

            #endregion

            #region DC

            else if (EVSE.CurrentType == CurrentTypes.DC)
            {

                if (EVSE.MaxPower > 50000)
                    _ChargingFacilities.Add(ChargingFacilities.DCCharging_moreThan50kW);

                else if (EVSE.MaxPower <= 20000)
                    _ChargingFacilities.Add(ChargingFacilities.DCCharging_lessOrEquals20kW);

                else if (EVSE.MaxPower <= 50000)
                    _ChargingFacilities.Add(ChargingFacilities.DCCharging_lessOrEquals50kW);

            }

            #endregion

            if (!_ChargingFacilities.Any())
                _ChargingFacilities.Add(ChargingFacilities.Unspecified);

            return _ChargingFacilities;

        }

        #endregion

        #region AsPlugType(PlugType)

        /// <summary>
        /// Maps an OICP plug type to a WWCP plug type.
        /// </summary>
        /// <param name="PlugType">A plug type.</param>
        public static PlugTypes AsPlugType(String PlugType)
        {

            switch (PlugType.Trim())
            {

                case "Small Paddle Inductive":              return PlugTypes.SmallPaddleInductive;
                case "Large Paddle Inductive":              return PlugTypes.LargePaddleInductive;
                case "AVCONConnector":                      return PlugTypes.AVCONConnector;
                case "TeslaConnector":                      return PlugTypes.TeslaConnector;
                case "NEMA 5-20":                           return PlugTypes.NEMA5_20;
                case "Type E French Standard":              return PlugTypes.TypeEFrenchStandard;
                case "Type F Schuko":                       return PlugTypes.TypeFSchuko;
                case "Type G British Standard":             return PlugTypes.TypeGBritishStandard;
                case "Type J Swiss Standard":               return PlugTypes.TypeJSwissStandard;
                case "Type 1 Connector (Cable Attached)":   return PlugTypes.Type1Connector_CableAttached;
                case "Type 2 Outlet":                       return PlugTypes.Type2Outlet;
                case "Type 2 Connector (Cable Attached)":   return PlugTypes.Type2Connector_CableAttached;
                case "Type 3 Outlet":                       return PlugTypes.Type3Outlet;
                case "IEC 60309 Single Phase":              return PlugTypes.IEC60309SinglePhase;
                case "IEC 60309 Three Phase":               return PlugTypes.IEC60309ThreePhase;
                case "CCS Combo 2 Plug (Cable Attached)":   return PlugTypes.CCSCombo2Plug_CableAttached;
                case "CCS Combo 1 Plug (Cable Attached)":   return PlugTypes.CCSCombo1Plug_CableAttached;
                case "CHAdeMO":                             return PlugTypes.CHAdeMO;

                default:                                    return PlugTypes.Unspecified;

            }

        }

        #endregion

        #region AsAuthenticationMode(AuthenticationMode)

        /// <summary>
        /// Maps a string to an OICP authentication mode.
        /// </summary>
        /// <param name="AuthenticationMode">A text-representation of an authentication mode.</param>
        public static AuthenticationModes AsAuthenticationMode(String AuthenticationMode)
        {

            switch (AuthenticationMode.Trim())
            {

                case "NFC RFID Classic":   return AuthenticationModes.NFC_RFID_Classic;
                case "NFC RFID DESFire":   return AuthenticationModes.NFC_RFID_DESFire;
                case "PnC":                return AuthenticationModes.PnC;
                case "REMOTE":             return AuthenticationModes.REMOTE;
                case "Direct Payment":     return AuthenticationModes.DirectPayment;

                default:                   return AuthenticationModes.Unkown;

            }

        }

        #endregion

        #region AsPaymetOptions(PaymetOption)

        /// <summary>
        /// Maps an OICP paymet option to a WWCP paymet option.
        /// </summary>
        /// <param name="PaymetOption">A paymet option.</param>
        public static PaymentOptions AsPaymetOption(String PaymetOption)
        {

            switch (PaymetOption.Trim())
            {

                case "NoPayment":
                    return PaymentOptions.Free;

                case "Direct":
                    return PaymentOptions.Direct;

                case "Contract":
                    return PaymentOptions.Contract;

                default:
                    return PaymentOptions.Unspecified;

            }

        }

        #endregion

        #region AsValueAddedService(ValueAddedService)

        /// <summary>
        /// Parses the OICP ValueAddedService.
        /// </summary>
        /// <param name="ValueAddedService">A value added service.</param>
        public static ValueAddedServices AsValueAddedService(String ValueAddedService)
        {

            switch (ValueAddedService.Trim())
            {

                case "Reservation":
                    return ValueAddedServices.Reservation;

                case "DynamicPricing":
                    return ValueAddedServices.DynamicPricing;

                case "ParkingSensors":
                    return ValueAddedServices.ParkingSensors;

                case "MaximumPowerCharging":
                    return ValueAddedServices.MaximumPowerCharging;

                case "PredictiveChargePointUsage":
                    return ValueAddedServices.PredictiveChargePointUsage;

                case "ChargingPlans":
                    return ValueAddedServices.ChargingPlans;

                default:
                    return ValueAddedServices.None;

            }

        }

        #endregion

        #region AsAccessibilityType(AccessibilityType)

        /// <summary>
        /// Maps an OICP accessibility type to a WWCP accessibility type.
        /// </summary>
        /// <param name="AccessibilityType">A accessibility type.</param>
        public static AccessibilityTypes AsAccessibilityType(String AccessibilityType)
        {

            switch (AccessibilityType.Trim())
            {

                case "Free publicly accessible":    return AccessibilityTypes.Free_publicly_accessible;
                case "Restricted access":           return AccessibilityTypes.Restricted_access;
                case "Paying publicly accessible":  return AccessibilityTypes.Paying_publicly_accessible;

                default:                            return AccessibilityTypes.Unspecified;

            }

        }

        #endregion


        #region AsString(ChargingMode)

        public static String AsString(this ChargingModes ChargingMode)
        {

            switch (ChargingMode)
            {

                case ChargingModes.Mode_1:
                    return "Mode_1";

                case ChargingModes.Mode_2:
                    return "Mode_2";

                case ChargingModes.Mode_3:
                    return "Mode_3";

                case ChargingModes.Mode_4:
                    return "Mode_4";

                case ChargingModes.CHAdeMO:
                    return "CHAdeMO";


                default:
                    return "Unspecified";

            }

        }

        #endregion

        #region AsString(ChargingFacility)

        public static String AsString(this ChargingFacilities ChargingFacility)
        {

            switch (ChargingFacility)
            {

                case ChargingFacilities.CF_100_120V_1Phase_lessOrEquals10A:
                    return "100 - 120V, 1-Phase ≤10A";

                case ChargingFacilities.CF_100_120V_1Phase_lessOrEquals16A:
                    return "100 - 120V, 1-Phase ≤16A";

                case ChargingFacilities.CF_100_120V_1Phase_lessOrEquals32A:
                    return "100 - 120V, 1-Phase ≤32A";

                case ChargingFacilities.CF_200_240V_1Phase_lessOrEquals10A:
                    return "200 - 240V, 1-Phase ≤10A";

                case ChargingFacilities.CF_200_240V_1Phase_lessOrEquals16A:
                    return "200 - 240V, 1-Phase ≤16A";

                case ChargingFacilities.CF_200_240V_1Phase_lessOrEquals32A:
                    return "200 - 240V, 1-Phase ≤32A";

                case ChargingFacilities.CF_200_240V_1Phase_moreThan32A:
                    return "200 - 240V, 1-Phase >32A";

                case ChargingFacilities.CF_380_480V_3Phase_lessOrEquals16A:
                    return "380 - 480V, 3-Phase ≤16A";

                case ChargingFacilities.CF_380_480V_3Phase_lessOrEquals32A:
                    return "380 - 480V, 3-Phase ≤32A";

                case ChargingFacilities.CF_380_480V_3Phase_lessOrEquals63A:
                    return "380 - 480V, 3-Phase ≤63A";

                case ChargingFacilities.Battery_exchange:
                    return "Battery exchange";

                case ChargingFacilities.DCCharging_lessOrEquals20kW:
                    return "DC Charging ≤20kW";

                case ChargingFacilities.DCCharging_lessOrEquals50kW:
                    return "DC Charging ≤50kW";

                case ChargingFacilities.DCCharging_moreThan50kW:
                    return "DC Charging >50kW";


                default:
                    return "Unspecified";

            }

        }

        #endregion

        #region AsString(PlugType)

        public static String AsString(this PlugTypes PlugType)
        {

            switch (PlugType)
            {

                case PlugTypes.SmallPaddleInductive:
                    return "Small Paddle Inductive";

                case PlugTypes.LargePaddleInductive:
                    return "Large Paddle Inductive";

                case PlugTypes.AVCONConnector:
                    return "AVCONConnector";

                case PlugTypes.TeslaConnector:
                    return "TeslaConnector";

                case PlugTypes.NEMA5_20:
                    return "NEMA 5-20";

                case PlugTypes.TypeEFrenchStandard:
                    return "Type E French Standard";

                case PlugTypes.TypeFSchuko:
                    return "Type F Schuko";

                case PlugTypes.TypeGBritishStandard:
                    return "Type G British Standard";

                case PlugTypes.TypeJSwissStandard:
                    return "Type J Swiss Standard";

                case PlugTypes.Type1Connector_CableAttached:
                    return "Type 1 Connector (Cable Attached)";

                case PlugTypes.Type2Outlet:
                    return "Type 2 Outlet";

                case PlugTypes.Type2Connector_CableAttached:
                    return "Type 2 Connector (Cable Attached)";

                case PlugTypes.Type3Outlet:
                    return "Type 3 Outlet";

                case PlugTypes.IEC60309SinglePhase:
                    return "IEC 60309 Single Phase";

                case PlugTypes.IEC60309ThreePhase:
                    return "IEC 60309 Three Phase";

                case PlugTypes.CCSCombo2Plug_CableAttached:
                    return "CCS Combo 2 Plug (Cable Attached)";

                case PlugTypes.CCSCombo1Plug_CableAttached:
                    return "CCS Combo 1 Plug (Cable Attached)";

                case PlugTypes.CHAdeMO:
                    return "CHAdeMO";


                default:
                    return "Unspecified";

            }

        }

        #endregion

        #region AsString(AuthenticationMode)

        public static String AsString(this AuthenticationModes AuthenticationMode)
        {

            switch (AuthenticationMode)
            {

                case AuthenticationModes.NFC_RFID_Classic:
                    return "NFC RFID Classic";

                case AuthenticationModes.NFC_RFID_DESFire:
                    return "NFC RFID DESFire";

                case AuthenticationModes.PnC:
                    return "PnC";

                case AuthenticationModes.REMOTE:
                    return "REMOTE";

                case AuthenticationModes.DirectPayment:
                    return "Direct Payment";


                default:
                    return "Unkown";

            }

        }

        #endregion

        #region AsString(SocketOutlet)

        public static String AsString(SocketOutlet SocketOutlet)
        {

            switch (SocketOutlet.Plug)
            {

                // Type F Schuko                      -> CEE 7/4
                case PlugTypes.TypeFSchuko: return "Type F Schuko";

                // Type 2 Outlet                      -> IEC 62196-1 type 2
                // Type 2 Connector (Cable Attached)  -> Cable attached to IEC 62196-1 type 2 connector.
                //case PlugType.IEC62196_Type_2   : return (SocketOutlet.CableAttached == CableType.attached)
                //                                  ? "Type 2 Connector (Cable Attached)"
                //                                  : "Type 2 Outlet";
                case PlugTypes.Type2Outlet: return "Type 2 Outlet";
                case PlugTypes.Type2Connector_CableAttached: return "Type 2 Connector (Cable Attached)";

                // CHAdeMO, DC CHAdeMO Connector
                case PlugTypes.CHAdeMO: return "CHAdeMO";

                // Small Paddle Inductive
                // Large Paddle Inductive
                // AVCON Connector
                // Tesla Connector
                // NEMA 5-20
                // Type E French Standard                  CEE 7/5
                // Type G British Standard                 BS 1363
                // Type J Swiss Standard                   SEV 1011
                // Type 1 Connector (Cable Attached)       Cable attached to IEC 62196-1 type 1, SAE J1772 connector.
                // Type 3 Outlet                           IEC 62196-1 type 3
                // IEC 60309 Single Phase                  IEC 60309
                // IEC 60309 Three Phase                   IEC 60309
                // CCS Combo 2 Plug (Cable Attached)       IEC 62196-3 CDV DC Combined Charging Connector DIN SPEC 70121 refers to ISO / IEC 15118-1 DIS, -2 DIS and 15118-3
                // CCS Combo 1 Plug (Cable Attached)       IEC 62196-3 CDV DC Combined Charging Connector with IEC 62196-1 type 2 SAE J1772 connector

                default: return "Unspecified";

            }

        }

        #endregion

        #region AsString(PaymentOption)

        public static String AsString(this PaymentOptions PaymentOption)
        {

            switch (PaymentOption)
            {

                case PaymentOptions.Free:
                    return "NoPayment";

                case PaymentOptions.Direct:
                    return "Direct";

                case PaymentOptions.Contract:
                    return "Contract";


                default:
                    return "Unkown";

            }

        }

        #endregion

        #region AsString(ValueAddedService)

        public static String AsString(this ValueAddedServices ValueAddedService)
        {

            switch (ValueAddedService)
            {

                case ValueAddedServices.Reservation:
                    return "Reservation";

                case ValueAddedServices.DynamicPricing:
                    return "DynamicPricing";

                case ValueAddedServices.ParkingSensors:
                    return "ParkingSensors";

                case ValueAddedServices.MaximumPowerCharging:
                    return "MaximumPowerCharging";

                case ValueAddedServices.PredictiveChargePointUsage:
                    return "PredictiveChargePointUsage";

                case ValueAddedServices.ChargingPlans:
                    return "ChargingPlans";

                default:
                    return "None";

            }

        }

        #endregion

        #region AsString(AccessibilityType)

        public static String AsString(this AccessibilityTypes AccessibilityType)
        {

            switch (AccessibilityType)
            {

                case AccessibilityTypes.Free_publicly_accessible:
                    return "Free publicly accessible";

                case AccessibilityTypes.Restricted_access:
                    return "Restricted access";

                case AccessibilityTypes.Paying_publicly_accessible:
                    return "Paying publicly accessible";


                default:
                    return "Unspecified";

            }

        }

        #endregion


        #region AsOICPEVSEDataRecord(this EVSE, EVSE2EVSEDataRecord = null)

        /// <summary>
        /// Convert a WWCP EVSE into a corresponding OICP EVSE data record.
        /// </summary>
        /// <param name="EVSE">A WWCP EVSE.</param>
        /// <param name="EVSE2EVSEDataRecord">A delegate to process an EVSE data record, e.g. before pushing it to a roaming provider.</param>
        /// <returns>The corresponding OICP EVSE data record.</returns>
        public static EVSEDataRecord AsOICPEVSEDataRecord(this WWCP.EVSE               EVSE,
                                                          EVSE2EVSEDataRecordDelegate  EVSE2EVSEDataRecord = null)
        {

            var _EVSEDataRecord = new EVSEDataRecord(EVSE,
                                                     EVSE.ChargingStation.Id.ToFormat(IdFormatType.NEW).ToString(),
                                                     EVSE.ChargingStation.Name,
                                                     EVSE.ChargingStation.Address,
                                                     EVSE.ChargingStation.GeoLocation,
                                                     EVSE.SocketOutlets?.Select(socketoutlet => socketoutlet.Plug),
                                                     OICPMapper.AsChargingFacilities(EVSE),
                                                     EVSE.ChargingModes,
                                                     EVSE.ChargingStation.AuthenticationModes?.
                                                                              SelectMany(mode => OICPMapper.AsOICPAuthenticationMode(mode)).
                                                                              Where     (mode => mode != AuthenticationModes.Unkown),
                                                     null, // MaxCapacity [kWh]
                                                     EVSE.ChargingStation.PaymentOptions,
                                                     EVSE.ChargingStation.Accessibility,
                                                     EVSE.ChargingStation.HotlinePhoneNumber,
                                                     EVSE.ChargingStation.Description, // AdditionalInfo
                                                     EVSE.ChargingStation.ChargingPool.EntranceLocation,
                                                     EVSE.ChargingStation.OpeningTimes?.IsOpen24Hours,
                                                     EVSE.ChargingStation.OpeningTimes,
                                                     null, // HubOperatorId
                                                     null, // ClearingHouseId
                                                     EVSE.ChargingStation.IsHubjectCompatible,
                                                     EVSE.ChargingStation.DynamicInfoAvailable);

            return EVSE2EVSEDataRecord != null
                       ? EVSE2EVSEDataRecord(EVSE, _EVSEDataRecord)
                       : _EVSEDataRecord;

        }

        #endregion


        #region AsWWCPEVSEStatus(this EVSEStatus)

        /// <summary>
        /// Convert an OICP v2.0 EVSE status into a corresponding WWCP EVSE status.
        /// </summary>
        /// <param name="EVSEStatus">An OICP v2.0 EVSE status.</param>
        /// <returns>The corresponding WWCP EVSE status.</returns>
        public static WWCP.EVSEStatusType AsWWCPEVSEStatus(this EVSEStatusType EVSEStatus)
        {

            switch (EVSEStatus)
            {

                case EVSEStatusType.Available:
                    return WWCP.EVSEStatusType.Available;

                case EVSEStatusType.Reserved:
                    return WWCP.EVSEStatusType.Reserved;

                case EVSEStatusType.Occupied:
                    return WWCP.EVSEStatusType.Charging;

                case EVSEStatusType.OutOfService:
                    return WWCP.EVSEStatusType.OutOfService;

                case EVSEStatusType.EvseNotFound:
                    return WWCP.EVSEStatusType.UnknownEVSE;

                default:
                    return WWCP.EVSEStatusType.Unspecified;

            }

        }

        #endregion

        #region AsOICPEVSEStatus(this EVSEStatus)

        /// <summary>
        /// Convert a WWCP EVSE status into a corresponding OICP v2.0 EVSE status.
        /// </summary>
        /// <param name="EVSEStatus">An WWCP EVSE status.</param>
        /// <returns>The corresponding OICP v2.0 EVSE status.</returns>
        public static EVSEStatusType AsOICPEVSEStatus(this WWCP.EVSEStatusType EVSEStatus)
        {

            switch (EVSEStatus)
            {

                case WWCP.EVSEStatusType.Available:
                    return EVSEStatusType.Available;

                case WWCP.EVSEStatusType.Reserved:
                    return EVSEStatusType.Reserved;

                case WWCP.EVSEStatusType.Charging:
                    return EVSEStatusType.Occupied;

                case WWCP.EVSEStatusType.OutOfService:
                    return EVSEStatusType.OutOfService;

                case WWCP.EVSEStatusType.UnknownEVSE:
                    return EVSEStatusType.EvseNotFound;

                default:
                    return EVSEStatusType.Unknown;

            }

        }

        #endregion


        #region AsWWCPActionType(this Action)

        /// <summary>
        /// Convert an OICP v2.0 action type into a corresponding WWCP EVSE action type.
        /// </summary>
        /// <param name="ActionType">An OICP v2.0 action type.</param>
        /// <returns>The corresponding WWCP action type.</returns>
        public static WWCP.ActionType AsWWCPActionType(this ActionType ActionType)
        {

            switch (ActionType)
            {

                case ActionType.fullLoad:
                    return WWCP.ActionType.fullLoad;

                case ActionType.update:
                    return WWCP.ActionType.update;

                case ActionType.insert:
                    return WWCP.ActionType.insert;

                case ActionType.delete:
                    return WWCP.ActionType.delete;

                default:
                    return WWCP.ActionType.fullLoad;

            }

        }

        #endregion

        #region AsOICPActionType(this ActionType)

        /// <summary>
        /// Convert a WWCP action type into a corresponding OICP v2.0 action type.
        /// </summary>
        /// <param name="ActionType">An WWCP action type.</param>
        /// <returns>The corresponding OICP v2.0 action type.</returns>
        public static ActionType AsOICPActionType(this WWCP.ActionType ActionType)
        {

            switch (ActionType)
            {

                case WWCP.ActionType.fullLoad:
                    return OICPv2_1.ActionType.fullLoad;

                case WWCP.ActionType.update:
                    return OICPv2_1.ActionType.update;

                case WWCP.ActionType.insert:
                    return OICPv2_1.ActionType.insert;

                case WWCP.ActionType.delete:
                    return OICPv2_1.ActionType.delete;

                default:
                    return OICPv2_1.ActionType.fullLoad;

            }

        }

        #endregion


        #region AsOICPAuthenticationMode(AuthenticationMode)

        /// <summary>
        /// Maps a WWCP authentication mode to an OICP authentication mode.
        /// </summary>
        /// <param name="AuthMode">A WWCP-representation of an authentication mode.</param>
        public static IEnumerable<AuthenticationModes> AsOICPAuthenticationMode(AuthenticationMode AuthMode)
        {

            switch (AuthMode.Type)
            {

                case "RFID":               return new AuthenticationModes[] { AuthenticationModes.NFC_RFID_Classic, AuthenticationModes.NFC_RFID_DESFire };
                //case "RFIDMifareDESFire":  return AuthenticationModes.NFC_RFID_DESFire;
                case "ISO/IEC 15118 PLC":  return new AuthenticationModes[] { AuthenticationModes.PnC };
                case "REMOTE":             return new AuthenticationModes[] { AuthenticationModes.REMOTE };
                case "Direct payment":     return new AuthenticationModes[] { AuthenticationModes.DirectPayment };

                default:                   return new AuthenticationModes[] { AuthenticationModes.Unkown };

        }

        }

        #endregion

        #region AsWWCPAuthenticationMode(AuthenticationMode)

        public static AuthenticationMode AsWWCPAuthenticationMode(this AuthenticationModes AuthMode)
        {

            switch (AuthMode)
            {

                case AuthenticationModes.NFC_RFID_Classic:
                    return AuthenticationMode.RFID(RFIDAuthenticationModes.MifareClassic);

                case AuthenticationModes.NFC_RFID_DESFire:
                    return AuthenticationMode.RFID(RFIDAuthenticationModes.MifareDESFire);

                case AuthenticationModes.PnC:
                    return AuthenticationMode.ISO15118_PLC;

                case AuthenticationModes.REMOTE:
                    return AuthenticationMode.REMOTE;

                case AuthenticationModes.DirectPayment:
                    return AuthenticationMode.DirectPayment;


                default:
                    return AuthenticationMode.Unkown;

            }

        }

        #endregion


        #region AsWWCPChargeDetailRecord(this ChargeDetailRecord)

        /// <summary>
        /// Convert an OICP v2.0 EVSE charge detail record into a corresponding WWCP charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecord">An OICP v2.0 charge detail record.</param>
        /// <returns>The corresponding WWCP EVSE status.</returns>
        public static WWCP.ChargeDetailRecord AsWWCPChargeDetailRecord(this ChargeDetailRecord ChargeDetailRecord)
        {

            return new WWCP.ChargeDetailRecord(ChargeDetailRecord.SessionId,
                                               EVSEId:             ChargeDetailRecord.EVSEId,
                                               ChargingProductId:  ChargeDetailRecord.PartnerProductId,
                                               SessionTime:        new StartEndDateTime(ChargeDetailRecord.SessionStart, ChargeDetailRecord.SessionEnd),
                                               EnergyMeteringValues:  new List<Timestamped<double>>() { new Timestamped<double>(ChargeDetailRecord.ChargingStart.Value, ChargeDetailRecord.MeterValueStart.Value),
                                                                                                     new Timestamped<double>(ChargeDetailRecord.ChargingEnd.Value,   ChargeDetailRecord.MeterValueEnd.Value) },
                                               //MeterValuesInBetween
                                               //ConsumedEnergy
                                               MeteringSignature:  ChargeDetailRecord.MeteringSignature);

        }

        #endregion


        #region ApplyChargingFacilities

        public static void ApplyChargingFacilities(WWCP.EVSE EVSE,
                                                   IEnumerable<ChargingFacilities> ChargingFacilities)
        {

            foreach (var ChargingFacility in ChargingFacilities)
            {

                switch (ChargingFacility)
                {

                    case OICPv2_1.ChargingFacilities.CF_100_120V_1Phase_lessOrEquals10A:

                        if (EVSE.AverageVoltage < 110.0)
                            EVSE.AverageVoltage = 110;

                        if (EVSE.CurrentType == CurrentTypes.Undefined)
                            EVSE.CurrentType = CurrentTypes.AC_OnePhase;

                        if (EVSE.MaxCurrent < 10.0)
                            EVSE.MaxCurrent = 10.0;

                        break;

                    case OICPv2_1.ChargingFacilities.CF_100_120V_1Phase_lessOrEquals16A:

                        if (EVSE.AverageVoltage < 110.0)
                            EVSE.AverageVoltage = 110;

                        if (EVSE.CurrentType == CurrentTypes.Undefined)
                            EVSE.CurrentType = CurrentTypes.AC_OnePhase;

                        if (EVSE.MaxCurrent < 16.0)
                            EVSE.MaxCurrent = 16.0;

                        break;


                    case OICPv2_1.ChargingFacilities.CF_100_120V_1Phase_lessOrEquals32A:

                        if (EVSE.AverageVoltage < 110.0)
                            EVSE.AverageVoltage = 110;

                        if (EVSE.CurrentType == CurrentTypes.Undefined)
                            EVSE.CurrentType = CurrentTypes.AC_OnePhase;

                        if (EVSE.MaxCurrent < 32.0)
                            EVSE.MaxCurrent = 32.0;

                        break;



                    case OICPv2_1.ChargingFacilities.CF_200_240V_1Phase_lessOrEquals10A:

                        if (EVSE.AverageVoltage < 230.0)
                            EVSE.AverageVoltage = 230;

                        if (EVSE.CurrentType == CurrentTypes.Undefined)
                            EVSE.CurrentType = CurrentTypes.AC_OnePhase;

                        if (EVSE.MaxCurrent < 10.0)
                            EVSE.MaxCurrent = 10.0;

                        break;

                    case OICPv2_1.ChargingFacilities.CF_200_240V_1Phase_lessOrEquals16A:

                        if (EVSE.AverageVoltage < 230.0)
                            EVSE.AverageVoltage = 230;

                        if (EVSE.CurrentType == CurrentTypes.Undefined)
                            EVSE.CurrentType = CurrentTypes.AC_OnePhase;

                        if (EVSE.MaxCurrent < 16.0)
                            EVSE.MaxCurrent = 16.0;

                        break;

                    case OICPv2_1.ChargingFacilities.CF_200_240V_1Phase_lessOrEquals32A:

                        if (EVSE.AverageVoltage < 230.0)
                            EVSE.AverageVoltage = 230;

                        if (EVSE.CurrentType == CurrentTypes.Undefined)
                            EVSE.CurrentType = CurrentTypes.AC_OnePhase;

                        if (EVSE.MaxCurrent < 32.0)
                            EVSE.MaxCurrent = 32.0;

                        break;

                    case OICPv2_1.ChargingFacilities.CF_200_240V_1Phase_moreThan32A:

                        if (EVSE.AverageVoltage < 230.0)
                            EVSE.AverageVoltage = 230;

                        if (EVSE.CurrentType == CurrentTypes.Undefined)
                            EVSE.CurrentType = CurrentTypes.AC_OnePhase;

                        if (EVSE.MaxCurrent < 32.0)
                            EVSE.MaxCurrent = 32.0;

                        break;


                    case OICPv2_1.ChargingFacilities.CF_380_480V_3Phase_lessOrEquals16A:

                        if (EVSE.AverageVoltage < 400.0)
                            EVSE.AverageVoltage = 400;

                        if (EVSE.CurrentType == CurrentTypes.Undefined)
                            EVSE.CurrentType = CurrentTypes.AC_ThreePhases;

                        if (EVSE.MaxCurrent < 16.0)
                            EVSE.MaxCurrent = 16.0;

                        break;

                    case OICPv2_1.ChargingFacilities.CF_380_480V_3Phase_lessOrEquals32A:

                        if (EVSE.AverageVoltage < 400.0)
                            EVSE.AverageVoltage = 400;

                        if (EVSE.CurrentType == CurrentTypes.Undefined)
                            EVSE.CurrentType = CurrentTypes.AC_ThreePhases;

                        if (EVSE.MaxCurrent < 32.0)
                            EVSE.MaxCurrent = 32.0;

                        break;

                    case OICPv2_1.ChargingFacilities.CF_380_480V_3Phase_lessOrEquals63A:

                        if (EVSE.AverageVoltage < 400.0)
                            EVSE.AverageVoltage = 400;

                        if (EVSE.CurrentType == CurrentTypes.Undefined)
                            EVSE.CurrentType = CurrentTypes.AC_ThreePhases;

                        if (EVSE.MaxCurrent < 63.0)
                            EVSE.MaxCurrent = 63.0;

                        break;



                    case OICPv2_1.ChargingFacilities.DCCharging_lessOrEquals20kW:

                        if (EVSE.CurrentType == CurrentTypes.Undefined)
                            EVSE.CurrentType = CurrentTypes.DC;

                        if (EVSE.MaxPower < 20000.0)
                            EVSE.MaxPower = 20000.0;

                        break;

                    case OICPv2_1.ChargingFacilities.DCCharging_lessOrEquals50kW:

                        if (EVSE.CurrentType == CurrentTypes.Undefined)
                            EVSE.CurrentType = CurrentTypes.DC;

                        if (EVSE.MaxPower < 50000.0)
                            EVSE.MaxPower = 50000.0;

                        break;

                    case OICPv2_1.ChargingFacilities.DCCharging_moreThan50kW:

                        if (EVSE.CurrentType == CurrentTypes.Undefined)
                            EVSE.CurrentType = CurrentTypes.DC;

                        if (EVSE.MaxPower < 50000.0)
                            EVSE.MaxPower = 50000.0;

                        break;

                }

            }

        }

        #endregion


    }

}