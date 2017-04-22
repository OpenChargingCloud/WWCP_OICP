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
        public new const           String  DefaultHTTPUserAgent  = "GraphDefined OICP " + Version.Number + " Central Client";

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
        /// The attached OICP Central client (HTTP/SOAP client) logger.
        /// </summary>
        public CentralClientLogger Logger { get; }

        #endregion

        #region Custom request mappers

        // Towards CPOs

        #region CustomAuthorizeRemoteReservationStart(SOAP)RequestMapper

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

        public CustomMapperDelegate<Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>, Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>.Builder> CustomAuthorizeRemoteReservationStartResponseMapper  { get; set; }

        #endregion

        #region CustomAuthorizeRemoteReservationStop(SOAP)RequestMapper

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

        public CustomMapperDelegate<Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>, Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>.Builder> CustomAuthorizeRemoteReservationStopResponseMapper  { get; set; }

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

        public CustomMapperDelegate<CPO.AuthorizationStart, CPO.AuthorizationStart.Builder> CustomAuthorizeStartResponseMapper { get; set; }

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

        public CustomMapperDelegate<CPO.AuthorizationStop, CPO.AuthorizationStop.Builder> CustomAuthorizeStopResponseMapper { get; set; }

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

        public CustomMapperDelegate<Acknowledgement<CPO.SendChargeDetailRecordRequest>, Acknowledgement<CPO.SendChargeDetailRecordRequest>.Builder> CustomSendChargeDetailRecordResponseMapper  { get; set; }

        #endregion

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
        /// An event fired whenever a 'reservation stop' request will be send.
        /// </summary>
        public event OnAuthorizeRemoteReservationStopRequestHandler   OnAuthorizeRemoteReservationStopRequest;

        /// <summary>
        /// An event fired whenever a 'reservation stop' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                          OnAuthorizeRemoteReservationStopSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'reservation stop' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                         OnAuthorizeRemoteReservationStopSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'reservation stop' request had been received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStopResponseHandler  OnAuthorizeRemoteReservationStopResponse;

        #endregion


        // Towards EMPs

        #region OnAuthorizeStartRequest/-Response

        /// <summary>
        /// An event fired whenever an authorize start request will be send.
        /// </summary>
        public event OnAuthorizeStartHandler    OnAuthorizeStartRequest;

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
        public event OnAuthorizeStartedHandler  OnAuthorizeStartResponse;

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
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public CentralClient(String                               ClientId,
                             String                               Hostname,
                             IPPort                               RemotePort                  = null,
                             RemoteCertificateValidationCallback  RemoteCertificateValidator  = null,
                             X509Certificate                      ClientCert                  = null,
                             String                               HTTPVirtualHost             = null,
                             String                               URIPrefix                   = DefaultURIPrefix,
                             String                               HTTPUserAgent               = DefaultHTTPUserAgent,
                             TimeSpan?                            RequestTimeout              = null,
                             DNSClient                            DNSClient                   = null,
                             String                               LoggingContext              = CentralClientLogger.DefaultContext,
                             LogfileCreatorDelegate               LogfileCreator              = null)

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
                   DNSClient) //"/ibis/ws/eRoamingAuthorization_V2.0",

        {

            #region Initial checks

            if (ClientId.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Logger),    "The given client identification must not be null or empty!");

            if (Hostname.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Hostname),  "The given hostname must not be null or empty!");

            #endregion

            this.Logger = new CentralClientLogger(this,
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
        /// <param name="DNSClient">An optional DNS client.</param>
        public CentralClient(String                               ClientId,
                             CentralClientLogger                  Logger,
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


        // Towards CPOs

        #region AuthorizeRemoteReservationStart(Request)

        /// <summary>
        /// Create an authorize remote start request.
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

                OnAuthorizeRemoteReservationStartRequest?.Invoke(StartTime,
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
                                                                 Request.RequestTimeout.HasValue ? Request.RequestTimeout.Value : RequestTimeout.Value);

            }
            catch (Exception e)
            {
                e.Log(nameof(CentralClient) + "." + nameof(OnAuthorizeRemoteReservationStartRequest));
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
                                                 RequestLogDelegate:   OnAuthorizeRemoteReservationStartSOAPRequest,
                                                 ResponseLogDelegate:  OnAuthorizeRemoteReservationStartSOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 QueryTimeout:         Request.RequestTimeout.HasValue ? Request.RequestTimeout.Value : RequestTimeout.Value,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                      (request, xml, onexception) =>
                                                                                                          Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>.Parse(request,
                                                                                                                                                                            xml,
                                                                                                                                                                            CustomAuthorizeRemoteReservationStartResponseMapper,
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

                OnAuthorizeRemoteReservationStartResponse?.Invoke(Endtime,
                                                                  this,
                                                                  ClientId,
                                                                  Request.EventTrackingId,
                                                                  Request.ProviderId,
                                                                  Request.EVSEId,
                                                                  Request.EVCOId,
                                                                  Request.SessionId,
                                                                  Request.PartnerSessionId,
                                                                  Request.PartnerProductId,
                                                                  Request.RequestTimeout.HasValue ? Request.RequestTimeout.Value : RequestTimeout.Value,
                                                                  result.Content,
                                                                  Endtime - StartTime);

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
        /// Delete a reservation at the given EVSE.
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

                OnAuthorizeRemoteReservationStopRequest?.Invoke(StartTime,
                                                                Request.Timestamp.Value,
                                                                this,
                                                                ClientId,
                                                                Request.EventTrackingId,
                                                                Request.SessionId,
                                                                Request.ProviderId,
                                                                Request.EVSEId,
                                                                Request.PartnerSessionId,
                                                                Request.RequestTimeout.HasValue ? Request.RequestTimeout.Value : RequestTimeout.Value);

            }
            catch (Exception e)
            {
                e.Log(nameof(CentralClient) + "." + nameof(OnAuthorizeRemoteReservationStopRequest));
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
                                                 RequestLogDelegate:   OnAuthorizeRemoteReservationStopSOAPRequest,
                                                 ResponseLogDelegate:  OnAuthorizeRemoteReservationStopSOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 QueryTimeout:         Request.RequestTimeout.HasValue ? Request.RequestTimeout.Value : RequestTimeout.Value,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                      (request, xml, onexception) =>
                                                                                                          Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>.Parse(request,
                                                                                                                                                                           xml,
                                                                                                                                                                           CustomAuthorizeRemoteReservationStopResponseMapper,
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

                OnAuthorizeRemoteReservationStopResponse?.Invoke(EndTime,
                                                                 this,
                                                                 ClientId,
                                                                 Request.EventTrackingId,
                                                                 Request.SessionId,
                                                                 Request.ProviderId,
                                                                 Request.EVSEId,
                                                                 Request.PartnerSessionId,
                                                                 Request.RequestTimeout.HasValue ? Request.RequestTimeout.Value : RequestTimeout.Value,
                                                                 result.Content,
                                                                 EndTime - StartTime);

            }
            catch (Exception e)
            {
                e.Log(nameof(CentralClient) + "." + nameof(OnAuthorizeRemoteReservationStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeRemoteStart(ProviderId, EVSEId, EVCOId, SessionId = null, ChargingProductId = null, PartnerSessionId = null, RequestTimeout = null)

        /// <summary>
        /// Create an OICP authorize remote start request.
        /// </summary>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="EVCOId">An e-mobility contract indentification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="RequestTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<Acknowledgement>>

            AuthorizeRemoteStart(Provider_Id         ProviderId,
                                 EVSE_Id             EVSEId,
                                 EVCO_Id             EVCOId,
                                 Session_Id?         SessionId           = null,
                                 PartnerProduct_Id?  ChargingProductId   = null,
                                 PartnerSession_Id?  PartnerSessionId    = null,
                                 TimeSpan?           RequestTimeout      = null)

        {

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    URIPrefix + "/Authorization",
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                #region Documentation

                // <soapenv:Envelope xmlns:soapenv       = "http://schemas.xmlsoap.org/soap/envelope/"
                //                   xmlns:Authorization = "http://www.hubject.com/b2b/services/authorization/v2.0"
                //                   xmlns:CommonTypes   = "http://www.hubject.com/b2b/services/commontypes/v2.0">
                //
                //    <soapenv:Header/>
                //
                //    <soapenv:Body>
                //       <Authorization:eRoamingAuthorizeRemoteStart>
                // 
                //          <!--Optional:-->
                //          <Authorization:SessionID>?</Authorization:SessionID>
                // 
                //          <!--Optional:-->
                //          <Authorization:PartnerSessionID>?</Authorization:PartnerSessionID>
                // 
                //          <Authorization:ProviderID>?</Authorization:ProviderID>
                //          <Authorization:EVSEID>?</Authorization:EVSEID>
                // 
                //          <Authorization:Identification>
                //             <!--You have a CHOICE of the next 4 items at this level-->
                //
                //             <CommonTypes:RFIDmifarefamilyIdentification>
                //                <CommonTypes:UID>?</CommonTypes:UID>
                //             </CommonTypes:RFIDmifarefamilyIdentification>
                // 
                //             <CommonTypes:QRCodeIdentification>
                // 
                //                <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
                // 
                //                <!--You have a CHOICE of the next 2 items at this level-->
                //                <CommonTypes:PIN>?</CommonTypes:PIN>
                // 
                //                <CommonTypes:HashedPIN>
                //                   <CommonTypes:Value>?</CommonTypes:Value>
                //                   <CommonTypes:Function>?</CommonTypes:Function>
                //                   <CommonTypes:Salt>?</CommonTypes:Salt>
                //                </CommonTypes:HashedPIN>
                // 
                //             </CommonTypes:QRCodeIdentification>
                // 
                //             <CommonTypes:PlugAndChargeIdentification>
                //                <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
                //             </CommonTypes:PlugAndChargeIdentification>
                // 
                //             <CommonTypes:RemoteIdentification>
                //                <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
                //             </CommonTypes:RemoteIdentification>
                // 
                //          </Authorization:Identification>
                // 
                //          <!--Optional:-->
                //          <Authorization:PartnerProductID>?</Authorization:PartnerProductID>
                // 
                //       </Authorization:eRoamingAuthorizeRemoteStart>
                //    </soapenv:Body>
                //
                // </soapenv:Envelope>

                #endregion

                var XML = SOAP.Encapsulation(new XElement(OICPNS.Authorization + "eRoamingAuthorizeRemoteStart",

                                                 SessionId.HasValue
                                                     ? new XElement(OICPNS.Authorization + "SessionID",         SessionId.        ToString())
                                                     : null,

                                                 PartnerSessionId.HasValue
                                                     ? new XElement(OICPNS.Authorization + "PartnerSessionID",  PartnerSessionId. ToString())
                                                     : null,

                                                 new XElement(OICPNS.Authorization + "ProviderID",              ProviderId.       ToString()),
                                                 new XElement(OICPNS.Authorization + "EVSEID",                  EVSEId.           ToString()),

                                                 new XElement(OICPNS.Authorization + "Identification",
                                                     new XElement(OICPNS.CommonTypes + "QRCodeIdentification",
                                                         new XElement(OICPNS.CommonTypes + "EVCOID",            EVCOId.           ToString())
                                                     )
                                                 ),

                                                 ChargingProductId.HasValue
                                                     ? new XElement(OICPNS.Authorization + "PartnerProductID",  ChargingProductId.ToString())
                                                     : null

                                             ));


                return await _OICPClient.Query(XML,
                                               "AuthorizeRemoteStart",
                                               QueryTimeout: RequestTimeout.HasValue ? RequestTimeout.Value : this.RequestTimeout,
                                               HTTPRequestBuilder: req => { req.FakeURIPrefix = ""; },

                                               #region OnSuccess

                                               OnSuccess: XMLResponse => XMLResponse.ConvertContent(Acknowledgement.Parse),

                                               #endregion

                                               #region OnSOAPFault

                                               OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                   SendSOAPError(timestamp, this, httpresponse.Content);

                                                   return new HTTPResponse<Acknowledgement>(httpresponse,
                                                                                            Acknowledgement.SystemError(
                                                                                                httpresponse.HTTPStatusCode.ToString(),
                                                                                                httpresponse.HTTPBody.      ToUTF8String()
                                                                                            ),
                                                                                            IsFault: true);

                                               },

                                               #endregion

                                               #region OnHTTPError

                                               OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                   SendHTTPError(timestamp, this, httpresponse);

                                                   return new HTTPResponse<Acknowledgement>(httpresponse,
                                                                                            Acknowledgement.SystemError(
                                                                                                httpresponse.HTTPStatusCode.ToString(),
                                                                                                httpresponse.HTTPBody.      ToUTF8String()
                                                                                            ),
                                                                                            IsFault: true);

                                               },

                                               #endregion

                                               #region OnException

                                               OnException: (timestamp, sender, exception) => {

                                                   SendException(timestamp, sender, exception);

                                                   return null;

                                               }

                                               #endregion

                                              ).ConfigureAwait(false);

            }

        }

        #endregion

        #region AuthorizeRemoteStop(SessionId, ProviderId, EVSEId, PartnerSessionId = null, RequestTimeout = null)

        /// <summary>
        /// Create an OICP remote authorize stop request.
        /// </summary>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="ProviderId">Your e-mobility provider identification (EMP Id).</param>
        /// <param name="EVSEId">An optional EVSE identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="RequestTimeout">An optional timeout for this query.</param>
        public async Task<HTTPResponse<Acknowledgement>>

            AuthorizeRemoteStop(Session_Id          SessionId,
                                Provider_Id         ProviderId,
                                EVSE_Id             EVSEId,
                                PartnerSession_Id?  PartnerSessionId   = null,
                                TimeSpan?           RequestTimeout     = null)

        {

            using (var _OICPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    URIPrefix + "/Authorization",
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                #region Documentation

                // <soapenv:Envelope xmlns:soapenv       = "http://schemas.xmlsoap.org/soap/envelope/"
                //                   xmlns:Authorization = "http://www.hubject.com/b2b/services/authorization/v2.0">
                //
                //    <soapenv:Header/>
                //
                //    <soapenv:Body>
                //       <Authorization:eRoamingAuthorizeRemoteStop>
                // 
                //          <Authorization:SessionID>?</Authorization:SessionID>
                // 
                //          <!--Optional:-->
                //          <Authorization:PartnerSessionID>?</Authorization:PartnerSessionID>
                // 
                //          <Authorization:ProviderID>?</Authorization:ProviderID>
                // 
                //          <Authorization:EVSEID>?</Authorization:EVSEID>
                // 
                //       </Authorization:eRoamingAuthorizeRemoteStop>
                //    </soapenv:Body>
                //
                // </soapenv:Envelope>

                #endregion

                var XML = SOAP.Encapsulation(new XElement(OICPNS.Authorization + "eRoamingAuthorizeRemoteStop",

                                                 new XElement(OICPNS.Authorization + "SessionID",               SessionId.       ToString()),

                                                 PartnerSessionId.HasValue
                                                     ? new XElement(OICPNS.Authorization + "PartnerSessionID",  PartnerSessionId.ToString())
                                                     : null,

                                                 new XElement(OICPNS.Authorization + "ProviderID",              ProviderId.      ToString()),
                                                 new XElement(OICPNS.Authorization + "EVSEID",                  EVSEId.          ToString())

                                             ));


                return await _OICPClient.Query(XML,
                                               "AuthorizeRemoteStop",
                                               QueryTimeout: RequestTimeout.HasValue ? RequestTimeout.Value : this.RequestTimeout,
                                               HTTPRequestBuilder: req => { req.FakeURIPrefix = ""; },

                                               #region OnSuccess

                                               OnSuccess: XMLResponse => XMLResponse.ConvertContent(Acknowledgement.Parse),

                                               #endregion

                                               #region OnSOAPFault

                                               OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                   SendSOAPError(timestamp, this, httpresponse.Content);

                                                   return new HTTPResponse<Acknowledgement>(httpresponse,
                                                                                            Acknowledgement.SystemError(
                                                                                                httpresponse.HTTPStatusCode.ToString(),
                                                                                                httpresponse.HTTPBody.      ToUTF8String()
                                                                                            ),
                                                                                            IsFault: true);

                                               },

                                               #endregion

                                               #region OnHTTPError

                                               OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                   SendHTTPError(timestamp, this, httpresponse);

                                                   return new HTTPResponse<Acknowledgement>(httpresponse,
                                                                                            Acknowledgement.SystemError(
                                                                                                httpresponse.HTTPStatusCode.ToString(),
                                                                                                httpresponse.HTTPBody.      ToUTF8String()
                                                                                            ),
                                                                                            IsFault: true);

                                               },

                                               #endregion

                                               #region OnException

                                               OnException: (timestamp, sender, exception) => {

                                                   SendException(timestamp, sender, exception);

                                                   return null;

                                               }

                                               #endregion

                                              ).ConfigureAwait(false);

            }

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

                OnAuthorizeStartRequest?.Invoke(StartTime,
                                                Request.Timestamp.Value,
                                                this,
                                                ClientId,
                                                Request.OperatorId,
                                                Request.UID,
                                                Request.EVSEId,
                                                Request.SessionId,
                                                Request.PartnerProductId,
                                                Request.PartnerSessionId,
                                                Request.RequestTimeout.HasValue ? Request.RequestTimeout.Value : RequestTimeout.Value);

            }
            catch (Exception e)
            {
                e.Log(nameof(CentralClient) + "." + nameof(OnAuthorizeStartRequest));
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

                result = await _OICPClient.Query(_CustomAuthorizeStartSOAPRequestMapper(Request, SOAP.Encapsulation(Request.ToXML())),
                                                 "eRoamingAuthorizeStart",
                                                 RequestLogDelegate:   OnAuthorizeStartSOAPRequest,
                                                 ResponseLogDelegate:  OnAuthorizeStartSOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 QueryTimeout:         Request.RequestTimeout.HasValue ? Request.RequestTimeout.Value : RequestTimeout.Value,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                      (request, xml, onexception) => CPO.AuthorizationStart.Parse(request,
                                                                                                                                                                  xml,
                                                                                                                                                                  CustomAuthorizeStartResponseMapper,
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

          //  if (result == null)
          //      result = HTTPResponse<AuthorizationStart>.OK(
          //                   new AuthorizationStart(
          //                       Request,
          //                       StatusCodes.SystemError,
          //                       "HTTP request failed!"
          //                   )
          //               );


            #region Send OnAuthorizeStartResponse event

            try
            {

                var Endtime = DateTime.Now;

                OnAuthorizeStartResponse?.Invoke(Endtime,
                                                 this,
                                                 ClientId,
                                                 Request.OperatorId,
                                                 Request.UID,
                                                 Request.EVSEId,
                                                 Request.SessionId,
                                                 Request.PartnerProductId,
                                                 Request.PartnerSessionId,
                                                 Request.RequestTimeout.HasValue ? Request.RequestTimeout.Value : RequestTimeout.Value,
                                                 result.Content,
                                                 Endtime - StartTime);

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

                OnAuthorizeStopRequest?.Invoke(StartTime,
                                               Request.Timestamp.Value,
                                               this,
                                               ClientId,
                                               Request.OperatorId,
                                               Request.SessionId,
                                               Request.UID,
                                               Request.EVSEId,
                                               Request.PartnerSessionId,
                                               Request.RequestTimeout.HasValue ? Request.RequestTimeout.Value : RequestTimeout.Value);

            }
            catch (Exception e)
            {
                e.Log(nameof(CentralClient) + "." + nameof(OnAuthorizeStopRequest));
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

                result = await _OICPClient.Query(_CustomAuthorizeStopSOAPRequestMapper(Request,
                                                                                       SOAP.Encapsulation(Request.ToXML())),
                                                 "eRoamingAuthorizeStop",
                                                 RequestLogDelegate:   OnAuthorizeStopSOAPRequest,
                                                 ResponseLogDelegate:  OnAuthorizeStopSOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 QueryTimeout:         Request.RequestTimeout.HasValue ? Request.RequestTimeout.Value : RequestTimeout.Value,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                      (request, xml, onexception) => CPO.AuthorizationStop.Parse(request,
                                                                                                                                                                 xml,
                                                                                                                                                                 CustomAuthorizeStopResponseMapper,
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

                OnAuthorizeStopResponse?.Invoke(Endtime,
                                                this,
                                                ClientId,
                                                Request.OperatorId,
                                                Request.SessionId,
                                                Request.UID,
                                                Request.EVSEId,
                                                Request.PartnerSessionId,
                                                Request.RequestTimeout.HasValue ? Request.RequestTimeout.Value : RequestTimeout.Value,
                                                result.Content,
                                                Endtime - StartTime);

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

                OnSendChargeDetailRecordRequest?.Invoke(StartTime,
                                                        Request.Timestamp.Value,
                                                        this,
                                                        ClientId,
                                                        Request.EventTrackingId,
                                                        Request.ChargeDetailRecord,
                                                        Request.RequestTimeout.HasValue ? Request.RequestTimeout.Value : RequestTimeout.Value);

            }
            catch (Exception e)
            {
                e.Log(nameof(CentralClient) + "." + nameof(OnSendChargeDetailRecordRequest));
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

                result = await _OICPClient.Query(_CustomSendChargeDetailRecordSOAPRequestMapper(Request, SOAP.Encapsulation(Request.ToXML())),
                                                 "eRoamingChargeDetailRecord",
                                                 RequestLogDelegate:   OnSendChargeDetailRecordSOAPRequest,
                                                 ResponseLogDelegate:  OnSendChargeDetailRecordSOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 QueryTimeout:         Request.RequestTimeout.HasValue ? Request.RequestTimeout.Value : RequestTimeout.Value,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                      (request, xml, onexception) =>
                                                                                                      Acknowledgement<CPO.SendChargeDetailRecordRequest>.Parse(request,
                                                                                                                                                               xml,
                                                                                                                                                               CustomSendChargeDetailRecordResponseMapper,
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

                OnSendChargeDetailRecordResponse?.Invoke(Endtime,
                                                         this,
                                                         ClientId,
                                                         Request.EventTrackingId,
                                                         Request.ChargeDetailRecord,
                                                         Request.RequestTimeout.HasValue ? Request.RequestTimeout.Value : RequestTimeout.Value,
                                                         result.Content,
                                                         Endtime - StartTime);

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
