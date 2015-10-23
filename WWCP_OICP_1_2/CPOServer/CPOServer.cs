/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/WorldWideCharging/WWCP_OICP>
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
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.ConsoleLog;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

using org.GraphDefined.WWCP.LocalService;

#endregion

namespace org.GraphDefined.WWCP.OICP_1_2
{

    /// <summary>
    /// OICP v2.0 CPO HTTP/SOAP/XML server.
    /// </summary>
    public class CPOServer : HTTPServer
    {

        #region Events

        #region OnRemoteStart

        /// <summary>
        /// An event fired whenever a remote start command was received.
        /// </summary>
        public event OnRemoteStartDelegate OnRemoteStart;

        #endregion

        #region OnRemoteStop

        /// <summary>
        /// An event fired whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopDelegate OnRemoteStop;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initialize the OICP HTTP/SOAP/XML server using IPAddress.Any.
        /// </summary>
        /// <param name="IPPort">The TCP listing port.</param>
        public CPOServer(IPPort  IPPort)
        {

            this.AttachTCPPort(IPPort);

            #region / (HTTPRoot)

            // HTML
            this.AddMethodCallback(HTTPMethod.GET,
                                   "/",
                                   HTTPContentType.HTML_UTF8,
                                   HTTPDelegate: HTTPRequest => {

                                       var RoamingNetworkId = HTTPRequest.ParsedURIParameters[0];

                                       return new HTTPResponseBuilder() {
                                           HTTPStatusCode  = HTTPStatusCode.BadGateway,
                                           ContentType     = HTTPContentType.HTML_UTF8,
                                           Content         = ("/RNs/{RoamingNetworkId}/RemoteStartStop is a HTTP/SOAP/XML endpoint!").ToUTF8Bytes(),
                                           Connection      = "close"
                                       };

                                   });

            // Text
            this.AddMethodCallback(HTTPMethod.GET,
                                   "/",
                                   HTTPContentType.TEXT_UTF8,
                                   HTTPDelegate: HTTPRequest => {

                                       var RoamingNetworkId = HTTPRequest.ParsedURIParameters[0];

                                       return new HTTPResponseBuilder() {
                                           HTTPStatusCode  = HTTPStatusCode.BadGateway,
                                           ContentType     = HTTPContentType.HTML_UTF8,
                                           Content         = ("/RNs/{RoamingNetworkId}/RemoteStartStop is a HTTP/SOAP/XML endpoint!").ToUTF8Bytes(),
                                           Connection      = "close"
                                       };

                                   });

            #endregion

            #region /RNs/{RoamingNetworkId}/RemoteStartStop

            #region Generic RemoteStartStopDelegate

            HTTPDelegate RemoteStartStopDelegate = HTTPRequest => {

                //var _EventTrackingId = EventTracking_Id.New;
                //Log.WriteLine("Event tracking: " + _EventTrackingId);

                #region Try to parse the RoamingNetworkId

                RoamingNetwork_Id RoamingNetworkId;

                if (!RoamingNetwork_Id.TryParse(HTTPRequest.ParsedURIParameters[0], out RoamingNetworkId))
                    return new HTTPResponseBuilder() {
                        HTTPStatusCode  = HTTPStatusCode.BadRequest,
                        Server          = this.DefaultServerName,
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

                    GetEventSource(Semantics.DebugLog).
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

                //Log.WriteLine("");
                //Log.Timestamp("Incoming XML request:");
                //Log.WriteLine("XML payload:");
                //Log.WriteLine(XMLRequest.Data.ToString());
                //Log.WriteLine("");

                #region Get SOAP request...

                IEnumerable<XElement> RemoteStartXMLs;
                IEnumerable<XElement> RemoteStopXMLs;

                try
                {

                    RemoteStartXMLs = XMLRequest.Data.Root.Descendants(OICPNS.Authorization + "eRoamingAuthorizeRemoteStart");
                    RemoteStopXMLs  = XMLRequest.Data.Root.Descendants(OICPNS.Authorization + "eRoamingAuthorizeRemoteStop");

                    if (!RemoteStartXMLs.Any() && !RemoteStopXMLs.Any())
                        throw new Exception("Must be either RemoteStart or RemoteStop XML request!");

                    if (RemoteStartXMLs.Count() > 1)
                        throw new Exception("Multiple RemoteStart XML tags within a single request are not supported!");

                    if (RemoteStopXMLs. Count() > 1)
                        throw new Exception("Multiple RemoteStop XML tags within a single request are not supported!");

                }
                catch (Exception e)
                {

                    Log.WriteLine("Invalid XML request!");

                    GetEventSource(Semantics.DebugLog).
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

                #region Process an OICP RemoteStart HTTP/SOAP/XML call

                var RemoteStartXML = RemoteStartXMLs.FirstOrDefault();
                if (RemoteStartXML != null)
                {

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

                    #region Parse request parameters

                    ChargingSession_Id  SessionId;
                    ChargingSession_Id  PartnerSessionId;
                    EVSP_Id             ProviderId;
                    EVSE_Id             EVSEId;
                    XElement            IdentificationXML;
                    XElement            QRCodeIdentificationXML;
                    XElement            PnCIdentificationXML;
                    XElement            RemoteIdentificationXML;
                    eMA_Id              eMAId = null;
                    ChargingProduct_Id  ChargingProductId;

                    try
                    {

                        SessionId                = ChargingSession_Id.Parse(RemoteStartXML.ElementValueOrDefault(OICPNS.Authorization + "SessionID",        null));
                        PartnerSessionId         = ChargingSession_Id.Parse(RemoteStartXML.ElementValueOrDefault(OICPNS.Authorization + "PartnerSessionID", null));
                        ProviderId               = EVSP_Id.           Parse(RemoteStartXML.ElementValueOrFail   (OICPNS.Authorization + "ProviderID", "No ProviderID XML tag provided!"));
                        EVSEId                   = EVSE_Id.           Parse(RemoteStartXML.ElementValueOrFail   (OICPNS.Authorization + "EVSEID",     "No EVSEID XML tag provided!"));
                        ChargingProductId        = ChargingProduct_Id.Parse(RemoteStartXML.ElementValueOrDefault(OICPNS.Authorization + "PartnerProductID", null));

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

                        Log.Timestamp("Invalid RemoteStartXML: " + e.Message);

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


                    var Response = RemoteStartResult.Error;

                    var OnRemoteStartLocal = OnRemoteStart;
                    if (OnRemoteStartLocal != null)
                        Response = OnRemoteStartLocal(DateTime.Now,
                                                      RoamingNetworkId,
                                                      SessionId,
                                                      PartnerSessionId,
                                                      ProviderId,
                                                      eMAId,
                                                      EVSEId,
                                                      ChargingProductId);

                    Log.WriteLine(Response.ToString());

                    switch (Response)
                    {

                        case RemoteStartResult.EVSE_AlreadyInUse:
                            HubjectCode         = "602";
                            HubjectDescription  = "EVSE is already in use!";
                            break;

                        case RemoteStartResult.SessionId_AlreadyInUse:
                            HubjectCode         = "400";
                            HubjectDescription  = "Session is invalid";
                            break;

                        case RemoteStartResult.EVSE_NotReachable:
                            HubjectCode         = "501";
                            HubjectDescription  = "Communication to EVSE failed!";
                            break;

                        case RemoteStartResult.Start_Timeout:
                            HubjectCode         = "501";
                            HubjectDescription  = "Communication to EVSE failed!";
                            break;

                        case RemoteStartResult.Success:
                            HubjectCode         = "000";
                            HubjectDescription  = "Ready to charge!";
                            break;

                        default:
                            HubjectCode         = "000";
                            break;

                    }

                    #region HTTPResponse

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

                #region Process an OICP RemoteStop HTTP/SOAP/XML call

                var RemoteStopXML  = RemoteStopXMLs.FirstOrDefault();
                if (RemoteStopXML != null)
                {

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

                    #region Parse request parameters

                    ChargingSession_Id  SessionId;
                    ChargingSession_Id  PartnerSessionId;
                    EVSP_Id             ProviderId;
                    EVSE_Id             EVSEId;

                    try
                    {

                        SessionId         = ChargingSession_Id.Parse(RemoteStopXML.ElementValueOrFail   (OICPNS.Authorization + "SessionID",        "No SessionID XML tag provided!"));
                        PartnerSessionId  = ChargingSession_Id.Parse(RemoteStopXML.ElementValueOrDefault(OICPNS.Authorization + "ParnterSessionID", null));
                        ProviderId        = EVSP_Id.           Parse(RemoteStopXML.ElementValueOrFail   (OICPNS.Authorization + "ProviderID",       "No ProviderID XML tag provided!"));
                        EVSEId            = EVSE_Id.           Parse(RemoteStopXML.ElementValueOrFail   (OICPNS.Authorization + "EVSEID",           "No EVSEID XML tag provided!"));

                    }
                    catch (Exception e)
                    {

                        Log.Timestamp("Invalid RemoteStopXML: " + e.Message);

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


                    var Response = RemoteStopResult.Error;

                    var OnRemoteStopLocal = OnRemoteStop;
                    if (OnRemoteStopLocal != null)
                        Response = OnRemoteStopLocal(DateTime.Now,
                                                     RoamingNetworkId,
                                                     SessionId,
                                                     PartnerSessionId,
                                                     ProviderId,
                                                     EVSEId);

                    Log.WriteLine(Response.ToString());

                    switch (Response)
                    {

                        //case RemoteStopResult.EVSE_AlreadyInUse:
                        //    HubjectCode         = "602";
                        //    HubjectDescription  = "EVSE is already in use!";
                        //    break;

                        //case RemoteStopResult.SessionId_AlreadyInUse:
                        //    HubjectCode         = "400";
                        //    HubjectDescription  = "Session is invalid";
                        //    break;

                        //case RemoteStopResult.EVSE_NotReachable:
                        //    HubjectCode         = "501";
                        //    HubjectDescription  = "Communication to EVSE failed!";
                        //    break;

                        //case RemoteStopResult.Start_Timeout:
                        //    HubjectCode         = "501";
                        //    HubjectDescription  = "Communication to EVSE failed!";
                        //    break;

                        case RemoteStopResult.Success:
                            HubjectCode         = "000";
                            HubjectDescription  = "Ready to charge!";
                            break;

                        default:
                            HubjectCode         = "000";
                            break;

                    }

                    var SOAPContent = SOAP.Encapsulation(new XElement(OICPNS.CommonTypes + "eRoamingAcknowledgement",

                                                             new XElement(OICPNS.CommonTypes + "Result", "true"),

                                                             new XElement(OICPNS.CommonTypes + "StatusCode",
                                                                 new XElement(OICPNS.CommonTypes + "Code",            HubjectCode),
                                                                 new XElement(OICPNS.CommonTypes + "Description",     HubjectDescription),
                                                                 new XElement(OICPNS.CommonTypes + "AdditionalInfo",  HubjectAdditionalInfo)
                                                             ),

                                                             new XElement(OICPNS.CommonTypes + "SessionID", SessionId)
                            //new XElement(NS.OICPv1_2CommonTypes + "PartnerSessionID", SessionID),

                                                         )).ToString();

                    return new HTTPResponseBuilder() {
                        HTTPStatusCode  = HTTPStatusCode.OK,
                        ContentType     = HTTPContentType.XMLTEXT_UTF8,
                        Content         = SOAPContent.ToUTF8Bytes()
                    };

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

            this.AddMethodCallback(HTTPMethod.GET,
                                   "/RNs/{RoamingNetworkId}/RemoteStartStop",
                                   HTTPContentType.XMLTEXT_UTF8,
                                   HTTPDelegate: RemoteStartStopDelegate);

            #endregion

            #region Register SOAP-XML Request via POST

            this.AddMethodCallback(HTTPMethod.POST,
                                   "/RNs/{RoamingNetwork}/RemoteStartStop",
                                   HTTPContentType.XMLTEXT_UTF8,
                                   HTTPDelegate: RemoteStartStopDelegate);

            #endregion

            #region Register HTML+Plaintext ErrorResponse

            // HTML
            this.AddMethodCallback(HTTPMethod.GET,
                                   "/RNs/{RoamingNetwork}/RemoteStartStop",
                                   HTTPContentType.HTML_UTF8,
                                   HTTPDelegate: HTTPRequest => {

                                       var RoamingNetworkId = HTTPRequest.ParsedURIParameters[0];

                                       return new HTTPResponseBuilder() {
                                           HTTPStatusCode  = HTTPStatusCode.BadGateway,
                                           ContentType     = HTTPContentType.HTML_UTF8,
                                           Content         = ("/RNs/" + RoamingNetworkId + "/RemoteStartStop is a HTTP/SOAP/XML endpoint!").ToUTF8Bytes(),
                                           Connection      = "close"
                                       };

                                   });

            // Text
            this.AddMethodCallback(HTTPMethod.GET,
                                   "/RNs/{RoamingNetwork}/RemoteStartStop",
                                   HTTPContentType.TEXT_UTF8,
                                   HTTPDelegate: HTTPRequest => {

                                       var RoamingNetworkId = HTTPRequest.ParsedURIParameters[0];

                                       return new HTTPResponseBuilder() {
                                           HTTPStatusCode  = HTTPStatusCode.BadGateway,
                                           ContentType     = HTTPContentType.HTML_UTF8,
                                           Content         = ("/RNs/" + RoamingNetworkId + "/RemoteStartStop is a HTTP/SOAP/XML endpoint!").ToUTF8Bytes(),
                                           Connection      = "close"
                                       };

                                   });

            #endregion

            #endregion

            this.Start();

        }

        #endregion

    }

}
