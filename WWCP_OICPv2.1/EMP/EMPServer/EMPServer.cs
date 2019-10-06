/*
 * Copyright (c) 2014-2019 GraphDefined GmbH
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
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using System.Security.Authentication;
using System.Net.Security;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.EMP
{

    /// <summary>
    /// An OICP EMP HTTP/SOAP/XML server.
    /// </summary>
    public class EMPServer : ASOAPServer
    {

        #region Data

        /// <summary>
        /// The default HTTP/SOAP/XML server name.
        /// </summary>
        public new const           String           DefaultHTTPServerName      = "GraphDefined OICP " + Version.Number + " HTTP/SOAP/XML EMP API";

        /// <summary>
        /// The default HTTP/SOAP/XML server TCP port.
        /// </summary>
        public new static readonly IPPort           DefaultHTTPServerPort      = IPPort.Parse(2003);

        /// <summary>
        /// The default HTTP/SOAP/XML server URI prefix.
        /// </summary>
        public new static readonly HTTPPath          DefaultURIPrefix           = HTTPPath.Parse("/");

        /// <summary>
        /// The default HTTP/SOAP/XML URI for OICP authorization requests.
        /// </summary>
        public     const           String           DefaultAuthorizationURI    = "/Authorization";

        /// <summary>
        /// The default HTTP/SOAP/XML content type.
        /// </summary>
        public new static readonly HTTPContentType  DefaultContentType         = HTTPContentType.XMLTEXT_UTF8;

        /// <summary>
        /// The default request timeout.
        /// </summary>
        public new static readonly TimeSpan         DefaultRequestTimeout      = TimeSpan.FromMinutes(1);

        #endregion

        #region Properties

        /// <summary>
        /// The identification of this HTTP/SOAP service.
        /// </summary>
        public String  ServiceId           { get; }

        /// <summary>
        /// The HTTP/SOAP/XML URI for OICP authorization requests.
        /// </summary>
        public String  AuthorizationURI    { get; }

        #endregion

        #region Custom request/response mappers

        public CustomXMLParserDelegate<CPO.AuthorizeStartRequest>  CustomAuthorizeStartRequestParser   { get; set; }
        public CustomXMLParserDelegate<CPO.AuthorizeStopRequest>   CustomAuthorizeStopRequestParser    { get; set; }
        public CustomXMLParserDelegate<Identification>             CustomIdentificationParser          { get; set; }

        public CustomXMLParserDelegate<ChargeDetailRecord>         CustomChargeDetailRecordParser      { get; set; }


        public OnExceptionDelegate                                 OnException                         { get; set; }

        #endregion

        #region Events

        #region OnAuthorizeStart

        /// <summary>
        /// An event sent whenever a authorize start SOAP request was received.
        /// </summary>
        public event RequestLogHandler                  OnAuthorizeStartSOAPRequest;

        /// <summary>
        /// An event sent whenever a authorize start request was received.
        /// </summary>
        public event OnAuthorizeStartRequestDelegate    OnAuthorizeStartRequest;

        /// <summary>
        /// An event sent whenever a authorize start command was received.
        /// </summary>
        public event OnAuthorizeStartDelegate           OnAuthorizeStart;

        /// <summary>
        /// An event sent whenever a authorize start response was sent.
        /// </summary>
        public event OnAuthorizeStartResponseDelegate   OnAuthorizeStartResponse;

        /// <summary>
        /// An event sent whenever a authorize start SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                   OnAuthorizeStartSOAPResponse;

        #endregion

        #region OnAuthorizeStop

        /// <summary>
        /// An event sent whenever a authorize start SOAP request was received.
        /// </summary>
        public event RequestLogHandler                OnAuthorizeStopSOAPRequest;

        /// <summary>
        /// An event sent whenever a authorize stop request was received.
        /// </summary>
        public event OnAuthorizeStopRequestHandler    OnAuthorizeStopRequest;

        /// <summary>
        /// An event sent whenever a authorize stop command was received.
        /// </summary>
        public event OnAuthorizeStopDelegate          OnAuthorizeStop;

        /// <summary>
        /// An event sent whenever a authorize stop response was sent.
        /// </summary>
        public event OnAuthorizeStopResponseHandler   OnAuthorizeStopResponse;

        /// <summary>
        /// An event sent whenever a authorize stop SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                 OnAuthorizeStopSOAPResponse;

        #endregion

        #region OnChargeDetailRecord

        /// <summary>
        /// An event sent whenever a charge detail record SOAP request was received.
        /// </summary>
        public event RequestLogHandler                      OnChargeDetailRecordSOAPRequest;

        /// <summary>
        /// An event sent whenever a charge detail record request was received.
        /// </summary>
        public event OnChargeDetailRecordRequestHandler     OnChargeDetailRecordRequest;

        /// <summary>
        /// An event sent whenever a charge detail record was received.
        /// </summary>
        public event OnChargeDetailRecordDelegate           OnChargeDetailRecord;

        /// <summary>
        /// An event sent whenever a charge detail record response was sent.
        /// </summary>
        public event OnChargeDetailRecordResponseHandler    OnChargeDetailRecordResponse;

        /// <summary>
        /// An event sent whenever a charge detail record SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                       OnChargeDetailRecordSOAPResponse;

        #endregion

        #endregion

        #region Constructor(s)

        #region EMPServer(HTTPServerName, ServiceId = null, TCPPort = default, URIPrefix = default, AuthorizationURI = default, ContentType = default, DNSClient = null, AutoStart = false)

        /// <summary>
        /// Initialize an new HTTP server for the OICP HTTP/SOAP/XML EMP API.
        /// </summary>
        /// <param name="HTTPServerName">An optional identification string for the HTTP server.</param>
        /// <param name="ServiceId">An optional identification for this SOAP service.</param>
        /// <param name="TCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="ServerCertificateSelector">An optional delegate to select a SSL/TLS server certificate.</param>
        /// <param name="ClientCertificateValidator">An optional delegate to verify the SSL/TLS client certificate used for authentication.</param>
        /// <param name="ClientCertificateSelector">An optional delegate to select the SSL/TLS client certificate used for authentication.</param>
        /// <param name="AllowedTLSProtocols">The SSL/TLS protocol(s) allowed for this connection.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="AuthorizationURI">An alternative HTTP/SOAP/XML URI for OICP authorization requests.</param>
        /// <param name="ContentType">An optional HTTP content type to use.</param>
        /// <param name="RegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="AutoStart">Start the server immediately.</param>
        public EMPServer(String                               HTTPServerName               = DefaultHTTPServerName,
                         String                               ServiceId                    = null,
                         IPPort?                              TCPPort                      = null,
                         ServerCertificateSelectorDelegate    ServerCertificateSelector    = null,
                         RemoteCertificateValidationCallback  ClientCertificateValidator   = null,
                         LocalCertificateSelectionCallback    ClientCertificateSelector    = null,
                         SslProtocols                         AllowedTLSProtocols          = SslProtocols.Tls12,
                         HTTPPath?                             URIPrefix                    = null,
                         String                               AuthorizationURI             = DefaultAuthorizationURI,
                         HTTPContentType                      ContentType                  = null,
                         Boolean                              RegisterHTTPRootService      = true,
                         DNSClient                            DNSClient                    = null,
                         Boolean                              AutoStart                    = false)

            : base(HTTPServerName.IsNotNullOrEmpty() ? HTTPServerName : DefaultHTTPServerName,
                   TCPPort     ?? DefaultHTTPServerPort,
                   ServerCertificateSelector,
                   ClientCertificateValidator,
                   ClientCertificateSelector,
                   AllowedTLSProtocols,
                   URIPrefix   ?? DefaultURIPrefix,
                   ContentType ?? DefaultContentType,
                   RegisterHTTPRootService,
                   DNSClient,
                   AutoStart: false)

        {

            this.ServiceId         = ServiceId        ?? nameof(EMPServer);
            this.AuthorizationURI  = AuthorizationURI ?? DefaultAuthorizationURI;

            RegisterURITemplates();

            if (AutoStart)
                Start();

        }

        #endregion

        #region EMPServer(SOAPServer, ServiceId = null, URIPrefix = default, AuthorizationURI = default)

        /// <summary>
        /// Use the given SOAP server for the OICP HTTP/SOAP/XML EMP API.
        /// </summary>
        /// <param name="SOAPServer">A SOAP server.</param>
        /// <param name="ServiceId">An optional identification for this SOAP service.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="AuthorizationURI">An alternative HTTP/SOAP/XML URI for OICP authorization requests.</param>
        public EMPServer(SOAPServer  SOAPServer,
                         String      ServiceId          = null,
                         HTTPPath?    URIPrefix          = null,
                         String      AuthorizationURI   = DefaultAuthorizationURI)

            : base(SOAPServer,
                   URIPrefix ?? DefaultURIPrefix)

        {

            this.ServiceId         = ServiceId        ?? nameof(EMPServer);
            this.AuthorizationURI  = AuthorizationURI ?? DefaultAuthorizationURI;

            RegisterURITemplates();

        }

        #endregion

        #endregion


        #region (override) RegisterURITemplates()

        /// <summary>
        /// Register all URI templates for this SOAP API.
        /// </summary>
        protected void RegisterURITemplates()
        {

            #region /Authorization - AuthorizeStart

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + AuthorizationURI,
                                            "AuthorizeStart",
                                            XML => XML.Descendants(OICPNS.Authorization + "eRoamingAuthorizeStart").FirstOrDefault(),
                                            async (HTTPRequest, AuthorizeStartXML) => {


                CPO.AuthorizeStartRequest AuthorizeStartRequest  = null;
                CPO.AuthorizationStart    AuthorizationStart     = null;


                #region Send OnAuthorizeStartSOAPRequest event

                var StartTime = DateTime.Now;

                try
                {

                    if (OnAuthorizeStartSOAPRequest != null)
                        await Task.WhenAll(OnAuthorizeStartSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StartTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPServer) + "." + nameof(OnAuthorizeStartSOAPRequest));
                }

                                                #endregion

                try
                {

                    if (CPO.AuthorizeStartRequest.TryParse(AuthorizeStartXML,
                                                           out AuthorizeStartRequest,
                                                           CustomAuthorizeStartRequestParser,
                                                           CustomIdentificationParser,
                                                           OnException,

                                                           HTTPRequest.Timestamp,
                                                           HTTPRequest.CancellationToken,
                                                           HTTPRequest.EventTrackingId,
                                                           HTTPRequest.Timeout ?? DefaultRequestTimeout))
                    {

                        #region Send OnAuthorizeStartRequest event

                        try
                        {

                            if (OnAuthorizeStartRequest != null)
                                await Task.WhenAll(OnAuthorizeStartRequest.GetInvocationList().
                                                   Cast<OnAuthorizeStartRequestDelegate>().
                                                   Select(e => e(StartTime,
                                                                  AuthorizeStartRequest.Timestamp.Value,
                                                                  this,
                                                                  ServiceId,
                                                                  AuthorizeStartRequest.EventTrackingId,
                                                                  AuthorizeStartRequest.OperatorId,
                                                                  AuthorizeStartRequest.Identification,
                                                                  AuthorizeStartRequest.EVSEId,
                                                                  AuthorizeStartRequest.SessionId,
                                                                  AuthorizeStartRequest.PartnerProductId,
                                                                  AuthorizeStartRequest.PartnerSessionId,
                                                                  AuthorizeStartRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                                   ConfigureAwait(false);

                        }
                        catch (Exception e)
                        {
                            e.Log(nameof(EMPServer) + "." + nameof(OnAuthorizeStartRequest));
                        }

                        #endregion

                        #region Call async subscribers

                        if (OnAuthorizeStart != null)
                        {

                            var results = await Task.WhenAll(OnAuthorizeStart.GetInvocationList().
                                                                 Cast<OnAuthorizeStartDelegate>().
                                                                 Select(e => e(DateTime.Now,
                                                                               this,
                                                                               AuthorizeStartRequest))).
                                                                 ConfigureAwait(false);

                            AuthorizationStart = results.FirstOrDefault();

                        }

                        if (AuthorizationStart == null)
                            AuthorizationStart = CPO.AuthorizationStart.SystemError(
                                                                            AuthorizeStartRequest,
                                                                            "Could not process the incoming AuthorizationStart request!",
                                                                            null,
                                                                            AuthorizeStartRequest.SessionId,
                                                                            AuthorizeStartRequest.PartnerSessionId
                                                                        );

                        #endregion

                        #region Send OnAuthorizeStartResponse event

                        var EndTime = DateTime.Now;

                        try
                        {

                            if (OnAuthorizeStartResponse != null)
                                await Task.WhenAll(OnAuthorizeStartResponse.GetInvocationList().
                                                   Cast<OnAuthorizeStartResponseDelegate>().
                                                   Select(e => e(EndTime,
                                                                 this,
                                                                 ServiceId,
                                                                 AuthorizeStartRequest.EventTrackingId,
                                                                 AuthorizeStartRequest.OperatorId,
                                                                 AuthorizeStartRequest.Identification,
                                                                 AuthorizeStartRequest.EVSEId,
                                                                 AuthorizeStartRequest.SessionId,
                                                                 AuthorizeStartRequest.PartnerProductId,
                                                                 AuthorizeStartRequest.PartnerSessionId,
                                                                 AuthorizeStartRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                                 AuthorizationStart,
                                                                 EndTime - StartTime))).
                                                   ConfigureAwait(false);

                        }
                        catch (Exception e)
                        {
                            e.Log(nameof(EMPServer) + "." + nameof(OnAuthorizeStartResponse));
                        }

                        #endregion

                    }

                    else
                        AuthorizationStart = CPO.AuthorizationStart.DataError(
                                                 AuthorizeStartRequest,
                                                 "Could not process the incoming AuthorizeStart request!"
                                             );

                }
                catch (Exception e)
                {
                    AuthorizationStart = CPO.AuthorizationStart.DataError(
                        AuthorizeStartRequest,
                        e.Message,
                        e.StackTrace
                    );
                }


                #region Create SOAP response

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(AuthorizationStart.ToXML()).ToUTF8Bytes(),
                    Connection      = "close"
                };

                #endregion

                #region Send OnAuthorizeStartSOAPResponse event

                try
                {

                    if (OnAuthorizeStartSOAPResponse != null)
                        await Task.WhenAll(OnAuthorizeStartSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPServer) + "." + nameof(OnAuthorizeStartSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region /Authorization - AuthorizeStop

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + AuthorizationURI,
                                            "AuthorizeStop",
                                            XML => XML.Descendants(OICPNS.Authorization + "eRoamingAuthorizeStop").FirstOrDefault(),
                                            async (HTTPRequest, AuthorizeStopXML) => {


                CPO.AuthorizeStopRequest AuthorizeStopRequest  = null;
                CPO.AuthorizationStop    AuthorizationStop     = null;

                #region Send OnAuthorizeStopSOAPRequest event

                var StartTime = DateTime.Now;

                try
                {

                    if (OnAuthorizeStopSOAPRequest != null)
                        await Task.WhenAll(OnAuthorizeStopSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StartTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPServer) + "." + nameof(OnAuthorizeStartSOAPRequest));
                }

                #endregion


                try
                {

                    if (CPO.AuthorizeStopRequest.TryParse(AuthorizeStopXML,
                                                          out AuthorizeStopRequest,
                                                          CustomAuthorizeStopRequestParser,
                                                          CustomIdentificationParser,
                                                          OnException,

                                                          HTTPRequest.Timestamp,
                                                          HTTPRequest.CancellationToken,
                                                          HTTPRequest.EventTrackingId,
                                                          HTTPRequest.Timeout ?? DefaultRequestTimeout))
                    {

                        #region Send OnAuthorizeStopRequest event

                        try
                        {

                            if (OnAuthorizeStopRequest != null)
                                await Task.WhenAll(OnAuthorizeStopRequest.GetInvocationList().
                                                   Cast<OnAuthorizeStopRequestHandler>().
                                                   Select(e => e(StartTime,
                                                                 AuthorizeStopRequest.Timestamp.Value,
                                                                 this,
                                                                 ServiceId,
                                                                 AuthorizeStopRequest.EventTrackingId,
                                                                 AuthorizeStopRequest.SessionId,
                                                                 AuthorizeStopRequest.PartnerSessionId,
                                                                 AuthorizeStopRequest.OperatorId,
                                                                 AuthorizeStopRequest.EVSEId,
                                                                 AuthorizeStopRequest.Identification,
                                                                 AuthorizeStopRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                                   ConfigureAwait(false);

                        }
                        catch (Exception e)
                        {
                            e.Log(nameof(EMPServer) + "." + nameof(OnAuthorizeStopRequest));
                        }

                        #endregion

                        #region Call async subscribers

                        if (OnAuthorizeStop != null)
                        {

                            var results = await Task.WhenAll(OnAuthorizeStop.GetInvocationList().
                                                                 Cast<OnAuthorizeStopDelegate>().
                                                                 Select(e => e(DateTime.Now,
                                                                               this,
                                                                               AuthorizeStopRequest))).
                                                                 ConfigureAwait(false);

                            AuthorizationStop = results.FirstOrDefault();

                        }

                        if (AuthorizationStop == null)
                            AuthorizationStop = CPO.AuthorizationStop.SystemError(
                                                    null,
                                                    "Could not process the incoming AuthorizeStop request!",
                                                    null,
                                                    AuthorizeStopRequest.SessionId,
                                                    AuthorizeStopRequest.PartnerSessionId
                                                );

                        #endregion

                        #region Send OnAuthorizeStopResponse event

                        var EndTime = DateTime.Now;

                        try
                        {

                            if (OnAuthorizeStopResponse != null)
                                await Task.WhenAll(OnAuthorizeStopResponse.GetInvocationList().
                                                   Cast<OnAuthorizeStopResponseHandler>().
                                                   Select(e => e(EndTime,
                                                                 this,
                                                                 ServiceId,
                                                                 AuthorizeStopRequest.EventTrackingId,
                                                                 AuthorizeStopRequest.SessionId,
                                                                 AuthorizeStopRequest.PartnerSessionId,
                                                                 AuthorizeStopRequest.OperatorId,
                                                                 AuthorizeStopRequest.EVSEId,
                                                                 AuthorizeStopRequest.Identification,
                                                                 AuthorizeStopRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                                 AuthorizationStop,
                                                                 EndTime - StartTime))).
                                                   ConfigureAwait(false);

                        }
                        catch (Exception e)
                        {
                            e.Log(nameof(EMPServer) + "." + nameof(OnAuthorizeStopResponse));
                        }

                        #endregion

                    }

                    else
                        AuthorizationStop = CPO.AuthorizationStop.DataError(
                                                AuthorizeStopRequest,
                                                "Could not process the incoming AuthorizeStop request!"
                                            );

                }
                catch (Exception e)
                {
                    AuthorizationStop = CPO.AuthorizationStop.DataError(
                        AuthorizeStopRequest,
                        e.Message,
                        e.StackTrace
                    );
                }


                #region Create SOAP response

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(AuthorizationStop.ToXML()).ToUTF8Bytes(),
                    Connection      = "close"
                };

                #endregion

                #region Send OnAuthorizeStopSOAPResponse event

                try
                {

                    if (OnAuthorizeStopSOAPResponse != null)
                        await Task.WhenAll(OnAuthorizeStopSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPServer) + "." + nameof(OnAuthorizeStopSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region /Authorization - ChargeDetailRecord

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + AuthorizationURI,
                                            "ChargeDetailRecord",
                                            XML => XML.Descendants(OICPNS.Authorization + "eRoamingChargeDetailRecord").FirstOrDefault(),
                                            async (HTTPRequest, ChargeDetailRecordXML) => {

                CPO.SendChargeDetailRecordRequest                  SendChargeDetailRecordRequest  = null;
                Acknowledgement<CPO.SendChargeDetailRecordRequest> Acknowledgement                = null;

                #region Send OnChargeDetailRecordSOAPRequest event

                var StartTime = DateTime.Now;

                try
                {

                    if (OnChargeDetailRecordSOAPRequest != null)
                        await Task.WhenAll(OnChargeDetailRecordSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StartTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPServer) + "." + nameof(OnChargeDetailRecordSOAPRequest));
                }

                #endregion

                try
                {

                    if (CPO.SendChargeDetailRecordRequest.TryParse(ChargeDetailRecordXML,
                                                                   out SendChargeDetailRecordRequest,
                                                                   CustomChargeDetailRecordParser,
                                                                   CustomIdentificationParser,
                                                                   OnException,

                                                                   HTTPRequest.Timestamp,
                                                                   HTTPRequest.CancellationToken,
                                                                   HTTPRequest.EventTrackingId,
                                                                   HTTPRequest.Timeout ?? DefaultRequestTimeout))
                    {

                        #region Send OnChargeDetailRecordRequest event

                        try
                        {

                            if (OnChargeDetailRecordRequest != null)
                                await Task.WhenAll(OnChargeDetailRecordRequest.GetInvocationList().
                                                   Cast<OnChargeDetailRecordRequestHandler>().
                                                   Select(e => e(StartTime,
                                                                 SendChargeDetailRecordRequest.Timestamp.Value,
                                                                 this,
                                                                 ServiceId,
                                                                 SendChargeDetailRecordRequest.EventTrackingId,
                                                                 SendChargeDetailRecordRequest.ChargeDetailRecord,
                                                                 SendChargeDetailRecordRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                                   ConfigureAwait(false);

                        }
                        catch (Exception e)
                        {
                            e.Log(nameof(EMPServer) + "." + nameof(OnChargeDetailRecordRequest));
                        }

                        #endregion

                        #region Call async subscribers

                        if (OnChargeDetailRecord != null)
                        {

                            var results = await Task.WhenAll(OnChargeDetailRecord.GetInvocationList().
                                                                 Cast<OnChargeDetailRecordDelegate>().
                                                                 Select(e => e(DateTime.Now,
                                                                               this,
                                                                               SendChargeDetailRecordRequest))).
                                                                 ConfigureAwait(false);

                            Acknowledgement = results.FirstOrDefault();

                        }

                        if (Acknowledgement == null)
                            Acknowledgement = Acknowledgement<CPO.SendChargeDetailRecordRequest>.SystemError(
                                                  null,
                                                  "Could not process the incoming SendChargeDetailRecordRequest request!",
                                                  null
                                              );

                        #endregion

                        #region Send OnChargeDetailRecordResponse event

                        var EndTime = DateTime.Now;

                        try
                        {

                            if (OnChargeDetailRecordResponse != null)
                                await Task.WhenAll(OnChargeDetailRecordResponse.GetInvocationList().
                                                   Cast<OnChargeDetailRecordResponseHandler>().
                                                   Select(e => e(EndTime,
                                                                 this,
                                                                 ServiceId,
                                                                 SendChargeDetailRecordRequest.EventTrackingId,
                                                                 SendChargeDetailRecordRequest.ChargeDetailRecord,
                                                                 SendChargeDetailRecordRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                                 Acknowledgement,
                                                                 EndTime - StartTime))).
                                                   ConfigureAwait(false);

                        }
                        catch (Exception e)
                        {
                            e.Log(nameof(EMPServer) + "." + nameof(OnChargeDetailRecordResponse));
                        }

                        #endregion

                    }

                    else
                        Acknowledgement = Acknowledgement<CPO.SendChargeDetailRecordRequest>.DataError(
                                              SendChargeDetailRecordRequest,
                                              "Could not process the incoming SendChargeDetailRecord request!"
                                          );

                }
                catch (Exception e)
                {
                    Acknowledgement = Acknowledgement<CPO.SendChargeDetailRecordRequest>.DataError(
                        SendChargeDetailRecordRequest,
                        e.Message,
                        e.StackTrace
                    );
                }


                #region Create SOAP response

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(Acknowledgement.ToXML()).ToUTF8Bytes(),
                    Connection      = "close"
                };

                #endregion

                #region Send OnChargeDetailRecordSOAPResponse event

                try
                {

                    if (OnChargeDetailRecordSOAPResponse != null)
                        await Task.WhenAll(OnChargeDetailRecordSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPServer) + "." + nameof(OnChargeDetailRecordSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

        }

        #endregion


    }

}
