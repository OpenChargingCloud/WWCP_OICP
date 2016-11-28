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
using System.Xml.Linq;
using System.Threading;
using System.Net.Security;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

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
        public new const           String  DefaultHTTPUserAgent  = "GraphDefined OICP " + Version.Number + " CPO Client";

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
        /// The attached OICP CPO client (HTTP/SOAP client) logger.
        /// </summary>
        public CPOClientLogger  Logger   { get; }

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

        public CustomMapperDelegate<Acknowledgement<PushEVSEDataRequest>, Acknowledgement<PushEVSEDataRequest>.Builder> CustomPushEVSEDataResponseMapper { get; set; }

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

        public CustomMapperDelegate<Acknowledgement<PushEVSEStatusRequest>, Acknowledgement<PushEVSEStatusRequest>.Builder> CustomPushEVSEStatusResponseMapper { get; set; }

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

        public CustomMapperDelegate<Acknowledgement<AuthorizeStartRequest>, Acknowledgement<AuthorizeStartRequest>.Builder> CustomAuthorizeStartResponseMapper { get; set; }

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

        public CustomMapperDelegate<Acknowledgement<AuthorizeStopRequest>, Acknowledgement<AuthorizeStopRequest>.Builder> CustomAuthorizeStopResponseMapper { get; set; }

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

        public CustomMapperDelegate<Acknowledgement<SendChargeDetailRecordRequest>, Acknowledgement<SendChargeDetailRecordRequest>.Builder> CustomSendChargeDetailRecordResponseMapper { get; set; }

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

        public CustomMapperDelegate<Acknowledgement<PullAuthenticationDataRequest>, Acknowledgement<PullAuthenticationDataRequest>.Builder> CustomPullAuthenticationDataResponseMapper { get; set; }

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
        /// <param name="ClientCert">The TLS client certificate to use.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual hostname of the remote OICP service.</param>
        /// <param name="URIPrefix">An default URI prefix.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="QueryTimeout">An optional timeout for upstream queries.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="LoggingContext">An optional context for logging client methods.</param>
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
        public CPOClient(String                               ClientId,
                         String                               Hostname,
                         IPPort                               RemotePort                  = null,
                         RemoteCertificateValidationCallback  RemoteCertificateValidator  = null,
                         X509Certificate                      ClientCert                  = null,
                         String                               HTTPVirtualHost             = null,
                         String                               URIPrefix                   = DefaultURIPrefix,
                         String                               HTTPUserAgent               = DefaultHTTPUserAgent,
                         TimeSpan?                            QueryTimeout                = null,
                         DNSClient                            DNSClient                   = null,
                         String                               LoggingContext              = CPOClientLogger.DefaultContext,
                         Func<String, String, String>         LogFileCreator              = null)

            : base(ClientId,
                   Hostname,
                   RemotePort ?? DefaultRemotePort,
                   RemoteCertificateValidator,
                   ClientCert,
                   HTTPVirtualHost,
                   URIPrefix.Trim().IsNotNullOrEmpty() ? URIPrefix : DefaultURIPrefix,
                   HTTPUserAgent,
                   QueryTimeout,
                   DNSClient)

        {

            #region Initial checks

            if (ClientId.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Logger),    "The given client identification must not be null or empty!");

            if (Hostname.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Hostname),  "The given hostname must not be null or empty!");

            #endregion

            this.Logger = new CPOClientLogger(this,
                                              LoggingContext,
                                              LogFileCreator);

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
        /// <param name="ClientCert">The TLS client certificate to use.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual hostname of the remote OICP service.</param>
        /// <param name="URIPrefix">An default URI prefix.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="QueryTimeout">An optional timeout for upstream queries.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public CPOClient(String                               ClientId,
                         CPOClientLogger                      Logger,
                         String                               Hostname,
                         IPPort                               RemotePort                   = null,
                         RemoteCertificateValidationCallback  RemoteCertificateValidator   = null,
                         X509Certificate                      ClientCert                   = null,
                         String                               HTTPVirtualHost              = null,
                         String                               URIPrefix                    = DefaultURIPrefix,
                         String                               HTTPUserAgent                = DefaultHTTPUserAgent,
                         TimeSpan?                            QueryTimeout                 = null,
                         DNSClient                            DNSClient                    = null)

            : base(ClientId,
                   Hostname,
                   RemotePort ?? DefaultRemotePort,
                   RemoteCertificateValidator,
                   ClientCert,
                   HTTPVirtualHost,
                   URIPrefix.Trim().IsNotNullOrEmpty() ? URIPrefix : DefaultURIPrefix,
                   HTTPUserAgent,
                   QueryTimeout,
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


            HTTPResponse<Acknowledgement<PushEVSEDataRequest>> result = null;

            #endregion

            #region Send OnPushEVSEDataRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnPushEVSEDataRequest?.Invoke(StartTime,
                                              Request.Timestamp.Value,
                                              this,
                                              ClientId,
                                              Request.EventTrackingId,
                                              Request.Action,
                                              Request.EVSEDataRecords.ULongCount(),
                                              Request.EVSEDataRecords,
                                              Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnPushEVSEDataRequest));
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

                result = await _OICPClient.Query(_CustomPushEVSEDataSOAPRequestMapper(Request, SOAP.Encapsulation(Request.ToXML(false))),
                                                 "eRoamingPushEvseData",
                                                 RequestLogDelegate:   OnPushEVSEDataSOAPRequest,
                                                 ResponseLogDelegate:  OnPushEVSEDataSOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 QueryTimeout:         Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                      (request, xml, onexception) =>
                                                                                                      Acknowledgement<PushEVSEDataRequest>.Parse(request,
                                                                                                                                                 xml,
                                                                                                                                                 CustomPushEVSEDataResponseMapper,
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

                                                ).ConfigureAwait(false);

            }

            if (result == null)
                result = HTTPResponse<Acknowledgement<PushEVSEDataRequest>>.OK(
                             new Acknowledgement<PushEVSEDataRequest>(
                                 Request,
                                 StatusCodes.SystemError,
                                 "HTTP request failed!"
                             )
                         );


            #region Send OnPushEVSEDataResponse event

            var Endtime = DateTime.Now;

            try
            {

                OnPushEVSEDataResponse?.Invoke(Endtime,
                                               Request.Timestamp.Value,
                                               this,
                                               ClientId,
                                               Request.EventTrackingId,
                                               Request.Action,
                                               Request.EVSEDataRecords.ULongCount(),
                                               Request.EVSEDataRecords,
                                               Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout,
                                               result.Content,
                                               Endtime - StartTime);

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


            HTTPResponse<Acknowledgement<PushEVSEStatusRequest>> result = null;

            #endregion

            #region Send OnPushEVSEStatusRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnPushEVSEStatusRequest?.Invoke(StartTime,
                                              Request.Timestamp.Value,
                                              this,
                                              ClientId,
                                              Request.EventTrackingId,
                                              Request.Action,
                                              Request.EVSEStatusRecords.ULongCount(),
                                              Request.EVSEStatusRecords,
                                              Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnPushEVSEStatusRequest));
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

                result = await _OICPClient.Query(_CustomPushEVSEStatusSOAPRequestMapper(Request, SOAP.Encapsulation(Request.ToXML())),
                                                 "eRoamingPushEvseStatus",
                                                 RequestLogDelegate:   OnPushEVSEStatusSOAPRequest,
                                                 ResponseLogDelegate:  OnPushEVSEStatusSOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 QueryTimeout:         Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                      (request, xml, onexception) =>
                                                                                                      Acknowledgement<PushEVSEStatusRequest>.Parse(request,
                                                                                                                                                   xml,
                                                                                                                                                   CustomPushEVSEStatusResponseMapper,
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

                                                ).ConfigureAwait(false);

            }

            if (result == null)
                result = HTTPResponse<Acknowledgement<PushEVSEStatusRequest>>.OK(
                             new Acknowledgement<PushEVSEStatusRequest>(
                                 Request,
                                 StatusCodes.SystemError,
                                 "HTTP request failed!"
                             )
                         );


            #region Send OnPushEVSEStatusResponse event

            var Endtime = DateTime.Now;

            try
            {

                OnPushEVSEStatusResponse?.Invoke(Endtime,
                                                 Request.Timestamp.Value,
                                                 this,
                                                 ClientId,
                                                 Request.EventTrackingId,
                                                 Request.Action,
                                                 Request.EVSEStatusRecords.ULongCount(),
                                                 Request.EVSEStatusRecords,
                                                 Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout,
                                                 result.Content,
                                                 Endtime - StartTime);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnPushEVSEStatusResponse));
            }

            #endregion


            return result;

        }

        #endregion


        #region AuthorizeStart(Request)

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


            HTTPResponse<AuthorizationStart> result = null;

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
                                                Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnAuthorizeStartRequest));
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
                                                 QueryTimeout:         Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(AuthorizationStart.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<AuthorizationStart>(httpresponse,
                                                                                                 new AuthorizationStart(StatusCodes.DataError,
                                                                                                                        httpresponse.Content.ToString()),
                                                                                                 IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnHTTPError

                                                 OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                     SendHTTPError(timestamp, this, httpresponse);

                                                     return new HTTPResponse<AuthorizationStart>(httpresponse,
                                                                                                 new AuthorizationStart(StatusCodes.DataError,
                                                                                                                        httpresponse.HTTPStatusCode.ToString(),
                                                                                                                        httpresponse.HTTPBody.ToUTF8String()),
                                                                                                 IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnException

                                                 OnException: (timestamp, sender, exception) => {

                                                     SendException(timestamp, sender, exception);

                                                     return HTTPResponse<AuthorizationStart>.ExceptionThrown(new AuthorizationStart(StatusCodes.SystemError,
                                                                                                                                    exception.Message,
                                                                                                                                    exception.StackTrace),
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
                                                 Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout,
                                                 result.Content,
                                                 Endtime - StartTime);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnAuthorizeStartResponse));
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
        public async Task<HTTPResponse<AuthorizationStop>>

            AuthorizeStop(AuthorizeStopRequest  Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given AuthorizeStop request must not be null!");

            Request = _CustomAuthorizeStopRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped AuthorizeStop request must not be null!");


            HTTPResponse<AuthorizationStop> result = null;

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
                                               Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnAuthorizeStopRequest));
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

                result = await _OICPClient.Query(_CustomAuthorizeStopSOAPRequestMapper(Request, SOAP.Encapsulation(Request.ToXML())),
                                                 "eRoamingAuthorizeStop",
                                                 RequestLogDelegate:   OnAuthorizeStopSOAPRequest,
                                                 ResponseLogDelegate:  OnAuthorizeStopSOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 QueryTimeout:         Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(AuthorizationStop.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<AuthorizationStop>(httpresponse,
                                                                                                new AuthorizationStop(StatusCodes.DataError,
                                                                                                                      httpresponse.Content.ToString()),
                                                                                                IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnHTTPError

                                                 OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                     SendHTTPError(timestamp, this, httpresponse);

                                                     return new HTTPResponse<AuthorizationStop>(httpresponse,
                                                                                                new AuthorizationStop(StatusCodes.DataError,
                                                                                                                      httpresponse.HTTPStatusCode.ToString(),
                                                                                                                      httpresponse.HTTPBody.      ToUTF8String()),
                                                                                                IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnException

                                                 OnException: (timestamp, sender, exception) => {

                                                     SendException(timestamp, sender, exception);

                                                     return HTTPResponse<AuthorizationStop>.ExceptionThrown(new AuthorizationStop(StatusCodes.SystemError,
                                                                                                                                  exception.Message,
                                                                                                                                  exception.StackTrace),
                                                                                                            Exception: exception);

                                                 }

                                                 #endregion

                                                ).ConfigureAwait(false);

            }

       //     if (result == null)
       //         result = HTTPResponse<AuthorizationStop>.OK(
       //                      new AuthorizationStop(
       //                          Request,
       //                          StatusCodes.SystemError,
       //                          "HTTP request failed!"
       //                      )
       //                  );


            #region Send OnAuthorizeStopResponse event

            var Endtime = DateTime.Now;

            try
            {

                OnAuthorizeStopResponse?.Invoke(DateTime.Now,
                                                this,
                                                ClientId,
                                                Request.OperatorId,
                                                Request.SessionId,
                                                Request.UID,
                                                Request.EVSEId,
                                                Request.PartnerSessionId,
                                                Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout,
                                                result.Content,
                                                Endtime - StartTime);

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


            HTTPResponse<Acknowledgement<SendChargeDetailRecordRequest>> result = null;

            #endregion

            #region Send OnSendChargeDetailRecord event

            var StartTime = DateTime.Now;

            try
            {

                OnSendChargeDetailRecordRequest?.Invoke(StartTime,
                                                        Request.Timestamp.Value,
                                                        this,
                                                        ClientId,
                                                        Request.EventTrackingId,
                                                        Request.ChargeDetailRecord,
                                                        Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnSendChargeDetailRecordRequest));
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
                                                 QueryTimeout:         Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                      (request, xml, onexception) =>
                                                                                                      Acknowledgement<SendChargeDetailRecordRequest>.Parse(request,
                                                                                                                                                           xml,
                                                                                                                                                           CustomSendChargeDetailRecordResponseMapper,
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


            #region Send OnChargeDetailRecordSent event

            var Endtime = DateTime.Now;

            try
            {

                OnSendChargeDetailRecordResponse?.Invoke(Endtime,
                                                         Request.Timestamp.Value,
                                                         this,
                                                         ClientId,
                                                         Request.EventTrackingId,
                                                         Request.ChargeDetailRecord,
                                                         Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout,
                                                         result.Content,
                                                         Endtime - StartTime);

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


            HTTPResponse<AuthenticationData> result = null;

            #endregion

            #region Send OnPullAuthenticationData event

            var StartTime = DateTime.Now;

            try
            {

                OnPullAuthenticationDataRequest?.Invoke(StartTime,
                                                        Request.Timestamp.Value,
                                                        this,
                                                        ClientId,
                                                        Request.EventTrackingId,
                                                        Request.OperatorId,
                                                        Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnPullAuthenticationDataRequest));
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

                result = await _OICPClient.Query(_CustomPullAuthenticationDataSOAPRequestMapper(Request, SOAP.Encapsulation(Request.ToXML())),
                                                 "eRoamingPullAuthenticationData",
                                                 RequestLogDelegate:   OnPullAuthenticationDataSOAPRequest,
                                                 ResponseLogDelegate:  OnPullAuthenticationDataSOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 QueryTimeout:         Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(AuthenticationData.Parse),

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

                                                     return new HTTPResponse<AuthenticationData>(httpresponse,
                                                                                                 IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnException

                                                 OnException: (timestamp, sender, exception) => {

                                                     SendException(timestamp, sender, exception);

                                                     return HTTPResponse<AuthenticationData>.ExceptionThrown(new AuthenticationData(StatusCodes.SystemError,
                                                                                                                                    exception.Message,
                                                                                                                                    exception.StackTrace),
                                                                                                             Exception: exception);

                                                 }

                                                 #endregion

                                                ).ConfigureAwait(false);

                #region Send OnAuthenticationDataPulled event

                var Endtime = DateTime.Now;

                try
                {

                    OnPullAuthenticationDataResponse?.Invoke(DateTime.Now,
                                                             this,
                                                             ClientId,
                                                             Request.EventTrackingId,
                                                             Request.OperatorId,
                                                             Request.RequestTimeout.HasValue ? Request.RequestTimeout : RequestTimeout,
                                                             result.Content,
                                                             Endtime - StartTime);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CPOClient) + "." + nameof(OnPullAuthenticationDataResponse));
                }

                #endregion

                return result;

            }

        }

        #endregion


    }

}
