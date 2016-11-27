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

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// An OICP Mobile client.
    /// </summary>
    public partial class MobileClient : ASOAPClient
    {

        #region Data

        /// <summary>
        /// The default HTTP user agent string.
        /// </summary>
        public new const           String  DefaultHTTPUserAgent  = "GraphDefined OICP " + Version.Number + " Mobile client";

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
        /// The attached OICP Mobile client (HTTP/SOAP client) logger.
        /// </summary>
        public MobileClientLogger Logger { get; }

        #endregion

        #region Events

        #region OnMobileAuthorizeStartRequest/-Response

        /// <summary>
        /// An event fired whenever a MobileAuthorizeStart request will be send.
        /// </summary>
        public event OnMobileAuthorizeStartRequestDelegate   OnMobileAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever a MobileAuthorizeStart SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler                 OnMobileAuthorizeStartSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a MobileAuthorizeStart SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                OnMobileAuthorizeStartSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a MobileAuthorizeStart request had been received.
        /// </summary>
        public event OnMobileAuthorizeStartResponseDelegate  OnMobileAuthorizeStartResponse;

        #endregion

        #region OnMobileRemoteStart/-Stop

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
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
        public MobileClient(String                               ClientId,
                            String                               Hostname,
                            IPPort                               RemotePort                  = null,
                            RemoteCertificateValidationCallback  RemoteCertificateValidator  = null,
                            X509Certificate                      ClientCert                  = null,
                            String                               HTTPVirtualHost             = null,
                            String                               URIPrefix                   = DefaultURIPrefix,
                            String                               HTTPUserAgent               = DefaultHTTPUserAgent,
                            TimeSpan?                            QueryTimeout                = null,
                            DNSClient                            DNSClient                   = null,
                            String                               LoggingContext              = MobileClientLogger.DefaultContext,
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

            this.Logger = new MobileClientLogger(this,
                                                 LoggingContext,
                                                 LogFileCreator);

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


        #region MobileAuthorizeStart(EVSEId, EVCOId, PIN, PartnerProductId = null, GetNewSession = null, ...)

        /// <summary>
        /// Create a new task sending a MobileAuthorizeStart request.
        /// </summary>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="EVCOId">The eMA identification.</param>
        /// <param name="PIN">The PIN of the eMA identification.</param>
        /// <param name="PartnerProductId">The optional charging product identification.</param>
        /// <param name="GetNewSession">Optionaly start or start not an new charging session.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<MobileAuthorizationStart>>

            MobileAuthorizeStart(EVSE_Id              EVSEId,
                                 EVCO_Id              EVCOId,
                                 String               PIN,
                                 String               PartnerProductId   = null,
                                 Boolean?             GetNewSession      = null,

                                 DateTime?            Timestamp          = null,
                                 CancellationToken?   CancellationToken  = null,
                                 EventTracking_Id     EventTrackingId    = null,
                                 TimeSpan?            RequestTimeout     = null)


            => await MobileAuthorizeStart(EVSEId,
                                          new EVCOIdWithPIN(EVCOId, PIN),
                                          PartnerProductId,
                                          GetNewSession,

                                          Timestamp,
                                          CancellationToken,
                                          EventTrackingId,
                                          RequestTimeout);


        #endregion

        #region MobileAuthorizeStart(EVSEId, EVCOId, HashedPIN, Function, Salt, PartnerProductId = null, GetNewSession = null, ...)

        /// <summary>
        /// Create a new task sending a MobileAuthorizeStart request.
        /// </summary>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="EVCOId">The eMA identification.</param>
        /// <param name="HashedPIN">The PIN of the eMA identification.</param>
        /// <param name="Function">The crypto hash function of the eMA identification.</param>
        /// <param name="Salt">The Salt of the eMA identification.</param>
        /// <param name="PartnerProductId">The optional charging product identification.</param>
        /// <param name="GetNewSession">Optionaly start or start not an new charging session.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<MobileAuthorizationStart>>

            MobileAuthorizeStart(EVSE_Id             EVSEId,
                                 EVCO_Id             EVCOId,
                                 String              HashedPIN,
                                 PINCrypto           Function,
                                 String              Salt,
                                 String              PartnerProductId   = null,
                                 Boolean?            GetNewSession      = null,

                                 DateTime?           Timestamp          = null,
                                 CancellationToken?  CancellationToken  = null,
                                 EventTracking_Id    EventTrackingId    = null,
                                 TimeSpan?           RequestTimeout     = null)


            => await MobileAuthorizeStart(EVSEId,
                                          new EVCOIdWithPIN(EVCOId, HashedPIN, Function, Salt),
                                          PartnerProductId,
                                          GetNewSession,

                                          Timestamp,
                                          CancellationToken,
                                          EventTrackingId,
                                          RequestTimeout);


        #endregion

        #region MobileAuthorizeStart(EVSEId, EVCOIdWithPIN, PartnerProductId = null, GetNewSession = null, ...)

        /// <summary>
        /// Create a new task sending a MobileAuthorizeStart request.
        /// </summary>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="EVCOIdWithPIN">The eMA identification with its PIN.</param>
        /// <param name="ProductId">The optional charging product identification.</param>
        /// <param name="GetNewSession">Optionaly start or start not an new charging session.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<MobileAuthorizationStart>>

            MobileAuthorizeStart(EVSE_Id             EVSEId,
                                 EVCOIdWithPIN       EVCOIdWithPIN,
                                 String              ProductId          = null,
                                 Boolean?            GetNewSession      = null,

                                 DateTime?           Timestamp          = null,
                                 CancellationToken?  CancellationToken  = null,
                                 EventTracking_Id    EventTrackingId    = null,
                                 TimeSpan?           RequestTimeout     = null)

        {

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentNullException(nameof(EVSEId),        "The given EVSE identification must not be null!");

            if (EVCOIdWithPIN == null)
                throw new ArgumentNullException(nameof(EVCOIdWithPIN),  "The given e-mobility account identification with PIN must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;

            #endregion

            #region Send OnMobileAuthorizeStartRequest event

            try
            {

                OnMobileAuthorizeStartRequest?.Invoke(DateTime.Now,
                                                      Timestamp.Value,
                                                      this,
                                                      ClientId,
                                                      EventTrackingId,
                                                      EVSEId,
                                                      EVCOIdWithPIN,
                                                      ProductId,
                                                      GetNewSession,
                                                      RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(MobileClient) + "." + nameof(OnMobileAuthorizeStartRequest));
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

                var result = await _OICPClient.Query(MobileClient_XMLMethods.MobileAuthorizeStartXML(EVSEId,
                                                                                                     EVCOIdWithPIN,
                                                                                                     ProductId,
                                                                                                     GetNewSession),
                                                     "eRoamingMobileAuthorizeStart",
                                                     RequestLogDelegate:   OnMobileAuthorizeStartSOAPRequest,
                                                     ResponseLogDelegate:  OnMobileAuthorizeStartSOAPResponse,
                                                     CancellationToken:    CancellationToken,
                                                     EventTrackingId:      EventTrackingId,
                                                     QueryTimeout:         RequestTimeout,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(MobileAuthorizationStart.Parse),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                         return new HTTPResponse<MobileAuthorizationStart>(httpresponse,
                                                                                                           new MobileAuthorizationStart(StatusCodes.SystemError,
                                                                                                                                        httpresponse.Content.ToString()),
                                                                                                           IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, soapclient, httpresponse);

                                                         return new HTTPResponse<MobileAuthorizationStart>(httpresponse,
                                                                                                           new MobileAuthorizationStart(StatusCodes.SystemError,
                                                                                                                                                httpresponse.HTTPStatusCode.ToString(),
                                                                                                                                                httpresponse.HTTPBody.      ToUTF8String()),
                                                                                                           IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return HTTPResponse<MobileAuthorizationStart>.ExceptionThrown(new MobileAuthorizationStart(StatusCodes.SystemError,
                                                                                                                                                    exception.Message,
                                                                                                                                                    exception.StackTrace),
                                                                                                                       Exception: exception);

                                                     }

                                                     #endregion

                                                    );


                #region Send OnMobileAuthorizeStartResponse event

                try
                {

                    OnMobileAuthorizeStartResponse?.Invoke(DateTime.Now,
                                                           Timestamp.Value,
                                                           this,
                                                           ClientId,
                                                           EventTrackingId,
                                                           EVSEId,
                                                           EVCOIdWithPIN,
                                                           ProductId,
                                                           GetNewSession,
                                                           RequestTimeout,
                                                           result.Content,
                                                           DateTime.Now - Timestamp.Value);

                }
                catch (Exception e)
                {
                    e.Log(nameof(MobileClient) + "." + nameof(OnMobileAuthorizeStartResponse));
                }

                #endregion


                return result;

            }

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
                                                                                                  new Acknowledgement(StatusCodes.SystemError,
                                                                                                                      httpresponse.Content.ToString()),
                                                                                                  IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, soapclient, httpresponse);

                                                         return new HTTPResponse<Acknowledgement>(httpresponse,
                                                                                                  new Acknowledgement(StatusCodes.SystemError,
                                                                                                                      httpresponse.HTTPStatusCode.ToString(),
                                                                                                                      httpresponse.HTTPBody.      ToUTF8String()),
                                                                                                  IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return HTTPResponse<Acknowledgement>.ExceptionThrown(new Acknowledgement(StatusCodes.ServiceNotAvailable,
                                                                                                                                  exception.Message,
                                                                                                                                  exception.StackTrace,
                                                                                                                                  SessionId),
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
                                                                                                  new Acknowledgement(StatusCodes.SystemError,
                                                                                                                      httpresponse.Content.ToString()),
                                                                                                  IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, soapclient, httpresponse);

                                                         return new HTTPResponse<Acknowledgement>(httpresponse,
                                                                                                  new Acknowledgement(StatusCodes.SystemError,
                                                                                                                      httpresponse.HTTPStatusCode.ToString(),
                                                                                                                      httpresponse.HTTPBody.      ToUTF8String()),
                                                                                                  IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return HTTPResponse<Acknowledgement>.ExceptionThrown(new Acknowledgement(StatusCodes.ServiceNotAvailable,
                                                                                                                                  exception.Message,
                                                                                                                                  exception.StackTrace,
                                                                                                                                  SessionId),
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
