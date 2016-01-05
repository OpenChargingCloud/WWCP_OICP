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

        #region RoamingProviderId

        private readonly RoamingProvider_Id _RoamingProviderId;

        /// <summary>
        /// The unique identification of the roaming provider.
        /// </summary>
        public RoamingProvider_Id RoamingProviderId
        {
            get
            {
                return _RoamingProviderId;
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

        #region CPORoamingWWCP(RoamingNetwork, RoamingProviderId, CPORoaming)

        /// <summary>
        /// Create a new WWCP wrapper for the OICP roaming client for EVSE operators/CPOs.
        /// </summary>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// <param name="RoamingProviderId">The unique identification of the roaming provider.</param>
        /// <param name="CPORoaming">A OICP CPO raoming object to be mapped to WWCP.</param>
        public CPORoamingWWCP(RoamingNetwork      RoamingNetwork,
                              RoamingProvider_Id  RoamingProviderId,
                              CPORoaming          CPORoaming)
        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException("RoamingNetwork", "The given roaming network must not be null!");

            if (CPORoaming == null)
                throw new ArgumentNullException("CPORoaming", "The given OICP CPO Roaming object must not be null!");

            #endregion

            this._RoamingNetwork     = RoamingNetwork;
            this._CPORoaming         = CPORoaming;
            this._RoamingProviderId  = RoamingProviderId;
            this._AuthorizatorId     = Authorizator_Id.Parse(RoamingProviderId.ToString());

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

        #region CPORoamingWWCP(RoamingNetwork, RoamingProviderId, CPOClient, CPOServer)

        /// <summary>
        /// Create a new WWCP wrapper for the OICP roaming client for EVSE operators/CPOs.
        /// </summary>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// <param name="RoamingProviderId">The unique identification of the roaming provider.</param>
        /// <param name="CPOClient">An OICP CPO client.</param>
        /// <param name="CPOServer">An OICP CPO sever.</param>
        public CPORoamingWWCP(RoamingNetwork      RoamingNetwork,
                              RoamingProvider_Id  RoamingProviderId,
                              CPOClient           CPOClient,
                              CPOServer           CPOServer)

            : this(RoamingNetwork,
                   RoamingProviderId,
                   new CPORoaming(CPOClient,
                                  CPOServer))

        { }

        #endregion

        #region CPORoamingWWCP(RoamingNetwork, RoamingProviderId, ...)

        /// <summary>
        /// Create a new WWCP wrapper for the OICP roaming client for EVSE operators/CPOs.
        /// </summary>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// <param name="RoamingProviderId">The unique identification of the roaming provider.</param>
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
        public CPORoamingWWCP(RoamingNetwork      RoamingNetwork,
                              RoamingProvider_Id  RoamingProviderId,

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

            : this(RoamingNetwork,
                   RoamingProviderId,
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


        //#region PushEVSEData(EVSEDataRecord, OICPAction = insert, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        ///// <summary>
        ///// Create a new task pushing a single EVSE data record onto the OICP server.
        ///// </summary>
        ///// <param name="EVSEDataRecord">An EVSE data record.</param>
        ///// <param name="OICPAction">An optional OICP action.</param>
        ///// <param name="OperatorId">An optional EVSE operator Id to use. Otherwise it will be taken from the EVSE data record.</param>
        ///// <param name="OperatorName">An optional EVSE operator name.</param>
        ///// <param name="IncludeEVSEs">An optional delegate for filtering EVSE data records before pushing them to the server.</param>
        ///// <param name="QueryTimeout">An optional timeout for this query.</param>
        //public async Task<eRoamingAcknowledgement>

        //    PushEVSEData(EVSEDataRecord                 EVSEDataRecord,
        //                 ActionType                     OICPAction    = ActionType.insert,
        //                 EVSEOperator_Id                OperatorId    = null,
        //                 String                         OperatorName  = null,
        //                 Func<EVSEDataRecord, Boolean>  IncludeEVSEs  = null,
        //                 TimeSpan?                      QueryTimeout  = null)

        //{

        //    return await _CPORoaming.PushEVSEData(EVSEDataRecord,
        //                                          OICPAction,
        //                                          OperatorId,
        //                                          OperatorName,
        //                                          IncludeEVSEs,
        //                                          QueryTimeout);

        //}

        //#endregion

        //#region PushEVSEData(OICPAction, params EVSEDataRecords)

        ///// <summary>
        ///// Create a new task pushing EVSE data records onto the OICP server.
        ///// </summary>
        ///// <param name="OICPAction">The OICP action.</param>
        ///// <param name="EVSEDataRecords">An array of EVSE data records.</param>
        //public async Task<eRoamingAcknowledgement>

        //    PushEVSEData(ActionType               OICPAction,
        //                 params EVSEDataRecord[]  EVSEDataRecords)
        //{

        //    return await _CPORoaming.PushEVSEData(OICPAction,
        //                                          EVSEDataRecords);

        //}

        //#endregion

        //#region PushEVSEData(OICPAction, OperatorId, params EVSEDataRecords)

        ///// <summary>
        ///// Create a new task pushing EVSE data records onto the OICP server.
        ///// </summary>
        ///// <param name="OICPAction">The OICP action.</param>
        ///// <param name="OperatorId">The EVSE operator Id to use.</param>
        ///// <param name="EVSEDataRecords">An array of EVSE data records.</param>
        //public async Task<eRoamingAcknowledgement>

        //    PushEVSEData(ActionType               OICPAction,
        //                 EVSEOperator_Id          OperatorId,
        //                 params EVSEDataRecord[]  EVSEDataRecords)
        //{

        //    return await _CPORoaming.PushEVSEData(OICPAction,
        //                                          OperatorId,
        //                                          EVSEDataRecords);

        //}

        //#endregion

        //#region PushEVSEData(OICPAction, OperatorId, OperatorName, params EVSEDataRecords)

        ///// <summary>
        ///// Create a new task pushing EVSE data records onto the OICP server.
        ///// </summary>
        ///// <param name="OICPAction">The OICP action.</param>
        ///// <param name="OperatorId">The EVSE operator Id to use.</param>
        ///// <param name="OperatorName">The EVSE operator name.</param>
        ///// <param name="EVSEDataRecords">An array of EVSE data records.</param>
        //public async Task<eRoamingAcknowledgement>

        //    PushEVSEData(ActionType               OICPAction,
        //                 EVSEOperator_Id          OperatorId,
        //                 String                   OperatorName,
        //                 params EVSEDataRecord[]  EVSEDataRecords)
        //{

        //    return await _CPORoaming.PushEVSEData(OICPAction,
        //                                          OperatorId,
        //                                          OperatorName,
        //                                          EVSEDataRecords);

        //}

        //#endregion

        //#region PushEVSEData(EVSEDataRecords, OICPAction = fullLoad, OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        ///// <summary>
        ///// Create a new task pushing EVSE data records onto the OICP server.
        ///// </summary>
        ///// <param name="EVSEDataRecords">An enumeration of EVSE data records.</param>
        ///// <param name="OICPAction">An optional OICP action.</param>
        ///// <param name="OperatorId">An optional EVSE operator Id to use. Otherwise it will be taken from the EVSE data records.</param>
        ///// <param name="OperatorName">An optional EVSE operator name.</param>
        ///// <param name="IncludeEVSEs">An optional delegate for filtering EVSE data records before pushing them to the server.</param>
        ///// <param name="QueryTimeout">An optional timeout for this query.</param>
        //public async Task<eRoamingAcknowledgement>

        //    PushEVSEData(IEnumerable<EVSEDataRecord>    EVSEDataRecords,
        //                 ActionType                     OICPAction    = ActionType.fullLoad,
        //                 EVSEOperator_Id                OperatorId    = null,
        //                 String                         OperatorName  = null,
        //                 Func<EVSEDataRecord, Boolean>  IncludeEVSEs  = null,
        //                 TimeSpan?                      QueryTimeout  = null)

        //{

        //    #region Initial checks

        //    if (EVSEDataRecords == null)
        //        throw new ArgumentNullException("EVSEDataRecords", "The given parameter must not be null!");

        //    if (IncludeEVSEs == null)
        //        IncludeEVSEs = EVSEId => true;

        //    var _EVSEDataRecords = EVSEDataRecords.
        //                               Where(evse => IncludeEVSEs(evse)).
        //                               ToArray();

        //    #endregion

        //    return await _CPORoaming.PushEVSEData(EVSEDataRecords,
        //                                          OICPAction,
        //                                          OperatorId,
        //                                          OperatorName,
        //                                          IncludeEVSEs,
        //                                          QueryTimeout);

        //}

        //#endregion


        //#region PushEVSEStatus(EVSEStatus, OICPAction = update, OperatorId, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        ///// <summary>
        ///// Create a new task pushing EVSE status records onto the OICP server.
        ///// </summary>
        ///// <param name="EVSEStatus">An enumeration of EVSE Id and status records.</param>
        ///// <param name="OICPAction">An optional OICP action.</param>
        ///// <param name="OperatorId">An optional EVSE operator Id to use. Otherwise it will be taken from the EVSE data records.</param>
        ///// <param name="OperatorName">An optional EVSE operator name.</param>
        ///// <param name="QueryTimeout">An optional timeout for this query.</param>
        //public async Task<eRoamingAcknowledgement>

        //    PushEVSEStatus(IEnumerable<KeyValuePair<EVSE_Id, EVSEStatusType>>  EVSEStatus,
        //                   ActionType                                          OICPAction    = ActionType.update,
        //                   EVSEOperator_Id                                     OperatorId    = null,
        //                   String                                              OperatorName  = null,
        //                   TimeSpan?                                           QueryTimeout  = null)

        //{

        //    return await _CPORoaming.PushEVSEStatus(EVSEStatus,
        //                                            OICPAction,
        //                                            OperatorId,
        //                                            OperatorName,
        //                                            QueryTimeout);

        //}

        //#endregion

        //#region PushEVSEStatus(EVSEStatus, OICPAction = update, OperatorId, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

        ///// <summary>
        ///// Create a new task pushing EVSE status records onto the OICP server.
        ///// </summary>
        ///// <param name="EVSEStatus">An enumeration of EVSE Id and status records.</param>
        ///// <param name="OICPAction">An optional OICP action.</param>
        ///// <param name="OperatorId">An optional EVSE operator Id to use. Otherwise it will be taken from the EVSE data records.</param>
        ///// <param name="OperatorName">An optional EVSE operator name.</param>
        ///// <param name="QueryTimeout">An optional timeout for this query.</param>
        //public async Task<eRoamingAcknowledgement>

        //    PushEVSEStatus(IEnumerable<EVSEStatusRecord>  EVSEStatus,
        //                   ActionType                     OICPAction    = ActionType.update,
        //                   EVSEOperator_Id                OperatorId    = null,
        //                   String                         OperatorName  = null,
        //                   TimeSpan?                      QueryTimeout  = null)

        //{

        //    #region Initial checks

        //    if (EVSEStatus == null)
        //        throw new ArgumentNullException("EVSEStatus", "The given parameter must not be null!");

        //    if (OperatorId == null)
        //        OperatorId = EVSEStatus.First().Id.OperatorId;

        //    #endregion

        //    return await _CPORoaming.PushEVSEStatus(EVSEStatus,
        //                                            OICPAction,
        //                                            OperatorId,
        //                                            OperatorName,
        //                                            QueryTimeout);

        //}

        //#endregion

        //#region PushEVSEStatusUpdates(EVSEStatusDiff)

        ///// <summary>
        ///// Send EVSE status updates upstream.
        ///// </summary>
        ///// <param name="EVSEStatusDiff">An EVSE status diff.</param>
        //public async Task PushEVSEStatusUpdates(EVSEStatusDiff  EVSEStatusDiff)

        //{

        //    if (EVSEStatusDiff == null)
        //        return;

        //    await _CPORoaming.PushEVSEStatusUpdates(EVSEStatusDiff);

        //}

        //#endregion

        //#region PullAuthenticationData(OperatorId, QueryTimeout = null)

        ///// <summary>
        ///// Create an OICP v2.0 PullAuthenticationData request.
        ///// </summary>
        ///// <param name="OperatorId">An EVSE operator identification.</param>
        ///// <param name="QueryTimeout">An optional timeout for this query.</param>
        //public async Task<eRoamingAuthenticationData> PullAuthenticationData(EVSEOperator_Id  OperatorId,
        //                                                                     TimeSpan?        QueryTimeout = null)
        //{

        //    #region Initial checks

        //    if (OperatorId == null)
        //        throw new ArgumentNullException("OperatorId", "The given parameter must not be null!");

        //    #endregion

        //    return await _CPORoaming.PullAuthenticationData(OperatorId,
        //                                                    QueryTimeout);

        //}

        //#endregion

        //#region (internal) SendRemoteStart(...)

        //internal async Task<RemoteStartEVSEResult> SendRemoteStart(DateTime            Timestamp,
        //                                                           CPOServer           Sender,
        //                                                           CancellationToken   CancellationToken,
        //                                                           EVSE_Id             EVSEId,
        //                                                           ChargingProduct_Id  ChargingProductId,
        //                                                           ChargingSession_Id  SessionId,
        //                                                           ChargingSession_Id  PartnerSessionId,
        //                                                           EVSP_Id             ProviderId,
        //                                                           eMA_Id              eMAId)
        //{

        //    var OnRemoteStartLocal = OnRemoteStart;
        //    if (OnRemoteStartLocal == null)
        //        return RemoteStartEVSEResult.Error();

        //    var results = await Task.WhenAll(OnRemoteStartLocal.
        //                                         GetInvocationList().
        //                                         Select(subscriber => (subscriber as OnRemoteStartDelegate)
        //                                             (Timestamp,
        //                                              this.CPOServer,
        //                                              CancellationToken,
        //                                              EVSEId,
        //                                              ChargingProductId,
        //                                              SessionId,
        //                                              PartnerSessionId,
        //                                              ProviderId,
        //                                              eMAId)));

        //    return results.
        //               Where(result => result.Result != RemoteStartEVSEResultType.Unspecified).
        //               First();

        //}

        //#endregion

        //#region (internal) SendRemoteStop(...)

        //internal async Task<RemoteStopEVSEResult> SendRemoteStop(DateTime            Timestamp,
        //                                                         CPOServer           Sender,
        //                                                         CancellationToken   CancellationToken,
        //                                                         EVSE_Id             EVSEId,
        //                                                         ChargingSession_Id  SessionId,
        //                                                         ChargingSession_Id  PartnerSessionId,
        //                                                         EVSP_Id             ProviderId)
        //{

        //    var OnRemoteStopLocal = OnRemoteStop;
        //    if (OnRemoteStopLocal == null)
        //        return RemoteStopEVSEResult.Error();

        //    var results = await Task.WhenAll(OnRemoteStopLocal.
        //                                         GetInvocationList().
        //                                         Select(subscriber => (subscriber as OnRemoteStopDelegate)
        //                                             (Timestamp,
        //                                              this.CPOServer,
        //                                              CancellationToken,
        //                                              EVSEId,
        //                                              SessionId,
        //                                              PartnerSessionId,
        //                                              ProviderId)));

        //    return results.
        //               Where(result => result.Result != RemoteStopEVSEResultType.Unspecified).
        //               First();

        //}

        //#endregion



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

            var AuthStartTask = await _CPORoaming.AuthorizeStart(OperatorId,
                                                                 AuthToken,
                                                                 null,
                                                                 SessionId,
                                                                 ChargingProductId,
                                                                 null,
                                                                 QueryTimeout);

            if (AuthStartTask.AuthorizationStatus == AuthorizationStatusType.Authorized)
                return AuthStartResult.Authorized(_AuthorizatorId,
                                                  AuthStartTask.SessionId,
                                                  AuthStartTask.ProviderId,
                                                  AuthStartTask.StatusCode.Description,
                                                  AuthStartTask.StatusCode.AdditionalInfo);

            return AuthStartResult.NotAuthorized(_AuthorizatorId,
                                                 AuthStartTask.ProviderId,
                                                 AuthStartTask.StatusCode.Description,
                                                 AuthStartTask.StatusCode.AdditionalInfo);

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

            var AuthStartTask  = await _CPORoaming.AuthorizeStart(OperatorId,
                                                                  AuthToken,
                                                                  EVSEId,
                                                                  SessionId,
                                                                  ChargingProductId,
                                                                  null,
                                                                  QueryTimeout);

            if (AuthStartTask.AuthorizationStatus == AuthorizationStatusType.Authorized)
                return AuthStartEVSEResult.Authorized(_AuthorizatorId,
                                                      AuthStartTask.SessionId,
                                                      AuthStartTask.ProviderId,
                                                      AuthStartTask.StatusCode.Description,
                                                      AuthStartTask.StatusCode.AdditionalInfo);

            return AuthStartEVSEResult.NotAuthorized(_AuthorizatorId,
                                                     AuthStartTask.ProviderId,
                                                     AuthStartTask.StatusCode.Description,
                                                     AuthStartTask.StatusCode.AdditionalInfo);

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

            var AuthStopTask  = await _CPORoaming.AuthorizeStop(OperatorId,
                                                                SessionId,
                                                                AuthToken,
                                                                null,
                                                                null,
                                                                QueryTimeout);

            if (AuthStopTask.AuthorizationStatus == AuthorizationStatusType.Authorized)
                return AuthStopResult.Authorized(_AuthorizatorId,
                                                 AuthStopTask.ProviderId,
                                                 AuthStopTask.StatusCode.Description,
                                                 AuthStopTask.StatusCode.AdditionalInfo);

            return AuthStopResult.NotAuthorized(_AuthorizatorId,
                                                AuthStopTask.ProviderId,
                                                AuthStopTask.StatusCode.Description,
                                                AuthStopTask.StatusCode.AdditionalInfo);

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

            var AuthStopTask  = await _CPORoaming.AuthorizeStop(OperatorId,
                                                                SessionId,
                                                                AuthToken,
                                                                EVSEId,
                                                                null,
                                                                QueryTimeout);

            if (AuthStopTask.AuthorizationStatus == AuthorizationStatusType.Authorized)
                return AuthStopEVSEResult.Authorized(_AuthorizatorId,
                                                     AuthStopTask.ProviderId,
                                                     AuthStopTask.StatusCode.Description,
                                                     AuthStopTask.StatusCode.AdditionalInfo);

            return AuthStopEVSEResult.NotAuthorized(_AuthorizatorId,
                                                    AuthStopTask.ProviderId,
                                                    AuthStopTask.StatusCode.Description,
                                                    AuthStopTask.StatusCode.AdditionalInfo);

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
        public async Task<eRoamingAcknowledgement>

            SendChargeDetailRecord(ChargeDetailRecord  ChargeDetailRecord,
                                   TimeSpan?           QueryTimeout  = null)

        {

            #region Initial checks

            if (ChargeDetailRecord == null)
                throw new ArgumentNullException("ChargeDetailRecord", "The given parameter must not be null!");

            #endregion

            return await _CPORoaming.SendChargeDetailRecord(EVSEId:                ChargeDetailRecord.EVSEId,
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

            var Ack = await _CPORoaming.SendChargeDetailRecord(EVSEId,
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
            if (Ack.Result)
                return SendCDRResult.Forwarded(_AuthorizatorId);

            // false
            else
                return SendCDRResult.False(_AuthorizatorId);

        }

        #endregion


    }

}
