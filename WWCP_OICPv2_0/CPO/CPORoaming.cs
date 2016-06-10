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
    /// An OICP roaming client for CPOs which combines the CPO client
    /// and server and adds additional logging for both.
    /// </summary>
    public class CPORoaming
    {

        #region Properties

        /// <summary>
        /// The CPO client.
        /// </summary>
        public CPOClient        CPOClient         { get; }

        /// <summary>
        /// The CPO server.
        /// </summary>
        public CPOServer        CPOServer         { get; }

        /// <summary>
        /// The CPO client logger.
        /// </summary>
        public CPOClientLogger  CPOClientLogger   { get; }

        /// <summary>
        /// The CPO server logger.
        /// </summary>
        public CPOServerLogger  CPOServerLogger   { get; }

        /// <summary>
        /// The DNS client defines which DNS servers to use.
        /// </summary>
        public DNSClient DNSClient
            => CPOServer.DNSClient;

        #endregion

        #region Events

        // Client methods (logging)

        #region OnEVSEDataPush/-Pushed

        /// <summary>
        /// An event fired whenever a request pushing EVSE data records will be send.
        /// </summary>
        public event OnEVSEDataPushDelegate OnEVSEDataPush
        {

            add
            {
                CPOClient.OnEVSEDataPush += value;
            }

            remove
            {
                CPOClient.OnEVSEDataPush -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request pushing EVSE data records will be send.
        /// </summary>
        public event ClientRequestLogHandler OnEVSEDataPushRequest
        {

            add
            {
                CPOClient.OnEVSEDataPushRequest += value;
            }

            remove
            {
                CPOClient.OnEVSEDataPushRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a push EVSE data records SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnEVSEDataPushResponse
        {

            add
            {
                CPOClient.OnEVSEDataPushResponse += value;
            }

            remove
            {
                CPOClient.OnEVSEDataPushResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever EVSE data records had been sent upstream.
        /// </summary>
        public event OnEVSEDataPushedDelegate OnEVSEDataPushed
        {

            add
            {
                CPOClient.OnEVSEDataPushed += value;
            }

            remove
            {
                CPOClient.OnEVSEDataPushed -= value;
            }

        }

        #endregion

        #region OnEVSEStatusPush/-Pushed

        /// <summary>
        /// An event fired whenever a request pushing EVSE status records will be send.
        /// </summary>
        public event OnEVSEStatusPushDelegate OnEVSEStatusPush
        {

            add
            {
                CPOClient.OnEVSEStatusPush += value;
            }

            remove
            {
                CPOClient.OnEVSEStatusPush -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request pushing EVSE status records will be send.
        /// </summary>
        public event ClientRequestLogHandler OnEVSEStatusPushRequest
        {

            add
            {
                CPOClient.OnEVSEStatusPushRequest += value;
            }

            remove
            {
                CPOClient.OnEVSEStatusPushRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a push EVSE status records SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnEVSEStatusPushResponse
        {

            add
            {
                CPOClient.OnEVSEStatusPushResponse += value;
            }

            remove
            {
                CPOClient.OnEVSEStatusPushResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever EVSE status records had been sent upstream.
        /// </summary>
        public event OnEVSEStatusPushedDelegate OnEVSEStatusPushed
        {

            add
            {
                CPOClient.OnEVSEStatusPushed += value;
            }

            remove
            {
                CPOClient.OnEVSEStatusPushed -= value;
            }

        }

        #endregion

        #region OnAuthorizeStartRequest/-Response

        /// <summary>
        /// An event fired whenever an authorize start request will be send.
        /// </summary>
        public event OnAuthorizeStartHandler OnAuthorizeStart
        {

            add
            {
                CPOClient.OnAuthorizeStart += value;
            }

            remove
            {
                CPOClient.OnAuthorizeStart -= value;
            }

        }

        /// <summary>
        /// An event fired whenever an authorize start SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnAuthorizeStartRequest
        {

            add
            {
                CPOClient.OnAuthorizeStartRequest += value;
            }

            remove
            {
                CPOClient.OnAuthorizeStartRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an authorize start SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnAuthorizeStartResponse
        {

            add
            {
                CPOClient.OnAuthorizeStartResponse += value;
            }

            remove
            {
                CPOClient.OnAuthorizeStartResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever an authorize start request was sent.
        /// </summary>
        public event OnAuthorizeStartedHandler OnAuthorizeStarted
        {

            add
            {
                CPOClient.OnAuthorizeStarted += value;
            }

            remove
            {
                CPOClient.OnAuthorizeStarted -= value;
            }

        }

        #endregion

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event fired whenever an authorize stop request will be send.
        /// </summary>
        public event OnAuthorizeStopHandler OnAuthorizeStop
        {

            add
            {
                CPOClient.OnAuthorizeStop += value;
            }

            remove
            {
                CPOClient.OnAuthorizeStop -= value;
            }

        }

        /// <summary>
        /// An event fired whenever an authorize stop SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnAuthorizeStopRequest
        {

            add
            {
                CPOClient.OnAuthorizeStopRequest += value;
            }

            remove
            {
                CPOClient.OnAuthorizeStopRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an authorize stop SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnAuthorizeStopResponse
        {

            add
            {
                CPOClient.OnAuthorizeStopResponse += value;
            }

            remove
            {
                CPOClient.OnAuthorizeStopResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever an authorize start request was sent.
        /// </summary>
        public event OnAuthorizeStoppedHandler OnAuthorizeStopped
        {

            add
            {
                CPOClient.OnAuthorizeStopped += value;
            }

            remove
            {
                CPOClient.OnAuthorizeStopped -= value;
            }

        }

        #endregion

        #region OnSendChargeDetailRecordRequest/-Response

        /// <summary>
        /// An event fired whenever a charge detail record will be send.
        /// </summary>
        public event OnSendChargeDetailRecordHandler OnSendChargeDetailRecord
        {

            add
            {
                CPOClient.OnSendChargeDetailRecord += value;
            }

            remove
            {
                CPOClient.OnSendChargeDetailRecord -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a charge detail record will be send via SOAP.
        /// </summary>
        public event ClientRequestLogHandler OnSendChargeDetailRecordRequest
        {

            add
            {
                CPOClient.OnSendChargeDetailRecordRequest += value;
            }

            remove
            {
                CPOClient.OnSendChargeDetailRecordRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP response to a sent charge detail record had been received.
        /// </summary>
        public event ClientResponseLogHandler OnSendChargeDetailRecordResponse
        {

            add
            {
                CPOClient.OnSendChargeDetailRecordResponse += value;
            }

            remove
            {
                CPOClient.OnSendChargeDetailRecordResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a sent charge detail record had been received.
        /// </summary>
        public event OnChargeDetailRecordSentHandler OnChargeDetailRecordSent
        {

            add
            {
                CPOClient.OnChargeDetailRecordSent += value;
            }

            remove
            {
                CPOClient.OnChargeDetailRecordSent -= value;
            }

        }

        #endregion

        #region OnPullAuthenticationDataRequest/-Response

        /// <summary>
        /// An event fired whenever a request pulling authentication data will be send.
        /// </summary>
        public event OnPullAuthenticationDataHandler OnPullAuthenticationData
        {

            add
            {
                CPOClient.OnPullAuthenticationData += value;
            }

            remove
            {
                CPOClient.OnPullAuthenticationData -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request pulling authentication data will be send.
        /// </summary>
        public event ClientRequestLogHandler OnPullAuthenticationDataRequest
        {

            add
            {
                CPOClient.OnPullAuthenticationDataRequest += value;
            }

            remove
            {
                CPOClient.OnPullAuthenticationDataRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a pull authentication data SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnPullAuthenticationDataResponse
        {

            add
            {
                CPOClient.OnPullAuthenticationDataResponse += value;
            }

            remove
            {
                CPOClient.OnPullAuthenticationDataResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a pull authentication data request was received.
        /// </summary>
        public event OnAuthenticationDataPulledHandler OnAuthenticationDataPulled
        {

            add
            {
                CPOClient.OnAuthenticationDataPulled += value;
            }

            remove
            {
                CPOClient.OnAuthenticationDataPulled -= value;
            }

        }

        #endregion


        // Server methods via events

        #region OnRemoteStart/-Stop

        /// <summary>
        /// An event sent whenever a remote start command was received.
        /// </summary>
        public event OnRemoteStartDelegate OnRemoteStart
        {

            add
            {
                CPOServer.OnRemoteStart += value;
            }

            remove
            {
                CPOServer.OnRemoteStart -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopDelegate OnRemoteStop

        {

            add
            {
                CPOServer.OnRemoteStop += value;
            }

            remove
            {
                CPOServer.OnRemoteStop -= value;
            }

        }

        #endregion


        // Generic HTTP/SOAP server logging

        #region RequestLog

        /// <summary>
        /// An event called whenever a request came in.
        /// </summary>
        public event RequestLogHandler RequestLog
        {

            add
            {
                CPOServer.RequestLog += value;
            }

            remove
            {
                CPOServer.RequestLog -= value;
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
                CPOServer.AccessLog += value;
            }

            remove
            {
                CPOServer.AccessLog -= value;
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
                CPOServer.ErrorLog += value;
            }

            remove
            {
                CPOServer.ErrorLog -= value;
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

            this.CPOClient        = CPOClient;
            this.CPOServer        = CPOServer;
            this.CPOClientLogger  = new CPOClientLogger(CPOClient, ClientLoggingContext, LogFileCreator);
            this.CPOServerLogger  = new CPOServerLogger(CPOServer, ServerLoggingContext, LogFileCreator);

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

            var result = await CPOClient.PushEVSEData(GroupedEVSEs,
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

            var result = await CPOClient.PushEVSEData(EVSEDataRecord,
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

            var result = await CPOClient.PushEVSEData(EVSEDataRecords,
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

            var result = await CPOClient.PushEVSEData(OICPAction,
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

            var result = await CPOClient.PushEVSEData(OICPAction,
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

            var result = await CPOClient.PushEVSEData(OICPAction,
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

            var result = await CPOClient.PushEVSEStatus(EVSEStatusRecords,
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

            var result = await CPOClient.PushEVSEStatus(EVSEStatus,
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

            var result = await CPOClient.AuthorizeStart(OperatorId,
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

            var result = await CPOClient.AuthorizeStop(OperatorId,
                                                        SessionId,
                                                        AuthToken,
                                                        EVSEId,
                                                        PartnerSessionId,
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

            SendChargeDetailRecord(ChargeDetailRecord  ChargeDetailRecord,
                                   TimeSpan?           QueryTimeout  = null)

        {

            #region Initial checks

            if (ChargeDetailRecord == null)
                throw new ArgumentNullException(nameof(ChargeDetailRecord), "The given charge detail record must not be null!");

            #endregion

            var result = await CPOClient.SendChargeDetailRecord(ChargeDetailRecord,
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

            var result = await CPOClient.PullAuthenticationData(OperatorId,
                                                                 QueryTimeout);

            //ToDo: Process the HTTP!
            return result.Content;

        }

        #endregion



        #region Start()

        public void Start()
        {
            CPOServer.Start();
        }

        #endregion

        #region Shutdown(Message = null, Wait = true)

        public void Shutdown(String Message = null, Boolean Wait = true)
        {
            CPOServer.Shutdown(Message, Wait);
        }

        #endregion


    }

}
