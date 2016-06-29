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
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// Extentions methods for the WWCP wrapper for OICP roaming clients for EVSE operators/CPOs.
    /// </summary>
    public static class CPOExtentions
    {

        #region CreateOICPv2_0_CPORoamingProvider(this RoamingNetwork, Id, Name, RemoteHostname, ... , Action = null)

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
        /// <param name="ServerName">An optional identification string for the HTTP server.</param>
        /// <param name="ServerTCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="ServerURIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="ServerAutoStart">Whether to start the server immediately or not.</param>
        /// 
        /// <param name="ClientLoggingContext">An optional context for logging client methods.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// 
        /// <param name="EVSE2EVSEDataRecord">A delegate to process an EVSE data record, e.g. before pushing it to the roaming provider.</param>
        /// <param name="EVSEDataRecord2XML">A delegate to process the XML representation of an EVSE data record, e.g. before pushing it to the roaming provider.</param>
        /// 
        /// <param name="IncludeEVSEs">Only include the EVSEs matching the given delegate.</param>
        /// <param name="ServiceCheckEvery">The service check intervall.</param>
        /// <param name="StatusCheckEvery">The status check intervall.</param>
        /// <param name="DisableAutoUploads">This service can be disabled, e.g. for debugging reasons.</param>
        /// 
        /// <param name="OICPConfigurator">An optional delegate to configure the new OICP roaming provider after its creation.</param>
        /// <param name="Configurator">An optional delegate to configure the new roaming provider after its creation.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public static AEVSEOperatorRoamingProvider

            CreateOICPv2_0_CPORoamingProvider(this RoamingNetwork                   RoamingNetwork,
                                              RoamingProvider_Id                    Id,
                                              I18NString                            Name,

                                              String                                RemoteHostname,
                                              IPPort                                RemoteTCPPort               = null,
                                              String                                RemoteHTTPVirtualHost       = null,
                                              RemoteCertificateValidationCallback   RemoteCertificateValidator  = null,
                                              X509Certificate                       ClientCert                  = null,
                                              String                                HTTPUserAgent               = OICPv2_0.CPOClient.DefaultHTTPUserAgent,
                                              TimeSpan?                             QueryTimeout                = null,

                                              String                                ServerName                  = OICPv2_0.CPOServer.DefaultHTTPServerName,
                                              IPPort                                ServerTCPPort               = null,
                                              String                                ServerURIPrefix             = "",
                                              Boolean                               ServerAutoStart             = true,

                                              String                                ClientLoggingContext        = OICPv2_0.CPOClient.CPOClientLogger.DefaultContext,
                                              String                                ServerLoggingContext        = OICPv2_0.CPOServerLogger.DefaultContext,
                                              Func<String, String, String>          LogFileCreator              = null,

                                              OICPv2_0.EVSE2EVSEDataRecordDelegate  EVSE2EVSEDataRecord         = null,
                                              OICPv2_0.EVSEDataRecord2XMLDelegate   EVSEDataRecord2XML          = null,

                                              Func<EVSE, Boolean>                   IncludeEVSEs                = null,
                                              TimeSpan?                             ServiceCheckEvery           = null,
                                              TimeSpan?                             StatusCheckEvery            = null,
                                              Boolean                               DisableAutoUploads          = false,

                                              Action<OICPv2_0.CPORoamingWWCP>       OICPConfigurator            = null,
                                              Action<AEVSEOperatorRoamingProvider>  Configurator                = null,
                                              DNSClient                             DNSClient                   = null)

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

            var NewRoamingProvider = new OICPv2_0.CPORoamingWWCP(Id,
                                                                 Name,
                                                                 RoamingNetwork,

                                                                 RemoteHostname,
                                                                 RemoteTCPPort,
                                                                 RemoteCertificateValidator,
                                                                 ClientCert,
                                                                 RemoteHTTPVirtualHost,
                                                                 HTTPUserAgent,
                                                                 QueryTimeout,

                                                                 ServerName,
                                                                 ServerTCPPort,
                                                                 ServerURIPrefix,
                                                                 ServerAutoStart,

                                                                 ClientLoggingContext,
                                                                 ServerLoggingContext,
                                                                 LogFileCreator,

                                                                 EVSE2EVSEDataRecord,
                                                                 EVSEDataRecord2XML,

                                                                 IncludeEVSEs,
                                                                 ServiceCheckEvery,
                                                                 StatusCheckEvery,
                                                                 DisableAutoUploads,

                                                                 DNSClient);


            OICPConfigurator?.Invoke(NewRoamingProvider);

            return RoamingNetwork.
                       CreateNewRoamingProvider(NewRoamingProvider,
                                                Configurator);

        }

        #endregion

        #region CreateOICPv2_0_CPORoamingProvider(this RoamingNetwork, Id, Name, SOAPServer, RemoteHostname, ...)

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
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// 
        /// <param name="EVSE2EVSEDataRecord">A delegate to process an EVSE data record, e.g. before pushing it to the roaming provider.</param>
        /// <param name="EVSEDataRecord2XML">A delegate to process the XML representation of an EVSE data record, e.g. before pushing it to the roaming provider.</param>
        /// 
        /// <param name="IncludeEVSEs">Only include the EVSEs matching the given delegate.</param>
        /// <param name="ServiceCheckEvery">The service check intervall.</param>
        /// <param name="StatusCheckEvery">The status check intervall.</param>
        /// <param name="DisableAutoUploads">This service can be disabled, e.g. for debugging reasons.</param>
        /// 
        /// <param name="OICPConfigurator">An optional delegate to configure the new OICP roaming provider after its creation.</param>
        /// <param name="Configurator">An optional delegate to configure the new roaming provider after its creation.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public static AEVSEOperatorRoamingProvider

            CreateOICPv2_0_CPORoamingProvider(this RoamingNetwork                   RoamingNetwork,
                                              RoamingProvider_Id                    Id,
                                              I18NString                            Name,
                                              SOAPServer                            SOAPServer,

                                              String                                RemoteHostname,
                                              IPPort                                RemoteTCPPort               = null,
                                              RemoteCertificateValidationCallback   RemoteCertificateValidator  = null,
                                              X509Certificate                       ClientCert                  = null,
                                              String                                RemoteHTTPVirtualHost       = null,
                                              String                                HTTPUserAgent               = OICPv2_0.CPOClient.DefaultHTTPUserAgent,
                                              TimeSpan?                             QueryTimeout                = null,

                                              String                                ServerURIPrefix             = null,

                                              String                                ClientLoggingContext        = OICPv2_0.CPOClient.CPOClientLogger.DefaultContext,
                                              String                                ServerLoggingContext        = OICPv2_0.CPOServerLogger.DefaultContext,
                                              Func<String, String, String>          LogFileCreator              = null,

                                              OICPv2_0.EVSE2EVSEDataRecordDelegate  EVSE2EVSEDataRecord         = null,
                                              OICPv2_0.EVSEDataRecord2XMLDelegate   EVSEDataRecord2XML          = null,

                                              Func<EVSE, Boolean>                   IncludeEVSEs                = null,
                                              TimeSpan?                             ServiceCheckEvery           = null,
                                              TimeSpan?                             StatusCheckEvery            = null,
                                              Boolean                               DisableAutoUploads          = false,

                                              Action<OICPv2_0.CPORoamingWWCP>       OICPConfigurator            = null,
                                              Action<AEVSEOperatorRoamingProvider>  Configurator                = null,
                                              DNSClient                             DNSClient                   = null)

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

            var NewRoamingProvider = new OICPv2_0.CPORoamingWWCP(Id,
                                                                 Name,
                                                                 RoamingNetwork,

                                                                 new OICPv2_0.CPOClient(Id.ToString(),
                                                                                        RemoteHostname,
                                                                                        RemoteTCPPort,
                                                                                        RemoteCertificateValidator,
                                                                                        ClientCert,
                                                                                        RemoteHTTPVirtualHost,
                                                                                        HTTPUserAgent,
                                                                                        QueryTimeout,
                                                                                        DNSClient,
                                                                                        ClientLoggingContext,
                                                                                        LogFileCreator),

                                                                 new OICPv2_0.CPOServer(SOAPServer,
                                                                                        ServerURIPrefix),

                                                                 ServerLoggingContext,
                                                                 LogFileCreator,

                                                                 EVSE2EVSEDataRecord,
                                                                 EVSEDataRecord2XML,

                                                                 IncludeEVSEs,
                                                                 ServiceCheckEvery,
                                                                 StatusCheckEvery,
                                                                 DisableAutoUploads);

            OICPConfigurator?.Invoke(NewRoamingProvider);

            return RoamingNetwork.
                       CreateNewRoamingProvider(NewRoamingProvider,
                                                Configurator);

        }

        #endregion

    }

}
