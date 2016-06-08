﻿/*
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

namespace org.GraphDefined.WWCP.OICPv2_1
{

    /// <summary>
    /// An OICP v2.1 CPO HTTP/SOAP/XML Server API.
    /// </summary>
    public class CPOServer : ASOAPServer
    {

        #region Data

        /// <summary>
        /// The default HTTP/SOAP/XML server name.
        /// </summary>
        public new const           String    DefaultHTTPServerName  = "GraphDefined OICP v2.1 HTTP/SOAP/XML CPO Server API";

        /// <summary>
        /// The default HTTP/SOAP/XML server TCP port.
        /// </summary>
        public new static readonly IPPort    DefaultHTTPServerPort  = new IPPort(2002);

        /// <summary>
        /// The default query timeout.
        /// </summary>
        public new static readonly TimeSpan  DefaultQueryTimeout    = TimeSpan.FromMinutes(1);

        #endregion

        #region Events

        #region OnRemoteReservationStart

        /// <summary>
        /// An event sent whenever a remote reservation start command was received.
        /// </summary>
        public event RequestLogHandler                 OnLogRemoteReservationStart;

        /// <summary>
        /// An event sent whenever a remote reservation start response was sent.
        /// </summary>
        public event AccessLogHandler                  OnLogRemoteReservationStarted;

        /// <summary>
        /// An event sent whenever a remote reservation start command was received.
        /// </summary>
        public event OnRemoteReservationStartDelegate  OnRemoteReservationStart;

        #endregion

        #region OnRemoteReservationStop

        /// <summary>
        /// An event sent whenever a remote reservation stop command was received.
        /// </summary>
        public event RequestLogHandler                OnLogRemoteReservationStop;

        /// <summary>
        /// An event sent whenever a remote reservation stop response was sent.
        /// </summary>
        public event AccessLogHandler                 OnLogRemoteReservationStopped;

        /// <summary>
        /// An event sent whenever a remote reservation stop command was received.
        /// </summary>
        public event OnRemoteReservationStopDelegate  OnRemoteReservationStop;

        #endregion

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

            : base(HTTPServerName.IsNotNullOrEmpty() ? HTTPServerName : DefaultHTTPServerName,
                   TCPPort != null                   ? TCPPort        : DefaultHTTPServerPort,
                   URIPrefix,
                   HTTPContentType.XMLTEXT_UTF8,
                   DNSClient,
                   AutoStart: false)

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

            : base(SOAPServer,
                   URIPrefix)

        { }

        #endregion

        #endregion


        #region (override) RegisterURITemplates()

        protected override void RegisterURITemplates()
        {

            #region / (HTTPRoot)

            SOAPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.GET,
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


            #region /Reservation - AuthorizeRemoteReservationStart

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/Reservation",
                                            "AuthorizeRemoteReservationStart",
                                            XML => XML.Descendants(OICPNS.Reservation + "eRoamingAuthorizeRemoteReservationStart").FirstOrDefault(),
                                            (Request, RemoteStartXML) => {


                    #region Documentation

                    // <soapenv:Envelope xmlns:soapenv      = "http://schemas.xmlsoap.org/soap/envelope/"
                    //                   xmlns:Reservation  = "http://www.hubject.com/b2b/services/reservation/v1.0"
                    //                   xmlns:CommonTypes  = "http://www.hubject.com/b2b/services/commontypes/v2.0">
                    //
                    //    <soapenv:Header/>
                    //
                    //    <soapenv:Body>
                    //       <Reservation:eRoamingAuthorizeRemoteReservationStart>
                    //
                    //          <!--Optional:-->
                    //          <Reservation:SessionID>?</Reservation:SessionID>
                    //
                    //          <!--Optional:-->
                    //          <Reservation:PartnerSessionID>?</Reservation:PartnerSessionID>
                    //
                    //          <Reservation:ProviderID>?</Reservation:ProviderID>
                    //          <Reservation:EVSEID>?</Reservation:EVSEID>
                    //
                    //          <Reservation:Identification>
                    //
                    //             <!--You have a CHOICE of the next 4 items at this level-->
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
                    //          </Reservation:Identification>
                    //
                    //          <!--Optional:-->
                    //          <Reservation:PartnerProductID>?</Reservation:PartnerProductID>
                    //
                    //       </Reservation:eRoamingAuthorizeRemoteReservationStart>
                    //    </soapenv:Body>
                    // </soapenv:Envelope>

                    #endregion

                    #region Send OnLogRemoteReservationStart event

                    try
                    {

                        OnLogRemoteReservationStart?.Invoke(DateTime.Now,
                                                            this.SOAPServer,
                                                            Request);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnLogRemoteReservationStart));
                    }

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

                        SessionId                = ChargingSession_Id.Parse(RemoteStartXML.ElementValueOrDefault(OICPNS.Reservation + "SessionID",        null));

                        PartnerSessionIdXML      = RemoteStartXML.Element(OICPNS.Reservation + "PartnerSessionID");
                        if (PartnerSessionIdXML != null)
                            PartnerSessionId = ChargingSession_Id.Parse(PartnerSessionIdXML.Value);

                        ProviderId               = EVSP_Id.           Parse(RemoteStartXML.ElementValueOrFail   (OICPNS.Reservation + "ProviderID", "No ProviderID XML tag provided!"));
                        EVSEId                   = EVSE_Id.           Parse(RemoteStartXML.ElementValueOrFail   (OICPNS.Reservation + "EVSEID",     "No EVSEID XML tag provided!"));

                        ChargingProductIdXML = RemoteStartXML.Element(OICPNS.Reservation + "PartnerProductID");
                        if (ChargingProductIdXML != null)
                            ChargingProductId = ChargingProduct_Id.Parse(ChargingProductIdXML.Value);

                        IdentificationXML        = RemoteStartXML.   ElementOrFail(OICPNS.Reservation   + "Identification",       "No EVSEID XML tag provided!");
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
                            Content         = SOAP.Encapsulation(
                                                  new eRoamingAcknowledgement(false,
                                                                              22,
                                                                              "Request led to an exception!",
                                                                              e.Message).ToXML).ToUTF8Bytes()
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

                    ReservationResult response = null;

                    var OnRemoteReservationStartLocal = OnRemoteReservationStart;
                    if (OnRemoteReservationStartLocal != null)
                    {

                        var CTS = new CancellationTokenSource();

                        var task = OnRemoteReservationStartLocal(DateTime.Now,
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

                    var HubjectCode            = 320;
                    var HubjectDescription     = "Service not available!";
                    var HubjectAdditionalInfo  = "";

                    if (response != null)
                        switch (response.Result)
                        {

                            case ReservationResultType.Success:
                                HubjectCode         = 0;
                                HubjectDescription  = "Ready to charge!";
                                break;

                            //case ReservationResultType.InvalidSessionId:
                            //    HubjectCode         = "400";
                            //    HubjectDescription  = "Session is invalid";
                            //    break;

                            case ReservationResultType.Offline:
                                HubjectCode         = 501;
                                HubjectDescription  = "Communication to EVSE failed!";
                                break;

                            case ReservationResultType.Timeout:
                                HubjectCode         = 510;
                                HubjectDescription  = "Communication to EVSE failed!";
                                break;

                            case ReservationResultType.AlreadyReserved:
                                HubjectCode         = 601;
                                HubjectDescription  = "EVSE reserved!";
                                break;

                            case ReservationResultType.AlreadyInUse:
                                HubjectCode         = 602;
                                HubjectDescription  = "EVSE is already in use!";
                                break;

                            case ReservationResultType.UnknownEVSE:
                                HubjectCode         = 603;
                                HubjectDescription  = "Unknown EVSE ID!";
                                break;

                            case ReservationResultType.OutOfService:
                                HubjectCode         = 700;
                                HubjectDescription  = "EVSE out of service!";
                                break;


                            default:
                                HubjectCode         = 320;
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
                                              new eRoamingAcknowledgement(response != null && response.Result == ReservationResultType.Success,
                                                                          HubjectCode,
                                                                          HubjectDescription,
                                                                          HubjectAdditionalInfo,
                                                                          SessionId).ToXML).ToUTF8Bytes()
                    };


                    #endregion


                    #region Send OnLogRemoteReservationStarted event

                    try
                    {

                        OnLogRemoteReservationStarted?.Invoke(HTTPResponse.Timestamp,
                                                              this.SOAPServer,
                                                              Request,
                                                              HTTPResponse);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnLogRemoteReservationStarted));
                    }

                    #endregion

                    return HTTPResponse;

            });

            #endregion

            #region /Reservation - AuthorizeRemoteReservationStop

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/Reservation",
                                            "AuthorizeRemoteReservationStop",
                                            XML => XML.Descendants(OICPNS.Reservation + "eRoamingAuthorizeRemoteReservationStop").FirstOrDefault(),
                                            (Request, RemoteStopXML) => {

                    #region Documentation

                    // <soapenv:Envelope xmlns:soapenv      = "http://schemas.xmlsoap.org/soap/envelope/"
                    //                   xmlns:Reservation  = "http://www.hubject.com/b2b/services/reservation/v1.0">
                    //
                    //    <soapenv:Header/>
                    //
                    //    <soapenv:Body>
                    //       <Reservation:eRoamingAuthorizeRemoteReservationStop>
                    //
                    //          <Reservation:SessionID>?</Authorization:SessionID>
                    //
                    //          <!--Optional:-->
                    //          <Reservation:PartnerSessionID>?</Authorization:PartnerSessionID>
                    //
                    //          <Reservation:ProviderID>?</Authorization:ProviderID>
                    //          <Reservation:EVSEID>?</Authorization:EVSEID>
                    //
                    //       </Reservation:eRoamingAuthorizeRemoteReservationStop>
                    //    </soapenv:Body>
                    //
                    // </soapenv:Envelope>

                    #endregion

                    #region Send OnLogRemoteReservationStop event

                    try
                    {

                        OnLogRemoteReservationStop?.Invoke(DateTime.Now,
                                                           this.SOAPServer,
                                                           Request);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnLogRemoteReservationStop));
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

                        SessionId         = ChargingSession_Id.Parse(RemoteStopXML.ElementValueOrFail(OICPNS.Reservation + "SessionID",  "No SessionID XML tag provided!"));

                        PartnerSessionIdXML = RemoteStopXML.Element(OICPNS.Reservation + "PartnerSessionID");
                        if (PartnerSessionIdXML != null)
                            PartnerSessionId = ChargingSession_Id.Parse(PartnerSessionIdXML.Value);

                        ProviderId        = EVSP_Id.           Parse(RemoteStopXML.ElementValueOrFail(OICPNS.Reservation + "ProviderID", "No ProviderID XML tag provided!"));
                        EVSEId            = EVSE_Id.           Parse(RemoteStopXML.ElementValueOrFail(OICPNS.Reservation + "EVSEID",     "No EVSEID XML tag provided!"));

                    }
                    catch (Exception e)
                    {

                        return new HTTPResponseBuilder(Request) {
                            HTTPStatusCode  = HTTPStatusCode.OK,
                            ContentType     = HTTPContentType.XMLTEXT_UTF8,
                            Content         = SOAP.Encapsulation(
                                                  new eRoamingAcknowledgement(false,
                                                                              22,
                                                                              "Request led to an exception!",
                                                                              e.Message).ToXML).ToUTF8Bytes()
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

                    CancelReservationResult response = null;

                    var OnRemoteReservationStopLocal = OnRemoteReservationStop;
                    if (OnRemoteReservationStopLocal != null)
                    {

                        var CTS = new CancellationTokenSource();

                        var task = OnRemoteReservationStopLocal(DateTime.Now,
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

                    var HubjectCode            = 320;
                    var HubjectDescription     = "Service not available!";
                    var HubjectAdditionalInfo  = "";

                    if (response != null)
                        switch (response.Result)
                        {

                            case CancelReservationResultType.Success:
                                HubjectCode         = 0;
                                HubjectDescription  = "Ready to stop charging!";
                                break;

                            case CancelReservationResultType.UnknownReservationId:
                                HubjectCode         = 400;
                                HubjectDescription  = "Session is invalid";
                                break;

                            case CancelReservationResultType.Offline:
                                HubjectCode         = 501;
                                HubjectDescription  = "Communication to EVSE failed!";
                                break;

                            case CancelReservationResultType.Timeout:
                                HubjectCode         = 510;
                                HubjectDescription  = "No EV connected to EVSE!";
                                break;

                            case CancelReservationResultType.UnknownEVSE:
                                HubjectCode         = 603;
                                HubjectDescription  = "Unknown EVSE ID!";
                                break;

                            case CancelReservationResultType.OutOfService:
                                HubjectCode         = 700;
                                HubjectDescription  = "EVSE out of service!";
                                break;


                            default:
                                HubjectCode         = 320;
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
                                              new eRoamingAcknowledgement(response != null && response.Result == CancelReservationResultType.Success,
                                                                          HubjectCode,
                                                                          HubjectDescription,
                                                                          HubjectAdditionalInfo,
                                                                          SessionId).ToXML).ToUTF8Bytes()
                    };

                    #endregion


                    #region Send OnLogRemoteReservationStopped event

                    try
                    {

                        OnLogRemoteReservationStopped?.Invoke(HTTPResponse.Timestamp,
                                                              this.SOAPServer,
                                                              Request,
                                                              HTTPResponse);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnLogRemoteReservationStopped));
                    }

                    #endregion

                    return HTTPResponse;

            });

            #endregion


            #region /Authorization - AuthorizeRemoteStart

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/Authorization",
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

                    #region Send OnLogRemoteStart event

                    try
                    {

                        OnLogRemoteStart?.Invoke(DateTime.Now, this.SOAPServer, Request);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnLogRemoteStart));
                    }

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
                            Content         = SOAP.Encapsulation(
                                                  new eRoamingAcknowledgement(false,
                                                                              22,
                                                                              "Request led to an exception!",
                                                                              e.Message).ToXML).ToUTF8Bytes()
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

                    var HubjectCode            = 320;
                    var HubjectDescription     = "Service not available!";
                    var HubjectAdditionalInfo  = "";

                    if (response != null)
                        switch (response.Result)
                        {

                            case RemoteStartEVSEResultType.Success:
                                HubjectCode         = 0;
                                HubjectDescription  = "Ready to charge!";
                                break;

                            case RemoteStartEVSEResultType.InvalidSessionId:
                                HubjectCode         = 400;
                                HubjectDescription  = "Session is invalid";
                                break;

                            case RemoteStartEVSEResultType.Offline:
                                HubjectCode         = 501;
                                HubjectDescription  = "Communication to EVSE failed!";
                                break;

                            case RemoteStartEVSEResultType.Timeout:
                                HubjectCode         = 510;
                                HubjectDescription  = "No EV connected to EVSE!";
                                break;

                            case RemoteStartEVSEResultType.Reserved:
                                HubjectCode         = 601;
                                HubjectDescription  = "EVSE reserved!";
                                break;

                            case RemoteStartEVSEResultType.AlreadyInUse:
                                HubjectCode         = 602;
                                HubjectDescription  = "EVSE is already in use!";
                                break;

                            case RemoteStartEVSEResultType.UnknownEVSE:
                                HubjectCode         = 603;
                                HubjectDescription  = "Unknown EVSE ID!";
                                break;

                            case RemoteStartEVSEResultType.OutOfService:
                                HubjectCode         = 700;
                                HubjectDescription  = "EVSE out of service!";
                                break;


                            default:
                                HubjectCode         = 320;
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
                                              new eRoamingAcknowledgement(response != null && response.Result == RemoteStartEVSEResultType.Success,
                                                                          HubjectCode,
                                                                          HubjectDescription,
                                                                          HubjectAdditionalInfo,
                                                                          SessionId).ToXML).ToUTF8Bytes()
                    };


                    #endregion


                    #region Send OnLogRemoteStarted event

                    try
                    {

                        OnLogRemoteStarted?.Invoke(HTTPResponse.Timestamp,
                                                   this.SOAPServer,
                                                   Request,
                                                   HTTPResponse);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnLogRemoteStarted));
                    }

                    #endregion

                    return HTTPResponse;

            });

            #endregion

            #region /Authorization - AuthorizeRemoteStop

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/Authorization",
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

                    #region Send OnLogRemoteStop event

                    try
                    {

                        OnLogRemoteStop?.Invoke(DateTime.Now,
                                                this.SOAPServer,
                                                Request);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnLogRemoteStop));
                    }

                    #endregion


                    #region Parse request parameters

                    XElement            PartnerSessionIdXML;

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
                            Content         = SOAP.Encapsulation(
                                                  new eRoamingAcknowledgement(false,
                                                                              22,
                                                                              "Request led to an exception!",
                                                                              e.Message).ToXML).ToUTF8Bytes()
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

                    var HubjectCode            = 320;
                    var HubjectDescription     = "Service not available!";
                    var HubjectAdditionalInfo  = "";

                    if (response != null)
                        switch (response.Result)
                        {

                            case RemoteStopEVSEResultType.Success:
                                HubjectCode         = 000;
                                HubjectDescription  = "Ready to stop charging!";
                                break;

                            case RemoteStopEVSEResultType.InvalidSessionId:
                                HubjectCode         = 400;
                                HubjectDescription  = "Session is invalid";
                                break;

                            case RemoteStopEVSEResultType.Offline:
                                HubjectCode         = 501;
                                HubjectDescription  = "Communication to EVSE failed!";
                                break;

                            case RemoteStopEVSEResultType.Timeout:
                                HubjectCode         = 510;
                                HubjectDescription  = "No EV connected to EVSE!";
                                break;

                            case RemoteStopEVSEResultType.UnknownEVSE:
                                HubjectCode         = 603;
                                HubjectDescription  = "Unknown EVSE ID!";
                                break;

                            case RemoteStopEVSEResultType.OutOfService:
                                HubjectCode         = 700;
                                HubjectDescription  = "EVSE out of service!";
                                break;


                            default:
                                HubjectCode         = 320;
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
                                              new eRoamingAcknowledgement(response != null && response.Result == RemoteStopEVSEResultType.Success,
                                                                          HubjectCode,
                                                                          HubjectDescription,
                                                                          HubjectAdditionalInfo,
                                                                          SessionId).ToXML).ToUTF8Bytes()
                    };

                    #endregion


                    #region Send OnLogRemoteStopped event

                    try
                    {

                        OnLogRemoteStopped?.Invoke(HTTPResponse.Timestamp,
                                                   this.SOAPServer,
                                                   Request,
                                                   HTTPResponse);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnLogRemoteStopped));
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


    }

}