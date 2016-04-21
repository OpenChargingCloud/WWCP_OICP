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
using System.Net.Security;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
{

    /// <summary>
    /// A OICP v2.0 roaming client for EMPs.
    /// </summary>
    public class EMPRoaming
    {

        #region Data

        /// <summary>
        /// The default logging context.
        /// </summary>
        public const String DefaultLoggingContext = "OICP_EMPServer";

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


        #region EMPClient

        private readonly EMPClient _EMPClient;

        /// <summary>
        /// The EMP client part.
        /// </summary>
        public EMPClient EMPClient
        {
            get
            {
                return _EMPClient;
            }
        }

        #endregion

        #region EMPServer

        private readonly EMPServer _EMPServer;

        /// <summary>
        /// The EMP server part.
        /// </summary>
        public EMPServer EMPServer
        {
            get
            {
                return _EMPServer;
            }
        }

        #endregion

        #region EMPServerLogger

        private readonly EMPServerLogger _EMPServerLogger;

        /// <summary>
        /// The EMP server logger.
        /// </summary>
        public EMPServerLogger EMPServerLogger
        {
            get
            {
                return _EMPServerLogger;
            }
        }

        #endregion

        #region DNSClient

        /// <summary>
        /// The DNS client defines which DNS servers to use.
        /// </summary>
        public DNSClient DNSClient
        {
            get
            {
                return _EMPServer.DNSClient;
            }
        }

        #endregion

        #endregion

        #region Events

        // Client methods (logging)



        // Server methods

        #region OnAuthorizeStart

        /// <summary>
        /// An event sent whenever a authorize start command was received.
        /// </summary>
        public event RequestLogHandler OnLogAuthorizeStart
        {

            add
            {
                _EMPServer.OnLogAuthorizeStart += value;
            }

            remove
            {
                _EMPServer.OnLogAuthorizeStart -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a authorize start response was sent.
        /// </summary>
        public event AccessLogHandler OnLogAuthorizeStarted
        {

            add
            {
                _EMPServer.OnLogAuthorizeStarted += value;
            }

            remove
            {
                _EMPServer.OnLogAuthorizeStarted -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a authorize start command was received.
        /// </summary>
        public event OnAuthorizeStartDelegate OnAuthorizeStart
        {

            add
            {
                _EMPServer.OnAuthorizeStart += value;
            }

            remove
            {
                _EMPServer.OnAuthorizeStart -= value;
            }

        }

        #endregion

        #region OnAuthorizeStop

        /// <summary>
        /// An event sent whenever a authorize stop command was received.
        /// </summary>
        public event RequestLogHandler OnLogAuthorizeStop
        {

            add
            {
                _EMPServer.OnLogAuthorizeStop += value;
            }

            remove
            {
                _EMPServer.OnLogAuthorizeStop -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a authorize stop response was sent.
        /// </summary>
        public event AccessLogHandler OnLogAuthorizeStopped
        {

            add
            {
                _EMPServer.OnLogAuthorizeStopped += value;
            }

            remove
            {
                _EMPServer.OnLogAuthorizeStopped -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a authorize stop command was received.
        /// </summary>
        public event OnAuthorizeStopDelegate  OnAuthorizeStop
        {

            add
            {
                _EMPServer.OnAuthorizeStop += value;
            }

            remove
            {
                _EMPServer.OnAuthorizeStop -= value;
            }

        }

        #endregion

        #region OnChargeDetailRecord

        /// <summary>
        /// An event sent whenever a charge detail record was received.
        /// </summary>
        public event RequestLogHandler OnLogChargeDetailRecordSend
        {

            add
            {
                _EMPServer.OnLogChargeDetailRecordSend += value;
            }

            remove
            {
                _EMPServer.OnLogChargeDetailRecordSend -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a charge detail record response was sent.
        /// </summary>
        public event AccessLogHandler OnLogChargeDetailRecordSent
        {

            add
            {
                _EMPServer.OnLogChargeDetailRecordSent += value;
            }

            remove
            {
                _EMPServer.OnLogChargeDetailRecordSent -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a charge detail record was received.
        /// </summary>
        public event OnChargeDetailRecordDelegate OnChargeDetailRecord
        {

            add
            {
                _EMPServer.OnChargeDetailRecord += value;
            }

            remove
            {
                _EMPServer.OnChargeDetailRecord -= value;
            }

        }

        #endregion

        #endregion

        #region Constructor(s)

        #region EMPRoaming(EMPClient, EMPServer, Context = DefaultLoggingContext, LogFileCreator = null)

        /// <summary>
        /// Create a new OICP roaming client for EMPs.
        /// </summary>
        /// <param name="EMPClient">A EMP client.</param>
        /// <param name="EMPServer">A EMP sever.</param>
        /// <param name="Context">A context of this API.</param>
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
        public EMPRoaming(EMPClient                     EMPClient,
                          EMPServer                     EMPServer,
                          String                        Context         = DefaultLoggingContext,
                          Func<String, String, String>  LogFileCreator  = null)
        {

            this._EMPClient        = EMPClient;
            this._EMPServer        = EMPServer;
            this._EMPServerLogger  = new EMPServerLogger(_EMPServer, Context, LogFileCreator);

        }

        #endregion

        #region EMPRoaming(ClientId, RemoteHostname, RemoteTCPPort = null, RemoteHTTPVirtualHost = null, ... )

        /// <summary>
        /// Create a new OICP roaming client for EMPs.
        /// </summary>
        /// <param name="ClientId">A unqiue identification of this client.</param>
        /// <param name="RemoteHostname">The hostname of the remote OICP service.</param>
        /// <param name="RemoteTCPPort">An optional TCP port of the remote OICP service.</param>
        /// <param name="RemoteHTTPVirtualHost">An optional HTTP virtual hostname of the remote OICP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="QueryTimeout">An optional timeout for upstream queries.</param>
        /// 
        /// <param name="ServerName">An optional identification string for the HTTP server.</param>
        /// <param name="ServerTCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="ServerURIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="ServerAutoStart">Whether to start the server immediately or not.</param>
        /// 
        /// <param name="Context">A context of this API.</param>
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// 
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public EMPRoaming(String                               ClientId,
                          String                               RemoteHostname,
                          IPPort                               RemoteTCPPort               = null,
                          String                               RemoteHTTPVirtualHost       = null,
                          RemoteCertificateValidationCallback  RemoteCertificateValidator  = null,
                          String                               HTTPUserAgent               = EMPClient.DefaultHTTPUserAgent,
                          TimeSpan?                            QueryTimeout                = null,

                          String                               ServerName                  = EMPServer.DefaultHTTPServerName,
                          IPPort                               ServerTCPPort               = null,
                          String                               ServerURIPrefix             = "",
                          Boolean                              ServerAutoStart             = false,

                          String                               Context                     = DefaultLoggingContext,
                          Func<String, String, String>         LogFileCreator              = null,

                          DNSClient                            DNSClient                   = null)

            : this(new EMPClient(ClientId,
                                 RemoteHostname,
                                 RemoteTCPPort,
                                 RemoteHTTPVirtualHost,
                                 RemoteCertificateValidator,
                                 HTTPUserAgent,
                                 QueryTimeout,
                                 DNSClient),

                   new EMPServer(ServerName,
                                 ServerTCPPort,
                                 ServerURIPrefix,
                                 DNSClient,
                                 false),

                   Context,
                   LogFileCreator)

        {

            if (ServerAutoStart)
                Start();

        }

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

            var result = await _EMPClient.PullEVSEData(ProviderId,
                                                       SearchCenter,
                                                       DistanceKM,
                                                       LastCall,
                                                       QueryTimeout);

            //ToDo: Process the HTTP!
            return result.Content;

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

            var result = await _EMPClient.PullEVSEStatus(ProviderId,
                                                         SearchCenter,
                                                         DistanceKM,
                                                         EVSEStatusFilter,
                                                         QueryTimeout);

            //ToDo: Process the HTTP!
            return result.Content;

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

            var result = await _EMPClient.PullEVSEStatusById(ProviderId,
                                                             EVSEIds,
                                                             QueryTimeout);

            //ToDo: Process the HTTP!
            return result.Content;

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

            var result = await _EMPClient.SearchEVSE(ProviderId,
                                                     SearchCenter,
                                                     DistanceKM,
                                                     Address,
                                                     Plug,
                                                     ChargingFacility,
                                                     QueryTimeout);

            //ToDo: Process the HTTP!
            return result.Content;

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

            var result = await _EMPClient.PushAuthenticationData(ProviderAuthenticationDataRecords,
                                                                 OICPAction,
                                                                 QueryTimeout);

            //ToDo: Process the HTTP!
            return result.Content;

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

            var result = await _EMPClient.PushAuthenticationData(AuthorizationIdentifications,
                                                                 ProviderId,
                                                                 OICPAction,
                                                                 QueryTimeout);

            //ToDo: Process the HTTP!
            return result.Content;

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

            var result = await _EMPClient.GetChargeDetailRecords(ProviderId,
                                                                 From,
                                                                 To,
                                                                 QueryTimeout);

            //ToDo: Process the HTTP!
            return result.Content;

        }

        #endregion



        #region Start()

        public void Start()
        {
            _EMPServer.Start();
        }

        #endregion

        #region Shutdown(Message = null, Wait = true)

        public void Shutdown(String Message = null, Boolean Wait = true)
        {
            _EMPServer.Shutdown(Message, Wait);
        }

        #endregion


    }

}
