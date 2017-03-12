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

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// Helper methods to map OICP data type values to
    /// WWCP data type values and vice versa.
    /// </summary>
    public static class OICPMapper
    {

        #region ToWWCP(this Action)

        /// <summary>
        /// Convert an OICP v2.0 action type into a corresponding WWCP EVSE action type.
        /// </summary>
        /// <param name="ActionType">An OICP v2.0 action type.</param>
        /// <returns>The corresponding WWCP action type.</returns>
        public static WWCP.ActionType ToWWCP(this ActionTypes ActionType)
        {

            switch (ActionType)
            {

                case ActionTypes.fullLoad:
                    return WWCP.ActionType.fullLoad;

                case ActionTypes.update:
                    return WWCP.ActionType.update;

                case ActionTypes.insert:
                    return WWCP.ActionType.insert;

                case ActionTypes.delete:
                    return WWCP.ActionType.delete;

                default:
                    return WWCP.ActionType.fullLoad;

            }

        }

        #endregion

        #region ToOICP(this ActionType)

        /// <summary>
        /// Convert a WWCP action type into a corresponding OICP v2.0 action type.
        /// </summary>
        /// <param name="ActionType">An WWCP action type.</param>
        /// <returns>The corresponding OICP v2.0 action type.</returns>
        public static ActionTypes ToOICP(this WWCP.ActionType ActionType)
        {

            switch (ActionType)
            {

                case WWCP.ActionType.fullLoad:
                    return OICPv2_1.ActionTypes.fullLoad;

                case WWCP.ActionType.update:
                    return OICPv2_1.ActionTypes.update;

                case WWCP.ActionType.insert:
                    return OICPv2_1.ActionTypes.insert;

                case WWCP.ActionType.delete:
                    return OICPv2_1.ActionTypes.delete;

                default:
                    return OICPv2_1.ActionTypes.fullLoad;

            }

        }

        #endregion


        #region ToOICP(this EVSE, EVSE2EVSEDataRecord = null)

        /// <summary>
        /// Convert a WWCP EVSE into a corresponding OICP EVSE data record.
        /// </summary>
        /// <param name="EVSE">A WWCP EVSE.</param>
        /// <param name="EVSE2EVSEDataRecord">A delegate to process an EVSE data record, e.g. before pushing it to a roaming provider.</param>
        /// <returns>The corresponding OICP EVSE data record.</returns>
        public static EVSEDataRecord ToOICP(this WWCP.EVSE                   EVSE,
                                            CPO.EVSE2EVSEDataRecordDelegate  EVSE2EVSEDataRecord = null)
        {

            var _EVSEDataRecord = new EVSEDataRecord(EVSE.Id.ToOICP(),
                                                     new DeltaTypes?(),
                                                     new DateTime?(),
                                                     EVSE.ChargingStation.Id.ToString(),
                                                     EVSE.ChargingStation.Name,
                                                     EVSE.ChargingStation.Address.ToOICP(),
                                                     EVSE.ChargingStation.GeoLocation,
                                                     EVSE.SocketOutlets.SafeSelect(socketoutlet => socketoutlet.Plug).AsOICPPlugTypes(),
                                                     AsChargingFacilities(EVSE).Reduce(),
                                                     EVSE.ChargingModes.AsOICPChargingMode(),
                                                     EVSE.ChargingStation.AuthenticationModes.
                                                                              Select(mode => AsOICPAuthenticationMode(mode)).
                                                                              Where(mode => mode != AuthenticationModes.Unknown).
                                                                              Reduce(),
                                                     null, // MaxCapacity [kWh]
                                                     EVSE.ChargingStation.PaymentOptions.SafeSelect(option => AsOICPPaymentOption(option)).Reduce(),
                                                     ValueAddedServices.None,
                                                     EVSE.ChargingStation.Accessibility.ToOICP(),
                                                     EVSE.ChargingStation.HotlinePhoneNumber,
                                                     EVSE.ChargingStation.Description, // AdditionalInfo
                                                     EVSE.ChargingStation.ChargingPool.EntranceLocation,
                                                     EVSE.ChargingStation.OpeningTimes != null ? EVSE.ChargingStation.OpeningTimes.IsOpen24Hours : true,
                                                     EVSE.ChargingStation.OpeningTimes != null ? EVSE.ChargingStation.OpeningTimes.ToString()    : null,
                                                     null, // HubOperatorId
                                                     null, // ClearingHouseId
                                                     EVSE.ChargingStation.IsHubjectCompatible,
                                                     EVSE.ChargingStation.DynamicInfoAvailable);

            return EVSE2EVSEDataRecord != null
                       ? EVSE2EVSEDataRecord(EVSE, _EVSEDataRecord)
                       : _EVSEDataRecord;

        }

        #endregion


        #region AsWWCPEVSEStatus(this EVSEStatusTypes)

        /// <summary>
        /// Convert an OICP EVSE status into a corresponding WWCP EVSE status.
        /// </summary>
        /// <param name="EVSEStatusTypes">An OICP EVSE status.</param>
        /// <returns>The corresponding WWCP EVSE status.</returns>
        public static WWCP.EVSEStatusTypes AsWWCPEVSEStatus(this EVSEStatusTypes EVSEStatusTypes)
        {

            switch (EVSEStatusTypes)
            {

                case EVSEStatusTypes.Available:
                    return WWCP.EVSEStatusTypes.Available;

                case EVSEStatusTypes.Reserved:
                    return WWCP.EVSEStatusTypes.Reserved;

                case EVSEStatusTypes.Occupied:
                    return WWCP.EVSEStatusTypes.Charging;

                case EVSEStatusTypes.OutOfService:
                    return WWCP.EVSEStatusTypes.OutOfService;

                case EVSEStatusTypes.EvseNotFound:
                    return WWCP.EVSEStatusTypes.UnknownEVSE;

                default:
                    return WWCP.EVSEStatusTypes.Unspecified;

            }

        }

        #endregion

        #region AsOICPEVSEStatus(this EVSEStatusType)

        /// <summary>
        /// Convert a WWCP EVSE status into a corresponding OICP EVSE status.
        /// </summary>
        /// <param name="EVSEStatusType">An WWCP EVSE status.</param>
        /// <returns>The corresponding OICP EVSE status.</returns>
        public static EVSEStatusTypes AsOICPEVSEStatus(this WWCP.EVSEStatusTypes EVSEStatusType)
        {

            switch (EVSEStatusType)
            {

                case WWCP.EVSEStatusTypes.Available:
                    return EVSEStatusTypes.Available;

                case WWCP.EVSEStatusTypes.Reserved:
                    return EVSEStatusTypes.Reserved;

                case WWCP.EVSEStatusTypes.Charging:
                    return EVSEStatusTypes.Occupied;

                case WWCP.EVSEStatusTypes.OutOfService:
                    return EVSEStatusTypes.OutOfService;

                case WWCP.EVSEStatusTypes.UnknownEVSE:
                    return EVSEStatusTypes.EvseNotFound;

                default:
                    return EVSEStatusTypes.Unknown;

            }

        }

        #endregion

        #region AsOICPEVSEStatus(this EVSEStatus)

        /// <summary>
        /// Create a new OICP EVSE status record based on the given WWCP EVSE status.
        /// </summary>
        /// <param name="EVSEStatus">The current status of an EVSE.</param>
        public static EVSEStatusRecord AsOICPEVSEStatus(this WWCP.EVSEStatus EVSEStatus)

        {

            #region Initial checks

            if (EVSEStatus == null)
                throw new ArgumentNullException(nameof(EVSEStatus), "The given EVSE status must not be null!");

            #endregion

            return new EVSEStatusRecord(EVSEStatus.Id.ToOICP(),
                                        AsOICPEVSEStatus(EVSEStatus.Status));

        }

        #endregion


        #region ChargingFacilities

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

        #region ApplyChargingFacilities

        public static void ApplyChargingFacilities(WWCP.EVSE EVSE,
                                                   ChargingFacilities ChargingFacilities)
        {

            foreach (var ChargingFacility in ChargingFacilities.ToEnumeration())
            {

                switch (ChargingFacility)
                {

                    case OICPv2_1.ChargingFacilities.CF_100_120V_1Phase_lessOrEquals10A:

                        if (EVSE.AverageVoltage < 110.0)
                            EVSE.AverageVoltage = 110;

                        if (EVSE.CurrentType == CurrentTypes.Undefined)
                            EVSE.CurrentType = CurrentTypes.AC_OnePhase;

                        if (EVSE.MaxCurrent < 10.0)
                            EVSE.MaxCurrent = 10.0f;

                        break;

                    case OICPv2_1.ChargingFacilities.CF_100_120V_1Phase_lessOrEquals16A:

                        if (EVSE.AverageVoltage < 110.0)
                            EVSE.AverageVoltage = 110;

                        if (EVSE.CurrentType == CurrentTypes.Undefined)
                            EVSE.CurrentType = CurrentTypes.AC_OnePhase;

                        if (EVSE.MaxCurrent < 16.0)
                            EVSE.MaxCurrent = 16.0f;

                        break;


                    case OICPv2_1.ChargingFacilities.CF_100_120V_1Phase_lessOrEquals32A:

                        if (EVSE.AverageVoltage < 110.0)
                            EVSE.AverageVoltage = 110;

                        if (EVSE.CurrentType == CurrentTypes.Undefined)
                            EVSE.CurrentType = CurrentTypes.AC_OnePhase;

                        if (EVSE.MaxCurrent < 32.0)
                            EVSE.MaxCurrent = 32.0f;

                        break;



                    case OICPv2_1.ChargingFacilities.CF_200_240V_1Phase_lessOrEquals10A:

                        if (EVSE.AverageVoltage < 230.0)
                            EVSE.AverageVoltage = 230;

                        if (EVSE.CurrentType == CurrentTypes.Undefined)
                            EVSE.CurrentType = CurrentTypes.AC_OnePhase;

                        if (EVSE.MaxCurrent < 10.0)
                            EVSE.MaxCurrent = 10.0f;

                        break;

                    case OICPv2_1.ChargingFacilities.CF_200_240V_1Phase_lessOrEquals16A:

                        if (EVSE.AverageVoltage < 230.0)
                            EVSE.AverageVoltage = 230;

                        if (EVSE.CurrentType == CurrentTypes.Undefined)
                            EVSE.CurrentType = CurrentTypes.AC_OnePhase;

                        if (EVSE.MaxCurrent < 16.0)
                            EVSE.MaxCurrent = 16.0f;

                        break;

                    case OICPv2_1.ChargingFacilities.CF_200_240V_1Phase_lessOrEquals32A:

                        if (EVSE.AverageVoltage < 230.0)
                            EVSE.AverageVoltage = 230;

                        if (EVSE.CurrentType == CurrentTypes.Undefined)
                            EVSE.CurrentType = CurrentTypes.AC_OnePhase;

                        if (EVSE.MaxCurrent < 32.0)
                            EVSE.MaxCurrent = 32.0f;

                        break;

                    case OICPv2_1.ChargingFacilities.CF_200_240V_1Phase_moreThan32A:

                        if (EVSE.AverageVoltage < 230.0)
                            EVSE.AverageVoltage = 230;

                        if (EVSE.CurrentType == CurrentTypes.Undefined)
                            EVSE.CurrentType = CurrentTypes.AC_OnePhase;

                        if (EVSE.MaxCurrent < 32.0)
                            EVSE.MaxCurrent = 32.0f;

                        break;


                    case OICPv2_1.ChargingFacilities.CF_380_480V_3Phase_lessOrEquals16A:

                        if (EVSE.AverageVoltage < 400.0)
                            EVSE.AverageVoltage = 400;

                        if (EVSE.CurrentType == CurrentTypes.Undefined)
                            EVSE.CurrentType = CurrentTypes.AC_ThreePhases;

                        if (EVSE.MaxCurrent < 16.0)
                            EVSE.MaxCurrent = 16.0f;

                        break;

                    case OICPv2_1.ChargingFacilities.CF_380_480V_3Phase_lessOrEquals32A:

                        if (EVSE.AverageVoltage < 400.0)
                            EVSE.AverageVoltage = 400;

                        if (EVSE.CurrentType == CurrentTypes.Undefined)
                            EVSE.CurrentType = CurrentTypes.AC_ThreePhases;

                        if (EVSE.MaxCurrent < 32.0)
                            EVSE.MaxCurrent = 32.0f;

                        break;

                    case OICPv2_1.ChargingFacilities.CF_380_480V_3Phase_lessOrEquals63A:

                        if (EVSE.AverageVoltage < 400.0)
                            EVSE.AverageVoltage = 400;

                        if (EVSE.CurrentType == CurrentTypes.Undefined)
                            EVSE.CurrentType = CurrentTypes.AC_ThreePhases;

                        if (EVSE.MaxCurrent < 63.0)
                            EVSE.MaxCurrent = 63.0f;

                        break;



                    case OICPv2_1.ChargingFacilities.DCCharging_lessOrEquals20kW:

                        if (EVSE.CurrentType == CurrentTypes.Undefined)
                            EVSE.CurrentType = CurrentTypes.DC;

                        if (EVSE.MaxPower < 20000.0)
                            EVSE.MaxPower = 20000.0f;

                        break;

                    case OICPv2_1.ChargingFacilities.DCCharging_lessOrEquals50kW:

                        if (EVSE.CurrentType == CurrentTypes.Undefined)
                            EVSE.CurrentType = CurrentTypes.DC;

                        if (EVSE.MaxPower < 50000.0)
                            EVSE.MaxPower = 50000.0f;

                        break;

                    case OICPv2_1.ChargingFacilities.DCCharging_moreThan50kW:

                        if (EVSE.CurrentType == CurrentTypes.Undefined)
                            EVSE.CurrentType = CurrentTypes.DC;

                        if (EVSE.MaxPower < 50000.0)
                            EVSE.MaxPower = 50000.0f;

                        break;

                }

            }

        }

        #endregion

        #endregion



        #region ToOICP(this AccessibilityType)

        /// <summary>
        /// Maps a WWCP accessibility type to an OICP  accessibility type.
        /// </summary>
        /// <param name="AccessibilityType">A accessibility type.</param>
        public static AccessibilityTypes ToOICP(this WWCP.AccessibilityTypes AccessibilityType)
        {

            switch (AccessibilityType)
            {

                case WWCP.AccessibilityTypes.Free_publicly_accessible:   return AccessibilityTypes.Free_publicly_accessible;
                case WWCP.AccessibilityTypes.Restricted_access:          return AccessibilityTypes.Restricted_access;
                case WWCP.AccessibilityTypes.Paying_publicly_accessible: return AccessibilityTypes.Paying_publicly_accessible;

                default: return AccessibilityTypes.Unspecified;

            }

        }

        #endregion

        #region ToWWCP(this AccessibilityType)

        /// <summary>
        /// Maps an OICP accessibility type to a WWCP accessibility type.
        /// </summary>
        /// <param name="AccessibilityType">A accessibility type.</param>
        public static WWCP.AccessibilityTypes ToWWCP(this AccessibilityTypes AccessibilityType)
        {

            switch (AccessibilityType)
            {

                case AccessibilityTypes.Free_publicly_accessible:   return WWCP.AccessibilityTypes.Free_publicly_accessible;
                case AccessibilityTypes.Restricted_access:          return WWCP.AccessibilityTypes.Restricted_access;
                case AccessibilityTypes.Paying_publicly_accessible: return WWCP.AccessibilityTypes.Paying_publicly_accessible;

                default: return WWCP.AccessibilityTypes.Unspecified;

            }

        }

        #endregion


        #region ToOICP(this PinCrypto)

        /// <summary>
        /// Maps a WWCP pin crypto type to an OICP pin crypto type.
        /// </summary>
        /// <param name="PinCrypto">A pin crypto type.</param>
        public static PINCrypto ToOICP(this WWCP.PINCrypto PinCrypto)
        {

            switch (PinCrypto)
            {

                case WWCP.PINCrypto.MD5:   return PINCrypto.MD5;
                case WWCP.PINCrypto.SHA1:  return PINCrypto.SHA1;

                default: return PINCrypto.none;

            }

        }

        #endregion

        #region ToWWCP(this PinCrypto)

        /// <summary>
        /// Maps an OICP pin crypto type to a WWCP pin crypto type.
        /// </summary>
        /// <param name="PinCrypto">A pin crypto type.</param>
        public static WWCP.PINCrypto ToWWCP(this PINCrypto PinCrypto)
        {

            switch (PinCrypto)
            {

                case PINCrypto.MD5:  return WWCP.PINCrypto.MD5;
                case PINCrypto.SHA1: return WWCP.PINCrypto.SHA1;

                default: return WWCP.PINCrypto.none;

            }

        }

        #endregion


        #region ToOICP(this WWCPAddress)

        /// <summary>
        /// Maps a WWCP address to an OICP address.
        /// </summary>
        /// <param name="WWCPAddress">A WWCP address.</param>
        public static Address ToOICP(this WWCP.Address WWCPAddress)

            => new Address(WWCPAddress.Street,
                           WWCPAddress.HouseNumber,
                           WWCPAddress.FloorLevel,
                           WWCPAddress.PostalCode,
                           WWCPAddress.PostalCodeSub,
                           WWCPAddress.City,
                           WWCPAddress.Country,
                           WWCPAddress.Comment);


        /// <summary>
        /// Maps an OICP address type to a WWCP address type.
        /// </summary>
        /// <param name="OICPAddress">A address type.</param>
        public static WWCP.Address ToWWCP(this Address OICPAddress)

            => new WWCP.Address(OICPAddress.Street,
                                OICPAddress.HouseNumber,
                                OICPAddress.FloorLevel,
                                OICPAddress.PostalCode,
                                OICPAddress.PostalCodeSub,
                                OICPAddress.City,
                                OICPAddress.Country,
                                OICPAddress.Comment);

        #endregion


        #region PaymentOptions

        #region AsWWCPPaymentOption(PaymentOption)

        public static WWCP.PaymentOptions AsWWCPPaymentOption(this PaymentOptions PaymentOption)
        {

            switch (PaymentOption)
            {

                case PaymentOptions.Free:
                    return WWCP.PaymentOptions.Free;

                case PaymentOptions.Direct:
                    return WWCP.PaymentOptions.Direct;

                case PaymentOptions.Contract:
                    return WWCP.PaymentOptions.Contract;


                default:
                    return WWCP.PaymentOptions.Unspecified;

            }

        }

        #endregion

        #region AsOICPPaymentOption(PaymentOption)

        public static PaymentOptions AsOICPPaymentOption(this WWCP.PaymentOptions PaymentOption)
        {

            switch (PaymentOption)
            {

                case WWCP.PaymentOptions.Free:
                    return PaymentOptions.Free;

                case WWCP.PaymentOptions.Direct:
                    return PaymentOptions.Direct;

                case WWCP.PaymentOptions.Contract:
                    return PaymentOptions.Contract;


                default:
                    return PaymentOptions.Unspecified;

            }

        }

        #endregion

        public static PaymentOptions AsOICPPaymentOptions(this IEnumerable<WWCP.PaymentOptions> WWCPPaymentOptions)
        {

            var _PaymentOptions = PaymentOptions.Unspecified;

            foreach (var WWCPPaymentOption in WWCPPaymentOptions)
                _PaymentOptions |= WWCPPaymentOption.AsOICPPaymentOption();

            return _PaymentOptions;

        }

        #endregion

        #region AuthenticationModes

        #region AsOICPAuthenticationMode(WWCPAuthMode)

        /// <summary>
        /// Maps a WWCP authentication mode to an OICP authentication mode.
        /// </summary>
        /// <param name="WWCPAuthMode">A WWCP-representation of an authentication mode.</param>
        public static AuthenticationModes AsOICPAuthenticationMode(this WWCP.AuthenticationModes WWCPAuthMode)
        {

            var _AuthenticationModes = OICPv2_1.AuthenticationModes.Unknown;

            switch (WWCPAuthMode.Type)
            {

                case "RFID":
                    _AuthenticationModes |= AuthenticationModes.NFC_RFID_Classic;
                    _AuthenticationModes |= AuthenticationModes.NFC_RFID_DESFire;
                    break;

                //case "RFIDMifareDESFire":  return AuthenticationModes.NFC_RFID_DESFire;
                case "ISO/IEC 15118 PLC":
                    _AuthenticationModes |= AuthenticationModes.PnC;
                    break;

                case "REMOTE":
                    _AuthenticationModes |= AuthenticationModes.REMOTE;
                    break;

                case "Direct payment":
                    _AuthenticationModes |= AuthenticationModes.DirectPayment;
                    break;

            }

            return _AuthenticationModes;

        }

        #endregion

        #region AsWWCPAuthenticationMode(AuthenticationMode)

        public static WWCP.AuthenticationModes AsWWCPAuthenticationMode(this AuthenticationModes AuthMode)
        {

            switch (AuthMode)
            {

                case AuthenticationModes.NFC_RFID_Classic:
                    return WWCP.AuthenticationModes.RFID(RFIDAuthenticationModes.MifareClassic);

                case AuthenticationModes.NFC_RFID_DESFire:
                    return WWCP.AuthenticationModes.RFID(RFIDAuthenticationModes.MifareDESFire);

                case AuthenticationModes.PnC:
                    return WWCP.AuthenticationModes.ISO15118_PLC;

                case AuthenticationModes.REMOTE:
                    return WWCP.AuthenticationModes.REMOTE;

                case AuthenticationModes.DirectPayment:
                    return WWCP.AuthenticationModes.DirectPayment;


                default:
                    return WWCP.AuthenticationModes.Unkown;

            }

        }

        #endregion

        public static AuthenticationModes AsOICPAuthenticationModes(this IEnumerable<WWCP.AuthenticationModes> WWCPAuthenticationModes)
        {

            var _AuthenticationModes = AuthenticationModes.Unknown;

            foreach (var WWCPAuthenticationMode in WWCPAuthenticationModes)
                _AuthenticationModes |= WWCPAuthenticationMode.AsOICPAuthenticationMode();

            return _AuthenticationModes;

        }

        #endregion

        #region ChargingModes

        #region AsWWCPChargingMode(ChargingMode)

        public static WWCP.ChargingModes AsWWCPChargingMode(this ChargingModes ChargingMode)
        {

            switch (ChargingMode)
            {

                case ChargingModes.Mode_1:
                    return WWCP.ChargingModes.Mode_1;

                case ChargingModes.Mode_2:
                    return WWCP.ChargingModes.Mode_2;

                case ChargingModes.Mode_3:
                    return WWCP.ChargingModes.Mode_3;

                case ChargingModes.Mode_4:
                    return WWCP.ChargingModes.Mode_4;

                case ChargingModes.CHAdeMO:
                    return WWCP.ChargingModes.CHAdeMO;


                default:
                    return WWCP.ChargingModes.Unspecified;

            }

        }

        #endregion

        #region AsOICPChargingMode(ChargingMode)

        public static ChargingModes AsOICPChargingMode(this WWCP.ChargingModes ChargingMode)
        {

            switch (ChargingMode)
            {

                case WWCP.ChargingModes.Mode_1:
                    return ChargingModes.Mode_1;

                case WWCP.ChargingModes.Mode_2:
                    return ChargingModes.Mode_2;

                case WWCP.ChargingModes.Mode_3:
                    return ChargingModes.Mode_3;

                case WWCP.ChargingModes.Mode_4:
                    return ChargingModes.Mode_4;

                case WWCP.ChargingModes.CHAdeMO:
                    return ChargingModes.CHAdeMO;


                default:
                    return ChargingModes.Unspecified;

            }

        }

        #endregion

        public static ChargingModes AsOICPChargingMode(this IEnumerable<WWCP.ChargingModes> WWCPChargingModes)
        {

            var _ChargingModes = ChargingModes.Unspecified;

            foreach (var WWCPChargingMode in WWCPChargingModes)
                _ChargingModes |= WWCPChargingMode.AsOICPChargingMode();

            return _ChargingModes;

        }

        #endregion

        #region Plugs

        #region AsWWCPPlugTypes(PlugType)

        public static WWCP.PlugTypes AsWWCPPlugTypes(this PlugTypes PlugType)
        {

            switch (PlugType)
            {

                case PlugTypes.SmallPaddleInductive:
                    return WWCP.PlugTypes.SmallPaddleInductive;

                case PlugTypes.LargePaddleInductive:
                    return WWCP.PlugTypes.LargePaddleInductive;

                case PlugTypes.AVCONConnector:
                    return WWCP.PlugTypes.AVCONConnector;

                case PlugTypes.TeslaConnector:
                    return WWCP.PlugTypes.TeslaConnector;

                case PlugTypes.NEMA5_20:
                    return WWCP.PlugTypes.NEMA5_20;

                case PlugTypes.TypeEFrenchStandard:
                    return WWCP.PlugTypes.TypeEFrenchStandard;

                case PlugTypes.TypeFSchuko:
                    return WWCP.PlugTypes.TypeFSchuko;

                case PlugTypes.TypeGBritishStandard:
                    return WWCP.PlugTypes.TypeGBritishStandard;

                case PlugTypes.TypeJSwissStandard:
                    return WWCP.PlugTypes.TypeJSwissStandard;

                case PlugTypes.Type1Connector_CableAttached:
                    return WWCP.PlugTypes.Type1Connector_CableAttached;

                case PlugTypes.Type2Outlet:
                    return WWCP.PlugTypes.Type2Outlet;

                case PlugTypes.Type2Connector_CableAttached:
                    return WWCP.PlugTypes.Type2Connector_CableAttached;

                case PlugTypes.Type3Outlet:
                    return WWCP.PlugTypes.Type3Outlet;

                case PlugTypes.IEC60309SinglePhase:
                    return WWCP.PlugTypes.IEC60309SinglePhase;

                case PlugTypes.IEC60309ThreePhase:
                    return WWCP.PlugTypes.IEC60309ThreePhase;

                case PlugTypes.CCSCombo2Plug_CableAttached:
                    return WWCP.PlugTypes.CCSCombo2Plug_CableAttached;

                case PlugTypes.CCSCombo1Plug_CableAttached:
                    return WWCP.PlugTypes.CCSCombo1Plug_CableAttached;

                case PlugTypes.CHAdeMO:
                    return WWCP.PlugTypes.CHAdeMO;


                default:
                    return WWCP.PlugTypes.Unspecified;

            }

        }

        #endregion

        #region AsOICPPlugTypes(PlugType)

        public static PlugTypes AsOICPPlugTypes(this WWCP.PlugTypes PlugType)
        {

            switch (PlugType)
            {

                case WWCP.PlugTypes.SmallPaddleInductive:
                    return PlugTypes.SmallPaddleInductive;

                case WWCP.PlugTypes.LargePaddleInductive:
                    return PlugTypes.LargePaddleInductive;

                case WWCP.PlugTypes.AVCONConnector:
                    return PlugTypes.AVCONConnector;

                case WWCP.PlugTypes.TeslaConnector:
                    return PlugTypes.TeslaConnector;

                case WWCP.PlugTypes.NEMA5_20:
                    return PlugTypes.NEMA5_20;

                case WWCP.PlugTypes.TypeEFrenchStandard:
                    return PlugTypes.TypeEFrenchStandard;

                case WWCP.PlugTypes.TypeFSchuko:
                    return PlugTypes.TypeFSchuko;

                case WWCP.PlugTypes.TypeGBritishStandard:
                    return PlugTypes.TypeGBritishStandard;

                case WWCP.PlugTypes.TypeJSwissStandard:
                    return PlugTypes.TypeJSwissStandard;

                case WWCP.PlugTypes.Type1Connector_CableAttached:
                    return PlugTypes.Type1Connector_CableAttached;

                case WWCP.PlugTypes.Type2Outlet:
                    return PlugTypes.Type2Outlet;

                case WWCP.PlugTypes.Type2Connector_CableAttached:
                    return PlugTypes.Type2Connector_CableAttached;

                case WWCP.PlugTypes.Type3Outlet:
                    return PlugTypes.Type3Outlet;

                case WWCP.PlugTypes.IEC60309SinglePhase:
                    return PlugTypes.IEC60309SinglePhase;

                case WWCP.PlugTypes.IEC60309ThreePhase:
                    return PlugTypes.IEC60309ThreePhase;

                case WWCP.PlugTypes.CCSCombo2Plug_CableAttached:
                    return PlugTypes.CCSCombo2Plug_CableAttached;

                case WWCP.PlugTypes.CCSCombo1Plug_CableAttached:
                    return PlugTypes.CCSCombo1Plug_CableAttached;

                case WWCP.PlugTypes.CHAdeMO:
                    return PlugTypes.CHAdeMO;


                default:
                    return PlugTypes.Unspecified;

            }

        }

        #endregion

        public static PlugTypes AsOICPPlugTypes(this IEnumerable<WWCP.PlugTypes> WWCPPlugTypes)
        {

            var _PlugTypes = PlugTypes.Unspecified;

            foreach (var WWCPPlugType in WWCPPlugTypes)
                _PlugTypes |= WWCPPlugType.AsOICPPlugTypes();

            return _PlugTypes;

        }

        #endregion


        public static EVSE_Id ToOICP(this WWCP.EVSE_Id EVSEId)
            => EVSE_Id.Parse(EVSEId.ToString());

        public static WWCP.EVSE_Id ToWWCP(this EVSE_Id EVSEId)
            => WWCP.EVSE_Id.Parse(EVSEId.ToString());


        public static Session_Id ToOICP(this WWCP.ChargingSession_Id SessionId)
            => Session_Id.Parse(SessionId.ToString());

        public static WWCP.ChargingSession_Id ToWWCP(this Session_Id SessionId)
            => WWCP.ChargingSession_Id.Parse(SessionId.ToString());


        public static Session_Id? ToOICP(this WWCP.ChargingSession_Id? SessionId)
            => SessionId.HasValue
                   ? Session_Id.Parse(SessionId.ToString())
                   : new Session_Id?();

        public static WWCP.ChargingSession_Id? ToWWCP(this Session_Id? SessionId)
            => SessionId.HasValue
                   ? WWCP.ChargingSession_Id.Parse(SessionId.ToString())
                   : new ChargingSession_Id?();


        public static PartnerProduct_Id? ToOICP(this WWCP.ChargingProduct_Id ProductId)
            => PartnerProduct_Id.Parse(ProductId.ToString());

        public static WWCP.ChargingProduct_Id ToWWCP(this PartnerProduct_Id ProductId)
            => WWCP.ChargingProduct_Id.Parse(ProductId.ToString());


        public static Operator_Id ToOICP(this WWCP.ChargingStationOperator_Id OperatorId)
            => Operator_Id.Parse(OperatorId.ToString());

        public static WWCP.ChargingStationOperator_Id ToWWCP(this Operator_Id OperatorId)
            => WWCP.ChargingStationOperator_Id.Parse(OperatorId.ToString());


        public static Provider_Id ToOICP(this WWCP.eMobilityProvider_Id ProviderId)
            => Provider_Id.Parse(ProviderId.ToString());

        public static WWCP.eMobilityProvider_Id ToWWCP(this Provider_Id ProviderId)
            => WWCP.eMobilityProvider_Id.Parse(ProviderId.ToString());


        public static Provider_Id? ToOICP(this WWCP.eMobilityProvider_Id? ProviderId)
            => ProviderId.HasValue
                   ? Provider_Id.Parse(ProviderId.ToString())
                   : new Provider_Id?();

        public static WWCP.eMobilityProvider_Id? ToWWCP(this Provider_Id? ProviderId)
            => ProviderId.HasValue
                   ? WWCP.eMobilityProvider_Id.Parse(ProviderId.ToString())
                   : new eMobilityProvider_Id?();


        public static EVCO_Id ToOICP(this WWCP.eMobilityAccount_Id eMAId)
            => EVCO_Id.Parse(eMAId.ToString());

        public static WWCP.eMobilityAccount_Id ToWWCP(this EVCO_Id EVCOId)
            => WWCP.eMobilityAccount_Id.Parse(EVCOId.ToString());


        public static EVCO_Id? ToOICP(this WWCP.eMobilityAccount_Id? eMAId)
            => eMAId.HasValue
                   ? EVCO_Id.Parse(eMAId.ToString())
                   : new EVCO_Id?();

        public static WWCP.eMobilityAccount_Id? ToWWCP(this EVCO_Id? EVCOId)
            => EVCOId.HasValue
                   ? WWCP.eMobilityAccount_Id.Parse(EVCOId.ToString())
                   : new eMobilityAccount_Id?();



        public static UID ToOICP(this WWCP.Auth_Token AuthToken)
            => UID.Parse(AuthToken.ToString());

        public static WWCP.Auth_Token ToWWCP(this UID UID)
            => WWCP.Auth_Token.Parse(UID.ToString());


        #region ToOICP(AuthInfo)

        /// <summary>
        /// Create a new identification for authorization based on the given WWCP AuthInfo.
        /// </summary>
        /// <param name="AuthInfo">A WWCP auth info.</param>
        public static AuthorizationIdentification ToOICP(this AuthInfo AuthInfo)
        {

            if (AuthInfo.AuthToken                   != null)
                return AuthorizationIdentification.FromRFIDId                     (AuthInfo.AuthToken.ToOICP());

            if (AuthInfo.QRCodeIdentification        != null)
                return AuthorizationIdentification.FromQRCodeIdentification       (new EVCOIdWithPIN(AuthInfo.QRCodeIdentification.eMAId.ToOICP(),
                                                                                                     AuthInfo.QRCodeIdentification.PIN,
                                                                                                     AuthInfo.QRCodeIdentification.Function.ToOICP(),
                                                                                                     AuthInfo.QRCodeIdentification.Salt));

            if (AuthInfo.PlugAndChargeIdentification.HasValue)
                return AuthorizationIdentification.FromPlugAndChargeIdentification(AuthInfo.PlugAndChargeIdentification.Value.ToOICP());

            if (AuthInfo.RemoteIdentification.       HasValue)
                return AuthorizationIdentification.FromRemoteIdentification       (AuthInfo.RemoteIdentification.       Value.ToOICP());

            throw new ArgumentException("Invalid AuthInfo!", nameof(AuthInfo));

        }

        #endregion


        #region ToWWCP(this ChargeDetailRecord)

        /// <summary>
        /// Convert an OICP charge detail record into a corresponding WWCP charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecord">An OICP charge detail record.</param>
        public static WWCP.ChargeDetailRecord ToWWCP(this ChargeDetailRecord ChargeDetailRecord)

            => new WWCP.ChargeDetailRecord(ChargeDetailRecord.SessionId.ToWWCP(),
                                           EVSEId:                ChargeDetailRecord.EVSEId.ToWWCP(),
                                           ChargingProduct:       ChargeDetailRecord.PartnerProductId.HasValue
                                                                      ? new ChargingProduct(ChargeDetailRecord.PartnerProductId.Value.ToWWCP())
                                                                      : null,
                                           SessionTime:           new StartEndDateTime(ChargeDetailRecord.SessionStart, ChargeDetailRecord.SessionEnd),
                                           EnergyMeteringValues:  new List<Timestamped<Single>> {

                                                                      new Timestamped<Single>(
                                                                          ChargeDetailRecord.ChargingStart.  Value,
                                                                          ChargeDetailRecord.MeterValueStart.Value
                                                                      ),

                                                                      new Timestamped<Single>(
                                                                          ChargeDetailRecord.ChargingEnd.  Value,
                                                                          ChargeDetailRecord.MeterValueEnd.Value
                                                                      )

                                                                  },
                                           //MeterValuesInBetween
                                           //ConsumedEnergy
                                           MeteringSignature:  ChargeDetailRecord.MeteringSignature);

        #endregion

        #region ToOICP(this ChargeDetailRecord, WWCPChargeDetailRecord2OICPChargeDetailRecord = null)

        /// <summary>
        /// Convert a WWCP charge detail record into a corresponding OICP charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecord">A WWCP charge detail record.</param>
        public static ChargeDetailRecord ToOICP(this WWCP.ChargeDetailRecord                               ChargeDetailRecord,
                                                CPO.WWCPChargeDetailRecord2OICPChargeDetailRecordDelegate  WWCPChargeDetailRecord2OICPChargeDetailRecord = null)

        {

            var CDR = new ChargeDetailRecord(
                          ChargeDetailRecord.EVSEId.Value.ToOICP(),
                          ChargeDetailRecord.SessionId.ToOICP(),
                          ChargeDetailRecord.SessionTime.Value.StartTime,
                          ChargeDetailRecord.SessionTime.Value.EndTime.Value,
                          ChargeDetailRecord.IdentificationStart.ToOICP(),
                          ChargeDetailRecord.ChargingProduct?.Id.ToOICP(),
                          null, // PartnerSessionId
                          ChargeDetailRecord.SessionTime.HasValue ? ChargeDetailRecord.SessionTime.Value.StartTime : new DateTime?(),
                          ChargeDetailRecord.SessionTime.HasValue ? ChargeDetailRecord.SessionTime.Value.EndTime : null,
                          ChargeDetailRecord.EnergyMeteringValues != null && ChargeDetailRecord.EnergyMeteringValues.Any() ? ChargeDetailRecord.EnergyMeteringValues.First().Value : new Single?(),
                          ChargeDetailRecord.EnergyMeteringValues != null && ChargeDetailRecord.EnergyMeteringValues.Any() ? ChargeDetailRecord.EnergyMeteringValues.Last(). Value : new Single?(),
                          ChargeDetailRecord.EnergyMeteringValues != null && ChargeDetailRecord.EnergyMeteringValues.Any() ? ChargeDetailRecord.EnergyMeteringValues.Select((Timestamped<Single> v) => v.Value) : null,
                          ChargeDetailRecord.ConsumedEnergy,
                          ChargeDetailRecord.MeteringSignature
                      );

            if (WWCPChargeDetailRecord2OICPChargeDetailRecord != null)
                CDR = WWCPChargeDetailRecord2OICPChargeDetailRecord(ChargeDetailRecord, CDR);

            return CDR;

        }

        #endregion


    }

}
