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


        #region POST_RemoteStartStop()

        public override HTTPResponse POST_RemoteStartStop()
        {

            #region ParseXMLRequestBody

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
                return DoRemoteStartXML(RemoteStartXML);

            else if (RemoteStopXML != null)
                return DoRemoteStopXML(RemoteStopXML);

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
            String   eMAId;

            try
            {

                SessionId                = RemoteStartXML.ElementOrDefault(NS.OICPv1Authorization + "SessionID", "");
                ProviderId               = RemoteStartXML.ElementOrDefault(NS.OICPv1Authorization + "ProviderID", "");
                EVSEId                   = EVSE_Id.Parse(RemoteStartXML.ElementOrDefault(NS.OICPv1Authorization + "EVSEID", ""));

                IdentificationXML        = RemoteStartXML.         Element         (NS.OICPv1Authorization + "Identification");
                QRCodeIdentificationXML  = IdentificationXML.      Element         (NS.OICPv1CommonTypes   + "QRCodeIdentification");
                eMAId                    = QRCodeIdentificationXML.ElementOrDefault(NS.OICPv1CommonTypes   + "EVCOID", "");

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

            //Log.Timestamp("Hubject downstream request:");
            //Log.WriteLine("RemoteStart for EVSE '" + EVSEId + "' for provider/user '" + ProviderId + " / " + EVCOId + "'");
            //Log.WriteLine("Hubject SessionID: '" + SessionId + "'");
            //Log.WriteLine("");

            InternalHTTPServer.FrontendHTTPServer.EventSource(Semantics.DebugLog).
                SubmitSubEvent("REMOTESTARTRequest",
                               new JObject(
                                   new JProperty("Timestamp",   DateTime.Now.ToIso8601()),
                                   new JProperty("SessionId",   SessionId),
                                   new JProperty("ProviderId",  ProviderId),
                                   new JProperty("EVSEId",      EVSEId.ToString()),
                                   new JProperty("eMAId",       eMAId.ToString())
                               ).ToString().
                                 Replace(Environment.NewLine, ""));

            var SOAPContent = "";

            var RequestPDU  = "";
            var ResponsePDU = "";

            try
            {

                var DNSClient      = new DNSClient();
                var IPv4Addresses  = DNSClient.Query<A>("portal.belectric-drive.de").Select(a => a.IPv4Address).ToArray();

                using (var HTTPClient1 = new HTTPClient(IPv4Addresses.First(), new IPPort(20080)))//, "portal.belectric-drive.de"))
                {

                    var HTTPRequestBuilder = HTTPClient1.CreateRequest(new HTTPMethod("REMOTESTART"), "/ps/rest/hubject/RNs/QA1/EVSEs/" + EVSEId.ToString().Replace("+", ""));
                    HTTPRequestBuilder.Host = "portal.belectric-drive.de";
                    HTTPRequestBuilder.ContentType  = HTTPContentType.JSON_UTF8;
                    HTTPRequestBuilder.Accept.Add(HTTPContentType.JSON_UTF8);

                    var JSON = new JObject(new JProperty("@context",   "http://emi3group.org/contexts/REMOTESTART-request.jsonld"),
                                           new JProperty("@id",        SessionId),
                                           new JProperty("ProviderId", ProviderId),
                                           new JProperty("eMAId",      eMAId)).ToString();

                    HTTPRequestBuilder.Content = JSON.ToUTF8Bytes();

                    RequestPDU = HTTPRequestBuilder.AsImmutable().EntirePDU;

                    var Task01 = HTTPClient1.Execute_Synced(HTTPRequestBuilder, Timeout: 60000);

                    ResponsePDU = Task01.EntirePDU;

                    // HTTP/1.1 200 OK
                    // Date: Fri, 28 Mar 2014 13:31:27 GMT
                    // Server: Apache/2.2.9 (Debian) mod_jk/1.2.26
                    // Content-Length: 34
                    // Content-Type: application/json
                    // 
                    // {
                    //   "code" : "EVSE_AlreadyInUse"
                    // }

                    JObject  JSONResponse = null;

                    var ReturnCode             = "";
                    var HubjectCode            = "";
                    var HubjectDescription     = "";
                    var HubjectAdditionalInfo  = "";

                    try
                    {

                        JSONResponse = JObject.Parse(Task01.Content.ToUTF8String());
                        ReturnCode   = JSONResponse["code"].ToString();

                        switch (ReturnCode)
                        {

                            case "EVSE_AlreadyInUse":
                                HubjectCode         = "602";
                                HubjectDescription  = "EVSE is already in use!";
                                break;

                            case "SessionId_AlreadyInUse":
                                HubjectCode         = "400";
                                HubjectDescription  = "Session is invalid";
                                break;

                            case "EVSE_NotReachable":
                                HubjectCode         = "501";
                                HubjectDescription  = "Communication to EVSE failed!";
                                break;

                            case "Success":
                                HubjectCode         = "000";
                                HubjectDescription  = "Ready to charge!";
                                break;

                            default:
                                HubjectCode         = "000";
                                break;

                        }


                    }
                    catch (Exception)
                    {
                        HubjectCode = "ERROR!";
                    }

                    SOAPContent = SOAP.Encapsulation(new XElement(NS.OICPv1CommonTypes + "HubjectAcknowledgement",

                                                 new XElement(NS.OICPv1CommonTypes + "Result", "true"),

                                                 new XElement(NS.OICPv1CommonTypes + "StatusCode",
                                                     new XElement(NS.OICPv1CommonTypes + "Code",            HubjectCode),
                                                     new XElement(NS.OICPv1CommonTypes + "Description",     HubjectDescription),
                                                     new XElement(NS.OICPv1CommonTypes + "AdditionalInfo",  HubjectAdditionalInfo)
                                                 ),

                                                 new XElement(NS.OICPv1CommonTypes + "SessionID", SessionId)
                        //new XElement(NS.OICPv1CommonTypes + "PartnerSessionID", SessionID),

                                            )).ToString();

              //      Task01.Wait(60000);

                    //ToDo: In case of errors this will not parse!
             //       var AuthStartResult = HubjectAuthorizationStart.Parse(XDocument.Parse(HttpResponse.Content.ToUTF8String()).Root);

                    #region Authorized

                    //if (AuthStartResult.AuthorizationStatus == AuthorizationStatusType.Authorized)

                    //    // <soapenv:Envelope xmlns:cmn     = "http://www.hubject.com/b2b/services/commontypes/v1"
                    //    //                   xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                    //    //                   xmlns:tns     = "http://www.hubject.com/b2b/services/authorization/v1">
                    //    //   <soapenv:Body>
                    //    //     <tns:HubjectAuthorizationStart>
                    //    //       <tns:SessionID>8fade8bd-0a88-1296-0f2f-41ae8a80af1b</tns:SessionID>
                    //    //       <tns:PartnerSessionID>0815</tns:PartnerSessionID>
                    //    //       <tns:ProviderID>BMW</tns:ProviderID>
                    //    //       <tns:AuthorizationStatus>Authorized</tns:AuthorizationStatus>
                    //    //       <tns:StatusCode>
                    //    //         <cmn:Code>000</cmn:Code>
                    //    //         <cmn:Description>Success</cmn:Description>
                    //    //       </tns:StatusCode>
                    //    //     </tns:HubjectAuthorizationStart>
                    //    //   </soapenv:Body>
                    //    // </soapenv:Envelope>

                    //    return new AUTHSTARTResult(AuthorizatorId) {
                    //                   AuthorizationResult  = AuthorizationResult.Authorized,
                    //                   SessionId            = AuthStartResult.SessionID,
                    //                   PartnerSessionId     = PartnerSessionId,
                    //                   ProviderId           = EVServiceProvider_Id.Parse(AuthStartResult.ProviderID),
                    //                   Description          = AuthStartResult.Description
                    //               };

                    //#endregion

                    //#region NotAuthorized

                    //else // AuthorizationStatus == AuthorizationStatusType.NotAuthorized
                    //{

                    //    //- Invalid OperatorId ----------------------------------------------------------------------

                    //    // <isns:Envelope xmlns:fn   = "http://www.w3.org/2005/xpath-functions"
                    //    //                xmlns:isns = "http://schemas.xmlsoap.org/soap/envelope/"
                    //    //                xmlns:v1   = "http://www.hubject.com/b2b/services/commontypes/v1"
                    //    //                xmlns:wsc  = "http://www.hubject.com/b2b/services/authorization/v1">
                    //    //   <isns:Body>
                    //    //     <wsc:HubjectAuthorizationStop>
                    //    //       <wsc:SessionID>8f9cbd74-0a88-1296-1078-6e9cca762de2</wsc:SessionID>
                    //    //       <wsc:PartnerSessionID>0815</wsc:PartnerSessionID>
                    //    //       <wsc:AuthorizationStatus>NotAuthorized</wsc:AuthorizationStatus>
                    //    //       <wsc:StatusCode>
                    //    //         <v1:Code>017</v1:Code>
                    //    //         <v1:Description>Unauthorized Access</v1:Description>
                    //    //         <v1:AdditionalInfo>The identification criterion for the provider/operator with the ID "812" doesn't match the given identification information "/C=DE/ST=Bayern/L=Kitzingen/O=Hubject/OU=Belectric Drive GmbH/CN=Belectric ITS Software Development/emailAddress=achim.friedland@belectric.com" from the certificate.</v1:AdditionalInfo>
                    //    //       </wsc:StatusCode>
                    //    //     </wsc:HubjectAuthorizationStop>
                    //    //   </isns:Body>
                    //    // </isns:Envelope>

                    //    if (AuthStartResult.Code == 017)
                    //        return new AUTHSTARTResult(AuthorizatorId) {
                    //                   AuthorizationResult  = AuthorizationResult.NotAuthorized,
                    //                   PartnerSessionId     = PartnerSessionId,
                    //                   Description          = AuthStartResult.Description + " - " + AuthStartResult.AdditionalInfo
                    //               };


                    //    //- Invalid UID -----------------------------------------------------------------------------

                    //    // <soapenv:Envelope xmlns:cmn     = "http://www.hubject.com/b2b/services/commontypes/v1"
                    //    //                   xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                    //    //                   xmlns:tns     = "http://www.hubject.com/b2b/services/authorization/v1">
                    //    //   <soapenv:Body>
                    //    //     <tns:HubjectAuthorizationStart>
                    //    //       <tns:PartnerSessionID>0815</tns:PartnerSessionID>
                    //    //       <tns:AuthorizationStatus>NotAuthorized</tns:AuthorizationStatus>
                    //    //       <tns:StatusCode>
                    //    //         <cmn:Code>320</cmn:Code>
                    //    //         <cmn:Description>Service not available</cmn:Description>
                    //    //       </tns:StatusCode>
                    //    //     </tns:HubjectAuthorizationStart>
                    //    //   </soapenv:Body>
                    //    // </soapenv:Envelope>

                    //    else
                    //        return new AUTHSTARTResult(AuthorizatorId) {
                    //                       AuthorizationResult  = AuthorizationResult.NotAuthorized,
                    //                       PartnerSessionId     = PartnerSessionId,
                    //                       Description          = AuthStartResult.Description
                    //                   };

                    //}

                    #endregion

                }

            }

            catch (Exception e)
            {

                //return new AUTHSTARTResult(AuthorizatorId) {
                //               AuthorizationResult  = AuthorizationResult.NotAuthorized,
                //               PartnerSessionId     = PartnerSessionId,
                //               Description          = "An exception occured: " + e.Message
                //           };

            }

            Log.WriteLine("Result:");
            Log.WriteLine(SOAPContent);

            return new HTTPResponseBuilder() {
                HTTPStatusCode  = HTTPStatusCode.OK,
                ContentType     = HTTPContentType.XMLTEXT_UTF8,
                Content         = SOAPContent.ToUTF8Bytes()
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

            String   SessionId;
            String   ProviderId;
            EVSE_Id  EVSEId;

            try
            {

                SessionId   = RemoteStopXML.ElementOrDefault(NS.OICPv1Authorization + "SessionID", "");
                ProviderId  = RemoteStopXML.ElementOrDefault(NS.OICPv1Authorization + "ProviderID", "");
                EVSEId      = EVSE_Id.Parse(RemoteStopXML.ElementOrDefault(NS.OICPv1Authorization + "EVSEID", ""));

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

            //Log.Timestamp("Hubject downstream request:");
            //Log.WriteLine("RemoteStop for EVSE '" + EVSEID + "' by provider '" + ProviderID + "'");
            //Log.WriteLine("Hubject SessionID: '" + SessionID + "'");
            //Log.WriteLine("");

            InternalHTTPServer.FrontendHTTPServer.EventSource(Semantics.DebugLog).
                SubmitSubEvent("REMOTESTOPRequest",
                               new JObject(
                                   new JProperty("Timestamp",   DateTime.Now.ToIso8601()),
                                   new JProperty("SessionId",   SessionId),
                                   new JProperty("ProviderId",  ProviderId),
                                   new JProperty("EVSEId",      EVSEId)
                               ).ToString().
                                 Replace(Environment.NewLine, ""));

            var SOAPContent = "";

            var RequestPDU  = "";
            var ResponsePDU = "";

            try
            {

                var DNSClient      = new DNSClient();
                var IPv4Addresses  = DNSClient.Query<A>("portal.belectric-drive.de").Select(a => a.IPv4Address).ToArray();

                using (var HTTPClient1 = new HTTPClient(IPv4Addresses.First(), new IPPort(20080)))//, "portal.belectric-drive.de"))
                {

                    var HTTPRequestBuilder = HTTPClient1.CreateRequest(new HTTPMethod("REMOTESTOP"), "/ps/rest/hubject/RNs/QA1/EVSEs/" + EVSEId.ToString().Replace("+", ""));
                    HTTPRequestBuilder.Host         = "portal.belectric-drive.de";
                    HTTPRequestBuilder.ContentType  = HTTPContentType.JSON_UTF8;
                    HTTPRequestBuilder.Accept.Add(HTTPContentType.JSON_UTF8);

                    var JSON = new JObject(new JProperty("@context",   "http://emi3group.org/contexts/REMOTESTOP-request.jsonld"),
                                           new JProperty("@id",        SessionId),
                                           new JProperty("ProviderId", ProviderId)).ToString();

                    HTTPRequestBuilder.Content = JSON.ToUTF8Bytes();

                    RequestPDU = HTTPRequestBuilder.AsImmutable().EntirePDU;

                    var Task01 = HTTPClient1.Execute_Synced(HTTPRequestBuilder, Timeout: 60000);

                    ResponsePDU = Task01.EntirePDU;

                    JObject  JSONResponse = null;

                    var ReturnCode             = "";
                    var HubjectCode            = "";
                    var HubjectDescription     = "";
                    var HubjectAdditionalInfo  = "";

                    try
                    {

                        JSONResponse = JObject.Parse(Task01.Content.ToUTF8String());
                        ReturnCode   = JSONResponse["code"].ToString();

                        switch (ReturnCode)
                        {

                            case "EVSE_AlreadyInUse":
                                HubjectCode         = "602";
                                HubjectDescription  = "EVSE is already in use!";
                                break;

                            case "SessionId_AlreadyInUse":
                                HubjectCode         = "400";
                                HubjectDescription  = "Session is invalid";
                                break;

                            case "EVSE_NotReachable":
                                HubjectCode         = "501";
                                HubjectDescription  = "Communication to EVSE failed!";
                                break;

                            case "Success":
                                HubjectCode         = "000";
                                HubjectDescription  = "Ready to charge!";
                                break;

                            default:
                                HubjectCode         = "000";
                                break;

                        }


                    }
                    catch (Exception)
                    {
                        HubjectCode = "ERROR!";
                    }

                    SOAPContent = SOAP.Encapsulation(new XElement(NS.OICPv1CommonTypes + "HubjectAcknowledgement",

                                                 new XElement(NS.OICPv1CommonTypes + "Result", "true"),

                                                 new XElement(NS.OICPv1CommonTypes + "StatusCode",
                                                     new XElement(NS.OICPv1CommonTypes + "Code",            HubjectCode),
                                                     new XElement(NS.OICPv1CommonTypes + "Description",     HubjectDescription),
                                                     new XElement(NS.OICPv1CommonTypes + "AdditionalInfo",  HubjectAdditionalInfo)
                                                 ),

                                                 new XElement(NS.OICPv1CommonTypes + "SessionID", SessionId)
                        //new XElement(NS.OICPv1CommonTypes + "PartnerSessionID", SessionID),

                                            )).ToString();

              //      Task01.Wait(60000);

                    //ToDo: In case of errors this will not parse!
             //       var AuthStartResult = HubjectAuthorizationStart.Parse(XDocument.Parse(HttpResponse.Content.ToUTF8String()).Root);

                    #region Authorized

                    //if (AuthStartResult.AuthorizationStatus == AuthorizationStatusType.Authorized)

                    //    // <soapenv:Envelope xmlns:cmn     = "http://www.hubject.com/b2b/services/commontypes/v1"
                    //    //                   xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                    //    //                   xmlns:tns     = "http://www.hubject.com/b2b/services/authorization/v1">
                    //    //   <soapenv:Body>
                    //    //     <tns:HubjectAuthorizationStart>
                    //    //       <tns:SessionID>8fade8bd-0a88-1296-0f2f-41ae8a80af1b</tns:SessionID>
                    //    //       <tns:PartnerSessionID>0815</tns:PartnerSessionID>
                    //    //       <tns:ProviderID>BMW</tns:ProviderID>
                    //    //       <tns:AuthorizationStatus>Authorized</tns:AuthorizationStatus>
                    //    //       <tns:StatusCode>
                    //    //         <cmn:Code>000</cmn:Code>
                    //    //         <cmn:Description>Success</cmn:Description>
                    //    //       </tns:StatusCode>
                    //    //     </tns:HubjectAuthorizationStart>
                    //    //   </soapenv:Body>
                    //    // </soapenv:Envelope>

                    //    return new AUTHSTARTResult(AuthorizatorId) {
                    //                   AuthorizationResult  = AuthorizationResult.Authorized,
                    //                   SessionId            = AuthStartResult.SessionID,
                    //                   PartnerSessionId     = PartnerSessionId,
                    //                   ProviderId           = EVServiceProvider_Id.Parse(AuthStartResult.ProviderID),
                    //                   Description          = AuthStartResult.Description
                    //               };

                    //#endregion

                    //#region NotAuthorized

                    //else // AuthorizationStatus == AuthorizationStatusType.NotAuthorized
                    //{

                    //    //- Invalid OperatorId ----------------------------------------------------------------------

                    //    // <isns:Envelope xmlns:fn   = "http://www.w3.org/2005/xpath-functions"
                    //    //                xmlns:isns = "http://schemas.xmlsoap.org/soap/envelope/"
                    //    //                xmlns:v1   = "http://www.hubject.com/b2b/services/commontypes/v1"
                    //    //                xmlns:wsc  = "http://www.hubject.com/b2b/services/authorization/v1">
                    //    //   <isns:Body>
                    //    //     <wsc:HubjectAuthorizationStop>
                    //    //       <wsc:SessionID>8f9cbd74-0a88-1296-1078-6e9cca762de2</wsc:SessionID>
                    //    //       <wsc:PartnerSessionID>0815</wsc:PartnerSessionID>
                    //    //       <wsc:AuthorizationStatus>NotAuthorized</wsc:AuthorizationStatus>
                    //    //       <wsc:StatusCode>
                    //    //         <v1:Code>017</v1:Code>
                    //    //         <v1:Description>Unauthorized Access</v1:Description>
                    //    //         <v1:AdditionalInfo>The identification criterion for the provider/operator with the ID "812" doesn't match the given identification information "/C=DE/ST=Bayern/L=Kitzingen/O=Hubject/OU=Belectric Drive GmbH/CN=Belectric ITS Software Development/emailAddress=achim.friedland@belectric.com" from the certificate.</v1:AdditionalInfo>
                    //    //       </wsc:StatusCode>
                    //    //     </wsc:HubjectAuthorizationStop>
                    //    //   </isns:Body>
                    //    // </isns:Envelope>

                    //    if (AuthStartResult.Code == 017)
                    //        return new AUTHSTARTResult(AuthorizatorId) {
                    //                   AuthorizationResult  = AuthorizationResult.NotAuthorized,
                    //                   PartnerSessionId     = PartnerSessionId,
                    //                   Description          = AuthStartResult.Description + " - " + AuthStartResult.AdditionalInfo
                    //               };


                    //    //- Invalid UID -----------------------------------------------------------------------------

                    //    // <soapenv:Envelope xmlns:cmn     = "http://www.hubject.com/b2b/services/commontypes/v1"
                    //    //                   xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                    //    //                   xmlns:tns     = "http://www.hubject.com/b2b/services/authorization/v1">
                    //    //   <soapenv:Body>
                    //    //     <tns:HubjectAuthorizationStart>
                    //    //       <tns:PartnerSessionID>0815</tns:PartnerSessionID>
                    //    //       <tns:AuthorizationStatus>NotAuthorized</tns:AuthorizationStatus>
                    //    //       <tns:StatusCode>
                    //    //         <cmn:Code>320</cmn:Code>
                    //    //         <cmn:Description>Service not available</cmn:Description>
                    //    //       </tns:StatusCode>
                    //    //     </tns:HubjectAuthorizationStart>
                    //    //   </soapenv:Body>
                    //    // </soapenv:Envelope>

                    //    else
                    //        return new AUTHSTARTResult(AuthorizatorId) {
                    //                       AuthorizationResult  = AuthorizationResult.NotAuthorized,
                    //                       PartnerSessionId     = PartnerSessionId,
                    //                       Description          = AuthStartResult.Description
                    //                   };

                    //}

                    #endregion

                }

            }

            catch (Exception e)
            {

                //return new AUTHSTARTResult(AuthorizatorId) {
                //               AuthorizationResult  = AuthorizationResult.NotAuthorized,
                //               PartnerSessionId     = PartnerSessionId,
                //               Description          = "An exception occured: " + e.Message
                //           };

            }

            Log.WriteLine("Result:");
            Log.WriteLine(SOAPContent);

            return new HTTPResponseBuilder() {
                HTTPStatusCode  = HTTPStatusCode.OK,
                ContentType     = HTTPContentType.XMLTEXT_UTF8,
                Content         = SOAPContent.ToUTF8Bytes()
            };

        }

        #endregion

    }

}
