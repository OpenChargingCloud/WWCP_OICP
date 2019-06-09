/*
 * Copyright (c) 2014-2018 GraphDefined GmbH
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
using org.GraphDefined.Vanaheimr.Hermod.SOAP.v1_1;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.CPO
{

    /// <summary>
    /// An OICP CPO client.
    /// </summary>
    public partial class CPOClient : ASOAPClient,
                                     ICPOClient
    {

        #region Data

        /// <summary>
        /// The default HTTP user agent string.
        /// </summary>
        public new const           String   DefaultHTTPUserAgent            = "GraphDefined OICP " + Version.Number + " CPO Client";

        /// <summary>
        /// The default remote TCP port to connect to.
        /// </summary>
        public new static readonly IPPort   DefaultRemotePort               = IPPort.Parse(443);

        /// <summary>
        /// The default URI prefix.
        /// </summary>
        public     static readonly HTTPURI  DefaultURIPrefix                = HTTPURI.Parse("/ibis/ws");

        /// <summary>
        /// The default HTTP/SOAP/XML URI for OICP EvseData requests.
        /// </summary>
        public     const           String   DefaultEVSEDataURI              = "/eRoamingEvseData_V2.1";

        /// <summary>
        /// The default HTTP/SOAP/XML URI for OICP EvseStatus requests.
        /// </summary>
        public     const           String   DefaultEVSEStatusURI            = "/eRoamingEvseStatus_V2.0";

        /// <summary>
        /// The default HTTP/SOAP/XML URI for OICP Authorization requests.
        /// </summary>
        public     const           String   DefaultAuthorizationURI         = "/eRoamingAuthorization_V2.0";

        /// <summary>
        /// The default HTTP/SOAP/XML URI for OICP AuthenticationData requests.
        /// </summary>
        public     const           String   DefaultAuthenticationDataURI    = "/eRoamingAuthenticationData_V2.0";

        #endregion

        #region Properties

        /// <summary>
        /// The HTTP/SOAP/XML URI for OICP EvseData requests.
        /// </summary>
        public String           EVSEDataURI             { get; }

        /// <summary>
        /// The HTTP/SOAP/XML URI for OICP EvseStatus requests.
        /// </summary>
        public String           EVSEStatusURI           { get; }

        /// <summary>
        /// The HTTP/SOAP/XML URI for OICP Authorization requests.
        /// </summary>
        public String           AuthorizationURI        { get; }

        /// <summary>
        /// The HTTP/SOAP/XML URI for OICP AuthenticationData requests.
        /// </summary>
        public String           AuthenticationDataURI   { get; }

        /// <summary>
        /// The attached OICP CPO client (HTTP/SOAP client) logger.
        /// </summary>
        public CPOClientLogger  Logger                  { get; }

        #endregion

        #region Custom request mappers

        #region CustomPushEVSEDataRequestMapper

        #region CustomPushEVSEDataRequestMapper

        private Func<PushEVSEDataRequest, PushEVSEDataRequest> _CustomPushEVSEDataRequestMapper = _ => _;

        public Func<PushEVSEDataRequest, PushEVSEDataRequest> CustomPushEVSEDataRequestMapper
        {

            get
            {
                return _CustomPushEVSEDataRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomPushEVSEDataRequestMapper = value;
            }

        }

        #endregion

        #region CustomPushEVSEDataSOAPRequestMapper

        private Func<PushEVSEDataRequest, XElement, XElement> _CustomPushEVSEDataSOAPRequestMapper = (request, xml) => xml;

        public Func<PushEVSEDataRequest, XElement, XElement> CustomPushEVSEDataSOAPRequestMapper
        {

            get
            {
                return _CustomPushEVSEDataSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomPushEVSEDataSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<Acknowledgement<PushEVSEDataRequest>> CustomPushEVSEDataParser { get; set; }

        #endregion

        #region CustomPushEVSEStatusRequestMapper

        #region CustomPushEVSEStatusRequestMapper

        private Func<PushEVSEStatusRequest, PushEVSEStatusRequest> _CustomPushEVSEStatusRequestMapper = _ => _;

        public Func<PushEVSEStatusRequest, PushEVSEStatusRequest> CustomPushEVSEStatusRequestMapper
        {

            get
            {
                return _CustomPushEVSEStatusRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomPushEVSEStatusRequestMapper = value;
            }

        }

        #endregion

        #region CustomPushEVSEStatusSOAPRequestMapper

        private Func<PushEVSEStatusRequest, XElement, XElement> _CustomPushEVSEStatusSOAPRequestMapper = (request, xml) => xml;

        public Func<PushEVSEStatusRequest, XElement, XElement> CustomPushEVSEStatusSOAPRequestMapper
        {

            get
            {
                return _CustomPushEVSEStatusSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomPushEVSEStatusSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<Acknowledgement<PushEVSEStatusRequest>> CustomPushEVSEStatusParser   { get; set; }

        #endregion


        #region CustomAuthorizeStartRequestMapper

        #region CustomAuthorizeStartRequestMapper

        private Func<AuthorizeStartRequest, AuthorizeStartRequest> _CustomAuthorizeStartRequestMapper = _ => _;

        public Func<AuthorizeStartRequest, AuthorizeStartRequest> CustomAuthorizeStartRequestMapper
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

        private Func<AuthorizeStartRequest, XElement, XElement> _CustomAuthorizeStartSOAPRequestMapper = (request, xml) => xml;

        public Func<AuthorizeStartRequest, XElement, XElement> CustomAuthorizeStartSOAPRequestMapper
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

        public CustomXMLParserDelegate<AuthorizationStart>  CustomAuthorizationStartParser { get; set; }

        #endregion

        #region CustomAuthorizeStopRequestMapper

        #region CustomAuthorizeStopRequestMapper

        private Func<AuthorizeStopRequest, AuthorizeStopRequest> _CustomAuthorizeStopRequestMapper = _ => _;

        public Func<AuthorizeStopRequest, AuthorizeStopRequest> CustomAuthorizeStopRequestMapper
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

        private Func<AuthorizeStopRequest, XElement, XElement> _CustomAuthorizeStopSOAPRequestMapper = (request, xml) => xml;

        public Func<AuthorizeStopRequest, XElement, XElement> CustomAuthorizeStopSOAPRequestMapper
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

        public CustomXMLParserDelegate<AuthorizationStop>  CustomAuthorizationStopParser { get; set; }

        #endregion

        #region CustomSendChargeDetailRecordRequestMapper

        #region CustomSendChargeDetailRecordRequestMapper

        private Func<SendChargeDetailRecordRequest, SendChargeDetailRecordRequest> _CustomSendChargeDetailRecordRequestMapper = _ => _;

        public Func<SendChargeDetailRecordRequest, SendChargeDetailRecordRequest> CustomSendChargeDetailRecordRequestMapper
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

        private Func<SendChargeDetailRecordRequest, XElement, XElement> _CustomSendChargeDetailRecordSOAPRequestMapper = (request, xml) => xml;

        public Func<SendChargeDetailRecordRequest, XElement, XElement> CustomSendChargeDetailRecordSOAPRequestMapper
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

        public CustomXMLParserDelegate<Acknowledgement<SendChargeDetailRecordRequest>> CustomSendChargeDetailRecordParser   { get; set; }

        #endregion


        #region CustomPullAuthenticationDataRequestMapper

        #region CustomPullAuthenticationDataRequestMapper

        private Func<PullAuthenticationDataRequest, PullAuthenticationDataRequest> _CustomPullAuthenticationDataRequestMapper = _ => _;

        public Func<PullAuthenticationDataRequest, PullAuthenticationDataRequest> CustomPullAuthenticationDataRequestMapper
        {

            get
            {
                return _CustomPullAuthenticationDataRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomPullAuthenticationDataRequestMapper = value;
            }

        }

        #endregion

        #region CustomPullAuthenticationDataSOAPRequestMapper

        private Func<PullAuthenticationDataRequest, XElement, XElement> _CustomPullAuthenticationDataSOAPRequestMapper = (request, xml) => xml;

        public Func<PullAuthenticationDataRequest, XElement, XElement> CustomPullAuthenticationDataSOAPRequestMapper
        {

            get
            {
                return _CustomPullAuthenticationDataSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomPullAuthenticationDataSOAPRequestMapper = value;
            }

        }

        #endregion

        //public CustomMapperDelegate<Acknowledgement<PullAuthenticationDataRequest>, Acknowledgement<PullAuthenticationDataRequest>.Builder> CustomPullAuthenticationDataResponseMapper { get; set; }

        #endregion


        public CustomXMLSerializerDelegate<PushEVSEDataRequest>            CustomPushEVSEDataRequestSerializer              { get; set; }
        public CustomXMLSerializerDelegate<OperatorEVSEData>               CustomOperatorEVSEDataSerializer                 { get; set; }
        public CustomXMLSerializerDelegate<EVSEDataRecord>                 CustomEVSEDataRecordSerializer                   { get; set; }

        public CustomXMLSerializerDelegate<PushEVSEStatusRequest>          CustomPushEVSEStatusRequestSerializer            { get; set; }
        public CustomXMLSerializerDelegate<OperatorEVSEStatus>             CustomOperatorEVSEStatusSerializer               { get; set; }
        public CustomXMLSerializerDelegate<EVSEStatusRecord>               CustomEVSEStatusRecordSerializer                 { get; set; }


        public CustomXMLParserDelegate<AuthenticationData>                 CustomAuthenticationDataParser                   { get; set; }
        public CustomXMLParserDelegate<ProviderAuthenticationData>         CustomProviderAuthenticationDataParser           { get; set; }
        public CustomXMLParserDelegate<Identification>                     CustomAuthorizationIdentificationParser          { get; set; }

        public CustomXMLSerializerDelegate<AuthorizeStartRequest>          CustomAuthorizeStartRequestSerializer            { get; set; }
        public CustomXMLSerializerDelegate<AuthorizeStopRequest>           CustomAuthorizeStopRequestSerializer             { get; set; }
        public CustomXMLSerializerDelegate<ChargeDetailRecord>             CustomSendChargeDetailRecordRequestSerializer    { get; set; }

        public CustomXMLSerializerDelegate<PullAuthenticationDataRequest>  CustomPullAuthenticationDataRequestSerializer    { get; set; }


        public CustomXMLParserDelegate<Identification>                     CustomIdentificationParser                       { get; set; }
        public CustomXMLParserDelegate<StatusCode>                         CustomStatusCodeParser                           { get; set; }

        #endregion

        #region Events

        #region OnPushEVSEDataRequest/-Response

        /// <summary>
        /// An event fired whenever a request pushing EVSE data records will be send.
        /// </summary>
        public event OnPushEVSEDataRequestDelegate   OnPushEVSEDataRequest;

        /// <summary>
        /// An event fired whenever a SOAP request pushing EVSE data records will be send.
        /// </summary>
        public event ClientRequestLogHandler         OnPushEVSEDataSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a push EVSE data records SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler        OnPushEVSEDataSOAPResponse;

        /// <summary>
        /// An event fired whenever EVSE data records had been sent upstream.
        /// </summary>
        public event OnPushEVSEDataResponseDelegate  OnPushEVSEDataResponse;

        #endregion

        #region OnPushEVSEStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request pushing EVSE status records will be send.
        /// </summary>
        public event OnPushEVSEStatusRequestDelegate   OnPushEVSEStatusRequest;

        /// <summary>
        /// An event fired whenever a SOAP request pushing EVSE status records will be send.
        /// </summary>
        public event ClientRequestLogHandler           OnPushEVSEStatusSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a push EVSE status records SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler          OnPushEVSEStatusSOAPResponse;

        /// <summary>
        /// An event fired whenever EVSE status records had been sent upstream.
        /// </summary>
        public event OnPushEVSEStatusResponseDelegate  OnPushEVSEStatusResponse;

        #endregion


        #region OnAuthorizeStartRequest/-Response

        /// <summary>
        /// An event fired whenever an authorize start request will be send.
        /// </summary>
        public event OnAuthorizeStartRequestHandler    OnAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever an authorize start SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler    OnAuthorizeStartSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to an authorize start SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler   OnAuthorizeStartSOAPResponse;

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
        /// An event fired whenever a 'charge detail record' will be send.
        /// </summary>
        public event OnSendChargeDetailRecordRequestHandler   OnSendChargeDetailRecordRequest;

        /// <summary>
        /// An event fired whenever a 'charge detail record' will be send via SOAP.
        /// </summary>
        public event ClientRequestLogHandler                  OnSendChargeDetailRecordSOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response to a sent 'charge detail record' had been received.
        /// </summary>
        public event ClientResponseLogHandler                 OnSendChargeDetailRecordSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a sent 'charge detail record' had been received.
        /// </summary>
        public event OnSendChargeDetailRecordResponseHandler  OnSendChargeDetailRecordResponse;

        #endregion


        #region OnPullAuthenticationDataRequest/-Response

        /// <summary>
        /// An event fired whenever a 'pull authentication data' request will be send.
        /// </summary>
        public event OnPullAuthenticationDataRequestHandler   OnPullAuthenticationDataRequest;

        /// <summary>
        /// An event fired whenever a 'pull authentication data' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                  OnPullAuthenticationDataSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'pull authentication data' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                 OnPullAuthenticationDataSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'pull authentication data' request had been received.
        /// </summary>
        public event OnPullAuthenticationDataResponseHandler  OnPullAuthenticationDataResponse;

        #endregion

        #endregion

        #region Constructor(s)

        #region CPOClient(ClientId, Hostname, ..., LoggingContext = CPOClientLogger.DefaultContext, ...)

        /// <summary>
        /// Create a new OICP CPO Client.
        /// </summary>
        /// <param name="ClientId">A unqiue identification of this client.</param>
        /// <param name="Hostname">The hostname of the remote OICP service.</param>
        /// <param name="RemotePort">An optional TCP port of the remote OICP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCertificateSelector">A delegate to select a TLS client certificate.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual hostname of the remote OICP service.</param>
        /// <param name="URIPrefix">An default URI prefix.</param>
        /// <param name="EVSEDataURI">The HTTP/SOAP/XML URI for OICP EvseData requests.</param>
        /// <param name="AuthorizationStartURI">The HTTP/SOAP/XML URI for OICP EvseStatus requests.</param>
        /// <param name="AuthorizationURI">The HTTP/SOAP/XML URI for OICP Authorization requests.</param>
        /// <param name="AuthenticationDataURI">The HTTP/SOAP/XML URI for OICP AuthenticationData requests.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// <param name="MaxNumberOfRetries">The default number of maximum transmission retries.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="LoggingContext">An optional context for logging client methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        public CPOClient(String                               ClientId,
                         HTTPHostname                         Hostname,
                         IPPort?                              RemotePort                   = null,
                         RemoteCertificateValidationCallback  RemoteCertificateValidator   = null,
                         LocalCertificateSelectionCallback    ClientCertificateSelector    = null,
                         HTTPHostname?                        HTTPVirtualHost              = null,
                         HTTPURI?                             URIPrefix                    = null,
                         String                               EVSEDataURI                  = DefaultEVSEDataURI,
                         String                               EVSEStatusURI                = DefaultEVSEStatusURI,
                         String                               AuthorizationURI             = DefaultAuthorizationURI,
                         String                               AuthenticationDataURI        = DefaultAuthenticationDataURI,
                         String                               HTTPUserAgent                = DefaultHTTPUserAgent,
                         TimeSpan?                            RequestTimeout               = null,
                         Byte?                                MaxNumberOfRetries           = DefaultMaxNumberOfRetries,
                         DNSClient                            DNSClient                    = null,
                         String                               LoggingContext               = CPOClientLogger.DefaultContext,
                         LogfileCreatorDelegate               LogfileCreator               = null)

            : base(ClientId,
                   Hostname,
                   RemotePort ?? DefaultRemotePort,
                   RemoteCertificateValidator,
                   ClientCertificateSelector,
                   HTTPVirtualHost,
                   URIPrefix ?? DefaultURIPrefix,
                   null,
                   HTTPUserAgent,
                   RequestTimeout,
                   MaxNumberOfRetries,
                   DNSClient)

        {

            #region Initial checks

            if (ClientId.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(ClientId), "The given client identification must not be null or empty!");

            #endregion

            this.EVSEDataURI            = EVSEDataURI           ?? DefaultEVSEDataURI;
            this.EVSEStatusURI          = EVSEStatusURI         ?? DefaultEVSEStatusURI;
            this.AuthorizationURI       = AuthorizationURI      ?? DefaultAuthorizationURI;
            this.AuthenticationDataURI  = AuthenticationDataURI ?? DefaultAuthenticationDataURI;

            this.Logger                 = new CPOClientLogger(this,
                                                              LoggingContext,
                                                              LogfileCreator);

        }

        #endregion

        #region CPOClient(ClientId, Logger, Hostname, ...)

        /// <summary>
        /// Create a new OICP CPO Client.
        /// </summary>
        /// <param name="ClientId">A unqiue identification of this client.</param>
        /// <param name="Hostname">The hostname of the remote OICP service.</param>
        /// <param name="RemotePort">An optional TCP port of the remote OICP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCertificateSelector">A delegate to select a TLS client certificate.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual hostname of the remote OICP service.</param>
        /// <param name="URIPrefix">An default URI prefix.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// <param name="MaxNumberOfRetries">The default number of maximum transmission retries.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public CPOClient(String                               ClientId,
                         CPOClientLogger                      Logger,
                         HTTPHostname                         Hostname,
                         IPPort?                              RemotePort                   = null,
                         RemoteCertificateValidationCallback  RemoteCertificateValidator   = null,
                         LocalCertificateSelectionCallback    ClientCertificateSelector    = null,
                         HTTPHostname?                        HTTPVirtualHost              = null,
                         HTTPURI?                             URIPrefix                    = null,
                         String                               EVSEDataURI                  = DefaultEVSEDataURI,
                         String                               EVSEStatusURI                = DefaultEVSEStatusURI,
                         String                               AuthorizationURI             = DefaultAuthorizationURI,
                         String                               AuthenticationDataURI        = DefaultAuthenticationDataURI,
                         String                               HTTPUserAgent                = DefaultHTTPUserAgent,
                         TimeSpan?                            RequestTimeout               = null,
                         Byte?                                MaxNumberOfRetries           = DefaultMaxNumberOfRetries,
                         DNSClient                            DNSClient                    = null)

            : base(ClientId,
                   Hostname,
                   RemotePort ?? DefaultRemotePort,
                   RemoteCertificateValidator,
                   ClientCertificateSelector,
                   HTTPVirtualHost,
                   URIPrefix ?? DefaultURIPrefix,
                   null,
                   HTTPUserAgent,
                   RequestTimeout,
                   MaxNumberOfRetries,
                   DNSClient)

        {

            #region Initial checks

            if (ClientId.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(ClientId), "The given client identification must not be null or empty!");

            #endregion

            this.EVSEDataURI            = EVSEDataURI           ?? DefaultEVSEDataURI;
            this.EVSEStatusURI          = EVSEStatusURI         ?? DefaultEVSEStatusURI;
            this.AuthorizationURI       = AuthorizationURI      ?? DefaultAuthorizationURI;
            this.AuthenticationDataURI  = AuthenticationDataURI ?? DefaultAuthenticationDataURI;

            this.Logger                 = Logger                ?? throw new ArgumentNullException(nameof(Logger), "The given mobile client logger must not be null!");

        }

        #endregion

        #endregion


        #region PushEVSEData  (Request)

        /// <summary>
        /// Upload the given EVSE data records.
        /// </summary>
        /// <param name="Request">A PushEVSEData request.</param>
        public async Task<HTTPResponse<Acknowledgement<PushEVSEDataRequest>>>

            PushEVSEData(PushEVSEDataRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given PushEVSEData request must not be null!");

            Request = _CustomPushEVSEDataRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped PushEVSEData request must not be null!");


            Byte                                               TransmissionRetry  = 0;
            HTTPResponse<Acknowledgement<PushEVSEDataRequest>> result             = null;

            #endregion

            #region Send OnPushEVSEDataRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                if (OnPushEVSEDataRequest != null)
                    await Task.WhenAll(OnPushEVSEDataRequest.GetInvocationList().
                                       Cast<OnPushEVSEDataRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.Action,
                                                     Request.EVSEDataRecords.ULongCount(),
                                                     Request.EVSEDataRecords,
                                                     Request.RequestTimeout ?? RequestTimeout.Value))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnPushEVSEDataRequest));
            }

            #endregion


            // Apply EVSE filter!

            #region No EVSE data to push?

            if (!Request.EVSEDataRecords.Any())
            {

                result = HTTPResponse<Acknowledgement<PushEVSEDataRequest>>.OK(
                             Acknowledgement<PushEVSEDataRequest>.Success(Request,
                                                                          StatusCodeDescription: "No EVSE data to push")
                         );

            }

            #endregion

            else do
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        URIPrefix + EVSEDataURI,
                                                        VirtualHostname,
                                                        RemotePort,
                                                        RemoteCertificateValidator,
                                                        ClientCertificateSelector,
                                                        UserAgent,
                                                        RequestTimeout,
                                                        DNSClient))
                {

                    result = await _OICPClient.Query(_CustomPushEVSEDataSOAPRequestMapper(Request,
                                                                                          SOAP.Encapsulation(Request.ToXML(CustomPushEVSEDataRequestSerializer: CustomPushEVSEDataRequestSerializer,
                                                                                                                           CustomOperatorEVSEDataSerializer:    CustomOperatorEVSEDataSerializer,
                                                                                                                           CustomEVSEDataRecordSerializer:      CustomEVSEDataRecordSerializer))),
                                                     "eRoamingPushEvseData",
                                                     RequestLogDelegate:   OnPushEVSEDataSOAPRequest,
                                                     ResponseLogDelegate:  OnPushEVSEDataSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout.Value,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, onexception) =>
                                                                                                          Acknowledgement<PushEVSEDataRequest>.Parse(request,
                                                                                                                                                     xml,
                                                                                                                                                     CustomPushEVSEDataParser,
                                                                                                                                                     CustomStatusCodeParser,
                                                                                                                                                     onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return new HTTPResponse<Acknowledgement<PushEVSEDataRequest>>(

                                                                    httpresponse,

                                                                    new Acknowledgement<PushEVSEDataRequest>(
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

                                                         SendHTTPError(timestamp, this, httpresponse);

                                                         if (httpresponse.HTTPStatusCode == HTTPStatusCode.ServiceUnavailable ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Unauthorized       ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Forbidden          ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                                                         {

                                                             return new HTTPResponse<Acknowledgement<PushEVSEDataRequest>>(

                                                                 httpresponse,

                                                                 new Acknowledgement<PushEVSEDataRequest>(
                                                                     Request,
                                                                     StatusCodes.ServiceNotAvailable,
                                                                     httpresponse.HTTPStatusCode.ToString(),
                                                                     httpresponse.HTTPBody.      ToUTF8String()
                                                                 ),

                                                                 IsFault: true);

                                                         }

                                                         return new HTTPResponse<Acknowledgement<PushEVSEDataRequest>>(

                                                                    httpresponse,

                                                                    new Acknowledgement<PushEVSEDataRequest>(
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

                                                         return HTTPResponse<Acknowledgement<PushEVSEDataRequest>>.ExceptionThrown(

                                                                new Acknowledgement<PushEVSEDataRequest>(
                                                                    Request,
                                                                    StatusCodes.ServiceNotAvailable,
                                                                    exception.Message,
                                                                    exception.StackTrace
                                                                ),

                                                                Exception: exception

                                                            );

                                                     }

                                                     #endregion

                                                    );

                }

                if (result == null)
                    result = HTTPResponse<Acknowledgement<PushEVSEDataRequest>>.ClientError(
                                 new Acknowledgement<PushEVSEDataRequest>(
                                     Request,
                                     StatusCodes.SystemError,
                                     "HTTP request failed!"
                                 )
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnPushEVSEDataResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                if (OnPushEVSEDataResponse != null)
                    await Task.WhenAll(OnPushEVSEDataResponse.GetInvocationList().
                                       Cast<OnPushEVSEDataResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.Action,
                                                     Request.EVSEDataRecords.ULongCount(),
                                                     Request.EVSEDataRecords,
                                                     Request.RequestTimeout ?? RequestTimeout.Value,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnPushEVSEDataResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region PushEVSEStatus(Request)

        /// <summary>
        /// Upload the given EVSE status records.
        /// </summary>
        /// <param name="Request">A PushEVSEStatus request.</param>
        public async Task<HTTPResponse<Acknowledgement<PushEVSEStatusRequest>>>

            PushEVSEStatus(PushEVSEStatusRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given PushEVSEStatus request must not be null!");

            Request = _CustomPushEVSEStatusRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped PushEVSEStatus request must not be null!");


            Byte                                                 TransmissionRetry  = 0;
            HTTPResponse<Acknowledgement<PushEVSEStatusRequest>> result             = null;

            #endregion

            #region Send OnPushEVSEStatusRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                if (OnPushEVSEStatusRequest != null)
                    await Task.WhenAll(OnPushEVSEStatusRequest.GetInvocationList().
                                       Cast<OnPushEVSEStatusRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.Action,
                                                     Request.EVSEStatusRecords.ULongCount(),
                                                     Request.EVSEStatusRecords,
                                                     Request.RequestTimeout ?? RequestTimeout.Value))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnPushEVSEStatusRequest));
            }

            #endregion


            #region No EVSE status to push?

            if (!Request.EVSEStatusRecords.Any())
            {

                result = HTTPResponse<Acknowledgement<PushEVSEStatusRequest>>.OK(
                             Acknowledgement<PushEVSEStatusRequest>.Success(Request,
                                                                            StatusCodeDescription: "No EVSE status to push")
                         );

            }

            #endregion

            else do
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        URIPrefix + EVSEStatusURI,
                                                        VirtualHostname,
                                                        RemotePort,
                                                        RemoteCertificateValidator,
                                                        ClientCertificateSelector,
                                                        UserAgent,
                                                        RequestTimeout,
                                                        DNSClient))
                {

                    result = await _OICPClient.Query(_CustomPushEVSEStatusSOAPRequestMapper(Request,
                                                                                            SOAP.Encapsulation(Request.ToXML(CustomPushEVSEStatusRequestSerializer: CustomPushEVSEStatusRequestSerializer,
                                                                                                                             CustomOperatorEVSEStatusSerializer:    CustomOperatorEVSEStatusSerializer,
                                                                                                                             CustomEVSEStatusRecordSerializer:      CustomEVSEStatusRecordSerializer))),
                                                     "eRoamingPushEvseStatus",
                                                     RequestLogDelegate:   OnPushEVSEStatusSOAPRequest,
                                                     ResponseLogDelegate:  OnPushEVSEStatusSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout.Value,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, onexception) =>
                                                                                                          Acknowledgement<PushEVSEStatusRequest>.Parse(request,
                                                                                                                                                       xml,
                                                                                                                                                       CustomPushEVSEStatusParser,
                                                                                                                                                       CustomStatusCodeParser,
                                                                                                                                                       onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return new HTTPResponse<Acknowledgement<PushEVSEStatusRequest>>(

                                                                    httpresponse,

                                                                    new Acknowledgement<PushEVSEStatusRequest>(
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

                                                         SendHTTPError(timestamp, this, httpresponse);


                                                         if (httpresponse.HTTPStatusCode == HTTPStatusCode.ServiceUnavailable ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Unauthorized       ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Forbidden          ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.NotFound)

                                                             return new HTTPResponse<Acknowledgement<PushEVSEStatusRequest>>(httpresponse,
                                                                                                                             new Acknowledgement<PushEVSEStatusRequest>(
                                                                                                                                 Request,
                                                                                                                                 StatusCodes.ServiceNotAvailable,
                                                                                                                                 httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                 httpresponse.HTTPBody.      ToUTF8String()
                                                                                                                             ),
                                                                                                                             IsFault: true);


                                                         return new HTTPResponse<Acknowledgement<PushEVSEStatusRequest>>(

                                                                    httpresponse,

                                                                    new Acknowledgement<PushEVSEStatusRequest>(
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

                                                         return HTTPResponse<Acknowledgement<PushEVSEStatusRequest>>.ExceptionThrown(

                                                                new Acknowledgement<PushEVSEStatusRequest>(
                                                                    Request,
                                                                    StatusCodes.ServiceNotAvailable,
                                                                    exception.Message,
                                                                    exception.StackTrace
                                                                ),

                                                                Exception: exception

                                                            );

                                                     }

                                                     #endregion

                                                    );

                }

                if (result == null)
                    result = HTTPResponse<Acknowledgement<PushEVSEStatusRequest>>.OK(
                                 new Acknowledgement<PushEVSEStatusRequest>(
                                     Request,
                                     StatusCodes.SystemError,
                                     "HTTP request failed!"
                                 )
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnPushEVSEStatusResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                if (OnPushEVSEStatusResponse != null)
                    await Task.WhenAll(OnPushEVSEStatusResponse.GetInvocationList().
                                       Cast<OnPushEVSEStatusResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.Action,
                                                     Request.EVSEStatusRecords.ULongCount(),
                                                     Request.EVSEStatusRecords,
                                                     Request.RequestTimeout ?? RequestTimeout.Value,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnPushEVSEStatusResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region AuthorizeStart        (Request)

        /// <summary>
        /// Create an OICP authorize start request.
        /// </summary>
        /// <param name="Request">A AuthorizeStart request.</param>
        public async Task<HTTPResponse<AuthorizationStart>>

            AuthorizeStart(AuthorizeStartRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given AuthorizeStart request must not be null!");

            Request = _CustomAuthorizeStartRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped AuthorizeStart request must not be null!");


            Byte                             TransmissionRetry  = 0;
            HTTPResponse<AuthorizationStart> result             = null;

            #endregion

            #region Send OnAuthorizeStartRequest event

            var StartTime = DateTime.UtcNow;

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
                                                     Request.SessionId,
                                                     Request.PartnerProductId,
                                                     Request.PartnerSessionId,
                                                     Request.RequestTimeout ?? RequestTimeout.Value))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnAuthorizeStartRequest));
            }

            #endregion


            DebugX.LogT("OICP.CPOClient AuthStart Request: " + Request.Identification);

            do
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        URIPrefix + AuthorizationURI,
                                                        VirtualHostname,
                                                        RemotePort,
                                                        RemoteCertificateValidator,
                                                        ClientCertificateSelector,
                                                        UserAgent,
                                                        RequestTimeout,
                                                        DNSClient))
                {

                    result = await _OICPClient.Query(_CustomAuthorizeStartSOAPRequestMapper(Request,
                                                                                            SOAP.Encapsulation(Request.ToXML(CustomAuthorizeStartRequestSerializer))),
                                                     "eRoamingAuthorizeStart",
                                                     RequestLogDelegate:   OnAuthorizeStartSOAPRequest,
                                                     ResponseLogDelegate:  OnAuthorizeStartSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout.Value,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, onexception) => AuthorizationStart.Parse(request,
                                                                                                                                                                  xml,
                                                                                                                                                                  CustomAuthorizationStartParser,
                                                                                                                                                                  CustomIdentificationParser,
                                                                                                                                                                  CustomStatusCodeParser,
                                                                                                                                                                  onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return new HTTPResponse<AuthorizationStart>(httpresponse,
                                                                                                     AuthorizationStart.DataError(
                                                                                                         Request,
                                                                                                         httpresponse.Content.ToString()
                                                                                                     ),
                                                                                                     IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, this, httpresponse);


                                                         if (httpresponse.HTTPStatusCode == HTTPStatusCode.ServiceUnavailable ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Unauthorized       ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Forbidden          ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.NotFound)

                                                             return new HTTPResponse<AuthorizationStart>(httpresponse,
                                                                                                         AuthorizationStart.ServiceNotAvailable(
                                                                                                             Request,
                                                                                                             httpresponse.HTTPStatusCode.ToString(),
                                                                                                             httpresponse.HTTPBody.      ToUTF8String()
                                                                                                         ),
                                                                                                         IsFault: true);


                                                         return new HTTPResponse<AuthorizationStart>(httpresponse,
                                                                                                     AuthorizationStart.DataError(
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

                                                         return HTTPResponse<AuthorizationStart>.ExceptionThrown(AuthorizationStart.SystemError(
                                                                                                                     Request,
                                                                                                                     exception.Message,
                                                                                                                     exception.StackTrace
                                                                                                                 ),
                                                                                                                 Exception: exception);

                                                     }

                                                     #endregion

                                                    );

                }

                if (result == null)
                    result = HTTPResponse<AuthorizationStart>.OK(
                                 AuthorizationStart.SystemError(
                                     Request,
                                     "HTTP request failed!",
                                     null,
                                     Request?.SessionId,
                                     Request?.PartnerSessionId
                                 )
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                TransmissionRetry++ < MaxNumberOfRetries);

            DebugX.LogT("OICP.CPOClient AuthStart Response: " + Request.Identification + " => " + result.Content);


            #region Send OnAuthorizeStartResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                if (OnAuthorizeStartResponse != null)
                    await Task.WhenAll(OnAuthorizeStartResponse.GetInvocationList().
                                       Cast<OnAuthorizeStartResponseHandler>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.OperatorId,
                                                     Request.Identification,
                                                     Request.EVSEId,
                                                     Request.SessionId,
                                                     Request.PartnerProductId,
                                                     Request.PartnerSessionId,
                                                     Request.RequestTimeout ?? RequestTimeout.Value,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnAuthorizeStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop         (Request)

        /// <summary>
        /// Create an OICP authorize stop request.
        /// </summary>
        /// <param name="Request">A AuthorizeStart request.</param>
        public async Task<HTTPResponse<AuthorizationStop>>

            AuthorizeStop(AuthorizeStopRequest  Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given AuthorizeStop request must not be null!");

            Request = _CustomAuthorizeStopRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped AuthorizeStop request must not be null!");


            Byte                             TransmissionRetry  = 0;
            HTTPResponse<AuthorizationStop>  result             = null;

            #endregion

            #region Send OnAuthorizeStopRequest event

            var StartTime = DateTime.UtcNow;

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
                                                     Request.Identification,
                                                     Request.EVSEId,
                                                     Request.PartnerSessionId,
                                                     Request.RequestTimeout ?? RequestTimeout.Value))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnAuthorizeStopRequest));
            }

            #endregion


            do
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        URIPrefix + AuthorizationURI,
                                                        VirtualHostname,
                                                        RemotePort,
                                                        RemoteCertificateValidator,
                                                        ClientCertificateSelector,
                                                        UserAgent,
                                                        RequestTimeout,
                                                        DNSClient))
                {

                    result = await _OICPClient.Query(_CustomAuthorizeStopSOAPRequestMapper(Request,
                                                                                           SOAP.Encapsulation(Request.ToXML(CustomAuthorizeStopRequestSerializer))),
                                                     "eRoamingAuthorizeStop",
                                                     RequestLogDelegate:   OnAuthorizeStopSOAPRequest,
                                                     ResponseLogDelegate:  OnAuthorizeStopSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout.Value,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, onexception) => AuthorizationStop.Parse(request,
                                                                                                                                                                 xml,
                                                                                                                                                                 CustomAuthorizationStopParser,
                                                                                                                                                                 CustomStatusCodeParser,
                                                                                                                                                                 onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return new HTTPResponse<AuthorizationStop>(httpresponse,
                                                                                                    AuthorizationStop.DataError(Request,
                                                                                                                                httpresponse.Content.ToString()),
                                                                                                    IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, this, httpresponse);


                                                         if (httpresponse.HTTPStatusCode == HTTPStatusCode.ServiceUnavailable ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Unauthorized       ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Forbidden          ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.NotFound)

                                                             return new HTTPResponse<AuthorizationStop>(httpresponse,
                                                                                                        AuthorizationStop.ServiceNotAvailable(
                                                                                                            Request,
                                                                                                            httpresponse.HTTPStatusCode.ToString(),
                                                                                                            httpresponse.HTTPBody.      ToUTF8String()
                                                                                                        ),
                                                                                                        IsFault: true);


                                                         return new HTTPResponse<AuthorizationStop>(httpresponse,
                                                                                                    AuthorizationStop.DataError(
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

                                                         return HTTPResponse<AuthorizationStop>.ExceptionThrown(AuthorizationStop.SystemError(
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
                    result = HTTPResponse<AuthorizationStop>.OK(
                                 AuthorizationStop.SystemError(
                                     Request,
                                     "HTTP request failed!",
                                     null,
                                     Request?.SessionId,
                                     Request?.PartnerSessionId
                                 )
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnAuthorizeStopResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                if (OnAuthorizeStopResponse != null)
                    await Task.WhenAll(OnAuthorizeStopResponse.GetInvocationList().
                                       Cast<OnAuthorizeStopResponseHandler>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.OperatorId,
                                                     Request.SessionId,
                                                     Request.Identification,
                                                     Request.EVSEId,
                                                     Request.PartnerSessionId,
                                                     Request.RequestTimeout ?? RequestTimeout.Value,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnAuthorizeStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region SendChargeDetailRecord(Request)

        /// <summary>
        /// Send a charge detail record to an OICP server.
        /// </summary>
        /// <param name="Request">A SendChargeDetailRecord request.</param>
        public async Task<HTTPResponse<Acknowledgement<SendChargeDetailRecordRequest>>>

            SendChargeDetailRecord(SendChargeDetailRecordRequest  Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given SendChargeDetailRecord request must not be null!");

            Request = _CustomSendChargeDetailRecordRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped SendChargeDetailRecord request must not be null!");


            Byte                                                         TransmissionRetry  = 0;
            HTTPResponse<Acknowledgement<SendChargeDetailRecordRequest>> result             = null;

            #endregion

            #region Send OnSendChargeDetailRecord event

            var StartTime = DateTime.UtcNow;

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
                e.Log(nameof(CPOClient) + "." + nameof(OnSendChargeDetailRecordRequest));
            }

            #endregion


            do
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        URIPrefix + AuthorizationURI,
                                                        VirtualHostname,
                                                        RemotePort,
                                                        RemoteCertificateValidator,
                                                        ClientCertificateSelector,
                                                        UserAgent,
                                                        RequestTimeout,
                                                        DNSClient))
                {

                    result = await _OICPClient.Query(_CustomSendChargeDetailRecordSOAPRequestMapper(Request,
                                                                                                    SOAP.Encapsulation(Request.ToXML(CustomChargeDetailRecordSerializer: CustomSendChargeDetailRecordRequestSerializer))),
                                                     "eRoamingChargeDetailRecord",
                                                     RequestLogDelegate:   OnSendChargeDetailRecordSOAPRequest,
                                                     ResponseLogDelegate:  OnSendChargeDetailRecordSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout.Value,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, onexception) =>
                                                                                                          Acknowledgement<SendChargeDetailRecordRequest>.Parse(request,
                                                                                                                                                               xml,
                                                                                                                                                               CustomSendChargeDetailRecordParser,
                                                                                                                                                               CustomStatusCodeParser,
                                                                                                                                                               onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         DebugX.Log("e:" + httpresponse.EntirePDU);

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return new HTTPResponse<Acknowledgement<SendChargeDetailRecordRequest>>(

                                                                    httpresponse,

                                                                    new Acknowledgement<SendChargeDetailRecordRequest>(
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


                                                         if (httpresponse.HTTPStatusCode == HTTPStatusCode.ServiceUnavailable ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Unauthorized ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Forbidden ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.NotFound)

                                                             return new HTTPResponse<Acknowledgement<SendChargeDetailRecordRequest>>(httpresponse,
                                                                                                        new Acknowledgement<SendChargeDetailRecordRequest>(
                                                                                                            Request,
                                                                                                            StatusCodes.ServiceNotAvailable,
                                                                                                            httpresponse.HTTPStatusCode.ToString(),
                                                                                                            httpresponse.HTTPBody.ToUTF8String()
                                                                                                        ),
                                                                                                        IsFault: true);


                                                         return new HTTPResponse<Acknowledgement<SendChargeDetailRecordRequest>>(

                                                                    httpresponse,

                                                                    new Acknowledgement<SendChargeDetailRecordRequest>(
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

                                                         return HTTPResponse<Acknowledgement<SendChargeDetailRecordRequest>>.ExceptionThrown(

                                                                new Acknowledgement<SendChargeDetailRecordRequest>(
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
                    result = HTTPResponse<Acknowledgement<SendChargeDetailRecordRequest>>.OK(
                                 new Acknowledgement<SendChargeDetailRecordRequest>(
                                     Request,
                                     StatusCodes.SystemError,
                                     "HTTP request failed!"
                                 )
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnChargeDetailRecordSent event

            var Endtime = DateTime.UtcNow;

            try
            {

                if (OnSendChargeDetailRecordResponse != null)
                    await Task.WhenAll(OnSendChargeDetailRecordResponse.GetInvocationList().
                                       Cast<OnSendChargeDetailRecordResponseHandler>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
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
                e.Log(nameof(CPOClient) + "." + nameof(OnSendChargeDetailRecordResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region PullAuthenticationData(Request)

        /// <summary>
        /// Pull authentication data from the OICP server.
        /// </summary>
        /// <param name="Request">A PullAuthenticationData request.</param>
        public async Task<HTTPResponse<AuthenticationData>>

            PullAuthenticationData(PullAuthenticationDataRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given PullAuthenticationData request must not be null!");

            Request = _CustomPullAuthenticationDataRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped PullAuthenticationData request must not be null!");


            Byte                              TransmissionRetry  = 0;
            HTTPResponse<AuthenticationData>  result             = null;

            #endregion

            #region Send OnPullAuthenticationData event

            var StartTime = DateTime.UtcNow;

            try
            {

                if (OnPullAuthenticationDataRequest != null)
                    await Task.WhenAll(OnPullAuthenticationDataRequest.GetInvocationList().
                                       Cast<OnPullAuthenticationDataRequestHandler>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.OperatorId,
                                                     Request.RequestTimeout ?? RequestTimeout.Value))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnPullAuthenticationDataRequest));
            }

            #endregion


            do
            {

                using (var _OICPClient = new SOAPClient(Hostname,
                                                        URIPrefix + AuthenticationDataURI,
                                                        VirtualHostname,
                                                        RemotePort,
                                                        RemoteCertificateValidator,
                                                        ClientCertificateSelector,
                                                        UserAgent,
                                                        RequestTimeout,
                                                        DNSClient))
                {

                    result = await _OICPClient.Query(_CustomPullAuthenticationDataSOAPRequestMapper(Request,
                                                                                                    SOAP.Encapsulation(Request.ToXML(CustomPullAuthenticationDataRequestSerializer))),
                                                     "eRoamingPullAuthenticationData",
                                                     RequestLogDelegate:   OnPullAuthenticationDataSOAPRequest,
                                                     ResponseLogDelegate:  OnPullAuthenticationDataSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout.Value,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, onexception) =>
                                                                                                          AuthenticationData.Parse(request,
                                                                                                                                   xml,
                                                                                                                                   CustomAuthenticationDataParser,
                                                                                                                                   CustomProviderAuthenticationDataParser,
                                                                                                                                   CustomAuthorizationIdentificationParser,
                                                                                                                                   CustomStatusCodeParser,
                                                                                                                                   onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return new HTTPResponse<AuthenticationData>(httpresponse,
                                                                                                     IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, this, httpresponse);


                                                         if (httpresponse.HTTPStatusCode == HTTPStatusCode.ServiceUnavailable ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Unauthorized       ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Forbidden          ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.NotFound)

                                                             return new HTTPResponse<AuthenticationData>(httpresponse,
                                                                                                         new AuthenticationData(
                                                                                                             Request,
                                                                                                             StatusCodes.ServiceNotAvailable,
                                                                                                             httpresponse.HTTPStatusCode.ToString(),
                                                                                                             httpresponse.HTTPBody.      ToUTF8String()
                                                                                                         ),
                                                                                                         IsFault: true);


                                                         return new HTTPResponse<AuthenticationData>(httpresponse,
                                                                                                     new AuthenticationData(
                                                                                                         Request,
                                                                                                         StatusCodes.DataError,
                                                                                                         httpresponse.HTTPStatusCode.ToString(),
                                                                                                         httpresponse.HTTPBody.ToUTF8String()
                                                                                                     ),
                                                                                                     IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return HTTPResponse<AuthenticationData>.ExceptionThrown(new AuthenticationData(Request,
                                                                                                                                        StatusCodes.SystemError,
                                                                                                                                        exception.Message,
                                                                                                                                        exception.StackTrace),
                                                                                                                 Exception: exception);

                                                     }

                                                     #endregion

                                                    ).ConfigureAwait(false);

                }

                if (result == null)
                    result = HTTPResponse<AuthenticationData>.OK(
                                 new AuthenticationData(
                                     Request,
                                     StatusCodes.SystemError,
                                     "HTTP request failed!"
                                 )
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnAuthenticationDataPulled event

            var Endtime = DateTime.UtcNow;

            try
            {

                if (OnPullAuthenticationDataResponse != null)
                    await Task.WhenAll(OnPullAuthenticationDataResponse.GetInvocationList().
                                       Cast<OnPullAuthenticationDataResponseHandler>().
                                       Select(e => e(Endtime,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.OperatorId,
                                                     Request.RequestTimeout ?? RequestTimeout.Value,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnPullAuthenticationDataResponse));
            }

            #endregion

            return result;

        }

        #endregion

    }

}
