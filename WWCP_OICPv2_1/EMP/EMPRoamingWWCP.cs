/*
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
using System.Linq;
using System.Threading;
using System.Net.Security;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// A WWCP wrapper for the OICP v2.1 EMP Roaming Client which maps
    /// WWCP data structures onto OICP data structures and vice versa.
    /// </summary>
    public class EMPRoamingWWCP : AEMPRoamingProvider
    {

        #region Data

        private readonly EVSEDataRecord2EVSEDelegate _EVSEDataRecord2EVSE;

        /// <summary>
        ///  The default reservation time.
        /// </summary>
        public static readonly TimeSpan DefaultReservationTime = TimeSpan.FromMinutes(15);

        #endregion

        #region Properties

        #region EMPRoaming

        private readonly EMPRoaming _EMPRoaming;

        /// <summary>
        /// The wrapped EMP roaming object.
        /// </summary>
        public EMPRoaming EMPRoaming
            => _EMPRoaming;

        #endregion

        #region EMPClient

        /// <summary>
        /// The EMP client.
        /// </summary>
        public EMPClient EMPClient
            => _EMPRoaming?.EMPClient;

        #endregion

        #region EMPServer

        /// <summary>
        /// The EMP server.
        /// </summary>
        public EMPServer EMPServer
            => _EMPRoaming?.EMPServer;

        #endregion

        #region ClientLogger

        /// <summary>
        /// The EMP client logger.
        /// </summary>
        public EMPClientLogger ClientLogger
            => _EMPRoaming?.EMPClientLogger;

        #endregion

        #region ServerLogger

        /// <summary>
        /// The EMP server logger.
        /// </summary>
        public EMPServerLogger ServerLogger
            => _EMPRoaming?.EMPServerLogger;

        #endregion

        #region DNSClient

        /// <summary>
        /// The DNSc server.
        /// </summary>
        public DNSClient DNSClient
            => _EMPRoaming?.DNSClient;

        #endregion

        #endregion

        #region Events

        #region OnAuthorizeStart

        /// <summary>
        /// An event sent whenever a authorize start command was received.
        /// </summary>
        public event RequestLogHandler OnLogAuthorizeStart
        {

            add
            {
                _EMPRoaming.OnLogAuthorizeStart += value;
            }

            remove
            {
                _EMPRoaming.OnLogAuthorizeStart -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a authorize start response was sent.
        /// </summary>
        public event AccessLogHandler OnLogAuthorizeStarted
        {

            add
            {
                _EMPRoaming.OnLogAuthorizeStarted += value;
            }

            remove
            {
                _EMPRoaming.OnLogAuthorizeStarted -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a authorize start command was received.
        /// </summary>
        public override event OnAuthorizeStartEVSEDelegate OnAuthorizeStartEVSE;

        #endregion

        #region OnAuthorizeStop

        /// <summary>
        /// An event sent whenever a authorize stop command was received.
        /// </summary>
        public event RequestLogHandler OnLogAuthorizeStop
        {

            add
            {
                _EMPRoaming.OnLogAuthorizeStop += value;
            }

            remove
            {
                _EMPRoaming.OnLogAuthorizeStop -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a authorize stop response was sent.
        /// </summary>
        public event AccessLogHandler OnLogAuthorizeStopped
        {

            add
            {
                _EMPRoaming.OnLogAuthorizeStopped += value;
            }

            remove
            {
                _EMPRoaming.OnLogAuthorizeStopped -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a authorize stop command was received.
        /// </summary>
        public override event OnAuthorizeStopEVSEDelegate OnAuthorizeStopEVSE;

        #endregion

        #region OnChargeDetailRecord

        /// <summary>
        /// An event sent whenever a charge detail record was received.
        /// </summary>
        public event RequestLogHandler OnLogChargeDetailRecordSend
        {

            add
            {
                _EMPRoaming.OnLogChargeDetailRecordSend += value;
            }

            remove
            {
                _EMPRoaming.OnLogChargeDetailRecordSend -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a charge detail record response was sent.
        /// </summary>
        public event AccessLogHandler OnLogChargeDetailRecordSent
        {

            add
            {
                _EMPRoaming.OnLogChargeDetailRecordSent += value;
            }

            remove
            {
                _EMPRoaming.OnLogChargeDetailRecordSent -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a charge detail record was received.
        /// </summary>
        public override event WWCP.OnChargeDetailRecordDelegate OnChargeDetailRecord;

        #endregion

        #endregion

        #region Constructor(s)

        #region EMPRoamingWWCP(Id, Name, RoamingNetwork, EMPRoaming, EVSEDataRecord2EVSE = null)

        /// <summary>
        /// Create a new WWCP wrapper for the OICP roaming client for e-mobility providers/EMPs.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// 
        /// <param name="EMPRoaming">A OICP EMP roaming object to be mapped to WWCP.</param>
        /// <param name="EVSEDataRecord2EVSE">A delegate to process an EVSE data record after receiving it from the roaming provider.</param>
        public EMPRoamingWWCP(RoamingProvider_Id           Id,
                              I18NString                   Name,
                              RoamingNetwork               RoamingNetwork,

                              EMPRoaming                   EMPRoaming,
                              EVSEDataRecord2EVSEDelegate  EVSEDataRecord2EVSE = null)

            : base(Id,
                   Name,
                   RoamingNetwork)

        {

            #region Initial checks

            if (Id             == null)
                throw new ArgumentNullException(nameof(Id),              "The given unique roaming provider identification must not be null or empty!");

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Name),            "The given roaming provider name must not be null or empty!");

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork),  "The given roaming network must not be null!");

            if (EMPRoaming     == null)
                throw new ArgumentNullException("EMPRoaming",      "The given OICP EMP Roaming object must not be null!");

            #endregion

            this._EMPRoaming           = EMPRoaming;
            this._EVSEDataRecord2EVSE  = EVSEDataRecord2EVSE;

            // Link AuthorizeStart/-Stop and CDR events
            this._EMPRoaming.OnAuthorizeStart     += SendAuthorizeStart;
            this._EMPRoaming.OnAuthorizeStop      += SendAuthorizeStop;
            this._EMPRoaming.OnChargeDetailRecord += SendChargeDetailRecord;

        }

        #endregion

        #region EMPRoamingWWCP(Id, Name, RoamingNetwork, EMPClient, EMPServer, Context = EMPRoaming.DefaultLoggingContext, LogFileCreator = null)

        /// <summary>
        /// Create a new WWCP wrapper for the OICP roaming client for e-mobility providers/EMPs.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// 
        /// <param name="EMPClient">An OICP EMP client.</param>
        /// <param name="EMPServer">An OICP EMP sever.</param>
        /// <param name="ClientLoggingContext">An optional context for logging client methods.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// 
        /// <param name="EVSEDataRecord2EVSE">A delegate to process an EVSE data record after receiving it from the roaming provider.</param>
        public EMPRoamingWWCP(RoamingProvider_Id            Id,
                              I18NString                    Name,
                              RoamingNetwork                RoamingNetwork,

                              EMPClient                     EMPClient,
                              EMPServer                     EMPServer,
                              String                        ClientLoggingContext  = EMPClientLogger.DefaultContext,
                              String                        ServerLoggingContext  = EMPServerLogger.DefaultContext,
                              Func<String, String, String>  LogFileCreator        = null,

                              EVSEDataRecord2EVSEDelegate   EVSEDataRecord2EVSE   = null)

            : this(Id,
                   Name,
                   RoamingNetwork,

                   new EMPRoaming(EMPClient,
                                  EMPServer,
                                  ClientLoggingContext,
                                  ServerLoggingContext,
                                  LogFileCreator),

                   EVSEDataRecord2EVSE)

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
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
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
        /// <param name="EVSEDataRecord2EVSE">A delegate to process an EVSE data record after receiving it from the roaming provider.</param>
        /// 
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public EMPRoamingWWCP(RoamingProvider_Id                   Id,
                              I18NString                           Name,
                              RoamingNetwork                       RoamingNetwork,

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

                              String                               ClientLoggingContext        = EMPClientLogger.DefaultContext,
                              String                               ServerLoggingContext        = EMPServerLogger.DefaultContext,
                              Func<String, String, String>         LogFileCreator              = null,

                              EVSEDataRecord2EVSEDelegate          EVSEDataRecord2EVSE         = null,

                              DNSClient                            DNSClient                   = null)

            : this(Id,
                   Name,
                   RoamingNetwork,

                   new EMPRoaming(Id.ToString(),
                                  RemoteHostname,
                                  RemoteTCPPort,
                                  RemoteHTTPVirtualHost,
                                  RemoteCertificateValidator,
                                  HTTPUserAgent,
                                  QueryTimeout,

                                  ServerName,
                                  ServerTCPPort,
                                  ServerURIPrefix,
                                  ServerAutoStart,

                                  ClientLoggingContext,
                                  ServerLoggingContext,
                                  LogFileCreator,

                                  DNSClient),

                   EVSEDataRecord2EVSE)

        { }

        #endregion

        #endregion


        #region (private) SendAuthorizeStart(...)

        private async Task<AuthStartEVSEResult>

            SendAuthorizeStart(DateTime            Timestamp,
                               EMPServer           Sender,
                               CancellationToken   CancellationToken,
                               EventTracking_Id    EventTrackingId,
                               EVSEOperator_Id     OperatorId,
                               Auth_Token          AuthToken,
                               EVSE_Id             EVSEId            = null,
                               ChargingSession_Id  SessionId         = null,
                               ChargingProduct_Id  PartnerProductId  = null,
                               ChargingSession_Id  PartnerSessionId  = null,
                               TimeSpan?           QueryTimeout      = null)

        {

            var OnAuthorizeStartLocal = OnAuthorizeStartEVSE;
            if (OnAuthorizeStartLocal != null)
            {

                return await OnAuthorizeStartLocal(Timestamp,
                                                   CancellationToken,
                                                   EventTrackingId,
                                                   OperatorId,
                                                   AuthToken,
                                                   EVSEId,
                                                   PartnerProductId,
                                                   SessionId,
                                                   QueryTimeout);

            }

            else
                return AuthStartEVSEResult.OutOfService(AuthorizatorId);

        }

        #endregion

        #region (private) SendAuthorizeStop(...)

        private async Task<AuthStopEVSEResult>

            SendAuthorizeStop(DateTime            Timestamp,
                              EMPServer           Sender,
                              CancellationToken   CancellationToken,
                              EventTracking_Id    EventTrackingId,
                              ChargingSession_Id  SessionId,
                              ChargingSession_Id  PartnerSessionId,
                              EVSEOperator_Id     OperatorId,
                              EVSE_Id             EVSEId,
                              Auth_Token          AuthToken,
                              TimeSpan?           QueryTimeout  = null)

        {

            var OnAuthorizeStopEVSELocal = OnAuthorizeStopEVSE;
            if (OnAuthorizeStopEVSELocal != null)
            {

                return await OnAuthorizeStopEVSELocal(Timestamp,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      OperatorId,
                                                      EVSEId,
                                                      SessionId,
                                                      AuthToken,
                                                      QueryTimeout);

            }

            else
                return AuthStopEVSEResult.OutOfService(AuthorizatorId);

        }

        #endregion

        #region (private) SendChargeDetailRecord(...)

        private async Task<SendCDRResult>

            SendChargeDetailRecord(DateTime                    Timestamp,
                                   CancellationToken           CancellationToken,
                                   EventTracking_Id            EventTrackingId,
                                   ChargeDetailRecord  ChargeDetailRecord,
                                   TimeSpan?                   QueryTimeout = null)

        {

            var OnChargeDetailRecordLocal = OnChargeDetailRecord;
            if (OnChargeDetailRecordLocal != null)
            {

                return await OnChargeDetailRecordLocal(Timestamp,
                                                       CancellationToken,
                                                       EventTrackingId,
                                                       ChargeDetailRecord.AsWWCPChargeDetailRecord(),
                                                       QueryTimeout);

            }

            else
                return SendCDRResult.OutOfService(AuthorizatorId);
        }

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
        public async Task<IEnumerable<ChargeDetailRecord>>

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


        #region ReservationStart(...EVSEId, ChargingProductId = null, ReservationId = null, SessionId = null, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at the given EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp of this request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSEId">The unique identification of the EVSE to be reserved.</param>
        /// <param name="StartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
        /// <param name="ChargingProductId">An optional unique identification of the charging product to be reserved.</param>
        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public override async Task<ReservationResult>

            Reserve(DateTime                 Timestamp,
                    CancellationToken        CancellationToken,
                    EventTracking_Id         EventTrackingId,
                    EVSE_Id                  EVSEId,
                    DateTime?                StartTime          = null,
                    TimeSpan?                Duration           = null,
                    ChargingReservation_Id   ReservationId      = null,
                    EVSP_Id                  ProviderId         = null,
                    eMA_Id                   eMAId              = null,
                    ChargingProduct_Id       ChargingProductId  = null,
                    IEnumerable<Auth_Token>  AuthTokens         = null,
                    IEnumerable<eMA_Id>      eMAIds             = null,
                    IEnumerable<UInt32>      PINs               = null,
                    TimeSpan?                QueryTimeout       = null)

        {

            #region Check if the PartnerProductId has a special format like 'D=15min|P=AC1'

            var PartnerProductIdElements = new Dictionary<String, String>();

            if (ChargingProductId.ToString().IsNotNullOrEmpty() &&
               !ChargingProductId.ToString().Contains('='))
            {
                PartnerProductIdElements.Add("P", ChargingProductId.ToString());
                ChargingProductId = null;
            }

            if (ChargingProductId != null)
                ChargingProductId.ToString().DoubleSplitInto('|', '=', PartnerProductIdElements);

            #endregion

            #region Copy the 'StartTime' value into the PartnerProductId

            if (StartTime.HasValue)
            {

                if (!PartnerProductIdElements.ContainsKey("S"))
                    PartnerProductIdElements.Add("S", StartTime.Value.ToIso8601());
                else
                    PartnerProductIdElements["S"] = StartTime.Value.ToIso8601();

            }

            #endregion

            #region Copy the 'Duration' value into the PartnerProductId

            if (Duration.HasValue && Duration.Value >= TimeSpan.FromSeconds(1))
            {

                if (Duration.Value.Minutes > 0 && Duration.Value.Seconds == 0)
                {
                    if (!PartnerProductIdElements.ContainsKey("D"))
                        PartnerProductIdElements.Add("D", Duration.Value.TotalMinutes + "min");
                    else
                        PartnerProductIdElements["D"] = Duration.Value.TotalMinutes + "min";
                }

                else
                {
                    if (!PartnerProductIdElements.ContainsKey("D"))
                        PartnerProductIdElements.Add("D", Duration.Value.TotalSeconds + "sec");
                    else
                        PartnerProductIdElements["D"] = Duration.Value.TotalSeconds + "sec";
                }

            }

            #endregion

            #region Add the eMAId to the list of valid eMAIds

            if (eMAIds == null && eMAId != null)
                eMAIds = new List<eMA_Id>() { eMAId };

            if (eMAIds != null && !eMAIds.Contains(eMAId))
            {
                var _eMAIds = new List<eMA_Id>(eMAIds);
                _eMAIds.Add(eMAId);
                eMAIds = _eMAIds;
            }

            #endregion


            var result = await _EMPRoaming.ReservationStart(Timestamp:          Timestamp,
                                                            CancellationToken:  CancellationToken,
                                                            EventTrackingId:    EventTrackingId,
                                                            EVSEId:             EVSEId,
                                                            ProviderId:         ProviderId,
                                                            eMAId:              eMAId,
                                                            SessionId:          ReservationId != null ? ChargingSession_Id.Parse(ReservationId.ToString()) : null,
                                                            PartnerSessionId:   null,
                                                            PartnerProductId:   ChargingProduct_Id.Parse(PartnerProductIdElements.
                                                                                                             Select(kvp => kvp.Key + "=" + kvp.Value).
                                                                                                             AggregateWith("|")),
                                                            QueryTimeout:       QueryTimeout);


            if (result.HTTPStatusCode == HTTPStatusCode.OK)
            {

                if (result.Content != null && result.Content.Result)
                {

                    return ReservationResult.Success(result.Content.SessionId != null
                                                         ? new ChargingReservation(ReservationId:           ChargingReservation_Id.Parse(result.Content.SessionId.ToString()),
                                                                                   Timestamp:               DateTime.Now,
                                                                                   StartTime:               DateTime.Now,
                                                                                   Duration:                Duration.HasValue ? Duration.Value : DefaultReservationTime,
                                                                                   EndTime:                 DateTime.Now + (Duration.HasValue ? Duration.Value : DefaultReservationTime),
                                                                                   ConsumedReservationTime: TimeSpan.FromSeconds(0),
                                                                                   ReservationLevel:        ChargingReservationLevel.EVSE,
                                                                                   ProviderId:              ProviderId,
                                                                                   eMAId:                   eMAId,
                                                                                   RoamingNetwork:          RoamingNetwork,
                                                                                   ChargingPoolId:          null,
                                                                                   ChargingStationId:       null,
                                                                                   EVSEId:                  EVSEId,
                                                                                   ChargingProductId:       ChargingProductId,
                                                                                   AuthTokens:              AuthTokens,
                                                                                   eMAIds:                  eMAIds,
                                                                                   PINs:                    PINs)
                                                         : null);

                }

            }

            else
            {
                return ReservationResult.Error();
            }

            return ReservationResult.Error();

        }

        #endregion

        #region CancelReservation(...ReservationId, Reason, ProviderId = null, EVSEId = null, ...)

        /// <summary>
        /// Try to remove the given charging reservation.
        /// </summary>
        /// <param name="Timestamp">The timestamp of this request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="ReservationId">The unique charging reservation identification.</param>
        /// <param name="Reason">A reason for this cancellation.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="EVSEId">An optional identification of the EVSE.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public override async Task<CancelReservationResult>

            CancelReservation(DateTime                               Timestamp,
                              CancellationToken                      CancellationToken,
                              EventTracking_Id                       EventTrackingId,
                              ChargingReservation_Id                 ReservationId,
                              ChargingReservationCancellationReason  Reason,
                              EVSP_Id                                ProviderId    = null,
                              EVSE_Id                                EVSEId        = null,
                              TimeSpan?                              QueryTimeout  = null)

        {

            var result = await _EMPRoaming.ReservationStop(Timestamp:          Timestamp,
                                                           CancellationToken:  CancellationToken,
                                                           EventTrackingId:    EventTrackingId,
                                                           SessionId:          ChargingSession_Id.Parse(ReservationId.ToString()),
                                                           ProviderId:         ProviderId,
                                                           EVSEId:             EVSEId,
                                                           PartnerSessionId:   null,
                                                           QueryTimeout:       QueryTimeout);

            if (result.HTTPStatusCode == HTTPStatusCode.OK)
            {

                if (result.Content != null && result.Content.Result)
                    return CancelReservationResult.Success(ReservationId);

            }

            else
            {
                return CancelReservationResult.Error();
            }

            return CancelReservationResult.Error();

        }

        #endregion


        #region RemoteStart(...EVSEId, ChargingProductId = null, ReservationId = null, SessionId = null, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSEId">The unique identification of the EVSE to be started remotely.</param>
        /// <param name="ChargingProductId">An optional identification of the charging product to use.</param>
        /// <param name="Duration">An optional maximum time span to charge. When it is reached, the charging process will stop automatically.</param>
        /// <param name="MaxEnergy">An optional maximum amount of energy to charge. When it is reached, the charging process will stop automatically.</param>
        /// <param name="ReservationId">An optional identification of a charging reservation.</param>
        /// <param name="SessionId">An optional identification of this charging session.</param>
        /// <param name="ProviderId">An optional identification of the e-mobility service provider, whenever this identification is different from the current message sender.</param>
        /// <param name="eMAId">An optional identification of the e-mobility account who wants to charge.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public override async Task<RemoteStartEVSEResult>

            RemoteStart(DateTime                Timestamp,
                        CancellationToken       CancellationToken,
                        EventTracking_Id        EventTrackingId,
                        EVSE_Id                 EVSEId,
                        ChargingProduct_Id      ChargingProductId  = null,
//                      TimeSpan?               Duration           = null,
//                      Double?                 MaxEnergy          = null,
                        ChargingReservation_Id  ReservationId      = null,
                        ChargingSession_Id      SessionId          = null,
                        EVSP_Id                 ProviderId         = null,
                        eMA_Id                  eMAId              = null,
                        TimeSpan?               QueryTimeout       = default(TimeSpan?))

        {

            #region Check if the PartnerProductId has a special format like 'R=12345-1234...|P=AC1'

            var PartnerProductIdElements = new Dictionary<String, String>();

            if (ChargingProductId.ToString().IsNotNullOrEmpty() &&
               !ChargingProductId.ToString().Contains('='))
            {
                PartnerProductIdElements.Add("P", ChargingProductId.ToString());
                ChargingProductId = null;
            }

            if (ChargingProductId != null)
                ChargingProductId.ToString().DoubleSplitInto('|', '=', PartnerProductIdElements);

            #endregion

            #region Copy the 'Duration' value into the PartnerProductId

            //if (Duration.HasValue && Duration.Value >= TimeSpan.FromSeconds(1))
            //{
            //
            //    if (Duration.Value.Minutes > 0 && Duration.Value.Seconds == 0)
            //    {
            //        if (!PartnerProductIdElements.ContainsKey("D"))
            //            PartnerProductIdElements.Add("D", Duration.Value.TotalMinutes + "min");
            //        else
            //            PartnerProductIdElements["D"] = Duration.Value.TotalMinutes + "min";
            //    }
            //
            //    else
            //    {
            //        if (!PartnerProductIdElements.ContainsKey("D"))
            //            PartnerProductIdElements.Add("D", Duration.Value.TotalSeconds + "sec");
            //        else
            //            PartnerProductIdElements["D"] = Duration.Value.TotalSeconds + "sec";
            //    }
            //
            //}

            #endregion

            #region Copy the 'MaxEnergy' value into the PartnerProductId

            //if (MaxEnergy.HasValue && MaxEnergy.Value > 0))
            //{
            //
            //    if (Duration.Value.Minutes > 0 && Duration.Value.Seconds == 0)
            //    {
            //        if (!PartnerProductIdElements.ContainsKey("D"))
            //            PartnerProductIdElements.Add("D", Duration.Value.TotalMinutes + "min");
            //        else
            //            PartnerProductIdElements["D"] = Duration.Value.TotalMinutes + "min";
            //    }
            //
            //    else
            //    {
            //        if (!PartnerProductIdElements.ContainsKey("D"))
            //            PartnerProductIdElements.Add("D", Duration.Value.TotalSeconds + "sec");
            //        else
            //            PartnerProductIdElements["D"] = Duration.Value.TotalSeconds + "sec";
            //    }
            //
            //}

            #endregion

            #region Copy the 'ReservationId' value into the PartnerProductId

            if (ReservationId != null)
            {

                if (!PartnerProductIdElements.ContainsKey("R"))
                    PartnerProductIdElements.Add("R", ReservationId.ToString());
                else
                    PartnerProductIdElements["R"] = ReservationId.ToString();

            }

            #endregion


            var result = await _EMPRoaming.RemoteStart(Timestamp:          Timestamp,
                                                       CancellationToken:  CancellationToken,
                                                       EventTrackingId:    EventTrackingId,
                                                       EVSEId:             EVSEId,
                                                       ProviderId:         ProviderId,
                                                       eMAId:              eMAId,
                                                       SessionId:          SessionId,
                                                       PartnerSessionId:   null,
                                                       PartnerProductId:   ChargingProduct_Id.Parse(PartnerProductIdElements.
                                                                                                        Select(kvp => kvp.Key + "=" + kvp.Value).
                                                                                                        AggregateWith("|")),
                                                       QueryTimeout:       QueryTimeout);

            if (result.HTTPStatusCode == HTTPStatusCode.OK)
            {

                if (result.Content != null && result.Content.Result)
                    return RemoteStartEVSEResult.Success(result.Content.SessionId != null
                                                             ? new ChargingSession(result.Content.SessionId)
                                                             : null);

            }

            else
            {
                return RemoteStartEVSEResult.Error();
            }

            return RemoteStartEVSEResult.Error();

        }

        #endregion

        #region RemoteStop(...EVSEId, SessionId, ReservationHandling, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Stop the given charging session at the given EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSEId">The unique identification of the EVSE to be stopped.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public override async Task<RemoteStopEVSEResult>

            RemoteStop(DateTime             Timestamp,
                       CancellationToken    CancellationToken,
                       EventTracking_Id     EventTrackingId,
                       EVSE_Id              EVSEId,
                       ChargingSession_Id   SessionId,
                       ReservationHandling  ReservationHandling,
                       EVSP_Id              ProviderId    = null,
                       eMA_Id               eMAId         = null,
                       TimeSpan?            QueryTimeout  = null)

        {

            var result = await _EMPRoaming.RemoteStop(Timestamp:          Timestamp,
                                                      CancellationToken:  CancellationToken,
                                                      EventTrackingId:    EventTrackingId,
                                                      SessionId:          SessionId,
                                                      ProviderId:         ProviderId,
                                                      EVSEId:             EVSEId,
                                                      PartnerSessionId:   null,
                                                      QueryTimeout:       QueryTimeout);

            if (result.HTTPStatusCode == HTTPStatusCode.OK)
            {

                if (result.Content != null && result.Content.Result)
                    return RemoteStopEVSEResult.Success(SessionId);

            }

            else
            {
                return RemoteStopEVSEResult.Error(SessionId);
            }

            return RemoteStopEVSEResult.Error(SessionId);

        }

        #endregion


    }

}
