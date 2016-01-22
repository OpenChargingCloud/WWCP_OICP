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
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using System.Threading;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
{

    /// <summary>
    /// A WWCP wrapper for the OICP roaming client for EVSE operators/CPOs.
    /// </summary>
    public class CPORoamingWWCP : IOperatorRoamingService
    {

        #region Data

        private readonly RoamingNetwork               _RoamingNetwork;

        private readonly Authorizator_Id              _AuthorizatorId;

        private readonly EVSE2EVSEDataRecordDelegate  _EVSE2EVSEDataRecord;

        private readonly EVSEDataRecord2XMLDelegate   _EVSEDataRecord2XML;

        #endregion

        #region Properties

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


        #region CPORoaming

        private readonly CPORoaming _CPORoaming;

        /// <summary>
        /// The wrapped CPO roaming object.
        /// </summary>
        public CPORoaming CPORoaming
        {
            get
            {
                return _CPORoaming;
            }
        }

        #endregion

        #region CPOClient

        /// <summary>
        /// The CPO client.
        /// </summary>
        public CPOClient CPOClient
        {
            get
            {
                return _CPORoaming.CPOClient;
            }
        }

        #endregion

        #region CPOServer

        /// <summary>
        /// The CPO server.
        /// </summary>
        public CPOServer CPOServer
        {
            get
            {
                return _CPORoaming.CPOServer;
            }
        }

        #endregion

        #region CPOServerLogger

        /// <summary>
        /// The CPO server logger.
        /// </summary>
        public CPOServerLogger CPOServerLogger
        {
            get
            {
                return _CPORoaming.CPOServerLogger;
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
                return _CPORoaming.DNSClient;
            }
        }

        #endregion

        #endregion

        #region Events

        #region OnEVSEDataPush/-Pushed

        /// <summary>
        /// An event fired whenever new EVSE data will be send upstream.
        /// </summary>
        public event WWCP.OnEVSEDataPushDelegate OnEVSEDataPush;

        /// <summary>
        /// An event fired whenever new EVSE data had been sent upstream.
        /// </summary>
        public event WWCP.OnEVSEDataPushedDelegate OnEVSEDataPushed;

        #endregion

        #region OnEVSEStatusPush/-Pushed

        /// <summary>
        /// An event fired whenever new EVSE status will be send upstream.
        /// </summary>
        public event WWCP.OnEVSEStatusPushDelegate OnEVSEStatusPush;

        /// <summary>
        /// An event fired whenever new EVSE status had been sent upstream.
        /// </summary>
        public event WWCP.OnEVSEStatusPushedDelegate OnEVSEStatusPushed;

        #endregion

        #endregion

        #region Constructor(s)

        #region CPORoamingWWCP(Id, Name, RoamingNetwork, CPORoaming, EVSE2EVSEDataRecord = null)

        /// <summary>
        /// Create a new WWCP wrapper for the OICP roaming client for EVSE operators/CPOs.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// <param name="CPORoaming">A OICP CPO roaming object to be mapped to WWCP.</param>
        /// <param name="EVSE2EVSEDataRecord">A delegate to process an EVSE data record, e.g. before pushing it to the roaming provider.</param>
        /// <param name="EVSEDataRecord2XML">A delegate to process the XML representation of an EVSE data record, e.g. before pushing it to the roaming provider.</param>
        public CPORoamingWWCP(RoamingProvider_Id           Id,
                              I18NString                   Name,
                              RoamingNetwork               RoamingNetwork,
                              CPORoaming                   CPORoaming,
                              EVSE2EVSEDataRecordDelegate  EVSE2EVSEDataRecord  = null,
                              EVSEDataRecord2XMLDelegate   EVSEDataRecord2XML   = null)

        {

            #region Initial checks

            if (Id             == null)
                throw new ArgumentNullException("Id",              "The given unique roaming provider identification must not be null or empty!");

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException("Name",            "The given roaming provider name must not be null or empty!");

            if (RoamingNetwork == null)
                throw new ArgumentNullException("RoamingNetwork",  "The given roaming network must not be null!");

            if (CPORoaming     == null)
                throw new ArgumentNullException("CPORoaming",      "The given OICP CPO Roaming object must not be null!");

            #endregion

            this._Id                        = Id;
            this._Name                      = Name;
            this._RoamingNetwork            = RoamingNetwork;
            this._CPORoaming                = CPORoaming;
            this._AuthorizatorId            = Authorizator_Id.Parse(Id.ToString());
            this._EVSE2EVSEDataRecord       = EVSE2EVSEDataRecord;
            this._EVSEDataRecord2XML        = EVSEDataRecord2XML;

            #region Link RemoteStart/-Stop events

            this._CPORoaming.OnRemoteStart += (Timestamp,
                                               Sender,
                                               CancellationToken,
                                               EventTrackingId,
                                               EVSEId,
                                               ChargingProductId,
                                               SessionId,
                                               PartnerSessionId,
                                               ProviderId,
                                               eMAId,
                                               QueryTimeout)  => _RoamingNetwork.RemoteStart(Timestamp,
                                                                                             //Sender,
                                                                                             CancellationToken,
                                                                                             EventTrackingId,
                                                                                             EVSEId,
                                                                                             ChargingProductId,
                                                                                             null,
                                                                                             SessionId,
                                                                                             //PartnerSessionId,
                                                                                             ProviderId,
                                                                                             eMAId,
                                                                                             QueryTimeout);

            this._CPORoaming.OnRemoteStop += (Timestamp,
                                              Sender,
                                              CancellationToken,
                                              EventTrackingId,
                                              EVSEId,
                                              SessionId,
                                              PartnerSessionId,
                                              ProviderId,
                                              QueryTimeout)  => _RoamingNetwork.RemoteStop(Timestamp,
                                                                                           CancellationToken,
                                                                                           EventTrackingId,
                                                                                           EVSEId,
                                                                                           SessionId,
                                                                                           ReservationHandling.Close,
                                                                                           ProviderId,
                                                                                           QueryTimeout);

            #endregion

        }

        #endregion

        #region CPORoamingWWCP(Id, Name, RoamingNetwork, CPOClient, CPOServer, EVSEDataRecordProcessing = null)

        /// <summary>
        /// Create a new WWCP wrapper for the OICP roaming client for EVSE operators/CPOs.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// <param name="CPOClient">An OICP CPO client.</param>
        /// <param name="CPOServer">An OICP CPO sever.</param>
        /// <param name="EVSE2EVSEDataRecord">A delegate to process an EVSE data record, e.g. before pushing it to the roaming provider.</param>
        /// <param name="EVSEDataRecord2XML">A delegate to process the XML representation of an EVSE data record, e.g. before pushing it to the roaming provider.</param>
        public CPORoamingWWCP(RoamingProvider_Id           Id,
                              I18NString                   Name,
                              RoamingNetwork               RoamingNetwork,
                              CPOClient                    CPOClient,
                              CPOServer                    CPOServer,
                              EVSE2EVSEDataRecordDelegate  EVSE2EVSEDataRecord  = null,
                              EVSEDataRecord2XMLDelegate   EVSEDataRecord2XML   = null)

            : this(Id,
                   Name,
                   RoamingNetwork,
                   new CPORoaming(CPOClient,
                                  CPOServer),
                   EVSE2EVSEDataRecord,
                   EVSEDataRecord2XML)

        { }

        #endregion

        #region CPORoamingWWCP(Id, Name, RoamingNetwork, RemoteHostName, ...)

        /// <summary>
        /// Create a new WWCP wrapper for the OICP roaming client for EVSE operators/CPOs.
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
        /// <param name="EVSE2EVSEDataRecord">A delegate to process an EVSE data record, e.g. before pushing it to the roaming provider.</param>
        /// <param name="EVSEDataRecord2XML">A delegate to process the XML representation of an EVSE data record, e.g. before pushing it to the roaming provider.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public CPORoamingWWCP(RoamingProvider_Id           Id,
                              I18NString                   Name,
                              RoamingNetwork               RoamingNetwork,

                              String                       RemoteHostname,
                              IPPort                       RemoteTCPPort          = null,
                              String                       RemoteHTTPVirtualHost  = null,
                              String                       HTTPUserAgent          = CPOClient.DefaultHTTPUserAgent,
                              TimeSpan?                    QueryTimeout           = null,

                              String                       ServerName             = CPOServer.DefaultHTTPServerName,
                              IPPort                       ServerTCPPort          = null,
                              String                       ServerURIPrefix        = "",
                              Boolean                      ServerAutoStart        = false,

                              EVSE2EVSEDataRecordDelegate  EVSE2EVSEDataRecord    = null,
                              EVSEDataRecord2XMLDelegate   EVSEDataRecord2XML     = null,
                              DNSClient                    DNSClient              = null)

            : this(Id,
                   Name,
                   RoamingNetwork,
                   new CPORoaming(Id.ToString(),
                                  RemoteHostname,
                                  RemoteTCPPort,
                                  RemoteHTTPVirtualHost,
                                  HTTPUserAgent,
                                  QueryTimeout,

                                  ServerName,
                                  ServerTCPPort,
                                  ServerURIPrefix,
                                  ServerAutoStart,

                                  DNSClient),
                   EVSE2EVSEDataRecord,
                   EVSEDataRecord2XML)

        { }

        #endregion

        #endregion


        #region PushEVSEData...

        #region PushEVSEData(GroupedEVSEs,     ActionType = fullLoad, OperatorId = null, OperatorName = null,                      QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE data of the given lookup of EVSEs grouped by their EVSE operator.
        /// </summary>
        /// <param name="GroupedEVSEs">A lookup of EVSEs grouped by their EVSE operator.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public async Task<Acknowledgement>

            PushEVSEData(ILookup<EVSEOperator, EVSE>  GroupedEVSEs,
                         WWCP.ActionType              ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id              OperatorId    = null,
                         String                       OperatorName  = null,
                         TimeSpan?                    QueryTimeout  = null)

        {

            #region Initial checks

            if (GroupedEVSEs == null)
                throw new ArgumentNullException("GroupedEVSEs", "The given lookup of EVSEs must not be null!");

            #endregion

            #region Get effective number of EVSE data records to upload

            Acknowledgement Acknowledgement = null;

            var NumberOfEVSEs = GroupedEVSEs.
                                    Select(group => group.Count()).
                                    Sum   ();

            var StartTime = DateTime.Now;

            #endregion


            if (NumberOfEVSEs > 0)
            {

                #region Send OnEVSEDataPush event

                var OnEVSEDataPushLocal = OnEVSEDataPush;
                if (OnEVSEDataPushLocal != null)
                    OnEVSEDataPushLocal(StartTime, this, this.Id.ToString(), this.RoamingNetworkId, ActionType, GroupedEVSEs, (UInt32) NumberOfEVSEs);

                #endregion

                var result = await _CPORoaming.PushEVSEData(GroupedEVSEs.
                                                                SelectMany(group => group).
                                                                ToLookup  (evse  => evse.Operator,
                                                                           evse  => evse.AsOICPEVSEDataRecord(_EVSE2EVSEDataRecord)),
                                                            ActionType.AsOICPActionType(),
                                                            OperatorId,
                                                            OperatorName,
                                                            QueryTimeout);

                if (result.Result == true)
                    Acknowledgement = new Acknowledgement(true);

                else
                    Acknowledgement = new Acknowledgement(false, result.StatusCode.Description);

            }

            else
                Acknowledgement = new Acknowledgement(true);


            #region Send OnEVSEDataPushed event

            var EndTime = DateTime.Now;

            var OnEVSEDataPushedLocal = OnEVSEDataPushed;
            if (OnEVSEDataPushedLocal != null)
                OnEVSEDataPushedLocal(EndTime, this, this.Id.ToString(), this.RoamingNetworkId, ActionType, GroupedEVSEs, (UInt32) NumberOfEVSEs, Acknowledgement, EndTime - StartTime);

            #endregion

            return Acknowledgement;

        }

        #endregion

        #region PushEVSEData(EVSE,             ActionType = insert,   OperatorId = null, OperatorName = null,                      QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE data of the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public async Task<Acknowledgement>

            PushEVSEData(EVSE                 EVSE,
                         WWCP.ActionType      ActionType    = WWCP.ActionType.insert,
                         EVSEOperator_Id      OperatorId    = null,
                         String               OperatorName  = null,
                         TimeSpan?            QueryTimeout  = null)

        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException("EVSE", "The given EVSE must not be null!");

            #endregion

            return await PushEVSEData(new EVSE[] { EVSE },
                                      ActionType,
                                      OperatorId,
                                      OperatorName.IsNotNullOrEmpty()
                                          ? OperatorName
                                          : EVSE.Operator.Name.Any()
                                                ? EVSE.Operator.Name.FirstText
                                                : null,
                                      null,
                                      QueryTimeout);

        }

        #endregion

        #region PushEVSEData(EVSEs,            ActionType = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE data of the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public async Task<Acknowledgement>

            PushEVSEData(IEnumerable<EVSE>    EVSEs,
                         WWCP.ActionType      ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id      OperatorId    = null,
                         String               OperatorName  = null,
                         Func<EVSE, Boolean>  IncludeEVSEs  = null,
                         TimeSpan?            QueryTimeout  = null)

        {

            #region Initial checks

            if (EVSEs == null)
                throw new ArgumentNullException("EVSEs", "The given enumeration of EVSEs must not be null!");

            if (IncludeEVSEs == null)
                IncludeEVSEs = EVSE => true;

            #endregion

            #region Get effective number of EVSE status to upload

            var _EVSEs = EVSEs.
                             Where(evse => IncludeEVSEs(evse)).
                             ToArray();

            #endregion


            if (_EVSEs.Any())
                return await PushEVSEData(_EVSEs.ToLookup(evse => evse.Operator,
                                                          evse => evse),
                                          ActionType,
                                          OperatorId,
                                          OperatorName,
                                          QueryTimeout);

            return new Acknowledgement(true);

        }

        #endregion

        #region PushEVSEData(ChargingStation,  ActionType = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE data of the given charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public async Task<Acknowledgement>

            PushEVSEData(ChargingStation      ChargingStation,
                         WWCP.ActionType      ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id      OperatorId    = null,
                         String               OperatorName  = null,
                         Func<EVSE, Boolean>  IncludeEVSEs  = null,
                         TimeSpan?            QueryTimeout  = null)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException("ChargingStation", "The given charging station must not be null!");

            #endregion

            return await PushEVSEData(ChargingStation.EVSEs,
                                      ActionType,
                                      OperatorId   != null ? OperatorId   : ChargingStation.ChargingPool.EVSEOperator.Id,
                                      OperatorName != null ? OperatorName : ChargingStation.ChargingPool.EVSEOperator.Name.FirstText,
                                      IncludeEVSEs,
                                      QueryTimeout);

        }

        #endregion

        #region PushEVSEData(ChargingStations, ActionType = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE data of the given charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public async Task<Acknowledgement>

            PushEVSEData(IEnumerable<ChargingStation>  ChargingStations,
                         WWCP.ActionType               ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id               OperatorId    = null,
                         String                        OperatorName  = null,
                         Func<EVSE, Boolean>           IncludeEVSEs  = null,
                         TimeSpan?                     QueryTimeout  = null)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException("ChargingStations", "The given enumeration of charging stations must not be null!");

            #endregion

            return await PushEVSEData(ChargingStations.SelectMany(station => station.EVSEs),
                                      ActionType,
                                      OperatorId,
                                      OperatorName,
                                      IncludeEVSEs,
                                      QueryTimeout);

        }

        #endregion

        #region PushEVSEData(ChargingPool,     ActionType = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE data of the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public async Task<Acknowledgement>

            PushEVSEData(ChargingPool         ChargingPool,
                         WWCP.ActionType      ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id      OperatorId    = null,
                         String               OperatorName  = null,
                         Func<EVSE, Boolean>  IncludeEVSEs  = null,
                         TimeSpan?            QueryTimeout  = null)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException("ChargingPool", "The given charging pool must not be null!");

            #endregion

            return await PushEVSEData(ChargingPool.EVSEs,
                                      ActionType,
                                      OperatorId   != null ? OperatorId   : ChargingPool.EVSEOperator.Id,
                                      OperatorName != null ? OperatorName : ChargingPool.EVSEOperator.Name.FirstText,
                                      IncludeEVSEs,
                                      QueryTimeout);

        }

        #endregion

        #region PushEVSEData(ChargingPools,    ActionType = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE data of the given charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public async Task<Acknowledgement>

            PushEVSEData(IEnumerable<ChargingPool>  ChargingPools,
                         WWCP.ActionType            ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id            OperatorId    = null,
                         String                     OperatorName  = null,
                         Func<EVSE, Boolean>        IncludeEVSEs  = null,
                         TimeSpan?                  QueryTimeout  = null)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException("ChargingPools", "The given enumeration of charging pools must not be null!");

            #endregion

            return await PushEVSEData(ChargingPools.SelectMany(pool    => pool.ChargingStations).
                                                    SelectMany(station => station.EVSEs),
                                      ActionType,
                                      OperatorId,
                                      OperatorName,
                                      IncludeEVSEs,
                                      QueryTimeout);

        }

        #endregion

        #region PushEVSEData(EVSEOperator,     ActionType = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE data of the given EVSE operator.
        /// </summary>
        /// <param name="EVSEOperator">An EVSE operator.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public async Task<Acknowledgement>

            PushEVSEData(EVSEOperator         EVSEOperator,
                         WWCP.ActionType      ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id      OperatorId    = null,
                         String               OperatorName  = null,
                         Func<EVSE, Boolean>  IncludeEVSEs  = null,
                         TimeSpan?            QueryTimeout  = null)

        {

            #region Initial checks

            if (EVSEOperator == null)
                throw new ArgumentNullException("EVSEOperator", "The given EVSE operator must not be null!");

            #endregion

            return await PushEVSEData(new EVSEOperator[] { EVSEOperator },
                                      ActionType,
                                      OperatorId,
                                      OperatorName,
                                      IncludeEVSEs,
                                      QueryTimeout);

        }

        #endregion

        #region PushEVSEData(EVSEOperators,    ActionType = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE data of the given EVSE operators.
        /// </summary>
        /// <param name="EVSEOperators">An enumeration of EVSE operators.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId"></param>
        /// <param name="OperatorName">An optional alternative EVSE operator name used for uploading all EVSEs.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        /// <returns></returns>
        public async Task<Acknowledgement>

            PushEVSEData(IEnumerable<EVSEOperator>             EVSEOperators,
                         WWCP.ActionType                       ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id                       OperatorId    = null,
                         String                                OperatorName  = null,
                         Func<EVSE, Boolean>                   IncludeEVSEs  = null,
                         TimeSpan?                             QueryTimeout  = null)

        {

            #region Initial checks

            if (EVSEOperators == null)
                throw new ArgumentNullException("EVSEOperators",  "The given enumeration of EVSE operators must not be null!");

            #endregion

            return await PushEVSEData(EVSEOperators.SelectMany(evseoperator => evseoperator.ChargingPools).
                                                    SelectMany(pool         => pool.ChargingStations).
                                                    SelectMany(station      => station.EVSEs),
                                      ActionType,
                                      OperatorId,
                                      OperatorName,
                                      IncludeEVSEs,
                                      QueryTimeout);

        }

        #endregion

        #region PushEVSEData(RoamingNetwork,   ActionType = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE data of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public async Task<Acknowledgement>

            PushEVSEData(RoamingNetwork       RoamingNetwork,
                         WWCP.ActionType      ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id      OperatorId    = null,
                         String               OperatorName  = null,
                         Func<EVSE, Boolean>  IncludeEVSEs  = null,
                         TimeSpan?            QueryTimeout  = null)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException("RoamingNetwork", "The given roaming network must not be null!");

            #endregion

            return await PushEVSEData(RoamingNetwork.EVSEs,
                                      ActionType,
                                      OperatorId,
                                      OperatorName,
                                      IncludeEVSEs,
                                      QueryTimeout);

        }

        #endregion

        #endregion

        #region PushEVSEStatus...

        #region PushEVSEStatus(GroupedEVSEs,     ActionType = update, OperatorId = null, OperatorName = null,                      QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE status of the given lookup of EVSE status types grouped by their EVSE operator.
        /// </summary>
        /// <param name="GroupedEVSEs">A lookup of EVSEs grouped by their EVSE operator.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public async Task<Acknowledgement>

            PushEVSEStatus(ILookup<EVSEOperator, EVSE>  GroupedEVSEs,
                           WWCP.ActionType              ActionType    = WWCP.ActionType.update,
                           EVSEOperator_Id              OperatorId    = null,
                           String                       OperatorName  = null,
                           TimeSpan?                    QueryTimeout  = null)

        {

            #region Initial checks

            if (GroupedEVSEs == null)
                throw new ArgumentNullException("GroupedEVSEStatusTypes", "The given lookup of EVSE status types must not be null!");

            #endregion

            #region Get effective number of EVSE status to upload

            Acknowledgement Acknowledgement = null;

            var NumberOfEVSEStatus = GroupedEVSEs.
                                         Select(group => group.Count()).
                                         Sum();

            var StartTime = DateTime.Now;

            #endregion


            if (NumberOfEVSEStatus > 0)
            {

                #region Send OnEVSEStatusPush event

                var OnEVSEStatusPushLocal = OnEVSEStatusPush;
                if (OnEVSEStatusPushLocal != null)
                    OnEVSEStatusPushLocal(StartTime, this, this.Id.ToString(), this.RoamingNetworkId, ActionType, GroupedEVSEs, (UInt32) NumberOfEVSEStatus);

                #endregion

                var result = await _CPORoaming.PushEVSEStatus(GroupedEVSEs.
                                                                  SelectMany(group => group).
                                                                  ToLookup  (evse  => evse.Operator.Id,
                                                                             evse  => new EVSEStatusRecord(evse.Id, evse.Status.Value.AsOICPEVSEStatus())),
                                                              ActionType.AsOICPActionType(),
                                                              OperatorId,
                                                              OperatorName,
                                                              QueryTimeout);

                if (result.Result == true)
                    Acknowledgement = new Acknowledgement(true);

                else
                    Acknowledgement = new Acknowledgement(false, result.StatusCode.Description +
                                                                 Environment.NewLine +
                                                                 result.StatusCode.AdditionalInfo);

            }

            else
                Acknowledgement = new Acknowledgement(true);


            #region Send OnEVSEStatusPushed event

            var EndTime = DateTime.Now;

            var OnEVSEStatusPushedLocal = OnEVSEStatusPushed;
            if (OnEVSEStatusPushedLocal != null)
                OnEVSEStatusPushedLocal(EndTime, this, this.Id.ToString(), this.RoamingNetworkId, ActionType, GroupedEVSEs, (UInt32) NumberOfEVSEStatus, Acknowledgement, EndTime - StartTime);

            #endregion

            return Acknowledgement;

        }

        #endregion

        #region PushEVSEStatus(EVSE,             ActionType = update, OperatorId = null, OperatorName = null,                      QueryTimeout = null)

        /// <summary>
        /// Upload the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public async Task<Acknowledgement>

            PushEVSEStatus(EVSE                 EVSE,
                           WWCP.ActionType      ActionType    = WWCP.ActionType.update,
                           EVSEOperator_Id      OperatorId    = null,
                           String               OperatorName  = null,
                           TimeSpan?            QueryTimeout  = null)

        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException("EVSE", "The given EVSE must not be null!");

            #endregion

            return await PushEVSEStatus(new EVSE[] { EVSE },
                                        ActionType,
                                        OperatorId,
                                        OperatorName,
                                        null,
                                        QueryTimeout);

        }

        #endregion

        #region PushEVSEStatus(EVSEs,            ActionType = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload the status of the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public async Task<Acknowledgement>

            PushEVSEStatus(IEnumerable<EVSE>    EVSEs,
                           WWCP.ActionType      ActionType    = WWCP.ActionType.update,
                           EVSEOperator_Id      OperatorId    = null,
                           String               OperatorName  = null,
                           Func<EVSE, Boolean>  IncludeEVSEs  = null,
                           TimeSpan?            QueryTimeout  = null)

        {

            #region Initial checks

            if (EVSEs == null)
                throw new ArgumentNullException("EVSEs", "The given enumeration of EVSEs must not be null!");

            if (IncludeEVSEs == null)
                IncludeEVSEs = EVSE => true;

            #endregion

            #region Get effective number of EVSE status to upload

            var _EVSEs = EVSEs.
                             Where(evse => IncludeEVSEs(evse)).
                             ToArray();

            #endregion


            if (_EVSEs.Any())
                return await PushEVSEStatus(_EVSEs.ToLookup(evse => evse.Operator,
                                                            evse => evse),
                                            ActionType,
                                            OperatorId,
                                            OperatorName,
                                            QueryTimeout);

            return new Acknowledgement(true);

        }

        #endregion

        #region PushEVSEStatus(ChargingStation,  ActionType = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE status of the given charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public async Task<Acknowledgement>

            PushEVSEStatus(ChargingStation      ChargingStation,
                           WWCP.ActionType      ActionType    = WWCP.ActionType.update,
                           EVSEOperator_Id      OperatorId    = null,
                           String               OperatorName  = null,
                           Func<EVSE, Boolean>  IncludeEVSEs  = null,
                           TimeSpan?            QueryTimeout  = null)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException("ChargingStation", "The given charging station must not be null!");

            #endregion

            return await PushEVSEStatus(ChargingStation.EVSEs,
                                        ActionType,
                                        OperatorId   != null ? OperatorId   : ChargingStation.ChargingPool.EVSEOperator.Id,
                                        OperatorName != null ? OperatorName : ChargingStation.ChargingPool.EVSEOperator.Name.FirstText,
                                        IncludeEVSEs,
                                        QueryTimeout);

        }

        #endregion

        #region PushEVSEStatus(ChargingStations, ActionType = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE status of the given charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public async Task<Acknowledgement>

            PushEVSEStatus(IEnumerable<ChargingStation>  ChargingStations,
                           WWCP.ActionType               ActionType    = WWCP.ActionType.update,
                           EVSEOperator_Id               OperatorId    = null,
                           String                        OperatorName  = null,
                           Func<EVSE, Boolean>           IncludeEVSEs  = null,
                           TimeSpan?                     QueryTimeout  = null)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException("ChargingStations", "The given enumeration of charging stations must not be null!");

            #endregion

            return await PushEVSEStatus(ChargingStations.SelectMany(station => station.EVSEs),
                                        ActionType,
                                        OperatorId,
                                        OperatorName,
                                        IncludeEVSEs,
                                        QueryTimeout);

        }

        #endregion

        #region PushEVSEStatus(ChargingPool,     ActionType = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE status of the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public async Task<Acknowledgement>

            PushEVSEStatus(ChargingPool         ChargingPool,
                           WWCP.ActionType      ActionType    = WWCP.ActionType.update,
                           EVSEOperator_Id      OperatorId    = null,
                           String               OperatorName  = null,
                           Func<EVSE, Boolean>  IncludeEVSEs  = null,
                           TimeSpan?            QueryTimeout  = null)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException("ChargingPool", "The given charging pool must not be null!");

            #endregion

            return await PushEVSEStatus(ChargingPool.EVSEs,
                                        ActionType,
                                        OperatorId   != null ? OperatorId   : ChargingPool.EVSEOperator.Id,
                                        OperatorName != null ? OperatorName : ChargingPool.EVSEOperator.Name.FirstText,
                                        IncludeEVSEs,
                                        QueryTimeout);

        }

        #endregion

        #region PushEVSEStatus(ChargingPools,    ActionType = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE status of the given charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public async Task<Acknowledgement>

            PushEVSEStatus(IEnumerable<ChargingPool>  ChargingPools,
                           WWCP.ActionType            ActionType    = WWCP.ActionType.update,
                           EVSEOperator_Id            OperatorId    = null,
                           String                     OperatorName  = null,
                           Func<EVSE, Boolean>        IncludeEVSEs  = null,
                           TimeSpan?                  QueryTimeout  = null)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException("ChargingPools", "The given enumeration of charging pools must not be null!");

            #endregion

            return await PushEVSEStatus(ChargingPools.SelectMany(pool    => pool.ChargingStations).
                                                      SelectMany(station => station.EVSEs),
                                        ActionType,
                                        OperatorId,
                                        OperatorName,
                                        IncludeEVSEs,
                                        QueryTimeout);

        }

        #endregion

        #region PushEVSEStatus(EVSEOperator,     ActionType = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE status of the given EVSE operator.
        /// </summary>
        /// <param name="EVSEOperator">An EVSE operator.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public async Task<Acknowledgement>

            PushEVSEStatus(EVSEOperator         EVSEOperator,
                           WWCP.ActionType      ActionType    = WWCP.ActionType.update,
                           EVSEOperator_Id      OperatorId    = null,
                           String               OperatorName  = null,
                           Func<EVSE, Boolean>  IncludeEVSEs  = null,
                           TimeSpan?            QueryTimeout  = null)

        {

            #region Initial checks

            if (EVSEOperator == null)
                throw new ArgumentNullException("EVSEOperator", "The given EVSE operator must not be null!");

            #endregion

            return await PushEVSEStatus(EVSEOperator.AllEVSEs,
                                        ActionType,
                                        EVSEOperator.Id,
                                        OperatorName.IsNotNullOrEmpty()
                                            ? OperatorName
                                            : EVSEOperator.Name.FirstText,
                                        IncludeEVSEs,
                                        QueryTimeout);

        }

        #endregion

        #region PushEVSEStatus(EVSEOperators,    ActionType = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE status of the given EVSE operators.
        /// </summary>
        /// <param name="EVSEOperators">An enumeration of EVSES operators.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public async Task<Acknowledgement>

            PushEVSEStatus(IEnumerable<EVSEOperator>  EVSEOperators,
                           WWCP.ActionType            ActionType    = WWCP.ActionType.update,
                           EVSEOperator_Id            OperatorId    = null,
                           String                     OperatorName  = null,
                           Func<EVSE, Boolean>        IncludeEVSEs  = null,
                           TimeSpan?                  QueryTimeout  = null)

        {

            #region Initial checks

            if (EVSEOperators == null)
                throw new ArgumentNullException("EVSEOperator", "The given enumeration of EVSE operators must not be null!");

            #endregion

            return await PushEVSEStatus(EVSEOperators.SelectMany(evseoperator => evseoperator.ChargingPools).
                                                      SelectMany(pool         => pool.ChargingStations).
                                                      SelectMany(station      => station.EVSEs),
                                        ActionType,
                                        OperatorId,
                                        OperatorName,
                                        IncludeEVSEs,
                                        QueryTimeout);

        }

        #endregion

        #region PushEVSEStatus(RoamingNetwork,   ActionType = update, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload the EVSE status of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public async Task<Acknowledgement>

            PushEVSEStatus(RoamingNetwork       RoamingNetwork,
                           WWCP.ActionType      ActionType    = WWCP.ActionType.update,
                           EVSEOperator_Id      OperatorId    = null,
                           String               OperatorName  = null,
                           Func<EVSE, Boolean>  IncludeEVSEs  = null,
                           TimeSpan?            QueryTimeout  = null)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException("RoamingNetwork", "The given roaming network must not be null!");

            #endregion

            return await PushEVSEStatus(RoamingNetwork.EVSEs,
                                        ActionType,
                                        OperatorId,
                                        OperatorName,
                                        IncludeEVSEs,
                                        QueryTimeout);

        }

        #endregion

        #region PushEVSEStatus(EVSEStatusDiff, QueryTimeout = null)

        /// <summary>
        /// Send EVSE status updates.
        /// </summary>
        /// <param name="EVSEStatusDiff">An EVSE status diff.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task PushEVSEStatus(EVSEStatusDiff  EVSEStatusDiff,
                                         TimeSpan?       QueryTimeout  = null)

        {

            await _CPORoaming.PushEVSEStatus(EVSEStatusDiff,
                                             QueryTimeout);

        }

        #endregion

        #endregion

        #region AuthorizeStart/-Stop...

        #region AuthorizeStart(OperatorId, AuthToken, ChargingProductId = null, SessionId = null, QueryTimeout = null)

        /// <summary>
        /// Create an OICP v2.0 AuthorizeStart request.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<AuthStartResult>

            AuthorizeStart(DateTime            Timestamp,
                           CancellationToken   CancellationToken,
                           EventTracking_Id    EventTrackingId,
                           EVSEOperator_Id     OperatorId,
                           Auth_Token          AuthToken,
                           ChargingProduct_Id  ChargingProductId  = null,
                           ChargingSession_Id  SessionId          = null,
                           TimeSpan?           QueryTimeout       = null)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The given EVSE operator identification must not be null!");

            if (AuthToken == null)
                throw new ArgumentNullException("AuthToken",  "The given authentication token must not be null!");

            #endregion

            var result = await _CPORoaming.AuthorizeStart(OperatorId,
                                                          AuthToken,
                                                          null,
                                                          SessionId,
                                                          ChargingProductId,
                                                          null,
                                                          QueryTimeout);

            if (result.AuthorizationStatus == AuthorizationStatusType.Authorized)
                return AuthStartResult.Authorized(_AuthorizatorId,
                                                  result.SessionId,
                                                  result.ProviderId,
                                                  result.StatusCode.Description,
                                                  result.StatusCode.AdditionalInfo);

            return AuthStartResult.NotAuthorized(_AuthorizatorId,
                                                 result.ProviderId,
                                                 result.StatusCode.Description,
                                                 result.StatusCode.AdditionalInfo);

        }

        #endregion

        #region AuthorizeStart(OperatorId, AuthToken, EVSEId, ChargingProductId = null, SessionId = null, QueryTimeout = null)

        /// <summary>
        /// Create an OICP v2.0 AuthorizeStart request at the given EVSE.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<AuthStartEVSEResult>

            AuthorizeStart(DateTime            Timestamp,
                           CancellationToken   CancellationToken,
                           EventTracking_Id    EventTrackingId,
                           EVSEOperator_Id     OperatorId,
                           Auth_Token          AuthToken,
                           EVSE_Id             EVSEId,
                           ChargingProduct_Id  ChargingProductId  = null,   // [maxlength: 100]
                           ChargingSession_Id  SessionId          = null,
                           TimeSpan?           QueryTimeout       = null)

        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The given EVSE operator identification must not be null!");

            if (AuthToken == null)
                throw new ArgumentNullException("AuthToken",  "The given authentication token must not be null!");

            if (EVSEId    == null)
                throw new ArgumentNullException("EVSEId",     "The given EVSE identification must not be null!");

            #endregion

            var result  = await _CPORoaming.AuthorizeStart(OperatorId,
                                                           AuthToken,
                                                           EVSEId,
                                                           SessionId,
                                                           ChargingProductId,
                                                           null,
                                                           QueryTimeout);

            if (result.AuthorizationStatus == AuthorizationStatusType.Authorized)
                return AuthStartEVSEResult.Authorized(_AuthorizatorId,
                                                      result.SessionId,
                                                      result.ProviderId,
                                                      result.StatusCode.Description,
                                                      result.StatusCode.AdditionalInfo);

            return AuthStartEVSEResult.NotAuthorized(_AuthorizatorId,
                                                     result.ProviderId,
                                                     result.StatusCode.Description,
                                                     result.StatusCode.AdditionalInfo);

        }

        #endregion

        #region AuthorizeStart(OperatorId, AuthToken, ChargingStationId, ChargingProductId = null, SessionId = null, QueryTimeout = null)

        /// <summary>
        /// Create an OICP v2.0 AuthorizeStart request at the given charging station.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingStationId">The unique identification of a charging station.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<AuthStartChargingStationResult>

            AuthorizeStart(DateTime            Timestamp,
                           CancellationToken   CancellationToken,
                           EventTracking_Id    EventTrackingId,
                           EVSEOperator_Id     OperatorId,
                           Auth_Token          AuthToken,
                           ChargingStation_Id  ChargingStationId,
                           ChargingProduct_Id  ChargingProductId  = null,   // [maxlength: 100]
                           ChargingSession_Id  SessionId          = null,
                           TimeSpan?           QueryTimeout       = null)

        {

            #region Initial checks

            if (OperatorId        == null)
                throw new ArgumentNullException("OperatorId",         "The given EVSE operator identification must not be null!");

            if (AuthToken         == null)
                throw new ArgumentNullException("AuthToken",          "The given authentication token must not be null!");

            if (ChargingStationId == null)
                throw new ArgumentNullException("ChargingStationId",  "The given charging station identification must not be null!");

            #endregion

            //ToDo: Implement AuthorizeStart(...ChargingStationId...)
            return AuthStartChargingStationResult.Error(_AuthorizatorId, "Not implemented!");

        }

        #endregion


        #region AuthorizeStop(OperatorId, SessionId, AuthToken, QueryTimeout = null)

        // UID => Not everybody can stop any session, but maybe another
        //        UID than the UID which started the session!
        //        (e.g. car sharing)

        /// <summary>
        /// Create an OICP v2.0 AuthorizeStop request.
        /// </summary>
        /// <param name="OperatorId">An EVSE Operator identification.</param>
        /// <param name="SessionId">The OICP session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<AuthStopResult>

            AuthorizeStop(DateTime            Timestamp,
                          CancellationToken   CancellationToken,
                          EventTracking_Id    EventTrackingId,
                          EVSEOperator_Id     OperatorId,
                          ChargingSession_Id  SessionId,
                          Auth_Token          AuthToken,
                          TimeSpan?           QueryTimeout  = null)
        {

            var result  = await _CPORoaming.AuthorizeStop(OperatorId,
                                                          SessionId,
                                                          AuthToken,
                                                          null,
                                                          null,
                                                          QueryTimeout);

            if (result.AuthorizationStatus == AuthorizationStatusType.Authorized)
                return AuthStopResult.Authorized(_AuthorizatorId,
                                                 result.ProviderId,
                                                 result.StatusCode.Description,
                                                 result.StatusCode.AdditionalInfo);

            return AuthStopResult.NotAuthorized(_AuthorizatorId,
                                                result.ProviderId,
                                                result.StatusCode.Description,
                                                result.StatusCode.AdditionalInfo);

        }

        #endregion

        #region AuthorizeStop(OperatorId, EVSEId, SessionId, AuthToken, QueryTimeout = null)

        // UID => Not everybody can stop any session, but maybe another
        //        UID than the UID which started the session!
        //        (e.g. car sharing)

        /// <summary>
        /// Create an OICP v2.0 AuthorizeStop request.
        /// </summary>
        /// <param name="OperatorId">An EVSE Operator identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="SessionId">The OICP session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<AuthStopEVSEResult>

            AuthorizeStop(DateTime            Timestamp,
                          CancellationToken   CancellationToken,
                          EventTracking_Id    EventTrackingId,
                          EVSEOperator_Id     OperatorId,
                          EVSE_Id             EVSEId,
                          ChargingSession_Id  SessionId,
                          Auth_Token          AuthToken,
                          TimeSpan?           QueryTimeout  = null)
        {

            var result  = await _CPORoaming.AuthorizeStop(OperatorId,
                                                          SessionId,
                                                          AuthToken,
                                                          EVSEId,
                                                          null,
                                                          QueryTimeout);

            if (result.AuthorizationStatus == AuthorizationStatusType.Authorized)
                return AuthStopEVSEResult.Authorized(_AuthorizatorId,
                                                     result.ProviderId,
                                                     result.StatusCode.Description,
                                                     result.StatusCode.AdditionalInfo);

            return AuthStopEVSEResult.NotAuthorized(_AuthorizatorId,
                                                    result.ProviderId,
                                                    result.StatusCode.Description,
                                                    result.StatusCode.AdditionalInfo);

        }

        #endregion

        #region AuthorizeStop(OperatorId, ChargingStationId, SessionId, AuthToken, QueryTimeout = null)

        /// <summary>
        /// Create an OICP v2.0 AuthorizeStop request.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="ChargingStationId">A charging station identification.</param>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<AuthStopChargingStationResult>

            AuthorizeStop(DateTime            Timestamp,
                          CancellationToken   CancellationToken,
                          EventTracking_Id    EventTrackingId,
                          EVSEOperator_Id     OperatorId,
                          ChargingStation_Id  ChargingStationId,
                          ChargingSession_Id  SessionId,
                          Auth_Token          AuthToken,
                          TimeSpan?           QueryTimeout  = null)

        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The given parameter must not be null!");

            if (SessionId  == null)
                throw new ArgumentNullException("SessionId",  "The given parameter must not be null!");

            if (AuthToken  == null)
                throw new ArgumentNullException("AuthToken",  "The given parameter must not be null!");

            #endregion

            return AuthStopChargingStationResult.Error(_AuthorizatorId);

        }

        #endregion

        #endregion

        #region SendChargeDetailRecord...

        #region SendChargeDetailRecord(ChargeDetailRecord, QueryTimeout = null)

        /// <summary>
        /// Create an OICP SendChargeDetailRecord request.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<SendCDRResult>

            SendChargeDetailRecord(DateTime            Timestamp,
                                   CancellationToken   CancellationToken,
                                   EventTracking_Id    EventTrackingId,
                                   ChargeDetailRecord  ChargeDetailRecord,
                                   TimeSpan?           QueryTimeout  = null)

        {

            #region Initial checks

            if (ChargeDetailRecord == null)
                throw new ArgumentNullException("ChargeDetailRecord", "The given parameter must not be null!");

            #endregion

            var result = await _CPORoaming.SendChargeDetailRecord(EVSEId:                ChargeDetailRecord.EVSE.Id,
                                                                  SessionId:             ChargeDetailRecord.SessionId,
                                                                  PartnerProductId:      ChargeDetailRecord.ChargingProductId,
                                                                  SessionStart:          ChargeDetailRecord.SessionTime.Value.StartTime,
                                                                  SessionEnd:            ChargeDetailRecord.SessionTime.Value.EndTime.Value,
                                                                  Identification:        new AuthorizationIdentification(ChargeDetailRecord.Identification),
                                                                  //PartnerSessionId:      ChargeDetailRecord.PartnerSessionId,
                                                                  ChargingStart:         ChargeDetailRecord.SessionTime.HasValue ? new Nullable<DateTime>(ChargeDetailRecord.SessionTime.Value.StartTime) : null,
                                                                  ChargingEnd:           ChargeDetailRecord.SessionTime.HasValue ?                        ChargeDetailRecord.SessionTime.Value.EndTime    : null,
                                                                  MeterValueStart:       ChargeDetailRecord.EnergyMeterValues != null && ChargeDetailRecord.EnergyMeterValues.Any() ? new Double?(ChargeDetailRecord.EnergyMeterValues.First().Value) : null,
                                                                  MeterValueEnd:         ChargeDetailRecord.EnergyMeterValues != null && ChargeDetailRecord.EnergyMeterValues.Any() ? new Double?(ChargeDetailRecord.EnergyMeterValues.Last(). Value) : null,
                                                                  MeterValuesInBetween:  ChargeDetailRecord.EnergyMeterValues != null && ChargeDetailRecord.EnergyMeterValues.Any() ? ChargeDetailRecord.EnergyMeterValues.Select(v => v.Value)       : null,
                                                                  ConsumedEnergy:        ChargeDetailRecord.ConsumedEnergy,
                                                                  MeteringSignature:     ChargeDetailRecord.MeteringSignature,
                                                                  QueryTimeout:          QueryTimeout);

            // true
            if (result.Result)
                return SendCDRResult.Forwarded(_AuthorizatorId);

            // false
            else
                return SendCDRResult.False(_AuthorizatorId);

        }

        #endregion

        #region SendChargeDetailRecord(EVSEId, SessionId, PartnerProductId, SessionStart, SessionEnd, Identification, ..., QueryTimeout = null)

        /// <summary>
        /// Create an OICP SendChargeDetailRecord request.
        /// </summary>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="SessionId">The OICP session identification from the Authorize Start request.</param>
        /// <param name="PartnerProductId">The ev charging product identification.</param>
        /// <param name="SessionStart">The session start timestamp.</param>
        /// <param name="SessionEnd">The session end timestamp.</param>
        /// <param name="Identification">An identification.</param>
        /// <param name="ChargingStart">An optional charging start timestamp.</param>
        /// <param name="ChargingEnd">An optional charging end timestamp.</param>
        /// <param name="MeterValueStart">An optional initial value of the energy meter.</param>
        /// <param name="MeterValueEnd">An optional final value of the energy meter.</param>
        /// <param name="MeterValuesInBetween">An optional enumeration of meter values during the charging session.</param>
        /// <param name="ConsumedEnergy">The optional amount of consumed energy.</param>
        /// <param name="MeteringSignature">An optional signature for the metering values.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<SendCDRResult>

            SendChargeDetailRecord(DateTime             Timestamp,
                                   CancellationToken    CancellationToken,
                                   EventTracking_Id     EventTrackingId,
                                   EVSE_Id              EVSEId,
                                   ChargingSession_Id   SessionId,
                                   ChargingProduct_Id   PartnerProductId,
                                   DateTime             SessionStart,
                                   DateTime             SessionEnd,
                                   AuthInfo             Identification,
                                   DateTime?            ChargingStart         = null,
                                   DateTime?            ChargingEnd           = null,
                                   Double?              MeterValueStart       = null,
                                   Double?              MeterValueEnd         = null,
                                   IEnumerable<Double>  MeterValuesInBetween  = null,
                                   Double?              ConsumedEnergy        = null,
                                   String               MeteringSignature     = null,
                                   TimeSpan?            QueryTimeout          = null)

        {

            var result = await _CPORoaming.SendChargeDetailRecord(EVSEId,
                                                                  SessionId,
                                                                  PartnerProductId,
                                                                  SessionStart,
                                                                  SessionEnd,
                                                                  new AuthorizationIdentification(Identification),
                                                                  null,
                                                                  ChargingStart,
                                                                  ChargingEnd,
                                                                  MeterValueStart,
                                                                  MeterValueEnd,
                                                                  MeterValuesInBetween,
                                                                  ConsumedEnergy,
                                                                  MeteringSignature,
                                                                  null,
                                                                  null,
                                                                  QueryTimeout);


            // true
            if (result.Result)
                return SendCDRResult.Forwarded(_AuthorizatorId);

            // false
            else
                return SendCDRResult.False(_AuthorizatorId);

        }

        #endregion

        #endregion


        #region PullAuthenticationData(OperatorId, QueryTimeout = null)

        /// <summary>
        /// Create an OICP v2.0 PullAuthenticationData request.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<eRoamingAuthenticationData> PullAuthenticationData(EVSEOperator_Id  OperatorId,
                                                                             TimeSpan?        QueryTimeout = null)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The given parameter must not be null!");

            #endregion

            return await _CPORoaming.PullAuthenticationData(OperatorId,
                                                            QueryTimeout);

        }

        #endregion


    }

}
