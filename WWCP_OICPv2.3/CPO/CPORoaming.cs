﻿/*
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
using cloud.charging.open.protocols.OICPv2_3.EMP;

#endregion

namespace cloud.charging.open.protocols.OICPv2_3.CPO
{

    /// <summary>
    /// The CPO roaming object combines the CPO client and CPO server
    /// and adds additional logging for both.
    /// </summary>
    /// <param name="CPOClient">A CPO client.</param>
    /// <param name="CPOServer">A CPO sever.</param>
    public class CPORoaming(CPOClient     CPOClient,
                            CPOServerAPI  CPOServer) : ICPOClient
    {

        #region Properties

        /// <summary>
        /// The CPO client.
        /// </summary>
        public CPOClient     CPOClient    { get; } = CPOClient ?? throw new ArgumentNullException(nameof(CPOClient), "The given CPOClient must not be null!");

        /// <summary>
        /// The CPO server.
        /// </summary>
        public CPOServerAPI  CPOServer    { get; } = CPOServer ?? new CPOServerAPI(AutoStart: false);

        #region ICPOClient

        /// <summary>
        /// The remote URL of the OICP HTTP endpoint to connect to.
        /// </summary>
        URL                                                         IHTTPClient.RemoteURL
            => CPOClient.RemoteURL;

        /// <summary>
        /// The virtual HTTP hostname to connect to.
        /// </summary>
        HTTPHostname?                                               IHTTPClient.VirtualHostname
            => CPOClient.VirtualHostname;

        /// <summary>
        /// An optional description of this CPO client.
        /// </summary>
        I18NString?                                                 IHTTPClient.Description
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
        /// The remote TLS certificate validator.
        /// </summary>
        RemoteTLSServerCertificateValidationHandler<IHTTPClient>?   IHTTPClient.RemoteCertificateValidator
            => CPOClient.RemoteCertificateValidator;

        /// <summary>
        /// The TLS client certificate to use of HTTP authentication.
        /// </summary>
        X509Certificate?                                            IHTTPClient.ClientCert
            => CPOClient.ClientCert;

        /// <summary>
        /// The TLS protocol to use.
        /// </summary>
        SslProtocols                                                IHTTPClient.TLSProtocol
            => CPOClient.TLSProtocol;

        /// <summary>
        /// Prefer IPv4 instead of IPv6.
        /// </summary>
        Boolean                                                     IHTTPClient.PreferIPv4
            => CPOClient.PreferIPv4;

        /// <summary>
        /// The optional HTTP connection type.
        /// </summary>
        ConnectionType?                                             IHTTPClient.Connection
            => CPOClient.Connection;

        /// <summary>
        /// The optional HTTP content type.
        /// </summary>
        HTTPContentType?                                            IHTTPClient.ContentType
            => CPOClient.ContentType;

        /// <summary>
        /// The optional HTTP accept header.
        /// </summary>
        AcceptTypes?                                                IHTTPClient.Accept
            => CPOClient.Accept;

        /// <summary>
        /// The optional HTTP authentication to use.
        /// </summary>
        IHTTPAuthentication?                                        IHTTPClient.Authentication
            => CPOClient.Authentication;

        /// <summary>
        /// The HTTP user agent identification.
        /// </summary>
        String                                                      IHTTPClient.HTTPUserAgent
            => CPOClient.HTTPUserAgent;

        /// <summary>
        /// The timeout for upstream requests.
        /// </summary>
        TimeSpan                                                    IHTTPClient.RequestTimeout
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
        TransmissionRetryDelayDelegate                              IHTTPClient.TransmissionRetryDelay
            => CPOClient.TransmissionRetryDelay;

        /// <summary>
        /// The maximum number of retries when communicationg with the remote OICP service.
        /// </summary>
        UInt16                                                      IHTTPClient.MaxNumberOfRetries
            => CPOClient.MaxNumberOfRetries;

        /// <summary>
        /// Make use of HTTP pipelining.
        /// </summary>
        Boolean                                                     IHTTPClient.UseHTTPPipelining
            => CPOClient.UseHTTPPipelining;

        /// <summary>
        /// The CPO client (HTTP client) logger.
        /// </summary>
        HTTPClientLogger                                            IHTTPClient.HTTPLogger
            => CPOClient.HTTPLogger;

        /// <summary>
        /// The DNS client defines which DNS servers to use.
        /// </summary>
        DNSClient                                                   IHTTPClient.DNSClient
            => CPOClient.DNSClient;


        Boolean IHTTPClient.Connected
            => CPOClient.Connected;

        IIPAddress? IHTTPClient.RemoteIPAddress
            => CPOClient.RemoteIPAddress;

        #endregion

        #endregion

        #region CPOClient events

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


        #region OnPushPricingProductDataRequest/-Response

        /// <summary>
        /// An event fired whenever a PushPricingProductData will be send.
        /// </summary>
        public event OnPushPricingProductDataRequestDelegate   OnPushPricingProductDataRequest
        {

            add
            {
                CPOClient.OnPushPricingProductDataRequest += value;
            }

            remove
            {
                CPOClient.OnPushPricingProductDataRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a PushPricingProductData HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                   OnPushPricingProductDataHTTPRequest
        {

            add
            {
                CPOClient.OnPushPricingProductDataHTTPRequest += value;
            }

            remove
            {
                CPOClient.OnPushPricingProductDataHTTPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a PushPricingProductData HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                  OnPushPricingProductDataHTTPResponse
        {

            add
            {
                CPOClient.OnPushPricingProductDataHTTPResponse += value;
            }

            remove
            {
                CPOClient.OnPushPricingProductDataHTTPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a PushPricingProductData HTTP request had been received.
        /// </summary>
        public event OnPushPricingProductDataResponseDelegate  OnPushPricingProductDataResponse
        {

            add
            {
                CPOClient.OnPushPricingProductDataResponse += value;
            }

            remove
            {
                CPOClient.OnPushPricingProductDataResponse -= value;
            }

        }

        #endregion

        #region OnPushEVSEPricingRequest/-Response

        /// <summary>
        /// An event fired whenever a PushEVSEPricing will be send.
        /// </summary>
        public event OnPushEVSEPricingRequestDelegate   OnPushEVSEPricingRequest
        {

            add
            {
                CPOClient.OnPushEVSEPricingRequest += value;
            }

            remove
            {
                CPOClient.OnPushEVSEPricingRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a PushEVSEPricing HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler            OnPushEVSEPricingHTTPRequest
        {

            add
            {
                CPOClient.OnPushEVSEPricingHTTPRequest += value;
            }

            remove
            {
                CPOClient.OnPushEVSEPricingHTTPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a PushEVSEPricing HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler           OnPushEVSEPricingHTTPResponse
        {

            add
            {
                CPOClient.OnPushEVSEPricingHTTPResponse += value;
            }

            remove
            {
                CPOClient.OnPushEVSEPricingHTTPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a PushEVSEPricing HTTP request had been received.
        /// </summary>
        public event OnPushEVSEPricingResponseDelegate  OnPushEVSEPricingResponse
        {

            add
            {
                CPOClient.OnPushEVSEPricingResponse += value;
            }

            remove
            {
                CPOClient.OnPushEVSEPricingResponse -= value;
            }

        }

        #endregion


        #region OnAuthorizeStartRequest/-Response

        /// <summary>
        /// An event fired whenever an authorize start request will be send.
        /// </summary>
        public event OnAuthorizeStartRequestDelegate OnAuthorizeStartRequest
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
        public event OnAuthorizeStartResponseDelegate OnAuthorizeStartResponse
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
        public event OnAuthorizeStopRequestDelegate OnAuthorizeStopRequest
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
        public event OnAuthorizeStopResponseDelegate OnAuthorizeStopResponse
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


        #region OnChargingStartNotificationRequest/-Response

        /// <summary>
        /// An event fired whenever a ChargingStartNotification will be send.
        /// </summary>
        public event OnChargingStartNotificationRequestDelegate   OnChargingStartNotificationRequest
        {

            add
            {
                CPOClient.OnChargingStartNotificationRequest += value;
            }

            remove
            {
                CPOClient.OnChargingStartNotificationRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a ChargingStartNotification HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                      OnChargingStartNotificationHTTPRequest
        {

            add
            {
                CPOClient.OnChargingStartNotificationHTTPRequest += value;
            }

            remove
            {
                CPOClient.OnChargingStartNotificationHTTPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a ChargingStartNotification HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                     OnChargingStartNotificationHTTPResponse
        {

            add
            {
                CPOClient.OnChargingStartNotificationHTTPResponse += value;
            }

            remove
            {
                CPOClient.OnChargingStartNotificationHTTPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a ChargingStartNotification had been received.
        /// </summary>
        public event OnChargingStartNotificationResponseDelegate  OnChargingStartNotificationResponse
        {

            add
            {
                CPOClient.OnChargingStartNotificationResponse += value;
            }

            remove
            {
                CPOClient.OnChargingStartNotificationResponse -= value;
            }

        }

        #endregion

        #region OnChargingProgressNotificationRequest/-Response

        /// <summary>
        /// An event fired whenever a ChargingProgressNotification will be send.
        /// </summary>
        public event OnChargingProgressNotificationRequestDelegate   OnChargingProgressNotificationRequest
        {

            add
            {
                CPOClient.OnChargingProgressNotificationRequest += value;
            }

            remove
            {
                CPOClient.OnChargingProgressNotificationRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a ChargingProgressNotification HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                         OnChargingProgressNotificationHTTPRequest
        {

            add
            {
                CPOClient.OnChargingProgressNotificationHTTPRequest += value;
            }

            remove
            {
                CPOClient.OnChargingProgressNotificationHTTPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a ChargingProgressNotification HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                        OnChargingProgressNotificationHTTPResponse
        {

            add
            {
                CPOClient.OnChargingProgressNotificationHTTPResponse += value;
            }

            remove
            {
                CPOClient.OnChargingProgressNotificationHTTPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a ChargingProgressNotification had been received.
        /// </summary>
        public event OnChargingProgressNotificationResponseDelegate  OnChargingProgressNotificationResponse
        {

            add
            {
                CPOClient.OnChargingProgressNotificationResponse += value;
            }

            remove
            {
                CPOClient.OnChargingProgressNotificationResponse -= value;
            }

        }

        #endregion

        #region OnChargingEndNotificationRequest/-Response

        /// <summary>
        /// An event fired whenever a ChargingEndNotification will be send.
        /// </summary>
        public event OnChargingEndNotificationRequestDelegate   OnChargingEndNotificationRequest
        {

            add
            {
                CPOClient.OnChargingEndNotificationRequest += value;
            }

            remove
            {
                CPOClient.OnChargingEndNotificationRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a ChargingEndNotification HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                    OnChargingEndNotificationHTTPRequest
        {

            add
            {
                CPOClient.OnChargingEndNotificationHTTPRequest += value;
            }

            remove
            {
                CPOClient.OnChargingEndNotificationHTTPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a ChargingEndNotification HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                   OnChargingEndNotificationHTTPResponse
        {

            add
            {
                CPOClient.OnChargingEndNotificationHTTPResponse += value;
            }

            remove
            {
                CPOClient.OnChargingEndNotificationHTTPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a ChargingEndNotification had been received.
        /// </summary>
        public event OnChargingEndNotificationResponseDelegate  OnChargingEndNotificationResponse
        {

            add
            {
                CPOClient.OnChargingEndNotificationResponse += value;
            }

            remove
            {
                CPOClient.OnChargingEndNotificationResponse -= value;
            }

        }

        #endregion

        #region OnChargingErrorNotificationRequest/-Response

        /// <summary>
        /// An event fired whenever a ChargingErrorNotification will be send.
        /// </summary>
        public event OnChargingErrorNotificationRequestDelegate   OnChargingErrorNotificationRequest
        {

            add
            {
                CPOClient.OnChargingErrorNotificationRequest += value;
            }

            remove
            {
                CPOClient.OnChargingErrorNotificationRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a ChargingErrorNotification HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                      OnChargingErrorNotificationHTTPRequest
        {

            add
            {
                CPOClient.OnChargingErrorNotificationHTTPRequest += value;
            }

            remove
            {
                CPOClient.OnChargingErrorNotificationHTTPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a ChargingErrorNotification HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                     OnChargingErrorNotificationHTTPResponse
        {

            add
            {
                CPOClient.OnChargingErrorNotificationHTTPResponse += value;
            }

            remove
            {
                CPOClient.OnChargingErrorNotificationHTTPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a ChargingErrorNotification had been received.
        /// </summary>
        public event OnChargingErrorNotificationResponseDelegate  OnChargingErrorNotificationResponse
        {

            add
            {
                CPOClient.OnChargingErrorNotificationResponse += value;
            }

            remove
            {
                CPOClient.OnChargingErrorNotificationResponse -= value;
            }

        }

        #endregion


        #region OnSendChargeDetailRecordRequest/-Response

        /// <summary>
        /// An event fired whenever a charge detail record will be send.
        /// </summary>
        public event OnSendChargeDetailRecordRequestDelegate OnSendChargeDetailRecordRequest
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
        public event OnSendChargeDetailRecordResponseDelegate OnSendChargeDetailRecordResponse
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

        #endregion

        #region CPOServer events

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

        #endregion

        #region HTTP server logging

        /// <summary>
        /// An event called whenever a HTTP request came in.
        /// </summary>
        public HTTPRequestLogEvent   RequestLog
            => CPOServer.RequestLog;

        /// <summary>
        /// An event called whenever a HTTP request could successfully be processed.
        /// </summary>
        public HTTPResponseLogEvent  ResponseLog
            => CPOServer.ResponseLog;

        /// <summary>
        /// An event called whenever a HTTP request resulted in an error.
        /// </summary>
        public HTTPErrorLogEvent     ErrorLog
            => CPOServer.ErrorLog;

        #endregion


        #region PushEVSEData                    (Request)

        /// <summary>
        /// Upload the given EVSE data records.
        /// </summary>
        /// <param name="Request">A PushEVSEData request.</param>
        public Task<OICPResult<Acknowledgement<PushEVSEDataRequest>>>

            PushEVSEData(PushEVSEDataRequest Request)

                => CPOClient.PushEVSEData(Request);

        #endregion

        #region PushEVSEStatus                  (Request)

        /// <summary>
        /// Upload the given EVSE status records.
        /// </summary>
        /// <param name="Request">A PushEVSEStatus request.</param>
        public Task<OICPResult<Acknowledgement<PushEVSEStatusRequest>>>

            PushEVSEStatus(PushEVSEStatusRequest Request)

                => CPOClient.PushEVSEStatus(Request);

        #endregion


        #region PushPricingProductData          (Request)

        /// <summary>
        /// Upload the given pricing product data.
        /// </summary>
        /// <param name="Request">A PushPricingProductDataRequest request.</param>
        public Task<OICPResult<Acknowledgement<PushPricingProductDataRequest>>>

            PushPricingProductData(PushPricingProductDataRequest Request)

                => CPOClient.PushPricingProductData(Request);

        #endregion

        #region PushEVSEPricing                 (Request)

        /// <summary>
        /// Upload the given EVSE pricing data.
        /// </summary>
        /// <param name="Request">A PushEVSEPricingRequest request.</param>
        public Task<OICPResult<Acknowledgement<PushEVSEPricingRequest>>>

            PushEVSEPricing(PushEVSEPricingRequest Request)

                => CPOClient.PushEVSEPricing(Request);

        #endregion


        #region PullAuthenticationData          (Request)  [Obsolete!]

        /// <summary>
        /// Download provider authentication data.
        /// </summary>
        /// <param name="Request">A PullAuthenticationData request.</param>
        [Obsolete("PullAuthenticationData was removed from OICP.")]
        public Task<OICPResult<PullAuthenticationDataResponse>>

            PullAuthenticationData(PullAuthenticationDataRequest Request)

                => CPOClient.PullAuthenticationData(Request);

        #endregion


        #region AuthorizeStart                  (Request)

        /// <summary>
        /// Authorize for starting a charging session.
        /// </summary>
        /// <param name="Request">An AuthorizeStart request.</param>
        public Task<OICPResult<AuthorizationStartResponse>>

            AuthorizeStart(AuthorizeStartRequest Request)

                => CPOClient.AuthorizeStart(Request);

        #endregion

        #region AuthorizeStop                   (Request)

        /// <summary>
        /// Authorize for stopping a charging session.
        /// </summary>
        /// <param name="Request">An AuthorizeStop request.</param>
        public Task<OICPResult<AuthorizationStopResponse>>

            AuthorizeStop(AuthorizeStopRequest Request)

                => CPOClient.AuthorizeStop(Request);

        #endregion


        #region SendChargingStartNotification   (Request)

        /// <summary>
        /// Send a charging start notification.
        /// </summary>
        /// <param name="Request">A ChargingStartNotification request.</param>
        public Task<OICPResult<Acknowledgement<ChargingStartNotificationRequest>>>

            SendChargingStartNotification(ChargingStartNotificationRequest Request)

                => CPOClient.SendChargingStartNotification(Request);

        #endregion

        #region SendChargingProgressNotification(Request)

        /// <summary>
        /// Send a charging progress notification.
        /// </summary>
        /// <param name="Request">A ChargingProgressNotification request.</param>
        public Task<OICPResult<Acknowledgement<ChargingProgressNotificationRequest>>>

            SendChargingProgressNotification(ChargingProgressNotificationRequest Request)

                => CPOClient.SendChargingProgressNotification(Request);

        #endregion

        #region SendChargingEndNotification     (Request)

        /// <summary>
        /// Send a charging end notification.
        /// </summary>
        /// <param name="Request">A ChargingEndNotification request.</param>
        public Task<OICPResult<Acknowledgement<ChargingEndNotificationRequest>>>

            SendChargingEndNotification(ChargingEndNotificationRequest Request)

                => CPOClient.SendChargingEndNotification(Request);

        #endregion

        #region SendChargingErrorNotification   (Request)

        /// <summary>
        /// Send a charging error notification.
        /// </summary>
        /// <param name="Request">A ChargingErrorNotification request.</param>
        public Task<OICPResult<Acknowledgement<ChargingErrorNotificationRequest>>>

            SendChargingErrorNotification(ChargingErrorNotificationRequest Request)

                => CPOClient.SendChargingErrorNotification(Request);

        #endregion


        #region SendChargeDetailRecord          (Request)

        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="Request">A SendChargeDetailRecord request.</param>
        public Task<OICPResult<Acknowledgement<ChargeDetailRecordRequest>>>

            SendChargeDetailRecord(ChargeDetailRecordRequest Request)

                => CPOClient.SendChargeDetailRecord(Request);

        #endregion


        #region Start(EventTrackingId = null)

        /// <summary>
        /// Start this API.
        /// </summary>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        public Task<Boolean> Start(EventTracking_Id? EventTrackingId = null)

            => CPOServer.Start(EventTrackingId);

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

            => CPOServer.Shutdown(
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
            CPOServer?.Dispose();
        }

        #endregion


    }

}
