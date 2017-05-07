/*
 * Copyright (c) 2014-2017 GraphDefined GmbH
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
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.CPO
{

    /// <summary>
    /// An OICP roaming client for CPOs which combines the CPO client
    /// and server and adds additional logging for both.
    /// </summary>
    public class CPORoaming : ICPOClient
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

        /// <summary>
        /// The default request timeout for this client.
        /// </summary>
        public TimeSpan?        RequestTimeout    { get; }

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
        public event OnAuthorizeStartRequestHandler OnAuthorizeStartRequest
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
        public event OnAuthorizeStartResponseHandler OnAuthorizeStartResponse
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

        #region OnAuthorizeRemoteReservationStart

        /// <summary>
        /// An event sent whenever an 'authorize remote reservation start' command was received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStartDelegate OnAuthorizeRemoteReservationStart
        {

            add
            {
                CPOServer.OnAuthorizeRemoteReservationStart += value;
            }

            remove
            {
                CPOServer.OnAuthorizeRemoteReservationStart -= value;
            }

        }

        #endregion

        #region OnAuthorizeRemoteReservationStop

        /// <summary>
        /// An event sent whenever an 'authorize remote reservation stop' command was received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStopDelegate OnAuthorizeRemoteReservationStop

        {

            add
            {
                CPOServer.OnAuthorizeRemoteReservationStop += value;
            }

            remove
            {
                CPOServer.OnAuthorizeRemoteReservationStop -= value;
            }

        }

        #endregion


        #region OnAuthorizeRemoteStart

        /// <summary>
        /// An event sent whenever an 'authorize remote start' command was received.
        /// </summary>
        public event OnAuthorizeRemoteStartDelegate OnAuthorizeRemoteStart
        {

            add
            {
                CPOServer.OnAuthorizeRemoteStart += value;
            }

            remove
            {
                CPOServer.OnAuthorizeRemoteStart -= value;
            }

        }

        #endregion

        #region  OnAuthorizeRemoteStop

        /// <summary>
        /// An event sent whenever an 'authorize remote stop' command was received.
        /// </summary>
        public event OnAuthorizeRemoteStopDelegate OnAuthorizeRemoteStop

        {

            add
            {
                CPOServer.OnAuthorizeRemoteStop += value;
            }

            remove
            {
                CPOServer.OnAuthorizeRemoteStop -= value;
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

        #region Custom request mappers

        #region CustomPushEVSEDataRequestMapper

        #region CustomPushEVSEDataRequestMapper

        public Func<PushEVSEDataRequest, PushEVSEDataRequest> CustomPushEVSEDataRequestMapper
        {

            get
            {
                return CPOClient.CustomPushEVSEDataRequestMapper;
            }

            set
            {
                CPOClient.CustomPushEVSEDataRequestMapper = value;
            }

        }

        #endregion

        #region CustomPushEVSEDataSOAPRequestMapper

        public Func<PushEVSEDataRequest, XElement, XElement> CustomPushEVSEDataSOAPRequestMapper
        {

            get
            {
                return CPOClient.CustomPushEVSEDataSOAPRequestMapper;
            }

            set
            {
                CPOClient.CustomPushEVSEDataSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<Acknowledgement<PushEVSEDataRequest>> CustomPushEVSEDataParser
        {

            get
            {
                return CPOClient.CustomPushEVSEDataParser;
            }

            set
            {
                CPOClient.CustomPushEVSEDataParser = value;
            }

        }

        #endregion

        #region CustomPushEVSEStatusRequestMapper

        #region CustomPushEVSEStatusRequestMapper

        public Func<PushEVSEStatusRequest, PushEVSEStatusRequest> CustomPushEVSEStatusRequestMapper
        {

            get
            {
                return CPOClient.CustomPushEVSEStatusRequestMapper;
            }

            set
            {
                CPOClient.CustomPushEVSEStatusRequestMapper = value;
            }

        }

        #endregion

        #region CustomPushEVSEStatusSOAPRequestMapper

        public Func<PushEVSEStatusRequest, XElement, XElement> CustomPushEVSEStatusSOAPRequestMapper
        {

            get
            {
                return CPOClient.CustomPushEVSEStatusSOAPRequestMapper;
            }

            set
            {
                CPOClient.CustomPushEVSEStatusSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<Acknowledgement<PushEVSEStatusRequest>> CustomPushEVSEStatusParser
        {

            get
            {
                return CPOClient.CustomPushEVSEStatusParser;
            }

            set
            {
                CPOClient.CustomPushEVSEStatusParser = value;
            }

        }

        #endregion


        #region CustomAuthorizeStartRequestMapper

        #region CustomAuthorizeStartRequestMapper

        public Func<AuthorizeStartRequest, AuthorizeStartRequest> CustomAuthorizeStartRequestMapper
        {

            get
            {
                return CPOClient.CustomAuthorizeStartRequestMapper;
            }

            set
            {
                CPOClient.CustomAuthorizeStartRequestMapper = value;
            }

        }

        #endregion

        #region CustomAuthorizeStartSOAPRequestMapper

        public Func<AuthorizeStartRequest, XElement, XElement> CustomAuthorizeStartSOAPRequestMapper
        {

            get
            {
                return CPOClient.CustomAuthorizeStartSOAPRequestMapper;
            }

            set
            {
                CPOClient.CustomAuthorizeStartSOAPRequestMapper = value;
            }

        }

        #endregion

        //public CustomParserDelegate<AuthorizationStart> CustomAuthorizeStartResponseMapper
        //{

        //    get
        //    {
        //        return CPOClient.CustomAuthorizeStartRequestMapper
        //    }

        //    set
        //    {
        //        CPOClient.CustomAuthorizeStartResponseMapper = value;
        //    }

        //}

        #endregion

        #region CustomAuthorizeStopRequestMapper

        #region CustomAuthorizeStopRequestMapper

        public Func<AuthorizeStopRequest, AuthorizeStopRequest> CustomAuthorizeStopRequestMapper
        {

            get
            {
                return CPOClient.CustomAuthorizeStopRequestMapper;
            }

            set
            {
                CPOClient.CustomAuthorizeStopRequestMapper = value;
            }

        }

        #endregion

        #region CustomAuthorizeStopSOAPRequestMapper

        public Func<AuthorizeStopRequest, XElement, XElement> CustomAuthorizeStopSOAPRequestMapper
        {

            get
            {
                return CPOClient.CustomAuthorizeStopSOAPRequestMapper;
            }

            set
            {
                CPOClient.CustomAuthorizeStopSOAPRequestMapper = value;
            }

        }

        #endregion

        //public CustomMapperDelegate<AuthorizationStop, AuthorizationStop.Builder> CustomAuthorizeStopResponseMapper
        //{

        //    get
        //    {
        //        return CPOClient.CustomAuthorizeStopResponseMapper;
        //    }

        //    set
        //    {
        //        CPOClient.CustomAuthorizeStopResponseMapper = value;
        //    }

        //}

        #endregion

        #region CustomSendChargeDetailRecordRequestMapper

        #region CustomSendChargeDetailRecordRequestMapper

        public Func<SendChargeDetailRecordRequest, SendChargeDetailRecordRequest> CustomSendChargeDetailRecordRequestMapper
        {

            get
            {
                return CPOClient.CustomSendChargeDetailRecordRequestMapper;
            }

            set
            {
                CPOClient.CustomSendChargeDetailRecordRequestMapper = value;
            }

        }

        #endregion

        #region CustomSendChargeDetailRecordSOAPRequestMapper

        public Func<SendChargeDetailRecordRequest, XElement, XElement> CustomSendChargeDetailRecordSOAPRequestMapper
        {

            get
            {
                return CPOClient.CustomSendChargeDetailRecordSOAPRequestMapper;
            }

            set
            {
                CPOClient.CustomSendChargeDetailRecordSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<Acknowledgement<SendChargeDetailRecordRequest>> CustomSendChargeDetailRecordParser
        {

            get
            {
                return CPOClient.CustomSendChargeDetailRecordParser;
            }

            set
            {
                CPOClient.CustomSendChargeDetailRecordParser = value;
            }

        }

        #endregion


        #region CustomPullAuthenticationDataRequestMapper

        #region CustomPullAuthenticationDataRequestMapper

        public Func<PullAuthenticationDataRequest, PullAuthenticationDataRequest> CustomPullAuthenticationDataRequestMapper
        {

            get
            {
                return CPOClient.CustomPullAuthenticationDataRequestMapper;
            }

            set
            {
                CPOClient.CustomPullAuthenticationDataRequestMapper = value;
            }

        }

        #endregion

        #region CustomPullAuthenticationDataSOAPRequestMapper

        public Func<PullAuthenticationDataRequest, XElement, XElement> CustomPullAuthenticationDataSOAPRequestMapper
        {

            get
            {
                return CPOClient.CustomPullAuthenticationDataSOAPRequestMapper;
            }

            set
            {
                CPOClient.CustomPullAuthenticationDataSOAPRequestMapper = value;
            }

        }

        #endregion

        //public CustomMapperDelegate<Acknowledgement<PullAuthenticationDataRequest>, Acknowledgement<PullAuthenticationDataRequest>.Builder> CustomPullAuthenticationDataResponseMapper
        //{

        //    get
        //    {
        //        return CPOClient.CustomPullAuthenticationDataResponseMapper;
        //    }

        //    set
        //    {
        //        CPOClient.CustomPullAuthenticationDataResponseMapper = value;
        //    }

        //}

        #endregion

        #endregion

        #region Constructor(s)

        #region CPORoaming(CPOClient, CPOServer, ServerLoggingContext = CPOServerLogger.DefaultContext, LogfileCreator = null)

        /// <summary>
        /// Create a new OICP roaming client for CPOs.
        /// </summary>
        /// <param name="CPOClient">A CPO client.</param>
        /// <param name="CPOServer">A CPO sever.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        public CPORoaming(CPOClient               CPOClient,
                          CPOServer               CPOServer,
                          String                  ServerLoggingContext  = CPOServerLogger.DefaultContext,
                          LogfileCreatorDelegate  LogfileCreator        = null)
        {

            this.CPOClient        = CPOClient;
            this.CPOServer        = CPOServer;

            this.CPOServerLogger  = new CPOServerLogger(CPOServer,
                                                        ServerLoggingContext,
                                                        LogfileCreator);

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
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// 
        /// <param name="ServerName">An optional identification string for the HTTP server.</param>
        /// <param name="ServerTCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="ServerURIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="ServerContentType">An optional HTTP content type to use.</param>
        /// <param name="ServerRegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="ServerAutoStart">Whether to start the server immediately or not.</param>
        /// 
        /// <param name="ClientLoggingContext">An optional context for logging client methods.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// 
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public CPORoaming(String                               ClientId,
                          String                               RemoteHostname,
                          IPPort                               RemoteTCPPort                   = null,
                          RemoteCertificateValidationCallback  RemoteCertificateValidator      = null,
                          X509Certificate                      ClientCert                      = null,
                          String                               RemoteHTTPVirtualHost           = null,
                          String                               URIPrefix                       = CPOClient.DefaultURIPrefix,
                          String                               EVSEDataURI                     = CPOClient.DefaultEVSEDataURI,
                          String                               EVSEStatusURI                   = CPOClient.DefaultEVSEStatusURI,
                          String                               AuthorizationURI                = CPOClient.DefaultAuthorizationURI,
                          String                               AuthenticationDataURI           = CPOClient.DefaultAuthenticationDataURI,
                          String                               HTTPUserAgent                   = CPOClient.DefaultHTTPUserAgent,
                          TimeSpan?                            RequestTimeout                  = null,

                          String                               ServerName                      = CPOServer.DefaultHTTPServerName,
                          IPPort                               ServerTCPPort                   = null,
                          String                               ServerURIPrefix                 = CPOServer.DefaultURIPrefix,
                          String                               ServerAuthorizationURI          = CPOServer.DefaultAuthorizationURI,
                          String                               ServerReservationURI            = CPOServer.DefaultReservationURI,
                          HTTPContentType                      ServerContentType               = null,
                          Boolean                              ServerRegisterHTTPRootService   = true,
                          Boolean                              ServerAutoStart                 = false,

                          String                               ClientLoggingContext            = CPOClient.CPOClientLogger.DefaultContext,
                          String                               ServerLoggingContext            = CPOServerLogger.DefaultContext,
                          LogfileCreatorDelegate               LogfileCreator                  = null,

                          DNSClient                            DNSClient                       = null)

            : this(new CPOClient(ClientId,
                                 RemoteHostname,
                                 RemoteTCPPort,
                                 RemoteCertificateValidator,
                                 ClientCert,
                                 RemoteHTTPVirtualHost,
                                 URIPrefix,
                                 EVSEDataURI,
                                 EVSEStatusURI,
                                 AuthorizationURI,
                                 AuthenticationDataURI,
                                 HTTPUserAgent,
                                 RequestTimeout,
                                 DNSClient,
                                 ClientLoggingContext,
                                 LogfileCreator),

                   new CPOServer(ServerName,
                                 ServerTCPPort,
                                 ServerURIPrefix,
                                 ServerAuthorizationURI,
                                 ServerReservationURI,
                                 ServerContentType,
                                 ServerRegisterHTTPRootService,
                                 DNSClient,
                                 false),

                   ServerLoggingContext,
                   LogfileCreator)

        {

            if (ServerAutoStart)
                Start();

        }

        #endregion

        #endregion


        #region PushEVSEData  (Request)

        /// <summary>
        /// Upload the given EVSE data records.
        /// </summary>
        /// <param name="Request">A PushEVSEData request.</param>
        public Task<HTTPResponse<Acknowledgement<PushEVSEDataRequest>>>

            PushEVSEData(PushEVSEDataRequest Request)

                => CPOClient.PushEVSEData(Request);

        #endregion

        #region PushEVSEStatus(Request)

        /// <summary>
        /// Upload the given EVSE status records.
        /// </summary>
        /// <param name="Request">A PushEVSEStatus request.</param>
        public Task<HTTPResponse<Acknowledgement<PushEVSEStatusRequest>>>

            PushEVSEStatus(PushEVSEStatusRequest Request)

                => CPOClient.PushEVSEStatus(Request);

        #endregion


        #region AuthorizeStart        (Request)

        /// <summary>
        /// Create an OICP authorize start request.
        /// </summary>
        /// <param name="Request">An AuthorizeStart request.</param>
        public Task<HTTPResponse<AuthorizationStart>>

            AuthorizeStart(AuthorizeStartRequest Request)

                => CPOClient.AuthorizeStart(Request);

        #endregion

        #region AuthorizeStop         (Request)

        /// <summary>
        /// Create an OICP authorize stop request.
        /// </summary>
        /// <param name="Request">An AuthorizeStop request.</param>
        public Task<HTTPResponse<AuthorizationStop>>

            AuthorizeStop(AuthorizeStopRequest Request)

                => CPOClient.AuthorizeStop(Request);

        #endregion

        #region SendChargeDetailRecord(Request)

        /// <summary>
        /// Send a charge detail record to an OICP server.
        /// </summary>
        /// <param name="Request">A SendChargeDetailRecord request.</param>
        public Task<HTTPResponse<Acknowledgement<SendChargeDetailRecordRequest>>>

            SendChargeDetailRecord(SendChargeDetailRecordRequest Request)

                => CPOClient.SendChargeDetailRecord(Request);

        #endregion


        #region PullAuthenticationData(Request)

        /// <summary>
        /// Pull authentication data from the OICP server.
        /// </summary>
        /// <param name="Request">A PullAuthenticationData request.</param>
        public Task<HTTPResponse<AuthenticationData>>

            PullAuthenticationData(PullAuthenticationDataRequest Request)

                => CPOClient.PullAuthenticationData(Request);

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
