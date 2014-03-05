/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@belectric.com>
 * This file is part of eMI3 HTTP <http://www.github.com/eMI3/HTTP>
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
using System.Diagnostics;

using eu.Vanaheimr.Illias.Commons;
using eu.Vanaheimr.Hermod.HTTP;
using Newtonsoft.Json.Linq;
using org.emi3group.HTTP;

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

        #region (private) ParseRequestBody()

        private HTTPResult<XDocument> ParseRequestBody()
        {

            var RequestBodyString = GetRequestBodyAsUTF8String(HTTPContentType.XMLTEXT_UTF8);
            if (RequestBodyString.HasErrors)
                return new HTTPResult<XDocument>(RequestBodyString.Error);


            // Parse XML string
            XDocument RequestBodyXML;
            try
            {
                RequestBodyXML = XDocument.Parse(RequestBodyString.Data);
            }
            catch (Exception)
            {
                return new HTTPResult<XDocument>(IHTTPConnection.RequestHeader, HTTPStatusCode.BadRequest);
            }

            return new HTTPResult<XDocument>(RequestBodyXML);

        }

        #endregion


        #region POST_RemoteStartStop()

        public override HTTPResponse POST_RemoteStartStop()
        {

            var XMLRequest = ParseRequestBody();
            if (XMLRequest.HasErrors)
                return XMLRequest.Error;

            Log.WriteLine("");
            Log.Timestamp("Incoming XML request:");
            Log.WriteLine("XML payload:");
            Log.WriteLine(XMLRequest.Data.ToString());
            Log.WriteLine("");

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

                Log.Timestamp("Bad request: " + e.Message);

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
                return DoRemoteStartXML(RemoteStartXML);

            else if (RemoteStopXML != null)
                return DoRemoteStopXML(RemoteStopXML);

            #region ...or fail!

            else
            {

                Log.Timestamp("Must be either RemoteStart or RemoteStop!");

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


        #region (private) DoRemoteStartXML(XElement RemoteStartXML)

        private HTTPResponse DoRemoteStartXML(XElement RemoteStartXML)
        {

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

            #region Parse request parameters

            String   SessionId;
            String   ProviderId;
            EVSE_Id  EVSEId;
            XElement IdentificationXML;
            XElement QRCodeIdentificationXML;
            String   EVCOId;

            try
            {

                SessionId                = RemoteStartXML.ElementOrDefault(NS.OICPv1Authorization + "SessionID", "");
                ProviderId               = RemoteStartXML.ElementOrDefault(NS.OICPv1Authorization + "ProviderID", "");
                EVSEId                   = EVSE_Id.Parse(RemoteStartXML.ElementOrDefault(NS.OICPv1Authorization + "EVSEID", ""));

                IdentificationXML        = RemoteStartXML.         Element         (NS.OICPv1Authorization + "Identification");
                QRCodeIdentificationXML  = IdentificationXML.      Element         (NS.OICPv1CommonTypes   + "QRCodeIdentification");
                EVCOId                   = QRCodeIdentificationXML.ElementOrDefault(NS.OICPv1CommonTypes   + "EVCOID", "");

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

            Log.Timestamp("Hubject downstream request:");
            Log.WriteLine("RemoteStart for EVSE '" + EVSEId + "' for provider/user '" + ProviderId + " / " + EVCOId + "'");
            Log.WriteLine("Hubject SessionID: '" + SessionId + "'");
            Log.WriteLine("");

            InternalHTTPServer.eMI3_HTTPServer.URLMapping.EventSource(Semantics.DebugLog).
                SubmitSubEvent("REMOTESTARTRequest",
                               new JObject(
                                   new JProperty("Timestamp",   DateTime.Now.ToIso8601()),
                                   new JProperty("SessionId",   SessionId),
                                   new JProperty("ProviderId",  ProviderId),
                                   new JProperty("EVSEId",      EVSEId),
                                   new JProperty("EVCOId",      EVCOId)
                               ).ToString().
                                 Replace(Environment.NewLine, ""));


            var Content = SOAP.Encapsulation(new XElement(NS.OICPv1CommonTypes + "HubjectAcknowledgement",

                                                 new XElement(NS.OICPv1CommonTypes + "Result", "true"),

                                                 new XElement(NS.OICPv1CommonTypes + "StatusCode",
                                                     new XElement(NS.OICPv1CommonTypes + "Code",           "000"),
                                                     new XElement(NS.OICPv1CommonTypes + "Description",    "Out of service!"),
                                                     new XElement(NS.OICPv1CommonTypes + "AdditionalInfo", "Reserved for testing!")
                                                 ),

                                                 new XElement(NS.OICPv1CommonTypes + "SessionID",        SessionId)
                                                 //new XElement(NS.OICPv1CommonTypes + "PartnerSessionID", SessionID),

                                            )).ToString();

            Log.WriteLine("Result:");
            Log.WriteLine(Content);

            return new HTTPResponseBuilder() {
                HTTPStatusCode  = HTTPStatusCode.OK,
                ContentType     = HTTPContentType.XMLTEXT_UTF8,
                Content         = Content.ToUTF8Bytes()
            };

        }

        #endregion

        #region (private) DoRemoteStopXML(RemoteStopXML)

        private HTTPResponse DoRemoteStopXML(XElement RemoteStopXML)
        {

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

            String   SessionID;
            String   ProviderID;
            EVSE_Id  EVSEID;

            try
            {

                SessionID   = RemoteStopXML.ElementOrDefault(NS.OICPv1Authorization + "SessionID", "");
                ProviderID  = RemoteStopXML.ElementOrDefault(NS.OICPv1Authorization + "ProviderID", "");
                EVSEID      = EVSE_Id.Parse(RemoteStopXML.ElementOrDefault(NS.OICPv1Authorization + "EVSEID", ""));

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

            Log.Timestamp("Hubject downstream request:");
            Log.WriteLine("RemoteStop for EVSE '" + EVSEID + "' by provider '" + ProviderID + "'");
            Log.WriteLine("Hubject SessionID: '" + SessionID + "'");
            Log.WriteLine("");

            var Content = SOAP.Encapsulation(new XElement(NS.OICPv1CommonTypes + "HubjectAcknowledgement",

                                                 new XElement(NS.OICPv1CommonTypes + "Result", "true"),

                                                 new XElement(NS.OICPv1CommonTypes + "StatusCode",
                                                     new XElement(NS.OICPv1CommonTypes + "Code",           "000"),
                                                     new XElement(NS.OICPv1CommonTypes + "Description",    "Out of service!"),
                                                     new XElement(NS.OICPv1CommonTypes + "AdditionalInfo", "Reserved for testing!")
                                                 ),

                                                 new XElement(NS.OICPv1CommonTypes + "SessionID",        SessionID)
                                                 //new XElement(NS.OICPv1CommonTypes + "PartnerSessionID", SessionID),

                                            )).ToString();

            Log.WriteLine("Result:");
            Log.WriteLine(Content);

            return new HTTPResponseBuilder() {
                HTTPStatusCode  = HTTPStatusCode.OK,
                ContentType     = HTTPContentType.XMLTEXT_UTF8,
                Content         = Content.ToUTF8Bytes()
            };

        }

        #endregion

    }

}
