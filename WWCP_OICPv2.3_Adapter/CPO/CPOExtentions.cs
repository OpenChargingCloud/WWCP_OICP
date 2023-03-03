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

using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extensions methods for the WWCP wrapper for OICP roaming clients for charging station operators.
    /// </summary>
    public static class CPOExtensions
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
        /// <param name="EVSE2EVSEDataRecord">A delegate to process an EVSE data record, e.g. before pushing it to the roaming provider.</param>
        /// 
        /// <param name="DefaultOperator">An optional Charging Station Operator, which will be copied into the main OperatorID-section of the OICP SOAP request.</param>
        /// <param name="OperatorNameSelector">An optional delegate to select an Charging Station Operator name, which will be copied into the OperatorName-section of the OICP SOAP request.</param>
        /// <param name="IncludeEVSEs">Only include the EVSEs matching the given delegate.</param>
        /// <param name="ServiceCheckEvery">The service check intervall.</param>
        /// <param name="StatusCheckEvery">The status check intervall.</param>
        /// 
        /// <param name="DisablePushData">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisablePushStatus">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableAuthentication">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableSendChargeDetailRecords">This service can be disabled, e.g. for debugging reasons.</param>
        /// 
        /// <param name="OICPConfigurator">An optional delegate to configure the new OICP roaming provider after its creation.</param>
        /// <param name="Configurator">An optional delegate to configure the new roaming provider after its creation.</param>
        public static OICPv2_3.CPO.CPOAdapter?

            CreateOICPv2_3_CPOAdapter(this RoamingNetwork                                          RoamingNetwork,
                                      EMPRoamingProvider_Id                                        Id,
                                      I18NString                                                   Name,
                                      I18NString                                                   Description,
                                      OICPv2_3.CPO.CPORoaming                                      CPORoaming,

                                      OICPv2_3.WWCPEVSEId_2_EVSEId_Delegate?                       CustomEVSEIdConverter                           = null,
                                      OICPv2_3.EVSE2EVSEDataRecordDelegate?                        EVSE2EVSEDataRecord                             = null,
                                      OICPv2_3.EVSEStatusUpdate2EVSEStatusRecordDelegate?          EVSEStatusUpdate2EVSEStatusRecord               = null,
                                      OICPv2_3.WWCPChargeDetailRecord2ChargeDetailRecordDelegate?  WWCPChargeDetailRecord2OICPChargeDetailRecord   = null,

                                      IChargingStationOperator?                                    DefaultOperator                                 = null,
                                      OperatorIdFormats                                            DefaultOperatorIdFormat                         = OperatorIdFormats.ISO_STAR,
                                      ChargingStationOperatorNameSelectorDelegate?                 OperatorNameSelector                            = null,

                                      IncludeEVSEIdDelegate?                                       IncludeEVSEIds                                  = null,
                                      IncludeEVSEDelegate?                                         IncludeEVSEs                                    = null,
                                      ChargeDetailRecordFilterDelegate?                            ChargeDetailRecordFilter                        = null,

                                      TimeSpan?                                                    ServiceCheckEvery                               = null,
                                      TimeSpan?                                                    StatusCheckEvery                                = null,
                                      TimeSpan?                                                    CDRCheckEvery                                   = null,

                                      Boolean                                                      DisablePushData                                 = false,
                                      Boolean                                                      DisablePushAdminStatus                          = true,
                                      Boolean                                                      DisablePushStatus                               = false,
                                      Boolean                                                      DisableAuthentication                           = false,
                                      Boolean                                                      DisableSendChargeDetailRecords                  = false,

                                      Action<OICPv2_3.CPO.CPOAdapter>?                             OICPConfigurator                                = null,
                                      Action<IEMPRoamingProvider>?                                 Configurator                                    = null,

                                      String                                                       EllipticCurve                                   = "P-256",
                                      ECPrivateKeyParameters?                                      PrivateKey                                      = null,
                                      PublicKeyCertificates?                                       PublicKeyCertificates                           = null)

        {

            #region Initial checks

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Name), "The given roaming provider name must not be null or empty!");

            #endregion

            var newRoamingProvider = new OICPv2_3.CPO.CPOAdapter(

                                         Id,
                                         Name,
                                         Description,
                                         RoamingNetwork,
                                         CPORoaming,

                                         CustomEVSEIdConverter,
                                         EVSE2EVSEDataRecord,
                                         EVSEStatusUpdate2EVSEStatusRecord,
                                         WWCPChargeDetailRecord2OICPChargeDetailRecord,

                                         DefaultOperator,
                                         DefaultOperatorIdFormat,
                                         OperatorNameSelector,

                                         IncludeEVSEIds,
                                         IncludeEVSEs,
                                         ChargeDetailRecordFilter,

                                         ServiceCheckEvery,
                                         StatusCheckEvery,
                                         CDRCheckEvery,

                                         DisablePushData,
                                         DisablePushAdminStatus,
                                         DisablePushStatus,
                                         DisableAuthentication,
                                         DisableSendChargeDetailRecords,

                                         EllipticCurve,
                                         PrivateKey,
                                         PublicKeyCertificates

                                     );

            OICPConfigurator?.Invoke(newRoamingProvider);

            return RoamingNetwork.
                       CreateEMPRoamingProvider(newRoamingProvider,
                                                Configurator) as OICPv2_3.CPO.CPOAdapter;

        }

    }

}
