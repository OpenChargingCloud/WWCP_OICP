/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;
using System.Threading;
using System.Net.Security;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// An OICP roaming client for EMPs.
    /// </summary>
    public class EMPRoaming
    {

        #region Properties

        public Authorizator_Id  AuthorizatorId      { get; }

        /// <summary>
        /// The EMP client part.
        /// </summary>
        public EMPClient        EMPClient           { get; }

        /// <summary>
        /// The EMP server part.
        /// </summary>
        public EMPServer        EMPServer           { get; }

        /// <summary>
        /// The EMP server logger.
        /// </summary>
        public EMPServerLogger  EMPServerLogger     { get; }

        /// <summary>
        /// The DNS client defines which DNS servers to use.
        /// </summary>
        public DNSClient DNSClient
            => EMPServer?.DNSClient;

        #endregion

        #region Events

        // EMPClient logging methods

        #region OnPullEVSEDataRequest/-Response

        /// <summary>
        /// An event fired whenever a 'pull EVSE data' request will be send.
        /// </summary>
        public event OnPullEVSEDataRequestHandler OnPullEVSEDataRequest
        {

            add
            {
                EMPClient.OnPullEVSEDataRequest += value;
            }

            remove
            {
                EMPClient.OnPullEVSEDataRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a 'pull EVSE data' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnPullEVSEDataSOAPRequest
        {

            add
            {
                EMPClient.OnPullEVSEDataSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnPullEVSEDataSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE data' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnPullEVSEDataSOAPResponse
        {

            add
            {
                EMPClient.OnPullEVSEDataSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnPullEVSEDataSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE data' request had been received.
        /// </summary>
        public event OnPullEVSEDataResponseHandler OnPullEVSEDataResponse
        {

            add
            {
                EMPClient.OnPullEVSEDataResponse += value;
            }

            remove
            {
                EMPClient.OnPullEVSEDataResponse -= value;
            }

        }

        #endregion

        #region OnSearchEVSERequest/-Response

        /// <summary>
        /// An event fired whenever a 'search EVSE' request will be send.
        /// </summary>
        public event OnSearchEVSERequestHandler OnSearchEVSERequest
        {

            add
            {
                EMPClient.OnSearchEVSERequest += value;
            }

            remove
            {
                EMPClient.OnSearchEVSERequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a 'search EVSE' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnSearchEVSESOAPRequest
        {

            add
            {
                EMPClient.OnSearchEVSESOAPRequest += value;
            }

            remove
            {
                EMPClient.OnSearchEVSESOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'search EVSE' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnSearchEVSESOAPResponse
        {

            add
            {
                EMPClient.OnSearchEVSESOAPResponse += value;
            }

            remove
            {
                EMPClient.OnSearchEVSESOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'search EVSE' request had been received.
        /// </summary>
        public event OnSearchEVSEResponseHandler OnSearchEVSEResponse
        {

            add
            {
                EMPClient.OnSearchEVSEResponse += value;
            }

            remove
            {
                EMPClient.OnSearchEVSEResponse -= value;
            }

        }

        #endregion

        #region OnPullEVSEStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a 'pull EVSE status' request will be send.
        /// </summary>
        public event OnPullEVSEStatusRequestHandler OnPullEVSEStatusRequest
        {

            add
            {
                EMPClient.OnPullEVSEStatusRequest += value;
            }

            remove
            {
                EMPClient.OnPullEVSEStatusRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a 'pull EVSE status' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnPullEVSEStatusSOAPRequest
        {

            add
            {
                EMPClient.OnPullEVSEStatusSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnPullEVSEStatusSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE status' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnPullEVSEStatusSOAPResponse
        {

            add
            {
                EMPClient.OnPullEVSEStatusSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnPullEVSEStatusSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE status' request had been received.
        /// </summary>
        public event OnPullEVSEStatusResponseHandler OnPullEVSEStatusResponse
        {

            add
            {
                EMPClient.OnPullEVSEStatusResponse += value;
            }

            remove
            {
                EMPClient.OnPullEVSEStatusResponse -= value;
            }

        }

        #endregion

        #region OnPullEVSEStatusByIdRequest/-Response

        /// <summary>
        /// An event fired whenever a 'pull EVSE status by id' request will be send.
        /// </summary>
        public event OnPullEVSEStatusByIdRequestHandler OnPullEVSEStatusByIdRequest
        {

            add
            {
                EMPClient.OnPullEVSEStatusByIdRequest += value;
            }

            remove
            {
                EMPClient.OnPullEVSEStatusByIdRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a 'pull EVSE status by id' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnPullEVSEStatusByIdSOAPRequest
        {

            add
            {
                EMPClient.OnPullEVSEStatusByIdSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnPullEVSEStatusByIdSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE status by id' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnPullEVSEStatusByIdSOAPResponse
        {

            add
            {
                EMPClient.OnPullEVSEStatusByIdSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnPullEVSEStatusByIdSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE status by id' request had been received.
        /// </summary>
        public event OnPullEVSEStatusByIdResponseHandler OnPullEVSEStatusByIdResponse
        {

            add
            {
                EMPClient.OnPullEVSEStatusByIdResponse += value;
            }

            remove
            {
                EMPClient.OnPullEVSEStatusByIdResponse -= value;
            }

        }

        #endregion

        #region OnPushAuthenticationDataRequest/-Response

        /// <summary>
        /// An event fired whenever a 'push authentication data' request will be send.
        /// </summary>
        public event OnPushAuthenticationDataRequestHandler OnPushAuthenticationDataRequest
        {

            add
            {
                EMPClient.OnPushAuthenticationDataRequest += value;
            }

            remove
            {
                EMPClient.OnPushAuthenticationDataRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a 'push authentication data' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnPushAuthenticationDataSOAPRequest
        {

            add
            {
                EMPClient.OnPushAuthenticationDataSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnPushAuthenticationDataSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'push authentication data' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnPushAuthenticationDataSOAPResponse
        {

            add
            {
                EMPClient.OnPushAuthenticationDataSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnPushAuthenticationDataSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'push authentication data' request had been received.
        /// </summary>
        public event OnPushAuthenticationDataResponseHandler OnPushAuthenticationDataResponse
        {

            add
            {
                EMPClient.OnPushAuthenticationDataResponse += value;
            }

            remove
            {
                EMPClient.OnPushAuthenticationDataResponse -= value;
            }

        }

        #endregion

        #region OnReservationStartRequest/-Response

        /// <summary>
        /// An event fired whenever a 'reservation start' request will be send.
        /// </summary>
        public event OnReservationStartRequestHandler OnReservationStartRequest
        {

            add
            {
                EMPClient.OnReservationStartRequest += value;
            }

            remove
            {
                EMPClient.OnReservationStartRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a 'reservation start' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnReservationStartSOAPRequest
        {

            add
            {
                EMPClient.OnReservationStartSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnReservationStartSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'reservation start' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnReservationStartSOAPResponse
        {

            add
            {
                EMPClient.OnReservationStartSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnReservationStartSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'reservation start' request had been received.
        /// </summary>
        public event OnReservationStartResponseHandler OnReservationStartResponse
        {

            add
            {
                EMPClient.OnReservationStartResponse += value;
            }

            remove
            {
                EMPClient.OnReservationStartResponse -= value;
            }

        }

        #endregion

        #region OnReservationStopRequest/-Response

        /// <summary>
        /// An event fired whenever a 'reservation stop' request will be send.
        /// </summary>
        public event OnReservationStopRequestHandler OnReservationStopRequest
        {

            add
            {
                EMPClient.OnReservationStopRequest += value;
            }

            remove
            {
                EMPClient.OnReservationStopRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a 'reservation stop' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnReservationStopSOAPRequest
        {

            add
            {
                EMPClient.OnReservationStopSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnReservationStopSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'reservation stop' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnReservationStopSOAPResponse
        {

            add
            {
                EMPClient.OnReservationStopSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnReservationStopSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'reservation stop' request had been received.
        /// </summary>
        public event OnReservationStopResponseHandler OnReservationStopResponse
        {

            add
            {
                EMPClient.OnReservationStopResponse += value;
            }

            remove
            {
                EMPClient.OnReservationStopResponse -= value;
            }

        }

        #endregion

        #region OnAuthorizeRemoteStartRequest/-Response

        /// <summary>
        /// An event fired whenever an 'authorize remote start' request will be send.
        /// </summary>
        public event OnAuthorizeRemoteStartRequestHandler OnAuthorizeRemoteStartRequest
        {

            add
            {
                EMPClient.OnAuthorizeRemoteStartRequest += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteStartRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever an 'authorize remote start' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnAuthorizeRemoteStartSOAPRequest
        {

            add
            {
                EMPClient.OnAuthorizeRemoteStartSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteStartSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an 'authorize remote start' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnAuthorizeRemoteStartSOAPResponse
        {

            add
            {
                EMPClient.OnAuthorizeRemoteStartSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteStartSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an 'authorize remote start' request had been received.
        /// </summary>
        public event OnAuthorizeRemoteStartResponseHandler OnAuthorizeRemoteStartResponse
        {

            add
            {
                EMPClient.OnAuthorizeRemoteStartResponse += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteStartResponse -= value;
            }

        }

        #endregion

        #region OnAuthorizeRemoteStopRequest/-Response

        /// <summary>
        /// An event fired whenever an 'authorize remote stop' request will be send.
        /// </summary>
        public event OnAuthorizeRemoteStopRequestHandler OnAuthorizeRemoteStopRequest
        {

            add
            {
                EMPClient.OnAuthorizeRemoteStopRequest += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteStopRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever an 'authorize remote stop' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnAuthorizeRemoteStopSOAPRequest
        {

            add
            {
                EMPClient.OnAuthorizeRemoteStopSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteStopSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an 'authorize remote stop' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnAuthorizeRemoteStopSOAPResponse
        {

            add
            {
                EMPClient.OnAuthorizeRemoteStopSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteStopSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an 'authorize remote stop' request had been received.
        /// </summary>
        public event OnAuthorizeRemoteStopResponseHandler OnAuthorizeRemoteStopResponse
        {

            add
            {
                EMPClient.OnAuthorizeRemoteStopResponse += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteStopResponse -= value;
            }

        }

        #endregion

        #region OnGetChargeDetailRecordsRequest/-Response

        /// <summary>
        /// An event fired whenever a 'get charge detail records' request will be send.
        /// </summary>
        public event OnGetChargeDetailRecordsRequestHandler OnGetChargeDetailRecordsRequest
        {

            add
            {
                EMPClient.OnGetChargeDetailRecordsRequest += value;
            }

            remove
            {
                EMPClient.OnGetChargeDetailRecordsRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a 'get charge detail records' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnGetChargeDetailRecordsSOAPRequest
        {

            add
            {
                EMPClient.OnGetChargeDetailRecordsSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnGetChargeDetailRecordsSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'get charge detail records' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnGetChargeDetailRecordsSOAPResponse
        {

            add
            {
                EMPClient.OnGetChargeDetailRecordsSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnGetChargeDetailRecordsSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response for a 'get charge detail records' request was received.
        /// </summary>
        public event OnGetChargeDetailRecordsResponseHandler OnGetChargeDetailRecordsResponse
        {

            add
            {
                EMPClient.OnGetChargeDetailRecordsResponse += value;
            }

            remove
            {
                EMPClient.OnGetChargeDetailRecordsResponse -= value;
            }

        }

        #endregion


        // EMPServer methods

        #region OnAuthorizeStart

        /// <summary>
        /// An event sent whenever a authorize start command was received.
        /// </summary>
        public event RequestLogHandler OnLogAuthorizeStart
        {

            add
            {
                EMPServer.OnLogAuthorizeStart += value;
            }

            remove
            {
                EMPServer.OnLogAuthorizeStart -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a authorize start response was sent.
        /// </summary>
        public event AccessLogHandler OnLogAuthorizeStarted
        {

            add
            {
                EMPServer.OnLogAuthorizeStarted += value;
            }

            remove
            {
                EMPServer.OnLogAuthorizeStarted -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a authorize start command was received.
        /// </summary>
        public event OnAuthorizeStartDelegate OnAuthorizeStart
        {

            add
            {
                EMPServer.OnAuthorizeStart += value;
            }

            remove
            {
                EMPServer.OnAuthorizeStart -= value;
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
                EMPServer.OnLogAuthorizeStop += value;
            }

            remove
            {
                EMPServer.OnLogAuthorizeStop -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a authorize stop response was sent.
        /// </summary>
        public event AccessLogHandler OnLogAuthorizeStopped
        {

            add
            {
                EMPServer.OnLogAuthorizeStopped += value;
            }

            remove
            {
                EMPServer.OnLogAuthorizeStopped -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a authorize stop command was received.
        /// </summary>
        public event OnAuthorizeStopDelegate  OnAuthorizeStop
        {

            add
            {
                EMPServer.OnAuthorizeStop += value;
            }

            remove
            {
                EMPServer.OnAuthorizeStop -= value;
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
                EMPServer.OnLogChargeDetailRecordSend += value;
            }

            remove
            {
                EMPServer.OnLogChargeDetailRecordSend -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a charge detail record response was sent.
        /// </summary>
        public event AccessLogHandler OnLogChargeDetailRecordSent
        {

            add
            {
                EMPServer.OnLogChargeDetailRecordSent += value;
            }

            remove
            {
                EMPServer.OnLogChargeDetailRecordSent -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a charge detail record was received.
        /// </summary>
        public event OnChargeDetailRecordDelegate OnChargeDetailRecord
        {

            add
            {
                EMPServer.OnChargeDetailRecord += value;
            }

            remove
            {
                EMPServer.OnChargeDetailRecord -= value;
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
                EMPServer.RequestLog += value;
            }

            remove
            {
                EMPServer.RequestLog -= value;
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
                EMPServer.AccessLog += value;
            }

            remove
            {
                EMPServer.AccessLog -= value;
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
                EMPServer.ErrorLog += value;
            }

            remove
            {
                EMPServer.ErrorLog -= value;
            }

        }

        #endregion

        #endregion

        #region Constructor(s)

        #region EMPRoaming(EMPClient, EMPServer, ServerLoggingContext = EMPServerLogger.DefaultContext, LogFileCreator = null)

        /// <summary>
        /// Create a new OICP roaming client for EMPs.
        /// </summary>
        /// <param name="EMPClient">A EMP client.</param>
        /// <param name="EMPServer">A EMP sever.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
        public EMPRoaming(EMPClient                     EMPClient,
                          EMPServer                     EMPServer,
                          String                        ServerLoggingContext   = EMPServerLogger.DefaultContext,
                          Func<String, String, String>  LogFileCreator  = null)
        {

            this.EMPClient        = EMPClient;
            this.EMPServer        = EMPServer;
            this.EMPServerLogger  = new EMPServerLogger(EMPServer, ServerLoggingContext, LogFileCreator);

        }

        #endregion

        #region EMPRoaming(ClientId, RemoteHostname, RemoteTCPPort = null, RemoteHTTPVirtualHost = null, ... )

        /// <summary>
        /// Create a new OICP roaming client for EMPs.
        /// </summary>
        /// <param name="ClientId">A unqiue identification of this client.</param>
        /// <param name="RemoteHostname">The hostname of the remote OICP service.</param>
        /// <param name="RemoteTCPPort">An optional TCP port of the remote OICP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCert">The TLS client certificate to use.</param>
        /// <param name="RemoteHTTPVirtualHost">An optional HTTP virtual hostname of the remote OICP service.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
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
        public EMPRoaming(String                               ClientId,
                          String                               RemoteHostname,
                          IPPort                               RemoteTCPPort               = null,
                          RemoteCertificateValidationCallback  RemoteCertificateValidator  = null,
                          X509Certificate                      ClientCert                  = null,
                          String                               RemoteHTTPVirtualHost       = null,
                          String                               URIPrefix                   = EMPClient.DefaultURIPrefix,
                          String                               HTTPUserAgent               = EMPClient.DefaultHTTPUserAgent,
                          TimeSpan?                            RequestTimeout              = null,

                          String                               ServerName                  = EMPServer.DefaultHTTPServerName,
                          IPPort                               ServerTCPPort               = null,
                          String                               ServerURIPrefix             = "",
                          Boolean                              ServerAutoStart             = false,

                          String                               ClientLoggingContext        = EMPClient.EMPClientLogger.DefaultContext,
                          String                               ServerLoggingContext        = EMPServerLogger.DefaultContext,
                          Func<String, String, String>         LogFileCreator              = null,

                          DNSClient                            DNSClient                   = null)

            : this(new EMPClient(ClientId,
                                 RemoteHostname,
                                 RemoteTCPPort,
                                 RemoteCertificateValidator,
                                 ClientCert,
                                 RemoteHTTPVirtualHost,
                                 URIPrefix,
                                 HTTPUserAgent,
                                 RequestTimeout,
                                 DNSClient,
                                 ClientLoggingContext,
                                 LogFileCreator),

                   new EMPServer(ServerName,
                                 ServerTCPPort,
                                 ServerURIPrefix,
                                 DNSClient,
                                 false),

                   ServerLoggingContext,
                   LogFileCreator)

        {

            if (ServerAutoStart)
                Start();

        }

        #endregion

        #endregion


        #region PullEVSEData(ProviderId, SearchCenter = null, DistanceKM = 0.0, LastCall = null, ...)

        /// <summary>
        /// Create a new task querying EVSE data from the OICP server.
        /// The request might either have none, 'SearchCenter + DistanceKM' or 'LastCall' parameters.
        /// Because of limitations at Hubject the SearchCenter and LastCall parameters can not be used at the same time!
        /// </summary>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="LastCall">An optional timestamp of the last call.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<EVSEData>>

            PullEVSEData(eMobilityProvider_Id  ProviderId,
                         GeoCoordinate         SearchCenter        = null,
                         Double                DistanceKM          = 0.0,
                         DateTime?             LastCall            = null,

                         DateTime?             Timestamp           = null,
                         CancellationToken?    CancellationToken   = null,
                         EventTracking_Id      EventTrackingId     = null,
                         TimeSpan?             RequestTimeout      = null)


            => await EMPClient.PullEVSEData(ProviderId,
                                            SearchCenter,
                                            DistanceKM,
                                            LastCall,

                                            Timestamp,
                                            CancellationToken,
                                            EventTrackingId,
                                            RequestTimeout);

        #endregion

        #region SearchEVSE(ProviderId, SearchCenter = null, DistanceKM = 0.0, Address = null, Plug = null, ChargingFacility = null, ...)

        /// <summary>
        /// Create a new Search EVSE request.
        /// </summary>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="SearchCenter">An optional geocoordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="Address">An optional address of the charging stations.</param>
        /// <param name="Plug">Optional plugs of the charging station.</param>
        /// <param name="ChargingFacility">Optional charging facilities of the charging station.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<EVSESearchResult>>

            SearchEVSE(eMobilityProvider_Id  ProviderId,
                       GeoCoordinate         SearchCenter        = null,
                       Double                DistanceKM          = 0.0,
                       Address               Address             = null,
                       PlugTypes?            Plug                = null,
                       ChargingFacilities?   ChargingFacility    = null,

                       DateTime?             Timestamp           = null,
                       CancellationToken?    CancellationToken   = null,
                       EventTracking_Id      EventTrackingId     = null,
                       TimeSpan?             RequestTimeout      = null)


            => await EMPClient.SearchEVSE(ProviderId,
                                          SearchCenter,
                                          DistanceKM,
                                          Address,
                                          Plug,
                                          ChargingFacility,

                                          Timestamp,
                                          CancellationToken,
                                          EventTrackingId,
                                          RequestTimeout);

        #endregion


        #region PullEVSEStatus(ProviderId, SearchCenter = null, DistanceKM = 0.0, EVSEStatusFilter = null, ...)

        /// <summary>
        /// Create a new task requesting the current status of all EVSEs (within an optional search radius and status).
        /// </summary>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="SearchCenter">An optional geo coordinate of the search center.</param>
        /// <param name="DistanceKM">An optional search distance relative to the search center.</param>
        /// <param name="EVSEStatusFilter">An optional EVSE status as filter criteria.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<EVSEStatus>>

            PullEVSEStatus(eMobilityProvider_Id  ProviderId,
                           GeoCoordinate         SearchCenter        = null,
                           Double                DistanceKM          = 0.0,
                           EVSEStatusTypes?      EVSEStatusFilter    = null,

                           DateTime?             Timestamp           = null,
                           CancellationToken?    CancellationToken   = null,
                           EventTracking_Id      EventTrackingId     = null,
                           TimeSpan?             RequestTimeout      = null)


            => await EMPClient.PullEVSEStatus(ProviderId,
                                              SearchCenter,
                                              DistanceKM,
                                              EVSEStatusFilter,

                                              Timestamp,
                                              CancellationToken,
                                              EventTrackingId,
                                              RequestTimeout);

        #endregion

        #region PullEVSEStatusById(ProviderId, EVSEIds, ...)

        /// <summary>
        /// Create a new task requesting the current status of up to 100 EVSEs by their EVSE Ids.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="EVSEIds">Up to 100 EVSE Ids.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<EVSEStatusById>>

            PullEVSEStatusById(eMobilityProvider_Id  ProviderId,
                               IEnumerable<EVSE_Id>  EVSEIds,

                               DateTime?             Timestamp          = null,
                               CancellationToken?    CancellationToken  = null,
                               EventTracking_Id      EventTrackingId    = null,
                               TimeSpan?             RequestTimeout     = null)


            => await EMPClient.PullEVSEStatusById(ProviderId,
                                                  EVSEIds,

                                                  Timestamp,
                                                  CancellationToken,
                                                  EventTrackingId,
                                                  RequestTimeout);

        #endregion


        #region PushAuthenticationData(ProviderAuthenticationDataRecords, OICPAction = fullLoad, ...)

        /// <summary>
        /// Create a new task pushing provider authentication data records onto the OICP server.
        /// </summary>
        /// <param name="ProviderAuthenticationDataRecords">An enumeration of provider authentication data records.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<Acknowledgement>>

            PushAuthenticationData(IEnumerable<ProviderAuthenticationData>  ProviderAuthenticationDataRecords,
                                   ActionTypes                              OICPAction         = ActionTypes.fullLoad,

                                   DateTime?                                Timestamp          = null,
                                   CancellationToken?                       CancellationToken  = null,
                                   EventTracking_Id                         EventTrackingId    = null,
                                   TimeSpan?                                RequestTimeout     = null)


            => await EMPClient.PushAuthenticationData(ProviderAuthenticationDataRecords,
                                                      OICPAction,

                                                      Timestamp,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      RequestTimeout);

        #endregion

        #region PushAuthenticationData(AuthorizationIdentifications, ProviderId, OICPAction = fullLoad, ...)

        /// <summary>
        /// Create a new task pushing authorization identifications onto the OICP server.
        /// </summary>
        /// <param name="AuthorizationIdentifications">An enumeration of authorization identifications.</param>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="OICPAction">An optional OICP action.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<Acknowledgement>>

            PushAuthenticationData(IEnumerable<AuthorizationIdentification>  AuthorizationIdentifications,
                                   eMobilityProvider_Id                      ProviderId,
                                   ActionTypes                               OICPAction         = ActionTypes.fullLoad,

                                   DateTime?                                 Timestamp          = null,
                                   CancellationToken?                        CancellationToken  = null,
                                   EventTracking_Id                          EventTrackingId    = null,
                                   TimeSpan?                                 RequestTimeout     = null)


            => await EMPClient.PushAuthenticationData(AuthorizationIdentifications,
                                                      ProviderId,
                                                      OICPAction,

                                                      Timestamp,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      RequestTimeout);

        #endregion


        #region ReservationStart(EVSEId, ProviderId, eMAId, SessionId = null, PartnerSessionId = null, ChargingProductId = null, ...)

        /// <summary>
        /// Create a reservation at the given EVSE.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the EVSE to be started.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <param name="SessionId">An optional unique identification of the charging session.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="PartnerProductId">The unique identification of the choosen charging product.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<Acknowledgement>>

            ReservationStart(EVSE_Id               EVSEId,
                             eMobilityProvider_Id  ProviderId,
                             eMobilityAccount_Id   eMAId,
                             ChargingSession_Id    SessionId           = null,
                             ChargingSession_Id    PartnerSessionId    = null,
                             ChargingProduct_Id    PartnerProductId    = null,

                             DateTime?             Timestamp           = null,
                             CancellationToken?    CancellationToken   = null,
                             EventTracking_Id      EventTrackingId     = null,
                             TimeSpan?             RequestTimeout      = null)


            => await EMPClient.ReservationStart(ProviderId,
                                                EVSEId,
                                                eMAId,
                                                SessionId,
                                                PartnerSessionId,
                                                PartnerProductId,

                                                Timestamp,
                                                CancellationToken,
                                                EventTrackingId,
                                                RequestTimeout);

        #endregion

        #region ReservationStop(SessionId, ProviderId, EVSEId, PartnerSessionId = null, ...)

        /// <summary>
        /// Delete a reservation at the given EVSE.
        /// </summary>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="EVSEId">The unique identification of the EVSE to be stopped.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<Acknowledgement>>

            ReservationStop(ChargingSession_Id    SessionId,
                            eMobilityProvider_Id  ProviderId,
                            EVSE_Id               EVSEId,
                            ChargingSession_Id    PartnerSessionId    = null,

                            DateTime?             Timestamp           = null,
                            CancellationToken?    CancellationToken   = null,
                            EventTracking_Id      EventTrackingId     = null,
                            TimeSpan?             RequestTimeout      = null)


            => await EMPClient.ReservationStop(SessionId,
                                               ProviderId,
                                               EVSEId,
                                               PartnerSessionId,

                                               Timestamp,
                                               CancellationToken,
                                               EventTrackingId,
                                               RequestTimeout);

        #endregion


        #region RemoteStart(EVSEId, ProviderId, eMAId, SessionId = null, PartnerSessionId = null, ChargingProductId = null, ...)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the EVSE to be started.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <param name="SessionId">An optional unique identification of the charging session.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="PartnerProductId">The unique identification of the choosen charging product.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<Acknowledgement>>

            RemoteStart(EVSE_Id               EVSEId,
                        eMobilityProvider_Id  ProviderId,
                        eMobilityAccount_Id   eMAId,
                        ChargingSession_Id    SessionId           = null,
                        ChargingSession_Id    PartnerSessionId    = null,
                        ChargingProduct_Id    PartnerProductId    = null,

                        DateTime?             Timestamp           = null,
                        CancellationToken?    CancellationToken   = null,
                        EventTracking_Id      EventTrackingId     = null,
                        TimeSpan?             RequestTimeout      = null)


            => await EMPClient.RemoteStart(ProviderId,
                                           EVSEId,
                                           eMAId,
                                           SessionId,
                                           PartnerSessionId,
                                           PartnerProductId,

                                           Timestamp,
                                           CancellationToken,
                                           EventTrackingId,
                                           RequestTimeout);

        #endregion

        #region RemoteStop(SessionId, ProviderId, EVSEId, PartnerSessionId = null, ...)

        /// <summary>
        /// Stop the given charging session at the given EVSE.
        /// </summary>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="EVSEId">The unique identification of the EVSE to be stopped.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<Acknowledgement>>

            RemoteStop(ChargingSession_Id    SessionId,
                       eMobilityProvider_Id  ProviderId,
                       EVSE_Id               EVSEId,
                       ChargingSession_Id    PartnerSessionId    = null,

                       DateTime?             Timestamp           = null,
                       CancellationToken?    CancellationToken   = null,
                       EventTracking_Id      EventTrackingId     = null,
                       TimeSpan?             RequestTimeout      = null)


            => await EMPClient.RemoteStop(SessionId,
                                          ProviderId,
                                          EVSEId,
                                          PartnerSessionId,

                                          Timestamp,
                                          CancellationToken,
                                          EventTrackingId,
                                          RequestTimeout);

        #endregion


        #region GetChargeDetailRecords(ProviderId, From, To = null, ...)

        /// <summary>
        /// Create a new task querying charge detail records from the OICP server.
        /// </summary>
        /// <param name="ProviderId">The unique identification of the EVSP.</param>
        /// <param name="From">The starting time.</param>
        /// <param name="To">An optional end time. [default: current time].</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<IEnumerable<ChargeDetailRecord>>>

            GetChargeDetailRecords(eMobilityProvider_Id  ProviderId,
                                   DateTime              From,
                                   DateTime?             To                  = null,

                                   DateTime?             Timestamp           = null,
                                   CancellationToken?    CancellationToken   = null,
                                   EventTracking_Id      EventTrackingId     = null,
                                   TimeSpan?             RequestTimeout      = null)


            => await EMPClient.GetChargeDetailRecords(ProviderId,
                                                      From,
                                                      To,

                                                      Timestamp,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      RequestTimeout);

        #endregion


        #region Start()

        public void Start()
        {
            EMPServer.Start();
        }

        #endregion

        #region Shutdown(Message = null, Wait = true)

        public void Shutdown(String Message = null, Boolean Wait = true)
        {
            EMPServer.Shutdown(Message, Wait);
        }

        #endregion


    }

}
