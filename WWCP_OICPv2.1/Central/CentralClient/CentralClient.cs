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
using System.Linq;
using System.Xml.Linq;
using System.Net.Security;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.Central
{

    /// <summary>
    /// An OICP Central client.
    /// </summary>
    public partial class CentralClient : ASOAPClient,
                                         ICentralClient
    {

        #region Data

        /// <summary>
        /// The default HTTP user agent string.
        /// </summary>
        public new const           String  DefaultHTTPUserAgent         = "GraphDefined OICP " + Version.Number + " Central Client";

        /// <summary>
        /// The default remote TCP port to connect to.
        /// </summary>
        public new static readonly IPPort  DefaultRemotePort            = IPPort.Parse(443);

        /// <summary>
        /// The default URI prefix.
        /// </summary>
        public const               String  DefaultURIPrefix             = "/ibis/ws";

        /// <summary>
        /// The default HTTP/SOAP/XML URI for OICP authorization requests.
        /// </summary>
        public     const           String  DefaultAuthorizationURI      = "/Authorization";

        /// <summary>
        /// The default HTTP/SOAP/XML URI for OICP Reservation requests.
        /// </summary>
        public     const           String  DefaultReservationURI        = "/Reservation";

        #endregion

        #region Properties

        /// <summary>
        /// The HTTP/SOAP/XML URI for OICP Reservation requests.
        /// </summary>
        public String               ReservationURI      { get; }

        /// <summary>
        /// The HTTP/SOAP/XML URI for OICP Authorization requests.
        /// </summary>
        public String               AuthorizationURI    { get; }


        /// <summary>
        /// The attached OICP Central client (HTTP/SOAP client) logger.
        /// </summary>
        public CentralClientLogger  Logger              { get; }

        #endregion

        #region Custom request mappers

        // Towards CPOs

        #region CustomAuthorizeRemoteReservationStart...

        #region CustomAuthorizeRemoteReservationStartRequestMapper

        private Func<EMP.AuthorizeRemoteReservationStartRequest, EMP.AuthorizeRemoteReservationStartRequest> _CustomAuthorizeRemoteReservationStartRequestMapper = _ => _;

        public Func<EMP.AuthorizeRemoteReservationStartRequest, EMP.AuthorizeRemoteReservationStartRequest> CustomAuthorizeRemoteReservationStartRequestMapper
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

        private Func<EMP.AuthorizeRemoteReservationStartRequest, XElement, XElement> _CustomAuthorizeRemoteReservationStartSOAPRequestMapper = (request, xml) => xml;

        public Func<EMP.AuthorizeRemoteReservationStartRequest, XElement, XElement> CustomAuthorizeRemoteReservationStartSOAPRequestMapper
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

        public CustomXMLParserDelegate<Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>>  CustomAuthorizeRemoteReservationStartParser               { get; set; }

        public CustomXMLSerializerDelegate<EMP.AuthorizeRemoteReservationStartRequest>               CustomAuthorizeRemoteReservationStartRequestSerializer    { get; set; }

        #endregion

        #region CustomAuthorizeRemoteReservationStop...

        #region CustomAuthorizeRemoteReservationStopRequestMapper

        private Func<EMP.AuthorizeRemoteReservationStopRequest, EMP.AuthorizeRemoteReservationStopRequest> _CustomAuthorizeRemoteReservationStopRequestMapper = _ => _;

        public Func<EMP.AuthorizeRemoteReservationStopRequest, EMP.AuthorizeRemoteReservationStopRequest> CustomAuthorizeRemoteReservationStopRequestMapper
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

        private Func<EMP.AuthorizeRemoteReservationStopRequest, XElement, XElement> _CustomAuthorizeRemoteReservationStopSOAPRequestMapper = (request, xml) => xml;

        public Func<EMP.AuthorizeRemoteReservationStopRequest, XElement, XElement> CustomAuthorizeRemoteReservationStopSOAPRequestMapper
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

        public CustomXMLParserDelegate<Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>>   CustomAuthorizeRemoteReservationStopParser                { get; set; }

        public CustomXMLSerializerDelegate<EMP.AuthorizeRemoteReservationStopRequest>                CustomAuthorizeRemoteReservationStopRequestSerializer     { get; set; }

        #endregion

        #region CustomAuthorizeRemoteStart...

        #region CustomAuthorizeRemoteStartRequestMapper

        private Func<EMP.AuthorizeRemoteStartRequest, EMP.AuthorizeRemoteStartRequest> _CustomAuthorizeRemoteStartRequestMapper = _ => _;

        public Func<EMP.AuthorizeRemoteStartRequest, EMP.AuthorizeRemoteStartRequest> CustomAuthorizeRemoteStartRequestMapper
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

        private Func<EMP.AuthorizeRemoteStartRequest, XElement, XElement> _CustomAuthorizeRemoteStartSOAPRequestMapper = (request, xml) => xml;

        public Func<EMP.AuthorizeRemoteStartRequest, XElement, XElement> CustomAuthorizeRemoteStartSOAPRequestMapper
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

        public CustomXMLParserDelegate<Acknowledgement<EMP.AuthorizeRemoteStartRequest>>             CustomAuthorizeRemoteStartParser                          { get; set; }

        public CustomXMLSerializerDelegate<EMP.AuthorizeRemoteStartRequest>                          CustomAuthorizeRemoteStartRequestSerializer               { get; set; }

        #endregion

        #region CustomAuthorizeRemoteStop...

        #region CustomAuthorizeRemoteStopRequestMapper

        private Func<EMP.AuthorizeRemoteStopRequest, EMP.AuthorizeRemoteStopRequest> _CustomAuthorizeRemoteStopRequestMapper = _ => _;

        public Func<EMP.AuthorizeRemoteStopRequest, EMP.AuthorizeRemoteStopRequest> CustomAuthorizeRemoteStopRequestMapper
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

        private Func<EMP.AuthorizeRemoteStopRequest, XElement, XElement> _CustomAuthorizeRemoteStopSOAPRequestMapper = (request, xml) => xml;

        public Func<EMP.AuthorizeRemoteStopRequest, XElement, XElement> CustomAuthorizeRemoteStopSOAPRequestMapper
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

        public CustomXMLParserDelegate<Acknowledgement<EMP.AuthorizeRemoteStopRequest>>              CustomAuthorizeRemoteStopParser                           { get; set; }

        public CustomXMLSerializerDelegate<EMP.AuthorizeRemoteStopRequest>                           CustomAuthorizeRemoteStopRequestSerializer                { get; set; }

        #endregion


        // Towards EMPs

        #region CustomAuthorizeStart(SOAP)RequestMapper

        #region CustomAuthorizeStartRequestMapper

        private Func<CPO.AuthorizeStartRequest, CPO.AuthorizeStartRequest> _CustomAuthorizeStartRequestMapper = _ => _;

        public Func<CPO.AuthorizeStartRequest, CPO.AuthorizeStartRequest> CustomAuthorizeStartRequestMapper
        {

            get
            {
                return _CustomAuthorizeStartRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomAuthorizeStartRequestMapper = value;
            }

        }

        #endregion

        #region CustomAuthorizeStartSOAPRequestMapper

        private Func<CPO.AuthorizeStartRequest, XElement, XElement> _CustomAuthorizeStartSOAPRequestMapper = (request, xml) => xml;

        public Func<CPO.AuthorizeStartRequest, XElement, XElement> CustomAuthorizeStartSOAPRequestMapper
        {

            get
            {
                return _CustomAuthorizeStartSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomAuthorizeStartSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<CPO.AuthorizationStart>                                       CustomAuthorizeStartResponseParser                        { get; set; }

        public CustomXMLSerializerDelegate<CPO.AuthorizeStartRequest>                                CustomAuthorizeStartRequestSerializer                     { get; set; }

        #endregion

        #region CustomAuthorizeStop(SOAP)RequestMapper

        #region CustomAuthorizeStopRequestMapper

        private Func<CPO.AuthorizeStopRequest, CPO.AuthorizeStopRequest> _CustomAuthorizeStopRequestMapper = _ => _;

        public Func<CPO.AuthorizeStopRequest, CPO.AuthorizeStopRequest> CustomAuthorizeStopRequestMapper
        {

            get
            {
                return _CustomAuthorizeStopRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomAuthorizeStopRequestMapper = value;
            }

        }

        #endregion

        #region CustomAuthorizeStopSOAPRequestMapper

        private Func<CPO.AuthorizeStopRequest, XElement, XElement> _CustomAuthorizeStopSOAPRequestMapper = (request, xml) => xml;

        public Func<CPO.AuthorizeStopRequest, XElement, XElement> CustomAuthorizeStopSOAPRequestMapper
        {

            get
            {
                return _CustomAuthorizeStopSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomAuthorizeStopSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<CPO.AuthorizationStop>                                        CustomAuthorizeStopParser                                 { get; set; }

        public CustomXMLSerializerDelegate<CPO.AuthorizeStopRequest>                                 CustomAuthorizeStopRequestSerializer                      { get; set; }

        #endregion

        #region CustomSendChargeDetailRecord(SOAP)RequestMapper

        #region CustomSendChargeDetailRecordRequestMapper

        private Func<CPO.SendChargeDetailRecordRequest, CPO.SendChargeDetailRecordRequest> _CustomSendChargeDetailRecordRequestMapper = _ => _;

        public Func<CPO.SendChargeDetailRecordRequest, CPO.SendChargeDetailRecordRequest> CustomSendChargeDetailRecordRequestMapper
        {

            get
            {
                return _CustomSendChargeDetailRecordRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomSendChargeDetailRecordRequestMapper = value;
            }

        }

        #endregion

        #region CustomSendChargeDetailRecordSOAPRequestMapper

        private Func<CPO.SendChargeDetailRecordRequest, XElement, XElement> _CustomSendChargeDetailRecordSOAPRequestMapper = (request, xml) => xml;

        public Func<CPO.SendChargeDetailRecordRequest, XElement, XElement> CustomSendChargeDetailRecordSOAPRequestMapper
        {

            get
            {
                return _CustomSendChargeDetailRecordSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomSendChargeDetailRecordSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<Acknowledgement<CPO.SendChargeDetailRecordRequest>>           CustomSendChargeDetailRecordParser                        { get; set; }

        public CustomXMLSerializerDelegate<ChargeDetailRecord>                                       CustomChargeDetailRecordSerializer                        { get; set; }

        #endregion

        public CustomXMLSerializerDelegate<Identification>                                           CustomIdentificationSerializer                            { get; set; }

        public CustomXMLParserDelegate<Identification>                                               CustomIdentificationParser                                { get; set; }



        public CustomXMLParserDelegate<StatusCode>         CustomStatusCodeParser       { get; set; }

        #endregion

        #region Events

        // Towards CPOs

        #region OnAuthorizeRemoteReservationStartRequest/-Response

        /// <summary>
        /// An event fired whenever an 'authorize remote reservation start' request will be send.
        /// </summary>
        public event OnAuthorizeRemoteReservationStartRequestHandler   OnAuthorizeRemoteReservationStartRequest;

        /// <summary>
        /// An event fired whenever an 'authorize remote reservation start' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                           OnAuthorizeRemoteReservationStartSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to an 'authorize remote reservation start' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                          OnAuthorizeRemoteReservationStartSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to an 'authorize remote reservation start' request had been received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStartResponseHandler  OnAuthorizeRemoteReservationStartResponse;

        #endregion

        #region OnAuthorizeRemoteReservationStopRequest/-Response

        /// <summary>
        /// An event fired whenever a 'authorize remote reservation stop' request will be send.
        /// </summary>
        public event OnAuthorizeRemoteReservationStopRequestHandler   OnAuthorizeRemoteReservationStopRequest;

        /// <summary>
        /// An event fired whenever a 'authorize remote reservation stop' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                          OnAuthorizeRemoteReservationStopSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'authorize remote reservation stop' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                         OnAuthorizeRemoteReservationStopSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'authorize remote reservation stop' request had been received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStopResponseHandler  OnAuthorizeRemoteReservationStopResponse;

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
        /// An event fired whenever a 'authorize remote stop' request will be send.
        /// </summary>
        public event OnAuthorizeRemoteStopRequestHandler   OnAuthorizeRemoteStopRequest;

        /// <summary>
        /// An event fired whenever a 'authorize remote stop' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler               OnAuthorizeRemoteStopSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'authorize remote stop' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler              OnAuthorizeRemoteStopSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'authorize remote stop' request had been received.
        /// </summary>
        public event OnAuthorizeRemoteStopResponseHandler  OnAuthorizeRemoteStopResponse;

        #endregion


        // Towards EMPs

        #region OnAuthorizeStartRequest/-Response

        /// <summary>
        /// An event fired whenever an authorize start request will be send.
        /// </summary>
        public event OnAuthorizeStartRequestHandler   OnAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever an authorize start SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler          OnAuthorizeStartSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to an authorize start SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler         OnAuthorizeStartSOAPResponse;

        /// <summary>
        /// An event fired whenever an authorize start request was sent.
        /// </summary>
        public event OnAuthorizeStartResponseHandler  OnAuthorizeStartResponse;

        #endregion

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event fired whenever an 'authorize stop' request will be send.
        /// </summary>
        public event OnAuthorizeStopRequestHandler   OnAuthorizeStopRequest;

        /// <summary>
        /// An event fired whenever an 'authorize stop' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler         OnAuthorizeStopSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'authorize stop' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler        OnAuthorizeStopSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'authorize stop' request had been received.
        /// </summary>
        public event OnAuthorizeStopResponseHandler  OnAuthorizeStopResponse;

        #endregion

        #region OnSendChargeDetailRecordRequest/-Response

        /// <summary>
        /// An event fired whenever a 'send charge detail record' request will be send.
        /// </summary>
        public event OnSendChargeDetailRecordRequestHandler   OnSendChargeDetailRecordRequest;

        /// <summary>
        /// An event fired whenever a 'send charge detail record' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                  OnSendChargeDetailRecordSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'send charge detail record' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                 OnSendChargeDetailRecordSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'send charge detail record' request had been received.
        /// </summary>
        public event OnSendChargeDetailRecordResponseHandler  OnSendChargeDetailRecordResponse;

        #endregion

        #endregion

        #region Constructor(s)

        #region CentralClient(ClientId, Hostname, ..., LoggingContext = CentralClientLogger.DefaultContext, ...)

        /// <summary>
        /// Create a new OICP Central client.
        /// </summary>
        /// <param name="ClientId">A unqiue identification of this client.</param>
        /// <param name="Hostname">The hostname of the remote OICP service.</param>
        /// <param name="RemotePort">An optional TCP port of the remote OICP service.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual hostname of the remote OICP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// <param name="MaxNumberOfRetries">The default number of maximum transmission retries.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public CentralClient(String                               ClientId,
                             String                               Hostname,
                             IPPort                               RemotePort                   = null,
                             RemoteCertificateValidationCallback  RemoteCertificateValidator   = null,
                             LocalCertificateSelectionCallback    LocalCertificateSelector     = null,
                             X509Certificate                      ClientCert                   = null,
                             String                               HTTPVirtualHost              = null,
                             String                               URIPrefix                    = DefaultURIPrefix,
                             String                               HTTPUserAgent                = DefaultHTTPUserAgent,
                             TimeSpan?                            RequestTimeout               = null,
                             Byte?                                MaxNumberOfRetries           = DefaultMaxNumberOfRetries,
                             DNSClient                            DNSClient                    = null,
                             String                               LoggingContext               = CentralClientLogger.DefaultContext,
                             LogfileCreatorDelegate               LogfileCreator               = null)

            : base(ClientId,
                   Hostname,
                   RemotePort ?? DefaultRemotePort,
                   RemoteCertificateValidator,
                   LocalCertificateSelector,
                   ClientCert,
                   HTTPVirtualHost,
                   URIPrefix.Trim().IsNotNullOrEmpty() ? URIPrefix : DefaultURIPrefix,
                   null,
                   HTTPUserAgent,
                   RequestTimeout,
                   MaxNumberOfRetries,
                   DNSClient)

        {

            #region Initial checks

            if (ClientId.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Logger),    "The given client identification must not be null or empty!");

            if (Hostname.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Hostname),  "The given hostname must not be null or empty!");

            #endregion


            this.AuthorizationURI  = AuthorizationURI ?? DefaultAuthorizationURI;
            this.ReservationURI    = ReservationURI   ?? DefaultReservationURI;

            this.Logger            = new CentralClientLogger(this,
                                                             LoggingContext,
                                                             LogfileCreator);

        }

        #endregion

        #region CentralClient(ClientId, Logger, Hostname, ...)

        /// <summary>
        /// Create a new OICP Central client.
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
        /// <param name="MaxNumberOfRetries">The default number of maximum transmission retries.</param>
        /// <param name="DNSClient">An optional DNS client.</param>
        public CentralClient(String                               ClientId,
                             CentralClientLogger                  Logger,
                             String                               Hostname,
                             IPPort                               RemotePort                   = null,
                             RemoteCertificateValidationCallback  RemoteCertificateValidator   = null,
                             LocalCertificateSelectionCallback    LocalCertificateSelector     = null,
                             X509Certificate                      ClientCert                   = null,
                             String                               HTTPVirtualHost              = null,
                             String                               URIPrefix                    = DefaultURIPrefix,
                             String                               HTTPUserAgent                = DefaultHTTPUserAgent,
                             TimeSpan?                            RequestTimeout               = null,
                             Byte?                                MaxNumberOfRetries           = DefaultMaxNumberOfRetries,
                             DNSClient                            DNSClient                    = null)

            : base(ClientId,
                   Hostname,
                   RemotePort ?? DefaultRemotePort,
                   RemoteCertificateValidator,
                   LocalCertificateSelector,
                   ClientCert,
                   HTTPVirtualHost,
                   URIPrefix.Trim().IsNotNullOrEmpty() ? URIPrefix : DefaultURIPrefix,
                   null,
                   HTTPUserAgent,
                   RequestTimeout,
                   MaxNumberOfRetries,
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


            this.AuthorizationURI  = AuthorizationURI ?? DefaultAuthorizationURI;
            this.ReservationURI    = ReservationURI   ?? DefaultReservationURI;

            this.Logger            = Logger;

        }

        #endregion

        #endregion


        // Towards CPOs

        #region AuthorizeRemoteReservationStart(Request)

        /// <summary>
        /// Reserve an EVSE at an operator.
        /// </summary>
        /// <param name="Request">A AuthorizeRemoteReservationStart request.</param>
        public async Task<HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>>>

            AuthorizeRemoteReservationStart(EMP.AuthorizeRemoteReservationStartRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The given AuthorizeRemoteReservationStart request must not be null!");

            Request = _CustomAuthorizeRemoteReservationStartRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The mapped AuthorizeRemoteReservationStart request must not be null!");


            HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>> result = null;

            #endregion

            #region Send OnAuthorizeRemoteReservationStartRequest event

            var StartTime = DateTime.Now;

            try
            {

                if (OnAuthorizeRemoteReservationStartRequest != null)
                    await Task.WhenAll(OnAuthorizeRemoteReservationStartRequest.GetInvocationList().
                                       Cast<OnAuthorizeRemoteReservationStartRequestHandler>().
                                       Select(e => e(StartTime,
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
                                                     Request.RequestTimeout ?? RequestTimeout.Value))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CentralClient) + "." + nameof(OnAuthorizeRemoteReservationStartRequest));
            }

            #endregion


            using (var _OICPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    URIPrefix + ReservationURI,
                                                    RemoteCertificateValidator,
                                                    LocalCertificateSelector,
                                                    ClientCert,
                                                    UserAgent,
                                                    RequestTimeout,
                                                    DNSClient))
            {

                result = await _OICPClient.Query(_CustomAuthorizeRemoteReservationStartSOAPRequestMapper(Request,
                                                                                                         SOAP.Encapsulation(Request.ToXML(CustomAuthorizeRemoteReservationStartRequestSerializer,
                                                                                                                                          CustomIdentificationSerializer))),
                                                 "eRoamingAuthorizeRemoteReservationStart",
                                                 RequestLogDelegate:   OnAuthorizeRemoteReservationStartSOAPRequest,
                                                 ResponseLogDelegate:  OnAuthorizeRemoteReservationStartSOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 RequestTimeout:         Request.RequestTimeout ?? RequestTimeout.Value,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                      (request, xml, onexception) =>
                                                                                                          Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>.Parse(request,
                                                                                                                                                                            xml,
                                                                                                                                                                            CustomAuthorizeRemoteReservationStartParser,
                                                                                                                                                                            CustomStatusCodeParser,
                                                                                                                                                                            onexception)),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                     return new HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>>(

                                                                httpresponse,

                                                                new Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>(
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

                                                     return new HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>>(

                                                                httpresponse,

                                                                new Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>(
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

                                                     return HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>>.ExceptionThrown(

                                                                new Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>(
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
                result = HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>>.ClientError(
                             new Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>(
                                 Request,
                                 StatusCodes.SystemError,
                                 "HTTP request failed!"
                             )
                         );


            #region Send OnAuthorizeRemoteReservationStartResponse event

            var Endtime = DateTime.Now;

            try
            {

                if (OnAuthorizeRemoteReservationStartResponse != null)
                    await Task.WhenAll(OnAuthorizeRemoteReservationStartResponse.GetInvocationList().
                                       Cast<OnAuthorizeRemoteReservationStartResponseHandler>().
                                       Select(e => e(Endtime,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.ProviderId,
                                                     Request.EVSEId,
                                                     Request.EVCOId,
                                                     Request.SessionId,
                                                     Request.PartnerSessionId,
                                                     Request.PartnerProductId,
                                                     Request.RequestTimeout ?? RequestTimeout.Value,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CentralClient) + "." + nameof(OnAuthorizeRemoteReservationStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeRemoteReservationStop (Request)

        /// <summary>
        /// Delete a reservation of an EVSE at an operator.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteReservationStop request.</param>
        public async Task<HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>>>

            AuthorizeRemoteReservationStop(EMP.AuthorizeRemoteReservationStopRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given AuthorizeRemoteReservationStop request must not be null!");

            Request = _CustomAuthorizeRemoteReservationStopRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped AuthorizeRemoteReservationStop request must not be null!");


            HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>> result = null;

            #endregion

            #region Send OnAuthorizeRemoteReservationStopRequest event

            var StartTime = DateTime.Now;

            try
            {

                if (OnAuthorizeRemoteReservationStopRequest != null)
                    await Task.WhenAll(OnAuthorizeRemoteReservationStopRequest.GetInvocationList().
                                       Cast<OnAuthorizeRemoteReservationStopRequestHandler>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.SessionId,
                                                     Request.ProviderId,
                                                     Request.EVSEId,
                                                     Request.PartnerSessionId,
                                                     Request.RequestTimeout ?? RequestTimeout.Value))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CentralClient) + "." + nameof(OnAuthorizeRemoteReservationStopRequest));
            }

            #endregion


            using (var _OICPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    URIPrefix + ReservationURI,
                                                    RemoteCertificateValidator,
                                                    LocalCertificateSelector,
                                                    ClientCert,
                                                    UserAgent,
                                                    RequestTimeout,
                                                    DNSClient))
            {

                result = await _OICPClient.Query(_CustomAuthorizeRemoteReservationStopSOAPRequestMapper(Request,
                                                                                                        SOAP.Encapsulation(Request.ToXML(CustomAuthorizeRemoteReservationStopRequestSerializer))),
                                                 "eRoamingAuthorizeRemoteReservationStop",
                                                 RequestLogDelegate:   OnAuthorizeRemoteReservationStopSOAPRequest,
                                                 ResponseLogDelegate:  OnAuthorizeRemoteReservationStopSOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 RequestTimeout:       Request.RequestTimeout ?? RequestTimeout.Value,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                      (request, xml, onexception) =>
                                                                                                          Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>.Parse(request,
                                                                                                                                                                           xml,
                                                                                                                                                                           CustomAuthorizeRemoteReservationStopParser,
                                                                                                                                                                           CustomStatusCodeParser,
                                                                                                                                                                           onexception)),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                     return new HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>>(

                                                                httpresponse,

                                                                new Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>(
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

                                                     return new HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>>(

                                                                httpresponse,

                                                                new Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>(
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

                                                     return HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>>.ExceptionThrown(

                                                                new Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>(
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
                result = HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>>.ClientError(
                             new Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>(
                                 Request,
                                 StatusCodes.SystemError,
                                 "HTTP request failed!"
                             )
                         );


            #region Send OnAuthorizeRemoteReservationStopResponse event

            var EndTime = DateTime.Now;

            try
            {

                if (OnAuthorizeRemoteReservationStopResponse != null)
                    await Task.WhenAll(OnAuthorizeRemoteReservationStopResponse.GetInvocationList().
                                       Cast<OnAuthorizeRemoteReservationStopResponseHandler>().
                                       Select(e => e(EndTime,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.SessionId,
                                                     Request.ProviderId,
                                                     Request.EVSEId,
                                                     Request.PartnerSessionId,
                                                     Request.RequestTimeout ?? RequestTimeout.Value,
                                                     result.Content,
                                                     EndTime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CentralClient) + "." + nameof(OnAuthorizeRemoteReservationStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeRemoteStart(Request)

        /// <summary>
        /// Start a remote charging session at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteStart request.</param>
        public async Task<HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteStartRequest>>>

            AuthorizeRemoteStart(EMP.AuthorizeRemoteStartRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The given AuthorizeRemoteStart request must not be null!");

            Request = _CustomAuthorizeRemoteStartRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The mapped AuthorizeRemoteStart request must not be null!");


            HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteStartRequest>> result = null;

            #endregion

            #region Send OnAuthorizeRemoteStartRequest event

            var StartTime = DateTime.Now;

            try
            {

                if (OnAuthorizeRemoteStartRequest != null)
                    await Task.WhenAll(OnAuthorizeRemoteStartRequest.GetInvocationList().
                                       Cast<OnAuthorizeRemoteStartRequestHandler>().
                                       Select(e => e(StartTime,
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
                                                     Request.RequestTimeout ?? RequestTimeout.Value))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CentralClient) + "." + nameof(OnAuthorizeRemoteStartRequest));
            }

            #endregion


            using (var _OICPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    URIPrefix + AuthorizationURI,
                                                    RemoteCertificateValidator,
                                                    LocalCertificateSelector,
                                                    ClientCert,
                                                    UserAgent,
                                                    RequestTimeout,
                                                    DNSClient))
            {


                result = await _OICPClient.Query(_CustomAuthorizeRemoteStartSOAPRequestMapper(Request,
                                                                                              SOAP.Encapsulation(Request.ToXML(CustomAuthorizeRemoteStartRequestSerializer,
                                                                                                                               CustomIdentificationSerializer))),
                                                 "AuthorizeRemoteStart",
                                                 RequestLogDelegate:   OnAuthorizeRemoteStartSOAPRequest,
                                                 ResponseLogDelegate:  OnAuthorizeRemoteStartSOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 RequestTimeout:         Request.RequestTimeout ?? RequestTimeout.Value,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                      (request, xml, onexception) =>
                                                                                                          Acknowledgement<EMP.AuthorizeRemoteStartRequest>.Parse(request,
                                                                                                                                                                 xml,
                                                                                                                                                                 CustomAuthorizeRemoteStartParser,
                                                                                                                                                                 CustomStatusCodeParser,
                                                                                                                                                                 onexception)),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                     return new HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteStartRequest>>(

                                                                httpresponse,

                                                                new Acknowledgement<EMP.AuthorizeRemoteStartRequest>(
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

                                                     return new HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteStartRequest>>(

                                                                httpresponse,

                                                                new Acknowledgement<EMP.AuthorizeRemoteStartRequest>(
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

                                                     return HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteStartRequest>>.ExceptionThrown(

                                                                new Acknowledgement<EMP.AuthorizeRemoteStartRequest>(
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
                result = HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteStartRequest>>.ClientError(
                             new Acknowledgement<EMP.AuthorizeRemoteStartRequest>(
                                 Request,
                                 StatusCodes.SystemError,
                                 "HTTP request failed!"
                             )
                         );


            #region Send OnAuthorizeRemoteStartResponse event

            var Endtime = DateTime.Now;

            try
            {

                if (OnAuthorizeRemoteStartResponse != null)
                    await Task.WhenAll(OnAuthorizeRemoteStartResponse.GetInvocationList().
                                       Cast<OnAuthorizeRemoteStartResponseHandler>().
                                       Select(e => e(Endtime,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.ProviderId,
                                                     Request.EVSEId,
                                                     Request.EVCOId,
                                                     Request.SessionId,
                                                     Request.PartnerSessionId,
                                                     Request.PartnerProductId,
                                                     Request.RequestTimeout ?? RequestTimeout.Value,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CentralClient) + "." + nameof(OnAuthorizeRemoteStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeRemoteStop (Request)

        /// <summary>
        /// Stop a remote charging session at the given EVSE.
        /// </summary>
        /// <param name="Request">An AuthorizeRemoteStop request.</param>
        public async Task<HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteStopRequest>>>

            AuthorizeRemoteStop(EMP.AuthorizeRemoteStopRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given AuthorizeRemoteStop request must not be null!");

            Request = _CustomAuthorizeRemoteStopRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped AuthorizeRemoteStop request must not be null!");


            HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteStopRequest>> result = null;

            #endregion

            #region Send OnAuthorizeRemoteStopRequest event

            var StartTime = DateTime.Now;

            try
            {

                if (OnAuthorizeRemoteStopRequest != null)
                    await Task.WhenAll(OnAuthorizeRemoteStopRequest.GetInvocationList().
                                       Cast<OnAuthorizeRemoteStopRequestHandler>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.SessionId,
                                                     Request.ProviderId,
                                                     Request.EVSEId,
                                                     Request.PartnerSessionId,
                                                     Request.RequestTimeout ?? RequestTimeout.Value))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CentralClient) + "." + nameof(OnAuthorizeRemoteStopRequest));
            }

            #endregion


            using (var _OICPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    URIPrefix + AuthorizationURI,
                                                    RemoteCertificateValidator,
                                                    LocalCertificateSelector,
                                                    ClientCert,
                                                    UserAgent,
                                                    RequestTimeout,
                                                    DNSClient))
            {

                result = await _OICPClient.Query(_CustomAuthorizeRemoteStopSOAPRequestMapper(Request,
                                                                                             SOAP.Encapsulation(Request.ToXML(CustomAuthorizeRemoteStopRequestSerializer))),
                                                 "AuthorizeRemoteStop",
                                                 RequestLogDelegate:   OnAuthorizeRemoteStopSOAPRequest,
                                                 ResponseLogDelegate:  OnAuthorizeRemoteStopSOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 RequestTimeout:         Request.RequestTimeout ?? RequestTimeout.Value,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                      (request, xml, onexception) =>
                                                                                                          Acknowledgement<EMP.AuthorizeRemoteStopRequest>.Parse(request,
                                                                                                                                                                xml,
                                                                                                                                                                CustomAuthorizeRemoteStopParser,
                                                                                                                                                                CustomStatusCodeParser,
                                                                                                                                                                onexception)),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                     return new HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteStopRequest>>(

                                                                httpresponse,

                                                                new Acknowledgement<EMP.AuthorizeRemoteStopRequest>(
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

                                                     return new HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteStopRequest>>(

                                                                httpresponse,

                                                                new Acknowledgement<EMP.AuthorizeRemoteStopRequest>(
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

                                                     return HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteStopRequest>>.ExceptionThrown(

                                                                new Acknowledgement<EMP.AuthorizeRemoteStopRequest>(
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
                result = HTTPResponse<Acknowledgement<EMP.AuthorizeRemoteStopRequest>>.ClientError(
                             new Acknowledgement<EMP.AuthorizeRemoteStopRequest>(
                                 Request,
                                 StatusCodes.SystemError,
                                 "HTTP request failed!"
                             )
                         );


            #region Send OnAuthorizeRemoteStopResponse event

            var EndTime = DateTime.Now;

            try
            {

                if (OnAuthorizeRemoteStopResponse != null)
                    await Task.WhenAll(OnAuthorizeRemoteStopResponse.GetInvocationList().
                                       Cast<OnAuthorizeRemoteStopResponseHandler>().
                                       Select(e => e(EndTime,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.SessionId,
                                                     Request.ProviderId,
                                                     Request.EVSEId,
                                                     Request.PartnerSessionId,
                                                     Request.RequestTimeout ?? RequestTimeout.Value,
                                                     result.Content,
                                                     EndTime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CentralClient) + "." + nameof(OnAuthorizeRemoteStartResponse));
            }

            #endregion

            return result;

        }

        #endregion


        // Towards EMPs

        #region AuthorizeStart(Request)

        /// <summary>
        /// Create an OICP authorize start request.
        /// </summary>
        /// <param name="Request">A AuthorizeStart request.</param>
        public async Task<HTTPResponse<CPO.AuthorizationStart>>

            AuthorizeStart(CPO.AuthorizeStartRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given AuthorizeStart request must not be null!");

            Request = _CustomAuthorizeStartRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped AuthorizeStart request must not be null!");


            HTTPResponse<CPO.AuthorizationStart> result = null;

            #endregion

            #region Send OnAuthorizeStartRequest event

            var StartTime = DateTime.Now;

            try
            {

                if (OnAuthorizeStartRequest != null)
                    await Task.WhenAll(OnAuthorizeStartRequest.GetInvocationList().
                                       Cast<OnAuthorizeStartRequestHandler>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.OperatorId,
                                                     Request.Identification,
                                                     Request.EVSEId,
                                                     Request.PartnerProductId,
                                                     Request.SessionId,
                                                     Request.PartnerSessionId,
                                                     Request.RequestTimeout ?? RequestTimeout.Value))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CentralClient) + "." + nameof(OnAuthorizeStartRequest));
            }

            #endregion


            using (var _OICPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    URIPrefix + AuthorizationURI,
                                                    RemoteCertificateValidator,
                                                    LocalCertificateSelector,
                                                    ClientCert,
                                                    UserAgent,
                                                    RequestTimeout,
                                                    DNSClient))
            {

                result = await _OICPClient.Query(_CustomAuthorizeStartSOAPRequestMapper(Request,
                                                                                        SOAP.Encapsulation(Request.ToXML(CustomAuthorizeStartRequestSerializer,
                                                                                                                         CustomIdentificationSerializer))),
                                                 "eRoamingAuthorizeStart",
                                                 RequestLogDelegate:   OnAuthorizeStartSOAPRequest,
                                                 ResponseLogDelegate:  OnAuthorizeStartSOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 RequestTimeout:         Request.RequestTimeout ?? RequestTimeout.Value,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                      (request, xml, onexception) => CPO.AuthorizationStart.Parse(request,
                                                                                                                                                                  xml,
                                                                                                                                                                  CustomAuthorizeStartResponseParser,
                                                                                                                                                                  CustomIdentificationParser,
                                                                                                                                                                  CustomStatusCodeParser,
                                                                                                                                                                  onexception)),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<CPO.AuthorizationStart>(httpresponse,
                                                                                                     CPO.AuthorizationStart.DataError(
                                                                                                         Request,
                                                                                                         httpresponse.Content.ToString()
                                                                                                     ),
                                                                                                     IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnHTTPError

                                                 OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                     SendHTTPError(timestamp, this, httpresponse);

                                                     return new HTTPResponse<CPO.AuthorizationStart>(httpresponse,
                                                                                                     CPO.AuthorizationStart.DataError(
                                                                                                         Request,
                                                                                                         httpresponse.HTTPStatusCode.ToString(),
                                                                                                         httpresponse.HTTPBody.ToUTF8String()
                                                                                                     ),
                                                                                                     IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnException

                                                 OnException: (timestamp, sender, exception) => {

                                                     SendException(timestamp, sender, exception);

                                                     return HTTPResponse<CPO.AuthorizationStart>.ExceptionThrown(CPO.AuthorizationStart.SystemError(
                                                                                                                     Request,
                                                                                                                     exception.Message,
                                                                                                                     exception.StackTrace
                                                                                                                 ),
                                                                                                                 Exception: exception);

                                                 }

                                                 #endregion

                                                ).ConfigureAwait(false);

            }

            if (result == null)
                  result = HTTPResponse<CPO.AuthorizationStart>.ClientError(
                               CPO.AuthorizationStart.SystemError(
                                   Request,
                                   "HTTP request failed!"
                               )
                           );


            #region Send OnAuthorizeStartResponse event

            try
            {

                var Endtime = DateTime.Now;

                if (OnAuthorizeStartResponse != null)
                    await Task.WhenAll(OnAuthorizeStartResponse.GetInvocationList().
                                       Cast<OnAuthorizeStartResponseHandler>().
                                       Select(e => e(Endtime,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.OperatorId,
                                                     Request.Identification,
                                                     Request.EVSEId,
                                                     Request.PartnerProductId,
                                                     Request.SessionId,
                                                     Request.PartnerSessionId,
                                                     Request.RequestTimeout ?? RequestTimeout.Value,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CentralClient) + "." + nameof(OnAuthorizeStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop (Request)

        /// <summary>
        /// Create an OICP authorize stop request.
        /// </summary>
        /// <param name="Request">A AuthorizeStart request.</param>
        public async Task<HTTPResponse<CPO.AuthorizationStop>>

            AuthorizeStop(CPO.AuthorizeStopRequest  Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given AuthorizeStop request must not be null!");

            Request = _CustomAuthorizeStopRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped AuthorizeStop request must not be null!");


            HTTPResponse<CPO.AuthorizationStop> result = null;

            #endregion

            #region Send OnAuthorizeStopRequest event

            var StartTime = DateTime.Now;

            try
            {

                if (OnAuthorizeStopRequest != null)
                    await Task.WhenAll(OnAuthorizeStopRequest.GetInvocationList().
                                       Cast<OnAuthorizeStopRequestHandler>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.OperatorId,
                                                     Request.SessionId,
                                                     Request.UID,
                                                     Request.EVSEId,
                                                     Request.PartnerSessionId,
                                                     Request.RequestTimeout ?? RequestTimeout.Value))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CentralClient) + "." + nameof(OnAuthorizeStopRequest));
            }

            #endregion


            using (var _OICPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    URIPrefix + AuthorizationURI,
                                                    RemoteCertificateValidator,
                                                    LocalCertificateSelector,
                                                    ClientCert,
                                                    UserAgent,
                                                    RequestTimeout,
                                                    DNSClient))
            {

                result = await _OICPClient.Query(_CustomAuthorizeStopSOAPRequestMapper(Request,
                                                                                       SOAP.Encapsulation(Request.ToXML(CustomAuthorizeStopRequestSerializer,
                                                                                                                        CustomIdentificationSerializer))),
                                                 "eRoamingAuthorizeStop",
                                                 RequestLogDelegate:   OnAuthorizeStopSOAPRequest,
                                                 ResponseLogDelegate:  OnAuthorizeStopSOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 RequestTimeout:         Request.RequestTimeout ?? RequestTimeout.Value,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                      (request, xml, onexception) => CPO.AuthorizationStop.Parse(request,
                                                                                                                                                                 xml,
                                                                                                                                                                 CustomAuthorizeStopParser,
                                                                                                                                                                 CustomStatusCodeParser,
                                                                                                                                                                 onexception)),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<CPO.AuthorizationStop>(httpresponse,
                                                                                                    CPO.AuthorizationStop.DataError(Request,
                                                                                                                                    httpresponse.Content.ToString()),
                                                                                                    IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnHTTPError

                                                 OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                     SendHTTPError(timestamp, this, httpresponse);

                                                     return new HTTPResponse<CPO.AuthorizationStop>(httpresponse,
                                                                                                    CPO.AuthorizationStop.DataError(
                                                                                                        Request,
                                                                                                        httpresponse.HTTPStatusCode.ToString(),
                                                                                                        httpresponse.HTTPBody.      ToUTF8String()
                                                                                                    ),
                                                                                                    IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnException

                                                 OnException: (timestamp, sender, exception) => {

                                                     SendException(timestamp, sender, exception);

                                                     return HTTPResponse<CPO.AuthorizationStop>.ExceptionThrown(CPO.AuthorizationStop.SystemError(
                                                                                                                    Request,
                                                                                                                    exception.Message,
                                                                                                                    exception.StackTrace
                                                                                                                ),
                                                                                                                Exception: exception);

                                                 }

                                                 #endregion

                                                ).ConfigureAwait(false);

            }

            if (result == null)
                result = HTTPResponse<CPO.AuthorizationStop>.ClientError(
                             CPO.AuthorizationStop.SystemError(
                                 Request,
                                 "HTTP request failed!"
                             )
                         );


            #region Send OnAuthorizeStopResponse event

            var Endtime = DateTime.Now;

            try
            {

                if (OnAuthorizeStopResponse != null)
                    await Task.WhenAll(OnAuthorizeStopResponse.GetInvocationList().
                                       Cast<OnAuthorizeStopResponseHandler>().
                                       Select(e => e(Endtime,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.OperatorId,
                                                     Request.SessionId,
                                                     Request.UID,
                                                     Request.EVSEId,
                                                     Request.PartnerSessionId,
                                                     Request.RequestTimeout ?? RequestTimeout.Value,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CentralClient) + "." + nameof(OnAuthorizeStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region SendChargeDetailRecord(Request)

        /// <summary>
        /// Create an authorize remote start request.
        /// </summary>
        /// <param name="Request">A SendChargeDetailRecord request.</param>
        public async Task<HTTPResponse<Acknowledgement<CPO.SendChargeDetailRecordRequest>>>

            SendChargeDetailRecord(CPO.SendChargeDetailRecordRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The given SendChargeDetailRecord request must not be null!");

            Request = _CustomSendChargeDetailRecordRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The mapped SendChargeDetailRecord request must not be null!");


            HTTPResponse<Acknowledgement<CPO.SendChargeDetailRecordRequest>> result = null;

            #endregion

            #region Send OnSendChargeDetailRecordRequest event

            var StartTime = DateTime.Now;

            try
            {

                if (OnSendChargeDetailRecordRequest != null)
                    await Task.WhenAll(OnSendChargeDetailRecordRequest.GetInvocationList().
                                       Cast<OnSendChargeDetailRecordRequestHandler>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.ChargeDetailRecord,
                                                     Request.RequestTimeout ?? RequestTimeout.Value))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CentralClient) + "." + nameof(OnSendChargeDetailRecordRequest));
            }

            #endregion


            using (var _OICPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    URIPrefix + AuthorizationURI,
                                                    RemoteCertificateValidator,
                                                    LocalCertificateSelector,
                                                    ClientCert,
                                                    UserAgent,
                                                    RequestTimeout,
                                                    DNSClient))
            {

                result = await _OICPClient.Query(_CustomSendChargeDetailRecordSOAPRequestMapper(Request,
                                                                                                SOAP.Encapsulation(Request.ToXML(CustomChargeDetailRecordSerializer: CustomChargeDetailRecordSerializer,
                                                                                                                                 CustomIdentificationSerializer:     CustomIdentificationSerializer))),
                                                 "eRoamingChargeDetailRecord",
                                                 RequestLogDelegate:   OnSendChargeDetailRecordSOAPRequest,
                                                 ResponseLogDelegate:  OnSendChargeDetailRecordSOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 RequestTimeout:         Request.RequestTimeout ?? RequestTimeout.Value,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                      (request, xml, onexception) =>
                                                                                                      Acknowledgement<CPO.SendChargeDetailRecordRequest>.Parse(request,
                                                                                                                                                               xml,
                                                                                                                                                               CustomSendChargeDetailRecordParser,
                                                                                                                                                               CustomStatusCodeParser,
                                                                                                                                                               onexception)),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     DebugX.Log("e:" + httpresponse.EntirePDU);

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<Acknowledgement<CPO.SendChargeDetailRecordRequest>>(

                                                                httpresponse,

                                                                new Acknowledgement<CPO.SendChargeDetailRecordRequest>(
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

                                                     DebugX.Log("e:" + httpresponse.EntirePDU);

                                                     SendHTTPError(timestamp, this, httpresponse);

                                                     return new HTTPResponse<Acknowledgement<CPO.SendChargeDetailRecordRequest>>(

                                                                httpresponse,

                                                                new Acknowledgement<CPO.SendChargeDetailRecordRequest>(
                                                                    Request,
                                                                    StatusCodes.DataError,
                                                                    httpresponse.HTTPStatusCode.ToString(),
                                                                    httpresponse.HTTPBody.ToUTF8String()
                                                                ),

                                                                IsFault: true

                                                            );

                                                 },

                                                 #endregion

                                                 #region OnException

                                                 OnException: (timestamp, sender, exception) => {

                                                     DebugX.Log("e:" + exception.Message);

                                                     SendException(timestamp, sender, exception);

                                                     return HTTPResponse<Acknowledgement<CPO.SendChargeDetailRecordRequest>>.ExceptionThrown(

                                                            new Acknowledgement<CPO.SendChargeDetailRecordRequest>(
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
                result = HTTPResponse<Acknowledgement<CPO.SendChargeDetailRecordRequest>>.ClientError(
                             new Acknowledgement<CPO.SendChargeDetailRecordRequest>(
                                 Request,
                                 StatusCodes.SystemError,
                                 "HTTP request failed!"
                             )
                         );


            #region Send OnSendChargeDetailRecordResponse event

            var Endtime = DateTime.Now;

            try
            {

                if (OnSendChargeDetailRecordResponse != null)
                    await Task.WhenAll(OnSendChargeDetailRecordResponse.GetInvocationList().
                                       Cast<OnSendChargeDetailRecordResponseHandler>().
                                       Select(e => e(Endtime,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.ChargeDetailRecord,
                                                     Request.RequestTimeout ?? RequestTimeout.Value,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CentralClient) + "." + nameof(OnSendChargeDetailRecordResponse));
            }

            #endregion

            return result;

        }

        #endregion


    }

}
