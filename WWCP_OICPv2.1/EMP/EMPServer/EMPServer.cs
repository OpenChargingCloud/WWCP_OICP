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
        public event RequestLogHandler                 OnAuthorizeStartSOAPRequest;

        /// <summary>
        /// An event sent whenever a authorize start request was received.
        /// </summary>
        public event OnAuthorizeStartRequestHandler    OnAuthorizeStartRequest;

        /// <summary>
        /// An event sent whenever a authorize start command was received.
        /// </summary>
        public event OnAuthorizeStartDelegate          OnAuthorizeStart;

        /// <summary>
        /// An event sent whenever a authorize start response was sent.
        /// </summary>
        public event OnAuthorizeStartResponseHandler   OnAuthorizeStartResponse;

        /// <summary>
        /// An event sent whenever a authorize start SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                  OnAuthorizeStartSOAPResponse;

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
                                            async (Request, AuthorizeStartXML) => {

                #region Documentation

                // <soapenv:Envelope xmlns:soapenv       = "http://schemas.xmlsoap.org/soap/envelope/"
                //                   xmlns:Authorization = "http://www.hubject.com/b2b/services/authorization/v2.0"
                //                   xmlns:CommonTypes   = "http://www.hubject.com/b2b/services/commontypes/v2.0">
                //
                //    <soapenv:Header/>
                //
                //    <soapenv:Body>
                //       <Authorization:eRoamingAuthorizeAuthorizeStart>
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
                //       </Authorization:eRoamingAuthorizeAuthorizeStart>
                //    </soapenv:Body>
                //
                // </soapenv:Envelope>


                // POST /RemoteStartStop HTTP/1.1
                // Content-type: text/xml;charset=utf-8
                // Soapaction: ""
                // Accept: text/xml, multipart/related
                // User-Agent: JAX-WS RI 2.2-hudson-752-
                // Cache-Control: no-cache
                // Pragma: no-cache
                // Host: 5.9.142.73:7001
                // Connection: keep-alive
                // Content-Length: 822
                // 
                // <?xml version='1.0' encoding='UTF-8'?>
                // <soapenv:Envelope xmlns:cmn     = "http://www.hubject.com/b2b/services/commontypes/v2.0"
                //                   xmlns:fn      = "http://www.w3.org/2005/xpath-functions"
                //                   xmlns:isns    = "http://schemas.xmlsoap.org/soap/envelope/"
                //                   xmlns:sbp     = "http://www.inubit.com/eMobility/SBP"
                //                   xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                //                   xmlns:tns     = "http://www.hubject.com/b2b/services/authorization/v2.0">
                // 
                //   <soapenv:Body>
                // 
                //     <tns:eRoamingAuthorizeStart>
                //
                //       <tns:SessionID>88efb713-0a88-1296-5c83-2e66786be68b</tns:SessionID>
                //       <tns:OperatorID>+49*822</tns:OperatorID>
                //       <tns:EVSEID>+49*822*028630243*1</tns:EVSEID>
                //
                //       <tns:Identification>
                //         <cmn:RFIDmifarefamilyIdentification>
                //           <cmn:UID>AA3634527A2280</cmn:UID>
                //         </cmn:RFIDmifarefamilyIdentification>
                //       </tns:Identification>
                //
                //       <tns:PartnerProductID>AC1</tns:PartnerProductID>
                // 
                //     </tns:eRoamingAuthorizeStart>
                //
                //   </soapenv:Body>
                //
                // </soapenv:Envelope>

                #endregion

                #region Send OnAuthorizeStartSOAPRequest event

                var StartTime = DateTime.Now;

                try
                {

                    OnAuthorizeStartSOAPRequest?.Invoke(StartTime,
                                                        this.SOAPServer,
                                                        Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPServer) + "." + nameof(OnAuthorizeStartSOAPRequest));
                }

                #endregion


                #region Parse request parameters

                Session_Id?         SessionId          = null;
                PartnerSession_Id?  PartnerSessionId   = null;
                Operator_Id         OperatorId         = default(Operator_Id);
                EVSE_Id             EVSEId             = default(EVSE_Id);
                PartnerProduct_Id?  PartnerProductId   = null;
                UID                 UID                = default(UID);

                AuthorizationStart  response           = null;

                try
                {

                    XElement            SessionIdXML;
                    XElement            PartnerSessionIdXML;
                    XElement            EVSEIdXML;
                    XElement            IdentificationXML;
                    XElement            PartnerProductIdXML;

                    SessionIdXML = AuthorizeStartXML.Element(OICPNS.Authorization + "SessionID");
                    if (SessionIdXML != null)
                        SessionId            = Session_Id.Parse(AuthorizeStartXML.ElementValueOrDefault(OICPNS.Authorization + "SessionID",   null));

                    PartnerSessionIdXML      = AuthorizeStartXML.Element(OICPNS.Authorization + "PartnerSessionID");
                    if (PartnerSessionIdXML != null)
                        PartnerSessionId = PartnerSession_Id.Parse(PartnerSessionIdXML.Value);

                    OperatorId               = Operator_Id.   Parse(AuthorizeStartXML.ElementValueOrFail   (OICPNS.Authorization + "OperatorID",  "No OperatorID XML tag provided!"));

                    EVSEIdXML = AuthorizeStartXML.Element(OICPNS.Authorization + "EVSEID");
                    if (EVSEIdXML != null)
                        EVSEId               = EVSE_Id.           Parse(EVSEIdXML.Value);

                    IdentificationXML = AuthorizeStartXML.Element(OICPNS.Authorization + "Identification");
                    if (IdentificationXML != null)
                    {

                        var RFIDmifarefamilyIdentificationXML = IdentificationXML.Element(OICPNS.CommonTypes + "RFIDmifarefamilyIdentification");
                        if (RFIDmifarefamilyIdentificationXML != null)
                        {

                            var UIDXML = RFIDmifarefamilyIdentificationXML.Element(OICPNS.CommonTypes + "UID");

                            if (UIDXML != null)
                                UID = UID.Parse(UIDXML.Value);

                        }

                    }
                    else
                        throw new Exception("Missing 'Identification'-XML tag!");

                    PartnerProductIdXML = AuthorizeStartXML.Element(OICPNS.Authorization + "PartnerProductID");
                    if (PartnerProductIdXML != null)
                        PartnerProductId = PartnerProduct_Id.Parse(PartnerProductIdXML.Value);

                }
                catch (Exception e)
                {

                    response = AuthorizationStart.DataError(
                                   "The AuthorizeStart request led to an exception!",
                                   e.Message,
                                   SessionId,
                                   PartnerSessionId
                               );

                }

                #endregion

                #region Send OnAuthorizeStartRequest event

                try
                {

                    OnAuthorizeStartRequest?.Invoke(StartTime,
                                                    StartTime, //Request.Timestamp.Value,
                                                    this,
                                                    "EMPServer", //ClientId,
                                                    Request.EventTrackingId,
                                                    OperatorId,
                                                    UID,
                                                    EVSEId,
                                                    SessionId,
                                                    PartnerProductId,
                                                    PartnerSessionId,
                                                    Request.Timeout.HasValue ? Request.Timeout.Value : DefaultRequestTimeout); //RequestTimeout.HasValue ? Request.RequestTimeout.Value : RequestTimeout.Value);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPClient) + "." + nameof(OnAuthorizeStartRequest));
                }

                #endregion

                #region Call async subscribers

                if (response == null)
                {

                    var results = OnAuthorizeStart?.
                                  GetInvocationList()?.
                                  SafeSelect(subscriber => (subscriber as OnAuthorizeStartDelegate)
                                      (DateTime.Now,
                                       this,
                                       Request.CancellationToken,
                                       Request.EventTrackingId,
                                       OperatorId,
                                       UID,
                                       EVSEId,
                                       SessionId,
                                       PartnerProductId,
                                       PartnerSessionId,
                                       DefaultRequestTimeout)).
                                  ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = AuthorizationStart.SystemError(
                                       "Could not process the incoming AuthorizationStart request!",
                                       null,
                                       SessionId,
                                       PartnerSessionId
                                   );

                }

                #endregion

                #region Send OnAuthorizeStartResponse event

                var EndTime = DateTime.Now;

                try
                {

                    OnAuthorizeStartResponse?.Invoke(EndTime,
                                                     //EndTime, //Request.Timestamp.Value,
                                                     this,
                                                     "EMPServer", //ClientId,
                                                     Request.EventTrackingId,
                                                     OperatorId,
                                                     UID,
                                                     EVSEId,
                                                     SessionId,
                                                     PartnerProductId,
                                                     PartnerSessionId,
                                                     Request.Timeout.HasValue ? Request.Timeout.Value : DefaultRequestTimeout,
                                                     response,
                                                     EndTime - StartTime);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPClient) + "." + nameof(OnAuthorizeStartResponse));
                }

                #endregion

                #region Create SOAP response

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = EndTime,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnAuthorizeStartSOAPResponse event

                try
                {

                    OnAuthorizeStartSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                         this.SOAPServer,
                                                         Request,
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
                                            async (Request, AuthorizeStopXML) => {

                #region Documentation

                // <soapenv:Envelope xmlns:soapenv       = "http://schemas.xmlsoap.org/soap/envelope/"
                //                   xmlns:Authorization = "http://www.hubject.com/b2b/services/authorization/v2.0">
                //
                //    <soapenv:Header/>
                //
                //    <soapenv:Body>
                //       <Authorization:eRoamingAuthorizeAuthorizeStop>
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
                //       </Authorization:eRoamingAuthorizeAuthorizeStop>
                //    </soapenv:Body>
                //
                // </soapenv:Envelope>

                #endregion

                #region Send OnLogAuthorizeStop event

                try
                {

                    OnAuthorizeStopSOAPRequest?.Invoke(DateTime.Now,
                                               this.SOAPServer,
                                               Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPServer) + "." + nameof(OnAuthorizeStartSOAPRequest));
                }

                #endregion


                #region Parse request parameters

                Session_Id?         SessionId          = null;
                PartnerSession_Id?  PartnerSessionId   = null;
                Operator_Id         OperatorId         = default(Operator_Id);
                EVSE_Id             EVSEId             = default(EVSE_Id);
                UID                 UID                = default(UID);

                AuthorizationStop   response           = null;

                try
                {

                    XElement  PartnerSessionIdXML;
                    XElement  IdentificationXML;

                    SessionId         = Session_Id.Parse(AuthorizeStopXML.ElementValueOrFail    (OICPNS.Authorization + "SessionID",  "No SessionID XML tag provided!"));

                    PartnerSessionIdXML = AuthorizeStopXML.Element(OICPNS.Authorization + "PartnerSessionID");
                    if (PartnerSessionIdXML != null)
                        PartnerSessionId = PartnerSession_Id.Parse(PartnerSessionIdXML.Value);

                    OperatorId        = Operator_Id.   Parse(AuthorizeStopXML.ElementValueOrFail(OICPNS.Authorization + "OperatorID",  "No OperatorID XML tag provided!"));
                    EVSEId            = EVSE_Id.           Parse(AuthorizeStopXML.ElementValueOrFail(OICPNS.Authorization + "EVSEID",      "No EVSEID XML tag provided!"));

                    IdentificationXML = AuthorizeStopXML.Element(OICPNS.Authorization + "Identification");
                    if (IdentificationXML != null)
                    {


                        var RFIDmifarefamilyIdentificationXML = IdentificationXML.Element(OICPNS.CommonTypes + "RFIDmifarefamilyIdentification");
                        if (RFIDmifarefamilyIdentificationXML != null)
                        {

                            var UIDXML = RFIDmifarefamilyIdentificationXML.Element(OICPNS.CommonTypes + "UID");

                            if (UIDXML != null)
                                UID = UID.Parse(UIDXML.Value);

                        }

                    }

                }
                catch (Exception e)
                {

                    response = AuthorizationStop.DataError(
                                   "The AuthorizeStop request led to an exception!",
                                   e.Message,
                                   SessionId,
                                   PartnerSessionId
                               );

                }

                #endregion

                #region Call async subscribers

                if (response == null)
                {

                    var results = OnAuthorizeStop?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnAuthorizeStopDelegate)
                                          (DateTime.Now,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           SessionId,
                                           PartnerSessionId,
                                           OperatorId,
                                           EVSEId,
                                           UID,
                                           DefaultRequestTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = AuthorizationStop.SystemError(
                                       "Could not process the incoming AuthorizeStop request!",
                                       null,
                                       SessionId,
                                       PartnerSessionId
                                   );

                }

                #endregion

                #region Create SOAP response

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnLogAuthorizeStopped event

                try
                {

                    OnAuthorizeStopSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                  this.SOAPServer,
                                                  Request,
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
