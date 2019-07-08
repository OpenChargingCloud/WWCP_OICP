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
using System.Net.Security;
using System.Security.Authentication;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// Extentions methods for the WWCP wrapper for OICP roaming clients for e-mobility providers/EMPs.
    /// </summary>
    public static class EMPExtentions
    {

        #region CreateOICPv2_2_EMPRoamingProvider(this RoamingNetwork, Id, Name, RemoteHostname, ...)

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
        /// <param name="ClientCertificateSelector">A delegate to select a TLS client certificate.</param>
        /// <param name="RemoteHTTPVirtualHost">An optional HTTP virtual hostname of the remote OICP service.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// <param name="MaxNumberOfRetries">The default number of maximum transmission retries.</param>
        /// 
        /// <param name="ServerName">An optional identification string for the HTTP server.</param>
        /// <param name="ServerTCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="ServerCertificateSelector">An optional delegate to select a SSL/TLS server certificate.</param>
        /// <param name="RemoteClientCertificateValidator">An optional delegate to verify the SSL/TLS client certificate used for authentication.</param>
        /// <param name="RemoteClientCertificateSelector">An optional delegate to select the SSL/TLS client certificate used for authentication.</param>
        /// <param name="AllowedTLSProtocols">The SSL/TLS protocol(s) allowed for this connection.</param>
        /// <param name="ServerURIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="ServerAutoStart">Whether to start the server immediately or not.</param>
        /// 
        /// <param name="ClientLoggingContext">An optional context for logging client methods.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// 
        /// <param name="EVSEDataRecord2EVSE">A delegate to process an EVSE data record after receiving it from the roaming provider.</param>
        /// 
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// 
        /// <param name="OICPConfigurator">An optional delegate to configure the new OICP roaming provider after its creation.</param>
        /// <param name="Configurator">An optional delegate to configure the new roaming provider after its creation.</param>
        public static OICPv2_2.EMP.WWCPEMPAdapter

            CreateOICPv2_2_EMPRoamingProvider(this RoamingNetwork                       RoamingNetwork,
                                              EMPRoamingProvider_Id                     Id,
                                              I18NString                                Name,

                                              HTTPHostname                              RemoteHostname,
                                              IPPort?                                   RemoteTCPPort                      = null,
                                              RemoteCertificateValidationCallback       RemoteCertificateValidator         = null,
                                              LocalCertificateSelectionCallback         ClientCertificateSelector          = null,
                                              HTTPHostname?                             RemoteHTTPVirtualHost              = null,
                                              HTTPPath?                                  URIPrefix                          = null,
                                              String                                    EVSEDataURI                        = OICPv2_2.EMP.EMPClient.DefaultEVSEDataURI,
                                              String                                    EVSEStatusURI                      = OICPv2_2.EMP.EMPClient.DefaultEVSEStatusURI,
                                              String                                    AuthenticationDataURI              = OICPv2_2.EMP.EMPClient.DefaultAuthenticationDataURI,
                                              String                                    ReservationURI                     = OICPv2_2.EMP.EMPClient.DefaultReservationURI,
                                              String                                    AuthorizationURI                   = OICPv2_2.EMP.EMPClient.DefaultAuthorizationURI,
                                              OICPv2_2.Provider_Id?                     DefaultProviderId                  = null,

                                              String                                    HTTPUserAgent                      = OICPv2_2.EMP.EMPClient.DefaultHTTPUserAgent,
                                              TimeSpan?                                 RequestTimeout                     = null,
                                              Byte?                                     MaxNumberOfRetries                 = OICPv2_2.EMP.EMPClient.DefaultMaxNumberOfRetries,

                                              String                                    ServerName                         = OICPv2_2.EMP.EMPServer.DefaultHTTPServerName,
                                              String                                    ServiceId                          = null,
                                              IPPort?                                   ServerTCPPort                      = null,
                                              ServerCertificateSelectorDelegate         ServerCertificateSelector          = null,
                                              RemoteCertificateValidationCallback       RemoteClientCertificateValidator   = null,
                                              LocalCertificateSelectionCallback         RemoteClientCertificateSelector    = null,
                                              SslProtocols                              AllowedTLSProtocols                = SslProtocols.Tls12,
                                              HTTPPath?                                  ServerURIPrefix                    = null,
                                              String                                    ServerAuthorizationURI             = OICPv2_2.EMP.EMPServer.DefaultAuthorizationURI,
                                              HTTPContentType                           ServerContentType                  = null,
                                              Boolean                                   ServerRegisterHTTPRootService      = true,
                                              Boolean                                   ServerAutoStart                    = false,

                                              String                                    ClientLoggingContext               = OICPv2_2.EMP.EMPClient.EMPClientLogger.DefaultContext,
                                              String                                    ServerLoggingContext               = OICPv2_2.EMP.EMPServerLogger.DefaultContext,
                                              LogfileCreatorDelegate                    LogfileCreator                     = null,

                                              OICPv2_2.EMP.EVSEDataRecord2EVSEDelegate  EVSEDataRecord2EVSE                = null,

                                              OICPv2_2.EVSEOperatorFilterDelegate       EVSEOperatorFilter                 = null,

                                              TimeSpan?                                 PullDataServiceEvery               = null,
                                              Boolean                                   DisablePullData                    = false,
                                              TimeSpan?                                 PullDataServiceRequestTimeout      = null,

                                              TimeSpan?                                 PullStatusServiceEvery             = null,
                                              Boolean                                   DisablePullStatus                  = false,
                                              TimeSpan?                                 PullStatusServiceRequestTimeout    = null,

                                              eMobilityProvider                         DefaultProvider                    = null,
                                              GeoCoordinate?                            DefaultSearchCenter                = null,
                                              UInt64?                                   DefaultDistanceKM                  = null,

                                              DNSClient                                 DNSClient                          = null,

                                              Action<OICPv2_2.EMP.WWCPEMPAdapter>       OICPConfigurator                   = null,
                                              Action<IEMPRoamingProvider>               Configurator                       = null)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork),  "The given roaming network must not be null!");

            if (Id == null)
                throw new ArgumentNullException(nameof(Id),              "The given unique roaming provider identification must not be null!");

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Name),            "The given roaming provider name must not be null or empty!");

            if (RemoteHostname == null)
                throw new ArgumentNullException(nameof(RemoteHostname),  "The given remote hostname must not be null!");

            #endregion

            var NewRoamingProvider = new OICPv2_2.EMP.WWCPEMPAdapter(Id,
                                                                     Name,
                                                                     RoamingNetwork,

                                                                     RemoteHostname,
                                                                     RemoteTCPPort,
                                                                     RemoteCertificateValidator,
                                                                     ClientCertificateSelector,
                                                                     RemoteHTTPVirtualHost,
                                                                     URIPrefix ?? OICPv2_2.EMP.EMPClient.DefaultURIPrefix,
                                                                     EVSEDataURI,
                                                                     EVSEStatusURI,
                                                                     AuthenticationDataURI,
                                                                     ReservationURI,
                                                                     AuthorizationURI,
                                                                     DefaultProviderId,

                                                                     HTTPUserAgent,
                                                                     RequestTimeout,
                                                                     MaxNumberOfRetries,

                                                                     ServerName,
                                                                     ServiceId,
                                                                     ServerTCPPort,
                                                                     ServerCertificateSelector,
                                                                     RemoteClientCertificateValidator,
                                                                     RemoteClientCertificateSelector,
                                                                     AllowedTLSProtocols,
                                                                     ServerURIPrefix ?? OICPv2_2.EMP.EMPServer.DefaultURIPrefix,
                                                                     ServerAuthorizationURI,
                                                                     ServerContentType,
                                                                     ServerRegisterHTTPRootService,
                                                                     ServerAutoStart,

                                                                     ClientLoggingContext,
                                                                     ServerLoggingContext,
                                                                     LogfileCreator,

                                                                     EVSEDataRecord2EVSE,

                                                                     EVSEOperatorFilter,

                                                                     PullDataServiceEvery,
                                                                     DisablePullData,
                                                                     PullDataServiceRequestTimeout,

                                                                     PullStatusServiceEvery,
                                                                     DisablePullStatus,
                                                                     PullStatusServiceRequestTimeout,

                                                                     DefaultProvider,
                                                                     DefaultSearchCenter,
                                                                     DefaultDistanceKM,

                                                                     DNSClient);


            OICPConfigurator?.Invoke(NewRoamingProvider);

            return RoamingNetwork.
                       CreateNewRoamingProvider(NewRoamingProvider,
                                                Configurator) as OICPv2_2.EMP.WWCPEMPAdapter;

        }

        #endregion

        #region CreateOICPv2_2_EMPRoamingProvider(this RoamingNetwork, Id, Name, SOAPServer, RemoteHostname, ...)

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
        /// <param name="RemoteHTTPVirtualHost">An optional HTTP virtual hostname of the remote OICP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCertificateSelector">A delegate to select a TLS client certificate.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// <param name="MaxNumberOfRetries">The default number of maximum transmission retries.</param>
        /// 
        /// <param name="ClientLoggingContext">An optional context for logging client methods.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// 
        /// <param name="EVSEDataRecord2EVSE">A delegate to process an EVSE data record after receiving it from the roaming provider.</param>
        /// 
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// 
        /// <param name="OICPConfigurator">An optional delegate to configure the new OICP roaming provider after its creation.</param>
        /// <param name="Configurator">An optional delegate to configure the new roaming provider after its creation.</param>
        public static OICPv2_2.EMP.WWCPEMPAdapter

            CreateOICPv2_2_EMPRoamingProvider(this RoamingNetwork                       RoamingNetwork,
                                              EMPRoamingProvider_Id                     Id,
                                              I18NString                                Name,
                                              SOAPServer                                SOAPServer,

                                              HTTPHostname                              RemoteHostname,
                                              IPPort?                                   RemoteTCPPort                     = null,
                                              RemoteCertificateValidationCallback       RemoteCertificateValidator        = null,
                                              LocalCertificateSelectionCallback         ClientCertificateSelector         = null,
                                              HTTPHostname?                             RemoteHTTPVirtualHost             = null,
                                              HTTPPath?                                  URIPrefix                         = null,
                                              String                                    EVSEDataURI                       = OICPv2_2.EMP.EMPClient.DefaultEVSEDataURI,
                                              String                                    EVSEStatusURI                     = OICPv2_2.EMP.EMPClient.DefaultEVSEStatusURI,
                                              String                                    AuthenticationDataURI             = OICPv2_2.EMP.EMPClient.DefaultAuthenticationDataURI,
                                              String                                    ReservationURI                    = OICPv2_2.EMP.EMPClient.DefaultReservationURI,
                                              String                                    AuthorizationURI                  = OICPv2_2.EMP.EMPClient.DefaultAuthorizationURI,
                                              OICPv2_2.Provider_Id?                     DefaultProviderId                 = null,

                                              String                                    HTTPUserAgent                     = OICPv2_2.EMP.EMPClient.DefaultHTTPUserAgent,
                                              TimeSpan?                                 RequestTimeout                    = null,
                                              Byte?                                     MaxNumberOfRetries                = OICPv2_2.EMP.EMPClient.DefaultMaxNumberOfRetries,

                                              String                                    ServiceId                         = null,
                                              HTTPPath?                                  ServerURIPrefix                   = null,
                                              String                                    ServerAuthorizationURI            = OICPv2_2.EMP.EMPServer.DefaultAuthorizationURI,

                                              String                                    ClientLoggingContext              = OICPv2_2.EMP.EMPClient.EMPClientLogger.DefaultContext,
                                              String                                    ServerLoggingContext              = OICPv2_2.EMP.EMPServerLogger.DefaultContext,
                                              LogfileCreatorDelegate                    LogfileCreator                    = null,

                                              OICPv2_2.EMP.EVSEDataRecord2EVSEDelegate  EVSEDataRecord2EVSE               = null,

                                              OICPv2_2.EVSEOperatorFilterDelegate       EVSEOperatorFilter                = null,

                                              TimeSpan?                                 PullDataServiceEvery              = null,
                                              Boolean                                   DisablePullData                   = false,
                                              TimeSpan?                                 PullDataServiceRequestTimeout     = null,

                                              TimeSpan?                                 PullStatusServiceEvery            = null,
                                              Boolean                                   DisablePullStatus                 = false,
                                              TimeSpan?                                 PullStatusServiceRequestTimeout   = null,

                                              eMobilityProvider                         DefaultProvider                   = null,
                                              GeoCoordinate?                            DefaultSearchCenter               = null,
                                              UInt64?                                   DefaultDistanceKM                 = null,

                                              DNSClient                                 DNSClient                         = null,

                                              Action<OICPv2_2.EMP.WWCPEMPAdapter>       OICPConfigurator                  = null,
                                              Action<IEMPRoamingProvider>               Configurator                      = null)

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

            var NewRoamingProvider = new OICPv2_2.EMP.WWCPEMPAdapter(Id,
                                                                     Name,
                                                                     RoamingNetwork,

                                                                     new OICPv2_2.EMP.EMPClient(Id.ToString(),
                                                                                                RemoteHostname,
                                                                                                RemoteTCPPort,
                                                                                                RemoteCertificateValidator,
                                                                                                ClientCertificateSelector,
                                                                                                RemoteHTTPVirtualHost,
                                                                                                URIPrefix ?? OICPv2_2.EMP.EMPClient.DefaultURIPrefix,
                                                                                                EVSEDataURI,
                                                                                                EVSEStatusURI,
                                                                                                AuthenticationDataURI,
                                                                                                ReservationURI,
                                                                                                AuthorizationURI,
                                                                                                DefaultProviderId,

                                                                                                HTTPUserAgent,
                                                                                                RequestTimeout,
                                                                                                MaxNumberOfRetries,
                                                                                                DNSClient,
                                                                                                ClientLoggingContext,
                                                                                                LogfileCreator),

                                                                     new OICPv2_2.EMP.EMPServer(SOAPServer,
                                                                                                ServiceId,
                                                                                                ServerURIPrefix ?? OICPv2_2.EMP.EMPServer.DefaultURIPrefix,
                                                                                                ServerAuthorizationURI),

                                                                     ServerLoggingContext,
                                                                     LogfileCreator,

                                                                     EVSEDataRecord2EVSE,

                                                                     EVSEOperatorFilter,

                                                                     PullDataServiceEvery,
                                                                     DisablePullData,
                                                                     PullDataServiceRequestTimeout,

                                                                     PullStatusServiceEvery,
                                                                     DisablePullStatus,
                                                                     PullStatusServiceRequestTimeout,

                                                                     DefaultProvider,
                                                                     DefaultSearchCenter,
                                                                     DefaultDistanceKM);


            OICPConfigurator?.Invoke(NewRoamingProvider);

            return RoamingNetwork.
                       CreateNewRoamingProvider(NewRoamingProvider,
                                                Configurator) as OICPv2_2.EMP.WWCPEMPAdapter;

        }

        #endregion

    }

}
