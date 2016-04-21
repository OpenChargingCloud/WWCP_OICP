/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
{

    /// <summary>
    /// A OICP v2.0 CPO HTTP/SOAP/XML Server API.
    /// </summary>
    public class CPOServer
    {

        #region Data

        /// <summary>
        /// The default HTTP/SOAP/XML server name.
        /// </summary>
        public const           String    DefaultHTTPServerName  = "GraphDefined OICP v2.0 HTTP/SOAP/XML CPO Server API";

        /// <summary>
        /// The default HTTP/SOAP/XML server TCP port.
        /// </summary>
        public static readonly IPPort    DefaultHTTPServerPort  = new IPPort(2002);

        /// <summary>
        /// The default query timeout.
        /// </summary>
        public static readonly TimeSpan  DefaultQueryTimeout    = TimeSpan.FromMinutes(1);

        #endregion

        #region Properties

        #region SOAPServer

        private readonly SOAPServer _SOAPServer;

        /// <summary>
        /// The HTTP/SOAP server.
        /// </summary>
        public SOAPServer SOAPServer
        {
            get
            {
                return _SOAPServer;
            }
        }

        #endregion

        #region URIPrefix

        private readonly String _URIPrefix;

        /// <summary>
        /// The common URI prefix for this HTTP/SOAP service.
        /// </summary>
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

        /// <summary>
        /// The DNS client used by this server.
        /// </summary>
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

        #region OnRemoteStart

        /// <summary>
        /// An event sent whenever a remote start command was received.
        /// </summary>
        public event RequestLogHandler      OnLogRemoteStart;

        /// <summary>
        /// An event sent whenever a remote start response was sent.
        /// </summary>
        public event AccessLogHandler       OnLogRemoteStarted;

        /// <summary>
        /// An event sent whenever a remote start command was received.
        /// </summary>
        public event OnRemoteStartDelegate  OnRemoteStart;

        #endregion

        #region OnRemoteStop

        /// <summary>
        /// An event sent whenever a remote stop command was received.
        /// </summary>
        public event RequestLogHandler     OnLogRemoteStop;

        /// <summary>
        /// An event sent whenever a remote stop response was sent.
        /// </summary>
        public event AccessLogHandler      OnLogRemoteStopped;

        /// <summary>
        /// An event sent whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopDelegate  OnRemoteStop;

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
                _SOAPServer.RequestLog += value;
            }

            remove
            {
                _SOAPServer.RequestLog -= value;
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
                _SOAPServer.AccessLog += value;
            }

            remove
            {
                _SOAPServer.AccessLog -= value;
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
                _SOAPServer.ErrorLog += value;
            }

            remove
            {
                _SOAPServer.ErrorLog -= value;
            }

        }

        #endregion

        #endregion

        #region Constructor(s)

        #region CPOServer(HTTPServerName, TCPPort = null, URIPrefix = "", DNSClient = null, AutoStart = false)

        /// <summary>
        /// Initialize an new HTTP server for the OICP HTTP/SOAP/XML CPO Server API using IPAddress.Any.
        /// </summary>
        /// <param name="HTTPServerName">An optional identification string for the HTTP server.</param>
        /// <param name="TCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="AutoStart">Whether to start the server immediately or not.</param>
        public CPOServer(String    HTTPServerName  = DefaultHTTPServerName,
                         IPPort    TCPPort         = null,
                         String    URIPrefix       = "",
                         DNSClient DNSClient       = null,
                         Boolean   AutoStart       = false)

            : this(new SOAPServer(TCPPort != null ? TCPPort : DefaultHTTPServerPort,
                                  DefaultServerName:  HTTPServerName,
                                  DNSClient:          DNSClient,
                                  Autostart:          AutoStart),
                   URIPrefix)

        {

            if (AutoStart)
                Start();

        }

        #endregion

        #region CPOServer(SOAPServer, URIPrefix = "")

        /// <summary>
        /// Use the given HTTP server for the OICP HTTP/SOAP/XML CPO Server API using IPAddress.Any.
        /// </summary>
        /// <param name="SOAPServer">A SOAP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        public CPOServer(SOAPServer  SOAPServer,
                         String      URIPrefix  = "")
        {

            #region Initial checks

            if (SOAPServer == null)
                throw new ArgumentNullException(nameof(SOAPServer),  "The given SOAP server must not be null!");

            if (URIPrefix == null)
                URIPrefix = "";

            if (URIPrefix.Length > 0 && !URIPrefix.StartsWith("/"))
                URIPrefix = "/" + URIPrefix;

            #endregion

            this._SOAPServer  = SOAPServer;
            this._URIPrefix   = URIPrefix;
            this._DNSClient   = SOAPServer.DNSClient;

            RegisterURITemplates();

        }

        #endregion

        #endregion


        #region (private) RegisterURITemplates()

        private void RegisterURITemplates()
        {

            #region / (HTTPRoot)

            _SOAPServer.AddMethodCallback(HTTPMethod.GET,
                                          new String[] { "/", URIPrefix + "/" },
                                          HTTPContentType.TEXT_UTF8,
                                          HTTPDelegate: Request => {

                                              return new HTTPResponseBuilder(Request) {

                                                  HTTPStatusCode  = HTTPStatusCode.BadGateway,
                                                  ContentType     = HTTPContentType.TEXT_UTF8,
                                                  Content         = ("Welcome at " + DefaultHTTPServerName + Environment.NewLine +
                                                                     "This is a HTTP/SOAP/XML endpoint!" + Environment.NewLine + Environment.NewLine +
                                                                     "Defined endpoints: " + Environment.NewLine + Environment.NewLine +
                                                                     _SOAPServer.
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

            #region /Authorization - AuthorizeRemoteStart

            _SOAPServer.RegisterSOAPDelegate(URIPrefix + "/Authorization",
                                             "AuthorizeRemoteStart",
                                             XML => XML.Descendants(OICPNS.Authorization + "eRoamingAuthorizeRemoteStart").FirstOrDefault(),
                                             (Request, RemoteStartXML) => {


                    #region Documentation

                    // <soapenv:Envelope xmlns:soapenv       = "http://schemas.xmlsoap.org/soap/envelope/"
                    //                   xmlns:Authorization = "http://www.hubject.com/b2b/services/authorization/v2.0"
                    //                   xmlns:CommonTypes   = "http://www.hubject.com/b2b/services/commontypes/v2.0">
                    //
                    //    <soapenv:Header/>
                    //
                    //    <soapenv:Body>
                    //       <Authorization:eRoamingAuthorizeRemoteStart>
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
                    //       </Authorization:eRoamingAuthorizeRemoteStart>
                    //    </soapenv:Body>
                    //
                    // </soapenv:Envelope>

                    #endregion

                    #region Send OnLogChargeDetailRecordSend event

                    try
                    {

                        var OnLogRemoteStartLocal = OnLogRemoteStart;
                        if (OnLogRemoteStartLocal != null)
                            OnLogRemoteStartLocal(DateTime.Now, this.SOAPServer, Request);

                    }
                    catch (Exception e)
                    {
                        e.Log("CPOServer.OnLogRemoteStart");
                    }

                    #endregion


                    #region Parse request parameters

                    XElement IdentificationXML;
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

                        SessionId                = ChargingSession_Id.Parse(RemoteStartXML.ElementValueOrDefault(OICPNS.Authorization + "SessionID",        null));

                        PartnerSessionIdXML      = RemoteStartXML.Element(OICPNS.Authorization + "PartnerSessionID");
                        if (PartnerSessionIdXML != null)
                            PartnerSessionId = ChargingSession_Id.Parse(PartnerSessionIdXML.Value);

                        ProviderId               = EVSP_Id.           Parse(RemoteStartXML.ElementValueOrFail   (OICPNS.Authorization + "ProviderID", "No ProviderID XML tag provided!"));
                        EVSEId                   = EVSE_Id.           Parse(RemoteStartXML.ElementValueOrFail   (OICPNS.Authorization + "EVSEID",     "No EVSEID XML tag provided!"));

                        ChargingProductIdXML = RemoteStartXML.Element(OICPNS.Authorization + "PartnerProductID");
                        if (ChargingProductIdXML != null)
                            ChargingProductId = ChargingProduct_Id.Parse(ChargingProductIdXML.Value);

                        IdentificationXML        = RemoteStartXML.   ElementOrFail(OICPNS.Authorization + "Identification",       "No EVSEID XML tag provided!");
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

                        //Log.Timestamp("Invalid RemoteStartXML: " + e.Message);

                        return new HTTPResponseBuilder(Request) {

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

                    RemoteStartEVSEResult response = null;

                    var OnRemoteStartLocal = OnRemoteStart;
                    if (OnRemoteStartLocal != null)
                    {

                        var CTS = new CancellationTokenSource();

                        var task = OnRemoteStartLocal(DateTime.Now,
                                                      this,
                                                      Request.CancellationToken,
                                                      Request.EventTrackingId,
                                                      EVSEId,
                                                      ChargingProductId,
                                                      SessionId,
                                                      PartnerSessionId,
                                                      ProviderId,
                                                      eMAId,
                                                      DefaultQueryTimeout);

                        task.Wait();
                        response = task.Result;

                    }

                    #endregion

                    #region Map result

                    var HubjectCode            = "320";
                    var HubjectDescription     = "Service not available!";
                    var HubjectAdditionalInfo  = "";

                    if (response != null)
                        switch (response.Result)
                        {

                            case RemoteStartEVSEResultType.Success:
                                HubjectCode         = "000";
                                HubjectDescription  = "Ready to charge!";
                                break;

                            case RemoteStartEVSEResultType.InvalidSessionId:
                                HubjectCode         = "400";
                                HubjectDescription  = "Session is invalid";
                                break;

                            case RemoteStartEVSEResultType.Offline:
                                HubjectCode         = "501";
                                HubjectDescription  = "Communication to EVSE failed!";
                                break;

                            case RemoteStartEVSEResultType.Timeout:
                                HubjectCode         = "510";
                                HubjectDescription  = "No EV connected to EVSE!";
                                break;

                            case RemoteStartEVSEResultType.Reserved:
                                HubjectCode         = "601";
                                HubjectDescription  = "EVSE reserved!";
                                break;

                            case RemoteStartEVSEResultType.AlreadyInUse:
                                HubjectCode         = "602";
                                HubjectDescription  = "EVSE is already in use!";
                                break;

                            case RemoteStartEVSEResultType.UnknownEVSE:
                                HubjectCode         = "603";
                                HubjectDescription  = "Unknown EVSE ID!";
                                break;

                            case RemoteStartEVSEResultType.OutOfService:
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

                                                  new XElement(OICPNS.CommonTypes + "Result", response != null && response.Result == RemoteStartEVSEResultType.Success ? "true" : "false"),

                                                  new XElement(OICPNS.CommonTypes + "StatusCode",
                                                      new XElement(OICPNS.CommonTypes + "Code",            HubjectCode),
                                                      new XElement(OICPNS.CommonTypes + "Description",     HubjectDescription),
                                                      new XElement(OICPNS.CommonTypes + "AdditionalInfo",  HubjectAdditionalInfo)
                                                  ),

                                                  new XElement(OICPNS.CommonTypes + "SessionID", SessionId)
                                                  //new XElement(NS.OICPv1_2CommonTypes + "PartnerSessionID", SessionID),

                                             )).ToUTF8Bytes()
                    };


                    #endregion


                    #region Send OnLogRemoteStarted event

                    try
                    {

                        var OnLogRemoteStartedLocal = OnLogRemoteStarted;
                        if (OnLogRemoteStartedLocal != null)
                            OnLogRemoteStartedLocal(HTTPResponse.Timestamp, this.SOAPServer, Request, HTTPResponse);

                    }
                    catch (Exception e)
                    {
                        e.Log("CPOServer.OnLogRemoteStarted");
                    }

                    #endregion

                    return HTTPResponse;

            });

            #endregion

            #region /Authorization - AuthorizeRemoteStop

            _SOAPServer.RegisterSOAPDelegate(URIPrefix + "/Authorization",
                                             "AuthorizeRemoteStop",
                                             XML => XML.Descendants(OICPNS.Authorization + "eRoamingAuthorizeRemoteStop").FirstOrDefault(),
                                             (Request, RemoteStopXML) => {

                    #region Documentation

                    // <soapenv:Envelope xmlns:soapenv       = "http://schemas.xmlsoap.org/soap/envelope/"
                    //                   xmlns:Authorization = "http://www.hubject.com/b2b/services/authorization/v2.0">
                    //
                    //    <soapenv:Header/>
                    //
                    //    <soapenv:Body>
                    //       <Authorization:eRoamingAuthorizeRemoteStop>
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
                    //       </Authorization:eRoamingAuthorizeRemoteStop>
                    //    </soapenv:Body>
                    //
                    // </soapenv:Envelope>

                    #endregion

                    #region Send OnLogChargeDetailRecordSend event

                    try
                    {

                        var OnLogRemoteStopLocal = OnLogRemoteStop;
                        if (OnLogRemoteStopLocal != null)
                            OnLogRemoteStopLocal(DateTime.Now, this.SOAPServer, Request);

                    }
                    catch (Exception e)
                    {
                        e.Log("CPOServer.OnLogRemoteStop");
                    }

                    #endregion


                    #region Parse request parameters

                    XElement PartnerSessionIdXML;

                    ChargingSession_Id  SessionId         = null;
                    ChargingSession_Id  PartnerSessionId  = null;
                    EVSP_Id             ProviderId        = null;
                    EVSE_Id             EVSEId            = null;

                    try
                    {

                        SessionId         = ChargingSession_Id.Parse(RemoteStopXML.ElementValueOrFail(OICPNS.Authorization + "SessionID",  "No SessionID XML tag provided!"));

                        PartnerSessionIdXML = RemoteStopXML.Element(OICPNS.Authorization + "PartnerSessionID");
                        if (PartnerSessionIdXML != null)
                            PartnerSessionId = ChargingSession_Id.Parse(PartnerSessionIdXML.Value);

                        ProviderId        = EVSP_Id.           Parse(RemoteStopXML.ElementValueOrFail(OICPNS.Authorization + "ProviderID", "No ProviderID XML tag provided!"));
                        EVSEId            = EVSE_Id.           Parse(RemoteStopXML.ElementValueOrFail(OICPNS.Authorization + "EVSEID",     "No EVSEID XML tag provided!"));

                    }
                    catch (Exception e)
                    {

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

                    RemoteStopEVSEResult response = null;

                    var OnRemoteStopLocal = OnRemoteStop;
                    if (OnRemoteStopLocal != null)
                    {

                        var CTS = new CancellationTokenSource();

                        var task = OnRemoteStopLocal(DateTime.Now,
                                                     this,
                                                     Request.CancellationToken,
                                                     Request.EventTrackingId,
                                                     EVSEId,
                                                     SessionId,
                                                     PartnerSessionId,
                                                     ProviderId,
                                                     DefaultQueryTimeout);

                        task.Wait();
                        response = task.Result;

                    }

                    #endregion

                    #region Map result

                    var HubjectCode            = "320";
                    var HubjectDescription     = "Service not available!";
                    var HubjectAdditionalInfo  = "";

                    if (response != null)
                        switch (response.Result)
                        {

                            case RemoteStopEVSEResultType.Success:
                                HubjectCode         = "000";
                                HubjectDescription  = "Ready to stop charging!";
                                break;

                            case RemoteStopEVSEResultType.InvalidSessionId:
                                HubjectCode         = "400";
                                HubjectDescription  = "Session is invalid";
                                break;

                            case RemoteStopEVSEResultType.Offline:
                                HubjectCode         = "501";
                                HubjectDescription  = "Communication to EVSE failed!";
                                break;

                            case RemoteStopEVSEResultType.Timeout:
                                HubjectCode         = "510";
                                HubjectDescription  = "No EV connected to EVSE!";
                                break;

                            case RemoteStopEVSEResultType.UnknownEVSE:
                                HubjectCode         = "603";
                                HubjectDescription  = "Unknown EVSE ID!";
                                break;

                            case RemoteStopEVSEResultType.OutOfService:
                                HubjectCode         = "700";
                                HubjectDescription  = "EVSE out of service!";
                                break;


                            default:
                                HubjectCode         = "320";
                                HubjectDescription  = "Service not available!";
                                break;

                        }

                    #endregion

                    #region Create SOAP response

                    var HTTPResponse = new HTTPResponseBuilder(Request) {
                        HTTPStatusCode  = HTTPStatusCode.OK,
                        Server          = SOAPServer.DefaultServerName,
                        Date            = DateTime.Now,
                        ContentType     = HTTPContentType.XMLTEXT_UTF8,
                        Content         = SOAP.Encapsulation(
                                              new XElement(OICPNS.CommonTypes + "eRoamingAcknowledgement",

                                                  new XElement(OICPNS.CommonTypes + "Result", response != null && response.Result == RemoteStopEVSEResultType.Success ? "true" : "false"),

                                                  new XElement(OICPNS.CommonTypes + "StatusCode",
                                                      new XElement(OICPNS.CommonTypes + "Code",           HubjectCode),
                                                      new XElement(OICPNS.CommonTypes + "Description",    HubjectDescription),
                                                      new XElement(OICPNS.CommonTypes + "AdditionalInfo", HubjectAdditionalInfo)
                                                  ),

                                                  new XElement(OICPNS.CommonTypes + "SessionID", SessionId)

                                              )).ToUTF8Bytes()
                    };

                    #endregion


                    #region Send OnLogRemoteStopped event

                    try
                    {

                        var OnLogRemoteStoppedLocal = OnLogRemoteStopped;
                        if (OnLogRemoteStoppedLocal != null)
                            OnLogRemoteStoppedLocal(HTTPResponse.Timestamp, this.SOAPServer, Request, HTTPResponse);

                    }
                    catch (Exception e)
                    {
                        e.Log("CPOServer.OnLogRemoteStopped");
                    }

                    #endregion

                    return HTTPResponse;

            });

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

        }

        #endregion


        #region (internal) SendRemoteStart(...)

        internal async Task<RemoteStartEVSEResult> SendRemoteStart(DateTime            Timestamp,
                                                                   CPOServer           Sender,
                                                                   CancellationToken   CancellationToken,
                                                                   EventTracking_Id    EventTrackingId,
                                                                   EVSE_Id             EVSEId,
                                                                   ChargingProduct_Id  ChargingProductId,
                                                                   ChargingSession_Id  SessionId,
                                                                   ChargingSession_Id  PartnerSessionId,
                                                                   EVSP_Id             ProviderId,
                                                                   eMA_Id              eMAId,
                                                                   TimeSpan?           QueryTimeout  = null)
        {

            var OnRemoteStartLocal = OnRemoteStart;
            if (OnRemoteStartLocal == null)
                return RemoteStartEVSEResult.Error();

            var results = await Task.WhenAll(OnRemoteStartLocal.
                                                 GetInvocationList().
                                                 Select(subscriber => (subscriber as OnRemoteStartDelegate)
                                                     (Timestamp,
                                                      this,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      EVSEId,
                                                      ChargingProductId,
                                                      SessionId,
                                                      PartnerSessionId,
                                                      ProviderId,
                                                      eMAId,
                                                      QueryTimeout)));

            return results.
                       Where(result => result.Result != RemoteStartEVSEResultType.Unspecified).
                       First();

        }

        #endregion

        #region (internal) SendRemoteStop(...)

        internal async Task<RemoteStopEVSEResult> SendRemoteStop(DateTime            Timestamp,
                                                                 CPOServer           Sender,
                                                                 CancellationToken   CancellationToken,
                                                                 EventTracking_Id    EventTrackingId,
                                                                 EVSE_Id             EVSEId,
                                                                 ChargingSession_Id  SessionId,
                                                                 ChargingSession_Id  PartnerSessionId,
                                                                 EVSP_Id             ProviderId,
                                                                 TimeSpan?           QueryTimeout  = null)
        {

            var OnRemoteStopLocal = OnRemoteStop;
            if (OnRemoteStopLocal == null)
                return RemoteStopEVSEResult.Error(SessionId);

            var results = await Task.WhenAll(OnRemoteStopLocal.
                                                 GetInvocationList().
                                                 Select(subscriber => (subscriber as OnRemoteStopDelegate)
                                                     (Timestamp,
                                                      this,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      EVSEId,
                                                      SessionId,
                                                      PartnerSessionId,
                                                      ProviderId,
                                                      QueryTimeout)));

            return results.
                       Where(result => result.Result != RemoteStopEVSEResultType.Unspecified).
                       First();

        }

        #endregion


        #region GetEVSEByIdRequest(EVSEId, QueryTimeout = null) // <- Note!

        ///// <summary>
        ///// Create a new task requesting the static EVSE data
        ///// for the given EVSE identification.
        ///// </summary>
        ///// <param name="EVSEId">The unique identification of the EVSE.</param>
        ///// <param name="QueryTimeout">An optional timeout for this query.</param>
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
        //                                         //      <EVSEData:eRoamingEvseDataRecord deltaType="?" lastUpdate="?">
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


        #region Start()

        public void Start()
        {
            _SOAPServer.Start();
        }

        #endregion

        #region Shutdown(Message = null, Wait = true)

        public void Shutdown(String Message = null, Boolean Wait = true)
        {
            _SOAPServer.Shutdown(Message, Wait);
        }

        #endregion

    }

}
