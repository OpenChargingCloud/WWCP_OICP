/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
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

namespace org.GraphDefined.WWCP.OICPv2_2.Mobile
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
        /// The default URL prefix.
        /// </summary>
        public     static readonly HTTPPath DefaultURLPrefix                = HTTPPath.Parse("/ibis/ws");

        /// <summary>
        /// The default HTTP/SOAP/XML URL for OICP MobileAuthorization requests.
        /// </summary>
        public     const           String  DefaultMobileAuthorizationURL   = "/eRoamingMobileAuthorization_V2.0";

        #endregion

        #region Properties

        /// <summary>
        /// The HTTP/SOAP/XML URL for OICP Mobile Authorization requests.
        /// </summary>
        public String              MobileAuthorizationURL    { get; }

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

        #region MobileRemoteStart   (SOAP)RequestMapper

        #region CustomMobileRemoteStartRequestMapper

        private Func<MobileRemoteStartRequest, MobileRemoteStartRequest> _CustomMobileRemoteStartRequestMapper = _ => _;

        public Func<MobileRemoteStartRequest, MobileRemoteStartRequest> CustomMobileRemoteStartRequestMapper
        {

            get
            {
                return _CustomMobileRemoteStartRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomMobileRemoteStartRequestMapper = value;
            }

        }

        #endregion

        #region CustomMobileRemoteStartSOAPRequestMapper

        private Func<MobileRemoteStartRequest, XElement, XElement> _CustomMobileRemoteStartSOAPRequestMapper = (request, xml) => xml;

        public Func<MobileRemoteStartRequest, XElement, XElement> CustomMobileRemoteStartSOAPRequestMapper
        {

            get
            {
                return _CustomMobileRemoteStartSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomMobileRemoteStartSOAPRequestMapper = value;
            }

        }

        #endregion

        #endregion

        #region MobileRemoteStop    (SOAP)RequestMapper

        #region CustomMobileRemoteStopRequestMapper

        private Func<MobileRemoteStopRequest, MobileRemoteStopRequest> _CustomMobileRemoteStopRequestMapper = _ => _;

        public Func<MobileRemoteStopRequest, MobileRemoteStopRequest> CustomMobileRemoteStopRequestMapper
        {

            get
            {
                return _CustomMobileRemoteStopRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomMobileRemoteStopRequestMapper = value;
            }

        }

        #endregion

        #region CustomMobileRemoteStopSOAPRequestMapper

        private Func<MobileRemoteStopRequest, XElement, XElement> _CustomMobileRemoteStopSOAPRequestMapper = (request, xml) => xml;

        public Func<MobileRemoteStopRequest, XElement, XElement> CustomMobileRemoteStopSOAPRequestMapper
        {

            get
            {
                return _CustomMobileRemoteStopSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomMobileRemoteStopSOAPRequestMapper = value;
            }

        }

        #endregion

        #endregion

        public CustomXMLSerializerDelegate<MobileAuthorizeStartRequest>            CustomMobileAuthorizeStartRequestSerializer    { get; set; }
        public CustomXMLParserDelegate<MobileAuthorizationStart>                   CustomMobileAuthorizationStartParser           { get; set; }

        public CustomXMLSerializerDelegate<MobileRemoteStartRequest>               CustomMobileRemoteStartRequestSerializer       { get; set; }
        public CustomXMLSerializerDelegate<MobileRemoteStopRequest>                CustomMobileRemoteStopRequestSerializer        { get; set; }

        public CustomXMLParserDelegate<Address>                                    CustomAddressParser                            { get; set; }
        public CustomXMLParserDelegate<StatusCode>                                 CustomStatusCodeParser                         { get; set; }

        public CustomXMLParserDelegate<Acknowledgement<MobileRemoteStartRequest>>  CustomAcknowledgementMobileRemoteStartParser   { get; set; }
        public CustomXMLParserDelegate<Acknowledgement<MobileRemoteStopRequest>>   CustomAcknowledgementMobileRemoteStopParser    { get; set; }

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
        /// <param name="ClientCertificateSelector">A delegate to select a TLS client certificate.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual host name to use.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent to use.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// <param name="MaxNumberOfRetries">The default number of maximum transmission retries.</param>
        /// <param name="DNSClient">An optional DNS client.</param>
        /// <param name="LoggingContext">An optional context for logging client methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        public MobileClient(String                               ClientId,
                            HTTPHostname                         Hostname,
                            IPPort?                              RemotePort                   = null,
                            RemoteCertificateValidationCallback  RemoteCertificateValidator   = null,
                            LocalCertificateSelectionCallback    ClientCertificateSelector    = null,
                            HTTPHostname?                        HTTPVirtualHost              = null,
                            HTTPPath?                             URLPrefix                    = null,
                            String                               MobileAuthorizationURL       = DefaultMobileAuthorizationURL,
                            String                               HTTPUserAgent                = DefaultHTTPUserAgent,
                            TimeSpan?                            RequestTimeout               = null,
                            Byte?                                MaxNumberOfRetries           = DefaultMaxNumberOfRetries,
                            DNSClient                            DNSClient                    = null,
                            String                               LoggingContext               = MobileClientLogger.DefaultContext,
                            LogfileCreatorDelegate               LogfileCreator               = null)

            : base(ClientId,
                   Hostname,
                   RemotePort ?? DefaultRemotePort,
                   RemoteCertificateValidator,
                   ClientCertificateSelector,
                   HTTPVirtualHost,
                   URLPrefix ?? DefaultURLPrefix,
                   null,
                   HTTPUserAgent,
                   RequestTimeout,
                   null,
                   MaxNumberOfRetries,
                   DNSClient)

        {

            #region Initial checks

            if (ClientId.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(ClientId), "The given client identification must not be null or empty!");

            #endregion

            this.MobileAuthorizationURL  = MobileAuthorizationURL ?? DefaultMobileAuthorizationURL;

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
        /// <param name="ClientCertificateSelector">A delegate to select a TLS client certificate.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual host name to use.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent to use.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// <param name="MaxNumberOfRetries">The default number of maximum transmission retries.</param>
        /// <param name="DNSClient">An optional DNS client.</param>
        public MobileClient(String                               ClientId,
                            MobileClientLogger                   Logger,
                            HTTPHostname                         Hostname,
                            IPPort?                              RemotePort                   = null,
                            RemoteCertificateValidationCallback  RemoteCertificateValidator   = null,
                            LocalCertificateSelectionCallback    ClientCertificateSelector    = null,
                            HTTPHostname?                        HTTPVirtualHost              = null,
                            HTTPPath?                             URLPrefix                    = null,
                            String                               MobileAuthorizationURL       = DefaultMobileAuthorizationURL,
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
                   URLPrefix ?? DefaultURLPrefix,
                   null,
                   HTTPUserAgent,
                   RequestTimeout,
                   null,
                   MaxNumberOfRetries,
                   DNSClient)

        {

            #region Initial checks

            if (ClientId.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(ClientId), "The given client identification must not be null or empty!");

            #endregion

            this.MobileAuthorizationURL  = MobileAuthorizationURL ?? DefaultMobileAuthorizationURL;

            this.Logger                  = Logger                 ?? throw new ArgumentNullException(nameof(Logger), "The given mobile client logger must not be null!");

        }

        #endregion

        #endregion


        #region MobileAuthorizeStart(Request)

        /// <summary>
        /// Authorize for starting a remote charging session (later).
        /// </summary>
        /// <param name="Request">A MobileAuthorizeStart request.</param>
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

            var StartTime = DateTime.UtcNow;

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
                                                    URLPathPrefix + MobileAuthorizationURL,
                                                    VirtualHostname,
                                                    RemotePort,
                                                    RemoteCertificateValidator,
                                                    ClientCertificateSelector,
                                                    UserAgent,
                                                    RequestTimeout,
                                                    DNSClient))

            {

                result = await _OICPClient.Query(_CustomMobileAuthorizeStartSOAPRequestMapper(Request,
                                                                                              SOAP.Encapsulation(Request.ToXML(CustomMobileAuthorizeStartRequestSerializer))),
                                                 "eRoamingMobileAuthorizeStart",
                                                 RequestLogDelegate:   OnMobileAuthorizeStartSOAPRequest,
                                                 ResponseLogDelegate:  OnMobileAuthorizeStartSOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 RequestTimeout:         Request.RequestTimeout ?? RequestTimeout.Value,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                      (request, xml, onexception) =>
                                                                                                          MobileAuthorizationStart.Parse(request,
                                                                                                                                         xml,
                                                                                                                                         CustomMobileAuthorizationStartParser,
                                                                                                                                         CustomAddressParser,
                                                                                                                                         CustomStatusCodeParser,
                                                                                                                                         onexception)),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                     return new HTTPResponse<MobileAuthorizationStart>(httpresponse,
                                                                                                       new MobileAuthorizationStart(
                                                                                                           Request,
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
                                                                                                           Request,
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
                                                                                                                       Request,
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
                                 Request,
                                 StatusCodes.SystemError,
                                 "HTTP request failed!"
                             )
                         );


            #region Send OnMobileAuthorizeStartResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                if (OnMobileAuthorizeStartResponse != null)
                    await Task.WhenAll(OnMobileAuthorizeStartResponse.GetInvocationList().
                                       Cast<OnMobileAuthorizeStartResponseHandler>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
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

        #region MobileRemoteStart   (Request)

        /// <summary>
        /// Start a remote charging session.
        /// </summary>
        /// <param name="Request">A MobileRemoteStart request.</param>
        public async Task<HTTPResponse<Acknowledgement<MobileRemoteStartRequest>>>

            MobileRemoteStart(MobileRemoteStartRequest  Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The given MobileRemoteStart request must not be null!");

            Request = _CustomMobileRemoteStartRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The mapped MobileRemoteStart request must not be null!");


            HTTPResponse<Acknowledgement<MobileRemoteStartRequest>> result = null;

            #endregion

            #region Send OnMobileRemoteStartRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                if (OnMobileRemoteStartRequest != null)
                    await Task.WhenAll(OnMobileRemoteStartRequest.GetInvocationList().
                                       Cast<OnMobileRemoteStartRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.SessionId,
                                                     Request.RequestTimeout ?? RequestTimeout.Value))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(MobileClient) + "." + nameof(OnMobileRemoteStartRequest));
            }

            #endregion


            using (var _OICPClient = new SOAPClient(Hostname,
                                                    URLPathPrefix + MobileAuthorizationURL,
                                                    VirtualHostname,
                                                    RemotePort,
                                                    RemoteCertificateValidator,
                                                    ClientCertificateSelector,
                                                    UserAgent,
                                                    RequestTimeout,
                                                    DNSClient))

            {

                result = await _OICPClient.Query(_CustomMobileRemoteStartSOAPRequestMapper(Request,
                                                                                           SOAP.Encapsulation(Request.ToXML(CustomMobileRemoteStartRequestSerializer))),
                                                 "eRoamingMobileRemoteStart",
                                                 RequestLogDelegate:   OnMobileRemoteStartSOAPRequest,
                                                 ResponseLogDelegate:  OnMobileRemoteStartSOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 RequestTimeout:         Request.RequestTimeout ?? RequestTimeout.Value,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                      (request, xml, onexception) =>
                                                                                                          Acknowledgement<MobileRemoteStartRequest>.Parse(request,
                                                                                                                                                          xml,
                                                                                                                                                          CustomAcknowledgementMobileRemoteStartParser,
                                                                                                                                                          CustomStatusCodeParser,
                                                                                                                                                          onexception)),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<Acknowledgement<MobileRemoteStartRequest>>(

                                                                httpresponse,

                                                                new Acknowledgement<MobileRemoteStartRequest>(
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

                                                     return new HTTPResponse<Acknowledgement<MobileRemoteStartRequest>>(

                                                                httpresponse,

                                                                new Acknowledgement<MobileRemoteStartRequest>(
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

                                                     return HTTPResponse<Acknowledgement<MobileRemoteStartRequest>>.ExceptionThrown(

                                                            new Acknowledgement<MobileRemoteStartRequest>(
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
                result = HTTPResponse<Acknowledgement<MobileRemoteStartRequest>>.OK(
                             new Acknowledgement<MobileRemoteStartRequest>(
                                 Request,
                                 StatusCodes.SystemError,
                                 "HTTP request failed!"
                             )
                         );


            #region Send OnMobileRemoteStartResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                if (OnMobileRemoteStartResponse != null)
                    await Task.WhenAll(OnMobileRemoteStartResponse.GetInvocationList().
                                       Cast<OnMobileRemoteStartResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.SessionId,
                                                     Request.RequestTimeout ?? RequestTimeout.Value,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(MobileClient) + "." + nameof(OnMobileRemoteStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region MobileRemoteStop    (Request)

        /// <summary>
        /// Stop a remote charging session.
        /// </summary>
        /// <param name="Request">A MobileRemoteStop request.</param>
        public async Task<HTTPResponse<Acknowledgement<MobileRemoteStopRequest>>>

            MobileRemoteStop(MobileRemoteStopRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The given MobileRemoteStop request must not be null!");

            Request = _CustomMobileRemoteStopRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request),  "The mapped MobileRemoteStop request must not be null!");


            HTTPResponse<Acknowledgement<MobileRemoteStopRequest>> result = null;

            #endregion

            #region Send OnMobileRemoteStopRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                if (OnMobileRemoteStopRequest != null)
                    await Task.WhenAll(OnMobileRemoteStopRequest.GetInvocationList().
                                       Cast<OnMobileRemoteStopRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.SessionId,
                                                     Request.RequestTimeout ?? RequestTimeout.Value))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(MobileClient) + "." + nameof(OnMobileRemoteStopRequest));
            }

            #endregion


            using (var _OICPClient = new SOAPClient(Hostname,
                                                    URLPathPrefix + MobileAuthorizationURL,
                                                    VirtualHostname,
                                                    RemotePort,
                                                    RemoteCertificateValidator,
                                                    ClientCertificateSelector,
                                                    UserAgent,
                                                    RequestTimeout,
                                                    DNSClient))

            {

                result = await _OICPClient.Query(_CustomMobileRemoteStopSOAPRequestMapper(Request,
                                                                                          SOAP.Encapsulation(Request.ToXML(CustomMobileRemoteStopRequestSerializer))),
                                                 "eRoamingMobileRemoteStop",
                                                 RequestLogDelegate:   OnMobileRemoteStartSOAPRequest,
                                                 ResponseLogDelegate:  OnMobileRemoteStartSOAPResponse,
                                                 CancellationToken:    Request.CancellationToken,
                                                 EventTrackingId:      Request.EventTrackingId,
                                                 RequestTimeout:         Request.RequestTimeout ?? RequestTimeout.Value,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                      (request, xml, onexception) =>
                                                                                                          Acknowledgement<MobileRemoteStopRequest>.Parse(request,
                                                                                                                                                         xml,
                                                                                                                                                         CustomAcknowledgementMobileRemoteStopParser,
                                                                                                                                                         CustomStatusCodeParser,
                                                                                                                                                         onexception)),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<Acknowledgement<MobileRemoteStopRequest>>(

                                                                httpresponse,

                                                                new Acknowledgement<MobileRemoteStopRequest>(
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

                                                     return new HTTPResponse<Acknowledgement<MobileRemoteStopRequest>>(

                                                                httpresponse,

                                                                new Acknowledgement<MobileRemoteStopRequest>(
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

                                                     return HTTPResponse<Acknowledgement<MobileRemoteStopRequest>>.ExceptionThrown(

                                                            new Acknowledgement<MobileRemoteStopRequest>(
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
                result = HTTPResponse<Acknowledgement<MobileRemoteStopRequest>>.OK(
                             new Acknowledgement<MobileRemoteStopRequest>(
                                 Request,
                                 StatusCodes.SystemError,
                                 "HTTP request failed!"
                             )
                         );


            #region Send OnMobileRemoteStopResponse event

            var Endtime = DateTime.UtcNow;

            try
            {

                if (OnMobileRemoteStopResponse != null)
                    await Task.WhenAll(OnMobileRemoteStopResponse.GetInvocationList().
                                       Cast<OnMobileRemoteStopResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     Request.EventTrackingId,
                                                     Request.SessionId,
                                                     Request.RequestTimeout ?? RequestTimeout.Value,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                e.Log(nameof(MobileClient) + "." + nameof(OnMobileRemoteStopResponse));
            }

            #endregion

            return result;

        }

        #endregion


    }

}
