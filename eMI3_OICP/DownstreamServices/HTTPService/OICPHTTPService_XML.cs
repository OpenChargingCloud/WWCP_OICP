/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@belectric.com>
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
using System.Text;
using System.Xml.Linq;
using System.Diagnostics;

using eu.Vanaheimr.Illias.Commons;
using eu.Vanaheimr.Hermod.HTTP;
using Newtonsoft.Json.Linq;
using eu.Vanaheimr.Hermod.Services.DNS;
using eu.Vanaheimr.Hermod;
using org.emi3group.LocalService;
//using org.emi3group.HTTP;

#endregion

namespace org.emi3group.IO.OICP
{

    public static class Log
    {

        public static void WriteLine(String Message = null)
        {
            if (Message == null)
                Debug.WriteLine("");
            else
                Debug.WriteLine(Message);
        }

        public static void Timestamp(String Message = null)
        {
            if (Message == null)
                Debug.WriteLine("");
            else
                Debug.WriteLine(DateTime.Now + " - " + Message);
        }


    }

    /// <summary>
    /// XML content representation.
    /// </summary>
    public class OICPHTTPService_XML : AOICPHTTPService
    {

        #region Constructor(s)

        #region OICPHTTPService_XML()

        /// <summary>
        /// XML content representation.
        /// </summary>
        public OICPHTTPService_XML()
            : base(HTTPContentType.XMLTEXT_UTF8)
        { }

        #endregion

        #region OICPHTTPService_XML(IHTTPConnection)

        /// <summary>
        /// XML content representation.
        /// </summary>
        /// <param name="IHTTPConnection">The http connection for this request.</param>
        public OICPHTTPService_XML(IHTTPConnection IHTTPConnection)
            : base(IHTTPConnection, HTTPContentType.XMLTEXT_UTF8)
        { }

        #endregion

        #endregion


        #region POST_RemoteStartStop(RoamingNetwork_Id)

        /// <summary>
        /// Main SOAP/HTTP endpoint.
        /// </summary>
        /// <param name="RoamingNetwork_Id">The unique identification of the roaming network.</param>
        public override HTTPResponse POST_RemoteStartStop(String RoamingNetwork_Id)
        {

            #region ParseXMLRequestBody... or fail!

            var XMLRequest = ParseXMLRequestBody();
            if (XMLRequest.HasErrors)
            {

                InternalHTTPServer.URLMapping.EventSource(Semantics.DebugLog).
                    SubmitSubEvent("InvalidXMLRequest",
                                   new JObject(
                                       new JProperty("@context",      "http://emi3group.org/contexts/InvalidXMLRequest.jsonld"),
                                       new JProperty("Timestamp",     DateTime.Now.ToIso8601()),
                                       new JProperty("RemoteSocket",  IHTTPConnection.RemoteSocket.ToString()),
                                       new JProperty("XMLRequest",    IHTTPConnection.RequestBody.ToUTF8String()) //ToDo: Handle errors!
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

                InternalHTTPServer.URLMapping.EventSource(Semantics.DebugLog).
                    SubmitSubEvent("InvalidXMLRequest",
                                   new JObject(
                                       new JProperty("@context",      "http://emi3group.org/contexts/InvalidXMLRequest.jsonld"),
                                       new JProperty("Timestamp",     DateTime.Now.ToIso8601()),
                                       new JProperty("RemoteSocket",  IHTTPConnection.RemoteSocket.ToString()),
                                       new JProperty("Exception",     e.Message),
                                       new JProperty("XMLRequest",    IHTTPConnection.RequestBody.ToUTF8String()) //ToDo: Handle errors!
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

            if (RemoteStartXML != null)
                return DoRemoteStartXML(RoamingNetwork_Id, RemoteStartXML);

            else if (RemoteStopXML != null)
                return DoRemoteStopXML(RoamingNetwork_Id, RemoteStopXML);

            #region ...or fail!

            else
            {

                //Log.Timestamp("Must be either a RemoteStart or RemoteStop request!");

                InternalHTTPServer.URLMapping.EventSource(Semantics.DebugLog).
                    SubmitSubEvent("InvalidXMLRequest",
                                   new JObject(
                                       new JProperty("@context",      "http://emi3group.org/contexts/InvalidXMLRequest.jsonld"),
                                       new JProperty("Timestamp",     DateTime.Now.ToIso8601()),
                                       new JProperty("RemoteSocket",  IHTTPConnection.RemoteSocket.ToString()),
                                       new JProperty("Exception",     "Must be either a RemoteStart or RemoteStop request!"),
                                       new JProperty("XMLRequest",    IHTTPConnection.RequestBody.ToUTF8String()) //ToDo: Handle errors!
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

        }

        #endregion


        #region (private) DoRemoteStartXML(RoamingNetwork_Id, RemoteStartXML)

        /// <summary>
        /// Process an OICP RemoteStart SOAP/XML/HTTP call.
        /// </summary>
        /// <param name="RoamingNetwork_Id">The unique identification of the roaming network.</param>
        /// <param name="RemoteStartXML">The SOAP/XML PDU.</param>
        /// <returns>An appropriate SOAP/XML/HTTP response.</returns>
        private HTTPResponse DoRemoteStartXML(String    RoamingNetwork_Id,
                                              XElement  RemoteStartXML)
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

            switch (InternalHTTPServer.RequestRouter.RemoteStart(EVSEId, SessionId, ProviderId, eMAId))
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

        }

        #endregion

        #region (private) DoRemoteStopXML(RoamingNetwork_Id, RemoteStopXML)

        /// <summary>
        /// Process an OICP RemoteStop SOAP/XML/HTTP call.
        /// </summary>
        /// <param name="RoamingNetwork_Id">The unique identification of the roaming network.</param>
        /// <param name="RemoteStopXML">The SOAP/XML PDU.</param>
        /// <returns>An appropriate SOAP/XML/HTTP response.</returns>
        private HTTPResponse DoRemoteStopXML(String    RoamingNetwork_Id,
                                             XElement  RemoteStopXML)
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

            switch (InternalHTTPServer.RequestRouter.RemoteStop(EVSEId, SessionId, ProviderId))
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

    }

}
