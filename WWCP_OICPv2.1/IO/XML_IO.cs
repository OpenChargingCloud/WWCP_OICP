/*
 * Copyright (c) 2014-2017 GraphDefined GmbH
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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// OCHP XML I/O.
    /// </summary>
    public static class XML_IO
    {

        #region ParseGeoCoordinatesXML(GeoCoordinatesXML)

        public static GeoCoordinate ParseGeoCoordinatesXML(XElement GeoCoordinatesXML)
        {

            var EVSEGoogleXML              = GeoCoordinatesXML.Element(OICPNS.CommonTypes + "Google");
            var EVSEDecimalDegreeXML       = GeoCoordinatesXML.Element(OICPNS.CommonTypes + "DecimalDegree");
            var EVSEDegreeMinuteSecondsXML = GeoCoordinatesXML.Element(OICPNS.CommonTypes + "DegreeMinuteSeconds");

            if ((EVSEGoogleXML        != null && EVSEDecimalDegreeXML       != null) ||
                (EVSEGoogleXML        != null && EVSEDegreeMinuteSecondsXML != null) ||
                (EVSEDecimalDegreeXML != null && EVSEDegreeMinuteSecondsXML != null))
                throw new ApplicationException("Invalid GeoCoordinates XML tag: Should only include one of the following XML tags Google, DecimalDegree or DegreeMinuteSeconds!");

            if (EVSEGoogleXML != null)
            {
                throw new NotImplementedException("GeoCoordinates Google XML parsing!");
            }

            if (EVSEDecimalDegreeXML != null)
            {

                Longitude LongitudeValue;
                if (!Longitude.TryParse(EVSEDecimalDegreeXML.ElementValueOrFail(OICPNS.CommonTypes + "Longitude", "No GeoCoordinates DecimalDegree Longitude XML tag provided!"), out LongitudeValue))
                    throw new ApplicationException("Invalid Longitude XML tag provided!");

                Latitude LatitudeValue;
                if (!Latitude. TryParse(EVSEDecimalDegreeXML.ElementValueOrFail(OICPNS.CommonTypes + "Latitude",  "No GeoCoordinates DecimalDegree Latitude XML tag provided!"),  out LatitudeValue))
                    throw new ApplicationException("Invalid Latitude XML tag provided!");

                return new GeoCoordinate(LatitudeValue, LongitudeValue);

            }

            if (EVSEDegreeMinuteSecondsXML != null)
            {
                throw new NotImplementedException("GeoCoordinates DegreeMinuteSeconds XML parsing!");
            }

            throw new ApplicationException("Invalid GeoCoordinates XML tag: Should at least include one of the following XML tags Google, DecimalDegree or DegreeMinuteSeconds!");

        }

        #endregion

        #region ParseAddressXML(AddressXML)

        public static Address ParseAddressXML(XElement AddressXML)
        {

            var _CountryTXT = AddressXML.ElementValueOrFail(OICPNS.CommonTypes + "Country", "Missing 'Country'-XML tag!").Trim();

            Country _Country;
            if (!Country.TryParse(_CountryTXT, out _Country))
            {

                if (_CountryTXT.ToUpper() == "UNKNOWN")
                    _Country = Country.unknown;

                else
                    throw new Exception("'" + _CountryTXT + "' is an unknown country name!");

            }

            return new Address(AddressXML.ElementValueOrFail(OICPNS.CommonTypes + "Street", "Missing 'Street'-XML tag!").Trim(),
                               AddressXML.ElementValueOrDefault(OICPNS.CommonTypes + "HouseNum", "").Trim(),
                               AddressXML.ElementValueOrDefault(OICPNS.CommonTypes + "Floor", "").Trim(),
                               AddressXML.ElementValueOrDefault(OICPNS.CommonTypes + "PostalCode", "").Trim(),
                               "",
                               I18NString.Create(Languages.unknown, AddressXML.ElementValueOrFail(OICPNS.CommonTypes + "City", "Missing 'City'-XML tag!").Trim()),
                               _Country);

            // Currently not used OICP address information!
            //var _Region       = AddressXML.       ElementValueOrDefault(OICPNS.OICPv2_0CommonTypes + "Region",     "").Trim();
            //var _Timezone     = AddressXML.       ElementValueOrDefault(OICPNS.OICPv2_0CommonTypes + "Timezone",   "").Trim();


        }

        #endregion


        #region AsDeltaType(Text)

        public static DeltaTypes AsDeltaType(this String Text)
        {

            switch (Text)
            {

                case "update":
                    return DeltaTypes.update;

                case "insert":
                    return DeltaTypes.insert;

                case "delete":
                    return DeltaTypes.delete;

                default:
                    return DeltaTypes.Unknown;

            }

        }

        #endregion

        #region AsText(this DeltaType)

        public static String AsText(this DeltaTypes DeltaType)
        {

            switch (DeltaType)
            {

                case DeltaTypes.update:
                    return "update";

                case DeltaTypes.insert:
                    return "insert";

                case DeltaTypes.delete:
                    return "delete";

                default:
                    return "Unknown";

            }

        }

        #endregion


        #region AsActionType(Text)

        public static ActionTypes AsActionType(this String Text)
        {

            switch (Text)
            {

                case "fullLoad":
                    return ActionTypes.fullLoad;

                case "update":
                    return ActionTypes.update;

                case "insert":
                    return ActionTypes.insert;

                case "delete":
                    return ActionTypes.delete;

                default:
                    return ActionTypes.Unknown;

            }

        }

        #endregion

        #region AsText(this ActionType)

        public static String AsText(this ActionTypes ActionType)
        {

            switch (ActionType)
            {

                case ActionTypes.fullLoad:
                    return "fullLoad";

                case ActionTypes.update:
                    return "update";

                case ActionTypes.insert:
                    return "insert";

                case ActionTypes.delete:
                    return "delete";

                default:
                    return "Unknown";

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

        public static PlugTypes Reduce(this IEnumerable<PlugTypes> OICPPlugTypes)
        {

            var _PlugTypes = PlugTypes.Unspecified;

            foreach (var _PlugType in OICPPlugTypes)
                _PlugTypes |= _PlugType;

            return _PlugTypes;

        }

        public static IEnumerable<PlugTypes> ToEnumeration(this PlugTypes e)

            => Enum.GetValues(typeof(PlugTypes)).
                    Cast<PlugTypes>().
                    Where(flag => e.HasFlag(flag) && flag != PlugTypes.Unspecified);


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

        public static ChargingModes Reduce(this IEnumerable<ChargingModes> ChargingModes)
        {

            var _ChargingModes = OICPv2_1.ChargingModes.Unspecified;

            foreach (var _ChargingMode in ChargingModes)
                _ChargingModes |= _ChargingMode;

            return _ChargingModes;

        }

        public static IEnumerable<ChargingModes> ToEnumeration(this ChargingModes e)
        {

            return Enum.GetValues(typeof(ChargingModes)).
                        Cast<ChargingModes>().
                        Where(flag => e.HasFlag(flag) && flag != ChargingModes.Unspecified);

        }


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

                default:                   return AuthenticationModes.Unknown;

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

        public static AuthenticationModes Reduce(this IEnumerable<AuthenticationModes> AuthenticationModes)
        {

            var _AuthenticationModes = OICPv2_1.AuthenticationModes.Unknown;

            foreach (var _AuthenticationMode in AuthenticationModes)
                _AuthenticationModes |= _AuthenticationMode;

            return _AuthenticationModes;

        }

        public static IEnumerable<AuthenticationModes> ToEnumeration(this AuthenticationModes e)

            => Enum.GetValues(typeof(AuthenticationModes)).
                    Cast<AuthenticationModes>().
                    Where(flag => e.HasFlag(flag) && flag != AuthenticationModes.Unknown);


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

        public static PaymentOptions Reduce(this IEnumerable<PaymentOptions> OICPPaymentOptions)
        {

            var _PaymentOptions = PaymentOptions.Unspecified;

            foreach (var PaymentOption in OICPPaymentOptions)
                _PaymentOptions |= PaymentOption;

            return _PaymentOptions;

        }

        public static IEnumerable<PaymentOptions> ToEnumeration(this PaymentOptions e)
        {

            return Enum.GetValues(typeof(PaymentOptions)).
                        Cast<PaymentOptions>().
                        Where(flag => e.HasFlag(flag) && flag != PaymentOptions.Unspecified);

        }



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

        public static AccessibilityTypes Reduce(this IEnumerable<AccessibilityTypes> OICPAccessibilityTypes)
        {

            var _AccessibilityTypes = AccessibilityTypes.Unspecified;

            foreach (var _AccessibilityType in OICPAccessibilityTypes)
                _AccessibilityTypes |= _AccessibilityType;

            return _AccessibilityTypes;

        }

        public static IEnumerable<AccessibilityTypes> ToEnumeration(this AccessibilityTypes e)
        {

            return Enum.GetValues(typeof(AccessibilityTypes)).
                        Cast<AccessibilityTypes>().
                        Where(flag => e.HasFlag(flag) && flag != AccessibilityTypes.Unspecified);

        }


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

        public static ChargingFacilities Reduce(this IEnumerable<ChargingFacilities> OICPChargingFacilities)
        {

            var _ChargingFacilities = ChargingFacilities.Unspecified;

            foreach (var _ChargingFacility in OICPChargingFacilities)
                _ChargingFacilities |= _ChargingFacility;

            return _ChargingFacilities;

        }

        public static IEnumerable<ChargingFacilities> ToEnumeration(this ChargingFacilities e)
        {

            return Enum.GetValues(typeof(ChargingFacilities)).
                        Cast<ChargingFacilities>().
                        Where(flag => e.HasFlag(flag) && flag != ChargingFacilities.Unspecified);

        }


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

        public static ValueAddedServices Reduce(this IEnumerable<ValueAddedServices> OICPPlugTypes)
        {

            var _PlugTypes = ValueAddedServices.None;

            foreach (var _PlugType in OICPPlugTypes)
                _PlugTypes |= _PlugType;

            return _PlugTypes;

        }

        public static IEnumerable<ValueAddedServices> ToEnumeration(this ValueAddedServices e)

            => Enum.GetValues(typeof(ValueAddedServices)).
                    Cast<ValueAddedServices>().
                    Where(flag => e.HasFlag(flag) && flag != ValueAddedServices.None);



        #region AsEVSEStatusType(EVSEStatusType)

        /// <summary>
        /// Parses the OICP ValueAddedService.
        /// </summary>
        /// <param name="EVSEStatusType">A value added service.</param>
        public static EVSEStatusTypes AsEVSEStatusType(String EVSEStatusType)
        {

            switch (EVSEStatusType.Trim())
            {

                case "Available":
                    return EVSEStatusTypes.Available;

                case "Reserved":
                    return EVSEStatusTypes.Reserved;

                case "Occupied":
                    return EVSEStatusTypes.Occupied;

                case "OutOfService":
                    return EVSEStatusTypes.OutOfService;

                case "EvseNotFound":
                    return EVSEStatusTypes.EvseNotFound;


                default:
                    return EVSEStatusTypes.Unknown;

            }

        }

        #endregion

        #region AsString(EVSEStatusType)

        public static String AsText(this EVSEStatusTypes EVSEStatusType)
        {

            switch (EVSEStatusType)
            {

                case EVSEStatusTypes.Available:
                    return "Available";

                case EVSEStatusTypes.Reserved:
                    return "Reserved";

                case EVSEStatusTypes.Occupied:
                    return "Occupied";

                case EVSEStatusTypes.OutOfService:
                    return "OutOfService";

                case EVSEStatusTypes.EvseNotFound:
                    return "EvseNotFound";


                default:
                    return "Unknown";

            }

        }

        #endregion


    }

}
