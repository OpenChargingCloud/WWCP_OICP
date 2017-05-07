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
using System.Threading;
using System.Net.Security;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.Mobile
{

    /// <summary>
    /// An OICP Mobile client.
    /// </summary>
    public partial class MobileClient : ASOAPClient,
                                        IMobileClient
    {

        #region Data

        /// <summary>
        /// The default HTTP user agent string.
        /// </summary>
        public new const           String  DefaultHTTPUserAgent            = "GraphDefined OICP " + Version.Number + " Mobile client";

        /// <summary>
        /// The default remote TCP port to connect to.
        /// </summary>
        public new static readonly IPPort  DefaultRemotePort               = IPPort.Parse(443);

        /// <summary>
        /// The default URI prefix.
        /// </summary>
        public const               String  DefaultURIPrefix                = "/ibis/ws";

        /// <summary>
        /// The default HTTP/SOAP/XML URI for OICP MobileAuthorization requests.
        /// </summary>
        public     const           String  DefaultMobileAuthorizationURI   = "/eRoamingMobileAuthorization_V2.0";

        #endregion

        #region Properties

        /// <summary>
        /// The HTTP/SOAP/XML URI for OICP Mobile Authorization requests.
        /// </summary>
        public String              MobileAuthorizationURI    { get; }

        /// <summary>
        /// The attached OICP Mobile client (HTTP/SOAP client) logger.
        /// </summary>
        public MobileClientLogger  Logger                    { get; }

        #endregion

        #region Custom request/response mappers

        #region MobileAuthorizeStart(SOAP)RequestMapper

        #region CustomMobileAuthorizeStartRequestMapper

        private Func<MobileAuthorizeStartRequest, MobileAuthorizeStartRequest> _CustomMobileAuthorizeStartRequestMapper = _ => _;

        public Func<MobileAuthorizeStartRequest, MobileAuthorizeStartRequest> CustomMobileAuthorizeStartRequestMapper
        {

            get
            {
                return _CustomMobileAuthorizeStartRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomMobileAuthorizeStartRequestMapper = value;
            }

        }

        #endregion

        #region CustomMobileAuthorizeStartSOAPRequestMapper

        private Func<MobileAuthorizeStartRequest, XElement, XElement> _CustomMobileAuthorizeStartSOAPRequestMapper = (request, xml) => xml;

        public Func<MobileAuthorizeStartRequest, XElement, XElement> CustomMobileAuthorizeStartSOAPRequestMapper
        {

            get
            {
                return _CustomMobileAuthorizeStartSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomMobileAuthorizeStartSOAPRequestMapper = value;
            }

        }

        #endregion

        #endregion



        public CustomXMLParserDelegate<StatusCode>                       CustomStatusCodeParser                         { get; set; }
        public CustomXMLSerializerDelegate<MobileAuthorizeStartRequest>  CustomMobileAuthorizeStartRequestSerializer    { get; set; }


        #endregion

        #region Events

        #region OnMobileAuthorizeStartRequest

        /// <summary>
        /// An event fired whenever a 'mobile authorize start' request will be send.
        /// </summary>
        public event OnMobileAuthorizeStartRequestHandler   OnMobileAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever a 'mobile authorize start' SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                OnMobileAuthorizeStartSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a 'mobile authorize start' SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler               OnMobileAuthorizeStartSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a 'mobile authorize start' request had been received.
        /// </summary>
        public event OnMobileAuthorizeStartResponseHandler  OnMobileAuthorizeStartResponse;

        #endregion


        #region OnMobileRemoteStart

        /// <summary>
        /// An event fired whenever a MobileRemoteStart request will be send.
        /// </summary>
        public event OnMobileRemoteStartRequestDelegate   OnMobileRemoteStartRequest;

        /// <summary>
        /// An event fired whenever a MobileRemoteStart SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler              OnMobileRemoteStartSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a MobileRemoteStart SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler             OnMobileRemoteStartSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a MobileRemoteStart request had been received.
        /// </summary>
        public event OnMobileRemoteStartResponseDelegate  OnMobileRemoteStartResponse;

        #endregion

        #region OnMobileRemoteStop

        /// <summary>
        /// An event fired whenever a MobileRemoteStop request will be send.
        /// </summary>
        public event OnMobileRemoteStopRequestDelegate    OnMobileRemoteStopRequest;

        /// <summary>
        /// An event fired whenever a MobileRemoteStop SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler              OnMobileRemoteStopSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a MobileRemoteStop SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler             OnMobileRemoteStopSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a MobileRemoteStop request had been received.
        /// </summary>
        public event OnMobileRemoteStopResponseDelegate   OnMobileRemoteStopResponse;

        #endregion

        #endregion

        #region Constructor(s)

        #region MobileClient(ClientId, Hostname, ..., LoggingContext = MobileClientLogger.DefaultContext, ...)

        /// <summary>
        /// Create a new OICP Mobile client.
        /// </summary>
        /// <param name="ClientId">A unqiue identification of this client.</param>
        /// <param name="Hostname">The OICP hostname to connect to.</param>
        /// <param name="RemotePort">An optional OICP TCP port to connect to.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCert">The TLS client certificate to use.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual host name to use.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent to use.</param>
        /// <param name="QueryTimeout">An optional timeout for upstream queries.</param>
        /// <param name="DNSClient">An optional DNS client.</param>
        /// <param name="LoggingContext">An optional context for logging client methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        public MobileClient(String                               ClientId,
                            String                               Hostname,
                            IPPort                               RemotePort                  = null,
                            RemoteCertificateValidationCallback  RemoteCertificateValidator  = null,
                            X509Certificate                      ClientCert                  = null,
                            String                               HTTPVirtualHost             = null,
                            String                               URIPrefix                   = DefaultURIPrefix,
                            String                               MobileAuthorizationURI      = DefaultMobileAuthorizationURI,
                            String                               HTTPUserAgent               = DefaultHTTPUserAgent,
                            TimeSpan?                            QueryTimeout                = null,
                            DNSClient                            DNSClient                   = null,
                            String                               LoggingContext              = MobileClientLogger.DefaultContext,
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
                   QueryTimeout,
                   DNSClient)

        {

            #region Initial checks

            if (ClientId.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Logger),    "The given client identification must not be null or empty!");

            if (Hostname.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Hostname),  "The given hostname must not be null or empty!");

            #endregion


            this.MobileAuthorizationURI  = MobileAuthorizationURI ?? DefaultMobileAuthorizationURI;

            this.Logger                  = new MobileClientLogger(this,
                                                                  LoggingContext,
                                                                  LogfileCreator);

        }

        #endregion

        #region MobileClient(ClientId, Logger, Hostname, ...)

        /// <summary>
        /// Create a new OICP Mobile client.
        /// </summary>
        /// <param name="ClientId">A unqiue identification of this client.</param>
        /// <param name="Logger">A mobile client logger.</param>
        /// <param name="Hostname">The OICP hostname to connect to.</param>
        /// <param name="RemotePort">An optional OICP TCP port to connect to.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCert">The TLS client certificate to use.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual host name to use.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent to use.</param>
        /// <param name="QueryTimeout">An optional timeout for upstream queries.</param>
        /// <param name="DNSClient">An optional DNS client.</param>
        public MobileClient(String                               ClientId,
                            MobileClientLogger                   Logger,
                            String                               Hostname,
                            IPPort                               RemotePort                  = null,
                            RemoteCertificateValidationCallback  RemoteCertificateValidator  = null,
                            X509Certificate                      ClientCert                  = null,
                            String                               HTTPVirtualHost             = null,
                            String                               URIPrefix                   = DefaultURIPrefix,
                            String                               MobileAuthorizationURI      = DefaultMobileAuthorizationURI,
                            String                               HTTPUserAgent               = DefaultHTTPUserAgent,
                            TimeSpan?                            QueryTimeout                = null,
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

            this.MobileAuthorizationURI  = MobileAuthorizationURI ?? DefaultMobileAuthorizationURI;

            this.Logger                  = Logger;

        }

        #endregion

        #endregion


        #region MobileAuthorizeStart(Request)

        /// <summary>
        /// Create a new task sending a MobileAuthorizeStart request.
        /// </summary>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="EVCOIdWithPIN">The eMA identification with its PIN.</param>
        /// <param name="PartnerProductId">The optional charging product identification.</param>
        /// <param name="GetNewSession">Optionaly start or start not an new charging session.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<MobileAuthorizationStart>>

            MobileAuthorizeStart(MobileAuthorizeStartRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The given MobileAuthorizeStart request must not be null!");

            Request = _CustomMobileAuthorizeStartRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The mapped MobileAuthorizeStart request must not be null!");


            HTTPResponse<MobileAuthorizationStart> result = null;

            #endregion

            #region Send OnMobileAuthorizeStartRequest event

            var StartTime = DateTime.Now;

            try
            {

                if (OnMobileAuthorizeStartRequest != null)
                    await Task.WhenAll(OnMobileAuthorizeStartRequest.GetInvocationList().
                                       Cast<OnMobileAuthorizeStartRequestHandler>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.EVSEId,
                                                     Request.QRCodeIdentification,
                                                     Request.PartnerProductId,
                                                     Request.GetNewSession,
                                                     Request.RequestTimeout ?? RequestTimeout.Value))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(MobileClient) + "." + nameof(OnMobileAuthorizeStartRequest));
            }

            #endregion


            using (var _OICPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    URIPrefix + MobileAuthorizationURI,
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))

            {

                result = await _OICPClient.Query(_CustomMobileAuthorizeStartSOAPRequestMapper(Request,
                                                                                              SOAP.Encapsulation(Request.ToXML(CustomMobileAuthorizeStartRequestSerializer))),
                                                 "eRoamingMobileAuthorizeStart",
                                                 RequestLogDelegate:   OnMobileAuthorizeStartSOAPRequest,
                                                 ResponseLogDelegate:  OnMobileAuthorizeStartSOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 QueryTimeout:         Request.RequestTimeout ?? RequestTimeout.Value,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                      (request, xml, onexception) =>
                                                                                                          MobileAuthorizationStart.Parse(request,
                                                                                                                                         xml,
                                                                                                                                         CustomStatusCodeParser,
                                                                                                                                         onexception)),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                     return new HTTPResponse<MobileAuthorizationStart>(httpresponse,
                                                                                                       new MobileAuthorizationStart(
                                                                                                           StatusCodes.SystemError,
                                                                                                           httpresponse.Content.ToString()
                                                                                                       ),
                                                                                                       IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnHTTPError

                                                 OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                     SendHTTPError(timestamp, soapclient, httpresponse);

                                                     return new HTTPResponse<MobileAuthorizationStart>(httpresponse,
                                                                                                       new MobileAuthorizationStart(
                                                                                                           StatusCodes.SystemError,
                                                                                                           httpresponse.HTTPStatusCode.ToString(),
                                                                                                           httpresponse.HTTPBody.      ToUTF8String()
                                                                                                       ),
                                                                                                       IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnException

                                                 OnException: (timestamp, sender, exception) => {

                                                     SendException(timestamp, sender, exception);

                                                     return HTTPResponse<MobileAuthorizationStart>.ExceptionThrown(new MobileAuthorizationStart(
                                                                                                                       StatusCodes.SystemError,
                                                                                                                       exception.Message,
                                                                                                                       exception.StackTrace
                                                                                                                   ),
                                                                                                                   Exception: exception);

                                                 }

                                                 #endregion

                                                ).ConfigureAwait(false);

            }

            if (result == null)
                result = HTTPResponse<MobileAuthorizationStart>.ClientError(
                             new MobileAuthorizationStart(
                                 StatusCodes.SystemError,
                                 "HTTP request failed!"
                             )
                         );


            #region Send OnMobileAuthorizeStartResponse event

            var Endtime = DateTime.Now;

            try
            {

                if (OnMobileAuthorizeStartResponse != null)
                    await Task.WhenAll(OnMobileAuthorizeStartResponse.GetInvocationList().
                                       Cast<OnMobileAuthorizeStartResponseHandler>().
                                       Select(e => e(Endtime,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.EVSEId,
                                                     Request.QRCodeIdentification,
                                                     Request.PartnerProductId,
                                                     Request.GetNewSession,
                                                     Request.RequestTimeout ?? RequestTimeout.Value,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(MobileClient) + "." + nameof(OnMobileAuthorizeStartResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region MobileRemoteStart(SessionId, ...)

        /// <summary>
        /// Create a new task starting a remote charging session.
        /// </summary>
        /// <param name="SessionId">A charging session identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<Acknowledgement>>

            MobileRemoteStart(Session_Id          SessionId,

                              DateTime?           Timestamp          = null,
                              CancellationToken?  CancellationToken  = null,
                              EventTracking_Id    EventTrackingId    = null,
                              TimeSpan?           RequestTimeout     = null)

        {

            #region Initial checks

            if (SessionId == null)
                throw new ArgumentNullException(nameof(SessionId),  "The given charging session identification must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;

            #endregion

            #region Send OnMobileRemoteStartRequest event

            try
            {

                OnMobileRemoteStartRequest?.Invoke(DateTime.Now,
                                                   Timestamp.Value,
                                                   this,
                                                   ClientId,
                                                   EventTrackingId,
                                                   SessionId,
                                                   RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(MobileClient) + "." + nameof(OnMobileRemoteStartRequest));
            }

            #endregion


            using (var _OICPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    URIPrefix + "/eRoamingMobileAuthorization_V2.0",
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))

            {

                var result = await _OICPClient.Query(MobileClient_XMLMethods.MobileRemoteStartXML(SessionId),
                                                     "eRoamingMobileRemoteStart",
                                                     RequestLogDelegate:   OnMobileRemoteStartSOAPRequest,
                                                     ResponseLogDelegate:  OnMobileRemoteStartSOAPResponse,
                                                     CancellationToken:    CancellationToken,
                                                     EventTrackingId:      EventTrackingId,
                                                     QueryTimeout:         RequestTimeout,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Acknowledgement.Parse),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                         return new HTTPResponse<Acknowledgement>(httpresponse,
                                                                                                  Acknowledgement.SystemError(
                                                                                                      httpresponse.Content.ToString()
                                                                                                  ),
                                                                                                  IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, soapclient, httpresponse);

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

                                                         return HTTPResponse<Acknowledgement>.ExceptionThrown(Acknowledgement.ServiceNotAvailable(
                                                                                                                  exception.Message,
                                                                                                                  exception.StackTrace,
                                                                                                                  SessionId
                                                                                                              ),
                                                                                                              Exception: exception);

                                                     }

                                                     #endregion

                                                    );


                #region Send OnMobileRemoteStartResponse event

                try
                {

                    OnMobileRemoteStartResponse?.Invoke(DateTime.Now,
                                                        Timestamp.Value,
                                                        this,
                                                        ClientId,
                                                        EventTrackingId,
                                                        SessionId,
                                                        RequestTimeout,
                                                        result.Content,
                                                        DateTime.Now - Timestamp.Value);

                }
                catch (Exception e)
                {
                    e.Log(nameof(MobileClient) + "." + nameof(OnMobileRemoteStartResponse));
                }

                #endregion


                return result;

            }

        }

        #endregion

        #region MobileRemoteStop(SessionId, ...)

        /// <summary>
        /// Create a new task stopping a remote charging session.
        /// </summary>
        /// <param name="SessionId">A charging session identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<Acknowledgement>>

            MobileRemoteStop(Session_Id          SessionId,

                             DateTime?           Timestamp          = null,
                             CancellationToken?  CancellationToken  = null,
                             EventTracking_Id    EventTrackingId    = null,
                             TimeSpan?           RequestTimeout     = null)

        {

            #region Initial checks

            if (SessionId == null)
                throw new ArgumentNullException(nameof(SessionId),  "The given charging session identification must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;

            #endregion

            #region Send OnMobileRemoteStopRequest event

            try
            {

                OnMobileRemoteStopRequest?.Invoke(DateTime.Now,
                                                  Timestamp.Value,
                                                  this,
                                                  ClientId,
                                                  EventTrackingId,
                                                  SessionId,
                                                  RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(MobileClient) + "." + nameof(OnMobileRemoteStopRequest));
            }

            #endregion


            using (var _OICPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    URIPrefix + "/eRoamingMobileAuthorization_V2.0",
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))

            {

                var result = await _OICPClient.Query(MobileClient_XMLMethods.MobileRemoteStopXML(SessionId),
                                                     "eRoamingMobileRemoteStop",
                                                     RequestLogDelegate:   OnMobileRemoteStopSOAPRequest,
                                                     ResponseLogDelegate:  OnMobileRemoteStopSOAPResponse,
                                                     CancellationToken:    CancellationToken,
                                                     EventTrackingId:      EventTrackingId,
                                                     QueryTimeout:         RequestTimeout,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Acknowledgement.Parse),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                         return new HTTPResponse<Acknowledgement>(httpresponse,
                                                                                                  Acknowledgement.SystemError(
                                                                                                      httpresponse.Content.ToString()
                                                                                                  ),
                                                                                                  IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, soapclient, httpresponse);

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

                                                         return HTTPResponse<Acknowledgement>.ExceptionThrown(Acknowledgement.ServiceNotAvailable(
                                                                                                                  exception.Message,
                                                                                                                  exception.StackTrace,
                                                                                                                  SessionId
                                                                                                              ),
                                                                                                              Exception: exception);

                                                     }

                                                     #endregion

                                                    );


                #region Send OnMobileRemoteStopResponse event

                try
                {

                    OnMobileRemoteStopResponse?.Invoke(DateTime.Now,
                                                       Timestamp.Value,
                                                       this,
                                                       ClientId,
                                                       EventTrackingId,
                                                       SessionId,
                                                       RequestTimeout,
                                                       result.Content,
                                                       DateTime.Now - Timestamp.Value);

                }
                catch (Exception e)
                {
                    e.Log(nameof(MobileClient) + "." + nameof(OnMobileRemoteStopResponse));
                }

                #endregion


                return result;

            }

        }

        #endregion


    }

}
