/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extensions methods for the WWCP wrapper for OICP roaming clients for e-mobility providers/EMPs.
    /// </summary>
    public static class EMPExtensions
    {

        /// <summary>
        /// Create and register a new electric vehicle roaming provider
        /// using the OICP protocol and having the given unique electric
        /// vehicle roaming provider identification.
        /// </summary>
        /// 
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// 
        /// <param name="EVSEDataRecord2EVSE">A delegate to process an EVSE data record after receiving it from the roaming provider.</param>
        /// 
        /// <param name="OICPConfigurator">An optional delegate to configure the new OICP roaming provider after its creation.</param>
        /// <param name="Configurator">An optional delegate to configure the new roaming provider after its creation.</param>
        public static OICPv2_3.EMP.EMPAdapter?

            CreateOICPv2_3_EMPAdapter(this IRoamingNetwork                                     RoamingNetwork,
                                      EMPRoamingProvider_Id                                    Id,
                                      I18NString                                               Name,
                                      I18NString                                               Description,
                                      OICPv2_3.EMP.EMPRoaming                                  EMPRoaming,

                                      OICPv2_3.EMP.EVSEDataRecord2EVSEDelegate?                EVSEDataRecord2EVSE                                 = null,

                                      Boolean                                                  PullEVSEData_IsDisabled                             = false,
                                      TimeSpan?                                                PullEVSEData_InitialDelay                           = null,
                                      TimeSpan?                                                PullEVSEData_Every                                  = null,
                                      UInt32?                                                  PullEVSEData_RequestPageSize                        = null,
                                      TimeSpan?                                                PullEVSEData_RequestTimeout                         = null,

                                      IEnumerable<OICPv2_3.Operator_Id>?                       PullEVSEData_OperatorIdFilter                       = null,
                                      IEnumerable<Country>?                                    PullEVSEData_CountryCodeFilter                      = null,
                                      IEnumerable<OICPv2_3.AccessibilityTypes>?                PullEVSEData_AccessibilityFilter                    = null,
                                      IEnumerable<OICPv2_3.AuthenticationModes>?               PullEVSEData_AuthenticationModeFilter               = null,
                                      IEnumerable<OICPv2_3.CalibrationLawDataAvailabilities>?  PullEVSEData_CalibrationLawDataAvailabilityFilter   = null,
                                      Boolean?                                                 PullEVSEData_RenewableEnergyFilter                  = null,
                                      Boolean?                                                 PullEVSEData_IsHubjectCompatibleFilter              = null,
                                      Boolean?                                                 PullEVSEData_IsOpen24HoursFilter                    = null,

                                      Boolean                                                  PullEVSEData_UpdateRoamingNetwork                   = false,

                                      Boolean                                                  PullEVSEStatus_IsDisabled                           = false,
                                      TimeSpan?                                                PullEVSEStatus_InitialDelay                         = null,
                                      TimeSpan?                                                PullEVSEStatus_Every                                = null,
                                      TimeSpan?                                                PullEVSEStatus_RequestTimeout                       = null,

                                      Boolean                                                  PullEVSEStatus_CalculateEVSEStatusDiffs             = false,
                                      Boolean                                                  PullEVSEStatus_UpdateRoamingNetwork                 = false,

                                      Boolean                                                  GetChargeDetailRecords_IsDisabled                   = false,
                                      TimeSpan?                                                GetChargeDetailRecords_InitialDelay                 = null,
                                      TimeSpan?                                                GetChargeDetailRecords_Every                        = null,
                                      DateTime?                                                GetChargeDetailRecords_LastRunTimestamp             = null,
                                      TimeSpan?                                                GetChargeDetailRecords_RequestTimeout               = null,

                                      IEMobilityProvider?                                      DefaultProvider                                     = null,
                                      EMobilityProvider_Id?                                    DefaultProviderId                                   = null,
                                      GeoCoordinate?                                           DefaultSearchCenter                                 = null,
                                      UInt64?                                                  DefaultDistanceKM                                   = null,

                                      Action<OICPv2_3.EMP.EMPAdapter>?                         OICPConfigurator                                    = null,
                                      Action<IEMPRoamingProvider>?                             Configurator                                        = null)

        {

            #region Initial checks

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Name),            "The given roaming provider name must not be null or empty!");

            if (EMPRoaming is null)
                throw new ArgumentNullException(nameof(EMPRoaming),      "The given EMP Roaming must not be null!");

            #endregion

            var newRoamingProvider = new OICPv2_3.EMP.EMPAdapter(Id,
                                                                 Name,
                                                                 Description,
                                                                 RoamingNetwork,
                                                                 EMPRoaming,

                                                                 EVSEDataRecord2EVSE,

                                                                 PullEVSEData_IsDisabled,
                                                                 PullEVSEData_InitialDelay,
                                                                 PullEVSEData_Every,
                                                                 PullEVSEData_RequestPageSize,
                                                                 PullEVSEData_RequestTimeout,

                                                                 PullEVSEData_OperatorIdFilter,
                                                                 PullEVSEData_CountryCodeFilter,
                                                                 PullEVSEData_AccessibilityFilter,
                                                                 PullEVSEData_AuthenticationModeFilter,
                                                                 PullEVSEData_CalibrationLawDataAvailabilityFilter,
                                                                 PullEVSEData_RenewableEnergyFilter,
                                                                 PullEVSEData_IsHubjectCompatibleFilter,
                                                                 PullEVSEData_IsOpen24HoursFilter,

                                                                 PullEVSEData_UpdateRoamingNetwork,

                                                                 PullEVSEStatus_IsDisabled,
                                                                 PullEVSEStatus_InitialDelay,
                                                                 PullEVSEStatus_Every,
                                                                 PullEVSEStatus_RequestTimeout,

                                                                 PullEVSEStatus_CalculateEVSEStatusDiffs,
                                                                 PullEVSEStatus_UpdateRoamingNetwork,

                                                                 GetChargeDetailRecords_IsDisabled,
                                                                 GetChargeDetailRecords_InitialDelay,
                                                                 GetChargeDetailRecords_Every,
                                                                 GetChargeDetailRecords_LastRunTimestamp,
                                                                 GetChargeDetailRecords_RequestTimeout,

                                                                 DefaultProvider,
                                                                 DefaultProviderId,
                                                                 DefaultSearchCenter,
                                                                 DefaultDistanceKM);


            OICPConfigurator?.Invoke(newRoamingProvider);

            return RoamingNetwork.
                       CreateEMPRoamingProvider(newRoamingProvider,
                                                Configurator) as OICPv2_3.EMP.EMPAdapter;

        }

    }

}
