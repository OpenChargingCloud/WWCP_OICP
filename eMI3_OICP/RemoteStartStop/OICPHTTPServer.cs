/*
 * Copyright (c) 2014 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of eMI3 OICP <http://www.github.com/eMI3/OICP-Bindings>
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

using Newtonsoft.Json.Linq;

using eu.Vanaheimr.Illias.Commons;
using eu.Vanaheimr.Illias.Commons.ConsoleLog;
using eu.Vanaheimr.Hermod;
using eu.Vanaheimr.Hermod.HTTP;

using com.graphdefined.eMI3.LocalService;

#endregion

namespace com.graphdefined.eMI3.IO.OICP
{

    /// <summary>
    /// OICP Downstream HTTP/SOAP server.
    /// </summary>
    public class OICPHTTPServer : HTTPServer
    {

        #region Properties

        #region HTTPRoot

        public String       HTTPRoot            { get; set; }

        #endregion

        #region RequestRouter

        private readonly RequestRouter _RequestRouter;

        public RequestRouter RequestRouter
        {
            get
            {
                return _RequestRouter;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initialize the OICP HTTP server using IPAddress.Any.
        /// </summary>
        /// <param name="RequestRouter">The request router.</param>
        /// <param name="IPPort">The IP listing port.</param>
        public OICPHTTPServer(RequestRouter  RequestRouter,
                              IPPort         IPPort)
        {

            this._RequestRouter = RequestRouter;

            this.AttachTCPPort(IPPort);
            this.Start();

            #region / (HTTPRoot)

            this.AddMethodCallback(HTTPMethod.GET,
                                   "/",
                                   HTTPContentType.HTML_UTF8,
                                   HTTPDelegate: HTTPRequest => {

                                       var RoamingNetworkId = HTTPRequest.ParsedQueryParameters[0];

                                       return new HTTPResponseBuilder() {
                                           HTTPStatusCode  = HTTPStatusCode.BadGateway,
                                           ContentType     = HTTPContentType.HTML_UTF8,
                                           Content         = ("/RNs/{RoamingNetworkId}/RemoteStartStop is a HTTP/SOAP/XML endpoint!").ToUTF8Bytes(),
                                           Connection      = "close"
                                       };

                                   });

            this.AddMethodCallback(HTTPMethod.GET,
                                   "/",
                                   HTTPContentType.TEXT_UTF8,
                                   HTTPDelegate: HTTPRequest => {

                                       var RoamingNetworkId = HTTPRequest.ParsedQueryParameters[0];

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

                var RoamingNetworkId = HTTPRequest.ParsedQueryParameters[0];

                #region ParseXMLRequestBody... or fail!

                var XMLRequest = HTTPRequest.ParseXMLRequestBody();
                if (XMLRequest.HasErrors)
                {

                    Log.WriteLine("XMLRequest.HasErrors!");
                    Log.WriteLine(HTTPRequest.Content.ToUTF8String());

                    GetEventSource(Semantics.DebugLog).
                        SubmitSubEvent("InvalidXMLRequest",
                                       new JObject(
                                           new JProperty("@context",      "http://emi3group.org/contexts/InvalidXMLRequest.jsonld"),
                                           new JProperty("Timestamp",     DateTime.Now.ToIso8601()),
                                           new JProperty("RemoteSocket",  HTTPRequest.RemoteSocket.ToString()),
                                           new JProperty("XMLRequest",    HTTPRequest.Content.ToUTF8String()) //ToDo: Handle errors!
                                       ).ToString().
                                         Replace(Environment.NewLine, ""));

                    return XMLRequest.Error;

                }

                Log.WriteLine("XMLRequest ok!");

                #endregion

                // Log.WriteLine("");
                // Log.Timestamp("Incoming XML request:");
                // Log.WriteLine("XML payload:");
                // Log.WriteLine(XMLRequest.Data.ToString());
                // Log.WriteLine("");

                #region Get SOAP request...

                XElement RemoteStartXML;
                XElement RemoteStopXML;

                try
                {
                    RemoteStartXML = XMLRequest.Data.Root.Descendants(NS.OICPv1Authorization + "HubjectAuthorizeRemoteStart").FirstOrDefault();
                    RemoteStopXML  = XMLRequest.Data.Root.Descendants(NS.OICPv1Authorization + "HubjectAuthorizeRemoteStop"). FirstOrDefault();
                }
                catch (Exception e)
                {

                    //Log.Timestamp("Bad request: " + e.Message);

                    GetEventSource(Semantics.DebugLog).
                        SubmitSubEvent("InvalidXMLRequest",
                                       new JObject(
                                           new JProperty("@context",      "http://emi3group.org/contexts/InvalidXMLRequest.jsonld"),
                                           new JProperty("Timestamp",     DateTime.Now.ToIso8601()),
                                           new JProperty("RemoteSocket",  HTTPRequest.RemoteSocket.ToString()),
                                           new JProperty("Exception",     e.Message),
                                           new JProperty("XMLRequest",    HTTPRequest.Content.ToUTF8String()) //ToDo: Handle errors!
                                       ).ToString().
                                         Replace(Environment.NewLine, ""));

                    return new HTTPResponseBuilder() {

                        HTTPStatusCode = HTTPStatusCode.OK,
                        ContentType    = HTTPContentType.XMLTEXT_UTF8,
                        Content        = SOAP.Encapsulation(new XElement(NS.OICPv1CommonTypes + "HubjectAcknowledgement",

                                                                new XElement(NS.OICPv1CommonTypes + "Result", "false"),

                                                                new XElement(NS.OICPv1CommonTypes + "StatusCode",
                                                                    new XElement(NS.OICPv1CommonTypes + "Code",           "022"),
                                                                    new XElement(NS.OICPv1CommonTypes + "Description",    "Request lead to an exception!"),
                                                                    new XElement(NS.OICPv1CommonTypes + "AdditionalInfo", e.Message)
                                                                )

                                                            )).ToString().ToUTF8Bytes()

                    };

                }

                #endregion

                #region Process an OICP RemoteStart SOAP/XML/HTTP call.

                if (RemoteStartXML != null)
                {

                    #region Parse request parameters

                    // POST /RemoteStartStop HTTP/1.1
                    // Content-type: text/xml;charset=utf-8
                    // Soapaction: ""
                    // Accept: text/xml, multipart/related
                    // User-Agent: JAX-WS RI 2.2-hudson-752-
                    // Cache-Control: no-cache
                    // Pragma: no-cache
                    // Host: 80.148.29.35:3001
                    // Connection: keep-alive
                    // Content-Length: 794
                    // 
                    // <?xml version='1.0' encoding='UTF-8'?>
                    // <isns:Envelope xmlns:cmn     = "http://www.hubject.com/b2b/services/commontypes/v1"
                    //                xmlns:isns    = "http://schemas.xmlsoap.org/soap/envelope/"
                    //                xmlns:sbp     = "http://www.inubit.com/eMobility/SBP"
                    //                xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                    //                xmlns:v1      = "http://www.hubject.com/b2b/services/mobileauthorization/v1"
                    //                xmlns:wsc     = "http://www.hubject.com/b2b/services/authorization/v1">
                    // 
                    //   <isns:Body>
                    //     <wsc:HubjectAuthorizeRemoteStart>
                    //       <wsc:SessionID>5c24515b-0a88-1296-32ea-1226ce8a3cd0</wsc:SessionID>
                    //       <wsc:ProviderID>ICE</wsc:ProviderID>
                    //       <wsc:EVSEID>+49*822*4201*1</wsc:EVSEID>
                    //       <wsc:Identification>
                    //         <cmn:QRCodeIdentification>
                    //           <cmn:EVCOID>DE*ICE*I00811*1</cmn:EVCOID>
                    //         </cmn:QRCodeIdentification>
                    //       </wsc:Identification>
                    //     </wsc:HubjectAuthorizeRemoteStart>
                    //   </isns:Body>
                    // 
                    // </isns:Envelope>


                    String                SessionId;
                    EVServiceProvider_Id  ProviderId;
                    EVSE_Id               EVSEId;
                    XElement              IdentificationXML;
                    XElement              QRCodeIdentificationXML;
                    eMA_Id                eMAId;

                    try
                    {

                        SessionId                = RemoteStartXML.    ElementOrDefault(NS.OICPv1Authorization + "SessionID", "");
                        ProviderId               = EVServiceProvider_Id.Parse(RemoteStartXML.ElementOrDefault(NS.OICPv1Authorization + "ProviderID", ""));
                        EVSEId                   = EVSE_Id.             Parse(RemoteStartXML.ElementOrDefault(NS.OICPv1Authorization + "EVSEID", ""));

                        IdentificationXML        = RemoteStartXML.    Element         (NS.OICPv1Authorization + "Identification");
                        QRCodeIdentificationXML  = IdentificationXML. Element         (NS.OICPv1CommonTypes   + "QRCodeIdentification");
                        eMAId                    = eMA_Id.              Parse(QRCodeIdentificationXML.ElementOrDefault(NS.OICPv1CommonTypes   + "EVCOID", ""));

                    }
                    catch (Exception e)
                    {

                        Log.Timestamp("Bad request: " + e.Message);

                        return new HTTPResponseBuilder() {

                                HTTPStatusCode  = HTTPStatusCode.OK,
                                ContentType     = HTTPContentType.XMLTEXT_UTF8,
                                Content         = SOAP.Encapsulation(new XElement(NS.OICPv1CommonTypes + "HubjectAcknowledgement",

                                                                         new XElement(NS.OICPv1CommonTypes + "Result", "false"),

                                                                         new XElement(NS.OICPv1CommonTypes + "StatusCode",
                                                                             new XElement(NS.OICPv1CommonTypes + "Code",           "022"),
                                                                             new XElement(NS.OICPv1CommonTypes + "Description",    "Request lead to an exception!"),
                                                                             new XElement(NS.OICPv1CommonTypes + "AdditionalInfo",  e.Message)
                                                                         )

                                                                     )).ToString().ToUTF8Bytes()

                        };

                    }

                    #endregion

                    var HubjectCode            = "";
                    var HubjectDescription     = "";
                    var HubjectAdditionalInfo  = "";

                    var Response               = RequestRouter.RemoteStart(EVSEId, SessionId, ProviderId, eMAId);
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
                        Content         = SOAP.Encapsulation(new XElement(NS.OICPv1CommonTypes + "HubjectAcknowledgement",

                                                                 new XElement(NS.OICPv1CommonTypes + "Result", "true"),

                                                                 new XElement(NS.OICPv1CommonTypes + "StatusCode",
                                                                     new XElement(NS.OICPv1CommonTypes + "Code",            HubjectCode),
                                                                     new XElement(NS.OICPv1CommonTypes + "Description",     HubjectDescription),
                                                                     new XElement(NS.OICPv1CommonTypes + "AdditionalInfo",  HubjectAdditionalInfo)
                                                                 ),

                                                                 new XElement(NS.OICPv1CommonTypes + "SessionID", SessionId)
                                                                 //new XElement(NS.OICPv1CommonTypes + "PartnerSessionID", SessionID),

                                                            )).ToString().
                                                               ToUTF8Bytes()
                    };

                    #endregion

                }

                #endregion

                #region Process an OICP RemoteStop SOAP/XML/HTTP call.

                else if (RemoteStopXML != null)
                {

                    #region Parse request parameters

                    // POST /RemoteStartStop HTTP/1.1
                    // Content-type: text/xml;charset=utf-8
                    // Soapaction: ""
                    // Accept: text/xml, multipart/related
                    // User-Agent: JAX-WS RI 2.2-hudson-752-
                    // Cache-Control: no-cache
                    // Pragma: no-cache
                    // Host: 80.148.29.35:3001
                    // Connection: keep-alive
                    // Content-Length: 794
                    //
                    // <?xml version='1.0' encoding='UTF-8'?>
                    // <isns:Envelope xmlns:isns    = "http://schemas.xmlsoap.org/soap/envelope/" 
                    //                xmlns:sbp     = "http://www.inubit.com/eMobility/SBP"
                    //                xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                    //                xmlns:v1      = "http://www.hubject.com/b2b/services/mobileauthorization/v1"
                    //                xmlns:wsc     = "http://www.hubject.com/b2b/services/authorization/v1">
                    // 
                    //   <isns:Body>
                    //     <wsc:HubjectAuthorizeRemoteStop>
                    //       <wsc:SessionID>94407ee4-0a88-1295-13fe-d5e439c3381c</wsc:SessionID>
                    //       <wsc:ProviderID>ICE</wsc:ProviderID>
                    //       <wsc:EVSEID>+49*822*4201*1</wsc:EVSEID>
                    //     </wsc:HubjectAuthorizeRemoteStop>
                    //   </isns:Body>
                    //
                    // </isns:Envelope>


                    String                SessionId;
                    EVServiceProvider_Id  ProviderId;
                    EVSE_Id               EVSEId;

                    try
                    {

                        SessionId   = RemoteStopXML.ElementOrDefault(NS.OICPv1Authorization + "SessionID", "");
                        ProviderId  = EVServiceProvider_Id.Parse(RemoteStopXML.ElementOrDefault(NS.OICPv1Authorization + "ProviderID", ""));
                        EVSEId      = EVSE_Id.             Parse(RemoteStopXML.ElementOrDefault(NS.OICPv1Authorization + "EVSEID",     ""));

                    }
                    catch (Exception e)
                    {

                        Log.Timestamp("Bad request: " + e.Message);

                        return new HTTPResponseBuilder() {

                                HTTPStatusCode  = HTTPStatusCode.OK,
                                ContentType     = HTTPContentType.XMLTEXT_UTF8,
                                Content         = SOAP.Encapsulation(new XElement(NS.OICPv1CommonTypes + "HubjectAcknowledgement",

                                                                         new XElement(NS.OICPv1CommonTypes + "Result", "false"),

                                                                         new XElement(NS.OICPv1CommonTypes + "StatusCode",
                                                                             new XElement(NS.OICPv1CommonTypes + "Code",           "022"),
                                                                             new XElement(NS.OICPv1CommonTypes + "Description",    "Request lead to an exception!"),
                                                                             new XElement(NS.OICPv1CommonTypes + "AdditionalInfo", e.Message)
                                                                         )

                                                                     )).ToString().ToUTF8Bytes()

                        };

                    }

                    #endregion

                    var HubjectCode            = "";
                    var HubjectDescription     = "";
                    var HubjectAdditionalInfo  = "";

                    var Response               = RequestRouter.RemoteStop(EVSEId, SessionId, ProviderId);
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

                    var SOAPContent = SOAP.Encapsulation(new XElement(NS.OICPv1CommonTypes + "HubjectAcknowledgement",

                                                             new XElement(NS.OICPv1CommonTypes + "Result", "true"),

                                                             new XElement(NS.OICPv1CommonTypes + "StatusCode",
                                                                 new XElement(NS.OICPv1CommonTypes + "Code",            HubjectCode),
                                                                 new XElement(NS.OICPv1CommonTypes + "Description",     HubjectDescription),
                                                                 new XElement(NS.OICPv1CommonTypes + "AdditionalInfo",  HubjectAdditionalInfo)
                                                             ),

                                                             new XElement(NS.OICPv1CommonTypes + "SessionID", SessionId)
                            //new XElement(NS.OICPv1CommonTypes + "PartnerSessionID", SessionID),

                                                         )).ToString();

                    return new HTTPResponseBuilder() {
                        HTTPStatusCode  = HTTPStatusCode.OK,
                        ContentType     = HTTPContentType.XMLTEXT_UTF8,
                        Content         = SOAPContent.ToUTF8Bytes()
                    };

                }

                #endregion

                #region ...or fail!

                else
                {

                    //Log.Timestamp("Must be either a RemoteStart or RemoteStop request!");

                    GetEventSource(Semantics.DebugLog).
                        SubmitSubEvent("InvalidXMLRequest",
                                       new JObject(
                                           new JProperty("@context",      "http://emi3group.org/contexts/InvalidXMLRequest.jsonld"),
                                           new JProperty("Timestamp",     DateTime.Now.ToIso8601()),
                                           new JProperty("RemoteSocket",  HTTPRequest.RemoteSocket.ToString()),
                                           new JProperty("Exception",     "Must be either a RemoteStart or RemoteStop request!"),
                                           new JProperty("XMLRequest",    HTTPRequest.Content.ToUTF8String()) //ToDo: Handle errors!
                                       ).ToString().
                                         Replace(Environment.NewLine, ""));

                    return new HTTPResponseBuilder() {

                        HTTPStatusCode = HTTPStatusCode.OK,
                        ContentType    = HTTPContentType.XMLTEXT_UTF8,
                        Content        = SOAP.Encapsulation(new XElement(NS.OICPv1CommonTypes + "HubjectAcknowledgement",

                                                                new XElement(NS.OICPv1CommonTypes + "Result", "false"),

                                                                new XElement(NS.OICPv1CommonTypes + "StatusCode",
                                                                    new XElement(NS.OICPv1CommonTypes + "Code",           "022"),
                                                                    new XElement(NS.OICPv1CommonTypes + "Description",    "Unknown request!"),
                                                                    new XElement(NS.OICPv1CommonTypes + "AdditionalInfo", "Must be either RemoteStart or RemoteStop!")
                                                                )

                                                            )).ToString().ToUTF8Bytes()

                    };

                }

                #endregion

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

            this.AddMethodCallback(HTTPMethod.GET,
                                   "/RNs/{RoamingNetwork}/RemoteStartStop",
                                   HTTPContentType.HTML_UTF8,
                                   HTTPDelegate: HTTPRequest => {

                                       var RoamingNetworkId = HTTPRequest.ParsedQueryParameters[0];

                                       return new HTTPResponseBuilder() {
                                           HTTPStatusCode  = HTTPStatusCode.BadGateway,
                                           ContentType     = HTTPContentType.HTML_UTF8,
                                           Content         = ("/RNs/" + RoamingNetworkId + "/RemoteStartStop is a HTTP/SOAP/XML endpoint!").ToUTF8Bytes(),
                                           Connection      = "close"
                                       };

                                   });

            this.AddMethodCallback(HTTPMethod.GET,
                                   "/RNs/{RoamingNetwork}/RemoteStartStop",
                                   HTTPContentType.TEXT_UTF8,
                                   HTTPDelegate: HTTPRequest => {

                                       var RoamingNetworkId = HTTPRequest.ParsedQueryParameters[0];

                                       return new HTTPResponseBuilder() {
                                           HTTPStatusCode  = HTTPStatusCode.BadGateway,
                                           ContentType     = HTTPContentType.HTML_UTF8,
                                           Content         = ("/RNs/" + RoamingNetworkId + "/RemoteStartStop is a HTTP/SOAP/XML endpoint!").ToUTF8Bytes(),
                                           Connection      = "close"
                                       };

                                   });

            #endregion

            #endregion

        }

        #endregion

    }

}
