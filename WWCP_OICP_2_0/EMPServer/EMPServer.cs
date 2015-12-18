/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/GraphDefined/WWCP_OICP>
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
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.ConsoleLog;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.OICP_2_0
{

    /// <summary>
    /// OICP v2.0 EMP HTTP/SOAP/XML server.
    /// </summary>
    public class EMPServer
    {

        #region Data

        private const           String  DefaultHTTPServerName  = "OICP v2.0 EMP HTTP/SOAP/XML Server API";
        private static readonly IPPort  DefaultHTTPServerPort  = new IPPort(2002);

        #endregion

        #region Properties

        #region HTTPServer

        private readonly HTTPServer _HTTPServer;

        public HTTPServer HTTPServer
        {
            get
            {
                return _HTTPServer;
            }
        }

        #endregion

        #region URIPrefix

        private readonly String _URIPrefix;

        public String URIPrefix
        {
            get
            {
                return _URIPrefix;
            }
        }

        #endregion

        #region DNSClient

        private readonly DNSClient _DNSClient;

        public DNSClient DNSClient
        {
            get
            {
                return _DNSClient;
            }
        }

        #endregion

        #endregion

        #region Events

        #region OnAuthorizeStart

        public event OnAuthorizeStartDelegate OnAuthorizeStart;

        #endregion

        #region OnAuthorizeStop

        public event OnAuthorizeStopDelegate  OnAuthorizeStop;

        #endregion


        // Generic HTTP events...

        #region RequestLog

        /// <summary>
        /// An event called whenever a request came in.
        /// </summary>
        public event RequestLogHandler RequestLog
        {

            add
            {
                _HTTPServer.RequestLog += value;
            }

            remove
            {
                _HTTPServer.RequestLog -= value;
            }

        }

        #endregion

        #region AccessLog

        /// <summary>
        /// An event called whenever a request could successfully be processed.
        /// </summary>
        public event AccessLogHandler AccessLog
        {

            add
            {
                _HTTPServer.AccessLog += value;
            }

            remove
            {
                _HTTPServer.AccessLog -= value;
            }

        }

        #endregion

        #region ErrorLog

        /// <summary>
        /// An event called whenever a request resulted in an error.
        /// </summary>
        public event ErrorLogHandler ErrorLog
        {

            add
            {
                _HTTPServer.ErrorLog += value;
            }

            remove
            {
                _HTTPServer.ErrorLog -= value;
            }

        }

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

            : this(new HTTPServer(TCPPort != null ? TCPPort : DefaultHTTPServerPort,
                                  DefaultServerName:  HTTPServerName,
                                  DNSClient:          DNSClient,
                                  Autostart:          AutoStart),
                   URIPrefix)

        { }

        #endregion

        #region EMPServer(HTTPServer, URIPrefix = "")

        /// <summary>
        /// Use the given HTTP server for the OICP HTTP/SOAP/XML EMP Server API using IPAddress.Any.
        /// </summary>
        /// <param name="HTTPServer">A HTTP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        public EMPServer(HTTPServer  HTTPServer,
                         String      URIPrefix  = "")
        {

            #region Initial checks

            if (HTTPServer == null)
                throw new ArgumentNullException("HTTPServer", "The given parameter must not be null!");

            if (URIPrefix.Length > 0 && !URIPrefix.StartsWith("/"))
                URIPrefix = "/" + URIPrefix;

            #endregion

            this._HTTPServer  = HTTPServer;
            this._URIPrefix   = URIPrefix;
            this._DNSClient   = HTTPServer.DNSClient;

            RegisterURITemplates();

        }

        #endregion

        #endregion


        #region (private) RegisterURITemplates()

        private void RegisterURITemplates()
        {

            #region / (HTTPRoot)

            // HTML
            _HTTPServer.AddMethodCallback(HTTPMethod.GET,
                                          URIPrefix + "/",
                                          HTTPContentType.HTML_UTF8,
                                          HTTPDelegate: HTTPRequest => {

                                              var RoamingNetworkId = HTTPRequest.ParsedURIParameters[0];

                                              return new HTTPResponseBuilder() {
                                                  HTTPStatusCode  = HTTPStatusCode.BadGateway,
                                                  ContentType     = HTTPContentType.HTML_UTF8,
                                                  Content         = ("/RNs/{RoamingNetworkId}/AuthorizeStartStop is a HTTP/SOAP/XML endpoint!").ToUTF8Bytes(),
                                                  Connection      = "close"
                                              };

                                          });

            // Text
            _HTTPServer.AddMethodCallback(HTTPMethod.GET,
                                          URIPrefix + "/",
                                          HTTPContentType.TEXT_UTF8,
                                          HTTPDelegate: HTTPRequest => {

                                              var RoamingNetworkId = HTTPRequest.ParsedURIParameters[0];

                                              return new HTTPResponseBuilder() {
                                                  HTTPStatusCode  = HTTPStatusCode.BadGateway,
                                                  ContentType     = HTTPContentType.HTML_UTF8,
                                                  Content         = ("/RNs/{RoamingNetworkId}/AuthorizeStartStop is a HTTP/SOAP/XML endpoint!").ToUTF8Bytes(),
                                                  Connection      = "close"
                                              };

                                          });

            #endregion

            #region /RNs/{RoamingNetworkId}/AuthorizeStartStop

            #region Generic AuthorizeStartStopDelegate

            HTTPDelegate AuthorizeStartStopDelegate = HTTPRequest => {

                //var _EventTrackingId = EventTracking_Id.New;
                //Log.WriteLine("Event tracking: " + _EventTrackingId);

                #region Try to parse the RoamingNetworkId

                RoamingNetwork_Id RoamingNetworkId;

                if (!RoamingNetwork_Id.TryParse(HTTPRequest.ParsedURIParameters[0], out RoamingNetworkId))
                    return new HTTPResponseBuilder() {
                        HTTPStatusCode  = HTTPStatusCode.BadRequest,
                        Server          = _HTTPServer.DefaultServerName,
                    };

                #endregion

                //#region Try to get the RoamingNetwork

                //RoamingNetwork RoamingNetwork;

                //if (!RoamingNetworks.TryGetRoamingNetwork(RoamingNetworkId, out RoamingNetwork))
                //    return new HTTPResponseBuilder() {
                //        HTTPStatusCode  = HTTPStatusCode.NotFound,
                //        Server          = this.DefaultServerName,
                //    };

                //#endregion

                #region ParseXMLRequestBody... or fail!

                var XMLRequest = HTTPRequest.ParseXMLRequestBody();
                if (XMLRequest.HasErrors)
                {

                    Log.WriteLine("Invalid XML request!");
                    Log.WriteLine(HTTPRequest.Content.ToUTF8String());

                    _HTTPServer.GetEventSource(Semantics.DebugLog).
                        SubmitSubEvent("InvalidXMLRequest",
                                       new JObject(
                                           new JProperty("@context",      "http://wwcp.graphdefined.org/contexts/InvalidXMLRequest.jsonld"),
                                           new JProperty("Timestamp",     DateTime.Now.ToIso8601()),
                                           new JProperty("RemoteSocket",  HTTPRequest.RemoteSocket.ToString()),
                                           new JProperty("XMLRequest",    HTTPRequest.Content.ToUTF8String()) //ToDo: Handle errors!
                                       ).ToString().
                                         Replace(Environment.NewLine, ""));

                    return XMLRequest.Error;

                }

                #endregion

                #region Get SOAP request...

                IEnumerable<XElement> AuthorizeStartXMLs;
                IEnumerable<XElement> AuthorizeStopXMLs;

                try
                {

                    AuthorizeStartXMLs = XMLRequest.Data.Root.Descendants(OICPNS.Authorization + "eRoamingAuthorizeAuthorizeStart");
                    AuthorizeStopXMLs  = XMLRequest.Data.Root.Descendants(OICPNS.Authorization + "eRoamingAuthorizeAuthorizeStop");

                    if (!AuthorizeStartXMLs.Any() && !AuthorizeStopXMLs.Any())
                        throw new Exception("Must be either AuthorizeStart or AuthorizeStop XML request!");

                    if (AuthorizeStartXMLs.Count() > 1)
                        throw new Exception("Multiple AuthorizeStart XML tags within a single request are not supported!");

                    if (AuthorizeStopXMLs. Count() > 1)
                        throw new Exception("Multiple AuthorizeStop XML tags within a single request are not supported!");

                }
                catch (Exception e)
                {

                    Log.WriteLine("Invalid XML request!");

                    _HTTPServer.GetEventSource(Semantics.DebugLog).
                        SubmitSubEvent("InvalidXMLRequest",
                                       new JObject(
                                           new JProperty("@context",      "http://wwcp.graphdefined.org/contexts/InvalidXMLRequest.jsonld"),
                                           new JProperty("Timestamp",     DateTime.Now.ToIso8601()),
                                           new JProperty("RemoteSocket",  HTTPRequest.RemoteSocket.ToString()),
                                           new JProperty("Exception",     e.Message),
                                           new JProperty("XMLRequest",    XMLRequest.ToString())
                                       ).ToString().
                                         Replace(Environment.NewLine, ""));

                    return new HTTPResponseBuilder() {

                        HTTPStatusCode = HTTPStatusCode.OK,
                        ContentType    = HTTPContentType.XMLTEXT_UTF8,
                        Content        = SOAP.Encapsulation(new XElement(OICPNS.CommonTypes + "eRoamingAcknowledgement",

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

                #region Process an OICP AuthorizeStart HTTP/SOAP/XML call

                var AuthorizeStartXML = AuthorizeStartXMLs.FirstOrDefault();
                if (AuthorizeStartXML != null)
                {

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

                    #endregion

                    #region Parse request parameters

                    XElement            IdentificationXML;
                    XElement            QRCodeIdentificationXML;
                    XElement            PnCIdentificationXML;
                    XElement            RemoteIdentificationXML;
                    XElement            PartnerSessionIdXML;
                    XElement            ChargingProductIdXML;

                    ChargingSession_Id  SessionId           = null;
                    ChargingSession_Id  PartnerSessionId    = null;
                    EVSP_Id             ProviderId          = null;
                    EVSE_Id             EVSEId              = null;
                    eMA_Id              eMAId               = null;
                    ChargingProduct_Id  ChargingProductId   = null;

                    try
                    {

                        SessionId                = ChargingSession_Id.Parse(AuthorizeStartXML.ElementValueOrDefault(OICPNS.Authorization + "SessionID",        null));

                        PartnerSessionIdXML      = AuthorizeStartXML.Element(OICPNS.Authorization + "PartnerSessionID");
                        if (PartnerSessionIdXML != null)
                            PartnerSessionId = ChargingSession_Id.Parse(PartnerSessionIdXML.Value);

                        ProviderId               = EVSP_Id.           Parse(AuthorizeStartXML.ElementValueOrFail   (OICPNS.Authorization + "ProviderID", "No ProviderID XML tag provided!"));
                        EVSEId                   = EVSE_Id.           Parse(AuthorizeStartXML.ElementValueOrFail   (OICPNS.Authorization + "EVSEID",     "No EVSEID XML tag provided!"));

                        ChargingProductIdXML = AuthorizeStartXML.Element(OICPNS.Authorization + "PartnerProductID");
                        if (ChargingProductIdXML != null)
                            ChargingProductId = ChargingProduct_Id.Parse(ChargingProductIdXML.Value);

                        IdentificationXML        = AuthorizeStartXML.   ElementOrFail(OICPNS.Authorization + "Identification",       "No EVSEID XML tag provided!");
                        QRCodeIdentificationXML  = IdentificationXML.Element      (OICPNS.CommonTypes   + "QRCodeIdentification");
                        PnCIdentificationXML     = IdentificationXML.Element      (OICPNS.CommonTypes   + "PlugAndChargeIdentification");
                        RemoteIdentificationXML  = IdentificationXML.Element      (OICPNS.CommonTypes   + "RemoteIdentification");

                        if (QRCodeIdentificationXML == null &&
                            PnCIdentificationXML    == null &&
                            RemoteIdentificationXML == null)
                            throw new Exception("Neither a QRCodeIdentification, PlugAndChargeIdentification, nor a RemoteIdentification was provided!");

                        if      (QRCodeIdentificationXML != null)
                            eMAId = eMA_Id.Parse(QRCodeIdentificationXML.ElementValueOrFail(OICPNS.CommonTypes   + "EVCOID",    "No EVCOID XML tag provided!"));

                        else if (PnCIdentificationXML != null)
                            eMAId = eMA_Id.Parse(PnCIdentificationXML.   ElementValueOrFail(OICPNS.CommonTypes   + "EVCOID",    "No EVCOID XML tag provided!"));

                        else if (RemoteIdentificationXML != null)
                            eMAId = eMA_Id.Parse(RemoteIdentificationXML.ElementValueOrFail(OICPNS.CommonTypes   + "EVCOID",    "No EVCOID XML tag provided!"));

                    }
                    catch (Exception e)
                    {

                        Log.Timestamp("Invalid AuthorizeStartXML: " + e.Message);

                        return new HTTPResponseBuilder() {

                                HTTPStatusCode  = HTTPStatusCode.OK,
                                ContentType     = HTTPContentType.XMLTEXT_UTF8,
                                Content         = SOAP.Encapsulation(new XElement(OICPNS.CommonTypes + "eRoamingAcknowledgement",

                                                                         new XElement(OICPNS.CommonTypes + "Result", "false"),

                                                                         new XElement(OICPNS.CommonTypes + "StatusCode",
                                                                             new XElement(OICPNS.CommonTypes + "Code",           "022"),
                                                                             new XElement(OICPNS.CommonTypes + "Description",    "Request led to an exception!"),
                                                                             new XElement(OICPNS.CommonTypes + "AdditionalInfo",  e.Message)
                                                                         )

                                                                     )).ToString().ToUTF8Bytes()

                        };

                    }

                    #endregion

                    var HubjectCode            = "";
                    var HubjectDescription     = "";
                    var HubjectAdditionalInfo  = "";

                    #region Documentation

                    // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
                    //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">
                    //
                    //    <soapenv:Header/>
                    //
                    //    <soapenv:Body>
                    //       <CommonTypes:eRoamingAcknowledgement>
                    // 
                    //          <CommonTypes:Result>?</CommonTypes:Result>
                    // 
                    //          <CommonTypes:StatusCode>
                    // 
                    //             <CommonTypes:Code>?</CommonTypes:Code>
                    // 
                    //             <!--Optional:-->
                    //             <CommonTypes:Description>?</CommonTypes:Description>
                    // 
                    //             <!--Optional:-->
                    //             <CommonTypes:AdditionalInfo>?</CommonTypes:AdditionalInfo>
                    // 
                    //          </CommonTypes:StatusCode>
                    // 
                    //          <!--Optional:-->
                    //          <CommonTypes:SessionID>?</CommonTypes:SessionID>
                    // 
                    //          <!--Optional:-->
                    //          <CommonTypes:PartnerSessionID>?</CommonTypes:PartnerSessionID>
                    // 
                    //       </CommonTypes:eRoamingAcknowledgement>
                    //    </soapenv:Body>
                    //
                    // </soapenv:Envelope>

                    #endregion


                    EVSEOperator_Id     OperatorId        = null;
                    Auth_Token          AuthToken         = null;
                    ChargingProduct_Id  PartnerProductId  = null;
                    TimeSpan            QueryTimeout      = TimeSpan.FromMinutes(1);

                    #region Call async subscribers

                    var Response = AuthStartEVSEResultType.Error;

                    var OnAuthorizeStartLocal = OnAuthorizeStart;
                    if (OnAuthorizeStartLocal != null)
                    {

                        var CTS = new CancellationTokenSource();

                        var task = OnAuthorizeStartLocal(DateTime.Now,
                                                         this,
                                                         CTS.Token,
                                                         RoamingNetworkId,
                                                         OperatorId,
                                                         AuthToken,
                                                         EVSEId,
                                                         SessionId,
                                                         PartnerProductId,
                                                         PartnerSessionId,
                                                         QueryTimeout);

                        task.Wait();
                        Response = task.Result;

                    }

                    //Log.WriteLine("[" + DateTime.Now + "] CPOServer: AuthorizeStartResult: " + Response.ToString());

                    #endregion

                    #region Map result

                    switch (Response)
                    {

                        case AuthStartEVSEResultType.Authorized:
                            HubjectCode         = "000";
                            HubjectDescription  = "Ready to charge!";
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

                    #region Return SOAPResponse

                    return new HTTPResponseBuilder() {
                        HTTPStatusCode  = HTTPStatusCode.OK,
                        ContentType     = HTTPContentType.XMLTEXT_UTF8,
                        Content         = SOAP.Encapsulation(new XElement(OICPNS.CommonTypes + "eRoamingAcknowledgement",

                                                                 new XElement(OICPNS.CommonTypes + "Result", "true"),

                                                                 new XElement(OICPNS.CommonTypes + "StatusCode",
                                                                     new XElement(OICPNS.CommonTypes + "Code",            HubjectCode),
                                                                     new XElement(OICPNS.CommonTypes + "Description",     HubjectDescription),
                                                                     new XElement(OICPNS.CommonTypes + "AdditionalInfo",  HubjectAdditionalInfo)
                                                                 ),

                                                                 new XElement(OICPNS.CommonTypes + "SessionID", SessionId)
                                                                 //new XElement(NS.OICPv1_2CommonTypes + "PartnerSessionID", SessionID),

                                                            )).ToString().
                                                               ToUTF8Bytes()
                    };

                    #endregion

                }

                #endregion

                #region Process an OICP AuthorizeStop HTTP/SOAP/XML call

                var AuthorizeStopXML  = AuthorizeStopXMLs.FirstOrDefault();
                if (AuthorizeStopXML != null)
                {

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

                    #region Parse request parameters

                    XElement            PartnerSessionIdXML;

                    ChargingSession_Id  SessionId;
                    ChargingSession_Id  PartnerSessionId        = null;
                    EVSP_Id             ProviderId;
                    EVSE_Id             EVSEId;

                    try
                    {

                        SessionId         = ChargingSession_Id.Parse(AuthorizeStopXML.ElementValueOrFail   (OICPNS.Authorization + "SessionID",        "No SessionID XML tag provided!"));

                        PartnerSessionIdXML = AuthorizeStopXML.Element(OICPNS.Authorization + "PartnerSessionID");
                        if (PartnerSessionIdXML != null)
                            PartnerSessionId = ChargingSession_Id.Parse(PartnerSessionIdXML.Value);

                        ProviderId        = EVSP_Id.           Parse(AuthorizeStopXML.ElementValueOrFail   (OICPNS.Authorization + "ProviderID",       "No ProviderID XML tag provided!"));
                        EVSEId            = EVSE_Id.           Parse(AuthorizeStopXML.ElementValueOrFail   (OICPNS.Authorization + "EVSEID",           "No EVSEID XML tag provided!"));

                    }
                    catch (Exception e)
                    {

                        Log.Timestamp("Invalid AuthorizeStopXML: " + e.Message);

                        return new HTTPResponseBuilder() {

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

                    var HubjectCode            = "";
                    var HubjectDescription     = "";
                    var HubjectAdditionalInfo  = "";

                    #region Documentation

                    // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
                    //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">
                    //
                    //    <soapenv:Header/>
                    //
                    //    <soapenv:Body>
                    //       <CommonTypes:eRoamingAcknowledgement>
                    // 
                    //          <CommonTypes:Result>?</CommonTypes:Result>
                    // 
                    //          <CommonTypes:StatusCode>
                    // 
                    //             <CommonTypes:Code>?</CommonTypes:Code>
                    // 
                    //             <!--Optional:-->
                    //             <CommonTypes:Description>?</CommonTypes:Description>
                    // 
                    //             <!--Optional:-->
                    //             <CommonTypes:AdditionalInfo>?</CommonTypes:AdditionalInfo>
                    // 
                    //          </CommonTypes:StatusCode>
                    // 
                    //          <!--Optional:-->
                    //          <CommonTypes:SessionID>?</CommonTypes:SessionID>
                    // 
                    //          <!--Optional:-->
                    //          <CommonTypes:PartnerSessionID>?</CommonTypes:PartnerSessionID>
                    // 
                    //       </CommonTypes:eRoamingAcknowledgement>
                    //    </soapenv:Body>
                    //
                    // </soapenv:Envelope>

                    #endregion

                    #region Call async subscribers

                    var Response = AuthStopEVSEResultType.Error;

                    var OnAuthorizeStopLocal = OnAuthorizeStop;
                    if (OnAuthorizeStopLocal != null)
                    {

                        var CTS = new CancellationTokenSource();

                        var task = OnAuthorizeStopLocal(DateTime.Now,
                                                        this,
                                                        CTS.Token,
                                                        RoamingNetworkId,
                                                        SessionId,
                                                        PartnerSessionId,
                                                        ProviderId,
                                                        EVSEId);

                        task.Wait();
                        Response = task.Result;

                    }

                    //Log.WriteLine("[" + DateTime.Now + "] CPOServer: AuthorizeStartResult: " + Response.ToString());

                    #endregion

                    #region Map result

                    switch (Response)
                    {

                        case AuthStopEVSEResultType.Success:
                            HubjectCode         = "000";
                            HubjectDescription  = "Ready to stop charging!";
                            break;

                        case AuthStopEVSEResultType.SessionIsInvalid:
                            HubjectCode         = "400";
                            HubjectDescription  = "Session is invalid";
                            break;

                        case AuthStopEVSEResultType.NotReachable:
                            HubjectCode         = "501";
                            HubjectDescription  = "Communication to EVSE failed!";
                            break;

                        case AuthStopEVSEResultType.Timeout:
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

                    var SOAPContent = SOAP.Encapsulation(new XElement(OICPNS.CommonTypes + "eRoamingAcknowledgement",

                                                             new XElement(OICPNS.CommonTypes + "Result", "true"),

                                                             new XElement(OICPNS.CommonTypes + "StatusCode",
                                                                 new XElement(OICPNS.CommonTypes + "Code",            HubjectCode),
                                                                 new XElement(OICPNS.CommonTypes + "Description",     HubjectDescription),
                                                                 new XElement(OICPNS.CommonTypes + "AdditionalInfo",  HubjectAdditionalInfo)
                                                             ),

                                                             new XElement(OICPNS.CommonTypes + "SessionID", SessionId)

                                                         )).ToString();

                    return new HTTPResponseBuilder() {
                        HTTPStatusCode  = HTTPStatusCode.OK,
                        ContentType     = HTTPContentType.XMLTEXT_UTF8,
                        Content         = SOAPContent.ToUTF8Bytes()
                    };

                    #endregion

                }

                #endregion


                #region GetEVSEByIdRequest(EVSEId, QueryTimeout = null)

                /// <summary>
                /// Create a new task requesting the static EVSE data
                /// for the given EVSE identification.
                /// </summary>
                /// <param name="EVSEId">The unique identification of the EVSE.</param>
                /// <param name="QueryTimeout">An optional timeout for this query.</param>
                //public Task<HTTPResponse<EVSEDataRecord>>

                //    GetEVSEByIdRequest(EVSE_Id    EVSEId,
                //                       TimeSpan?  QueryTimeout  = null)

                //{

                //    try
                //    {

                //        using (var _OICPClient = new SOAPClient(Hostname,
                //                                                TCPPort,
                //                                                HTTPVirtualHost,
                //                                                "/ibis/ws/eRoamingEvseData_V2.0",
                //                                                UserAgent,
                //                                                DNSClient))
                //        {

                //            return _OICPClient.Query(EMP_XMLMethods.GetEVSEByIdRequestXML(EVSEId),
                //                                     "eRoamingEvseById",
                //                                     QueryTimeout: QueryTimeout != null ? QueryTimeout.Value : this.QueryTimeout,

                //                                     OnSuccess: XMLData =>

                //                                         #region Documentation

                //                                         // <soapenv:Envelope xmlns:soapenv     = "http://schemas.xmlsoap.org/soap/envelope/"
                //                                         //                   xmlns:EVSEData    = "http://www.hubject.com/b2b/services/evsedata/v2.0"
                //                                         //                   xmlns:CommonTypes = "http://www.hubject.com/b2b/services/commontypes/v2.0">
                //                                         //   <soapenv:Header/>
                //                                         //   <soapenv:Body>
                //                                         //      <EVSEData:eRoamingEvseDataRecord deltaType="update|insert|delete" lastUpdate="?">
                //                                         //          [...]
                //                                         //      </EVSEData:eRoamingEvseDataRecord>
                //                                         //    </soapenv:Body>
                //                                         // </soapenv:Envelope>

                //                                         #endregion

                //                                         new HTTPResponse<EVSEDataRecord>(XMLData.HttpResponse,
                //                                                                          XMLMethods.ParseEVSEDataRecordXML(XMLData.Content)),

                //                                     OnSOAPFault: Fault =>
                //                                         new HTTPResponse<EVSEDataRecord>(
                //                                             Fault.HttpResponse,
                //                                             new Exception(Fault.Content.ToString())),

                //                                     OnHTTPError: (t, s, e) => SendOnHTTPError(t, s, e),

                //                                     OnException: (t, s, e) => SendOnException(t, s, e)

                //                                    );

                //        }

                //    }

                //    catch (Exception e)
                //    {

                //        SendOnException(DateTime.Now, this, e);

                //        return new Task<HTTPResponse<EVSEDataRecord>>(
                //            () => new HTTPResponse<EVSEDataRecord>(e));

                //    }

                //}

                #endregion


                return new HTTPResponseBuilder() {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = "Error!".ToUTF8Bytes()
                };

            };

            #endregion

            #region Register SOAP-XML Request via GET

            _HTTPServer.AddMethodCallback(HTTPMethod.GET,
                                          URIPrefix + "/RNs/{RoamingNetworkId}/AuthorizeStartStop",
                                          HTTPContentType.XMLTEXT_UTF8,
                                          HTTPDelegate: AuthorizeStartStopDelegate);

            #endregion

            #region Register SOAP-XML Request via POST

            _HTTPServer.AddMethodCallback(HTTPMethod.POST,
                                          URIPrefix + "/RNs/{RoamingNetwork}/AuthorizeStartStop",
                                          HTTPContentType.XMLTEXT_UTF8,
                                          HTTPDelegate: AuthorizeStartStopDelegate);

            #endregion

            #region Register HTML+Plaintext ErrorResponse

            // HTML
            _HTTPServer.AddMethodCallback(HTTPMethod.GET,
                                          URIPrefix + "/RNs/{RoamingNetwork}/AuthorizeStartStop",
                                          HTTPContentType.HTML_UTF8,
                                          HTTPDelegate: HTTPRequest => {

                                              var RoamingNetworkId = HTTPRequest.ParsedURIParameters[0];

                                              return new HTTPResponseBuilder() {
                                                  HTTPStatusCode  = HTTPStatusCode.BadGateway,
                                                  ContentType     = HTTPContentType.HTML_UTF8,
                                                  Content         = ("/RNs/" + RoamingNetworkId + "/AuthorizeStartStop is a HTTP/SOAP/XML endpoint!").ToUTF8Bytes(),
                                                  Connection      = "close"
                                              };

                                          });

            // Text
            _HTTPServer.AddMethodCallback(HTTPMethod.GET,
                                          "/RNs/{RoamingNetwork}/AuthorizeStartStop",
                                          HTTPContentType.TEXT_UTF8,
                                          HTTPDelegate: HTTPRequest => {

                                              var RoamingNetworkId = HTTPRequest.ParsedURIParameters[0];

                                              return new HTTPResponseBuilder() {
                                                  HTTPStatusCode  = HTTPStatusCode.BadGateway,
                                                  ContentType     = HTTPContentType.HTML_UTF8,
                                                  Content         = ("/RNs/" + RoamingNetworkId + "/AuthorizeStartStop is a HTTP/SOAP/XML endpoint!").ToUTF8Bytes(),
                                                  Connection      = "close"
                                              };

                                          });

            #endregion

            #endregion

        }

        #endregion


        #region (internal) SendAuthorizeStart(...)

        internal async Task<AuthStartEVSEResultType> SendAuthorizeStart(DateTime            Timestamp,
                                                                         CancellationToken   CancellationToken,
                                                                         RoamingNetwork_Id   RoamingNetworkId,
                                                                         EVSEOperator_Id     OperatorId,
                                                                         Auth_Token          AuthToken,
                                                                         EVSE_Id             EVSEId            = null,
                                                                         ChargingSession_Id  SessionId         = null,
                                                                         ChargingProduct_Id  PartnerProductId  = null,
                                                                         ChargingSession_Id  PartnerSessionId  = null,
                                                                         TimeSpan?           QueryTimeout      = null)
        {

            var OnAuthorizeStartLocal = OnAuthorizeStart;
            if (OnAuthorizeStartLocal == null)
                return AuthStartEVSEResultType.Error;

            var results = await Task.WhenAll(OnAuthorizeStartLocal.
                                                 GetInvocationList().
                                                 Select(subscriber => (subscriber as OnAuthorizeStartDelegate)
                                                     (Timestamp,
                                                      this,
                                                      CancellationToken,
                                                      RoamingNetworkId,
                                                      OperatorId,
                                                      AuthToken,
                                                      EVSEId            = null,
                                                      SessionId         = null,
                                                      PartnerProductId  = null,
                                                      PartnerSessionId  = null,
                                                      QueryTimeout      = null)));

            return results.
                       Where(result => result != AuthStartEVSEResultType.Unspecified).
                       First();

        }

        #endregion

        #region (internal) SendAuthorizeStop(...)

        internal async Task<AuthStopEVSEResultType> SendAuthorizeStop(DateTime            Timestamp,
                                                                       CancellationToken   CancellationToken,
                                                                       RoamingNetwork_Id   RoamingNetworkId,
                                                                       ChargingSession_Id  SessionId,
                                                                       ChargingSession_Id  PartnerSessionId,
                                                                       EVSP_Id             ProviderId,
                                                                       EVSE_Id             EVSEId)
        {

            var OnAuthorizeStopLocal = OnAuthorizeStop;
            if (OnAuthorizeStopLocal == null)
                return AuthStopEVSEResultType.Error;

            var results = await Task.WhenAll(OnAuthorizeStopLocal.
                                                 GetInvocationList().
                                                 Select(subscriber => (subscriber as OnAuthorizeStopDelegate)
                                                     (Timestamp,
                                                      this,
                                                      CancellationToken,
                                                      RoamingNetworkId,
                                                      SessionId,
                                                      PartnerSessionId,
                                                      ProviderId,
                                                      EVSEId)));

            return results.
                       Where(result => result != AuthStopEVSEResultType.Unspecified).
                       First();

        }

        #endregion


        #region Start()

        public void Start()
        {
            _HTTPServer.Start();
        }

        #endregion

        #region Shutdown(Message = null, Wait = true)

        public void Shutdown(String Message = null, Boolean Wait = true)
        {
            _HTTPServer.Shutdown(Message, Wait);
        }

        #endregion

    }

}
