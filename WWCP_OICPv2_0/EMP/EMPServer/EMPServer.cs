/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
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

namespace org.GraphDefined.WWCP.OICPv2_0
{

    /// <summary>
    /// An OICP v2.0 EMP HTTP/SOAP/XML server.
    /// </summary>
    public class EMPServer : ASOAPServer
    {

        #region Data

        /// <summary>
        /// The default HTTP/SOAP/XML server name.
        /// </summary>
        public new const           String    DefaultHTTPServerName  = "GraphDefined OICP v2.0 HTTP/SOAP/XML EMP Server API";

        /// <summary>
        /// The default HTTP/SOAP/XML server TCP port.
        /// </summary>
        public new static readonly IPPort    DefaultHTTPServerPort  = new IPPort(2003);

        /// <summary>
        /// The default query timeout.
        /// </summary>
        public new static readonly TimeSpan  DefaultQueryTimeout    = TimeSpan.FromMinutes(1);

        #endregion

        #region Properties

        #region AuthorizatorId

        private readonly Authorizator_Id _AuthorizatorId;

        public Authorizator_Id AuthorizatorId
        {
            get
            {
                return _AuthorizatorId;
            }
        }

        #endregion

        #endregion

        #region Events

        #region OnAuthorizeStart

        /// <summary>
        /// An event sent whenever a authorize start command was received.
        /// </summary>
        public event RequestLogHandler         OnLogAuthorizeStart;

        /// <summary>
        /// An event sent whenever a authorize start response was sent.
        /// </summary>
        public event AccessLogHandler          OnLogAuthorizeStarted;

        /// <summary>
        /// An event sent whenever a authorize start command was received.
        /// </summary>
        public event OnAuthorizeStartDelegate  OnAuthorizeStart;

        #endregion

        #region OnAuthorizeStop

        /// <summary>
        /// An event sent whenever a authorize start command was received.
        /// </summary>
        public event RequestLogHandler         OnLogAuthorizeStop;

        /// <summary>
        /// An event sent whenever a authorize start response was sent.
        /// </summary>
        public event AccessLogHandler          OnLogAuthorizeStopped;

        /// <summary>
        /// An event sent whenever a authorize start command was received.
        /// </summary>
        public event OnAuthorizeStopDelegate   OnAuthorizeStop;

        #endregion

        #region OnChargeDetailRecord

        /// <summary>
        /// An event sent whenever a charge detail record was received.
        /// </summary>
        public event RequestLogHandler             OnLogChargeDetailRecordSend;

        /// <summary>
        /// An event sent whenever a charge detail record response was sent.
        /// </summary>
        public event AccessLogHandler              OnLogChargeDetailRecordSent;

        /// <summary>
        /// An event sent whenever a charge detail record was received.
        /// </summary>
        public event OnChargeDetailRecordDelegate  OnChargeDetailRecord;

        #endregion

        #endregion

        #region Constructor(s)

        #region EMPServer(HTTPServerName, TCPPort = null, URIPrefix = "", DNSClient = null, AutoStart = false)

        /// <summary>
        /// Initialize an new HTTP server for the OICP HTTP/SOAP/XML EMP Server API using IPAddress.Any.
        /// </summary>
        /// <param name="HTTPServerName">An optional identification string for the HTTP server.</param>
        /// <param name="TCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="AutoStart">Start the server immediately.</param>
        public EMPServer(String    HTTPServerName  = DefaultHTTPServerName,
                         IPPort    TCPPort         = null,
                         String    URIPrefix       = "",
                         DNSClient DNSClient       = null,
                         Boolean   AutoStart       = false)

            : base(HTTPServerName.IsNotNullOrEmpty() ? HTTPServerName : DefaultHTTPServerName,
                   TCPPort ?? DefaultHTTPServerPort,
                   URIPrefix,
                   HTTPContentType.XMLTEXT_UTF8,
                   DNSClient,
                   AutoStart: false)

        {

            if (AutoStart)
                Start();

        }

        #endregion

        #region EMPServer(SOAPServer, URIPrefix = "")

