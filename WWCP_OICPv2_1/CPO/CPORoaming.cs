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
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
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

        // CPOClient logging methods

        #region OnPushEVSEDataRequest/-Response

        /// <summary>
        /// An event fired whenever a request pushing EVSE data records will be send.
        /// </summary>
        public event OnPushEVSEDataRequestDelegate OnPushEVSEDataRequest
        {

            add
            {
                CPOClient.OnPushEVSEDataRequest += value;
            }

            remove
            {
                CPOClient.OnPushEVSEDataRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request pushing EVSE data records will be send.
        /// </summary>
        public event ClientRequestLogHandler OnPushEVSEDataSOAPRequest
        {

            add
            {
                CPOClient.OnPushEVSEDataSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnPushEVSEDataSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a push EVSE data records SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnPushEVSEDataSOAPResponse
        {

            add
            {
                CPOClient.OnPushEVSEDataSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnPushEVSEDataSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever EVSE data records had been sent upstream.
        /// </summary>
        public event OnPushEVSEDataResponseDelegate OnPushEVSEDataResponse
        {

            add
            {
                CPOClient.OnPushEVSEDataResponse += value;
            }

            remove
            {
                CPOClient.OnPushEVSEDataResponse -= value;
            }

        }

        #endregion

        #region OnPushEVSEStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request pushing EVSE status records will be send.
        /// </summary>
        public event OnPushEVSEStatusRequestDelegate OnPushEVSEStatusRequest
        {

            add
            {
                CPOClient.OnPushEVSEStatusRequest += value;
            }

            remove
            {
                CPOClient.OnPushEVSEStatusRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request pushing EVSE status records will be send.
        /// </summary>
        public event ClientRequestLogHandler OnPushEVSEStatusSOAPRequest
        {

            add
            {
                CPOClient.OnPushEVSEStatusSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnPushEVSEStatusSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a push EVSE status records SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnPushEVSEStatusSOAPResponse
        {

            add
            {
                CPOClient.OnPushEVSEStatusSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnPushEVSEStatusSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever EVSE status records had been sent upstream.
        /// </summary>
        public event OnPushEVSEStatusResponseDelegate OnPushEVSEStatusResponse
        {

            add
            {
                CPOClient.OnPushEVSEStatusResponse += value;
            }

            remove
            {
                CPOClient.OnPushEVSEStatusResponse -= value;
            }

        }

        #endregion

        #region OnAuthorizeStartRequest/-Response

        /// <summary>
        /// An event fired whenever an authorize start request will be send.
        /// </summary>
        public event OnAuthorizeStartHandler OnAuthorizeStartRequest
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
        /// An event fired whenever an authorize start SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnAuthorizeStartSOAPRequest
        {

            add
            {
                CPOClient.OnAuthorizeStartSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnAuthorizeStartSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an authorize start SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnAuthorizeStartSOAPResponse
        {

            add
            {
                CPOClient.OnAuthorizeStartSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnAuthorizeStartSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever an authorize start request was sent.
        /// </summary>
        public event OnAuthorizeStartedHandler OnAuthorizeStartResponse
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

        #endregion

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event fired whenever an authorize stop request will be send.
        /// </summary>
        public event OnAuthorizeStopRequestHandler OnAuthorizeStopRequest
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
        /// An event fired whenever an authorize stop SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnAuthorizeStopSOAPRequest
        {

            add
            {
                CPOClient.OnAuthorizeStopSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnAuthorizeStopSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an authorize stop SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnAuthorizeStopSOAPResponse
        {

            add
            {
                CPOClient.OnAuthorizeStopSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnAuthorizeStopSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever an authorize start request was sent.
        /// </summary>
        public event OnAuthorizeStopResponseHandler OnAuthorizeStopResponse
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

        #endregion

        #region OnSendChargeDetailRecordRequest/-Response

        /// <summary>
        /// An event fired whenever a charge detail record will be send.
        /// </summary>
        public event OnSendChargeDetailRecordRequestHandler OnSendChargeDetailRecordRequest
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
        /// An event fired whenever a charge detail record will be send via SOAP.
        /// </summary>
        public event ClientRequestLogHandler OnSendChargeDetailRecordSOAPRequest
        {

            add
            {
                CPOClient.OnSendChargeDetailRecordSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnSendChargeDetailRecordSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP response to a sent charge detail record had been received.
        /// </summary>
        public event ClientResponseLogHandler OnSendChargeDetailRecordSOAPResponse
        {

            add
            {
                CPOClient.OnSendChargeDetailRecordSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnSendChargeDetailRecordSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a sent charge detail record had been received.
        /// </summary>
        public event OnSendChargeDetailRecordResponseHandler OnSendChargeDetailRecordResponse
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

        #endregion

        #region OnPullAuthenticationDataRequest/-Response

        /// <summary>
        /// An event fired whenever a request pulling authentication data will be send.
        /// </summary>
        public event OnPullAuthenticationDataRequestHandler OnPullAuthenticationDataRequest
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
        /// An event fired whenever a SOAP request pulling authentication data will be send.
        /// </summary>
        public event ClientRequestLogHandler OnPullAuthenticationDataSOAPRequest
        {

            add
            {
                CPOClient.OnPullAuthenticationDataSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnPullAuthenticationDataSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a pull authentication data SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnPullAuthenticationDataSOAPResponse
        {

            add
            {
                CPOClient.OnPullAuthenticationDataSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnPullAuthenticationDataSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a pull authentication data request was received.
        /// </summary>
        public event OnPullAuthenticationDataResponseHandler OnPullAuthenticationDataResponse
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

        #endregion


        // CPOServer methods

        #region OnRemoteReservationStart/-Stop

        /// <summary>
        /// An event sent whenever a remote reservation start command was received.
        /// </summary>
        public event OnRemoteReservationStartDelegate OnRemoteReservationStart
        {

            add
            {
                CPOServer.OnRemoteReservationStart += value;
            }

            remove
            {
                CPOServer.OnRemoteReservationStart -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a remote reservation stop command was received.
        /// </summary>
        public event OnRemoteReservationStopDelegate OnRemoteReservationStop

        {

            add
            {
                CPOServer.OnRemoteReservationStop += value;
            }

            remove
            {
                CPOServer.OnRemoteReservationStop -= value;
            }

        }

        #endregion

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

        #region CPORoaming(CPOClient, CPOServer, ServerLoggingContext = CPOServerLogger.DefaultContext, LogFileCreator = null)

        /// <summary>
        /// Create a new OICP roaming client for CPOs.
        /// </summary>
        /// <param name="CPOClient">A CPO client.</param>
        /// <param name="CPOServer">A CPO sever.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
        public CPORoaming(CPOClient                     CPOClient,
                          CPOServer                     CPOServer,
                          String                        ServerLoggingContext  = CPOServerLogger.DefaultContext,
                          Func<String, String, String>  LogFileCreator        = null)
        {

            this.CPOClient        = CPOClient;
            this.CPOServer        = CPOServer;
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
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCert">The TLS client certificate to use.</param>
        /// <param name="RemoteHTTPVirtualHost">An optional HTTP virtual hostname of the remote OICP service.</param>
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
                          RemoteCertificateValidationCallback  RemoteCertificateValidator  = null,
                          X509Certificate                      ClientCert                  = null,
                          String                               RemoteHTTPVirtualHost       = null,
                          String                               HTTPUserAgent               = CPOClient.DefaultHTTPUserAgent,
                          TimeSpan?                            QueryTimeout                = null,

                          String                               ServerName                  = CPOServer.DefaultHTTPServerName,
                          IPPort                               ServerTCPPort               = null,
                          String                               ServerURIPrefix             = "",
                          Boolean                              ServerAutoStart             = false,

                          String                               ClientLoggingContext        = CPOClient.CPOClientLogger.DefaultContext,
                          String                               ServerLoggingContext        = CPOServerLogger.DefaultContext,
                          Func<String, String, String>         LogFileCreator              = null,

                          DNSClient                            DNSClient                   = null)

            : this(new CPOClient(ClientId,
                                 RemoteHostname,
                                 RemoteTCPPort,
                                 RemoteCertificateValidator,
                                 ClientCert,
                                 RemoteHTTPVirtualHost,
                                 HTTPUserAgent,
                                 QueryTimeout,
                                 DNSClient,
                                 ClientLoggingContext,
                                 LogFileCreator),

                   new CPOServer(ServerName,
                                 ServerTCPPort,
                                 ServerURIPrefix,
                                 DNSClient,
                                 false),

    //               ClientLoggingContext,
                   ServerLoggingContext,
                   LogFileCreator)

        {

            if (ServerAutoStart)
                Start();

        }

        #endregion

        #endregion


        #region PushEVSEData(GroupedEVSEDataRecords,    OICPAction = fullLoad, ...)

        /// <summary>
        /// Upload the given lookup of EVSE data records grouped by their EVSE operator.
        /// </summary>
        /// <param name="GroupedEVSEDataRecords">A lookup of EVSE data records grouped by their EVSE operator.</param>
        /// <param name="OICPAction">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushEVSEData(ILookup<EVSEOperator, EVSEDataRecord>  GroupedEVSEDataRecords,
                         ActionType                             OICPAction         = ActionType.fullLoad,

                         DateTime?                              Timestamp          = null,
                         CancellationToken?                     CancellationToken  = null,
                         EventTracking_Id                       EventTrackingId    = null,
                         TimeSpan?                              RequestTimeout     = null)


            => await CPOClient.PushEVSEData(GroupedEVSEDataRecords,
                                            OICPAction,

                                            Timestamp,
                                            CancellationToken,
                                            EventTrackingId,
                                            RequestTimeout);

        #endregion

        #region PushEVSEData(EVSEDataRecord,  OICPAction = insert, IncludeEVSEDataRecords = null, ...)

        /// <summary>
        /// Create a new task pushing a single EVSE data record onto the OICP server.
        /// </summary>
        /// <param name="EVSEDataRecord">An EVSE data record.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// <param name="IncludeEVSEDataRecords">An optional delegate for filtering EVSE data records before pushing them to the server.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushEVSEData(EVSEDataRecord                 EVSEDataRecord,
                         ActionType                     OICPAction              = ActionType.insert,
                         Func<EVSEDataRecord, Boolean>  IncludeEVSEDataRecords  = null,

                         DateTime?                      Timestamp               = null,
                         CancellationToken?             CancellationToken       = null,
                         EventTracking_Id               EventTrackingId         = null,
                         TimeSpan?                      RequestTimeout          = null)


            => await CPOClient.PushEVSEData(EVSEDataRecord,
                                            OICPAction,
                                            IncludeEVSEDataRecords,

                                            Timestamp,
                                            CancellationToken,
                                            EventTrackingId,
                                            RequestTimeout);

        #endregion

        #region PushEVSEData(EVSEDataRecords, OICPAction = fullLoad, IncludeEVSEDataRecords = null, ...)

        /// <summary>
        /// Upload the given enumeration of EVSE data records.
        /// </summary>
        /// <param name="EVSEDataRecords">An enumeration of EVSE data records.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// <param name="IncludeEVSEDataRecords">An optional delegate for filtering EVSE data records before pushing them to the server.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushEVSEData(IEnumerable<EVSEDataRecord>    EVSEDataRecords,
                         ActionType                     OICPAction              = ActionType.fullLoad,
                         Func<EVSEDataRecord, Boolean>  IncludeEVSEDataRecords  = null,

                         DateTime?                      Timestamp               = null,
                         CancellationToken?             CancellationToken       = null,
                         EventTracking_Id               EventTrackingId         = null,
                         TimeSpan?                      RequestTimeout          = null)


            => await CPOClient.PushEVSEData(EVSEDataRecords,
                                            OICPAction,
                                            IncludeEVSEDataRecords,

                                            Timestamp,
                                            CancellationToken,
                                            EventTrackingId,
                                            RequestTimeout);

        #endregion

        #region PushEVSEData(OICPAction, params EVSEDataRecords)

        /// <summary>
        /// Create a new task pushing EVSE data records onto the OICP server.
        /// </summary>
        /// <param name="OICPAction">The OICP action.</param>
        /// <param name="EVSEDataRecords">An array of EVSE data records.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushEVSEData(ActionType               OICPAction,
                         params EVSEDataRecord[]  EVSEDataRecords)


            => await CPOClient.PushEVSEData(OICPAction,
                                            EVSEDataRecords);

        #endregion


        #region PushEVSEStatus(GroupedEVSEStatusRecords,  OICPAction = fullLoad, ...)

        /// <summary>
        /// Upload the given enumeration of EVSE status records.
        /// </summary>
        /// <param name="GroupedEVSEStatusRecords">A lookup of EVSE status records grouped by their EVSE operator.</param>
        /// <param name="OICPAction">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushEVSEStatus(ILookup<EVSEOperator, EVSEStatusRecord>  GroupedEVSEStatusRecords,
                           ActionType                               OICPAction         = ActionType.fullLoad,

                           DateTime?                                Timestamp          = null,
                           CancellationToken?                       CancellationToken  = null,
                           EventTracking_Id                         EventTrackingId    = null,
                           TimeSpan?                                RequestTimeout     = null)


            => await CPOClient.PushEVSEStatus(GroupedEVSEStatusRecords,
                                              OICPAction,

                                              Timestamp,
                                              CancellationToken,
                                              EventTrackingId,
                                              RequestTimeout);

        #endregion

        #region PushEVSEStatus(EVSEStatusRecord,          OICPAction = insert,   IncludeEVSEStatusRecords = null, ...)

        /// <summary>
        /// Create a new task pushing a single EVSE status record onto the OICP server.
        /// </summary>
        /// <param name="EVSEStatusRecord">An EVSE status record.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// <param name="IncludeEVSEStatusRecords">An optional delegate for filtering EVSE status records before pushing them to the server.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushEVSEStatus(EVSEStatusRecord                  EVSEStatusRecord,
                           ActionType                        OICPAction                = ActionType.insert,
                           IncludeEVSEStatusRecordsDelegate  IncludeEVSEStatusRecords  = null,

                           DateTime?                         Timestamp                 = null,
                           CancellationToken?                CancellationToken         = null,
                           EventTracking_Id                  EventTrackingId           = null,
                           TimeSpan?                         RequestTimeout            = null)


            => await CPOClient.PushEVSEStatus(EVSEStatusRecord,
                                              OICPAction,
                                              IncludeEVSEStatusRecords,

                                              Timestamp,
                                              CancellationToken,
                                              EventTrackingId,
                                              RequestTimeout);

        #endregion

        #region PushEVSEStatus(EVSEStatusRecords,         OICPAction = fullLoad, IncludeEVSEStatusRecords = null, ...)

        /// <summary>
        /// Upload the given enumeration of EVSE status records.
        /// </summary>
        /// <param name="EVSEStatusRecords">An enumeration of EVSE status records.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// <param name="IncludeEVSEStatusRecords">An optional delegate for filtering EVSE status records before pushing them to the server.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushEVSEStatus(IEnumerable<EVSEStatusRecord>     EVSEStatusRecords,
                           ActionType                        OICPAction                = ActionType.fullLoad,
                           IncludeEVSEStatusRecordsDelegate  IncludeEVSEStatusRecords  = null,

                           DateTime?                         Timestamp                 = null,
                           CancellationToken?                CancellationToken         = null,
                           EventTracking_Id                  EventTrackingId           = null,
                           TimeSpan?                         RequestTimeout            = null)


            => await CPOClient.PushEVSEStatus(EVSEStatusRecords,
                                              OICPAction,
                                              IncludeEVSEStatusRecords,

                                              Timestamp,
                                              CancellationToken,
                                              EventTrackingId,
                                              RequestTimeout);

        #endregion

        #region PushEVSEStatus(OICPAction, params EVSEStatusRecords)

        /// <summary>
        /// Create a new task pushing EVSE status records onto the OICP server.
        /// </summary>
        /// <param name="OICPAction">The OICP action.</param>
        /// <param name="EVSEStatusRecords">An array of EVSE status records.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            PushEVSEStatus(ActionType                 OICPAction,
                           params EVSEStatusRecord[]  EVSEStatusRecords)


            => await CPOClient.PushEVSEStatus(OICPAction,
                                              EVSEStatusRecords);

        #endregion


        #region AuthorizeStart(OperatorId, AuthToken, EVSEId = null, SessionId = null, PartnerProductId = null, PartnerSessionId = null, ...)

        /// <summary>
        /// Create an OICP authorize start request.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="PartnerProductId">An optional partner product identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<eRoamingAuthorizationStart>>

            AuthorizeStart(EVSEOperator_Id     OperatorId,
                           Auth_Token          AuthToken,
                           EVSE_Id             EVSEId             = null,
                           ChargingSession_Id  SessionId          = null,
                           ChargingProduct_Id  PartnerProductId   = null,
                           ChargingSession_Id  PartnerSessionId   = null,

                           DateTime?           Timestamp          = null,
                           CancellationToken?  CancellationToken  = null,
                           EventTracking_Id    EventTrackingId    = null,
                           TimeSpan?           RequestTimeout     = null)


            => await CPOClient.AuthorizeStart(OperatorId,
                                              AuthToken,
                                              EVSEId,
                                              SessionId,
                                              PartnerProductId,
                                              PartnerSessionId,

                                              Timestamp,
                                              CancellationToken,
                                              EventTrackingId,
                                              RequestTimeout);

        #endregion

        #region AuthorizeStop (OperatorId, SessionId, AuthToken, EVSEId = null, PartnerSessionId = null, ...)

        // UID => Not everybody can stop any session, but maybe another
        //        UID than the UID which started the session!
        //        (e.g. car sharing)

        /// <summary>
        /// Create an OICP authorize stop request.
        /// </summary>
        /// <param name="OperatorId">An EVSE Operator identification.</param>
        /// <param name="SessionId">The OICP session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<eRoamingAuthorizationStop>>

            AuthorizeStop(EVSEOperator_Id     OperatorId,
                          ChargingSession_Id  SessionId,
                          Auth_Token          AuthToken,
                          EVSE_Id             EVSEId             = null,
                          ChargingSession_Id  PartnerSessionId   = null,

                          DateTime?           Timestamp          = null,
                          CancellationToken?  CancellationToken  = null,
                          EventTracking_Id    EventTrackingId    = null,
                          TimeSpan?           RequestTimeout     = null)


            => await CPOClient.AuthorizeStop(OperatorId,
                                             SessionId,
                                             AuthToken,
                                             EVSEId,
                                             PartnerSessionId,

                                             Timestamp,
                                             CancellationToken,
                                             EventTrackingId,
                                             RequestTimeout);

        #endregion

        #region SendChargeDetailRecord(ChargeDetailRecord, ...)

        /// <summary>
        /// Send a charge detail record to an OICP server.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<eRoamingAcknowledgement>>

            SendChargeDetailRecord(ChargeDetailRecord  ChargeDetailRecord,

                                   DateTime?           Timestamp          = null,
                                   CancellationToken?  CancellationToken  = null,
                                   EventTracking_Id    EventTrackingId    = null,
                                   TimeSpan?           RequestTimeout     = null)


            => await CPOClient.SendChargeDetailRecord(ChargeDetailRecord,

                                                      Timestamp,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      RequestTimeout);

        #endregion


        #region PullAuthenticationData(OperatorId, ...)

        /// <summary>
        /// Pull authentication data from the OICP server.
        /// </summary>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<eRoamingAuthenticationData>>

            PullAuthenticationData(EVSEOperator_Id     OperatorId,

                                   DateTime?           Timestamp          = null,
                                   CancellationToken?  CancellationToken  = null,
                                   EventTracking_Id    EventTrackingId    = null,
                                   TimeSpan?           RequestTimeout     = null)


            => await CPOClient.PullAuthenticationData(OperatorId,

                                                      Timestamp,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      RequestTimeout);

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
