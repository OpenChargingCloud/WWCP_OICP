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
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

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
        public new const           String           DefaultHTTPServerName  = "GraphDefined OICP " + Version.Number + " HTTP/SOAP/XML EMP API";

        /// <summary>
        /// The default HTTP/SOAP/XML server TCP port.
        /// </summary>
        public new static readonly IPPort           DefaultHTTPServerPort  = new IPPort(2003);

        /// <summary>
        /// The default HTTP/SOAP/XML server URI prefix.
        /// </summary>
        public new const           String           DefaultURIPrefix       = "";

        /// <summary>
        /// The default HTTP/SOAP/XML content type.
        /// </summary>
        public new static readonly HTTPContentType  DefaultContentType     = HTTPContentType.XMLTEXT_UTF8;

        /// <summary>
        /// The default request timeout.
        /// </summary>
        public new static readonly TimeSpan         DefaultRequestTimeout  = TimeSpan.FromMinutes(1);

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

        #region EMPServer(HTTPServerName, TCPPort = default, URIPrefix = default, ContentType = default, DNSClient = null, AutoStart = false)

        /// <summary>
        /// Initialize an new HTTP server for the OICP HTTP/SOAP/XML EMP API.
        /// </summary>
        /// <param name="HTTPServerName">An optional identification string for the HTTP server.</param>
        /// <param name="TCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="ContentType">An optional HTTP content type to use.</param>
        /// <param name="RegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="AutoStart">Start the server immediately.</param>
        public EMPServer(String          HTTPServerName           = DefaultHTTPServerName,
                         IPPort          TCPPort                  = null,
                         String          URIPrefix                = DefaultURIPrefix,
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

            if (AutoStart)
                Start();

        }

        #endregion

        #region EMPServer(SOAPServer, URIPrefix = default)

        /// <summary>
        /// Use the given SOAP server for the OICP HTTP/SOAP/XML EMP API.
        /// </summary>
        /// <param name="SOAPServer">A SOAP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        public EMPServer(SOAPServer  SOAPServer,
                         String      URIPrefix  = DefaultURIPrefix)

            : base(SOAPServer,
                   URIPrefix)

        { }

        #endregion

        #endregion


        #region (override) RegisterURITemplates()

        /// <summary>
        /// Register all URI templates for this SOAP API.
        /// </summary>
        protected override void RegisterURITemplates()
        {

            #region /Authorization - AuthorizeStart

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/Authorization",
                                            "AuthorizeStart",
                                            XML => XML.Descendants(OICPNS.Authorization + "eRoamingAuthorizeStart").FirstOrDefault(),
                                            async (HTTPRequest, AuthorizeStartXML) => {


                CPO.AuthorizeStartRequest AuthorizeStartRequest  = null;
                CPO.AuthorizationStart    AuthorizationStart     = null;

                #region Send OnAuthorizeStartSOAPRequest event

                var StartTime = DateTime.Now;

                try
                {

                    OnAuthorizeStartSOAPRequest?.Invoke(StartTime,
                                                        this.SOAPServer,
                                                        HTTPRequest);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPServer) + "." + nameof(OnAuthorizeStartSOAPRequest));
                }

                #endregion


                if (CPO.AuthorizeStartRequest.TryParse(AuthorizeStartXML,
                                                       out AuthorizeStartRequest,
                                                       null,
                                                       HTTPRequest.Timestamp,
                                                       HTTPRequest.CancellationToken,
                                                       HTTPRequest.EventTrackingId,
                                                       HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnAuthorizeStartRequest event

                    try
                    {

                        OnAuthorizeStartRequest?.Invoke(StartTime,
                                                        AuthorizeStartRequest.Timestamp.Value,
                                                        this,
                                                        nameof(EMPServer),   // ClientId
                                                        AuthorizeStartRequest.EventTrackingId,
                                                        AuthorizeStartRequest.OperatorId,
                                                        AuthorizeStartRequest.UID,
                                                        AuthorizeStartRequest.EVSEId,
                                                        AuthorizeStartRequest.SessionId,
                                                        AuthorizeStartRequest.PartnerProductId,
                                                        AuthorizeStartRequest.PartnerSessionId,
                                                        AuthorizeStartRequest.RequestTimeout.HasValue ? AuthorizeStartRequest.RequestTimeout.Value : DefaultRequestTimeout);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(EMPServer) + "." + nameof(OnAuthorizeStartRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    if (AuthorizationStart == null)
                    {

                        var results = OnAuthorizeStart?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnAuthorizeStartDelegate)
                                          (DateTime.Now,
                                           this,
                                           AuthorizeStartRequest)).
                                      ToArray();

                        if (results.Length > 0)
                        {

                            await Task.WhenAll(results);

                            AuthorizationStart = results.FirstOrDefault()?.Result;

                        }

                        if (results.Length == 0 || AuthorizationStart == null)
                            AuthorizationStart = CPO.AuthorizationStart.SystemError(
                                           AuthorizeStartRequest,
                                           "Could not process the incoming AuthorizationStart request!",
                                           null,
                                           AuthorizeStartRequest.SessionId,
                                           AuthorizeStartRequest.PartnerSessionId
                                       );

                    }

                    #endregion

                    #region Send OnAuthorizeStartResponse event

                    var EndTime = DateTime.Now;

                    try
                    {

                        OnAuthorizeStartResponse?.Invoke(EndTime,
                                                         this,
                                                         nameof(EMPServer),   // ClientId
                                                         AuthorizeStartRequest.EventTrackingId,
                                                         AuthorizeStartRequest.OperatorId,
                                                         AuthorizeStartRequest.UID,
                                                         AuthorizeStartRequest.EVSEId,
                                                         AuthorizeStartRequest.SessionId,
                                                         AuthorizeStartRequest.PartnerProductId,
                                                         AuthorizeStartRequest.PartnerSessionId,
                                                         AuthorizeStartRequest.RequestTimeout.HasValue ? AuthorizeStartRequest.RequestTimeout.Value : DefaultRequestTimeout,
                                                         AuthorizationStart,
                                                         EndTime - StartTime);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(EMPServer) + "." + nameof(OnAuthorizeStartResponse));
                    }

                    #endregion

                }


                #region Create SOAP response

                var HTTPResponse = new HTTPResponseBuilder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(AuthorizationStart.ToXML()).ToUTF8Bytes()
                };

                #endregion

                #region Send OnAuthorizeStartSOAPResponse event

                try
                {

                    OnAuthorizeStartSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                         this.SOAPServer,
                                                         HTTPRequest,
                                                         HTTPResponse);

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
                                            URIPrefix + "/Authorization",
                                            "AuthorizeStop",
                                            XML => XML.Descendants(OICPNS.Authorization + "eRoamingAuthorizeStop").FirstOrDefault(),
                                            async (HTTPRequest, AuthorizeStopXML) => {


                CPO.AuthorizeStopRequest AuthorizeStopRequest  = null;
                CPO.AuthorizationStop    AuthorizationStop     = null;

                #region Send OnAuthorizeStopSOAPRequest event

                var StartTime = DateTime.Now;

                try
                {

                    OnAuthorizeStopSOAPRequest?.Invoke(StartTime,
                                                       this.SOAPServer,
                                                       HTTPRequest);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPServer) + "." + nameof(OnAuthorizeStartSOAPRequest));
                }

                #endregion


                if (CPO.AuthorizeStopRequest.TryParse(AuthorizeStopXML,
                                                      out AuthorizeStopRequest,
                                                      null,
                                                      HTTPRequest.Timestamp,
                                                      HTTPRequest.CancellationToken,
                                                      HTTPRequest.EventTrackingId,
                                                      HTTPRequest.Timeout ?? DefaultRequestTimeout))
                {

                    #region Send OnAuthorizeStopRequest event

                    try
                    {

                        OnAuthorizeStopRequest?.Invoke(StartTime,
                                                       AuthorizeStopRequest.Timestamp.Value,
                                                       this,
                                                       nameof(EMPServer),   // ClientId
                                                       AuthorizeStopRequest.EventTrackingId,
                                                       AuthorizeStopRequest.SessionId,
                                                       AuthorizeStopRequest.PartnerSessionId,
                                                       AuthorizeStopRequest.OperatorId,
                                                       AuthorizeStopRequest.EVSEId,
                                                       AuthorizeStopRequest.UID,
                                                       AuthorizeStopRequest.RequestTimeout.HasValue ? AuthorizeStopRequest.RequestTimeout.Value : DefaultRequestTimeout);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(EMPServer) + "." + nameof(OnAuthorizeStopRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    if (AuthorizationStop == null)
                    {

                        var results = OnAuthorizeStop?.
                                          GetInvocationList()?.
                                          SafeSelect(subscriber => (subscriber as OnAuthorizeStopDelegate)
                                              (DateTime.Now,
                                               this,
                                               AuthorizeStopRequest)).
                                          ToArray();

                        if (results.Length > 0)
                        {

                            await Task.WhenAll(results);

                            AuthorizationStop = results.FirstOrDefault()?.Result;

                        }

                        if (results.Length == 0 || AuthorizationStop == null)
                            AuthorizationStop = CPO.AuthorizationStop.SystemError(
                                           null,
                                           "Could not process the incoming AuthorizeStop request!",
                                           null,
                                           AuthorizeStopRequest.SessionId,
                                           AuthorizeStopRequest.PartnerSessionId
                                       );

                    }

                    #endregion

                    #region Send OnAuthorizeStopResponse event

                    var EndTime = DateTime.Now;

                    try
                    {

                        OnAuthorizeStopResponse?.Invoke(EndTime,
                                                        this,
                                                        nameof(EMPServer),   // ClientId
                                                        AuthorizeStopRequest.EventTrackingId,
                                                        AuthorizeStopRequest.SessionId,
                                                        AuthorizeStopRequest.PartnerSessionId,
                                                        AuthorizeStopRequest.OperatorId,
                                                        AuthorizeStopRequest.EVSEId,
                                                        AuthorizeStopRequest.UID,
                                                        AuthorizeStopRequest.RequestTimeout.HasValue ? AuthorizeStopRequest.RequestTimeout.Value : DefaultRequestTimeout,
                                                        AuthorizationStop,
                                                        EndTime - StartTime);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(EMPServer) + "." + nameof(OnAuthorizeStopResponse));
                    }

                    #endregion

                }


                #region Create SOAP response

                var HTTPResponse = new HTTPResponseBuilder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(AuthorizationStop.ToXML()).ToUTF8Bytes()
                };

                #endregion

                #region Send OnLogAuthorizeStopped event

                try
                {

                    OnAuthorizeStopSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                  this.SOAPServer,
                                                  HTTPRequest,
                                                  HTTPResponse);

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
                                            URIPrefix + "/Authorization",
                                            "ChargeDetailRecord",
                                            XML => XML.Descendants(OICPNS.Authorization + "eRoamingChargeDetailRecord").FirstOrDefault(),
                                            async (Request, ChargeDetailRecordXML) => {

                    #region Send OnLogChargeDetailRecordSend event

                    try
                    {

                        OnChargeDetailRecordSOAPRequest?.Invoke(DateTime.Now,
                                                                this.SOAPServer,
                                                                Request);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(EMPServer) + "." + nameof(OnChargeDetailRecordSOAPRequest));
                    }

                    #endregion


                    #region Parse request parameters

                    ChargeDetailRecord CDR       = null;
                    Acknowledgement    response  = null;

                    try
                    {

                        CDR = ChargeDetailRecord.Parse(ChargeDetailRecordXML);

                    }
                    catch (Exception e)
                    {
                        response = Acknowledgement.DataError(
                                       "The ChargeDetailRecord request led to an exception!",
                                       e.Message
                                   );
                    }

                    #endregion

                    #region Call async subscribers

                    if (response == null)
                    {

                        var results = OnChargeDetailRecord?.
                                          GetInvocationList()?.
                                          SafeSelect(subscriber => (subscriber as OnChargeDetailRecordDelegate)
                                              (DateTime.Now,
                                               this,
                                               Request.CancellationToken,
                                               Request.EventTrackingId,
                                               CDR,
                                               DefaultRequestTimeout)).
                                          ToArray();

                        if (results.Length > 0)
                        {

                            await Task.WhenAll(results);

                            response = results.FirstOrDefault()?.Result;

                        }

                        if (results.Length == 0 || response == null)
                            response = Acknowledgement.SystemError("Could not process the incoming request!");

                    }

                    #endregion

                    #region Return SOAPResponse

                    var HTTPResponse = new HTTPResponseBuilder(Request) {
                        HTTPStatusCode  = HTTPStatusCode.OK,
                        Server          = SOAPServer.DefaultServerName,
                        Date            = DateTime.Now,
                        ContentType     = HTTPContentType.XMLTEXT_UTF8,
                        Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()

                    };

                    #endregion


                    #region Send OnLogChargeDetailRecordSent event

                    try
                    {

                        OnChargeDetailRecordSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                            this.SOAPServer,
                                                            Request,
                                                            HTTPResponse);

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