        /// <summary>
        /// Use the given HTTP server for the OICP HTTP/SOAP/XML EMP Server API using IPAddress.Any.
        /// </summary>
        /// <param name="SOAPServer">A SOAP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        public EMPServer(SOAPServer  SOAPServer,
                         String      URIPrefix  = "")

            : base(SOAPServer,
                   URIPrefix)

        { }

        #endregion

        #endregion


        #region (override) RegisterURITemplates()

        protected override void RegisterURITemplates()
        {

            #region / (HTTPRoot)

            SOAPServer.AddMethodCallback(HTTPMethod.GET,
                                         new String[] { "/", URIPrefix + "/" },
                                         HTTPContentType.TEXT_UTF8,
                                         HTTPDelegate: Request => {

                                             return new HTTPResponseBuilder(Request) {

                                                 HTTPStatusCode  = HTTPStatusCode.BadGateway,
                                                 ContentType     = HTTPContentType.TEXT_UTF8,
                                                 Content         = ("Welcome at " + DefaultHTTPServerName + Environment.NewLine +
                                                                    "This is a HTTP/SOAP/XML endpoint!" + Environment.NewLine + Environment.NewLine +
                                                                    "Defined endpoints: " + Environment.NewLine + Environment.NewLine +
                                                                    SOAPServer.
                                                                        SOAPDispatchers.
                                                                        Select(group => " - " + group.Key + Environment.NewLine +
                                                                                        "   " + group.SelectMany(dispatcher => dispatcher.SOAPDispatches).
                                                                                                      Select    (dispatch   => dispatch.  Description).
                                                                                                      AggregateWith(", ")
                                                                              ).AggregateWith(Environment.NewLine + Environment.NewLine)
                                                                   ).ToUTF8Bytes(),
                                                 Connection      = "close"

                                             };

                                         },
                                         AllowReplacement: URIReplacement.Allow);

            #endregion

            #region /Authorization - AuthorizeStart

            SOAPServer.RegisterSOAPDelegate(URIPrefix + "/Authorization",
                                            "AuthorizeStart",
                                            XML => XML.Descendants(OICPNS.Authorization + "eRoamingAuthorizeStart").FirstOrDefault(),
                                            (Request, AuthorizeStartXML) => {

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

                #region Send OnLogAuthorizeStart event

                try
                {

                    OnLogAuthorizeStart?.Invoke(DateTime.Now,
                                                this.SOAPServer,
                                                Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPServer) + "." + nameof(OnLogAuthorizeStart));
                }

                #endregion


                #region Parse request parameters

                XElement            SessionIdXML;
                XElement            PartnerSessionIdXML;
                XElement            EVSEIdXML;
                XElement            IdentificationXML;
                XElement            ChargingProductIdXML;

                ChargingSession_Id  SessionId           = null;
                ChargingSession_Id  PartnerSessionId    = null;
                EVSEOperator_Id     OperatorId          = null;
                EVSE_Id             EVSEId              = null;
                eMA_Id              eMAId               = null;
                ChargingProduct_Id  ChargingProductId   = null;
                Auth_Token          AuthToken           = null;

                SessionIdXML = AuthorizeStartXML.Element(OICPNS.Authorization + "SessionID");
                if (SessionIdXML != null)
                    SessionId            = ChargingSession_Id.Parse(AuthorizeStartXML.ElementValueOrDefault(OICPNS.Authorization + "SessionID",   null));

                PartnerSessionIdXML      = AuthorizeStartXML.Element(OICPNS.Authorization + "PartnerSessionID");
                if (PartnerSessionIdXML != null)
                    PartnerSessionId = ChargingSession_Id.Parse(PartnerSessionIdXML.Value);

                OperatorId               = EVSEOperator_Id.   Parse(AuthorizeStartXML.ElementValueOrFail   (OICPNS.Authorization + "OperatorID",  "No OperatorID XML tag provided!"));

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
                            AuthToken = Auth_Token.Parse(UIDXML.Value);

                    }

                }
                else
                    throw new Exception("Missing 'Identification'-XML tag!");

