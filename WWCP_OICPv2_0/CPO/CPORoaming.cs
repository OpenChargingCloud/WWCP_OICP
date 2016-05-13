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
using System.Net.Security;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
{

    /// <summary>
    /// A OICP v2.0 roaming client for CPOs.
    /// </summary>
    public class CPORoaming
    {

        #region Properties

        #region CPOClient

        private readonly CPOClient _CPOClient;

        /// <summary>
        /// The CPO client part.
        /// </summary>
        public CPOClient CPOClient
        {
            get
            {
                return _CPOClient;
            }
        }

        #endregion

        #region CPOServer

        private readonly CPOServer _CPOServer;

        /// <summary>
        /// The CPO server part.
        /// </summary>
        public CPOServer CPOServer
        {
            get
            {
                return _CPOServer;
            }
        }

        #endregion

        #region CPOClientLogger

        private readonly CPOClientLogger _CPOClientLogger;

        /// <summary>
        /// The CPO client logger.
        /// </summary>
        public CPOClientLogger CPOClientLogger
        {
            get
            {
                return _CPOClientLogger;
            }
        }

        #endregion

        #region CPOServerLogger

        private readonly CPOServerLogger _CPOServerLogger;

        /// <summary>
        /// The CPO server logger.
        /// </summary>
        public CPOServerLogger CPOServerLogger
        {
            get
            {
                return _CPOServerLogger;
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
                return _CPOServer.DNSClient;
            }
        }

        #endregion

        #endregion

        #region Events

        // Client methods (logging)

        #region OnEVSEDataPush/-Pushed

        /// <summary>
        /// An event fired whenever new EVSE data record will be send upstream.
        /// </summary>
        public event OnEVSEDataPushDelegate OnEVSEDataPush
        {

            add
            {
                _CPOClient.OnEVSEDataPush += value;
            }

            remove
            {
                 _CPOClient.OnEVSEDataPush -= value;
            }

        }

        /// <summary>
        /// An event fired whenever new EVSE data record had been sent upstream.
        /// </summary>
        public event OnEVSEDataPushedDelegate OnEVSEDataPushed
        {

            add
            {
                _CPOClient.OnEVSEDataPushed += value;
            }

            remove
            {
                _CPOClient.OnEVSEDataPushed -= value;
            }

        }

        #endregion

        //ToDo: Add more CPOClient methods!


        // Server methods

        #region OnRemoteStart/-Stop

        /// <summary>
        /// An event sent whenever a remote start command was received.
        /// </summary>
        public event OnRemoteStartDelegate OnRemoteStart
        {

            add
            {
                _CPOServer.OnRemoteStart += value;
            }

            remove
            {
                _CPOServer.OnRemoteStart -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopDelegate OnRemoteStop

        {

            add
            {
                _CPOServer.OnRemoteStop += value;
            }

            remove
            {
                _CPOServer.OnRemoteStop -= value;
            }

        }

        #endregion


        #region RequestLog

        /// <summary>
        /// An event called whenever a request came in.
        /// </summary>
        public event RequestLogHandler RequestLog
        {

            add
            {
                _CPOServer.RequestLog += value;
            }

            remove
            {
                _CPOServer.RequestLog -= value;
            }

        }

        #endregion

        #region AccessLog

        /// <summary>
        /// An event called whenever a request could successfully be processed.
        /// </summary>
        public event AccessLogHandler AccessLog
        {

            add
            {
                _CPOServer.AccessLog += value;
            }

            remove
            {
                _CPOServer.AccessLog -= value;
            }

        }

        #endregion

        #region ErrorLog

        /// <summary>
        /// An event called whenever a request resulted in an error.
        /// </summary>
        public event ErrorLogHandler ErrorLog
        {

            add
            {
                _CPOServer.ErrorLog += value;
            }

            remove
            {
                _CPOServer.ErrorLog -= value;
            }

        }

        #endregion

        #endregion

        #region Constructor(s)

        #region CPORoaming(CPOClient, CPOServer, ClientLoggingContext = CPOClientLogger.DefaultContext, ServerLoggingContext = CPOServerLogger.DefaultContext, LogFileCreator = null)

        /// <summary>
        /// Create a new OICP roaming client for CPOs.
        /// </summary>
        /// <param name="CPOClient">A CPO client.</param>
        /// <param name="CPOServer">A CPO sever.</param>
        /// <param name="ClientLoggingContext">An optional context for logging client methods.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
        public CPORoaming(CPOClient                     CPOClient,
                          CPOServer                     CPOServer,
                          String                        ClientLoggingContext  = CPOClientLogger.DefaultContext,
                          String                        ServerLoggingContext  = CPOServerLogger.DefaultContext,
                          Func<String, String, String>  LogFileCreator        = null)
        {

            this._CPOClient        = CPOClient;
            this._CPOServer        = CPOServer;
            this._CPOClientLogger  = new CPOClientLogger(_CPOClient, ClientLoggingContext, LogFileCreator);
            this._CPOServerLogger  = new CPOServerLogger(_CPOServer, ServerLoggingContext, LogFileCreator);

        }

        #endregion

        #region CPORoaming(ClientId, RemoteHostname, RemoteTCPPort = null, RemoteHTTPVirtualHost = null, ... )

        /// <summary>
        /// Create a new OICP roaming client for CPOs.
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
        /// <param name="ClientLoggingContext">An optional context for logging client methods.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// 
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public CPORoaming(String                               ClientId,
                          String                               RemoteHostname,
                          IPPort                               RemoteTCPPort               = null,
                          String                               RemoteHTTPVirtualHost       = null,
                          RemoteCertificateValidationCallback  RemoteCertificateValidator  = null,
                          String                               HTTPUserAgent               = CPOClient.DefaultHTTPUserAgent,
                          TimeSpan?                            QueryTimeout                = null,

                          String                               ServerName                  = CPOServer.DefaultHTTPServerName,
                          IPPort                               ServerTCPPort               = null,
                          String                               ServerURIPrefix             = "",
                          Boolean                              ServerAutoStart             = false,

                          String                               ClientLoggingContext        = CPOClientLogger.DefaultContext,
                          String                               ServerLoggingContext        = CPOServerLogger.DefaultContext,
                          Func<String, String, String>         LogFileCreator              = null,

                          DNSClient                            DNSClient                   = null)

            : this(new CPOClient(ClientId,
                                 RemoteHostname,
                                 RemoteTCPPort,
                                 RemoteHTTPVirtualHost,
                                 RemoteCertificateValidator,
                                 HTTPUserAgent,
                                 QueryTimeout,
                                 DNSClient),

                   new CPOServer(ServerName,
                                 ServerTCPPort,
                                 ServerURIPrefix,
                                 DNSClient,
                                 false),

                   ClientLoggingContext,
                   ServerLoggingContext,
                   LogFileCreator)

        {

            if (ServerAutoStart)
                Start();

        }

        #endregion

        #endregion


        #region PushEVSEData(GroupedEVSEs,    OICPAction = fullLoad, OperatorId = null, OperatorName = null,                      QueryTimeout = null)

        /// <summary>
        /// Upload the given lookup of EVSEs grouped by their EVSE operator.
        /// </summary>
        /// <param name="GroupedEVSEs">A lookup of EVSEs grouped by their EVSE operator.</param>
        /// <param name="OICPAction">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public async Task<eRoamingAcknowledgement>

            PushEVSEData(ILookup<EVSEOperator, EVSEDataRecord>  GroupedEVSEs,
                         ActionType                             OICPAction    = ActionType.fullLoad,
                         EVSEOperator_Id                        OperatorId    = null,
                         String                                 OperatorName  = null,
                         TimeSpan?                              QueryTimeout  = null)

        {

            var result = await _CPOClient.PushEVSEData(GroupedEVSEs,
                                                       OICPAction,
                                                       OperatorId,
                                                       OperatorName,
                                                       QueryTimeout);

            //ToDo: Process the HTTP!
            return result.Content;

        }

        #endregion

        #region PushEVSEData(EVSEDataRecord,  OICPAction = insert,   OperatorId = null, OperatorName = null, IncludeEVSEs = null, QueryTimeout = null)

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

            var result = await _CPOClient.PushEVSEData(EVSEDataRecord,
                                                       OICPAction,
                                                       OperatorId,
                                                       OperatorName,
                                                       IncludeEVSEs,
                                                       QueryTimeout);

            //ToDo: Process the HTTP!
            return result.Content;

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

            var result = await _CPOClient.PushEVSEData(EVSEDataRecords,
                                                       OICPAction,
                                                       OperatorId,
                                                       OperatorName,
                                                       IncludeEVSEs,
                                                       QueryTimeout);

            //ToDo: Process the HTTP!
            return result.Content;

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

            var result = await _CPOClient.PushEVSEData(OICPAction,
                                                       EVSEDataRecords);

            //ToDo: Process the HTTP!
            return result.Content;

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

            var result = await _CPOClient.PushEVSEData(OICPAction,
                                                       OperatorId,
                                                       EVSEDataRecords);

            //ToDo: Process the HTTP!
            return result.Content;

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

            var result = await _CPOClient.PushEVSEData(OICPAction,
                                                       OperatorId,
                                                       OperatorName,
                                                       EVSEDataRecords);

            //ToDo: Process the HTTP!
            return result.Content;

        }

        #endregion


        #region PushEVSEStatus(EVSEStatusRecords,  OICPAction = fullLoad, OperatorId = null, OperatorName = null,                                  QueryTimeout = null)

        /// <summary>
        /// Upload the given enumeration of EVSE status records.
        /// </summary>
        /// <param name="EVSEStatusRecords">An enumeration of EVSE status records.</param>
        /// <param name="OICPAction">The server-side data management operation.</param>
        /// <param name="OperatorId">An optional unique identification of the EVSE operator.</param>
        /// <param name="OperatorName">The optional name of the EVSE operator.</param>
        /// <param name="QueryTimeout">An optional timeout of the HTTP client [default 60 sec.]</param>
        public async Task<eRoamingAcknowledgement>

            PushEVSEStatus(IEnumerable<EVSEStatusRecord>  EVSEStatusRecords,
                           ActionType                     OICPAction    = ActionType.fullLoad,
                           EVSEOperator_Id                OperatorId    = null,
                           String                         OperatorName  = null,
                           TimeSpan?                      QueryTimeout  = null)

        {

            var result = await _CPOClient.PushEVSEStatus(EVSEStatusRecords,
                                                         OICPAction,
                                                         OperatorId,
                                                         OperatorName,
                                                         QueryTimeout);

            //ToDo: Process the HTTP!
            return result.Content;

        }

        #endregion

        #region PushEVSEStatus(KeyValuePairs<...>, OICPAction = update,   OperatorId = null, OperatorName = null, IncludeEVSEStatusRecords = null, QueryTimeout = null)

        /// <summary>
        /// Create a new task pushing EVSE status key-value-pairs onto the OICP server.
        /// </summary>
        /// <param name="EVSEStatus">An enumeration of EVSE identification and status key-value-pairs.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// <param name="OperatorId">An optional EVSE operator identification to use. Otherwise it will be taken from the EVSE data records.</param>
        /// <param name="OperatorName">An optional EVSE operator name.</param>
        /// <param name="IncludeEVSEStatusRecords">An optional delegate for filtering EVSE status records before pushing them to the server.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<eRoamingAcknowledgement>

            PushEVSEStatus(IEnumerable<KeyValuePair<EVSE_Id, EVSEStatusType>>  EVSEStatus,
                           ActionType                                          OICPAction                = ActionType.update,
                           EVSEOperator_Id                                     OperatorId                = null,
                           String                                              OperatorName              = null,
                           Func<EVSEStatusRecord, Boolean>                     IncludeEVSEStatusRecords  = null,
                           TimeSpan?                                           QueryTimeout              = null)

        {

            var result = await _CPOClient.PushEVSEStatus(EVSEStatus,
                                                         OICPAction,
                                                         OperatorId,
                                                         OperatorName,
                                                         IncludeEVSEStatusRecords,
                                                         QueryTimeout);

            //ToDo: Process the HTTP!
            return result.Content;

        }

        #endregion


        #region AuthorizeStart(OperatorId, AuthToken, EVSEId = null, SessionId = null, PartnerProductId = null, PartnerSessionId = null, QueryTimeout = null)

        /// <summary>
        /// Create an OICP v2.0 authorize start request.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="PartnerProductId">An optional partner product identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<eRoamingAuthorizationStart>

            AuthorizeStart(EVSEOperator_Id     OperatorId,
                           Auth_Token          AuthToken,
                           EVSE_Id             EVSEId            = null,
                           ChargingSession_Id  SessionId         = null,
                           ChargingProduct_Id  PartnerProductId  = null,   // [maxlength: 100]
                           ChargingSession_Id  PartnerSessionId  = null,   // [maxlength: 50]
                           TimeSpan?           QueryTimeout      = null)

        {

            var result = await _CPOClient.AuthorizeStart(OperatorId,
                                                         AuthToken,
                                                         EVSEId,
                                                         SessionId,
                                                         PartnerProductId,
                                                         PartnerSessionId,
                                                         QueryTimeout);

            //ToDo: Process the HTTP!
            return result.Content;

        }

        #endregion

        #region AuthorizeStop (OperatorId, SessionId, AuthToken, EVSEId = null, PartnerSessionId = null, QueryTimeout = null)

        // UID => Not everybody can stop any session, but maybe another
        //        UID than the UID which started the session!
        //        (e.g. car sharing)

        /// <summary>
        /// Create an OICP v2.0 authorize stop request.
        /// </summary>
        /// <param name="OperatorId">An EVSE Operator identification.</param>
        /// <param name="SessionId">The OICP session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<eRoamingAuthorizationStop> AuthorizeStop(EVSEOperator_Id      OperatorId,
                                                                   ChargingSession_Id   SessionId,
                                                                   Auth_Token           AuthToken,
                                                                   EVSE_Id              EVSEId            = null,
                                                                   ChargingSession_Id   PartnerSessionId  = null,   // [maxlength: 50]
                                                                   TimeSpan?            QueryTimeout      = null)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId), "The given parameter must not be null!");

            if (SessionId  == null)
                throw new ArgumentNullException(nameof(SessionId),  "The given parameter must not be null!");

            if (AuthToken  == null)
                throw new ArgumentNullException(nameof(AuthToken),  "The given parameter must not be null!");

            #endregion

            var result = await _CPOClient.AuthorizeStop(OperatorId,
                                                        SessionId,
                                                        AuthToken,
                                                        EVSEId,
                                                        PartnerSessionId,
                                                        QueryTimeout);

            //ToDo: Process the HTTP!
            return result.Content;

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
                throw new ArgumentNullException(nameof(OperatorId), "The given parameter must not be null!");

            #endregion

            var result = await _CPOClient.PullAuthenticationData(OperatorId,
                                                                 QueryTimeout);

            //ToDo: Process the HTTP!
            return result.Content;

        }

        #endregion


        #region SendChargeDetailRecord(ChargeDetailRecord, QueryTimeout = null)

        /// <summary>
        /// Create an OICP SendChargeDetailRecord request.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="QueryTimeout">An optional timeout for this query.</param>
        public async Task<eRoamingAcknowledgement>

            SendChargeDetailRecord(eRoamingChargeDetailRecord  ChargeDetailRecord,
                                   TimeSpan?                   QueryTimeout  = null)

        {

            #region Initial checks

            if (ChargeDetailRecord == null)
                throw new ArgumentNullException("ChargeDetailRecord", "The given parameter must not be null!");

            #endregion

            var result = await _CPOClient.SendChargeDetailRecord(ChargeDetailRecord,
                                                                 QueryTimeout);

            //ToDo: Process the HTTP!
            return result.Content;

        }

        #endregion

        #region SendChargeDetailRecord(EVSEId, SessionId, PartnerProductId, SessionStart, SessionEnd, Identification, PartnerSessionId = null, ..., QueryTimeout = null)

        /// <summary>
        /// Create an OICP v2.0 SendChargeDetailRecord request.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="SessionId">The OICP session identification from the Authorize Start request.</param>
        /// <param name="PartnerProductId"></param>
        /// <param name="SessionStart">The timestamp of the session start.</param>
        /// <param name="SessionEnd">The timestamp of the session end.</param>
        /// <param name="Identification">An ev customer identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
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
        public async Task<eRoamingAcknowledgement>

            SendChargeDetailRecord(EVSE_Id                      EVSEId,
                                   ChargingSession_Id           SessionId,
                                   ChargingProduct_Id           PartnerProductId,
                                   DateTime                     SessionStart,
                                   DateTime                     SessionEnd,
                                   AuthorizationIdentification  Identification,
                                   ChargingSession_Id           PartnerSessionId      = null,
                                   DateTime?                    ChargingStart         = null,
                                   DateTime?                    ChargingEnd           = null,
                                   Double?                      MeterValueStart       = null,
                                   Double?                      MeterValueEnd         = null,
                                   IEnumerable<Double>          MeterValuesInBetween  = null,
                                   Double?                      ConsumedEnergy        = null,
                                   String                       MeteringSignature     = null,
                                   HubOperator_Id               HubOperatorId         = null,
                                   EVSP_Id                      HubProviderId         = null,
                                   TimeSpan?                    QueryTimeout          = null)

        {

            #region Initial checks

            if (EVSEId           == null)
                throw new ArgumentNullException(nameof(EVSEId),            "The given parameter must not be null!");

            if (SessionId        == null)
                throw new ArgumentNullException(nameof(SessionId),         "The given parameter must not be null!");

            if (PartnerProductId == null)
                throw new ArgumentNullException("PartnerProductId",  "The given parameter must not be null!");

            if (SessionStart     == null)
                throw new ArgumentNullException("SessionStart",      "The given parameter must not be null!");

            if (SessionEnd       == null)
                throw new ArgumentNullException("SessionEnd",        "The given parameter must not be null!");

            if (Identification   == null)
                throw new ArgumentNullException("Identification",    "The given parameter must not be null!");

            #endregion

            var result = await _CPOClient.SendChargeDetailRecord(
                                              new eRoamingChargeDetailRecord(
                                                  EVSEId,
                                                  SessionId,
                                                  PartnerProductId,
                                                  SessionStart,
                                                  SessionEnd,
                                                  Identification,
                                                  PartnerSessionId,
                                                  ChargingStart,
                                                  ChargingEnd,
                                                  MeterValueStart,
                                                  MeterValueEnd,
                                                  MeterValuesInBetween,
                                                  ConsumedEnergy,
                                                  MeteringSignature,
                                                  HubOperatorId,
                                                  HubProviderId),
                                              QueryTimeout);

            //ToDo: Process the HTTP!
            return result.Content;

        }

        #endregion



        #region Start()

        public void Start()
        {
            _CPOServer.Start();
        }

        #endregion

        #region Shutdown(Message = null, Wait = true)

        public void Shutdown(String Message = null, Boolean Wait = true)
        {
            _CPOServer.Shutdown(Message, Wait);
        }

        #endregion


    }

}
