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
        public new const           String           DefaultHTTPServerName  = "GraphDefined OICP " + Version.Number + " HTTP/SOAP/XML CPO API";

        /// <summary>
        /// The default HTTP/SOAP/XML server TCP port.
        /// </summary>
        public new static readonly IPPort           DefaultHTTPServerPort  = new IPPort(2002);

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

        #region CPOServer(HTTPServerName, TCPPort = default, URIPrefix = default, ContentType = default, DNSClient = null, AutoStart = false)

        /// <summary>
        /// Initialize an new HTTP server for the OICP HTTP/SOAP/XML CPO API.
        /// </summary>
        /// <param name="HTTPServerName">An optional identification string for the HTTP server.</param>
        /// <param name="TCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="ContentType">An optional HTTP content type to use.</param>
        /// <param name="RegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="AutoStart">Start the server immediately.</param>
        public CPOServer(String          HTTPServerName           = DefaultHTTPServerName,
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

        #region CPOServer(SOAPServer, URIPrefix = default)

        /// <summary>
        /// Use the given SOAP server for the OICP HTTP/SOAP/XML CPO API.
        /// </summary>
        /// <param name="SOAPServer">A SOAP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        public CPOServer(SOAPServer  SOAPServer,
                         String      URIPrefix  = DefaultURIPrefix)

            : base(SOAPServer,
                   URIPrefix ?? DefaultURIPrefix)

        { }

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
                                            URIPrefix + "/Reservation",
                                            "AuthorizeRemoteReservationStart",
                                            XML => XML.Descendants(OICPNS.Reservation + "eRoamingAuthorizeRemoteReservationStart").FirstOrDefault(),
                                            async (Request, RemoteStartXML) => {


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

                    Session_Id?         SessionId          = null;
                    PartnerSession_Id?  PartnerSessionId   = null;
                    Provider_Id?        ProviderId         = null;
                    EVSE_Id?            EVSEId             = null;
                    EVCO_Id?            EVCOId             = null;
                    PartnerProduct_Id?  PartnerProductId   = null;

                    Acknowledgement     response           = null;

                    try
                    {

                        XElement            IdentificationXML;
                        XElement            QRCodeIdentificationXML;
                        XElement            PnCIdentificationXML;
                        XElement            RemoteIdentificationXML;
                        XElement            SessionIdXML;
                        XElement            PartnerSessionIdXML;
                        XElement            ChargingProductIdXML;

                        SessionIdXML             = RemoteStartXML.Element(OICPNS.Reservation + "SessionID");
                        if (SessionIdXML != null)
                            SessionId        = Session_Id.Parse(SessionIdXML.Value);

                        PartnerSessionIdXML      = RemoteStartXML.Element(OICPNS.Reservation + "PartnerSessionID");
                        if (PartnerSessionIdXML != null)
                            PartnerSessionId = PartnerSession_Id.Parse(PartnerSessionIdXML.Value);

                        ProviderId               = Provider_Id.Parse(RemoteStartXML.ElementValueOrFail   (OICPNS.Reservation + "ProviderID", "No ProviderID XML tag provided!"));
                        EVSEId                   = EVSE_Id.    Parse(RemoteStartXML.ElementValueOrFail   (OICPNS.Reservation + "EVSEID",     "No EVSEID XML tag provided!"));

                        ChargingProductIdXML = RemoteStartXML.Element(OICPNS.Reservation + "PartnerProductID");
                        if (ChargingProductIdXML != null && ChargingProductIdXML.Value.IsNotNullOrEmpty())
                            PartnerProductId = PartnerProduct_Id.Parse(ChargingProductIdXML.Value);

                        IdentificationXML        = RemoteStartXML.   ElementOrFail(OICPNS.Reservation   + "Identification",       "No EVSEID XML tag provided!");
                        QRCodeIdentificationXML  = IdentificationXML.Element      (OICPNS.CommonTypes   + "QRCodeIdentification");
                        PnCIdentificationXML     = IdentificationXML.Element      (OICPNS.CommonTypes   + "PlugAndChargeIdentification");
                        RemoteIdentificationXML  = IdentificationXML.Element      (OICPNS.CommonTypes   + "RemoteIdentification");

                        if (QRCodeIdentificationXML == null &&
                            PnCIdentificationXML    == null &&
                            RemoteIdentificationXML == null)
                            throw new Exception("Neither a QRCodeIdentification, PlugAndChargeIdentification, nor a RemoteIdentification was provided!");

                        if      (QRCodeIdentificationXML != null)
                            EVCOId = EVCO_Id.Parse(QRCodeIdentificationXML.ElementValueOrFail(OICPNS.CommonTypes   + "EVCOID",    "No EVCOID XML tag provided!"));

                        else if (PnCIdentificationXML != null)
                            EVCOId = EVCO_Id.Parse(PnCIdentificationXML.   ElementValueOrFail(OICPNS.CommonTypes   + "EVCOID",    "No EVCOID XML tag provided!"));

                        else if (RemoteIdentificationXML != null)
                            EVCOId = EVCO_Id.Parse(RemoteIdentificationXML.ElementValueOrFail(OICPNS.CommonTypes   + "EVCOID",    "No EVCOID XML tag provided!"));

                    }
                    catch (Exception e)
                    {

                        response = Acknowledgement.DataError(
                                       "Request led to an exception!",
                                       e.Message
                                   );

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

                    if (response == null)
                    {

                        var results = OnRemoteReservationStart?.
                                          GetInvocationList()?.
                                          SafeSelect(subscriber => (subscriber as OnRemoteReservationStartDelegate)
                                              (DateTime.Now,
                                               this,
                                               Request.CancellationToken,
                                               Request.EventTrackingId,
                                               EVSEId.Value,
                                               PartnerProductId,
                                               SessionId,
                                               PartnerSessionId,
                                               ProviderId,
                                               EVCOId,
                                               DefaultRequestTimeout)).
                                          ToArray();

                        if (results.Length > 0)
                        {

                            await Task.WhenAll(results);

                            response = results.FirstOrDefault()?.Result;

                        }

                        if (results.Length == 0 || response == null)
                            response = Acknowledgement.SystemError(
                                           "Could not process the incoming AuthorizeRemoteReservationStart request!",
                                           null,
                                           SessionId,
                                           PartnerSessionId
                                       );

                    }

                    #endregion

                    #region Create SOAPResponse

                    var HTTPResponse = new HTTPResponseBuilder(Request) {
                        HTTPStatusCode  = HTTPStatusCode.OK,
                        Server          = SOAPServer.DefaultServerName,
                        Date            = DateTime.Now,
                        ContentType     = HTTPContentType.XMLTEXT_UTF8,
                        Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
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
                                            async (Request, RemoteStopXML) => {

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

                    Session_Id?         SessionId          = null;
                    PartnerSession_Id?  PartnerSessionId   = null;
                    Provider_Id?        ProviderId         = null;
                    EVSE_Id?            EVSEId             = null;

                    Acknowledgement     response           = null;

                    try
                    {

                        XElement PartnerSessionIdXML;

                        SessionId         = Session_Id.Parse(RemoteStopXML.ElementValueOrFail(OICPNS.Reservation + "SessionID",  "No SessionID XML tag provided!"));

                        PartnerSessionIdXML = RemoteStopXML.Element(OICPNS.Reservation + "PartnerSessionID");
                        if (PartnerSessionIdXML != null)
                            PartnerSessionId = PartnerSession_Id.Parse(PartnerSessionIdXML.Value);

                        ProviderId        = Provider_Id.Parse(RemoteStopXML.ElementValueOrFail(OICPNS.Reservation + "ProviderID", "No ProviderID XML tag provided!"));
                        EVSEId            = EVSE_Id.    Parse(RemoteStopXML.ElementValueOrFail(OICPNS.Reservation + "EVSEID",     "No EVSEID XML tag provided!"));

                    }
                    catch (Exception e)
                    {

                        response = Acknowledgement.DataError(
                                       "Request led to an exception!",
                                       e.Message
                                   );

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

                    if (response == null)
                    {

                        var results = OnRemoteReservationStop?.
                                          GetInvocationList()?.
                                          SafeSelect(subscriber => (subscriber as OnRemoteReservationStopDelegate)
                                              (DateTime.Now,
                                               this,
                                               Request.CancellationToken,
                                               Request.EventTrackingId,
                                               EVSEId.Value,
                                               SessionId,
                                               PartnerSessionId,
                                               ProviderId,
                                               DefaultRequestTimeout)).
                                          ToArray();

                        if (results.Length > 0)
                        {

                            await Task.WhenAll(results);

                            response = results.FirstOrDefault()?.Result;

                        }

                        if (results.Length == 0 || response == null)
                            response = Acknowledgement.SystemError(
                                           "Could not process the incoming AuthorizeRemoteReservationStop request!",
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
                                            async (Request, RemoteStartXML) => {


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

                    Session_Id?         SessionId          = null;
                    PartnerSession_Id?  PartnerSessionId   = null;
                    Provider_Id?        ProviderId         = null;
                    EVSE_Id?            EVSEId             = null;
                    EVCO_Id?            EVCOId             = null;
                    PartnerProduct_Id?  PartnerProductId   = null;

                    Acknowledgement     response           = null;

                    try
                    {

                        XElement  IdentificationXML;
                        XElement  QRCodeIdentificationXML;
                        XElement  PnCIdentificationXML;
                        XElement  RemoteIdentificationXML;
                        XElement  PartnerSessionIdXML;
                        XElement  ChargingProductIdXML;

                        SessionId                = Session_Id.Parse(RemoteStartXML.ElementValueOrDefault(OICPNS.Authorization + "SessionID",        null));

                        PartnerSessionIdXML      = RemoteStartXML.Element(OICPNS.Authorization + "PartnerSessionID");
                        if (PartnerSessionIdXML != null)
                            PartnerSessionId = PartnerSession_Id.Parse(PartnerSessionIdXML.Value);

                        ProviderId               = Provider_Id.Parse(RemoteStartXML.ElementValueOrFail   (OICPNS.Authorization + "ProviderID", "No ProviderID XML tag provided!"));
                        EVSEId                   = EVSE_Id.    Parse(RemoteStartXML.ElementValueOrFail   (OICPNS.Authorization + "EVSEID",     "No EVSEID XML tag provided!"));

                        ChargingProductIdXML = RemoteStartXML.Element(OICPNS.Authorization + "PartnerProductID");
                        if (ChargingProductIdXML != null && ChargingProductIdXML.Value.IsNotNullOrEmpty())
                            PartnerProductId = PartnerProduct_Id.Parse(ChargingProductIdXML.Value);

                        IdentificationXML        = RemoteStartXML.   ElementOrFail(OICPNS.Authorization + "Identification",       "No EVSEID XML tag provided!");
                        QRCodeIdentificationXML  = IdentificationXML.Element      (OICPNS.CommonTypes   + "QRCodeIdentification");
                        PnCIdentificationXML     = IdentificationXML.Element      (OICPNS.CommonTypes   + "PlugAndChargeIdentification");
                        RemoteIdentificationXML  = IdentificationXML.Element      (OICPNS.CommonTypes   + "RemoteIdentification");

                        if (QRCodeIdentificationXML == null &&
                            PnCIdentificationXML    == null &&
                            RemoteIdentificationXML == null)
                            throw new Exception("Neither a QRCodeIdentification, PlugAndChargeIdentification, nor a RemoteIdentification was provided!");

                        if      (QRCodeIdentificationXML != null)
                            EVCOId = EVCO_Id.Parse(QRCodeIdentificationXML.ElementValueOrFail(OICPNS.CommonTypes   + "EVCOID",    "No EVCOID XML tag provided!"));

                        else if (PnCIdentificationXML != null)
                            EVCOId = EVCO_Id.Parse(PnCIdentificationXML.   ElementValueOrFail(OICPNS.CommonTypes   + "EVCOID",    "No EVCOID XML tag provided!"));

                        else if (RemoteIdentificationXML != null)
                            EVCOId = EVCO_Id.Parse(RemoteIdentificationXML.ElementValueOrFail(OICPNS.CommonTypes   + "EVCOID",    "No EVCOID XML tag provided!"));

                    }
                    catch (Exception e)
                    {

                        response = Acknowledgement.DataError(
                                       "Request led to an exception!",
                                       e.Message
                                   );

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

                    if (response == null)
                    {

                        var results = OnRemoteStart?.
                                          GetInvocationList()?.
                                          SafeSelect(subscriber => (subscriber as OnRemoteStartDelegate)
                                              (DateTime.Now,
                                               this,
                                               Request.CancellationToken,
                                               Request.EventTrackingId,
                                               EVSEId.Value,
                                               PartnerProductId,
                                               SessionId,
                                               PartnerSessionId,
                                               ProviderId,
                                               EVCOId,
                                               DefaultRequestTimeout)).
                                          ToArray();

                        if (results.Length > 0)
                        {

                            await Task.WhenAll(results);

                            response = results.FirstOrDefault()?.Result;

                        }

                        if (results.Length == 0 || response == null)
                            response = Acknowledgement.SystemError(
                                           "Could not process the incoming AuthorizeRemoteStart request!",
                                           null,
                                           SessionId,
                                           PartnerSessionId
                                       );

                    }

                    #endregion

                    #region Create SOAPResponse

                    var HTTPResponse = new HTTPResponseBuilder(Request) {
                        HTTPStatusCode  = HTTPStatusCode.OK,
                        Server          = SOAPServer.DefaultServerName,
                        Date            = DateTime.Now,
                        ContentType     = HTTPContentType.XMLTEXT_UTF8,
                        Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
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
                                            async (Request, RemoteStopXML) => {

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

                    Session_Id?         SessionId          = null;
                    PartnerSession_Id?  PartnerSessionId   = null;
                    Provider_Id?        ProviderId         = null;
                    EVSE_Id?            EVSEId             = null;

                    Acknowledgement     response           = null;

                    try
                    {

                        XElement  PartnerSessionIdXML;

                        SessionId         = Session_Id.          Parse(RemoteStopXML.ElementValueOrFail(OICPNS.Authorization + "SessionID",  "No SessionID XML tag provided!"));

                        PartnerSessionIdXML = RemoteStopXML.Element(OICPNS.Authorization + "PartnerSessionID");
                        if (PartnerSessionIdXML != null)
                            PartnerSessionId = PartnerSession_Id.Parse(PartnerSessionIdXML.Value);

                        ProviderId        = Provider_Id.Parse(RemoteStopXML.ElementValueOrFail(OICPNS.Authorization + "ProviderID", "No ProviderID XML tag provided!"));
                        EVSEId            = EVSE_Id.    Parse(RemoteStopXML.ElementValueOrFail(OICPNS.Authorization + "EVSEID",     "No EVSEID XML tag provided!"));

                    }
                    catch (Exception e)
                    {

                        response = Acknowledgement.DataError(
                                       "Request led to an exception!",
                                       e.Message
                                   );

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

                    if (response == null)
                    {

                        var results = OnRemoteStop?.
                                          GetInvocationList()?.
                                          SafeSelect(subscriber => (subscriber as OnRemoteStopDelegate)
                                              (DateTime.Now,
                                               this,
                                               Request.CancellationToken,
                                               Request.EventTrackingId,
                                               EVSEId.Value,
                                               SessionId.Value,
                                               PartnerSessionId,
                                               ProviderId,
                                               DefaultRequestTimeout)).
                                          ToArray();

                        if (results.Length > 0)
                        {

                            await Task.WhenAll(results);

                            response = results.FirstOrDefault()?.Result;

                        }

                        if (results.Length == 0 || response == null)
                            response = Acknowledgement.SystemError(
                                           "Could not process the incoming AuthorizeRemoteStop request!",
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


    }

}
