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
using System.Net.Security;
using System.Threading.Tasks;
using System.Security.Authentication;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2.EMP
{

    /// <summary>
    /// The EMP HTTP/SOAP/XML server.
    /// </summary>
    public class EMPSOAPServer : ASOAPServer
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
        /// The default HTTP/SOAP/XML server URL prefix.
        /// </summary>
        public new static readonly HTTPPath         DefaultURLPathPrefix           = HTTPPath.Parse("/");

        /// <summary>
        /// The default HTTP/SOAP/XML URL for OICP authorization requests.
        /// </summary>
        public     const           String           DefaultAuthorizationURL    = "/Authorization";

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
        public String  ServiceName         { get; }

        /// <summary>
        /// The HTTP/SOAP/XML URL for OICP authorization requests.
        /// </summary>
        public String  AuthorizationURL    { get; }

        #endregion

        #region Custom request/response mappers

        public CustomXMLParserDelegate<CPO.AuthorizeStartRequest>  CustomAuthorizeStartRequestParser   { get; set; }
        public CustomXMLParserDelegate<CPO.AuthorizeStopRequest>   CustomAuthorizeStopRequestParser    { get; set; }
        public CustomXMLParserDelegate<Identification>             CustomIdentificationParser          { get; set; }

        public CustomXMLParserDelegate<RFIDIdentification>         CustomRFIDIdentificationParser      { get; set; }

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

        #region EMPServer(HTTPServerName, ServiceName = null, TCPPort = default, URLPrefix = default, AuthorizationURL = default, ContentType = default, DNSClient = null, AutoStart = false)

        /// <summary>
        /// Initialize an new HTTP server for the OICP HTTP/SOAP/XML EMP API.
        /// </summary>
        /// <param name="HTTPServerName">An optional identification string for the HTTP server.</param>
        /// <param name="ServiceName">An optional identification for this SOAP service.</param>
        /// <param name="TCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="ServerCertificateSelector">An optional delegate to select a SSL/TLS server certificate.</param>
        /// <param name="ClientCertificateValidator">An optional delegate to verify the SSL/TLS client certificate used for authentication.</param>
        /// <param name="ClientCertificateSelector">An optional delegate to select the SSL/TLS client certificate used for authentication.</param>
        /// <param name="AllowedTLSProtocols">The SSL/TLS protocol(s) allowed for this connection.</param>
        /// <param name="URLPathPrefix">An optional prefix for all HTTP URLs.</param>
        /// <param name="AuthorizationURL">An alternative HTTP/SOAP/XML URL for OICP authorization requests.</param>
        /// <param name="ContentType">An optional HTTP content type to use.</param>
        /// <param name="RegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="AutoStart">Start the server immediately.</param>
        public EMPSOAPServer(String                               HTTPServerName               = DefaultHTTPServerName,
                             IPPort?                              TCPPort                      = null,
                             String                               ServiceName                  = null,
                             ServerCertificateSelectorDelegate    ServerCertificateSelector    = null,
                             RemoteCertificateValidationCallback  ClientCertificateValidator   = null,
                             LocalCertificateSelectionCallback    ClientCertificateSelector    = null,
                             SslProtocols                         AllowedTLSProtocols          = SslProtocols.Tls12,
                             HTTPPath?                            URLPathPrefix                = null,
                             String                               AuthorizationURL             = DefaultAuthorizationURL,
                             HTTPContentType                      ContentType                  = null,
                             Boolean                              RegisterHTTPRootService      = true,
                             DNSClient                            DNSClient                    = null,
                             Boolean                              AutoStart                    = false)

            : base(HTTPServerName.IsNotNullOrEmpty() ? HTTPServerName : DefaultHTTPServerName,
                   TCPPort       ?? DefaultHTTPServerPort,
                   ServiceName   ?? "OICP " + Version.Number + " " + nameof(EMPSOAPServer),
                   ServerCertificateSelector,
                   ClientCertificateValidator,
                   ClientCertificateSelector,
                   AllowedTLSProtocols,
                   URLPathPrefix ?? DefaultURLPathPrefix,
                   ContentType   ?? DefaultContentType,
                   RegisterHTTPRootService,
                   DNSClient,
                   AutoStart: false)

        {

            this.ServiceName       = ServiceName      ?? "OICP " + Version.Number + " " + nameof(EMPSOAPServer);
            this.AuthorizationURL  = AuthorizationURL ?? DefaultAuthorizationURL;

            RegisterURLTemplates();

            if (AutoStart)
                Start();

        }

        #endregion

        #region EMPServer(SOAPServer, ServiceName = null, URLPrefix = default, AuthorizationURL = default)

        /// <summary>
        /// Use the given SOAP server for the OICP HTTP/SOAP/XML EMP API.
        /// </summary>
        /// <param name="SOAPServer">A SOAP server.</param>
        /// <param name="ServiceName">An optional identification for this SOAP service.</param>
        /// <param name="URLPathPrefix">An optional prefix for the HTTP URLs.</param>
        /// <param name="AuthorizationURL">An alternative HTTP/SOAP/XML URL for OICP authorization requests.</param>
        public EMPSOAPServer(SOAPServer  SOAPServer,
                             String      ServiceName        = null,
                             HTTPPath?   URLPathPrefix      = null,
                             String      AuthorizationURL   = DefaultAuthorizationURL)

            : base(SOAPServer,
                   URLPathPrefix ?? DefaultURLPathPrefix)

        {

            this.ServiceName       = ServiceName      ?? "OICP " + Version.Number + " " + nameof(EMPSOAPServer);
            this.AuthorizationURL  = AuthorizationURL ?? DefaultAuthorizationURL;

            RegisterURLTemplates();

        }

        #endregion

        #endregion


        #region (override) RegisterURLTemplates()

        /// <summary>
        /// Register all URL templates for this SOAP API.
        /// </summary>
        protected void RegisterURLTemplates()
        {

            #region /Authorization - AuthorizeStart

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URLPrefix + AuthorizationURL,
                                            "AuthorizeStart",
                                            XML => XML.Descendants(OICPNS.Authorization + "eRoamingAuthorizeStart").FirstOrDefault(),
                                            async (HTTPRequest, AuthorizeStartXML) => {


                CPO.AuthorizeStartRequest AuthorizeStartRequest  = null;
                CPO.AuthorizationStart    AuthorizationStart     = null;


                #region Send OnAuthorizeStartSOAPRequest event

                var StartTime = DateTime.UtcNow;

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
                    e.Log(nameof(EMPSOAPServer) + "." + nameof(OnAuthorizeStartSOAPRequest));
                }

                #endregion

                try
                {

                     if (CPO.AuthorizeStartRequest.TryParse(AuthorizeStartXML,
                                                            out AuthorizeStartRequest,
                                                            CustomAuthorizeStartRequestParser,
                                                            CustomIdentificationParser,
                                                            CustomRFIDIdentificationParser,
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
                                                                  ServiceName,
                                                                  AuthorizeStartRequest.EventTrackingId,
                                                                  AuthorizeStartRequest.OperatorId,
                                                                  AuthorizeStartRequest.Identification,
                                                                  AuthorizeStartRequest.EVSEId,
                                                                  AuthorizeStartRequest.SessionId,
                                                                  AuthorizeStartRequest.PartnerProductId,
                                                                  AuthorizeStartRequest.CPOPartnerSessionId,
                                                                  AuthorizeStartRequest.EMPPartnerSessionId,
                                                                  AuthorizeStartRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                                   ConfigureAwait(false);

                        }
                        catch (Exception e)
                        {
                            e.Log(nameof(EMPSOAPServer) + "." + nameof(OnAuthorizeStartRequest));
                        }

                        #endregion

                        #region Call async subscribers

                        if (OnAuthorizeStart != null)
                        {

                            var results = await Task.WhenAll(OnAuthorizeStart.GetInvocationList().
                                                                 Cast<OnAuthorizeStartDelegate>().
                                                                 Select(e => e(DateTime.UtcNow,
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
                                                                            AuthorizeStartRequest.CPOPartnerSessionId,
                                                                            AuthorizeStartRequest.EMPPartnerSessionId
                                                                        );

                        #endregion

                        #region Send OnAuthorizeStartResponse event

                        var EndTime = DateTime.UtcNow;

                        try
                        {

                            if (OnAuthorizeStartResponse != null)
                                await Task.WhenAll(OnAuthorizeStartResponse.GetInvocationList().
                                                   Cast<OnAuthorizeStartResponseDelegate>().
                                                   Select(e => e(EndTime,
                                                                 this,
                                                                 ServiceName,
                                                                 AuthorizeStartRequest.EventTrackingId,
                                                                 AuthorizeStartRequest.OperatorId,
                                                                 AuthorizeStartRequest.Identification,
                                                                 AuthorizeStartRequest.EVSEId,
                                                                 AuthorizeStartRequest.SessionId,
                                                                 AuthorizeStartRequest.PartnerProductId,
                                                                 AuthorizeStartRequest.CPOPartnerSessionId,
                                                                 AuthorizeStartRequest.EMPPartnerSessionId,
                                                                 AuthorizeStartRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                                 AuthorizationStart,
                                                                 EndTime - StartTime))).
                                                   ConfigureAwait(false);

                        }
                        catch (Exception e)
                        {
                            e.Log(nameof(EMPSOAPServer) + "." + nameof(OnAuthorizeStartResponse));
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
                    Date            = DateTime.UtcNow,
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
                    e.Log(nameof(EMPSOAPServer) + "." + nameof(OnAuthorizeStartSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region /Authorization - AuthorizeStop

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URLPrefix + AuthorizationURL,
                                            "AuthorizeStop",
                                            XML => XML.Descendants(OICPNS.Authorization + "eRoamingAuthorizeStop").FirstOrDefault(),
                                            async (HTTPRequest, AuthorizeStopXML) => {


                CPO.AuthorizeStopRequest AuthorizeStopRequest  = null;
                CPO.AuthorizationStop    AuthorizationStop     = null;

                #region Send OnAuthorizeStopSOAPRequest event

                var StartTime = DateTime.UtcNow;

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
                    e.Log(nameof(EMPSOAPServer) + "." + nameof(OnAuthorizeStartSOAPRequest));
                }

                #endregion


                try
                {

                    if (CPO.AuthorizeStopRequest.TryParse(AuthorizeStopXML,
                                                          out AuthorizeStopRequest,
                                                          CustomAuthorizeStopRequestParser,
                                                          CustomIdentificationParser,
                                                          CustomRFIDIdentificationParser,
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
                                                                 ServiceName,
                                                                 AuthorizeStopRequest.EventTrackingId,
                                                                 AuthorizeStopRequest.SessionId,
                                                                 AuthorizeStopRequest.CPOPartnerSessionId,
                                                                 AuthorizeStopRequest.EMPPartnerSessionId,
                                                                 AuthorizeStopRequest.OperatorId,
                                                                 AuthorizeStopRequest.EVSEId,
                                                                 AuthorizeStopRequest.Identification,
                                                                 AuthorizeStopRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                                   ConfigureAwait(false);

                        }
                        catch (Exception e)
                        {
                            e.Log(nameof(EMPSOAPServer) + "." + nameof(OnAuthorizeStopRequest));
                        }

                        #endregion

                        #region Call async subscribers

                        if (OnAuthorizeStop != null)
                        {

                            var results = await Task.WhenAll(OnAuthorizeStop.GetInvocationList().
                                                                 Cast<OnAuthorizeStopDelegate>().
                                                                 Select(e => e(DateTime.UtcNow,
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
                                                    AuthorizeStopRequest.CPOPartnerSessionId,
                                                    AuthorizeStopRequest.EMPPartnerSessionId
                                                );

                        #endregion

                        #region Send OnAuthorizeStopResponse event

                        var EndTime = DateTime.UtcNow;

                        try
                        {

                            if (OnAuthorizeStopResponse != null)
                                await Task.WhenAll(OnAuthorizeStopResponse.GetInvocationList().
                                                   Cast<OnAuthorizeStopResponseHandler>().
                                                   Select(e => e(EndTime,
                                                                 this,
                                                                 ServiceName,
                                                                 AuthorizeStopRequest.EventTrackingId,
                                                                 AuthorizeStopRequest.SessionId,
                                                                 AuthorizeStopRequest.CPOPartnerSessionId,
                                                                 AuthorizeStopRequest.EMPPartnerSessionId,
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
                            e.Log(nameof(EMPSOAPServer) + "." + nameof(OnAuthorizeStopResponse));
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
                    Date            = DateTime.UtcNow,
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
                    e.Log(nameof(EMPSOAPServer) + "." + nameof(OnAuthorizeStopSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region /Authorization - ChargeDetailRecord

            // curl -v -X POST --data "@../Testdata-UID-01.xml" -H "Content-Type: text/xml" -H "Accept: text/xml" http://127.0.0.1:3114/RNs/PROD/Authorization
            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URLPrefix + AuthorizationURL,
                                            "ChargeDetailRecord",
                                            XML => XML.Descendants(OICPNS.Authorization + "eRoamingChargeDetailRecord").FirstOrDefault(),
                                            async (HTTPRequest, ChargeDetailRecordXML) => {

                CPO.SendChargeDetailRecordRequest                  SendChargeDetailRecordRequest  = null;
                Acknowledgement<CPO.SendChargeDetailRecordRequest> Acknowledgement                = null;

                #region Send OnChargeDetailRecordSOAPRequest event

                var StartTime = DateTime.UtcNow;

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
                    e.Log(nameof(EMPSOAPServer) + "." + nameof(OnChargeDetailRecordSOAPRequest));
                }

                #endregion

                try
                {

                    if (CPO.SendChargeDetailRecordRequest.TryParse(ChargeDetailRecordXML,
                                                                   out SendChargeDetailRecordRequest,
                                                                   CustomChargeDetailRecordParser,
                                                                   CustomIdentificationParser,
                                                                   CustomRFIDIdentificationParser,
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
                                                                 ServiceName,
                                                                 SendChargeDetailRecordRequest.EventTrackingId,
                                                                 SendChargeDetailRecordRequest.ChargeDetailRecord,
                                                                 SendChargeDetailRecordRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                                   ConfigureAwait(false);

                        }
                        catch (Exception e)
                        {
                            e.Log(nameof(EMPSOAPServer) + "." + nameof(OnChargeDetailRecordRequest));
                        }

                        #endregion

                        #region Call async subscribers

                        if (OnChargeDetailRecord != null)
                        {

                            var results = await Task.WhenAll(OnChargeDetailRecord.GetInvocationList().
                                                                 Cast<OnChargeDetailRecordDelegate>().
                                                                 Select(e => e(DateTime.UtcNow,
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

                        var EndTime = DateTime.UtcNow;

                        try
                        {

                            if (OnChargeDetailRecordResponse != null)
                                await Task.WhenAll(OnChargeDetailRecordResponse.GetInvocationList().
                                                   Cast<OnChargeDetailRecordResponseHandler>().
                                                   Select(e => e(EndTime,
                                                                 this,
                                                                 ServiceName,
                                                                 SendChargeDetailRecordRequest.EventTrackingId,
                                                                 SendChargeDetailRecordRequest.ChargeDetailRecord,
                                                                 SendChargeDetailRecordRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                                 Acknowledgement,
                                                                 EndTime - StartTime))).
                                                   ConfigureAwait(false);

                        }
                        catch (Exception e)
                        {
                            e.Log(nameof(EMPSOAPServer) + "." + nameof(OnChargeDetailRecordResponse));
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
                    Date            = DateTime.UtcNow,
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
                    e.Log(nameof(EMPSOAPServer) + "." + nameof(OnChargeDetailRecordSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

        }

        #endregion


    }

}