                ChargingProductIdXML = AuthorizeStartXML.Element(OICPNS.Authorization + "PartnerProductID");
                if (ChargingProductIdXML != null)
                    ChargingProductId = ChargingProduct_Id.Parse(ChargingProductIdXML.Value);

                #endregion

                #region Call async subscribers

                AuthStartEVSEResult result = null;

                var OnAuthorizeStartLocal = OnAuthorizeStart;
                if (OnAuthorizeStartLocal != null)
                {

                    var CTS = new CancellationTokenSource();

                    var task = OnAuthorizeStartLocal(DateTime.Now,
                                                     this,
                                                     CTS.Token,
                                                     EventTracking_Id.New,
                                                     OperatorId,
                                                     AuthToken,
                                                     EVSEId,
                                                     SessionId,
                                                     ChargingProductId,
                                                     PartnerSessionId,
                                                     DefaultQueryTimeout);

                    task.Wait();
                    result = task.Result;

                }

                #endregion


                #region Map result

                var HubjectCode            = "";
                var HubjectDescription     = "";
                var HubjectAdditionalInfo  = "";

                if (result != null)
                    switch (result.Result)
                    {

                        case AuthStartEVSEResultType.Authorized:
                            HubjectCode         = "000";
                            HubjectDescription  = "Ready to charge!";
                            break;

                        case AuthStartEVSEResultType.NotAuthorized:
                            HubjectCode         = "102";
                            HubjectDescription  = "RFID Authentication failed - invalid UID";
                            break;

                        case AuthStartEVSEResultType.InvalidSessionId:
                            HubjectCode         = "400";
                            HubjectDescription  = "Session is invalid";
                            break;

                        case AuthStartEVSEResultType.EVSECommunicationTimeout:
                            HubjectCode         = "501";
                            HubjectDescription  = "Communication to EVSE failed!";
                            break;

                        case AuthStartEVSEResultType.StartChargingTimeout:
                            HubjectCode         = "510";
                            HubjectDescription  = "No EV connected to EVSE!";
                            break;

                        case AuthStartEVSEResultType.Reserved:
                            HubjectCode         = "601";
                            HubjectDescription  = "EVSE reserved!";
                            break;

                        //Note: Can not happen, or?
                        //case AuthStartEVSEResultType.AlreadyInUse:
                        //    HubjectCode         = "602";
                        //    HubjectDescription  = "EVSE is already in use!";
                        //    break;

                        case AuthStartEVSEResultType.UnknownEVSE:
                            HubjectCode         = "603";
                            HubjectDescription  = "Unknown EVSE ID!";
                            break;

                        case AuthStartEVSEResultType.OutOfService:
                            HubjectCode         = "700";
                            HubjectDescription  = "EVSE out of service!";
                            break;


                        default:
                            HubjectCode         = "320";
                            HubjectDescription  = "Service not available!";
                            break;

                    }

                #endregion

