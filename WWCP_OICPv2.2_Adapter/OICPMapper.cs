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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2
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
                    return OICPv2_2.ActionTypes.fullLoad;

                case WWCP.ActionType.update:
                    return OICPv2_2.ActionTypes.update;

                case WWCP.ActionType.insert:
                    return OICPv2_2.ActionTypes.insert;

                case WWCP.ActionType.delete:
                    return OICPv2_2.ActionTypes.delete;

                default:
                    return OICPv2_2.ActionTypes.fullLoad;

            }

        }

        #endregion


        #region ToWWCP(this EVSEDataRecord, ..., EVSEDataRecord2EVSE = null)

        /// <summary>
        /// Convert an OICP EVSE data record into a corresponding WWCP EVSE.
        /// </summary>
        /// <param name="EVSEDataRecord">An EVSE data record.</param>
        /// <param name="EVSEDataRecord2EVSE">A delegate to process an EVSE data record, e.g. before importing it into a roaming network.</param>
        public static EVSE ToWWCP(this EVSEDataRecord              EVSEDataRecord,

                                  String                           DataSource                              = "",
                                  EVSEAdminStatusTypes             InitialEVSEAdminStatus                  = EVSEAdminStatusTypes.OutOfService,
                                  ChargingStationAdminStatusTypes  InitialChargingStationAdminStatus       = ChargingStationAdminStatusTypes.OutOfService,
                                  WWCP.EVSEStatusTypes             InitialEVSEStatus                       = WWCP.EVSEStatusTypes.OutOfService,
                                  WWCP.ChargingStationStatusTypes  InitialChargingStationStatus            = WWCP.ChargingStationStatusTypes.OutOfService,
                                  UInt16                           MaxEVSEAdminStatusListSize              = EVSE.DefaultMaxAdminStatusListSize,
                                  UInt16                           MaxChargingStationAdminStatusListSize   = EVSE.DefaultMaxAdminStatusListSize,
                                  UInt16                           MaxEVSEStatusListSize                   = EVSE.DefaultMaxEVSEStatusListSize,
                                  UInt16                           MaxChargingStationStatusListSize        = EVSE.DefaultMaxEVSEStatusListSize,

                                  EMP.EVSEDataRecord2EVSEDelegate  EVSEDataRecord2EVSE                     = null)

        {

            EVSE _EVSE = null;

            try
            {

                var _EVSEId             = EVSEDataRecord.Id.ToWWCP();

                if (!_EVSEId.HasValue)
                    return null;

                var _ChargingStationId  = WWCP.ChargingStation_Id.Create(_EVSEId.Value);

                var CustomData = new Dictionary<String, Object>();
                CustomData.Add("OICP.EVSEDataRecord", EVSEDataRecord);


                _EVSE = new EVSE(_EVSEId.Value,
                                 new ChargingStation(_ChargingStationId,
                                                     station => {
                                                         station.DataSource   = DataSource;
                                                         station.Address      = EVSEDataRecord.Address.ToWWCP();
                                                         station.GeoLocation  = EVSEDataRecord.GeoCoordinate;
                                                     },
                                                     null,
                                                     InitialChargingStationAdminStatus,
                                                     InitialChargingStationStatus,
                                                     MaxChargingStationAdminStatusListSize,
                                                     MaxChargingStationStatusListSize),
                                 evse => {
                                     evse.DataSource  = DataSource;
                                 },
                                 null,
                                 InitialEVSEAdminStatus,
                                 InitialEVSEStatus,
                                 MaxEVSEAdminStatusListSize,
                                 MaxEVSEStatusListSize,
                                 CustomData);


                //evse.Description     = CurrentEVSEDataRecord.AdditionalInfo;
                //evse.ChargingModes   = new ReactiveSet<WWCP.ChargingModes>(CurrentEVSEDataRecord.ChargingModes.ToEnumeration().SafeSelect(mode => OICPMapper.AsWWCPChargingMode(mode)));
                //OICPMapper.ApplyChargingFacilities(evse, CurrentEVSEDataRecord.ChargingFacilities);
                //evse.MaxCapacity     = CurrentEVSEDataRecord.MaxCapacity;
                //evse.SocketOutlets   = new ReactiveSet<SocketOutlet>(CurrentEVSEDataRecord.Plugs.ToEnumeration().SafeSelect(Plug => new SocketOutlet(Plug.AsWWCPPlugTypes())));


            }
            catch (Exception e)
            {
                return null;
            }

            return EVSEDataRecord2EVSE != null
                       ? EVSEDataRecord2EVSE(EVSEDataRecord, _EVSE)
                       : _EVSE;

        }

        #endregion

        #region ToOICP(this EVSE, EVSE2EVSEDataRecord = null)

        /// <summary>
        /// Convert a WWCP EVSE into a corresponding OICP EVSE data record.
        /// </summary>
        /// <param name="EVSE">A WWCP EVSE.</param>
        /// <param name="EVSE2EVSEDataRecord">A delegate to process an EVSE data record, e.g. before pushing it to a roaming provider.</param>
        /// <returns>The corresponding OICP EVSE data record.</returns>
        public static EVSEDataRecord ToOICP(this EVSE                        EVSE,
                                            CPO.EVSE2EVSEDataRecordDelegate  EVSE2EVSEDataRecord   = null,
                                            DeltaTypes?                      DeltaType             = null,
                                            DateTime?                        LastUpdate            = null,
                                            HubOperator_Id?                  HubOperatorId         = null,
                                            ClearingHouse_Id?                ClearingHouseId       = null)
        {

            #region Verify EVSE identification

            var _EVSEId = EVSE.Id.ToOICP();

            if (!_EVSEId.HasValue)
                throw new InvalidEVSEIdentificationException(EVSE.Id.ToString());

            #endregion

            try
            {

                #region Copy custom data and add WWCP EVSE as "WWCP.EVSE"...

                var CustomData = new Dictionary<String, Object>();
                EVSE.CustomData.ForEach(kvp => CustomData.Add(kvp.Key, kvp.Value));

                if (!CustomData.ContainsKey("WWCP.EVSE"))
                    CustomData.Add("WWCP.EVSE", EVSE);
                else
                    CustomData["WWCP.EVSE"] = EVSE;

                #endregion


                var _EVSEDataRecord  = new EVSEDataRecord(_EVSEId.Value,
                                                          DeltaType  ?? new DeltaTypes?(),
                                                          LastUpdate ?? new DateTime?(),
                                                          ChargingStation_Id.Parse(EVSE.ChargingStation.Id.ToString()),
                                                          ChargingPool_Id.   Parse(EVSE.ChargingPool.   Id.ToString()),
                                                          EVSE.ChargingStation.Name,
                                                          EVSE.ChargingStation.Address.ToOICP(),
                                                          EVSE.ChargingStation.GeoLocation,
                                                          EVSE.SocketOutlets.SafeSelect(socketoutlet => socketoutlet.Plug).AsOICPPlugTypes(),
                                                          EVSE.AsChargingFacilities(),
                                                          EVSE.ChargingModes.AsOICPChargingMode(),
                                                          EVSE.ChargingStation.AuthenticationModes.
                                                                                   Select(AsOICPAuthenticationMode).
                                                                                   Where(mode => mode != AuthenticationModes.Unknown).
                                                                                   Reduce(),
                                                          null, // MaxCapacity [kWh]
                                                          EVSE.ChargingStation.PaymentOptions.SafeSelect(AsOICPPaymentOption).Reduce(),
                                                          ValueAddedServices.None,
                                                          EVSE.ChargingStation.Accessibility.ToOICP(),
                                                          EVSE.ChargingStation.HotlinePhoneNumber.FirstText(),
                                                          EVSE.ChargingStation.Description, // AdditionalInfo
                                                          EVSE.ChargingStation.ChargingPool.EntranceLocation,
                                                          EVSE.ChargingStation.OpeningTimes != null ? EVSE.ChargingStation.OpeningTimes.IsOpen24Hours : true,
                                                          EVSE.ChargingStation.OpeningTimes != null ? EVSE.ChargingStation.OpeningTimes.ToString()    : null,
                                                          HubOperatorId,
                                                          ClearingHouseId,
                                                          EVSE.ChargingStation.IsHubjectCompatible,
                                                          EVSE.ChargingStation.DynamicInfoAvailable,

                                                          CustomData);


                return EVSE2EVSEDataRecord != null
                           ? EVSE2EVSEDataRecord(EVSE, _EVSEDataRecord)
                           : _EVSEDataRecord;

            }
            catch (Exception e)
            {
                throw new EVSEToOICPException(EVSE, e);
            }

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
                    return WWCP.EVSEStatusTypes.OutOfService;

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
                    return EVSEStatusTypes.OutOfService;

            }

        }

        #endregion

        #region AsOICPEVSEStatus(this EVSEStatus)

        /// <summary>
        /// Create a new OICP EVSE status record based on the given WWCP EVSE status.
        /// </summary>
        /// <param name="EVSEStatus">The current status of an EVSE.</param>
        public static EVSEStatusRecord AsOICPEVSEStatus(this EVSEStatus EVSEStatus)

        {

            #region Initial checks

            if (EVSEStatus == null)
                throw new ArgumentNullException(nameof(EVSEStatus), "The given EVSE status must not be null!");

            #endregion

            return new EVSEStatusRecord(EVSEStatus.Id.ToOICP().Value,
                                        AsOICPEVSEStatus(EVSEStatus.Status.Value));

        }

        #endregion


        #region AsChargingFacility(this EVSE)

        /// <summary>
        /// Maps a WWCP EVSE to an OICP charging facility.
        /// </summary>
        /// <param name="EVSE">An WWCP EVSE.</param>
        public static ChargingFacility AsChargingFacility(this EVSE EVSE)
        {

            if (!EVSE.CurrentType.   HasValue &&
                !EVSE.AverageVoltage.HasValue &&
                !EVSE.MaxCurrent.    HasValue &&
                !EVSE.MaxPower.      HasValue)
            {
                return null;
            }


            PowerTypes? powerType = null;

            if (EVSE.CurrentType.HasValue)
            {
                switch (EVSE.CurrentType)
                {

                    case CurrentTypes.AC_OnePhase:
                        powerType = PowerTypes.AC_1_PHASE;
                        break;

                    case CurrentTypes.AC_ThreePhases:
                        powerType = PowerTypes.AC_3_PHASE;
                        break;

                    case CurrentTypes.DC:
                        powerType = PowerTypes.DC;
                        break;

                    case CurrentTypes.Unspecified:
                        powerType = PowerTypes.Unspecified;
                        break;

                }
            }

            return new ChargingFacility(powerType,
                                        EVSE.AverageVoltage.HasValue ? new UInt32?(Convert.ToUInt32(EVSE.AverageVoltage)) : null,
                                        EVSE.MaxCurrent.    HasValue ? new UInt32?(Convert.ToUInt32(EVSE.MaxCurrent))     : null,
                                        EVSE.MaxPower);

        }

        #endregion

        #region AsChargingFacilities(this EVSE)

        /// <summary>
        /// Maps a WWCP EVSE to an enumeration of charging facilities.
        /// </summary>
        /// <param name="EVSE">An WWCP EVSE.</param>
        public static IEnumerable<ChargingFacility> AsChargingFacilities(this EVSE EVSE)
        {

            if (!EVSE.CurrentType.   HasValue &&
                !EVSE.AverageVoltage.HasValue &&
                !EVSE.MaxCurrent.    HasValue &&
                !EVSE.MaxPower.      HasValue)
            {
                return new ChargingFacility[0];
            }


            PowerTypes? powerType = null;

            if (EVSE.CurrentType.HasValue)
            {
                switch (EVSE.CurrentType)
                {

                    case CurrentTypes.AC_OnePhase:
                        powerType = PowerTypes.AC_1_PHASE;
                        break;

                    case CurrentTypes.AC_ThreePhases:
                        powerType = PowerTypes.AC_3_PHASE;
                        break;

                    case CurrentTypes.DC:
                        powerType = PowerTypes.DC;
                        break;

                    case CurrentTypes.Unspecified:
                        powerType = PowerTypes.Unspecified;
                        break;

                }
            }

            return new ChargingFacility[1] {
                       new ChargingFacility(powerType,
                                            EVSE.AverageVoltage.HasValue ? new UInt32?(Convert.ToUInt32(EVSE.AverageVoltage)) : null,
                                            EVSE.MaxCurrent.    HasValue ? new UInt32?(Convert.ToUInt32(EVSE.MaxCurrent))     : null,
                                            EVSE.MaxPower)
                   };

        }

        #endregion

        #region ApplyChargingFacilities(this ChargingFacilities, EVSE)

        public static void ApplyChargingFacilities(this IEnumerable<ChargingFacility> ChargingFacilities, EVSE EVSE)
        {

            foreach (var chargingFacility in ChargingFacilities)
            {

                if (chargingFacility.PowerType.HasValue && !EVSE.CurrentType.HasValue)
                    EVSE.CurrentType = chargingFacility.PowerType.ToWWCP();

                if (chargingFacility.Voltage.HasValue && (!EVSE.AverageVoltage.HasValue || EVSE.AverageVoltage.Value < Convert.ToDecimal(chargingFacility.Voltage. Value)))
                    EVSE.AverageVoltage = chargingFacility.Voltage;

                if (chargingFacility.Amperage.HasValue && (!EVSE.MaxCurrent.   HasValue || EVSE.MaxCurrent.    Value < Convert.ToDecimal(chargingFacility.Amperage.Value)))
                    EVSE.MaxCurrent     = chargingFacility.Amperage;

                if (chargingFacility.Power.   HasValue && (!EVSE.MaxPower.     HasValue || EVSE.MaxPower.      Value < Convert.ToDecimal(chargingFacility.Power.   Value)))
                    EVSE.MaxPower       = chargingFacility.Power;

            }

        }

        #endregion


        #region ToOICP(this CurrentType)

        /// <summary>
        /// Maps a WWCP current type to an OICP power type.
        /// </summary>
        /// <param name="CurrentType">A current type.</param>
        public static PowerTypes? ToOICP(this CurrentTypes? CurrentType)
        {

            if (!CurrentType.HasValue)
                return null;

            switch (CurrentType.Value)
            {

                case CurrentTypes.AC_OnePhase:    return PowerTypes.AC_1_PHASE;
                case CurrentTypes.AC_ThreePhases: return PowerTypes.AC_3_PHASE;
                case CurrentTypes.DC:             return PowerTypes.DC;

                default: return PowerTypes.Unspecified;

            }

        }

        #endregion

        #region ToWWCP(this PowerType)

        /// <summary>
        /// Maps an OICP power type to a WWCP current type.
        /// </summary>
        /// <param name="PowerType">A power type.</param>
        public static CurrentTypes? ToWWCP(this PowerTypes? PowerType)
        {

            if (!PowerType.HasValue)
                return null;

            switch (PowerType.Value)
            {

                case PowerTypes.AC_1_PHASE: return CurrentTypes.AC_OnePhase;
                case PowerTypes.AC_3_PHASE: return CurrentTypes.AC_ThreePhases;
                case PowerTypes.DC:         return CurrentTypes.DC;

                default: return CurrentTypes.Unspecified;

            }

        }

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

        /// <summary>
        /// Maps a WWCP accessibility type to an OICP  accessibility type.
        /// </summary>
        /// <param name="AccessibilityType">A accessibility type.</param>
        public static AccessibilityTypes ToOICP(this WWCP.AccessibilityTypes? AccessibilityType)
        {

            if (!AccessibilityType.HasValue)
                return AccessibilityTypes.Free_publicly_accessible;

            switch (AccessibilityType.Value)
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
        public static Address ToOICP(this Vanaheimr.Illias.Address WWCPAddress)

            => new Address(WWCPAddress.Country,
                           WWCPAddress.City,
                           WWCPAddress.Street,
                           WWCPAddress.PostalCode,
                           WWCPAddress.HouseNumber,
                           WWCPAddress.FloorLevel);


        /// <summary>
        /// Maps an OICP address type to a WWCP address type.
        /// </summary>
        /// <param name="OICPAddress">A address type.</param>
        public static Vanaheimr.Illias.Address ToWWCP(this Address OICPAddress)

            => Vanaheimr.Illias.Address.Create(OICPAddress.Country,
                                               OICPAddress.PostalCode,
                                               OICPAddress.City,
                                               OICPAddress.Street,
                                               OICPAddress.HouseNumber,
                                               OICPAddress.FloorLevel);

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

            var _AuthenticationModes = OICPv2_2.AuthenticationModes.Unknown;

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

        public static ChargingModes AsOICPChargingMode(this WWCP.ChargingModes? ChargingMode)
        {

            if (!ChargingMode.HasValue)
                return ChargingModes.Unspecified;

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


        public static EVSE_Id? ToOICP(this WWCP.EVSE_Id EVSEId)
        {

            if (EVSE_Id.TryParse(EVSEId.ToString(), out EVSE_Id OICPEVSEId))
                return OICPEVSEId;

            return null;

        }

        public static WWCP.EVSE_Id? ToWWCP(this EVSE_Id EVSEId)
        {

            if (WWCP.EVSE_Id.TryParse(EVSEId.ToString(), out WWCP.EVSE_Id WWCPEVSEId))
                return WWCPEVSEId;

            return null;

        }

        public static WWCP.EVSE_Id? ToWWCP(this EVSE_Id? EVSEId)
        {

            if (!EVSEId.HasValue)
                return null;

            if (WWCP.EVSE_Id.TryParse(EVSEId.ToString(), out WWCP.EVSE_Id WWCPEVSEId))
                return WWCPEVSEId;

            return null;

        }


        public static Session_Id ToOICP(this ChargingSession_Id SessionId)
            => Session_Id.Parse(SessionId.ToString());

        public static ChargingSession_Id ToWWCP(this Session_Id SessionId)
            => ChargingSession_Id.Parse(SessionId.ToString());


        public static Session_Id? ToOICP(this ChargingSession_Id? SessionId)
            => SessionId.HasValue
                   ? Session_Id.Parse(SessionId.ToString())
                   : new Session_Id?();

        public static ChargingSession_Id? ToWWCP(this Session_Id? SessionId)
            => SessionId.HasValue
                   ? ChargingSession_Id.Parse(SessionId.ToString())
                   : new ChargingSession_Id?();


        public static PartnerProduct_Id? ToOICP(this ChargingProduct_Id ProductId)
            => PartnerProduct_Id.Parse(ProductId.ToString());

        public static ChargingProduct_Id ToWWCP(this PartnerProduct_Id ProductId)
            => ChargingProduct_Id.Parse(ProductId.ToString());


        public static Operator_Id ToOICP(this ChargingStationOperator_Id  OperatorId,
                                         WWCP.OperatorIdFormats           Format = WWCP.OperatorIdFormats.ISO_STAR)
            => Operator_Id.Parse(OperatorId.ToString(Format));

        public static ChargingStationOperator_Id? ToWWCP(this Operator_Id OperatorId)
        {

            if (ChargingStationOperator_Id.TryParse(OperatorId.ToString(), out ChargingStationOperator_Id ChargingStationOperatorId))
                return ChargingStationOperatorId;

            return null;

        }


        public static Provider_Id ToOICP(this eMobilityProvider_Id ProviderId)
            => Provider_Id.Parse(ProviderId.ToString());

        public static eMobilityProvider_Id ToWWCP(this Provider_Id ProviderId)
            => eMobilityProvider_Id.Parse(ProviderId.ToString());


        public static Provider_Id? ToOICP(this eMobilityProvider_Id? ProviderId)
            => ProviderId.HasValue
                   ? Provider_Id.Parse(ProviderId.ToString())
                   : new Provider_Id?();

        public static eMobilityProvider_Id? ToWWCP(this Provider_Id? ProviderId)
            => ProviderId.HasValue
                   ? eMobilityProvider_Id.Parse(ProviderId.ToString())
                   : new eMobilityProvider_Id?();


        //public static EVCO_Id ToOICP(this RemoteAuthentication RemoteAuthentication)
        //    => EVCO_Id.Parse(RemoteAuthentication.RemoteIdentification.ToString());

        public static RemoteAuthentication ToWWCP(this EVCO_Id EVCOId)
            => RemoteAuthentication.FromRemoteIdentification(eMobilityAccount_Id.Parse(EVCOId.ToString()));


        public static EVCO_Id ToOICP(this eMobilityAccount_Id eMAId)
            => EVCO_Id.Parse(eMAId.ToString());

        public static eMobilityAccount_Id ToWWCP_eMAId(this EVCO_Id EVCOId)
            => eMobilityAccount_Id.Parse(EVCOId.ToString());


        public static EVCO_Id? ToOICP(this eMobilityAccount_Id? eMAId)
            => eMAId.HasValue
                   ? EVCO_Id.Parse(eMAId.ToString())
                   : new EVCO_Id?();

        public static eMobilityAccount_Id? ToWWCP(this EVCO_Id? EVCOId)
            => EVCOId.HasValue
                   ? eMobilityAccount_Id.Parse(EVCOId.ToString())
                   : new eMobilityAccount_Id?();




        public static UID ToOICP(this Auth_Token AuthToken)
            => UID.Parse(AuthToken.ToString());

        public static Auth_Token ToWWCP(this UID UID)
            => Auth_Token.Parse(UID.ToString());

        public static UID? ToOICP(this Auth_Token? AuthToken)
            => AuthToken.HasValue
                   ? UID.Parse(AuthToken.ToString())
                   : new UID?();


        #region ToWWCP(this Identification)

        public static RemoteAuthentication ToWWCP(this Identification Identification)
        {

            if (Identification.RFIDId.HasValue)
                return RemoteAuthentication.FromAuthToken(Auth_Token.Parse(Identification.RFIDId.ToString()));

            if (Identification.QRCodeIdentification.HasValue)
                return RemoteAuthentication.FromQRCodeIdentification(Identification.QRCodeIdentification.Value.EVCOId.ToWWCP_eMAId(),
                                                         Identification.QRCodeIdentification.Value.PIN);

            if (Identification.PlugAndChargeIdentification.HasValue)
                return RemoteAuthentication.FromPlugAndChargeIdentification(Identification.PlugAndChargeIdentification.Value.ToWWCP_eMAId());

            if (Identification.RemoteIdentification.HasValue)
                return RemoteAuthentication.FromRemoteIdentification(Identification.RemoteIdentification.Value.ToWWCP_eMAId());

            return null;

        }

        #endregion

        #region ToOICP(this RemoteAuthentication)

        /// <summary>
        /// Create a new identification for authorization based on the given WWCP AuthInfo.
        /// </summary>
        /// <param name="Authentication">A WWCP auth info.</param>
        public static Identification ToOICP(this AAuthentication Authentication)
        {

            if (Authentication.AuthToken                   != null)
                return Identification.FromUID                     (Authentication.AuthToken.ToOICP());

            if (Authentication.QRCodeIdentification        != null)
                return Identification.FromQRCodeIdentification       (new QRCodeIdentification(Authentication.QRCodeIdentification.eMAId.ToOICP(),
                                                                                               Authentication.QRCodeIdentification.PIN,
                                                                                               Authentication.QRCodeIdentification.Function.ToOICP(),
                                                                                               Authentication.QRCodeIdentification.Salt));

            if (Authentication.PlugAndChargeIdentification.HasValue)
                return Identification.FromPlugAndChargeIdentification(Authentication.PlugAndChargeIdentification.Value.ToOICP());

            if (Authentication.RemoteIdentification.       HasValue)
                return Identification.FromRemoteIdentification       (Authentication.RemoteIdentification.       Value.ToOICP());

            throw new ArgumentException("Invalid AuthInfo!", nameof(Authentication));

        }

        #endregion


        #region ToWWCP(this ChargeDetailRecord, ChargeDetailRecord2WWCPChargeDetailRecord = null)

        /// <summary>
        /// Convert an OICP charge detail record into a corresponding WWCP charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecord">An OICP charge detail record.</param>
        /// <param name="ChargeDetailRecord2WWCPChargeDetailRecord">A delegate which allows you to modify the convertion from OICP charge detail records to WWCP charge detail records.</param>
        public static WWCP.ChargeDetailRecord ToWWCP(this ChargeDetailRecord                                ChargeDetailRecord,
                                                     CPO.ChargeDetailRecord2WWCPChargeDetailRecordDelegate  ChargeDetailRecord2WWCPChargeDetailRecord = null)
        {

            var CustomData = new Dictionary<String, Object> {
                                 { "OICP.CDR", ChargeDetailRecord }
                             };

            if (ChargeDetailRecord.CPOPartnerSessionId.HasValue)
                CustomData.Add("OICP.CPOPartnerSessionId",  ChargeDetailRecord.CPOPartnerSessionId.ToString());

            if (ChargeDetailRecord.EMPPartnerSessionId.HasValue)
                CustomData.Add("OICP.EMPPartnerSessionId",  ChargeDetailRecord.EMPPartnerSessionId.ToString());

            if (ChargeDetailRecord.HubOperatorId.HasValue)
                CustomData.Add("OICP.HubOperatorId",        ChargeDetailRecord.HubOperatorId.      ToString());

            if (ChargeDetailRecord.HubProviderId.HasValue)
                CustomData.Add("OICP.HubProviderId",        ChargeDetailRecord.HubProviderId.      ToString());


            var CDR = new  WWCP.ChargeDetailRecord(SessionId:             ChargeDetailRecord.SessionId.ToWWCP(),
                                                   EVSEId:                ChargeDetailRecord.EVSEId.   ToWWCP(),
                                                   ProviderIdStart:       ChargeDetailRecord.HubProviderId.HasValue ? new eMobilityProvider_Id?(eMobilityProvider_Id.Parse(ChargeDetailRecord.HubProviderId.ToString())) : null,

                                                   ChargingProduct:       ChargeDetailRecord.PartnerProductId.HasValue
                                                                              ? new ChargingProduct(ChargeDetailRecord.PartnerProductId.Value.ToWWCP())
                                                                              : null,

                                                   SessionTime:           new StartEndDateTime(ChargeDetailRecord.SessionStart,
                                                                                               ChargeDetailRecord.SessionEnd),

                                                   AuthenticationStart:   ChargeDetailRecord.Identification.ToWWCP(),

                                                   EnergyMeteringValues:  (ChargeDetailRecord.ChargingStart.HasValue &&
                                                                           ChargeDetailRecord.ChargingEnd  .HasValue)
                                                                              ? new Timestamped<Decimal>[] {

                                                                                    new Timestamped<Decimal>(
                                                                                        ChargeDetailRecord.ChargingStart.  Value,
                                                                                        ChargeDetailRecord.MeterValueStart.Value
                                                                                    ),

                                                                                    //ToDo: Meter values in between... but we don't have timestamps for them!

                                                                                    new Timestamped<Decimal>(
                                                                                        ChargeDetailRecord.ChargingEnd.  Value,
                                                                                        ChargeDetailRecord.MeterValueEnd.Value
                                                                                    )

                                                                                }
                                                                              : new Timestamped<Decimal>[0],

                                                   //ConsumedEnergy:      Will be calculated!

                                                   Signatures:            new String[] { ChargeDetailRecord.MeteringSignature },

                                                   CustomData:            CustomData

                                                  );

            if (ChargeDetailRecord2WWCPChargeDetailRecord != null)
                CDR = ChargeDetailRecord2WWCPChargeDetailRecord(ChargeDetailRecord, CDR);

            return CDR;

        }

        #endregion

        #region ToOICP(this ChargeDetailRecord, WWCPChargeDetailRecord2ChargeDetailRecord = null)

        public static String WWCP_CDR = "WWCP.CDR";

        /// <summary>
        /// Convert a WWCP charge detail record into a corresponding OICP charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecord">A WWCP charge detail record.</param>
        /// <param name="WWCPChargeDetailRecord2ChargeDetailRecord">A delegate which allows you to modify the convertion from WWCP charge detail records to OICP charge detail records.</param>
        public static ChargeDetailRecord ToOICP(this WWCP.ChargeDetailRecord                           ChargeDetailRecord,
                                                CPO.WWCPChargeDetailRecord2ChargeDetailRecordDelegate  WWCPChargeDetailRecord2ChargeDetailRecord = null)
        {

            var CDR = new ChargeDetailRecord(
                          EVSEId:                ChargeDetailRecord.EVSEId.Value.ToOICP().Value,
                          SessionId:             ChargeDetailRecord.SessionId.ToOICP(),
                          SessionStart:          ChargeDetailRecord.SessionTime.StartTime,
                          SessionEnd:            ChargeDetailRecord.SessionTime.EndTime.Value,
                          Identification:        ChargeDetailRecord.AuthenticationStart.ToOICP(),
                          PartnerProductId:      ChargeDetailRecord.ChargingProduct?.Id.ToOICP(),
                          CPOPartnerSessionId:   ChargeDetailRecord.GetCustomDataAs<CPOPartnerSession_Id?>("OICP.CPOPartnerSessionId"),
                          EMPPartnerSessionId:   ChargeDetailRecord.GetCustomDataAs<EMPPartnerSession_Id?>("OICP.EMPPartnerSessionId"),
                          ChargingStart:         ChargeDetailRecord.EnergyMeteringValues?.Any() == true ? ChargeDetailRecord.EnergyMeteringValues.First().Timestamp : ChargeDetailRecord.SessionTime.StartTime,
                          ChargingEnd:           ChargeDetailRecord.EnergyMeteringValues?.Any() == true ? ChargeDetailRecord.EnergyMeteringValues.Last(). Timestamp : ChargeDetailRecord.SessionTime.EndTime.Value,
                          MeterValueStart:       ChargeDetailRecord.EnergyMeteringValues?.Any() == true ? ChargeDetailRecord.EnergyMeteringValues.First().Value     : new Decimal?(),
                          MeterValueEnd:         ChargeDetailRecord.EnergyMeteringValues?.Any() == true ? ChargeDetailRecord.EnergyMeteringValues.Last(). Value     : new Decimal?(),
                          MeterValuesInBetween:  ChargeDetailRecord.EnergyMeteringValues?.Any() == true ? ChargeDetailRecord.EnergyMeteringValues.Select((Timestamped<Decimal> v) => v.Value) : null,
                          ConsumedEnergy:        ChargeDetailRecord.ConsumedEnergy,
                          MeteringSignature:     ChargeDetailRecord.Signatures.FirstOrDefault(),
                          HubOperatorId:         ChargeDetailRecord.GetCustomDataAs<HubOperator_Id?>("OICP.HubOperatorId"),
                          HubProviderId:         ChargeDetailRecord.GetCustomDataAs<HubProvider_Id?>("OICP.HubProviderId"),
                          CustomData:            new Dictionary<String, Object> {
                                                     { WWCP_CDR, ChargeDetailRecord }
                                                 }
                      );

            if (WWCPChargeDetailRecord2ChargeDetailRecord != null)
                CDR = WWCPChargeDetailRecord2ChargeDetailRecord(ChargeDetailRecord, CDR);

            return CDR;

        }

        #endregion


    }

}
