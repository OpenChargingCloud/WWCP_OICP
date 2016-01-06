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

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    /// <summary>
    /// A WWCP wrapper for the OICP roaming client for EVSE operators/CPOs.
    /// </summary>
    public class CPORoamingWWCP : IOperatorRoamingService
    {

        #region Data

        private readonly RoamingNetwork  _RoamingNetwork;

        private readonly Authorizator_Id _AuthorizatorId;

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

        #endregion

        #region Events


        #endregion

        #region Constructor(s)

        #region CPORoamingWWCP(Id, Name, RoamingNetwork, CPORoaming)

        /// <summary>
        /// Create a new WWCP wrapper for the OICP roaming client for EVSE operators/CPOs.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// <param name="CPORoaming">A OICP CPO roaming object to be mapped to WWCP.</param>
        public CPORoamingWWCP(RoamingProvider_Id  Id,
                              I18NString          Name,
                              RoamingNetwork      RoamingNetwork,
                              CPORoaming          CPORoaming)
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

            this._Id                 = Id;
            this._Name               = Name;
            this._RoamingNetwork     = RoamingNetwork;
            this._CPORoaming         = CPORoaming;
            this._AuthorizatorId     = Authorizator_Id.Parse(Id.ToString());

            #region Link RemoteStart/-Stop events

            this._CPORoaming.OnRemoteStart += (Timestamp,
                                               Sender,
                                               CancellationToken,
                                               EVSEId,
                                               ChargingProductId,
                                               SessionId,
                                               PartnerSessionId,
                                               ProviderId,
                                               eMAId)  => _RoamingNetwork.RemoteStart(Timestamp,
                                                                                      //Sender,
                                                                                      CancellationToken,
                                                                                      EVSEId,
                                                                                      ChargingProductId,
                                                                                      null,
                                                                                      SessionId,
                                                                                      //PartnerSessionId,
                                                                                      ProviderId,
                                                                                      eMAId);

            this._CPORoaming.OnRemoteStop += (Timestamp,
                                              Sender,
                                              CancellationToken,
                                              EVSEId,
                                              SessionId,
                                              PartnerSessionId,
                                              ProviderId)  => _RoamingNetwork.RemoteStop(Timestamp,
                                                                                         CancellationToken,
                                                                                         EVSEId,
                                                                                         SessionId,
                                                                                         ReservationHandling.Close,
                                                                                         ProviderId);

            #endregion

        }

        #endregion

        #region CPORoamingWWCP(Id, Name, RoamingNetwork, CPOClient, CPOServer)

        /// <summary>
        /// Create a new WWCP wrapper for the OICP roaming client for EVSE operators/CPOs.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// <param name="CPOClient">An OICP CPO client.</param>
        /// <param name="CPOServer">An OICP CPO sever.</param>
        public CPORoamingWWCP(RoamingProvider_Id  Id,
                              I18NString          Name,
                              RoamingNetwork      RoamingNetwork,
                              CPOClient           CPOClient,
                              CPOServer           CPOServer)

            : this(Id,
                   Name,
                   RoamingNetwork,
                   new CPORoaming(CPOClient,
                                  CPOServer))

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
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public CPORoamingWWCP(RoamingProvider_Id  Id,
                              I18NString          Name,
                              RoamingNetwork      RoamingNetwork,

                              String              RemoteHostname,
                              IPPort              RemoteTCPPort          = null,
                              String              RemoteHTTPVirtualHost  = null,
                              String              HTTPUserAgent          = CPOClient.DefaultHTTPUserAgent,
                              TimeSpan?           QueryTimeout           = null,

                              String              ServerName             = CPOServer.DefaultHTTPServerName,
                              IPPort              ServerTCPPort          = null,
                              String              ServerURIPrefix        = "",
                              Boolean             ServerAutoStart        = false,

                              DNSClient           DNSClient              = null)

            : this(Id,
                   Name,
                   RoamingNetwork,
                   new CPORoaming(RemoteHostname,
                                  RemoteTCPPort,
                                  RemoteHTTPVirtualHost,
                                  HTTPUserAgent,
                                  QueryTimeout,

                                  ServerName,
                                  ServerTCPPort,
                                  ServerURIPrefix,
                                  ServerAutoStart,

                                  DNSClient))

        { }

        #endregion

        #endregion


        #region PushEVSEData(GroupedData,      ActionType = fullLoad, OperatorId = null, OperatorName = null, QueryTimeout = null)

        /// <summary>
        /// Upload the given lookup of EVSEs grouped by their EVSE operator.
        /// </summary>
        /// <param name="GroupedData">A lookup of EVSEs grouped by their EVSE operator.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public async Task<Acknowledgement>

            PushEVSEData(ILookup<EVSEOperator, IEnumerable<EVSE>>  GroupedData,
                         WWCP.ActionType                           ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id                           OperatorId    = null,
                         String                                    OperatorName  = null,
                         TimeSpan?                                 QueryTimeout  = null)

        {

            #region Initial checks

            if (GroupedData == null)
                throw new ArgumentNullException("GroupedData", "The given parameter must not be null!");

            #endregion

            return new Acknowledgement(true);

        }

        #endregion

        #region PushEVSEData(EVSEOperator,     ActionType = fullLoad, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload the all filtered EVSE data of the given EVSE operator.
        /// </summary>
        /// <param name="EVSEOperator">An EVSE operator.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorName">An optional alternative EVSE operator name used for uploading.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<Acknowledgement>

            PushEVSEData(EVSEOperator         EVSEOperator,
                         WWCP.ActionType      ActionType    = WWCP.ActionType.fullLoad,
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
                                      EVSEOperator.Id,
                                      OperatorName.IsNotNullOrEmpty()
                                          ? OperatorName
                                          : EVSEOperator.Name.Any()
                                                ? EVSEOperator.Name.FirstText
                                                : null,
                                      IncludeEVSEs,
                                      QueryTimeout);

        }

        #endregion

        #region PushEVSEData(EVSEOperators,    ActionType = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Upload the all filtered EVSE data of all given EVSE operators.
        /// </summary>
        /// <param name="EVSEOperators">An enumeration of EVSE operators.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// <param name="OperatorId"></param>
        /// <param name="OperatorName">An optional alternative EVSE operator name used for uploading all EVSEs.</param>
        /// <param name="IncludeEVSEs">Only upload the EVSEs returned by the given filter delegate.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        /// <returns></returns>
        public async Task<Acknowledgement>

            PushEVSEData(IEnumerable<EVSEOperator>  EVSEOperators,
                         WWCP.ActionType            ActionType    = WWCP.ActionType.fullLoad,
                         EVSEOperator_Id            OperatorId    = null,
                         String                     OperatorName  = null,
                         Func<EVSE, Boolean>        IncludeEVSEs  = null,
                         TimeSpan?                  QueryTimeout  = null)

        {

            #region Initial checks

            if (EVSEOperators == null)
                throw new ArgumentNullException("EVSEOperators",  "The given enumeration of EVSE operators must not be null!");

            var _EVSEOperators = EVSEOperators.ToArray();

            if (!_EVSEOperators.Any())
                throw new ArgumentNullException("EVSEOperators",  "The given enumeration of EVSE operators must not be empty!");

            if (IncludeEVSEs == null)
                IncludeEVSEs = EVSEId => true;

            #endregion

            return await PushEVSEData(_EVSEOperators.ToLookup(evseoperator => evseoperator,
                                                              evseoperator => evseoperator.SelectMany(pool    => pool.ChargingStations).
                                                                                           SelectMany(station => station.EVSEs).
                                                                                           Where     (evse    => IncludeEVSEs(evse))),
                                      ActionType,
                                      OperatorId,
                                      OperatorName,
                                      QueryTimeout);

        }

        #endregion





        #region PushEVSEData(EVSEDataRecord, OICPAction = insert, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Create a new task pushing a single EVSE data record onto the OICP server.
        /// </summary>
        /// <param name="EVSEDataRecord">An EVSE data record.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// <param name="OperatorId">An optional EVSE operator Id to use. Otherwise it will be taken from the EVSE data record.</param>
        /// <param name="OperatorName">An optional EVSE operator name.</param>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSE data records before pushing them to the server.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<eRoamingAcknowledgement>

            PushEVSEData(EVSEDataRecord                 EVSEDataRecord,
                         ActionType                     OICPAction    = ActionType.insert,
                         EVSEOperator_Id                OperatorId    = null,
                         String                         OperatorName  = null,
                         Func<EVSEDataRecord, Boolean>  IncludeEVSEs  = null,
                         TimeSpan?                      QueryTimeout  = null)

        {

            return await _CPORoaming.PushEVSEData(EVSEDataRecord,
                                                  OICPAction,
                                                  OperatorId,
                                                  OperatorName,
                                                  IncludeEVSEs,
                                                  QueryTimeout);

        }

        #endregion

        #region PushEVSEData(OICPAction, params EVSEDataRecords)

        /// <summary>
        /// Create a new task pushing EVSE data records onto the OICP server.
        /// </summary>
        /// <param name="OICPAction">The OICP action.</param>
        /// <param name="EVSEDataRecords">An array of EVSE data records.</param>
        public async Task<eRoamingAcknowledgement>

            PushEVSEData(ActionType               OICPAction,
                         params EVSEDataRecord[]  EVSEDataRecords)
        {

            return await _CPORoaming.PushEVSEData(OICPAction,
                                                  EVSEDataRecords);

        }

        #endregion

        #region PushEVSEData(OICPAction, OperatorId, params EVSEDataRecords)

        /// <summary>
        /// Create a new task pushing EVSE data records onto the OICP server.
        /// </summary>
        /// <param name="OICPAction">The OICP action.</param>
        /// <param name="OperatorId">The EVSE operator Id to use.</param>
        /// <param name="EVSEDataRecords">An array of EVSE data records.</param>
        public async Task<eRoamingAcknowledgement>

            PushEVSEData(ActionType               OICPAction,
                         EVSEOperator_Id          OperatorId,
                         params EVSEDataRecord[]  EVSEDataRecords)
        {

            return await _CPORoaming.PushEVSEData(OICPAction,
                                                  OperatorId,
                                                  EVSEDataRecords);

        }

        #endregion

        #region PushEVSEData(OICPAction, OperatorId, OperatorName, params EVSEDataRecords)

        /// <summary>
        /// Create a new task pushing EVSE data records onto the OICP server.
        /// </summary>
        /// <param name="OICPAction">The OICP action.</param>
        /// <param name="OperatorId">The EVSE operator Id to use.</param>
        /// <param name="OperatorName">The EVSE operator name.</param>
        /// <param name="EVSEDataRecords">An array of EVSE data records.</param>
        public async Task<eRoamingAcknowledgement>

            PushEVSEData(ActionType               OICPAction,
                         EVSEOperator_Id          OperatorId,
                         String                   OperatorName,
                         params EVSEDataRecord[]  EVSEDataRecords)
        {

            return await _CPORoaming.PushEVSEData(OICPAction,
                                                  OperatorId,
                                                  OperatorName,
                                                  EVSEDataRecords);

        }

        #endregion

        #region PushEVSEData(EVSEDataRecords, OICPAction = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        /// <summary>
        /// Create a new task pushing EVSE data records onto the OICP server.
        /// </summary>
        /// <param name="EVSEDataRecords">An enumeration of EVSE data records.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// <param name="OperatorId">An optional EVSE operator Id to use. Otherwise it will be taken from the EVSE data records.</param>
        /// <param name="OperatorName">An optional EVSE operator name.</param>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSE data records before pushing them to the server.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<eRoamingAcknowledgement>

            PushEVSEData(IEnumerable<EVSEDataRecord>    EVSEDataRecords,
                         ActionType                     OICPAction    = ActionType.fullLoad,
                         EVSEOperator_Id                OperatorId    = null,
                         String                         OperatorName  = null,
                         Func<EVSEDataRecord, Boolean>  IncludeEVSEs  = null,
                         TimeSpan?                      QueryTimeout  = null)

        {

            #region Initial checks

            if (EVSEDataRecords == null)
                throw new ArgumentNullException("EVSEDataRecords", "The given parameter must not be null!");

            if (IncludeEVSEs == null)
                IncludeEVSEs = EVSEId => true;

            var _EVSEDataRecords = EVSEDataRecords.
                                       Where(evse => IncludeEVSEs(evse)).
                                       ToArray();

            #endregion

            return await _CPORoaming.PushEVSEData(EVSEDataRecords,
                                                  OICPAction,
                                                  OperatorId,
                                                  OperatorName,
                                                  IncludeEVSEs,
                                                  QueryTimeout);

        }

        #endregion


        #region PushEVSEStatus(EVSEOperator, ActionType = update, IncludeEVSEs = null, QueryTimeout = null)

        public async Task<Acknowledgement>

            PushEVSEStatus(EVSEOperator         EVSEOperator,
                           WWCP.ActionType      ActionType    = WWCP.ActionType.update,
                           Func<EVSE, Boolean>  IncludeEVSEs  = null,
                           TimeSpan?            QueryTimeout  = null)

        {

            #region Initial checks

            if (EVSEOperator == null)
                throw new ArgumentNullException("EVSEOperator", "The given parameter must not be null!");

            if (IncludeEVSEs == null)
                IncludeEVSEs = EVSE => true;

            #endregion

            var _AllEVSEs = EVSEOperator.
                                AllEVSEs.
                                Where(IncludeEVSEs).
                                Select(evse => new KeyValuePair<EVSE_Id, EVSEStatusType>(evse.Id, evse.Status.Value.AsOICPEVSEStatus())).
                                ToArray();

            if (_AllEVSEs.Any())
            {

                var result = await _CPORoaming.PushEVSEStatus(_AllEVSEs,
                                                              ActionType.AsOICPActionType(),
                                                              EVSEOperator.Id,
                                                              EVSEOperator.Name.Any() ? EVSEOperator.Name.FirstText : null,
                                                              QueryTimeout);

                if (result.Result == true)
                    return new Acknowledgement(true);

                else
                    return new Acknowledgement(false, result.StatusCode.Description);

            }

            return new Acknowledgement(true);

        }

        #endregion

        #region PushEVSEStatus(EVSEStatus, ActionType = update, OperatorId, QueryTimeout = null)

        /// <summary>
        /// Create a new task pushing EVSE status key-value-pairs.
        /// </summary>
        /// <param name="EVSEStatus">An enumeration of EVSE identification and status key-value-pairs.</param>
        /// <param name="ActionType">An optional action type.</param>
        /// <param name="OperatorId">An optional EVSE operator identification to use. Otherwise it will be taken from the EVSE data records.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<Acknowledgement>

            PushEVSEStatus(IEnumerable<KeyValuePair<EVSE_Id, WWCP.EVSEStatusType>>  EVSEStatus,
                           WWCP.ActionType                                          ActionType    = WWCP.ActionType.update,
                           EVSEOperator_Id                                          OperatorId    = null,
                           TimeSpan?                                                QueryTimeout  = null)

        {

            var result = await _CPORoaming.PushEVSEStatus(EVSEStatus.Select(kvp => new KeyValuePair<EVSE_Id, EVSEStatusType>(kvp.Key, kvp.Value.AsOICPEVSEStatus())),
                                                          ActionType.AsOICPActionType(),
                                                          OperatorId,
                                                          null,
                                                          QueryTimeout);

            if (result.Result == true)
                return new Acknowledgement(true);

            else
                return new Acknowledgement(false, result.StatusCode.Description);

        }

        #endregion

        #region PushEVSEStatus(EVSEStatus, OICPAction = update, OperatorId, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        ///// <summary>
        ///// Create a new task pushing EVSE status records onto the OICP server.
        ///// </summary>
        ///// <param name="EVSEStatus">An enumeration of EVSE Id and status records.</param>
        ///// <param name="OICPAction">An optional OICP action.</param>
        ///// <param name="OperatorId">An optional EVSE operator Id to use. Otherwise it will be taken from the EVSE data records.</param>
        ///// <param name="OperatorName">An optional EVSE operator name.</param>
        ///// <param name="QueryTimeout">An optional timeout for this query.</param>
        //public async Task<Acknowledgement>

        //    PushEVSEStatus(IEnumerable<EVSEStatusRecord>  EVSEStatus,
        //                   ActionType                     OICPAction    = ActionType.update,
        //                   EVSEOperator_Id                OperatorId    = null,
        //                   String                         OperatorName  = null,
        //                   TimeSpan?                      QueryTimeout  = null)

        //{

        //    #region Initial checks

        //    if (EVSEStatus == null)
        //        throw new ArgumentNullException("EVSEStatus", "The given parameter must not be null!");

        //    var _EVSEStatus = EVSEStatus.ToArray();

        //    if (!_EVSEStatus.Any())
        //        return new Acknowledgement(true);

        //    if (OperatorId == null)
        //        OperatorId = _EVSEStatus.First().Id.OperatorId;

        //    #endregion

        //    var result = await _CPORoaming.PushEVSEStatus(_EVSEStatus,
        //                                                  OICPAction,
        //                                                  OperatorId,
        //                                                  OperatorName,
        //                                                  QueryTimeout);


        //    if (result.Result == true)
        //        return new Acknowledgement(true);

        //    else
        //        return new Acknowledgement(false, result.StatusCode.Description);


        //}

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

            if (EVSEStatusDiff == null)
                return;

            await _CPORoaming.PushEVSEStatus(EVSEStatusDiff);

        }

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

            AuthorizeStart(EVSEOperator_Id     OperatorId,
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

            AuthorizeStart(EVSEOperator_Id     OperatorId,
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

            AuthorizeStart(EVSEOperator_Id     OperatorId,
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

            AuthorizeStop(EVSEOperator_Id     OperatorId,
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

            AuthorizeStop(EVSEOperator_Id     OperatorId,
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

            AuthorizeStop(EVSEOperator_Id     OperatorId,
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


        #region SendChargeDetailRecord(ChargeDetailRecord, QueryTimeout = null)

        /// <summary>
        /// Create an OICP SendChargeDetailRecord request.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<SendCDRResult>

            SendChargeDetailRecord(ChargeDetailRecord  ChargeDetailRecord,
                                   TimeSpan?           QueryTimeout  = null)

        {

            #region Initial checks

            if (ChargeDetailRecord == null)
                throw new ArgumentNullException("ChargeDetailRecord", "The given parameter must not be null!");

            #endregion

            var result = await _CPORoaming.SendChargeDetailRecord(EVSEId:                ChargeDetailRecord.EVSEId,
                                                                  SessionId:             ChargeDetailRecord.SessionId,
                                                                  PartnerProductId:      ChargeDetailRecord.PartnerProductId,
                                                                  SessionStart:          ChargeDetailRecord.SessionTime.Value.StartTime,
                                                                  SessionEnd:            ChargeDetailRecord.SessionTime.Value.EndTime.Value,
                                                                  Identification:        new AuthorizationIdentification(ChargeDetailRecord.Identification),
                                                                  PartnerSessionId:      ChargeDetailRecord.PartnerSessionId,
                                                                  ChargingStart:         ChargeDetailRecord.SessionTime.HasValue ? new Nullable<DateTime>(ChargeDetailRecord.SessionTime.Value.StartTime) : null,
                                                                  ChargingEnd:           ChargeDetailRecord.SessionTime.HasValue ?                        ChargeDetailRecord.SessionTime.Value.EndTime    : null,
                                                                  MeterValueStart:       ChargeDetailRecord.MeterValues != null && ChargeDetailRecord.MeterValues.Any() ? new Double?(ChargeDetailRecord.MeterValues.First().Value) : null,
                                                                  MeterValueEnd:         ChargeDetailRecord.MeterValues != null && ChargeDetailRecord.MeterValues.Any() ? new Double?(ChargeDetailRecord.MeterValues.Last(). Value) : null,
                                                                  MeterValuesInBetween:  ChargeDetailRecord.MeterValues != null && ChargeDetailRecord.MeterValues.Any() ? ChargeDetailRecord.MeterValues.Select(v => v.Value)       : null,
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
        /// <param name="HubOperatorId">An optional identification of the hub operator.</param>
        /// <param name="HubProviderId">An optional identification of the hub provider.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<SendCDRResult>

            SendChargeDetailRecord(EVSE_Id              EVSEId,
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
                                   HubOperator_Id       HubOperatorId         = null,
                                   EVSP_Id              HubProviderId         = null,
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
                                                                  HubOperatorId,
                                                                  HubProviderId,
                                                                  QueryTimeout);


            // true
            if (result.Result)
                return SendCDRResult.Forwarded(_AuthorizatorId);

            // false
            else
                return SendCDRResult.False(_AuthorizatorId);

        }

        #endregion


    }

}