                #region Send SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(
                                          new XElement(OICPNS.Authorization + "eRoamingAuthorizationStart",

                                              result.SessionId != null
                                                  ? new XElement(OICPNS.Authorization + "SessionID",         result.SessionId.ToString())
                                                  : SessionId != null
                                                        ? new XElement(OICPNS.Authorization + "SessionID",   SessionId.ToString())
                                                        : null,

                                              PartnerSessionId != null
                                                  ? new XElement(OICPNS.Authorization + "PartnerSessionID",  PartnerSessionId.ToString())
                                                  : null,

                                              result.ProviderId != null
                                                  ? new XElement(OICPNS.Authorization + "ProviderID",        result.ProviderId.ToString())
                                                  : null,

                                              new XElement(OICPNS.Authorization + "AuthorizationStatus", result.Result == AuthStartEVSEResultType.Authorized ? "Authorized" : "NotAuthorized"),

                                              new XElement(OICPNS.Authorization + "StatusCode",

                                                  new XElement(OICPNS.CommonTypes +       "Code",            HubjectCode),

                                                  HubjectDescription.IsNotNullOrEmpty()
                                                      ? new XElement(OICPNS.CommonTypes + "Description",     HubjectDescription)
                                                      : null,

                                                  HubjectAdditionalInfo.IsNotNullOrEmpty()
                                                      ? new XElement(OICPNS.CommonTypes + "AdditionalInfo",  HubjectAdditionalInfo)
                                                      : null

                                              )

                                         )).ToUTF8Bytes()
                };

                #endregion


                #region Send OnLogAuthorizeStarted event

                try
                {

                    OnLogAuthorizeStarted?.Invoke(HTTPResponse.Timestamp,
                                                  this.SOAPServer,
                                                  Request,
                                                  HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPServer) + "." + nameof(OnLogAuthorizeStarted));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region /Authorization - AuthorizeStop

            SOAPServer.RegisterSOAPDelegate(URIPrefix + "/Authorization",
                                            "AuthorizeStop",
                                            XML => XML.Descendants(OICPNS.Authorization + "eRoamingAuthorizeStop").FirstOrDefault(),
                                            (Request, AuthorizeStopXML) => {

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

                        OnLogAuthorizeStop?.Invoke(DateTime.Now,
                                                   this.SOAPServer,
                                                   Request);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(EMPServer) + "." + nameof(OnLogAuthorizeStart));
                    }

                    #endregion


                    #region Parse request parameters

                    XElement            PartnerSessionIdXML;
                    XElement            IdentificationXML;

                    ChargingSession_Id  SessionId;
                    ChargingSession_Id  PartnerSessionId  = null;
                    EVSEOperator_Id     OperatorId;
                    EVSE_Id             EVSEId            = null;
                    Auth_Token          AuthToken         = null;

                    try
                    {

                        SessionId         = ChargingSession_Id.Parse(AuthorizeStopXML.ElementValueOrFail    (OICPNS.Authorization + "SessionID",        "No SessionID XML tag provided!"));

                        PartnerSessionIdXML = AuthorizeStopXML.Element(OICPNS.Authorization + "PartnerSessionID");
                        if (PartnerSessionIdXML != null)
                            PartnerSessionId = ChargingSession_Id.Parse(PartnerSessionIdXML.Value);

                        OperatorId        = EVSEOperator_Id.   Parse(AuthorizeStopXML.ElementValueOrFail(OICPNS.Authorization + "OperatorID",       "No OperatorID XML tag provided!"));
                        EVSEId            = EVSE_Id.           Parse(AuthorizeStopXML.ElementValueOrFail(OICPNS.Authorization + "EVSEID",           "No EVSEID XML tag provided!"));

                        IdentificationXML = AuthorizeStopXML.Element(OICPNS.Authorization + "Identification");
                        if (IdentificationXML != null)
                        {


                            var RFIDmifarefamilyIdentificationXML = IdentificationXML.Element(OICPNS.CommonTypes + "RFIDmifarefamilyIdentification");
                            if (RFIDmifarefamilyIdentificationXML != null)
                            {

                                var UIDXML = RFIDmifarefamilyIdentificationXML.Element(OICPNS.CommonTypes + "UID");

                                if (UIDXML != null)
                                    AuthToken = Auth_Token.Parse(UIDXML.Value);

                            }

                        }

                    }
                    catch (Exception e)
                    {

                        DebugX.LogT("Invalid AuthorizeStopXML: " + e.Message);

                        return new HTTPResponseBuilder(Request) {

                                HTTPStatusCode  = HTTPStatusCode.OK,
                                ContentType     = HTTPContentType.XMLTEXT_UTF8,
                                Content         = SOAP.Encapsulation(new XElement(OICPNS.CommonTypes + "eRoamingAcknowledgement",

                                                                         new XElement(OICPNS.CommonTypes + "Result", "false"),

                                                                         new XElement(OICPNS.CommonTypes + "StatusCode",
                                                                             new XElement(OICPNS.CommonTypes + "Code",           "022"),
                                                                             new XElement(OICPNS.CommonTypes + "Description",    "Request led to an exception!"),
                                                                             new XElement(OICPNS.CommonTypes + "AdditionalInfo", e.Message)
                                                                         )

                                                                     )).ToString().ToUTF8Bytes()

                        };

                    }

                    #endregion


                    #region Call async subscribers

                    AuthStopEVSEResult Response = null;

                    var OnAuthorizeStopLocal = OnAuthorizeStop;
                    if (OnAuthorizeStopLocal != null)
                    {

                        var CTS = new CancellationTokenSource();

                        var task = OnAuthorizeStopLocal(DateTime.Now,
                                                        this,
                                                        CTS.Token,
                                                        EventTracking_Id.New,
                                                        SessionId,
                                                        PartnerSessionId,
                                                        OperatorId,
                                                        EVSEId,
                                                        AuthToken,
                                                        DefaultQueryTimeout);

                        task.Wait();
                        Response = task.Result;

                    }

                    #endregion

                    #region Map result

                    var HubjectCode            = "320";
                    var HubjectDescription     = "Service not available!";
                    var HubjectAdditionalInfo  = "";

                    if (Response != null)
                        switch (Response.Result)
                        {

                            case AuthStopEVSEResultType.Authorized:
                                HubjectCode         = "000";
                                HubjectDescription  = "Ready to stop charging!";
                                break;

                            case AuthStopEVSEResultType.InvalidSessionId:
                                HubjectCode         = "400";
                                HubjectDescription  = "Session is invalid";
                                break;

                            case AuthStopEVSEResultType.EVSECommunicationTimeout:
                                HubjectCode         = "501";
                                HubjectDescription  = "Communication to EVSE failed!";
                                break;

                            case AuthStopEVSEResultType.StopChargingTimeout:
                                HubjectCode         = "510";
                                HubjectDescription  = "No EV connected to EVSE!";
                                break;

                            case AuthStopEVSEResultType.UnknownEVSE:
                                HubjectCode         = "603";
                                HubjectDescription  = "Unknown EVSE ID!";
                                break;

                            case AuthStopEVSEResultType.OutOfService:
                                HubjectCode         = "700";
                                HubjectDescription  = "EVSE out of service!";
                                break;


                            default:
                                HubjectCode         = "320";
                                HubjectDescription  = "Service not available!";
                                break;

                        }

                    #endregion

                    #region Return SOAPResponse

                    var HTTPResponse = new HTTPResponseBuilder(Request) {
                        HTTPStatusCode  = HTTPStatusCode.OK,
                        Server          = SOAPServer.DefaultServerName,
                        Date            = DateTime.Now,
                        ContentType     = HTTPContentType.XMLTEXT_UTF8,
                        Content         = SOAP.Encapsulation(
                                              new XElement(OICPNS.CommonTypes + "eRoamingAcknowledgement",

                                                  new XElement(OICPNS.CommonTypes + "Result", "true"),

                                                  new XElement(OICPNS.CommonTypes + "StatusCode",
                                                      new XElement(OICPNS.CommonTypes + "Code",            HubjectCode),
                                                      new XElement(OICPNS.CommonTypes + "Description",     HubjectDescription),
                                                      new XElement(OICPNS.CommonTypes + "AdditionalInfo",  HubjectAdditionalInfo)
                                                  ),

                                                  new XElement(OICPNS.CommonTypes + "SessionID", SessionId)

                                              )).ToString().ToUTF8Bytes()
                    };

                    #endregion


                    #region Send OnLogAuthorizeStopped event

                    try
                    {

                        OnLogAuthorizeStopped?.Invoke(HTTPResponse.Timestamp,
                                                      this.SOAPServer,
                                                      Request,
                                                      HTTPResponse);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(EMPServer) + "." + nameof(OnLogAuthorizeStopped));
                    }

                    #endregion

                    return HTTPResponse;

            });

            #endregion

            #region /Authorization - ChargeDetailRecord

            SOAPServer.RegisterSOAPDelegate(URIPrefix + "/Authorization",
                                            "ChargeDetailRecord",
                                            XML => XML.Descendants(OICPNS.Authorization + "eRoamingChargeDetailRecord").FirstOrDefault(),
                                            (Request, ChargeDetailRecordXML) => {

                    #region Send OnLogChargeDetailRecordSend event

                    try
                    {

                        OnLogChargeDetailRecordSend?.Invoke(DateTime.Now,
                                                            this.SOAPServer,
                                                            Request);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(EMPServer) + "." + nameof(OnLogChargeDetailRecordSend));
                    }

                    #endregion


                    #region Parse request parameters

                    SendCDRResult       response  = null;
                    ChargeDetailRecord  CDR       = null;

                    try
                    {

                        CDR = ChargeDetailRecord.Parse(ChargeDetailRecordXML);

                    }
                    catch (Exception e)
                    {
                        response = SendCDRResult.Error(_AuthorizatorId, e.Message);
                    }

                    #endregion


                    #region Call async subscribers

                    if (response == null)
                    {

                        var OnChargeDetailRecordLocal = OnChargeDetailRecord;
                        if (OnChargeDetailRecordLocal != null)
                        {

                            var CTS = new CancellationTokenSource();

                            var task = OnChargeDetailRecord(DateTime.Now,
                                                            CTS.Token,
                                                            EventTracking_Id.New,
                                                            CDR,
                                                            DefaultQueryTimeout);

                            task.Wait();
                            response = task.Result;

                        }

                    }

                    #endregion

                    #region Map result

                    var HubjectCode            = "320";
                    var HubjectDescription     = "Service not available!";
                    var HubjectAdditionalInfo  = "";

                    if (response != null)
                        switch (response.Status)
                        {

                            case SendCDRResultType.NotForwared:
                                HubjectCode         = "009";
                                HubjectDescription  = "Communication to EVSE failed!";
                                break;

                            case SendCDRResultType.Forwarded:
                                HubjectCode         = "000";
                                HubjectDescription  = "Charge detail record forwarded!";
                                break;

                            case SendCDRResultType.InvalidSessionId:
                                HubjectCode         = "400";
                                HubjectDescription  = "Session is invalid";
                                break;

                            case SendCDRResultType.UnknownEVSE:
                                HubjectCode         = "603";
                                HubjectDescription  = "Unknown EVSE identification!";
                                break;

                            case SendCDRResultType.Error:
                                HubjectCode         = "022";
                                HubjectDescription  = response.Description;
                                break;


                            default:
                                HubjectCode         = "320";
                                HubjectDescription  = "Service not available!";
                                break;

                        }

                    #endregion

                    #region Return SOAPResponse

                    var HTTPResponse = new HTTPResponseBuilder(Request) {
                        HTTPStatusCode  = HTTPStatusCode.OK,
                        Server          = SOAPServer.DefaultServerName,
                        Date            = DateTime.Now,
                        ContentType     = HTTPContentType.XMLTEXT_UTF8,
                        Content         = SOAP.Encapsulation(
                                              new XElement(OICPNS.CommonTypes + "eRoamingAcknowledgement",

                                                  new XElement(OICPNS.CommonTypes + "Result", response != null && response.Status == SendCDRResultType.Forwarded ? "true" : "false"),

                                                  new XElement(OICPNS.CommonTypes + "StatusCode",
                                                      new XElement(OICPNS.CommonTypes + "Code",            HubjectCode),
                                                      new XElement(OICPNS.CommonTypes + "Description",     HubjectDescription),
                                                      new XElement(OICPNS.CommonTypes + "AdditionalInfo",  HubjectAdditionalInfo)
                                                  )

                                             )).ToUTF8Bytes()
                    };

                    #endregion


                    #region Send OnLogChargeDetailRecordSent event

                    try
                    {

                        OnLogChargeDetailRecordSent?.Invoke(HTTPResponse.Timestamp,
                                                            this.SOAPServer,
                                                            Request,
                                                            HTTPResponse);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(EMPServer) + "." + nameof(OnLogChargeDetailRecordSent));
                    }

                    #endregion

                    return HTTPResponse;

            });

            #endregion

        }

        #endregion

    }

}
