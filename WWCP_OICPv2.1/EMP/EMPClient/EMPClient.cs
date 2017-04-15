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
using System.Xml.Linq;
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
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.EMP
{

    /// <summary>
    /// An OICP EMP client.
    /// </summary>
    public partial class EMPClient : ASOAPClient,
                                     IEMPClient
    {

        #region Data

        /// <summary>
        /// The default HTTP user agent string.
        /// </summary>
        public new const           String  DefaultHTTPUserAgent  = "GraphDefined OICP " + Version.Number + " EMP Client";

        /// <summary>
        /// The default remote TCP port to connect to.
        /// </summary>
        public new static readonly IPPort  DefaultRemotePort     = IPPort.Parse(443);

        /// <summary>
        /// The default URI prefix.
        /// </summary>
        public const               String  DefaultURIPrefix      = "/ibis/ws";

        #endregion

        #region Properties

        /// <summary>
        /// The attached OICP EMP client (HTTP/SOAP client) logger.
        /// </summary>
        public EMPClientLogger Logger { get; }

        #endregion

        #region Custom request mappers

        #region CustomPullEVSEData(SOAP)RequestMapper

        #region CustomPullEVSEDataRequestMapper

        private Func<PullEVSEDataRequest, PullEVSEDataRequest> _CustomPullEVSEDataRequestMapper = _ => _;

        public Func<PullEVSEDataRequest, PullEVSEDataRequest> CustomPullEVSEDataRequestMapper
        {

            get
            {
                return _CustomPullEVSEDataRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomPullEVSEDataRequestMapper = value;
            }

        }

        #endregion

        #region CustomPullEVSEDataSOAPRequestMapper

        private Func<PullEVSEDataRequest, XElement, XElement> _CustomPullEVSEDataSOAPRequestMapper = (request, xml) => xml;

        public Func<PullEVSEDataRequest, XElement, XElement> CustomPullEVSEDataSOAPRequestMapper
        {

            get
            {
                return _CustomPullEVSEDataSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomPullEVSEDataSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<Acknowledgement<PullEVSEDataRequest>, Acknowledgement<PullEVSEDataRequest>.Builder> CustomPullEVSEDataResponseMapper  { get; set; }

        #endregion

        #region CustomSearchEVSE(SOAP)RequestMapper

        #region CustomSearchEVSERequestMapper

        private Func<SearchEVSERequest, SearchEVSERequest> _CustomSearchEVSERequestMapper = _ => _;

        public Func<SearchEVSERequest, SearchEVSERequest> CustomSearchEVSERequestMapper
        {

            get
            {
                return _CustomSearchEVSERequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomSearchEVSERequestMapper = value;
            }

        }

        #endregion

        #region CustomSearchEVSESOAPRequestMapper

        private Func<SearchEVSERequest, XElement, XElement> _CustomSearchEVSESOAPRequestMapper = (request, xml) => xml;

        public Func<SearchEVSERequest, XElement, XElement> CustomSearchEVSESOAPRequestMapper
        {

            get
            {
                return _CustomSearchEVSESOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomSearchEVSESOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<Acknowledgement<SearchEVSERequest>, Acknowledgement<SearchEVSERequest>.Builder> CustomSearchEVSEResponseMapper  { get; set; }

        #endregion


        #region CustomPullEVSEStatus(SOAP)RequestMapper

        #region CustomPullEVSEStatusRequestMapper

        private Func<PullEVSEStatusRequest, PullEVSEStatusRequest> _CustomPullEVSEStatusRequestMapper = _ => _;

        public Func<PullEVSEStatusRequest, PullEVSEStatusRequest> CustomPullEVSEStatusRequestMapper
        {

            get
            {
                return _CustomPullEVSEStatusRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomPullEVSEStatusRequestMapper = value;
            }

        }

        #endregion

        #region CustomPullEVSEStatusSOAPRequestMapper

        private Func<PullEVSEStatusRequest, XElement, XElement> _CustomPullEVSEStatusSOAPRequestMapper = (request, xml) => xml;

        public Func<PullEVSEStatusRequest, XElement, XElement> CustomPullEVSEStatusSOAPRequestMapper
        {

            get
            {
                return _CustomPullEVSEStatusSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomPullEVSEStatusSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<Acknowledgement<PullEVSEStatusRequest>, Acknowledgement<PullEVSEStatusRequest>.Builder> CustomPullEVSEStatusResponseMapper  { get; set; }

        #endregion

        #region CustomPullEVSEStatusById(SOAP)RequestMapper

        #region CustomPullEVSEStatusByIdRequestMapper

        private Func<PullEVSEStatusByIdRequest, PullEVSEStatusByIdRequest> _CustomPullEVSEStatusByIdRequestMapper = _ => _;

        public Func<PullEVSEStatusByIdRequest, PullEVSEStatusByIdRequest> CustomPullEVSEStatusByIdRequestMapper
        {

            get
            {
                return _CustomPullEVSEStatusByIdRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomPullEVSEStatusByIdRequestMapper = value;
            }

        }

        #endregion

        #region CustomPullEVSEStatusByIdSOAPRequestMapper

        private Func<PullEVSEStatusByIdRequest, XElement, XElement> _CustomPullEVSEStatusByIdSOAPRequestMapper = (request, xml) => xml;

        public Func<PullEVSEStatusByIdRequest, XElement, XElement> CustomPullEVSEStatusByIdSOAPRequestMapper
        {

            get
            {
                return _CustomPullEVSEStatusByIdSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomPullEVSEStatusByIdSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<Acknowledgement<PullEVSEStatusByIdRequest>, Acknowledgement<PullEVSEStatusByIdRequest>.Builder> CustomPullEVSEStatusByIdResponseMapper  { get; set; }

        #endregion


        #region CustomPushAuthenticationData(SOAP)RequestMapper

        #region CustomPushAuthenticationDataRequestMapper

        private Func<PushAuthenticationDataRequest, PushAuthenticationDataRequest> _CustomPushAuthenticationDataRequestMapper = _ => _;

        public Func<PushAuthenticationDataRequest, PushAuthenticationDataRequest> CustomPushAuthenticationDataRequestMapper
        {

            get
            {
                return _CustomPushAuthenticationDataRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomPushAuthenticationDataRequestMapper = value;
            }

        }

        #endregion

        #region CustomPushAuthenticationDataSOAPRequestMapper

        private Func<PushAuthenticationDataRequest, XElement, XElement> _CustomPushAuthenticationDataSOAPRequestMapper = (request, xml) => xml;

        public Func<PushAuthenticationDataRequest, XElement, XElement> CustomPushAuthenticationDataSOAPRequestMapper
        {

            get
            {
                return _CustomPushAuthenticationDataSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomPushAuthenticationDataSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<Acknowledgement<PushAuthenticationDataRequest>, Acknowledgement<PushAuthenticationDataRequest>.Builder> CustomPushAuthenticationDataResponseMapper  { get; set; }

        #endregion


        #region CustomAuthorizeRemoteReservationStart(SOAP)RequestMapper

        #region CustomAuthorizeRemoteReservationStartRequestMapper

        private Func<AuthorizeRemoteReservationStartRequest, AuthorizeRemoteReservationStartRequest> _CustomAuthorizeRemoteReservationStartRequestMapper = _ => _;

        public Func<AuthorizeRemoteReservationStartRequest, AuthorizeRemoteReservationStartRequest> CustomAuthorizeRemoteReservationStartRequestMapper
        {

            get
            {
                return _CustomAuthorizeRemoteReservationStartRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomAuthorizeRemoteReservationStartRequestMapper = value;
            }

        }

        #endregion

        #region CustomAuthorizeRemoteReservationStartSOAPRequestMapper

        private Func<AuthorizeRemoteReservationStartRequest, XElement, XElement> _CustomAuthorizeRemoteReservationStartSOAPRequestMapper = (request, xml) => xml;

        public Func<AuthorizeRemoteReservationStartRequest, XElement, XElement> CustomAuthorizeRemoteReservationStartSOAPRequestMapper
        {

            get
            {
                return _CustomAuthorizeRemoteReservationStartSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomAuthorizeRemoteReservationStartSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<Acknowledgement<AuthorizeRemoteReservationStartRequest>, Acknowledgement<AuthorizeRemoteReservationStartRequest>.Builder> CustomAuthorizeRemoteReservationStartResponseMapper  { get; set; }

        #endregion

        #region CustomAuthorizeRemoteReservationStop(SOAP)RequestMapper

        #region CustomAuthorizeRemoteReservationStopRequestMapper

        private Func<AuthorizeRemoteReservationStopRequest, AuthorizeRemoteReservationStopRequest> _CustomAuthorizeRemoteReservationStopRequestMapper = _ => _;

        public Func<AuthorizeRemoteReservationStopRequest, AuthorizeRemoteReservationStopRequest> CustomAuthorizeRemoteReservationStopRequestMapper
        {

            get
            {
                return _CustomAuthorizeRemoteReservationStopRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomAuthorizeRemoteReservationStopRequestMapper = value;
            }

        }

        #endregion

        #region CustomAuthorizeRemoteReservationStopSOAPRequestMapper

        private Func<AuthorizeRemoteReservationStopRequest, XElement, XElement> _CustomAuthorizeRemoteReservationStopSOAPRequestMapper = (request, xml) => xml;

        public Func<AuthorizeRemoteReservationStopRequest, XElement, XElement> CustomAuthorizeRemoteReservationStopSOAPRequestMapper
        {

            get
            {
                return _CustomAuthorizeRemoteReservationStopSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomAuthorizeRemoteReservationStopSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<Acknowledgement<AuthorizeRemoteReservationStopRequest>, Acknowledgement<AuthorizeRemoteReservationStopRequest>.Builder> CustomAuthorizeRemoteReservationStopResponseMapper  { get; set; }

        #endregion


        #region CustomAuthorizeRemoteStart(SOAP)RequestMapper

        #region CustomAuthorizeRemoteStartRequestMapper

        private Func<AuthorizeRemoteStartRequest, AuthorizeRemoteStartRequest> _CustomAuthorizeRemoteStartRequestMapper = request => request;

        public Func<AuthorizeRemoteStartRequest, AuthorizeRemoteStartRequest> CustomAuthorizeRemoteStartRequestMapper
        {

            get
            {
                return _CustomAuthorizeRemoteStartRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomAuthorizeRemoteStartRequestMapper = value;
            }

        }

        #endregion

        #region CustomAuthorizeRemoteStartSOAPRequestMapper

        private Func<AuthorizeRemoteStartRequest, XElement, XElement> _CustomAuthorizeRemoteStartSOAPRequestMapper = (request, xml) => xml;

        public Func<AuthorizeRemoteStartRequest, XElement, XElement> CustomAuthorizeRemoteStartSOAPRequestMapper
        {

            get
            {
                return _CustomAuthorizeRemoteStartSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomAuthorizeRemoteStartSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<Acknowledgement<AuthorizeRemoteStartRequest>, Acknowledgement<AuthorizeRemoteStartRequest>.Builder> CustomAuthorizeRemoteStartResponseMapper  { get; set; }

        #endregion

        #region CustomAuthorizeRemoteStop(SOAP)Mappers

        #region CustomAuthorizeRemoteStopRequestMapper

        private Func<AuthorizeRemoteStopRequest, AuthorizeRemoteStopRequest> _CustomAuthorizeRemoteStopRequestMapper = request => request;

        public Func<AuthorizeRemoteStopRequest, AuthorizeRemoteStopRequest> CustomAuthorizeRemoteStopRequestMapper
        {

            get
            {
                return _CustomAuthorizeRemoteStopRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomAuthorizeRemoteStopRequestMapper = value;
            }

        }

        #endregion

        #region CustomAuthorizeRemoteStopSOAPRequestMapper

        private Func<AuthorizeRemoteStopRequest, XElement, XElement> _CustomAuthorizeRemoteStopSOAPRequestMapper = (request, xml) => xml;

        public Func<AuthorizeRemoteStopRequest, XElement, XElement> CustomAuthorizeRemoteStopSOAPRequestMapper
        {

            get
            {
                return _CustomAuthorizeRemoteStopSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomAuthorizeRemoteStopSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<Acknowledgement<AuthorizeRemoteStopRequest>, Acknowledgement<AuthorizeRemoteStopRequest>.Builder> CustomAuthorizeRemoteStopResponseMapper  { get; set; }

        #endregion


        #region CustomGetChargeDetailRecords(SOAP)RequestMapper

        #region CustomGetChargeDetailRecordsRequestMapper

        private Func<GetChargeDetailRecordsRequest, GetChargeDetailRecordsRequest> _CustomGetChargeDetailRecordsRequestMapper = _ => _;

        public Func<GetChargeDetailRecordsRequest, GetChargeDetailRecordsRequest> CustomGetChargeDetailRecordsRequestMapper
        {

            get
            {
                return _CustomGetChargeDetailRecordsRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomGetChargeDetailRecordsRequestMapper = value;
            }

        }

        #endregion

        #region CustomGetChargeDetailRecordsSOAPRequestMapper

        private Func<XElement, XElement> _CustomGetChargeDetailRecordsSOAPRequestMapper = _ => _;

        public Func<XElement, XElement> CustomGetChargeDetailRecordsSOAPRequestMapper
        {

            get
            {
                return _CustomGetChargeDetailRecordsSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomGetChargeDetailRecordsSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<GetChargeDetailRecordsResponse, GetChargeDetailRecordsResponse.Builder> CustomGetChargeDetailRecordsResponseMapper  { get; set; }

        public CustomMapperDelegate<ChargeDetailRecord>                                                     CustomChargeDetailRecordXMLMapper           { get; set; }

        #endregion

        #endregion

        #region Events

        #region OnPullEVSEDataRequest/-Response

        /// <summary>
        /// An event fired whenever a 'pull EVSE data' request will be send.
        /// </summary>
        public event OnPullEVSEDataRequestHandler   OnPullEVSEDataRequest;

        /// <summary>
        /// An event fired whenever a 'pull EVSE data' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler        OnPullEVSEDataSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE data' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler       OnPullEVSEDataSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE data' request had been received.
        /// </summary>
        public event OnPullEVSEDataResponseHandler  OnPullEVSEDataResponse;

        #endregion

        #region OnSearchEVSERequest/-Response

        /// <summary>
        /// An event fired whenever a 'search EVSE' request will be send.
        /// </summary>
        public event OnSearchEVSERequestHandler   OnSearchEVSERequest;

        /// <summary>
        /// An event fired whenever a 'search EVSE' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler      OnSearchEVSESOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'search EVSE' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler     OnSearchEVSESOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'search EVSE' request had been received.
        /// </summary>
        public event OnSearchEVSEResponseHandler  OnSearchEVSEResponse;

        #endregion

        #region OnPullEVSEStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a 'pull EVSE status' request will be send.
        /// </summary>
        public event OnPullEVSEStatusRequestHandler   OnPullEVSEStatusRequest;

        /// <summary>
        /// An event fired whenever a 'pull EVSE status' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler          OnPullEVSEStatusSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE status' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler         OnPullEVSEStatusSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE status' request had been received.
        /// </summary>
        public event OnPullEVSEStatusResponseHandler  OnPullEVSEStatusResponse;

        #endregion

        #region OnPullEVSEStatusByIdRequest/-Response

        /// <summary>
        /// An event fired whenever a 'pull EVSE status by id' request will be send.
        /// </summary>
        public event OnPullEVSEStatusByIdRequestHandler   OnPullEVSEStatusByIdRequest;

        /// <summary>
        /// An event fired whenever a 'pull EVSE status by id' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler              OnPullEVSEStatusByIdSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE status by id' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler             OnPullEVSEStatusByIdSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'pull EVSE status by id' request had been received.
        /// </summary>
        public event OnPullEVSEStatusByIdResponseHandler  OnPullEVSEStatusByIdResponse;

        #endregion

        #region OnPushAuthenticationDataRequest/-Response

        /// <summary>
        /// An event fired whenever a 'push authentication data' request will be send.
        /// </summary>
        public event OnPushAuthenticationDataRequestHandler   OnPushAuthenticationDataRequest;

        /// <summary>
        /// An event fired whenever a 'push authentication data' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                  OnPushAuthenticationDataSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'push authentication data' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                 OnPushAuthenticationDataSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'push authentication data' request had been received.
        /// </summary>
        public event OnPushAuthenticationDataResponseHandler  OnPushAuthenticationDataResponse;

        #endregion

        #region OnReservationStartRequest/-Response

        /// <summary>
        /// An event fired whenever a 'reservation start' request will be send.
        /// </summary>
        public event OnReservationStartRequestHandler   OnReservationStartRequest;

        /// <summary>
        /// An event fired whenever a 'reservation start' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler            OnReservationStartSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'reservation start' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler           OnReservationStartSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'reservation start' request had been received.
        /// </summary>
        public event OnReservationStartResponseHandler  OnReservationStartResponse;

        #endregion

        #region OnReservationStopRequest/-Response

        /// <summary>
        /// An event fired whenever a 'reservation stop' request will be send.
        /// </summary>
        public event OnReservationStopRequestHandler   OnReservationStopRequest;

        /// <summary>
        /// An event fired whenever a 'reservation stop' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler           OnReservationStopSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'reservation stop' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler          OnReservationStopSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'reservation stop' request had been received.
        /// </summary>
        public event OnReservationStopResponseHandler  OnReservationStopResponse;

        #endregion

        #region OnAuthorizeRemoteStartRequest/-Response

        /// <summary>
        /// An event fired whenever an 'authorize remote start' request will be send.
        /// </summary>
        public event OnAuthorizeRemoteStartRequestHandler   OnAuthorizeRemoteStartRequest;

        /// <summary>
        /// An event fired whenever an 'authorize remote start' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                OnAuthorizeRemoteStartSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to an 'authorize remote start' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler               OnAuthorizeRemoteStartSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to an 'authorize remote start' request had been received.
        /// </summary>
        public event OnAuthorizeRemoteStartResponseHandler  OnAuthorizeRemoteStartResponse;

        #endregion

        #region OnAuthorizeRemoteStopRequest/-Response

        /// <summary>
        /// An event fired whenever an 'authorize remote stop' request will be send.
        /// </summary>
        public event OnAuthorizeRemoteStopRequestHandler   OnAuthorizeRemoteStopRequest;

        /// <summary>
        /// An event fired whenever an 'authorize remote stop' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler               OnAuthorizeRemoteStopSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to an 'authorize remote stop' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler              OnAuthorizeRemoteStopSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to an 'authorize remote stop' request had been received.
        /// </summary>
        public event OnAuthorizeRemoteStopResponseHandler  OnAuthorizeRemoteStopResponse;

        #endregion

        #region OnGetChargeDetailRecordsRequest/-Response

        /// <summary>
        /// An event fired whenever a 'get charge detail records' request will be send.
        /// </summary>
        public event OnGetChargeDetailRecordsRequestHandler   OnGetChargeDetailRecordsRequest;

        /// <summary>
        /// An event fired whenever a 'get charge detail records' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                  OnGetChargeDetailRecordsSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'get charge detail records' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                 OnGetChargeDetailRecordsSOAPResponse;

        /// <summary>
        /// An event fired whenever a response for a 'get charge detail records' request was received.
        /// </summary>
        public event OnGetChargeDetailRecordsResponseHandler  OnGetChargeDetailRecordsResponse;

        #endregion

        #endregion

        #region Constructor(s)

        #region EMPClient(ClientId, Hostname, ..., LoggingContext = EMPClientLogger.DefaultContext, ...)

        /// <summary>
        /// Create a new OICP EMP client.
        /// </summary>
        /// <param name="ClientId">A unqiue identification of this client.</param>
        /// <param name="Hostname">The OICP hostname to connect to.</param>
        /// <param name="RemotePort">An optional OICP TCP port to connect to.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCert">The TLS client certificate to use.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual host name to use.</param>
        /// <param name="URIPrefix">An default URI prefix.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent to use.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// <param name="DNSClient">An optional DNS client.</param>
        /// <param name="LoggingContext">An optional context for logging client methods.</param>
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
        public EMPClient(String                               ClientId,
                         String                               Hostname,
                         IPPort                               RemotePort                  = null,
                         RemoteCertificateValidationCallback  RemoteCertificateValidator  = null,
                         X509Certificate                      ClientCert                  = null,
                         String                               HTTPVirtualHost             = null,
                         String                               URIPrefix                   = DefaultURIPrefix,
                         String                               HTTPUserAgent               = DefaultHTTPUserAgent,
                         TimeSpan?                            RequestTimeout              = null,
                         DNSClient                            DNSClient                   = null,
                         String                               LoggingContext              = EMPClientLogger.DefaultContext,
                         Func<String, String, String>         LogFileCreator              = null)

            : base(ClientId,
                   Hostname,
                   RemotePort ?? DefaultRemotePort,
                   RemoteCertificateValidator,
                   ClientCert,
                   HTTPVirtualHost,
                   URIPrefix.Trim().IsNotNullOrEmpty() ? URIPrefix : DefaultURIPrefix,
                   null,
                   HTTPUserAgent,
                   RequestTimeout,
                   DNSClient)

        {

            #region Initial checks

            if (ClientId.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Logger),    "The given client identification must not be null or empty!");

            if (Hostname.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Hostname),  "The given hostname must not be null or empty!");

            #endregion

            this.Logger = new EMPClientLogger(this,
                                              LoggingContext,
                                              LogFileCreator);

        }

        #endregion

        #region EMPClient(ClientId, Logger, Hostname, ...)

        /// <summary>
        /// Create a new OICP EMP client.
        /// </summary>
        /// <param name="ClientId">A unqiue identification of this client.</param>
        /// <param name="Hostname">The OICP hostname to connect to.</param>
        /// <param name="RemotePort">An optional OICP TCP port to connect to.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCert">The TLS client certificate to use.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual host name to use.</param>
        /// <param name="URIPrefix">An default URI prefix.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent to use.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// <param name="DNSClient">An optional DNS client.</param>
        public EMPClient(String                               ClientId,
                         EMPClientLogger                      Logger,
                         String                               Hostname,
                         IPPort                               RemotePort                  = null,
                         RemoteCertificateValidationCallback  RemoteCertificateValidator  = null,
                         X509Certificate                      ClientCert                  = null,
                         String                               HTTPVirtualHost             = null,
                         String                               URIPrefix                   = DefaultURIPrefix,
                         String                               HTTPUserAgent               = DefaultHTTPUserAgent,
                         TimeSpan?                            RequestTimeout              = null,
                         DNSClient                            DNSClient                   = null)

            : base(ClientId,
                   Hostname,
                   RemotePort ?? DefaultRemotePort,
                   RemoteCertificateValidator,
                   ClientCert,
                   HTTPVirtualHost,
                   URIPrefix.Trim().IsNotNullOrEmpty() ? URIPrefix : DefaultURIPrefix,
                   null,
                   HTTPUserAgent,
                   RequestTimeout,
                   DNSClient)

        {

            #region Initial checks

            if (ClientId.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Logger),    "The given client identification must not be null or empty!");

            if (Logger == null)
                throw new ArgumentNullException(nameof(Logger),    "The given mobile client logger must not be null!");

            if (Hostname.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Hostname),  "The given hostname must not be null or empty!");

            #endregion

            this.Logger = Logger;

        }

        #endregion

        #endregion


        #region PullEVSEData(Request)

        /// <summary>
        /// Create a new task querying EVSE data from the OICP server.
        /// The request might either have none, 'SearchCenter + DistanceKM' or 'LastCall' parameters.
        /// Because of limitations at Hubject the SearchCenter and LastCall parameters can not be used at the same time!
        /// </summary>
        /// <param name="Request">A PullEVSEData request.</param>
        public async Task<HTTPResponse<EVSEData>>

            PullEVSEData(PullEVSEDataRequest  Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The given PullEVSEData request must not be null!");

            Request = _CustomPullEVSEDataRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The mapped PullEVSEData request must not be null!");


            HTTPResponse<EVSEData> result = null;

            #endregion

            #region Send OnPullEVSEDataRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnPullEVSEDataRequest?.Invoke(StartTime,
                                              Request.Timestamp.Value,
                                              this,
                                              ClientId,
                                              Request.EventTrackingId,
                                              Request.ProviderId,
                                              Request.SearchCenter,
                                              Request.DistanceKM,
                                              Request.LastCall,
                                              Request.GeoCoordinatesResponseFormat,
                                              Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnPullEVSEDataRequest));
            }

            #endregion


            using (var _OICPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    URIPrefix + "/eRoamingEvseData_V2.1",
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                result = await _OICPClient.Query(_CustomPullEVSEDataSOAPRequestMapper(Request,
                                                                                      SOAP.Encapsulation(Request.ToXML())),
                                                 "eRoamingPullEVSEData",
                                                 RequestLogDelegate:   OnPullEVSEDataSOAPRequest,
                                                 ResponseLogDelegate:  OnPullEVSEDataSOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 QueryTimeout:         Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(EVSEData.Parse, SendException),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     DebugX.Log("'PullEVSEDataRequest' lead to a SOAP fault!");

                                                     return new HTTPResponse<EVSEData>(httpresponse,
                                                                                               IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnHTTPError

                                                 OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                     SendHTTPError(timestamp, soapclient, httpresponse);

                                                     return new HTTPResponse<EVSEData>(httpresponse,
                                                                                               new EVSEData(StatusCodes.DataError,
                                                                                                                    Description:    httpresponse.HTTPStatusCode.ToString(),
                                                                                                                    AdditionalInfo: httpresponse.HTTPBody.      ToUTF8String()),
                                                                                               IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnException

                                                 OnException: (timestamp, sender, exception) => {

                                                     SendException(timestamp, sender, exception);

                                                     return HTTPResponse<EVSEData>.ExceptionThrown(new EVSEData(StatusCodes.ServiceNotAvailable,
                                                                                                                                exception.Message,
                                                                                                                                exception.StackTrace),
                                                                                                           Exception: exception);

                                                 }

                                                 #endregion

                                                ).ConfigureAwait(false);

            }

            if (result == null)
                result = HTTPResponse<EVSEData>.ClientError(
                             new EVSEData(
                                 StatusCodes.SystemError,
                                 "HTTP request failed!"
                             )
                         );


            #region Send OnPullEVSEDataResponse event

            var Endtime = DateTime.Now;

            try
            {

                OnPullEVSEDataResponse?.Invoke(Endtime,
                                               this,
                                               ClientId,
                                               Request.EventTrackingId,
                                               Request.ProviderId,
                                               Request.SearchCenter,
                                               Request.DistanceKM,
                                               Request.LastCall,
                                               Request.GeoCoordinatesResponseFormat,
                                               Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout,
                                               result.Content,
                                               Endtime - StartTime);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnPullEVSEDataResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region SearchEVSE  (Request)

        /// <summary>
        /// Create a new Search EVSE request.
        /// </summary>
        /// <param name="Request">A SearchEVSE request.</param>
        public async Task<HTTPResponse<EVSESearchResult>>

            SearchEVSE(SearchEVSERequest  Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The given SearchEVSE request must not be null!");

            Request = _CustomSearchEVSERequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The mapped SearchEVSE request must not be null!");


            HTTPResponse<EVSESearchResult> result = null;

            #endregion

            #region Send OnSearchEVSERequest event

            var StartTime = DateTime.Now;

            try
            {

                OnSearchEVSERequest?.Invoke(StartTime,
                                            Request.Timestamp.Value,
                                            this,
                                            ClientId,
                                            Request.EventTrackingId,
                                            Request.ProviderId,
                                            Request.SearchCenter,
                                            Request.DistanceKM,
                                            Request.Address,
                                            Request.Plug,
                                            Request.ChargingFacility,
                                            Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnSearchEVSERequest));
            }

            #endregion


            using (var _OICPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    URIPrefix + "/eRoamingEvseSearch_V2.0",
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                result = await _OICPClient.Query(_CustomSearchEVSESOAPRequestMapper(Request,
                                                                                    SOAP.Encapsulation(Request.ToXML())),
                                                 "eRoamingSearchEvse",
                                                 RequestLogDelegate:   OnSearchEVSESOAPRequest,
                                                 ResponseLogDelegate:  OnSearchEVSESOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 QueryTimeout:         Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => {

                                                     OICPException _OICPException = null;
                                                     if (OICPClientHelper.IsHubjectError(XMLResponse.Content, out _OICPException, SendException))
                                                         return new HTTPResponse<EVSESearchResult>(XMLResponse.HTTPRequest, _OICPException);

                                                     return XMLResponse.ConvertContent(EVSESearchResult.Parse);

                                                 },

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     DebugX.Log("'PullEVSEStatusByIdRequest' lead to a SOAP fault!");

                                                     return new HTTPResponse<EVSESearchResult>(httpresponse,
                                                                                                       IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnHTTPError

                                                 OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                     SendHTTPError(timestamp, soapclient, httpresponse);

                                                     return new HTTPResponse<EVSESearchResult>(httpresponse,
                                                                                                       IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnException

                                                 OnException: (timestamp, sender, exception) => {

                                                     SendException(timestamp, sender, exception);

                                                     return HTTPResponse<EVSESearchResult>.ExceptionThrown(new EVSESearchResult(new EVSEMatch[0]),
                                                                                                                   Exception: exception);

                                                 }

                                                 #endregion

                                                ).ConfigureAwait(false);

            }

            if (result == null)
                result = HTTPResponse<EVSESearchResult>.ClientError(
                             new EVSESearchResult(
                                 StatusCodes.SystemError,
                                 "HTTP request failed!"
                             )
                         );


            #region Send OnSearchEVSEResponse event

            var Endtime = DateTime.Now;

            try
            {

                OnSearchEVSEResponse?.Invoke(Endtime,
                                             this,
                                             ClientId,
                                             Request.EventTrackingId,
                                             Request.ProviderId,
                                             Request.SearchCenter,
                                             Request.DistanceKM,
                                             Request.Address,
                                             Request.Plug,
                                             Request.ChargingFacility,
                                             Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout,
                                             result.Content,
                                             Endtime - StartTime);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnSearchEVSEResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region PullEVSEStatus    (Request)

        /// <summary>
        /// Create a new task requesting the current status of all EVSEs (within an optional search radius and status).
        /// </summary>
        /// <param name="Request">A PullEVSEStatus request.</param>
        public async Task<HTTPResponse<EVSEStatus>>

            PullEVSEStatus(PullEVSEStatusRequest  Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The given PullEVSEData request must not be null!");

            Request = _CustomPullEVSEStatusRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The mapped PullEVSEData request must not be null!");


            HTTPResponse<EVSEStatus> result = null;

            #endregion

            #region Send OnPullEVSEStatusRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnPullEVSEStatusRequest?.Invoke(StartTime,
                                                Request.Timestamp.Value,
                                                this,
                                                ClientId,
                                                Request.EventTrackingId,
                                                Request.ProviderId,
                                                Request.SearchCenter,
                                                Request.DistanceKM,
                                                Request.EVSEStatusFilter,
                                                Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnPullEVSEStatusRequest));
            }

            #endregion


            using (var _OICPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    URIPrefix + "/eRoamingEvseStatus_V2.0",
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                result = await _OICPClient.Query(_CustomPullEVSEStatusSOAPRequestMapper(Request,
                                                                                        SOAP.Encapsulation(Request.ToXML())),
                                                 "eRoamingPullEVSEStatus",
                                                 RequestLogDelegate:   OnPullEVSEDataSOAPRequest,
                                                 ResponseLogDelegate:  OnPullEVSEDataSOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 QueryTimeout:         Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(EVSEStatus.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     DebugX.Log("'PullEVSEStatusByIdRequest' lead to a SOAP fault!");

                                                     return new HTTPResponse<EVSEStatus>(httpresponse,
                                                                                                 IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnHTTPError

                                                 OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                     SendHTTPError(timestamp, soapclient, httpresponse);

                                                     return new HTTPResponse<EVSEStatus>(httpresponse,
                                                                                                 new EVSEStatus(StatusCodes.DataError,
                                                                                                                        Description:    httpresponse.HTTPStatusCode.ToString(),
                                                                                                                        AdditionalInfo: httpresponse.HTTPBody.      ToUTF8String()),
                                                                                                 IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnException

                                                 OnException: (timestamp, sender, exception) => {

                                                     SendException(timestamp, sender, exception);

                                                     return HTTPResponse<EVSEStatus>.ExceptionThrown(new EVSEStatus(StatusCodes.ServiceNotAvailable,
                                                                                                                                    exception.Message,
                                                                                                                                    exception.StackTrace),
                                                                                                             Exception: exception);

                                                 }

                                                 #endregion

                                                ).ConfigureAwait(false);

            }

            if (result == null)
                result = HTTPResponse<EVSEStatus>.ClientError(
                             new EVSEStatus(
                                 StatusCodes.SystemError,
                                 "HTTP request failed!"
                             )
                         );


            #region Send OnPullEVSEStatusResponse event

            var Endtime = DateTime.Now;

            try
            {

                OnPullEVSEStatusResponse?.Invoke(Endtime,
                                                 this,
                                                 ClientId,
                                                 Request.EventTrackingId,
                                                 Request.ProviderId,
                                                 Request.SearchCenter,
                                                 Request.DistanceKM,
                                                 Request.EVSEStatusFilter,
                                                 Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout,
                                                 result.Content,
                                                 Endtime - StartTime);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnPullEVSEStatusResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region PullEVSEStatusById(Request)

        /// <summary>
        /// Create a new task requesting the current status of up to 100 EVSEs by their EVSE Ids.
        /// </summary>
        /// <param name="Request">A PullEVSEStatusById request.</param>
        public async Task<HTTPResponse<EVSEStatusById>>

            PullEVSEStatusById(PullEVSEStatusByIdRequest  Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The given PullEVSEStatusById request must not be null!");

            Request = _CustomPullEVSEStatusByIdRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The mapped PullEVSEStatusById request must not be null!");


            HTTPResponse<EVSEStatusById> result = null;

            #endregion

            #region Send OnPullEVSEStatusByIdRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnPullEVSEStatusByIdRequest?.Invoke(StartTime,
                                                    Request.Timestamp.Value,
                                                    this,
                                                    ClientId,
                                                    Request.EventTrackingId,
                                                    Request.ProviderId,
                                                    Request.EVSEIds,
                                                    Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnPullEVSEStatusByIdRequest));
            }

            #endregion


            using (var _OICPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    URIPrefix + "/eRoamingEvseStatus_V2.0",
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                result = await _OICPClient.Query(_CustomPullEVSEStatusByIdSOAPRequestMapper(Request,
                                                                                            SOAP.Encapsulation(Request.ToXML())),
                                                 "eRoamingPullEvseStatusById",
                                                 RequestLogDelegate:   OnPullEVSEStatusByIdSOAPRequest,
                                                 ResponseLogDelegate:  OnPullEVSEStatusByIdSOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 QueryTimeout:         Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(EVSEStatusById.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     DebugX.Log("'PullEVSEStatusByIdRequest' lead to a SOAP fault!");

                                                     return new HTTPResponse<EVSEStatusById>(httpresponse,
                                                                                                     IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnHTTPError

                                                 OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                     SendHTTPError(timestamp, soapclient, httpresponse);

                                                     return new HTTPResponse<EVSEStatusById>(httpresponse,
                                                                                                     new EVSEStatusById(StatusCodes.DataError,
                                                                                                                                Description:    httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                AdditionalInfo: httpresponse.HTTPBody.      ToUTF8String()),
                                                                                                     IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnException

                                                 OnException: (timestamp, sender, exception) => {

                                                     SendException(timestamp, sender, exception);

                                                     return HTTPResponse<EVSEStatusById>.ExceptionThrown(new EVSEStatusById(StatusCodes.ServiceNotAvailable,
                                                                                                                                            exception.Message,
                                                                                                                                            exception.StackTrace),
                                                                                                                 Exception: exception);

                                                 }

                                                 #endregion

                                                ).ConfigureAwait(false);

            }

            if (result == null)
                result = HTTPResponse<EVSEStatusById>.ClientError(
                             new EVSEStatusById(
                                 StatusCodes.SystemError,
                                 "HTTP request failed!"
                             )
                         );


            #region Send OnPullEVSEStatusByIdResponse event

            var Endtime = DateTime.Now;

            try
            {

                OnPullEVSEStatusByIdResponse?.Invoke(Endtime,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.ProviderId,
                                                     Request.EVSEIds,
                                                     Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout,
                                                     result.Content,
                                                     Endtime - StartTime);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnPullEVSEStatusByIdResponse));
            }

            #endregion

            return result;


        }

        #endregion


        #region PushAuthenticationData(Request)

        /// <summary>
        /// Create a new task pushing provider authentication data records onto the OICP server.
        /// </summary>
        /// <param name="Request">A PushAuthenticationData request.</param>
        public async Task<HTTPResponse<Acknowledgement<PushAuthenticationDataRequest>>>

            PushAuthenticationData(PushAuthenticationDataRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given PushAuthenticationData request must not be null!");

            Request = _CustomPushAuthenticationDataRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped PushAuthenticationData request must not be null!");


            HTTPResponse<Acknowledgement<PushAuthenticationDataRequest>> result = null;

            #endregion

            #region Send OnPushAuthenticationDataRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnPushAuthenticationDataRequest?.Invoke(StartTime,
                                                        Request.Timestamp.Value,
                                                        this,
                                                        ClientId,
                                                        Request.EventTrackingId,
                                                        Request.AuthorizationIdentifications,
                                                        Request.ProviderId,
                                                        Request.OICPAction,
                                                        RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnPushAuthenticationDataRequest));
            }

            #endregion


            using (var _OICPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    URIPrefix + "/eRoamingAuthenticationData_V2.0",
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                result = await _OICPClient.Query(_CustomPushAuthenticationDataSOAPRequestMapper(Request, SOAP.Encapsulation(Request.ToXML())),
                                                 "eRoamingPushAuthenticationData",
                                                 RequestLogDelegate:   OnPushAuthenticationDataSOAPRequest,
                                                 ResponseLogDelegate:  OnPushAuthenticationDataSOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 QueryTimeout:         Request.RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                      (request, xml, onexception) =>
                                                                                                          Acknowledgement<PushAuthenticationDataRequest>.Parse(request,
                                                                                                                                                               xml,
                                                                                                                                                               CustomPushAuthenticationDataResponseMapper,
                                                                                                                                                               onexception)),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                     return new HTTPResponse<Acknowledgement<PushAuthenticationDataRequest>>(

                                                                httpresponse,

                                                                new Acknowledgement<PushAuthenticationDataRequest>(
                                                                    Request,
                                                                    StatusCodes.DataError,
                                                                    httpresponse.Content.ToString()
                                                                ),

                                                                IsFault: true

                                                            );

                                                 },

                                                 #endregion

                                                 #region OnHTTPError

                                                 OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                     SendHTTPError(timestamp, soapclient, httpresponse);

                                                     return new HTTPResponse<Acknowledgement<PushAuthenticationDataRequest>>(

                                                                httpresponse,

                                                                new Acknowledgement<PushAuthenticationDataRequest>(
                                                                    Request,
                                                                    StatusCodes.DataError,
                                                                    httpresponse.HTTPStatusCode.ToString(),
                                                                    httpresponse.HTTPBody.      ToUTF8String()
                                                                ),

                                                                IsFault: true

                                                            );

                                                 },

                                                 #endregion

                                                 #region OnHTTPError

                                                 OnException: (timestamp, sender, exception) => {

                                                     SendException(timestamp, sender, exception);

                                                     return HTTPResponse<Acknowledgement<PushAuthenticationDataRequest>>.ExceptionThrown(

                                                            new Acknowledgement<PushAuthenticationDataRequest>(
                                                                Request,
                                                                StatusCodes.ServiceNotAvailable,
                                                                exception.Message,
                                                                exception.StackTrace
                                                            ),

                                                            Exception: exception

                                                        );

                                                 }

                                                 #endregion

                                                ).ConfigureAwait(false);

            }

            if (result == null)
                result = HTTPResponse<Acknowledgement<PushAuthenticationDataRequest>>.OK(
                             new Acknowledgement<PushAuthenticationDataRequest>(
                                 Request,
                                 StatusCodes.SystemError,
                                 "HTTP request failed!"
                             )
                         );


            #region Send OnPushAuthenticationDataResponse event

            var Endtime = DateTime.Now;

            try
            {

                OnPushAuthenticationDataResponse?.Invoke(Endtime,
                                                         this,
                                                         ClientId,
                                                         Request.EventTrackingId,
                                                         Request.AuthorizationIdentifications,
                                                         Request.ProviderId,
                                                         Request.OICPAction,
                                                         Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout,
                                                         result.Content,
                                                         Endtime - StartTime);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnPushAuthenticationDataResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region ReservationStart(Request)

        /// <summary>
        /// Create a reservation at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteReservationStart request.</param>
        public async Task<HTTPResponse<Acknowledgement<AuthorizeRemoteReservationStartRequest>>>

            ReservationStart(AuthorizeRemoteReservationStartRequest  Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given AuthorizeRemoteReservationStart request must not be null!");

            Request = _CustomAuthorizeRemoteReservationStartRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped AuthorizeRemoteReservationStart request must not be null!");


            HTTPResponse<Acknowledgement<AuthorizeRemoteReservationStartRequest>> result = null;

            #endregion

            #region Send OnReservationStartRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnReservationStartRequest?.Invoke(StartTime,
                                                  Request.Timestamp.Value,
                                                  this,
                                                  ClientId,
                                                  Request.EventTrackingId,
                                                  Request.ProviderId,
                                                  Request.EVSEId,
                                                  Request.EVCOId,
                                                  Request.SessionId,
                                                  Request.PartnerSessionId,
                                                  Request.PartnerProductId,
                                                  RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnReservationStartRequest));
            }

            #endregion


            using (var _OICPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    URIPrefix + "/eRoamingReservation_V1.0",
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                result = await _OICPClient.Query(_CustomAuthorizeRemoteReservationStartSOAPRequestMapper(Request, SOAP.Encapsulation(Request.ToXML())),
                                                 "eRoamingAuthorizeRemoteReservationStart",
                                                 RequestLogDelegate:   OnReservationStartSOAPRequest,
                                                 ResponseLogDelegate:  OnReservationStartSOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 QueryTimeout:         Request.RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                      (request, xml, onexception) =>
                                                                                                          Acknowledgement<AuthorizeRemoteReservationStartRequest>.Parse(request,
                                                                                                                                                                        xml,
                                                                                                                                                                        CustomAuthorizeRemoteReservationStartResponseMapper,
                                                                                                                                                                        onexception)),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                     return new HTTPResponse<Acknowledgement<AuthorizeRemoteReservationStartRequest>>(

                                                                httpresponse,

                                                                new Acknowledgement<AuthorizeRemoteReservationStartRequest>(
                                                                    Request,
                                                                    StatusCodes.DataError,
                                                                    httpresponse.Content.ToString()
                                                                ),

                                                                IsFault: true

                                                            );

                                                 },

                                                 #endregion

                                                 #region OnHTTPError

                                                 OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                     SendHTTPError(timestamp, soapclient, httpresponse);

                                                     return new HTTPResponse<Acknowledgement<AuthorizeRemoteReservationStartRequest>>(

                                                                httpresponse,

                                                                new Acknowledgement<AuthorizeRemoteReservationStartRequest>(
                                                                    Request,
                                                                    StatusCodes.DataError,
                                                                    httpresponse.HTTPStatusCode.ToString(),
                                                                    httpresponse.HTTPBody.      ToUTF8String()
                                                                ),

                                                                IsFault: true

                                                            );

                                                 },

                                                 #endregion

                                                 #region OnException

                                                 OnException: (timestamp, sender, exception) => {

                                                     SendException(timestamp, sender, exception);

                                                     return HTTPResponse<Acknowledgement<AuthorizeRemoteReservationStartRequest>>.ExceptionThrown(

                                                                new Acknowledgement<AuthorizeRemoteReservationStartRequest>(
                                                                    Request,
                                                                    StatusCodes.ServiceNotAvailable,
                                                                    exception.Message,
                                                                    exception.StackTrace,
                                                                    Request.SessionId
                                                                ),

                                                                Exception: exception

                                                            );

                                                 }

                                                 #endregion

                                                ).ConfigureAwait(false);

            }

            if (result == null)
                result = HTTPResponse<Acknowledgement<AuthorizeRemoteReservationStartRequest>>.ClientError(
                             new Acknowledgement<AuthorizeRemoteReservationStartRequest>(
                                 Request,
                                 StatusCodes.SystemError,
                                 "HTTP request failed!"
                             )
                         );


            #region Send OnReservationStartResponse event

            var Endtime = DateTime.Now;

            try
            {

                OnReservationStartResponse?.Invoke(Endtime,
                                                   this,
                                                   ClientId,
                                                   Request.EventTrackingId,
                                                   Request.ProviderId,
                                                   Request.EVSEId,
                                                   Request.EVCOId,
                                                   Request.SessionId,
                                                   Request.PartnerSessionId,
                                                   Request.PartnerProductId,
                                                   Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout,
                                                   result.Content,
                                                   Endtime - StartTime);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnReservationStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region ReservationStop (Request)

        /// <summary>
        /// Delete a reservation at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteReservationStop request.</param>
        public async Task<HTTPResponse<Acknowledgement<AuthorizeRemoteReservationStopRequest>>>

            ReservationStop(AuthorizeRemoteReservationStopRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given AuthorizeRemoteReservationStop request must not be null!");

            Request = _CustomAuthorizeRemoteReservationStopRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped AuthorizeRemoteReservationStop request must not be null!");


            HTTPResponse<Acknowledgement<AuthorizeRemoteReservationStopRequest>> result = null;

            #endregion

            #region Send OnReservationStopRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnReservationStopRequest?.Invoke(StartTime,
                                                 Request.Timestamp.Value,
                                                 this,
                                                 ClientId,
                                                 Request.EventTrackingId,
                                                 Request.SessionId,
                                                 Request.ProviderId,
                                                 Request.EVSEId,
                                                 Request.PartnerSessionId,
                                                 RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnReservationStopRequest));
            }

            #endregion


            using (var _OICPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    URIPrefix + "/eRoamingReservation_V1.0",
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                result = await _OICPClient.Query(_CustomAuthorizeRemoteReservationStopSOAPRequestMapper(Request, SOAP.Encapsulation(Request.ToXML())),
                                                 "eRoamingAuthorizeRemoteReservationStop",
                                                 RequestLogDelegate:   OnReservationStartSOAPRequest,
                                                 ResponseLogDelegate:  OnReservationStopSOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 QueryTimeout:         Request.RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                      (request, xml, onexception) =>
                                                                                                          Acknowledgement<AuthorizeRemoteReservationStopRequest>.Parse(request,
                                                                                                                                                                       xml,
                                                                                                                                                                       CustomAuthorizeRemoteReservationStopResponseMapper,
                                                                                                                                                                       onexception)),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                     return new HTTPResponse<Acknowledgement<AuthorizeRemoteReservationStopRequest>>(

                                                                httpresponse,

                                                                new Acknowledgement<AuthorizeRemoteReservationStopRequest>(
                                                                    Request,
                                                                    StatusCodes.DataError,
                                                                    httpresponse.Content.ToString()
                                                                ),

                                                                IsFault: true

                                                            );

                                                 },

                                                 #endregion

                                                 #region OnHTTPError

                                                 OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                     SendHTTPError(timestamp, soapclient, httpresponse);

                                                     return new HTTPResponse<Acknowledgement<AuthorizeRemoteReservationStopRequest>>(

                                                                httpresponse,

                                                                new Acknowledgement<AuthorizeRemoteReservationStopRequest>(
                                                                    Request,
                                                                    StatusCodes.DataError,
                                                                    httpresponse.HTTPStatusCode.ToString(),
                                                                    httpresponse.HTTPBody.      ToUTF8String()
                                                                ),

                                                                IsFault: true

                                                            );

                                                 },

                                                 #endregion

                                                 #region OnException

                                                 OnException: (timestamp, sender, exception) => {

                                                     SendException(timestamp, sender, exception);

                                                     return HTTPResponse<Acknowledgement<AuthorizeRemoteReservationStopRequest>>.ExceptionThrown(

                                                                new Acknowledgement<AuthorizeRemoteReservationStopRequest>(
                                                                    Request,
                                                                    StatusCodes.ServiceNotAvailable,
                                                                    exception.Message,
                                                                    exception.StackTrace,
                                                                    Request.SessionId
                                                                ),

                                                                Exception: exception

                                                            );

                                                 }

                                                 #endregion

                                                ).ConfigureAwait(false);

            }

            if (result == null)
                result = HTTPResponse<Acknowledgement<AuthorizeRemoteReservationStopRequest>>.ClientError(
                             new Acknowledgement<AuthorizeRemoteReservationStopRequest>(
                                 Request,
                                 StatusCodes.SystemError,
                                 "HTTP request failed!"
                             )
                         );


            #region Send OnReservationStopResponse event

            var EndTime = DateTime.Now;

            try
            {

                OnReservationStopResponse?.Invoke(EndTime,
                                                  this,
                                                  ClientId,
                                                  Request.EventTrackingId,
                                                  Request.SessionId,
                                                  Request.ProviderId,
                                                  Request.EVSEId,
                                                  Request.PartnerSessionId,
                                                  Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout,
                                                  result.Content,
                                                  EndTime - StartTime);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnReservationStartResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region RemoteStart     (Request)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteStart request.</param>
        public async Task<HTTPResponse<Acknowledgement<AuthorizeRemoteStartRequest>>>

            RemoteStart(AuthorizeRemoteStartRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given AuthorizeRemoteStart request must not be null!");

            Request = _CustomAuthorizeRemoteStartRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped AuthorizeRemoteStart request must not be null!");


            HTTPResponse<Acknowledgement<AuthorizeRemoteStartRequest>> result = null;

            #endregion

            #region Send OnAuthorizeRemoteStartRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnAuthorizeRemoteStartRequest?.Invoke(StartTime,
                                                      Request.Timestamp.Value,
                                                      this,
                                                      ClientId,
                                                      Request.EventTrackingId,
                                                      Request.ProviderId,
                                                      Request.EVSEId,
                                                      Request.EVCOId,
                                                      Request.SessionId,
                                                      Request.PartnerSessionId,
                                                      Request.PartnerProductId,
                                                      RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnAuthorizeRemoteStartRequest));
            }

            #endregion


            using (var _OICPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    URIPrefix + "/eRoamingAuthorization_V2.0",
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                result = await _OICPClient.Query(_CustomAuthorizeRemoteStartSOAPRequestMapper(Request, SOAP.Encapsulation(Request.ToXML())),
                                                 "eRoamingAuthorizeRemoteStart",
                                                 RequestLogDelegate:   OnAuthorizeRemoteStartSOAPRequest,
                                                 ResponseLogDelegate:  OnAuthorizeRemoteStartSOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 QueryTimeout:         Request.RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                      (request, xml, onexception) =>
                                                                                                          Acknowledgement<AuthorizeRemoteStartRequest>.Parse(request,
                                                                                                                                                             xml,
                                                                                                                                                             CustomAuthorizeRemoteStartResponseMapper,
                                                                                                                                                             onexception)),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                     return new HTTPResponse<Acknowledgement<AuthorizeRemoteStartRequest>>(

                                                                httpresponse,

                                                                new Acknowledgement<AuthorizeRemoteStartRequest>(
                                                                    Request,
                                                                    StatusCodes.DataError,
                                                                    httpresponse.Content.ToString()
                                                                ),

                                                                IsFault: true
                                                            );

                                                 },

                                                 #endregion

                                                 #region OnHTTPError

                                                 OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                     SendHTTPError(timestamp, soapclient, httpresponse);

                                                     return new HTTPResponse<Acknowledgement<AuthorizeRemoteStartRequest>>(

                                                                httpresponse,

                                                                new Acknowledgement<AuthorizeRemoteStartRequest>(
                                                                    Request,
                                                                    StatusCodes.DataError,
                                                                    httpresponse.HTTPStatusCode.ToString(),
                                                                    httpresponse.HTTPBody.      ToUTF8String()
                                                                ),

                                                                IsFault: true

                                                            );

                                                 },

                                                 #endregion

                                                 #region OnException

                                                 OnException: (timestamp, sender, exception) => {

                                                     SendException(timestamp, sender, exception);

                                                     return HTTPResponse<Acknowledgement<AuthorizeRemoteStartRequest>>.ExceptionThrown(

                                                            new Acknowledgement<AuthorizeRemoteStartRequest>(
                                                                Request,
                                                                StatusCodes.ServiceNotAvailable,
                                                                exception.Message,
                                                                exception.StackTrace,
                                                                Request.SessionId
                                                            ),

                                                            Exception: exception);

                                                 }

                                                 #endregion

                                                ).ConfigureAwait(false);

            }

            if (result == null)
                result = HTTPResponse<Acknowledgement<AuthorizeRemoteStartRequest>>.ClientError(
                             new Acknowledgement<AuthorizeRemoteStartRequest>(
                                 Request,
                                 StatusCodes.SystemError,
                                 "HTTP request failed!"
                             )
                         );


            #region Send OnAuthorizeRemoteStartResponse event

            var EndTime = DateTime.Now;

            try
            {

                OnAuthorizeRemoteStartResponse?.Invoke(EndTime,
                                                       this,
                                                       ClientId,
                                                       Request.EventTrackingId,
                                                       Request.ProviderId,
                                                       Request.EVSEId,
                                                       Request.EVCOId,
                                                       Request.SessionId,
                                                       Request.PartnerSessionId,
                                                       Request.PartnerProductId,
                                                       Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout,
                                                       result.Content,
                                                       EndTime - StartTime);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnAuthorizeRemoteStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region RemoteStop      (Request)

        /// <summary>
        /// Stop the given charging session at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteStop request.</param>
        public async Task<HTTPResponse<Acknowledgement<AuthorizeRemoteStopRequest>>>

            RemoteStop(AuthorizeRemoteStopRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given AuthorizeRemoteStop request must not be null!");

            Request = _CustomAuthorizeRemoteStopRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped AuthorizeRemoteStop request must not be null!");


            HTTPResponse<Acknowledgement<AuthorizeRemoteStopRequest>> result = null;

            #endregion

            #region Send OnAuthorizeRemoteStopRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnAuthorizeRemoteStopRequest?.Invoke(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.SessionId,
                                                     Request.ProviderId,
                                                     Request.EVSEId,
                                                     Request.PartnerSessionId,
                                                     RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnAuthorizeRemoteStopRequest));
            }

            #endregion


            using (var _OICPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    URIPrefix + "/eRoamingAuthorization_V2.0",
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                result = await _OICPClient.Query(_CustomAuthorizeRemoteStopSOAPRequestMapper(Request, SOAP.Encapsulation(Request.ToXML())),
                                                 "eRoamingAuthorizeRemoteStop",
                                                 RequestLogDelegate:   OnAuthorizeRemoteStopSOAPRequest,
                                                 ResponseLogDelegate:  OnAuthorizeRemoteStopSOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 QueryTimeout:         Request.RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                      (request, xml, onexception) =>
                                                                                                      Acknowledgement<AuthorizeRemoteStopRequest>.Parse(request,
                                                                                                                                                        xml,
                                                                                                                                                        CustomAuthorizeRemoteStopResponseMapper,
                                                                                                                                                        onexception)),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                     return new HTTPResponse<Acknowledgement<AuthorizeRemoteStopRequest>>(

                                                                httpresponse,

                                                                new Acknowledgement<AuthorizeRemoteStopRequest>(
                                                                    Request,
                                                                    StatusCodes.DataError,
                                                                    httpresponse.Content.ToString()
                                                                ),

                                                                IsFault: true

                                                            );

                                                 },

                                                 #endregion

                                                 #region OnHTTPError

                                                 OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                     SendHTTPError(timestamp, soapclient, httpresponse);

                                                     return new HTTPResponse<Acknowledgement<AuthorizeRemoteStopRequest>>(

                                                                httpresponse,

                                                                new Acknowledgement<AuthorizeRemoteStopRequest>(
                                                                    Request,
                                                                    StatusCodes.DataError,
                                                                    httpresponse.HTTPStatusCode.ToString(),
                                                                    httpresponse.HTTPBody.      ToUTF8String()
                                                                ),

                                                                IsFault: true

                                                            );

                                                 },

                                                 #endregion

                                                 #region OnException

                                                 OnException: (timestamp, sender, exception) => {

                                                     SendException(timestamp, sender, exception);

                                                     return HTTPResponse<Acknowledgement<AuthorizeRemoteStopRequest>>.ExceptionThrown(

                                                            new Acknowledgement<AuthorizeRemoteStopRequest>(
                                                                Request,
                                                                StatusCodes.ServiceNotAvailable,
                                                                exception.Message,
                                                                exception.StackTrace,
                                                                Request.SessionId
                                                            ),

                                                            Exception: exception

                                                        );

                                                 }

                                                 #endregion

                                                ).ConfigureAwait(false);

            }

            if (result == null)
                result = HTTPResponse<Acknowledgement<AuthorizeRemoteStopRequest>>.ClientError(
                             new Acknowledgement<AuthorizeRemoteStopRequest>(
                                 Request,
                                 StatusCodes.SystemError,
                                 "HTTP request failed!"
                             )
                         );


            #region Send OnAuthorizeRemoteStopResponse event

            var EndTime = DateTime.Now;

            try
            {

                OnAuthorizeRemoteStopResponse?.Invoke(EndTime,
                                                      this,
                                                      ClientId,
                                                      Request.EventTrackingId,
                                                      Request.SessionId,
                                                      Request.ProviderId,
                                                      Request.EVSEId,
                                                      Request.PartnerSessionId,
                                                      Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout,
                                                      result.Content,
                                                      EndTime - StartTime);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnAuthorizeRemoteStopResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region GetChargeDetailRecords(Request)

        /// <summary>
        /// Create a new task querying charge detail records from the OICP server.
        /// </summary>
        /// <param name="Request">An GetChargeDetailRecords request.</param>
        public async Task<HTTPResponse<GetChargeDetailRecordsResponse>>

            GetChargeDetailRecords(GetChargeDetailRecordsRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given GetChargeDetailRecords request must not be null!");

            Request = _CustomGetChargeDetailRecordsRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped GetChargeDetailRecords request must not be null!");

            #endregion

            #region Send OnGetChargeDetailRecordsRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnGetChargeDetailRecordsRequest?.Invoke(StartTime,
                                                        Request.Timestamp.Value,
                                                        this,
                                                        ClientId,
                                                        Request.EventTrackingId,
                                                        Request.ProviderId,
                                                        Request.From,
                                                        Request.To,
                                                        RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnGetChargeDetailRecordsRequest));
            }

            #endregion


            using (var _OICPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    URIPrefix + "/eRoamingAuthorization_V2.0",
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                var result = await _OICPClient.Query(_CustomGetChargeDetailRecordsSOAPRequestMapper(SOAP.Encapsulation(Request.ToXML())),
                                                     "eRoamingGetChargeDetailRecords",
                                                     RequestLogDelegate:   OnGetChargeDetailRecordsSOAPRequest,
                                                     ResponseLogDelegate:  OnGetChargeDetailRecordsSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     QueryTimeout:         Request.RequestTimeout,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, onexception)
                                                                                                              => GetChargeDetailRecordsResponse.ParseXML(request,
                                                                                                                                                         xml,
                                                                                                                                                         CustomGetChargeDetailRecordsResponseMapper,
                                                                                                                                                         CustomChargeDetailRecordXMLMapper,
                                                                                                                                                         onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                         return new HTTPResponse<GetChargeDetailRecordsResponse>(
                                                                    httpresponse,
                                                                    new GetChargeDetailRecordsResponse(
                                                                        Request,
                                                                        new ChargeDetailRecord[0]
                                                                    ),
                                                                    IsFault: true
                                                                );

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, soapclient, httpresponse);

                                                         return new HTTPResponse<GetChargeDetailRecordsResponse>(
                                                                    httpresponse,
                                                                    new GetChargeDetailRecordsResponse(
                                                                        Request,
                                                                        new ChargeDetailRecord[0]
                                                                    ),
                                                                    IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return HTTPResponse<GetChargeDetailRecordsResponse>.ExceptionThrown(
                                                                new GetChargeDetailRecordsResponse(
                                                                    Request,
                                                                    new ChargeDetailRecord[0]
                                                                ),
                                                                exception);

                                                     }

                                                     #endregion

                                                    ).ConfigureAwait(false);

                #region Send OnGetChargeDetailRecordsResponse event

                var EndTime = DateTime.Now;

                try
                {

                    OnGetChargeDetailRecordsResponse?.Invoke(EndTime,
                                                             this,
                                                             ClientId,
                                                             Request.EventTrackingId,
                                                             Request.ProviderId,
                                                             Request.From,
                                                             Request.To,
                                                             Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout,
                                                             result.Content,
                                                             EndTime - StartTime);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPClient) + "." + nameof(OnGetChargeDetailRecordsResponse));
                }

                #endregion

                return result;

            }

        }

        #endregion


    }

}
