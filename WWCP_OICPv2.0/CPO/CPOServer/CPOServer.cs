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

namespace org.GraphDefined.WWCP.OICPv2_0
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
        public new const           String    DefaultHTTPServerName  = "GraphDefined OICP " + Version.Number + " HTTP/SOAP/XML CPO Server API";

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
                                         HTTPDelegate: async Request => {

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

                    ChargingSession_Id      SessionId           = null;
                    ChargingSession_Id      PartnerSessionId    = null;
                    eMobilityProvider_Id                 ProviderId          = null;
                    EVSE_Id                 EVSEId              = null;
                    eMobilityAccount_Id                  eMAId               = null;
                    ChargingProduct_Id      ChargingProductId   = null;

                    eRoamingAcknowledgement response            = null;

                    try
                    {

                        XElement  IdentificationXML;
                        XElement  QRCodeIdentificationXML;
                        XElement  PnCIdentificationXML;
                        XElement  RemoteIdentificationXML;
                        XElement  PartnerSessionIdXML;
                        XElement  ChargingProductIdXML;

                        SessionId                = ChargingSession_Id.Parse(RemoteStartXML.ElementValueOrDefault(OICPNS.Authorization + "SessionID",        null));

                        PartnerSessionIdXML      = RemoteStartXML.Element(OICPNS.Authorization + "PartnerSessionID");
                        if (PartnerSessionIdXML != null)
                            PartnerSessionId = ChargingSession_Id.Parse(PartnerSessionIdXML.Value);

                        ProviderId               = eMobilityProvider_Id.           Parse(RemoteStartXML.ElementValueOrFail   (OICPNS.Authorization + "ProviderID", "No ProviderID XML tag provided!"));
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
                            eMAId = eMobilityAccount_Id.Parse(QRCodeIdentificationXML.ElementValueOrFail(OICPNS.CommonTypes   + "EVCOID",    "No EVCOID XML tag provided!"));

                        else if (PnCIdentificationXML != null)
                            eMAId = eMobilityAccount_Id.Parse(PnCIdentificationXML.   ElementValueOrFail(OICPNS.CommonTypes   + "EVCOID",    "No EVCOID XML tag provided!"));

                        else if (RemoteIdentificationXML != null)
                            eMAId = eMobilityAccount_Id.Parse(RemoteIdentificationXML.ElementValueOrFail(OICPNS.CommonTypes   + "EVCOID",    "No EVCOID XML tag provided!"));

                    }
                    catch (Exception e)
                    {

                        response = new eRoamingAcknowledgement(StatusCodes.DataError,
                                                               "Request led to an exception!",
                                                               e.Message);

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
                                               EVSEId,
                                               ChargingProductId,
                                               SessionId,
                                               PartnerSessionId,
                                               ProviderId,
                                               eMAId,
                                               DefaultQueryTimeout)).
                                          ToArray();

                        if (results.Length > 0)
                        {

                            await Task.WhenAll(results);

                            response = results.FirstOrDefault()?.Result;

                        }

                        if (results.Length == 0 || response == null)
                            response = new eRoamingAcknowledgement(StatusCodes.SystemError,
                                                                   "Could not process the incoming AuthorizeRemoteStart request!",
                                                                   null,
                                                                   SessionId,
                                                                   PartnerSessionId);

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

                    ChargingSession_Id      SessionId         = null;
                    ChargingSession_Id      PartnerSessionId  = null;
                    eMobilityProvider_Id                 ProviderId        = null;
                    EVSE_Id                 EVSEId            = null;

                    eRoamingAcknowledgement response          = null;

                    try
                    {

                        XElement  PartnerSessionIdXML;

                        SessionId         = ChargingSession_Id.Parse(RemoteStopXML.ElementValueOrFail(OICPNS.Authorization + "SessionID",  "No SessionID XML tag provided!"));

                        PartnerSessionIdXML = RemoteStopXML.Element(OICPNS.Authorization + "PartnerSessionID");
                        if (PartnerSessionIdXML != null)
                            PartnerSessionId = ChargingSession_Id.Parse(PartnerSessionIdXML.Value);

                        ProviderId        = eMobilityProvider_Id.           Parse(RemoteStopXML.ElementValueOrFail(OICPNS.Authorization + "ProviderID", "No ProviderID XML tag provided!"));
                        EVSEId            = EVSE_Id.           Parse(RemoteStopXML.ElementValueOrFail(OICPNS.Authorization + "EVSEID",     "No EVSEID XML tag provided!"));

                    }
                    catch (Exception e)
                    {

                        response = new eRoamingAcknowledgement(StatusCodes.DataError,
                                                               "Request led to an exception!",
                                                               e.Message);

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
                                               EVSEId,
                                               SessionId,
                                               PartnerSessionId,
                                               ProviderId,
                                               DefaultQueryTimeout)).
                                          ToArray();

                        if (results.Length > 0)
                        {

                            await Task.WhenAll(results);

                            response = results.FirstOrDefault()?.Result;

                        }

                        if (results.Length == 0 || response == null)
                            response = new eRoamingAcknowledgement(StatusCodes.SystemError,
                                                                   "Could not process the incoming AuthorizeRemoteStop request!",
                                                                   null,
                                                                   SessionId,
                                                                   PartnerSessionId);

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