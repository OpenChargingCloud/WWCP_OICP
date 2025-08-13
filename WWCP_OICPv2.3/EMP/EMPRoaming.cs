/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.EMP
{

    /// <summary>
    /// The EMP roaming object combines the EMP client and EMP server.
    /// </summary>
    /// <remarks>
    /// Create a new EMP roaming.
    /// </remarks>
    /// <param name="EMPClient">An EMP client.</param>
    /// <param name="EMPServer">An optional EMP Server.</param>
    public class EMPRoaming(EMPClient      EMPClient,
                            EMPServerAPI?  EMPServer   = null) : IEMPClient
    {

        #region Properties

        /// <summary>
        /// The EMP client part.
        /// </summary>
        public EMPClient     EMPClient    { get; } = EMPClient ?? throw new ArgumentNullException(nameof(EMPClient), "The given EMPClient must not be null!");

        /// <summary>
        /// The EMP server part.
        /// </summary>
        public EMPServerAPI  EMPServer    { get; } = EMPServer ?? new EMPServerAPI(AutoStart: false);

        #region IEMPClient

        /// <summary>
        /// The remote URL of the OICP HTTP endpoint to connect to.
        /// </summary>
        URL                                                         IHTTPClient.RemoteURL
            => EMPClient.RemoteURL;

        /// <summary>
        /// The virtual HTTP hostname to connect to.
        /// </summary>
        HTTPHostname?                                               IHTTPClient.VirtualHostname
            => EMPClient.VirtualHostname;

        /// <summary>
        /// An optional description of this CPO client.
        /// </summary>
        I18NString                                                  IHTTPClient.Description
        {

            get
            {
                return EMPClient.Description;
            }

            //set
            //{
            //    EMPClient.Description = value;
            //}

        }

        /// <summary>
        /// The remote TLS certificate validator.
        /// </summary>
        RemoteTLSServerCertificateValidationHandler<IHTTPClient>?   IHTTPClient.RemoteCertificateValidator
            => EMPClient.RemoteCertificateValidator;

        /// <summary>
        /// The TLS client certificate to use of HTTP authentication.
        /// </summary>
        X509Certificate?                                            IHTTPClient.ClientCert
            => EMPClient.ClientCert;

        /// <summary>
        /// The TLS protocol to use.
        /// </summary>
        SslProtocols                                                IHTTPClient.TLSProtocols
            => EMPClient.TLSProtocols;

        /// <summary>
        /// Prefer IPv4 instead of IPv6.
        /// </summary>
        Boolean                                                     IHTTPClient.PreferIPv4
            => EMPClient.PreferIPv4;

        /// <summary>
        /// The optional HTTP connection type.
        /// </summary>
        ConnectionType?                                             IHTTPClient.Connection
            => EMPClient.Connection;

        /// <summary>
        /// The optional HTTP content type.
        /// </summary>
        HTTPContentType?                                            IHTTPClient.ContentType
            => EMPClient.ContentType;

        /// <summary>
        /// The optional HTTP accept header.
        /// </summary>
        AcceptTypes?                                                IHTTPClient.Accept
            => EMPClient.Accept;

        /// <summary>
        /// The optional HTTP authentication to use.
        /// </summary>
        IHTTPAuthentication?                                        IHTTPClient.Authentication
            => EMPClient.Authentication;

        /// <summary>
        /// The HTTP user agent identification.
        /// </summary>
        String                                                      IHTTPClient.HTTPUserAgent
            => EMPClient.HTTPUserAgent;

        /// <summary>
        /// The timeout for upstream requests.
        /// </summary>
        TimeSpan                                                    IHTTPClient.RequestTimeout
        {

            get
            {
                return EMPClient.RequestTimeout;
            }

            set
            {
                EMPClient.RequestTimeout = value;
            }

        }

        /// <summary>
        /// The delay between transmission retries.
        /// </summary>
        TransmissionRetryDelayDelegate                              IHTTPClient.TransmissionRetryDelay
            => EMPClient.TransmissionRetryDelay;

        /// <summary>
        /// The maximum number of retries when communicating with the remote OICP service.
        /// </summary>
        UInt16                                                      IHTTPClient.MaxNumberOfRetries
            => EMPClient.MaxNumberOfRetries;

        /// <summary>
        /// Make use of HTTP pipelining.
        /// </summary>
        Boolean                                                     IHTTPClient.UseHTTPPipelining
            => EMPClient.UseHTTPPipelining;

        /// <summary>
        /// The CPO client (HTTP client) logger.
        /// </summary>
        HTTPClientLogger?                                           IHTTPClient.HTTPLogger
            => EMPClient.HTTPLogger;

        /// <summary>
        /// The DNS client defines which DNS servers to use.
        /// </summary>
        DNSClient                                                   IHTTPClient.DNSClient
            => EMPClient.DNSClient;


        Boolean                                                     IHTTPClient.Connected
            => EMPClient.Connected;

        IIPAddress?                                                 IHTTPClient.RemoteIPAddress
            => EMPClient.RemoteIPAddress;

        UInt64                                                      IHTTPClient.KeepAliveMessageCount
            => EMPClient.KeepAliveMessageCount;

        #endregion

        #endregion

        #region EMPClient events

        #region OnPullEVSEDataRequest/-Response

        /// <summary>
        /// An event fired whenever a PullEVSEData request will be send.
        /// </summary>
        public event OnPullEVSEDataRequestDelegate   OnPullEVSEDataRequest
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
        /// An event fired whenever a PullEVSEData HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler         OnPullEVSEDataHTTPRequest
        {

            add
            {
                EMPClient.OnPullEVSEDataHTTPRequest += value;
            }

            remove
            {
                EMPClient.OnPullEVSEDataHTTPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a PullEVSEData HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler        OnPullEVSEDataHTTPResponse
        {

            add
            {
                EMPClient.OnPullEVSEDataHTTPResponse += value;
            }

            remove
            {
                EMPClient.OnPullEVSEDataHTTPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a PullEVSEData request had been received.
        /// </summary>
        public event OnPullEVSEDataResponseDelegate  OnPullEVSEDataResponse
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

        #region OnPullEVSEStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a PullEVSEStatus request will be send.
        /// </summary>
        public event OnPullEVSEStatusRequestDelegate   OnPullEVSEStatusRequest
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
        /// An event fired whenever a PullEVSEStatus HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler           OnPullEVSEStatusHTTPRequest
        {

            add
            {
                EMPClient.OnPullEVSEStatusHTTPRequest += value;
            }

            remove
            {
                EMPClient.OnPullEVSEStatusHTTPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a PullEVSEStatus HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler          OnPullEVSEStatusHTTPResponse
        {

            add
            {
                EMPClient.OnPullEVSEStatusHTTPResponse += value;
            }

            remove
            {
                EMPClient.OnPullEVSEStatusHTTPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a PullEVSEStatus request had been received.
        /// </summary>
        public event OnPullEVSEStatusResponseDelegate  OnPullEVSEStatusResponse
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
        /// An event fired whenever a PullEVSEStatusById request will be send.
        /// </summary>
        public event OnPullEVSEStatusByIdRequestDelegate   OnPullEVSEStatusByIdRequest
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
        /// An event fired whenever a PullEVSEStatusById HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler               OnPullEVSEStatusByIdHTTPRequest
        {

            add
            {
                EMPClient.OnPullEVSEStatusByIdHTTPRequest += value;
            }

            remove
            {
                EMPClient.OnPullEVSEStatusByIdHTTPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a PullEVSEStatusById HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler              OnPullEVSEStatusByIdHTTPResponse
        {

            add
            {
                EMPClient.OnPullEVSEStatusByIdHTTPResponse += value;
            }

            remove
            {
                EMPClient.OnPullEVSEStatusByIdHTTPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a PullEVSEStatusById request had been received.
        /// </summary>
        public event OnPullEVSEStatusByIdResponseDelegate  OnPullEVSEStatusByIdResponse
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

        #region OnPullEVSEStatusByOperatorIdRequest/-Response

        /// <summary>
        /// An event fired whenever a PullEVSEStatusByOperatorId request will be send.
        /// </summary>
        public event OnPullEVSEStatusByOperatorIdRequestDelegate   OnPullEVSEStatusByOperatorIdRequest
        {

            add
            {
                EMPClient.OnPullEVSEStatusByOperatorIdRequest += value;
            }

            remove
            {
                EMPClient.OnPullEVSEStatusByOperatorIdRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a PullEVSEStatusByOperatorId HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                       OnPullEVSEStatusByOperatorIdHTTPRequest
        {

            add
            {
                EMPClient.OnPullEVSEStatusByOperatorIdHTTPRequest += value;
            }

            remove
            {
                EMPClient.OnPullEVSEStatusByOperatorIdHTTPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a PullEVSEStatusByOperatorId HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                      OnPullEVSEStatusByOperatorIdHTTPResponse
        {

            add
            {
                EMPClient.OnPullEVSEStatusByOperatorIdHTTPResponse += value;
            }

            remove
            {
                EMPClient.OnPullEVSEStatusByOperatorIdHTTPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a PullEVSEStatusByOperatorId request had been received.
        /// </summary>
        public event OnPullEVSEStatusByOperatorIdResponseDelegate  OnPullEVSEStatusByOperatorIdResponse
        {

            add
            {
                EMPClient.OnPullEVSEStatusByOperatorIdResponse += value;
            }

            remove
            {
                EMPClient.OnPullEVSEStatusByOperatorIdResponse -= value;
            }

        }

        #endregion


        #region OnPullPricingProductDataRequest/-Response

        /// <summary>
        /// An event fired whenever a PullPricingProductData request will be send.
        /// </summary>
        public event OnPullPricingProductDataRequestDelegate   OnPullPricingProductDataRequest
        {

            add
            {
                EMPClient.OnPullPricingProductDataRequest += value;
            }

            remove
            {
                EMPClient.OnPullPricingProductDataRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a PullPricingProductData HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                   OnPullPricingProductDataHTTPRequest
        {

            add
            {
                EMPClient.OnPullPricingProductDataHTTPRequest += value;
            }

            remove
            {
                EMPClient.OnPullPricingProductDataHTTPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a PullPricingProductData HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                  OnPullPricingProductDataHTTPResponse
        {

            add
            {
                EMPClient.OnPullPricingProductDataHTTPResponse += value;
            }

            remove
            {
                EMPClient.OnPullPricingProductDataHTTPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a PullPricingProductData request had been received.
        /// </summary>
        public event OnPullPricingProductDataResponseDelegate  OnPullPricingProductDataResponse
        {

            add
            {
                EMPClient.OnPullPricingProductDataResponse += value;
            }

            remove
            {
                EMPClient.OnPullPricingProductDataResponse -= value;
            }

        }

        #endregion

        #region OnPullEVSEPricingRequest/-Response

        /// <summary>
        /// An event fired whenever a PullEVSEPricing request will be send.
        /// </summary>
        public event OnPullEVSEPricingRequestDelegate   OnPullEVSEPricingRequest
        {

            add
            {
                EMPClient.OnPullEVSEPricingRequest += value;
            }

            remove
            {
                EMPClient.OnPullEVSEPricingRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a PullEVSEPricing HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler            OnPullEVSEPricingHTTPRequest
        {

            add
            {
                EMPClient.OnPullEVSEPricingHTTPRequest += value;
            }

            remove
            {
                EMPClient.OnPullEVSEPricingHTTPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a PullEVSEPricing HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler           OnPullEVSEPricingHTTPResponse
        {

            add
            {
                EMPClient.OnPullEVSEPricingHTTPResponse += value;
            }

            remove
            {
                EMPClient.OnPullEVSEPricingHTTPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a PullEVSEPricing request had been received.
        /// </summary>
        public event OnPullEVSEPricingResponseDelegate  OnPullEVSEPricingResponse
        {

            add
            {
                EMPClient.OnPullEVSEPricingResponse += value;
            }

            remove
            {
                EMPClient.OnPullEVSEPricingResponse -= value;
            }

        }

        #endregion


        #region OnPushAuthenticationDataRequest/-Response

        /// <summary>
        /// An event fired whenever a PushAuthenticationData request will be send.
        /// </summary>
        public event OnPushAuthenticationDataRequestDelegate   OnPushAuthenticationDataRequest
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
        /// An event fired whenever a PushAuthenticationData HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                   OnPushAuthenticationDataHTTPRequest
        {

            add
            {
                EMPClient.OnPushAuthenticationDataHTTPRequest += value;
            }

            remove
            {
                EMPClient.OnPushAuthenticationDataHTTPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a PushAuthenticationData HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                  OnPushAuthenticationDataHTTPResponse
        {

            add
            {
                EMPClient.OnPushAuthenticationDataHTTPResponse += value;
            }

            remove
            {
                EMPClient.OnPushAuthenticationDataHTTPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a PushAuthenticationData request had been received.
        /// </summary>
        public event OnPushAuthenticationDataResponseDelegate  OnPushAuthenticationDataResponse
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


        #region OnAuthorizeRemoteReservationStartRequest/-Response

        /// <summary>
        /// An event fired whenever a 'reservation start' request will be send.
        /// </summary>
        public event OnAuthorizeRemoteReservationStartRequestDelegate   OnAuthorizeRemoteReservationStartRequest
        {

            add
            {
                EMPClient.OnAuthorizeRemoteReservationStartRequest += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteReservationStartRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a 'reservation start' HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                            OnAuthorizeRemoteReservationStartHTTPRequest

        {

            add
            {
                EMPClient.OnAuthorizeRemoteReservationStartHTTPRequest += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteReservationStartHTTPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'reservation start' HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                           OnAuthorizeRemoteReservationStartHTTPResponse
        {

            add
            {
                EMPClient.OnAuthorizeRemoteReservationStartHTTPResponse += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteReservationStartHTTPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'reservation start' request had been received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStartResponseDelegate  OnAuthorizeRemoteReservationStartResponse
        {

            add
            {
                EMPClient.OnAuthorizeRemoteReservationStartResponse += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteReservationStartResponse -= value;
            }

        }

        #endregion

        #region OnAuthorizeRemoteReservationStopRequest/-Response

        /// <summary>
        /// An event fired whenever a 'reservation stop' request will be send.
        /// </summary>
        public event OnAuthorizeRemoteReservationStopRequestDelegate   OnAuthorizeRemoteReservationStopRequest
        {

            add
            {
                EMPClient.OnAuthorizeRemoteReservationStopRequest += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteReservationStopRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a 'reservation stop' HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                           OnAuthorizeRemoteReservationStopHTTPRequest
        {

            add
            {
                EMPClient.OnAuthorizeRemoteReservationStopHTTPRequest += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteReservationStopHTTPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'reservation stop' HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                          OnAuthorizeRemoteReservationStopHTTPResponse
        {

            add
            {
                EMPClient.OnAuthorizeRemoteReservationStopHTTPResponse += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteReservationStopHTTPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'reservation stop' request had been received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStopResponseDelegate  OnAuthorizeRemoteReservationStopResponse
        {

            add
            {
                EMPClient.OnAuthorizeRemoteReservationStopResponse += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteReservationStopResponse -= value;
            }

        }

        #endregion

        #region OnAuthorizeRemoteStartRequest/-Response

        /// <summary>
        /// An event fired whenever an 'authorize remote start' request will be send.
        /// </summary>
        public event OnAuthorizeRemoteStartRequestDelegate   OnAuthorizeRemoteStartRequest
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
        /// An event fired whenever an 'authorize remote start' HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                 OnAuthorizeRemoteStartHTTPRequest
        {

            add
            {
                EMPClient.OnAuthorizeRemoteStartHTTPRequest += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteStartHTTPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an 'authorize remote start' HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                OnAuthorizeRemoteStartHTTPResponse
        {

            add
            {
                EMPClient.OnAuthorizeRemoteStartHTTPResponse += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteStartHTTPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an 'authorize remote start' request had been received.
        /// </summary>
        public event OnAuthorizeRemoteStartResponseDelegate  OnAuthorizeRemoteStartResponse
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
        public event OnAuthorizeRemoteStopRequestDelegate   OnAuthorizeRemoteStopRequest
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
        /// An event fired whenever an 'authorize remote stop' HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                OnAuthorizeRemoteStopHTTPRequest
        {

            add
            {
                EMPClient.OnAuthorizeRemoteStopHTTPRequest += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteStopHTTPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an 'authorize remote stop' HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler               OnAuthorizeRemoteStopHTTPResponse
        {

            add
            {
                EMPClient.OnAuthorizeRemoteStopHTTPResponse += value;
            }

            remove
            {
                EMPClient.OnAuthorizeRemoteStopHTTPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an 'authorize remote stop' request had been received.
        /// </summary>
        public event OnAuthorizeRemoteStopResponseDelegate  OnAuthorizeRemoteStopResponse
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
        public event OnGetChargeDetailRecordsRequestDelegate   OnGetChargeDetailRecordsRequest
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
        /// An event fired whenever a 'get charge detail records' HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                   OnGetChargeDetailRecordsHTTPRequest
        {

            add
            {
                EMPClient.OnGetChargeDetailRecordsHTTPRequest += value;
            }

            remove
            {
                EMPClient.OnGetChargeDetailRecordsHTTPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a 'get charge detail records' HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                  OnGetChargeDetailRecordsHTTPResponse
        {

            add
            {
                EMPClient.OnGetChargeDetailRecordsHTTPResponse += value;
            }

            remove
            {
                EMPClient.OnGetChargeDetailRecordsHTTPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response for a 'get charge detail records' request was received.
        /// </summary>
        public event OnGetChargeDetailRecordsResponseDelegate  OnGetChargeDetailRecordsResponse
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

        #endregion

        #region EMPServer events

        #region OnAuthorizeStart       (HTTP)(Request/-Response)

        /// <summary>
        /// An event sent whenever an AuthorizeStart HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnAuthorizeStartHTTPRequest
            => EMPServer.OnAuthorizeStartHTTPRequest;

        /// <summary>
        /// An event sent whenever a authorize start request was received.
        /// </summary>
        public event OnAuthorizeStartRequestDelegate   OnAuthorizeStartRequest
        {

            add
            {
                EMPServer.OnAuthorizeStartRequest += value;
            }

            remove
            {
                EMPServer.OnAuthorizeStartRequest -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a authorize start request was received.
        /// </summary>
        public event OnAuthorizeStartDelegate          OnAuthorizeStart
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

        /// <summary>
        /// An event sent whenever a authorize start response was sent.
        /// </summary>
        public event OnAuthorizeStartResponseDelegate  OnAuthorizeStartResponse
        {

            add
            {
                EMPServer.OnAuthorizeStartResponse += value;
            }

            remove
            {
                EMPServer.OnAuthorizeStartResponse -= value;
            }

        }

        /// <summary>
        /// An event sent whenever an AuthorizationStart HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnAuthorizationStartHTTPResponse
            => EMPServer.OnAuthorizationStartHTTPResponse;

        #endregion

        #region OnAuthorizeStop        (HTTP)(Request/-Response)

        /// <summary>
        /// An event sent whenever an AuthorizeStop HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnAuthorizeStopHTTPRequest
            => EMPServer.OnAuthorizeStopHTTPRequest;

        /// <summary>
        /// An event sent whenever a authorize stop HTTP request was received.
        /// </summary>
        public event OnAuthorizeStopRequestDelegate   OnAuthorizeStopRequest
        {

            add
            {
                EMPServer.OnAuthorizeStopRequest += value;
            }

            remove
            {
                EMPServer.OnAuthorizeStopRequest -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a authorize stop command was received.
        /// </summary>
        public event OnAuthorizeStopDelegate          OnAuthorizeStop
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

        /// <summary>
        /// An event sent whenever a authorize stop SOAP response was sent.
        /// </summary>
        public event OnAuthorizeStopResponseDelegate  OnAuthorizeStopResponse
        {

            add
            {
                EMPServer.OnAuthorizeStopResponse += value;
            }

            remove
            {
                EMPServer.OnAuthorizeStopResponse -= value;
            }

        }

        /// <summary>
        /// An event sent whenever an AuthorizationStop HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnAuthorizationStopHTTPResponse
            => EMPServer.OnAuthorizationStopHTTPResponse;

        #endregion


        #region OnChargingNotifications(HTTP)(Request/-Response)

        /// <summary>
        /// An event sent whenever a ChargingNotification HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnChargingNotificationsHTTPRequest
            => EMPServer.OnChargingNotificationsHTTPRequest;


        public event OnChargingStartNotificationRequestDelegate?      OnChargingStartNotificationRequest
        {

            add
            {
                EMPServer.OnChargingStartNotificationRequest += value;
            }

            remove
            {
                EMPServer.OnChargingStartNotificationRequest -= value;
            }

        }

        public event OnChargingStartNotificationDelegate?             OnChargingStartNotification
        {

            add
            {
                EMPServer.OnChargingStartNotification += value;
            }

            remove
            {
                EMPServer.OnChargingStartNotification -= value;
            }

        }

        public event OnChargingStartNotificationResponseDelegate?     OnChargingStartNotificationResponse
        {

            add
            {
                EMPServer.OnChargingStartNotificationResponse += value;
            }

            remove
            {
                EMPServer.OnChargingStartNotificationResponse -= value;
            }

        }


        public event OnChargingProgressNotificationRequestDelegate?   OnChargingProgressNotificationRequest
        {

            add
            {
                EMPServer.OnChargingProgressNotificationRequest += value;
            }

            remove
            {
                EMPServer.OnChargingProgressNotificationRequest -= value;
            }

        }

        public event OnChargingProgressNotificationDelegate?          OnChargingProgressNotification
        {

            add
            {
                EMPServer.OnChargingProgressNotification += value;
            }

            remove
            {
                EMPServer.OnChargingProgressNotification -= value;
            }

        }

        public event OnChargingProgressNotificationResponseDelegate?  OnChargingProgressNotificationResponse
        {

            add
            {
                EMPServer.OnChargingProgressNotificationResponse += value;
            }

            remove
            {
                EMPServer.OnChargingProgressNotificationResponse -= value;
            }

        }


        public event OnChargingEndNotificationRequestDelegate?        OnChargingEndNotificationRequest
        {

            add
            {
                EMPServer.OnChargingEndNotificationRequest += value;
            }

            remove
            {
                EMPServer.OnChargingEndNotificationRequest -= value;
            }

        }

        public event OnChargingEndNotificationDelegate?               OnChargingEndNotification
        {

            add
            {
                EMPServer.OnChargingEndNotification += value;
            }

            remove
            {
                EMPServer.OnChargingEndNotification -= value;
            }

        }

        public event OnChargingEndNotificationResponseDelegate?       OnChargingEndNotificationResponse
        {

            add
            {
                EMPServer.OnChargingEndNotificationResponse += value;
            }

            remove
            {
                EMPServer.OnChargingEndNotificationResponse -= value;
            }

        }


        public event OnChargingErrorNotificationRequestDelegate?      OnChargingErrorNotificationRequest
        {

            add
            {
                EMPServer.OnChargingErrorNotificationRequest += value;
            }

            remove
            {
                EMPServer.OnChargingErrorNotificationRequest -= value;
            }

        }

        public event OnChargingErrorNotificationDelegate?             OnChargingErrorNotification
        {

            add
            {
                EMPServer.OnChargingErrorNotification += value;
            }

            remove
            {
                EMPServer.OnChargingErrorNotification -= value;
            }

        }

        public event OnChargingErrorNotificationResponseDelegate?     OnChargingErrorNotificationResponse
        {

            add
            {
                EMPServer.OnChargingErrorNotificationResponse += value;
            }

            remove
            {
                EMPServer.OnChargingErrorNotificationResponse -= value;
            }

        }


        /// <summary>
        /// An event sent whenever a ChargingNotification HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnChargingNotificationsHTTPResponse
            => EMPServer.OnChargingNotificationsHTTPResponse;

        #endregion


        #region OnChargeDetailRecord   (HTTP)(Request/-Response)

        /// <summary>
        /// An event sent whenever a ChargeDetailRecord HTTP request was received.
        /// </summary>
        public HTTPRequestLogEvent OnChargeDetailRecordHTTPRequest
            => EMPServer.OnChargeDetailRecordHTTPRequest;

        /// <summary>
        /// An event sent whenever a charge detail record request was received.
        /// </summary>
        public event OnChargeDetailRecordRequestDelegate   OnChargeDetailRecordRequest
        {

            add
            {
                EMPServer.OnChargeDetailRecordRequest += value;
            }

            remove
            {
                EMPServer.OnChargeDetailRecordRequest -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a charge detail record was received.
        /// </summary>
        public event OnChargeDetailRecordDelegate          OnChargeDetailRecord
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

        /// <summary>
        /// An event sent whenever a charge detail record response was sent.
        /// </summary>
        public event OnChargeDetailRecordResponseDelegate  OnSendChargeDetailRecordResponse
        {

            add
            {
                EMPServer.OnChargeDetailRecordResponse += value;
            }

            remove
            {
                EMPServer.OnChargeDetailRecordResponse -= value;
            }

        }

        /// <summary>
        /// An event sent whenever a ChargeDetailRecord HTTP response was sent.
        /// </summary>
        public HTTPResponseLogEvent OnChargeDetailRecordHTTPResponse
            => EMPServer.OnChargeDetailRecordHTTPResponse;

        #endregion

        #endregion

        #region HTTP server logging

        /// <summary>
        /// An event called whenever a HTTP request came in.
        /// </summary>
        public HTTPRequestLogEvent   RequestLog
            => EMPServer.RequestLog;

        /// <summary>
        /// An event called whenever a HTTP request could successfully be processed.
        /// </summary>
        public HTTPResponseLogEvent  ResponseLog
            => EMPServer.ResponseLog;

        /// <summary>
        /// An event called whenever a HTTP request resulted in an error.
        /// </summary>
        public HTTPErrorLogEvent     ErrorLog
            => EMPServer.ErrorLog;

        #endregion


        #region PullEVSEData               (Request)

        /// <summary>
        /// Download EVSE data records.
        /// The request might either have none, 'SearchCenter + DistanceKM' or 'LastCall' parameters.
        /// Because of limitations at Hubject the SearchCenter and LastCall parameters can not be used at the same time!
        /// </summary>
        /// <param name="Request">An PullEVSEData request.</param>
        public Task<OICPResult<PullEVSEDataResponse>>

            PullEVSEData(PullEVSEDataRequest Request)

                => EMPClient.PullEVSEData(Request);

        #endregion

        #region PullEVSEStatus             (Request)

        /// <summary>
        /// Download EVSE status records.
        /// The request might have an optional search radius and/or status filter.
        /// </summary>
        /// <param name="Request">A PullEVSEStatus request.</param>
        public Task<OICPResult<PullEVSEStatusResponse>>

            PullEVSEStatus(PullEVSEStatusRequest Request)

                => EMPClient.PullEVSEStatus(Request);

        #endregion

        #region PullEVSEStatusById         (Request)

        /// <summary>
        /// Download the current status of up to 100 EVSEs.
        /// </summary>
        /// <param name="Request">A PullEVSEStatusById request.</param>
        public Task<OICPResult<PullEVSEStatusByIdResponse>>

            PullEVSEStatusById(PullEVSEStatusByIdRequest Request)

                => EMPClient.PullEVSEStatusById(Request);

        #endregion

        #region PullEVSEStatusByOperatorId (Request)

        /// <summary>
        /// Download the current EVSE status of the given charge point operators.
        /// </summary>
        /// <param name="Request">A PullEVSEStatusById request.</param>
        public Task<OICPResult<PullEVSEStatusByOperatorIdResponse>>

            PullEVSEStatusByOperatorId(PullEVSEStatusByOperatorIdRequest Request)

                => EMPClient.PullEVSEStatusByOperatorId(Request);

        #endregion


        #region PullPricingProductData     (Request)

        /// <summary>
        /// Download pricing product data.
        /// </summary>
        /// <param name="Request">A PullPricingProductData request.</param>
        public Task<OICPResult<PullPricingProductDataResponse>>

            PullPricingProductData(PullPricingProductDataRequest Request)

                => EMPClient.PullPricingProductData(Request);

        #endregion

        #region PullEVSEPricing            (Request)

        /// <summary>
        /// Create a new task pushing provider authentication data records onto the OICP server.
        /// </summary>
        /// <param name="Request">A PushAuthenticationData request.</param>
        public Task<OICPResult<PullEVSEPricingResponse>>

            PullEVSEPricing(PullEVSEPricingRequest Request)

                => EMPClient.PullEVSEPricing(Request);

        #endregion


        #region PushAuthenticationData     (Request)

        /// <summary>
        /// Create a new task pushing provider authentication data records onto the OICP server.
        /// </summary>
        /// <param name="Request">A PushAuthenticationData request.</param>
        public Task<OICPResult<Acknowledgement<PushAuthenticationDataRequest>>>

            PushAuthenticationData(PushAuthenticationDataRequest Request)

                => EMPClient.PushAuthenticationData(Request);

        #endregion


        #region ReservationStart           (Request)

        /// <summary>
        /// Create a charging reservation at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteReservationStart request.</param>
        public Task<OICPResult<Acknowledgement<AuthorizeRemoteReservationStartRequest>>>

            AuthorizeRemoteReservationStart(AuthorizeRemoteReservationStartRequest  Request)

                => EMPClient.AuthorizeRemoteReservationStart(Request);

        #endregion

        #region ReservationStop            (Request)

        /// <summary>
        /// Stop the given charging reservation.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteReservationStop request.</param>
        public Task<OICPResult<Acknowledgement<AuthorizeRemoteReservationStopRequest>>>

            AuthorizeRemoteReservationStop(AuthorizeRemoteReservationStopRequest Request)

                => EMPClient.AuthorizeRemoteReservationStop(Request);

        #endregion

        #region RemoteStart                (Request)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteStart request.</param>
        public Task<OICPResult<Acknowledgement<AuthorizeRemoteStartRequest>>>

            AuthorizeRemoteStart(AuthorizeRemoteStartRequest Request)

                => EMPClient.AuthorizeRemoteStart(Request);

        #endregion

        #region RemoteStop                 (Request)

        /// <summary>
        /// Stop the given charging session.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteStop request.</param>
        public Task<OICPResult<Acknowledgement<AuthorizeRemoteStopRequest>>>

            AuthorizeRemoteStop(AuthorizeRemoteStopRequest Request)

                => EMPClient.AuthorizeRemoteStop(Request);

        #endregion


        #region GetChargeDetailRecords     (Request)

        /// <summary>
        /// Download charge detail records.
        /// </summary>
        /// <param name="Request">An GetChargeDetailRecords request.</param>
        public Task<OICPResult<GetChargeDetailRecordsResponse>>

            GetChargeDetailRecords(GetChargeDetailRecordsRequest Request)

                => EMPClient.GetChargeDetailRecords(Request);

        #endregion


        #region Start(EventTrackingId = null)

        /// <summary>
        /// Start this API.
        /// </summary>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        public Task<Boolean> Start(EventTracking_Id? EventTrackingId = null)

            => EMPServer.Start(EventTrackingId);

        #endregion

        #region Shutdown(EventTrackingId = null, Message = null, Wait = true)

        /// <summary>
        /// Shutdown this API.
        /// </summary>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="Message">An optional shutdown message.</param>
        /// <param name="Wait">Whether to wait for the shutdown to complete.</param>
        public Task<Boolean> Shutdown(EventTracking_Id?  EventTrackingId   = null,
                                      String?            Message           = null,
                                      Boolean            Wait              = true)

            => EMPServer.Shutdown(
                   EventTrackingId ?? EventTracking_Id.New,
                   Message,
                   Wait
               );

        #endregion

        #region Dispose()

        /// <summary>
        /// Dispose this object.
        /// </summary>
        public void Dispose()
        {
            EMPServer?.Dispose();
        }

        #endregion


    }

}
