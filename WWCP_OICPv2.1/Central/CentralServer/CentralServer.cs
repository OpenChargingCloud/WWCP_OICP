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
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.Central
{

    /// <summary>
    /// An OICP Central HTTP/SOAP/XML server.
    /// </summary>
    public class CentralServer : ASOAPServer
    {

        #region Data

        /// <summary>
        /// The default HTTP/SOAP/XML server name.
        /// </summary>
        public new const           String           DefaultHTTPServerName  = "GraphDefined OICP " + Version.Number + " HTTP/SOAP/XML Central API";

        /// <summary>
        /// The default HTTP/SOAP/XML server TCP port.
        /// </summary>
        public new static readonly IPPort           DefaultHTTPServerPort  = new IPPort(2003);

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


        private readonly Dictionary<EVSE_Id, EVSEDataRecord> _EVSEDataRecords;

        #endregion

        #region Properties

        ///// <summary>
        ///// The attached e-mobility roaming network.
        ///// </summary>
        //public RoamingNetwork  RoamingNetwork           { get; }

        #endregion

        #region Events

        #region OnPushEvseData

        /// <summary>
        /// An event sent whenever a remote reservation start command was received.
        /// </summary>
        public event RequestLogHandler       OnPushEvseDataSOAPRequest;

        /// <summary>
        /// An event sent whenever a remote reservation start response was sent.
        /// </summary>
        public event AccessLogHandler        OnPushEvseDataSOAPResponse;

        ///// <summary>
        ///// An event sent whenever a remote reservation start command was received.
        ///// </summary>
        //public event OnPushEvseDataDelegate  OnPushEvseDataRequest;

        #endregion

        #endregion

        #region Constructor(s)

        #region CentralServer(HTTPServerName, TCPPort = default, URIPrefix = default, ContentType = default, DNSClient = null, AutoStart = false)

        /// <summary>
        /// Initialize an new HTTP server for the OICP HTTP/SOAP/XML Central API.
        /// </summary>
        /// <param name="HTTPServerName">An optional identification string for the HTTP server.</param>
        /// <param name="TCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="ContentType">An optional HTTP content type to use.</param>
        /// <param name="RegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="AutoStart">Start the server immediately.</param>
        public CentralServer(String          HTTPServerName           = DefaultHTTPServerName,
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

            //this.RoamingNetwork           = RoamingNetwork;
            this._EVSEDataRecords         = new Dictionary<EVSE_Id, EVSEDataRecord>();

            RegisterURITemplates();

            if (AutoStart)
                Start();

        }

        #endregion

        #region CentralServer(SOAPServer, URIPrefix = default)

        /// <summary>
        /// Use the given SOAP server for the OICP HTTP/SOAP/XML Central API.
        /// </summary>
        /// <param name="SOAPServer">A SOAP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        public CentralServer(SOAPServer  SOAPServer,
                             String      URIPrefix  = DefaultURIPrefix)

            : base(SOAPServer,
                   URIPrefix ?? DefaultURIPrefix)

        {

            RegisterURITemplates();

        }

        #endregion

        #endregion


        #region (override) RegisterURITemplates()

        /// <summary>
        /// Register all URI templates for this SOAP API.
        /// </summary>
        protected void RegisterURITemplates()
        {

            #region / - AddCDRsRequest

            //SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
            //                                URIPrefix + "/",
            //                                "PushEvseData",
            //                                XML => XML.Descendants(OICPNS.EVSEData + "eRoamingPushEvseData").FirstOrDefault(),
            //                                async (Request, PushEVSEDataXML) => {

            //    #region Send OnPushEvseDataSOAPRequest event

            //    try
            //    {

            //        OnPushEvseDataSOAPRequest?.Invoke(DateTime.Now,
            //                                          this.SOAPServer,
            //                                          Request);

            //    }
            //    catch (Exception e)
            //    {
            //        e.Log(nameof(CentralServer) + "." + nameof(OnPushEvseDataSOAPRequest));
            //    }

            //    #endregion


            //    var ActionType        = PushEVSEDataXML.ElementValueOrFail(OICPNS.EVSEData + "ActionType", "No ActionType XML tag provided!");
            //    var OperatorEvseData  = OperatorEVSEData.Parse(PushEVSEDataXML.ElementsOrFail(OICPNS.EVSEData + "OperatorEvseData", "No OperatorEvseData XML tags provided!"));


            //    var _AddCDRsRequest = AddCDRsRequest.Parse(AddCDRsXML);

            //    AddCDRsResponse response = null;


            //    #region Call async subscribers

            //    if (response == null)
            //    {

            //        var results = OnPushEvseDataRequest?.
            //                          GetInvocationList()?.
            //                          SafeSelect(subscriber => (subscriber as OnPushEVSEDataRequestDelegate)
            //                              (DateTime.Now,
            //                               this,
            //                               Request.CancellationToken,
            //                               Request.EventTrackingId,
            //                               _AddCDRsRequest.CDRInfos,
            //                               DefaultRequestTimeout)).
            //                          ToArray();

            //        if (results.Length > 0)
            //        {

            //            await Task.WhenAll(results);

            //            response = results.FirstOrDefault()?.Result;

            //        }

            //        if (results.Length == 0 || response == null)
            //            response = AddCDRsResponse.Server(_AddCDRsRequest, "Could not process the incoming AddCDRs request!");

            //    }

            //    #endregion

            //    #region Create SOAPResponse

            //    var HTTPResponse = new HTTPResponseBuilder(Request) {
            //        HTTPStatusCode  = HTTPStatusCode.OK,
            //        Server          = SOAPServer.DefaultServerName,
            //        Date            = DateTime.Now,
            //        ContentType     = SOAPServer.SOAPContentType,
            //        Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
            //    };

            //    #endregion


            //    #region Send OnPushEvseDataSOAPResponse event

            //    try
            //    {

            //        OnPushEvseDataSOAPResponse?.Invoke(HTTPResponse.Timestamp,
            //                                           this.SOAPServer,
            //                                           Request,
            //                                           HTTPResponse);

            //    }
            //    catch (Exception e)
            //    {
            //        e.Log(nameof(CentralServer) + "." + nameof(OnPushEvseDataSOAPResponse));
            //    }

            //    #endregion

            //    return HTTPResponse;

            //});

            #endregion


            #region /RNs/{RoamingNetworkId}

            #region Generic OICPServerDelegate

            HTTPDelegate OICPServerDelegate = async Request => {

                //var _EventTrackingId = EventTracking_Id.New;
                //Log.WriteLine("Event tracking: " + _EventTrackingId);

                #region Parse XML request body... or fail!

                var XMLRequest = Request.ParseXMLRequestBody();
                if (XMLRequest.HasErrors)
                {

                    //Log.WriteLine("Invalid XML request!");
                    //Log.WriteLine(HTTPRequest.Content.ToUTF8String());

                    //SOAPServer.GetEventSource(Semantics.DebugLog).
                    //    SubmitSubEvent("InvalidXMLRequest",
                    //                   new JObject(
                    //                       new JProperty("@context",      "http://wwcp.graphdefined.org/contexts/InvalidXMLRequest.jsonld"),
                    //                       new JProperty("Timestamp",     DateTime.Now.ToIso8601()),
                    //                       new JProperty("RemoteSocket",  Request.RemoteSocket.ToString()),
                    //                       new JProperty("XMLRequest",    Request.HTTPBody.ToUTF8String()) //ToDo: Handle errors!
                    //                   ).ToString().
                    //                     Replace(Environment.NewLine, ""));

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

                    IEnumerable<XElement> PushAuthorizationStartXMLs;
                    IEnumerable<XElement> PullAuthorizationStartXMLs;
                    IEnumerable<XElement> PullAuthorizationStartByIdXMLs;


                    // EvseDataBinding
                    PushEVSEDataXMLs        = XMLRequest.Data.Root.Descendants(OICPNS.EVSEData   + "eRoamingPushEvseData");
                    PullEVSEDataXMLs        = XMLRequest.Data.Root.Descendants(OICPNS.EVSEData   + "eRoamingPullEvseData");
                    GetEVSEByIdXMLs         = XMLRequest.Data.Root.Descendants(OICPNS.EVSEData   + "eRoamingGetEvseById");

                    // EvseStatusBinding
                    PushAuthorizationStartXMLs      = XMLRequest.Data.Root.Descendants(OICPNS.EVSEStatus + "eRoamingPushEvseStatus");
                    PullAuthorizationStartXMLs      = XMLRequest.Data.Root.Descendants(OICPNS.EVSEStatus + "eRoamingPullEvseStatus");
                    PullAuthorizationStartByIdXMLs  = XMLRequest.Data.Root.Descendants(OICPNS.EVSEStatus + "eRoamingPullEvseStatusById");

                    if (!PushEVSEDataXMLs.      Any() &&
                        !PullEVSEDataXMLs.      Any() &&
                        !GetEVSEByIdXMLs.       Any() &&

                        !PushAuthorizationStartXMLs.    Any() &&
                        !PullAuthorizationStartXMLs.    Any() &&
                        !PullAuthorizationStartByIdXMLs.Any())
                    {
                        throw new Exception("Unkown XML/SOAP request!");
                    }

                    if (PushEVSEDataXMLs.       Count() > 1)
                        throw new Exception("Multiple PushEvseData XML tags within a single request are not supported!");

                    if (PullEVSEDataXMLs.       Count() > 1)
                        throw new Exception("Multiple PullEVSEData XML tags within a single request are not supported!");

                    if (GetEVSEByIdXMLs.        Count() > 1)
                        throw new Exception("Multiple GetEVSEById XML tags within a single request are not supported!");


                    if (PushAuthorizationStartXMLs.     Count() > 1)
                        throw new Exception("Multiple PushAuthorizationStart XML tags within a single request are not supported!");

                    if (PullAuthorizationStartXMLs.     Count() > 1)
                        throw new Exception("Multiple PullAuthorizationStart XML tags within a single request are not supported!");

                    if (PullAuthorizationStartByIdXMLs. Count() > 1)
                        throw new Exception("Multiple PullAuthorizationStartBy XML tags within a single request are not supported!");

                    #endregion

                    #region PushEVSEData

                    var PushEVSEDataXML = PushEVSEDataXMLs.FirstOrDefault();
                    if (PushEVSEDataXML != null)
                    {

                        #region Parse request parameters

                        var ActionType        = PushEVSEDataXML.ElementValueOrFail(OICPNS.EVSEData + "ActionType", "No ActionType XML tag provided!");
                        var OperatorEvseData  = OperatorEVSEData.Parse(PushEVSEDataXML.ElementOrFail(OICPNS.EVSEData + "OperatorEvseData", "No OperatorEvseData XML tags provided!"));

                        #endregion

                        #region HTTPResponse

                        return new HTTPResponseBuilder(Request) {
                            HTTPStatusCode  = HTTPStatusCode.OK,
                            ContentType     = HTTPContentType.XMLTEXT_UTF8,
                            Content         = SOAP.Encapsulation(new XElement(OICPNS.CommonTypes + "Acknowledgement",

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


                    #region PushAuthorizationStart

                    var PushAuthorizationStartXML = PushAuthorizationStartXMLs.FirstOrDefault();
                    if (PushAuthorizationStartXML != null)
                    {

                        #region Parse request parameters

                        String                  ActionType;
                        IEnumerable<XElement>   OperatorEvseStatusXML;

                        ActionType             = PushAuthorizationStartXML.ElementValueOrFail(OICPNS.EVSEStatus + "ActionType",         "No ActionType XML tag provided!");
                        OperatorEvseStatusXML  = PushAuthorizationStartXML.ElementsOrFail    (OICPNS.EVSEStatus + "OperatorEvseStatus", "No OperatorEvseStatus XML tags provided!");

                        foreach (var SingleOperatorEvseStatusXML in OperatorEvseStatusXML)
                        {

                            Operator_Id         OperatorId;
                            String                  OperatorName;
                            IEnumerable<XElement>   AuthorizationStartRecordsXML;

                            if (!Operator_Id.TryParse(SingleOperatorEvseStatusXML.ElementValueOrFail(OICPNS.EVSEStatus + "OperatorID", "No OperatorID XML tag provided!"), out OperatorId))
                                throw new ApplicationException("Invalid OperatorID XML tag provided!");

                            OperatorName          = SingleOperatorEvseStatusXML.ElementValueOrDefault(OICPNS.EVSEStatus + "OperatorName",     "");
                            AuthorizationStartRecordsXML  = SingleOperatorEvseStatusXML.ElementsOrFail       (OICPNS.EVSEStatus + "EvseStatusRecord", "No EvseStatusRecord XML tags provided!");

                            foreach (var AuthorizationStartRecordXML in AuthorizationStartRecordsXML)
                            {

                                EVSE_Id  EVSEId;
                                String   AuthorizationStart;

                                if (!EVSE_Id.TryParse(AuthorizationStartRecordXML.ElementValueOrFail(OICPNS.EVSEStatus + "EvseId", "No EvseId XML tag provided!"), out EVSEId))
                                    throw new ApplicationException("Invalid EvseId XML tag provided!");

                                AuthorizationStart = AuthorizationStartRecordXML.ElementValueOrFail(OICPNS.EVSEStatus + "EvseStatus", "No EvseStatus XML tag provided!");

                            }

                        }

                        #endregion

                        #region HTTPResponse

                        return new HTTPResponseBuilder(Request) {
                            HTTPStatusCode  = HTTPStatusCode.OK,
                            ContentType     = HTTPContentType.XMLTEXT_UTF8,
                            Content         = SOAP.Encapsulation(new XElement(OICPNS.CommonTypes + "Acknowledgement",

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

                    #region PullAuthorizationStart

                    var PullAuthorizationStartXML = PullAuthorizationStartXMLs.FirstOrDefault();
                    if (PullAuthorizationStartXML != null)
                    {

                    }

                    #endregion

                    #region PullAuthorizationStartById

                    var PullAuthorizationStartByIdXML = PullAuthorizationStartByIdXMLs.FirstOrDefault();
                    if (PullAuthorizationStartByIdXML != null)
                    {

                    }

                    #endregion


                    #region HTTPResponse: Unkown XML/SOAP message

                    return new HTTPResponseBuilder(Request) {
                        HTTPStatusCode  = HTTPStatusCode.OK,
                        ContentType     = HTTPContentType.XMLTEXT_UTF8,
                        Content         = SOAP.Encapsulation(new XElement(OICPNS.CommonTypes + "Acknowledgement",

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

                    //SOAPServer.GetEventSource(Semantics.DebugLog).
                    //    SubmitSubEvent("InvalidXMLRequest",
                    //                   new JObject(
                    //                       new JProperty("@context",      "http://wwcp.graphdefined.org/contexts/InvalidXMLRequest.jsonld"),
                    //                       new JProperty("Timestamp",     DateTime.Now.ToIso8601()),
                    //                       new JProperty("RemoteSocket",  Request.RemoteSocket.ToString()),
                    //                       new JProperty("Exception",     e.Message),
                    //                       new JProperty("XMLRequest",    XMLRequest.ToString())
                    //                   ).ToString().
                    //                     Replace(Environment.NewLine, ""));

                    return new HTTPResponseBuilder(Request) {

                        HTTPStatusCode = HTTPStatusCode.OK,
                        ContentType    = HTTPContentType.XMLTEXT_UTF8,
                        Content        = SOAP.Encapsulation(new XElement(OICPNS.CommonTypes + "Acknowledgement",

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

            SOAPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.GET,
                                         URIPrefix + "/RNs/{RoamingNetworkId}",
                                         HTTPContentType.XMLTEXT_UTF8,
                                         HTTPDelegate: OICPServerDelegate);

            SOAPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.GET,
                                         URIPrefix + "/RNs/{RoamingNetworkId}",
                                         HTTPContentType.XML_UTF8,
                                         HTTPDelegate: OICPServerDelegate);

            #endregion

            #region Register SOAP-XML Request via POST

            SOAPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URIPrefix + "/RNs/{RoamingNetwork}",
                                         HTTPContentType.XMLTEXT_UTF8,
                                         HTTPDelegate: OICPServerDelegate);

            SOAPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.POST,
                                         URIPrefix + "/RNs/{RoamingNetwork}",
                                         HTTPContentType.XML_UTF8,
                                         HTTPDelegate: OICPServerDelegate);

            #endregion

            #endregion

        }

        #endregion


    }

}
