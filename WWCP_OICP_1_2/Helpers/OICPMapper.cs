/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/WorldWideCharging/WWCP_OICP>
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

#endregion

namespace org.GraphDefined.WWCP.OICP_1_2
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

            switch (ChargingMode)
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

        #region AsChargingFacility(ChargingFacility)

        /// <summary>
        /// Maps an OICP charging facility to a WWCP charging facility.
        /// </summary>
        /// <param name="ChargingFacility">A charging facility.</param>
        public static ChargingFacilities AsChargingFacility(String ChargingFacility)
        {

            switch (ChargingFacility)
            {

                case "100 - 120V, 1-Phase ≤ 10A":   return ChargingFacilities.CF_100_120V_1Phase_less10A;
                case "100 - 120V, 1-Phase ≤ 16A":   return ChargingFacilities.CF_100_120V_1Phase_less16A;
                case "100 - 120V, 1-Phase ≤ 32A":   return ChargingFacilities.CF_100_120V_1Phase_less32A;
                case "200 - 240V, 1-Phase ≤ 10A":   return ChargingFacilities.CF_200_240V_1Phase_less10A;
                case "200 - 240V, 1-Phase ≤ 16A":   return ChargingFacilities.CF_200_240V_1Phase_less16A;
                case "200 - 240V, 1-Phase ≤ 32A":   return ChargingFacilities.CF_200_240V_1Phase_less32A;
                case "200 - 240V, 1-Phase > 32A":   return ChargingFacilities.CF_200_240V_1Phase_over32A;
                case "380 - 480V, 3-Phase ≤ 16A":   return ChargingFacilities.CF_380_480V_3Phase_less16A;
                case "380 - 480V, 3-Phase ≤ 32A":   return ChargingFacilities.CF_380_480V_3Phase_less32A;
                case "380 - 480V, 3-Phase ≤ 63A":   return ChargingFacilities.CF_380_480V_3Phase_less63A;
                case "Battery exchange":            return ChargingFacilities.Battery_exchange;
                case "DC Charging ≤ 20kW":          return ChargingFacilities.DCCharging_less20kW;
                case "DC Charging ≤ 50kW":          return ChargingFacilities.DCCharging_less50kW;
                case "DC Charging > 50kW":          return ChargingFacilities.DCCharging_over50kW;
                case "Unspecified":                 return ChargingFacilities.Unspecified;

                default: return ChargingFacilities.Unspecified;

            }

        }

        #endregion

        #region AsPlugType(PlugType)

        /// <summary>
        /// Maps an OICP plug type to a WWCP plug type.
        /// </summary>
        /// <param name="PlugType">A plug type.</param>
        public static PlugTypes AsPlugType(String PlugType)
        {

            switch (PlugType)
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
                case "CHAdeMO DC CHAdeMO Connector":        return PlugTypes.CHAdeMO_DC_CHAdeMOConnector;

                default:                                    return PlugTypes.Unspecified;

            }

        }

        #endregion

        #region AsAuthenticationMode(AuthenticationMode)

        /// <summary>
        /// Maps an OICP authentication mode to a WWCP authentication mode.
        /// </summary>
        /// <param name="AuthenticationMode">An authentication mode.</param>
        public static AuthenticationModes AsAuthenticationMode(String AuthenticationMode)
        {

            switch (AuthenticationMode)
            {

                case "NFC_RFID_Classic":   return AuthenticationModes.NFC_RFID_Classic;
                case "NFC_RFID_DESFire":   return AuthenticationModes.NFC_RFID_DESFire;
                case "PnC":                return AuthenticationModes.PnC;
                case "REMOTE":             return AuthenticationModes.REMOTE;
                case "DirectPayment":      return AuthenticationModes.DirectPayment;

                default:                   return AuthenticationModes.Unkown;

            }

        }

        #endregion

        #region AsPaymetOptions(PaymetOptions)

        /// <summary>
        /// Maps an OICP paymet option to a WWCP paymet option.
        /// </summary>
        /// <param name="PaymetOption">A paymet option.</param>
        public static PaymentOptions AsPaymetOption(String PaymetOption)
        {

            switch (PaymetOption)
            {

                case "NoPayment":   return PaymentOptions.Free;
                case "Direct":      return PaymentOptions.Direct;
                case "SMS":         return PaymentOptions.SMS;
                case "Cash":        return PaymentOptions.Cash;
                case "CreditCard":  return PaymentOptions.CreditCard;
                case "Contract":    return PaymentOptions.Contract;

                default:            return PaymentOptions.Unspecified;

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

            switch (AccessibilityType)
            {

                case "Free publicly accessible":    return AccessibilityTypes.Free_publicly_accessible;
                case "Restricted access":           return AccessibilityTypes.Restricted_access;
                case "Paying publicly accessible":  return AccessibilityTypes.Paying_publicly_accessible;

                default:                            return AccessibilityTypes.Unspecified;

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

                case PlugTypes.CHAdeMO_DC_CHAdeMOConnector:
                    return "CHAdeMO DC CHAdeMO Connector";


                default:
                    return "Unspecified";

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
                case PlugTypes.CHAdeMO_DC_CHAdeMOConnector: return "CHAdeMO";

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

    }

}