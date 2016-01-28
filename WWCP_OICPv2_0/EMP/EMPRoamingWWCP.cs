﻿/*
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
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using System.Threading;
using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
{

    /// <summary>
    /// A WWCP wrapper for the OICP v2.0 roaming client for e-mobility providers/EMPs.
    /// </summary>
    public class EMPRoamingWWCP : IeMobilityRoamingService
    {

        #region Data

        private readonly RoamingNetwork               _RoamingNetwork;

        #endregion

        #region Properties

        #region AuthorizatorId

        private readonly Authorizator_Id _AuthorizatorId;

        public Authorizator_Id AuthorizatorId
        {
            get
            {
                return _AuthorizatorId;
            }
        }

        #endregion

        #region Id

        private readonly RoamingProvider_Id _Id;

        /// <summary>
        /// The unique identification of the roaming provider.
        /// </summary>
        public RoamingProvider_Id Id
        {
            get
            {
                return _Id;
            }
        }

        #endregion

        #region Name

        private readonly I18NString _Name;

        /// <summary>
        /// The offical (multi-language) name of the roaming provider.
        /// </summary>
        public I18NString Name
        {
            get
            {
                return _Name;
            }
        }

        #endregion

        #region RoamingNetworkId

        /// <summary>
        /// The unique identification of the attached roaming network.
        /// </summary>
        public RoamingNetwork_Id RoamingNetworkId
        {
            get
            {
                return _RoamingNetwork.Id;
            }
        }

        #endregion


        #region EMPRoaming

        private readonly EMPRoaming _EMPRoaming;

        /// <summary>
        /// The wrapped EMP roaming object.
        /// </summary>
        public EMPRoaming EMPRoaming
        {
            get
            {
                return _EMPRoaming;
            }
        }

        #endregion

        #region EMPClient

        /// <summary>
        /// The EMP client.
        /// </summary>
        public EMPClient EMPClient
        {
            get
            {
                return _EMPRoaming.EMPClient;
            }
        }

        #endregion

        #region EMPServer

        /// <summary>
        /// The EMP server.
        /// </summary>
        public EMPServer EMPServer
        {
            get
            {
                return _EMPRoaming.EMPServer;
            }
        }

        #endregion

        #region EMPServerLogger

        /// <summary>
        /// The EMP server logger.
        /// </summary>
        public EMPServerLogger EMPServerLogger
        {
            get
            {
                return _EMPRoaming.EMPServerLogger;
            }
        }

        #endregion

        #region DNSClient

        /// <summary>
        /// The DNSc server.
        /// </summary>
        public DNSClient DNSClient
        {
            get
            {
                return _EMPRoaming.DNSClient;
            }
        }

        #endregion

        #endregion

        #region Events

        #region OnAuthorizeStart

        /// <summary>
        /// An event sent whenever a authorize start command was received.
        /// </summary>
        public event RequestLogHandler OnLogAuthorizeStart;

        /// <summary>
        /// An event sent whenever a authorize start response was sent.
        /// </summary>
        public event AccessLogHandler OnLogAuthorizeStarted;

        /// <summary>
        /// An event sent whenever a authorize start command was received.
        /// </summary>
        public event OnAuthorizeStartDelegate OnAuthorizeStart
        {

            add
            {
                _EMPRoaming.OnAuthorizeStart += value;
            }

            remove
            {
                _EMPRoaming.OnAuthorizeStart -= value;
            }

        }

        #endregion

        #region OnAuthorizeStop

        /// <summary>
        /// An event sent whenever a authorize start command was received.
        /// </summary>
        public event RequestLogHandler OnLogAuthorizeStop;

        /// <summary>
        /// An event sent whenever a authorize start response was sent.
        /// </summary>
        public event AccessLogHandler OnLogAuthorizeStopped;

        /// <summary>
        /// An event sent whenever a authorize start command was received.
        /// </summary>
        public event OnAuthorizeStopDelegate OnAuthorizeStop
        {

            add
            {
                _EMPRoaming.OnAuthorizeStop += value;
            }

            remove
            {
                _EMPRoaming.OnAuthorizeStop -= value;
            }

        }

        #endregion

        #endregion

        #region Constructor(s)

        #region EMPRoamingWWCP(Id, Name, RoamingNetwork, EMPRoaming)

        /// <summary>
        /// Create a new WWCP wrapper for the OICP roaming client for e-mobility providers/EMPs.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// <param name="EMPRoaming">A OICP EMP roaming object to be mapped to WWCP.</param>
        public EMPRoamingWWCP(RoamingProvider_Id  Id,
                              I18NString          Name,
                              RoamingNetwork      RoamingNetwork,
                              EMPRoaming          EMPRoaming)


        {

            #region Initial checks

            if (Id             == null)
                throw new ArgumentNullException("Id",              "The given unique roaming provider identification must not be null or empty!");

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException("Name",            "The given roaming provider name must not be null or empty!");

            if (RoamingNetwork == null)
                throw new ArgumentNullException("RoamingNetwork",  "The given roaming network must not be null!");

            if (EMPRoaming     == null)
                throw new ArgumentNullException("EMPRoaming",      "The given OICP EMP Roaming object must not be null!");

            #endregion

            this._Id                        = Id;
            this._Name                      = Name;
            this._RoamingNetwork            = RoamingNetwork;
            this._EMPRoaming                = EMPRoaming;
            this._AuthorizatorId            = Authorizator_Id.Parse(Id.ToString());

        }

        #endregion

        #region EMPRoamingWWCP(Id, Name, RoamingNetwork, EMPClient, EMPServer)

        /// <summary>
        /// Create a new WWCP wrapper for the OICP roaming client for e-mobility providers/EMPs.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// <param name="EMPClient">An OICP EMP client.</param>
        /// <param name="EMPServer">An OICP EMP sever.</param>
        public EMPRoamingWWCP(RoamingProvider_Id  Id,
                              I18NString          Name,
                              RoamingNetwork      RoamingNetwork,
                              EMPClient           EMPClient,
                              EMPServer           EMPServer)

            : this(Id,
                   Name,
                   RoamingNetwork,
                   new EMPRoaming(EMPClient,
                                  EMPServer)
                  )

        { }

        #endregion

        #region EMPRoamingWWCP(Id, Name, RoamingNetwork, RemoteHostName, ...)

        /// <summary>
        /// Create a new WWCP wrapper for the OICP roaming client for e-mobility providers/EMPs.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
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
        public EMPRoamingWWCP(RoamingProvider_Id           Id,
                              I18NString                   Name,
                              RoamingNetwork               RoamingNetwork,

                              String                       RemoteHostname,
                              IPPort                       RemoteTCPPort          = null,
                              String                       RemoteHTTPVirtualHost  = null,
                              String                       HTTPUserAgent          = EMPClient.DefaultHTTPUserAgent,
                              TimeSpan?                    QueryTimeout           = null,

                              String                       ServerName             = EMPServer.DefaultHTTPServerName,
                              IPPort                       ServerTCPPort          = null,
                              String                       ServerURIPrefix        = "",
                              Boolean                      ServerAutoStart        = false,

                              DNSClient                    DNSClient              = null)

            : this(Id,
                   Name,
                   RoamingNetwork,
                   new EMPRoaming(Id.ToString(),
                                  RemoteHostname,
                                  RemoteTCPPort,
                                  RemoteHTTPVirtualHost,
                                  HTTPUserAgent,
                                  QueryTimeout,

                                  ServerName,
                                  ServerTCPPort,
                                  ServerURIPrefix,
                                  ServerAutoStart,

                                  DNSClient)
                  )

        { }

        #endregion

        #endregion


        #region PullEVSEData(ProviderId, SearchCenter = null, DistanceKM = 0.0, LastCall = null, QueryTimeout = null, OnException = null)

        /// <summary>
        /// Create a new task querying EVSE data from the OICP server.
        /// The request might either have none, 'SearchCenter + DistanceKM' or 'LastCall' parameters.
        /// Because of limitations at Hubject the SearchCenter and LastCall parameters can not be used at the same time!
        /// </summary>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="LastCall">An optional timestamp of the last call.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<eRoamingEVSEData>

            PullEVSEData(EVSP_Id           ProviderId,
                         GeoCoordinate     SearchCenter  = null,
                         Double            DistanceKM    = 0.0,
                         DateTime?         LastCall      = null,
                         TimeSpan?         QueryTimeout  = null)

        {

            return await _EMPRoaming.PullEVSEData(ProviderId,
                                                        SearchCenter,
                                                        DistanceKM,
                                                        LastCall,
                                                        QueryTimeout);

        }

        #endregion


        #region PullEVSEStatus(ProviderId, SearchCenter = null, DistanceKM = 0.0, EVSEStatusFilter = null, QueryTimeout = null)

        /// <summary>
        /// Create a new task requesting the current status of all EVSEs (within an optional search radius and status).
        /// </summary>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="EVSEStatusFilter">An optional EVSE status as filter criteria.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<eRoamingEVSEStatus>

            PullEVSEStatus(EVSP_Id          ProviderId,
                           GeoCoordinate    SearchCenter      = null,
                           Double           DistanceKM        = 0.0,
                           EVSEStatusType?  EVSEStatusFilter  = null,
                           TimeSpan?        QueryTimeout      = null)

        {

            var result = await _EMPRoaming.PullEVSEStatus(ProviderId,
                                                         SearchCenter,
                                                         DistanceKM,
                                                         EVSEStatusFilter,
                                                         QueryTimeout);

            //ToDo: Process the HTTP!
            return result;

        }

        #endregion

        #region PullEVSEStatusById(ProviderId, EVSEIds, QueryTimeout = null)

        /// <summary>
        /// Create a new task requesting the current status of up to 100 EVSEs by their EVSE Ids.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="EVSEIds">Up to 100 EVSE Ids.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<eRoamingEVSEStatusById>

            PullEVSEStatusById(EVSP_Id               ProviderId,
                               IEnumerable<EVSE_Id>  EVSEIds,
                               TimeSpan?             QueryTimeout = null)

        {

            var result = await _EMPRoaming.PullEVSEStatusById(ProviderId,
                                                             EVSEIds,
                                                             QueryTimeout);

            //ToDo: Process the HTTP!
            return result;

        }

        #endregion


        #region SearchEVSE(ProviderId, SearchCenter = null, DistanceKM = 0.0, Address = null, Plug = null, ChargingFacility = null, QueryTimeout = null)

        /// <summary>
        /// Create a new Search EVSE request.
        /// </summary>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="SearchCenter">An optional geocoordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="Address">An optional address of the charging stations.</param>
        /// <param name="Plug">Optional plugs of the charging station.</param>
        /// <param name="ChargingFacility">Optional charging facilities of the charging station.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<eRoamingEvseSearchResult>

            SearchEVSE(EVSP_Id              ProviderId,
                       GeoCoordinate        SearchCenter      = null,
                       Double               DistanceKM        = 0.0,
                       Address              Address           = null,
                       PlugTypes?           Plug              = null,
                       ChargingFacilities?  ChargingFacility  = null,
                       TimeSpan?            QueryTimeout      = null)

        {

            var result = await _EMPRoaming.SearchEVSE(ProviderId,
                                                     SearchCenter,
                                                     DistanceKM,
                                                     Address,
                                                     Plug,
                                                     ChargingFacility,
                                                     QueryTimeout);

            //ToDo: Process the HTTP!
            return result;

        }

        #endregion


        #region PushAuthenticationData(ProviderAuthenticationDataRecords, OICPAction = fullLoad, QueryTimeout = null)

        /// <summary>
        /// Create a new task pushing provider authentication data records onto the OICP server.
        /// </summary>
        /// <param name="ProviderAuthenticationDataRecords">An enumeration of provider authentication data records.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<eRoamingAcknowledgement>

            PushAuthenticationData(IEnumerable<ProviderAuthenticationData>  ProviderAuthenticationDataRecords,
                                   ActionType                               OICPAction    = ActionType.fullLoad,
                                   TimeSpan?                                QueryTimeout  = null)

        {

            var result = await _EMPRoaming.PushAuthenticationData(ProviderAuthenticationDataRecords,
                                                                 OICPAction,
                                                                 QueryTimeout);

            //ToDo: Process the HTTP!
            return result;

        }

        #endregion

        #region PushAuthenticationData(AuthorizationIdentifications, ProviderId, OICPAction = fullLoad, QueryTimeout = null)

        /// <summary>
        /// Create a new task pushing authorization identifications onto the OICP server.
        /// </summary>
        /// <param name="AuthorizationIdentifications">An enumeration of authorization identifications.</param>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<eRoamingAcknowledgement>

            PushAuthenticationData(IEnumerable<AuthorizationIdentification>  AuthorizationIdentifications,
                                   EVSP_Id                                   ProviderId,
                                   ActionType                                OICPAction    = ActionType.fullLoad,
                                   TimeSpan?                                 QueryTimeout  = null)

        {

            var result = await _EMPRoaming.PushAuthenticationData(AuthorizationIdentifications,
                                                                 ProviderId,
                                                                 OICPAction,
                                                                 QueryTimeout);

            //ToDo: Process the HTTP!
            return result;

        }

        #endregion


        #region GetChargeDetailRecords(ProviderId, From, To, QueryTimeout = null)

        /// <summary>
        /// Create a new task querying charge detail records from the OICP server.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="From">The starting time.</param>
        /// <param name="To">The end time.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<IEnumerable<eRoamingChargeDetailRecord>>

            GetChargeDetailRecords(EVSP_Id    ProviderId,
                                   DateTime   From,
                                   DateTime   To,
                                   TimeSpan?  QueryTimeout  = null)

        {

            var result = await _EMPRoaming.GetChargeDetailRecords(ProviderId,
                                                                 From,
                                                                 To,
                                                                 QueryTimeout);

            //ToDo: Process the HTTP!
            return result;

        }

        #endregion


    }

}