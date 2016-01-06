/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/GraphDefined/WWCP_OICP>
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

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// Exytentions methods for the WWCP wrapper for OICP roaming clients for EVSE operators/CPOs.
    /// </summary>
    public static class CPORoamingWWCPExtentions
    {

        #region CreateNewOICPRoamingProvider(this RoamingNetwork, Id, Name, RemoteHostname, ... , Action = null)

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
        /// <param name="RemoteHTTPVirtualHost">An optional HTTP virtual hostname of the remote OICP service.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="QueryTimeout">An optional timeout for upstream queries.</param>
        /// 
        /// <param name="ServerName">An optional identification string for the HTTP server.</param>
        /// <param name="ServerTCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="ServerURIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="ServerAutoStart">Whether to start the server immediately or not.</param>
        /// 
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// 
        /// <param name="OICPConfigurator">An optional delegate to configure the new OICP roaming provider after its creation.</param>
        /// <param name="Configurator">An optional delegate to configure the new roaming provider after its creation.</param>
        public static RoamingProvider CreateNewOICPRoamingProvider(this RoamingNetwork              RoamingNetwork,
                                                                   RoamingProvider_Id               Id,
                                                                   I18NString                       Name,

                                                                   String                           RemoteHostname,
                                                                   IPPort                           RemoteTCPPort          = null,
                                                                   String                           RemoteHTTPVirtualHost  = null,
                                                                   String                           HTTPUserAgent          = OICP_2_0.CPOClient.DefaultHTTPUserAgent,
                                                                   TimeSpan?                        QueryTimeout           = null,

                                                                   String                           ServerName             = OICP_2_0.CPOServer.DefaultHTTPServerName,
                                                                   IPPort                           ServerTCPPort          = null,
                                                                   String                           ServerURIPrefix        = "",
                                                                   Boolean                          ServerAutoStart        = true,

                                                                   DNSClient                        DNSClient              = null,

                                                                   Action<OICP_2_0.CPORoamingWWCP>  OICPConfigurator       = null,
                                                                   Action<RoamingProvider>          Configurator           = null)

        {

            #region Initial checks

            if (RoamingNetwork    == null)
                throw new ArgumentNullException("RoamingNetwork",  "The given roaming network must not be null!");

            if (Id == null)
                throw new ArgumentNullException("Id",              "The given unique roaming provider identification must not be null!");

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException("Name",            "The given roaming provider name must not be null or empty!");

            if (RemoteHostname    == null)
                throw new ArgumentNullException("RemoteHostname",  "The given remote hostname must not be null!");

            #endregion

            var NewRoamingProvider = new OICP_2_0.CPORoamingWWCP(Id,
                                                                 Name,
                                                                 RoamingNetwork,

                                                                 RemoteHostname,
                                                                 RemoteTCPPort,
                                                                 RemoteHTTPVirtualHost,
                                                                 HTTPUserAgent,
                                                                 QueryTimeout,

                                                                 ServerName,
                                                                 ServerTCPPort,
                                                                 ServerURIPrefix,
                                                                 ServerAutoStart,

                                                                 DNSClient);

            var ConfiguratorLocal = OICPConfigurator;
            if (ConfiguratorLocal != null)
                ConfiguratorLocal(NewRoamingProvider);

            return RoamingNetwork.CreateNewRoamingProvider(NewRoamingProvider,
                                                           Configurator);

        }

        #endregion

    }

}
