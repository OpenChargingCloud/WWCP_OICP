/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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
using WWCP = org.GraphDefined.WWCP;
using Newtonsoft.Json.Linq;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3
{

    /// <summary>
    /// A delegate which allows you to modify the convertion from WWCP EVSEs to EVSE data records.
    /// </summary>
    /// <param name="EVSE">A WWCP EVSE.</param>
    /// <param name="EVSEDataRecord">An EVSE data record.</param>
    public delegate EVSEDataRecord      EVSE2EVSEDataRecordDelegate                      (WWCP.EVSE                EVSE,
                                                                                          EVSEDataRecord           EVSEDataRecord);

    /// <summary>
    /// A delegate which allows you to modify the convertion from WWCP EVSE status updates to EVSE status records.
    /// </summary>
    /// <param name="EVSEStatusUpdate">A WWCP EVSE status update.</param>
    /// <param name="EVSEStatusRecord">An OICP EVSE status record.</param>
    public delegate EVSEStatusRecord    EVSEStatusUpdate2EVSEStatusRecordDelegate        (WWCP.EVSEStatusUpdate    EVSEStatusUpdate,
                                                                                          EVSEStatusRecord         EVSEStatusRecord);


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
    /// A delegate which allows you to modify the convertion from WWCP charge detail records to OICP charge detail records.
    /// </summary>
    /// <param name="WWCPChargeDetailRecord">A WWCP charge detail record.</param>
    /// <param name="OCIPChargeDetailRecord">An OICP charge detail record.</param>
    public delegate ChargeDetailRecord  WWCPChargeDetailRecord2ChargeDetailRecordDelegate(WWCP.ChargeDetailRecord  WWCPChargeDetailRecord,
                                                                                          ChargeDetailRecord       OCIPChargeDetailRecord);

    /// <summary>
    /// A delegate which allows you to modify the convertion from OICP charge detail records to WWCP charge detail records.
    /// </summary>
    /// <param name="OCIPChargeDetailRecord">An OICP charge detail record.</param>
    /// <param name="WWCPChargeDetailRecord">A WWCP charge detail record.</param>
    public delegate WWCP.ChargeDetailRecord  ChargeDetailRecord2WWCPChargeDetailRecordDelegate(ChargeDetailRecord       OICPChargeDetailRecord,
                                                                                               WWCP.ChargeDetailRecord  WWCPChargeDetailRecord);

    /// <summary>
    /// Helper methods to map OICP data type values to
    /// WWCP data type values and vice versa.
    /// </summary>
    public static class OICPMapper
    {

        public const String WWCP_CDR                  = "WWCP.CDR";
        public const String OICP_CDR                  = "OICP.CDR";
        public const String OICP_CPOPartnerSessionId  = "OICP.CPOPartnerSessionId";
        public const String OICP_EMPPartnerSessionId  = "OICP.EMPPartnerSessionId";
        public const String OICP_HubOperatorId        = "OICP.HubOperatorId";
        public const String OICP_HubProviderId        = "OICP.HubProviderId";


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


        #region ToWWCP(this EVSEDataRecord, ..., EVSEDataRecord2EVSE = null)

        /// <summary>
        /// Convert an OICP EVSE data record into a corresponding WWCP EVSE.
        /// </summary>
        /// <param name="EVSEDataRecord">An EVSE data record.</param>
        /// <param name="EVSEDataRecord2EVSE">A delegate to process an EVSE data record, e.g. before importing it into a roaming network.</param>
        public static WWCP.EVSE ToWWCP(this EVSEDataRecord                   EVSEDataRecord,

                                       String                                DataSource                              = "",
                                       WWCP.EVSEAdminStatusTypes             InitialEVSEAdminStatus                  = WWCP.EVSEAdminStatusTypes.OutOfService,
                                       WWCP.ChargingStationAdminStatusTypes  InitialChargingStationAdminStatus       = WWCP.ChargingStationAdminStatusTypes.OutOfService,
                                       WWCP.EVSEStatusTypes                  InitialEVSEStatus                       = WWCP.EVSEStatusTypes.OutOfService,
                                       WWCP.ChargingStationStatusTypes       InitialChargingStationStatus            = WWCP.ChargingStationStatusTypes.OutOfService,
                                       UInt16                                MaxEVSEAdminStatusListSize              = WWCP.EVSE.DefaultMaxAdminStatusListSize,
                                       UInt16                                MaxChargingStationAdminStatusListSize   = WWCP.EVSE.DefaultMaxAdminStatusListSize,
                                       UInt16                                MaxEVSEStatusListSize                   = WWCP.EVSE.DefaultMaxEVSEStatusListSize,
                                       UInt16                                MaxChargingStationStatusListSize        = WWCP.EVSE.DefaultMaxEVSEStatusListSize,

                                       EVSEDataRecord2EVSEDelegate           EVSEDataRecord2EVSE                     = null)

        {

            WWCP.EVSE _EVSE = null;

            try
            {

                var _EVSEId             = EVSEDataRecord.Id.ToWWCP();

                if (!_EVSEId.HasValue)
                    return null;

                var _ChargingStationId  = WWCP.ChargingStation_Id.Create(_EVSEId.Value);

                var internalData = new Dictionary<String, Object>();
                internalData.Add("OICP.EVSEDataRecord", EVSEDataRecord);


                _EVSE = new WWCP.EVSE(_EVSEId.Value,
                                      new WWCP.ChargingStation(_ChargingStationId,
                                                               station => {
                                                                   station.DataSource   = DataSource;
                                                                   //station.Address      = EVSEDataRecord.Address.ToWWCP();
                                                                   station.GeoLocation  = new org.GraphDefined.Vanaheimr.Aegir.GeoCoordinate(
                                                                                              org.GraphDefined.Vanaheimr.Aegir.Latitude. Parse(EVSEDataRecord.GeoCoordinates.Latitude),
                                                                                              org.GraphDefined.Vanaheimr.Aegir.Longitude.Parse(EVSEDataRecord.GeoCoordinates.Longitude)
                                                                                          );
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
                                      EVSEDataRecord.CustomData,
                                      internalData);


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
        public static EVSEDataRecord ToOICP(this WWCP.EVSE               EVSE,
                                            EVSE2EVSEDataRecordDelegate  EVSE2EVSEDataRecord   = null,
                                            DeltaTypes?                  DeltaType             = null,
                                            DateTime?                    LastUpdate            = null,
                                            String                       OperatorName          = null)
        {

            try
            {

                #region Verifications

                var EVSEId = EVSE.Id.ToOICP();

                if (!EVSEId.HasValue)
                    throw new InvalidEVSEIdentificationException(EVSE.Id.ToString());

                var geoLocation    = (EVSE.ChargingStation.GeoLocation ?? EVSE.ChargingPool.GeoLocation).ToOICP();
                if (!geoLocation.HasValue)
                    throw new ArgumentNullException("GeoCoordinates",  "Within OICP v2.3 the geo coordinates of an EVSE are mandatory!");

                var accessibility  = EVSE.ChargingStation.Accessibility.ToOICP();
                if (!accessibility.HasValue)
                    throw new ArgumentNullException("Accessibility",   "Within OICP v2.3 the accessibility of an EVSE is mandatory!");

                #endregion

                #region Copy custom data and add WWCP EVSE as "WWCP.EVSE"...

                var internalData = new Dictionary<String, Object>();
                EVSE.InternalData.ForEach(kvp => internalData.Add(kvp.Key, kvp.Value));

                if (!internalData.ContainsKey("WWCP.EVSE"))
                    internalData.Add("WWCP.EVSE", EVSE);
                else
                    internalData["WWCP.EVSE"] = EVSE;

                #endregion


                var evseDataRecord = new EVSEDataRecord(Id:                                EVSEId.Value,
                                                        OperatorId:                        EVSEId.Value.OperatorId,
                                                        OperatorName:                      OperatorName,
                                                        ChargingStationName:               EVSE.ChargingStation.Name.   ToOICP(),
                                                        Address:                           EVSE.ChargingStation.Address.ToOICP(),
                                                        GeoCoordinates:                    geoLocation.Value,
                                                        PlugTypes:                         EVSE.SocketOutlets.SafeSelect(socketoutlet => socketoutlet.Plug.ToOICP()),
                                                        ChargingFacilities:                EVSE.AsChargingFacilities(),
                                                        RenewableEnergy:                   false,
                                                        CalibrationLawDataAvailability:    CalibrationLawDataAvailabilities.NotAvailable,
                                                        AuthenticationModes:               EVSE.ChargingStation.AuthenticationModes.ToOICP(),
                                                        PaymentOptions:                    EVSE.ChargingStation.PaymentOptions.SafeSelect(paymentOption => paymentOption.ToOICP()),
                                                        ValueAddedServices:                new ValueAddedServices[] { ValueAddedServices.None },
                                                        Accessibility:                     accessibility.Value,
                                                        HotlinePhoneNumber:                Phone_Number.Parse(EVSE.ChargingStation.HotlinePhoneNumber.FirstText()),
                                                        IsOpen24Hours:                     EVSE.ChargingStation.OpeningTimes != null ? EVSE.ChargingStation.OpeningTimes.IsOpen24Hours : true,
                                                        IsHubjectCompatible:               EVSE.ChargingStation.IsHubjectCompatible,
                                                        DynamicInfoAvailable:              EVSE.ChargingStation.DynamicInfoAvailable ? FalseTrueAuto.True : FalseTrueAuto.False,

                                                        DeltaType:                         DeltaType,
                                                        LastUpdate:                        LastUpdate,

                                                        ChargingStationId:                 ChargingStation_Id.Parse(EVSE.ChargingStation.Id.ToString()),
                                                        ChargingPoolId:                    ChargingPool_Id.   Parse(EVSE.ChargingPool.   Id.ToString()),
                                                        HardwareManufacturer:              null,
                                                        ChargingStationImageURL:           null,
                                                        SubOperatorName:                   EVSE.ChargingStation.ChargingPool.SubOperator?.Name?.FirstText(),
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
                                                        GeoChargingPointEntrance:          EVSE.ChargingStation.ChargingPool.EntranceLocation.ToOICP(),
                                                        OpeningTimes:                      null,//EVSE.ChargingStation.OpeningTimes?.ToOICP(),
                                                        HubOperatorId:                     EVSE.GetInternalDataAs<Operator_Id?>     ("OICP.HubOperatorId"),
                                                        ClearingHouseId:                   EVSE.GetInternalDataAs<ClearingHouse_Id?>("OICP.ClearingHouseId"),

                                                        CustomData:                        EVSE.CustomData,
                                                        InternalData:                      internalData);

                return EVSE2EVSEDataRecord != null
                           ? EVSE2EVSEDataRecord(EVSE, evseDataRecord)
                           : evseDataRecord;

            }
            catch (Exception e)
            {
                throw new EVSEToOICPException(EVSE, e);
            }

        }

        #endregion


        public static I18NText ToOICP(this I18NString MLText)
            => new I18NText(MLText.Select(text => new KeyValuePair<LanguageCode, String>(LanguageCode.Parse(text.Language.ToString()),
                                                                                                            text.Text)));


        #region ToWWCP(this EVSEStatusTypes)

        /// <summary>
        /// Convert an OICP EVSE status into a corresponding WWCP EVSE status.
        /// </summary>
        /// <param name="EVSEStatusTypes">An OICP EVSE status.</param>
        /// <returns>The corresponding WWCP EVSE status.</returns>
        public static WWCP.EVSEStatusTypes ToWWCP(this EVSEStatusTypes EVSEStatusType)

            => EVSEStatusType switch {
                   EVSEStatusTypes.Available     => WWCP.EVSEStatusTypes.Available,
                   EVSEStatusTypes.Reserved      => WWCP.EVSEStatusTypes.Reserved,
                   EVSEStatusTypes.Occupied      => WWCP.EVSEStatusTypes.Charging,
                   EVSEStatusTypes.OutOfService  => WWCP.EVSEStatusTypes.OutOfService,
                   EVSEStatusTypes.EVSENotFound  => WWCP.EVSEStatusTypes.UnknownEVSE,
                   _                             => WWCP.EVSEStatusTypes.OutOfService,
               };


        /// <summary>
        /// Convert an OICP EVSE status into a corresponding WWCP EVSE status.
        /// </summary>
        /// <param name="EVSEStatusTypes">An OICP EVSE status.</param>
        /// <returns>The corresponding WWCP EVSE status.</returns>
        public static WWCP.EVSEStatusTypes? ToWWCP(this EVSEStatusTypes? EVSEStatusType)

            => EVSEStatusType.HasValue
                   ? EVSEStatusType.Value.ToWWCP()
                   : new WWCP.EVSEStatusTypes?();

        #endregion

        #region ToOICP(this EVSEStatusType)

        /// <summary>
        /// Convert a WWCP EVSE status into a corresponding OICP EVSE status.
        /// </summary>
        /// <param name="EVSEStatusType">An WWCP EVSE status.</param>
        /// <returns>The corresponding OICP EVSE status.</returns>
        public static EVSEStatusTypes ToOICP(this WWCP.EVSEStatusTypes EVSEStatusType)

            => EVSEStatusType switch {
                   WWCP.EVSEStatusTypes.Available     => EVSEStatusTypes.Available,
                   WWCP.EVSEStatusTypes.Reserved      => EVSEStatusTypes.Reserved,
                   WWCP.EVSEStatusTypes.Charging      => EVSEStatusTypes.Occupied,
                   WWCP.EVSEStatusTypes.OutOfService  => EVSEStatusTypes.OutOfService,
                   WWCP.EVSEStatusTypes.UnknownEVSE   => EVSEStatusTypes.EVSENotFound,
                   _                                  => EVSEStatusTypes.OutOfService,
               };


        /// <summary>
        /// Convert a WWCP EVSE status into a corresponding OICP EVSE status.
        /// </summary>
        /// <param name="EVSEStatusType">An WWCP EVSE status.</param>
        /// <returns>The corresponding OICP EVSE status.</returns>
        public static EVSEStatusTypes? ToOICP(this WWCP.EVSEStatusTypes? EVSEStatusType)

            => EVSEStatusType.HasValue
                   ? EVSEStatusType.Value.ToOICP()
                   : new EVSEStatusTypes?();

        #endregion

        #region ToOICP(this EVSEStatus)

        /// <summary>
        /// Create a new OICP EVSE status record based on the given WWCP EVSE status.
        /// </summary>
        /// <param name="EVSEStatus">The current status of an EVSE.</param>
        public static EVSEStatusRecord ToOICP(this WWCP.EVSEStatus EVSEStatus)

        {

            if (EVSEStatus == null)
                throw new ArgumentNullException(nameof(EVSEStatus), "The given EVSE status must not be null!");

            return new EVSEStatusRecord(EVSEStatus.Id.          ToOICP().Value,
                                        EVSEStatus.Status.Value.ToOICP());

        }

        #endregion


        #region ToOICP(this WWCPAddress)

        /// <summary>
        /// Maps a WWCP address to an OICP address.
        /// </summary>
        /// <param name="WWCPAddress">A WWCP address.</param>
        public static Address ToOICP(this org.GraphDefined.Vanaheimr.Illias.Address WWCPAddress)

            => new Address(WWCPAddress.Country,
                           WWCPAddress.City.FirstText(),
                           WWCPAddress.Street,
                           WWCPAddress.PostalCode,
                           WWCPAddress.HouseNumber,
                           WWCPAddress.FloorLevel,
                           null,
                           null,
                           null,
                           Time_Zone.Parse(WWCPAddress.Timezone.ToString()));

        #endregion

        #region ToWWCP(this OICPAddress)

        /// <summary>
        /// Maps an OICP address type to a WWCP address type.
        /// </summary>
        /// <param name="OICPAddress">A address type.</param>
        public static org.GraphDefined.Vanaheimr.Illias.Address ToWWCP(this Address OICPAddress)

            => new org.GraphDefined.Vanaheimr.Illias.Address(OICPAddress.Street,
                                                             OICPAddress.HouseNumber,
                                                             OICPAddress.Floor,
                                                             OICPAddress.PostalCode,
                                                             "",
                                                             I18NString.Create(Languages.de, OICPAddress.City),
                                                             OICPAddress.Country);

        #endregion


        #region ToOICP(this GeoLocation)

        public static GeoCoordinates ToOICP(this org.GraphDefined.Vanaheimr.Aegir.GeoCoordinate GeoLocation)

            => new GeoCoordinates(GeoLocation.Latitude. Value,
                                  GeoLocation.Longitude.Value);

        public static GeoCoordinates? ToOICP(this org.GraphDefined.Vanaheimr.Aegir.GeoCoordinate? GeoLocation)

            => GeoLocation.HasValue
                   ? GeoLocation.Value.ToOICP()
                   : new GeoCoordinates?();

        #endregion

        #region ToOICP(this GeoLocation)

        public static org.GraphDefined.Vanaheimr.Aegir.GeoCoordinate ToWWCP(this GeoCoordinates GeoLocation)

            => new org.GraphDefined.Vanaheimr.Aegir.GeoCoordinate(org.GraphDefined.Vanaheimr.Aegir.Latitude. Parse(GeoLocation.Latitude),
                                                                  org.GraphDefined.Vanaheimr.Aegir.Longitude.Parse(GeoLocation.Longitude));

        public static org.GraphDefined.Vanaheimr.Aegir.GeoCoordinate? ToWWCP(this GeoCoordinates? GeoLocation)

            => GeoLocation.HasValue
                   ? GeoLocation.Value.ToWWCP()
                   : new org.GraphDefined.Vanaheimr.Aegir.GeoCoordinate?();

        #endregion



        #region ToOICP(this OperatorId, Format = WWCP.OperatorIdFormats.ISO_STAR)

        public static Operator_Id? ToOICP(this WWCP.ChargingStationOperator_Id  OperatorId,
                                          WWCP.OperatorIdFormats                Format = WWCP.OperatorIdFormats.ISO_STAR)

            => Operator_Id.TryParse(OperatorId.ToString(Format));

        public static Operator_Id? ToOICP(this WWCP.ChargingStationOperator_Id? OperatorId)

            => OperatorId.HasValue
                   ? OperatorId.Value.ToOICP()
                   : new Operator_Id?();

        #endregion

        #region ToWWCP(this OperatorId)

        public static WWCP.ChargingStationOperator_Id? ToWWCP(this Operator_Id OperatorId)

            => WWCP.ChargingStationOperator_Id.TryParse(OperatorId.ToString());

        public static WWCP.ChargingStationOperator_Id? ToWWCP(this Operator_Id? OperatorId)

            => OperatorId.HasValue
                   ? OperatorId.Value.ToWWCP()
                   : new WWCP.ChargingStationOperator_Id?();

        #endregion


        #region ToOICP(this ProviderId)

        public static Provider_Id? ToOICP(this WWCP.eMobilityProvider_Id ProviderId)

            => Provider_Id.TryParse(ProviderId.ToString());

        public static Provider_Id? ToOICP(this WWCP.eMobilityProvider_Id? ProviderId)

            => ProviderId.HasValue
                   ? ProviderId.Value.ToOICP()
                   : new Provider_Id?();

        #endregion

        #region ToWWCP(this ProviderId)

        public static WWCP.eMobilityProvider_Id? ToWWCP(this Provider_Id ProviderId)

            => WWCP.eMobilityProvider_Id.TryParse(ProviderId.ToString());

        public static WWCP.eMobilityProvider_Id? ToWWCP(this Provider_Id? ProviderId)

            => ProviderId.HasValue
                   ? ProviderId.Value.ToWWCP()
                   : new WWCP.eMobilityProvider_Id?();

        #endregion


        #region ToOICP(this EVSEId)

        public static EVSE_Id? ToOICP(this WWCP.EVSE_Id EVSEId)

            => EVSE_Id.TryParse(EVSEId.ToString());


        public static EVSE_Id? ToOICP(this WWCP.EVSE_Id? EVSEId)

            => EVSEId.HasValue
                   ? EVSEId.Value.ToOICP()
                   : new EVSE_Id?();

        #endregion

        #region ToWWCP(this EVSEId)

        public static WWCP.EVSE_Id? ToWWCP(this EVSE_Id EVSEId)

            => WWCP.EVSE_Id.TryParse(EVSEId.ToString(),
                                     WWCP.EVSEIdParsingMode.relaxed);

        public static WWCP.EVSE_Id? ToWWCP(this EVSE_Id? EVSEId)

            => EVSEId.HasValue
                   ? EVSEId.Value.ToWWCP()
                   : new WWCP.EVSE_Id?();

        #endregion


        #region ToOICP(this ChargingStationId)

        public static ChargingStation_Id? ToOICP(this WWCP.ChargingStation_Id ChargingStationId)

            => ChargingStation_Id.TryParse(ChargingStationId.ToString());


        public static ChargingStation_Id? ToOICP(this WWCP.ChargingStation_Id? ChargingStationId)

            => ChargingStationId.HasValue
                   ? ChargingStationId.Value.ToOICP()
                   : new ChargingStation_Id?();

        #endregion

        #region ToWWCP(this ChargingStationId)

        public static WWCP.ChargingStation_Id? ToWWCP(this ChargingStation_Id ChargingStationId)

            => WWCP.ChargingStation_Id.TryParse(ChargingStationId.ToString());

        public static WWCP.ChargingStation_Id? ToWWCP(this ChargingStation_Id? ChargingStationId)

            => ChargingStationId.HasValue
                   ? ChargingStationId.Value.ToWWCP()
                   : new WWCP.ChargingStation_Id?();

        #endregion


        #region ToOICP(this ChargingPoolId)

        public static ChargingPool_Id? ToOICP(this WWCP.ChargingPool_Id ChargingPoolId)

            => ChargingPool_Id.TryParse(ChargingPoolId.ToString());


        public static ChargingPool_Id? ToOICP(this WWCP.ChargingPool_Id? ChargingPoolId)

            => ChargingPoolId.HasValue
                   ? ChargingPoolId.Value.ToOICP()
                   : new ChargingPool_Id?();

        #endregion

        #region ToWWCP(this ChargingPoolId)

        public static WWCP.ChargingPool_Id? ToWWCP(this ChargingPool_Id ChargingPoolId)

            => WWCP.ChargingPool_Id.TryParse(ChargingPoolId.ToString());

        public static WWCP.ChargingPool_Id? ToWWCP(this ChargingPool_Id? ChargingPoolId)

            => ChargingPoolId.HasValue
                   ? ChargingPoolId.Value.ToWWCP()
                   : new WWCP.ChargingPool_Id?();

        #endregion


        #region ToOICP(this SessionId)

        public static Session_Id? ToOICP(this WWCP.ChargingSession_Id SessionId)

            => Session_Id.TryParse(SessionId.ToString());

        public static Session_Id? ToOICP(this WWCP.ChargingSession_Id? SessionId)

            => SessionId.HasValue
                   ? SessionId.Value.ToOICP()
                   : new Session_Id?();

        #endregion

        #region ToWWCP(this SessionId)

        public static WWCP.ChargingSession_Id? ToWWCP(this Session_Id SessionId)

            => WWCP.ChargingSession_Id.TryParse(SessionId.ToString());

        public static WWCP.ChargingSession_Id? ToWWCP(this Session_Id? SessionId)

            => SessionId.HasValue
                   ? SessionId.Value.ToWWCP()
                   : new WWCP.ChargingSession_Id?();

        #endregion


        #region ToWWCP(this EMPPartnerSessionId)

        public static WWCP.ChargingSession_Id? ToWWCP(this EMPPartnerSession_Id EMPPartnerSessionId)

            => WWCP.ChargingSession_Id.TryParse(EMPPartnerSessionId.ToString());

        public static WWCP.ChargingSession_Id? ToWWCP(this EMPPartnerSession_Id? EMPPartnerSessionId)

            => EMPPartnerSessionId.HasValue
                   ? EMPPartnerSessionId.Value.ToWWCP()
                   : new WWCP.ChargingSession_Id?();

        #endregion

        #region ToWWCP(this CPOPartnerSessionId)

        public static WWCP.ChargingSession_Id? ToWWCP(this CPOPartnerSession_Id CPOPartnerSessionId)

            => WWCP.ChargingSession_Id.TryParse(CPOPartnerSessionId.ToString());

        public static WWCP.ChargingSession_Id? ToWWCP(this CPOPartnerSession_Id? CPOPartnerSessionId)

            => CPOPartnerSessionId.HasValue
                   ? CPOPartnerSessionId.Value.ToWWCP()
                   : new WWCP.ChargingSession_Id?();

        #endregion


        #region ToOICP(this ChargingReservationId)

        public static Session_Id? ToOICP(this WWCP.ChargingReservation_Id ChargingReservationId)

            => Session_Id.Parse(ChargingReservationId.Suffix);

        public static Session_Id? ToOICP(this WWCP.ChargingReservation_Id? ChargingReservationId)

            => ChargingReservationId.HasValue
                   ? ChargingReservationId.Value.ToOICP()
                   : new Session_Id?();

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
                   : new PartnerProduct_Id?();

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
                   : new WWCP.ChargingProduct_Id?();

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
                   ? EVCO_Id.Parse(eMAId.ToString())
                   : new EVCO_Id?();

        public static WWCP.eMobilityAccount_Id? ToWWCP(this EVCO_Id? EVCOId)
            => EVCOId.HasValue
                   ? WWCP.eMobilityAccount_Id.Parse(EVCOId.ToString())
                   : new WWCP.eMobilityAccount_Id?();






        public static UID ToOICP(this WWCP.Auth_Token AuthToken)

            => UID.Parse(AuthToken.ToString());

        public static UID? ToOICP(this WWCP.Auth_Token? AuthToken)

            => AuthToken.HasValue
                   ? UID.Parse(AuthToken.ToString())
                   : new UID?();


        public static WWCP.Auth_Token ToWWCP(this UID UID)

            => WWCP.Auth_Token.Parse(UID.ToString());




        #region ToWWCP(this Identification)

        public static WWCP.RemoteAuthentication ToWWCP(this Identification Identification)
        {

            if (Identification.RFIDId.HasValue)
                return WWCP.RemoteAuthentication.FromAuthToken(WWCP.Auth_Token.Parse (Identification.RFIDId.ToString()));

            if (Identification.RFIDIdentification != null)
                return WWCP.RemoteAuthentication.FromAuthToken(WWCP.Auth_Token.Parse (Identification.RFIDIdentification.         UID.   ToString()));

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

        public static Identification ToOICP(this WWCP.AAuthentication Authentication)
        {

            if (Authentication.AuthToken.                  HasValue)
                return Identification.FromUID                        (UID.Parse(Authentication.AuthToken.ToString()));

            if (Authentication.QRCodeIdentification.       HasValue)
                return Identification.FromQRCodeIdentification       (          Authentication.QRCodeIdentification.Value.eMAId.ToOICP(),
                                                                      PIN.Parse(Authentication.QRCodeIdentification.Value.PIN));

            if (Authentication.PlugAndChargeIdentification.HasValue)
                return Identification.FromPlugAndChargeIdentification(Authentication.PlugAndChargeIdentification.Value.ToOICP());

            if (Authentication.RemoteIdentification.       HasValue)
                return Identification.FromRemoteIdentification       (Authentication.RemoteIdentification.       Value.ToOICP());

            throw new ArgumentException("Invalid AuthInfo!", nameof(Authentication));

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


        #region ToWWCP(PaymentOption)

        public static WWCP.PaymentOptions ToWWCP(this PaymentOptions PaymentOption)

            => PaymentOption switch {
                   PaymentOptions.NoPayment  => WWCP.PaymentOptions.Free,
                   PaymentOptions.Direct     => WWCP.PaymentOptions.Direct,
                   PaymentOptions.Contract   => WWCP.PaymentOptions.Contract,
                   _                         => throw new ArgumentException("Invalid payment option!")
               };

        #endregion

        #region ToOICP(PaymentOption)

        public static PaymentOptions ToOICP(this WWCP.PaymentOptions PaymentOption)

            => PaymentOption switch {
                   WWCP.PaymentOptions.Free      => PaymentOptions.NoPayment,
                   WWCP.PaymentOptions.Direct    => PaymentOptions.Direct,
                   WWCP.PaymentOptions.Contract  => PaymentOptions.Contract,
                   _                             => throw new ArgumentException("Invalid payment option!")
               };

        #endregion


        #region ToOICP(AuthenticationMode)

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

        #region ToWWCP(AuthenticationMode)

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


        #region ToWWCP(ChargingMode)

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

        #region ToOICP(ChargingMode)

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


        #region ToWWCP(PlugType)

        public static WWCP.PlugTypes ToWWCP(this PlugTypes PlugType)

            => PlugType switch {
                   PlugTypes.SmallPaddleInductive          => WWCP.PlugTypes.SmallPaddleInductive,
                   PlugTypes.LargePaddleInductive          => WWCP.PlugTypes.LargePaddleInductive,
                   PlugTypes.AVCONConnector                => WWCP.PlugTypes.AVCONConnector,
                   PlugTypes.TeslaConnector                => WWCP.PlugTypes.TeslaConnector,
                   PlugTypes.NEMA5_20                      => WWCP.PlugTypes.NEMA5_20,
                   PlugTypes.TypeEFrenchStandard           => WWCP.PlugTypes.TypeEFrenchStandard,
                   PlugTypes.TypeFSchuko                   => WWCP.PlugTypes.TypeFSchuko,
                   PlugTypes.TypeGBritishStandard          => WWCP.PlugTypes.TypeGBritishStandard,
                   PlugTypes.TypeJSwissStandard            => WWCP.PlugTypes.TypeJSwissStandard,
                   PlugTypes.Type1Connector_CableAttached  => WWCP.PlugTypes.Type1Connector_CableAttached,
                   PlugTypes.Type2Outlet                   => WWCP.PlugTypes.Type2Outlet,
                   PlugTypes.Type2Connector_CableAttached  => WWCP.PlugTypes.Type2Connector_CableAttached,
                   PlugTypes.Type3Outlet                   => WWCP.PlugTypes.Type3Outlet,
                   PlugTypes.IEC60309SinglePhase           => WWCP.PlugTypes.IEC60309SinglePhase,
                   PlugTypes.IEC60309ThreePhase            => WWCP.PlugTypes.IEC60309ThreePhase,
                   PlugTypes.CCSCombo2Plug_CableAttached   => WWCP.PlugTypes.CCSCombo2Plug_CableAttached,
                   PlugTypes.CCSCombo1Plug_CableAttached   => WWCP.PlugTypes.CCSCombo1Plug_CableAttached,
                   PlugTypes.CHAdeMO                       => WWCP.PlugTypes.CHAdeMO,
                   _                                       => throw new ArgumentException("Invalid plug type!")
               };

        #endregion

        #region ToOICP(PlugType)

        public static PlugTypes ToOICP(this WWCP.PlugTypes PlugType)

            => PlugType switch {
                   WWCP.PlugTypes.SmallPaddleInductive          => PlugTypes.SmallPaddleInductive,
                   WWCP.PlugTypes.LargePaddleInductive          => PlugTypes.LargePaddleInductive,
                   WWCP.PlugTypes.AVCONConnector                => PlugTypes.AVCONConnector,
                   WWCP.PlugTypes.TeslaConnector                => PlugTypes.TeslaConnector,
                   WWCP.PlugTypes.NEMA5_20                      => PlugTypes.NEMA5_20,
                   WWCP.PlugTypes.TypeEFrenchStandard           => PlugTypes.TypeEFrenchStandard,
                   WWCP.PlugTypes.TypeFSchuko                   => PlugTypes.TypeFSchuko,
                   WWCP.PlugTypes.TypeGBritishStandard          => PlugTypes.TypeGBritishStandard,
                   WWCP.PlugTypes.TypeJSwissStandard            => PlugTypes.TypeJSwissStandard,
                   WWCP.PlugTypes.Type1Connector_CableAttached  => PlugTypes.Type1Connector_CableAttached,
                   WWCP.PlugTypes.Type2Outlet                   => PlugTypes.Type2Outlet,
                   WWCP.PlugTypes.Type2Connector_CableAttached  => PlugTypes.Type2Connector_CableAttached,
                   WWCP.PlugTypes.Type3Outlet                   => PlugTypes.Type3Outlet,
                   WWCP.PlugTypes.IEC60309SinglePhase           => PlugTypes.IEC60309SinglePhase,
                   WWCP.PlugTypes.IEC60309ThreePhase            => PlugTypes.IEC60309ThreePhase,
                   WWCP.PlugTypes.CCSCombo2Plug_CableAttached   => PlugTypes.CCSCombo2Plug_CableAttached,
                   WWCP.PlugTypes.CCSCombo1Plug_CableAttached   => PlugTypes.CCSCombo1Plug_CableAttached,
                   WWCP.PlugTypes.CHAdeMO                       => PlugTypes.CHAdeMO,
                   _                                            => throw new ArgumentException("Invalid plug type!")
               };

        #endregion


        #region AsChargingFacility  (this EVSE)

        /// <summary>
        /// Maps a WWCP EVSE to an OICP charging facility.
        /// </summary>
        /// <param name="EVSE">An WWCP EVSE.</param>
        public static ChargingFacility AsChargingFacility(this WWCP.EVSE EVSE)
        {

            if (!EVSE.CurrentType.HasValue &&
                !EVSE.MaxPower.   HasValue)
            {
                throw new ArgumentException("AsOICPChargingFacility failed!");
            }


            var powerType = PowerTypes.AC_1_PHASE;

            if (EVSE.CurrentType.HasValue)
            {
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

                    default:
                        throw new ArgumentException("Invalid current type!");

                }
            }

            return new ChargingFacility(powerType,
                                        Convert.ToUInt32(EVSE.MaxPower.Value),
                                        EVSE.AverageVoltage.HasValue ? new UInt32?(Convert.ToUInt32(EVSE.AverageVoltage)) : null,
                                        EVSE.MaxCurrent.    HasValue ? new UInt32?(Convert.ToUInt32(EVSE.MaxCurrent))     : null,
                                        EVSE.ChargingModes.Select(chargingMode => chargingMode.ToOICP()));

        }

        #endregion

        #region AsChargingFacilities(this EVSE)

        /// <summary>
        /// Maps a WWCP EVSE to an enumeration of charging facilities.
        /// </summary>
        /// <param name="EVSE">An WWCP EVSE.</param>
        public static IEnumerable<ChargingFacility> AsChargingFacilities(this WWCP.EVSE EVSE)
        {

            if (!EVSE.CurrentType.HasValue &&
                !EVSE.MaxPower.   HasValue)
            {
                return new ChargingFacility[0];
            }


            var powerType = PowerTypes.AC_1_PHASE;

            if (EVSE.CurrentType.HasValue)
            {
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
            }

            return new ChargingFacility[] {
                       new ChargingFacility(powerType,
                                            Convert.ToUInt32(EVSE.MaxPower.Value),
                                            EVSE.AverageVoltage.HasValue ? new UInt32?(Convert.ToUInt32(EVSE.AverageVoltage)) : null,
                                            EVSE.MaxCurrent.    HasValue ? new UInt32?(Convert.ToUInt32(EVSE.MaxCurrent))     : null,
                                            EVSE.ChargingModes.Select(chargingMode => chargingMode.ToOICP()))
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


        #region ToWWCP(this ChargeDetailRecord, ChargeDetailRecord2WWCPChargeDetailRecord = null)

        /// <summary>
        /// Convert an OICP charge detail record into a corresponding WWCP charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecord">An OICP charge detail record.</param>
        /// <param name="ChargeDetailRecord2WWCPChargeDetailRecord">A delegate which allows you to modify the convertion from OICP charge detail records to WWCP charge detail records.</param>
        public static WWCP.ChargeDetailRecord ToWWCP(this ChargeDetailRecord                            ChargeDetailRecord,
                                                     ChargeDetailRecord2WWCPChargeDetailRecordDelegate  ChargeDetailRecord2WWCPChargeDetailRecord = null)
        {

            var CustomData = new Dictionary<String, Object> {
                                 { OICP_CDR, ChargeDetailRecord }
                             };

            if (ChargeDetailRecord.CPOPartnerSessionId.HasValue)
                CustomData.Add(OICP_CPOPartnerSessionId,  ChargeDetailRecord.CPOPartnerSessionId.ToString());

            if (ChargeDetailRecord.EMPPartnerSessionId.HasValue)
                CustomData.Add(OICP_EMPPartnerSessionId,  ChargeDetailRecord.EMPPartnerSessionId.ToString());

            if (ChargeDetailRecord.HubOperatorId.HasValue)
                CustomData.Add(OICP_HubOperatorId,        ChargeDetailRecord.HubOperatorId.      ToString());

            if (ChargeDetailRecord.HubProviderId.HasValue)
                CustomData.Add(OICP_HubProviderId,        ChargeDetailRecord.HubProviderId.      ToString());


            var CDR = new WWCP.ChargeDetailRecord(
                          Id:                    WWCP.ChargeDetailRecord_Id.Parse(ChargeDetailRecord.SessionId.ToWWCP().ToString()),
                          SessionId:             ChargeDetailRecord.SessionId.ToWWCP().Value,
                          EVSEId:                ChargeDetailRecord.EVSEId.   ToWWCP(),
                          ProviderIdStart:       ChargeDetailRecord.HubProviderId.HasValue ? new WWCP.eMobilityProvider_Id?(WWCP.eMobilityProvider_Id.Parse(ChargeDetailRecord.HubProviderId.ToString())) : null,

                          ChargingProduct:       ChargeDetailRecord.PartnerProductId.HasValue
                                                     ? WWCP.ChargingProduct.FromId(ChargeDetailRecord.PartnerProductId.Value.ToString())
                                                     : null,

                          SessionTime:           new StartEndDateTime(ChargeDetailRecord.SessionStart,
                                                                      ChargeDetailRecord.SessionEnd),

                          AuthenticationStart:   ChargeDetailRecord.Identification.ToWWCP(),

                          EnergyMeteringValues:  new Timestamped<Decimal>[] {

                                                     new Timestamped<Decimal>(
                                                         ChargeDetailRecord.ChargingStart,
                                                         ChargeDetailRecord.MeterValueStart ?? 0
                                                     ),

                                                     //ToDo: Meter values in between... but we don't have timestamps for them!

                                                     new Timestamped<Decimal>(
                                                         ChargeDetailRecord.ChargingEnd,
                                                         ChargeDetailRecord.MeterValueEnd   ?? ChargeDetailRecord.ConsumedEnergy
                                                     )

                                                 },

                          //ConsumedEnergy:      Will be calculated!

                          //Signatures:            new String[] { ChargeDetailRecord.MeteringSignature },

                          CustomData:            CustomData

                      );

            if (ChargeDetailRecord2WWCPChargeDetailRecord != null)
                CDR = ChargeDetailRecord2WWCPChargeDetailRecord(ChargeDetailRecord, CDR);

            return CDR;

        }

        #endregion

        #region ToOICP(this ChargeDetailRecord, WWCPChargeDetailRecord2ChargeDetailRecord = null)

        /// <summary>
        /// Convert a WWCP charge detail record into a corresponding OICP charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecord">A WWCP charge detail record.</param>
        /// <param name="WWCPChargeDetailRecord2ChargeDetailRecord">A delegate which allows you to modify the convertion from WWCP charge detail records to OICP charge detail records.</param>
        public static ChargeDetailRecord ToOICP(this WWCP.ChargeDetailRecord                       ChargeDetailRecord,
                                                WWCPChargeDetailRecord2ChargeDetailRecordDelegate  WWCPChargeDetailRecord2ChargeDetailRecord = null)
        {

            var CDR = new ChargeDetailRecord(
                          EVSEId:                ChargeDetailRecord.EVSEId.Value.ToOICP().Value,
                          SessionId:             ChargeDetailRecord.SessionId.   ToOICP().Value,
                          SessionStart:          ChargeDetailRecord.SessionTime.StartTime,
                          SessionEnd:            ChargeDetailRecord.SessionTime.EndTime.Value,
                          Identification:        ChargeDetailRecord.AuthenticationStart.ToOICP(),
                          PartnerProductId:      ChargeDetailRecord.ChargingProduct?.Id.ToOICP(),
                          CPOPartnerSessionId:   ChargeDetailRecord.GetInternalDataAs<CPOPartnerSession_Id?>(OICP_CPOPartnerSessionId),
                          EMPPartnerSessionId:   ChargeDetailRecord.GetInternalDataAs<EMPPartnerSession_Id?>(OICP_EMPPartnerSessionId),
                          ChargingStart:         ChargeDetailRecord.EnergyMeteringValues?.Any() == true ? ChargeDetailRecord.EnergyMeteringValues.First().Timestamp : ChargeDetailRecord.SessionTime.StartTime,
                          ChargingEnd:           ChargeDetailRecord.EnergyMeteringValues?.Any() == true ? ChargeDetailRecord.EnergyMeteringValues.Last(). Timestamp : ChargeDetailRecord.SessionTime.EndTime.Value,
                          MeterValueStart:       ChargeDetailRecord.EnergyMeteringValues?.Any() == true ? ChargeDetailRecord.EnergyMeteringValues.First().Value     : new Decimal?(),
                          MeterValueEnd:         ChargeDetailRecord.EnergyMeteringValues?.Any() == true ? ChargeDetailRecord.EnergyMeteringValues.Last(). Value     : new Decimal?(),
                          MeterValuesInBetween:  ChargeDetailRecord.EnergyMeteringValues?.Any() == true ? ChargeDetailRecord.EnergyMeteringValues.Select((Timestamped<Decimal> v) => v.Value) : null,
                          ConsumedEnergy:        ChargeDetailRecord.ConsumedEnergy ?? 0,
                          //MeteringSignature:     ChargeDetailRecord.Signatures.FirstOrDefault(),
                          HubOperatorId:         ChargeDetailRecord.GetInternalDataAs<Operator_Id?>(OICP_HubOperatorId),
                          HubProviderId:         ChargeDetailRecord.GetInternalDataAs<Provider_Id?>(OICP_HubProviderId),
                          InternalData:          new Dictionary<String, Object>() {
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
