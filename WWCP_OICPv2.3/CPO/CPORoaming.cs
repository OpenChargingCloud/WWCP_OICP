/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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
using System.Net.Security;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    /// <summary>
    /// The CPO roaming object combines the CPO client and CPO server
    /// and adds additional logging for both.
    /// </summary>
    public class CPORoaming : ICPOClient
    {

        #region Properties

        /// <summary>
        /// The CPO client.
        /// </summary>
        public CPOClient     CPOClient    { get; }

        #region ICPOClient

        /// <summary>
        /// The remote URL of the OICP HTTP endpoint to connect to.
        /// </summary>
        URL                                  IHTTPClient.RemoteURL
            => CPOClient.RemoteURL;

        /// <summary>
        /// The virtual HTTP hostname to connect to.
        /// </summary>
        HTTPHostname?                        IHTTPClient.VirtualHostname
            => CPOClient.VirtualHostname;

        /// <summary>
        /// An optional description of this CPO client.
        /// </summary>
        String                               IHTTPClient.Description
        {

            get
            {
                return CPOClient.Description;
            }

            set
            {
                CPOClient.Description = value;
            }

        }

        /// <summary>
        /// The remote SSL/TLS certificate validator.
        /// </summary>
        RemoteCertificateValidationCallback  IHTTPClient.RemoteCertificateValidator
            => CPOClient.RemoteCertificateValidator;

        /// <summary>
        /// The SSL/TLS client certificate to use of HTTP authentication.
        /// </summary>
        X509Certificate                      IHTTPClient.ClientCert
            => CPOClient.ClientCert;

        /// <summary>
        /// The HTTP user agent identification.
        /// </summary>
        String                               IHTTPClient.HTTPUserAgent
            => CPOClient.HTTPUserAgent;

        /// <summary>
        /// The timeout for upstream requests.
        /// </summary>
        TimeSpan                             IHTTPClient.RequestTimeout
        {

            get
            {
                return CPOClient.RequestTimeout;
            }

            set
            {
                CPOClient.RequestTimeout = value;
            }

        }

        /// <summary>
        /// The delay between transmission retries.
        /// </summary>
        TransmissionRetryDelayDelegate       IHTTPClient.TransmissionRetryDelay
            => CPOClient.TransmissionRetryDelay;

        /// <summary>
        /// The maximum number of retries when communicationg with the remote OICP service.
        /// </summary>
        UInt16                               IHTTPClient.MaxNumberOfRetries
            => CPOClient.MaxNumberOfRetries;

        /// <summary>
        /// Make use of HTTP pipelining.
        /// </summary>
        Boolean                              IHTTPClient.UseHTTPPipelining
            => CPOClient.UseHTTPPipelining;

        /// <summary>
        /// The CPO client (HTTP client) logger.
        /// </summary>
        HTTPClientLogger                     IHTTPClient.HTTPLogger
        {

            get
            {
                return CPOClient.HTTPLogger;
            }

            set
            {
                if (value is CPOClient.Logger logger)
                    CPOClient.HTTPLogger = logger;
            }

        }

        /// <summary>
        /// The DNS client defines which DNS servers to use.
        /// </summary>
        DNSClient                            IHTTPClient.DNSClient
            => CPOClient.DNSClient;

        #endregion


        /// <summary>
        /// The CPO server.
        /// </summary>
        public CPOServerAPI  CPOServer    { get; }

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
        /// An event fired whenever a HTTP request pushing EVSE data records will be send.
        /// </summary>
        public event ClientRequestLogHandler OnPushEVSEDataHTTPRequest
        {

            add
            {
                CPOClient.OnPushEVSEDataHTTPRequest += value;
            }

            remove
            {
                CPOClient.OnPushEVSEDataHTTPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a push EVSE data records HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnPushEVSEDataHTTPResponse
        {

            add
            {
                CPOClient.OnPushEVSEDataHTTPResponse += value;
            }

            remove
            {
                CPOClient.OnPushEVSEDataHTTPResponse -= value;
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
        /// An event fired whenever a HTTP request pushing EVSE status records will be send.
        /// </summary>
        public event ClientRequestLogHandler OnPushEVSEStatusHTTPRequest
        {

            add
            {
                CPOClient.OnPushEVSEStatusHTTPRequest += value;
            }

            remove
            {
                CPOClient.OnPushEVSEStatusHTTPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a push EVSE status records HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnPushEVSEStatusHTTPResponse
        {

            add
            {
                CPOClient.OnPushEVSEStatusHTTPResponse += value;
            }

            remove
            {
                CPOClient.OnPushEVSEStatusHTTPResponse -= value;
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
        /// An event fired whenever an authorize start HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnAuthorizeStartHTTPRequest
        {

            add
            {
                CPOClient.OnAuthorizeStartHTTPRequest += value;
            }

            remove
            {
                CPOClient.OnAuthorizeStartHTTPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an authorize start HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnAuthorizeStartHTTPResponse
        {

            add
            {
                CPOClient.OnAuthorizeStartHTTPResponse += value;
            }

            remove
            {
                CPOClient.OnAuthorizeStartHTTPResponse -= value;
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
        /// An event fired whenever an authorize stop HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler OnAuthorizeStopHTTPRequest
        {

            add
            {
                CPOClient.OnAuthorizeStopHTTPRequest += value;
            }

            remove
            {
                CPOClient.OnAuthorizeStopHTTPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an authorize stop HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler OnAuthorizeStopHTTPResponse
        {

            add
            {
                CPOClient.OnAuthorizeStopHTTPResponse += value;
            }

            remove
            {
                CPOClient.OnAuthorizeStopHTTPResponse -= value;
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
        /// An event fired whenever a charge detail record will be send via HTTP.
        /// </summary>
        public event ClientRequestLogHandler OnSendChargeDetailRecordHTTPRequest
        {

            add
            {
                CPOClient.OnSendChargeDetailRecordHTTPRequest += value;
            }

            remove
            {
                CPOClient.OnSendChargeDetailRecordHTTPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a HTTP response to a sent charge detail record had been received.
        /// </summary>
        public event ClientResponseLogHandler OnSendChargeDetailRecordHTTPResponse
        {

            add
            {
                CPOClient.OnSendChargeDetailRecordHTTPResponse += value;
            }

            remove
            {
                CPOClient.OnSendChargeDetailRecordHTTPResponse -= value;
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

        ///// <summary>
        ///// An event fired whenever a request pulling authentication data will be send.
        ///// </summary>
        //public event OnPullAuthenticationDataRequestHandler OnPullAuthenticationDataRequest
        //{

        //    add
        //    {
        //        CPOClient.OnPullAuthenticationDataRequest += value;
        //    }

        //    remove
        //    {
        //        CPOClient.OnPullAuthenticationDataRequest -= value;
        //    }

        //}

        ///// <summary>
        ///// An event fired whenever a HTTP request pulling authentication data will be send.
        ///// </summary>
        //public event ClientRequestLogHandler OnPullAuthenticationDataHTTPRequest
        //{

        //    add
        //    {
        //        CPOClient.OnPullAuthenticationDataHTTPRequest += value;
        //    }

        //    remove
        //    {
        //        CPOClient.OnPullAuthenticationDataHTTPRequest -= value;
        //    }

        //}

        ///// <summary>
        ///// An event fired whenever a response to a pull authentication data HTTP request had been received.
        ///// </summary>
        //public event ClientResponseLogHandler OnPullAuthenticationDataOICPResult
        //{

        //    add
        //    {
        //        CPOClient.OnPullAuthenticationDataOICPResult += value;
        //    }

        //    remove
        //    {
        //        CPOClient.OnPullAuthenticationDataOICPResult -= value;
        //    }

        //}

        ///// <summary>
        ///// An event fired whenever a response to a pull authentication data request was received.
        ///// </summary>
        //public event OnPullAuthenticationDataResponseHandler OnPullAuthenticationDataResponse
        //{

        //    add
        //    {
        //        CPOClient.OnPullAuthenticationDataResponse += value;
        //    }

        //    remove
        //    {
        //        CPOClient.OnPullAuthenticationDataResponse -= value;
        //    }

        //}

        #endregion



        // CPOServer methods

        #region OnAuthorizeRemoteReservationStart

        ///// <summary>
        ///// An event sent whenever an 'authorize remote reservation start' command was received.
        ///// </summary>
        //public event OnAuthorizeRemoteReservationStartDelegate OnAuthorizeRemoteReservationStart
        //{

        //    add
        //    {
        //        CPOServer.OnAuthorizeRemoteReservationStart += value;
        //    }

        //    remove
        //    {
        //        CPOServer.OnAuthorizeRemoteReservationStart -= value;
        //    }

        //}

        #endregion

        #region OnAuthorizeRemoteReservationStop

        ///// <summary>
        ///// An event sent whenever an 'authorize remote reservation stop' command was received.
        ///// </summary>
        //public event OnAuthorizeRemoteReservationStopDelegate OnAuthorizeRemoteReservationStop

        //{

        //    add
        //    {
        //        CPOServer.OnAuthorizeRemoteReservationStop += value;
        //    }

        //    remove
        //    {
        //        CPOServer.OnAuthorizeRemoteReservationStop -= value;
        //    }

        //}

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


        #region Generic HTTP/HTTP server logging

        /// <summary>
        /// An event called whenever a HTTP request came in.
        /// </summary>
        public HTTPRequestLogEvent   RequestLog    = new HTTPRequestLogEvent();

        /// <summary>
        /// An event called whenever a HTTP request could successfully be processed.
        /// </summary>
        public HTTPResponseLogEvent  ResponseLog   = new HTTPResponseLogEvent();

        /// <summary>
        /// An event called whenever a HTTP request resulted in an error.
        /// </summary>
        public HTTPErrorLogEvent     ErrorLog      = new HTTPErrorLogEvent();

        #endregion

        #endregion

        #region Custom request mappers

        public CustomJObjectParserDelegate<Acknowledgement<PushEVSEDataRequest>> CustomPushEVSEDataAcknowledgementParser
        {

            get
            {
                return CPOClient.CustomPushEVSEDataAcknowledgementParser;
            }

            set
            {
                CPOClient.CustomPushEVSEDataAcknowledgementParser = value;
            }

        }

        public CustomJObjectParserDelegate<Acknowledgement<PushEVSEStatusRequest>> CustomPushEVSEStatusAcknowledgementParser
        {

            get
            {
                return CPOClient.CustomPushEVSEStatusAcknowledgementParser;
            }

            set
            {
                CPOClient.CustomPushEVSEStatusAcknowledgementParser = value;
            }

        }


        public CustomJObjectParserDelegate<AuthorizationStartResponse> CustomAuthorizationStartResponseParser
        {

            get
            {
                return CPOClient.CustomAuthorizationStartResponseParser;
            }

            set
            {
                CPOClient.CustomAuthorizationStartResponseParser = value;
            }

        }

        public CustomJObjectParserDelegate<AuthorizationStopResponse> CustomAuthorizationStopResponseParser
        {

            get
            {
                return CPOClient.CustomAuthorizationStopResponseParser;
            }

            set
            {
                CPOClient.CustomAuthorizationStopResponseParser = value;
            }

        }

        public CustomJObjectParserDelegate<Acknowledgement<SendChargeDetailRecordRequest>> CustomSendChargeDetailRecordAcknowledgementParser
        {

            get
            {
                return CPOClient.CustomSendChargeDetailRecordAcknowledgementParser;
            }

            set
            {
                CPOClient.CustomSendChargeDetailRecordAcknowledgementParser = value;
            }

        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new CPO roaming.
        /// </summary>
        /// <param name="CPOClient">A CPO client.</param>
        /// <param name="CPOServer">A CPO sever.</param>
        public CPORoaming(CPOClient     CPOClient,
                          CPOServerAPI  CPOServer)
        {

            this.CPOClient  = CPOClient ?? throw new ArgumentNullException(nameof(CPOClient), "The given CPOClient must not be null!");
            this.CPOServer  = CPOServer ?? throw new ArgumentNullException(nameof(CPOServer), "The given CPOServer must not be null!");

            // Link HTTP server events...
            CPOServer.RequestLog   += (HTTPProcessor, ServerTimestamp, Request)                                 => RequestLog. WhenAll(HTTPProcessor, ServerTimestamp, Request);
            CPOServer.ResponseLog  += (HTTPProcessor, ServerTimestamp, Request, Response)                       => ResponseLog.WhenAll(HTTPProcessor, ServerTimestamp, Request, Response);
            CPOServer.ErrorLog     += (HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException) => ErrorLog.   WhenAll(HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException);

        }

        #endregion



        #region PushEVSEData  (Request)

        /// <summary>
        /// Upload the given EVSE data records.
        /// </summary>
        /// <param name="Request">A PushEVSEData request.</param>
        public Task<OICPResult<Acknowledgement<PushEVSEDataRequest>>>

            PushEVSEData(PushEVSEDataRequest Request)

                => CPOClient.PushEVSEData(Request);

        #endregion

        #region PushEVSEStatus(Request)

        /// <summary>
        /// Upload the given EVSE status records.
        /// </summary>
        /// <param name="Request">A PushEVSEStatus request.</param>
        public Task<OICPResult<Acknowledgement<PushEVSEStatusRequest>>>

            PushEVSEStatus(PushEVSEStatusRequest Request)

                => CPOClient.PushEVSEStatus(Request);

        #endregion


        #region AuthorizeStart        (Request)

        /// <summary>
        /// Create an OICP authorize start request.
        /// </summary>
        /// <param name="Request">An AuthorizeStart request.</param>
        public Task<OICPResult<AuthorizationStartResponse>>

            AuthorizeStart(AuthorizeStartRequest Request)

                => CPOClient.AuthorizeStart(Request);

        #endregion

        #region AuthorizeStop         (Request)

        /// <summary>
        /// Create an OICP authorize stop request.
        /// </summary>
        /// <param name="Request">An AuthorizeStop request.</param>
        public Task<OICPResult<AuthorizationStopResponse>>

            AuthorizeStop(AuthorizeStopRequest Request)

                => CPOClient.AuthorizeStop(Request);

        #endregion


        #region SendChargeDetailRecord(Request)

        /// <summary>
        /// Send a charging start notification.
        /// </summary>
        /// <param name="Request">A ChargingNotificationsStart request.</param>
        public Task<OICPResult<Acknowledgement<ChargingNotificationsStartRequest>>>

            SendChargingNotificationsStart(ChargingNotificationsStartRequest Request)

                => CPOClient.SendChargingNotificationsStart(Request);

        #endregion

        #region SendChargeDetailRecord(Request)

        /// <summary>
        /// Send a charging start notification.
        /// </summary>
        /// <param name="Request">A ChargingNotificationsProgress request.</param>
        public Task<OICPResult<Acknowledgement<ChargingNotificationsProgressRequest>>>

            SendChargingNotificationsProgress(ChargingNotificationsProgressRequest Request)

                => CPOClient.SendChargingNotificationsProgress(Request);

        #endregion

        #region SendChargeDetailRecord(Request)

        /// <summary>
        /// Send a charging start notification.
        /// </summary>
        /// <param name="Request">A ChargingNotificationsEnd request.</param>
        public Task<OICPResult<Acknowledgement<ChargingNotificationsEndRequest>>>

            SendChargingNotificationsEnd(ChargingNotificationsEndRequest Request)

                => CPOClient.SendChargingNotificationsEnd(Request);

        #endregion

        #region SendChargeDetailRecord(Request)

        /// <summary>
        /// Send a charging start notification.
        /// </summary>
        /// <param name="Request">A ChargingNotificationsError request.</param>
        public Task<OICPResult<Acknowledgement<ChargingNotificationsErrorRequest>>>

            SendChargingNotificationsError(ChargingNotificationsErrorRequest Request)

                => CPOClient.SendChargingNotificationsError(Request);

        #endregion


        #region SendChargeDetailRecord(Request)

        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="Request">A SendChargeDetailRecord request.</param>
        public Task<OICPResult<Acknowledgement<SendChargeDetailRecordRequest>>>

            SendChargeDetailRecord(SendChargeDetailRecordRequest Request)

                => CPOClient.SendChargeDetailRecord(Request);

        #endregion


        #region PullAuthenticationData(Request)

        ///// <summary>
        ///// Pull authentication data from the OICP server.
        ///// </summary>
        ///// <param name="Request">A PullAuthenticationData request.</param>
        //public Task<OICPResult<AuthenticationData>>

        //    PullAuthenticationData(PullAuthenticationDataRequest Request)

        //        => CPOClient.PullAuthenticationData(Request);

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

        public void Dispose()
        { }

    }

}
