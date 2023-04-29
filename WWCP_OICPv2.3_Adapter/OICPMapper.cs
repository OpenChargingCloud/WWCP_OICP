/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// A delegate which allows you to modify the convertion from WWCP EVSE identifications to OICP EVSE identifications.
    /// </summary>
    /// <param name="EVSEId">A WWCP EVSE identification.</param>
    public delegate EVSE_Id                  WWCPEVSEId_2_EVSEId_Delegate                     (WWCP.EVSE_Id             EVSEId);

    /// <summary>
    /// A delegate which allows you to modify the convertion from WWCP EVSEs to EVSE data records.
    /// </summary>
    /// <param name="EVSE">A WWCP EVSE.</param>
    /// <param name="EVSEDataRecord">An EVSE data record.</param>
    public delegate EVSEDataRecord           EVSE2EVSEDataRecordDelegate                      (WWCP.IEVSE               EVSE,
                                                                                               EVSEDataRecord           EVSEDataRecord);

    /// <summary>
    /// A delegate which allows you to modify the convertion from WWCP EVSE status updates to EVSE status records.
    /// </summary>
    /// <param name="EVSEStatusUpdate">A WWCP EVSE status update.</param>
    /// <param name="EVSEStatusRecord">An OICP EVSE status record.</param>
    public delegate EVSEStatusRecord         EVSEStatusUpdate2EVSEStatusRecordDelegate        (WWCP.EVSEStatusUpdate    EVSEStatusUpdate,
                                                                                               EVSEStatusRecord         EVSEStatusRecord);

    /// <summary>
    /// A delegate which allows you to modify the convertion from WWCP charge detail records to OICP charge detail records.
    /// </summary>
    /// <param name="WWCPChargeDetailRecord">A WWCP charge detail record.</param>
    /// <param name="OCIPChargeDetailRecord">An OICP charge detail record.</param>
    public delegate ChargeDetailRecord       WWCPChargeDetailRecord2ChargeDetailRecordDelegate(WWCP.ChargeDetailRecord  WWCPChargeDetailRecord,
                                                                                               ChargeDetailRecord       OCIPChargeDetailRecord);


    /// <summary>
    /// A delegate which allows you to modify the convertion from EVSE data records to WWCP EVSEs.
    /// </summary>
    /// <param name="EVSEDataRecord">An OICP EVSE data record.</param>
    /// <param name="EVSE">A WWCP EVSE.</param>
    public delegate WWCP.EVSE                EVSEDataRecord2EVSEDelegate                      (EVSEDataRecord           EVSEDataRecord,
                                                                                               WWCP.EVSE                EVSE);

    /// <summary>
    /// A delegate which allows you to modify the convertion from EVSE status records to WWCP EVSE status updates.
    /// </summary>
    /// <param name="EVSEStatusRecord">An OICP EVSE status record.</param>
    /// <param name="EVSEStatusUpdate">A WWCP EVSE status update.</param>
    public delegate WWCP.EVSEStatusUpdate    EVSEStatusRecord2EVSEStatusUpdateDelegate        (EVSEStatusRecord         EVSEStatusRecord,
                                                                                               WWCP.EVSEStatusUpdate    EVSEStatusUpdate);

    /// <summary>
    /// A delegate which allows you to modify the convertion from OICP charge detail records to WWCP charge detail records.
    /// </summary>
    /// <param name="OCIPChargeDetailRecord">An OICP charge detail record.</param>
    /// <param name="WWCPChargeDetailRecord">A WWCP charge detail record.</param>
    public delegate WWCP.ChargeDetailRecord  ChargeDetailRecord2WWCPChargeDetailRecordDelegate(ChargeDetailRecord       OICPChargeDetailRecord,
                                                                                               WWCP.ChargeDetailRecord  WWCPChargeDetailRecord);


    /// <summary>
    /// Helper methods to map OICP data structures to
    /// WWCP data structures and vice versa.
    /// </summary>
    public static class OICPMapper
    {

        public const String WWCP_CDR                              = "WWCP.CDR";
        public const String OICP_EVSEDataRecord                   = "OICP.EVSEDataRecord";
        public const String OICP_CDR                              = "OICP.CDR";
        public const String OICP_CPOPartnerSessionId              = "OICP.CPOPartnerSessionId";
        public const String OICP_EMPPartnerSessionId              = "OICP.EMPPartnerSessionId";
        public const String OICP_MeterValuesInBetween             = "OICP.MeterValuesInBetween";
        public const String OICP_SignedMeteringValues             = "OICP.SignedMeteringValues";
        public const String OICP_CalibrationLawVerificationInfo   = "OICP.CalibrationLawVerificationInfo";
        public const String OICP_HubOperatorId                    = "OICP.HubOperatorId";
        public const String OICP_HubProviderId                    = "OICP.HubProviderId";
        public const String OICP_ClearingHouseId                  = "OICP.ClearingHouseId";


        #region ToWWCP(this Action)

        /// <summary>
        /// Convert an OICP v2.0 action type into a corresponding WWCP EVSE action type.
        /// </summary>
        /// <param name="ActionType">An OICP v2.0 action type.</param>
        /// <returns>The corresponding WWCP action type.</returns>
        public static WWCP.ActionTypes ToWWCP(this ActionTypes ActionType)

            => ActionType switch {
                   ActionTypes.FullLoad  => WWCP.ActionTypes.fullLoad,
                   ActionTypes.Update    => WWCP.ActionTypes.update,
                   ActionTypes.Insert    => WWCP.ActionTypes.insert,
                   ActionTypes.Delete    => WWCP.ActionTypes.delete,
                   _                     => WWCP.ActionTypes.fullLoad,
               };

        #endregion

        #region ToOICP(this ActionType)

        /// <summary>
        /// Convert a WWCP action type into a corresponding OICP v2.0 action type.
        /// </summary>
        /// <param name="ActionType">An WWCP action type.</param>
        /// <returns>The corresponding OICP v2.0 action type.</returns>
        public static ActionTypes ToOICP(this WWCP.ActionTypes ActionType)

            => ActionType switch {
                   WWCP.ActionTypes.fullLoad  => ActionTypes.FullLoad,
                   WWCP.ActionTypes.update    => ActionTypes.Update,
                   WWCP.ActionTypes.insert    => ActionTypes.Insert,
                   WWCP.ActionTypes.delete    => ActionTypes.Delete,
                   _                          => ActionTypes.FullLoad,
               };

        #endregion


        #region ToWWCP(this EVSEDataRecord, ..., DataSource = null, CustomConverter = null)

        /// <summary>
        /// Convert an OICP EVSE data record into a corresponding WWCP EVSE.
        /// </summary>
        /// <param name="EVSEDataRecord">An EVSE data record.</param>
        /// 
        /// <param name="CustomConverter">A delegate to process an EVSE data record, e.g. before importing it into a roaming network.</param>
        public static WWCP.EVSE? ToWWCP(this EVSEDataRecord                    EVSEDataRecord,

                                        WWCP.EVSEAdminStatusTypes?             InitialEVSEAdminStatus                  = null,
                                        WWCP.ChargingStationAdminStatusTypes?  InitialChargingStationAdminStatus       = null,
                                        WWCP.EVSEStatusTypes?                  InitialEVSEStatus                       = null,
                                        WWCP.ChargingStationStatusTypes?       InitialChargingStationStatus            = null,
                                        UInt16                                 MaxEVSEAdminStatusListSize              = WWCP.EVSE.           DefaultMaxEVSEAdminStatusScheduleSize,
                                        UInt16                                 MaxChargingStationAdminStatusListSize   = WWCP.ChargingStation.DefaultMaxChargingStationAdminStatusScheduleSize,
                                        UInt16                                 MaxEVSEStatusListSize                   = WWCP.EVSE.           DefaultMaxEVSEStatusScheduleSize,
                                        UInt16                                 MaxChargingStationStatusListSize        = WWCP.ChargingStation.DefaultMaxChargingStationStatusScheduleSize,

                                        String?                                DataSource                              = null,
                                        EVSEDataRecord2EVSEDelegate?           CustomConverter                         = null)

        {

            WWCP.EVSE? evse = null;

            try
            {

                var evseId             = EVSEDataRecord.Id.ToWWCP();
                if (!evseId.HasValue)
                    return null;

                var chargingStationId  = WWCP.ChargingStation_Id.Create(evseId.Value);

                evse                   = new WWCP.EVSE(
                                             evseId.Value,
                                             new WWCP.ChargingStation(
                                                 chargingStationId,
                                                 null,
                                                 null,
                                                 null,
                                                 null,
                                                 station => {
                                                     station.DataSource   = DataSource;
                                                     station.Address      = EVSEDataRecord.Address.ToWWCP();
                                                     station.GeoLocation  = new org.GraphDefined.Vanaheimr.Aegir.GeoCoordinate(
                                                                                org.GraphDefined.Vanaheimr.Aegir.Latitude. Parse(EVSEDataRecord.GeoCoordinates.Latitude),
                                                                                org.GraphDefined.Vanaheimr.Aegir.Longitude.Parse(EVSEDataRecord.GeoCoordinates.Longitude)
                                                                            );
                                                 },
                                                 null,
                                                 InitialChargingStationAdminStatus,
                                                 InitialChargingStationStatus,
                                                 MaxChargingStationAdminStatusListSize,
                                                 MaxChargingStationStatusListSize
                                             ),
                                             null,          // Name
                                             null,          // Description

                                             InitialEVSEAdminStatus ?? WWCP.EVSEAdminStatusTypes.OutOfService,
                                             InitialEVSEStatus      ?? WWCP.EVSEStatusTypes.OutOfService,
                                             MaxEVSEAdminStatusListSize,
                                             MaxEVSEStatusListSize,

                                             null,          // PhotoURLs
                                             null,          // Brands
                                             null,          // OpenDataLicenses
                                             null,          // ChargingModes
                                             null,          // ChargingTariffs
                                             null,          // CurrentType
                                             null,          // AverageVoltage
                                             null,          // AverageVoltageRealTime
                                             null,          // AverageVoltagePrognoses
                                             null,          // MaxCurrent
                                             null,          // MaxCurrentRealTime
                                             null,          // MaxCurrentPrognoses
                                             null,          // MaxPower
                                             null,          // MaxPowerRealTime
                                             null,          // MaxPowerPrognoses
                                             null,          // MaxCapacity
                                             null,          // MaxCapacityRealTime
                                             null,          // MaxCapacityPrognoses
                                             null,          // EnergyMix
                                             null,          // EnergyMixRealTime
                                             null,          // EnergyMixPrognoses
                                             null,          // EnergyMeter
                                             null,          // IsFreeOfCharge
                                             null,          // ChargingConnectors

                                             null,          // ChargingSession
                                             null,          // LastStatusUpdate
                                             DataSource,
                                             null,          // LastChange

                                             _evse => {     // Configurator

                                             },
                                             null,          // RemoteEVSECreator

                                             EVSEDataRecord.CustomData,
                                             new UserDefinedDictionary(new Dictionary<String, Object?> {
                                                 { OICP_EVSEDataRecord, EVSEDataRecord }
                                             })

                                         );

            }
            catch (Exception e)
            {
                DebugX.Log(String.Concat("Could not convert OICP EVSEDataRecord '", EVSEDataRecord.Id, "' into a WWCP EVSE: ", e.Message + Environment.NewLine + e.StackTrace));
                return null;
            }

            return CustomConverter is not null
                       ? CustomConverter(EVSEDataRecord, evse)
                       : evse;

        }

        #endregion

        #region ToOICP(this EVSE, EVSE2EVSEDataRecord = null)

        /// <summary>
        /// Convert a WWCP EVSE into a corresponding OICP EVSE data record.
        /// </summary>
        /// <param name="EVSE">A WWCP EVSE.</param>
        /// <param name="EVSE2EVSEDataRecord">A delegate to process an EVSE data record, e.g. before pushing it to a roaming provider.</param>
        /// <returns>The corresponding OICP EVSE data record.</returns>
        public static EVSEDataRecord ToOICP(this WWCP.IEVSE               EVSE,
                                            String                        OperatorName,

                                            EVSE2EVSEDataRecordDelegate?  EVSE2EVSEDataRecord   = null,
                                            DeltaTypes?                   DeltaType             = null,
                                            DateTime?                     LastUpdate            = null)
        {

            try
            {

                #region Verifications

                var evseId = EVSE.Id.ToOICP();
                if (!evseId.HasValue)
                    throw new InvalidEVSEIdentificationException(EVSE.Id.ToString());

                if (EVSE.ChargingStation is null)
                    throw new ArgumentNullException("ChargingStation", "Within OICP v2.3 some charging station information is mandatory!");

                var geoLocation    = (EVSE.ChargingStation.GeoLocation ?? EVSE.ChargingPool?.GeoLocation)?.ToOICP();
                if (!geoLocation.HasValue)
                    throw new ArgumentNullException("GeoCoordinates",  "Within OICP v2.3 the geo coordinates of an EVSE are mandatory!");

                var accessibility  = EVSE.ChargingStation.Accessibility?.ToOICP();
                if (!accessibility.HasValue)
                    throw new ArgumentNullException("Accessibility",   "Within OICP v2.3 the accessibility of an EVSE is mandatory!");

                #endregion

                #region Copy custom data and add WWCP EVSE as "WWCP.EVSE"...

                var internalData = new UserDefinedDictionary();
                EVSE.InternalData.ForEach(kvp => internalData.Set(kvp.Key, kvp.Value));

                internalData.Set("WWCP.EVSE", EVSE);

                #endregion


                var evseDataRecord = new EVSEDataRecord(
                                         Id:                                evseId.Value,
                                         OperatorId:                        evseId.Value.OperatorId,
                                         OperatorName:                      OperatorName,
                                         ChargingStationName:               EVSE.ChargingStation.Name.ToOICP(MaxLength: 150),
                                         Address:                           (EVSE.ChargingStation.Address ??
                                                                             EVSE.ChargingPool.   Address).ToOICP(),
                                         GeoCoordinates:                    geoLocation.Value,
                                         PlugTypes:                         EVSE.ChargingConnectors.SafeSelect(chargingConnector => chargingConnector.Plug.ToOICP()),
                                         ChargingFacilities:                EVSE.AsChargingFacilities(),
                                         RenewableEnergy:                   false,
                                         CalibrationLawDataAvailability:    CalibrationLawDataAvailabilities.NotAvailable,
                                         AuthenticationModes:               EVSE.ChargingStation.AuthenticationModes.ToOICP(),
                                         PaymentOptions:                    EVSE.IsFreeOfCharge
                                                                                ? new PaymentOptions[] { PaymentOptions.NoPayment }
                                                                                : EVSE.ChargingStation.PaymentOptions.SafeSelect(paymentOption => paymentOption.ToOICP()),
                                         ValueAddedServices:                new ValueAddedServices[] { ValueAddedServices.None },
                                         Accessibility:                     accessibility.Value,
                                         HotlinePhoneNumber:                EVSE.ChargingStation.HotlinePhoneNumber.HasValue ? Phone_Number.Parse(EVSE.ChargingStation.HotlinePhoneNumber.Value.ToString()) : null,
                                         IsOpen24Hours:                     EVSE.ChargingStation.OpeningTimes is not null
                                                                                ? EVSE.ChargingStation.OpeningTimes?.ToOICP()?.Any() == true  // OpeningTimes == false AND an empty list is invalid at Hubject!
                                                                                      ? EVSE.ChargingStation.OpeningTimes.IsOpen24Hours
                                                                                      : true
                                                                                : true,
                                         IsHubjectCompatible:               EVSE.ChargingStation.Features.Contains(WWCP.Features.HubjectCompatible),
                                         DynamicInfoAvailable:              EVSE.ChargingStation.Features.Contains(WWCP.Features.StatusInfoAvailable)
                                                                                ? FalseTrueAuto.True
                                                                                : FalseTrueAuto.False,

                                         DeltaType:                         DeltaType,
                                         LastUpdate:                        LastUpdate,

                                         ChargingStationId:                 ChargingStation_Id.TryParse(EVSE.ChargingStation.Id.ToString()),
                                         ChargingPoolId:                    ChargingPool_Id.   TryParse(EVSE.ChargingPool?.  Id.ToString()),
                                         HardwareManufacturer:              null,
                                         ChargingStationImageURL:           null,
                                         SubOperatorName:                   EVSE.ChargingPool?.SubOperator?.Name?.FirstText(),
                                         DynamicPowerLevel:                 null,
                                         EnergySources:                     null,
                                         EnvironmentalImpact:               null,
                                         MaxCapacity:                       null,
                                         AccessibilityLocationType:         null,
                                         AdditionalInfo:                    (EVSE.Description ?? EVSE.ChargingStation.Description ?? EVSE.ChargingPool.Description).IsNeitherNullNorEmpty()
                                                                                ? new I18NText(
                                                                                      LanguageCode.Parse(
                                                                                          (EVSE.Description ?? EVSE.ChargingStation.Description ?? EVSE.ChargingPool.Description).First().Language.ToString()),
                                                                                          (EVSE.Description ?? EVSE.ChargingStation.Description ?? EVSE.ChargingPool.Description).FirstText().SubstringMax(150)
                                                                                      )
                                                                                : null,
                                         ChargingStationLocationReference:  null,
                                         GeoChargingPointEntrance:          EVSE.ChargingPool?.EntranceLocation.ToOICP(),
                                         OpeningTimes:                      EVSE.ChargingStation.OpeningTimes?.ToOICP(),
                                         HubOperatorId:                     EVSE.GetInternalDataAs<Operator_Id?>     (OICP_HubOperatorId),
                                         ClearingHouseId:                   EVSE.GetInternalDataAs<ClearingHouse_Id?>(OICP_ClearingHouseId),

                                         CustomData:                        EVSE.CustomData,
                                         InternalData:                      internalData
                                     );

                return EVSE2EVSEDataRecord is not null
                           ? EVSE2EVSEDataRecord(EVSE, evseDataRecord)
                           : evseDataRecord;

            }
            catch (Exception e)
            {
                throw new EVSEToOICPException(EVSE, e);
            }

        }

        #endregion


        #region ToOICP(this I18NString, MaxLength = null)

        public static I18NText ToOICP(this I18NString  I18NString,
                                      UInt16?          MaxLength   = null)

            => new (I18NString.Select(text => new KeyValuePair<LanguageCode, String>(LanguageCode.Parse(text.Language.ToString()),
                                                                                     MaxLength.HasValue
                                                                                         ? text.Text.SubstringMax(150)
                                                                                         : text.Text)));

        #endregion


        #region ToWWCP(this EVSEStatusType)

        /// <summary>
        /// Convert an OICP EVSE status into a corresponding WWCP EVSE status.
        /// </summary>
        /// <param name="EVSEStatusType">An OICP EVSE status.</param>
        /// <returns>The corresponding WWCP EVSE status.</returns>
        public static WWCP.EVSEStatusTypes? ToWWCP(this EVSEStatusTypes EVSEStatusType)

            => EVSEStatusType switch {
                   EVSEStatusTypes.Available     => WWCP.EVSEStatusTypes.Available,
                   EVSEStatusTypes.Reserved      => WWCP.EVSEStatusTypes.Reserved,
                   EVSEStatusTypes.Occupied      => WWCP.EVSEStatusTypes.Charging,
                   EVSEStatusTypes.OutOfService  => WWCP.EVSEStatusTypes.OutOfService,
                 //  EVSEStatusTypes.EVSENotFound  => WWCP.EVSEStatusTypes.UnknownEVSE,
                   _                             => WWCP.EVSEStatusTypes.OutOfService,
               };


        /// <summary>
        /// Convert an OICP EVSE status into a corresponding WWCP EVSE status.
        /// </summary>
        /// <param name="EVSEStatusType">An OICP EVSE status type.</param>
        /// <returns>The corresponding WWCP EVSE status.</returns>
        public static WWCP.EVSEStatusTypes? ToWWCP(this EVSEStatusTypes? EVSEStatusType)

            => EVSEStatusType.HasValue
                   ? EVSEStatusType.Value.ToWWCP()
                   : null;

        #endregion

        #region ToOICP(this EVSEStatusType)

        /// <summary>
        /// Convert a WWCP EVSE status into a corresponding OICP EVSE status.
        /// </summary>
        /// <param name="EVSEStatusType">An WWCP EVSE status.</param>
        /// <returns>The corresponding OICP EVSE status.</returns>
        public static EVSEStatusTypes? ToOICP(this WWCP.EVSEStatusTypes EVSEStatusType)
        {

            if      (EVSEStatusType == WWCP.EVSEStatusTypes.Available)
                return EVSEStatusTypes.Available;

            else if (EVSEStatusType == WWCP.EVSEStatusTypes.Reserved)
                return EVSEStatusTypes.Reserved;

            else if (EVSEStatusType == WWCP.EVSEStatusTypes.Charging)
                return EVSEStatusTypes.Occupied;

            else if (EVSEStatusType == WWCP.EVSEStatusTypes.OutOfService)
                return EVSEStatusTypes.OutOfService;

            else
                return EVSEStatusTypes.OutOfService;

        }


        /// <summary>
        /// Convert a WWCP EVSE status into a corresponding OICP EVSE status.
        /// </summary>
        /// <param name="EVSEStatusType">An WWCP EVSE status.</param>
        /// <returns>The corresponding OICP EVSE status.</returns>
        public static EVSEStatusTypes? ToOICP(this WWCP.EVSEStatusTypes? EVSEStatusType)

            => EVSEStatusType.HasValue
                   ? EVSEStatusType.Value.ToOICP()
                   : null;

        #endregion

        #region ToOICP(this EVSEStatus)

        /// <summary>
        /// Create a new OICP EVSE status record based on the given WWCP EVSE status.
        /// </summary>
        /// <param name="EVSEStatus">The current status of an EVSE.</param>
        public static EVSEStatusRecord? ToOICP(this WWCP.EVSEStatus EVSEStatus)
        {

            var evseId  = EVSEStatus.Id.    ToOICP();
            var status  = EVSEStatus.Status.ToOICP();

            if (evseId.HasValue && status.HasValue)
                return new EVSEStatusRecord(evseId.Value,
                                            status.Value);

            return null;

        }

        #endregion


        #region ToOICP(this WWCPAddress)

        /// <summary>
        /// Maps a WWCP address to an OICP address.
        /// </summary>
        /// <param name="WWCPAddress">A WWCP address.</param>
        public static Address ToOICP(this org.GraphDefined.Vanaheimr.Illias.Address WWCPAddress)

            => new (WWCPAddress.Country,
                    WWCPAddress.City.FirstText(),
                    WWCPAddress.Street,
                    WWCPAddress.PostalCode,
                    WWCPAddress.HouseNumber,
                    WWCPAddress.FloorLevel,
                    null,
                    null,
                    null,
                    WWCPAddress.TimeZone.HasValue
                        ? Time_Zone.Parse(WWCPAddress.TimeZone.Value.ToString())
                        : null);

        #endregion

        #region ToWWCP(this OICPAddress)

        /// <summary>
        /// Maps an OICP address type to a WWCP address type.
        /// </summary>
        /// <param name="OICPAddress">A address type.</param>
        public static org.GraphDefined.Vanaheimr.Illias.Address ToWWCP(this Address OICPAddress)

            => new (OICPAddress.Street,
                    OICPAddress.PostalCode,
                    I18NString.Create(Languages.de, OICPAddress.City),
                    OICPAddress.Country,
                    OICPAddress.HouseNumber,
                    OICPAddress.Floor);

        #endregion


        #region ToOICP(this GeoLocation)

        public static GeoCoordinates ToOICP(this org.GraphDefined.Vanaheimr.Aegir.GeoCoordinate GeoLocation)

            => new (GeoLocation.Latitude. Value,
                    GeoLocation.Longitude.Value);

        public static GeoCoordinates? ToOICP(this org.GraphDefined.Vanaheimr.Aegir.GeoCoordinate? GeoLocation)

            => GeoLocation.HasValue
                   ? GeoLocation.Value.ToOICP()
                   : null;

        #endregion

        #region ToOICP(this GeoLocation)

        public static org.GraphDefined.Vanaheimr.Aegir.GeoCoordinate ToWWCP(this GeoCoordinates GeoLocation)

            => new (org.GraphDefined.Vanaheimr.Aegir.Latitude. Parse(GeoLocation.Latitude),
                    org.GraphDefined.Vanaheimr.Aegir.Longitude.Parse(GeoLocation.Longitude));

        public static org.GraphDefined.Vanaheimr.Aegir.GeoCoordinate? ToWWCP(this GeoCoordinates? GeoLocation)

            => GeoLocation.HasValue
                   ? GeoLocation.Value.ToWWCP()
                   : null;

        #endregion



        #region ToOICP(this OperatorId, Format = WWCP.OperatorIdFormats.ISO_STAR)

        public static Operator_Id? ToOICP(this WWCP.ChargingStationOperator_Id  OperatorId,
                                          WWCP.OperatorIdFormats                Format = WWCP.OperatorIdFormats.ISO_STAR)

            => Operator_Id.TryParse(OperatorId.ToString(Format));

        public static Operator_Id? ToOICP(this WWCP.ChargingStationOperator_Id? OperatorId)

            => OperatorId.HasValue
                   ? OperatorId.Value.ToOICP()
                   : null;

        #endregion

        #region ToWWCP(this OperatorId)

        public static WWCP.ChargingStationOperator_Id? ToWWCP(this Operator_Id OperatorId)

            => WWCP.ChargingStationOperator_Id.TryParse(OperatorId.ToString());

        public static WWCP.ChargingStationOperator_Id? ToWWCP(this Operator_Id? OperatorId)

            => OperatorId.HasValue
                   ? OperatorId.Value.ToWWCP()
                   : null;

        #endregion


        #region ToOICP(this ProviderId)

        public static Provider_Id? ToOICP(this WWCP.EMobilityProvider_Id ProviderId)

            => Provider_Id.TryParse(ProviderId.ToString());

        public static Provider_Id? ToOICP(this WWCP.EMobilityProvider_Id? ProviderId)

            => ProviderId.HasValue
                   ? ProviderId.Value.ToOICP()
                   : null;

        #endregion

        #region ToWWCP(this ProviderId)

        public static WWCP.EMobilityProvider_Id? ToWWCP(this Provider_Id ProviderId)

            => WWCP.EMobilityProvider_Id.TryParse(ProviderId.ToString());

        public static WWCP.EMobilityProvider_Id? ToWWCP(this Provider_Id? ProviderId)

            => ProviderId.HasValue
                   ? ProviderId.Value.ToWWCP()
                   : null;

        #endregion


        #region ToOICP(this EVSEId, CustomConverter = null)

        public static EVSE_Id? ToOICP(this WWCP.EVSE_Id              EVSEId,
                                      WWCPEVSEId_2_EVSEId_Delegate?  CustomConverter   = null)

            => CustomConverter is not null
                   ? CustomConverter(EVSEId)
                   : EVSE_Id.TryParse(EVSEId.ToString());


        public static EVSE_Id? ToOICP(this WWCP.EVSE_Id?             EVSEId,
                                      WWCPEVSEId_2_EVSEId_Delegate?  CustomConverter   = null)

            => EVSEId.HasValue
                   ? EVSEId.Value.ToOICP(CustomConverter)
                   : null;

        #endregion

        #region ToWWCP(this EVSEId)

        public static WWCP.EVSE_Id? ToWWCP(this EVSE_Id EVSEId)

            => WWCP.EVSE_Id.TryParse(EVSEId.ToString(),
                                     WWCP.EVSEIdParsingMode.relaxed);

        public static WWCP.EVSE_Id? ToWWCP(this EVSE_Id? EVSEId)

            => EVSEId.HasValue
                   ? EVSEId.Value.ToWWCP()
                   : null;

        #endregion


        #region ToOICP(this ChargingStationId)

        public static ChargingStation_Id? ToOICP(this WWCP.ChargingStation_Id ChargingStationId)

            => ChargingStation_Id.TryParse(ChargingStationId.ToString());


        public static ChargingStation_Id? ToOICP(this WWCP.ChargingStation_Id? ChargingStationId)

            => ChargingStationId.HasValue
                   ? ChargingStationId.Value.ToOICP()
                   : null;

        #endregion

        #region ToWWCP(this ChargingStationId)

        public static WWCP.ChargingStation_Id? ToWWCP(this ChargingStation_Id ChargingStationId)

            => WWCP.ChargingStation_Id.TryParse(ChargingStationId.ToString());

        public static WWCP.ChargingStation_Id? ToWWCP(this ChargingStation_Id? ChargingStationId)

            => ChargingStationId.HasValue
                   ? ChargingStationId.Value.ToWWCP()
                   : null;

        #endregion


        #region ToOICP(this ChargingPoolId)

        public static ChargingPool_Id? ToOICP(this WWCP.ChargingPool_Id ChargingPoolId)

            => ChargingPool_Id.TryParse(ChargingPoolId.ToString());


        public static ChargingPool_Id? ToOICP(this WWCP.ChargingPool_Id? ChargingPoolId)

            => ChargingPoolId.HasValue
                   ? ChargingPoolId.Value.ToOICP()
                   : null;

        #endregion

        #region ToWWCP(this ChargingPoolId)

        public static WWCP.ChargingPool_Id? ToWWCP(this ChargingPool_Id ChargingPoolId)

            => WWCP.ChargingPool_Id.TryParse(ChargingPoolId.ToString());

        public static WWCP.ChargingPool_Id? ToWWCP(this ChargingPool_Id? ChargingPoolId)

            => ChargingPoolId.HasValue
                   ? ChargingPoolId.Value.ToWWCP()
                   : null;

        #endregion


        #region ToOICP(this SessionId)

        public static Session_Id? ToOICP(this WWCP.ChargingSession_Id SessionId)

            => Session_Id.TryParse(SessionId.ToString());

        public static Session_Id? ToOICP(this WWCP.ChargingSession_Id? SessionId)

            => SessionId.HasValue
                   ? SessionId.Value.ToOICP()
                   : null;

        #endregion

        #region ToWWCP(this SessionId)

        public static WWCP.ChargingSession_Id? ToWWCP(this Session_Id SessionId)

            => WWCP.ChargingSession_Id.TryParse(SessionId.ToString());

        public static WWCP.ChargingSession_Id? ToWWCP(this Session_Id? SessionId)

            => SessionId.HasValue
                   ? SessionId.Value.ToWWCP()
                   : null;

        #endregion


        #region ToWWCP(this EMPPartnerSessionId)

        public static WWCP.ChargingSession_Id? ToWWCP(this EMPPartnerSession_Id EMPPartnerSessionId)

            => WWCP.ChargingSession_Id.TryParse(EMPPartnerSessionId.ToString());

        public static WWCP.ChargingSession_Id? ToWWCP(this EMPPartnerSession_Id? EMPPartnerSessionId)

            => EMPPartnerSessionId.HasValue
                   ? EMPPartnerSessionId.Value.ToWWCP()
                   : null;

        #endregion

        #region ToWWCP(this CPOPartnerSessionId)

        public static WWCP.ChargingSession_Id? ToWWCP(this CPOPartnerSession_Id CPOPartnerSessionId)

            => WWCP.ChargingSession_Id.TryParse(CPOPartnerSessionId.ToString());

        public static WWCP.ChargingSession_Id? ToWWCP(this CPOPartnerSession_Id? CPOPartnerSessionId)

            => CPOPartnerSessionId.HasValue
                   ? CPOPartnerSessionId.Value.ToWWCP()
                   : null;

        #endregion


        #region ToOICP(this ChargingReservationId)

        public static Session_Id? ToOICP(this WWCP.ChargingReservation_Id ChargingReservationId)

            => Session_Id.Parse(ChargingReservationId.Suffix);

        public static Session_Id? ToOICP(this WWCP.ChargingReservation_Id? ChargingReservationId)

            => ChargingReservationId.HasValue
                   ? ChargingReservationId.Value.ToOICP()
                   : null;

        #endregion

        #region ToOICP(this ChargingReservation)

        public static Session_Id? ToOICP(this WWCP.ChargingReservation ChargingReservation)

            => ChargingReservation?.Id.ToOICP();

        #endregion


        #region ToOICP(this ProductId)

        public static PartnerProduct_Id? ToOICP(this WWCP.ChargingProduct_Id ChargingProductId)

            => PartnerProduct_Id.TryParse(ChargingProductId.ToString());

        public static PartnerProduct_Id? ToOICP(this WWCP.ChargingProduct_Id? ChargingProductId)

            => ChargingProductId.HasValue
                   ? ChargingProductId.Value.ToOICP()
                   : null;

        public static PartnerProduct_Id? ToOICP(this WWCP.ChargingProduct ChargingProduct)

            => ChargingProduct is null
                   ? new PartnerProduct_Id?()
                   : ChargingProduct.Id.ToOICP();

        #endregion

        #region ToWWCP(this ProductId)

        public static WWCP.ChargingProduct_Id? ToWWCP(this PartnerProduct_Id PartnerProductId)

            => WWCP.ChargingProduct_Id.TryParse(PartnerProductId.ToString());

        public static WWCP.ChargingProduct_Id? ToWWCP(this PartnerProduct_Id? PartnerProductId)

            => PartnerProductId.HasValue
                   ? PartnerProductId.Value.ToWWCP()
                   : null;

        #endregion


        //public static EVCO_Id ToOICP(this RemoteAuthentication RemoteAuthentication)
        //    => EVCO_Id.Parse(RemoteAuthentication.RemoteIdentification.ToString());

        public static WWCP.RemoteAuthentication ToWWCP(this EVCO_Id EVCOId)
            => WWCP.RemoteAuthentication.FromRemoteIdentification(WWCP.eMobilityAccount_Id.Parse(EVCOId.ToString()));



        public static EVCO_Id ToOICP(this WWCP.eMobilityAccount_Id eMAId)
            => EVCO_Id.Parse(eMAId.ToString());

        public static WWCP.eMobilityAccount_Id ToWWCP_eMAId(this EVCO_Id EVCOId)
            => WWCP.eMobilityAccount_Id.Parse(EVCOId.ToString());


        public static EVCO_Id? ToOICP(this WWCP.eMobilityAccount_Id? eMAId)
            => eMAId.HasValue
                   ? EVCO_Id.Parse(eMAId.Value.ToString())
                   : null;

        public static WWCP.eMobilityAccount_Id? ToWWCP(this EVCO_Id? EVCOId)
            => EVCOId.HasValue
                   ? WWCP.eMobilityAccount_Id.Parse(EVCOId.Value.ToString())
                   : null;






        public static UID ToOICP(this WWCP.AuthenticationToken AuthToken)

            => UID.Parse(AuthToken.ToString().ToUpper());

        public static UID? ToOICP(this WWCP.AuthenticationToken? AuthToken)

            => AuthToken.HasValue
                   ? UID.Parse(AuthToken.Value.ToString().ToUpper())
                   : null;


        public static WWCP.AuthenticationToken ToWWCP(this UID UID)

            => WWCP.AuthenticationToken.Parse(UID.ToString());




        #region ToWWCP(this Identification)

        public static WWCP.RemoteAuthentication? ToWWCP(this Identification Identification)
        {

            if (Identification.RFIDId.HasValue)
                return WWCP.RemoteAuthentication.FromAuthToken(WWCP.AuthenticationToken.Parse (Identification.RFIDId.Value.ToString()));

            if (Identification.RFIDIdentification is not null)
                return WWCP.RemoteAuthentication.FromAuthToken(WWCP.AuthenticationToken.Parse (Identification.RFIDIdentification.         UID.   ToString()));

            if (Identification.QRCodeIdentification.HasValue && Identification.QRCodeIdentification.Value.PIN.HasValue)
                return WWCP.RemoteAuthentication.FromQRCodeIdentification       (Identification.QRCodeIdentification.Value.EVCOId.ToWWCP_eMAId(),
                                                                                 Identification.QRCodeIdentification.Value.PIN.      Value.ToString());

            if (Identification.QRCodeIdentification.HasValue && Identification.QRCodeIdentification.Value.HashedPIN.HasValue)
                return WWCP.RemoteAuthentication.FromQRCodeIdentification       (Identification.QRCodeIdentification.Value.EVCOId.ToWWCP_eMAId(),
                                                                                 Identification.QRCodeIdentification.Value.HashedPIN.Value.ToString());

            if (Identification.PlugAndChargeIdentification.HasValue)
                return WWCP.RemoteAuthentication.FromPlugAndChargeIdentification(Identification.PlugAndChargeIdentification.Value.ToWWCP_eMAId());

            if (Identification.RemoteIdentification.HasValue)
                return WWCP.RemoteAuthentication.FromRemoteIdentification       (Identification.RemoteIdentification.       Value.ToWWCP_eMAId());

            return null;

        }

        #endregion

        #region ToOICP(this Authentication)

        public static Identification? ToOICP(this WWCP.AAuthentication Authentication)
        {

            if (Authentication.AuthToken.                  HasValue)
                return Identification.FromUID                        (UID.Parse(Authentication.AuthToken.Value.ToString().ToUpper()));

            if (Authentication.QRCodeIdentification.       HasValue)
                return Identification.FromQRCodeIdentification       (          Authentication.QRCodeIdentification.Value.eMAId.ToOICP(),
                                                                      PIN.Parse(Authentication.QRCodeIdentification.Value.PIN));

            if (Authentication.PlugAndChargeIdentification.HasValue)
                return Identification.FromPlugAndChargeIdentification(Authentication.PlugAndChargeIdentification.Value.ToOICP());

            if (Authentication.RemoteIdentification.       HasValue)
                return Identification.FromRemoteIdentification       (Authentication.RemoteIdentification.       Value.ToOICP());

            return null;

        }

        #endregion


        #region ToOICP(this AccessibilityType)

        public static AccessibilityTypes ToOICP(this WWCP.AccessibilityTypes AccessibilityType)

            => AccessibilityType switch {
                   WWCP.AccessibilityTypes.FreePubliclyAccessible    => AccessibilityTypes.FreePubliclyAccessible,
                   WWCP.AccessibilityTypes.RestrictedAccess          => AccessibilityTypes.RestrictedAccess,
                   WWCP.AccessibilityTypes.PayingPubliclyAccessible  => AccessibilityTypes.PayingPubliclyAccessible,
                   WWCP.AccessibilityTypes.TestStation               => AccessibilityTypes.TestStation,
                   _                                                 => throw new ArgumentException("Invalid accessibility type!")
               };

        public static AccessibilityTypes? ToOICP(this WWCP.AccessibilityTypes? AccessibilityType)

            => AccessibilityType.HasValue
                   ? AccessibilityType.Value.ToOICP()
                   : default;

        #endregion

        #region ToWWCP(this AccessibilityType)

        public static WWCP.AccessibilityTypes ToWWCP(this AccessibilityTypes AccessibilityType)

            => AccessibilityType switch {
                   AccessibilityTypes.FreePubliclyAccessible    => WWCP.AccessibilityTypes.FreePubliclyAccessible,
                   AccessibilityTypes.RestrictedAccess          => WWCP.AccessibilityTypes.RestrictedAccess,
                   AccessibilityTypes.PayingPubliclyAccessible  => WWCP.AccessibilityTypes.PayingPubliclyAccessible,
                   AccessibilityTypes.TestStation               => WWCP.AccessibilityTypes.TestStation,
                   _                                            => throw new ArgumentException("Invalid accessibility type!")
               };

        public static WWCP.AccessibilityTypes? ToWWCP(this AccessibilityTypes? AccessibilityType)

            => AccessibilityType.HasValue
                   ? AccessibilityType.Value.ToWWCP()
                   : new WWCP.AccessibilityTypes?();

        #endregion


        #region ToWWCP(this PaymentOption)

        public static WWCP.PaymentOptions ToWWCP(this PaymentOptions PaymentOption)

            => PaymentOption switch {
                   PaymentOptions.NoPayment  => WWCP.PaymentOptions.Free,
                   PaymentOptions.Direct     => WWCP.PaymentOptions.Direct,
                   PaymentOptions.Contract   => WWCP.PaymentOptions.Contract,
                   _                         => throw new ArgumentException("Invalid payment option!")
               };

        #endregion

        #region ToOICP(this PaymentOption)

        public static PaymentOptions ToOICP(this WWCP.PaymentOptions PaymentOption)

            => PaymentOption switch {
                   WWCP.PaymentOptions.Free      => PaymentOptions.NoPayment,
                   WWCP.PaymentOptions.Direct    => PaymentOptions.Direct,
                   WWCP.PaymentOptions.Contract  => PaymentOptions.Contract,
                   _                             => throw new ArgumentException("Invalid payment option!")
               };

        #endregion


        #region ToOICP(this AuthenticationMode)

        /// <summary>
        /// Maps a WWCP authentication mode to an OICP authentication mode.
        /// </summary>
        /// <param name="WWCPAuthMode">A WWCP-representation of an authentication mode.</param>
        public static AuthenticationModes? ToOICP(this WWCP.AuthenticationModes  AuthenticationMode,
                                                  Boolean                        FailOnUnknown = true)
        {

            switch (AuthenticationMode.Type)
            {

                case "RFID":
                    if ((AuthenticationMode as WWCP.AuthenticationModes.RFID).CardTypes.Any(cardType => cardType == WWCP.RFIDCardTypes.MifareClassic))
                        return AuthenticationModes.NFC_RFID_Classic;

                    if ((AuthenticationMode as WWCP.AuthenticationModes.RFID).CardTypes.Any(cardType => cardType == WWCP.RFIDCardTypes.MifareDESFire))
                        return AuthenticationModes.NFC_RFID_DESFire;

                    if (FailOnUnknown)
                        throw new ArgumentException("Unknown AuthenticationMode '" + AuthenticationMode.Type + "'!");
                    return new AuthenticationModes?();

                case "ISO/IEC 15118 PLC":
                    return AuthenticationModes.PnC;

                case "REMOTE":
                    return AuthenticationModes.REMOTE;

                case "Direct payment":
                    return AuthenticationModes.DirectPayment;

                case "No authentication required":
                    return AuthenticationModes.NoAuthenticationRequired;

                default:
                    if (FailOnUnknown)
                        throw new ArgumentException("Unknown AuthenticationMode '" + AuthenticationMode.Type + "'!");
                    return new AuthenticationModes?();

            }

        }

        public static IEnumerable<AuthenticationModes> ToOICP(this IEnumerable<WWCP.AuthenticationModes> AuthenticationModes)
        {

            if (AuthenticationModes == null)
                return new AuthenticationModes[0];

            var authenticationModes = new List<AuthenticationModes>();

            foreach (var authenticationMode in AuthenticationModes)
            {

                var _authenticationMode = authenticationMode.ToOICP(FailOnUnknown: false);

                if (_authenticationMode.HasValue)
                    authenticationModes.Add(_authenticationMode.Value);

            }

            return authenticationModes;

        }

        #endregion

        #region ToWWCP(this AuthenticationMode)

        public static WWCP.AuthenticationModes ToWWCP(this AuthenticationModes AuthenticationMode)

            => AuthenticationMode switch {
                   AuthenticationModes.NFC_RFID_Classic          => new WWCP.AuthenticationModes.RFID(WWCP.RFIDCardTypes.MifareClassic),
                   AuthenticationModes.NFC_RFID_DESFire          => new WWCP.AuthenticationModes.RFID(WWCP.RFIDCardTypes.MifareDESFire),
                   AuthenticationModes.PnC                       => new WWCP.AuthenticationModes.ISO15118_PLC(),
                   AuthenticationModes.REMOTE                    => new WWCP.AuthenticationModes.REMOTE(),
                   AuthenticationModes.DirectPayment             => new WWCP.AuthenticationModes.DirectPayment(),
                   AuthenticationModes.NoAuthenticationRequired  => new WWCP.AuthenticationModes.NoAuthenticationRequired(),
                   _                                             => throw new ArgumentException("Invalid authentication mode!")
               };

        #endregion


        #region ToWWCP(this ChargingMode)

        public static WWCP.ChargingModes ToWWCP(this ChargingModes ChargingMode)

            => ChargingMode switch {
                ChargingModes.Mode_1   => WWCP.ChargingModes.Mode_1,
                ChargingModes.Mode_2   => WWCP.ChargingModes.Mode_2,
                ChargingModes.Mode_3   => WWCP.ChargingModes.Mode_3,
                ChargingModes.Mode_4   => WWCP.ChargingModes.Mode_4,
                ChargingModes.CHAdeMO  => WWCP.ChargingModes.CHAdeMO,
                _                      => throw new ArgumentException("Invalid charging mode!")
            };

        public static WWCP.ChargingModes? ToWWCP(this ChargingModes? ChargingMode)

            => ChargingMode.HasValue
                   ? ChargingMode switch {
                         ChargingModes.Mode_1   => WWCP.ChargingModes.Mode_1,
                         ChargingModes.Mode_2   => WWCP.ChargingModes.Mode_2,
                         ChargingModes.Mode_3   => WWCP.ChargingModes.Mode_3,
                         ChargingModes.Mode_4   => WWCP.ChargingModes.Mode_4,
                         ChargingModes.CHAdeMO  => WWCP.ChargingModes.CHAdeMO,
                         _                      => new WWCP.ChargingModes?()
                   }
                   : default;

        #endregion

        #region ToOICP(this ChargingMode)

        public static ChargingModes ToOICP(this WWCP.ChargingModes ChargingMode)

            => ChargingMode switch {
                   WWCP.ChargingModes.Mode_1   => ChargingModes.Mode_1,
                   WWCP.ChargingModes.Mode_2   => ChargingModes.Mode_2,
                   WWCP.ChargingModes.Mode_3   => ChargingModes.Mode_3,
                   WWCP.ChargingModes.Mode_4   => ChargingModes.Mode_4,
                   WWCP.ChargingModes.CHAdeMO  => ChargingModes.CHAdeMO,
                   _                           => throw new ArgumentException("Invalid charging mode!")
               };

        public static ChargingModes? ToOICP(this WWCP.ChargingModes? ChargingMode)

            => ChargingMode.HasValue
                   ? ChargingMode switch {
                         WWCP.ChargingModes.Mode_1   => ChargingModes.Mode_1,
                         WWCP.ChargingModes.Mode_2   => ChargingModes.Mode_2,
                         WWCP.ChargingModes.Mode_3   => ChargingModes.Mode_3,
                         WWCP.ChargingModes.Mode_4   => ChargingModes.Mode_4,
                         WWCP.ChargingModes.CHAdeMO  => ChargingModes.CHAdeMO,
                         _                           => new ChargingModes?()
                     }
                   : default;

        #endregion


        #region ToWWCP(this PlugType)

        public static WWCP.ChargingPlugTypes ToWWCP(this PlugTypes PlugType)

            => PlugType switch {
                   PlugTypes.SmallPaddleInductive          => WWCP.ChargingPlugTypes.SmallPaddleInductive,
                   PlugTypes.LargePaddleInductive          => WWCP.ChargingPlugTypes.LargePaddleInductive,
                   PlugTypes.AVCONConnector                => WWCP.ChargingPlugTypes.AVCONConnector,
                   PlugTypes.TeslaConnector                => WWCP.ChargingPlugTypes.TeslaConnector,
                   PlugTypes.NEMA5_20                      => WWCP.ChargingPlugTypes.NEMA5_20,
                   PlugTypes.TypeEFrenchStandard           => WWCP.ChargingPlugTypes.TypeEFrenchStandard,
                   PlugTypes.TypeFSchuko                   => WWCP.ChargingPlugTypes.TypeFSchuko,
                   PlugTypes.TypeGBritishStandard          => WWCP.ChargingPlugTypes.TypeGBritishStandard,
                   PlugTypes.TypeJSwissStandard            => WWCP.ChargingPlugTypes.TypeJSwissStandard,
                   PlugTypes.Type1Connector_CableAttached  => WWCP.ChargingPlugTypes.Type1Connector_CableAttached,
                   PlugTypes.Type2Outlet                   => WWCP.ChargingPlugTypes.Type2Outlet,
                   PlugTypes.Type2Connector_CableAttached  => WWCP.ChargingPlugTypes.Type2Connector_CableAttached,
                   PlugTypes.Type3Outlet                   => WWCP.ChargingPlugTypes.Type3Outlet,
                   PlugTypes.IEC60309SinglePhase           => WWCP.ChargingPlugTypes.IEC60309SinglePhase,
                   PlugTypes.IEC60309ThreePhase            => WWCP.ChargingPlugTypes.IEC60309ThreePhase,
                   PlugTypes.CCSCombo2Plug_CableAttached   => WWCP.ChargingPlugTypes.CCSCombo2Plug_CableAttached,
                   PlugTypes.CCSCombo1Plug_CableAttached   => WWCP.ChargingPlugTypes.CCSCombo1Plug_CableAttached,
                   PlugTypes.CHAdeMO                       => WWCP.ChargingPlugTypes.CHAdeMO,
                   _                                       => throw new ArgumentException("Invalid plug type!")
               };

        #endregion

        #region ToOICP(this PlugType)

        public static PlugTypes ToOICP(this WWCP.ChargingPlugTypes PlugType)

            => PlugType switch {
                   WWCP.ChargingPlugTypes.SmallPaddleInductive          => PlugTypes.SmallPaddleInductive,
                   WWCP.ChargingPlugTypes.LargePaddleInductive          => PlugTypes.LargePaddleInductive,
                   WWCP.ChargingPlugTypes.AVCONConnector                => PlugTypes.AVCONConnector,
                   WWCP.ChargingPlugTypes.TeslaConnector                => PlugTypes.TeslaConnector,
                   WWCP.ChargingPlugTypes.NEMA5_20                      => PlugTypes.NEMA5_20,
                   WWCP.ChargingPlugTypes.TypeEFrenchStandard           => PlugTypes.TypeEFrenchStandard,
                   WWCP.ChargingPlugTypes.TypeFSchuko                   => PlugTypes.TypeFSchuko,
                   WWCP.ChargingPlugTypes.TypeGBritishStandard          => PlugTypes.TypeGBritishStandard,
                   WWCP.ChargingPlugTypes.TypeJSwissStandard            => PlugTypes.TypeJSwissStandard,
                   WWCP.ChargingPlugTypes.Type1Connector_CableAttached  => PlugTypes.Type1Connector_CableAttached,
                   WWCP.ChargingPlugTypes.Type2Outlet                   => PlugTypes.Type2Outlet,
                   WWCP.ChargingPlugTypes.Type2Connector_CableAttached  => PlugTypes.Type2Connector_CableAttached,
                   WWCP.ChargingPlugTypes.Type3Outlet                   => PlugTypes.Type3Outlet,
                   WWCP.ChargingPlugTypes.IEC60309SinglePhase           => PlugTypes.IEC60309SinglePhase,
                   WWCP.ChargingPlugTypes.IEC60309ThreePhase            => PlugTypes.IEC60309ThreePhase,
                   WWCP.ChargingPlugTypes.CCSCombo2Plug_CableAttached   => PlugTypes.CCSCombo2Plug_CableAttached,
                   WWCP.ChargingPlugTypes.CCSCombo1Plug_CableAttached   => PlugTypes.CCSCombo1Plug_CableAttached,
                   WWCP.ChargingPlugTypes.CHAdeMO                       => PlugTypes.CHAdeMO,
                   _                                            => throw new ArgumentException("Invalid plug type!")
               };

        #endregion


        #region AsChargingFacility  (this EVSE)

        /// <summary>
        /// Maps a WWCP EVSE to an OICP charging facility.
        /// </summary>
        /// <param name="EVSE">An WWCP EVSE.</param>
        public static ChargingFacility AsChargingFacility(this WWCP.IEVSE EVSE)
        {

            if (!EVSE.MaxPower.HasValue)
                throw new ArgumentException("AsOICPChargingFacility failed!");

            var powerType = EVSE.CurrentType switch {
                WWCP.CurrentTypes.AC_OnePhase     => PowerTypes.AC_1_PHASE,
                WWCP.CurrentTypes.AC_ThreePhases  => PowerTypes.AC_3_PHASE,
                WWCP.CurrentTypes.DC              => PowerTypes.DC,
                WWCP.CurrentTypes.Unspecified     => PowerTypes.Unspecified,
                _                                 => throw new ArgumentException("Invalid current type!"),
            };

            return new ChargingFacility(
                       powerType,
                       Convert.ToUInt32(EVSE.MaxPower),
                       EVSE.AverageVoltage.HasValue ? new UInt32?(Convert.ToUInt32(EVSE.AverageVoltage)) : null,
                       EVSE.MaxCurrent.    HasValue ? new UInt32?(Convert.ToUInt32(EVSE.MaxCurrent))     : null,
                       EVSE.ChargingModes.Select(chargingMode => chargingMode.ToOICP())
                   );

        }

        #endregion

        #region AsChargingFacilities(this EVSE)

        /// <summary>
        /// Maps a WWCP EVSE to an enumeration of charging facilities.
        /// </summary>
        /// <param name="EVSE">An WWCP EVSE.</param>
        public static IEnumerable<ChargingFacility> AsChargingFacilities(this WWCP.IEVSE EVSE)
        {

            if (!EVSE.MaxPower.HasValue)
                return Array.Empty<ChargingFacility>();


            var powerType = PowerTypes.AC_1_PHASE;

            switch (EVSE.CurrentType)
            {

                case WWCP.CurrentTypes.AC_OnePhase:
                    powerType = PowerTypes.AC_1_PHASE;
                    break;

                case WWCP.CurrentTypes.AC_ThreePhases:
                    powerType = PowerTypes.AC_3_PHASE;
                    break;

                case WWCP.CurrentTypes.DC:
                    powerType = PowerTypes.DC;
                    break;

                case WWCP.CurrentTypes.Unspecified:
                    powerType = PowerTypes.Unspecified;
                    break;

            }

            return new ChargingFacility[] {
                       new ChargingFacility(
                           powerType,
                           Convert.ToUInt32(EVSE.MaxPower.Value),
                           EVSE.AverageVoltage.HasValue ? new UInt32?(Convert.ToUInt32(EVSE.AverageVoltage)) : null,
                           EVSE.MaxCurrent.    HasValue ? new UInt32?(Convert.ToUInt32(EVSE.MaxCurrent))     : null,
                           EVSE.ChargingModes.Select(chargingMode => chargingMode.ToOICP())
                       )
                   };

        }

        #endregion

        #region ApplyChargingFacilities(this ChargingFacilities, EVSE)

        //public static void ApplyChargingFacilities(this IEnumerable<ChargingFacility> ChargingFacilities, EVSE EVSE)
        //{

        //    foreach (var chargingFacility in ChargingFacilities)
        //    {

        //        if (chargingFacility.PowerType.HasValue && !EVSE.CurrentType.HasValue)
        //            EVSE.CurrentType = chargingFacility.PowerType.ToWWCP();

        //        if (chargingFacility.Voltage.HasValue && (!EVSE.AverageVoltage.HasValue || EVSE.AverageVoltage.Value < Convert.ToDecimal(chargingFacility.Voltage. Value)))
        //            EVSE.AverageVoltage = chargingFacility.Voltage;

        //        if (chargingFacility.Amperage.HasValue && (!EVSE.MaxCurrent.   HasValue || EVSE.MaxCurrent.    Value < Convert.ToDecimal(chargingFacility.Amperage.Value)))
        //            EVSE.MaxCurrent     = chargingFacility.Amperage;

        //        if (chargingFacility.Power.   HasValue && (!EVSE.MaxPower.     HasValue || EVSE.MaxPower.      Value < Convert.ToDecimal(chargingFacility.Power.   Value)))
        //            EVSE.MaxPower       = chargingFacility.Power;

        //    }

        //}

        #endregion


        #region ToWWCP(this OpeningTime)

        /// <summary>
        /// Convert the given OICP opening time enumeration and 24/7 open information into an WWCP opening time.
        /// </summary>
        /// <param name="OpeningTimes">An OICP opening time enumeration.</param>
        public static OpeningTimes? ToWWCP(this IEnumerable<OpeningTime>  OpeningTime)
        {

            var openingTimes = OpeningTimes.Open24Hours;

            foreach (var openingTime in OpeningTime)
            {
                foreach (var period in openingTime.Periods)
                {

                    var begin  = period.Begin.ToWWCP();
                    var end    = period.End.  ToWWCP();

                    switch (openingTime.On)
                    {

                        case DaysOfWeek.Everyday:
                            openingTimes.AddRegularOpening(DayOfWeek.Monday,     begin, end);
                            openingTimes.AddRegularOpening(DayOfWeek.Tuesday,    begin, end);
                            openingTimes.AddRegularOpening(DayOfWeek.Wednesday,  begin, end);
                            openingTimes.AddRegularOpening(DayOfWeek.Thursday,   begin, end);
                            openingTimes.AddRegularOpening(DayOfWeek.Friday,     begin, end);
                            openingTimes.AddRegularOpening(DayOfWeek.Saturday,   begin, end);
                            openingTimes.AddRegularOpening(DayOfWeek.Sunday,     begin, end);
                            break;

                        case DaysOfWeek.Workdays:
                            openingTimes.AddRegularOpening(DayOfWeek.Monday,     begin, end);
                            openingTimes.AddRegularOpening(DayOfWeek.Tuesday,    begin, end);
                            openingTimes.AddRegularOpening(DayOfWeek.Wednesday,  begin, end);
                            openingTimes.AddRegularOpening(DayOfWeek.Thursday,   begin, end);
                            openingTimes.AddRegularOpening(DayOfWeek.Friday,     begin, end);
                            break;

                        case DaysOfWeek.Weekend:
                            openingTimes.AddRegularOpening(DayOfWeek.Saturday,   begin, end);
                            openingTimes.AddRegularOpening(DayOfWeek.Sunday,     begin, end);
                            break;

                        case DaysOfWeek.Monday:
                            openingTimes.AddRegularOpening(DayOfWeek.Monday,     begin, end);
                            break;

                        case DaysOfWeek.Tuesday:
                            openingTimes.AddRegularOpening(DayOfWeek.Tuesday,    begin, end);
                            break;

                        case DaysOfWeek.Wednesday:
                            openingTimes.AddRegularOpening(DayOfWeek.Wednesday,  begin, end);
                            break;

                        case DaysOfWeek.Thursday:
                            openingTimes.AddRegularOpening(DayOfWeek.Thursday,   begin, end);
                            break;

                        case DaysOfWeek.Friday:
                            openingTimes.AddRegularOpening(DayOfWeek.Friday,     begin, end);
                            break;

                        case DaysOfWeek.Saturday:
                            openingTimes.AddRegularOpening(DayOfWeek.Saturday,   begin, end);
                            break;

                        case DaysOfWeek.Sunday:
                            openingTimes.AddRegularOpening(DayOfWeek.Sunday,     begin, end);
                            break;

                        default:
                            return default;

                    };

                }
            }

            return openingTimes;

        }

        #endregion

        #region ToOICP(this OpeningTimes)

        /// <summary>
        /// Convert the given WWCP opening times into an OICP opening time enumeration.
        /// </summary>
        /// <param name="OpeningTimes">A WWCP opening times object.</param>
        public static IEnumerable<OpeningTime>? ToOICP(this OpeningTimes OpeningTimes)
        {

            var openingTimes          = new List<OpeningTime>();
            var regularOpeningGroups  = OpeningTimes.RegularOpenings.GroupBy(kvp => kvp.Key);

            foreach (var regularOpeningGroup in OpeningTimes.RegularOpenings)
            {
                openingTimes.Add(new OpeningTime(regularOpeningGroup.Value.Select(period => new Period(period.PeriodBegin.ToOICP(),
                                                                                                       period.PeriodEnd.  ToOICP())),
                                                 regularOpeningGroup.Key.  ToOICP()));
            }

            return openingTimes.Any()
                       ? openingTimes
                       : null;

        }

        #endregion


        #region ToWWCP(this HourMinute)

        /// <summary>
        /// Convert the given OICP HourMinute object into a WWCP HourMin object.
        /// </summary>
        /// <param name="HourMinute">An OICP HourMinute object.</param>
        public static HourMin ToWWCP(this HourMinute HourMinute)

            => new (HourMinute.Hour,
                    HourMinute.Minute);

        #endregion

        #region ToOICP(this HourMin)

        /// <summary>
        /// Convert the given WWCP HourMin object into an OICP HourMinute object.
        /// </summary>
        /// <param name="HourMin">A WWCP HourMin object.</param>
        public static HourMinute ToOICP(this HourMin HourMin)

            => new (HourMin.Hour,
                    HourMin.Minute);

        #endregion


        #region ToOICP(this DayOfWeek)

        /// <summary>
        /// Convert the given WWCP DayOfWeek object into an OICP DaysOfWeek object.
        /// </summary>
        /// <param name="DayOfWeek">A WWCP DayOfWeek object.</param>
        public static DaysOfWeek ToOICP(this DayOfWeek DayOfWeek)

            => DayOfWeek switch {
                   DayOfWeek.Monday     => DaysOfWeek.Monday,
                   DayOfWeek.Tuesday    => DaysOfWeek.Tuesday,
                   DayOfWeek.Wednesday  => DaysOfWeek.Wednesday,
                   DayOfWeek.Thursday   => DaysOfWeek.Thursday,
                   DayOfWeek.Friday     => DaysOfWeek.Friday,
                   DayOfWeek.Saturday   => DaysOfWeek.Saturday,
                   _                    => DaysOfWeek.Sunday
               };

        #endregion


        #region ToWWCP(this ChargeDetailRecord, ChargeDetailRecord2WWCPChargeDetailRecord = null)

        /// <summary>
        /// Convert the given OICP charge detail record into a corresponding WWCP charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecord">An OICP charge detail record.</param>
        /// <param name="ChargeDetailRecord2WWCPChargeDetailRecord">A delegate which allows you to modify the convertion from OICP charge detail records to WWCP charge detail records.</param>
        public static WWCP.ChargeDetailRecord? ToWWCP(this ChargeDetailRecord                             ChargeDetailRecord,
                                                      ChargeDetailRecord2WWCPChargeDetailRecordDelegate?  ChargeDetailRecord2WWCPChargeDetailRecord   = null)
        {

            var internalData = new UserDefinedDictionary(
                                   new Dictionary<String, Object?> {
                                       { OICP_CDR, ChargeDetailRecord }
                                   }
                               );

            if (ChargeDetailRecord.CPOPartnerSessionId.HasValue)
                internalData.Set(OICP_CPOPartnerSessionId,             ChargeDetailRecord.CPOPartnerSessionId. Value.ToString());

            if (ChargeDetailRecord.EMPPartnerSessionId.HasValue)
                internalData.Set(OICP_EMPPartnerSessionId,             ChargeDetailRecord.EMPPartnerSessionId. Value.ToString());

            if (ChargeDetailRecord.MeterValuesInBetween           is not null && ChargeDetailRecord.MeterValuesInBetween.Any())
                internalData.Set(OICP_MeterValuesInBetween,            ChargeDetailRecord.MeterValuesInBetween.ToArray());

            if (ChargeDetailRecord.SignedMeteringValues           is not null && ChargeDetailRecord.SignedMeteringValues.Any())
                internalData.Set(OICP_SignedMeteringValues,            JSONArray.Create(ChargeDetailRecord.SignedMeteringValues.Select(signedMeteringValue => signedMeteringValue.ToJSON())));

            if (ChargeDetailRecord.CalibrationLawVerificationInfo is not null)
                internalData.Set(OICP_CalibrationLawVerificationInfo,  ChargeDetailRecord.CalibrationLawVerificationInfo.ToJSON());

            if (ChargeDetailRecord.HubOperatorId.HasValue)
                internalData.Set(OICP_HubOperatorId,                   ChargeDetailRecord.HubOperatorId.       Value.ToString());

            if (ChargeDetailRecord.HubProviderId.HasValue)
                internalData.Set(OICP_HubProviderId,                   ChargeDetailRecord.HubProviderId.       Value.ToString());


            var sessionId = ChargeDetailRecord.SessionId.ToWWCP();
            if (sessionId is null)
                return null;

            var evseId    = ChargeDetailRecord.EVSEId.ToWWCP();
            if (evseId    is null)
                return null;

            var CDR = new WWCP.ChargeDetailRecord(
                          Id:                     WWCP.ChargeDetailRecord_Id.Parse(ChargeDetailRecord.SessionId.ToWWCP()?.ToString() ?? ""),
                          SessionId:              sessionId.Value,
                          EVSEId:                 evseId.   Value,
                          ProviderIdStart:        ChargeDetailRecord.HubProviderId.HasValue
                                                      ? new WWCP.EMobilityProvider_Id?(WWCP.EMobilityProvider_Id.Parse(ChargeDetailRecord.HubProviderId.ToString() ?? ""))
                                                      : null,

                          ChargingProduct:        ChargeDetailRecord.PartnerProductId.HasValue
                                                      ? WWCP.ChargingProduct.FromId(ChargeDetailRecord.PartnerProductId.Value.ToString())
                                                      : null,

                          SessionTime:            new StartEndDateTime(ChargeDetailRecord.SessionStart,
                                                                       ChargeDetailRecord.SessionEnd),

                          AuthenticationStart:    ChargeDetailRecord.Identification.ToWWCP(),

                          EnergyMeteringValues:   new[] {

                                                      new WWCP.EnergyMeteringValue(
                                                          ChargeDetailRecord.ChargingStart,
                                                          ChargeDetailRecord.MeterValueStart ?? 0
                                                      ),

                                                      //ToDo: Meter values in between... but we don't have timestamps for them!

                                                      new WWCP.EnergyMeteringValue(
                                                          ChargeDetailRecord.ChargingEnd,
                                                          ChargeDetailRecord.MeterValueEnd   ?? ChargeDetailRecord.ConsumedEnergy
                                                      )

                                                  },

                          //ConsumedEnergy:       Will be calculated!

                          //Signatures:             new String[] { ChargeDetailRecord.MeteringSignature },

                          CustomData:             ChargeDetailRecord.CustomData,
                          InternalData:           internalData

                      );

            if (ChargeDetailRecord2WWCPChargeDetailRecord is not null)
                CDR = ChargeDetailRecord2WWCPChargeDetailRecord(ChargeDetailRecord, CDR);

            return CDR;

        }

        #endregion

        #region ToOICP(this ChargeDetailRecord, WWCPChargeDetailRecord2ChargeDetailRecord = null)

        /// <summary>
        /// Convert the given WWCP charge detail record into a corresponding OICP charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecord">A WWCP charge detail record.</param>
        /// <param name="WWCPChargeDetailRecord2ChargeDetailRecord">A delegate which allows you to modify the convertion from WWCP charge detail records to OICP charge detail records.</param>
        public static ChargeDetailRecord? ToOICP(this WWCP.ChargeDetailRecord                        ChargeDetailRecord,
                                                 WWCPChargeDetailRecord2ChargeDetailRecordDelegate?  WWCPChargeDetailRecord2ChargeDetailRecord = null)
        {

            var evseId         = ChargeDetailRecord.EVSEId.   ToOICP();
            if (evseId         is null)
                return null;

            var sessionId      = ChargeDetailRecord.SessionId.ToOICP();
            if (sessionId      is null)
                return null;

            var sessionEndTime = ChargeDetailRecord.SessionTime.EndTime;
            if (sessionEndTime is null)
                return null;


            var CDR = new ChargeDetailRecord(
                          EVSEId:                evseId.        Value,
                          SessionId:             sessionId.     Value,
                          SessionStart:          ChargeDetailRecord.SessionTime.StartTime,
                          SessionEnd:            sessionEndTime.Value,
                          Identification:        ChargeDetailRecord.AuthenticationStart.ToOICP(),
                          PartnerProductId:      ChargeDetailRecord.ChargingProduct?.Id.ToOICP(),
                          CPOPartnerSessionId:   ChargeDetailRecord.GetInternalDataAs<CPOPartnerSession_Id?>(OICP_CPOPartnerSessionId),
                          EMPPartnerSessionId:   ChargeDetailRecord.GetInternalDataAs<EMPPartnerSession_Id?>(OICP_EMPPartnerSessionId),
                          ChargingStart:         ChargeDetailRecord.EnergyMeteringValues?.Any() == true ? ChargeDetailRecord.EnergyMeteringValues.First().Timestamp : ChargeDetailRecord.SessionTime.StartTime,
                          ChargingEnd:           ChargeDetailRecord.EnergyMeteringValues?.Any() == true ? ChargeDetailRecord.EnergyMeteringValues.Last(). Timestamp : sessionEndTime.Value,
                          MeterValueStart:       ChargeDetailRecord.EnergyMeteringValues?.Any() == true ? ChargeDetailRecord.EnergyMeteringValues.First().Value     : null,
                          MeterValueEnd:         ChargeDetailRecord.EnergyMeteringValues?.Any() == true ? ChargeDetailRecord.EnergyMeteringValues.Last(). Value     : null,
                          MeterValuesInBetween:  ChargeDetailRecord.EnergyMeteringValues?.Any() == true ? ChargeDetailRecord.EnergyMeteringValues.Select(energyMeteringValue => energyMeteringValue.Value) : null,
                          ConsumedEnergy:        ChargeDetailRecord.ConsumedEnergy ?? 0,
                          //MeteringSignature:     ChargeDetailRecord.Signatures.FirstOrDefault(),
                          HubOperatorId:         ChargeDetailRecord.GetInternalDataAs<Operator_Id?>(OICP_HubOperatorId),
                          HubProviderId:         ChargeDetailRecord.GetInternalDataAs<Provider_Id?>(OICP_HubProviderId),
                          InternalData:          new UserDefinedDictionary(
                                                     new Dictionary<String, Object?>() {
                                                         { WWCP_CDR, ChargeDetailRecord }
                                                     }
                                                 )
                      );

            if (WWCPChargeDetailRecord2ChargeDetailRecord is not null)
                CDR = WWCPChargeDetailRecord2ChargeDetailRecord(ChargeDetailRecord, CDR);

            return CDR;

        }

        #endregion

    }

}
