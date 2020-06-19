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
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;
using System.Net.Security;
using System.Security.Authentication;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_2.CPO
{

    /// <summary>
    /// An OICP CPO HTTP/SOAP/XML Server API.
    /// </summary>
    public class CPOServer : ASOAPServer
    {

        #region Data

        /// <summary>
        /// The default HTTP/SOAP/XML server name.
        /// </summary>
        public new const           String           DefaultHTTPServerName      = "GraphDefined OICP " + Version.Number + " HTTP/SOAP/XML CPO API";

        /// <summary>
        /// The default HTTP/SOAP/XML server TCP port.
        /// </summary>
        public new static readonly IPPort           DefaultHTTPServerPort      = IPPort.Parse(2002);

        /// <summary>
        /// The default HTTP/SOAP/XML server URL prefix.
        /// </summary>
        public new static readonly HTTPPath          DefaultURLPrefix           = HTTPPath.Parse("/");

        /// <summary>
        /// The default HTTP/SOAP/XML URL for OICP authorization requests.
        /// </summary>
        public     const           String           DefaultAuthorizationURL    = "/Authorization";

        /// <summary>
        /// The default HTTP/SOAP/XML URL for OICP reservation requests.
        /// </summary>
        public     const           String           DefaultReservationURL      = "/Reservation";

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
        /// The HTTP/SOAP/XML URL for OICP authorization requests.
        /// </summary>
        public String  AuthorizationURL    { get; }

        /// <summary>
        /// The HTTP/SOAP/XML URL for OICP reservation requests.
        /// </summary>
        public String  ReservationURL      { get; }

        #endregion

        #region Custom request/response mappers

        public CustomXMLParserDelegate<EMP.AuthorizeRemoteReservationStartRequest>                       CustomAuthorizeRemoteReservationStartRequestParser               { get; set; }
        public CustomXMLParserDelegate<EMP.AuthorizeRemoteReservationStopRequest>                        CustomAuthorizeRemoteReservationStopRequestParser                { get; set; }
        public CustomXMLParserDelegate<EMP.AuthorizeRemoteStartRequest>                                  CustomAuthorizeRemoteStartRequestParser                          { get; set; }
        public CustomXMLParserDelegate<EMP.AuthorizeRemoteStopRequest>                                   CustomAuthorizeRemoteStopRequestParser                           { get; set; }

        public CustomXMLParserDelegate<Identification>                                                   CustomIdentificationParser                                       { get; set; }
        public CustomXMLParserDelegate<RFIDIdentification>                                               CustomRFIDIdentificationParser                                   { get; set; }

        public OnExceptionDelegate                                                                       OnException                                                      { get; set; }


        public CustomXMLSerializerDelegate<Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>>  CustomAuthorizeRemoteReservationStartAcknowledgementSerializer   { get; set; }
        public CustomXMLSerializerDelegate<Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>>   CustomAuthorizeRemoteReservationStopAcknowledgementSerializer    { get; set; }
        public CustomXMLSerializerDelegate<Acknowledgement<EMP.AuthorizeRemoteStartRequest>>             CustomAuthorizeRemoteStartAcknowledgementSerializer              { get; set; }
        public CustomXMLSerializerDelegate<Acknowledgement<EMP.AuthorizeRemoteStopRequest>>              CustomAuthorizeRemoteStopAcknowledgementSerializer               { get; set; }

        public CustomXMLSerializerDelegate<StatusCode>                                                   CustomStatusCodeSerializer                                       { get; set; }

        #endregion

        #region Events

        #region OnAuthorizeRemoteReservationStart

        /// <summary>
        /// An event sent whenever an 'authorize remote reservation start' SOAP request was received.
        /// </summary>
        public event RequestLogHandler                                   OnAuthorizeRemoteReservationStartSOAPRequest;

        /// <summary>
        /// An event sent whenever an 'authorize remote reservation start' request was received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStartRequestDelegate    OnAuthorizeRemoteReservationStartRequest;

        /// <summary>
        /// An event sent whenever an 'authorize remote reservation start' request was received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStartDelegate           OnAuthorizeRemoteReservationStart;

        /// <summary>
        /// An event sent whenever a response to an 'authorize remote reservation start' request was sent.
        /// </summary>
        public event OnAuthorizeRemoteReservationStartResponseDelegate   OnAuthorizeRemoteReservationStartResponse;

        /// <summary>
        /// An event sent whenever a response to an 'authorize remote reservation start' SOAP request was sent.
        /// </summary>
        public event AccessLogHandler                                    OnAuthorizeRemoteReservationStartSOAPResponse;

        #endregion

        #region OnAuthorizeRemoteReservationStop

        /// <summary>
        /// An event sent whenever an 'authorize remote reservation stop' SOAP request was received.
        /// </summary>
        public event RequestLogHandler                                  OnAuthorizeRemoteReservationStopSOAPRequest;

        /// <summary>
        /// An event sent whenever an 'authorize remote reservation stop' request was received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStopRequestDelegate    OnAuthorizeRemoteReservationStopRequest;

        /// <summary>
        /// An event sent whenever an 'authorize remote reservation stop' request was received.
        /// </summary>
        public event OnAuthorizeRemoteReservationStopDelegate           OnAuthorizeRemoteReservationStop;

        /// <summary>
        /// An event sent whenever a response to an 'authorize remote reservation stop' request was sent.
        /// </summary>
        public event OnAuthorizeRemoteReservationStopResponseDelegate   OnAuthorizeRemoteReservationStopResponse;

        /// <summary>
        /// An event sent whenever a response to an 'authorize remote reservation stop' SOAP request was sent.
        /// </summary>
        public event AccessLogHandler                                   OnAuthorizeRemoteReservationStopSOAPResponse;

        #endregion

        #region OnAuthorizeRemoteStart

        /// <summary>
        /// An event sent whenever an 'authorize remote start' SOAP request was received.
        /// </summary>
        public event RequestLogHandler                        OnAuthorizeRemoteStartSOAPRequest;

        /// <summary>
        /// An event sent whenever an 'authorize remote start' request was received.
        /// </summary>
        public event OnAuthorizeRemoteStartRequestDelegate    OnAuthorizeRemoteStartRequest;

        /// <summary>
        /// An event sent whenever an 'authorize remote start' request was received.
        /// </summary>
        public event OnAuthorizeRemoteStartDelegate           OnAuthorizeRemoteStart;

        /// <summary>
        /// An event sent whenever a response to an 'authorize remote start' request was sent.
        /// </summary>
        public event OnAuthorizeRemoteStartResponseDelegate   OnAuthorizeRemoteStartResponse;

        /// <summary>
        /// An event sent whenever a response to an 'authorize remote start' SOAP request was sent.
        /// </summary>
        public event AccessLogHandler                         OnAuthorizeRemoteStartSOAPResponse;

        #endregion

        #region OnAuthorizeRemoteStop

        /// <summary>
        /// An event sent whenever an 'authorize remote stop' SOAP request was received.
        /// </summary>
        public event RequestLogHandler                       OnAuthorizeRemoteStopSOAPRequest;

        /// <summary>
        /// An event sent whenever an 'authorize remote stop' request was received.
        /// </summary>
        public event OnAuthorizeRemoteStopRequestDelegate    OnAuthorizeRemoteStopRequest;

        /// <summary>
        /// An event sent whenever an 'authorize remote stop' request was received.
        /// </summary>
        public event OnAuthorizeRemoteStopDelegate           OnAuthorizeRemoteStop;

        /// <summary>
        /// An event sent whenever a response to an 'authorize remote stop' request was sent.
        /// </summary>
        public event OnAuthorizeRemoteStopResponseDelegate   OnAuthorizeRemoteStopResponse;

        /// <summary>
        /// An event sent whenever a response to an 'authorize remote stop' SOAP request was sent.
        /// </summary>
        public event AccessLogHandler                        OnAuthorizeRemoteStopSOAPResponse;

        #endregion

        #endregion

        #region Constructor(s)

        #region CPOServer(HTTPServerName, ServiceId = null, TCPPort = default, URLPrefix = default, AuthorizationURL = default, ReservationURL = default, ContentType = default, DNSClient = null, AutoStart = false)

        /// <summary>
        /// Initialize an new HTTP server for the OICP HTTP/SOAP/XML CPO API.
        /// </summary>
        /// <param name="HTTPServerName">An optional identification string for the HTTP server.</param>
        /// <param name="ServiceId">An optional identification for this SOAP service.</param>
        /// <param name="TCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="ServerCertificateSelector">An optional delegate to select a SSL/TLS server certificate.</param>
        /// <param name="ClientCertificateValidator">An optional delegate to verify the SSL/TLS client certificate used for authentication.</param>
        /// <param name="ClientCertificateSelector">An optional delegate to select the SSL/TLS client certificate used for authentication.</param>
        /// <param name="AllowedTLSProtocols">The SSL/TLS protocol(s) allowed for this connection.</param>
        /// <param name="URLPrefix">An optional prefix for the HTTP URLs.</param>
        /// <param name="AuthorizationURL">The HTTP/SOAP/XML URL for OICP authorization requests.</param>
        /// <param name="ReservationURL">The HTTP/SOAP/XML URL for OICP reservation requests.</param>
        /// <param name="ContentType">An optional HTTP content type to use.</param>
        /// <param name="RegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="AutoStart">Start the server immediately.</param>
        public CPOServer(String                               HTTPServerName               = DefaultHTTPServerName,
                         String                               ServiceId                    = null,
                         IPPort?                              TCPPort                      = null,
                         ServerCertificateSelectorDelegate    ServerCertificateSelector    = null,
                         RemoteCertificateValidationCallback  ClientCertificateValidator   = null,
                         LocalCertificateSelectionCallback    ClientCertificateSelector    = null,
                         SslProtocols                         AllowedTLSProtocols          = SslProtocols.Tls12,
                         HTTPPath?                             URLPrefix                    = null,
                         String                               AuthorizationURL             = DefaultAuthorizationURL,
                         String                               ReservationURL               = DefaultReservationURL,
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
                   URLPrefix   ?? DefaultURLPrefix,
                   ContentType ?? DefaultContentType,
                   RegisterHTTPRootService,
                   DNSClient,
                   AutoStart: false)

        {

            this.ServiceId         = ServiceId        ?? nameof(CPOServer);
            this.AuthorizationURL  = AuthorizationURL ?? DefaultAuthorizationURL;
            this.ReservationURL    = ReservationURL   ?? DefaultReservationURL;

            RegisterURLTemplates();

            if (AutoStart)
                Start();

        }

        #endregion

        #region CPOServer(HTTPServerName, ServiceId = null, TCPPort = default, URLPrefix = default, AuthorizationURL = default, ReservationURL = default, ContentType = default, DNSClient = null, AutoStart = false)

        /// <summary>
        /// Initialize an new HTTP server for the OICP HTTP/SOAP/XML CPO API.
        /// </summary>
        /// <param name="HTTPServerName">An optional identification string for the HTTP server.</param>
        /// <param name="ServiceId">An optional identification for this SOAP service.</param>
        /// <param name="TCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="URLPrefix">An optional prefix for the HTTP URLs.</param>
        /// <param name="AuthorizationURL">The HTTP/SOAP/XML URL for OICP authorization requests.</param>
        /// <param name="ReservationURL">The HTTP/SOAP/XML URL for OICP reservation requests.</param>
        /// <param name="ContentType">An optional HTTP content type to use.</param>
        /// <param name="RegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="AutoStart">Start the server immediately.</param>
        public CPOServer(String           HTTPServerName            = DefaultHTTPServerName,
                         String           ServiceId                 = null,
                         IPPort?          TCPPort                   = null,
                         HTTPPath?         URLPrefix                 = null,
                         String           AuthorizationURL          = DefaultAuthorizationURL,
                         String           ReservationURL            = DefaultReservationURL,
                         HTTPContentType  ContentType               = null,
                         Boolean          RegisterHTTPRootService   = true,
                         DNSClient        DNSClient                 = null,
                         Boolean          AutoStart                 = false)

            : base(HTTPServerName.IsNotNullOrEmpty() ? HTTPServerName : DefaultHTTPServerName,
                   TCPPort     ?? DefaultHTTPServerPort,
                   URLPrefix   ?? DefaultURLPrefix,
                   ContentType ?? DefaultContentType,
                   RegisterHTTPRootService,
                   DNSClient,
                   AutoStart: false)

        {

            this.ServiceId         = ServiceId        ?? nameof(CPOServer);
            this.AuthorizationURL  = AuthorizationURL ?? DefaultAuthorizationURL;
            this.ReservationURL    = ReservationURL   ?? DefaultReservationURL;

            RegisterURLTemplates();

            if (AutoStart)
                Start();

        }

        #endregion

        #region CPOServer(SOAPServer, ServiceId = null, URLPrefix = default, AuthorizationURL = default, ReservationURL = default)

        /// <summary>
        /// Use the given SOAP server for the OICP HTTP/SOAP/XML CPO API.
        /// </summary>
        /// <param name="SOAPServer">A SOAP server.</param>
        /// <param name="ServiceId">An optional identification for this SOAP service.</param>
        /// <param name="URLPrefix">An optional prefix for the HTTP URLs.</param>
        /// <param name="AuthorizationURL">The HTTP/SOAP/XML URL for OICP authorization requests.</param>
        /// <param name="ReservationURL">The HTTP/SOAP/XML URL for OICP reservation requests.</param>
        public CPOServer(SOAPServer  SOAPServer,
                         String      ServiceId          = null,
                         HTTPPath?    URLPrefix          = null,
                         String      AuthorizationURL   = DefaultAuthorizationURL,
                         String      ReservationURL     = DefaultReservationURL)

            : base(SOAPServer,
                   URLPrefix ?? DefaultURLPrefix)

        {

            this.ServiceId         = ServiceId        ?? nameof(CPOServer);
            this.AuthorizationURL  = AuthorizationURL ?? DefaultAuthorizationURL;
            this.ReservationURL    = ReservationURL   ?? DefaultReservationURL;

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

            #region /Reservation   - AuthorizeRemoteReservationStart

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URLPrefix + ReservationURL,
                                            "AuthorizeRemoteReservationStart",
                                            XML => XML.Descendants(OICPNS.Reservation + "eRoamingAuthorizeRemoteReservationStart").FirstOrDefault(),
                                            async (HTTPRequest, AuthorizeRemoteReservationStartXML) => {


                EMP.AuthorizeRemoteReservationStartRequest                   AuthorizeRemoteReservationStartRequest   = null;
                Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>  Acknowledgement                          = null;

                #region Send OnAuthorizeRemoteReservationStartSOAPRequest event

                var StartTime = DateTime.UtcNow;

                try
                {

                    if (OnAuthorizeRemoteReservationStartSOAPRequest != null)
                        await Task.WhenAll(OnAuthorizeRemoteReservationStartSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StartTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteReservationStartSOAPRequest));
                }

                #endregion


                try
                {

                    if (EMP.AuthorizeRemoteReservationStartRequest.TryParse(AuthorizeRemoteReservationStartXML,
                                                                            out AuthorizeRemoteReservationStartRequest,
                                                                            CustomAuthorizeRemoteReservationStartRequestParser,
                                                                            CustomIdentificationParser,
                                                                            CustomRFIDIdentificationParser,
                                                                            OnException,

                                                                            HTTPRequest.Timestamp,
                                                                            HTTPRequest.CancellationToken,
                                                                            HTTPRequest.EventTrackingId,
                                                                            HTTPRequest.Timeout ?? DefaultRequestTimeout))
                    {

                        #region Send OnAuthorizeRemoteReservationStartRequest event

                        try
                        {

                            if (OnAuthorizeRemoteReservationStartRequest != null)
                                await Task.WhenAll(OnAuthorizeRemoteReservationStartRequest.GetInvocationList().
                                                   Cast<OnAuthorizeRemoteReservationStartRequestDelegate>().
                                                   Select(e => e(StartTime,
                                                                 AuthorizeRemoteReservationStartRequest.Timestamp.Value,
                                                                 this,
                                                                 ServiceId,
                                                                 AuthorizeRemoteReservationStartRequest.EventTrackingId,
                                                                 AuthorizeRemoteReservationStartRequest.EVSEId,
                                                                 AuthorizeRemoteReservationStartRequest.PartnerProductId,
                                                                 AuthorizeRemoteReservationStartRequest.SessionId,
                                                                 AuthorizeRemoteReservationStartRequest.CPOPartnerSessionId,
                                                                 AuthorizeRemoteReservationStartRequest.EMPPartnerSessionId,
                                                                 AuthorizeRemoteReservationStartRequest.ProviderId,
                                                                 AuthorizeRemoteReservationStartRequest.Identification,
                                                                 AuthorizeRemoteReservationStartRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                                   ConfigureAwait(false);

                        }
                        catch (Exception e)
                        {
                            e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteReservationStartRequest));
                        }

                        #endregion

                        #region Call async subscribers

                        if (OnAuthorizeRemoteReservationStart != null)
                        {

                            var results = await Task.WhenAll(OnAuthorizeRemoteReservationStart.GetInvocationList().
                                                                 Cast<OnAuthorizeRemoteReservationStartDelegate>().
                                                                 Select(e => e(DateTime.UtcNow,
                                                                               this,
                                                                               AuthorizeRemoteReservationStartRequest))).
                                                                 ConfigureAwait(false);

                            Acknowledgement = results.FirstOrDefault();

                        }

                        if (Acknowledgement == null)
                            Acknowledgement = Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>.SystemError(
                                                 AuthorizeRemoteReservationStartRequest,
                                                 "Could not process the incoming AuthorizeRemoteReservationStart request!",
                                                 null,
                                                 AuthorizeRemoteReservationStartRequest.SessionId,
                                                 AuthorizeRemoteReservationStartRequest.CPOPartnerSessionId,
                                                 AuthorizeRemoteReservationStartRequest.EMPPartnerSessionId
                                             );

                        #endregion

                        #region Send OnAuthorizeRemoteReservationStartResponse event

                        var EndTime = DateTime.UtcNow;

                        try
                        {

                            if (OnAuthorizeRemoteReservationStartResponse != null)
                                await Task.WhenAll(OnAuthorizeRemoteReservationStartResponse.GetInvocationList().
                                                   Cast<OnAuthorizeRemoteReservationStartResponseDelegate>().
                                                   Select(e => e(EndTime,
                                                                 this,
                                                                 ServiceId,
                                                                 AuthorizeRemoteReservationStartRequest.EventTrackingId,
                                                                 AuthorizeRemoteReservationStartRequest.EVSEId,
                                                                 AuthorizeRemoteReservationStartRequest.PartnerProductId,
                                                                 AuthorizeRemoteReservationStartRequest.SessionId,
                                                                 AuthorizeRemoteReservationStartRequest.CPOPartnerSessionId,
                                                                 AuthorizeRemoteReservationStartRequest.EMPPartnerSessionId,
                                                                 AuthorizeRemoteReservationStartRequest.ProviderId,
                                                                 AuthorizeRemoteReservationStartRequest.Identification,
                                                                 AuthorizeRemoteReservationStartRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                                 Acknowledgement,
                                                                 EndTime - StartTime))).
                                                   ConfigureAwait(false);

                        }
                        catch (Exception e)
                        {
                            e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteReservationStartResponse));
                        }

                        #endregion

                    }

                    else
                        Acknowledgement = Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>.DataError(
                                              AuthorizeRemoteReservationStartRequest,
                                              "Could not process the incoming AuthorizeRemoteReservationStart request!"
                                          );

                }
                catch (Exception e)
                {
                    Acknowledgement = Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>.DataError(
                        AuthorizeRemoteReservationStartRequest,
                        e.Message,
                        e.StackTrace
                    );
                }


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(Acknowledgement.ToXML(CustomAuthorizeRemoteReservationStartAcknowledgementSerializer,
                                                                               CustomStatusCodeSerializer)).ToUTF8Bytes(),
                    Connection      = "close"
                };

                #endregion

                #region Send OnAuthorizeRemoteReservationStartSOAPResponse event

                try
                {

                    if (OnAuthorizeRemoteReservationStartSOAPResponse != null)
                        await Task.WhenAll(OnAuthorizeRemoteReservationStartSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteReservationStartSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region /Reservation   - AuthorizeRemoteReservationStop

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URLPrefix + ReservationURL,
                                            "AuthorizeRemoteReservationStop",
                                            XML => XML.Descendants(OICPNS.Reservation + "eRoamingAuthorizeRemoteReservationStop").FirstOrDefault(),
                                            async (HTTPRequest, AuthorizeRemoteReservationStopXML) => {


                EMP.AuthorizeRemoteReservationStopRequest                   AuthorizeRemoteReservationStopRequest   = null;
                Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>  Acknowledgement                         = null;

                #region Send OnAuthorizeRemoteReservationStopSOAPRequest event

                var StartTime = DateTime.UtcNow;

                try
                {

                    if (OnAuthorizeRemoteReservationStopSOAPRequest != null)
                        await Task.WhenAll(OnAuthorizeRemoteReservationStopSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StartTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteReservationStopSOAPRequest));
                }

                #endregion


                try
                {

                    if (EMP.AuthorizeRemoteReservationStopRequest.TryParse(AuthorizeRemoteReservationStopXML,
                                                                           out AuthorizeRemoteReservationStopRequest,
                                                                           CustomAuthorizeRemoteReservationStopRequestParser,
                                                                           OnException,

                                                                           HTTPRequest.Timestamp,
                                                                           HTTPRequest.CancellationToken,
                                                                           HTTPRequest.EventTrackingId,
                                                                           HTTPRequest.Timeout ?? DefaultRequestTimeout))
                    {

                        #region Send OnAuthorizeRemoteReservationStopRequest event

                        try
                        {

                            if (OnAuthorizeRemoteReservationStopRequest != null)
                                await Task.WhenAll(OnAuthorizeRemoteReservationStopRequest.GetInvocationList().
                                                   Cast<OnAuthorizeRemoteReservationStopRequestDelegate>().
                                                   Select(e => e(StartTime,
                                                                 AuthorizeRemoteReservationStopRequest.Timestamp.Value,
                                                                 this,
                                                                 ServiceId,
                                                                 AuthorizeRemoteReservationStopRequest.EventTrackingId,
                                                                 AuthorizeRemoteReservationStopRequest.EVSEId,
                                                                 AuthorizeRemoteReservationStopRequest.SessionId,
                                                                 AuthorizeRemoteReservationStopRequest.CPOPartnerSessionId,
                                                                 AuthorizeRemoteReservationStopRequest.EMPPartnerSessionId,
                                                                 AuthorizeRemoteReservationStopRequest.ProviderId,
                                                                 AuthorizeRemoteReservationStopRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                                   ConfigureAwait(false);

                        }
                        catch (Exception e)
                        {
                            e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteReservationStopRequest));
                        }

                        #endregion

                        #region Call async subscribers

                        if (OnAuthorizeRemoteReservationStop != null)
                        {

                            var results = await Task.WhenAll(OnAuthorizeRemoteReservationStop.GetInvocationList().
                                                                 Cast<OnAuthorizeRemoteReservationStopDelegate>().
                                                                 Select(e => e(DateTime.UtcNow,
                                                                               this,
                                                                               AuthorizeRemoteReservationStopRequest))).
                                                                 ConfigureAwait(false);

                            Acknowledgement = results.FirstOrDefault();

                        }

                        if (Acknowledgement == null)
                            Acknowledgement = Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>.SystemError(
                                                 AuthorizeRemoteReservationStopRequest,
                                                 "Could not process the incoming AuthorizeRemoteReservationStop request!",
                                                 null,
                                                 AuthorizeRemoteReservationStopRequest.SessionId,
                                                 AuthorizeRemoteReservationStopRequest.CPOPartnerSessionId,
                                                 AuthorizeRemoteReservationStopRequest.EMPPartnerSessionId
                                             );

                        #endregion

                        #region Send OnAuthorizeRemoteReservationStopResponse event

                        var EndTime = DateTime.UtcNow;

                        try
                        {

                            if (OnAuthorizeRemoteReservationStopResponse != null)
                                await Task.WhenAll(OnAuthorizeRemoteReservationStopResponse.GetInvocationList().
                                                   Cast<OnAuthorizeRemoteReservationStopResponseDelegate>().
                                                   Select(e => e(EndTime,
                                                                 this,
                                                                 ServiceId,
                                                                 AuthorizeRemoteReservationStopRequest.EventTrackingId,
                                                                 AuthorizeRemoteReservationStopRequest.EVSEId,
                                                                 AuthorizeRemoteReservationStopRequest.SessionId,
                                                                 AuthorizeRemoteReservationStopRequest.CPOPartnerSessionId,
                                                                 AuthorizeRemoteReservationStopRequest.EMPPartnerSessionId,
                                                                 AuthorizeRemoteReservationStopRequest.ProviderId,
                                                                 AuthorizeRemoteReservationStopRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                                 Acknowledgement,
                                                                 EndTime - StartTime))).
                                                   ConfigureAwait(false);

                        }
                        catch (Exception e)
                        {
                            e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteReservationStopResponse));
                        }

                        #endregion

                    }

                    else
                        Acknowledgement = Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>.DataError(
                                              AuthorizeRemoteReservationStopRequest,
                                              "Could not process the incoming AuthorizeRemoteReservationStop request!"
                                          );

                }
                catch (Exception e)
                {
                    Acknowledgement = Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>.DataError(
                        AuthorizeRemoteReservationStopRequest,
                        e.Message,
                        e.StackTrace
                    );
                }


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(Acknowledgement.ToXML(CustomAuthorizeRemoteReservationStopAcknowledgementSerializer,
                                                                               CustomStatusCodeSerializer)).ToUTF8Bytes(),
                    Connection      = "close"
                };

                #endregion

                #region Send OnAuthorizeRemoteReservationStopSOAPResponse event

                try
                {

                    if (OnAuthorizeRemoteReservationStopSOAPResponse != null)
                        await Task.WhenAll(OnAuthorizeRemoteReservationStopSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteReservationStopSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region /Authorization - AuthorizeRemoteStart

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URLPrefix + AuthorizationURL,
                                            "AuthorizeRemoteStart",
                                            XML => XML.Descendants(OICPNS.Authorization + "eRoamingAuthorizeRemoteStart").FirstOrDefault(),
                                            async (HTTPRequest, AuthorizeRemoteStartXML) => {


                EMP.AuthorizeRemoteStartRequest                   AuthorizeRemoteStartRequest   = null;
                Acknowledgement<EMP.AuthorizeRemoteStartRequest>  Acknowledgement               = null;

                #region Send OnAuthorizeRemoteStartSOAPRequest event

                var StartTime = DateTime.UtcNow;

                try
                {

                    if (OnAuthorizeRemoteStartSOAPRequest != null)
                        await Task.WhenAll(OnAuthorizeRemoteStartSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StartTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteStartSOAPRequest));
                }

                #endregion


                try
                {

                    if (EMP.AuthorizeRemoteStartRequest.TryParse(AuthorizeRemoteStartXML,
                                                                 out AuthorizeRemoteStartRequest,
                                                                 CustomAuthorizeRemoteStartRequestParser,
                                                                 CustomIdentificationParser,
                                                                 CustomRFIDIdentificationParser,
                                                                 OnException,

                                                                 HTTPRequest.Timestamp,
                                                                 HTTPRequest.CancellationToken,
                                                                 HTTPRequest.EventTrackingId,
                                                                 HTTPRequest.Timeout ?? DefaultRequestTimeout))
                    {

                        #region Send OnAuthorizeRemoteStartRequest event

                        try
                        {

                            if (OnAuthorizeRemoteStartRequest != null)
                                await Task.WhenAll(OnAuthorizeRemoteStartRequest.GetInvocationList().
                                                   Cast<OnAuthorizeRemoteStartRequestDelegate>().
                                                   Select(e => e(StartTime,
                                                                  AuthorizeRemoteStartRequest.Timestamp.Value,
                                                                  this,
                                                                  ServiceId,
                                                                  AuthorizeRemoteStartRequest.EventTrackingId,
                                                                  AuthorizeRemoteStartRequest.EVSEId,
                                                                  AuthorizeRemoteStartRequest.PartnerProductId,
                                                                  AuthorizeRemoteStartRequest.SessionId,
                                                                  AuthorizeRemoteStartRequest.CPOPartnerSessionId,
                                                                  AuthorizeRemoteStartRequest.EMPPartnerSessionId,
                                                                  AuthorizeRemoteStartRequest.ProviderId,
                                                                  AuthorizeRemoteStartRequest.Identification,
                                                                  AuthorizeRemoteStartRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                                   ConfigureAwait(false);

                        }
                        catch (Exception e)
                        {
                            e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteStartRequest));
                        }

                        #endregion

                        #region Call async subscribers

                        if (OnAuthorizeRemoteStart != null)
                        {

                            var results = await Task.WhenAll(OnAuthorizeRemoteStart.GetInvocationList().
                                                                 Cast<OnAuthorizeRemoteStartDelegate>().
                                                                 Select(e => e(DateTime.UtcNow,
                                                                               this,
                                                                               AuthorizeRemoteStartRequest))).
                                                                 ConfigureAwait(false);

                            Acknowledgement = results.FirstOrDefault();

                        }

                        if (Acknowledgement == null)
                            Acknowledgement = Acknowledgement<EMP.AuthorizeRemoteStartRequest>.SystemError(
                                                 AuthorizeRemoteStartRequest,
                                                 "Could not process the incoming AuthorizeRemoteStart request!",
                                                 null,
                                                 AuthorizeRemoteStartRequest.SessionId,
                                                 AuthorizeRemoteStartRequest.CPOPartnerSessionId,
                                                 AuthorizeRemoteStartRequest.EMPPartnerSessionId
                                             );

                        #endregion

                        #region Send OnAuthorizeRemoteStartResponse event

                        var EndTime = DateTime.UtcNow;

                        try
                        {

                            if (OnAuthorizeRemoteStartResponse != null)
                                await Task.WhenAll(OnAuthorizeRemoteStartResponse.GetInvocationList().
                                                   Cast<OnAuthorizeRemoteStartResponseDelegate>().
                                                   Select(e => e(EndTime,
                                                                 this,
                                                                 ServiceId,
                                                                 AuthorizeRemoteStartRequest.EventTrackingId,
                                                                 AuthorizeRemoteStartRequest.EVSEId,
                                                                 AuthorizeRemoteStartRequest.PartnerProductId,
                                                                 AuthorizeRemoteStartRequest.SessionId,
                                                                 AuthorizeRemoteStartRequest.CPOPartnerSessionId,
                                                                 AuthorizeRemoteStartRequest.EMPPartnerSessionId,
                                                                 AuthorizeRemoteStartRequest.ProviderId,
                                                                 AuthorizeRemoteStartRequest.Identification,
                                                                 AuthorizeRemoteStartRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                                 Acknowledgement,
                                                                 EndTime - StartTime))).
                                                   ConfigureAwait(false);

                        }
                        catch (Exception e)
                        {
                            e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteStartResponse));
                        }

                        #endregion

                    }

                    else
                        Acknowledgement = Acknowledgement<EMP.AuthorizeRemoteStartRequest>.DataError(
                                              AuthorizeRemoteStartRequest,
                                              "Could not process the incoming AuthorizeRemoteStart request!"
                                          );

                }
                catch (Exception e)
                {
                    Acknowledgement = Acknowledgement<EMP.AuthorizeRemoteStartRequest>.DataError(
                        AuthorizeRemoteStartRequest,
                        e.Message,
                        e.StackTrace
                    );
                }


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(Acknowledgement.ToXML(CustomAuthorizeRemoteStartAcknowledgementSerializer,
                                                                               CustomStatusCodeSerializer)).ToUTF8Bytes(),
                    Connection      = "close"
                };

                #endregion

                #region Send OnAuthorizeRemoteStartSOAPResponse event

                try
                {

                    if (OnAuthorizeRemoteStartSOAPResponse != null)
                        await Task.WhenAll(OnAuthorizeRemoteStartSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteStartSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region /Authorization - AuthorizeRemoteStop

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URLPrefix + AuthorizationURL,
                                            "AuthorizeRemoteStop",
                                            XML => XML.Descendants(OICPNS.Authorization + "eRoamingAuthorizeRemoteStop").FirstOrDefault(),
                                            async (HTTPRequest, AuthorizeRemoteStopXML) => {



                EMP.AuthorizeRemoteStopRequest                   AuthorizeRemoteStopRequest   = null;
                Acknowledgement<EMP.AuthorizeRemoteStopRequest>  Acknowledgement              = null;

                #region Send OnAuthorizeRemoteStopSOAPRequest event

                var StartTime = DateTime.UtcNow;

                try
                {

                    if (OnAuthorizeRemoteStopSOAPRequest != null)
                        await Task.WhenAll(OnAuthorizeRemoteStopSOAPRequest.GetInvocationList().
                                           Cast<RequestLogHandler>().
                                           Select(e => e(StartTime,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteStopSOAPRequest));
                }

                #endregion


                try
                {

                    if (EMP.AuthorizeRemoteStopRequest.TryParse(AuthorizeRemoteStopXML,
                                                                out AuthorizeRemoteStopRequest,
                                                                CustomAuthorizeRemoteStopRequestParser,
                                                                OnException,

                                                                HTTPRequest.Timestamp,
                                                                HTTPRequest.CancellationToken,
                                                                HTTPRequest.EventTrackingId,
                                                                HTTPRequest.Timeout ?? DefaultRequestTimeout))
                    {

                        #region Send OnAuthorizeRemoteStopRequest event

                        try
                        {

                            if (OnAuthorizeRemoteStopRequest != null)
                                await Task.WhenAll(OnAuthorizeRemoteStopRequest.GetInvocationList().
                                                   Cast<OnAuthorizeRemoteStopRequestDelegate>().
                                                   Select(e => e(StartTime,
                                                                 AuthorizeRemoteStopRequest.Timestamp.Value,
                                                                 this,
                                                                 ServiceId,
                                                                 AuthorizeRemoteStopRequest.EventTrackingId,
                                                                 AuthorizeRemoteStopRequest.EVSEId,
                                                                 AuthorizeRemoteStopRequest.SessionId,
                                                                 AuthorizeRemoteStopRequest.CPOPartnerSessionId,
                                                                 AuthorizeRemoteStopRequest.EMPPartnerSessionId,
                                                                 AuthorizeRemoteStopRequest.ProviderId,
                                                                 AuthorizeRemoteStopRequest.RequestTimeout ?? DefaultRequestTimeout))).
                                                   ConfigureAwait(false);

                        }
                        catch (Exception e)
                        {
                            e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteStopRequest));
                        }

                        #endregion

                        #region Call async subscribers

                        if (OnAuthorizeRemoteStop != null)
                        {

                            var results = await Task.WhenAll(OnAuthorizeRemoteStop.GetInvocationList().
                                                                 Cast<OnAuthorizeRemoteStopDelegate>().
                                                                 Select(e => e(DateTime.UtcNow,
                                                                               this,
                                                                               AuthorizeRemoteStopRequest))).
                                                                 ConfigureAwait(false);

                            Acknowledgement = results.FirstOrDefault();

                        }

                        if (Acknowledgement == null)
                            Acknowledgement = Acknowledgement<EMP.AuthorizeRemoteStopRequest>.SystemError(
                                                 AuthorizeRemoteStopRequest,
                                                 "Could not process the incoming AuthorizeRemoteStop request!",
                                                 null,
                                                 AuthorizeRemoteStopRequest.SessionId,
                                                 AuthorizeRemoteStopRequest.CPOPartnerSessionId,
                                                 AuthorizeRemoteStopRequest.EMPPartnerSessionId
                                             );

                        #endregion

                        #region Send OnAuthorizeRemoteStopResponse event

                        var EndTime = DateTime.UtcNow;

                        try
                        {

                            if (OnAuthorizeRemoteStopResponse != null)
                                await Task.WhenAll(OnAuthorizeRemoteStopResponse.GetInvocationList().
                                                   Cast<OnAuthorizeRemoteStopResponseDelegate>().
                                                   Select(e => e(EndTime,
                                                                 this,
                                                                 ServiceId,
                                                                 AuthorizeRemoteStopRequest.EventTrackingId,
                                                                 AuthorizeRemoteStopRequest.EVSEId,
                                                                 AuthorizeRemoteStopRequest.SessionId,
                                                                 AuthorizeRemoteStopRequest.CPOPartnerSessionId,
                                                                 AuthorizeRemoteStopRequest.EMPPartnerSessionId,
                                                                 AuthorizeRemoteStopRequest.ProviderId,
                                                                 AuthorizeRemoteStopRequest.RequestTimeout ?? DefaultRequestTimeout,
                                                                 Acknowledgement,
                                                                 EndTime - StartTime))).
                                                   ConfigureAwait(false);

                        }
                        catch (Exception e)
                        {
                            e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteStopResponse));
                        }

                        #endregion

                    }

                    else
                        Acknowledgement = Acknowledgement<EMP.AuthorizeRemoteStopRequest>.DataError(
                                              AuthorizeRemoteStopRequest,
                                              "Could not process the incoming AuthorizeRemoteStop request!"
                                          );

                }
                catch (Exception e)
                {
                    Acknowledgement = Acknowledgement<EMP.AuthorizeRemoteStopRequest>.DataError(
                        AuthorizeRemoteStopRequest,
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
                    Content         = SOAP.Encapsulation(Acknowledgement.ToXML(CustomAuthorizeRemoteStopAcknowledgementSerializer,
                                                                               CustomStatusCodeSerializer)).ToUTF8Bytes(),
                    Connection      = "close"
                };

                #endregion

                #region Send OnAuthorizeRemoteStopSOAPResponse event

                try
                {

                    if (OnAuthorizeRemoteStopSOAPResponse != null)
                        await Task.WhenAll(OnAuthorizeRemoteStopSOAPResponse.GetInvocationList().
                                           Cast<AccessLogHandler>().
                                           Select(e => e(HTTPResponse.Timestamp,
                                                         SOAPServer.HTTPServer,
                                                         HTTPRequest,
                                                         HTTPResponse))).
                                           ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteStopSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

        }

        #endregion


    }

}
