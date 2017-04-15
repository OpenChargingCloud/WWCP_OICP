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
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// Extentions methods for the WWCP wrapper for OICP roaming clients for charging station operators.
    /// </summary>
    public static class CPOExtentions
    {

        #region CreateOICPv2_1_CPORoamingProvider(this RoamingNetwork, Id, Name, RemoteHostname, ... , Action = null)

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
        /// <param name="RemoteHostname">The hostname of the remote OICP service.</param>
        /// <param name="RemoteTCPPort">An optional TCP port of the remote OICP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCert">The TLS client certificate to use.</param>
        /// <param name="RemoteHTTPVirtualHost">An optional HTTP virtual hostname of the remote OICP service.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="QueryTimeout">An optional timeout for upstream queries.</param>
        /// 
        /// <param name="ServerName"> An optional identification string for the HTTP server.</param>
        /// <param name="ServerTCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="ServerURIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="ServerContentType">An optional HTTP content type to use.</param>
        /// <param name="ServerRegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="ServerAutoStart">Whether to start the server immediately or not.</param>
        /// 
        /// <param name="ClientLoggingContext">An optional context for logging client methods.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// 
        /// <param name="EVSE2EVSEDataRecord">A delegate to process an EVSE data record, e.g. before pushing it to the roaming provider.</param>
        /// <param name="EVSEDataRecord2XML">A delegate to process the XML representation of an EVSE data record, e.g. before pushing it to the roaming provider.</param>
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
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public static ICSORoamingProvider

            CreateOICPv2_1_CPORoamingProvider(this RoamingNetwork                                                 RoamingNetwork,
                                              CSORoamingProvider_Id                                               Id,
                                              I18NString                                                          Name,

                                              String                                                              RemoteHostname,
                                              IPPort                                                              RemoteTCPPort                                   = null,
                                              String                                                              RemoteHTTPVirtualHost                           = null,
                                              RemoteCertificateValidationCallback                                 RemoteCertificateValidator                      = null,
                                              X509Certificate                                                     ClientCert                                      = null,
                                              String                                                              URIPrefix                                       = OICPv2_1.CPO.CPOClient.DefaultURIPrefix,
                                              String                                                              HTTPUserAgent                                   = OICPv2_1.CPO.CPOClient.DefaultHTTPUserAgent,
                                              TimeSpan?                                                           QueryTimeout                                    = null,

                                              String                                                              ServerName                                      = OICPv2_1.CPO.CPOServer.DefaultHTTPServerName,
                                              IPPort                                                              ServerTCPPort                                   = null,
                                              String                                                              ServerURIPrefix                                 = OICPv2_1.CPO.CPOServer.DefaultURIPrefix,
                                              HTTPContentType                                                     ServerContentType                               = null,
                                              Boolean                                                             ServerRegisterHTTPRootService                   = true,
                                              Boolean                                                             ServerAutoStart                                 = false,

                                              String                                                              ClientLoggingContext                            = OICPv2_1.CPO.CPOClient.CPOClientLogger.DefaultContext,
                                              String                                                              ServerLoggingContext                            = OICPv2_1.CPO.CPOServerLogger.DefaultContext,
                                              LogfileCreatorDelegate                                              LogfileCreator                                  = null,

                                              OICPv2_1.CPO.EVSE2EVSEDataRecordDelegate                            EVSE2EVSEDataRecord                             = null,
                                              OICPv2_1.CPO.EVSEStatusUpdate2EVSEStatusRecordDelegate              EVSEStatusUpdate2EVSEStatusRecord               = null,
                                              OICPv2_1.CPO.WWCPChargeDetailRecord2OICPChargeDetailRecordDelegate  WWCPCDR2OICPCDR   = null,
                                              OICPv2_1.CPO.EVSEDataRecord2XMLDelegate                             EVSEDataRecord2XML                              = null,
                                              OICPv2_1.CPO.EVSEStatusRecord2XMLDelegate                           EVSEStatusRecord2XML                            = null,
                                              OICPv2_1.CPO.ChargeDetailRecord2XMLDelegate                         ChargeDetailRecord2XML                          = null,

                                              ChargingStationOperator                                             DefaultOperator                                 = null,
                                              OperatorIdFormats                                                   DefaultOperatorIdFormat                         = OperatorIdFormats.ISO_STAR,
                                              ChargingStationOperatorNameSelectorDelegate                         OperatorNameSelector                            = null,
                                              IncludeEVSEDelegate                                                 IncludeEVSEs                                    = null,
                                              TimeSpan?                                                           ServiceCheckEvery                               = null,
                                              TimeSpan?                                                           StatusCheckEvery                                = null,

                                              Boolean                                                             DisablePushData                                 = false,
                                              Boolean                                                             DisablePushStatus                               = false,
                                              Boolean                                                             DisableAuthentication                           = false,
                                              Boolean                                                             DisableSendChargeDetailRecords                  = false,

                                              Action<OICPv2_1.CPO.WWCPCPOAdapter>                                 OICPConfigurator                                = null,
                                              Action<ICSORoamingProvider>                                         Configurator                                    = null,
                                              DNSClient                                                           DNSClient                                       = null)

        {

            #region Initial checks

            if (RoamingNetwork    == null)
                throw new ArgumentNullException(nameof(RoamingNetwork),  "The given roaming network must not be null!");

            if (Id == null)
                throw new ArgumentNullException(nameof(Id),              "The given unique roaming provider identification must not be null!");

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Name),            "The given roaming provider name must not be null or empty!");

            if (RemoteHostname    == null)
                throw new ArgumentNullException(nameof(RemoteHostname),  "The given remote hostname must not be null!");

            #endregion

            var NewRoamingProvider = new OICPv2_1.CPO.WWCPCPOAdapter(Id,
                                                                     Name,
                                                                     RoamingNetwork,

                                                                     RemoteHostname,
                                                                     RemoteTCPPort,
                                                                     RemoteCertificateValidator,
                                                                     ClientCert,
                                                                     RemoteHTTPVirtualHost,
                                                                     URIPrefix,
                                                                     HTTPUserAgent,
                                                                     QueryTimeout,

                                                                     ServerName,
                                                                     ServerTCPPort,
                                                                     ServerURIPrefix,
                                                                     ServerContentType,
                                                                     ServerRegisterHTTPRootService,
                                                                     ServerAutoStart,

                                                                     ClientLoggingContext,
                                                                     ServerLoggingContext,
                                                                     LogfileCreator,

                                                                     EVSE2EVSEDataRecord,
                                                                     EVSEStatusUpdate2EVSEStatusRecord,
                                                                     WWCPCDR2OICPCDR,
                                                                     EVSEDataRecord2XML,
                                                                     EVSEStatusRecord2XML,
                                                                     ChargeDetailRecord2XML,

                                                                     DefaultOperator,
                                                                     DefaultOperatorIdFormat,
                                                                     OperatorNameSelector,
                                                                     IncludeEVSEs,
                                                                     ServiceCheckEvery,
                                                                     StatusCheckEvery,

                                                                     DisablePushData,
                                                                     DisablePushStatus,
                                                                     DisableAuthentication,
                                                                     DisableSendChargeDetailRecords,

                                                                     DNSClient);


            OICPConfigurator?.Invoke(NewRoamingProvider);

            return RoamingNetwork.
                       CreateNewRoamingProvider(NewRoamingProvider,
                                                Configurator);

        }

        #endregion

        #region CreateOICPv2_1_CPORoamingProvider(this RoamingNetwork, Id, Name, SOAPServer, RemoteHostname, ...)

        /// <summary>
        /// Create and register a new electric vehicle roaming provider
        /// using the OICP protocol and having the given unique electric
        /// vehicle roaming provider identification.
        /// </summary>
        /// 
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="SOAPServer">An optional identification string for the HTTP server.</param>
        /// <param name="ServerURIPrefix">An optional prefix for the HTTP URIs.</param>
        /// 
        /// <param name="RemoteHostname">The hostname of the remote OICP service.</param>
        /// <param name="RemoteTCPPort">An optional TCP port of the remote OICP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCert">The TLS client certificate to use.</param>
        /// <param name="RemoteHTTPVirtualHost">An optional HTTP virtual hostname of the remote OICP service.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="QueryTimeout">An optional timeout for upstream queries.</param>
        /// 
        /// <param name="ClientLoggingContext">An optional context for logging client methods.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// 
        /// <param name="EVSE2EVSEDataRecord">A delegate to process an EVSE data record, e.g. before pushing it to the roaming provider.</param>
        /// <param name="EVSEDataRecord2XML">A delegate to process the XML representation of an EVSE data record, e.g. before pushing it to the roaming provider.</param>
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
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public static ICSORoamingProvider

            CreateOICPv2_1_CPORoamingProvider(this RoamingNetwork                                                 RoamingNetwork,
                                              CSORoamingProvider_Id                                               Id,
                                              I18NString                                                          Name,
                                              SOAPServer                                                          SOAPServer,

                                              String                                                              RemoteHostname,
                                              IPPort                                                              RemoteTCPPort                                   = null,
                                              RemoteCertificateValidationCallback                                 RemoteCertificateValidator                      = null,
                                              X509Certificate                                                     ClientCert                                      = null,
                                              String                                                              RemoteHTTPVirtualHost                           = null,
                                              String                                                              URIPrefix                                       = OICPv2_1.CPO.CPOClient.DefaultURIPrefix,
                                              String                                                              HTTPUserAgent                                   = OICPv2_1.CPO.CPOClient.DefaultHTTPUserAgent,
                                              TimeSpan?                                                           QueryTimeout                                    = null,

                                              String                                                              ServerURIPrefix                                 = null,

                                              String                                                              ClientLoggingContext                            = OICPv2_1.CPO.CPOClient.CPOClientLogger.DefaultContext,
                                              String                                                              ServerLoggingContext                            = OICPv2_1.CPO.CPOServerLogger.DefaultContext,
                                              LogfileCreatorDelegate                                              LogfileCreator                                  = null,

                                              OICPv2_1.CPO.EVSE2EVSEDataRecordDelegate                            EVSE2EVSEDataRecord                             = null,
                                              OICPv2_1.CPO.EVSEStatusUpdate2EVSEStatusRecordDelegate              EVSEStatusUpdate2EVSEStatusRecord               = null,
                                              OICPv2_1.CPO.WWCPChargeDetailRecord2OICPChargeDetailRecordDelegate  WWCPChargeDetailRecord2OICPChargeDetailRecord   = null,
                                              OICPv2_1.CPO.EVSEDataRecord2XMLDelegate                             EVSEDataRecord2XML                              = null,
                                              OICPv2_1.CPO.EVSEStatusRecord2XMLDelegate                           EVSEStatusRecord2XML                            = null,
                                              OICPv2_1.CPO.ChargeDetailRecord2XMLDelegate                         ChargeDetailRecord2XML                          = null,

                                              ChargingStationOperator                                             DefaultOperator                                 = null,
                                              OperatorIdFormats                                                   DefaultOperatorIdFormat                         = OperatorIdFormats.ISO_STAR,
                                              ChargingStationOperatorNameSelectorDelegate                         OperatorNameSelector                            = null,
                                              IncludeEVSEDelegate                                                 IncludeEVSEs                                    = null,
                                              TimeSpan?                                                           ServiceCheckEvery                               = null,
                                              TimeSpan?                                                           StatusCheckEvery                                = null,

                                              Boolean                                                             DisablePushData                                 = false,
                                              Boolean                                                             DisablePushStatus                               = false,
                                              Boolean                                                             DisableAuthentication                           = false,
                                              Boolean                                                             DisableSendChargeDetailRecords                  = false,

                                              Action<OICPv2_1.CPO.WWCPCPOAdapter>                                 OICPConfigurator                                = null,
                                              Action<ICSORoamingProvider>                                         Configurator                                    = null,
                                              DNSClient                                                           DNSClient                                       = null)

        {

            #region Initial checks

            if (SOAPServer == null)
                throw new ArgumentNullException(nameof(SOAPServer),      "The given SOAP/HTTP server must not be null!");


            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork),  "The given roaming network must not be null!");

            if (Id == null)
                throw new ArgumentNullException(nameof(Id),              "The given unique roaming provider identification must not be null!");

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Name),            "The given roaming provider name must not be null or empty!");

            if (RemoteHostname == null)
                throw new ArgumentNullException(nameof(RemoteHostname),  "The given remote hostname must not be null!");

            #endregion

            var NewRoamingProvider = new OICPv2_1.CPO.WWCPCPOAdapter(Id,
                                                                     Name,
                                                                     RoamingNetwork,

                                                                     new OICPv2_1.CPO.CPOClient(Id.ToString(),
                                                                                                RemoteHostname,
                                                                                                RemoteTCPPort,
                                                                                                RemoteCertificateValidator,
                                                                                                ClientCert,
                                                                                                RemoteHTTPVirtualHost,
                                                                                                URIPrefix,
                                                                                                HTTPUserAgent,
                                                                                                QueryTimeout,
                                                                                                DNSClient,
                                                                                                ClientLoggingContext,
                                                                                                LogfileCreator),

                                                                     new OICPv2_1.CPO.CPOServer(SOAPServer,
                                                                                                ServerURIPrefix),

                                                                     ServerLoggingContext,
                                                                     LogfileCreator,

                                                                     EVSE2EVSEDataRecord,
                                                                     EVSEStatusUpdate2EVSEStatusRecord,
                                                                     WWCPChargeDetailRecord2OICPChargeDetailRecord,
                                                                     EVSEDataRecord2XML,
                                                                     EVSEStatusRecord2XML,
                                                                     ChargeDetailRecord2XML,

                                                                     DefaultOperator,
                                                                     DefaultOperatorIdFormat,
                                                                     OperatorNameSelector,
                                                                     IncludeEVSEs,
                                                                     ServiceCheckEvery,
                                                                     StatusCheckEvery,

                                                                     DisablePushData,
                                                                     DisablePushStatus,
                                                                     DisableAuthentication,
                                                                     DisableSendChargeDetailRecords);

            OICPConfigurator?.Invoke(NewRoamingProvider);

            return RoamingNetwork.
                       CreateNewRoamingProvider(NewRoamingProvider,
                                                Configurator);

        }

        #endregion

    }

}
