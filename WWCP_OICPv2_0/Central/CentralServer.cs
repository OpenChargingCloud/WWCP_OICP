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
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.DNS;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_0
{

    /// <summary>
    /// OICP v2.0 HTTP/SOAP central server.
    /// </summary>
    public class CentralServer : HTTPServer
    {

        #region Data

        private readonly Dictionary<EVSE_Id, EVSEDataRecord> _EVSEDataRecords;

        #endregion

        #region Properties

        #region RoamingNetwork

        private readonly RoamingNetwork _RoamingNetwork;

        public RoamingNetwork RoamingNetwork
        {
            get
            {
                return _RoamingNetwork;
            }
        }

        #endregion

        #region URIPrefix

        protected readonly String _URIPrefix;

        public String URIPrefix
        {
            get
            {
                return _URIPrefix;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initialize the OICP v2.0 HTTP/SOAP central server using IPAddress.Any.
        /// </summary>
        /// <param name="RoamingNetwork">The roaming network to use.</param>
        /// <param name="IPPort">The TCP listing port of the HTTP/SOAP server.</param>
        /// <param name="URIPrefix">The URI prefix for the  HTTP/SOAP server.</param>
        /// <param name="RegisterHTTPRootService">Whether to register a simple webpage for '/', or not.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public CentralServer(RoamingNetwork  RoamingNetwork,
                             IPPort          IPPort,
                             String          URIPrefix                = "",
                             Boolean         RegisterHTTPRootService  = true,
                             DNSClient       DNSClient                = null)

            : base(//IPPort, //Note: Use AttachTCPPort(...) instead!
                   DNSClient: DNSClient)

        {

            this._RoamingNetwork   = RoamingNetwork;
            this._URIPrefix        = URIPrefix;
            this._EVSEDataRecords  = new Dictionary<EVSE_Id, EVSEDataRecord>();

            this.AttachTCPPort(IPPort);
            this.Start();

            #region / (HTTPRoot), if RegisterHTTPRootService == true

            if (RegisterHTTPRootService)
            {

                // HTML
                this.AddMethodCallback(HTTPMethod.GET,
                                       "/",
                                       HTTPContentType.HTML_UTF8,
                                       HTTPDelegate: Request => {

                                           var RoamingNetworkId = Request.ParsedURIParameters[0];

                                           return new HTTPResponseBuilder(Request) {
                                               HTTPStatusCode  = HTTPStatusCode.BadGateway,
                                               ContentType     = HTTPContentType.HTML_UTF8,
                                               Content         = ("/RNs/{RoamingNetworkId} is a HTTP/SOAP/XML endpoint!").ToUTF8Bytes(),
                                               Connection      = "close"
                                           };

                                       });

                // Text
                this.AddMethodCallback(HTTPMethod.GET,
                                       "/",
                                       HTTPContentType.TEXT_UTF8,
                                       HTTPDelegate: Request => {

                                           var RoamingNetworkId = Request.ParsedURIParameters[0];

                                           return new HTTPResponseBuilder(Request) {
                                               HTTPStatusCode  = HTTPStatusCode.BadGateway,
                                               ContentType     = HTTPContentType.HTML_UTF8,
                                               Content         = ("/RNs/{RoamingNetworkId} is a HTTP/SOAP/XML endpoint!").ToUTF8Bytes(),
                                               Connection      = "close"
                                           };

                                       });

            }

            #endregion

            #region /RNs/{RoamingNetworkId}

            #region Generic OICPServerDelegate

            HTTPDelegate OICPServerDelegate = Request => {

                //var _EventTrackingId = EventTracking_Id.New;
                //Log.WriteLine("Event tracking: " + _EventTrackingId);

                #region Parse XML request body... or fail!

                var XMLRequest = Request.ParseXMLRequestBody();
                if (XMLRequest.HasErrors)
                {

                    //Log.WriteLine("Invalid XML request!");
                    //Log.WriteLine(HTTPRequest.Content.ToUTF8String());

                    GetEventSource(Semantics.DebugLog).
                        SubmitSubEvent("InvalidXMLRequest",
                                       new JObject(
                                           new JProperty("@context",      "http://wwcp.graphdefined.org/contexts/InvalidXMLRequest.jsonld"),
                                           new JProperty("Timestamp",     DateTime.Now.ToIso8601()),
                                           new JProperty("RemoteSocket",  Request.RemoteSocket.ToString()),
                                           new JProperty("XMLRequest",    Request.HTTPBody.ToUTF8String()) //ToDo: Handle errors!
                                       ).ToString().
                                         Replace(Environment.NewLine, ""));

                    return XMLRequest.Error;

                }

                #endregion

                //ToDo: Check SOAP header/body XML tags!

                try
                {

                    #region Get and verify XML/SOAP request...

                    IEnumerable<XElement> PushEVSEDataXMLs;
                    IEnumerable<XElement> PullEVSEDataXMLs;
                    IEnumerable<XElement> GetEVSEByIdXMLs;

                    IEnumerable<XElement> PushEVSEStatusXMLs;
                    IEnumerable<XElement> PullEVSEStatusXMLs;
                    IEnumerable<XElement> PullEVSEStatusByIdXMLs;


                    // EvseDataBinding
                    PushEVSEDataXMLs        = XMLRequest.Data.Root.Descendants(OICPNS.EVSEData   + "eRoamingPushEvseData");
                    PullEVSEDataXMLs        = XMLRequest.Data.Root.Descendants(OICPNS.EVSEData   + "eRoamingPullEvseData");
                    GetEVSEByIdXMLs         = XMLRequest.Data.Root.Descendants(OICPNS.EVSEData   + "eRoamingGetEvseById");

                    // EvseStatusBinding
                    PushEVSEStatusXMLs      = XMLRequest.Data.Root.Descendants(OICPNS.EVSEStatus + "eRoamingPushEvseStatus");
                    PullEVSEStatusXMLs      = XMLRequest.Data.Root.Descendants(OICPNS.EVSEStatus + "eRoamingPullEvseStatus");
                    PullEVSEStatusByIdXMLs  = XMLRequest.Data.Root.Descendants(OICPNS.EVSEStatus + "eRoamingPullEvseStatusById");

                    if (!PushEVSEDataXMLs.      Any() &&
                        !PullEVSEDataXMLs.      Any() &&
                        !GetEVSEByIdXMLs.       Any() &&

                        !PushEVSEStatusXMLs.    Any() &&
                        !PullEVSEStatusXMLs.    Any() &&
                        !PullEVSEStatusByIdXMLs.Any())

                        throw new Exception("Unkown XML/SOAP request!");

                    if (PushEVSEDataXMLs.       Count() > 1)
                        throw new Exception("Multiple PushEvseData XML tags within a single request are not supported!");

                    if (PullEVSEDataXMLs.       Count() > 1)
                        throw new Exception("Multiple PullEVSEData XML tags within a single request are not supported!");

                    if (GetEVSEByIdXMLs.        Count() > 1)
                        throw new Exception("Multiple GetEVSEById XML tags within a single request are not supported!");


                    if (PushEVSEStatusXMLs.     Count() > 1)
                        throw new Exception("Multiple PushEVSEStatus XML tags within a single request are not supported!");

                    if (PullEVSEStatusXMLs.     Count() > 1)
                        throw new Exception("Multiple PullEVSEStatus XML tags within a single request are not supported!");

                    if (PullEVSEStatusByIdXMLs. Count() > 1)
                        throw new Exception("Multiple PullEVSEStatusBy XML tags within a single request are not supported!");

                    #endregion

                    #region PushEVSEData

                    var PushEVSEDataXML = PushEVSEDataXMLs.FirstOrDefault();
                    if (PushEVSEDataXML != null)
                    {

                        #region Parse request parameters

                        var ActionType        = PushEVSEDataXML.ElementValueOrFail(OICPNS.EVSEData + "ActionType", "No ActionType XML tag provided!");
                        var OperatorEvseData  = OperatorEVSEData.Parse(PushEVSEDataXML.ElementsOrFail(OICPNS.EVSEData + "OperatorEvseData", "No OperatorEvseData XML tags provided!"));

                        #endregion

                        #region HTTPResponse

                        return new HTTPResponseBuilder(Request) {
                            HTTPStatusCode  = HTTPStatusCode.OK,
                            ContentType     = HTTPContentType.XMLTEXT_UTF8,
                            Content         = SOAP.Encapsulation(new XElement(OICPNS.CommonTypes + "eRoamingAcknowledgement",

                                                                     new XElement(OICPNS.CommonTypes + "Result", "true"),

                                                                     new XElement(OICPNS.CommonTypes + "StatusCode",
                                                                         new XElement(OICPNS.CommonTypes + "Code",            "000"),
                                                                         new XElement(OICPNS.CommonTypes + "Description",     "Success"),
                                                                         new XElement(OICPNS.CommonTypes + "AdditionalInfo",  "")
                                                                     )

                                                                )).ToUTF8Bytes()
                        };

                        #endregion

                    }

                    #endregion

                    #region PullEVSEData

                    var PullEVSEDataXML = PullEVSEDataXMLs.FirstOrDefault();
                    if (PullEVSEDataXML != null)
                    {
                    }

                    #endregion

                    #region GetEVSEById

                    var GetEVSEByIdXML = GetEVSEByIdXMLs.FirstOrDefault();
                    if (GetEVSEByIdXML != null)
                    {
                    }

                    #endregion


                    #region PushEVSEStatus

                    var PushEVSEStatusXML = PushEVSEStatusXMLs.FirstOrDefault();
                    if (PushEVSEStatusXML != null)
                    {

                        #region Parse request parameters

                        String                  ActionType;
                        IEnumerable<XElement>   OperatorEvseStatusXML;

                        ActionType             = PushEVSEStatusXML.ElementValueOrFail(OICPNS.EVSEStatus + "ActionType",         "No ActionType XML tag provided!");
                        OperatorEvseStatusXML  = PushEVSEStatusXML.ElementsOrFail    (OICPNS.EVSEStatus + "OperatorEvseStatus", "No OperatorEvseStatus XML tags provided!");

                        foreach (var SingleOperatorEvseStatusXML in OperatorEvseStatusXML)
                        {

                            EVSEOperator_Id         OperatorId;
                            String                  OperatorName;
                            IEnumerable<XElement>   EVSEStatusRecordsXML;

                            if (!EVSEOperator_Id.TryParse(SingleOperatorEvseStatusXML.ElementValueOrFail(OICPNS.EVSEStatus + "OperatorID", "No OperatorID XML tag provided!"), out OperatorId))
                                throw new ApplicationException("Invalid OperatorID XML tag provided!");

                            OperatorName          = SingleOperatorEvseStatusXML.ElementValueOrDefault(OICPNS.EVSEStatus + "OperatorName",     "");
                            EVSEStatusRecordsXML  = SingleOperatorEvseStatusXML.ElementsOrFail       (OICPNS.EVSEStatus + "EvseStatusRecord", "No EvseStatusRecord XML tags provided!");

                            foreach (var EVSEStatusRecordXML in EVSEStatusRecordsXML)
                            {

                                EVSE_Id  EVSEId;
                                String   EVSEStatus;

                                if (!EVSE_Id.TryParse(EVSEStatusRecordXML.ElementValueOrFail(OICPNS.EVSEStatus + "EvseId", "No EvseId XML tag provided!"), out EVSEId))
                                    throw new ApplicationException("Invalid EvseId XML tag provided!");

                                EVSEStatus = EVSEStatusRecordXML.ElementValueOrFail(OICPNS.EVSEStatus + "EvseStatus", "No EvseStatus XML tag provided!");

                            }

                        }

                        #endregion

                        #region HTTPResponse

                        return new HTTPResponseBuilder(Request) {
                            HTTPStatusCode  = HTTPStatusCode.OK,
                            ContentType     = HTTPContentType.XMLTEXT_UTF8,
                            Content         = SOAP.Encapsulation(new XElement(OICPNS.CommonTypes + "eRoamingAcknowledgement",

                                                                     new XElement(OICPNS.CommonTypes + "Result", "true"),

                                                                     new XElement(OICPNS.CommonTypes + "StatusCode",
                                                                         new XElement(OICPNS.CommonTypes + "Code",            "000"),
                                                                         new XElement(OICPNS.CommonTypes + "Description",     "Success"),
                                                                         new XElement(OICPNS.CommonTypes + "AdditionalInfo",  "")
                                                                     )

                                                                )).ToUTF8Bytes()
                        };

                        #endregion

                    }

                    #endregion

                    #region PullEVSEStatus

                    var PullEVSEStatusXML = PullEVSEStatusXMLs.FirstOrDefault();
                    if (PullEVSEStatusXML != null)
                    {

                    }

                    #endregion

                    #region PullEVSEStatusById

                    var PullEVSEStatusByIdXML = PullEVSEStatusByIdXMLs.FirstOrDefault();
                    if (PullEVSEStatusByIdXML != null)
                    {

                    }

                    #endregion


                    #region HTTPResponse: Unkown XML/SOAP message

                    return new HTTPResponseBuilder(Request) {
                        HTTPStatusCode  = HTTPStatusCode.OK,
                        ContentType     = HTTPContentType.XMLTEXT_UTF8,
                        Content         = SOAP.Encapsulation(new XElement(OICPNS.CommonTypes + "eRoamingAcknowledgement",

                                                                 new XElement(OICPNS.CommonTypes + "Result", "false"),

                                                                 new XElement(OICPNS.CommonTypes + "StatusCode",
                                                                     new XElement(OICPNS.CommonTypes + "Code",            ""),
                                                                     new XElement(OICPNS.CommonTypes + "Description",     "Unkown XML/SOAP message"),
                                                                     new XElement(OICPNS.CommonTypes + "AdditionalInfo",  "")
                                                                 ),

                                                                 new XElement(OICPNS.CommonTypes + "SessionID", "")
                                                                 //new XElement(NS.OICPv1_2CommonTypes + "PartnerSessionID", SessionID),

                                                            )).ToUTF8Bytes()
                    };

                    #endregion

                }

                #region Catch exceptions...

                catch (Exception e)
                {

                    //Log.WriteLine("Invalid XML request!");

                    GetEventSource(Semantics.DebugLog).
                        SubmitSubEvent("InvalidXMLRequest",
                                       new JObject(
                                           new JProperty("@context",      "http://wwcp.graphdefined.org/contexts/InvalidXMLRequest.jsonld"),
                                           new JProperty("Timestamp",     DateTime.Now.ToIso8601()),
                                           new JProperty("RemoteSocket",  Request.RemoteSocket.ToString()),
                                           new JProperty("Exception",     e.Message),
                                           new JProperty("XMLRequest",    XMLRequest.ToString())
                                       ).ToString().
                                         Replace(Environment.NewLine, ""));

                    return new HTTPResponseBuilder(Request) {

                        HTTPStatusCode = HTTPStatusCode.OK,
                        ContentType    = HTTPContentType.XMLTEXT_UTF8,
                        Content        = SOAP.Encapsulation(new XElement(OICPNS.CommonTypes + "eRoamingAcknowledgement",

                                                                new XElement(OICPNS.CommonTypes + "Result", "false"),

                                                                new XElement(OICPNS.CommonTypes + "StatusCode",
                                                                    new XElement(OICPNS.CommonTypes + "Code",           "022"),
                                                                    new XElement(OICPNS.CommonTypes + "Description",    "Request led to an exception!"),
                                                                    new XElement(OICPNS.CommonTypes + "AdditionalInfo", e.Message)
                                                                )

                                                            )).ToUTF8Bytes()

                    };

                }

                #endregion

            };

            #endregion

            #region Register SOAP-XML Request via GET

            this.AddMethodCallback(HTTPMethod.GET,
                                   _URIPrefix + "/RNs/{RoamingNetworkId}",
                                   HTTPContentType.XMLTEXT_UTF8,
                                   HTTPDelegate: OICPServerDelegate);

            this.AddMethodCallback(HTTPMethod.GET,
                                   _URIPrefix + "/RNs/{RoamingNetworkId}",
                                   HTTPContentType.XML_UTF8,
                                   HTTPDelegate: OICPServerDelegate);

            #endregion

            #region Register SOAP-XML Request via POST

            this.AddMethodCallback(HTTPMethod.POST,
                                   _URIPrefix + "/RNs/{RoamingNetwork}",
                                   HTTPContentType.XMLTEXT_UTF8,
                                   HTTPDelegate: OICPServerDelegate);

            this.AddMethodCallback(HTTPMethod.POST,
                                   _URIPrefix + "/RNs/{RoamingNetwork}",
                                   HTTPContentType.XML_UTF8,
                                   HTTPDelegate: OICPServerDelegate);

            #endregion

            #region Register HTML+Plaintext ErrorResponse

            // HTML
            this.AddMethodCallback(HTTPMethod.GET,
                                   "/RNs/{RoamingNetwork}",
                                   HTTPContentType.HTML_UTF8,
                                   HTTPDelegate: Request => {

                                       var RoamingNetworkId = Request.ParsedURIParameters[0];

                                       return new HTTPResponseBuilder(Request) {
                                           HTTPStatusCode  = HTTPStatusCode.BadGateway,
                                           ContentType     = HTTPContentType.HTML_UTF8,
                                           Content         = ("/RNs/" + RoamingNetworkId + " is a HTTP/SOAP/XML endpoint!").ToUTF8Bytes(),
                                           Connection      = "close"
                                       };

                                   });

            // Text
            this.AddMethodCallback(HTTPMethod.GET,
                                   "/RNs/{RoamingNetwork}",
                                   HTTPContentType.TEXT_UTF8,
                                   HTTPDelegate: Request => {

                                       var RoamingNetworkId = Request.ParsedURIParameters[0];

                                       return new HTTPResponseBuilder(Request) {
                                           HTTPStatusCode  = HTTPStatusCode.BadGateway,
                                           ContentType     = HTTPContentType.HTML_UTF8,
                                           Content         = ("/RNs/" + RoamingNetworkId + " is a HTTP/SOAP/XML endpoint!").ToUTF8Bytes(),
                                           Connection      = "close"
                                       };

                                   });

            #endregion

            #endregion

        }

        #endregion

    }

}
