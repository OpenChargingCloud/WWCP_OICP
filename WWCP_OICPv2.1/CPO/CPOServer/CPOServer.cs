﻿/*
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
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.CPO
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
        public new static readonly IPPort           DefaultHTTPServerPort      = new IPPort(2002);

        /// <summary>
        /// The default HTTP/SOAP/XML server URI prefix.
        /// </summary>
        public new const           String           DefaultURIPrefix           = "";

        /// <summary>
        /// The default HTTP/SOAP/XML URI for OICP authorization requests.
        /// </summary>
        public     const           String           DefaultAuthorizationURI    = "/Authorization";

        /// <summary>
        /// The default HTTP/SOAP/XML URI for OICP reservation requests.
        /// </summary>
        public     const           String           DefaultReservationURI      = "/Reservation";

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
        /// The HTTP/SOAP/XML URI for OICP authorization requests.
        /// </summary>
        public String  AuthorizationURI    { get; }

        /// <summary>
        /// The HTTP/SOAP/XML URI for OICP reservation requests.
        /// </summary>
        public String  ReservationURI      { get; }

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

        #region CPOServer(HTTPServerName, TCPPort = default, URIPrefix = default, AuthorizationURI = default, ReservationURI = default, ContentType = default, DNSClient = null, AutoStart = false)

        /// <summary>
        /// Initialize an new HTTP server for the OICP HTTP/SOAP/XML CPO API.
        /// </summary>
        /// <param name="HTTPServerName">An optional identification string for the HTTP server.</param>
        /// <param name="TCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="AuthorizationURI">The HTTP/SOAP/XML URI for OICP authorization requests.</param>
        /// <param name="ReservationURI">The HTTP/SOAP/XML URI for OICP reservation requests.</param>
        /// <param name="ContentType">An optional HTTP content type to use.</param>
        /// <param name="RegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="AutoStart">Start the server immediately.</param>
        public CPOServer(String          HTTPServerName           = DefaultHTTPServerName,
                         IPPort          TCPPort                  = null,
                         String          URIPrefix                = DefaultURIPrefix,
                         String          AuthorizationURI         = DefaultAuthorizationURI,
                         String          ReservationURI           = DefaultReservationURI,
                         HTTPContentType ContentType              = null,
                         Boolean         RegisterHTTPRootService  = true,
                         DNSClient       DNSClient                = null,
                         Boolean         AutoStart                = false)

            : base(HTTPServerName.IsNotNullOrEmpty() ? HTTPServerName : DefaultHTTPServerName,
                   TCPPort     ?? DefaultHTTPServerPort,
                   URIPrefix   ?? DefaultURIPrefix,
                   ContentType ?? DefaultContentType,
                   RegisterHTTPRootService,
                   DNSClient,
                   AutoStart: false)

        {

            this.AuthorizationURI  = AuthorizationURI ?? DefaultAuthorizationURI;
            this.ReservationURI    = ReservationURI   ?? DefaultReservationURI;

            if (AutoStart)
                Start();

        }

        #endregion

        #region CPOServer(SOAPServer, URIPrefix = default, AuthorizationURI = default, ReservationURI = default)

        /// <summary>
        /// Use the given SOAP server for the OICP HTTP/SOAP/XML CPO API.
        /// </summary>
        /// <param name="SOAPServer">A SOAP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="AuthorizationURI">The HTTP/SOAP/XML URI for OICP authorization requests.</param>
        /// <param name="ReservationURI">The HTTP/SOAP/XML URI for OICP reservation requests.</param>
        public CPOServer(SOAPServer  SOAPServer,
                         String      URIPrefix         = DefaultURIPrefix,
                         String      AuthorizationURI  = DefaultAuthorizationURI,
                         String      ReservationURI    = DefaultReservationURI)

            : base(SOAPServer,
                   URIPrefix ?? DefaultURIPrefix)

        {

            this.AuthorizationURI  = AuthorizationURI ?? DefaultAuthorizationURI;
            this.ReservationURI    = ReservationURI   ?? DefaultReservationURI;

        }

        #endregion

        #endregion


        #region (override) RegisterURITemplates()

        /// <summary>
        /// Register all URI templates for this SOAP API.
        /// </summary>
        protected override void RegisterURITemplates()
        {

            #region /Reservation - AuthorizeRemoteReservationStart

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + ReservationURI,
                                            "AuthorizeRemoteReservationStart",
                                            XML => XML.Descendants(OICPNS.Reservation + "eRoamingAuthorizeRemoteReservationStart").FirstOrDefault(),
                                            async (HTTPRequest, AuthorizeRemoteReservationStartXML) => {


                EMP.AuthorizeRemoteReservationStartRequest                  AuthorizeRemoteReservationStartRequest  = null;
                Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest> Acknowledgement                         = null;

                #region Send OnAuthorizeRemoteReservationStartSOAPRequest event

                var StartTime = DateTime.Now;

                try
                {

                    OnAuthorizeRemoteReservationStartSOAPRequest?.Invoke(StartTime,
                                                                         this.SOAPServer,
                                                                         HTTPRequest);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteReservationStartSOAPRequest));
                }

                #endregion


                if (EMP.AuthorizeRemoteReservationStartRequest.TryParse(AuthorizeRemoteReservationStartXML,
                                                                        out AuthorizeRemoteReservationStartRequest,
                                                                        null,
                                                                        HTTPRequest.Timestamp,
                                                                        HTTPRequest.CancellationToken,
                                                                        HTTPRequest.EventTrackingId,
                                                                        HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnAuthorizeRemoteReservationStartRequest event

                    try
                    {

                        OnAuthorizeRemoteReservationStartRequest?.Invoke(StartTime,
                                                                         AuthorizeRemoteReservationStartRequest.Timestamp.Value,
                                                                         this,
                                                                         nameof(CPOServer),  // ClientId
                                                                         AuthorizeRemoteReservationStartRequest.EventTrackingId,
                                                                         AuthorizeRemoteReservationStartRequest.EVSEId,
                                                                         AuthorizeRemoteReservationStartRequest.PartnerProductId,
                                                                         AuthorizeRemoteReservationStartRequest.SessionId,
                                                                         AuthorizeRemoteReservationStartRequest.PartnerSessionId,
                                                                         AuthorizeRemoteReservationStartRequest.ProviderId,
                                                                         AuthorizeRemoteReservationStartRequest.EVCOId,
                                                                         AuthorizeRemoteReservationStartRequest.RequestTimeout.HasValue ? AuthorizeRemoteReservationStartRequest.RequestTimeout.Value : DefaultRequestTimeout);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteReservationStartRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    var results = OnAuthorizeRemoteReservationStart?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnAuthorizeRemoteReservationStartDelegate)
                                          (DateTime.Now,
                                           this,
                                           AuthorizeRemoteReservationStartRequest)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        Acknowledgement = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || Acknowledgement == null)
                        Acknowledgement = Acknowledgement<EMP.AuthorizeRemoteReservationStartRequest>.SystemError(
                                             AuthorizeRemoteReservationStartRequest,
                                             "Could not process the incoming AuthorizeRemoteReservationStart request!",
                                             null,
                                             AuthorizeRemoteReservationStartRequest.SessionId,
                                             AuthorizeRemoteReservationStartRequest.PartnerSessionId
                                         );

                    #endregion

                    #region Send OnAuthorizeRemoteReservationStartResponse event

                    var EndTime = DateTime.Now;

                    try
                    {

                        OnAuthorizeRemoteReservationStartResponse?.Invoke(EndTime,
                                                                          this,
                                                                          nameof(CPOServer),  // ClientId
                                                                          AuthorizeRemoteReservationStartRequest.EventTrackingId,
                                                                          AuthorizeRemoteReservationStartRequest.EVSEId,
                                                                          AuthorizeRemoteReservationStartRequest.PartnerProductId,
                                                                          AuthorizeRemoteReservationStartRequest.SessionId,
                                                                          AuthorizeRemoteReservationStartRequest.PartnerSessionId,
                                                                          AuthorizeRemoteReservationStartRequest.ProviderId,
                                                                          AuthorizeRemoteReservationStartRequest.EVCOId,
                                                                          AuthorizeRemoteReservationStartRequest.RequestTimeout.HasValue ? AuthorizeRemoteReservationStartRequest.RequestTimeout.Value : DefaultRequestTimeout,
                                                                          Acknowledgement,
                                                                          EndTime - StartTime);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteReservationStartResponse));
                    }

                    #endregion

                }


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(Acknowledgement.ToXML()).ToUTF8Bytes()
                };

                #endregion

                #region Send OnAuthorizeRemoteReservationStartSOAPResponse event

                try
                {

                    OnAuthorizeRemoteReservationStartSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                                          this.SOAPServer,
                                                                          HTTPRequest,
                                                                          HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteReservationStartSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region /Reservation - AuthorizeRemoteReservationStop

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + ReservationURI,
                                            "AuthorizeRemoteReservationStop",
                                            XML => XML.Descendants(OICPNS.Reservation + "eRoamingAuthorizeRemoteReservationStop").FirstOrDefault(),
                                            async (HTTPRequest, AuthorizeRemoteReservationStopXML) => {

                EMP.AuthorizeRemoteReservationStopRequest                  AuthorizeRemoteReservationStopRequest  = null;
                Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest> Acknowledgement                        = null;

                #region Send OnAuthorizeRemoteReservationStopSOAPRequest event

                var StartTime = DateTime.Now;

                try
                {

                    OnAuthorizeRemoteReservationStopSOAPRequest?.Invoke(StartTime,
                                                                        this.SOAPServer,
                                                                        HTTPRequest);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteReservationStopSOAPRequest));
                }

                #endregion


                if (EMP.AuthorizeRemoteReservationStopRequest.TryParse(AuthorizeRemoteReservationStopXML,
                                                                       out AuthorizeRemoteReservationStopRequest,
                                                                       null,
                                                                       HTTPRequest.Timestamp,
                                                                       HTTPRequest.CancellationToken,
                                                                       HTTPRequest.EventTrackingId,
                                                                       HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnAuthorizeRemoteReservationStopRequest event

                    try
                    {

                        OnAuthorizeRemoteReservationStopRequest?.Invoke(StartTime,
                                                                         AuthorizeRemoteReservationStopRequest.Timestamp.Value,
                                                                         this,
                                                                         nameof(CPOServer),  // ClientId
                                                                         AuthorizeRemoteReservationStopRequest.EventTrackingId,
                                                                         AuthorizeRemoteReservationStopRequest.EVSEId,
                                                                         AuthorizeRemoteReservationStopRequest.SessionId,
                                                                         AuthorizeRemoteReservationStopRequest.PartnerSessionId,
                                                                         AuthorizeRemoteReservationStopRequest.ProviderId,
                                                                         AuthorizeRemoteReservationStopRequest.RequestTimeout.HasValue ? AuthorizeRemoteReservationStopRequest.RequestTimeout.Value : DefaultRequestTimeout);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteReservationStopRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    var results = OnAuthorizeRemoteReservationStop?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnAuthorizeRemoteReservationStopDelegate)
                                          (DateTime.Now,
                                           this,
                                           AuthorizeRemoteReservationStopRequest)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        Acknowledgement = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || Acknowledgement == null)
                        Acknowledgement = Acknowledgement<EMP.AuthorizeRemoteReservationStopRequest>.SystemError(
                                             AuthorizeRemoteReservationStopRequest,
                                             "Could not process the incoming AuthorizeRemoteReservationStop request!",
                                             null,
                                             AuthorizeRemoteReservationStopRequest.SessionId,
                                             AuthorizeRemoteReservationStopRequest.PartnerSessionId
                                         );

                    #endregion

                    #region Send OnAuthorizeRemoteReservationStopResponse event

                    var EndTime = DateTime.Now;

                    try
                    {

                        OnAuthorizeRemoteReservationStopResponse?.Invoke(EndTime,
                                                                          this,
                                                                          nameof(CPOServer),  // ClientId
                                                                          AuthorizeRemoteReservationStopRequest.EventTrackingId,
                                                                          AuthorizeRemoteReservationStopRequest.EVSEId,
                                                                          AuthorizeRemoteReservationStopRequest.SessionId,
                                                                          AuthorizeRemoteReservationStopRequest.PartnerSessionId,
                                                                          AuthorizeRemoteReservationStopRequest.ProviderId,
                                                                          AuthorizeRemoteReservationStopRequest.RequestTimeout.HasValue ? AuthorizeRemoteReservationStopRequest.RequestTimeout.Value : DefaultRequestTimeout,
                                                                          Acknowledgement,
                                                                          EndTime - StartTime);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteReservationStopResponse));
                    }

                    #endregion

                }


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(Acknowledgement.ToXML()).ToUTF8Bytes()
                };

                #endregion

                #region Send OnAuthorizeRemoteReservationStopSOAPResponse event

                try
                {

                    OnAuthorizeRemoteReservationStopSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                                         this.SOAPServer,
                                                                         HTTPRequest,
                                                                         HTTPResponse);

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
                                            URIPrefix + AuthorizationURI,
                                            "AuthorizeRemoteStart",
                                            XML => XML.Descendants(OICPNS.Authorization + "eRoamingAuthorizeRemoteStart").FirstOrDefault(),
                                            async (HTTPRequest, AuthorizeRemoteStartXML) => {


                EMP.AuthorizeRemoteStartRequest                  AuthorizeRemoteStartRequest  = null;
                Acknowledgement<EMP.AuthorizeRemoteStartRequest> Acknowledgement              = null;

                #region Send OnAuthorizeRemoteStartSOAPRequest event

                var StartTime = DateTime.Now;

                try
                {

                    OnAuthorizeRemoteStartSOAPRequest?.Invoke(StartTime,
                                                              this.SOAPServer,
                                                              HTTPRequest);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteStartSOAPRequest));
                }

                #endregion


                if (EMP.AuthorizeRemoteStartRequest.TryParse(AuthorizeRemoteStartXML,
                                                             out AuthorizeRemoteStartRequest,
                                                             null,
                                                             HTTPRequest.Timestamp,
                                                             HTTPRequest.CancellationToken,
                                                             HTTPRequest.EventTrackingId,
                                                             HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnAuthorizeRemoteStartRequest event

                    try
                    {

                        OnAuthorizeRemoteStartRequest?.Invoke(StartTime,
                                                              AuthorizeRemoteStartRequest.Timestamp.Value,
                                                              this,
                                                              nameof(CPOServer),  // ClientId
                                                              AuthorizeRemoteStartRequest.EventTrackingId,
                                                              AuthorizeRemoteStartRequest.EVSEId,
                                                              AuthorizeRemoteStartRequest.PartnerProductId,
                                                              AuthorizeRemoteStartRequest.SessionId,
                                                              AuthorizeRemoteStartRequest.PartnerSessionId,
                                                              AuthorizeRemoteStartRequest.ProviderId,
                                                              AuthorizeRemoteStartRequest.EVCOId,
                                                              AuthorizeRemoteStartRequest.RequestTimeout.HasValue ? AuthorizeRemoteStartRequest.RequestTimeout.Value : DefaultRequestTimeout);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteStartRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    var results = OnAuthorizeRemoteStart?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnAuthorizeRemoteStartDelegate)
                                          (DateTime.Now,
                                           this,
                                           AuthorizeRemoteStartRequest)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        Acknowledgement = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || Acknowledgement == null)
                        Acknowledgement = Acknowledgement<EMP.AuthorizeRemoteStartRequest>.SystemError(
                                             AuthorizeRemoteStartRequest,
                                             "Could not process the incoming AuthorizeRemoteStart request!",
                                             null,
                                             AuthorizeRemoteStartRequest.SessionId,
                                             AuthorizeRemoteStartRequest.PartnerSessionId
                                         );

                    #endregion

                    #region Send OnAuthorizeRemoteStartResponse event

                    var EndTime = DateTime.Now;

                    try
                    {

                        OnAuthorizeRemoteStartResponse?.Invoke(EndTime,
                                                               this,
                                                               nameof(CPOServer),  // ClientId
                                                               AuthorizeRemoteStartRequest.EventTrackingId,
                                                               AuthorizeRemoteStartRequest.EVSEId,
                                                               AuthorizeRemoteStartRequest.PartnerProductId,
                                                               AuthorizeRemoteStartRequest.SessionId,
                                                               AuthorizeRemoteStartRequest.PartnerSessionId,
                                                               AuthorizeRemoteStartRequest.ProviderId,
                                                               AuthorizeRemoteStartRequest.EVCOId,
                                                               AuthorizeRemoteStartRequest.RequestTimeout.HasValue ? AuthorizeRemoteStartRequest.RequestTimeout.Value : DefaultRequestTimeout,
                                                               Acknowledgement,
                                                               EndTime - StartTime);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteStartResponse));
                    }

                    #endregion

                }


                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(Acknowledgement.ToXML()).ToUTF8Bytes()
                };

                #endregion

                #region Send OnAuthorizeRemoteStartSOAPResponse event

                try
                {

                    OnAuthorizeRemoteStartSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                               this.SOAPServer,
                                                               HTTPRequest,
                                                               HTTPResponse);

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
                                            URIPrefix + AuthorizationURI,
                                            "AuthorizeRemoteStop",
                                            XML => XML.Descendants(OICPNS.Authorization + "eRoamingAuthorizeRemoteStop").FirstOrDefault(),
                                            async (HTTPRequest, AuthorizeRemoteStopXML) => {

                EMP.AuthorizeRemoteStopRequest                  AuthorizeRemoteStopRequest  = null;
                Acknowledgement<EMP.AuthorizeRemoteStopRequest> Acknowledgement             = null;

                #region Send OnAuthorizeRemoteStopSOAPRequest event

                var StartTime = DateTime.Now;

                try
                {

                    OnAuthorizeRemoteStopSOAPRequest?.Invoke(StartTime,
                                                             this.SOAPServer,
                                                             HTTPRequest);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteStopSOAPRequest));
                }

                #endregion


                if (EMP.AuthorizeRemoteStopRequest.TryParse(AuthorizeRemoteStopXML,
                                                            out AuthorizeRemoteStopRequest,
                                                            null,
                                                            HTTPRequest.Timestamp,
                                                            HTTPRequest.CancellationToken,
                                                            HTTPRequest.EventTrackingId,
                                                            HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnAuthorizeRemoteStopRequest event

                    try
                    {

                        OnAuthorizeRemoteStopRequest?.Invoke(StartTime,
                                                             AuthorizeRemoteStopRequest.Timestamp.Value,
                                                             this,
                                                             nameof(CPOServer),  // ClientId
                                                             AuthorizeRemoteStopRequest.EventTrackingId,
                                                             AuthorizeRemoteStopRequest.EVSEId,
                                                             AuthorizeRemoteStopRequest.SessionId,
                                                             AuthorizeRemoteStopRequest.PartnerSessionId,
                                                             AuthorizeRemoteStopRequest.ProviderId,
                                                             AuthorizeRemoteStopRequest.RequestTimeout.HasValue ? AuthorizeRemoteStopRequest.RequestTimeout.Value : DefaultRequestTimeout);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteStopRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    var results = OnAuthorizeRemoteStop?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnAuthorizeRemoteStopDelegate)
                                          (DateTime.Now,
                                           this,
                                           AuthorizeRemoteStopRequest)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        Acknowledgement = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || Acknowledgement == null)
                        Acknowledgement = Acknowledgement<EMP.AuthorizeRemoteStopRequest>.SystemError(
                                             AuthorizeRemoteStopRequest,
                                             "Could not process the incoming AuthorizeRemoteStop request!",
                                             null,
                                             AuthorizeRemoteStopRequest.SessionId,
                                             AuthorizeRemoteStopRequest.PartnerSessionId
                                         );

                    #endregion

                    #region Send OnAuthorizeRemoteStopResponse event

                    var EndTime = DateTime.Now;

                    try
                    {

                        OnAuthorizeRemoteStopResponse?.Invoke(EndTime,
                                                              this,
                                                              nameof(CPOServer),  // ClientId
                                                              AuthorizeRemoteStopRequest.EventTrackingId,
                                                              AuthorizeRemoteStopRequest.EVSEId,
                                                              AuthorizeRemoteStopRequest.SessionId,
                                                              AuthorizeRemoteStopRequest.PartnerSessionId,
                                                              AuthorizeRemoteStopRequest.ProviderId,
                                                              AuthorizeRemoteStopRequest.RequestTimeout.HasValue ? AuthorizeRemoteStopRequest.RequestTimeout.Value : DefaultRequestTimeout,
                                                              Acknowledgement,
                                                              EndTime - StartTime);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnAuthorizeRemoteStopResponse));
                    }

                    #endregion

                }


                #region Create SOAP response

                var HTTPResponse = new HTTPResponseBuilder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(Acknowledgement.ToXML()).ToUTF8Bytes()
                };

                #endregion

                #region Send OnAuthorizeRemoteStopSOAPResponse event

                try
                {

                    OnAuthorizeRemoteStopSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                              this.SOAPServer,
                                                              HTTPRequest,
                                                              HTTPResponse);

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
